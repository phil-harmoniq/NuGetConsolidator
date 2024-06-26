﻿using NuGet.Versioning;
using NuGetConsolidator.Core.Targeting;
using NuGetConsolidator.Core.Utilities;

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

    [Fact]
    public async Task Test3()
    {
        var baseDir = AppDomain.CurrentDomain.BaseDirectory;
        var solutionDir = Path.Combine(baseDir, "..", "..", "..", "..");
        var exampleDir = Path.Combine(solutionDir, "NuGetConsolidator.Example") + Path.DirectorySeparatorChar;
        var projects = await ProjectAnalyzer.GetRedundantPackages(exampleDir);

        foreach (var project in projects)
        {
            foreach (var library in project.TargetFrameworks.First().RedundantLibraries)
            {
                Assert.ThrowsAny<Exception>(() =>
                {
                    MsBuildHelper.RemovePackageReference(exampleDir, library.Name);
                });
            }
        }
    }
}
