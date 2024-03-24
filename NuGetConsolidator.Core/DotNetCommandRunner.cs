using System.Diagnostics;

namespace NuGetConsolidator.Core;

public class DotNetCommandRunner : IDisposable
{
    private readonly Process _dotNetProcess;

    public DotNetCommandRunner(string workingDirectory, params string[] arguments)
    {
        _dotNetProcess = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = string.Join(" ", arguments),
                WorkingDirectory = workingDirectory,
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            }
        };
    }

    public CommandResult Execute()
    {
        try
        {
            _dotNetProcess.Start();

            //var output = new StringBuilder();
            //var errors = new StringBuilder();
            //var outputTask = ConsumeStreamReaderAsync(dotNetProcess.StandardOutput, output);
            //var errorTask = ConsumeStreamReaderAsync(dotNetProcess.StandardError, errors);
            _dotNetProcess.WaitForExit(20000);

            if (!_dotNetProcess.HasExited)
            {
                _dotNetProcess.Kill();
                var output = _dotNetProcess.StandardOutput.ReadToEnd();
                var errors = _dotNetProcess.StandardError.ReadToEnd();
                return new CommandResult(output, errors, _dotNetProcess.ExitCode);
            }
            else
            {
                var output = _dotNetProcess.StandardOutput.ReadToEnd();
                var errors = _dotNetProcess.StandardError.ReadToEnd();
                return new CommandResult(output.ToString(), errors.ToString(), _dotNetProcess.ExitCode);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }

    public void Dispose()
    {
        _dotNetProcess.Dispose();
    }
}
