using System.IO;
using Task = Microsoft.Build.Utilities.Task;

namespace SDCCTask
{
    /// <summary>
    ///     SDCCLib CDB file generator. Combines all files into a single
    ///     CDB.
    /// </summary>
    public class SDCCLibCdbGenerator : Task
    {
        /// <summary>
        ///     The collection of input files to combine.
        /// </summary>
        public string[] InputFiles { get; set; }
        /// <summary>
        ///     The target output file.
        /// </summary>
        public string OutputFile { get; set; }

        /// <inheritdoc />
        public override bool Execute()
        {
            bool valid = true;

            using (var outFile = File.OpenWrite(OutputFile))
            {
                using (var writer = new StreamWriter(outFile))
                {
                    foreach (var file in InputFiles)
                    {
                        if (File.Exists(file))
                        {
                            writer.Write(File.ReadAllText(file));
                        }
                    }
                }

                outFile.Close();
            }

            return valid;
        }
    }
}
