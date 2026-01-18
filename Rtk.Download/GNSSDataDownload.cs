namespace Rtk.Download;
public class GNSSDataDownload
{
    public static async Task DownloadERPFile(string remotePath, string localPath)
    {
        var ftpserver = "ftp://igs.gnsswhu.cn";
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
    public static async Task DownloadBrdcFile(int doy, int year, string localPath)
    {
        var ftpserver = "ftp://igs.gnsswhu.cn";
        var remotePath = $"/pub/gps/data/daily/{year}/brdc/BRDC00IGS_R_{year}{doy:000}0000_01D_MN.rnx.gz";
        FtpDownloader ftp = new FtpDownloader(ftpserver);
        try
        {
            await ftp.Download(remotePath, localPath);
        }
        catch (Exception ex)
        {
            throw new Exception($"下载文件失败：{remotePath}，错误信息：{ex.Message}");
        }
    }
}