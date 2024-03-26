using NuGet.Common;
using NuGet.ProjectModel;
using NuGetConsolidator.Core.Modification;

namespace NuGetConsolidator.Core.Targeting;

public class LockFileGenerator
{
    public LockFile GetLockFile(string projectPath, string outputPath)
    {
        var directoryName = Path.GetDirectoryName(projectPath);
        var arguments = new[] { "restore", $"\"{projectPath}\"" };

        using (var commandRunner = new DotNetCommandRunner(directoryName, arguments))
        {
            var commandResult = commandRunner.Execute();
            var outputLockFile = Path.Combine(outputPath, "project.assets.json");
            var lockFile = LockFileUtilities.GetLockFile(outputLockFile, NullLogger.Instance);
            return lockFile;
        }
    }
}
