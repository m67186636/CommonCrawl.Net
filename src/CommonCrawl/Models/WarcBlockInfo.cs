namespace CommonCrawl.Models;

public abstract record WarcBlockInfo(WarcHeaders Headers)
{
    private const string KeyWarcType = "warc-type";
    private const string KeyWarcTargetUri = "warc-target-uri";
    public static WarcBlockInfo Create(WarcHeaders headers)
    {
        ArgumentNullException.ThrowIfNull(headers);
        if (!headers.TryGetValue(KeyWarcType, out var type) || string.IsNullOrWhiteSpace(type))
            throw new ArgumentException("Missing or empty  warc-type", nameof(headers));
        return type switch
        {
            "request" => new WarcRequestRecord(headers),
            "response" => new WarcResponseRecord(headers),
            "warcinfo" => new WarcInfoRecord(headers),
            "metadata" => new WarcMetadataRecord(headers),
            "conversion" => new WarcConversionRecord(headers),
            _ => new NullRecord(headers)
        };
    }



    public string? Type => Headers.GetValueOrDefault(KeyWarcType);
    public string? TargetUri => Headers.GetValueOrDefault(KeyWarcTargetUri);
}

public sealed record NullRecord(WarcHeaders Headers) : WarcBlockInfo(Headers)
{
}
public sealed record WarcRequestRecord(WarcHeaders Headers) : WarcBlockInfo(Headers)
{
    public RequestHeaders? RequestHeaders { get; internal set; }
}
public sealed record WarcResponseRecord(WarcHeaders Headers) : WarcBlockInfo(Headers)
{
    public ResponseHeaders? ResponseHeaders { get; internal set; }
    public string? Payload { get; internal set; }
}

public sealed record WarcInfoRecord(WarcHeaders Headers) : WarcBlockInfo(Headers)
{
    public InfoHeaders? InfoHeaders { get; internal set; }
}
public sealed record WarcMetadataRecord(WarcHeaders Headers) : WarcBlockInfo(Headers)
{
    public MetadataHeaders? MetadataHeaders { get; internal set; }
    public string? Payload { get; set; }
}
public sealed record WarcConversionRecord(WarcHeaders Headers) : WarcBlockInfo(Headers)
{
    public ConversionHeaders? ConversionHeaders { get; internal set; }
    public string? Payload { get; set; }
}



public abstract class WarcHeadersBase : Dictionary<string, string?>
{
    public string? Version { set; get; }
}

public sealed class WarcHeaders : WarcHeadersBase
{
    public long? ContentLength => TryGetValue("content-length", out var value) && long.TryParse(value, out var length) ? length : null;
    public string? RecordId => TryGetValue("warc-record-id", out var value) ? value : null;
}

public sealed class RequestHeaders : WarcHeadersBase
{
}
public sealed class ResponseHeaders : WarcHeadersBase
{
    public long? ContentLength => TryGetValue("content-length", out var value) && long.TryParse(value, out var length) ? length : null;
}

public sealed class InfoHeaders : WarcHeadersBase
{
}
public sealed class MetadataHeaders : WarcHeadersBase
{
}
public sealed class ConversionHeaders : WarcHeadersBase
{
}










