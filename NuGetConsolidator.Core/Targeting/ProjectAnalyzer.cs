using NuGet.ProjectModel;
using NuGetConsolidator.Core.Models;

namespace NuGetConsolidator.Core.Targeting;

public static class ProjectAnalyzer
{
    public static async Task<IList<Project>> GetRedundantPackages(string projectPath)
    {

        using var dependencyGraphGenerator = new DependencyGraphGenerator();
        var dependencyGraph = dependencyGraphGenerator.GetDependencyGraph(projectPath);
        var projects = new List<Project>();

        foreach (var project in dependencyGraph.Projects)
        {
            var returnedProject = new Project
            {
                Name = project.Name,
            };

            var lockFileGenerator = new LockFileGenerator();
            var lockFile = lockFileGenerator.GetLockFile(projectPath, project.RestoreMetadata.OutputPath);
            var redundantLibraries = new List<LockFileTargetLibrary>();

            Console.WriteLine(project.Name);
            Console.WriteLine(project.Version);

            var redundantTopLevelPackagesForAllTargets = new List<LockFileTargetLibrary>();


            foreach (var projectFileDependencyGroup in lockFile.ProjectFileDependencyGroups)
            {

                var topLevelPackageNames = projectFileDependencyGroup.Dependencies.Select(GetPackageName);
                var target = lockFile.Targets.FirstOrDefault(x => x.Name == projectFileDependencyGroup.FrameworkName);
                var topLevelPackages = target.Libraries.Where(library => topLevelPackageNames.Contains(library.Name));
                var projectMeta = new PackageReferenceAnalyzer(target.Name, topLevelPackages, target.Libraries);
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
