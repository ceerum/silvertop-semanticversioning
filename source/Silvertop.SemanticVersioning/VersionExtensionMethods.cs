namespace Silvertop.SemanticVersioning;

/// <summary>
///     Extension methods for <see cref="Version" /> to convert to and from <see cref="SemanticVersion" />
/// </summary>
public static class VersionExtensionMethods
{
    /// <summary>
    ///     Converts this version into a <see cref="SemanticVersion" />
    /// </summary>
    /// <param name="version">This version</param>
    /// <param name="treatRevisionAsPreReleaseNumber">
    ///     If true and a revision number is set then the revision number will be treated as the pre-release version. Default
    ///     is false.
    /// </param>
    /// <param name="preReleasePrefix">
    ///     If treatRevisionAsPreReleaseNumber is true, and a revision number is set, pre-release string will start with this prefix.
    ///     Default is 'beta'.
    /// </param>
    /// <returns>A <see cref="SemanticVersion" /> representation of this version</returns>
    public static SemanticVersion ToSemanticVersion(this Version version, bool treatRevisionAsPreReleaseNumber = false, string preReleasePrefix = "beta")
    {
        return SemanticVersion.FromVersion(version, treatRevisionAsPreReleaseNumber, preReleasePrefix);
    }
}
