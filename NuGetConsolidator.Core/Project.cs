namespace NuGetConsolidator.Core;

public static partial class ProjectAnalyzer
{
    public class Project
    {
        public string Name { get; set; }
        public IList<TargetFramework> TargetFrameworks { get; } = new List<TargetFramework>();
    }
}
