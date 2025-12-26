# CommonCrawl.Net

[ **English** ](README.md) | [ 中文 ](README.zh-CN.md) | [ 日本語 ](README.ja-JP.md) | [ Français ](README.fr-FR.md)

---

## 🇬🇧 English

### Overview
**CommonCrawl.Net** is a comprehensive .NET solution for interacting with the [Common Crawl](https://commoncrawl.org/) dataset. It provides tools to navigate the dataset index, download files with resume support, and parse WARC (Web ARChive) files efficiently.

This repository contains the following components:

- **CommonCrawl**: The core library handling dataset metadata, download management, and WARC file parsing.
- **CommonCrawl.Parquet**: A specialized library for reading Common Crawl's parquet index files.
- **CommonCraw.ConsoleApp**: A console application demonstrating the usage of the libraries.

### Features
- **Dataset Discovery**: Easily fetch the latest available crawl versions.
- **Resilient Downloads**: Built-in support for HTTP Range requests to resume interrupted downloads.
- **WARC Parsing**: High-performance, streaming GZIP decompression and parsing of WARC records.
- **Parquet Support**: Tools to read and process Common Crawl index files stored in Parquet format.

### Getting Started

#### Prerequisites
- .NET 10.0 SDK or later.

#### Installation
Currently, you can build the project from source:
```bash
git clone https://github.com/m67186636/CommonCrawl.Net.git
cd CommonCrawl.Net
dotnet build
```

### Usage Examples

#### 1. Core Library (CommonCrawl)
Refer to the [Core Library README](src/CommonCrawl/README.md) for detailed documentation.

```csharp
using CommonCrawl.Handlers;
using CommonCrawl.Readers;

// Get latest crawl info
var latestInfo = await DataSetHandler.Instance.GetLatestAsync();

// Read a WARC file stream
await foreach (var record in GzWarcReader.Instance.ReadAsAsyncEnumerable("https://example.com/sample.warc.gz"))
{
    Console.WriteLine($"Record: {record.Type}");
}
```

#### 2. Parquet Reader (CommonCrawl.Parquet)
```csharp
using CommonCrawl.Readers;

// Read records from a local Parquet file
await foreach (var record in ParquetReader.Instance.ReadAsAsyncEnumerable<IndexTableRecord>("cc-index.parquet"))
{
    Console.WriteLine($"URL: {record.Url}");
}
```

### License
This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
