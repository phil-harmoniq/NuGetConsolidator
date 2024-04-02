using NuGet.ProjectModel;

namespace NuGetConsolidator.Core.Targeting;

public class PackageReferenceAnalyzer
{
    public string FrameworkName { get; }
    public IReadOnlyDictionary<string, LockFileTargetLibrary> TopLevelPackages { get; }
    public IReadOnlyDictionary<string, LockFileTargetLibrary> AllPackages { get; }

    public PackageReferenceAnalyzer(
        string frameworkName,
        IEnumerable<LockFileTargetLibrary> topLevelPackages,
        IEnumerable<LockFileTargetLibrary> allPackages)
    {
        FrameworkName = frameworkName;
        TopLevelPackages = topLevelPackages.ToDictionary(x => x.Name);
        AllPackages = allPackages.ToDictionary(x => x.Name);
    }

    public PackageReferenceAnalyzer(ProjectFileDependencyGroup projectFileDependencyGroup, LockFile lockFile)
    {
        var topLevelPackageNames = projectFileDependencyGroup.Dependencies.Select(GetPackageName);
        var target = lockFile.Targets.FirstOrDefault(x => x.Name == projectFileDependencyGroup.FrameworkName);
        FrameworkName = projectFileDependencyGroup.FrameworkName;
        TopLevelPackages = target.Libraries.Where(library => topLevelPackageNames.Contains(library.Name)).ToDictionary(x => x.Name);
        AllPackages = target.Libraries.ToDictionary(x => x.Name);
    }

    public IReadOnlyList<LockFileTargetLibrary> GetRedundantPackages()
    {
        var topLevelPackageList = TopLevelPackages.Values;
        var redundantTopLevelPackages = new List<LockFileTargetLibrary>();

        foreach (var library in topLevelPackageList)
        {
            var otherTopLevelPackages = topLevelPackageList.Where(x => x.Name != library.Name);

            foreach (var topLevelPackageToCheck in otherTopLevelPackages)
            {
                if (topLevelPackageToCheck.Dependencies.Any(x => x.Id == library.Name))
                {
                    if (!redundantTopLevelPackages.Contains(library))
                    {
                        redundantTopLevelPackages.Add(library);
                    }
                }
            }
        }

        return redundantTopLevelPackages;
    }

    private static string GetPackageName(string message)
    {
        var stringSplit = message.Split(' ');
        var name = stringSplit[0];
        return name;
    }
}
