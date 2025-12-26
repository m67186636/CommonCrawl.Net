using CommonCrawl.HostedServices;
using CommonCrawl.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

CommonCrawlOptions.LocalPath = @"A:\temp\CommonCrawl";
Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();
var builder = Host.CreateApplicationBuilder();
builder.Services.AddSerilog();
builder.Services.AddHostedService<MainHostedService>();
var host = builder.Build();
await host.RunAsync();