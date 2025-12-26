using CommonCrawl.Models;

namespace CommonCrawl.Utils;

public class CommonCrawlUrlSplicer
{
    private const string BaseUrl = "https://data.commoncrawl.org/";
    public static PathUrl FromCollectionInfo(string connectionId)
    {
        return new PathUrl(connectionId);
    }

    public static string Build(string path)
    {
        
        return $"{BaseUrl}{path.TrimStart('/')}";
    }
}