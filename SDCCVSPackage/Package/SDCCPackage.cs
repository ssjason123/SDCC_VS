using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Task = System.Threading.Tasks.Task;

namespace SDCCVSPackage.Package
{
    /// <summary>
    ///     SDCC package plugin registration.
    /// </summary>
    [PackageRegistration(UseManagedResourcesOnly = false, AllowsBackgroundLoading = true)]
    [ProvideAutoLoad(UIContextGuids80.SolutionExistsAndNotBuildingAndNotDebugging, PackageAutoLoadFlags.BackgroundLoad)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)] // Info on this package for Help/About
    [Guid(PackageGuidString)]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "pkgdef, VS and vsixmanifest are valid VS terms")]
    public class SDCCPackage : AsyncPackage
    {
        /// <summary>
        ///     Package guid
        /// </summary>
        public const string PackageGuidString = "95B086A0-DD9C-4AE8-AC45-543F69548856";

        // TODO: is this necessary? Emulicious package is set to autoload on its own.
        /*
        /// <inheritdoc />
        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            await base.InitializeAsync(cancellationToken, progress);

            await JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

            // Force load the emulicious debugging package.
            var shell = await GetServiceAsync(typeof(SVsShell)) as IVsShell;
            if (shell != null)
            {
                IVsPackage package = null;
                var packageToBeLoadedGuid = new Guid("efed050d-270a-4a5a-a28c-d008bda32b8e");
                shell.LoadPackage(ref packageToBeLoadedGuid, out package);
            }
        }
        */
    }
}
