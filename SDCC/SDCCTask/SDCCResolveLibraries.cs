using System.IO;
using System.Linq;
using Microsoft.Build.Framework;
using Task = Microsoft.Build.Utilities.Task;

namespace SDCCTask
{
    /// <summary>
    ///     Task to resolve all the SDCC lib files.
    /// </summary>
    public class SDCCResolveLibraries : Task
    {
        /// <summary>
        ///     The libraries to resolve.
        /// </summary>
        public string[] Libraries { get; set; }
        /// <summary>
        ///     The library source folders.
        /// </summary>
        public string[] Directories { get; set; }

        /// <summary>
        ///     The library files resolved to their full paths.
        /// </summary>
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
