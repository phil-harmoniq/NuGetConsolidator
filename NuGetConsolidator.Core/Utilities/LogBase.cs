using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Options;

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

    //public static void Init(LogLevel minimumLevel)
    //{
    //    _loggerFactory = LoggerFactory.Create(builder =>
    //    {
    //        builder.AddSimpleConsole(console =>
    //        {
    //            console.IncludeScopes = false;
    //            console.ColorBehavior = LoggerColorBehavior.Default;
    //            //console.SingleLine = true;
    //        });
    //    });
    //}

    public static ILogger Create<T>() => _loggerFactory.CreateLogger<T>();
}
