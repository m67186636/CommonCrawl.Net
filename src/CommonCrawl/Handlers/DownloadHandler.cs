using System.Net.Http.Headers;

namespace CommonCrawl.Handlers;

public interface IDownloadHandler
{
    Task DownloadAsync(string url, string finalPath, IProgress<(long, long)>? progress = null,
        CancellationToken cancellationToken = default);
}

public class DownloadHandler : IDownloadHandler
{

    private const string TempExtension = ".downloading";
    private const int BufferSize = 8192 * 8; // 64 KB
    public static IDownloadHandler Instance { get; } = new DownloadHandler();

    private DownloadHandler()
    {
    }

    public async Task DownloadAsync(string url, string finalPath, IProgress<(long, long)>? progress = null, CancellationToken cancellationToken = default)
    {
        var tempPath = finalPath + TempExtension;
        // 如果最终文件已存在，直接返回（根据业务可改为覆盖或跳过）
        if (File.Exists(finalPath))
        {
            var fileSize = new FileInfo(finalPath).Length;
            progress?.Report((fileSize, fileSize));
            return;
        }

        var tempFileInfo = new FileInfo(tempPath);
        // 2. 获取当前下载进度（断点续传逻辑）
        var existingLength = tempFileInfo.Exists ? tempFileInfo.Length : 0;


        var httpClient = HttpClientCreator.Create();
        // 3. 获取远程文件信息
        var totalLength =  -1L;

        // 4. 发起带 Range 头的请求
        using var request = new HttpRequestMessage(HttpMethod.Get, url);
        if (existingLength > 0)
            request.Headers.Range = new RangeHeaderValue(existingLength, null);

        using var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

        // 处理服务器不支持断点续传的情况（返回 200 而非 206）
        var isResuming = response.StatusCode == System.Net.HttpStatusCode.PartialContent;
        if (!isResuming) existingLength = 0;

        if (response.Content.Headers.ContentRange is { Length: not null })
        {
            totalLength = response.Content.Headers.ContentRange.Length.Value;
        }
        else if (response.Content.Headers.ContentLength.HasValue)
        {
            totalLength = response.Content.Headers.ContentLength.Value;
        }

        if (!tempFileInfo.Directory!.Exists)
            tempFileInfo.Directory.Create();
        // 5. 分块写入临时文件
        await using (var contentStream = await response.Content.ReadAsStreamAsync(cancellationToken))
        await using (var fileStream = new FileStream(tempPath, isResuming ? FileMode.Append : FileMode.Create, FileAccess.Write, FileShare.None, BufferSize, true))
        {
            var buffer = new byte[BufferSize];
            int bytesRead;
            var currentTotal = existingLength;

            while ((bytesRead = await contentStream.ReadAsync(buffer, cancellationToken)) > 0)
            {
                await fileStream.WriteAsync(buffer.AsMemory(0, bytesRead), cancellationToken);
                currentTotal += bytesRead;
                progress?.Report((currentTotal, totalLength));
            }
            // 确保数据刷新到磁盘
            await fileStream.FlushAsync(cancellationToken);
        }

        // 6. 下载完成后，原子化重命名
        if (File.Exists(finalPath)) File.Delete(finalPath); // 确保目标路径清理
        File.Move(tempPath, finalPath);
    }
}