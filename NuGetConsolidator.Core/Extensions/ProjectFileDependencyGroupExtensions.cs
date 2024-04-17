using NuGet.ProjectModel;

namespace NuGetConsolidator.Core.Extensions;

public static class ProjectFileDependencyGroupExtensions
{
    public static IList<string> GetPackageReferenceNames(
        this ProjectFileDependencyGroup group)
    {
        return group.Dependencies
            .Select(GetPackageName)
            .ToList();
    }

    private static string GetPackageName(string message)
    {
        var stringSplit = message.Split(' ');
        var name = stringSplit[0];
        return name;
    }
}
