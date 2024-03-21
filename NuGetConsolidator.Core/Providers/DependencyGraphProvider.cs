using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NuGet.ProjectModel;

namespace NuGetConsolidator.Core.Providers;

public class DependencyGraphProvider
{
    public DependencyGraphProvider() { }

    public DependencyGraphSpec GenerateDependencyGraph(string projectPath)
    {
        var dgOutput = Path.Combine(Path.GetTempPath(), Path.GetTempFileName());
        var arguments = new[] { "msbuild", $"\"{projectPath}\"", "/t:GenerateRestoreGraphFile", $"/p:RestoreGraphOutputPath={dgOutput}" };
        var directoryName = Path.GetDirectoryName(projectPath);

        var commandResult = Utilities.RunDotNetCommand(directoryName, arguments);

        if (commandResult.IsSuccessful)
        {
            var dependencyGraphText = File.ReadAllText(dgOutput);
            return new DependencyGraphSpec().GetProjectSpec(projectPath);
        }
        else
        {
            throw new Exception($"Unable to process the the project `{projectPath}. Are you sure this is a valid .NET Core or .NET Standard project type?" +
                                                 $"\r\n\r\nHere is the full error message returned from the Microsoft Build Engine:\r\n\r\n" + commandResult.Output);
        }
    }
}
