// See https://aka.ms/new-console-template for more information
using FluentFTP;
using Rtk.Download;


var gnssDownloader = new GNSSDataDownload();
await gnssDownloader.DownloadIonoFile(1,365, 2022, $"./iono/2022", true);

// await gnssDownloader.DownloadObsFile(1, 10, 2024, "ABMF", "./obs", true);
// foreach (var file in Directory.GetFiles("./iono"))
// {
//     Console.WriteLine(file);
// }

// for (int i = 1; i < 365; i++)
// {
//     if(!File.Exists($"C:\\Users\\zx\\Desktop\\毕业论文\\iono\\gim\\COD0OPSFIN_2025{i:000}0000_01D_01H_GIM.INX"))
//     {
//         System.Console.WriteLine($"Missing file: ./iono/COD0OPSFIN_2025{i:000}0000_01D_01H_GIM.INX");
//     }
// }