# CommonCrawl.Net

[ English ](README.md) | [ **中文** ](README.zh-CN.md) | [ 日本語 ](README.ja-JP.md) | [ Français ](README.fr-FR.md)

---

## 🇨🇳 中文

### 概述
**CommonCrawl.Net** 是一个全面的 .NET 解决方案，用于与 [Common Crawl](https://commoncrawl.org/) 数据集进行交互。它提供了导航数据集索引、支持断点续传的文件下载以及高效解析 WARC（Web ARChive）文件的工具。

本仓库包含以下组件：

- **CommonCrawl**: 核心库，处理数据集元数据、下载管理和 WARC 文件解析。
- **CommonCrawl.Parquet**: 一个专门用于读取 Common Crawl Parquet 索引文件的库。
- **CommonCraw.ConsoleApp**: 一个演示库用法的控制台应用程序。

### 功能特性
- **数据集发现**: 轻松获取最新的爬取版本。
- **弹性下载**: 内置支持 HTTP Range 请求，可恢复中断的下载。
- **WARC 解析**: 高性能的流式 GZIP 解压和 WARC 记录解析。
- **Parquet 支持**: 用于读取和处理存储为 Parquet 格式的 Common Crawl 索引文件的工具。

### 快速开始

#### 前置条件
- .NET 10.0 SDK 或更高版本。

#### 安装
目前，您可以从源代码构建项目：
```bash
git clone https://github.com/m67186636/CommonCrawl.Net.git
cd CommonCrawl.Net
dotnet build
```

### 使用示例

#### 1. 核心库 (CommonCrawl)
详细文档请参考 [核心库 README](src/CommonCrawl/README.zh-CN.md)。

```csharp
using CommonCrawl.Handlers;
using CommonCrawl.Readers;

// 获取最新的爬取信息
var latestInfo = await DataSetHandler.Instance.GetLatestAsync();

// 读取 WARC 文件流
await foreach (var record in GzWarcReader.Instance.ReadAsAsyncEnumerable("https://example.com/sample.warc.gz"))
{
    Console.WriteLine($"Record: {record.Type}");
}
```

#### 2. Parquet 读取器 (CommonCrawl.Parquet)
```csharp
using CommonCrawl.Readers;

// 从本地 Parquet 文件读取记录
await foreach (var record in ParquetReader.Instance.ReadAsAsyncEnumerable<IndexTableRecord>("cc-index.parquet"))
{
    Console.WriteLine($"URL: {record.Url}");
}
```

### 许可证
本项目采用 MIT 许可证 - 详情请参阅 [LICENSE](LICENSE) 文件。
