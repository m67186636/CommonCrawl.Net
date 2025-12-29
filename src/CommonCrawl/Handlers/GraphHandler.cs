
using CommonCrawl.Models;
using CommonCrawl.Options;
using System.Text.Json;

namespace CommonCrawl.Handlers
{
    public interface IGraphHander
    {
        Task<IReadOnlyCollection<GraphInfo>> GetAllVersionAsync(CancellationToken cancellationToken = default);
        Task<GraphInfo> GetLatestAsync(CancellationToken cancellationToken = default);
        Task<string> GetVersionByCollectionAsync(string collectionId);
    }

    public class GraphHandler : IGraphHander
    {
        private const string Url = "https://index.commoncrawl.org/graphinfo.json";
        public static IGraphHander Instance { get; } = new GraphHandler();

        protected Lazy<Task<IReadOnlyCollection<GraphInfo>>> LazyAllVersionTask { get; }
        public Lazy<Task<GraphInfo>> LazyLatestTask { get; }
        private GraphHandler()
        {
            LazyAllVersionTask = new Lazy<Task<IReadOnlyCollection<GraphInfo>>>(() => InitializeAllVersionAsync(CancellationToken.None));
            LazyLatestTask = new Lazy<Task<GraphInfo>>(() => InitializeLatestAsync(CancellationToken.None));
        }



        public Task<GraphInfo> GetLatestAsync(
            CancellationToken cancellationToken = default)
            => LazyLatestTask.Value.WaitAsync(cancellationToken);

        public async Task<string> GetVersionByCollectionAsync(string collectionId)
        {
            var all = await LazyAllVersionTask.Value;
            var graphInfo = all.FirstOrDefault(g => g.Crawls.EndsWith(collectionId));
            return graphInfo?.Id ?? string.Empty;
        }

        public Task<IReadOnlyCollection<GraphInfo>> GetAllVersionAsync(
            CancellationToken cancellationToken = default)
            => LazyAllVersionTask.Value.WaitAsync(cancellationToken);

        private async Task<GraphInfo> InitializeLatestAsync(CancellationToken cancellationToken = default)
        {
            var all = await LazyAllVersionTask.Value;
            return all.First();
        }
        private static async Task<IReadOnlyCollection<GraphInfo>> InitializeAllVersionAsync(CancellationToken cancellationToken = default)
        {
            var filename = Path.Combine(CommonCrawlOptions.LocalPath, "graphs.json");
            var fileInfo = new FileInfo(filename);
            if (!fileInfo.Exists || DateTime.UtcNow - fileInfo.LastWriteTimeUtc > CommonCrawlOptions.CollectionExpired)
            {
                await DownloadHandler.Instance.DownloadAsync(Url, filename, cancellationToken: cancellationToken);
            }
            await using var fileStream = File.OpenRead(filename);
            return JsonSerializer.Deserialize<List<GraphInfo>>(fileStream, CommonCrawlOptions.JsonSerializerOptions)!.AsReadOnly();
        }
    }

}
