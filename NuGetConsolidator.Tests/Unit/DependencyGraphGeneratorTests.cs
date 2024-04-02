using NuGetConsolidator.Core.Targeting;

namespace NuGetConsolidator.Tests.Unit;

public class DependencyGraphGeneratorTests : TestBase, IDisposable
{
    public readonly DependencyGraphGenerator DependencyGraphGenerator;

    public DependencyGraphGeneratorTests()
    {
        DependencyGraphGenerator = new DependencyGraphGenerator();
    }

    [Fact]
    public void ValidForProjectFolder()
    {
        var deps = DependencyGraphGenerator.GetDependencyGraph(ExamplePath);

        Assert.NotNull(deps);
        Assert.Single(deps.Projects);
        Assert.Equal("NuGetConsolidator.Example", deps.Projects.First().Name);
    }

    [Fact]
    public void ValidForProjectFile()
    {
        var path = Path.Combine(ExamplePath, "NuGetConsolidator.Example.csproj");
        var deps = DependencyGraphGenerator.GetDependencyGraph(path);

        Assert.NotNull(deps);
        Assert.Single(deps.Projects);
        Assert.Equal("NuGetConsolidator.Example", deps.Projects.First().Name);
    }

    [Fact]
    public void ValidForSolutionFolder()
    {
        var deps = DependencyGraphGenerator.GetDependencyGraph(SolutionPath);

        Assert.NotNull(deps);
        Assert.Equal(4, deps.Projects.Count);
        Assert.Contains(deps.Projects, x => x.Name == "NuGetConsolidator.Cli");
        Assert.Contains(deps.Projects, x => x.Name == "NuGetConsolidator.Core");
        Assert.Contains(deps.Projects, x => x.Name == "NuGetConsolidator.Example");
        Assert.Contains(deps.Projects, x => x.Name == "NuGetConsolidator.Tests");
    }

    [Fact]
    public void ValidForSolutionFile()
    {
        var path = Path.Combine(SolutionPath, "NuGetConsolidator.sln");
        var deps = DependencyGraphGenerator.GetDependencyGraph(path);

        Assert.NotNull(deps);
        Assert.Equal(4, deps.Projects.Count);
        Assert.Contains(deps.Projects, x => x.Name == "NuGetConsolidator.Cli");
        Assert.Contains(deps.Projects, x => x.Name == "NuGetConsolidator.Core");
        Assert.Contains(deps.Projects, x => x.Name == "NuGetConsolidator.Example");
        Assert.Contains(deps.Projects, x => x.Name == "NuGetConsolidator.Tests");
    }

    [Fact]
    public void InvalidFileThrowsException()
    {
        var path = Path.Combine(ExamplePath, "NuGetConsolidator.None.csproj");

        Assert.Throws<FileNotFoundException>(() =>
        {
            var deps = DependencyGraphGenerator.GetDependencyGraph(path);
        });
    }

    public void Dispose()
    {
        DependencyGraphGenerator.Dispose();
    }
}
