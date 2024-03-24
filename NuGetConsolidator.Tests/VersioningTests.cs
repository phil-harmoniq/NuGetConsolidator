using NuGet.ProjectModel;
using NuGet.Versioning;
using NuGetConsolidator.Core;

namespace NuGetConsolidator.Tests;

public class VersioningTests
{
    [Fact]
    public void Test()
    {
        var lowerVersion = new NuGetVersion("1.0.3.100");
        var middleVersion = new NuGetVersion("1.1.5.10");
        var upperVersion = new NuGetVersion("1.1.10");

        Assert.True(lowerVersion < middleVersion);
        Assert.True(middleVersion < upperVersion);
        Assert.True(lowerVersion < upperVersion);
    }

    [Fact]
    public async Task Test2()
    {
        var baseDir = AppDomain.CurrentDomain.BaseDirectory;
        var solutionDir = Path.Combine(baseDir, "..", "..", "..", "..");
        var exampleDir = Path.Combine(solutionDir, "NuGetConsolidator.Example");
        var projects = await ProjectAnalyzer.GetRedundantPackages(exampleDir);
    }
}
