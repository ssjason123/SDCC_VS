using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace EmuliciousShared
{
    /// <summary>
    ///     The emulicious Launch debugger.
    /// </summary>
    public class LaunchSettings
    {
        /// <summary>
        ///     The debugger launch mode.
        /// </summary>
        public enum RequestMode
        {
            Attach = 0,
            Launch
        }

        /// <summary>
        ///     The debugger type.
        /// </summary>
        public string DebuggerType { get; private set; }
        /// <summary>
        ///     The name of the launch command.
        /// </summary>
        public string LaunchName { get; private set; }
        /// <summary>
        ///     The debugger launch mode.
        /// </summary>
        public RequestMode Request { get; private set; }
        /// <summary>
        ///     The program to launch.
        /// </summary>
        public string Program { get; private set; }
        /// <summary>
        ///     Flag to break the debugger on starting.
        /// </summary>
        public bool StopOnEntry { get; private set; }
        /// <summary>
        ///     The collection of additional source folders.
        /// </summary>
        public IEnumerable<string> AdditionalSrcFolders { get; private set; }
        /// <summary>
        ///     The debugger port.
        /// </summary>
        public int Port { get; private set; }

        /// <summary>
        ///     Create the launch settings file.
        /// </summary>
        /// <param name="request">
        ///     The launch request mode.
        /// </param>
        /// <param name="port">
        ///     The debugger port.
        /// </param>
        /// <param name="program">
        ///     The program to launch when using Launch mode.
        /// </param>
        /// <param name="stopOnEntry">
        ///     Break the debugger on start.
        /// </param>
        /// <param name="additionalSrcFolders">
        ///     The collection additional source folders.
        /// </param>
        public LaunchSettings(RequestMode request, int port, 
            string program, bool stopOnEntry, IEnumerable<string> additionalSrcFolders)
        {
            DebuggerType = "Emulicious Debugger";
            LaunchName = "Launch Emulicious";
            Request = request;
            Program = program;
            Port = port;
            StopOnEntry = stopOnEntry;
            AdditionalSrcFolders = additionalSrcFolders;
        }

        /// <summary>
        ///     Write the the launcher json file.
        /// </summary>
        /// <param name="file">
        ///     The file to write to.
        /// </param>
        public void WriteToFile(string file)
        {
            using (var jsonFile = new JsonTextWriter(File.CreateText(file)))
            {
                jsonFile.Formatting = Formatting.Indented;

                jsonFile.WriteStartObject();

                // Type
                jsonFile.WritePropertyName("type");
                jsonFile.WriteValue(DebuggerType);

                // Name
                jsonFile.WritePropertyName("name");
                jsonFile.WriteValue(LaunchName);

                // Request type
                jsonFile.WritePropertyName("request");
                jsonFile.WriteValue(Request.ToString().ToLower());

                if (Request == RequestMode.Launch)
                {
                    // Program
                    jsonFile.WritePropertyName("program");
                    jsonFile.WriteValue(Program);

                    // Stop on entry.
                    jsonFile.WritePropertyName("stopOnEntry");
                    jsonFile.WriteValue(StopOnEntry);
                }

                if (AdditionalSrcFolders.Any())
                {
                    jsonFile.WritePropertyName("additionalSrcFolders");
                    // Build the additional source paths.
                    jsonFile.WriteValue(string.Join(";", AdditionalSrcFolders));
                }

                // Port
                jsonFile.WritePropertyName("port");
                jsonFile.WriteValue(Port);
                jsonFile.WriteEndObject();
            }
        }
    }
}
