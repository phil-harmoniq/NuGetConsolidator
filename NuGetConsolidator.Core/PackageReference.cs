namespace NuGetConsolidator.Core;

public class PackageReference
{
    public string Name { get; set; }
    public string Version { get; set; }
    public PackageReference Parent { get; set; }
    public IList<PackageReference> Children { get; set; } = new List<PackageReference>();
}
