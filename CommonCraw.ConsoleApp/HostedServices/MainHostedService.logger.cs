using Microsoft.Extensions.Logging;

namespace CommonCrawl.HostedServices;

internal partial class MainHostedService
{
    [LoggerMessage(LogLevel.Information, "Start Get Index: {version}")]
    static partial void LogStartGetIndexVersion(ILogger<MainHostedService> logger, string version);

    [LoggerMessage(LogLevel.Information, "MainHostedService is starting.")]
    static partial void LogMainHostedServiceIsStarting(ILogger<MainHostedService> logger);

    [LoggerMessage(LogLevel.Information, "Latest Collection Version: {version}")]
    static partial void LogLatestCollectionVersionVersion(ILogger<MainHostedService> logger, string version);

    [LoggerMessage(LogLevel.Information, "Get Index: {version} Completed,{count} files")]
    static partial void LogCompletedGetIndexVersion(ILogger<MainHostedService> logger, int count, string version);
}