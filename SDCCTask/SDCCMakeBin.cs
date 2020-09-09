using System.Collections;
using Microsoft.Build.CPPTasks;

namespace SDCCTask
{
    public class SDCCMakeBin : SDCCToolBase
    {
        #region File Input Output.

        public string SourceInput
        {
            get
            {
                if (this.IsPropertySet(nameof(SourceInput)))
                    return this.ActiveToolSwitches[nameof(SourceInput)].Value;
                return (string)null;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(SourceInput));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.String);
                switchToAdd.DisplayName = "SourceInput";
                switchToAdd.Description = "Input source file(s)";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "";
                switchToAdd.Name = nameof(SourceInput);
                switchToAdd.Value = value;
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

        #region Options

        public bool PackBinary
        {
            get
            {
                if (this.IsPropertySet(nameof(PackBinary)))
                    return this.ActiveToolSwitches[nameof(PackBinary)].BooleanValue;
                return false;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(PackBinary));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
                switchToAdd.DisplayName = "Pack Binary Mode";
                switchToAdd.Description = "pack mode: the binary file size will be truncated to the last occupied byte";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "-p";
                switchToAdd.Name = nameof(PackBinary);
                switchToAdd.BooleanValue = value;
                this.ActiveToolSwitches.Add(nameof(PackBinary), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }
        public int BinarySize
        {
            get
            {
                if (this.IsPropertySet(nameof(BinarySize)))
                    return this.ActiveToolSwitches[nameof(BinarySize)].Number;
                return 0;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(BinarySize));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Integer);
                switchToAdd.DisplayName = "Binary Size";
                switchToAdd.Description = "size of the binary file (default: 32768)";
                //switchToAdd.Parents.AddLast("");
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.IsValid = this.ValidateInteger(nameof(BinarySize), int.MinValue, int.MaxValue, value);
                switchToAdd.SwitchValue = "-s ";
                switchToAdd.Name = nameof(BinarySize);
                switchToAdd.Number = value;
                this.ActiveToolSwitches.Add(nameof(BinarySize), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }
        public bool GenerateGameBoyBinary
        {
            get
            {
                if (this.IsPropertySet(nameof(GenerateGameBoyBinary)))
                    return this.ActiveToolSwitches[nameof(GenerateGameBoyBinary)].BooleanValue;
                return false;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(GenerateGameBoyBinary));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
                switchToAdd.DisplayName = "Generate Gameboy Binary";
                switchToAdd.Description = "genarate GameBoy format binary file";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "-Z";
                switchToAdd.Name = nameof(GenerateGameBoyBinary);
                switchToAdd.BooleanValue = value;
                this.ActiveToolSwitches.Add(nameof(GenerateGameBoyBinary), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }

        #endregion

        #region GameBoy Options

        public int GameBoyRomBankCount
        {
            get
            {
                if (this.IsPropertySet(nameof(GameBoyRomBankCount)))
                    return this.ActiveToolSwitches[nameof(GameBoyRomBankCount)].Number;
                return 0;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(GameBoyRomBankCount));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Integer);
                switchToAdd.DisplayName = "Number of Rom Banks";
                switchToAdd.Description = "number of rom banks (default: 2)";
                //switchToAdd.Parents.AddLast("");
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.IsValid = this.ValidateInteger(nameof(GameBoyRomBankCount), int.MinValue, int.MaxValue, value);
                switchToAdd.SwitchValue = "-yo ";
                switchToAdd.Name = nameof(GameBoyRomBankCount);
                switchToAdd.Number = value;
                this.ActiveToolSwitches.Add(nameof(GameBoyRomBankCount), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }
        public int GameBoyRamBankCount
        {
            get
            {
                if (this.IsPropertySet(nameof(GameBoyRamBankCount)))
                    return this.ActiveToolSwitches[nameof(GameBoyRamBankCount)].Number;
                return 0;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(GameBoyRamBankCount));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Integer);
                switchToAdd.DisplayName = "Number of Ram Banks";
                switchToAdd.Description = "number of ram banks (default: 0)";
                //switchToAdd.Parents.AddLast("");
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.IsValid = this.ValidateInteger(nameof(GameBoyRamBankCount), int.MinValue, int.MaxValue, value);
                switchToAdd.SwitchValue = "-ya ";
                switchToAdd.Name = nameof(GameBoyRamBankCount);
                switchToAdd.Number = value;
                this.ActiveToolSwitches.Add(nameof(GameBoyRamBankCount), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }
        public int GameBoyMemoryBankController
        {
            get
            {
                if (this.IsPropertySet(nameof(GameBoyMemoryBankController)))
                    return this.ActiveToolSwitches[nameof(GameBoyMemoryBankController)].Number;
                return 0;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(GameBoyMemoryBankController));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Integer);
                switchToAdd.DisplayName = "Memory Bank Controller";
                switchToAdd.Description = "MMemory Bank Controller type (default: no MBC)";
                //switchToAdd.Parents.AddLast("");
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.IsValid = this.ValidateInteger(nameof(GameBoyMemoryBankController), int.MinValue, int.MaxValue, value);
                switchToAdd.SwitchValue = "-yt ";
                switchToAdd.Name = nameof(GameBoyMemoryBankController);
                switchToAdd.Number = value;
                this.ActiveToolSwitches.Add(nameof(GameBoyMemoryBankController), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }

        public string GameBoyCartridgeName
        {
            get
            {
                if (this.IsPropertySet(nameof(GameBoyCartridgeName)))
                    return this.ActiveToolSwitches[nameof(GameBoyCartridgeName)].Value;
                return string.Empty;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(GameBoyCartridgeName));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.String);
                switchToAdd.DisplayName = "Cartridge Name";
                switchToAdd.Description = "Cartridge Name (default: none)";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "-yn ";
                switchToAdd.Name = nameof(GameBoyCartridgeName);
                switchToAdd.Value = value;
                this.ActiveToolSwitches.Add(nameof(GameBoyCartridgeName), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }
        public bool GameBoyColor
        {
            get
            {
                if (this.IsPropertySet(nameof(GameBoyColor)))
                    return this.ActiveToolSwitches[nameof(GameBoyColor)].BooleanValue;
                return false;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(GameBoyColor));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
                switchToAdd.DisplayName = "Gameboy Color";
                switchToAdd.Description = "Flag to indicate if the binary is built for Gameboy Color";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "-yc";
                switchToAdd.Name = nameof(GameBoyColor);
                switchToAdd.BooleanValue = value;
                this.ActiveToolSwitches.Add(nameof(GameBoyColor), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }

        #endregion

        public SDCCMakeBin()
        {
            SwitchList = new ArrayList();
            SwitchList.Add(nameof(PackBinary));
            SwitchList.Add(nameof(BinarySize));
            SwitchList.Add(nameof(GenerateGameBoyBinary));

            SwitchList.Add(nameof(GameBoyRomBankCount));
            SwitchList.Add(nameof(GameBoyRamBankCount));
            SwitchList.Add(nameof(GameBoyMemoryBankController));
            SwitchList.Add(nameof(GameBoyCartridgeName));
            SwitchList.Add(nameof(GameBoyColor));

            SwitchList.Add(nameof(SourceInput));
            SwitchList.Add(nameof(OutputFile));
        }

        protected override string ToolName
        {
            get { return "makebin.exe"; }
        }
    }
}
