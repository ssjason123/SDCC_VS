using System;
using System.IO;
using Task = Microsoft.Build.Utilities.Task;

namespace SDCCTask
{
    public class SDCCLibCdbGenerator : Task
    {
        public string[] InputFiles { get; set; }
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
