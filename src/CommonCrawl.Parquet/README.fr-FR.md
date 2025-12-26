# CommonCrawl.Parquet

[English](README.md) | [中文](README.zh-CN.md) | [日本語](README.ja.md) | [Français](README.fr.md)

Une bibliothèque .NET pour lire les données d'index Common Crawl stockées au format Parquet. Cette bibliothèque fournit des modèles fortement typés et un lecteur efficace pour traiter les enregistrements d'index Common Crawl.

## Fonctionnalités

- **Modèles fortement typés** : Mappe le schéma Parquet de Common Crawl à la classe C# `IndexTableRecord`.
- **Lecture efficace** : Utilise `ParquetReader` pour lire les fichiers Parquet de manière asynchrone.
- **Filtrage** : Prend en charge les prédicats pour filtrer les enregistrements lors de la lecture.

## Installation

Assurez-vous de disposer des dépendances nécessaires. Ce projet utilise `Parquet.Net`.

## Utilisation

Vous pouvez utiliser `ParquetReader.Instance` pour lire des fichiers Parquet. Le lecteur renvoie un `IAsyncEnumerable<T>`, permettant un traitement efficace de la mémoire.

```csharp
using CommonCrawl.Readers;
using CommonCrawl.Models;

// Lire à partir d'un chemin de fichier
var reader = ParquetReader.Instance;
string filePath = "path/to/cc-index.parquet";

await foreach (var record in reader.ReadAsAsyncEnumerable<IndexTableRecord>(filePath))
{
    Console.WriteLine($"URL : {record.Url}, Date de récupération : {record.FetchTime}");
}

// Lire avec un filtre (par exemple, uniquement les récupérations réussies)
await foreach (var record in reader.ReadAsAsyncEnumerable<IndexTableRecord>(filePath, r => r.FetchStatus == 200))
{
    Console.WriteLine($"URL valide trouvée : {record.Url}");
}
```

## Modèles

### IndexTableRecord

Représente un enregistrement unique dans l'index Common Crawl. Les propriétés clés incluent :

- `Url` : La chaîne URL complète.
- `UrlSurtKey` : Clé URL SURT pour la canonicalisation.
- `UrlHostName` : Nom d'hôte de l'URL.
- `FetchTime` : Horodatage de la capture.
- `FetchStatus` : Code d'état HTTP.
- `ContentMimeType` : Type MIME du contenu.
- `WarcFilename` : Emplacement du fichier WARC dans le bucket S3 de Common Crawl.
- `WarcRecordOffset` & `WarcRecordLength` : Position de l'enregistrement dans le fichier WARC.

Pour une liste complète des champs, reportez-vous au [code source](Models/IndexTableRecord.cs) ou au [Schéma d'index Common Crawl](https://github.com/commoncrawl/cc-index-table/blob/main/src/main/resources/schema/cc-index-schema-flat.json).
