using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace SDCCTask
{
    internal static class ModuleInitializer
    {
        internal static void Run()
        {
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
        }

        static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            Assembly result = null;
            if (args.Name == "Microsoft.Build.CPPTasks.Common")
            {
                // Find the VC install directories.
                var vcWherePath = Environment.GetEnvironmentVariable("ProgramFiles(x86)");
                var wherePath = Path.Combine(vcWherePath, @"Microsoft Visual Studio\Installer\vswhere.exe");

                if (!File.Exists(wherePath))
                {
                    vcWherePath = Environment.GetEnvironmentVariable("ProgramFiles");
                    wherePath = Path.Combine(vcWherePath, @"Microsoft Visual Studio\Installer\vswhere.exe");

                    if (!File.Exists(wherePath))
                    {
                        throw new FileNotFoundException("Unable to find 'vswhere.exe', cannot resolve location of: " + args.Name);
                    }
                }

                var processInfo = new ProcessStartInfo(wherePath)
                {
                    Arguments = "-format xml",
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardOutput = true
                };
                var process = new Process()
                {
                    StartInfo = processInfo
                };
                process.Start();
                string xmlOutput = process.StandardOutput.ReadToEnd();

                process.WaitForExit();

                // Search the visual studio installation paths for the assembly.
                var xmlData = XDocument.Parse(xmlOutput);
                var installDirs = xmlData.Root.Elements("instance").Elements("installationPath").ToList();

                foreach (var installDir in installDirs)
                {
                    var files = Directory.EnumerateFiles(installDir.Value, args.Name + ".dll", SearchOption.AllDirectories);
                    foreach (var file in files)
                    {
                        result = Assembly.LoadFrom(file);
                        return result;
                    }
                }
            }

            return result;
        }
    }
}
