using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Build.CPPTasks;

namespace SDCCTask
{
    public class SDCCLink : SDCCToolBase
    {
        public static readonly Dictionary<string, string> LinkApplicationMapping = new Dictionary<string, string>
        {
            {"avr", "linkavr.exe"},
            {"ds390", "sdld.exe"},
            {"TININative", null},
            {"ds400", "sdld.exe"},
            {"hc08", "sdld6808.exe"},
            {"s08", "sdld6808.exe"},
            {"mcs51", "sdld.exe"},
            {"pdk13", "sdldpdk.exe"},
            {"pdk14", "sdldpdk.exe"},
            {"pdk15", "sdldpdk.exe"},
            {"pic14", null},
            {"pic16", "gplink.exe"},
            {"stm8", "sdldstm8.exe"},
            {"z80", "sdldz80.exe"},
            {"z180", "sdldz80.exe"},
            {"r2k", "sdldz80.exe"},
            {"r3ka", "sdldz80.exe"},
            {"gbz80", "sdldgb.exe"},
            {"tlcs90", "sdldz80.exe"},
            {"ez80_z80", "sdldz80.exe"},
            {"z80n", "sdldz80.exe"},
        };

        #region Startup

        public string EchoMode
        {
            get
            {
                if (this.IsPropertySet(nameof(EchoMode)))
                    return this.ActiveToolSwitches[nameof(EchoMode)].Value;
                return (string)null;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(EchoMode));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.String);
                switchToAdd.DisplayName = "Echo Commands";
                switchToAdd.Description = "Echo commands to stdout.";
                switchToAdd.ArgumentRelationList = new ArrayList();
                string[][] switchMap = new string[][]
                {
                    new string[2]{ "Echo", "-p" },
                    new string[2]{ "NoEcho", "-n" },
                };
                switchToAdd.SwitchValue = this.ReadSwitchMap(nameof(EchoMode), switchMap, value);
                switchToAdd.Name = nameof(EchoMode);
                switchToAdd.Value = value;
                switchToAdd.MultipleValues = true;
                this.ActiveToolSwitches.Add(nameof(EchoMode), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }

        #endregion

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

        #region Libraries

        public string[] LibraryPaths
        {
            get
            {
                if (this.IsPropertySet(nameof(LibraryPaths)))
                    return this.ActiveToolSwitches[nameof(LibraryPaths)].StringList;
                return (string[])null;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(LibraryPaths));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.StringPathArray);
                switchToAdd.DisplayName = "Library Path Include Directories";
                switchToAdd.Description = "Library path specification, one per -k";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "-k ";
                switchToAdd.Name = nameof(LibraryPaths);
                switchToAdd.StringList = value;
                this.ActiveToolSwitches.Add(nameof(LibraryPaths), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }
        public string[] LibraryFiles
        {
            get
            {
                if (this.IsPropertySet(nameof(LibraryFiles)))
                    return this.ActiveToolSwitches[nameof(LibraryFiles)].StringList;
                return (string[])null;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(LibraryFiles));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.StringArray);
                switchToAdd.DisplayName = "Libraries to Link";
                switchToAdd.Description = "Library file specification, one per -l";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "-l ";
                switchToAdd.Name = nameof(LibraryFiles);
                switchToAdd.StringList = value;
                this.ActiveToolSwitches.Add(nameof(LibraryFiles), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }

        #endregion

        #region Relocation

        public string[] AreaBaseAddress
        {
            get
            {
                if (this.IsPropertySet(nameof(AreaBaseAddress)))
                    return this.ActiveToolSwitches[nameof(AreaBaseAddress)].StringList;
                return (string[])null;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(AreaBaseAddress));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.StringArray);
                switchToAdd.DisplayName = "Area Base Adddress Locations";
                switchToAdd.Description = "area base address = expression";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "-b ";
                switchToAdd.Name = nameof(AreaBaseAddress);
                switchToAdd.StringList = value;
                this.ActiveToolSwitches.Add(nameof(AreaBaseAddress), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }
        public string[] GlobalSymbols
        {
            get
            {
                if (this.IsPropertySet(nameof(GlobalSymbols)))
                    return this.ActiveToolSwitches[nameof(GlobalSymbols)].StringList;
                return (string[])null;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(GlobalSymbols));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.StringArray);
                switchToAdd.DisplayName = "Global Symbol Expressions";
                switchToAdd.Description = "global symbol = expression";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "-g ";
                switchToAdd.Name = nameof(GlobalSymbols);
                switchToAdd.StringList = value;
                this.ActiveToolSwitches.Add(nameof(GlobalSymbols), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }

        #endregion

        #region Map format

        public bool GenerateMapFile
        {
            get
            {
                if (this.IsPropertySet(nameof(GenerateMapFile)))
                    return this.ActiveToolSwitches[nameof(GenerateMapFile)].BooleanValue;
                return false;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(GenerateMapFile));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
                switchToAdd.DisplayName = "Generate Map File";
                switchToAdd.Description = "Map output generated as (out)file[.map]";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "-m";
                switchToAdd.Name = nameof(GenerateMapFile);
                switchToAdd.BooleanValue = value;
                this.ActiveToolSwitches.Add(nameof(GenerateMapFile), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }
        public bool WideListMapFormatEnabled
        {
            get
            {
                if (this.IsPropertySet(nameof(WideListMapFormatEnabled)))
                    return this.ActiveToolSwitches[nameof(WideListMapFormatEnabled)].BooleanValue;
                return false;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(WideListMapFormatEnabled));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
                switchToAdd.DisplayName = "Use Wide Listing Map Format";
                switchToAdd.Description = "Wide listing format for map file";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "-w";
                switchToAdd.Name = nameof(WideListMapFormatEnabled);
                switchToAdd.BooleanValue = value;
                this.ActiveToolSwitches.Add(nameof(WideListMapFormatEnabled), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }

        public string MapNumberFormat
        {
            get
            {
                if (this.IsPropertySet(nameof(MapNumberFormat)))
                    return this.ActiveToolSwitches[nameof(MapNumberFormat)].Value;
                return (string)null;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(MapNumberFormat));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.String);
                switchToAdd.DisplayName = "Map File Number Format";
                switchToAdd.Description = "The number format used to write numbers in the map file.";
                switchToAdd.ArgumentRelationList = new ArrayList();
                string[][] switchMap = new string[][]
                {
                    new string[2]{ "Hexadecimal", "-x" },
                    new string[2]{ "Decimal", "-d" },
                    new string[2]{ "Octal", "-q" },
                };
                switchToAdd.SwitchValue = this.ReadSwitchMap(nameof(MapNumberFormat), switchMap, value);
                switchToAdd.Name = nameof(MapNumberFormat);
                switchToAdd.Value = value;
                switchToAdd.MultipleValues = true;
                this.ActiveToolSwitches.Add(nameof(MapNumberFormat), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }

        #endregion

        #region Output

        public string OutputFormat
        {
            get
            {
                if (this.IsPropertySet(nameof(OutputFormat)))
                    return this.ActiveToolSwitches[nameof(OutputFormat)].Value;
                return (string)null;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(OutputFormat));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.String);
                switchToAdd.DisplayName = "Output Format";
                switchToAdd.Description = "Output file format.";
                switchToAdd.ArgumentRelationList = new ArrayList();
                string[][] switchMap = new string[][]
                {
                    new string[2]{ "Intel", "-i" },
                    new string[2]{ "S19", "-s" },
                    new string[2]{ "ELF", "-E" },
                };
                switchToAdd.SwitchValue = this.ReadSwitchMap(nameof(OutputFormat), switchMap, value);
                switchToAdd.Name = nameof(OutputFormat);
                switchToAdd.Value = value;
                switchToAdd.MultipleValues = true;
                this.ActiveToolSwitches.Add(nameof(OutputFormat), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }
        /*
        public bool OutputIntelHex
        {
            get
            {
                if (this.IsPropertySet(nameof(OutputIntelHex)))
                    return this.ActiveToolSwitches[nameof(OutputIntelHex)].BooleanValue;
                return false;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(OutputIntelHex));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
                switchToAdd.DisplayName = "Output Intel Hex File";
                switchToAdd.Description = "Intel Hex as (out)file[.ihx]";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "-i";
                switchToAdd.Name = nameof(OutputIntelHex);
                switchToAdd.BooleanValue = value;
                this.ActiveToolSwitches.Add(nameof(OutputIntelHex), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }
        public bool OutputMotorolaSRecord
        {
            get
            {
                if (this.IsPropertySet(nameof(OutputMotorolaSRecord)))
                    return this.ActiveToolSwitches[nameof(OutputMotorolaSRecord)].BooleanValue;
                return false;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(OutputMotorolaSRecord));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
                switchToAdd.DisplayName = "Output Motorola S Record File";
                switchToAdd.Description = "Motorola S Record as (out)file[.s19]";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "-s";
                switchToAdd.Name = nameof(OutputMotorolaSRecord);
                switchToAdd.BooleanValue = value;
                this.ActiveToolSwitches.Add(nameof(OutputMotorolaSRecord), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }
        */

        public bool NoIceDebugOutput
        {
            get
            {
                if (this.IsPropertySet(nameof(NoIceDebugOutput)))
                    return this.ActiveToolSwitches[nameof(NoIceDebugOutput)].BooleanValue;
                return false;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(NoIceDebugOutput));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
                switchToAdd.DisplayName = "Output NoICE Debug File";
                switchToAdd.Description = "NoICE Debug output as (out)file[.noi]";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "-j";
                switchToAdd.Name = nameof(NoIceDebugOutput);
                switchToAdd.BooleanValue = value;
                this.ActiveToolSwitches.Add(nameof(NoIceDebugOutput), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }
        public bool SDCDBDebugOutput
        {
            get
            {
                if (this.IsPropertySet(nameof(SDCDBDebugOutput)))
                    return this.ActiveToolSwitches[nameof(SDCDBDebugOutput)].BooleanValue;
                return false;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(SDCDBDebugOutput));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
                switchToAdd.DisplayName = "Output SDCDB Debug File";
                switchToAdd.Description = "SDCDB Debug output as (out)file[.cdb]";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "-y";
                switchToAdd.Name = nameof(SDCDBDebugOutput);
                switchToAdd.BooleanValue = value;
                this.ActiveToolSwitches.Add(nameof(SDCDBDebugOutput), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }

        #endregion

        #region List

        public bool UpdateListingFiles
        {
            get
            {
                if (this.IsPropertySet(nameof(UpdateListingFiles)))
                    return this.ActiveToolSwitches[nameof(UpdateListingFiles)].BooleanValue;
                return false;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(UpdateListingFiles));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
                switchToAdd.DisplayName = "Update Listing File(s)";
                switchToAdd.Description = "Update listing file(s) with link data as file(s)[.rst]";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "-u";
                switchToAdd.Name = nameof(UpdateListingFiles);
                switchToAdd.BooleanValue = value;
                this.ActiveToolSwitches.Add(nameof(UpdateListingFiles), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }

        #endregion

        #region Case Sensitivity

        public bool DisableSymbolCaseSensitivity
        {
            get
            {
                if (this.IsPropertySet(nameof(DisableSymbolCaseSensitivity)))
                    return this.ActiveToolSwitches[nameof(DisableSymbolCaseSensitivity)].BooleanValue;
                return false;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(DisableSymbolCaseSensitivity));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
                switchToAdd.DisplayName = "Disable Symbol Case Sensitivity";
                switchToAdd.Description = "Disable Case Sensitivity for Symbols";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "-z";
                switchToAdd.Name = nameof(DisableSymbolCaseSensitivity);
                switchToAdd.BooleanValue = value;
                this.ActiveToolSwitches.Add(nameof(DisableSymbolCaseSensitivity), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }

        #endregion

        #region Miscellaneous

        public string IRamSize
        {
            get
            {
                if (this.IsPropertySet(nameof(IRamSize)))
                    return this.ActiveToolSwitches[nameof(IRamSize)].Value;
                return null;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(IRamSize));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.String);
                switchToAdd.DisplayName = "Check for Internal RAM Overflow";
                switchToAdd.Description = "[iram-size] Check for internal RAM overflow";
                //switchToAdd.Parents.AddLast("");
                switchToAdd.ArgumentRelationList = new ArrayList();
                var intConverter = new Int32Converter();
                switchToAdd.IsValid = this.ValidateInteger(nameof(IRamSize), int.MinValue, int.MaxValue,
                    (int)intConverter.ConvertFromString(value));
                switchToAdd.SwitchValue = "-I ";
                switchToAdd.Name = nameof(IRamSize);
                switchToAdd.Value = value;
                this.ActiveToolSwitches.Add(nameof(IRamSize), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }
        public string XRamSize
        {
            get
            {
                if (this.IsPropertySet(nameof(XRamSize)))
                    return this.ActiveToolSwitches[nameof(XRamSize)].Value;
                return null;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(XRamSize));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.String);
                switchToAdd.DisplayName = "Check for External RAM Overflow";
                switchToAdd.Description = "[xram-size] Check for external RAM overflow";
                //switchToAdd.Parents.AddLast("");
                switchToAdd.ArgumentRelationList = new ArrayList();
                var intConverter = new Int32Converter();
                switchToAdd.IsValid = this.ValidateInteger(nameof(XRamSize), int.MinValue, int.MaxValue,
                    (int)intConverter.ConvertFromString(value));
                switchToAdd.SwitchValue = "-X ";
                switchToAdd.Name = nameof(XRamSize);
                switchToAdd.Value = value;
                this.ActiveToolSwitches.Add(nameof(XRamSize), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }
        public string CodeSize
        {
            get
            {
                if (this.IsPropertySet(nameof(CodeSize)))
                    return this.ActiveToolSwitches[nameof(CodeSize)].Value;
                return null;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(CodeSize));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.String);
                switchToAdd.DisplayName = "Check for Code Overflow";
                switchToAdd.Description = "[code-size] Check for code overflow";
                //switchToAdd.Parents.AddLast("");
                switchToAdd.ArgumentRelationList = new ArrayList();
                var intConverter = new Int32Converter();
                switchToAdd.IsValid = this.ValidateInteger(nameof(CodeSize), int.MinValue, int.MaxValue,
                    (int)intConverter.ConvertFromString(value));
                switchToAdd.SwitchValue = "-C ";
                switchToAdd.Name = nameof(CodeSize);
                switchToAdd.Value = value;
                this.ActiveToolSwitches.Add(nameof(CodeSize), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }
        public bool GenerateMemSummaryFile
        {
            get
            {
                if (this.IsPropertySet(nameof(GenerateMemSummaryFile)))
                    return this.ActiveToolSwitches[nameof(GenerateMemSummaryFile)].BooleanValue;
                return false;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(GenerateMemSummaryFile));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
                switchToAdd.DisplayName = "Generate Memory Usage Summary File";
                switchToAdd.Description = "Generate memory usage summary file[.mem]";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "-M";
                switchToAdd.Name = nameof(GenerateMemSummaryFile);
                switchToAdd.BooleanValue = value;
                this.ActiveToolSwitches.Add(nameof(GenerateMemSummaryFile), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }
        public bool PackInternalRam
        {
            get
            {
                if (this.IsPropertySet(nameof(PackInternalRam)))
                    return this.ActiveToolSwitches[nameof(PackInternalRam)].BooleanValue;
                return false;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(PackInternalRam));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
                switchToAdd.DisplayName = "Pack Internal RAM";
                switchToAdd.Description = "Pack internal ram";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "-Y";
                switchToAdd.Name = nameof(PackInternalRam);
                switchToAdd.BooleanValue = value;
                this.ActiveToolSwitches.Add(nameof(PackInternalRam), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }
        public string StackSize
        {
            get
            {
                if (this.IsPropertySet(nameof(StackSize)))
                    return this.ActiveToolSwitches[nameof(StackSize)].Value;
                return null;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(StackSize));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.String);
                switchToAdd.DisplayName = "Allocate Space for Stack";
                switchToAdd.Description = "[stack-size] Allocate space for stack";
                //switchToAdd.Parents.AddLast("");
                switchToAdd.ArgumentRelationList = new ArrayList();
                var intConverter = new Int32Converter();
                switchToAdd.IsValid = this.ValidateInteger(nameof(StackSize), int.MinValue, int.MaxValue, 
                    (int)intConverter.ConvertFromString(value));
                switchToAdd.SwitchValue = "-S ";
                switchToAdd.Name = nameof(StackSize);
                switchToAdd.Value = value;
                this.ActiveToolSwitches.Add(nameof(StackSize), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }

        #endregion

        #region Custom CRT0

        public string CustomCrt
        {
            get
            {
                if (this.IsPropertySet(nameof(CustomCrt)))
                    return this.ActiveToolSwitches[nameof(CustomCrt)].Value;
                return (string)null;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(CustomCrt));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.String);
                switchToAdd.DisplayName = "CustomCrt0";
                switchToAdd.Description = "Custom CRT0";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "";
                switchToAdd.Name = nameof(CustomCrt);
                switchToAdd.Value = value;
                this.ActiveToolSwitches.Add(nameof(CustomCrt), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }
        #endregion


        public string ExternalLinker
        {
            get
            {
                if (this.IsPropertySet(nameof(ExternalLinker)))
                    return this.ActiveToolSwitches[nameof(ExternalLinker)].Value;
                return string.Empty;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(ExternalLinker));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.String);
                switchToAdd.DisplayName = "External Linker Executable Name";
                switchToAdd.Description = "Specific name of the desired linker executable.";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "";
                switchToAdd.Name = nameof(ExternalLinker);
                switchToAdd.Value = value;
                this.ActiveToolSwitches.Add(nameof(ExternalLinker), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }

        public string PortType
        {
            get
            {
                if (this.IsPropertySet(nameof(PortType)))
                    return this.ActiveToolSwitches[nameof(PortType)].Value;
                return (string)null;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(PortType));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.String);
                switchToAdd.DisplayName = "Processor";
                switchToAdd.Description = "The processor type to build for.    (-m)";
                switchToAdd.ArgumentRelationList = new ArrayList();
                string[][] switchMap = new string[][]
                {
                    new string[2]{ "mcs51", "-mmcs51" },
                    new string[2]{ "ds390", "-mds390" },
                    new string[2]{ "ds400", "-mds400" },
                    new string[2]{ "hc08", "-mhc08" },
                    new string[2]{ "s08", "-ms08" },
                    new string[2]{ "z80", "-mz80" },
                    new string[2]{ "z180", "-mz180" },
                    new string[2]{ "r2k", "-mr2k" },
                    new string[2]{ "r3ka", "-mr3ka" },
                    new string[2]{ "gbz80", "-mgbz80" },
                    new string[2]{ "tlcs90", "-mtlcs90" },
                    new string[2]{ "ez80_z80", "-mez80_z80" },
                    new string[2]{ "stm8", "-mstm8" },
                    new string[2]{ "pdk13", "-mpdk13" },
                    new string[2]{ "pdk14", "-mpdk14" },
                    new string[2]{ "pdk15", "-mpdk15" },
                    new string[2]{ "pic14", "-mpic14" },
                    new string[2]{ "pic16", "-mpic16" },
                    new string[2]{ "TININative", "-mTININative" },
                };
                switchToAdd.SwitchValue = this.ReadSwitchMap(nameof(PortType), switchMap, value);
                switchToAdd.Name = nameof(PortType);
                switchToAdd.Value = value;
                switchToAdd.MultipleValues = true;
                this.ActiveToolSwitches.Add(nameof(PortType), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }

        public SDCCLink()
        {
            SwitchList = new ArrayList();
            SwitchList.Add(nameof(EchoMode));

            SwitchList.Add(nameof(GenerateMapFile));
            SwitchList.Add(nameof(WideListMapFormatEnabled));
            SwitchList.Add(nameof(MapNumberFormat));

            SwitchList.Add(nameof(OutputFormat));
            SwitchList.Add(nameof(NoIceDebugOutput));
            SwitchList.Add(nameof(SDCDBDebugOutput));

            SwitchList.Add(nameof(UpdateListingFiles));

            SwitchList.Add(nameof(DisableSymbolCaseSensitivity));

            SwitchList.Add(nameof(GenerateMemSummaryFile));

            SwitchList.Add(nameof(LibraryPaths));
            SwitchList.Add(nameof(LibraryFiles));

            SwitchList.Add(nameof(AreaBaseAddress));
            SwitchList.Add(nameof(GlobalSymbols));

            SwitchList.Add(nameof(IRamSize));
            SwitchList.Add(nameof(XRamSize));
            SwitchList.Add(nameof(CodeSize));

            SwitchList.Add(nameof(PackInternalRam));
            SwitchList.Add(nameof(StackSize));

            SwitchList.Add(nameof(AdditionalOptions));

            SwitchList.Add(nameof(OutputFile));
            SwitchList.Add(nameof(CustomCrt));
            SwitchList.Add(nameof(SourceInput));
        }

        protected override string ToolName
        {
            get
            {
                string toolName = ExternalLinker;
                if (string.IsNullOrEmpty(toolName))
                {
                    LinkApplicationMapping.TryGetValue(PortType, out toolName);
                }

                return toolName;
            }
        }
    }
}
