using NuGet.ProjectModel;

namespace NuGetConsolidator.Core;

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

    public IReadOnlyList<LockFileTargetLibrary> GetRedundantPackages()
    {
        var dependencyTreeRoots = new List<PackageReference>();
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
}
