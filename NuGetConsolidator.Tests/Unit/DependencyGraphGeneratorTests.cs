using NuGetConsolidator.Core.Targeting;

namespace NuGetConsolidator.Tests.Unit;

public class DependencyGraphGeneratorTests : IDisposable
{
    private readonly string _solutionPath;
    private readonly string _examplePath;
    private readonly DependencyGraphGenerator _dependencyGraphGenerator;

    public DependencyGraphGeneratorTests()
    {
        var baseDir = AppDomain.CurrentDomain.BaseDirectory;
        var solutionPath = Path.Combine(baseDir, "..", "..", "..", "..");
        var examplePath = Path.Combine(solutionPath, "NuGetConsolidator.Example") + "\\";

        _solutionPath = solutionPath;
        _examplePath = examplePath;
        _dependencyGraphGenerator = new DependencyGraphGenerator();
    }

    [Fact]
    public void ValidForProjectFolder()
    {
        var deps = _dependencyGraphGenerator.GetDependencyGraph(_examplePath);

        Assert.NotNull(deps);
        Assert.Single(deps.Projects);
        Assert.Equal("NuGetConsolidator.Example", deps.Projects.First().Name);
    }

    [Fact]
    public void ValidForProjectFile()
    {
        var path = Path.Combine(_examplePath, "NuGetConsolidator.Example.csproj");
        var deps = _dependencyGraphGenerator.GetDependencyGraph(path);

        Assert.NotNull(deps);
        Assert.Single(deps.Projects);
        Assert.Equal("NuGetConsolidator.Example", deps.Projects.First().Name);
    }

    [Fact]
    public void ValidForSolutionFolder()
    {
        var deps = _dependencyGraphGenerator.GetDependencyGraph(_solutionPath);

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
        var path = Path.Combine(_solutionPath, "NuGetConsolidator.sln");
        var deps = _dependencyGraphGenerator.GetDependencyGraph(path);

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
        var path = Path.Combine(_examplePath, "NuGetConsolidator.None.csproj");

        Assert.Throws<FileNotFoundException>(() =>
        {
            var deps = _dependencyGraphGenerator.GetDependencyGraph(path);
        });
    }

    public void Dispose()
    {
        _dependencyGraphGenerator.Dispose();
    }
}
