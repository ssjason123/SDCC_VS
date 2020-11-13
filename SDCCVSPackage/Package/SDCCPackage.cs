using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell;

namespace SDCCVSPackage.Package
{

    [PackageRegistration(UseManagedResourcesOnly = false)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)] // Info on this package for Help/About
    [Guid(PackageGuidString)]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "pkgdef, VS and vsixmanifest are valid VS terms")]
    public class SDCCPackage : Microsoft.VisualStudio.Shell.Package
    {
        /// <summary>
        /// Package guid
        /// </summary>
        public const string PackageGuidString = "95B086A0-DD9C-4AE8-AC45-543F69548856";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public SDCCPackage()
        {

        }
    }
}
