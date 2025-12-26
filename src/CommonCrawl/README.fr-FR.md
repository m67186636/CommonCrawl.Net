# CommonCrawl.Net

[ English ](README.md) | [ 中文 ](README.zh-CN.md) | [ 日本語 ](README.ja-JP.md) | [ **Français** ](README.fr-FR.md)

---

## 🇫🇷 Français

### Vue d'ensemble
**CommonCrawl.Net** est une bibliothèque .NET robuste conçue pour interagir avec le jeu de données Common Crawl. Elle simplifie le processus de récupération des métadonnées des jeux de données, le téléchargement de fichiers avec prise en charge intégrée de la reprise, et l'analyse efficace des fichiers WARC compressés en GZIP.

### Fonctionnalités
- **DataSetHandler** : Récupère et met en cache automatiquement la liste des versions de jeux de données Common Crawl disponibles (collections) depuis `index.commoncrawl.org`.
- **DownloadHandler** : Utilitaire de téléchargement de fichiers fiable qui prend en charge la reprise des téléchargements interrompus (requêtes HTTP Range).
- **GzWarcReader** : Un lecteur haute performance pour analyser les fichiers WARC (Web ARChive) directement depuis une URL ou un flux local. Il gère la décompression GZIP de manière transparente.

### Exemples d'utilisation

#### 1. Obtenir les informations du jeu de données et les URL
```csharp
using CommonCrawl.Handlers;
using CommonCrawl.Utils;

// Obtenir la dernière version du jeu de données
var latestInfo = await DataSetHandler.Instance.GetLatestAsync();
Console.WriteLine($"Latest Crawl: {latestInfo.Id}");

// Obtenir les URL de téléchargement pour les chemins (WARC, WAT, WET, etc.)
var pathUrls = CommonCrawlUrlSplicer.FromCollectionInfo(latestInfo.Id);
Console.WriteLine($"WARC Paths URL: {pathUrls.WARC}");
```

#### 2. Télécharger un fichier
```csharp
using CommonCrawl.Handlers;

string url = "https://data.commoncrawl.org/crawl-data/CC-MAIN-2023-50/warc.paths.gz";
string destination = "warc.paths.gz";

// Téléchargement avec capacité de reprise
await DownloadHandler.Instance.DownloadAsync(url, destination);
```

#### 3. Lire les enregistrements WARC
```csharp
using CommonCrawl.Readers;

string warcUrl = "https://data.commoncrawl.org/.../file.warc.gz";

// Flux et analyse des enregistrements WARC directement depuis l'URL
await foreach (var block in GzWarcReader.Instance.ReadAsAsyncEnumerable(warcUrl))
{
    Console.WriteLine($"Record Type: {block.Type}");
    if (block is WarcResponseRecord response)
    {
        Console.WriteLine($"Target URI: {response.TargetUri}");
    }
}
```