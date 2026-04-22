using Microsoft.AspNetCore.Http.Features;
using Rtk.Core;
using Rtk.Web.Services;
using MudBlazor.Services; 

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 100_000_000; // 100 MB
});
builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = 100_000_000; // 必须和上面一致
});
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<RtkEngine>(sp => 
{
    var env = sp.GetRequiredService<IWebHostEnvironment>();
    return new RtkEngine(env.ContentRootPath);
});
builder.Services.AddSingleton<RtkQueueService>();
builder.Services.AddHostedService(provider => provider.GetRequiredService<RtkQueueService>());
builder.Services.AddScoped<RtkLibConfigState>();
// =========================================================
// 【新增】注册 Blazor 和 UI 服务
// =========================================================
builder.Services.AddRazorPages();       // 启用页面支持
builder.Services.AddServerSideBlazor(); // 启用 Blazor 服务端模式
builder.Services.AddMudServices();      // 启用漂亮的 UI 组件
// =========================================================

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // 启用 Swagger 网页
    app.UseSwagger();
    app.UseSwaggerUI(); // 这样你访问 /swagger 就能看到界面了
}
app.UseStaticFiles();
app.UseRouting(); // 确保这行在 MapControllers 之前


//app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.MapControllers();
Console.WriteLine($"[系统启动] 运行目录: {app.Environment.ContentRootPath}");
app.MapBlazorHub(); // SignalR 通道
app.MapFallbackToPage("/_Host"); // 找不到 API 就去首页
app.Run();

