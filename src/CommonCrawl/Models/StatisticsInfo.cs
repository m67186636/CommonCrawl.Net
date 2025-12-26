namespace CommonCrawl.Models
{
    public class StatisticsInfo
    {
        public Dictionary<int, long> HttpStatusCodes { get; } = new();
        public Dictionary<string, long> PageCharsets { get; } = new();
        public Dictionary<string, long> UrlCharsets { get; } = new();
        public Dictionary<string, long> PageLanguages { get; } = new();
        public Dictionary<string, long> UrlLanguages { get; } = new();
        public long SurtDomainTotal { set; get; }
        public long DomainTotal { set; get; }
        public long HostTotal { set; get; }
        public long FetchTotal { set; get; }
        public long PageTotal { set; get; }
        public long UrlTotal { set; get; }
        public void AddHttpStatusCode(int statusCode, long result)
        {
            HttpStatusCodes[statusCode] = result;
        }

        public void AddPageCharset(string charset, long count)
        {
            PageCharsets[charset] = count;
        }
        public void AddUrlCharset(string charset, long count)
        {
            UrlCharsets[charset] = count;
        }

        public void AddPageLanguage(string language, long count)
        {
            PageLanguages[language] = count;
        }
        public void AddUrlLanguage(string language, long count)
        {
            UrlLanguages[language] = count;
        }
    }
}
