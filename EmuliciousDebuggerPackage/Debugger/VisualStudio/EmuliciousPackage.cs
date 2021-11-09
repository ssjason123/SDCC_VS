using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Task = System.Threading.Tasks.Task;

namespace EmuliciousDebuggerPackage.Debugger.VisualStudio
{
    /// <summary>
    ///     This is the class that implements the package exposed by this assembly.
    /// </summary>
    /// <remarks>
    /// <para>
    ///     The minimum requirement for a class to be considered a valid package for Visual Studio
    ///     is to implement the IVsPackage interface and register itself with the shell.
    ///     This package uses the helper classes defined inside the Managed Package Framework (MPF)
    ///     to do it: it derives from the Package class that provides the implementation of the
    ///     IVsPackage interface and uses the registration attributes defined in the framework to
    ///     register itself and its components with the shell. These attributes tell the pkgdef creation
    ///     utility what data to put into .pkgdef file.
    /// </para>
    /// <para>
    ///     To get loaded into VS, the package must be referred by &lt;Asset Type="Microsoft.VisualStudio.VsPackage" ...&gt; in .vsixmanifest file.
    /// </para>
    /// </remarks>
    [PackageRegistration(UseManagedResourcesOnly = false, AllowsBackgroundLoading = true)]
    [ProvideAutoLoad(UIContextGuids80.SolutionExists, PackageAutoLoadFlags.BackgroundLoad)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)] // Info on this package for Help/About
    [Guid(EmuliciousPackage.PackageGuidString)]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "pkgdef, VS and vsixmanifest are valid VS terms")]
    public sealed class EmuliciousPackage : AsyncPackage
    {
        /// <summary>
        ///     Static path to debug adapter.
        /// </summary>
        public static string DebugAdapterPath { get; set; }

        /// <summary>
        ///     EmuliciousPackage GUID string.
        /// </summary>
        public const string PackageGuidString = "efed050d-270a-4a5a-a28c-d008bda32b8e";

        /// <inheritdoc />
        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            await base.InitializeAsync(cancellationToken, progress);

            await JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

            try
            {
                var regInfo1 =
                    ApplicationRegistryRoot.OpenSubKey(@"AD7Metrics\Engine\{BE99C8E2-969A-450C-8FAB-73BECCC53DF4}");
                if (regInfo1 != null)
                {
                    DebugAdapterPath = regInfo1.GetValue("Adapter").ToString();

                    /*
                    File.AppendAllText(
                        @"C:\Development\Development\Projects\GBDKProjects\GBDKEngine\Debug\PackageLog.log",
                        "App Reg: " + DebugAdapterPath + "\n" + regInfo1.Name);
                    */
                }
            }
            catch (Exception err)
            {
                /*
                File.AppendAllText(
                    @"C:\Development\Development\Projects\GBDKProjects\GBDKEngine\Debug\PackageLog.log",
                    "App Ex: " + err.ToString());
                */
            }
        }
    }
}
