using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NuGet.ProjectModel;

namespace NuGetConsolidator.Core;
public class DependencyGraphGenerator
{
    public string GraphOutputFile { get; }

    public DependencyGraphGenerator()
    {
        GraphOutputFile = Path.GetTempFileName();
    }

    public DependencyGraphSpec GetDependencyGraph(string projectPath)
    {
        var arguments = new[] { "msbuild", $"\"{projectPath}\"", "/t:GenerateRestoreGraphFile", $"/p:RestoreGraphOutputPath={GraphOutputFile}" };
        var directoryName = Path.GetDirectoryName(projectPath);

        using (var commandRunner = new DotNetCommandRunner(directoryName, arguments))
        {
            var commandResult = commandRunner.Execute();

            if (commandResult.IsSuccessful)
            {
                var dependencyGraphText = File.ReadAllText(GraphOutputFile);
                return new DependencyGraphSpec(JsonConvert.DeserializeObject<JObject>(dependencyGraphText));
            }
            else
            {
                throw new Exception($"Error generating dependency graph output.{Environment.NewLine}{commandResult.Output}{commandResult.Error}");
            }
        }
    }
}
