using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Silvertop.SemVer;

/// <summary>
///     A model of a SemVer that strictly supports semantic versioning as described at http://semver.org.
/// </summary>
[Serializable]
public sealed class SemanticVersion : IComparable, IComparable<SemanticVersion>, IEquatable<SemanticVersion>
{
    private const int EqualPrecendence = 0;
    private const int ThisIsGreaterThanOther = 1;
    private const int ThisIsLessThanOther = -1;

    private const char NumericSeperator = '.';
    private const char PreReleasePrefix = '-';
    private const char BuildMetadataPrefix = '+';

    private const RegexOptions RegexFlags = RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture;

    // https://semver.org/#is-there-a-suggested-regular-expression-regex-to-check-a-semver-string
    private static readonly Regex OfficialRegexWithGroups = new(
        @"^(?<major>0|[1-9]\d*)\.(?<minor>0|[1-9]\d*)\.(?<patch>0|[1-9]\d*)(?:-(?<prerelease>(?:0|[1-9]\d*|\d*[a-zA-Z-][0-9a-zA-Z-]*)(?:\.(?:0|[1-9]\d*|\d*[a-zA-Z-][0-9a-zA-Z-]*))*))?(?:\+(?<buildmetadata>[0-9a-zA-Z-]+(?:\.[0-9a-zA-Z-]+)*))?$",
        RegexFlags);

    private static readonly Regex PreReleaseOnlyRegex = new(@"^(?<prerelease>(?:0|[1-9]\d*|\d*[a-zA-Z-][0-9a-zA-Z-]*)(?:\.(?:0|[1-9]\d*|\d*[a-zA-Z-][0-9a-zA-Z-]*))*)$", RegexFlags);

    private static readonly Regex BuildMetadataOnlyRegex = new(@"^(?<buildmetadata>(?!\+)[0-9a-zA-Z-]+(?:\.[0-9a-zA-Z-]+)*)$", RegexFlags);

    /// <summary>Initializes a new instance of the <see cref="SemanticVersion" /> class.</summary>
    /// <param name="versionString">A string representation of the semantic version.</param>
    public SemanticVersion(string versionString)
    {
        var version = Parse(versionString);
        Major = version.Major;
        Minor = version.Minor;
        Patch = version.Patch;
        PreRelease = version.PreRelease;
        BuildMetadata = version.BuildMetadata;
    }

    /// <summary>Initializes a new instance of the <see cref="SemanticVersion" /> class.</summary>
    /// <param name="major">The major version number.</param>
    /// <param name="minor">The minor version number.</param>
    /// <param name="patch">The patch version number.</param>
    /// <param name="preRelease">A pre-release string</param>
    /// <param name="buildMetadata">Additional build metadata</param>
    public SemanticVersion(uint major, uint minor, uint patch = 0, string? preRelease = null, string? buildMetadata = null)
    {
        Major = major;
        Minor = minor;
        Patch = patch;

        if (string.IsNullOrWhiteSpace(preRelease))
        {
            PreRelease = string.Empty;
        }
        else
        {
            var match = PreReleaseOnlyRegex.Match(preRelease.Trim());
            if (!match.Success)
            {
                throw new ArgumentException($"The string '{preRelease}' is not a valid pre-release string");
            }

            PreRelease = preRelease.Trim();
        }

        if (string.IsNullOrWhiteSpace(buildMetadata))
        {
            BuildMetadata = string.Empty;
        }
        else
        {
            var match = BuildMetadataOnlyRegex.Match(buildMetadata.Trim());
            if (!match.Success)
            {
                throw new ArgumentException($"The string '{buildMetadata}' is not a valid build metadata string");
            }

            BuildMetadata = buildMetadata.Trim();
        }
    }

    /// <summary>Gets the buildMetadata string.</summary>
    /// <value>The buildMetadata string.</value>
    public string BuildMetadata { get; }

    /// <summary>Gets a value indicating if this represents a pre-release version.</summary>
    /// <value>A value indicating if this represents a pre-release version.</value>
    public bool IsPreRelease => !string.IsNullOrWhiteSpace(PreRelease);

    /// <summary>Gets the major version number.</summary>
    /// <value>The major version number.</value>
    public uint Major { get; }

    /// <summary>Gets the minor version number.</summary>
    /// <value>The minor version number.</value>
    public uint Minor { get; }

    /// <summary>Gets the patch version number.</summary>
    /// <value>The patch version number.</value>
    public uint Patch { get; }

    /// <summary>Gets the pre-release string.</summary>
    /// <value>The pre-release string.</value>
    public string PreRelease { get; }

    /// <inheritdoc />
    public int CompareTo(object? obj)
    {
        if (obj is null)
        {
            return ThisIsGreaterThanOther;
        }

        if (obj is not SemanticVersion other)
        {
            throw new ArgumentException($"Supplied object is not a {nameof(SemanticVersion)}", nameof(obj));
        }

        return CompareTo(other);
    }

    /// <inheritdoc />
    public int CompareTo(SemanticVersion? other)
    {
        if (other is null)
        {
            return ThisIsGreaterThanOther;
        }

        var numericComparisonResult = CompareNumericParts(other);
        return numericComparisonResult != 0 ? numericComparisonResult : ComparePreReleaseString(other.PreRelease);
    }

    /// <inheritdoc />
    public bool Equals(SemanticVersion? other)
    {
        return other is not null &&
            Major == other.Major &&
            Minor == other.Minor &&
            Patch == other.Patch &&
            PreRelease.Equals(other.PreRelease, StringComparison.Ordinal) &&
            BuildMetadata.Equals(other.BuildMetadata, StringComparison.Ordinal);
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return obj is SemanticVersion semVer && Equals(semVer);
    }

    /// <summary>Converts the given System.Version into a SemanticVersion.</summary>
    /// <param name="version">The version.</param>
    /// <param name="treatRevisionAsPreReleaseNumber">
    ///     If true and a revision number is set then the revision number will be
    ///     treated as the pre-release version. Default is false.
    /// </param>
    /// <param name="preReleasePrefix">
    ///     If treatRevisionAsPreReleaseNumber is true, and a revision number is set, pre-release
    ///     string will start with this prefix. Default is 'beta'.
    /// </param>
    /// <returns>A semantic version</returns>
    public static SemanticVersion FromVersion(Version version, bool treatRevisionAsPreReleaseNumber = false, string preReleasePrefix = "beta")
    {
        ArgumentNullException.ThrowIfNull(version, nameof(version));
        if (version.Revision > 0 && treatRevisionAsPreReleaseNumber)
        {
            return new SemanticVersion((uint)version.Major, (uint)version.Minor, (uint)version.Build, $"{preReleasePrefix}.{version.Revision}");
        }

        return new SemanticVersion((uint)version.Major, (uint)version.Minor, (uint)version.Build);
    }

    /// <summary>Returns a hash code for this instance.</summary>
    /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
    public override int GetHashCode()
    {
        var hashCode = (int)(Major * 4588 + Minor * 4 + Patch * 775 + PreRelease.GetHashCode() + BuildMetadata.GetHashCode());

        return hashCode;
    }

    /// <summary>
    /// </summary>
    /// <param name="version1"></param>
    /// <param name="version2"></param>
    /// <returns></returns>
    public static bool operator ==(SemanticVersion? version1, SemanticVersion? version2)
    {
        if (version1 is null)
        {
            throw new ArgumentNullException(nameof(version1));
        }

        if (version2 is null)
        {
            throw new ArgumentNullException(nameof(version2));
        }

        return version1?.CompareTo(version2) == 0;
    }

    /// <summary>Implements the operator &gt;.</summary>
    /// <param name="version1">The version1.</param>
    /// <param name="version2">The version2.</param>
    /// <returns>The result of the operator.</returns>
    /// <exception cref="ArgumentNullException">version1</exception>
    public static bool operator >(SemanticVersion version1, SemanticVersion version2)
    {
        if (version1 is null)
        {
            throw new ArgumentNullException(nameof(version1));
        }

        if (version2 is null)
        {
            throw new ArgumentNullException(nameof(version2));
        }

        return version1.CompareTo(version2) > 0;
    }

    /// <summary>Implements the operator &gt;=.</summary>
    /// <param name="version1">The version1.</param>
    /// <param name="version2">The version2.</param>
    /// <returns>The result of the operator.</returns>
    public static bool operator >=(SemanticVersion version1, SemanticVersion version2)
    {
        return version1 == version2 || version1 > version2;
    }

    /// <summary>Implements the operator !=.</summary>
    /// <param name="version1">The version1.</param>
    /// <param name="version2">The version2.</param>
    /// <returns>The result of the operator.</returns>
    public static bool operator !=(SemanticVersion version1, SemanticVersion version2)
    {
        return !(version1 == version2);
    }

    /// <summary>Implements the operator &lt;.</summary>
    /// <param name="version1">The version1.</param>
    /// <param name="version2">The version2.</param>
    /// <returns>The result of the operator.</returns>
    /// <exception cref="ArgumentNullException">version1</exception>
    public static bool operator <(SemanticVersion version1, SemanticVersion version2)
    {
        return version2 > version1;
    }

    /// <summary>Implements the operator &lt;=.</summary>
    /// <param name="version1">The version1.</param>
    /// <param name="version2">The version2.</param>
    /// <returns>The result of the operator.</returns>
    public static bool operator <=(SemanticVersion version1, SemanticVersion version2)
    {
        return version1 == version2 || version1 < version2;
    }

    /// <summary>
    ///     Parses a version string using loose semantic versioning rules that allows 2-4 version components followed by an
    ///     optional special version.
    /// </summary>
    public static SemanticVersion Parse(string versionString)
    {
        ArgumentNullException.ThrowIfNull(versionString);

        if (string.IsNullOrEmpty(versionString))
        {
            throw new ArgumentException("The string is empty", nameof(versionString));
        }

        if (!TryParse(versionString, out var semVer))
        {
            throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, $"'{versionString}' could not be parsed as a valid semantic version"), nameof(versionString));
        }

        return semVer!;
    }

    /// <summary>Converts to string.</summary>
    /// <returns>A <see cref="string" /> that represents this instance.</returns>
    public override string ToString()
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.Append(Major);
        stringBuilder.Append(NumericSeperator);
        stringBuilder.Append(Minor);
        stringBuilder.Append(NumericSeperator);
        stringBuilder.Append(Patch);

        if (!string.IsNullOrWhiteSpace(PreRelease))
        {
            stringBuilder.Append(PreReleasePrefix);
            stringBuilder.Append(PreRelease);
        }

        if (!string.IsNullOrWhiteSpace(BuildMetadata))
        {
            stringBuilder.Append(BuildMetadataPrefix);
            stringBuilder.Append(BuildMetadata);
        }

        return stringBuilder.ToString();
    }

    /// <summary>Converts this instance to a System.Version.</summary>
    /// <remarks>
    ///     ToVersion(false) on 1.2.4-alpha.7 returns 1.2.4
    ///     ToVersion(true) on 1.2.4-alpha.7 returns 1.2.4.7
    /// </remarks>
    /// <param name="treatLastPreReleaseNumberAsRevision">
    ///     If the last section of a pre-release tag is numeric, use it as the
    ///     revision number.
    /// </param>
    /// <returns>A version</returns>
    public Version ToVersion(bool treatLastPreReleaseNumberAsRevision = false)
    {
        if (Major > int.MaxValue)
        {
            throw new InvalidCastException($"Major version number of {Major} is higher than a System.Version can represent");
        }

        if (Minor > int.MaxValue)
        {
            throw new InvalidCastException($"Minor version number of {Minor} is higher than a System.Version can represent");
        }

        if (Patch > int.MaxValue)
        {
            throw new InvalidCastException($"Patch version number of {Patch} is higher than a System.Version can represent");
        }

        if (treatLastPreReleaseNumberAsRevision)
        {
            if (TryExtractPreReleaseNumber(out var preReleaseNumber))
            {
                if (preReleaseNumber > int.MaxValue)
                {
                    throw new InvalidCastException($"Pre-release number of {preReleaseNumber} is higher than a System.Version can represent");
                }

                return new Version((int)Major, (int)Minor, (int)Patch, (int)preReleaseNumber);
            }
        }

        return new Version((int)Major, (int)Minor, (int)Patch, 0);
    }

    /// <summary>Attempts to parse a semantic version string</summary>
    /// <param name="versionString">A string representation of the SemVer</param>
    /// <param name="semVer">The semantic version</param>
    /// <returns>true if the string could be parsed successfully, false if unsuccessful</returns>
    public static bool TryParse(string versionString, out SemanticVersion? semVer)
    {
        semVer = null;
        if (string.IsNullOrEmpty(versionString))
        {
            return false;
        }

        var match = OfficialRegexWithGroups.Match(versionString.Trim());
        if (!match.Success)
        {
            return false;
        }

        var major = uint.Parse(match.Groups["major"].Value.Trim());

        uint minor = 0;
        if (match.Groups.ContainsKey("minor"))
        {
            minor = uint.Parse(match.Groups["minor"].Value.Trim());
        }

        uint patch = 0;
        if (match.Groups.ContainsKey("patch"))
        {
            patch = uint.Parse(match.Groups["patch"].Value.Trim());
        }

        string? preRelease = null;
        if (match.Groups.ContainsKey("prerelease"))
        {
            preRelease = match.Groups["prerelease"].Value.Trim();
        }

        string? build = null;
        if (match.Groups.ContainsKey("buildmetadata"))
        {
            build = match.Groups["buildmetadata"].Value.Trim();
        }

        semVer = new SemanticVersion(major, minor, patch, preRelease, build);

        return true;
    }

    private int ComparePreReleaseString(string otherPreRelease)
    {
        var thisHasPrelease = !string.IsNullOrEmpty(PreRelease);
        var otherHasPreRelease = !string.IsNullOrEmpty(otherPreRelease);
        if (!thisHasPrelease && otherHasPreRelease)
        {
            return ThisIsGreaterThanOther;
        }

        if (thisHasPrelease && !otherHasPreRelease)
        {
            return ThisIsLessThanOther;
        }

        var thisParts = PreRelease.Split('.');
        var otherParts = otherPreRelease.Split('.');

        for (var partIndex = 0; partIndex < thisParts.Length; partIndex++)
        {
            var thisPartDoesNotExistOnOther = otherParts.Length < partIndex + 1;
            if (thisPartDoesNotExistOnOther)
            {
                // 4. A larger set of pre-release fields has a higher precedence than a smaller set, if all of the preceding identifiers are equal.
                return ThisIsLessThanOther;
            }

            var thisPart = thisParts[partIndex];
            var otherPart = otherParts[partIndex];

            var isThisPartNumeric = uint.TryParse(thisPart, out var thisAsNumber);
            var isOtherPartNumeric = uint.TryParse(otherPart, out var otherAsNumber);
            var bothPartsAreNumeric = isThisPartNumeric && isOtherPartNumeric;
            var bothPartsAreNonNumeric = !isThisPartNumeric && !isOtherPartNumeric;
            var otherIsNumericButThisIsNot = !isThisPartNumeric && isOtherPartNumeric;
            var thisIsNumericButOtherIsNot = isThisPartNumeric && !isOtherPartNumeric;

            // 1. Identifiers consisting of only digits are compared numerically.
            if (bothPartsAreNumeric)
            {
                var numericComparison = thisAsNumber.CompareTo(otherAsNumber);
                if (numericComparison == EqualPrecendence)
                {
                    continue;
                }

                return numericComparison;
            }

            // 2. Identifiers with letters or hyphens are compared lexically in ASCII sort order
            if (bothPartsAreNonNumeric)
            {
                // http://support.ecisolutions.com/doc-ddms/help/reportsmenu/ascii_sort_order_chart.htm
                var asciiComparison = StringComparer.Ordinal.Compare(thisPart, otherPart);
                if (asciiComparison == EqualPrecendence)
                {
                    continue;
                }

                return asciiComparison > 0 ? ThisIsGreaterThanOther : ThisIsLessThanOther;
            }

            // 3. Numeric identifiers always have lower precedence than non-numeric identifiers.
            if (otherIsNumericButThisIsNot)
            {
                return ThisIsGreaterThanOther;
            }

            if (thisIsNumericButOtherIsNot)
            {
                return ThisIsLessThanOther;
            }
        }

        // All parts in this are equal to all parts in other so check if other has more parts
        var otherHasMoreParts = otherParts.Length > thisParts.Length;
        return otherHasMoreParts ? ThisIsGreaterThanOther : EqualPrecendence;
    }

    private int CompareNumericParts(SemanticVersion other)
    {
        var majorComparison = Major.CompareTo(other.Major);
        if (majorComparison != 0)
        {
            return majorComparison;
        }

        var minorComparison = Minor.CompareTo(other.Minor);
        if (minorComparison != 0)
        {
            return minorComparison;
        }

        var patchComparison = Patch.CompareTo(other.Patch);
        return patchComparison != 0 ? patchComparison : 0;
    }

    private bool TryExtractPreReleaseNumber(out uint preReleaseNumber)
    {
        preReleaseNumber = 0;
        if (!IsPreRelease)
        {
            return false;
        }

        var parts = PreRelease.Split(NumericSeperator);
        if (!uint.TryParse(parts[^1], out var lastNumber))
        {
            return false;
        }

        preReleaseNumber = lastNumber;
        return true;
    }
}
