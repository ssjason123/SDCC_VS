using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using EmuliciousDebuggerPackage.Debugger.VisualStudio;
using Microsoft.VisualStudio.Debugger.DebugAdapterHost.Interfaces;
using Microsoft.VisualStudio.Setup.Configuration;
using Process = System.Diagnostics.Process;

namespace EmuliciousDebuggerPackage.Debugger
{
    /// <summary>
    ///     Adapter launcher.
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
            ITargetHostProcess resultProcess = null;

            try
            {
                ITargetHostProcess runningProcess = null;
                
                // Check if an emulicious process is running and reuse it.
                var emuliciousProcess =  Process.GetProcesses().Where(process => process.ProcessName.Contains("java") && process.MainWindowTitle == "Emulicious").ToList();

                var processId = -1;
                if (emuliciousProcess.Count > 0)
                {
                    // use the process and capture it in an ITargetHostProcess.
                    runningProcess = new ExistingTargetHostProcess(emuliciousProcess[0]);
                    processId = emuliciousProcess.First().Id;
                }

                // Check if an instance of Emulicious is already running.
                // If so capture it as the instance to debug.
                // Otherwise launch a new instance.
                // If launching an instance. Wait to connect.
                if (runningProcess == null)
                {
                    // Launch emulicious.

                    var emuliciousExec = EmuliciousDebuggerLaunchProvider.EmuliciousExecutable;
                    /*
                    File.WriteAllLines(Path.Combine(EmuliciousDebuggerLaunchProvider.EmuliciousMappingPath, "Properties.log"),
                        new []
                        {
                            emuliciousExec,
                            EmuliciousDebuggerLaunchProvider.EmuliciousMappingPath,
                            EmuliciousDebuggerLaunchProvider.EmuliciousDebugFolder,
                            EmuliciousDebuggerLaunchProvider.EmuliciousAttach.ToString(),
                            EmuliciousDebuggerLaunchProvider.EmuliciousLaunchDelay.ToString(),
                            EmuliciousDebuggerLaunchProvider.EmuliciousPort.ToString(),
                            EmuliciousPackage.DebugAdapterPath
                        });
                    */
                    var args = new List<string>();

                    if (EmuliciousDebuggerLaunchProvider.EmuliciousRemoteDebugArgument)
                    {
                        args.Add(string.Format("-remotedebug {0}", EmuliciousDebuggerLaunchProvider.EmuliciousPort));
                    }

                    try
                    {
                        var process = Process.Start(emuliciousExec, string.Join(" ", args));
                        if (process != null)
                        {
                            process.WaitForExit();
                        }
                    }
                    catch (Exception err)
                    {
                        File.WriteAllLines(Path.Combine(EmuliciousDebuggerLaunchProvider.EmuliciousMappingPath, "LaunchProcess.log"),
                            new[]
                            {
                                emuliciousExec,
                                err.ToString()
                            });
                    }

                    Thread.Sleep(EmuliciousDebuggerLaunchProvider.EmuliciousLaunchDelay);

                    // Capture the emulicious process id for destruction if not attaching.
                    emuliciousProcess = Process.GetProcesses().Where(process =>
                        process.ProcessName.Contains("java") && process.MainWindowTitle == "Emulicious").ToList();

                    if (emuliciousProcess.Count > 0)
                    {
                        // Update process id for debugger disconnection.
                        processId = emuliciousProcess.First().Id;
                    }
                    else
                    {
                        // No process was found, this is a failure.
                        processId = -1;
                    }
                }

                resultProcess = new EmuliciousTargetHostProcessWrapper(targetInterop.ExecuteCommandAsync(
                        EmuliciousPackage.DebugAdapterPath,
                        string.Format("{0} \"{1}\" \"{2}\"",
                            EmuliciousDebuggerLaunchProvider.EmuliciousPort,
                            EmuliciousDebuggerLaunchProvider.EmuliciousDebugFolder,
                            EmuliciousDebuggerLaunchProvider.EmuliciousMappingPath)), processId,
                    !EmuliciousDebuggerLaunchProvider.EmuliciousAttach);

#if __ADAPTER_LOG__
                File.AppendAllLines(Path.Combine(EmuliciousDebuggerLaunchProvider.EmuliciousMappingPath, "AdapterLog.log"),
                    new []{ "Process result: " + resultProcess + " Proc id: " + processId});
#endif
            }
            catch (Exception e)
            {
                // Capture any exceptions and save them for research.
                File.WriteAllText(Path.Combine(EmuliciousDebuggerLaunchProvider.EmuliciousMappingPath, "Exception.log"),
                    e.ToString());
                throw;
            }

            return resultProcess;
        }

        /// <summary>
        ///     Get the visual studio hive instance.
        /// </summary>
        /// <returns>
        ///     The instance version.
        /// </returns>
        private string GetHiveInstance()
        {
            var config = new SetupConfiguration();
            var instance = config.GetInstanceForCurrentProcess();

            // Get the first value from the install version.
            var installVersion = instance.GetInstallationVersion();
            return string.Format("{0}.0_{1}", installVersion.Substring(0, installVersion.IndexOf('.')), instance.GetInstanceId());
        }
#endregion
    }
}
