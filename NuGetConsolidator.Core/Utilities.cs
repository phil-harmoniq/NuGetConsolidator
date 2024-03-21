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

    private static async Task ConsumeStreamReaderAsync(StreamReader reader, StringBuilder lines)
    {
        await Task.Yield();

        string line;
        while ((line = await reader.ReadLineAsync()) != null)
        {
            lines.AppendLine(line);
        }
    }

    public class CommandResult
    {
        public string Output { get; }
        public string Error { get; }
        public int ExitCode { get; }
        public bool IsSuccessful => ExitCode == 0;

        internal CommandResult(string output, string error, int exitCode)
        {
            Output = output;
            Error = error;
            ExitCode = exitCode;
        }
    }
}
