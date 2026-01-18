using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Rtk.Core;

public class RtkEngine
{
    // 配置好 RTKLib 可执行文件的存放路径
    // Windows 放项目根目录的 bin/win-x64 下
    // Linux 放项目根目录的 bin/linux-x64 下
    private readonly string _basePath;

    public RtkEngine(string basePath)
    {
        _basePath = basePath;
    }

    public async Task<string> RunRnx2RtkpAsync(string confPath, string obsPath, string navPath, string outPath,string? sp3Path = null, string? clkPath = null)
    {
        // 1. 自动判断操作系统，选择对应的“打手”
        string exeName;
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            exeName = "rnx2rtkp.exe";
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            exeName = "rnx2rtkp"; // Linux下通常没有后缀
        }
        else
        {
            throw new PlatformNotSupportedException("暂不支持 MacOS");
        }

        var exePath = Path.Combine(_basePath, "bin", exeName);
        if (!File.Exists(exePath))
        {
            throw new FileNotFoundException($"找不到 RTKLib 可执行文件: {exePath}");
        }
        
        // 2. 构造参数 (这里只是简单示例，后面要根据 Conf 生成器来做)
        var args = $"-k \"{confPath}\" -o \"{outPath}\" \"{obsPath}\" \"{navPath}\"";
        if (!string.IsNullOrEmpty(sp3Path))
        {
            args += $" \"{sp3Path}\"";
        }

        if (!string.IsNullOrEmpty(clkPath))
        {
            args += $" \"{clkPath}\"";
        }

        // 3. 启动进程
        var psi = new ProcessStartInfo
        {
            FileName = exePath,
            Arguments = args,
            RedirectStandardOutput = true, // 捕获标准输出
            RedirectStandardError = true,  // 捕获错误输出
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var process = new Process { StartInfo = psi };

        var outputBuilder = new System.Text.StringBuilder();
        var errorBuilder = new System.Text.StringBuilder();

        process.OutputDataReceived += (sender, e) =>
    {
        if (!string.IsNullOrEmpty(e.Data))
        {
            // A. 实时打印到控制台，让你看到进度条在动！
            Console.WriteLine($"[RTK] {e.Data}");
            
            // B. 收集起来最后返回
            outputBuilder.AppendLine(e.Data);
        }
    };

    process.ErrorDataReceived += (sender, e) =>
    {
        if (!string.IsNullOrEmpty(e.Data))
        {
            Console.WriteLine($"[ERR] {e.Data}");
            errorBuilder.AppendLine(e.Data);
        }
    };
        
        process.Start();

    process.BeginOutputReadLine();
    process.BeginErrorReadLine();

    // 异步等待结束，不会卡死主线程
    await process.WaitForExitAsync();

        if (process.ExitCode != 0)
        {
            throw new Exception($"解算失败 (Code {process.ExitCode}): {errorBuilder}");
        }

        return outputBuilder.ToString(); // 或者返回 output.pos 的文件路径
    }
}
