# RTK定位服务 (MyRtkService)

一个基于 .NET 10.0 和 RTKLib 的实时动态定位 (Real-Time Kinematic) Web服务平台，支持高精度GPS/GNSS数据处理和定位计算。

## 📋 项目概述

RTK定位服务提供Web界面和REST API，允许用户：

- 上传GPS原始观测数据 (RINEX格式)
- 选择定位模式 (SPP/PPP)
- 配置RTK处理参数
- 获取实时处理进度
- 下载定位计算结果
- 在地图上可视化结果

**核心技术栈：**

- 后端：ASP.NET Core 10.0 + Blazor Server
- 计算引擎：RTKLib (`rnx2rtkp`)
- UI框架：MudBlazor + LeafletForBlazor
- 队列系统：System.Threading.Channels (异步处理)

---

## 🏗️ 项目结构

```
MyRtkService/
├── Rtk.Core/                 # RTK核心引擎库
│   ├── RtkEngine.cs          # RTKLib包装层（进程调用）
│   ├── rtklibconfig/
│   │   ├── Config.cs         # 配置基类
│   │   ├── RtkLibSPPOptions.cs     # SPP模式参数
│   │   └── RtkLibPPPOptions.cs     # PPP模式参数
│   ├── SPP.conf              # SPP默认配置文件
│   └── Rtk.Core.csproj
│
├── Rtk.Web/                  # Web应用（ASP.NET Core）
│   ├── Program.cs            # 应用入口、DI配置
│   ├── Controllers/
│   │   └── RtkController.cs  # RTK任务提交API
│   ├── Services/
│   │   └── RtkQueueService.cs     # 后台任务队列处理
│   ├── Models/
│   │   └── RtkRequestDto.cs  # 数据传输对象
│   ├── Pages/
│   │   ├── _Host.cshtml      # 页面模板
│   │   └── Index.razor       # 主页
│   ├── Shared/
│   │   ├── MainLayout.razor
│   │   ├── Map.razor         # 地图组件
│   │   ├── Ppp.razor         # PPP模式界面
│   │   ├── Spp.razor         # SPP模式界面
│   │   ├── RtkLibConfig.razor     # 配置编辑组件
│   │   ├── RtkLibConfigState.cs   # 状态管理
│   │   └── TaskViewModel.cs  # 任务列表视图模型
│   ├── Data/
│   │   ├── Tasks/            # 任务数据存储（自动生成）
│   │   │   └── {TaskId}/     # 每个任务一个独立文件夹
│   │   └── DataPool/brdc/    # 广播星历数据池
│   ├── appsettings.json      # 应用配置
│   ├── appsettings.Development.json
│   └── Rtk.Web.csproj
│
├── Rtk.TestConsole/          # 控制台测试项目
│   ├── Program.cs
│   └── Rtk.TestConsole.csproj
│
├── Publish/                  # 发布文件（预编译）
│   ├── bin/
│   │   └── rnx2rtkp*         # RTKLib可执行文件
│   ├── web.config
│   ├── appsettings.json
│   └── wwwroot/
│
└── MyRtkService.slnx         # 解决方案文件
```

---

## 🚀 快速开始

### 系统要求

- **.NET 10.0 SDK** 或更高版本
- 支持平台：Windows (x64) / Linux (x64)
- 磁盘空间：≥ 500MB (用于存储任务数据)
- 内存：≥ 2GB (推荐 4GB+)

### 安装步骤

1. **克隆或下载项目**

   ```bash
   git clone <repo-url>
   cd rtkcore
   ```

2. **恢复NuGet包**

   ```bash
   dotnet restore
   ```

3. **获取RTKLib可执行文件**

   RTKLib是GNSS处理的核心库，需要预编译的二进制文件：

   - **Windows**: 将 `rnx2rtkp.exe` 放在 `Publish/bin/` 目录
   - **Linux**: 将 `rnx2rtkp` (无扩展名) 放在 `Publish/bin/` 目录

   从 [RTKLib官网](http://www.rtklib.com/) 下载预编译版本，或自行编译。

4. **构建项目**

   ```bash
   dotnet build -c Release
   ```

5. **运行Web应用**

   ```bash
   cd Rtk.Web
   dotnet run
   ```

   应用将在 `https://localhost:5001` 启动（开发模式）

6. **访问应用**
   - Web UI：`https://localhost:5001`
   - Swagger API文档：`https://localhost:5001/swagger`

---

## 📡 API 文档

### 提交RTK任务

**端点：** `POST /api/rtk/submit`

**请求头：**

```
Content-Type: multipart/form-data
```

**请求体参数：**

| 参数 | 类型 | 必需 | 说明 |
|------|------|------|------|
| `ObsFile` | File | ✓ | RINEX观测文件 (.obs) |
| `NavFile` | File | ✓ | RINEX导航文件 (.nav) |
| `ConfFile` | File | ✗ | RTK配置文件 (.conf)，若无则使用默认配置 |

**cURL示例：**

```bash
curl -X POST "https://localhost:5001/api/rtk/submit" \
  -F "ObsFile=@observation.obs" \
  -F "NavFile=@navigation.nav" \
  -F "ConfFile=@config.conf"
```

**响应示例：**

```json
{
  "taskId": "a90ad1e26ffc4a10992aacf5e9967df7",
  "status": "queued",
  "message": "任务已提交，等待处理"
}
```

### 查询任务状态

**端点：** `GET /api/rtk/status/{taskId}`

**响应示例：**

```json
{
  "taskId": "a90ad1e26ffc4a10992aacf5e9967df7",
  "status": "processing",
  "progress": 45,
  "createdTime": "2026-01-18T10:30:00Z",
  "completedTime": null
}
```

### 下载结果文件

**端点：** `GET /api/rtk/result/{taskId}`

返回定位计算结果文件 (通常为 .pos 格式)

---

## 🏃 工作流程

```
1. 用户上传文件
   ↓
2. 后端API接收 & 保存文件
   ├─ 生成唯一TaskId (UUID)
   ├─ 创建 /Data/Tasks/{TaskId}/ 文件夹
   ├─ 保存 .obs, .nav, .conf 文件
   ↓
3. 任务加入队列
   ├─ RtkQueueService 无限通道
   ├─ 后台线程监听
   ↓
4. 异步处理任务
   ├─ 读取任务参数
   ├─ 调用 rnx2rtkp 进程
   ├─ 实时捕获进程输出
   ├─ 监听进程完成
   ↓
5. 保存结果
   ├─ 输出文件 → /Data/Tasks/{TaskId}/result.pos
   ├─ 日志 → /Data/Tasks/{TaskId}/process.log
   ↓
6. 用户下载/查看
   └─ 通过Web UI 或 API
```

---

## ⚙️ 配置说明

### 上传文件大小限制

在 `Rtk.Web/Program.cs` 中修改：

```csharp
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 100_000_000; // 改为所需的字节数
});

builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = 100_000_000; // 必须与上面一致
});
```

### 应用配置文件

**appsettings.json：**

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

### RTK处理参数

在 `Rtk.Core/rtklibconfig/` 中定义RTK参数类，支持：

- **SPP** (单点定位) - `RtkLibSPPOptions.cs`
- **PPP** (精密单点定位) - `RtkLibPPPOptions.cs`

---

## 🔧 开发指南

### 添加新的定位模式

1. 在 `Rtk.Core/rtklibconfig/` 创建新的配置类，继承 `Config.cs`
2. 在 `RtkController.cs` 中添加对应的处理逻辑
3. 在Blazor UI中添加对应的参数设置界面

### 扩展任务队列

修改 `RtkQueueService.cs`：

```csharp
// 添加新的任务类型
public record NewTaskType(string TaskId, ...);

// 在 ExecuteAsync 中处理新任务
await foreach (var task in _channel.Reader.ReadAllAsync(stoppingToken))
{
    if (task is NewTaskType newTask)
    {
        // 处理逻辑
    }
}
```

### 本地调试

1. 使用Visual Studio Code的C#开发工具包
2. 在 `RtkEngine.cs` 中的进程调用处设置断点
3. F5启动调试，监控进程输出和文件操作

---

## 📊 监控与日志

### 查看日志

日志输出到控制台，可通过修改 `appsettings.Development.json` 调整日志级别：

```json
{
  "Logging": {
    "LogLevel": {
      "Rtk": "Debug"
    }
  }
}
```

### 任务数据位置

所有任务数据存储在：`Rtk.Web/Data/Tasks/{TaskId}/`

包含：

- `observation.obs` - 原始观测文件
- `navigation.nav` - 导航文件
- `config.conf` - 配置文件
- `result.pos` - 定位结果
- `process.log` - 处理日志

---

## ⚠️ 故障排除

#### 问题1：找不到 rnx2rtkp 可执行文件

**错误消息：** `找不到 RTKLib 可执行文件`

**解决方案：**

- 确保 `rnx2rtkp` (或 `rnx2rtkp.exe`) 在 `bin/` 目录
- 检查文件权限 (Linux需要可执行权限)

  ```bash
  chmod +x bin/rnx2rtkp
  ```

- 确认二进制文件与操作系统匹配 (Windows x64 / Linux x64)

#### 问题2：文件上传超时

**错误消息：** `Request timeout` 或 `413 Payload Too Large`

**解决方案：**

- 检查 `Program.cs` 中的 `MultipartBodyLengthLimit` 是否足够大
- 检查服务器磁盘空间是否充足
- 增加Kestrel请求超时时间

#### 问题3：任务一直处于 "processing" 状态

**解决方案：**

- 查看控制台日志，检查 `rnx2rtkp` 进程是否卡死
- 检查RINEX文件格式是否有效
- 检查配置文件参数是否正确

#### 问题4：Web UI无法连接

**错误消息：** `连接被拒绝` 或 `ERR_CONNECTION_REFUSED`

**解决方案：**

- 确认应用已启动：`dotnet run`
- 检查防火墙设置
- 检查端口 5001 (HTTPS) 或 5000 (HTTP) 是否被占用
- 修改 `appsettings.json` 更改端口配置

---

## 🔐 安全性建议

### 生产环境部署

1. **禁用Swagger文档**

   ```csharp
   if (app.Environment.IsDevelopment())
   {
       app.UseSwagger();
   }
   ```

2. **启用HTTPS**

   ```csharp
   app.UseHttpsRedirection();
   ```

3. **文件路径验证**

   - 验证上传文件名，防止路径遍历攻击
   - 使用 `Path.GetFileName()` 获取安全的文件名

4. **添加身份验证**

   ```csharp
   builder.Services.AddAuthentication(...);
   builder.Services.AddAuthorization(...);
   ```

5. **定期清理任务数据**

   - 实现定时任务删除过期文件
   - 防止磁盘空间持续增长

---

## 📈 性能优化

### 队列处理优化

- 使用 `System.Threading.Channels` 实现高效异步队列
- 支持并发处理多个任务（取决于CPU核心数）
- 定期监控队列长度和处理时间

### 内存优化

- 在处理大文件时使用流式IO
- 避免完整加载RINEX文件到内存

### 磁盘优化

- 定期清理过期的任务数据
- 实现增量备份策略

---

## 📝 常见配置文件

### SPP模式配置示例 (SPP.conf)

```
# 定位模式
pos1-posmode=1          # 1=SPP

# 输出格式
output-opt=1            # 1=LLH (纬度/经度/高度)

# 采样时间
pos1-soltype=0          # 0=single solution

# 卡尔曼滤波
pos1-elmask=15          # 仰角掩码 (15°)
```

---

## 🤝 贡献指南

1. Fork项目
2. 创建特性分支 (`git checkout -b feature/AmazingFeature`)
3. 提交更改 (`git commit -m 'Add some AmazingFeature'`)
4. 推送到分支 (`git push origin feature/AmazingFeature`)
5. 提交Pull Request

---

## 📄 许可证

本项目基于RTKLib库，请遵守相应的许可协议。

RTKLib使用BSD许可证。详见 [RTKLib许可证](http://www.rtklib.com/)

---

## 📞 支持与反馈

- 📧 提交Issue报告问题
- 💬 讨论功能需求
- 🐛 帮助改进文档

---

## 🔗 相关资源

- [RTKLib官网](http://www.rtklib.com/)
- [RINEX格式说明](https://www.igs.org/formats)
- [ASP.NET Core文档](https://docs.microsoft.com/aspnet/core)
- [Blazor文档](https://docs.microsoft.com/aspnet/blazor)

---

**最后更新：** 2026年1月18日  
**版本：** 1.0.0
