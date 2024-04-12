using NuGet.ProjectModel;
using NuGetConsolidator.Core.Targeting;

namespace NuGetConsolidator.Tests.Unit.Fixtures;

public class PackageReferenceAnalyzerTestFixture : TestBase
{
    public DependencyGraphSpec DependencyGraph { get; }
    public LockFileGenerator LockFileGenerator { get; }
    public IList<LockFile> LockFiles { get; } = [];

    public PackageReferenceAnalyzerTestFixture()
    {
        using var dependencyGraphGenerator = new DependencyGraphGenerator();
        DependencyGraph = dependencyGraphGenerator.GetDependencyGraph(SolutionPath).Result;
        LockFileGenerator = new LockFileGenerator();

        foreach (var project in DependencyGraph.Projects)
        {
            var lockFile = LockFileGenerator.GetLockFile(project.FilePath, project.RestoreMetadata.OutputPath).Result;
            LockFiles.Add(lockFile);
        }
    }
}
