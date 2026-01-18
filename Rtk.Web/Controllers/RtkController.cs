using Microsoft.AspNetCore.Mvc;
using Rtk.Web.Models;
using Rtk.Web.Services; // 引用你的队列服务

namespace Rtk.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RtkController : ControllerBase
{
    private readonly ILogger<RtkController> _logger;
    private readonly RtkQueueService _queueService;
    private readonly IWebHostEnvironment _env;

    // 注入：日志、队列服务、环境路径
    public RtkController(
        ILogger<RtkController> logger, 
        RtkQueueService queueService,
        IWebHostEnvironment env)
    {
        _logger = logger;
        _queueService = queueService;
        _env = env;
    }

    [HttpPost("submit")]
    public async Task<IActionResult> SubmitTask([FromForm] SubmitTaskRequest request)
    {
        // 1. 生成唯一的任务 ID (UUID)
        var taskId = Guid.NewGuid().ToString("N");
        
        // 2. 准备该任务的专属文件夹 (比如 /app/data/tasks/xxxx-xxxx...)
        // 在 Linux 上 ContentRootPath 可能是 /var/www/rtkapp
        var taskFolder = Path.Combine(_env.ContentRootPath, "Data", "Tasks", taskId);
        Directory.CreateDirectory(taskFolder);

        _logger.LogInformation($"接收到新任务: {taskId}，正在保存文件...");

        // 3. 保存上传的文件
        var obsPath = Path.Combine(taskFolder, request.ObsFile.FileName);
        var navPath = Path.Combine(taskFolder, request.NavFile.FileName);
        
        using (var stream = new FileStream(obsPath, FileMode.Create))
            await request.ObsFile.CopyToAsync(stream);
            
        using (var stream = new FileStream(navPath, FileMode.Create))
            await request.NavFile.CopyToAsync(stream);

        // 处理配置文件：如果用户没传，就用默认的
        string confPath;
        if (request.ConfFile != null)
        {
            confPath = Path.Combine(taskFolder, "rtk.conf");
            using (var stream = new FileStream(confPath, FileMode.Create))
                await request.ConfFile.CopyToAsync(stream);
        }
        else
        {
            // TODO: 这里可以调用你 Rtk.Core 里的生成器生成默认配置
            confPath = Path.Combine(_env.ContentRootPath, "Data", "Defaults", "default.conf");
        }

        // 4. 打包任务对象
        // 注意：这里构造的是我们在 QueueService 里定义的记录类型
        var taskPayload = new RtkTask(taskId, confPath, obsPath, navPath, Path.Combine(taskFolder, "result.pos"));

        // 5. 【关键】入队！
        // 这步非常快，完全不涉及计算，瞬间返回
        await _queueService.EnqueueAsync(taskPayload);

        // 6. 返回给前端
        return Ok(new 
        { 
            TaskId = taskId, 
            Status = "Queued",
            Message = "任务已接收，正在排队处理中",
            QueryUrl = $"/api/rtk/status/{taskId}" // 告诉前端去哪里查进度
        });
    }

    // 查询状态接口（简单实现）
    [HttpGet("status/{taskId}")]
    public IActionResult GetStatus(string taskId)
    {
        var taskFolder = Path.Combine(_env.ContentRootPath, "Data", "Tasks", taskId);
        var resultPath = Path.Combine(taskFolder, "result.pos");

        if (System.IO.File.Exists(resultPath))
        {
            return Ok(new { Status = "Completed", ResultUrl = $"/api/rtk/download/{taskId}" });
        }
        
        // 这里的判断其实很不严谨，严谨的做法是用 SQLite 查状态
        // 但对于 MVP 来说，文件不存在 = 还在算（或失败了）
        return Ok(new { Status = "Processing" });
    }

    // 下载接口
    [HttpGet("download/{taskId}")]
    public IActionResult DownloadResult(string taskId)
    {
        var taskFolder = Path.Combine(_env.ContentRootPath, "Data", "Tasks", taskId);
        var resultPath = Path.Combine(taskFolder, "result.pos");

        if (!System.IO.File.Exists(resultPath))
            return NotFound("结果文件尚未生成或计算失败");

        var bytes = System.IO.File.ReadAllBytes(resultPath);
        return File(bytes, "text/plain", $"{taskId}_result.pos");
    }
}