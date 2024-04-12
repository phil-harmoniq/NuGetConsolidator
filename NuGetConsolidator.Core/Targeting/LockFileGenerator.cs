using Microsoft.Extensions.Logging;
using NuGet.ProjectModel;
using NuGetConsolidator.Core.Utilities;

namespace NuGetConsolidator.Core.Targeting;

public class LockFileGenerator
{
    private static readonly ILogger _logger = LogBase.Create<LockFileGenerator>();

    public async Task<LockFile> GetLockFile(string projectPath, string outputPath)
    {
        _logger.LogInformation($"Generating lock file for {projectPath} at {outputPath}");

        var directoryName = Path.GetDirectoryName(projectPath);
        var arguments = new[] { "restore", $"\"{projectPath}\"" };

        using (var commandRunner = new DotNetCommandRunner(directoryName, arguments))
        {
            var commandResult = await commandRunner.ExecuteAsync();
            var outputLockFile = Path.Combine(outputPath, "project.assets.json");
            var lockFile = LockFileUtilities.GetLockFile(outputLockFile, NuGet.Common.NullLogger.Instance);
            return lockFile;
        }
    }
}
