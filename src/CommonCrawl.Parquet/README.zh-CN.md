# CommonCrawl.Parquet

[English](README.md) | [中文](README.zh-CN.md) | [日本語](README.ja-JP.md) | [Français](README.fr-FR.md)

这是一个用于读取 Parquet 格式的 Common Crawl 索引数据的 .NET 库。该库提供了强类型模型和高效的读取器来处理 Common Crawl 索引记录。

## 功能特性

- **强类型模型**：将 Common Crawl Parquet 模式映射到 `IndexTableRecord` C# 类。
- **高效读取**：使用 `ParquetReader` 异步读取 Parquet 文件。
- **过滤支持**：支持在读取时使用谓词（Predicate）过滤记录。

## 安装

通过 NuGet 安装包：

[![NuGet](https://img.shields.io/nuget/v/CommonCrawl.Parquet.svg)](https://www.nuget.org/packages/CommonCrawl.Parquet)

```bash
dotnet add package CommonCrawl.Parquet
```

## 使用方法

您可以使用 `ParquetReader.Instance` 读取 Parquet 文件。读取器返回 `IAsyncEnumerable<T>`，支持内存高效的处理方式。

```csharp
using CommonCrawl.Readers;
using CommonCrawl.Models;

// 从文件路径读取
var reader = ParquetReader.Instance;
string filePath = "path/to/cc-index.parquet";

await foreach (var record in reader.ReadAsAsyncEnumerable<IndexTableRecord>(filePath))
{
    Console.WriteLine($"URL: {record.Url}, 抓取时间: {record.FetchTime}");
}

// 带过滤条件的读取 (例如：仅读取成功的抓取)
await foreach (var record in reader.ReadAsAsyncEnumerable<IndexTableRecord>(filePath, r => r.FetchStatus == 200))
{
    Console.WriteLine($"发现有效 URL: {record.Url}");
}
```

## 模型

### IndexTableRecord

表示 Common Crawl 索引中的一条记录。主要属性包括：

- `Url`: 完整的 URL 字符串。
- `UrlSurtKey`: 用于规范化的 SURT URL 键。
- `UrlHostName`: URL 的主机名。
- `FetchTime`: 抓取的时间戳。
- `FetchStatus`: HTTP 状态码。
- `ContentMimeType`: 内容的 MIME 类型。
- `WarcFilename`: WARC 文件在 Common Crawl S3 存储桶中的位置。
- `WarcRecordOffset` & `WarcRecordLength`: 记录在 WARC 文件中的位置。

有关字段的完整列表，请参阅 [源代码](Models/IndexTableRecord.cs) 或 [Common Crawl 索引模式](https://github.com/commoncrawl/cc-index-table/blob/main/src/main/resources/schema/cc-index-schema-flat.json)。
