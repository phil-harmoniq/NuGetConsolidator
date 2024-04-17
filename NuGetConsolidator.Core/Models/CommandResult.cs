namespace NuGetConsolidator.Core.Models;

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