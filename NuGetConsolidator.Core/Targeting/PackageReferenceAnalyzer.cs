using Microsoft.Extensions.Logging;
using NuGet.ProjectModel;
using NuGetConsolidator.Core.Extensions;
using NuGetConsolidator.Core.Utilities;

namespace NuGetConsolidator.Core.Targeting;

public class PackageReferenceAnalyzer
{
    private static readonly ILogger _logger = LogBase.Create<PackageReferenceAnalyzer>();

    public string FrameworkName { get; }
    public IList<LockFileTargetLibrary> TopLevelPackages { get; }
    public IList<LockFileTargetLibrary> AllPackages { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="projectFileDependencyGroup">Contains the top-level dependencies defined in the project file as strings.</param>
    /// <param name="lockFile">Contains additional details about the project file.</param>
    public PackageReferenceAnalyzer(ProjectFileDependencyGroup projectFileDependencyGroup, LockFile lockFile)
    {
        var topLevelPackageNames = projectFileDependencyGroup.GetPackageReferenceNames();
        var target = lockFile.Targets.First(x => x.Name == projectFileDependencyGroup.FrameworkName);
        FrameworkName = projectFileDependencyGroup.FrameworkName;
        TopLevelPackages = target.GetLibraries(topLevelPackageNames);
        AllPackages = target.Libraries;
    }

    public IReadOnlyList<LockFileTargetLibrary> GetRedundantPackages()
    {
        _logger.LogInformation($"Scanning redundant top-level package references for {FrameworkName}");

        var redundantTopLevelPackages = new List<LockFileTargetLibrary>();

        foreach (var library in TopLevelPackages)
        {
            var otherTopLevelPackages = TopLevelPackages.Where(x => x.Name != library.Name);

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
