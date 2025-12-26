using Parquet.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace CommonCrawl.Models;

/// <summary>
/// <seealso href="https://github.com/commoncrawl/cc-index-table/blob/main/src/main/resources/schema/cc-index-schema-flat.json"/>
/// </summary>
public class IndexTableRecord
{
    /// <summary>
    /// SURT URL key. 
    /// <example>com,example)/path/index.html</example>
    /// </summary>
    [JsonPropertyName("url_surtkey")]
    [ParquetRequired]

    public string UrlSurtKey { get; set; } = string.Empty;

    /// <summary>
    /// URL string.
    /// <example>https://www.example.com/path/index.html (fromCDX: url)</example>
    /// </summary>
    [JsonPropertyName("url")]
    [ParquetRequired]
    public string Url { get; set; } = string.Empty;

    /// <summary>
    /// Hostname, including IP addresses.
    /// <example>www.example.com</example>
    /// </summary>
    [JsonPropertyName("url_host_name")]
    [ParquetRequired]
    public string UrlHostName { get; set; } = string.Empty;

    /// <summary>
    /// Top-level domain or last part of the hostname.
    /// <example>com for the hostname www.example.com</example>
    /// </summary>
    [JsonPropertyName("url_host_tld")]
    public string? UrlHostTld { get; set; }

    /// <summary>
    /// Second last part of the hostname.
    /// <example>example for www.example.com, co for bbc.co.uk</example>
    /// </summary>
    [JsonPropertyName("url_host_2nd_last_part")]
    public string? UrlHost2ndLastPart { get; set; }

    /// <summary>
    /// Third last part of the hostname.
    /// <example>www for www.example.com</example>
    /// </summary>
    [JsonPropertyName("url_host_3rd_last_part")]
    public string? UrlHost3rdLastPart { get; set; }

    /// <summary>
    /// 4th last part of the hostname.
    /// <example>host1 for host1.subdomain.example.com</example>
    /// </summary>
    [JsonPropertyName("url_host_4th_last_part")]
    public string? UrlHost4thLastPart { get; set; }

    /// <summary>
    /// 5th last part of the hostname.
    /// <example>host1 for host1.sub2.subdomain.example.com</example>
    /// </summary>
    [JsonPropertyName("url_host_5th_last_part")]
    public string? UrlHost5thLastPart { get; set; }

    /// <summary>
    /// Domain registry suffix.
    /// <example>com, co.uk</example>
    /// </summary>
    [JsonPropertyName("url_host_registry_suffix")]
    public string? UrlHostRegistrySuffix { get; set; }

    /// <summary>
    /// Domain name of the host (one level below the registry suffix).
    /// <example>example.com, bbc.co.uk</example>
    /// </summary>
    [JsonPropertyName("url_host_registered_domain")]
    public string? UrlHostRegisteredDomain { get; set; }

    /// <summary>
    /// Suffix of domain registries including private registrars, see
    /// <see href="https://publicsuffix.org/"/>
    /// <example>com, co.uk,s3.amazonaws.com, blogspot.com</example>
    /// </summary>
    [JsonPropertyName("url_host_private_suffix")]
    public string? UrlHostPrivateSuffix { get; set; }

    /// <summary>
    /// Domain name of the host (one level below the private suffix).
    /// <example>mypublicbucket.s3.amazonaws.com, myblog.blogspot.com</example>
    /// </summary>
    [JsonPropertyName("url_host_private_domain")]
    public string? UrlHostPrivateDomain { get; set; }

    /// <summary>
    /// Hostname, excluding IP addresses, in reverse domain name notation.
    /// <example>com.example.www</example>
    /// </summary>
    [JsonPropertyName("url_host_name_reversed")]
    public string? UrlHostNameReversed { get; set; }

    /// <summary>
    /// Protocol of the URL.
    /// <example>https</example>
    /// </summary>
    [JsonPropertyName("url_protocol")]
    [ParquetRequired]
    public string UrlProtocol { get; set; } = string.Empty;

    /// <summary>
    /// Port of the URL (null if not explicitly specified in the URL).
    /// <example>8443</example>
    /// </summary>
    [JsonPropertyName("url_port")]
    public int? UrlPort { get; set; }

    /// <summary>
    /// File path of the URL.
    /// <example>/path/index.html</example>
    /// </summary>
    [JsonPropertyName("url_path")]
    [ParquetRequired]
    public string UrlPath { get; set; } = string.Empty;

    /// <summary>
    /// Query part of the URL.
    /// <example>q=abc&lang=en for .../search?q=abc&lang=en</example>
    /// </summary>
    [JsonPropertyName("url_query")]
    public string? UrlQuery { get; set; }

    /// <summary>
    /// Fetch time (capture time stamp).
    /// <example>2017-10-24T00:14:32Z</example>
    /// </summary>
    [JsonPropertyName("fetch_time")]
    [ParquetRequired]
    public DateTime FetchTime { get; set; } = DateTime.MinValue;

    /// <summary>
    /// HTTP response status code.
    /// <example>200 (fromCDX: status)</example>
    /// </summary>
    [JsonPropertyName("fetch_status")]
    [ParquetRequired]
    public short FetchStatus { get; set; } = short.MinValue;

    /// <summary>
    /// Target location of HTTP redirect.
    /// <example>https://example.com/ (since: CC-MAIN-2019-47, fromCDX: redirect)</example>
    /// </summary>
    [JsonPropertyName("fetch_redirect")]
    public string? FetchRedirect { get; set; }

    /// <summary>
    /// SHA-1 content digest (WARC-Payload-Digest).
    /// <example>CH7IV3XAD3M7A42JARKRLJ3T5PGGCGXD (fromCDX: digest)</example>
    /// </summary>
    [JsonPropertyName("content_digest")]
    public string? ContentDigest { get; set; }

    /// <summary>
    /// Content-Type sent in HTTP response header.
    /// <example>text/xml (fromCDX: mime)</example>
    /// </summary>
    [JsonPropertyName("content_mime_type")]
    public string? ContentMimeType { get; set; }

    /// <summary>
    /// Content-Type detected based on content (WARC-Identified-Payload-Type).
    /// <example>application/rss+xml (fromCDX: mime-detected)</example>
    /// </summary>
    [JsonPropertyName("content_mime_detected")]
    public string? ContentMimeDetected { get; set; }

    /// <summary>
    ///  Character set of an HTML page and other text-based document formats.
    /// <example>UTF-8 (since: CC-MAIN-2018-39, fromCDX: charset)</example>
    /// </summary>

    [JsonPropertyName("content_charset")]
    public string? ContentCharset { get; set; }

    /// <summary>
    /// Language(s) of a document as ISO-639-3 language code(s), multiple values are separated by a comma.
    /// <example>fra,eng (since: CC-MAIN-2018-39, fromCDX: languages)</example>
    /// </summary>
    [JsonPropertyName("content_languages")]
    public string? ContentLanguages { get; set; }

    /// <summary>
    /// Non-null if the WARC record payload is truncated. The value then indicates the reason for the truncation, cf. https://iipc.github.io/warc-specifications/specifications/warc-format/warc-1.1/#warc-truncated
    /// </summary>
    [JsonPropertyName("content_truncated")]
    public string? ContentTruncated { get; set; }

    /// <summary>
    /// WARC filename/path below s3://commoncrawl/.
    /// <example>crawl-data/CC-MAIN-2017-39/segments/1505818689752.21/warc/CC-MAIN-20170923160736-20170923180736-00256.warc.gz (fromCDX: filename)</example>
    /// </summary>
    [JsonPropertyName("warc_filename")]
    [ParquetRequired]
    public string WarcFilename { get; set; } = string.Empty;

    /// <summary>
    /// Offset of the WARC record.
    /// <example>397346194 (fromCDX: offset)</example>
    /// </summary>
    [JsonPropertyName("warc_record_offset")]
    [ParquetRequired]
    public int WarcRecordOffset { get; set; } = int.MinValue;

    /// <summary>
    /// Length of the WARC record.
    /// <example>24662 (fromCDX: length)</example>
    /// </summary>
    [JsonPropertyName("warc_record_length")]
    [ParquetRequired]
    public int WarcRecordLength { get; set; } = int.MinValue;

    /// <summary>
    /// Segment the WARC file belongs to.
    /// <example>1505818689752.21</example>
    /// </summary>
    [JsonPropertyName("warc_segment")]
    [ParquetRequired]
    public string WarcSegment { get; set; } = string.Empty;

    /// <summary>
    /// Crawl the capture/record is part of.
    /// <example>CC-MAIN-2017-39</example>
    /// </summary>
    [JsonPropertyName("crawl")]
    [ParquetRequired]
    public string Crawl { get; set; } = string.Empty;

    /// <summary>
    /// Subset of responses (organized as subdirectory in segments).
    /// <example>warc: successful captures, crawldiagnostics: redirects, 404s, and other non-successful captures, robotstxt: robots.txt responses</example>
    /// </summary>
    [JsonPropertyName("subset")]
    [ParquetRequired]
    public string Subset { get; set; } = string.Empty;
}