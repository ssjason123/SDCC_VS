using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.ProjectSystem;
using Microsoft.VisualStudio.ProjectSystem.Debug;
using Microsoft.VisualStudio.ProjectSystem.Properties;
using Microsoft.VisualStudio.ProjectSystem.VS.Debug;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Threading;
using Newtonsoft.Json;
using Task = System.Threading.Tasks.Task;

namespace EmuliciousDebuggerPackage.Debugger.VisualStudio
{
    /// <summary>
    /// Debugger launcher to start the emulicious Host Debug Adapter.
    /// </summary>
    [ExportDebugger(EmuliciousDebugger.SchemaName)]
    [AppliesTo(ProjectCapabilities.VisualC)]
    public class EmuliciousDebuggerLaunchProvider : DebugLaunchProviderBase
    {
        /// <summary>
        /// The file location to the emulicious Executable.
        /// </summary>
        public static string EmuliciousExecutable = "";
        /// <summary>
        /// The emulicious port.
        /// </summary>
        public static int EmuliciousPort = 58870;
        /// <summary>
        /// Delay value before attempting to connect to the emulicious instance.
        /// </summary>
        public static int EmuliciousLaunchDelay = 1000;
        /// <summary>
        /// The debug folder for logging output.
        /// </summary>
        public static string EmuliciousDebugFolder = "";
        /// <summary>
        /// Attach mode flag, true to attach, false to launch.
        /// </summary>
        public static bool EmuliciousAttach = false;

        /// <summary>
        /// Task factory for the main thread synchronization.
        /// </summary>
        private JoinableTaskFactory TaskFactory;
        /// <summary>
        /// DTE instance for project details.
        /// </summary>
        private DTE2 VisualStudio;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="configuredProject">The configured project instance.</param>
        [ImportingConstructor]
        public EmuliciousDebuggerLaunchProvider(ConfiguredProject configuredProject)
            : base(configuredProject)
        {
            TaskFactory = new JoinableTaskFactory(ThreadHelper.JoinableTaskContext);
            VisualStudio = (DTE2)Package.GetGlobalService(typeof(SDTE));
        }

        // TODO: Specify the assembly full name here
        [ExportPropertyXamlRuleDefinition("EmuliciousDebuggerPackage.Debugger.VisualStudio, Version=1.0.0.0, Culture=neutral, PublicKeyToken=9be6e469bc4921f1", "XamlRuleToCode:EmuliciousDebugger.xaml", "Project")]
        [AppliesTo(ProjectCapabilities.VisualC)]
        private object DebuggerXaml { get { throw new NotImplementedException(); } }

        /// <summary>
        /// Gets project properties that the debugger needs to launch.
        /// </summary>
        [Import]
        private ProjectProperties DebuggerProperties { get; set; }

        /// <summary>
        /// Can launch the debugger.
        /// </summary>
        /// <param name="launchOptions">The debug launch options.</param>
        /// <returns>True to launch the debugger, false otherwise.</returns>
        public override async Task<bool> CanLaunchAsync(DebugLaunchOptions launchOptions)
        {
            var properties = await this.DebuggerProperties.GetEmuliciousDebuggerPropertiesAsync();
            string commandValue = await properties.EmuliciousDebuggerExecutable.GetEvaluatedValueAtEndAsync();
            return !string.IsNullOrEmpty(commandValue) && File.Exists(commandValue);
        }

        /// <inheritdoc />
        public override async Task<IReadOnlyList<IDebugLaunchSettings>> QueryDebugTargetsAsync(DebugLaunchOptions launchOptions)
        {
            // This is not actually used due to the LaunchAsync override.
            var settings = new DebugLaunchSettings(launchOptions);

            return await Task.FromResult<IReadOnlyList<IDebugLaunchSettings>>(new IDebugLaunchSettings[] { settings });
        }

        /// <inheritdoc />
        public override async Task LaunchAsync(DebugLaunchOptions launchOptions)
        {
            var props = ConfiguredProject.Services.ProjectPropertiesProvider.GetCommonProperties();

            var debugDir = await props.GetEvaluatedPropertyValueAsync("OutDir");

            await TaskFactory.SwitchToMainThreadAsync();

            // Get the projects for folder mapping.
            var projectPairs = new Dictionary<string, string>();
            foreach (Project project in VisualStudio.Solution.Projects)
            {
                var fullName = project.FullName;
                if (!string.IsNullOrEmpty(fullName))
                {
                    projectPairs.Add(project.Name, Path.GetDirectoryName(fullName));
                }
            }

            CreateProjectJunctions(projectPairs, debugDir);

            // Create a ProjectMapping.json file, to map the debug -> project junction mappings.
            File.WriteAllText(Path.Combine(debugDir, "ProjectMapping.json"),
                JsonConvert.SerializeObject(projectPairs.ToDictionary(
                    pair => pair.Value, pair => Path.Combine(debugDir, "ProjectDirs", pair.Key)), Formatting.Indented));

            var properties = await DebuggerProperties.GetEmuliciousDebuggerPropertiesAsync();
            var executable = (string) await properties.EmuliciousDebuggerExecutable.GetValueAsync();
            var romName = (string) await properties.EmuliciousLaunchRom.GetValueAsync();
            var attach = (bool) await properties.EmuliciousDebuggerDebuggerAttach.GetValueAsync();
            var stopOnEntry = (bool) await properties.EmuliciousDebuggerDebuggerStopOnEntry.GetValueAsync();
            var debuggerPort = (int) await properties.EmuliciousDebuggerPort.GetValueAsync();
            var startDelay = (int) await properties.EmuliciousDebuggerStartDelay.GetValueAsync();
            var debugPath = (string) await properties.EmuliciousDebugLogPath.GetValueAsync();

            // Cache properties used by the launch adapter.
            EmuliciousExecutable = executable;
            EmuliciousPort = debuggerPort;
            EmuliciousAttach = attach;
            EmuliciousLaunchDelay = startDelay;
            EmuliciousDebugFolder = debugPath;

            // Generate the launch.json file for emulicious.
            var jsonFile = Path.Combine(debugDir, "launch.json");
            GenerateLaunchJson(jsonFile, romName,
                attach,
                stopOnEntry, 
                debuggerPort);

            // Launch the debug host adapter with our JSON file.
            var launchArgs = "/EngineGuid:{BE99C8E2-969A-450C-8FAB-73BECCC53DF4} " + string.Format("/LaunchJson:\"{0}\"",
                jsonFile);
            VisualStudio.ExecuteCommand("DebugAdapterHost.Launch", launchArgs);
        }

        /// <summary>
        /// Create the debugging->project junctions for emulicious.
        /// </summary>
        /// <param name="solutionDir">The path to map the junction to.</param>
        /// <param name="junctionPath">The junction location.</param>
        protected void CreateSolutionJunction(string solutionDir, string junctionPath)
        {
            if (!File.Exists(junctionPath))
            {
                var psi = new ProcessStartInfo("cmd.exe",
                    " /C mklink /J \"" + junctionPath + "\" \"" + solutionDir + "\"");
                psi.CreateNoWindow = true;
                psi.UseShellExecute = false;
                System.Diagnostics.Process.Start(psi).WaitForExit();
            }
        }

        /// <summary>
        /// Create the junctions for the entire project.
        /// </summary>
        /// <param name="projectDirs">The junction name/path mapping.</param>
        /// <param name="junctionRoot">The root junction path.</param>
        protected void CreateProjectJunctions(Dictionary<string, string> projectDirs, string junctionRoot)
        {
            // Create a junction folder for each project.
            var rootFolder = Path.Combine(junctionRoot, "ProjectDirs");
            if (!Directory.Exists(rootFolder))
            {
                Directory.CreateDirectory(rootFolder);

                // Create a junction for each project dir.
                foreach (var projectDir in projectDirs)
                {
                    var junctionName = Path.Combine(rootFolder, projectDir.Key);
                    CreateSolutionJunction(projectDir.Value, junctionName);
                }
            }
        }

        /// <summary>
        /// Generate the launch Json file.
        /// </summary>
        /// <param name="filePath">The launch.json file path.</param>
        /// <param name="romFile">The rom file location.</param>
        /// <param name="attach">True to attach to emulicious, false to launch a new instance.</param>
        /// <param name="stopOnEntry">Flag to break the debugger on start when using launch mode.</param>
        /// <param name="port">The emulicious debugger port.</param>
        protected void GenerateLaunchJson(string filePath, string romFile, bool attach, bool stopOnEntry, int port)
        {
            using (var jsonFile = new JsonTextWriter(File.CreateText(filePath)))
            {
                jsonFile.Formatting = Formatting.Indented;

                jsonFile.WriteStartObject();

                // Type
                jsonFile.WritePropertyName("type");
                jsonFile.WriteValue("Emulicious Debugger");

                // Name
                jsonFile.WritePropertyName("name");
                jsonFile.WriteValue("Launch Emulicious");

                if (attach)
                {
                    // Request type
                    jsonFile.WritePropertyName("request");
                    jsonFile.WriteValue("attach");
                }
                else
                {
                    // Request type
                    jsonFile.WritePropertyName("request");
                    jsonFile.WriteValue("launch");

                    // Program
                    jsonFile.WritePropertyName("program");
                    jsonFile.WriteValue(romFile);

                    // Stop on entry.
                    jsonFile.WritePropertyName("stopOnEntry");
                    jsonFile.WriteValue(stopOnEntry);
                }

                // Port
                jsonFile.WritePropertyName("port");
                jsonFile.WriteValue(port);
                jsonFile.WriteEndObject();
            }
        }
    }

}
