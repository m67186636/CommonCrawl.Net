using CommonCrawl.Models;
using System.Text;

namespace CommonCrawl.Readers;

public interface IGzWarcReader
{
    IAsyncEnumerable<WarcBlockInfo> ReadAsAsyncEnumerable(string url);
    IAsyncEnumerable<WarcBlockInfo> ReadAsAsyncEnumerable(Stream stream);
    Task<WarcBlockInfo?> ReadBlockAsync(string url, int offset, int length);
}
public class GzWarcReader : IGzWarcReader
{
    public static IGzWarcReader Instance { get; } = new GzWarcReader();

    public async IAsyncEnumerable<WarcBlockInfo> ReadAsAsyncEnumerable(string url)
    {
        var httpClient = HttpClientCreator.Create();
        var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
        var responseMessage = await httpClient.SendAsync(requestMessage, HttpCompletionOption.ResponseHeadersRead);
        responseMessage.EnsureSuccessStatusCode();
        await using var stream = await responseMessage.Content.ReadAsStreamAsync();
        await foreach (var block in ReadAsAsyncEnumerable(stream))
        {
            yield return block;
        }
    }
    public async IAsyncEnumerable<WarcBlockInfo> ReadAsAsyncEnumerable(Stream stream)
    {
        await using var gzipStream = new System.IO.Compression.GZipStream(stream, System.IO.Compression.CompressionMode.Decompress); 
        await using var bufferedStream = new BufferedStream(gzipStream);
        using var reader = new BinaryReader(bufferedStream);
        while (ReadBlock(reader) is { } result)
        {
            yield return result;
        }
    }

    public async Task<WarcBlockInfo?> ReadBlockAsync(string url, int offset, int length)
    {
        var httpClient = HttpClientCreator.Create();
        var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
        requestMessage.Headers.Range = new System.Net.Http.Headers.RangeHeaderValue(offset, offset + length);
        var responseMessage = await httpClient.SendAsync(requestMessage, HttpCompletionOption.ResponseHeadersRead);
        responseMessage.EnsureSuccessStatusCode();
        await using var stream = await responseMessage.Content.ReadAsStreamAsync();
        await using var gzipStream = new System.IO.Compression.GZipStream(stream, System.IO.Compression.CompressionMode.Decompress);
        await using var bufferedStream = new BufferedStream(gzipStream);
        var reader = new BinaryReader(bufferedStream);
        var record = ReadBlock(reader);
        return record;
    }

    private static WarcBlockInfo? ReadBlock(BinaryReader reader)
    {

        var headers = ReadWarcHeaders(reader);
        if (headers is null) return null;

        var result = WarcBlockInfo.Create(headers);

        if (result is WarcResponseRecord responseRecord)
        {
            responseRecord.ResponseHeaders = ReadHeaders<ResponseHeaders>(reader);
            if (responseRecord.ResponseHeaders.ContentLength.HasValue)
            {
                var contentLength = responseRecord.ResponseHeaders.ContentLength.Value;
                var buffer = reader.ReadBytes((int)contentLength);
                responseRecord.Payload = Encoding.UTF8.GetString(buffer);
            }
        }
        else if (result is WarcRequestRecord requestRecord)
        {
            requestRecord.RequestHeaders = ReadHeaders<RequestHeaders>(reader);
        }
        else if (result is WarcInfoRecord infoRecord)
        {
            infoRecord.InfoHeaders = ReadHeaders<InfoHeaders>(reader);
        }
        else if (result is WarcConversionRecord conversionRecord)
        {
            if (conversionRecord.Headers.ContentLength.HasValue)
            {
                var contentLength = conversionRecord.Headers.ContentLength.Value;
                var buffer = reader.ReadBytes((int)contentLength);
                conversionRecord.Payload = Encoding.UTF8.GetString(buffer);
            }
        }
        else if (result is WarcMetadataRecord metadataRecord)
        {
            if (metadataRecord.Headers.ContentLength.HasValue)
            {
                var contentLength = metadataRecord.Headers.ContentLength.Value;
                var buffer = reader.ReadBytes((int)contentLength);
                metadataRecord.Payload = Encoding.UTF8.GetString(buffer);
            }
        }

        return result;
    }


    private static string? ReadLine(BinaryReader reader)
    {
        var builder = new StringBuilder();
        int b;
        var hasChars = false;
        while ((b = reader.Read()) != -1)
        {
            hasChars = true;
            var c = (char)b;
            if (c == '\n') break;
            if (c != '\r') builder.Append(c);
        }

        return !hasChars ? null : builder.ToString();
    }


    private static WarcHeaders? ReadWarcHeaders(BinaryReader reader)
    {
        var firstLine = ReadLine(reader);
        while (string.IsNullOrWhiteSpace(firstLine))
        {
            if (firstLine is null) return null;
            firstLine = ReadLine(reader);
        }


        var result = new WarcHeaders() { Version = firstLine };
        while (ReadLine(reader) is { } line)
        {
            if (string.IsNullOrWhiteSpace((line))) break;
            var index = line.IndexOf(':');
            if (index <= 0) continue;
            var key = line[..index].Trim().ToLower();
            var value = line[(index + 1)..].Trim();
            result[key] = value;
        }
        return result;
    }

    private static T ReadHeaders<T>(BinaryReader reader)
    where T : WarcHeadersBase, new()
    {

        var firstLine = ReadLine(reader);
        var result = new T() { Version = firstLine };

        while (ReadLine(reader) is { } line)
        {
            if (string.IsNullOrWhiteSpace((line))) break;
            var index = line.IndexOf(':');
            if (index <= 0) continue;
            var key = line[..index].Trim().ToLower();
            var value = line[(index + 1)..].Trim();
            result[key] = value;
        }

        return result;
    }

}