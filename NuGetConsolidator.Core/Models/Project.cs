namespace NuGetConsolidator.Core.Models;

public class Project
{
    public string Name { get; set; }
    public IList<TargetFramework> TargetFrameworks { get; } = new List<TargetFramework>();
}