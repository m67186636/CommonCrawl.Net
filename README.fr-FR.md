# CommonCrawl.Net

[ English ](README.md) | [ 中文 ](README.zh-CN.md) | [ 日本語 ](README.ja-JP.md) | [ **Français** ](README.fr-FR.md)

---

## 🇫🇷 Français

### Vue d'ensemble
**CommonCrawl.Net** est une solution .NET complète pour interagir avec le jeu de données [Common Crawl](https://commoncrawl.org/). Elle fournit des outils pour naviguer dans l'index du jeu de données, télécharger des fichiers avec prise en charge de la reprise, et analyser efficacement les fichiers WARC (Web ARChive).

Ce dépôt contient les composants suivants :

- **CommonCrawl** : La bibliothèque principale gérant les métadonnées des jeux de données, la gestion des téléchargements et l'analyse des fichiers WARC.
- **CommonCrawl.Parquet** : Une bibliothèque spécialisée pour la lecture des fichiers d'index Parquet de Common Crawl.
- **CommonCraw.ConsoleApp** : Une application console démontrant l'utilisation des bibliothèques.

### Fonctionnalités
- **Découverte de jeux de données** : Récupérez facilement les dernières versions de crawl disponibles.
- **Téléchargements résilients** : Prise en charge intégrée des requêtes HTTP Range pour reprendre les téléchargements interrompus.
- **Analyse WARC** : Décompression GZIP en streaming haute performance et analyse des enregistrements WARC.
- **Support Parquet** : Outils pour lire et traiter les fichiers d'index Common Crawl stockés au format Parquet.

### Pour commencer

#### Prérequis
- SDK .NET 10.0 ou ultérieur.

#### Installation
Actuellement, vous pouvez compiler le projet à partir des sources :
```bash
git clone https://github.com/m67186636/CommonCrawl.Net.git
cd CommonCrawl.Net
dotnet build
```

### Exemples d'utilisation

#### 1. Bibliothèque principale (CommonCrawl)
Référez-vous au [README de la bibliothèque principale](src/CommonCrawl/README.fr-FR.md) pour une documentation détaillée.

```csharp
using CommonCrawl.Handlers;
using CommonCrawl.Readers;

// Obtenir les dernières informations de crawl
var latestInfo = await DataSetHandler.Instance.GetLatestAsync();

// Lire un flux de fichier WARC
await foreach (var record in GzWarcReader.Instance.ReadAsAsyncEnumerable("https://example.com/sample.warc.gz"))
{
    Console.WriteLine($"Record: {record.Type}");
}
```

#### 2. Lecteur Parquet (CommonCrawl.Parquet)
```csharp
using CommonCrawl.Readers;

// Lire des enregistrements depuis un fichier Parquet local
await foreach (var record in ParquetReader.Instance.ReadAsAsyncEnumerable<IndexTableRecord>("cc-index.parquet"))
{
    Console.WriteLine($"URL: {record.Url}");
}
```

### Licence
Ce projet est sous licence MIT - voir le fichier [LICENSE](LICENSE) pour plus de détails.
