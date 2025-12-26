# CommonCrawl.Net

[ English ](README.md) | [ 中文 ](README.zh-CN.md) | [ **日本語** ](README.ja-JP.md) | [ Français ](README.fr-FR.md)

---

## 🇯🇵 日本語

### 概要
**CommonCrawl.Net** は、[Common Crawl](https://commoncrawl.org/) データセットと対話するための包括的な .NET ソリューションです。データセットインデックスのナビゲーション、再開機能をサポートするファイルダウンロード、および WARC（Web ARChive）ファイルの効率的な解析ツールを提供します。

このリポジトリには以下のコンポーネントが含まれています：

- **CommonCrawl**: データセットのメタデータ、ダウンロード管理、および WARC ファイル解析を処理するコアライブラリ。
- **CommonCrawl.Parquet**: Common Crawl の Parquet インデックスファイルを読み取るための専用ライブラリ。
- **CommonCraw.ConsoleApp**: ライブラリの使用法を示すコンソールアプリケーション。

### 特徴
- **データセットの発見**: 最新のクロールバージョンを簡単に取得できます。
- **堅牢なダウンロード**: 中断されたダウンロードを再開するための HTTP Range リクエストを組み込みサポートしています。
- **WARC 解析**: 高性能なストリーミング GZIP 解凍と WARC レコードの解析。
- **Parquet サポート**: Parquet 形式で保存された Common Crawl インデックスファイルを読み取り処理するためのツール。

### 始め方

#### 前提条件
- .NET 10.0 SDK 以降。

#### インストール
NuGet 経由でパッケージをインストールできます：

- **CommonCrawl.Net**: [![NuGet](https://img.shields.io/nuget/v/CommonCrawl.Net.svg)](https://www.nuget.org/packages/CommonCrawl.Net)
- **CommonCrawl.Parquet**: [![NuGet](https://img.shields.io/nuget/v/CommonCrawl.Parquet.svg)](https://www.nuget.org/packages/CommonCrawl.Parquet)

```bash
dotnet add package CommonCrawl.Net
dotnet add package CommonCrawl.Parquet
```

または、ソースからプロジェクトをビルドできます：
```bash
git clone https://github.com/m67186636/CommonCrawl.Net.git
cd CommonCrawl.Net
dotnet build
```

### 使用例

#### 1. コアライブラリ (CommonCrawl)
詳細なドキュメントについては、[コアライブラリの README](src/CommonCrawl/README.ja-JP.md) を参照してください。

```csharp
using CommonCrawl.Handlers;
using CommonCrawl.Readers;

// 最新のクロール情報を取得
var latestInfo = await DataSetHandler.Instance.GetLatestAsync();

// WARC ファイルストリームを読み取る
await foreach (var record in GzWarcReader.Instance.ReadAsAsyncEnumerable("https://example.com/sample.warc.gz"))
{
    Console.WriteLine($"Record: {record.Type}");
}
```

#### 2. Parquet リーダー (CommonCrawl.Parquet)
```csharp
using CommonCrawl.Readers;

// ローカルの Parquet ファイルからレコードを読み取る
await foreach (var record in ParquetReader.Instance.ReadAsAsyncEnumerable<IndexTableRecord>("cc-index.parquet"))
{
    Console.WriteLine($"URL: {record.Url}");
}
```

### ライセンス
このプロジェクトは MIT ライセンスの下でライセンスされています。詳細については [LICENSE](LICENSE) ファイルを参照してください。
