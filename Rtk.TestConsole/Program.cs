using Rtk.Core;
// 1. 确定根路径
// 这里的 Environment.CurrentDirectory 通常是 .../bin/Debug/net8.0/
var currentDir = Environment.CurrentDirectory;
Console.WriteLine($"当前工作目录: {currentDir}");

// 2. 实例化引擎
// 假设我们在 RtkEngine 里的逻辑是去找 currentDir/win-x64/rnx2rtkp.exe
var engine = new RtkEngine(currentDir);

// 3. 准备文件路径 (建议使用绝对路径，避免出错)
var obsFile = Path.Combine(currentDir, "test_data", "NTUS00SGP_R_20241330000_01D_30S_MO.rnx");
var navFile = Path.Combine(currentDir, "test_data", "BRD400DLR_S_20241330000_01D_MN.rnx");
var confFile = Path.Combine(currentDir, "test_data", "SPP.conf");
var outFile = Path.Combine(currentDir, "test_data", "result.pos");

// 简单的文件存在性检查
if (!File.Exists(obsFile)) {
    Console.Error.WriteLine($"错误：找不到观测文件 {obsFile}");
    return;
}

if (!File.Exists(navFile)) {
    Console.Error.WriteLine($"错误：找不到导航文件 {navFile}");
    return;
}

if (!File.Exists(confFile)) {
    Console.Error.WriteLine($"错误：找不到配置文件 {confFile}");
    return;
}

Console.WriteLine("正在启动解算引擎...");
Console.WriteLine("--------------------------------------------------");

try
{
    // 4. 调用核心方法
    // 注意：RunRnx2RtkpAsync 方法需要你修改一下，支持传入 output 路径，
    // 或者让它返回 stdout 字符串
    string output = await engine.RunRnx2RtkpAsync(confFile, obsFile, navFile, outFile);

    Console.WriteLine("--------------------------------------------------");
    Console.WriteLine("解算完成！RTKLib 输出如下：");
    Console.WriteLine(output);

    if (File.Exists(outFile))
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"\n[成功] 结果文件已生成: {outFile}");
        Console.WriteLine($"文件大小: {new FileInfo(outFile).Length} bytes");
    }
    else
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("\n[失败] 进程未报错，但结果文件未生成。");
    }
}
catch (Exception ex)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine($"\n[严重错误] 引擎崩溃: {ex.Message}");
}
finally
{
    Console.ResetColor();
}
