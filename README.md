##  <center>Silvertop.SemanticVersioning</center>
The `Silvertop.SemanticVersioning` library contains a .Net class, `SemanticVersion`, that strictly supports Semantic Versioning 2.0.0 as described at https://semver.org/. 

It allows you to validate, parse, generate, compare and sort semantic version strings that strictly adhere to the definition. It also provides for quick conversion to and from `System.Version` objects.

### Installing The Package
The package is available from the NuGet.org public feed.
```shell
dotnet add package Silvertop.SemanticVersioning
```
### Comparable vs Equatable
The `IComparable` operators `< > <= >= ==` strictly follow the precedence rules laid down at https://semver.org/#spec-item-11, whereby build metadata is ignored. This means that two semantic versions that differ only by build metadata will return `true` when compared using the `==` operator.

The `IEquatable` interface allows you to check if two semantic versions are indeed identical, as if it were a string comparison. This allows you to use syntax like `semver1.Equals(semver2)` if you wish to check that two semantic versions, including any build metadata that may exist, are actually equal. 

### System.Version Conversion
By default, when converting from a `Version` to or from a `SemanticVersion` only the `Major`, `Minor` and `Build` properties are used, with `Build` mapping to `Patch` and vice versa, since Semantic Versioning has no concept of the `Revision` number.

The conversion methods do allow for some conversion to take place if required, whereby the `Revision` can be treated as a pre-release number, defaulting to using a '**beta**' prefix, but this can also be overridden. 
```csharp
// To Version with pre-release number conversion
var semanticVersion1 = SemanticVersion.Parse("3.4.66-beta.10+20220815.1");
var version1 = semanticVersion1.ToVersion(true);
var versionString1 = version1.ToString(); // 3.4.66.10

// To Version with no pre-release number conversion
var semanticVersion2 = SemanticVersion.Parse("3.4.66-beta.10+20220815.1");
var version2 = semanticVersion2.ToVersion(false);
var versionString2 = version2.ToString(); // 3.4.66.0

// From Version with pre-release number conversion
var version3 = new Version(5, 77, 4, 2);
var semanticVersion3 = SemanticVersion.FromVersion(version3, true, "alpha");
var semanticVersionString3 = semanticVersion3.ToString(); // 5.77.4-alpha.2

// From Version with no pre-release number conversion
var version4 = new Version(5, 77, 4, 2);
var semanticVersion4 = SemanticVersion.FromVersion(version4, false);
var semanticVersionString4 = semanticVersion4.ToString(); // 5.77.4
```

### Usage Examples
```csharp
using Silvertop.SemanticVersioning;

namespace SomeNamespace;

public class SomeClass
{
    public Usage()
    {
        // Construction
        var semVer1 = new SemanticVersion(1, 2);
        var semVer2 = new SemanticVersion(1, 2, 3);
        var semVer3 = new SemanticVersion(1, 2, 3, "alpha.1.4");
        var semVer4 = new SemanticVersion(1, 2, 3, "alpha.1.4", "build2656");

        // ToString
        var semVerString1 = semVer1.ToString();

        // Version Conversion
        var version1 = new Version(5, 4, 66);
        var semVer5 = version1.ToSemanticVersion(true, "beta");
        var version2 = semVer5.ToVersion(true);
        var semVer6 = SemanticVersion.FromVersion(version2);
    
        // Parsing
        var semVer6 = SemanticVersion.Parse("3.4.66-beta.10+20220815.1");
        var parsedOk = SemanticVersion.TryParse("3.4.66-beta.10+20220815.1", out var semVer7);

        // Comparison
        var greaterThan = semVer6 > semVer1;
        var greaterThanOrEqualTo = semVer6 >= semVer2;
        var lessThan = semVer6 < semVer2;
        var lessThanOrEqualTo = semVer6 <= semVer2;
        var equal = semVer6 == semVer2;

        // Equality
        var identical = semVer6.Equals(semVer5);
        
        // Member Access
        var major = semVer2.Major;
        var minor = semVer2.Minor;
        var patch = semVer2.Patch;
        var isPreRelease = semVer2.IsPreRelease;
        var preRelease = semVer2.PreRelease;
        var buildMetadata = semVer2.BuildMetadata;
    }
}
```

## License

`Silvertop.SemanticVersioning` is Open Source software and is released under the **MIT license**. This license allows the use of `Silvertop.SemanticVersioning` in free and commercial applications/libraries without restriction.