using System.Text.Json;
using FluentFTP;

namespace Rtk.Download;

public class GNSSDataDownload
{
    // static readonly string ftpserver = "ftp://igs.gnsswhu.cn";
    static readonly string ftpserver = "ftp://gdc.cddis.eosdis.nasa.gov";
    static  readonly string ftpuser = "anonymous";
    static  readonly string ftppwd = "zerofreezing@outlook.com";
    
    public GNSSDataDownload()
    {
    }
    public async Task DownloadObsFile(int startdoy, int enddoy, int year, string stationName, string localPathfolder,bool decompressGzip=false)
    {
        if (stationName.Length != 4)
        {
            throw new ArgumentException("Station name must be 4 characters long.");
        }
        var remotePath = $"/pub/gps/data/daily/{year}/{{doy}}/{year.ToString().Substring(2)}d";

        var json = await File.ReadAllTextAsync("igs_stations.json");
        var stations = JsonSerializer.Deserialize<List<Dictionary<string, JsonElement>>>(json);
        List<string> siteNames = stations.Select(s => s["name"].GetString()).Where(name => !string.IsNullOrEmpty(name)).ToList();

        string? fullSiteName = siteNames
            .FirstOrDefault(s => s.StartsWith(stationName, StringComparison.OrdinalIgnoreCase));

        if (fullSiteName != null)
        {
            var filePattern = $"{fullSiteName.ToUpper()}_R_{year}{{doy}}0000_01D_30S_MO.crx.gz";
            remotePath += $"/{filePattern}";
            await DownloadFiles(remotePath, startdoy, enddoy, year, localPathfolder,decompressGzip);
        }
        else
        {
            throw new ArgumentException($"Station '{stationName}' not found.");
        }

    }
    public async Task DownloadERPFile(int startdoy, int enddoy, int year, string localPathfolder,bool decompressGzip=false)
    {
        var remotePath = "/pub/gps/products/{gpsweek}/WUM0MGXRAP_{year}{doy}0000_01D_01D_ERP.ERP.gz";
        await DownloadFiles(remotePath, startdoy, enddoy, year, localPathfolder,decompressGzip);
    }
    public async Task DownloadSP3File(int startdoy, int enddoy, int year, string localPathfolder,bool decompressGzip=false)
    {
        var remotePath = "/pub/gps/products/{gpsweek}/WUM0MGXRAP_{year}{doy}0000_01D_05M_ORB.SP3.gz";
        await DownloadFiles(remotePath, startdoy, enddoy, year, localPathfolder,decompressGzip);
    }
    public async Task DownloadClockFile(int startdoy, int enddoy, int year, string localPathfolder,bool decompressGzip=false)
    {
        var remotePath = "/pub/gps/products/{gpsweek}/WUM0MGXRAP_{year}{doy}0000_01D_30S_CLK.CLK.gz";
        await DownloadFiles(remotePath, startdoy, enddoy, year, localPathfolder,decompressGzip);
    }
    public async Task DownloadBiasFile(int startdoy, int enddoy, int year, string localPathfolder,bool decompressGzip=false)
    {
        var remotePath = "/pub/gps/products/{gpsweek}/WUM0MGXRAP_{year}{doy}0000_01D_01D_OSB.BIA.gz";
        await DownloadFiles(remotePath, startdoy, enddoy, year, localPathfolder,decompressGzip);
    }
    public async Task DownloadBrdcFile(int startdoy, int enddoy, int year, string localPathfolder,bool decompressGzip=false)
    {
        var remotePath = "/pub/gps/data/daily/{year}/brdc/BRDC00IGS_R_{year}{doy}0000_01D_MN.rnx.gz";
        await DownloadFiles(remotePath, startdoy, enddoy, year, localPathfolder,decompressGzip);
    }

    public async Task DownloadIonoFile(int startdoy, int enddoy, int year, string localPathfolder,bool decompressGzip=false)
    {
        var remotePath = "/pub/gps/products/ionex/{year}/{doy}/COD0OPSFIN_{year}{doy}0000_01D_01H_GIM.INX.gz";
        await DownloadFiles(remotePath, startdoy, enddoy, year, localPathfolder,decompressGzip);
    }

    public async Task DownloadFiles(string remotePathTemplate, int startdoy, int enddoy, int year, string localPathFolder,bool decompressGzip=false)
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

            if (remotePathTemplate.Contains("{gpsweek}"))
            {
                var gpsweek = GetGPSWeek(year, doy);
                remotePath = remotePath.Replace("{gpsweek}", gpsweek.ToString("0000"));
            }

            var fileName = Path.GetFileName(remotePath);
            var localPath = Path.Combine(localPathFolder, fileName);

            try
            {
                await ftp.Download(remotePath, localPath);
                Console.WriteLine($"✅ 已下载: {fileName}");
                if (decompressGzip && localPath.EndsWith(".gz"))
                {
                    var decompressedFilePath = Path.Combine(localPathFolder, Path.GetFileNameWithoutExtension(fileName));
                    Decompress.DecompressGzip(localPath, decompressedFilePath);
                    Console.WriteLine($"   已解压: {decompressedFilePath}");
                    if (File.Exists(decompressedFilePath) )
                    {
                        File.Delete(localPath); // 删除原始的.gz文件
                        Console.WriteLine($"   已删除压缩文件: {localPath}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ 下载失败: {remotePath}\n错误: {ex.Message}");
                // 可选：记录日志或重试机制
            }
        }
    }

    private int GetGPSWeek(int year, int doy)
    {
        DateTime startDate = new DateTime(1980, 1, 6);
        DateTime currentDate = new DateTime(year, 1, 1).AddDays(doy - 1);
        TimeSpan timeSpan = currentDate - startDate;
        return (int)(timeSpan.TotalDays / 7);
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
    public static void DecompressCrx(string crxFilePath, string outputDirectory)
    {
        
    }
}