using NuGet.Common;
using NuGet.ProjectModel;

namespace NuGetConsolidator.Core.Providers;

internal class LockFileProvider
{
    internal LockFile GetLockFile(string projectPath, string outputPath)
    {
        var directoryName = Path.GetDirectoryName(projectPath);
        var arguments = new[] { "restore", $"\"{projectPath}\"" };
        var commandResult = Utilities.RunDotNetCommand(directoryName, arguments);
        var outputLockFile = Path.Combine(outputPath, "project.assets.json");
        var lockFile = LockFileUtilities.GetLockFile(outputLockFile, NullLogger.Instance);
        return lockFile;
    }
}
