using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EmuliciousShared;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.ProjectSystem;
using Microsoft.VisualStudio.ProjectSystem.Debug;
using Microsoft.VisualStudio.ProjectSystem.Properties;
using Microsoft.VisualStudio.ProjectSystem.VS.Debug;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Threading;
using Task = System.Threading.Tasks.Task;
using Thread = System.Threading.Thread;

namespace EmuliciousDebuggerPackage.Debugger.VisualStudio
{
    /// <summary>
    ///     Debugger launcher to start the emulicious Host Debug Adapter.
    /// </summary>
    [ExportDebugger(EmuliciousDebugger.SchemaName)]
    [AppliesTo(ProjectCapabilities.VisualC)]
    public class EmuliciousDebuggerLaunchProvider : DebugLaunchProviderBase
    {
        /// <summary>
        ///     The file location to the emulicious Executable.
        /// </summary>
        public static string EmuliciousExecutable = "";
        /// <summary>
        ///     The emulicious port.
        /// </summary>
        public static int EmuliciousPort = 58870;
        /// <summary>
        ///     Delay value before attempting to connect to the emulicious instance.
        /// </summary>
        public static int EmuliciousLaunchDelay = 1000;
        /// <summary>
        ///     The debug folder for logging output.
        /// </summary>
        public static string EmuliciousDebugFolder = "";
        /// <summary>
        ///     Attach mode flag, true to attach, false to launch.
        /// </summary>
        public static bool EmuliciousAttach = false;
        /// <summary>
        ///     Path to the mapping file.
        /// </summary>
        public static string EmuliciousMappingPath = "";
        /// <summary>
        ///     Argument that enables passing the remote debug argument to emulicious on process start.
        /// </summary>
        public static bool EmuliciousRemoteDebugArgument = true;
        /// <summary>
        ///     Use Junction based source mapping and debugger pass through re-mapping.
        /// </summary>
        public static bool EmuliciousLegacySourceFolders = false;
        /// <summary>
        ///     Use Junction based source mapping and debugger pass through re-mapping.
        /// </summary>
        public static bool EmuliciousLegacyBreakpointFix = false;


        /// <summary>
        ///     Task factory for the main thread synchronization.
        /// </summary>
        private JoinableTaskFactory TaskFactory;
        /// <summary>
        ///     DTE instance for project details.
        /// </summary>
        private DTE2 VisualStudio;

        /// <summary>
        ///     Default constructor.
        /// </summary>
        /// <param name="configuredProject">
        ///     The configured project instance.
        /// </param>
        [ImportingConstructor]
        public EmuliciousDebuggerLaunchProvider(ConfiguredProject configuredProject)
            : base(configuredProject)
        {
            TaskFactory = new JoinableTaskFactory(ThreadHelper.JoinableTaskContext);
            VisualStudio = (DTE2)Package.GetGlobalService(typeof(SDTE));
        }

        /// <summary>
        ///     Debugger Xaml binding.
        /// </summary>
        /// <remarks>
        ///     NOTE: Specify the assembly full name here
        /// </remarks>
        [ExportPropertyXamlRuleDefinition("EmuliciousDebuggerPackage.Debugger.VisualStudio, Version=1.0.0.0, Culture=neutral, PublicKeyToken=9be6e469bc4921f1", "XamlRuleToCode:EmuliciousDebugger.xaml", "Project")]
        [AppliesTo(ProjectCapabilities.VisualC)]
        private object DebuggerXaml { get { throw new NotImplementedException(); } }

        /// <summary>
        ///     Gets project properties that the debugger needs to launch.
        /// </summary>
        [Import]
        private ProjectProperties DebuggerProperties { get; set; }

        /// <summary>
        ///     Can launch the debugger.
        /// </summary>
        /// <param name="launchOptions">
        ///     The debug launch options.
        /// </param>
        /// <returns>
        ///     True to launch the debugger, false otherwise.
        /// </returns>
        public override async Task<bool> CanLaunchAsync(DebugLaunchOptions launchOptions)
        {
            var properties = await this.DebuggerProperties.GetEmuliciousDebuggerPropertiesAsync();
            var commandValue = (await properties.EmuliciousDebuggerExecutable.GetValueAsync()).ToString();
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

            // Collect the source folders to map.
            var properties = await DebuggerProperties.GetEmuliciousDebuggerPropertiesAsync();
            var sourcePaths = (IReadOnlyCollection<string>)await properties.EmuliciousSourcePaths.GetValueAsync();

            // Get the projects for folder mapping.
            var projectPairs = new Dictionary<string, string>();

            if (sourcePaths == null || sourcePaths.Count == 0)
            {
                // Bind every project.
                await RecurseProjectsAsync(VisualStudio.Solution.Projects, projectPairs);
            }
            else
            {
                var index = 0;
                foreach (var path in sourcePaths.Distinct())
                {
                    projectPairs.Add(string.Format("SourcePath{0}", index),
                        Path.GetFullPath(path).TrimEnd('/', '\\'));
                    ++index;
                }
            }
            
            if (EmuliciousLegacySourceFolders)
            {
                // Create junctions to the source files.
                CreateProjectJunctions(projectPairs, debugDir);
            }

            var executable = (string) await properties.EmuliciousDebuggerExecutable.GetValueAsync();
            var romName = (string) await properties.EmuliciousLaunchRom.GetValueAsync();
            var attach = (bool) await properties.EmuliciousDebuggerDebuggerAttach.GetValueAsync();
            var stopOnEntry = (bool) await properties.EmuliciousDebuggerDebuggerStopOnEntry.GetValueAsync();
            var debuggerPort = (int) await properties.EmuliciousDebuggerPort.GetValueAsync();
            var startDelay = (int) await properties.EmuliciousDebuggerStartDelay.GetValueAsync();
            var debugPath = (string) await properties.EmuliciousDebugLogPath.GetValueAsync();
            var useLegacySrc = (bool) await properties.EmuliciousLegacySourceFolders.GetValueAsync();
            var useLegacyBreakPointFix = (bool) await properties.EmuliciousLegacyBreakpointFix.GetValueAsync();
            var useRemoteDebugArg = (bool) await properties.EmuliciousRemoteDebugArgument.GetValueAsync();

            // Cache properties used by the launch adapter.
            EmuliciousExecutable = executable;
            EmuliciousPort = debuggerPort;
            EmuliciousAttach = attach;
            EmuliciousLaunchDelay = startDelay;
            EmuliciousDebugFolder = debugPath;
            EmuliciousMappingPath = debugDir;
            EmuliciousRemoteDebugArgument = useRemoteDebugArg;
            EmuliciousLegacySourceFolders = useLegacySrc;
            EmuliciousLegacyBreakpointFix = useLegacyBreakPointFix;


            // Create the PassthroughAdapter settings file.
            var mapping = new PassthroughSettings(EmuliciousLegacySourceFolders, EmuliciousLegacyBreakpointFix, projectPairs.Values, 
                EmuliciousMappingPath, EmuliciousDebugFolder);
            mapping.SaveToFile(Path.Combine(debugDir, "ProjectMapping.json"));

            /*
            File.WriteAllLines(Path.Combine(EmuliciousMappingPath, "LaunchProps.log"),
                new []
                {
                    EmuliciousExecutable.ToString(),
                    EmuliciousPort.ToString(),
                    EmuliciousAttach.ToString(),
                    EmuliciousLaunchDelay.ToString(),
                    EmuliciousDebugFolder.ToString(),
                    EmuliciousMappingPath.ToString(),
                    EmuliciousPackage.DebugAdapterPath.ToString()
                });
                */

            // Generate the launch.json file for emulicious.
            var jsonFile = Path.Combine(debugDir, "launch.json");
            var launchSettings = new LaunchSettings((attach) ? LaunchSettings.RequestMode.Attach : LaunchSettings.RequestMode.Launch,
                debuggerPort, romName, stopOnEntry, projectPairs.Values);
            launchSettings.WriteToFile(jsonFile);

            Task.WaitAll();

            // Launch the debug host adapter with our JSON file.
            var launchArgs = "/EngineGuid:{BE99C8E2-969A-450C-8FAB-73BECCC53DF4} " + string.Format("/LaunchJson:\"{0}\"",
                jsonFile);
            VisualStudio.ExecuteCommand("DebugAdapterHost.Launch", launchArgs);
        }

        /// <summary>
        ///     Get the collection of projects.
        /// </summary>
        /// <param name="projects">
        ///     The projects to iterate.
        /// </param>
        /// <param name="mappings">
        ///     The resulting project mappings.
        /// </param>
        protected async Task RecurseProjectsAsync(Projects projects, Dictionary<string, string> mappings)
        {
            if (projects != null)
            {
                foreach (Project project in projects)
                {
                    await ProcessProjectAsync(project, mappings);
                }
            }
        }

        /// <summary>
        ///     Process the projects folder.
        /// </summary>
        /// <param name="project">
        ///     The project.
        /// </param>
        /// <param name="mappings">
        ///     The mappings.
        /// </param>
        /// <returns>
        ///     The async tasks.
        /// </returns>
        protected async Task ProcessProjectAsync(Project project, Dictionary<string, string> mappings)
        {
            await TaskFactory.SwitchToMainThreadAsync();

            if (project != null)
            {
                var fullName = project.FullName;
                if (!string.IsNullOrEmpty(fullName))
                {
                    mappings.Add(project.Name, Path.GetDirectoryName(fullName));
                }

                if (project.Kind == EnvDTE.Constants.vsProjectKindSolutionItems)
                {
                    foreach (ProjectItem item in project.ProjectItems)
                    {
                        await ProcessProjectAsync((Project)item.Object, mappings);
                    }
                }
            }
        }

        /// <summary>
        ///     Create the debugging->project junctions for emulicious.
        /// </summary>
        /// <param name="solutionDir">
        ///     The path to map the junction to.
        /// </param>
        /// <param name="junctionPath">
        ///     The junction location.
        /// </param>
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
        ///     Create the junctions for the entire project.
        /// </summary>
        /// <param name="projectDirs">
        ///     The junction name/path mapping.
        /// </param>
        /// <param name="junctionRoot">
        ///     The root junction path.
        /// </param>
        protected void CreateProjectJunctions(Dictionary<string, string> projectDirs, string junctionRoot)
        {
            if (projectDirs.Count > 0)
            {
                // Create a junction folder for each project.
                var rootFolder = Path.Combine(junctionRoot, "ProjectDirs");
                if (!Directory.Exists(rootFolder))
                {
                    // Clean up the folder and create new links.
                    Directory.CreateDirectory(rootFolder);
                }

                // Create paths to monitor.
                var projectMapping = projectDirs.ToDictionary(path => path.Value,
                    path => new DirectoryInfo(Path.Combine(rootFolder, path.Key)));

                // Clean up any existing paths.
                var removePaths = projectMapping.Values.Where(info => info.Exists).ToList();
                removePaths.ForEach(info =>
                {
                    info.Delete();
                    info.Refresh();
                });

                while (removePaths.Any(info => info.Exists))
                {
                    Thread.Sleep(100);
                    removePaths.ForEach(info => info.Refresh());
                }

                // Create the new paths
                foreach (var pair in projectMapping)
                {
                    CreateSolutionJunction(pair.Key, pair.Value.FullName);
                }

                // Wait until all paths have been created successfully.
                var createdPaths = projectMapping.Values.ToList();
                createdPaths.ForEach(info => info.Refresh());
                while (!createdPaths.All(value => value.Exists))
                {
                    Thread.Sleep(100);
                    removePaths.ForEach(info => info.Refresh());
                }
            }
        }
    }
}
