using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CommonCrawl.Models;
public class GraphInfo
{
    public required string Id { get; set; }
    public required string[] Crawls { get; set; }
    public required string Index { get; set; }
    public required string Location { get; set; }
}
