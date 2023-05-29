using System;
using System.Collections.Generic;
using System.IO;
using EmuliciousShared;

namespace EmuliciousPassThroughAdapter
{
    /// <summary>
    ///     Pass through adapter program.
    /// </summary>
    public class Program
    {
        /// <summary>
        ///     Main application entry point.
        /// </summary>
        /// <param name="args">
        ///     Arguments.
        /// </param>
        public static void Main(string[] args)
        {
#if __DEBUGGER_WAIT__
            bool loop = true;
            while (loop)
            {
                Thread.Sleep(1);
            }
#endif

            // Emulicious Debug port.
            var port = int.Parse(args[0]);
            // Path to write communication logs to.
            var debugPath = args[1].Trim('"');
            // Mapping file path.
            var mappingPath = args[2].Trim('"');

            // Path to the project mapping file.
            var projectMapping = Path.Combine(mappingPath, "ProjectMapping.json");
            var settings = PassthroughSettings.LoadFromFile(projectMapping);

            // Start capturing STD In/Out so we dont lose any 
            var adapter = new PassthroughAdapter(Console.OpenStandardInput(), Console.OpenStandardOutput(), 
                settings, port);

            adapter.Run();
            adapter.Dispose();
        }
    }
}
