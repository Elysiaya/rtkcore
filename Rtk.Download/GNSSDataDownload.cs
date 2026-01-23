using FluentFTP;

namespace Rtk.Download;

public class GNSSDataDownload
{
    static readonly string ftpserver = "ftp://igs.gnsswhu.cn";
    public GNSSDataDownload()
    {
    }
    public static async Task DownloadERPFile(string remotePath, string localPath)
    {
        FtpDownloader ftp = new FtpDownloader(ftpserver);
        await ftp.Download(remotePath, localPath);
    }
    public static async Task DownloadSP3File(string remotePath, string localPath)
    {
        var ftpserver = "ftp://igs.gnsswhu.cn";
        FtpDownloader ftp = new FtpDownloader(ftpserver);
        await ftp.Download(remotePath, localPath);
    }
    public static async Task DownloadClockFile(string remotePath, string localPath)
    {
        var ftpserver = "ftp://igs.gnsswhu.cn";
        FtpDownloader ftp = new FtpDownloader(ftpserver);
        await ftp.Download(remotePath, localPath);
    }
    public static async Task DownloadBiasFile(string remotePath, string localPath)
    {
        var ftpserver = "ftp://igs.gnsswhu.cn";
        FtpDownloader ftp = new FtpDownloader(ftpserver);
        await ftp.Download(remotePath, localPath);
    }
    public static async Task DownloadAntennaFile(string remotePath, string localPath)
    {
        var ftpserver = "ftp://igs.gnsswhu.cn";
        FtpDownloader ftp = new FtpDownloader(ftpserver);
        await ftp.Download(remotePath, localPath);
    }
    public async Task DownloadBrdcFile(int startdoy, int enddoy, int year, string localPathfolder)
    {
        var remotePath = "/pub/gps/data/daily/{year}/brdc/BRDC00IGS_R_{year}{doy}0000_01D_MN.rnx.gz";
        await DownloadFiles(remotePath, startdoy, enddoy, year, localPathfolder);
    }

    public async Task DownloadIonoFile(int startdoy, int enddoy, int year, string localPathfolder)
    {
        var remotePath = "/pub/gps/products/ionex/{year}/{doy}/COD0OPSFIN_{year}{doy}0000_01D_01H_GIM.INX.gz";
        await DownloadFiles(remotePath, startdoy, enddoy, year, localPathfolder);
    }

    public async Task DownloadFiles(
    string remotePathTemplate,
    int startdoy,
    int enddoy,
    int year,
    string localPathFolder)
    {
        if (!Directory.Exists(localPathFolder))
        {
            Directory.CreateDirectory(localPathFolder);
        }

        using var ftp = new FtpDownloader(ftpserver);

        for (int doy = startdoy; doy <= enddoy; doy++)
        {

            string doyStr = doy.ToString("000");
            string yearStr = year.ToString();
            var remotePath = remotePathTemplate
                .Replace("{year}", yearStr)
                .Replace("{doy}", doyStr);

            var fileName = Path.GetFileName(remotePath);
            var localPath = Path.Combine(localPathFolder, fileName);

            try
            {
                await ftp.Download(remotePath, localPath);
                Console.WriteLine($"✅ 已下载: {fileName}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ 下载失败: {remotePath}\n错误: {ex.Message}");
                // 可选：记录日志或重试机制
            }
        }
    }

}
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