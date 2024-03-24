using NuGet.Packaging;
using NuGet.ProjectModel;

namespace NuGetConsolidator.Core;

public static partial class ProjectAnalyzer
{
    public static async Task<IList<Project>> GetRedundantPackages(string projectPath)
    {
        var dependencyGraph = Utilities.GenerateDependencyGraph(projectPath);
        var projects = new List<Project>();

        foreach (var project in dependencyGraph.Projects)
        {
            var returnedProject = new Project
            {
                Name = project.Name,
            };

            var lockFileProvider = new LockFileGenerator();
            var lockFile = lockFileProvider.GetLockFile(projectPath, project.RestoreMetadata.OutputPath);
            var redundantLibraries = new List<LockFileTargetLibrary>();

            Console.WriteLine(project.Name);
            Console.WriteLine(project.Language);
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

    //public static async Task ScanDepsJson(string projectPath)
    //{
    //    var dependencyGraph = Utilities.GenerateDependencyGraph(projectPath);

    //    foreach (var project in dependencyGraph.Projects)
    //    {
    //        var lockFileProvider = new LockFileGenerator();
    //        var lockFile = lockFileProvider.GetLockFile(projectPath, project.RestoreMetadata.OutputPath);
    //        var redundantLibraries = new List<LockFileTargetLibrary>();

    //        Console.WriteLine(project.Name);
    //        Console.WriteLine(project.Language);
    //        Console.WriteLine(project.Version);

    //        var redundantTopLevelPackagesForAllTargets = new List<LockFileTargetLibrary>();

    //        foreach (var projectFileDependencyGroup in lockFile.ProjectFileDependencyGroups)
    //        {
    //            var topLevelPackageNames = projectFileDependencyGroup.Dependencies.Select(GetPackageName);
    //            var target = lockFile.Targets.FirstOrDefault(x => x.Name == projectFileDependencyGroup.FrameworkName);
    //            var topLevelPackages = target.Libraries.Where(library => topLevelPackageNames.Contains(library.Name));
    //            var projectMeta = new PackageReferenceAnalyzer(target.Name, topLevelPackages, target.Libraries);
    //            var redundantTopLevelPackages = projectMeta.GetRedundantPackages();

    //            //var target = lockFile.Targets.FirstOrDefault(x => topLevelPackageInfo.Any(pi => pi.Name == x.Name));
    //        }
    //    }
    //}

    private static string GetPackageName(string message)
    {
        var stringSplit = message.Split(' ');
        var name = stringSplit[0];
        return name;
    }
}
