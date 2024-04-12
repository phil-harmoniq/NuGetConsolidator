using NuGet.ProjectModel;

namespace NuGetConsolidator.Core.Extensions;

public static class LockFileTargetExtensions
{
    public static IList<LockFileTargetLibrary> GetLibraries(
        this LockFileTarget target, IList<string> packageNames)
    {
        return target.Libraries
            .Where(library => packageNames.Contains(library.Name))
            .ToList();
    }
}
