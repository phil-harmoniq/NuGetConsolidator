using NuGet.ProjectModel;

namespace NuGetConsolidator.Core;

public class Library : LockFileTargetLibrary
{
    public bool IsRedundant { get; internal set; }
}
