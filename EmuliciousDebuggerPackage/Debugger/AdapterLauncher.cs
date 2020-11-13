using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EmuliciousDebuggerPackage.Debugger.VisualStudio;
using EnvDTE80;
using Microsoft.VisualStudio.Debugger.DebugAdapterHost.Interfaces;
using Microsoft.VisualStudio.Debugger.Interop;
using Microsoft.VisualStudio.Setup.Configuration;
using Microsoft.Win32;
using Newtonsoft.Json;
using Process = System.Diagnostics.Process;

namespace EmuliciousDebuggerPackage.Debugger
{
    /// <summary>
    /// Adapter launcher.
    /// </summary>
    [ComVisible(true)]
    [Guid("A17632B4-81DC-47F5-880F-1198CB9D4764")]
    public sealed class AdapterLauncher : IAdapterLauncher
    {
        #region IAdapterLauncher

        /// <inheritdoc />
        public void Initialize(IDebugAdapterHostContext context)
        {
        }

        /// <inheritdoc />
        public void UpdateLaunchOptions(IAdapterLaunchInfo launchInfo)
        {
            // Apply launch options.
        }

        /// <inheritdoc />
        public ITargetHostProcess LaunchAdapter(IAdapterLaunchInfo launchInfo, ITargetHostInterop targetInterop)
        {
            ITargetHostProcess runningProcess = null;
            
            // Check if an emulicious process is running and reuse it.
            var processId = 0;
            var emuliciousProcess =  Process.GetProcesses().Where(process => process.ProcessName.Contains("java") && process.MainWindowTitle == "Emulicious").ToList();

            if (emuliciousProcess.Count > 0)
            {
                // use the process and capture it in an ITargetHostProcess.
                runningProcess = new ExistingTargetHostProcess(emuliciousProcess[0]);
                processId = emuliciousProcess[0].Id;
            }

            // Check if an instance of Emulicious is already running.
            // If so capture it as the instance to debug.
            // Otherwise launch a new instance.
            // If launching an instance. Wait to connect.
            if (runningProcess == null)
            {
                var emuliciousPath = EmuliciousDebuggerLaunchProvider.EmuliciousExecutable;

                // Launch emulicious.
                var startProcess = targetInterop.ExecuteCommandAsync(emuliciousPath, "");

                // Launch a new process, delay before sending the initialize event.
                Thread.Sleep(EmuliciousDebuggerLaunchProvider.EmuliciousLaunchDelay);

                // Capture the emulicious process id for destruction if not attaching.
                emuliciousProcess = Process.GetProcesses().Where(process =>
                    process.ProcessName.Contains("java") && process.MainWindowTitle == "Emulicious").ToList();

                if (emuliciousProcess.Count > 0)
                {
                    // Update process id for debugger disconnection.
                    processId = emuliciousProcess[0].Id;
                }
            }

            // Get the path to the adapter executable.
            /*
            var hive = GetHiveInstance();
            var adapterPath = @"SOFTWARE\Microsoft\VisualStudio\"
                       + hive
                       + @"_Config\AD7Metrics\Engine\{BE99C8E2-969A-450C-8FAB-73BECCC53DF4}";
                       */



            // Get the adapter value.
            //RegistryKey registryKey = Registry.LocalMachine.GetValue(adapterPath) as RegistryKey;
            /*
            var adapterInfo = "Invalid no registry key";
            if (registryKey != null)
            {
                adapterInfo = registryKey.GetValue("Adapter").ToString();
            }

            File.WriteAllLines(@"C:\Development\Development\Projects\GBDKProjects\GBDKEngine\Debug\adapter.info",
                new []
                {
                    registryKey == null ? "No registry key found...." : registryKey.ToString(),
                    adapterInfo,
                });
            */

            return new EmuliciousTargetHostProcessWrapper(targetInterop.ExecuteCommandAsync(
                    EmuliciousPackage.DebugAdapterPath,
                    string.Format("{0} \"{1}\"",
                        EmuliciousDebuggerLaunchProvider.EmuliciousPort,
                        EmuliciousDebuggerLaunchProvider.EmuliciousDebugFolder)), processId,
                !EmuliciousDebuggerLaunchProvider.EmuliciousAttach);
        }

        /// <summary>
        /// Get the visual studio hive instance.
        /// </summary>
        /// <returns></returns>
        private string GetHiveInstance()
        {
            var config = new SetupConfiguration();
            var instance = config.GetInstanceForCurrentProcess();

            File.WriteAllLines(@"C:\Development\Development\Projects\GBDKProjects\GBDKEngine\Debug\install.info",
                new[]
                {
                    instance.GetInstallationVersion(),
                    instance.GetInstanceId(),
                    instance.GetDescription(),
                    instance.GetInstallationPath(),
                    instance.GetDisplayName(),
                    instance.GetInstallationName()
                });


            // Get the first value from the install version.
            var installVersion = instance.GetInstallationVersion();
            return string.Format("{0}.0_{1}", installVersion.Substring(0, installVersion.IndexOf('.')), instance.GetInstanceId());
        }
        #endregion
    }
}
