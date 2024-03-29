namespace NuGetConsolidator.Core.Utilities;

public static class PathHelper
{
    public static string SanitizePath(this string path)
    {
        // Trim trailing slash if directory. Trailing slash is not supported in dotnet command.
        var fileAttributes = File.GetAttributes(path);

        if (fileAttributes.HasFlag(FileAttributes.Directory))
        {
            var dir = new DirectoryInfo(path);
            path = Path.Combine(dir.Parent.ToString(), dir.Name);
        }

        return path;
    }
}
