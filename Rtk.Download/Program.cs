// See https://aka.ms/new-console-template for more information
using FluentFTP;
using Rtk.Download;

var ftpserver = "ftp://igs.gnsswhu.cn";
FtpDownloader ftp = new FtpDownloader(ftpserver);
await ftp.Download("/pub/gnss/products/mgex/2400/WUM0MGXFIN_20260040000_01D_01D_ERP.ERP.gz", "./WUM0MGXFIN_20260040000_01D_01D_ERP.ERP.gz");
