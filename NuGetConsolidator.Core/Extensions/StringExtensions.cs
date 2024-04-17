namespace NuGetConsolidator.Core.Extensions;

public static class StringExtensions
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
