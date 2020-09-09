using System.Collections;
using System.Reflection;
using System.Resources;
using Microsoft.Build.CPPTasks;
using Microsoft.Build.Framework;

namespace SDCCTask
{
    public abstract class SDCCToolBase : TrackedVCToolTask
    {
        protected ArrayList SwitchList = null;
        protected string TrackerLogDirectoryName;
        protected ITaskItem[] SourceItems;

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

        [Required]
        public virtual ITaskItem[] Sources
        {
            get { return this.SourceItems; }
            set { this.SourceItems = value; }
        }

        public SDCCToolBase()
            : base(new ResourceManager("SDCCTask.Strings", Assembly.GetExecutingAssembly()))
        {
            this.UseCommandProcessor = false;
            this.EchoOff = false;
        }

        public override string ToolExe
        {
            get { return ToolName; }
            set
            {
                // Intentionally empty.
            }
        }

        protected override string ToolName
        {
            get
            {
                return string.Empty;
            }
        }

        protected override string TrackerIntermediateDirectory
        {
            get
            {
                return this.TrackerLogDirectory ?? string.Empty;
            }
        }
        protected override ITaskItem[] TrackedInputFiles
        {
            get
            {
                return this.Sources;
            }
        }

        protected override ArrayList SwitchOrderList
        {
            get
            {
                return SwitchList;
            }
        }

        protected override string GenerateResponseFileCommands(CommandLineFormat format, EscapeFormat escapeFormat)
        {
            // DO NOT USE RESPONSE FILE GENERATION.
            // return base.GenerateResponseFileCommands(format, escapeFormat);
            return string.Empty;
        }

        protected override string GenerateCommandLineCommands(CommandLineFormat format, EscapeFormat escapeFormat)
        {
            // Use the response file command line generation for the normal command line.
            return base.GenerateResponseFileCommands(format, escapeFormat);
        }

        protected override void LogToolCommand(string message)
        {
            this.PrintMessage(new MessageStruct()
                {Text=message}, MessageImportance.High);
            //this.LogPrivate.LogCommandLine(MessageImportance.High, message);
        }
    }
}
