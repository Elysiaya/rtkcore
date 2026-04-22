namespace Rtk.Web.Models;

public class SubmitTaskRequest
{
    // [FromForm] 表示这些数据来自 multipart/form-data
    public required IFormFile ObsFile { get; set; } // 观测文件
    public required IFormFile NavFile { get; set; } // 导航文件
    
    // 选项 A：用户直接上传配置 .conf 文件
    public IFormFile? ConfFile { get; set; }

    // 选项 B：用户传参数，服务器生成配置（比如 0=Static, 1=Kinematic）
    // public int Mode { get; set; } 
}