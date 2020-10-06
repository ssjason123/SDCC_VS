using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.Build.Framework;
using Task = Microsoft.Build.Utilities.Task;

namespace SDCCTask
{
    public class SDCCVersion : Task
    {
        public string SDCCExecutable { get; set; }

        [Output]
        public uint MajorVersion { get; set; }
        [Output]
        public uint MinorVersion { get; set; }
        [Output]
        public uint PatchVersion { get; set; }
        [Output]
        public string Version { get; set; }
        [Output]
        public uint Revision { get; set; }


        /// <inheritdoc />
        public override bool Execute()
        {
            var result = false;
            // Reset the version info.
            MajorVersion = 0;
            MinorVersion = 0;
            PatchVersion = 0;
            Version = "0.0.0";
            Revision = 0;

            // Run the SDCC command and extract the version information.
            if (File.Exists(SDCCExecutable))
            {
                // Execute sdcc.exe -v and regex the version/revision info.
                var processInfo = new ProcessStartInfo(SDCCExecutable, "-v");
                processInfo.WindowStyle = ProcessWindowStyle.Hidden;
                processInfo.RedirectStandardOutput = true;
                processInfo.UseShellExecute = false;
                processInfo.CreateNoWindow = true;

                using (var process = Process.Start(processInfo))
                {
                    process.WaitForExit();

                    var commandLine = process.StandardOutput.ReadToEnd();
                    var match = Regex.Match(commandLine, @"([0-9]+)\.([0-9]+)\.([0-9]+) #([0-9]+)");

                    if (match.Success)
                    {
                        MajorVersion = uint.Parse(match.Groups[1].Value);
                        MinorVersion = uint.Parse(match.Groups[2].Value);
                        PatchVersion = uint.Parse(match.Groups[3].Value);
                        Revision = uint.Parse(match.Groups[4].Value);
                        Version = string.Format("{0}.{1}.{2}", MajorVersion, MinorVersion, PatchVersion);
                        result = true;
                    }
                }
            }

            return result;
        }
    }
}
