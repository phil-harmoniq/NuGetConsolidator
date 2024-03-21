using NuGetConsolidator.Core.Providers;

namespace NuGetConsolidator.Core;

public static class ProjectAnalyzer
{
    public static async Task ScanDepsJson(string projectPath)
    {
        var dependencyGraphProvider = new DependencyGraphProvider();
        var dependencyGraph = dependencyGraphProvider.GenerateDependencyGraph(projectPath);

        foreach (var project in dependencyGraph.Projects)
        {
            var lockFileProvider = new LockFileProvider();
            var lockFile = lockFileProvider.GetLockFile(projectPath, project.RestoreMetadata.OutputPath);

            Console.WriteLine(project.Name);
            Console.WriteLine(project.Language);
            Console.WriteLine(project.Version);

            foreach (var framework in project.TargetFrameworks)
            {
                Console.WriteLine($"- {framework}");
            }
        }
    }
}
