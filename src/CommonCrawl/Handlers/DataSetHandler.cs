using CommonCrawl.Models;
using CommonCrawl.Options;
using System.Text.Json;

namespace CommonCrawl.Handlers
{
    public interface IDataSetHandler
    {
        Task<IReadOnlyCollection<CollInfo>> GetAllVersionAsync(CancellationToken cancellationToken = default);
        Task<CollInfo> GetLatestAsync(CancellationToken cancellationToken = default);
    }

    public class DataSetHandler : IDataSetHandler
    {
        private const string Url = "https://index.commoncrawl.org/collinfo.json";
        public static IDataSetHandler Instance { get; } = new DataSetHandler();

        protected Lazy<Task<IReadOnlyCollection<CollInfo>>> LazyAllVersionTask { get; }
        public Lazy<Task<CollInfo>> LazyLatestTask { get; }
        private DataSetHandler()
        {
            LazyAllVersionTask = new Lazy<Task<IReadOnlyCollection<CollInfo>>>(() => InitializeAllVersionAsync(CancellationToken.None));
            LazyLatestTask = new Lazy<Task<CollInfo>>(() => InitializeLatestAsync(CancellationToken.None));
        }



        public Task<CollInfo> GetLatestAsync(
            CancellationToken cancellationToken = default)
            => LazyLatestTask.Value.WaitAsync(cancellationToken);
        public Task<IReadOnlyCollection<CollInfo>> GetAllVersionAsync(
            CancellationToken cancellationToken = default)
            => LazyAllVersionTask.Value.WaitAsync(cancellationToken);

        private async Task<CollInfo> InitializeLatestAsync(CancellationToken cancellationToken = default)
        {
            var all = await LazyAllVersionTask.Value;
            return all.First();
        }
        private static async Task<IReadOnlyCollection<CollInfo>> InitializeAllVersionAsync(CancellationToken cancellationToken = default)
        {
            var filename = Path.Combine(CommonCrawlOptions.LocalPath, "collections.json");
            var fileInfo = new FileInfo(filename);
            if (!fileInfo.Exists || DateTime.UtcNow - fileInfo.LastWriteTimeUtc > CommonCrawlOptions.CollectionExpired)
            {
                await DownloadHandler.Instance.DownloadAsync(Url, filename, cancellationToken: cancellationToken);
            }
            await using var fileStream = File.OpenRead(filename);
            return JsonSerializer.Deserialize<List<CollInfo>>(fileStream, CommonCrawlOptions.JsonSerializerOptions)!.AsReadOnly();
        }
    }

}
