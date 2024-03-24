using NuGet.Packaging.Core;
using NuGet.ProjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace NuGetConsolidator.Core;

public class TransitiveDependencyScanner
{
    public IList<LockFileTargetLibrary> GetRedundantTopLevelPackages(IList<LockFileTargetLibrary> libraries)
    {
        var topLevelPackages = new List<LockFileTargetLibrary>();
        var dependencyPackages = new List<PackageDependency>();

        foreach (var library in libraries)
        {
            topLevelPackages.Add(library);
            dependencyPackages.AddRange(library.Dependencies);
        }

        var redundantTopLevelPackages = topLevelPackages
            .Where(topLevelPackage => dependencyPackages.Any(dep => dep.Id.Equals(topLevelPackage)))
            .ToList();

        return redundantTopLevelPackages;
    }

    //private IList<LockFileTargetLibrary> RecursivelyFlattenDependencies(IList<LockFileTargetLibrary> dependencies)
    //{
    //    var flattenedDependencyList = new List<LockFileTargetLibrary>();

    //    foreach (var dependency in dependencies)
    //    {
    //        if (dependency.Dependencies.Any())
    //        {
    //            flattenedDependencyList.Add(RecursivelyFlattenDependencies(dependency.Dependencies);
    //        }
    //    }
    //}

    //private IList<LockFileTargetLibrary> CheckPackagesRecursively(
    //    IList<LockFileTargetLibrary> topLevellibraries)
    //{
    //    var redundantPackages = new List<LockFileTargetLibrary>();

    //    foreach (var library in topLevellibraries)
    //    {
    //        redundantPackages.AddRange(CheckPackagesRecursively(topLevellibraries));
    //    }
        
    //    return redundantPackages;
    //}
}
