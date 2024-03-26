using Microsoft.Build.Construction;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis.MSBuild;
using NuGet.LibraryModel;
using NuGet.Versioning;
using System.Globalization;

namespace NuGetConsolidator.Core.Modification;

/// <summary>
/// Implementation referenced from NuGet.Client repo.
/// https://github.com/NuGet/NuGet.Client/blob/dev/src/NuGet.Core/NuGet.CommandLine.XPlat/Utility/MSBuildAPIUtility.cs
/// </summary>
public static class MsBuildHelper
{
    private const string PACKAGE_REFERENCE_TYPE_TAG = "PackageReference";

    static MsBuildHelper()
    {
        //Environment.SetEnvironmentVariable("MSBuildSDKsPath", "C:\\Program Files\\dotnet\\sdk\\8.0.202\\Sdks");
        //var path = Environment.GetEnvironmentVariable("MSBuildSDKsPath");
        MSBuildLocator.RegisterDefaults();
        MSBuildWorkspace.Create();
    }

    public static int RemovePackageReference(string projectPath, string packageName)
    {
        var libraryDependency = new LibraryDependency
        {
            LibraryRange = new LibraryRange(
                name: packageName,
                versionRange: VersionRange.All,
                typeConstraint: LibraryDependencyTarget.Package)
        };

        return RemovePackageReference(projectPath, libraryDependency);
    }

    /// <summary>
    /// Remove all package references to the project.
    /// </summary>
    /// <param name="projectPath">Path to the csproj file of the project.</param>
    /// <param name="libraryDependency">Package Dependency of the package to be removed.</param>
    private static int RemovePackageReference(string projectPath, LibraryDependency libraryDependency)
    {
        var project = GetProject(projectPath);

        var existingPackageReferences = project.ItemsIgnoringCondition
            .Where(item => item.ItemType.Equals(PACKAGE_REFERENCE_TYPE_TAG, StringComparison.OrdinalIgnoreCase) &&
                           item.EvaluatedInclude.Equals(libraryDependency.Name, StringComparison.OrdinalIgnoreCase));

        if (existingPackageReferences.Any())
        {
            //// We validate that the operation does not remove any imported items
            //// If it does then we throw a user friendly exception without making any changes
            //ValidateNoImportedItemsAreUpdated(existingPackageReferences, libraryDependency, REMOVE_OPERATION);

            project.RemoveItems(existingPackageReferences);
            project.Save();
            ProjectCollection.GlobalProjectCollection.UnloadProject(project);

            return 0;
        }
        else
        {
            //Logger.LogError(string.Format(CultureInfo.CurrentCulture,
            //    Strings.Error_UpdatePkgNoSuchPackage,
            //    project.FullPath,
            //    libraryDependency.Name,
            //    REMOVE_OPERATION));
            ProjectCollection.GlobalProjectCollection.UnloadProject(project);

            return 1;
        }
    }

    /// <summary>
    /// Opens an MSBuild.Evaluation.Project type from a csproj file.
    /// </summary>
    /// <param name="projectCSProjPath">CSProj file which needs to be evaluated</param>
    /// <returns>MSBuild.Evaluation.Project</returns>
    private static Project GetProject(string projectCSProjPath)
    {
        var projectRootElement = TryOpenProjectRootElement(projectCSProjPath);
        if (projectCSProjPath == null)
        {
            throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "Error opening MsBuild project.", projectCSProjPath));
        }

        var globalProperties = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                { { "TargetFramework", "net8.0" } };

        return new Project(projectRootElement, globalProperties, toolsVersion: null);
    }

    private static ProjectRootElement TryOpenProjectRootElement(string filename)
    {
        try
        {
            // There is ProjectRootElement.TryOpen but it does not work as expected
            // I.e. it returns null for some valid projects
            return ProjectRootElement.Open(filename, ProjectCollection.GlobalProjectCollection, preserveFormatting: true);
        }
        catch (Microsoft.Build.Exceptions.InvalidProjectFileException)
        {
            return null;
        }
    }
}
