using NuGetConsolidator.Core.Modification;
using NuGetConsolidator.Core.Targeting;
using System.CommandLine;

namespace NuGetConsolidator.Cli;

public static class Program
{
    public static async Task<int> Main(string[] args)
    {
        //var file = new FileInfo("C:\\Users\\phhaw\\git\\NuGetConsolidator\\NuGetConsolidator.Example");
        var verboseOption = new Option<bool>(
            name: "--verbose",
            getDefaultValue: () => false,
            description: "Add more detail to the command output.");
        verboseOption.AddAlias("-v");

        var pathOption = new Option<string>(
            name: "--path",
            getDefaultValue: () => Directory.GetCurrentDirectory(),
            description: "The project file or directory to look for .NET projects.");
        verboseOption.AddAlias("-p");

        var rootCommand = new RootCommand("Consolidate redundant package references in .NET project files and solutions")
        {
            verboseOption,
            pathOption
        };

        rootCommand.SetHandler(async (verbose, path) =>
        {
            await ConsolidatePackages(verbose, path);
        }, verboseOption, pathOption);

        return await rootCommand.InvokeAsync(args);
    }

    private static async Task ConsolidatePackages(bool verbose, string path)
    {
        var projects = await ProjectAnalyzer.GetRedundantPackages(path);

        foreach (var project in projects)
        {
            foreach (var library in project.TargetFrameworks.First().RedundantLibraries)
            {
                var result = MsBuildHelper.RemovePackageReference(path, library.Name);
            }
        }
    }
}
