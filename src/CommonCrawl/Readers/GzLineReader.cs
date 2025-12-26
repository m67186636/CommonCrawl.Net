namespace CommonCrawl.Readers;

public interface IGzLineReader
{
    IAsyncEnumerable<string> ReadAsAsyncEnumerable(string url);
}
public class GzLineReader : IGzLineReader
{
    public static IGzLineReader Instance { get; } = new GzLineReader();
    public IAsyncEnumerable<string> ReadAsAsyncEnumerable(string url)
    {
        return url.StartsWith("http") ? ReadAsAsyncEnumerableFromUrl(url) : ReadAsAsyncEnumerableFromFile(url);
    }
    private async IAsyncEnumerable<string> ReadAsAsyncEnumerableFromUrl(string url)
    {
        var httpClient = HttpClientCreator.Create();
        var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
        var responseMessage = await httpClient.SendAsync(requestMessage, HttpCompletionOption.ResponseHeadersRead);
        responseMessage.EnsureSuccessStatusCode();
        await using var stream = await responseMessage.Content.ReadAsStreamAsync();
        await using var gzipStream = new System.IO.Compression.GZipStream(stream, System.IO.Compression.CompressionMode.Decompress);
        using var reader = new StreamReader(gzipStream);
        while (await reader.ReadLineAsync() is { } line)
        {
            yield return line;
        }
    }
    private async IAsyncEnumerable<string> ReadAsAsyncEnumerableFromFile(string filename)
    {
        await using var stream = File.OpenRead(filename);
        await using var gzipStream = new System.IO.Compression.GZipStream(stream, System.IO.Compression.CompressionMode.Decompress);
        using var reader = new StreamReader(gzipStream);
        while (await reader.ReadLineAsync() is { } line)
        {
            yield return line;
        }
    }
}
