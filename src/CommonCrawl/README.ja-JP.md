# CommonCrawl.Net

[ English ](README.md) | [ 中文 ](README.zh-CN.md) | [ **日本語** ](README.ja-JP.md) | [ Français ](README.fr-FR.md)

---

## 🇯🇵 日本語

### 概要
**CommonCrawl.Net** は、Common Crawl データセットと対話するために設計された堅牢な .NET ライブラリです。データセットのメタデータの取得、再開機能が組み込まれたファイルのダウンロード、および GZIP 圧縮された WARC ファイルの効率的な解析プロセスを簡素化します。

### 特徴
- **DataSetHandler**: `index.commoncrawl.org` から利用可能な Common Crawl データセットバージョン（コレクション）のリストを自動的に取得してキャッシュします。
- **DownloadHandler**: 中断されたダウンロードの再開（HTTP Range リクエスト）をサポートする信頼性の高いファイルダウンロードユーティリティ。
- **GzWarcReader**: URL またはローカルストリームから WARC（Web ARChive）ファイルを直接解析するための高性能リーダー。GZIP 解凍を透過的に処理します。

### 使用例

#### 1. データセット情報と URL の取得
```csharp
using CommonCrawl.Handlers;
using CommonCrawl.Utils;

// 最新のデータセットバージョンを取得
var latestInfo = await DataSetHandler.Instance.GetLatestAsync();
Console.WriteLine($"Latest Crawl: {latestInfo.Id}");

// パス（WARC, WAT, WET など）のダウンロード URL を取得
var pathUrls = CommonCrawlUrlSplicer.FromCollectionInfo(latestInfo.Id);
Console.WriteLine($"WARC Paths URL: {pathUrls.WARC}");
```

#### 2. ファイルのダウンロード
```csharp
using CommonCrawl.Handlers;

string url = "https://data.commoncrawl.org/crawl-data/CC-MAIN-2023-50/warc.paths.gz";
string destination = "warc.paths.gz";

// レジューム（再開）機能付きでダウンロード
await DownloadHandler.Instance.DownloadAsync(url, destination);
```

#### 3. WARC レコードの読み取り
```csharp
using CommonCrawl.Readers;

string warcUrl = "https://data.commoncrawl.org/.../file.warc.gz";

// URL から直接 WARC レコードをストリーミングして解析
await foreach (var block in GzWarcReader.Instance.ReadAsAsyncEnumerable(warcUrl))
{
    Console.WriteLine($"Record Type: {block.Type}");
    if (block is WarcResponseRecord response)
    {
        Console.WriteLine($"Target URI: {response.TargetUri}");
    }
}
```