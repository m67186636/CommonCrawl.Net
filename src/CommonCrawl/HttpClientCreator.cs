namespace CommonCrawl
{
    public class HttpClientCreator
    {
        protected static HttpClient GlobalClient { get; }
        static HttpClientCreator()
        {
            GlobalClient = new HttpClient();
            GlobalClient.DefaultRequestHeaders.Add("User-Agent", "CommonCrawl.Net");
        }


        public static HttpClient Create()
        {
            return GlobalClient;
        }
    }
}
