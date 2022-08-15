using NUnit.Framework;
using Shouldly;

namespace Silvertop.SemanticVersioning.UnitTests;

[Parallelizable(ParallelScope.All)]
[FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
public class SemanticVersionTests
{
    [Test]
    public void CompareTo_WithOtherAsNullObject_ReturnsCorrectOutput()
    {
        // Arrange
        var leftHand = new SemanticVersion("1.2.3-88.2");
        object? rightHand = null;

        // Act
        var result = leftHand.CompareTo(rightHand);

        // Assert
        result.ShouldBe(1);
    }

    [Test]
    public void CompareTo_WithOtherAsNullSemanticVersion_ReturnsCorrectOutput()
    {
        // Arrange
        var leftHand = new SemanticVersion("1.2.3-88.2");
        SemanticVersion? rightHand = null;

        // Act
        var result = leftHand.CompareTo(rightHand);

        // Assert
        result.ShouldBe(1);
    }

    [TestCase("0.0.1", "0.0.2", -1, TestName = "Left Patch < Right Patch")]
    [TestCase("0.1.0", "0.2.0", -1, TestName = "Left Minor < Right Minor")]
    [TestCase("1.0.0", "2.0.0", -1, TestName = "Left Major < Right Major")]
    [TestCase("1.2.3-beta.1", "1.2.3", -1, TestName = "Numerics Same, Left Has Pre-Release, Right Has No Pre-Release")]
    [TestCase("1.2.3-beta.1.1", "1.2.3-beta.1", -1, TestName = "Numerics Same, Left Has More Pre-Release Parts")]
    [TestCase("1.2.3-88", "1.2.3-89", -1, TestName = "Numerics Same, Left Has Lower Numerical Pre-Release")]
    [TestCase("1.2.3-88.1", "1.2.3-88.2", -1, TestName = "Numerics Same, Left Has Lower Numerical Pre-Release On Last Part")]
    [TestCase("1.2.3-88.2.A", "1.2.3-88.2.a", -1, TestName = "Numerics Same, Left Has Lower Pre-Release Lexically in ASCII Sort Order")]
    [TestCase("1.2.3-88.2.99", "1.2.3-88.2.Z", -1, TestName = "Numerics Same, Left Has Numeric Pre-Release, Right Has Alpha Pre-Release")]
    [TestCase("1.2.3", "1.2.3", 0, TestName = "Numerics Same, No Pre-Release")]
    [TestCase("1.2.3-beta.1", "1.2.3-beta.1", 0, TestName = "Numerics Same, Pre-Release Same")]
    [TestCase("1.2.3-beta.1.2", "1.2.3-beta.1.2", 0, TestName = "Numerics Same, Pre-Release Same, Multiple Pre-Release Parts")]
    [TestCase("1.2.3+2022.12.01", "1.2.3+2022.12.01", 0, TestName = "Numerics Same, No Pre-Release, Build Metadata Same")]
    [TestCase("1.2.3+2022.12.01", "1.2.3+2022.12.11", 0, TestName = "Numerics Same, No Pre-Release, Build Metadata Different")]
    [TestCase("1.2.3-beta.1+2022.12.01", "1.2.3-beta.1+2022.12.01", 0, TestName = "Numerics Same, Pre-Release Same, Build Metadata Same")]
    [TestCase("1.2.3-beta.1+2022.12.01", "1.2.3-beta.1+2022.11.01", 0, TestName = "Numerics Same, Pre-Release Same, Build Metadata Different")]
    [TestCase("0.0.2", "0.0.1", 1, TestName = "Left Patch > Right Patch")]
    [TestCase("0.2.0", "0.1.0", 1, TestName = "Left Minor > Right Minor")]
    [TestCase("2.0.0", "1.0.0", 1, TestName = "Left Major > Right Major")]
    [TestCase("1.2.3", "1.2.3-beta.1", 1, TestName = "Numerics Same, Left Has No Pre-Release, Right Has Pre-Release")]
    [TestCase("1.2.3-beta.1", "1.2.3-beta.1.1", 1, TestName = "Numerics Same, Left Has Less Pre-Release Parts")]
    [TestCase("1.2.3-89", "1.2.3-88", 1, TestName = "Numerics Same, Left Has Higher Numerical Pre-Release")]
    [TestCase("1.2.3-88.2", "1.2.3-88.1", 1, TestName = "Numerics Same, Left Has Higher Numerical Pre-Release On Last Part")]
    [TestCase("1.2.3-88.2.a", "1.2.3-88.2.A", 1, TestName = "Numerics Same, Left Has Higher Pre-Release Lexically in ASCII Sort Order")]
    [TestCase("1.2.3-88.2.Z", "1.2.3-88.2.99", 1, TestName = "Numerics Same, Left Has Alpha Pre-Release, Right Has Numeric Pre-Release")]
    public void CompareTo_WithOtherAsObject_ReturnsCorrectOutput(string leftHandSide, string rightHandSide, int expectedResult)
    {
        // Arrange
        var leftHand = new SemanticVersion(leftHandSide);
        var rightHand = new SemanticVersion(rightHandSide);

        // Act
        var result = leftHand.CompareTo((object)rightHand);

        // Assert
        result.ShouldBe(expectedResult);
    }

    [TestCase("0.0.1", "0.0.2", -1, TestName = "Left Patch < Right Patch")]
    [TestCase("0.1.0", "0.2.0", -1, TestName = "Left Minor < Right Minor")]
    [TestCase("1.0.0", "2.0.0", -1, TestName = "Left Major < Right Major")]
    [TestCase("1.2.3-beta.1", "1.2.3", -1, TestName = "Numerics Same, Left Has Pre-Release, Right Has No Pre-Release")]
    [TestCase("1.2.3-beta.1.1", "1.2.3-beta.1", -1, TestName = "Numerics Same, Left Has More Pre-Release Parts")]
    [TestCase("1.2.3-88", "1.2.3-89", -1, TestName = "Numerics Same, Left Has Lower Numerical Pre-Release")]
    [TestCase("1.2.3-88.1", "1.2.3-88.2", -1, TestName = "Numerics Same, Left Has Lower Numerical Pre-Release On Last Part")]
    [TestCase("1.2.3-88.2.A", "1.2.3-88.2.a", -1, TestName = "Numerics Same, Left Has Lower Pre-Release Lexically in ASCII Sort Order")]
    [TestCase("1.2.3-88.2.99", "1.2.3-88.2.Z", -1, TestName = "Numerics Same, Left Has Numeric Pre-Release, Right Has Alpha Pre-Release")]
    [TestCase("1.2.3", "1.2.3", 0, TestName = "Numerics Same, No Pre-Release")]
    [TestCase("1.2.3-beta.1", "1.2.3-beta.1", 0, TestName = "Numerics Same, Pre-Release Same")]
    [TestCase("1.2.3-beta.1.2", "1.2.3-beta.1.2", 0, TestName = "Numerics Same, Pre-Release Same, Multiple Pre-Release Parts")]
    [TestCase("1.2.3+2022.12.01", "1.2.3+2022.12.01", 0, TestName = "Numerics Same, No Pre-Release, Build Metadata Same")]
    [TestCase("1.2.3+2022.12.01", "1.2.3+2022.12.11", 0, TestName = "Numerics Same, No Pre-Release, Build Metadata Different")]
    [TestCase("1.2.3-beta.1+2022.12.01", "1.2.3-beta.1+2022.12.01", 0, TestName = "Numerics Same, Pre-Release Same, Build Metadata Same")]
    [TestCase("1.2.3-beta.1+2022.12.01", "1.2.3-beta.1+2022.11.01", 0, TestName = "Numerics Same, Pre-Release Same, Build Metadata Different")]
    [TestCase("0.0.2", "0.0.1", 1, TestName = "Left Patch > Right Patch")]
    [TestCase("0.2.0", "0.1.0", 1, TestName = "Left Minor > Right Minor")]
    [TestCase("2.0.0", "1.0.0", 1, TestName = "Left Major > Right Major")]
    [TestCase("1.2.3", "1.2.3-beta.1", 1, TestName = "Numerics Same, Left Has No Pre-Release, Right Has Pre-Release")]
    [TestCase("1.2.3-beta.1", "1.2.3-beta.1.1", 1, TestName = "Numerics Same, Left Has Less Pre-Release Parts")]
    [TestCase("1.2.3-89", "1.2.3-88", 1, TestName = "Numerics Same, Left Has Higher Numerical Pre-Release")]
    [TestCase("1.2.3-88.2", "1.2.3-88.1", 1, TestName = "Numerics Same, Left Has Higher Numerical Pre-Release On Last Part")]
    [TestCase("1.2.3-88.2.a", "1.2.3-88.2.A", 1, TestName = "Numerics Same, Left Has Higher Pre-Release Lexically in ASCII Sort Order")]
    [TestCase("1.2.3-88.2.Z", "1.2.3-88.2.99", 1, TestName = "Numerics Same, Left Has Alpha Pre-Release, Right Has Numeric Pre-Release")]
    public void CompareTo_WithOtherAsSemanticVersion_ReturnsCorrectOutput(string leftHandSide, string rightHandSide, int expectedResult)
    {
        // Arrange
        var leftHand = new SemanticVersion(leftHandSide);
        var rightHand = new SemanticVersion(rightHandSide);

        // Act
        var result = leftHand.CompareTo(rightHand);

        // Assert
        result.ShouldBe(expectedResult);
    }

    [Test]
    public void CompareToObject_WithNonSemVer_Throws()
    {
        // Arrange
        var leftHand = new SemanticVersion("1.2.3");
        object rightHand = new Version(1, 2, 3);

        // Act
        void Act() => _ = leftHand.CompareTo(rightHand);

        // Assert
        Should.Throw<ArgumentException>(Act);
    }

    [TestCase("@20220101.1", TestName = "Non alphanumeric characters")]
    [TestCase("20220101..1", TestName = "Double period")]
    [TestCase(".20220101.1", TestName = "Starting with a period")]
    [TestCase("20220101.1.2.", TestName = "Ending with a period")]
    [TestCase("20220101 .1.2", TestName = "Contains whitespace")]
    [TestCase("+20220101.1.2", TestName = "Starting with a plus")]
    public void Ctor_WithInvalidBuildMetadata_Throws(string buildMetadata)
    {
        // Arrange
        const uint major = 1;
        const uint minor = 2;
        const uint patch = 0;
        const string preRelease = "";

        // Act
        void Act() => _ = new SemanticVersion(major, minor, patch, preRelease, buildMetadata);

        // Assert
        Should.Throw<ArgumentException>(Act);
    }

    [TestCase("@beta.1", TestName = "Non alphanumeric characters")]
    [TestCase("beta..1", TestName = "Double period")]
    [TestCase(".beta.1", TestName = "Starting with a period")]
    [TestCase("beta.1.2.", TestName = "Ending with a period")]
    [TestCase("beta .1.2", TestName = "Contains whitespace")]
    public void Ctor_WithInvalidPreRelease_Throws(string preRelease)
    {
        // Arrange
        const uint major = 1;
        const uint minor = 2;
        const uint patch = 0;
        const string buildMetadata = "";

        // Act
        void Act() => _ = new SemanticVersion(major, minor, patch, preRelease, buildMetadata);

        // Assert
        Should.Throw<ArgumentException>(Act);
    }

    [TestCase("1.2.3-@beta.1+1", TestName = "Pre-Release: Non alphanumeric characters")]
    [TestCase("1.2.3-beta..1+1", TestName = "Pre-Release: Double period")]
    [TestCase("1.2.3-.beta.1+1", TestName = "Pre-Release: Starting with a period")]
    [TestCase("1.2.3-beta.1.2.+1", TestName = "Pre-Release: Ending with a period")]
    [TestCase("1.2.3-beta .1.2+1", TestName = "Pre-Release: Contains whitespace")]
    [TestCase("1.2.3-abc+@20220101.1", TestName = "Build Metadata: Non alphanumeric characters")]
    [TestCase("1.2.3-abc+20220101..1", TestName = "Build Metadata: Double period")]
    [TestCase("1.2.3-abc+.20220101.1", TestName = "Build Metadata: Starting with a period")]
    [TestCase("1.2.3-abc+20220101.1.2.", TestName = "Build Metadata: Ending with a period")]
    [TestCase("1.2.3-abc+20220101 .1.2", TestName = "Build Metadata: Contains whitespace")]
    [TestCase("1.2.3-abc++20220101.1.2", TestName = "Build Metadata: Starting with a plus")]
    [TestCase("1.2.3.4-abc+123", TestName = "Four numerics")]
    [TestCase("", TestName = "Empty string")]
    [TestCase(null, TestName = "Null")]
    public void Ctor_WithInvalidVersionString_Throws(string inputString)
    {
        // Arrange

        // Act
        void Act() => _ = new SemanticVersion(inputString);

        // Assert
        Should.Throw<ArgumentException>(Act);
    }

    [TestCase((uint)1, (uint)2, (uint)3)]
    [TestCase((uint)0, (uint)0, uint.MaxValue)]
    [TestCase((uint)99, (uint)101, (uint)0)]
    [TestCase((uint)0, (uint)1, (uint)0)]
    public void Ctor_WithJustNumbers_SetsPropertiesCorrectly(uint major, uint minor, uint patch)
    {
        // Arrange

        // Act
        var semanticVersion = new SemanticVersion(major, minor, patch);

        // Assert
        semanticVersion.ShouldNotBeNull();
        semanticVersion.Major.ShouldBe(major);
        semanticVersion.Minor.ShouldBe(minor);
        semanticVersion.Patch.ShouldBe(patch);
        semanticVersion.PreRelease.ShouldBe(string.Empty);
        semanticVersion.BuildMetadata.ShouldBe(string.Empty);
    }

    [TestCase((uint)0, (uint)1, (uint)0, "feature-branch.1", null)]
    public void Ctor_WithNullBuild_ConvertsToEmptyString(uint major, uint minor, uint patch, string preRelease, string build)
    {
        // Arrange

        // Act
        var semanticVersion = new SemanticVersion(major, minor, patch, preRelease, build);

        // Assert
        semanticVersion.ShouldNotBeNull();
        semanticVersion.Major.ShouldBe(major);
        semanticVersion.Minor.ShouldBe(minor);
        semanticVersion.Patch.ShouldBe(patch);
        semanticVersion.PreRelease.ShouldBe(preRelease);
        semanticVersion.BuildMetadata.ShouldBe(string.Empty);
    }

    [TestCase((uint)0, (uint)1, (uint)0, null, "abc134.122")]
    public void Ctor_WithNullPreRelease_ConvertsToEmptyString(uint major, uint minor, uint patch, string preRelease, string build)
    {
        // Arrange

        // Act
        var semanticVersion = new SemanticVersion(major, minor, patch, preRelease, build);

        // Assert
        semanticVersion.ShouldNotBeNull();
        semanticVersion.Major.ShouldBe(major);
        semanticVersion.Minor.ShouldBe(minor);
        semanticVersion.Patch.ShouldBe(patch);
        semanticVersion.PreRelease.ShouldBe(string.Empty);
        semanticVersion.BuildMetadata.ShouldBe(build);
    }

    [TestCase((uint)1, (uint)2, (uint)3, "20220308.01", TestName = "Containing period")]
    [TestCase((uint)0, (uint)0, uint.MaxValue, "1", TestName = "Numeric")]
    [TestCase((uint)99, (uint)101, (uint)0, "", TestName = "Empty string")]
    [TestCase((uint)0, (uint)1, (uint)0, "abc134-122", TestName = "Containing dash")]
    [TestCase((uint)0, (uint)1, (uint)0, "abc134.-122", TestName = "Containing dash and period")]
    public void Ctor_WithNumbersAndBuild_SetsPropertiesCorrectly(uint major, uint minor, uint patch, string build)
    {
        // Arrange

        // Act
        var semanticVersion = new SemanticVersion(major, minor, patch, buildMetadata: build);

        // Assert
        semanticVersion.ShouldNotBeNull();
        semanticVersion.Major.ShouldBe(major);
        semanticVersion.Minor.ShouldBe(minor);
        semanticVersion.Patch.ShouldBe(patch);
        semanticVersion.PreRelease.ShouldBe(string.Empty);
        semanticVersion.BuildMetadata.ShouldBe(build);
    }

    [TestCase((uint)1, (uint)2, (uint)3, "beta.1")]
    [TestCase((uint)0, (uint)0, uint.MaxValue, "some-branch-name")]
    [TestCase((uint)99, (uint)101, (uint)0, "12345-feature-branch.990")]
    [TestCase((uint)0, (uint)1, (uint)0, "some-branch-name")]
    public void Ctor_WithNumbersAndPreRelease_SetsPropertiesCorrectly(uint major, uint minor, uint patch, string preRelease)
    {
        // Arrange

        // Act
        var semanticVersion = new SemanticVersion(major, minor, patch, preRelease);

        // Assert
        semanticVersion.ShouldNotBeNull();
        semanticVersion.Major.ShouldBe(major);
        semanticVersion.Minor.ShouldBe(minor);
        semanticVersion.Patch.ShouldBe(patch);
        semanticVersion.PreRelease.ShouldBe(preRelease);
        semanticVersion.BuildMetadata.ShouldBe(string.Empty);
    }

    [TestCase((uint)1, (uint)2, (uint)3, "beta.1", "20220308.01")]
    [TestCase((uint)0, (uint)0, uint.MaxValue, "some-branch-name", "1")]
    [TestCase((uint)99, (uint)101, (uint)0, "2345-feature-branch.990", "")]
    [TestCase((uint)0, (uint)1, (uint)0, "12345-feature-branch.10001", "abc134-122")]
    [TestCase((uint)0, (uint)1, (uint)0, "prerelease.1234", "abc134.122")]
    public void Ctor_WithNumbersPreReleaseAndBuild_SetsPropertiesCorrectly(uint major, uint minor, uint patch, string preRelease, string build)
    {
        // Arrange

        // Act
        var semanticVersion = new SemanticVersion(major, minor, patch, preRelease, build);

        // Assert
        semanticVersion.ShouldNotBeNull();
        semanticVersion.Major.ShouldBe(major);
        semanticVersion.Minor.ShouldBe(minor);
        semanticVersion.Patch.ShouldBe(patch);
        semanticVersion.PreRelease.ShouldBe(preRelease);
        semanticVersion.BuildMetadata.ShouldBe(build);
    }

    [TestCase("1.2.3-beta.1+20220308.01", (uint)1, (uint)2, (uint)3, "beta.1", "20220308.01")]
    [TestCase("0.0.4294967295-some-branch-name+1", (uint)0, (uint)0, uint.MaxValue, "some-branch-name", "1")]
    [TestCase("99.101.0-12345-feature-branch.990", (uint)99, (uint)101, (uint)0, "12345-feature-branch.990", "")]
    [TestCase("0.1.0-12345-feature-branch.12234+abc134.122.a.4", (uint)0, (uint)1, (uint)0, "12345-feature-branch.12234", "abc134.122.a.4")]
    [TestCase("0.1.0-bigDOG.10001+abc134.122", (uint)0, (uint)1, (uint)0, "bigDOG.10001", "abc134.122")]
    public void Ctor_WithVersionString_CorrectlyParsesProperties(string inputString, uint major, uint minor, uint patch, string preRelease, string build)
    {
        // Arrange

        // Act
        var semanticVersion = new SemanticVersion(inputString);

        // Assert
        semanticVersion.ShouldNotBeNull();
        semanticVersion.Major.ShouldBe(major);
        semanticVersion.Minor.ShouldBe(minor);
        semanticVersion.Patch.ShouldBe(patch);
        semanticVersion.PreRelease.ShouldBe(preRelease);
        semanticVersion.BuildMetadata.ShouldBe(build);
    }

    [TestCase("1.2.3-beta.1+20220308.01", (uint)1, (uint)2, (uint)3, "beta.1", "20220308.01")]
    [TestCase("0.0.4294967295-some-branch-name+1", (uint)0, (uint)0, uint.MaxValue, "some-branch-name", "1")]
    [TestCase("99.101.0-12345-feature-branch.990", (uint)99, (uint)101, (uint)0, "12345-feature-branch.990", "")]
    [TestCase("0.1.0-12345-feature-branch.12234+abc134.122.a.4", (uint)0, (uint)1, (uint)0, "12345-feature-branch.12234", "abc134.122.a.4")]
    [TestCase("0.1.0-bigDOG.10001+abc134.122", (uint)0, (uint)1, (uint)0, "bigDOG.10001", "abc134.122")]
    [TestCase("0.1.0", (uint)0, (uint)1, (uint)0, "", null)]
    [TestCase("77.1.3+20220101.78", (uint)77, (uint)1, (uint)3, "", "20220101.78")]
    [TestCase("77.1.3-beta.1", (uint)77, (uint)1, (uint)3, "beta.1", "")]
    public void Equals_WithMatchingSemvers_ReturnsTrue(string asString, uint major, uint minor, uint patch, string preRelease, string build)
    {
        // Arrange
        var version1 = new SemanticVersion(asString);
        var version2 = new SemanticVersion(major, minor, patch, preRelease, build);

        // Act
        var areEqual = version1.Equals(version2);

        // Assert
        areEqual.ShouldBeTrue();
    }

    [TestCase("1.2.3-beta.1+20220308.01", (uint)2, (uint)2, (uint)3, "beta.1", "20220308.01")]
    [TestCase("0.0.4294967295-some-branch-name+1", (uint)0, (uint)1, uint.MaxValue, "some-branch-name", "1")]
    [TestCase("99.101.0-12345-feature-branch.990", (uint)99, (uint)101, (uint)77, "12345-feature-branch.990", "")]
    [TestCase("0.1.0-12345-feature-branch.12234+abc134.122.a.4", (uint)0, (uint)1, (uint)0, "12345-feature-branch.12235", "abc134.122.a.4")]
    [TestCase("0.1.0-bigDOG.10001+abc134.122", (uint)0, (uint)1, (uint)0, "bigDOG.10001", "abcd134.122")]
    [TestCase("0.1.0", (uint)0, (uint)1, (uint)0, "s", null)]
    [TestCase("77.1.3+20220101.78", (uint)77, (uint)1, (uint)3, "1", "20220101.78")]
    [TestCase("77.1.3-beta.1", (uint)77, (uint)1, (uint)3, "beta.2", "")]
    [TestCase("77.1.3-beta.1", (uint)77, (uint)1, (uint)3, "beta.1", "a")]
    public void Equals_WithNonMatchingSemvers_ReturnsFalse(string asString, uint major, uint minor, uint patch, string preRelease, string build)
    {
        // Arrange
        var version1 = new SemanticVersion(asString);
        var version2 = new SemanticVersion(major, minor, patch, preRelease, build);

        // Act
        var areEqual = version1.Equals(version2);

        // Assert
        areEqual.ShouldBeFalse();
    }

    [Test]
    public void EqualsMethod_WithObjectVersion_ReturnsFalse()
    {
        // Arrange
        var leftHand = new SemanticVersion("1.2.3-88.2.Z");
        object rightHand = new Version(1, 2, 3);

        // Act
        var result = leftHand.Equals(rightHand);

        // Assert
        result.ShouldBe(false);
    }

    [Test]
    public void EqualsMethod_WithSameSemVerAsObject_ReturnsTrue()
    {
        // Arrange
        var leftHand = new SemanticVersion("1.2.3-88.2.Z");
        object rightHand = new SemanticVersion("1.2.3-88.2.Z");

        // Act
        var result = leftHand.Equals(rightHand);

        // Assert
        result.ShouldBe(true);
    }

    [TestCase("1.2.3-88.2.Z", "1.2.3-88.2.99", false, TestName = "Numerics Same, Left Has Alpha Pre-Release, Right Has Numeric Pre-Release")]
    [TestCase("1.2.4-prerelease.2+abc", "1.2.3-prerelease.2+abc", false, TestName = "Numerics Differ, Pre-Release Same, Build Metadata Same")]
    [TestCase("1.2.4-prerelease.2", "1.2.3-prerelease.2", false, TestName = "Numerics Differ, Pre-Release Same, No Build Metadata")]
    [TestCase("1.2.3-prerelease.2+abc", "1.2.3-prerelease.2+abc", true, TestName = "Numerics Same, Pre-Release Same, Build Metadata Same")]
    [TestCase("1.2.3-prerelease.2+abc", "1.2.3-prerelease.2+fff", false, TestName = "Numerics Same, Pre-Release Same, Build Metadata Different")]
    public void EqualsMethod_WithSemanticVersion_ReturnsExpectedResult(string leftHandSide, string rightHandSide, bool expectedResult)
    {
        // Arrange
        var leftHand = new SemanticVersion(leftHandSide);
        var rightHand = new SemanticVersion(rightHandSide);

        // Act
        var result = leftHand.Equals(rightHand);

        // Assert
        result.ShouldBe(expectedResult);
    }

    [Test]
    public void EuqalsOperator_WithNullLeft_Throws()
    {
        // Arrange
        SemanticVersion? leftHand = null;
        var rightHand = new SemanticVersion("1.2.3");

        // Act
        void Act() => _ = leftHand! == rightHand;

        // Assert
        Should.Throw<ArgumentNullException>(Act);
    }

    [Test]
    public void EuqalsOperator_WithNullRight_Throws()
    {
        // Arrange
        var leftHand = new SemanticVersion("1.2.3");
        SemanticVersion? rightHand = null;

        // Act
        void Act() => _ = leftHand == rightHand!;

        // Assert
        Should.Throw<ArgumentNullException>(Act);
    }

    [TestCase(1, 2, 3, 1)]
    [TestCase(1, 2, 3, 97)]
    [TestCase(0, 0, 42949, 0)]
    [TestCase(99, 101, 0, 0)]
    [TestCase(77, 1, 3, 0)]
    public void FromVersion_WithNoFlagSet_ReturnsValidSemVer(int inputMajor, int inputMinor, int inputBuild, int inputRevision)
    {
        // Arrange
        var version = new Version(inputMajor, inputMinor, inputBuild, inputRevision);

        // Act
        var semanticVersion = SemanticVersion.FromVersion(version);

        // Assert
        semanticVersion.ShouldNotBeNull();
        semanticVersion.Major.ShouldBe((uint)inputMajor);
        semanticVersion.Minor.ShouldBe((uint)inputMinor);
        semanticVersion.Patch.ShouldBe((uint)inputBuild);
        semanticVersion.PreRelease.ShouldBeEmpty();
        semanticVersion.BuildMetadata.ShouldBeEmpty();
    }

    [TestCase(1, 2, 3, 1, "beta", "beta.1")]
    [TestCase(1, 2, 3, 97, "orange.1", "orange.1.97")]
    [TestCase(0, 0, 42949, 0, "beta", "")]
    public void FromVersion_WithPreReleaseFlagAndPrefixSet_ReturnsValidSemVer(int inputMajor, int inputMinor, int inputBuild, int inputRevision, string prefix, string expectedPreRelease)
    {
        // Arrange
        var version = new Version(inputMajor, inputMinor, inputBuild, inputRevision);

        // Act
        var semanticVersion = SemanticVersion.FromVersion(version, true, prefix);

        // Assert
        semanticVersion.ShouldNotBeNull();
        semanticVersion.Major.ShouldBe((uint)inputMajor);
        semanticVersion.Minor.ShouldBe((uint)inputMinor);
        semanticVersion.Patch.ShouldBe((uint)inputBuild);
        semanticVersion.PreRelease.ShouldBe(expectedPreRelease);
        semanticVersion.BuildMetadata.ShouldBeEmpty();
    }

    [TestCase(1, 2, 3, 1, "beta.1")]
    [TestCase(1, 2, 3, 97, "beta.97")]
    [TestCase(0, 0, 42949, 0, "")]
    public void FromVersion_WithPreReleaseFlagSet_ReturnsValidSemVer(int inputMajor, int inputMinor, int inputBuild, int inputRevision, string expectedPreRelease)
    {
        // Arrange
        var version = new Version(inputMajor, inputMinor, inputBuild, inputRevision);

        // Act
        var semanticVersion = SemanticVersion.FromVersion(version, true);

        // Assert
        semanticVersion.ShouldNotBeNull();
        semanticVersion.Major.ShouldBe((uint)inputMajor);
        semanticVersion.Minor.ShouldBe((uint)inputMinor);
        semanticVersion.Patch.ShouldBe((uint)inputBuild);
        semanticVersion.PreRelease.ShouldBe(expectedPreRelease);
        semanticVersion.BuildMetadata.ShouldBeEmpty();
    }

    [TestCase("0.0.1")]
    [TestCase("0.0.2")]
    [TestCase("0.1.0")]
    [TestCase("0.2.0")]
    [TestCase("1.0.0")]
    [TestCase("1.2.3")]
    [TestCase("1.2.3-beta.1")]
    [TestCase("1.2.3-beta.1.1")]
    [TestCase("1.2.3-88")]
    [TestCase("1.2.3-88.1")]
    [TestCase("1.2.3-88.2.A")]
    [TestCase("1.2.3-88.2.99")]
    [TestCase("1.2.3")]
    [TestCase("1.2.3-beta.1")]
    [TestCase("1.2.3-beta.1.2")]
    [TestCase("1.2.3+2022.12.01")]
    [TestCase("1.2.3+2022.12.01")]
    [TestCase("1.2.3-beta.1+2022.12.01")]
    [TestCase("1.2.3-beta.1+2022.12.01")]
    [TestCase("2.0.0")]
    [TestCase("99.101.0-12345-feature-branch.990")]
    public void GetHashCode_WithValidSemanticVersion_ReturnsInteger(string versionString)
    {
        // Arrange
        var semanticVersion = new SemanticVersion(versionString);

        // Act
        var result = semanticVersion.GetHashCode();

        // Assert
        result.ShouldNotBe(0);
    }

    [Test]
    public void GreaterThanOperator_WithNullLeft_Throws()
    {
        // Arrange
        SemanticVersion? leftHand = null;
        var rightHand = new SemanticVersion("1.2.3");

        // Act
        void Act() => _ = leftHand! > rightHand;

        // Assert
        Should.Throw<ArgumentNullException>(Act);
    }

    [Test]
    public void GreaterThanOperator_WithNullRight_Throws()
    {
        // Arrange
        var leftHand = new SemanticVersion("1.2.3");
        SemanticVersion? rightHand = null;

        // Act
        void Act() => _ = leftHand > rightHand!;

        // Assert
        Should.Throw<ArgumentNullException>(Act);
    }

    [TestCase("0.0.1", "0.0.2", false, TestName = "Left Patch < Right Patch")]
    [TestCase("0.1.0", "0.2.0", false, TestName = "Left Minor < Right Minor")]
    [TestCase("1.0.0", "2.0.0", false, TestName = "Left Major < Right Major")]
    [TestCase("1.2.3-beta.1", "1.2.3", false, TestName = "Numerics Same, Left Has Pre-Release, Right Has No Pre-Release")]
    [TestCase("1.2.3-beta.1.1", "1.2.3-beta.1", false, TestName = "Numerics Same, Left Has More Pre-Release Parts")]
    [TestCase("1.2.3-88", "1.2.3-89", false, TestName = "Numerics Same, Left Has Lower Numerical Pre-Release")]
    [TestCase("1.2.3-88.1", "1.2.3-88.2", false, TestName = "Numerics Same, Left Has Lower Numerical Pre-Release On Last Part")]
    [TestCase("1.2.3-88.2.A", "1.2.3-88.2.a", false, TestName = "Numerics Same, Left Has Lower Pre-Release Lexically in ASCII Sort Order")]
    [TestCase("1.2.3-88.2.99", "1.2.3-88.2.Z", false, TestName = "Numerics Same, Left Has Numeric Pre-Release, Right Has Alpha Pre-Release")]
    [TestCase("1.2.3", "1.2.3", false, TestName = "Numerics Same, No Pre-Release")]
    [TestCase("1.2.3-beta.1", "1.2.3-beta.1", false, TestName = "Numerics Same, Pre-Release Same")]
    [TestCase("1.2.3-beta.1.2", "1.2.3-beta.1.2", false, TestName = "Numerics Same, Pre-Release Same, Multiple Pre-Release Parts")]
    [TestCase("1.2.3+2022.12.01", "1.2.3+2022.12.01", false, TestName = "Numerics Same, No Pre-Release, Build Metadata Same")]
    [TestCase("1.2.3+2022.12.01", "1.2.3+2022.12.11", false, TestName = "Numerics Same, No Pre-Release, Build Metadata Different")]
    [TestCase("1.2.3-beta.1+2022.12.01", "1.2.3-beta.1+2022.12.01", false, TestName = "Numerics Same, Pre-Release Same, Build Metadata Same")]
    [TestCase("1.2.3-beta.1+2022.12.01", "1.2.3-beta.1+2022.11.01", false, TestName = "Numerics Same, Pre-Release Same, Build Metadata Different")]
    [TestCase("0.0.2", "0.0.1", true, TestName = "Left Patch > Right Patch")]
    [TestCase("0.2.0", "0.1.0", true, TestName = "Left Minor > Right Minor")]
    [TestCase("2.0.0", "1.0.0", true, TestName = "Left Major > Right Major")]
    [TestCase("1.2.3", "1.2.3-beta.1", true, TestName = "Numerics Same, Left Has No Pre-Release, Right Has Pre-Release")]
    [TestCase("1.2.3-beta.1", "1.2.3-beta.1.1", true, TestName = "Numerics Same, Left Has Less Pre-Release Parts")]
    [TestCase("1.2.3-89", "1.2.3-88", true, TestName = "Numerics Same, Left Has Higher Numerical Pre-Release")]
    [TestCase("1.2.3-88.2", "1.2.3-88.1", true, TestName = "Numerics Same, Left Has Higher Numerical Pre-Release On Last Part")]
    [TestCase("1.2.3-88.2.a", "1.2.3-88.2.A", true, TestName = "Numerics Same, Left Has Higher Pre-Release Lexically in ASCII Sort Order")]
    [TestCase("1.2.3-88.2.Z", "1.2.3-88.2.99", true, TestName = "Numerics Same, Left Has Alpha Pre-Release, Right Has Numeric Pre-Release")]
    public void GreaterThanOperator_WithOtherAsSemanticVersion_ReturnsCorrectOutput(string leftHandSide, string rightHandSide, bool expectedResult)
    {
        // Arrange
        var leftHand = new SemanticVersion(leftHandSide);
        var rightHand = new SemanticVersion(rightHandSide);

        // Act
        var result = leftHand > rightHand;

        // Assert
        result.ShouldBe(expectedResult);
    }

    [TestCase("0.0.1", "0.0.2", false, TestName = "Left Patch < Right Patch")]
    [TestCase("0.1.0", "0.2.0", false, TestName = "Left Minor < Right Minor")]
    [TestCase("1.0.0", "2.0.0", false, TestName = "Left Major < Right Major")]
    [TestCase("1.2.3-beta.1", "1.2.3", false, TestName = "Numerics Same, Left Has Pre-Release, Right Has No Pre-Release")]
    [TestCase("1.2.3-beta.1.1", "1.2.3-beta.1", false, TestName = "Numerics Same, Left Has More Pre-Release Parts")]
    [TestCase("1.2.3-88", "1.2.3-89", false, TestName = "Numerics Same, Left Has Lower Numerical Pre-Release")]
    [TestCase("1.2.3-88.1", "1.2.3-88.2", false, TestName = "Numerics Same, Left Has Lower Numerical Pre-Release On Last Part")]
    [TestCase("1.2.3-88.2.A", "1.2.3-88.2.a", false, TestName = "Numerics Same, Left Has Lower Pre-Release Lexically in ASCII Sort Order")]
    [TestCase("1.2.3-88.2.99", "1.2.3-88.2.Z", false, TestName = "Numerics Same, Left Has Numeric Pre-Release, Right Has Alpha Pre-Release")]
    [TestCase("1.2.3", "1.2.3", true, TestName = "Numerics Same, No Pre-Release")]
    [TestCase("1.2.3-beta.1", "1.2.3-beta.1", true, TestName = "Numerics Same, Pre-Release Same")]
    [TestCase("1.2.3-beta.1.2", "1.2.3-beta.1.2", true, TestName = "Numerics Same, Pre-Release Same, Multiple Pre-Release Parts")]
    [TestCase("1.2.3+2022.12.01", "1.2.3+2022.12.01", true, TestName = "Numerics Same, No Pre-Release, Build Metadata Same")]
    [TestCase("1.2.3+2022.12.01", "1.2.3+2022.12.11", true, TestName = "Numerics Same, No Pre-Release, Build Metadata Different")]
    [TestCase("1.2.3-beta.1+2022.12.01", "1.2.3-beta.1+2022.12.01", true, TestName = "Numerics Same, Pre-Release Same, Build Metadata Same")]
    [TestCase("1.2.3-beta.1+2022.12.01", "1.2.3-beta.1+2022.11.01", true, TestName = "Numerics Same, Pre-Release Same, Build Metadata Different")]
    [TestCase("0.0.2", "0.0.1", true, TestName = "Left Patch > Right Patch")]
    [TestCase("0.2.0", "0.1.0", true, TestName = "Left Minor > Right Minor")]
    [TestCase("2.0.0", "1.0.0", true, TestName = "Left Major > Right Major")]
    [TestCase("1.2.3", "1.2.3-beta.1", true, TestName = "Numerics Same, Left Has No Pre-Release, Right Has Pre-Release")]
    [TestCase("1.2.3-beta.1", "1.2.3-beta.1.1", true, TestName = "Numerics Same, Left Has Less Pre-Release Parts")]
    [TestCase("1.2.3-89", "1.2.3-88", true, TestName = "Numerics Same, Left Has Higher Numerical Pre-Release")]
    [TestCase("1.2.3-88.2", "1.2.3-88.1", true, TestName = "Numerics Same, Left Has Higher Numerical Pre-Release On Last Part")]
    [TestCase("1.2.3-88.2.a", "1.2.3-88.2.A", true, TestName = "Numerics Same, Left Has Higher Pre-Release Lexically in ASCII Sort Order")]
    [TestCase("1.2.3-88.2.Z", "1.2.3-88.2.99", true, TestName = "Numerics Same, Left Has Alpha Pre-Release, Right Has Numeric Pre-Release")]
    public void GreaterThanOrEqualOperator_WithOtherAsSemanticVersion_ReturnsCorrectOutput(string leftHandSide, string rightHandSide, bool expectedResult)
    {
        // Arrange
        var leftHand = new SemanticVersion(leftHandSide);
        var rightHand = new SemanticVersion(rightHandSide);

        // Act
        var result = leftHand >= rightHand;

        // Assert
        result.ShouldBe(expectedResult);
    }

    [TestCase("1.2.3-beta.1", true)]
    [TestCase("0.0.4294967295-some-branch-name+1", true)]
    [TestCase("99.101.0", false)]
    [TestCase("77.1.3+20220101.78", false)]
    public void IsPrerelease_WhenCalled_ReturnsCorrectFlag(string inputString, bool expectedIsPreRelease)
    {
        // Arrange
        var semanticVersion = new SemanticVersion(inputString);

        // Act
        var actualIsPreRelease = semanticVersion.IsPreRelease;

        // Assert
        actualIsPreRelease.ShouldBe(expectedIsPreRelease);
    }

    [Test]
    public void LessThanOperator_WithNullLeft_Throws()
    {
        // Arrange
        SemanticVersion? leftHand = null;
        var rightHand = new SemanticVersion("1.2.3");

        // Act
        void Act() => _ = leftHand! < rightHand;

        // Assert
        Should.Throw<ArgumentNullException>(Act);
    }

    [Test]
    public void LessThanOperator_WithNullRight_Throws()
    {
        // Arrange
        var leftHand = new SemanticVersion("1.2.3");
        SemanticVersion? rightHand = null;

        // Act
        void Act() => _ = leftHand < rightHand!;

        // Assert
        Should.Throw<ArgumentNullException>(Act);
    }

    [TestCase("0.0.1", "0.0.2", true, TestName = "Left Patch < Right Patch")]
    [TestCase("0.1.0", "0.2.0", true, TestName = "Left Minor < Right Minor")]
    [TestCase("1.0.0", "2.0.0", true, TestName = "Left Major < Right Major")]
    [TestCase("1.2.3-beta.1", "1.2.3", true, TestName = "Numerics Same, Left Has Pre-Release, Right Has No Pre-Release")]
    [TestCase("1.2.3-beta.1.1", "1.2.3-beta.1", true, TestName = "Numerics Same, Left Has More Pre-Release Parts")]
    [TestCase("1.2.3-88", "1.2.3-89", true, TestName = "Numerics Same, Left Has Lower Numerical Pre-Release")]
    [TestCase("1.2.3-88.1", "1.2.3-88.2", true, TestName = "Numerics Same, Left Has Lower Numerical Pre-Release On Last Part")]
    [TestCase("1.2.3-88.2.A", "1.2.3-88.2.a", true, TestName = "Numerics Same, Left Has Lower Pre-Release Lexically in ASCII Sort Order")]
    [TestCase("1.2.3-88.2.99", "1.2.3-88.2.Z", true, TestName = "Numerics Same, Left Has Numeric Pre-Release, Right Has Alpha Pre-Release")]
    [TestCase("1.2.3", "1.2.3", false, TestName = "Numerics Same, No Pre-Release")]
    [TestCase("1.2.3-beta.1", "1.2.3-beta.1", false, TestName = "Numerics Same, Pre-Release Same")]
    [TestCase("1.2.3-beta.1.2", "1.2.3-beta.1.2", false, TestName = "Numerics Same, Pre-Release Same, Multiple Pre-Release Parts")]
    [TestCase("1.2.3+2022.12.01", "1.2.3+2022.12.01", false, TestName = "Numerics Same, No Pre-Release, Build Metadata Same")]
    [TestCase("1.2.3+2022.12.01", "1.2.3+2022.12.11", false, TestName = "Numerics Same, No Pre-Release, Build Metadata Different")]
    [TestCase("1.2.3-beta.1+2022.12.01", "1.2.3-beta.1+2022.12.01", false, TestName = "Numerics Same, Pre-Release Same, Build Metadata Same")]
    [TestCase("1.2.3-beta.1+2022.12.01", "1.2.3-beta.1+2022.11.01", false, TestName = "Numerics Same, Pre-Release Same, Build Metadata Different")]
    [TestCase("0.0.2", "0.0.1", false, TestName = "Left Patch > Right Patch")]
    [TestCase("0.2.0", "0.1.0", false, TestName = "Left Minor > Right Minor")]
    [TestCase("2.0.0", "1.0.0", false, TestName = "Left Major > Right Major")]
    [TestCase("1.2.3", "1.2.3-beta.1", false, TestName = "Numerics Same, Left Has No Pre-Release, Right Has Pre-Release")]
    [TestCase("1.2.3-beta.1", "1.2.3-beta.1.1", false, TestName = "Numerics Same, Left Has Less Pre-Release Parts")]
    [TestCase("1.2.3-89", "1.2.3-88", false, TestName = "Numerics Same, Left Has Higher Numerical Pre-Release")]
    [TestCase("1.2.3-88.2", "1.2.3-88.1", false, TestName = "Numerics Same, Left Has Higher Numerical Pre-Release On Last Part")]
    [TestCase("1.2.3-88.2.a", "1.2.3-88.2.A", false, TestName = "Numerics Same, Left Has Higher Pre-Release Lexically in ASCII Sort Order")]
    [TestCase("1.2.3-88.2.Z", "1.2.3-88.2.99", false, TestName = "Numerics Same, Left Has Alpha Pre-Release, Right Has Numeric Pre-Release")]
    public void LessThanOperator_WithOtherAsSemanticVersion_ReturnsCorrectOutput(string leftHandSide, string rightHandSide, bool expectedResult)
    {
        // Arrange
        var leftHand = new SemanticVersion(leftHandSide);
        var rightHand = new SemanticVersion(rightHandSide);

        // Act
        var result = leftHand < rightHand;

        // Assert
        result.ShouldBe(expectedResult);
    }

    [TestCase("0.0.1", "0.0.2", true, TestName = "Left Patch < Right Patch")]
    [TestCase("0.1.0", "0.2.0", true, TestName = "Left Minor < Right Minor")]
    [TestCase("1.0.0", "2.0.0", true, TestName = "Left Major < Right Major")]
    [TestCase("1.2.3-beta.1", "1.2.3", true, TestName = "Numerics Same, Left Has Pre-Release, Right Has No Pre-Release")]
    [TestCase("1.2.3-beta.1.1", "1.2.3-beta.1", true, TestName = "Numerics Same, Left Has More Pre-Release Parts")]
    [TestCase("1.2.3-88", "1.2.3-89", true, TestName = "Numerics Same, Left Has Lower Numerical Pre-Release")]
    [TestCase("1.2.3-88.1", "1.2.3-88.2", true, TestName = "Numerics Same, Left Has Lower Numerical Pre-Release On Last Part")]
    [TestCase("1.2.3-88.2.A", "1.2.3-88.2.a", true, TestName = "Numerics Same, Left Has Lower Pre-Release Lexically in ASCII Sort Order")]
    [TestCase("1.2.3-88.2.99", "1.2.3-88.2.Z", true, TestName = "Numerics Same, Left Has Numeric Pre-Release, Right Has Alpha Pre-Release")]
    [TestCase("1.2.3", "1.2.3", true, TestName = "Numerics Same, No Pre-Release")]
    [TestCase("1.2.3-beta.1", "1.2.3-beta.1", true, TestName = "Numerics Same, Pre-Release Same")]
    [TestCase("1.2.3-beta.1.2", "1.2.3-beta.1.2", true, TestName = "Numerics Same, Pre-Release Same, Multiple Pre-Release Parts")]
    [TestCase("1.2.3+2022.12.01", "1.2.3+2022.12.01", true, TestName = "Numerics Same, No Pre-Release, Build Metadata Same")]
    [TestCase("1.2.3+2022.12.01", "1.2.3+2022.12.11", true, TestName = "Numerics Same, No Pre-Release, Build Metadata Different")]
    [TestCase("1.2.3-beta.1+2022.12.01", "1.2.3-beta.1+2022.12.01", true, TestName = "Numerics Same, Pre-Release Same, Build Metadata Same")]
    [TestCase("1.2.3-beta.1+2022.12.01", "1.2.3-beta.1+2022.11.01", true, TestName = "Numerics Same, Pre-Release Same, Build Metadata Different")]
    [TestCase("0.0.2", "0.0.1", false, TestName = "Left Patch > Right Patch")]
    [TestCase("0.2.0", "0.1.0", false, TestName = "Left Minor > Right Minor")]
    [TestCase("2.0.0", "1.0.0", false, TestName = "Left Major > Right Major")]
    [TestCase("1.2.3", "1.2.3-beta.1", false, TestName = "Numerics Same, Left Has No Pre-Release, Right Has Pre-Release")]
    [TestCase("1.2.3-beta.1", "1.2.3-beta.1.1", false, TestName = "Numerics Same, Left Has Less Pre-Release Parts")]
    [TestCase("1.2.3-89", "1.2.3-88", false, TestName = "Numerics Same, Left Has Higher Numerical Pre-Release")]
    [TestCase("1.2.3-88.2", "1.2.3-88.1", false, TestName = "Numerics Same, Left Has Higher Numerical Pre-Release On Last Part")]
    [TestCase("1.2.3-88.2.a", "1.2.3-88.2.A", false, TestName = "Numerics Same, Left Has Higher Pre-Release Lexically in ASCII Sort Order")]
    [TestCase("1.2.3-88.2.Z", "1.2.3-88.2.99", false, TestName = "Numerics Same, Left Has Alpha Pre-Release, Right Has Numeric Pre-Release")]
    public void LessThanOrEqualOperator_WithOtherAsSemanticVersion_ReturnsCorrectOutput(string leftHandSide, string rightHandSide, bool expectedResult)
    {
        // Arrange
        var leftHand = new SemanticVersion(leftHandSide);
        var rightHand = new SemanticVersion(rightHandSide);

        // Act
        var result = leftHand <= rightHand;

        // Assert
        result.ShouldBe(expectedResult);
    }

    [TestCase("1.2.3-beta.1+20220308.01", (uint)2, (uint)2, (uint)3, "beta.1", "20220308.01")]
    [TestCase("0.0.4294967295-some-branch-name+1", (uint)0, (uint)1, uint.MaxValue, "some-branch-name", "1")]
    [TestCase("99.101.0-12345-feature-branch.990", (uint)99, (uint)101, (uint)77, "12345-feature-branch.990", "")]
    [TestCase("0.1.0-12345-feature-branch.12234+abc134.122.a.4", (uint)0, (uint)1, (uint)0, "12345-feature-branch.12235", "abc134.122.a.4")]
    [TestCase("0.1.0-bigDOG.10001.1+abc134.122", (uint)0, (uint)1, (uint)0, "bigDOG.10001", "abcd134.122")]
    [TestCase("0.1.0", (uint)0, (uint)1, (uint)0, "s", null)]
    [TestCase("77.1.3+20220101.78", (uint)77, (uint)1, (uint)3, "1", "20220101.78")]
    [TestCase("77.1.3-beta.1", (uint)77, (uint)1, (uint)3, "beta.2", "")]
    [TestCase("77.1.3-beta.1.a", (uint)77, (uint)1, (uint)3, "beta.1", "a")]
    public void OperatorEqual_WithNonMatchingSemvers_ReturnsFalse(string asString, uint major, uint minor, uint patch, string preRelease, string build)
    {
        // Arrange
        var version1 = new SemanticVersion(asString);
        var version2 = new SemanticVersion(major, minor, patch, preRelease, build);

        // Act
        var areEqual = version1 == version2;

        // Assert
        areEqual.ShouldBeFalse();
    }

    [TestCase("1.2.3-beta.1+20220308.01", (uint)2, (uint)2, (uint)3, "beta.1", "20220308.01")]
    [TestCase("0.0.4294967295-some-branch-name+1", (uint)0, (uint)1, uint.MaxValue, "some-branch-name", "1")]
    [TestCase("99.101.0-12345-feature-branch.990", (uint)99, (uint)101, (uint)77, "12345-feature-branch.990", "")]
    [TestCase("0.1.0-12345-feature-branch.12234+abc134.122.a.4", (uint)0, (uint)1, (uint)0, "12345-feature-branch.12235", "abc134.122.a.4")]
    [TestCase("0.1.0-bigDOG.10002+abc134.122", (uint)0, (uint)1, (uint)0, "bigDOG.10001", "abcd134.122")]
    [TestCase("0.1.0", (uint)0, (uint)1, (uint)0, "s", null)]
    [TestCase("77.1.3+20220101.78", (uint)77, (uint)1, (uint)3, "1", "20220101.78")]
    [TestCase("77.1.3-beta.1", (uint)77, (uint)1, (uint)3, "beta.2", "")]
    [TestCase("77.1.3-beta.1.a", (uint)77, (uint)1, (uint)3, "beta.1", "a")]
    public void OperatorNotEqual_WithNonMatchingSemvers_ReturnsTrue(string asString, uint major, uint minor, uint patch, string preRelease, string build)
    {
        // Arrange
        var version1 = new SemanticVersion(asString);
        var version2 = new SemanticVersion(major, minor, patch, preRelease, build);

        // Act
        var areEqual = version1 != version2;

        // Assert
        areEqual.ShouldBeTrue();
    }

    [TestCase("1.2.3-beta.1+20220308.01")]
    [TestCase("0.0.4294967295-some-branch-name+1")]
    [TestCase("99.101.0-12345-feature-branch.990")]
    [TestCase("0.1.0-12345-feature-branch.12234+abc134.122.a.4")]
    [TestCase("0.1.0-bigDOG.10001+abc134.122")]
    [TestCase("0.1.0")]
    [TestCase("77.1.3+20220101.78")]
    [TestCase("77.1.3-beta.1")]
    public void ToString_OnParsingTheSameString_ProducesEqualOutput(string inputString)
    {
        // Arrange
        var semanticVersion = new SemanticVersion(inputString);

        // Act
        var outputString = semanticVersion.ToString();

        // Assert
        outputString.ShouldNotBeNullOrWhiteSpace();
        outputString.ShouldBe(inputString);
    }

    [TestCase("1.2.3-beta.1+20220308.01", (uint)1, (uint)2, (uint)3, "beta.1", "20220308.01")]
    [TestCase("0.0.4294967295-some-branch-name+1", (uint)0, (uint)0, uint.MaxValue, "some-branch-name", "1")]
    [TestCase("99.101.0-12345-feature-branch.990", (uint)99, (uint)101, (uint)0, "12345-feature-branch.990", "")]
    [TestCase("0.1.0-12345-feature-branch.12234+abc134.122.a.4", (uint)0, (uint)1, (uint)0, "12345-feature-branch.12234", "abc134.122.a.4")]
    [TestCase("0.1.0-bigDOG.10001+abc134.122", (uint)0, (uint)1, (uint)0, "bigDOG.10001", "abc134.122")]
    [TestCase("0.1.0", (uint)0, (uint)1, (uint)0, "", null)]
    [TestCase("77.1.3+20220101.78", (uint)77, (uint)1, (uint)3, "", "20220101.78")]
    [TestCase("77.1.3-beta.1", (uint)77, (uint)1, (uint)3, "beta.1", "")]
    public void ToString_WithValidState_ProducesCorrectString(string expectedOutput, uint major, uint minor, uint patch, string preRelease, string build)
    {
        // Arrange
        var semanticVersion = new SemanticVersion(major, minor, patch, preRelease, build);

        // Act
        var actualOutput = semanticVersion.ToString();

        // Assert
        actualOutput.ShouldNotBeNullOrWhiteSpace();
        actualOutput.ShouldBe(expectedOutput);
    }

    [TestCase("4294967295.2.3", "Major")]
    [TestCase("1.4294967295.2", "Minor")]
    [TestCase("1.2.4294967295", "Patch")]
    [TestCase("1.2.3-beta.4294967295", "Pre-release")]
    public void ToVersion_WhenCalledWithUncastableInts_Throws(string inputString, string expectedInExceptionMessage)
    {
        // Arrange
        var semanticVersion = new SemanticVersion(inputString);

        // Act
        void Act() => _ = semanticVersion.ToVersion(true);

        // Assert
        var exception = Should.Throw<InvalidCastException>(Act);
        exception.Message.Contains(expectedInExceptionMessage).ShouldBeTrue();
    }

    [TestCase("1.2.3-beta.1", 1, 2, 3)]
    [TestCase("0.0.42949-some-branch-name+1", 0, 0, 42949)]
    [TestCase("99.101.0", 99, 101, 0)]
    [TestCase("77.1.3+20220101.78", 77, 1, 3)]
    public void ToVersion_WithNoOptions_ReturnsMajorMinorPatch(string inputString, int expectedMajor, int expectedMinor, int expectedBuild)
    {
        // Arrange
        var semanticVersion = new SemanticVersion(inputString);

        // Act
        var outputVersion = semanticVersion.ToVersion();

        // Assert
        outputVersion.ShouldNotBeNull();
        outputVersion.Major.ShouldBe(expectedMajor);
        outputVersion.Major.ShouldBe((int)semanticVersion.Major);
        outputVersion.Minor.ShouldBe(expectedMinor);
        outputVersion.Minor.ShouldBe((int)semanticVersion.Minor);
        outputVersion.Build.ShouldBe(expectedBuild);
        outputVersion.Build.ShouldBe((int)semanticVersion.Patch);
        outputVersion.Revision.ShouldBe(0);
    }

    [TestCase("1.2.3-beta.1", 1, 2, 3, 1)]
    [TestCase("1.2.3-beta.97", 1, 2, 3, 97)]
    [TestCase("0.0.42949-some-branch-name+1", 0, 0, 42949, 0)]
    [TestCase("99.101.0", 99, 101, 0, 0)]
    [TestCase("77.1.3+20220101.78", 77, 1, 3, 0)]
    public void ToVersion_WithPreReleaseConversionFlagSet_ReturnsMajorMinorPatchRevision(string inputString, int expectedMajor, int expectedMinor, int expectedBuild, int expectedRevision)
    {
        // Arrange
        var semanticVersion = new SemanticVersion(inputString);

        // Act
        var outputVersion = semanticVersion.ToVersion(true);

        // Assert
        outputVersion.ShouldNotBeNull();
        outputVersion.Major.ShouldBe(expectedMajor);
        outputVersion.Major.ShouldBe((int)semanticVersion.Major);
        outputVersion.Minor.ShouldBe(expectedMinor);
        outputVersion.Minor.ShouldBe((int)semanticVersion.Minor);
        outputVersion.Build.ShouldBe(expectedBuild);
        outputVersion.Build.ShouldBe((int)semanticVersion.Patch);
        outputVersion.Revision.ShouldBe(expectedRevision);
    }

    [TestCase(null)]
    [TestCase("")]
    public void TryParse_WithEmptyOrNullString_ReturnsFalse(string input)
    {
        // Arrange

        // Act
        var result = SemanticVersion.TryParse(input, out var output);

        // Assert
        result.ShouldBe(false);
        output.ShouldBeNull();
    }
}
