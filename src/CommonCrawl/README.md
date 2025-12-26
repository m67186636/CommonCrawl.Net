# CommonCrawl.Net

[ **English** ](README.md) | [ 中文 ](README.zh-CN.md) | [ 日本語 ](README.ja-JP.md) | [ Français ](README.fr-FR.md)

---

## 🇬🇧 English

### Overview
**CommonCrawl.Net** is a robust .NET library designed to interact with the Common Crawl dataset. It simplifies the process of fetching dataset metadata, downloading files with built-in resume support, and parsing GZIP-compressed WARC files efficiently.

### Features
- **DataSetHandler**: Automatically fetch and cache the list of available Common Crawl dataset versions (collections) from `index.commoncrawl.org`.
- **DownloadHandler**: Reliable file downloading utility that supports resuming interrupted downloads (HTTP Range requests).
- **GzWarcReader**: A high-performance reader for parsing WARC (Web ARChive) files directly from a URL or a local stream. It handles GZIP decompression transparently.

### Usage Examples

#### 1. Get Dataset Information & URLs
```csharp
using CommonCrawl.Handlers;
using CommonCrawl.Utils;

// Get the latest dataset version
var latestInfo = await DataSetHandler.Instance.GetLatestAsync();
Console.WriteLine($"Latest Crawl: {latestInfo.Id}");

// Get download URLs for paths (WARC, WAT, WET, etc.)
var pathUrls = CommonCrawlUrlSplicer.FromCollectionInfo(latestInfo.Id);
Console.WriteLine($"WARC Paths URL: {pathUrls.WARC}");
```

#### 2. Download a File
```csharp
using CommonCrawl.Handlers;

string url = "https://data.commoncrawl.org/crawl-data/CC-MAIN-2023-50/warc.paths.gz";
string destination = "warc.paths.gz";

// Downloads with resume capability
await DownloadHandler.Instance.DownloadAsync(url, destination);
```

#### 3. Read WARC Records
```csharp
using CommonCrawl.Readers;

string warcUrl = "https://data.commoncrawl.org/.../file.warc.gz";

// Stream and parse WARC records directly from the URL
await foreach (var block in GzWarcReader.Instance.ReadAsAsyncEnumerable(warcUrl))
{
    Console.WriteLine($"Record Type: {block.Type}");
    if (block is WarcResponseRecord response)
    {
        Console.WriteLine($"Target URI: {response.TargetUri}");
    }
}
```