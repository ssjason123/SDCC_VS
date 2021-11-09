using System.Collections;
using System.Reflection;
using System.Resources;
using Microsoft.Build.CPPTasks;
using Microsoft.Build.Framework;

namespace SDCCTask
{
    /// <summary>
    ///     Base task for SDCC tools.
    /// </summary>
    public abstract class SDCCToolBase : TrackedVCToolTask
    {
        /// <summary>
        ///     List of switches.
        /// </summary>
        protected ArrayList SwitchList = null;
        /// <summary>
        ///     Tracker log directory name.
        /// </summary>
        protected string TrackerLogDirectoryName;
        /// <summary>
        ///     The collection of source files to work on.
        /// </summary>
        protected ITaskItem[] SourceItems;

        /// <summary>
        ///     Tracker log directory.
        /// </summary>
        public virtual string TrackerLogDirectory
        {
            get
            {
                return this.TrackerLogDirectoryName;
            }
            set
            {
                this.TrackerLogDirectoryName = VCToolTask.EnsureTrailingSlash(value);
            }
        }

        /// <summary>
        ///     The source items to operate on.
        /// </summary>
        [Required]
        public virtual ITaskItem[] Sources
        {
            get { return this.SourceItems; }
            set { this.SourceItems = value; }
        }

        /// <summary>
        ///     Default constructor.
        /// </summary>
        public SDCCToolBase()
            : base(new ResourceManager("SDCCTask.Strings", Assembly.GetExecutingAssembly()))
        {
            this.UseCommandProcessor = false;
            this.EchoOff = false;
        }

        /// <summary>
        ///     The tool executable name.
        /// </summary>
        public override string ToolExe
        {
            get { return ToolName; }
            set
            {
                // Intentionally empty.
            }
        }

        /// <summary>
        ///     The tool file name.
        /// </summary>
        protected override string ToolName
        {
            get
            {
                return string.Empty;
            }
        }

        /// <summary>
        ///     The tracker file intermediate directory.
        /// </summary>
        protected override string TrackerIntermediateDirectory
        {
            get
            {
                return this.TrackerLogDirectory ?? string.Empty;
            }
        }

        /// <summary>
        ///     The tracked input files.
        /// </summary>
        protected override ITaskItem[] TrackedInputFiles
        {
            get
            {
                return this.Sources;
            }
        }

        /// <summary>
        ///     The list of ordered switches.
        /// </summary>
        protected override ArrayList SwitchOrderList
        {
            get
            {
                return SwitchList;
            }
        }

        /// <inheritdoc/>
        protected override string GenerateResponseFileCommands(CommandLineFormat format, EscapeFormat escapeFormat)
        {
            // DO NOT USE RESPONSE FILE GENERATION.
            // return base.GenerateResponseFileCommands(format, escapeFormat);
            return string.Empty;
        }

        /// <inheritdoc/>
        protected override string GenerateCommandLineCommands(CommandLineFormat format, EscapeFormat escapeFormat)
        {
            // Use the response file command line generation for the normal command line.
            return base.GenerateResponseFileCommands(format, escapeFormat);
        }

        /// <inheritdoc/>
        protected override void LogToolCommand(string message)
        {
            this.PrintMessage(new MessageStruct()
                {Text=message}, MessageImportance.High);
        }
    }
}
