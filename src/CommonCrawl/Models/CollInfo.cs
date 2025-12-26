using System.Text.Json.Serialization;

namespace CommonCrawl.Models
{
    public class CollInfo
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public required string Timegate { get; set; }
        [JsonPropertyName("cdx-api")]
        public required string CdxApi { get; set; }
        public required DateTime From { get; set; }
        public required DateTime To { get; set; }
    }
}
