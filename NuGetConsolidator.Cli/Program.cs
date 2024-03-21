using NuGetConsolidator.Core;

namespace NuGetConsolidator.Cli;

public static class Program
{
    public static async Task Main(string[] args)
    {
        await ProjectAnalyzer.ScanDepsJson("C:\\Users\\phhaw\\git\\NuGetConsolidator\\NuGetConsolidator.Example");
    }
}
