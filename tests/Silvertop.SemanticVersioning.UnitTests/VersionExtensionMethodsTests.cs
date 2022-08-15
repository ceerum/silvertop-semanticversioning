using NUnit.Framework;
using Shouldly;

namespace Silvertop.SemanticVersioning.UnitTests;

[Parallelizable(ParallelScope.All)]
[FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
public class VersionExtensionMethodsTests
{
    [Test]
    public void ToSemanticVersion_TreatingRevisionAsPreReleaseWithDefaultText_ReturnsCorrectOutput()
    {
        // Arrange
        var version = new Version("1.2.3.2");
        
        // Act
        var result = version.ToSemanticVersion(true);

        // Assert
        result.ShouldNotBeNull();
        result.Major.ShouldBe((uint)version.Major);
        result.Minor.ShouldBe((uint)version.Minor);
        result.Patch.ShouldBe((uint)version.Build);
        result.PreRelease.ShouldBe($"beta.{version.Revision}");
        result.BuildMetadata.ShouldBeEmpty();
    }

    [Test]
    public void ToSemanticVersion_TreatingRevisionAsPreReleaseWithProvidedText_ReturnsCorrectOutput()
    {
        // Arrange
        var version = new Version("1.2.3.2");
        var preReleasePrefix = "someText";
        
        // Act
        var result = version.ToSemanticVersion(true, preReleasePrefix);

        // Assert
        result.ShouldNotBeNull();
        result.Major.ShouldBe((uint)version.Major);
        result.Minor.ShouldBe((uint)version.Minor);
        result.Patch.ShouldBe((uint)version.Build);
        result.PreRelease.ShouldBe($"{preReleasePrefix}.{version.Revision}");
        result.BuildMetadata.ShouldBeEmpty();
    }

    [Test]
    public void ToSemanticVersion_NotTreatingRevisionAsPreRelease_ReturnsCorrectOutput()
    {
        // Arrange
        var version = new Version("1.2.3.2");
        
        // Act
        var result = version.ToSemanticVersion(false);

        // Assert
        result.ShouldNotBeNull();
        result.Major.ShouldBe((uint)version.Major);
        result.Minor.ShouldBe((uint)version.Minor);
        result.Patch.ShouldBe((uint)version.Build);
        result.PreRelease.ShouldBeEmpty();
        result.BuildMetadata.ShouldBeEmpty();
    }
}
