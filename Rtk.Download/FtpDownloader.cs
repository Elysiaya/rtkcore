using System;
using FluentFTP;
using FluentFTP.Exceptions;
namespace Rtk.Download;

public class FtpDownloader:IDisposable
{
    private readonly AsyncFtpClient ftpClient;
    public FtpDownloader(string ftpServer, string ftpUser = "anonymous", string ftpPwd = "user@example.com")
    {
        ftpClient = new AsyncFtpClient(ftpServer, ftpUser, ftpPwd);
        ftpClient.Config.ConnectTimeout = 30000;
        ftpClient.Config.ReadTimeout = 30000;
        ftpClient.Config.SelfConnectMode = FtpSelfConnectMode.Always;
        ftpClient.Config.EncryptionMode = FtpEncryptionMode.None;
    }
    // <summary>
    /// 异步下载：输入FTP完整路径+本地保存路径，直接下载（默认匿名登录）
    /// </summary>
    /// <param name="remotePath">FTP完整路径，比如：/pub/products/2024/124/igs2024124.sp3.Z</param>
    /// <param name="localSavePath">本地保存路径，比如：D:/rtk-data/igs2024124.sp3.Z</param>
    /// <param name="ftpUser">FTP用户名，匿名登录留空或传"anonymous"</param>
    /// <param name="ftpPwd">FTP密码，匿名登录留空或传任意邮箱格式</param>
    public async Task Download(string remotePath, string localSavePath)
    {
        int maxRetries = 3;
        int retryDelay = 3000; // 3秒

        // if (!await ftpClient.FileExists(remotePath.TrimStart('/')))
        // {
        //     throw new FileNotFoundException($"远程文件不存在：{remotePath}");
        // }

        for (int attempt = 1; attempt <= maxRetries; attempt++)
        {
            try
            {
                await ftpClient.DownloadFile(localSavePath, remotePath.TrimStart('/'));
                Console.WriteLine($"已下载：{remotePath} 到 {localSavePath}");
                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"尝试 {attempt}/{maxRetries} 下载失败: {remotePath}。错误: {ex.Message}");
                if (attempt == maxRetries)
                {
                    // 这是最后一次尝试，仍然失败，所以向上抛出异常
                    throw; 
                }
                await Task.Delay(retryDelay);
            }
        }
    }
    // 实现 IDisposable
    public void Dispose()
    {
        ftpClient.Dispose();
        GC.SuppressFinalize(this);
    }
    public async Task<List<string>> ListFiles(string remoteDirectory)
    {
        await ftpClient.Connect();
        var items = await ftpClient.GetListing(remoteDirectory);
        List<string> fileNames = items.Select(item => item.Name).ToList();
        return fileNames;
    }
}

