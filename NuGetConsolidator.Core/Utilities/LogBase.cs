using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace NuGetConsolidator.Core.Utilities;

public static class LogBase
{
    private static ILoggerFactory _loggerFactory;

    static LogBase()
    {
        _loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddSimpleConsole(console =>
            {
                console.IncludeScopes = false;
                console.ColorBehavior = LoggerColorBehavior.Enabled;
            });
        });
    }

    public static void Init(LogLevel minimumLevel)
    {
        _loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.SetMinimumLevel(minimumLevel);
        });
    }

    public static ILogger Create<T>() => _loggerFactory.CreateLogger<T>();
}
