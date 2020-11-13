using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Debugger.Interop;

namespace EmuliciousDebuggerPackage.Debugger.VisualStudio
{
    // Future development, support "Attach to process", requires a custom Port Supplier.
#if __PORT_SUPPLIER__
    [ComVisible(true)]
    [Guid(PortSupplierCLSID)]
    public class EmuliciousPortSupplier : IDebugPortSupplier2
    {
        public class EmuliciousPortEnumerator : IEnumDebugPorts2
        {
            private IDebugPort2[] ports;
            private uint position;

            public EmuliciousPortEnumerator(IDebugPort2[] ports)
            {
                position = 0;
                this.ports = ports;
            }

            /// <inheritdoc />
            public int Next(uint celt, IDebugPort2[] rgelt, ref uint pceltFetched)
            {
                pceltFetched = 0;
                for (; pceltFetched < celt && position < ports.Length; ++pceltFetched)
                {
                    rgelt[pceltFetched] = ports[position + pceltFetched];
                }

                return Skip(pceltFetched);
            }

            /// <inheritdoc />
            public int Skip(uint celt)
            {
                var result = VSConstants.S_FALSE;

                if (celt + position <= ports.Length)
                {
                    position += celt;
                    result = VSConstants.S_OK;
                }

                return result;
            }

            /// <inheritdoc />
            public int Reset()
            {
                position = 0;
                return VSConstants.S_OK;
            }

            /// <inheritdoc />
            public int Clone(out IEnumDebugPorts2 ppEnum)
            {
                ppEnum = new EmuliciousPortEnumerator(ports){position = position};
                return VSConstants.S_OK;
            }

            /// <inheritdoc />
            public int GetCount(out uint pcelt)
            {
                pcelt = 0;

                if (ports != null)
                {
                    pcelt = (uint)ports.Length;
                }

                return VSConstants.S_OK;
            }
        }

        public class EmuliciousPort : IDebugPort2
        {
            private IDebugPortSupplier2 supplier;
            private Guid portGuid;

            public EmuliciousPort(IDebugPortSupplier2 supplier)
            {
                portGuid = new Guid();
            }

            /// <inheritdoc />
            public int GetPortName(out string pbstrName)
            {
                pbstrName = "Emulicious";
                return VSConstants.S_OK;
            }

            /// <inheritdoc />
            public int GetPortId(out Guid pguidPort)
            {
                pguidPort = portGuid;
                return VSConstants.S_OK;
            }

            /// <inheritdoc />
            public int GetPortRequest(out IDebugPortRequest2 ppRequest)
            {
                throw new NotImplementedException();
            }

            /// <inheritdoc />
            public int GetPortSupplier(out IDebugPortSupplier2 ppSupplier)
            {
                ppSupplier = supplier;
                return VSConstants.S_OK;
            }

            /// <inheritdoc />
            public int GetProcess(AD_PROCESS_ID ProcessId, out IDebugProcess2 ppProcess)
            {
                throw new NotImplementedException();
            }

            /// <inheritdoc />
            public int EnumProcesses(out IEnumDebugProcesses2 ppEnum)
            {
                ppEnum = null;
                return VSConstants.S_OK;
            }
        }

        public const int E_PORTSUPPLIER_NO_PORT = unchecked((int)0x80040080); // Cannot find port. Check the remote machine name.

        public const string PortSupplierCLSID = "9532C71E-183F-4D6A-BCC7-2624799ED5AD";
        public static readonly Guid PortSupplierGuid = new Guid("6AE12257-DCAF-4CBA-91D8-210955EE42F1");

        private List<IDebugPort2> ports = new List<IDebugPort2>();

        /// <inheritdoc />
        public int GetPortSupplierName(out string pbstrName)
        {
            pbstrName = "Emulicious";
            return VSConstants.S_OK;
        }

        /// <inheritdoc />
        public int GetPortSupplierId(out Guid pguidPortSupplier)
        {
            pguidPortSupplier = PortSupplierGuid;
            return VSConstants.S_OK;
        }

        /// <inheritdoc />
        public int GetPort(ref Guid guidPort, out IDebugPort2 ppPort)
        {
            ppPort = null;
            return VSConstants.S_OK;
        }

        /// <inheritdoc />
        public int EnumPorts(out IEnumDebugPorts2 ppEnum)
        {
            ppEnum = null;
            return VSConstants.S_OK;
        }

        /// <inheritdoc />
        public int CanAddPort()
        {
            return VSConstants.S_OK;
        }

        /// <inheritdoc />
        public int AddPort(IDebugPortRequest2 pRequest, out IDebugPort2 ppPort)
        {
            string portName;
            var result = pRequest.GetPortName(out portName);

            ppPort = null;
            return VSConstants.S_OK;
        }

        /// <inheritdoc />
        public int RemovePort(IDebugPort2 pPort)
        {
            return VSConstants.S_OK;
        }
    }
#endif
}
