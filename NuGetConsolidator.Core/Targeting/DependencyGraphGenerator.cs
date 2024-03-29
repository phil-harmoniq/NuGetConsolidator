using NuGet.ProjectModel;
using NuGetConsolidator.Core.Utilities;

namespace NuGetConsolidator.Core.Targeting;

public class DependencyGraphGenerator : IDisposable
{
    public string GraphOutputFile { get; }

    public DependencyGraphGenerator()
    {
        GraphOutputFile = Path.GetTempFileName();
    }

    public DependencyGraphSpec GetDependencyGraph(string projectPath)
    {
        var path = projectPath.SanitizePath();
        var arguments = new[] { "msbuild", $"\"{path}\"", "/t:GenerateRestoreGraphFile", $"/p:RestoreGraphOutputPath={GraphOutputFile}" };
        var directoryName = Path.GetDirectoryName(path);

        using (var commandRunner = new DotNetCommandRunner(directoryName, arguments))
        {
            var commandResult = commandRunner.Execute();

            if (commandResult.IsSuccessful)
            {
                //var dependencyGraphText = File.ReadAllText(GraphOutputFile);
                return DependencyGraphSpec.Load(GraphOutputFile);
            }
            else
            {
                throw new Exception($"Error generating dependency graph output.{Environment.NewLine}{commandResult.Output}{commandResult.Error}");
            }
        }
    }

    public void Dispose()
    {
        File.Delete(GraphOutputFile);
    }
}
