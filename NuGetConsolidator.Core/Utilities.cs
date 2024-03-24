using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using NuGet.ProjectModel;
using System.Diagnostics;
using System.Text;

namespace NuGetConsolidator.Core;

public static class Utilities
{
    public static CommandResult RunDotNetCommand(string workingDirectory, params string[] arguments)
    {
        var dotNetProcess = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = string.Join(" ", arguments),
                WorkingDirectory = workingDirectory,
                UseShellExecute = false,
                CreateNoWindow= true,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            }
        };

        try
        {
            dotNetProcess.Start();

            //var output = new StringBuilder();
            //var errors = new StringBuilder();
            //var outputTask = ConsumeStreamReaderAsync(dotNetProcess.StandardOutput, output);
            //var errorTask = ConsumeStreamReaderAsync(dotNetProcess.StandardError, errors);
            dotNetProcess.WaitForExit(20000);

            if (!dotNetProcess.HasExited)
            { 
                dotNetProcess.Kill();
                var output = dotNetProcess.StandardOutput.ReadToEnd();
                var errors = dotNetProcess.StandardError.ReadToEnd();
                return new CommandResult(output, errors, dotNetProcess.ExitCode);
            }
            else
            {
                var output = dotNetProcess.StandardOutput.ReadToEnd();
                var errors = dotNetProcess.StandardError.ReadToEnd();
                return new CommandResult(output.ToString(), errors.ToString(), dotNetProcess.ExitCode);
            }
        }
        finally
        {
            dotNetProcess.Dispose();
        }
    }

    public static DependencyGraphSpec GenerateDependencyGraph(string projectPath)
    {
        var graphOutputFile = Path.GetTempFileName();
        var arguments = new[] { "msbuild", $"\"{projectPath}\"", "/t:GenerateRestoreGraphFile", $"/p:RestoreGraphOutputPath={graphOutputFile}" };
        var directoryName = Path.GetDirectoryName(projectPath);

        var commandResult = Utilities.RunDotNetCommand(directoryName, arguments);

        if (commandResult.IsSuccessful)
        {
            var dependencyGraphText = File.ReadAllText(graphOutputFile);
            return new DependencyGraphSpec(JsonConvert.DeserializeObject<JObject>(dependencyGraphText));
        }
        else
        {
            throw new Exception($"Error generating dependency graph output.{Environment.NewLine}{commandResult.Output}{commandResult.Error}");
        }
    }
}
