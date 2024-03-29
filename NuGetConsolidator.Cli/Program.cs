using NuGetConsolidator.Core.Modification;
using NuGetConsolidator.Core.Targeting;
using System.CommandLine;

namespace NuGetConsolidator.Cli;

public static class Program
{
    private static readonly string _currentPath = Directory.GetCurrentDirectory();
    public static async Task<int> Main(string[] args)
    {
        //var file = new FileInfo("C:\\Users\\phhaw\\git\\NuGetConsolidator\\NuGetConsolidator.Example");
        var verboseOption = new Option<bool>(
            aliases: new[] { "--verbose", "-v" },
            getDefaultValue: () => false,
            description: "Add more detail to the command output.");

        var pathOption = new Option<string>(
            aliases: new[] { "--path", "-p" },
            getDefaultValue: () => Path.GetRelativePath(_currentPath, _currentPath),
            description: "The project file or directory to look for .NET projects.");

        var interactiveOption = new Option<bool>(
            aliases: new[] { "--interactive", "-i" },
            getDefaultValue: () => false,
            description: "Prompt for user confirmation before removing packages.");

        var rootCommand = new RootCommand("Consolidate redundant package references in .NET project files and solutions")
        {
            verboseOption,
            pathOption,
            interactiveOption,
        };

        rootCommand.SetHandler(async (verbose, path, interactive) =>
        {
            await ConsolidatePackages(verbose, path, interactive);
        }, verboseOption, pathOption, interactiveOption);

        return await rootCommand.InvokeAsync(args);
    }

    private static async Task ConsolidatePackages(bool verbose, string path, bool interactive)
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
