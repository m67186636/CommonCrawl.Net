using CommonCrawl.Models;
using CommonCrawl.Readers;
using System.Text.Json;

namespace CommonCrawl.Handlers;

public interface IStatisticsHandler
{
    Task<StatisticsInfo> GetStatisticsAsync(string collectionId, CancellationToken cancellationToken = default);
}

public class StatisticsHandler : IStatisticsHandler
{
    public static IStatisticsHandler Instance { get; } = new StatisticsHandler();

    private StatisticsHandler()
    {
    }

    public async Task<StatisticsInfo> GetStatisticsAsync(string collectionId,
        CancellationToken cancellationToken = default)
    {
        var url = $"https://data.commoncrawl.org/crawl-analysis/{collectionId}/stats/part-00000.gz";
        var result = new StatisticsInfo();
        var lines = GzLineReader.Instance.ReadAsAsyncEnumerable(url);
        await foreach (var line in lines)
        {
            // 解析每一行数据并填充到result中
            //BUG:CC-MAIN-2016-36 has a few lines that does not start with '['
            var parts = (line.StartsWith('[') ? line : $"[{line}").Split('\t');
            if (parts.Length != 2) continue;
            var args1 = JsonSerializer.Deserialize<JsonElement[]>(parts[0]);
            var type = args1![0].GetString();
            switch (type)
            {
                case "http_status":
                    var statusCode = args1![1].GetInt32();
                    if (int.TryParse(parts[1], out var count))
                        result.AddHttpStatusCode(statusCode, count);
                    break;
                case "charset":
                    {

                        var counts = parts[1].StartsWith('[')
                            ? JsonSerializer.Deserialize<long[]>(parts[1])!
                            :
                            [
                                int.Parse(parts[1]),
                            int.Parse(parts[1])
                            ];
                        var charset = args1![1]!.GetString()!;
                        result.AddPageCharset(charset, counts[0]);
                        result.AddUrlCharset(charset, counts[1]);
                        break;
                    }

                case "languages":
                    break;
                case "primary_language":
                    {

                        var counts = parts[1].StartsWith('[')
                            ? JsonSerializer.Deserialize<int[]>(parts[1])!
                            :
                            [
                                int.Parse(parts[1]),
                            int.Parse(parts[1])
                            ];
                        var languages = args1![1]!.GetString()!;
                        result.AddPageLanguage(languages, counts[0]);
                        result.AddUrlLanguage(languages, counts[1]);
                        break;
                    }
                case "tld":
                    {
                        var counts = parts[1].StartsWith('[')
                            ? JsonSerializer.Deserialize<long[]>(parts[1])!
                            : [long.Parse(parts[1])];
                        var domain = args1![1]!.GetString()!;
                    }

                    break;

                case "size":
                    {
                        var total = long.Parse(parts[1]);
                        var domain = args1![1]!.GetString()!;
                        switch (domain)
                        {
                            case "domain":
                                result.DomainTotal = total;
                                break;
                            case "fetch":
                                result.FetchTotal = total;
                                break;
                            case "host":
                                result.HostTotal = total;
                                break;
                            case "page":
                                result.PageTotal = total;
                                break;
                            case "surt_domain":
                                result.SurtDomainTotal = total;
                                break;
                            case "url":
                                result.UrlTotal = total;
                                break;
                        }
                    }
                    break;
            }
        }

        return result;
    }
}