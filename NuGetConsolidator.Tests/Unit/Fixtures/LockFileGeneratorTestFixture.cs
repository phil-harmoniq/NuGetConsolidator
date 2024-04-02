using NuGet.ProjectModel;
using NuGetConsolidator.Core.Targeting;

namespace NuGetConsolidator.Tests.Unit.Fixtures;

public class LockFileGeneratorTestFixture : TestBase
{
    public DependencyGraphSpec DependencyGraph { get; }

    public LockFileGeneratorTestFixture()
    {
        using var dependencyGraphGenerator = new DependencyGraphGenerator();
        DependencyGraph = dependencyGraphGenerator.GetDependencyGraph(SolutionPath);
    }
}
