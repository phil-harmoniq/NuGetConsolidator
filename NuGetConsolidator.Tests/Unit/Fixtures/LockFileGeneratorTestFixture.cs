using Microsoft.Extensions.Logging;
using NuGet.ProjectModel;
using NuGetConsolidator.Core.Targeting;
using NuGetConsolidator.Core.Utilities;

namespace NuGetConsolidator.Tests.Unit.Fixtures;

public class LockFileGeneratorTestFixture : TestBase
{
    public DependencyGraphSpec DependencyGraph { get; }

    public LockFileGeneratorTestFixture()
    {
        using var dependencyGraphGenerator = new DependencyGraphGenerator();
        DependencyGraph = dependencyGraphGenerator.GetDependencyGraph(SolutionPath).Result;
    }
}
