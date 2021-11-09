using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using EmuliciousPassThroughAdapter.Modifiers;
using EmuliciousShared;

namespace EmuliciousPassThroughAdapter
{
    /// <summary>
    ///     Create an adapter to connect to emulicious.
    /// </summary>
    public class PassthroughAdapter : IDisposable
    {
        /// <summary>
        ///     Input stream from the VSHost process.
        /// </summary>
        private Stream HostIn;
        /// <summary>
        ///     Output stream to the VSHost process.
        /// </summary>
        private Stream HostOut;

        /// <summary>
        ///     Network client to communicate with emulicious.
        /// </summary>
        private TcpClient AdapterClient;

        /// <summary>
        ///     Emulicious input stream.
        /// </summary>
        private Stream ClientIn;
        /// <summary>
        ///     Emulicious output stream.
        /// </summary>
        private Stream ClientOut;

        /// <summary>
        ///     Test debug log
        /// </summary>
        private StreamWriter DebugLog;

        /// <summary>
        ///     Stream to talk to the host.
        /// </summary>
        private PathRenameStream ToHostStream;

        /// <summary>
        ///     Stream to talk to the client.
        /// </summary>
        private PathRenameStream ToClientStream;

        /// <summary>
        ///     Default constructor.
        /// </summary>
        /// <param name="inStream">
        ///     Input stream.
        /// </param>
        /// <param name="outStream">
        ///     Output stream.
        /// </param>
        /// <param name="settings">
        ///     The passthrough project settings.
        /// </param>
        /// <param name="port">
        ///     Emulicious Debug port.
        /// </param>
        public PassthroughAdapter(Stream inStream, Stream outStream, PassthroughSettings settings,
                int port = 58870)
        {
            var debugPath = settings.DevelopmentPath;
            DebugLog = null;
            if (Directory.Exists(debugPath))
            {
                var debugFile = Path.Combine(debugPath, "Debug.log");
                DebugLog = new StreamWriter(File.Create(debugFile)) {AutoFlush = true};
            }

            try
            {
                HostIn = inStream;
                HostOut = outStream;

                AdapterClient = new TcpClient() {NoDelay = true};

                AdapterClient.Connect(IPAddress.Loopback, port);

                if (AdapterClient.Connected)
                {
                    ClientIn = AdapterClient.GetStream();
                    ClientOut = AdapterClient.GetStream();

                    ToClientStream = new PathRenameStream(ClientIn, StreamDirection.ToClient, ConfigureModifiers(settings,
                        StreamDirection.ToClient)) {DebugStream = DebugLog};
                    ToHostStream = new PathRenameStream(HostOut, StreamDirection.ToHost, ConfigureModifiers(settings,
                        StreamDirection.ToHost)) {DebugStream = DebugLog};
                }

                if (DebugLog != null)
                {
                    if (AdapterClient.Connected)
                    {
                        DebugLog.WriteLine("*** Connected ***");
                    }
                    else
                    {
                        DebugLog.WriteLine("*** Not Connected ***");
                    }
                }
            }
            catch (Exception err)
            {
                if (DebugLog != null)
                {
                    DebugLog.Write("*** EXECPTION ***\n" + err);
                }
            }
        }

        /// <summary>
        ///     Configure the modifiers to run.
        /// </summary>
        /// <returns>
        ///     The list of modifiers to execute.
        /// </returns>
        private IList<IJsonModifier> ConfigureModifiers(PassthroughSettings settings, StreamDirection direction)
        {
            // Configure the modifiers;
            var modifiers = new List<IJsonModifier>();

            if (settings.UseLegacySourceFolders)
            {
                if (direction == StreamDirection.ToClient)
                {
                    modifiers.Add(new LegacySourcePathRenameModifier(settings.GetLegacySourceFolders(),
                        DebugLog));
                }
                else
                {
                    // Flip the modifier for host mapping.
                    modifiers.Add(new LegacySourcePathRenameModifier(settings.GetLegacySourceFolders().ToDictionary((pair) => pair.Value, (pair) => pair.Key),
                        DebugLog));
                }
            }

            if (settings.UseLegacyBreakpointFix)
            {
                modifiers.Add(new LegacyBreakpointModifier());
            }

            return modifiers;
        }

        /// <summary>
        ///     Run the adapter process.
        /// </summary>
        public void Run()
        {
            try
            {
                var threadList = new List<Thread>
                {
                    new Thread(ReadThread),
                    new Thread(WriteThread)
                };

                threadList.ForEach(entry => entry.Start());

                // Wait for all threads to end.
                threadList.ForEach(entry => entry.Join());

                if (AdapterClient != null)
                {
                    AdapterClient.Close();
                }
            }
            catch(Exception err)
            {
                if (DebugLog != null)
                {
                    DebugLog.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                    DebugLog.WriteLine(err.ToString());
                    DebugLog.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                    DebugLog.Flush();
                }
            }
        }

        /// <summary>
        ///     Process commands from VSHost to Emulicious.
        /// </summary>
        private void ReadThread()
        {
            if (AdapterClient.Connected)
            {
                HostIn.CopyTo(ToClientStream);
            }
        }

        /// <summary>
        /// Process commands from Emulicious to the VSHost.
        /// </summary>
        private void WriteThread()
        {
            if (AdapterClient.Connected)
            {
                ClientOut.CopyTo(ToHostStream);
            }
        }

        /// <inheritdoc />
        public void Dispose()
        {
            HostIn?.Dispose();
            HostOut?.Dispose();
            AdapterClient?.Dispose();
            ClientIn?.Dispose();
            ClientOut?.Dispose();
            if (DebugLog != null)
            {
                DebugLog.Flush();
                DebugLog.Close();
                DebugLog.Dispose();
            }
        }
    }
}
