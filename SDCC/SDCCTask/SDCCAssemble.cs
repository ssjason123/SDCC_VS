using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Build.CPPTasks;

namespace SDCCTask
{
    /// <summary>
    ///     The SDCC Assembler tool.
    /// </summary>
    public class SDCCAssemble : SDCCToolBase
    {
        /// <summary>
        ///     Mapping of assembler to executable names.
        /// </summary>
        public static readonly Dictionary<string, string> AssemblerFile = new Dictionary<string, string>
        {
            {"avr", "asavr.exe"},
            {"ds390", "sdas390.exe"},
            {"TININative", "macro.exe"},
            {"ds400", "sdas390.exe"},
            {"hc08", "sdas6808.exe"},
            {"s08", "sdas6808.exe"},
            {"mcs51", "sdas8051.exe"},
            {"pdk13", "sdaspdk13.exe"},
            {"pdk14", "sdaspdk14.exe"},
            {"pdk15", "sdaspdk15.exe"},
            {"pic14", "gpasm.exe"},
            {"pic16", "gpasm.exe"},
            {"stm8", "sdasstm8.exe"},
            {"z80", "sdasz80.exe"},
            {"z180", "sdasz80.exe"},
            {"r2k", "sdasrab.exe"},
            {"r3ka", "sdasrab.exe"},
            {"gbz80", "sdasgb.exe"},
            {"tlcs90", "sdastlcs90.exe"},
            {"ez80_z80", "sdasz80.exe"},
            {"z80n", "sdasz80.exe"},
        };

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

        #region Assembler Commands.

        public string NumberFormat
        {
            get
            {
                if (this.IsPropertySet(nameof(NumberFormat)))
                    return this.ActiveToolSwitches[nameof(NumberFormat)].Value;
                return (string)null;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(NumberFormat));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.String);
                switchToAdd.DisplayName = "Listing Number Format";
                switchToAdd.Description = "The number format used to write numbers in the listing file.";
                switchToAdd.ArgumentRelationList = new ArrayList();
                string[][] switchMap = new string[][]
                {
                    new string[2]{ "Hexadecimal", "-x" },
                    new string[2]{ "Decimal", "-d" },
                    new string[2]{ "Octal", "-q" },
                };
                switchToAdd.SwitchValue = this.ReadSwitchMap(nameof(NumberFormat), switchMap, value);
                switchToAdd.Name = nameof(NumberFormat);
                switchToAdd.Value = value;
                switchToAdd.MultipleValues = true;
                this.ActiveToolSwitches.Add(nameof(NumberFormat), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }
        public bool UndefinedSymbolsMadeGlobal
        {
            get
            {
                if (this.IsPropertySet(nameof(UndefinedSymbolsMadeGlobal)))
                    return this.ActiveToolSwitches[nameof(UndefinedSymbolsMadeGlobal)].BooleanValue;
                return false;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(UndefinedSymbolsMadeGlobal));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
                switchToAdd.DisplayName = "Make Undefined Symbols Global";
                switchToAdd.Description = "Undefined symbols made global";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "-g";
                switchToAdd.Name = nameof(UndefinedSymbolsMadeGlobal);
                switchToAdd.BooleanValue = value;
                this.ActiveToolSwitches.Add(nameof(UndefinedSymbolsMadeGlobal), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }
        public bool AllUserSymbolsMadeGlobal
        {
            get
            {
                if (this.IsPropertySet(nameof(AllUserSymbolsMadeGlobal)))
                    return this.ActiveToolSwitches[nameof(AllUserSymbolsMadeGlobal)].BooleanValue;
                return false;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(AllUserSymbolsMadeGlobal));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
                switchToAdd.DisplayName = "Make All User Symbols Global";
                switchToAdd.Description = "All user symbols made global";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "-a";
                switchToAdd.Name = nameof(AllUserSymbolsMadeGlobal);
                switchToAdd.BooleanValue = value;
                this.ActiveToolSwitches.Add(nameof(AllUserSymbolsMadeGlobal), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }

        public string ListingDefineFormat
        {
            get
            {
                if (this.IsPropertySet(nameof(ListingDefineFormat)))
                    return this.ActiveToolSwitches[nameof(ListingDefineFormat)].Value;
                return (string)null;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(ListingDefineFormat));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.String);
                switchToAdd.DisplayName = "Define Display in Listing File";
                switchToAdd.Description = "The format to display .define substitutions in the listing file.";
                switchToAdd.ArgumentRelationList = new ArrayList();
                string[][] switchMap = new string[][]
                {
                    new string[2]{ "With Defines", "-b" },
                    new string[2]{ "Without Defines", "-bb" },
                };
                switchToAdd.SwitchValue = this.ReadSwitchMap(nameof(ListingDefineFormat), switchMap, value);
                switchToAdd.Name = nameof(ListingDefineFormat);
                switchToAdd.Value = value;
                switchToAdd.MultipleValues = true;
                this.ActiveToolSwitches.Add(nameof(ListingDefineFormat), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }
        public bool DisableInstructionCycleCountInListing
        {
            get
            {
                if (this.IsPropertySet(nameof(DisableInstructionCycleCountInListing)))
                    return this.ActiveToolSwitches[nameof(DisableInstructionCycleCountInListing)].BooleanValue;
                return false;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(DisableInstructionCycleCountInListing));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
                switchToAdd.DisplayName = "Disable Instruction Cycle Count in Listing";
                switchToAdd.Description = "Disable instruction cycle count in listing";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "-c";
                switchToAdd.Name = nameof(DisableInstructionCycleCountInListing);
                switchToAdd.BooleanValue = value;
                this.ActiveToolSwitches.Add(nameof(DisableInstructionCycleCountInListing), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }
        public bool EnableNoICEDebugSymbols
        {
            get
            {
                if (this.IsPropertySet(nameof(EnableNoICEDebugSymbols)))
                    return this.ActiveToolSwitches[nameof(EnableNoICEDebugSymbols)].BooleanValue;
                return false;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(EnableNoICEDebugSymbols));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
                switchToAdd.DisplayName = "Enable NoICE Debug Symbols";
                switchToAdd.Description = "Enable NoICE Debug Symbols";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "-j";
                switchToAdd.Name = nameof(EnableNoICEDebugSymbols);
                switchToAdd.BooleanValue = value;
                this.ActiveToolSwitches.Add(nameof(EnableNoICEDebugSymbols), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }
        public bool EnableSDCCDebugSymbols
        {
            get
            {
                if (this.IsPropertySet(nameof(EnableSDCCDebugSymbols)))
                    return this.ActiveToolSwitches[nameof(EnableSDCCDebugSymbols)].BooleanValue;
                return false;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(EnableSDCCDebugSymbols));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
                switchToAdd.DisplayName = "Enable SDCC Debug Symbols";
                switchToAdd.Description = "Enable SDCC Debug Symbols";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "-y";
                switchToAdd.Name = nameof(EnableSDCCDebugSymbols);
                switchToAdd.BooleanValue = value;
                this.ActiveToolSwitches.Add(nameof(EnableSDCCDebugSymbols), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }
        public bool CreateListFile
        {
            get
            {
                if (this.IsPropertySet(nameof(CreateListFile)))
                    return this.ActiveToolSwitches[nameof(CreateListFile)].BooleanValue;
                return false;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(CreateListFile));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
                switchToAdd.DisplayName = "Create List File";
                switchToAdd.Description = "Create list file/outfile[.lst]";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "-l";
                switchToAdd.Name = nameof(CreateListFile);
                switchToAdd.BooleanValue = value;
                this.ActiveToolSwitches.Add(nameof(CreateListFile), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }
        public bool CreateObjectFile
        {
            get
            {
                if (this.IsPropertySet(nameof(CreateObjectFile)))
                    return this.ActiveToolSwitches[nameof(CreateObjectFile)].BooleanValue;
                return false;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(CreateObjectFile));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
                switchToAdd.DisplayName = "Create Object File";
                switchToAdd.Description = "Create object file/outfile[.rel]";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "-o";
                switchToAdd.Name = nameof(CreateObjectFile);
                switchToAdd.BooleanValue = value;
                this.ActiveToolSwitches.Add(nameof(CreateObjectFile), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }
        public bool CreateSymbolFile
        {
            get
            {
                if (this.IsPropertySet(nameof(CreateSymbolFile)))
                    return this.ActiveToolSwitches[nameof(CreateSymbolFile)].BooleanValue;
                return false;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(CreateSymbolFile));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
                switchToAdd.DisplayName = "Create Symbol File";
                switchToAdd.Description = "Create symbol file/outfile[.sym]";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "-s";
                switchToAdd.Name = nameof(CreateSymbolFile);
                switchToAdd.BooleanValue = value;
                this.ActiveToolSwitches.Add(nameof(CreateSymbolFile), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }
        public bool DisableAutoListingPagination
        {
            get
            {
                if (this.IsPropertySet(nameof(DisableAutoListingPagination)))
                    return this.ActiveToolSwitches[nameof(DisableAutoListingPagination)].BooleanValue;
                return false;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(DisableAutoListingPagination));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
                switchToAdd.DisplayName = "Disable Automatic Listing Pagination";
                switchToAdd.Description = "Disable automatic listing pagination";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "-p";
                switchToAdd.Name = nameof(DisableAutoListingPagination);
                switchToAdd.BooleanValue = value;
                this.ActiveToolSwitches.Add(nameof(DisableAutoListingPagination), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }
        public bool DisableListProcessing
        {
            get
            {
                if (this.IsPropertySet(nameof(DisableListProcessing)))
                    return this.ActiveToolSwitches[nameof(DisableListProcessing)].BooleanValue;
                return false;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(DisableListProcessing));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
                switchToAdd.DisplayName = "Disable .list/.nlist Processing";
                switchToAdd.Description = "Disable .list/.nlist processing";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "-u";
                switchToAdd.Name = nameof(DisableListProcessing);
                switchToAdd.BooleanValue = value;
                this.ActiveToolSwitches.Add(nameof(DisableListProcessing), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }
        public bool UseWideListingFormat
        {
            get
            {
                if (this.IsPropertySet(nameof(UseWideListingFormat)))
                    return this.ActiveToolSwitches[nameof(UseWideListingFormat)].BooleanValue;
                return false;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(UseWideListingFormat));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
                switchToAdd.DisplayName = "Use Wide Listing Format";
                switchToAdd.Description = "Wide Listingformat for symbol table";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "-w";
                switchToAdd.Name = nameof(UseWideListingFormat);
                switchToAdd.BooleanValue = value;
                this.ActiveToolSwitches.Add(nameof(UseWideListingFormat), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }
        public bool DisableCaseSensitivity
        {
            get
            {
                if (this.IsPropertySet(nameof(DisableCaseSensitivity)))
                    return this.ActiveToolSwitches[nameof(DisableCaseSensitivity)].BooleanValue;
                return false;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(DisableCaseSensitivity));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
                switchToAdd.DisplayName = "Disable Case Sensitivity";
                switchToAdd.Description = "Disable case sensitivity for symbols";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "-z";
                switchToAdd.Name = nameof(DisableCaseSensitivity);
                switchToAdd.BooleanValue = value;
                this.ActiveToolSwitches.Add(nameof(DisableCaseSensitivity), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }
        public string RelocatableReferenceMode
        {
            get
            {
                if (this.IsPropertySet(nameof(RelocatableReferenceMode)))
                    return this.ActiveToolSwitches[nameof(RelocatableReferenceMode)].Value;
                return (string)null;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(RelocatableReferenceMode));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.String);
                switchToAdd.DisplayName = "Relocatable Reference Mode";
                switchToAdd.Description = "Method used to flag relocatable references in the listing file.";
                switchToAdd.ArgumentRelationList = new ArrayList();
                string[][] switchMap = new string[][]
                {
                    new string[2]{ "Flag by '", "-f" },
                    new string[2]{ "Flag by mode", "-ff" },
                };
                switchToAdd.SwitchValue = this.ReadSwitchMap(nameof(RelocatableReferenceMode), switchMap, value);
                switchToAdd.Name = nameof(RelocatableReferenceMode);
                switchToAdd.Value = value;
                switchToAdd.MultipleValues = true;
                this.ActiveToolSwitches.Add(nameof(RelocatableReferenceMode), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }
        public string[] AdditionalIncludeDirectories
        {
            get
            {
                if (this.IsPropertySet(nameof(AdditionalIncludeDirectories)))
                    return this.ActiveToolSwitches[nameof(AdditionalIncludeDirectories)].StringList;
                return (string[])null;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(AdditionalIncludeDirectories));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.StringPathArray);
                switchToAdd.DisplayName = "Additional Include Directories";
                switchToAdd.Description = "Add the named directory to the include file search path.  This option may be used more than once. Directories are searched in the order given.";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "-I";
                switchToAdd.Name = nameof(AdditionalIncludeDirectories);
                switchToAdd.StringList = value;
                this.ActiveToolSwitches.Add(nameof(AdditionalIncludeDirectories), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }

        #endregion

        /// <summary>
        ///     Name of an external assembler to use.
        /// </summary>
        public string ExternalAssembler
        {
            get
            {
                if (this.IsPropertySet(nameof(ExternalAssembler)))
                    return this.ActiveToolSwitches[nameof(ExternalAssembler)].Value;
                return string.Empty;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(ExternalAssembler));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.String);
                switchToAdd.DisplayName = "External Assembler Executable Name";
                switchToAdd.Description = "Specific name of the desired assembler executable.";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "";
                switchToAdd.Name = nameof(ExternalAssembler);
                switchToAdd.Value = value;
                this.ActiveToolSwitches.Add(nameof(ExternalAssembler), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }

        /// <summary>
        ///     The assembler port.
        /// </summary>
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
                switchToAdd.DisplayName = "Port Type";
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

        /// <summary>
        ///     Default constructor.
        /// </summary>
        public SDCCAssemble()
        {
            this.EnableErrorListRegex = true;

            this.errorListRegexList = new List<Regex>
            {
                new Regex(@"(?'FILENAME'.+):(?'LINE'\d+): (syntax )?(?'CATEGORY'Error|error|warning|info)( (?'CODE'\d+))?:(?'TEXT'.+?)( %3B column (?'COLUMN'\d+))?$"),
            };

            SwitchList = new ArrayList();
            SwitchList.Add(nameof(NumberFormat));
            SwitchList.Add(nameof(UndefinedSymbolsMadeGlobal));
            SwitchList.Add(nameof(AllUserSymbolsMadeGlobal));
            SwitchList.Add(nameof(ListingDefineFormat));
            SwitchList.Add(nameof(DisableInstructionCycleCountInListing));
            SwitchList.Add(nameof(EnableNoICEDebugSymbols));
            SwitchList.Add(nameof(EnableSDCCDebugSymbols));
            SwitchList.Add(nameof(CreateListFile));
            SwitchList.Add(nameof(CreateObjectFile));
            SwitchList.Add(nameof(CreateSymbolFile));
            SwitchList.Add(nameof(DisableAutoListingPagination));
            SwitchList.Add(nameof(DisableListProcessing));
            SwitchList.Add(nameof(UseWideListingFormat));
            SwitchList.Add(nameof(DisableCaseSensitivity));
            SwitchList.Add(nameof(RelocatableReferenceMode));
            SwitchList.Add(nameof(AdditionalIncludeDirectories));

            SwitchList.Add(nameof(AdditionalOptions));

            SwitchList.Add(nameof(OutputFile));
            SwitchList.Add(nameof(SourceInput));
        }

        /// <inheritdoc/>
        protected override string ToolName
        {
            get
            {
                string toolName = ExternalAssembler;
                if (string.IsNullOrEmpty(toolName))
                {
                    AssemblerFile.TryGetValue(PortType, out toolName);
                }

                return toolName;
            }
        }
    }
}
