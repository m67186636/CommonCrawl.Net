using Parquet.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace CommonCrawl.Models;

public class IndexTableRecordLite
{


    /// <summary>
    /// URL string.
    /// <example>https://www.example.com/path/index.html (fromCDX: url)</example>
    /// </summary>
    [JsonPropertyName("url")]
    [ParquetRequired]
    public string Url { get; set; } = string.Empty;

    /// <summary>
    /// Domain name of the host (one level below the registry suffix).
    /// <example>example.com, bbc.co.uk</example>
    /// </summary>
    [JsonPropertyName("url_host_registered_domain")]
    public string? UrlHostRegisteredDomain { get; set; }

    /// <summary>
    /// File path of the URL.
    /// <example>/path/index.html</example>
    /// </summary>
    [JsonPropertyName("url_path")]
    [ParquetRequired]
    public string UrlPath { get; set; } = string.Empty;



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

}
