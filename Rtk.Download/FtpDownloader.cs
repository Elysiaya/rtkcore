using System;
using FluentFTP;
namespace Rtk.Download;

public class FtpDownloader
{
    string ftpServer;
    public FtpDownloader(string ftpServer)
    {
        this.ftpServer = ftpServer;
    }
    // <summary>
    /// 异步下载：输入FTP完整路径+本地保存路径，直接下载（默认匿名登录）
    /// </summary>
    /// <param name="remotePath">FTP完整路径，比如：/pub/products/2024/124/igs2024124.sp3.Z</param>
    /// <param name="localSavePath">本地保存路径，比如：D:/rtk-data/igs2024124.sp3.Z</param>
    /// <param name="ftpUser">FTP用户名，匿名登录留空或传"anonymous"</param>
    /// <param name="ftpPwd">FTP密码，匿名登录留空或传任意邮箱格式</param>
    public async Task Download(string remotePath, string localSavePath, string ftpUser = "anonymous", string ftpPwd = "user@example.com")
    {
        // 自动处理FTP连接+下载+释放资源
        using var ftpClient = new AsyncFtpClient(ftpServer, ftpUser, ftpPwd);
        
        await ftpClient.Connect();
        if (!await ftpClient.FileExists(remotePath.TrimStart('/')))
        {
            throw new Exception($"远程文件不存在：{remotePath}");
        }
        // 下载文件（自动覆盖已存在的本地文件）
        await ftpClient.DownloadFile(localSavePath, remotePath.TrimStart('/'));
        await ftpClient.Disconnect();
    }
}

//解压.gz文件
public class Decompress
{
    public static void DecompressGzip(string gzipFilePath, string outputFilePath)
    {
        using FileStream originalFileStream = new FileStream(gzipFilePath, FileMode.Open, FileAccess.Read);
        using FileStream decompressedFileStream = new FileStream(outputFilePath, FileMode.Create, FileAccess.Write);
        using System.IO.Compression.GZipStream decompressionStream = new System.IO.Compression.GZipStream(originalFileStream, System.IO.Compression.CompressionMode.Decompress);
        decompressionStream.CopyTo(decompressedFileStream);
    }
}
