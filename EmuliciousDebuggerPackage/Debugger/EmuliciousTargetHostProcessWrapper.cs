using System;
using System.Diagnostics;
using System.IO;
using EmuliciousDebuggerPackage.Debugger.VisualStudio;
using Microsoft.VisualStudio.Debugger.DebugAdapterHost.Interfaces;

namespace EmuliciousDebuggerPackage.Debugger
{
    /// <summary>
    ///     <see cref="ITargetHostProcess"/> implementation for the emulicious process.
    /// </summary>
    public class EmuliciousTargetHostProcessWrapper : ITargetHostProcess
    {
        /// <summary>
        ///     The base adapter process.
        /// </summary>
        private ITargetHostProcess srcProcess;
        /// <summary>
        ///     The emulicious process id.
        /// </summary>
        private int processId = -1;
        /// <summary>
        ///     Flag to stop the process on exit.
        /// </summary>
        private bool stopProcessOnClose;

        /// <summary>
        ///     Default constructor.
        /// </summary>
        /// <param name="adapterProcess">
        ///     The emulicious process adapter.
        /// </param>
        /// <param name="processId">
        ///     The emulicious process id.
        /// </param>
        /// <param name="stopProcess">
        ///     The flag to stop the process on exit.
        /// </param>
        public EmuliciousTargetHostProcessWrapper(ITargetHostProcess adapterProcess, int processId,
            bool stopProcess = false)
        {
            srcProcess = adapterProcess;
            this.processId = processId;
            stopProcessOnClose = stopProcess;

            srcProcess.ErrorDataReceived += OnErrorReceived;
            srcProcess.Exited += OnExited;
        }

        /// <inheritdoc />
        public void Terminate()
        {
            srcProcess.Terminate();
        }

        /// <inheritdoc />
        public IntPtr Handle
        {
            get { return srcProcess.Handle; 
            }
        }

        /// <inheritdoc />
        public Stream StandardInput
        {
            get { return srcProcess.StandardInput; }
        }

        /// <inheritdoc />
        public Stream StandardOutput
        {
            get { return srcProcess.StandardOutput; }
        }

        /// <inheritdoc />
        public bool HasExited
        {
            get { return srcProcess.HasExited; }
        }

        /// <inheritdoc />
        public event EventHandler Exited
        {
            add { srcProcess.Exited += value; }
            remove { srcProcess.Exited -= value; }
        }

        /// <inheritdoc />
        public event DataReceivedEventHandler ErrorDataReceived
        {
            add { srcProcess.ErrorDataReceived += value; }
            remove { srcProcess.ErrorDataReceived -= value; }
        }

        private void OnErrorReceived(object sender, DataReceivedEventArgs args)
        {
            File.AppendAllLines(Path.Combine(EmuliciousDebuggerLaunchProvider.EmuliciousMappingPath, "Exception.log"),
                new[] { " OnErrorReceived:", args.Data});
        }

        /// <summary>
        ///     Handle the process exit request.
        /// </summary>
        /// <param name="sender">
        ///     The sender.
        /// </param>
        /// <param name="args">
        ///     The event args.
        /// </param>
        private void OnExited(object sender, EventArgs args)
        {
            srcProcess.Exited -= OnExited;
            srcProcess.ErrorDataReceived -= OnErrorReceived;

            if (stopProcessOnClose)
            {
                // Attempt to close the emulicious process.
                try
                {
                    var process = Process.GetProcessById(processId);
                    process.Kill();
                }
                catch
                {
                    // Intentionally empty.
                }
            }
        }
    }
}
