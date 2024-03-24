using NuGet.ProjectModel;

namespace NuGetConsolidator.Core;

public static partial class ProjectAnalyzer
{
    public class TargetFramework
    {
        public string Name { get; set; }
        public IList<LockFileTargetLibrary> RedundantLibraries { get; set; } = new List<LockFileTargetLibrary>();
    }
}
