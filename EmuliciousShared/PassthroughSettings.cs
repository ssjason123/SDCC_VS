using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace EmuliciousShared
{
    /// <summary>
    ///     Class used to wrap up project settings.
    /// </summary>
    public class PassthroughSettings
    {
        /// <summary>
        ///     The output directory path.
        /// </summary>
        public string OutputPath { get; private set; }
        /// <summary>
        ///     The collection of source folders.
        /// </summary>
        public IEnumerable<string> SourceFolders { get; private set; }
        /// <summary>
        ///     Path to log debug files to.
        /// </summary>
        public string DevelopmentPath { get; private set; }

        /// <summary>
        ///     Flag to use the legacy source folder mapping.
        /// </summary>
        public bool UseLegacySourceFolders { get; private set; }
        /// <summary>
        ///     Flag to use the legacy breakpoint fix or not.
        /// </summary>
        public bool UseLegacyBreakpointFix { get; private set; }

        /// <summary>
        ///     Default constructor.
        /// </summary>
        /// <param name="useLegacySourceFolders">
        ///     Flag to use legacy source folders.
        /// </param>
        /// <param name="useLegacyBreakpointFix">
        ///     Flag to use the legacy breakpoint fix.
        /// </param>
        /// <param name="sourceFolders">
        ///     The source folder locations.
        /// </param>
        /// <param name="outputPath">
        ///     The debug output folder.
        /// </param>
        /// <param name="developmentPath">
        ///     Development path for logging output.
        /// </param>
        public PassthroughSettings(bool useLegacySourceFolders, bool useLegacyBreakpointFix,
            IEnumerable<string> sourceFolders, string outputPath, string developmentPath)
        {
            OutputPath = outputPath;
            UseLegacySourceFolders = useLegacySourceFolders;
            UseLegacyBreakpointFix = useLegacyBreakpointFix;
            SourceFolders = sourceFolders;
            DevelopmentPath = developmentPath;
        }

        /// <summary>
        ///     Get the legacy source path locations.
        /// </summary>
        /// <param name="subPath">
        ///     The sub folder path identifier.
        /// </param>
        /// <returns>
        ///     The legacy project mappings.
        /// </returns>
        public IDictionary<string, string> GetLegacySourceFolders(string subPath = "ProjectDirs")
        {
            return SourceFolders.Select((value, index) => new KeyValuePair<string, int>(value, index))
                .ToDictionary((pair) => pair.Key, (pair) => Path.Combine(OutputPath, subPath, pair.Value.ToString()));
        }

        /// <summary>
        ///     Save this to a file.
        /// </summary>
        /// <param name="file">
        ///     The file to save to.
        /// </param>
        public void SaveToFile(string file)
        {
            File.WriteAllText(file,
                JsonConvert.SerializeObject(this, Formatting.Indented));
        }

        /// <summary>
        ///     Load <see cref="PassthroughSettings"/> from a file.
        /// </summary>
        /// <param name="file">
        ///     The file to load from.
        /// </param>
        /// <returns>
        ///     The settings loaded from the file.s
        /// </returns>
        public static PassthroughSettings LoadFromFile(string file)
        {
            return JsonConvert.DeserializeObject<PassthroughSettings>(File.ReadAllText(file));
        }
    }
}
