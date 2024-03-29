using Microsoft.Extensions.Logging;

namespace NuGetConsolidator.Core.Utilities;

public static class LogBase
{
    private static ILoggerFactory _loggerFactory;

    static LogBase()
    {
        _loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddConsole(console =>
            {
                console.LogToStandardErrorThreshold = LogLevel.Error;
            });
        });
    }

    public static void Log<T>(LogLevel logLevel, string message, Exception ex)
    {
        var logger = _loggerFactory.CreateLogger<T>();

        logger.Log(logLevel, message, ex);
    }
}
