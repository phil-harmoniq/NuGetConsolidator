using NuGet.ProjectModel;
using NuGetConsolidator.Core.Targeting;
using NuGetConsolidator.Tests.Unit.Fixtures;

namespace NuGetConsolidator.Tests.Unit;

public class LockFileGeneratorTests : IClassFixture<LockFileGeneratorTestFixture>
{
    public DependencyGraphSpec DependencyGraph { get; }
    public LockFileGenerator LockFileGenerator { get; }

    public LockFileGeneratorTests(LockFileGeneratorTestFixture fixture)
    {
        DependencyGraph = fixture.DependencyGraph;
        LockFileGenerator = new LockFileGenerator();
    }

    [Fact]
    public void CanGetLockFileForProject()
    {
        foreach (var project in DependencyGraph.Projects)
        {
            var lockFile = LockFileGenerator.GetLockFile(project.FilePath, project.RestoreMetadata.OutputPath);

            Assert.NotNull(lockFile);
        }
    }
}
