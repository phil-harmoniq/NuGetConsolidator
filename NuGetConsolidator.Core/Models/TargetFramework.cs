using NuGet.ProjectModel;

namespace NuGetConsolidator.Core.Models;

public class TargetFramework
{
    public string Name { get; set; }
    public IList<LockFileTargetLibrary> RedundantLibraries { get; set; } = new List<LockFileTargetLibrary>();
}
