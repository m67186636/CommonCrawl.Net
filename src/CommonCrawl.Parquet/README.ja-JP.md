# CommonCrawl.Parquet

[English](README.md) | [中文](README.zh-CN.md) | [日本語](README.ja-JP.md) | [Français](README.fr-FR.md)

Parquet形式で保存されたCommon Crawlインデックスデータを読み取るための.NETライブラリです。このライブラリは、Common Crawlインデックスレコードを処理するための強力に型付けされたモデルと効率的なリーダーを提供します。

## 特徴

- **強力に型付けされたモデル**: Common Crawl Parquetスキーマを `IndexTableRecord` C#クラスにマッピングします。
- **効率的な読み取り**: `ParquetReader`を使用してParquetファイルを非同期に読み取ります。
- **フィルタリング**: 読み取り時に述語（Predicate）を使用してレコードをフィルタリングできます。

## インストール

NuGet 経由でパッケージをインストールします：

[![NuGet](https://img.shields.io/nuget/v/CommonCrawl.Parquet.svg)](https://www.nuget.org/packages/CommonCrawl.Parquet)

```bash
dotnet add package CommonCrawl.Parquet
```

## 使い方

`ParquetReader.Instance` を使用してParquetファイルを読み取ることができます。リーダーは `IAsyncEnumerable<T>` を返すため、メモリ効率の良い処理が可能です。

```csharp
using CommonCrawl.Readers;
using CommonCrawl.Models;

// ファイルパスから読み取る
var reader = ParquetReader.Instance;
string filePath = "path/to/cc-index.parquet";

await foreach (var record in reader.ReadAsAsyncEnumerable<IndexTableRecord>(filePath))
{
    Console.WriteLine($"URL: {record.Url}, 取得日時: {record.FetchTime}");
}

// フィルタを使用して読み取る（例：成功した取得のみ）
await foreach (var record in reader.ReadAsAsyncEnumerable<IndexTableRecord>(filePath, r => r.FetchStatus == 200))
{
    Console.WriteLine($"有効なURLが見つかりました: {record.Url}");
}
```

## モデル

### IndexTableRecord

Common Crawlインデックス内の単一のレコードを表します。主なプロパティは以下の通りです：

- `Url`: 完全なURL文字列。
- `UrlSurtKey`: 正規化のためのSURT URLキー。
- `UrlHostName`: URLのホスト名。
- `FetchTime`: キャプチャのタイムスタンプ。
- `FetchStatus`: HTTPステータスコード。
- `ContentMimeType`: コンテンツのMIMEタイプ。
- `WarcFilename`: Common CrawlのS3バケット内のWARCファイルの場所。
- `WarcRecordOffset` & `WarcRecordLength`: WARCファイル内のレコードの位置。

フィールドの完全なリストについては、[ソースコード](Models/IndexTableRecord.cs) または [Common Crawl インデックススキーマ](https://github.com/commoncrawl/cc-index-table/blob/main/src/main/resources/schema/cc-index-schema-flat.json) を参照してください。
