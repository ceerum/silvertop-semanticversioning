
# Silvertop.SemanticVersioning

[![Build](https://github.com/ceerum/silvertop-semanticversioning/actions/workflows/github-actions.yml/badge.svg)](https://github.com/ceerum/silvertop-semanticversioning/actions)
[![Version](https://badgen.net/nuget/v/Silvertop.SemanticVersioning)](https://nuget.org/packages/Silvertop.SemanticVersioning)
[![Downloads](https://img.shields.io/nuget/dt/silvertop.semanticversioning)](https://nuget.org/packages/silvertop-semanticversioning)
[![Coverage](https://app.codacy.com/project/badge/Coverage/9acd907149ee4e9cacc15558dd583214)](https://www.codacy.com/gh/ceerum/silvertop-semanticversioning/dashboard?utm_source=github.com&utm_medium=referral&utm_content=ceerum/silvertop-semanticversioning&utm_campaign=Badge_Coverage)
[![Quality](https://app.codacy.com/project/badge/Grade/9acd907149ee4e9cacc15558dd583214)](https://www.codacy.com/gh/ceerum/silvertop-semanticversioning/dashboard?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=ceerum/silvertop-semanticversioning&amp;utm_campaign=Badge_Grade)
[![Open Issues](https://img.shields.io/github/issues-raw/ceerum/silvertop-semanticversioning)](https://github.com/ceerum/silvertop-semanticversioning/issues)
[![Open Pull Requests](https://img.shields.io/github/issues-pr-raw/ceerum/silvertop-semanticversioning)](https://github.com/ceerum/silvertop-semanticversioning/pulls)
[![GitHub license](https://img.shields.io/github/license/ceerum/silvertop-semanticversioning.svg)](https://github.com/ceerum/silvertop-semanticversioning/blob/master/LICENSE)

The **Silvertop.SemanticVersioning** library contains a .Net class, `SemanticVersion`, that strictly supports Semantic Versioning 2.0.0 as described at https://semver.org/. 

It allows you to validate, parse, generate, compare and sort semantic version strings that strictly adhere to the definition. It also provides quick conversion to and from `System.Version` objects.

## Installing The Package
The package is available from the NuGet.org public feed.
```shell
dotnet add package Silvertop.SemanticVersioning
```
## Comparable vs Equatable
The `IComparable` operators `< > <= >= ==` strictly follow the precedence rules laid down at https://semver.org/#spec-item-11, whereby build metadata is ignored. This means that two semantic versions that differ only by build metadata will return `true` when compared using the `==` operator.

The `IEquatable` interface allows you to check if two semantic versions are indeed identical, as if it were a string comparison. This allows you to use syntax like `semver1.Equals(semver2)` if you wish to check that two semantic versions, including any build metadata that may exist, are actually equal. 

## System.Version Conversion
By default, when converting from a *Version* to or from a **SemanticVersion** only the **Major**, **Minor** and **Build** properties are used, with **Build** mapping to **Patch** and vice versa, since Semantic Versioning has no concept of the **Revision** number. 

#### Pre-Release/Revision Mapping
The conversion methods do allow for some conversion to take place if required, whereby the **Revision** can be treated as a pre-release number, defaulting to using a '**beta**' prefix, but this prefix text can be overridden. 

**Note:** Be aware that if you use this mapping technique then the release version (i.e. no **PreRelease** part to the SemVer) will have a **Revision**  of **0**, which will be less than all pre-release versions. For versioning that ignores revision, such as MSI installs, this may not be an issue. For everything else the release version will be seen as an earlier version than any pre-release versions.

#### Conversion Examples
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

## Usage 
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
**Silvertop.SemanticVersioning** is open source software and is released under the **MIT license**. This license allows the use of **Silvertop.SemanticVersioning** in free and commercial applications/libraries without restriction.