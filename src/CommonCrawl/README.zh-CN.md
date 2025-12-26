# CommonCrawl.Net

[ English ](README.md) | [ **中文** ](README.zh-CN.md) | [ 日本語 ](README.ja-JP.md) | [ Français ](README.fr-FR.md)

---

## 🇨🇳 中文

### 概述
**CommonCrawl.Net** 是一个强大的 .NET 库，专为与 Common Crawl 数据集交互而设计。它简化了获取数据集元数据、通过内置断点续传功能下载文件以及高效解析 GZIP 压缩的 WARC 文件的过程。

### 功能特性
- **DataSetHandler**: 自动从 `index.commoncrawl.org` 获取并缓存可用的 Common Crawl 数据集版本（集合）列表。
- **DownloadHandler**: 可靠的文件下载工具，支持恢复中断的下载（HTTP Range 请求）。
- **GzWarcReader**: 一个高性能读取器，用于直接从 URL 或本地流解析 WARC（Web ARChive）文件。它透明地处理 GZIP 解压。

### 使用示例

#### 1. 获取数据集信息和 URL
```csharp
using CommonCrawl.Handlers;
using CommonCrawl.Utils;

// 获取最新的数据集版本
var latestInfo = await DataSetHandler.Instance.GetLatestAsync();
Console.WriteLine($"Latest Crawl: {latestInfo.Id}");

// 获取路径的下载 URL (WARC, WAT, WET 等)
var pathUrls = CommonCrawlUrlSplicer.FromCollectionInfo(latestInfo.Id);
Console.WriteLine($"WARC Paths URL: {pathUrls.WARC}");
```

#### 2. 下载文件
```csharp
using CommonCrawl.Handlers;

string url = "https://data.commoncrawl.org/crawl-data/CC-MAIN-2023-50/warc.paths.gz";
string destination = "warc.paths.gz";

// 支持断点续传的下载
await DownloadHandler.Instance.DownloadAsync(url, destination);
```

#### 3. 读取 WARC 记录
```csharp
using CommonCrawl.Readers;

string warcUrl = "https://data.commoncrawl.org/.../file.warc.gz";

// 直接从 URL 流式传输并解析 WARC 记录
await foreach (var block in GzWarcReader.Instance.ReadAsAsyncEnumerable(warcUrl))
{
    Console.WriteLine($"Record Type: {block.Type}");
    if (block is WarcResponseRecord response)
    {
        Console.WriteLine($"Target URI: {response.TargetUri}");
    }
}
```