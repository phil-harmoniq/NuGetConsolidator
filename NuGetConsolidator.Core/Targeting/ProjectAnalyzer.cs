using Microsoft.Extensions.Logging;
using NuGetConsolidator.Core.Models;
using NuGetConsolidator.Core.Utilities;

namespace NuGetConsolidator.Core.Targeting;

public class ProjectAnalyzer
{
    private static readonly ILogger _logger = LogBase.Create<ProjectAnalyzer>();

    public static async Task<IList<Project>> GetRedundantPackages(string projectPath)
    {
        using var dependencyGraphGenerator = new DependencyGraphGenerator();
        var dependencyGraph = await dependencyGraphGenerator.GetDependencyGraph(projectPath);
        var projects = new List<Project>();

        foreach (var project in dependencyGraph.Projects)
        {
            var returnedProject = new Project
            {
                Name = project.Name,
            };

            var lockFileGenerator = new LockFileGenerator();
            var lockFile = await lockFileGenerator.GetLockFile(projectPath, project.RestoreMetadata.OutputPath);

            foreach (var projectFileDependencyGroup in lockFile.ProjectFileDependencyGroups)
            {
                var projectMeta = new PackageReferenceAnalyzer(projectFileDependencyGroup, lockFile);
                var redundantTopLevelPackages = projectMeta.GetRedundantPackages();

                var returnedFramework = new TargetFramework
                {
                    Name = projectFileDependencyGroup.FrameworkName,
                    RedundantLibraries = redundantTopLevelPackages.ToList()
                };

                returnedProject.TargetFrameworks.Add(returnedFramework);
            }

            projects.Add(returnedProject);
        }

        return projects;
    }

    private static string GetPackageName(string message)
    {
        var stringSplit = message.Split(' ');
        var name = stringSplit[0];
        return name;
    }
}
