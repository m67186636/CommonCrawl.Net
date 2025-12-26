# CommonCrawl.Parquet

[English](README.md) | [中文](README.zh-CN.md) | [日本語](README.ja.md) | [Français](README.fr.md)

A .NET library for reading Common Crawl index data stored in Parquet format. This library provides strongly-typed models and an efficient reader to process Common Crawl index records.

## Features

- **Strongly Typed Models**: Maps Common Crawl Parquet schema to the `IndexTableRecord` C# class.
- **Efficient Reading**: Uses `ParquetReader` to read Parquet files asynchronously.
- **Filtering**: Supports predicates to filter records while reading.

## Installation

Ensure you have the necessary dependencies. This project uses `Parquet.Net`.

## Usage

You can use `ParquetReader.Instance` to read Parquet files. The reader returns an `IAsyncEnumerable<T>`, allowing for memory-efficient processing.

```csharp
using CommonCrawl.Readers;
using CommonCrawl.Models;

// Read from a file path
var reader = ParquetReader.Instance;
string filePath = "path/to/cc-index.parquet";

await foreach (var record in reader.ReadAsAsyncEnumerable<IndexTableRecord>(filePath))
{
    Console.WriteLine($"URL: {record.Url}, Fetch Time: {record.FetchTime}");
}

// Read with a filter (e.g., only successful fetches)
await foreach (var record in reader.ReadAsAsyncEnumerable<IndexTableRecord>(filePath, r => r.FetchStatus == 200))
{
    Console.WriteLine($"Found valid URL: {record.Url}");
}
```

## Models

### IndexTableRecord

Represents a single record in the Common Crawl index. Key properties include:

- `Url`: The full URL string.
- `UrlSurtKey`: SURT URL key for canonicalization.
- `UrlHostName`: Hostname of the URL.
- `FetchTime`: Timestamp of the capture.
- `FetchStatus`: HTTP status code.
- `ContentMimeType`: MIME type of the content.
- `WarcFilename`: Location of the WARC file in Common Crawl's S3 bucket.
- `WarcRecordOffset` & `WarcRecordLength`: Position of the record in the WARC file.

For a full list of fields, refer to the [source code](Models/IndexTableRecord.cs) or the [Common Crawl Index Schema](https://github.com/commoncrawl/cc-index-table/blob/main/src/main/resources/schema/cc-index-schema-flat.json).
