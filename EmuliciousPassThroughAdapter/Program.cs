using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace EmuliciousPassThroughAdapter
{
    public class Program
    {
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
            var mapping = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(projectMapping));

            // Start capturing STD In/Out so we dont lose any 
            var adapter = new PassthroughAdapter(Console.OpenStandardInput(), Console.OpenStandardOutput(), 
                mapping, port, debugPath);

            adapter.Run();
            adapter.Dispose();
        }
    }
}
