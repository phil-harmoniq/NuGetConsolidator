using NuGetConsolidator.Core.Modification;

namespace NuGetConsolidator.Cli;

public static class Program
{
    public static async Task Main(string[] args)
    {
        await ProjectAnalyzer.GetRedundantPackages("C:\\Users\\phhaw\\git\\NuGetConsolidator\\NuGetConsolidator.Example");
    }
}
