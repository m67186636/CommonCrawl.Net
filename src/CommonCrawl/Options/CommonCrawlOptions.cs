using System.Text.Json;

namespace CommonCrawl.Options;

public class CommonCrawlOptions
{
    public static string LocalPath { set; get; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), nameof(CommonCrawl));
    public static TimeSpan CollectionExpired { set; get; } = TimeSpan.FromDays(1);

    public static JsonSerializerOptions? JsonSerializerOptions { get; set; } =
        new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
}

