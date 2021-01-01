using System.IO;
using System.Linq;
using Microsoft.Build.Framework;
using Task = Microsoft.Build.Utilities.Task;

namespace SDCCTask
{
    public class SDCCResolveLibraries : Task
    {
        public string[] Libraries { get; set; }
        public string[] Directories { get; set; }

        [Output]
        public string[] FullLibraries { get; set; }

        /// <inheritdoc />
        public override bool Execute()
        {
            var result = true;

            if (Libraries != null && Libraries.Length > 0)
            {
                FullLibraries = new string[Libraries.Length];

                // Resolve the libraries input.
                foreach (var index in Enumerable.Range(0, Libraries.Length))
                {
                    // Find the first directory with the requested assembly.
                    var library = Libraries[index];
                    string foundName = string.Empty;

                    if (!string.IsNullOrEmpty(library))
                    {
                        foreach (var directory in Directories)
                        {
                            var tempPath = Path.Combine(directory, library);
                            if (File.Exists(tempPath))
                            {
                                foundName = tempPath;
                                break;
                            }
                        }

                        if (string.IsNullOrEmpty(foundName))
                        {
                            result = false;
                        }
                    }

                    FullLibraries[index] = foundName;
                }
            }

            return result;
        }
    }
}
