using NuGetConsolidator.Core.Targeting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuGetConsolidator.Tests;

public class TestBase
{
    protected string SolutionPath { get; }
    protected string ExamplePath { get; }

    public TestBase()
    {
        var baseDir = AppDomain.CurrentDomain.BaseDirectory;
        var solutionPath = Path.Combine(baseDir, "..", "..", "..", "..");
        var examplePath = Path.Combine(solutionPath, "NuGetConsolidator.Example") + Path.DirectorySeparatorChar;

        SolutionPath = solutionPath;
        ExamplePath = examplePath;
    }
}
