namespace CommonCrawl.Models;

public sealed record PathUrl
{
    public string CollectionId { get; }

    public PathUrl(string collectionId)
    {
        if (string.IsNullOrWhiteSpace(collectionId))
        {
            throw new ArgumentException("collectionId must not be null or whitespace.", nameof(collectionId));
        }

        CollectionId = collectionId;


        Segments = PathUrlTemplate("segment");
        WARC = PathUrlTemplate("warc");
        WAT = PathUrlTemplate("wat");
        WET = PathUrlTemplate("wet");
        Robots = PathUrlTemplate("robotstxt");
        Non200Responses = PathUrlTemplate("non200responses");
        UrlIndex = PathUrlTemplate("cc-index");
        ColumnarUrlIndex = PathUrlTemplate("cc-index-table");
    }

    private string PathUrlTemplate(string segment) =>
        $"https://data.commoncrawl.org/crawl-data/{CollectionId}/{segment}.paths.gz";

    public string Segments { get; }
    public string WARC { get; }
    public string WAT { get; }
    public string WET { get; }
    public string Robots { get; }
    public string Non200Responses { get; }
    public string UrlIndex { get; }
    public string ColumnarUrlIndex { get; }
}