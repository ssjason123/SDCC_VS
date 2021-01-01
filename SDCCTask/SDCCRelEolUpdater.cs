using System.IO;
using Task = Microsoft.Build.Utilities.Task;

namespace SDCCTask
{
    public class SDCCRelEolUpdater : Task
    {
        public string SourceFile { get; set; }

        /// <inheritdoc />
        public override bool Execute()
        {
            // Update the files with the correct eol
            if(File.Exists(SourceFile))
            {
                var fileData = File.ReadAllText(SourceFile);
                fileData = fileData.Replace("\r\n", "\n");
                File.WriteAllText(SourceFile, fileData);
            }

            return true;
        }
    }
}
