using NuGet.Packaging.Core;
using NuGet.ProjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace NuGetConsolidator.Core;

public static class Extensions
{
    public static LockFileTargetLibrary ToLockFileTargetLibrary(
        this PackageDependency packageDependency,
        IEnumerable<LockFileTargetLibrary> allPackages)
    {
        var targetLibrary = allPackages
            .FirstOrDefault(x => x.Name == packageDependency.Id);
        return targetLibrary;
    }

    //public static PackageReference ToPackageReference(
    //    this PackageDependency packageDependency)
    //{
    //    var packageReference = new PackageReference
    //    {
    //        Name = packageDependency.Id,
    //        Version = packageDependency.VersionRange.OriginalString
    //    };
    //}
}
