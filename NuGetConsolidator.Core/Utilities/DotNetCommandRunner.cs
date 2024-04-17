using Microsoft.Extensions.Logging;
using NuGetConsolidator.Core.Models;
using System.Diagnostics;
using System.Text;

namespace NuGetConsolidator.Core.Utilities;

public class DotNetCommandRunner : IDisposable
{
    private static readonly ILogger _logger = LogBase.Create<DotNetCommandRunner>();
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
            _dotNetProcess.WaitForExit();

            var output = _dotNetProcess.StandardOutput.ReadToEnd();
            var errors = _dotNetProcess.StandardError.ReadToEnd();

            if (_dotNetProcess.ExitCode != 0)
            {
                _logger.LogError(errors);
            }

            return new CommandResult(output.ToString(), errors.ToString(), _dotNetProcess.ExitCode);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }

    public async Task<CommandResult> ExecuteAsync()
    {
        var outputStringBuilder = new StringBuilder();
        var errorStringBuilder = new StringBuilder();

        _dotNetProcess.OutputDataReceived += new DataReceivedEventHandler((sender, args)
            => OutputHandler(sender, args, outputStringBuilder));

        _dotNetProcess.ErrorDataReceived += new DataReceivedEventHandler((sender, args)
            => ErrorHandler(sender, args, outputStringBuilder));

        try
        {
            _dotNetProcess.Start();
            _dotNetProcess.BeginOutputReadLine();
            _dotNetProcess.BeginErrorReadLine();
            await _dotNetProcess.WaitForExitAsync();

            var output = outputStringBuilder.ToString();
            var errors = errorStringBuilder.ToString();

            if (!_dotNetProcess.HasExited)
            {
                _dotNetProcess.Kill();
                return new CommandResult(output, errors, _dotNetProcess.ExitCode);
            }
            else
            {
                return new CommandResult(output, errors, _dotNetProcess.ExitCode);
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

    private static void OutputHandler(object sender, DataReceivedEventArgs outLine, StringBuilder stringBuilder)
    {
        stringBuilder.AppendLine(outLine.Data);
    }

    private static void ErrorHandler(object sender, DataReceivedEventArgs errorLine, StringBuilder stringBuilder)
    {
        stringBuilder.AppendLine(errorLine.Data);
    }
}
