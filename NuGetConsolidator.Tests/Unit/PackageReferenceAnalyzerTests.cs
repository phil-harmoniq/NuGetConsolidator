using NuGet.ProjectModel;
using NuGetConsolidator.Core.Targeting;
using NuGetConsolidator.Tests.Unit.Fixtures;

namespace NuGetConsolidator.Tests.Unit;

public class PackageReferenceAnalyzerTests : IClassFixture<PackageReferenceAnalyzerTestFixture>
{
    public IList<LockFile> LockFiles { get; }

    public PackageReferenceAnalyzerTests(PackageReferenceAnalyzerTestFixture fixture)
    {
        LockFiles = fixture.LockFiles;
    }

    [Fact]
    public void CanGetRedundantPackages()
    {
        foreach (var lockFile in LockFiles)
        {
            foreach (var dependencyGroup in lockFile.ProjectFileDependencyGroups)
            {
                var packageReferenceAnalyzer = new PackageReferenceAnalyzer(dependencyGroup, lockFile);

                var redundantPackages = packageReferenceAnalyzer.GetRedundantPackages();
            }
        }
    }
}
