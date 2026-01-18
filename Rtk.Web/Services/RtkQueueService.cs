using System.Threading.Channels;
using Rtk.Core; // 引用核心库

namespace Rtk.Web.Services; // 命名空间通常是 项目名.文件夹名

// 定义任务数据包 (DTO)
// 包含：任务ID，配置文件路径，观测文件路径，导航文件路径，结果输出路径
public record RtkTask(string TaskId, string ConfPath, string ObsPath, string NavPath, string ResultPath, string? Sp3Path = null, string? ClkPath = null);
public record RtkTaskPPP(
    string TaskId,
    string ConfPath,
    string ObsPath,
    string NavPath,
    string ResultPath,
    string? Sp3Path = null,
    string? ClkPath = null
);

public class RtkQueueService : BackgroundService
{
    // 创建一个无限容量的通道
    private readonly Channel<RtkTask> _channel = Channel.CreateUnbounded<RtkTask>();
    private readonly ILogger<RtkQueueService> _logger;
    private readonly IServiceProvider _serviceProvider; 

    public RtkQueueService(ILogger<RtkQueueService> logger, IServiceProvider sp)
    {
        _logger = logger;
        _serviceProvider = sp;
    }

    // 入队方法 (Controller 调用它)
    public async ValueTask EnqueueAsync(RtkTask task)
    {
        await _channel.Writer.WriteAsync(task);
        _logger.LogInformation($"[队列] 任务 {task.TaskId} 已加入等待队列");
    }

    // 后台执行逻辑 (系统自动运行)
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // 只要队列里有东西，就取出来处理
        await foreach (var task in _channel.Reader.ReadAllAsync(stoppingToken))
        {
            try
            {
                _logger.LogInformation($"[处理中] 开始计算任务: {task.TaskId}");
                
                // 创建作用域 (Scope) 来获取 RtkEngine
                // 因为 RtkEngine 可能是 Scoped 或 Singleton，养成建 Scope 的习惯更好
                using var scope = _serviceProvider.CreateScope();
                var engine = scope.ServiceProvider.GetRequiredService<RtkEngine>();
                
                // === 这里是真正调用 RTKLib 的地方 ===
                // 调用我们在 Core 里写好的 RunRnx2RtkpAsync
                string logOutput = await engine.RunRnx2RtkpAsync(
                    task.ConfPath, 
                    task.ObsPath, 
                    task.NavPath, 
                    task.ResultPath,
                    task.Sp3Path,
                    task.ClkPath
                );

                // 你可以将 logOutput 保存到文件或者数据库，方便以后查错
                _logger.LogInformation($"[完成] 任务 {task.TaskId} 计算结束！");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[失败] 任务 {task.TaskId} 发生异常");
            }
        }
    }
}