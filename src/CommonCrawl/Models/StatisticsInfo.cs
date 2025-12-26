namespace CommonCrawl.Models
{
    public class StatisticsInfo
    {
        public Dictionary<int, int> HttpStatusCodes { get; } = new();
        public Dictionary<string, int> PageCharsets { get; } = new();
        public Dictionary<string, int> UrlCharsets { get; } = new();
        public Dictionary<string, int> PageLanguages { get; } = new();
        public Dictionary<string, int> UrlLanguages { get; } = new();
        public long SurtDomainTotal { set; get; }
        public long DomainTotal { set; get; }
        public long HostTotal { set; get; }
        public long FetchTotal { set; get; }
        public long PageTotal { set; get; }
        public long UrlTotal { set; get; }
        public void AddHttpStatusCode(int statusCode, int result)
        {
            HttpStatusCodes[statusCode] = result;
        }

        public void AddPageCharset(string charset, int count)
        {
            PageCharsets[charset] = count;
        }
        public void AddUrlCharset(string charset, int count)
        {
            UrlCharsets[charset] = count;
        }

        public void AddPageLanguage(string language, int count)
        {
            PageLanguages[language] = count;
        }
        public void AddUrlLanguage(string language, int count)
        {
            UrlLanguages[language] = count;
        }
    }
}
