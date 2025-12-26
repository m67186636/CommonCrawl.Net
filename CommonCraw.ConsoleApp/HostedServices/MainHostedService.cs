using CommonCrawl.Handlers;
using CommonCrawl.Models;
using CommonCrawl.Options;
using CommonCrawl.Readers;
using CommonCrawl.Utils;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Reflection.Emit;

namespace CommonCrawl.HostedServices;

internal partial class MainHostedService(ILogger<MainHostedService> logger, IHostApplicationLifetime applicationLifetime) : BackgroundService
{
    protected ILogger<MainHostedService> Logger { get; } = logger;
    protected IHostApplicationLifetime ApplicationLifetime { get; } = applicationLifetime;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        LogMainHostedServiceIsStarting(Logger);
        var collInfo = await DataSetHandler.Instance.GetLatestAsync(stoppingToken);
        LogLatestCollectionVersionVersion(Logger, collInfo.Id);

        var pathUrl = CommonCrawlUrlSplicer.FromCollectionInfo(collInfo.Id);
        LogStartGetIndexVersion(Logger, collInfo.Id);


        var file = @"D:\Administrator\Documents\Downloads\cc-main-2025-oct-nov-dec-domain-ranks.txt.gz";
        var s=GzLineReader.Instance.ReadAsAsyncEnumerable(file);

        var index = 0;
        await foreach (var ss in s)
        {
            Console.WriteLine(ss);
            index++;
            if (index >= 100)
                break;
        }
        ApplicationLifetime.StopApplication();
        return;

        await RunIndexAsync(collInfo, pathUrl, stoppingToken);

        await RunWarcAsync(collInfo, pathUrl, stoppingToken);
        await RunWatAsync(collInfo, pathUrl, stoppingToken);
        await RunWatAsync(collInfo, pathUrl, stoppingToken);
        ApplicationLifetime.StopApplication();

    }

    private async Task RunWarcAsync(CollInfo collInfo,PathUrl pathUrl, CancellationToken stoppingToken)
    {
        var lines = GzLineReader.Instance.ReadAsAsyncEnumerable(pathUrl.WARC);
        var fileUrls = new List<string>();
        await foreach (var line in lines.WithCancellation(stoppingToken))
            fileUrls.Add(CommonCrawlUrlSplicer.Build(line));
        LogCompletedGetIndexVersion(Logger, fileUrls.Count, collInfo.Id);
        var index = 0;
        foreach (var fileUrl in fileUrls)
        {
            var infos = GzWarcReader.Instance.ReadAsAsyncEnumerable(fileUrl);
            await foreach (var info in infos.WithCancellation(stoppingToken))
            {
                index++;
                Logger.LogInformation("{g} Processing {Index}\t{type}\t{url} - ","WARC", index, info.Type,info.TargetUri);
            }

        }
    }
    private async Task RunWatAsync(CollInfo collInfo, PathUrl pathUrl, CancellationToken stoppingToken)
    {
        var lines = GzLineReader.Instance.ReadAsAsyncEnumerable(pathUrl.WAT);
        var fileUrls = new List<string>();
        await foreach (var line in lines.WithCancellation(stoppingToken))
            fileUrls.Add(CommonCrawlUrlSplicer.Build(line));
        LogCompletedGetIndexVersion(Logger, fileUrls.Count, collInfo.Id);
        var index = 0;
        foreach (var fileUrl in fileUrls)
        {
            var infos = GzWarcReader.Instance.ReadAsAsyncEnumerable(fileUrl);
            await foreach (var info in infos.WithCancellation(stoppingToken))
            {
                index++;
                Logger.LogInformation("{g} Processing {Index}\t{type}\t{url} - ", "WAT", index, info.Type, info.TargetUri);
            }

        }
    }
    private async Task RunWetAsync(CollInfo collInfo, PathUrl pathUrl, CancellationToken stoppingToken)
    {
        var lines = GzLineReader.Instance.ReadAsAsyncEnumerable(pathUrl.WET);
        var fileUrls = new List<string>();
        await foreach (var line in lines.WithCancellation(stoppingToken))
            fileUrls.Add(CommonCrawlUrlSplicer.Build(line));
        LogCompletedGetIndexVersion(Logger, fileUrls.Count, collInfo.Id);
        var index = 0;
        foreach (var fileUrl in fileUrls)
        {
            var infos = GzWarcReader.Instance.ReadAsAsyncEnumerable(fileUrl);
            await foreach (var info in infos.WithCancellation(stoppingToken))
            {
                index++;
                Logger.LogInformation("{g} Processing {Index}\t{type}\t{url} - ", "WET", index, info.Type, info.TargetUri);
            }

        }
    }

    private async Task RunIndexAsync(CollInfo collInfo, PathUrl pathUrl, CancellationToken stoppingToken)
    {
        var lines = GzLineReader.Instance.ReadAsAsyncEnumerable(pathUrl.ColumnarUrlIndex);
        var fileUrls = new List<string>();
        await foreach (var line in lines.WithCancellation(stoppingToken))
            fileUrls.Add(CommonCrawlUrlSplicer.Build(line));
        LogCompletedGetIndexVersion(Logger, fileUrls.Count, collInfo.Id);
        var index = 0;
        foreach (var fileUrl in fileUrls)
        {
            index++;
            var filename= Path.GetFileName(fileUrl);
            var filePath = Path.Combine(CommonCrawlOptions.LocalPath, collInfo.Id, "ColumnarUrlIndex", filename);
            var hashSet = new HashSet<string>();
            Logger.LogInformation("Processing file {filename}", filename);
            var stopwatch= Stopwatch.StartNew();
            var lastCount = 0L;
            IProgress<(long, long)> progress = new Progress<(long, long)>(value =>
            {
                if (stopwatch.ElapsedMilliseconds <= 1000) return;
                stopwatch.Restart();
                var rate = value.Item1 * 1.0 / value.Item2;
                var byteTotal = value.Item1 - lastCount-0.0;
                lastCount = value.Item1;
                var speedUnit = byteTotal;
                var units = "B,KB,MB,GB,TB".Split(',');
                var unitIndex = 0;
                while (byteTotal>1000&&unitIndex<units.Length-1)
                {
                    byteTotal /= 1024;
                    unitIndex++;
                }
                Logger.LogInformation("Download file {filename}:{count:##,###}/{total:##,###}\t({rate:0.00%}),speed:{byteTotal:##,#0.00}{unit}", filename, value.Item1, value.Item2, rate, byteTotal, units[unitIndex]);
            });
            await DownloadHandler.Instance.DownloadAsync(fileUrl, filePath, progress, stoppingToken);

            stopwatch.Restart();

            var infos = ParquetReader.Instance.ReadAsAsyncEnumerable<IndexTableRecordLite>(filePath, x => x.FetchStatus is >= 200 and < 300);
            await foreach (var info in infos.WithCancellation(stoppingToken))
            {
                if(!string.IsNullOrWhiteSpace(info.UrlHostRegisteredDomain))
                    if(hashSet.Add(info.UrlHostRegisteredDomain))
                        Logger.LogInformation("Found new domain {domain}", info.UrlHostRegisteredDomain);
                //Logger.LogInformation("Processing {Index}\t{status}\t{url} ", index, info.FetchStatus, info.Url);
            }

            var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
            Logger.LogInformation("Processed file {filename},{c} domains, {elapsedMilliseconds}ms", filename,hashSet.Count, elapsedMilliseconds);
        }
    }
}
