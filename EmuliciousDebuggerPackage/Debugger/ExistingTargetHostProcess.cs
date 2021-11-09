using System;
using System.Diagnostics;
using System.IO;
using Microsoft.VisualStudio.Debugger.DebugAdapterHost.Interfaces;

namespace EmuliciousDebuggerPackage.Debugger
{
    /// <summary>
    ///     Thin wrapper for a Process to ITargetHostProcess.
    /// </summary>
    public class ExistingTargetHostProcess : ITargetHostProcess
    {
        /// <summary>
        ///     Internal Process reference.
        /// </summary>
        private Process TargetProcess;

        /// <summary>
        ///     Default constructor.
        /// </summary>
        /// <param name="baseProcess">
        ///     The base process.
        /// </param>
        public ExistingTargetHostProcess(Process baseProcess)
        {
            if (baseProcess == null)
            {
                throw new ArgumentNullException(nameof(baseProcess));
            }
            TargetProcess = baseProcess;
        }

        /// <inheritdoc />
        public void Terminate()
        {
            TargetProcess.Kill();
        }

        /// <inheritdoc />
        public IntPtr Handle
        {
            get { return TargetProcess.Handle; }
        }

        /// <inheritdoc />
        public Stream StandardInput
        {
            get { return TargetProcess.StandardInput.BaseStream; }
        }

        /// <inheritdoc />
        public Stream StandardOutput
        {
            get { return TargetProcess.StandardOutput.BaseStream; }
        }

        /// <inheritdoc />
        public bool HasExited
        {
            get { return TargetProcess.HasExited; }
        }

        /// <inheritdoc />
        public event EventHandler Exited
        {
            add { TargetProcess.Exited += value; }
            remove { TargetProcess.Exited -= value; }
        }

        /// <inheritdoc />
        public event DataReceivedEventHandler ErrorDataReceived
        {
            add { TargetProcess.ErrorDataReceived += value; }
            remove { TargetProcess.ErrorDataReceived -= value; }
        }
    }
}
