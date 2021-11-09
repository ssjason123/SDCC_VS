using System.Collections;
using Microsoft.Build.CPPTasks;

namespace SDCCTask
{
    /// <summary>
    ///     SDCC MakeBin tool task.
    /// </summary>
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

        public string GameBoyRomBankCount
        {
            get
            {
                if (this.IsPropertySet(nameof(GameBoyRomBankCount)))
                    return this.ActiveToolSwitches[nameof(GameBoyRomBankCount)].Value;
                return "";
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(GameBoyRomBankCount));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.String);
                switchToAdd.DisplayName = "Number of Rom Banks";
                switchToAdd.Description = "number of rom banks (default: 2) (autosize: A)";
                //switchToAdd.Parents.AddLast("");
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "-yo ";
                switchToAdd.Name = nameof(GameBoyRomBankCount);
                switchToAdd.Value = value;
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
                switchToAdd.Description = "Memory Bank Controller type (default: no MBC)";
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

        public string OldLicenseeCode
        {
            get
            {
                if (this.IsPropertySet(nameof(OldLicenseeCode)))
                    return this.ActiveToolSwitches[nameof(OldLicenseeCode)].Value;
                return "";
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(OldLicenseeCode));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.String);
                switchToAdd.DisplayName = "Old Licensee Code";
                switchToAdd.Description = "Old licensee code (default: 0x33)";
                //switchToAdd.Parents.AddLast("");
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "-yl ";
                switchToAdd.Name = nameof(OldLicenseeCode);
                switchToAdd.Value = value;
                this.ActiveToolSwitches.Add(nameof(OldLicenseeCode), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }
        public string NewLicenseeString
        {
            get
            {
                if (this.IsPropertySet(nameof(NewLicenseeString)))
                    return this.ActiveToolSwitches[nameof(NewLicenseeString)].Value;
                return "";
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(NewLicenseeString));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.String);
                switchToAdd.DisplayName = "New Licensee String";
                switchToAdd.Description = "new licensee string (default: 00)";
                //switchToAdd.Parents.AddLast("");
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "-yk ";
                switchToAdd.Name = nameof(NewLicenseeString);
                switchToAdd.Value = value;
                this.ActiveToolSwitches.Add(nameof(NewLicenseeString), switchToAdd);
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
                switchToAdd.DisplayName = "Gameboy Color compatible";
                switchToAdd.Description = "Flag to indicate if the binary is compatible with Gameboy Color";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "-yc";
                switchToAdd.Name = nameof(GameBoyColor);
                switchToAdd.BooleanValue = value;
                this.ActiveToolSwitches.Add(nameof(GameBoyColor), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }


        public bool GameBoyColorOnly
        {
            get
            {
                if (this.IsPropertySet(nameof(GameBoyColorOnly)))
                    return this.ActiveToolSwitches[nameof(GameBoyColorOnly)].BooleanValue;
                return false;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(GameBoyColorOnly));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
                switchToAdd.DisplayName = "Gameboy Color only";
                switchToAdd.Description = "Flag to indicate if the binary only supports Gameboy Color";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "-yC";
                switchToAdd.Name = nameof(GameBoyColor);
                switchToAdd.BooleanValue = value;
                this.ActiveToolSwitches.Add(nameof(GameBoyColor), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }
        public bool SuperGameBoy
        {
            get
            {
                if (this.IsPropertySet(nameof(SuperGameBoy)))
                    return this.ActiveToolSwitches[nameof(SuperGameBoy)].BooleanValue;
                return false;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(SuperGameBoy));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
                switchToAdd.DisplayName = "Super GameBoy";
                switchToAdd.Description = "Flag to indicate if the binary  supports Super GameBoy.";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "-ys";
                switchToAdd.Name = nameof(SuperGameBoy);
                switchToAdd.BooleanValue = value;
                this.ActiveToolSwitches.Add(nameof(SuperGameBoy), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }
        public bool NoiToSym
        {
            get
            {
                if (this.IsPropertySet(nameof(NoiToSym)))
                    return this.ActiveToolSwitches[nameof(NoiToSym)].BooleanValue;
                return false;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(NoiToSym));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
                switchToAdd.DisplayName = ".Noi to .Sym";
                switchToAdd.Description = "Convert .noi file named like input file to .sym.";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "-yS";
                switchToAdd.Name = nameof(NoiToSym);
                switchToAdd.BooleanValue = value;
                this.ActiveToolSwitches.Add(nameof(NoiToSym), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }
        public bool NonJapaneseRegion
        {
            get
            {
                if (this.IsPropertySet(nameof(NonJapaneseRegion)))
                    return this.ActiveToolSwitches[nameof(NonJapaneseRegion)].BooleanValue;
                return false;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(NonJapaneseRegion));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
                switchToAdd.DisplayName = "Non-Japanese Region";
                switchToAdd.Description = "Set non-Japanese region flag.";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "-yj";
                switchToAdd.Name = nameof(NonJapaneseRegion);
                switchToAdd.BooleanValue = value;
                this.ActiveToolSwitches.Add(nameof(NonJapaneseRegion), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }

        public string[] SetAddrInRom
        {
            get
            {
                if (this.IsPropertySet(nameof(SetAddrInRom)))
                    return this.ActiveToolSwitches[nameof(SetAddrInRom)].StringList;
                return new string []{};
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(SetAddrInRom));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.StringArray);
                switchToAdd.DisplayName = "Set Address to Value";
                switchToAdd.Description = "Set address in ROM to given value (address 0x100-0x1FE) (addr=value)";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "-yp ";
                switchToAdd.Name = nameof(SetAddrInRom);
                switchToAdd.StringList = value;
                this.ActiveToolSwitches.Add(nameof(SetAddrInRom), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }

        #endregion

        /// <summary>
        ///     Default constructor.
        /// </summary>
        public SDCCMakeBin()
        {
            SwitchList = new ArrayList();
            SwitchList.Add(nameof(PackBinary));
            SwitchList.Add(nameof(BinarySize));
            SwitchList.Add(nameof(GenerateGameBoyBinary));

            SwitchList.Add(nameof(GameBoyRomBankCount));
            SwitchList.Add(nameof(GameBoyRamBankCount));
            SwitchList.Add(nameof(GameBoyMemoryBankController));
            SwitchList.Add(nameof(OldLicenseeCode));
            SwitchList.Add(nameof(NewLicenseeString));
            SwitchList.Add(nameof(GameBoyCartridgeName));
            SwitchList.Add(nameof(GameBoyColor));
            SwitchList.Add(nameof(GameBoyColorOnly));
            SwitchList.Add(nameof(SuperGameBoy));
            SwitchList.Add(nameof(NoiToSym));
            SwitchList.Add(nameof(NonJapaneseRegion));
            SwitchList.Add(nameof(SetAddrInRom));

            SwitchList.Add(nameof(SourceInput));
            SwitchList.Add(nameof(OutputFile));
        }

        /// <inheritdoc/>
        protected override string ToolName
        {
            get { return "makebin.exe"; }
        }
    }
}
