using Microsoft.Extensions.Logging;
using NuGet.ProjectModel;
using NuGetConsolidator.Core.Extensions;
using NuGetConsolidator.Core.Utilities;

namespace NuGetConsolidator.Core.Targeting;

public class DependencyGraphGenerator : IDisposable
{
    private static readonly ILogger _logger = LogBase.Create<DependencyGraphGenerator>();

    public string GraphOutputFile { get; }

    public DependencyGraphGenerator()
    {
        GraphOutputFile = Path.GetTempFileName();
    }

    public DependencyGraphSpec GetDependencyGraph(string projectPath)
    {
        _logger.LogInformation($"Generating dependency graph for {projectPath}");

        var path = projectPath.SanitizePath();
        var arguments = new[] { "msbuild", $"\"{path}\"", "/t:GenerateRestoreGraphFile", $"/p:RestoreGraphOutputPath={GraphOutputFile}" };
        var directoryName = Path.GetDirectoryName(path);

        using (var commandRunner = new DotNetCommandRunner(directoryName, arguments))
        {
            var commandResult = commandRunner.Execute();

            if (commandResult.IsSuccessful)
            {
                //var dependencyGraphText = File.ReadAllText(GraphOutputFile);
                _logger.LogInformation($"Generated dependency graph at {GraphOutputFile}");
                return DependencyGraphSpec.Load(GraphOutputFile);
            }
            else
            {
                var message = $"Error generating dependency graph output.{Environment.NewLine}{commandResult.Output}{commandResult.Error}";
                _logger.LogError(message);
                throw new Exception(message);
            }
        }
    }

    public void Dispose()
    {
        File.Delete(GraphOutputFile);
    }
}
