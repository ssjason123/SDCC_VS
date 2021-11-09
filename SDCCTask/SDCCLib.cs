using System.Collections;
using Microsoft.Build.CPPTasks;

namespace SDCCTask
{
    /// <summary>
    ///     SDCC library generator tool.
    /// </summary>
    public class SDCCLib : SDCCToolBase
    {
        /// <inheritdoc/>
        protected override string AlwaysAppend
        {
            get { return "-rc"; }
            set
            {
                /* Intentionally Blank */
            }
        }

        #region File Input Output.

        public string[] SourceInput
        {
            get
            {
                if (this.IsPropertySet(nameof(SourceInput)))
                    return this.ActiveToolSwitches[nameof(SourceInput)].StringList;
                return (string [])null;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(SourceInput));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.StringPathArray);
                switchToAdd.DisplayName = "SourceInput";
                switchToAdd.Description = "Input source file(s)";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "";
                switchToAdd.Name = nameof(SourceInput);
                switchToAdd.StringList = value;
                this.ActiveToolSwitches.Add(nameof(SourceInput), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }

        public string OutputFile
        {
            get
            {
                if (this.IsPropertySet(nameof(OutputFile)))
                    return this.ActiveToolSwitches[nameof(OutputFile)].Value;
                return (string)null;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(OutputFile));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.String);
                switchToAdd.DisplayName = "OutputFile";
                switchToAdd.Description = "Output source file";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "";
                switchToAdd.Name = nameof(OutputFile);
                switchToAdd.Value = value;
                this.ActiveToolSwitches.Add(nameof(OutputFile), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }

        #endregion 

        /// <summary>
        ///     Default constructor.
        /// </summary>
        public SDCCLib()
        {
            SwitchList = new ArrayList();
            SwitchList.Add(nameof(AlwaysAppend));

            SwitchList.Add(nameof(AdditionalOptions));

            SwitchList.Add(nameof(OutputFile));
            SwitchList.Add(nameof(SourceInput));
        }

        /// <inheritdoc/>
        protected override string ToolName
        {
            get { return "sdar.exe"; }
        }
    }
}
