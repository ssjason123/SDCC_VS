using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Build.CPPTasks;

namespace SDCCTask
{
    /// <summary>
    /// Custom build task for SDCC command line generation.
    /// </summary>
    public class SDCCCompile : SDCCToolBase
    {
        protected override string AlwaysAppend
        {
            get { return "-c"; }
            set { /* Intentionally Blank */ }
        }

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
                switchToAdd.SwitchValue = "-o ";
                switchToAdd.Name = nameof(OutputFile);
                switchToAdd.Value = value;
                this.ActiveToolSwitches.Add(nameof(OutputFile), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }

        #endregion 

        #region General Properties.

        public bool Verbose
        {
            get
            {
                if (this.IsPropertySet(nameof(Verbose)))
                    return this.ActiveToolSwitches[nameof(Verbose)].BooleanValue;
                return false;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(Verbose));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
                switchToAdd.DisplayName = "Execute Verbosely";
                switchToAdd.Description = "Execute verbosely. Show sub commands as they are run.    (-V)";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "-V";
                switchToAdd.Name = nameof(Verbose);
                switchToAdd.BooleanValue = value;
                this.ActiveToolSwitches.Add(nameof(Verbose), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }

        public bool OutputMacros
        {
            get
            {
                if (this.IsPropertySet(nameof(OutputMacros)))
                    return this.ActiveToolSwitches[nameof(OutputMacros)].BooleanValue;
                return false;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(OutputMacros));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
                switchToAdd.DisplayName = "Output Macro List";
                switchToAdd.Description = "Output list of macro definitions in effect. Use with -E.    (-d)";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "-d";
                switchToAdd.Name = nameof(OutputMacros);
                switchToAdd.BooleanValue = value;
                this.ActiveToolSwitches.Add(nameof(OutputMacros), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }

        public string[] PreprocessorDefinitions
        {
            get
            {
                if (this.IsPropertySet(nameof(PreprocessorDefinitions)))
                    return this.ActiveToolSwitches[nameof(PreprocessorDefinitions)].StringList;
                return (string[])null;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(PreprocessorDefinitions));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.StringPathArray);
                switchToAdd.DisplayName = "Preprocessor Definitions";
                switchToAdd.Description = "Command line definition of macros. Passed to the preprocessor.    (-D<macro[=value]>)";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "-D";
                switchToAdd.Name = nameof(PreprocessorDefinitions);
                switchToAdd.StringList = value;
                this.ActiveToolSwitches.Add(nameof(PreprocessorDefinitions), switchToAdd);
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
                switchToAdd.Description = "The additional location where the preprocessor will look for <..h> or \"..h\" files.    (-I<path>)";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "-I";
                switchToAdd.Name = nameof(AdditionalIncludeDirectories);
                switchToAdd.StringList = value;
                this.ActiveToolSwitches.Add(nameof(AdditionalIncludeDirectories), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }

        public string[] AssertAnswer
        {
            get
            {
                if (this.IsPropertySet(nameof(AssertAnswer)))
                    return this.ActiveToolSwitches[nameof(AssertAnswer)].StringList;
                return (string[])null;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(AssertAnswer));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.StringArray);
                switchToAdd.DisplayName = "Assert Question Answer";
                switchToAdd.Description = "Assert the answer answer for question, in case it is tested with a preprocessor conditional such as ‘#if #question(answer)’. ‘-A-’ disables the standard assertions that normally describe the target machine.    (-Aquestion(answer))";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "-A";
                switchToAdd.Name = nameof(AssertAnswer);
                switchToAdd.StringList = value;
                this.ActiveToolSwitches.Add(nameof(AssertAnswer), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }

        public string[] PreprocessorUndefines
        {
            get
            {
                if (this.IsPropertySet(nameof(PreprocessorUndefines)))
                    return this.ActiveToolSwitches[nameof(PreprocessorUndefines)].StringList;
                return (string[])null;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(PreprocessorUndefines));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.StringArray);
                switchToAdd.DisplayName = "Preprocessor Undefines";
                switchToAdd.Description = "Undefine macro macro. ‘-U’ options are evaluated after all ‘-D’ options, but before any ‘-include’ and ‘-imacros’ options.    (-Umacro)";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "-U";
                switchToAdd.Name = nameof(PreprocessorUndefines);
                switchToAdd.StringList = value;
                this.ActiveToolSwitches.Add(nameof(PreprocessorUndefines), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }

        public string DependencyRule
        {
            get
            {
                if (this.IsPropertySet(nameof(DependencyRule)))
                    return this.ActiveToolSwitches[nameof(DependencyRule)].Value;
                return (string)null;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(DependencyRule));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.String);
                switchToAdd.DisplayName = "Output Dependency Rules";
                switchToAdd.Description = "Output make rules for dependencies.";
                switchToAdd.ArgumentRelationList = new ArrayList();
                string[][] switchMap = new string[][]
                {
                    new string[2]{ "All Headers", "-M" },
                    new string[2]{ "User Headers Only", "-MM" },
                };
                switchToAdd.SwitchValue = this.ReadSwitchMap(nameof(DependencyRule), switchMap, value);
                switchToAdd.Name = nameof(DependencyRule);
                switchToAdd.Value = value;
                switchToAdd.MultipleValues = true;
                this.ActiveToolSwitches.Add(nameof(DependencyRule), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }

        public string[] PreprocessorOptions
        {
            get
            {
                if (this.IsPropertySet(nameof(PreprocessorOptions)))
                    return this.ActiveToolSwitches[nameof(PreprocessorOptions)].StringList;
                return (string[])null;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(PreprocessorOptions));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.StringArray);
                switchToAdd.DisplayName = "Preprocessor Options";
                switchToAdd.Description = "Pass the preprocessorOption to the preprocessor sdcpp.    (-Wp preprocessorOption[,preprocessorOption]...)";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "-Wp ";
                switchToAdd.Separator = ",";
                switchToAdd.Name = nameof(PreprocessorOptions);
                switchToAdd.StringList = value;
                this.ActiveToolSwitches.Add(nameof(PreprocessorOptions), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }

        public string[] AssemblerOptions
        {
            get
            {
                if (this.IsPropertySet(nameof(AssemblerOptions)))
                    return this.ActiveToolSwitches[nameof(AssemblerOptions)].StringList;
                return (string[])null;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(AssemblerOptions));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.StringArray);
                switchToAdd.DisplayName = "Assembler Options";
                switchToAdd.Description = "Pass the assemblerOption to the assembler sdcpp.    (-Wa assemblerOption[,assemblerOption]...)";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "-Wa ";
                switchToAdd.Separator = ",";
                switchToAdd.Name = nameof(AssemblerOptions);
                switchToAdd.StringList = value;
                this.ActiveToolSwitches.Add(nameof(AssemblerOptions), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }

        public string[] LinkerOptions
        {
            get
            {
                if (this.IsPropertySet(nameof(LinkerOptions)))
                    return this.ActiveToolSwitches[nameof(LinkerOptions)].StringList;
                return (string[])null;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(LinkerOptions));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.StringArray);
                switchToAdd.DisplayName = "Linker Options";
                switchToAdd.Description = "Pass the linkerOption to the linker sdcpp.    (-Wl linkerOption[,linkerOption]...)";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "-Wl ";
                switchToAdd.Separator = ",";
                switchToAdd.Name = nameof(LinkerOptions);
                switchToAdd.StringList = value;
                this.ActiveToolSwitches.Add(nameof(LinkerOptions), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }

        public bool PrintSearchDirs
        {
            get
            {
                if (this.IsPropertySet(nameof(PrintSearchDirs)))
                    return this.ActiveToolSwitches[nameof(PrintSearchDirs)].BooleanValue;
                return false;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(PrintSearchDirs));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
                switchToAdd.DisplayName = "Print Search Directories";
                switchToAdd.Description = "Display the directories in the compiler's search path.     (--print-search-dirs)";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "--print-search-dirs";
                switchToAdd.Name = nameof(PrintSearchDirs);
                switchToAdd.BooleanValue = value;
                this.ActiveToolSwitches.Add(nameof(PrintSearchDirs), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }

        public bool NoStdLib
        {
            get
            {
                if (this.IsPropertySet(nameof(NoStdLib)))
                    return this.ActiveToolSwitches[nameof(NoStdLib)].BooleanValue;
                return false;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(NoStdLib));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
                switchToAdd.DisplayName = "No Standard Library Calls";
                switchToAdd.Description = "Disable the optimization of calls to the standard library.     (--nostdlibcall)";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "--nostdlibcall";
                switchToAdd.Name = nameof(NoStdLib);
                switchToAdd.BooleanValue = value;
                this.ActiveToolSwitches.Add(nameof(NoStdLib), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }

        public bool NoStdInc
        {
            get
            {
                if (this.IsPropertySet(nameof(NoStdInc)))
                    return this.ActiveToolSwitches[nameof(NoStdInc)].BooleanValue;
                return false;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(NoStdInc));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
                switchToAdd.DisplayName = "No Standard Includes";
                switchToAdd.Description = "Do not include the standard include directory in the search path.     (--nostdinc)";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "--nostdinc";
                switchToAdd.Name = nameof(NoStdInc);
                switchToAdd.BooleanValue = value;
                this.ActiveToolSwitches.Add(nameof(NoStdInc), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }

        public bool LessPedantic
        {
            get
            {
                if (this.IsPropertySet(nameof(LessPedantic)))
                    return this.ActiveToolSwitches[nameof(LessPedantic)].BooleanValue;
                return false;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(LessPedantic));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
                switchToAdd.DisplayName = "Less Pedantic";
                switchToAdd.Description = "Disable some of the more pedantic warnings.     (--less-pedantic)";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "--less-pedantic";
                switchToAdd.Name = nameof(LessPedantic);
                switchToAdd.BooleanValue = value;
                this.ActiveToolSwitches.Add(nameof(LessPedantic), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }

        public string[] DisableWarning
        {
            get
            {
                if (this.IsPropertySet(nameof(DisableWarning)))
                    return this.ActiveToolSwitches[nameof(DisableWarning)].StringList;
                return (string[])null;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(DisableWarning));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.StringArray);
                switchToAdd.DisplayName = "Disable Warning";
                switchToAdd.Description = "Disable specific warning    (--disable-warning <num>)";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "--disable-warning ";
                switchToAdd.Name = nameof(DisableWarning);
                switchToAdd.StringList = value;
                this.ActiveToolSwitches.Add(nameof(DisableWarning), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }

        public bool WarnErrors
        {
            get
            {
                if (this.IsPropertySet(nameof(WarnErrors)))
                    return this.ActiveToolSwitches[nameof(WarnErrors)].BooleanValue;
                return false;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(WarnErrors));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
                switchToAdd.DisplayName = "Treat Warnings as Errors";
                switchToAdd.Description = "Treat Warnings as Errors.     (--Werror)";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "--Werror";
                switchToAdd.Name = nameof(WarnErrors);
                switchToAdd.BooleanValue = value;
                this.ActiveToolSwitches.Add(nameof(WarnErrors), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }

        public bool Debug
        {
            get
            {
                if (this.IsPropertySet(nameof(Debug)))
                    return this.ActiveToolSwitches[nameof(Debug)].BooleanValue;
                return false;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(Debug));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
                switchToAdd.DisplayName = "Enable Debugging Symbols";
                switchToAdd.Description = "Enable debugging symbol output.     (--debug)";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "--debug";
                switchToAdd.Name = nameof(Debug);
                switchToAdd.BooleanValue = value;
                this.ActiveToolSwitches.Add(nameof(Debug), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }

        public bool Cyclomatic
        {
            get
            {
                if (this.IsPropertySet(nameof(Cyclomatic)))
                    return this.ActiveToolSwitches[nameof(Cyclomatic)].BooleanValue;
                return false;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(Cyclomatic));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
                switchToAdd.DisplayName = "Cyclomatic";
                switchToAdd.Description = "Display complexity of the compiled functions.     (--cyclomatic)";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "--cyclomatic";
                switchToAdd.Name = nameof(Cyclomatic);
                switchToAdd.BooleanValue = value;
                this.ActiveToolSwitches.Add(nameof(Cyclomatic), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }

        public string CStandard
        {
            get
            {
                if (this.IsPropertySet(nameof(CStandard)))
                    return this.ActiveToolSwitches[nameof(CStandard)].Value;
                return (string)null;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(CStandard));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.String);
                switchToAdd.DisplayName = "C ISO Standard Format";
                switchToAdd.Description = "The C ISO Standard Format to use.";
                switchToAdd.ArgumentRelationList = new ArrayList();
                string[][] switchMap = new string[][]
                {
                    new string[2]{ "std-c89", "--std-c89" },
                    new string[2]{ "std-sdcc89", "--std-sdcc89" },
                    new string[2]{ "std-c95", "--std-c95" },
                    new string[2]{ "std-c99", "--std-c99" },
                    new string[2]{ "std-sdcc99", "--std-sdcc99" },
                    new string[2]{ "std-c11", "--std-c11" },
                    new string[2]{ "std-sdcc11", "--std-sdcc11" },
                    new string[2]{ "std-c2x", "--std-c2x" },
                    new string[2]{ "std-sdcc2x", "--std-sdcc2x" },
                };
                switchToAdd.SwitchValue = this.ReadSwitchMap(nameof(CStandard), switchMap, value);
                switchToAdd.Name = nameof(CStandard);
                switchToAdd.Value = value;
                switchToAdd.MultipleValues = true;
                this.ActiveToolSwitches.Add(nameof(CStandard), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }

        public bool DollarsInId
        {
            get
            {
                if (this.IsPropertySet(nameof(DollarsInId)))
                    return this.ActiveToolSwitches[nameof(DollarsInId)].BooleanValue;
                return false;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(DollarsInId));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
                switchToAdd.DisplayName = "Dollars In Identifiers";
                switchToAdd.Description = "Permit '$' as an identifier character.     (--fdollars-in-identifiers)";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "--fdollars-in-identifiers";
                switchToAdd.Name = nameof(DollarsInId);
                switchToAdd.BooleanValue = value;
                this.ActiveToolSwitches.Add(nameof(DollarsInId), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }

        public bool SignedChar
        {
            get
            {
                if (this.IsPropertySet(nameof(SignedChar)))
                    return this.ActiveToolSwitches[nameof(SignedChar)].BooleanValue;
                return false;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(SignedChar));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
                switchToAdd.DisplayName = "Signed Char";
                switchToAdd.Description = "Make 'char' signed by default.     (--fsigned-char)";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "--fsigned-char";
                switchToAdd.Name = nameof(SignedChar);
                switchToAdd.BooleanValue = value;
                this.ActiveToolSwitches.Add(nameof(SignedChar), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }

        public bool UseNonFree
        {
            get
            {
                if (this.IsPropertySet(nameof(UseNonFree)))
                    return this.ActiveToolSwitches[nameof(UseNonFree)].BooleanValue;
                return false;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(UseNonFree));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
                switchToAdd.DisplayName = "Use Non-Free Files";
                switchToAdd.Description = "Search / include non-free licensed libraries and header files.     (--use-non-free)";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "--use-non-free";
                switchToAdd.Name = nameof(UseNonFree);
                switchToAdd.BooleanValue = value;
                this.ActiveToolSwitches.Add(nameof(UseNonFree), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }


        #endregion

        #region Code Generation Properties.

        public string ProcessorType
        {
            get
            {
                if (this.IsPropertySet(nameof(ProcessorType)))
                    return this.ActiveToolSwitches[nameof(ProcessorType)].Value;
                return (string)null;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(ProcessorType));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.String);
                switchToAdd.DisplayName = "Processor Type";
                switchToAdd.Description = "Select port specific processor e.g. -mpic14 -p16f84.    (-p<processorId>)";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "-p";
                switchToAdd.Name = nameof(ProcessorType);
                switchToAdd.Value = value;
                this.ActiveToolSwitches.Add(nameof(ProcessorType), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }

        public bool StackAuto
        {
            get
            {
                if (this.IsPropertySet(nameof(StackAuto)))
                    return this.ActiveToolSwitches[nameof(StackAuto)].BooleanValue;
                return false;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(StackAuto));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
                switchToAdd.DisplayName = "Stack Automatic Variables";
                switchToAdd.Description = "Stack automatic variables.     (--stack-auto)";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "--stack-auto";
                switchToAdd.Name = nameof(StackAuto);
                switchToAdd.BooleanValue = value;
                this.ActiveToolSwitches.Add(nameof(StackAuto), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }

        public bool ExternalStack
        {
            get
            {
                if (this.IsPropertySet(nameof(ExternalStack)))
                    return this.ActiveToolSwitches[nameof(ExternalStack)].BooleanValue;
                return false;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(ExternalStack));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
                switchToAdd.DisplayName = "Use External Stack";
                switchToAdd.Description = "Use external stack.     (--xstack)";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "--xstack";
                switchToAdd.Name = nameof(ExternalStack);
                switchToAdd.BooleanValue = value;
                this.ActiveToolSwitches.Add(nameof(ExternalStack), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }

        public bool IntLongReent
        {
            get
            {
                if (this.IsPropertySet(nameof(IntLongReent)))
                    return this.ActiveToolSwitches[nameof(IntLongReent)].BooleanValue;
                return false;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(IntLongReent));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
                switchToAdd.DisplayName = "Int/Long Reenterant Functions";
                switchToAdd.Description = "Use reentrant calls on the int and long support functions.     (--int-long-reent)";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "--int-long-reent";
                switchToAdd.Name = nameof(IntLongReent);
                switchToAdd.BooleanValue = value;
                this.ActiveToolSwitches.Add(nameof(IntLongReent), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }

        public bool FloatReent
        {
            get
            {
                if (this.IsPropertySet(nameof(FloatReent)))
                    return this.ActiveToolSwitches[nameof(FloatReent)].BooleanValue;
                return false;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(FloatReent));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
                switchToAdd.DisplayName = "Float Reenterant Functions";
                switchToAdd.Description = "Use reentrant calls on the float support functions.     (--float-reent)";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "--float-reent";
                switchToAdd.Name = nameof(FloatReent);
                switchToAdd.BooleanValue = value;
                this.ActiveToolSwitches.Add(nameof(FloatReent), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }

        public bool XRamMovec
        {
            get
            {
                if (this.IsPropertySet(nameof(XRamMovec)))
                    return this.ActiveToolSwitches[nameof(XRamMovec)].BooleanValue;
                return false;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(XRamMovec));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
                switchToAdd.DisplayName = "Use MOVC to read XRAM";
                switchToAdd.Description = "Use movc instead of movx to read xram (xdata).     (--xram-movc)";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "--xram-movc";
                switchToAdd.Name = nameof(XRamMovec);
                switchToAdd.BooleanValue = value;
                this.ActiveToolSwitches.Add(nameof(XRamMovec), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }

        public string[] CalleeSaves
        {
            get
            {
                if (this.IsPropertySet(nameof(CalleeSaves)))
                    return this.ActiveToolSwitches[nameof(CalleeSaves)].StringList;
                return (string[])null;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(CalleeSaves));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.StringArray);
                switchToAdd.DisplayName = "Callee Saves Registers";
                switchToAdd.Description = "Cause the called function to save registers instead of the caller.     (--callee-saves <func[,func,...]>)";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "--callee-saves ";
                switchToAdd.Separator = ",";
                switchToAdd.Name = nameof(CalleeSaves);
                switchToAdd.StringList = value;
                this.ActiveToolSwitches.Add(nameof(CalleeSaves), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }

        public bool Profile
        {
            get
            {
                if (this.IsPropertySet(nameof(Profile)))
                    return this.ActiveToolSwitches[nameof(Profile)].BooleanValue;
                return false;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(Profile));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
                switchToAdd.DisplayName = "Profile";
                switchToAdd.Description = "On supported ports, generate extra profiling information.     (--profile)";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "--profile";
                switchToAdd.Name = nameof(Profile);
                switchToAdd.BooleanValue = value;
                this.ActiveToolSwitches.Add(nameof(Profile), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }

        public bool OmitFramePointer
        {
            get
            {
                if (this.IsPropertySet(nameof(OmitFramePointer)))
                    return this.ActiveToolSwitches[nameof(OmitFramePointer)].BooleanValue;
                return false;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(OmitFramePointer));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
                switchToAdd.DisplayName = "Omit Frame Pointer";
                switchToAdd.Description = "Leave out the frame pointer.     (--fomit-frame-pointer)";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "--fomit-frame-pointer";
                switchToAdd.Name = nameof(OmitFramePointer);
                switchToAdd.BooleanValue = value;
                this.ActiveToolSwitches.Add(nameof(OmitFramePointer), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }

        public bool AllCalleeSaves
        {
            get
            {
                if (this.IsPropertySet(nameof(AllCalleeSaves)))
                    return this.ActiveToolSwitches[nameof(AllCalleeSaves)].BooleanValue;
                return false;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(AllCalleeSaves));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
                switchToAdd.DisplayName = "All Callee Saves Register";
                switchToAdd.Description = "Callee will always save registers used.     (--all-callee-saves)";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "--all-callee-saves";
                switchToAdd.Name = nameof(AllCalleeSaves);
                switchToAdd.BooleanValue = value;
                this.ActiveToolSwitches.Add(nameof(AllCalleeSaves), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }

        public bool StackProbe
        {
            get
            {
                if (this.IsPropertySet(nameof(StackProbe)))
                    return this.ActiveToolSwitches[nameof(StackProbe)].BooleanValue;
                return false;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(StackProbe));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
                switchToAdd.DisplayName = "Stack Probe";
                switchToAdd.Description = "Insert call to function __stack_probe at each function prologue.     (--stack-probe)";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "--stack-probe";
                switchToAdd.Name = nameof(StackProbe);
                switchToAdd.BooleanValue = value;
                this.ActiveToolSwitches.Add(nameof(StackProbe), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }

        public bool NoXinitOpt
        {
            get
            {
                if (this.IsPropertySet(nameof(NoXinitOpt)))
                    return this.ActiveToolSwitches[nameof(NoXinitOpt)].BooleanValue;
                return false;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(NoXinitOpt));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
                switchToAdd.DisplayName = "No xData Init Optimization";
                switchToAdd.Description = "Will not memcpy initialized data from code space into xdata space. This saves a few bytes in code space if you don’t have initialized data.     (--no-xinit-opt)";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "--no-xinit-opt";
                switchToAdd.Name = nameof(NoXinitOpt);
                switchToAdd.BooleanValue = value;
                this.ActiveToolSwitches.Add(nameof(NoXinitOpt), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }

        public bool NoCCodeInAsm
        {
            get
            {
                if (this.IsPropertySet(nameof(NoCCodeInAsm)))
                    return this.ActiveToolSwitches[nameof(NoCCodeInAsm)].BooleanValue;
                return false;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(NoCCodeInAsm));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
                switchToAdd.DisplayName = "No C Code in Asm";
                switchToAdd.Description = "Don't include C-Code as comments in the asm file.     (--no-c-code-in-asm)";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "--no-c-code-in-asm";
                switchToAdd.Name = nameof(NoCCodeInAsm);
                switchToAdd.BooleanValue = value;
                this.ActiveToolSwitches.Add(nameof(NoCCodeInAsm), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }

        public bool NoPeepComments
        {
            get
            {
                if (this.IsPropertySet(nameof(NoPeepComments)))
                    return this.ActiveToolSwitches[nameof(NoPeepComments)].BooleanValue;
                return false;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(NoPeepComments));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
                switchToAdd.DisplayName = "No Peephole Comments";
                switchToAdd.Description = "Don't include peephole optimizer comments.     (--no-peep-comments)";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "--no-peep-comments";
                switchToAdd.Name = nameof(NoPeepComments);
                switchToAdd.BooleanValue = value;
                this.ActiveToolSwitches.Add(nameof(NoPeepComments), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }

        public string CodeSeg
        {
            get
            {
                if (this.IsPropertySet(nameof(CodeSeg)))
                    return this.ActiveToolSwitches[nameof(CodeSeg)].Value;
                return (string)null;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(CodeSeg));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.String);
                switchToAdd.DisplayName = "Code Segment Name";
                switchToAdd.Description = "Use this name for the code segment.     (--codeseg <name>)";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "--codeseg ";
                switchToAdd.Name = nameof(CodeSeg);
                switchToAdd.Value = value;
                this.ActiveToolSwitches.Add(nameof(CodeSeg), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }

        public string ConstSeg
        {
            get
            {
                if (this.IsPropertySet(nameof(ConstSeg)))
                    return this.ActiveToolSwitches[nameof(ConstSeg)].Value;
                return (string)null;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(ConstSeg));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.String);
                switchToAdd.DisplayName = "Const Segment Name";
                switchToAdd.Description = "Use this name for the const segment.     (--codeseg <name>)";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "--constseg ";
                switchToAdd.Name = nameof(ConstSeg);
                switchToAdd.Value = value;
                this.ActiveToolSwitches.Add(nameof(ConstSeg), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }

        public string DataSeg
        {
            get
            {
                if (this.IsPropertySet(nameof(DataSeg)))
                    return this.ActiveToolSwitches[nameof(DataSeg)].Value;
                return (string)null;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(DataSeg));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.String);
                switchToAdd.DisplayName = "Data Segment Name";
                switchToAdd.Description = "Use this name for the data segment.     (--dataseg <name>)";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "--dataseg ";
                switchToAdd.Name = nameof(DataSeg);
                switchToAdd.Value = value;
                this.ActiveToolSwitches.Add(nameof(DataSeg), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }

        #endregion

        #region Optimization Properties.

        public bool NoOverlay
        {
            get
            {
                if (this.IsPropertySet(nameof(NoOverlay)))
                    return this.ActiveToolSwitches[nameof(NoOverlay)].BooleanValue;
                return false;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(NoOverlay));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
                switchToAdd.DisplayName = "No Overlay Optimization";
                switchToAdd.Description = "The compiler will not overlay parameters and local variables of any function, see section Parameters and local variables for more details.     (--nooverlay)";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "--nooverlay";
                switchToAdd.Name = nameof(NoOverlay);
                switchToAdd.BooleanValue = value;
                this.ActiveToolSwitches.Add(nameof(NoOverlay), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }

        public bool NoGcse
        {
            get
            {
                if (this.IsPropertySet(nameof(NoGcse)))
                    return this.ActiveToolSwitches[nameof(NoGcse)].BooleanValue;
                return false;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(NoGcse));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
                switchToAdd.DisplayName = "No Global Common Subexpression Elimination";
                switchToAdd.Description = "Will not do global common subexpression elimination, this option may be used when the compiler creates undesirably large stack/data spaces to store compiler temporaries (spill locations, sloc). A warning message will be generated when this happens and the compiler will indicate the number of extra bytes it allocated. It is recommended that this option NOT be used, #pragma nogcse can be used to turn off global subexpression elimination for a given function only.     (--nogcse)";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "--nogcse";
                switchToAdd.Name = nameof(NoGcse);
                switchToAdd.BooleanValue = value;
                this.ActiveToolSwitches.Add(nameof(NoGcse), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }

        public bool NoLabelOpt
        {
            get
            {
                if (this.IsPropertySet(nameof(NoLabelOpt)))
                    return this.ActiveToolSwitches[nameof(NoLabelOpt)].BooleanValue;
                return false;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(NoLabelOpt));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
                switchToAdd.DisplayName = "No Label Optimization";
                switchToAdd.Description = "Will not optimize labels (makes the dumpfiles more readable).     (--nolabelopt)";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "--nolabelopt";
                switchToAdd.Name = nameof(NoLabelOpt);
                switchToAdd.BooleanValue = value;
                this.ActiveToolSwitches.Add(nameof(NoLabelOpt), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }

        public bool NoInvariant
        {
            get
            {
                if (this.IsPropertySet(nameof(NoInvariant)))
                    return this.ActiveToolSwitches[nameof(NoInvariant)].BooleanValue;
                return false;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(NoInvariant));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
                switchToAdd.DisplayName = "No Loop Invariant Optimization";
                switchToAdd.Description = "Will not do loop invariant optimizations, this may be turned off for reasons explained for the previous option. For more details of loop optimizations performed see Loop Invariants in section 8.1.4. It is recommended that this option NOT be used, #pragma noinvariant can be used to turn off invariant optimizations for a given function only.     (--noinvariant)";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "--noinvariant";
                switchToAdd.Name = nameof(NoInvariant);
                switchToAdd.BooleanValue = value;
                this.ActiveToolSwitches.Add(nameof(NoInvariant), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }

        public bool NoInduction
        {
            get
            {
                if (this.IsPropertySet(nameof(NoInduction)))
                    return this.ActiveToolSwitches[nameof(NoInduction)].BooleanValue;
                return false;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(NoInduction));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
                switchToAdd.DisplayName = "No Loop Induction Optimization";
                switchToAdd.Description = "Will not do loop induction optimizations, see section strength reduction for more details. It is recommended that this option is NOT used, #pragma noinduction can be used to turn off induction optimizations for a given function only.     (--noinduction)";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "--noinduction";
                switchToAdd.Name = nameof(NoInduction);
                switchToAdd.BooleanValue = value;
                this.ActiveToolSwitches.Add(nameof(NoInduction), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }

        public bool NoLoopReverse
        {
            get
            {
                if (this.IsPropertySet(nameof(NoLoopReverse)))
                    return this.ActiveToolSwitches[nameof(NoLoopReverse)].BooleanValue;
                return false;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(NoLoopReverse));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
                switchToAdd.DisplayName = "Disable Loop Reverse Optimization";
                switchToAdd.Description = "Disable the loop reverse optimization.     (--noloopreverse)";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "--noloopreverse";
                switchToAdd.Name = nameof(NoLoopReverse);
                switchToAdd.BooleanValue = value;
                this.ActiveToolSwitches.Add(nameof(NoLoopReverse), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }

        public bool NoPeep
        {
            get
            {
                if (this.IsPropertySet(nameof(NoPeep)))
                    return this.ActiveToolSwitches[nameof(NoPeep)].BooleanValue;
                return false;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(NoPeep));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
                switchToAdd.DisplayName = "No Peep-Hole Optimization";
                switchToAdd.Description = "Disable peep-hole optimization with built-in rules.     (--no-peep)";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "--no-peep";
                switchToAdd.Name = nameof(NoPeep);
                switchToAdd.BooleanValue = value;
                this.ActiveToolSwitches.Add(nameof(NoPeep), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }

        public bool NoRegParams
        {
            get
            {
                if (this.IsPropertySet(nameof(NoRegParams)))
                    return this.ActiveToolSwitches[nameof(NoRegParams)].BooleanValue;
                return false;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(NoRegParams));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
                switchToAdd.DisplayName = "Disable Passing Parameters in Registers";
                switchToAdd.Description = "On some ports, disable passing some parameters in registers.     (--no-reg-params)";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "--no-reg-params";
                switchToAdd.Name = nameof(NoRegParams);
                switchToAdd.BooleanValue = value;
                this.ActiveToolSwitches.Add(nameof(NoRegParams), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }

        public bool PeepAsm
        {
            get
            {
                if (this.IsPropertySet(nameof(PeepAsm)))
                    return this.ActiveToolSwitches[nameof(PeepAsm)].BooleanValue;
                return false;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(PeepAsm));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
                switchToAdd.DisplayName = "Assembly Peep-Hole Optimization";
                switchToAdd.Description = "Pass the inline assembler code through the peep hole optimizer. This can cause unexpected changes to inline assembler code, please go through the peephole optimizer rules defined in the source file tree ’<target>/peeph.def’ before using this option     (--peep-asm)";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "--peep-asm";
                switchToAdd.Name = nameof(PeepAsm);
                switchToAdd.BooleanValue = value;
                this.ActiveToolSwitches.Add(nameof(PeepAsm), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }

        public bool PeepReturn
        {
            get
            {
                if (this.IsPropertySet(nameof(PeepReturn)))
                    return this.ActiveToolSwitches[nameof(PeepReturn)].BooleanValue;
                return false;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(PeepReturn));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
                switchToAdd.DisplayName = "Peep-Hole Return Optimization";
                switchToAdd.Description = "Let the peep hole optimizer do return optimizations.This is the default without--debug.     (--peep -return)";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "--peep-return";
                switchToAdd.Name = nameof(PeepReturn);
                switchToAdd.BooleanValue = value;
                this.ActiveToolSwitches.Add(nameof(PeepReturn), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }

        public bool NoPeepReturn
        {
            get
            {
                if (this.IsPropertySet(nameof(NoPeepReturn)))
                    return this.ActiveToolSwitches[nameof(NoPeepReturn)].BooleanValue;
                return false;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(NoPeepReturn));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
                switchToAdd.DisplayName = "No Peep-Hole Return Optimization";
                switchToAdd.Description = "Do not let the peep hole optimizer do return optimizations. This is the default with --debug.     (--no-peep-return)";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "--no-peep-return";
                switchToAdd.Name = nameof(NoPeepReturn);
                switchToAdd.BooleanValue = value;
                this.ActiveToolSwitches.Add(nameof(NoPeepReturn), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }

        public string PeepFile
        {
            get
            {
                if (this.IsPropertySet(nameof(PeepFile)))
                    return this.ActiveToolSwitches[nameof(PeepFile)].Value;
                return (string)null;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(PeepFile));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.File);
                switchToAdd.DisplayName = "Peep Optimizer File";
                switchToAdd.Description = "This option can be used to use additional rules to be used by the peep hole optimizer. See section 8.1.16 Peep Hole optimizations for details on how to write these rules.     (--peep-file <filename>)";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "--peep-file ";
                switchToAdd.Name = nameof(PeepFile);
                switchToAdd.Value = value;
                this.ActiveToolSwitches.Add(nameof(PeepFile), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }

        public string OptimizeCode
        {
            get
            {
                if (this.IsPropertySet(nameof(OptimizeCode)))
                    return this.ActiveToolSwitches[nameof(OptimizeCode)].Value;
                return (string)null;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(OptimizeCode));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.String);
                switchToAdd.DisplayName = "Code Optimization";
                switchToAdd.Description = "Optimize code generation.";
                switchToAdd.ArgumentRelationList = new ArrayList();
                string[][] switchMap = new string[][]
                {
                    new string[2]{ "CodeSpeed", "--opt-code-speed" },
                    new string[2]{ "CodeSize", "--opt-code-size" },
                };
                switchToAdd.SwitchValue = this.ReadSwitchMap(nameof(OptimizeCode), switchMap, value);
                switchToAdd.Name = nameof(OptimizeCode);
                switchToAdd.Value = value;
                switchToAdd.MultipleValues = true;
                this.ActiveToolSwitches.Add(nameof(OptimizeCode), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }

        public int MaxAllocsPerNode
        {
            get
            {
                if (this.IsPropertySet(nameof(MaxAllocsPerNode)))
                    return this.ActiveToolSwitches[nameof(MaxAllocsPerNode)].Number;
                return 0;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(MaxAllocsPerNode));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Integer);
                switchToAdd.DisplayName = "Max Allocations Per Node";
                switchToAdd.Description = "Setting this to a high value will result in increased compilation time and more optimized code being generated. Setting it to lower values speed up compilation, but does not optimize as much. The default value is 3000. This option currently only affects the hc08, s08, z80, z180, r2k, r3ka and gbz80 ports.     (--max-allocs-per-node)";
                //switchToAdd.Parents.AddLast("");
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.IsValid = this.ValidateInteger(nameof(MaxAllocsPerNode), int.MinValue, int.MaxValue, value);
                switchToAdd.SwitchValue = "--max-allocs-per-node ";
                switchToAdd.Name = nameof(MaxAllocsPerNode);
                switchToAdd.Number = value;
                this.ActiveToolSwitches.Add(nameof(MaxAllocsPerNode), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }

        public bool NoLospre
        {
            get
            {
                if (this.IsPropertySet(nameof(NoLospre)))
                    return this.ActiveToolSwitches[nameof(NoLospre)].BooleanValue;
                return false;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(NoLospre));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
                switchToAdd.DisplayName = "No Lospre";
                switchToAdd.Description = "Disable lospre. lospre is an advanced redundancy elimination technique, essentially an improved variant of global subexpression elimination.     (--nolospre)";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "--nolospre";
                switchToAdd.Name = nameof(NoLospre);
                switchToAdd.BooleanValue = value;
                this.ActiveToolSwitches.Add(nameof(NoLospre), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }

        public bool AllowUnsafeRead
        {
            get
            {
                if (this.IsPropertySet(nameof(AllowUnsafeRead)))
                    return this.ActiveToolSwitches[nameof(AllowUnsafeRead)].BooleanValue;
                return false;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(AllowUnsafeRead));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
                switchToAdd.DisplayName = "Allow Unsafe Reads";
                switchToAdd.Description = "Allow optimizations to generate unsafe reads. This will enable additional optimizations, but can result in spurious reads from undefined memory addresses, which can be harmful if the target system uses certain ways of doing memory-mapped I/O.     (--allow-unsafe-read)";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "--allow-unsafe-read";
                switchToAdd.Name = nameof(AllowUnsafeRead);
                switchToAdd.BooleanValue = value;
                this.ActiveToolSwitches.Add(nameof(AllowUnsafeRead), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }

        public bool NoStdLibCalls
        {
            get
            {
                if (this.IsPropertySet(nameof(NoStdLibCalls)))
                    return this.ActiveToolSwitches[nameof(NoStdLibCalls)].BooleanValue;
                return false;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(NoStdLibCalls));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
                switchToAdd.DisplayName = "No STD Lib Calls";
                switchToAdd.Description = "Disable optimization of calls to standard library.     (--nostdlibcall)";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "--nostdlibcall";
                switchToAdd.Name = nameof(NoStdLibCalls);
                switchToAdd.BooleanValue = value;
                this.ActiveToolSwitches.Add(nameof(NoStdLibCalls), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }

        #endregion

        #region Degbugging Properties.

        public bool DumpAst
        {
            get
            {
                if (this.IsPropertySet(nameof(DumpAst)))
                    return this.ActiveToolSwitches[nameof(DumpAst)].BooleanValue;
                return false;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(DumpAst));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
                switchToAdd.DisplayName = "Dump AST";
                switchToAdd.Description = "Dump front-end AST before generating i-code.     (--dump-ast)";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "--dump-ast";
                switchToAdd.Name = nameof(DumpAst);
                switchToAdd.BooleanValue = value;
                this.ActiveToolSwitches.Add(nameof(DumpAst), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }

        public bool DumpICode
        {
            get
            {
                if (this.IsPropertySet(nameof(DumpICode)))
                    return this.ActiveToolSwitches[nameof(DumpICode)].BooleanValue;
                return false;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(DumpICode));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
                switchToAdd.DisplayName = "Dump I-Code";
                switchToAdd.Description = "Dump the i-code structure at all stages.     (--dump-i-code)";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "--dump-i-code";
                switchToAdd.Name = nameof(DumpICode);
                switchToAdd.BooleanValue = value;
                this.ActiveToolSwitches.Add(nameof(DumpICode), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }

        public bool DumpGraphs
        {
            get
            {
                if (this.IsPropertySet(nameof(DumpGraphs)))
                    return this.ActiveToolSwitches[nameof(DumpGraphs)].BooleanValue;
                return false;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(DumpGraphs));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
                switchToAdd.DisplayName = "Dump Graphs";
                switchToAdd.Description = "Dump graphs (control-flow, conflict, etc).     (--dump-graphs)";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "--dump-graphs";
                switchToAdd.Name = nameof(DumpGraphs);
                switchToAdd.BooleanValue = value;
                this.ActiveToolSwitches.Add(nameof(DumpGraphs), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }

        public bool ICodeInAsm
        {
            get
            {
                if (this.IsPropertySet(nameof(ICodeInAsm)))
                    return this.ActiveToolSwitches[nameof(ICodeInAsm)].BooleanValue;
                return false;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(ICodeInAsm));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
                switchToAdd.DisplayName = "Include I-Code in ASM";
                switchToAdd.Description = "Include i-code as comments in the asm file.     (--i-code-in-asm)";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "--i-code-in-asm";
                switchToAdd.Name = nameof(ICodeInAsm);
                switchToAdd.BooleanValue = value;
                this.ActiveToolSwitches.Add(nameof(ICodeInAsm), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }

        public bool VerboseAsm
        {
            get
            {
                if (this.IsPropertySet(nameof(VerboseAsm)))
                    return this.ActiveToolSwitches[nameof(VerboseAsm)].BooleanValue;
                return false;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(VerboseAsm));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
                switchToAdd.DisplayName = "Include Code Generator Comments in ASM";
                switchToAdd.Description = "Include code generator comments in the asm output.     (--fverbose-asm)";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "--fverbose-asm";
                switchToAdd.Name = nameof(VerboseAsm);
                switchToAdd.BooleanValue = value;
                this.ActiveToolSwitches.Add(nameof(VerboseAsm), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }

        #endregion

        #region Linker Properties.

        public string[] IncludeLibraries
        {
            get
            {
                if (this.IsPropertySet(nameof(IncludeLibraries)))
                    return this.ActiveToolSwitches[nameof(IncludeLibraries)].StringList;
                return (string[])null;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(IncludeLibraries));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.StringArray);
                switchToAdd.DisplayName = "Include Additional Library";
                switchToAdd.Description = "Include the given library in the link.    (-l<library>)";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "-l ";
                switchToAdd.Name = nameof(IncludeLibraries);
                switchToAdd.StringList = value;
                this.ActiveToolSwitches.Add(nameof(IncludeLibraries), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }

        public string[] AdditionalLibraryPath
        {
            get
            {
                if (this.IsPropertySet(nameof(AdditionalLibraryPath)))
                    return this.ActiveToolSwitches[nameof(AdditionalLibraryPath)].StringList;
                return (string[])null;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(AdditionalLibraryPath));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.StringArray);
                switchToAdd.DisplayName = "Additional Library Path";
                switchToAdd.Description = "Path to search for libraries.    (-L<path>)";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "-L ";
                switchToAdd.Name = nameof(AdditionalLibraryPath);
                switchToAdd.StringList = value;
                this.ActiveToolSwitches.Add(nameof(AdditionalLibraryPath), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }

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
                switchToAdd.DisplayName = "Linker Output Format";
                switchToAdd.Description = "Optimize code generation.";
                switchToAdd.ArgumentRelationList = new ArrayList();
                string[][] switchMap = new string[][]
                {
                    new string[2]{ "Intel", "--out-fmt-ihx" },
                    new string[2]{ "S19", "--out-fmt-s19" },
                    new string[2]{ "ELF", "--out-fmt-elf" },
                };
                switchToAdd.SwitchValue = this.ReadSwitchMap(nameof(OutputFormat), switchMap, value);
                switchToAdd.Name = nameof(OutputFormat);
                switchToAdd.Value = value;
                switchToAdd.MultipleValues = true;
                this.ActiveToolSwitches.Add(nameof(OutputFormat), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }

        public string XRamLocation
        {
            get
            {
                if (this.IsPropertySet(nameof(XRamLocation)))
                    return this.ActiveToolSwitches[nameof(XRamLocation)].Value;
                return null;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(XRamLocation));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.String);
                switchToAdd.DisplayName = "External Ram Location";
                switchToAdd.Description = "The start location of the external ram, default value is 0. The value entered can be in Hexadecimal or Decimal format, e.g.: --xram-loc 0x8000 or --xram-loc 32768.     (--xram-loc <Value>)";
                //switchToAdd.Parents.AddLast("");
                switchToAdd.ArgumentRelationList = new ArrayList();
                var intConverter = new Int32Converter();
                switchToAdd.IsValid = this.ValidateInteger(nameof(DataLocation), int.MinValue, int.MaxValue,
                    (int)intConverter.ConvertFromString(value));
                switchToAdd.SwitchValue = "--xram-loc ";
                switchToAdd.Name = nameof(XRamLocation);
                switchToAdd.Value = value;
                this.ActiveToolSwitches.Add(nameof(XRamLocation), switchToAdd);
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
                switchToAdd.DisplayName = "External Ram Size";
                switchToAdd.Description = "External Ram size.    (--xram-size <value>)";
                //switchToAdd.Parents.AddLast("");
                switchToAdd.ArgumentRelationList = new ArrayList();
                var intConverter = new Int32Converter();
                switchToAdd.IsValid = this.ValidateInteger(nameof(DataLocation), int.MinValue, int.MaxValue,
                    (int)intConverter.ConvertFromString(value));
                switchToAdd.SwitchValue = "--xram-size ";
                switchToAdd.Name = nameof(XRamSize);
                switchToAdd.Value = value;
                this.ActiveToolSwitches.Add(nameof(XRamSize), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }

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
                switchToAdd.DisplayName = "Internal Ram Size";
                switchToAdd.Description = "Internal Ram size.    (--iram-size <value>)";
                //switchToAdd.Parents.AddLast("");
                switchToAdd.ArgumentRelationList = new ArrayList();
                var intConverter = new Int32Converter();
                switchToAdd.IsValid = this.ValidateInteger(nameof(DataLocation), int.MinValue, int.MaxValue,
                    (int)intConverter.ConvertFromString(value));
                switchToAdd.SwitchValue = "--iram-size ";
                switchToAdd.Name = nameof(IRamSize);
                switchToAdd.Value = value;
                this.ActiveToolSwitches.Add(nameof(IRamSize), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }

        public string XStackLocation
        {
            get
            {
                if (this.IsPropertySet(nameof(XStackLocation)))
                    return this.ActiveToolSwitches[nameof(XStackLocation)].Value;
                return null;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(XStackLocation));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.String);
                switchToAdd.DisplayName = "External Stack Location";
                switchToAdd.Description = "By default the external stack is placed after the __pdata segment. Using this option the xstack can be placed anywhere in the external memory space of the 8051. The value entered can be in Hexadecimal or Decimal format, e.g. --xstack-loc 0x8000 or --stack-loc 32768. The provided value should not overlap any other memory areas such as the pdata or xdata segment and with enough space for the current application.     (--xstack-loc <Value>)";
                //switchToAdd.Parents.AddLast("");
                switchToAdd.ArgumentRelationList = new ArrayList();
                var intConverter = new Int32Converter();
                switchToAdd.IsValid = this.ValidateInteger(nameof(DataLocation), int.MinValue, int.MaxValue,
                    (int)intConverter.ConvertFromString(value));
                switchToAdd.SwitchValue = "--xstack-loc ";
                switchToAdd.Name = nameof(XStackLocation);
                switchToAdd.Value = value;
                this.ActiveToolSwitches.Add(nameof(XStackLocation), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }

        public string CodeLocation
        {
            get
            {
                if (this.IsPropertySet(nameof(CodeLocation)))
                    return this.ActiveToolSwitches[nameof(CodeLocation)].Value;
                return null;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(CodeLocation));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.String);
                switchToAdd.DisplayName = "Code Segment Location";
                switchToAdd.Description = "The start location of the code segment, default value 0. Note when this option is used the interrupt vector table is also relocated to the given address. The value entered can be in Hexadecimal or Decimal format, e.g.: --code-loc 0x8000 or --code-loc 32768.     (--code-loc <Value>)";
                //switchToAdd.Parents.AddLast("");
                switchToAdd.ArgumentRelationList = new ArrayList();
                var intConverter = new Int32Converter();
                switchToAdd.IsValid = this.ValidateInteger(nameof(DataLocation), int.MinValue, int.MaxValue,
                    (int)intConverter.ConvertFromString(value));
                switchToAdd.SwitchValue = "--code-loc ";
                switchToAdd.Name = nameof(CodeLocation);
                switchToAdd.Value = value;
                this.ActiveToolSwitches.Add(nameof(CodeLocation), switchToAdd);
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
                switchToAdd.DisplayName = "Code Segment Size";
                switchToAdd.Description = "Code Segment size.    (--code-size <value>)";
                //switchToAdd.Parents.AddLast("");
                switchToAdd.ArgumentRelationList = new ArrayList();
                var intConverter = new Int32Converter();
                switchToAdd.IsValid = this.ValidateInteger(nameof(DataLocation), int.MinValue, int.MaxValue,
                    (int)intConverter.ConvertFromString(value));
                switchToAdd.SwitchValue = "--code-size ";
                switchToAdd.Name = nameof(CodeSize);
                switchToAdd.Value = value;
                this.ActiveToolSwitches.Add(nameof(CodeSize), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }

        public string StackLocation
        {
            get
            {
                if (this.IsPropertySet(nameof(StackLocation)))
                    return this.ActiveToolSwitches[nameof(StackLocation)].Value;
                return null;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(StackLocation));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.String);
                switchToAdd.DisplayName = "Stack Location";
                switchToAdd.Description = "By default the stack is placed after the data segment for mcs51. By using this option the stack can be placed anywhere in the internal memory space of the mcs51. The value entered can be in Hexadecimal or Decimal format, e.g. --stack-loc 0x20 or --stack-loc 32. Since the sp register is incremented before a push or call, the initial sp will be set to one byte prior the provided value. The provided value should not overlap any other memory areas such as used register banks or the data segment and with enough space for the current application. The --pack-iram option (which is now a default setting) will override this setting, so you should also specify the --no-pack-iram option if you need to manually place the stack.\n For stm8, by default the stack is placed at the device-specific reset value. By using this option, the stack can be placed anywhere in the lower 16-bits of the stm8 memory space. This is particularly useful for working around the stack roll-over antifeature present in some stm8 devices.     (--stack-loc <Value>)";
                //switchToAdd.Parents.AddLast("");
                switchToAdd.ArgumentRelationList = new ArrayList();
                var intConverter = new Int32Converter();
                switchToAdd.IsValid = this.ValidateInteger(nameof(DataLocation), int.MinValue, int.MaxValue,
                    (int)intConverter.ConvertFromString(value));
                switchToAdd.SwitchValue = "--stack-loc ";
                switchToAdd.Name = nameof(StackLocation);
                switchToAdd.Value = value;
                this.ActiveToolSwitches.Add(nameof(StackLocation), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }

        public string DataLocation
        {
            get
            {
                if (this.IsPropertySet(nameof(DataLocation)))
                    return this.ActiveToolSwitches[nameof(DataLocation)].Value;
                return null;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(DataLocation));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.String);
                switchToAdd.DisplayName = "Ram Data Segment Location";
                switchToAdd.Description = "The start location of the internal ram data segment. The value entered can be in Hexadecimal or Decimal format, eg. --data-loc 0x20 or --data-loc 32. (By default, the start location of the internal ram data segment is set as low as possible in memory, taking into account the used register banks and the bit segment at address 0x20. For example if register banks 0 and 1 are used without bit variables, the data segment will be set, if --data-loc is not used, to location 0x10.).     (--data-loc <Value>)";
                //switchToAdd.Parents.AddLast("");
                switchToAdd.ArgumentRelationList = new ArrayList();
                var intConverter = new Int32Converter();
                switchToAdd.IsValid = this.ValidateInteger(nameof(DataLocation), int.MinValue, int.MaxValue,
                    (int)intConverter.ConvertFromString(value));
                switchToAdd.SwitchValue = "--data-loc ";
                switchToAdd.Name = nameof(DataLocation);
                switchToAdd.Value = value;
                this.ActiveToolSwitches.Add(nameof(DataLocation), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }

        public string IDataLocation
        {
            get
            {
                if (this.IsPropertySet(nameof(IDataLocation)))
                    return this.ActiveToolSwitches[nameof(IDataLocation)].Value;
                return null;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(IDataLocation));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.String);
                switchToAdd.DisplayName = "Internal Ram Data Segment Location";
                switchToAdd.Description = "The start location of the indirectly addressable internal ram of the 8051, default value is 0x80. The value entered can be in Hexadecimal or Decimal format, eg. --idata-loc 0x88 or --idata-loc 136.     (--idata-loc <Value>)";
                //switchToAdd.Parents.AddLast("");
                switchToAdd.ArgumentRelationList = new ArrayList();
                var intConverter = new Int32Converter();
                switchToAdd.IsValid = this.ValidateInteger(nameof(DataLocation), int.MinValue, int.MaxValue,
                    (int)intConverter.ConvertFromString(value));
                switchToAdd.SwitchValue = "--idata-loc ";
                switchToAdd.Name = nameof(IDataLocation);
                switchToAdd.Value = value;
                this.ActiveToolSwitches.Add(nameof(IDataLocation), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }

        public bool NoOptSdccInAsm
        {
            get
            {
                if (this.IsPropertySet(nameof(NoOptSdccInAsm)))
                    return this.ActiveToolSwitches[nameof(NoOptSdccInAsm)].BooleanValue;
                return false;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(NoOptSdccInAsm));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
                switchToAdd.DisplayName = "No .optsdcc in Asm";
                switchToAdd.Description = "Do not emit .optsdcc in asm.     (--no-optsdcc-in-asm)";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "--no-optsdcc-in-asm";
                switchToAdd.Name = nameof(NoOptSdccInAsm);
                switchToAdd.BooleanValue = value;
                this.ActiveToolSwitches.Add(nameof(NoOptSdccInAsm), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }

        #endregion

        #region Unclassified Properties.

        public bool KeepComments
        {
            get
            {
                if (this.IsPropertySet(nameof(KeepComments)))
                    return this.ActiveToolSwitches[nameof(KeepComments)].BooleanValue;
                return false;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(KeepComments));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
                switchToAdd.DisplayName = "Keep Comments";
                switchToAdd.Description = "Tell the preprocessor not to discard comments. Used with the ‘-E’ option.     (-C)";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "-C";
                switchToAdd.Name = nameof(KeepComments);
                switchToAdd.BooleanValue = value;
                this.ActiveToolSwitches.Add(nameof(KeepComments), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }

        public string PreprocessorOutput
        {
            get
            {
                if (this.IsPropertySet(nameof(PreprocessorOutput)))
                    return this.ActiveToolSwitches[nameof(PreprocessorOutput)].Value;
                return (string)null;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(PreprocessorOutput));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.String);
                switchToAdd.DisplayName = "Preprocessor Output";
                switchToAdd.Description = "Output a list of preprocessor macro definitions to the output.";
                switchToAdd.ArgumentRelationList = new ArrayList();
                string[][] switchMap = new string[][]
                {
                    new string[2]{ "MacroOnly", "-dM" },
                    new string[2]{ "AllMacros", "-dD" },
                    new string[2]{ "ExcludeArgContent", "-dN" },
                };
                switchToAdd.SwitchValue = this.ReadSwitchMap(nameof(PreprocessorOutput), switchMap, value);
                switchToAdd.Name = nameof(PreprocessorOutput);
                switchToAdd.Value = value;
                switchToAdd.MultipleValues = true;
                this.ActiveToolSwitches.Add(nameof(PreprocessorOutput), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }

        public bool PedanticParseNum
        {
            get
            {
                if (this.IsPropertySet(nameof(PedanticParseNum)))
                    return this.ActiveToolSwitches[nameof(PedanticParseNum)].BooleanValue;
                return false;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(PedanticParseNum));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
                switchToAdd.DisplayName = "Pedantic Parse Number";
                switchToAdd.Description = "Pedantic parse numbers so that situations like 0xfe-LO_B(3) are parsed properly and the macro LO_B(3) gets expanded. See also #pragma pedantic_parse_number on page 58 in section3.16 Note: this functionality is not in conformance with C99 standard!     (-pedantic-parse-number)";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "-pedantic-parse-number";
                switchToAdd.Name = nameof(PedanticParseNum);
                switchToAdd.BooleanValue = value;
                this.ActiveToolSwitches.Add(nameof(PedanticParseNum), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }

        #endregion

        #region Port Specific Commands.

        public bool UseACallAimp
        {
            get
            {
                if (this.IsPropertySet(nameof(UseACallAimp)))
                    return this.ActiveToolSwitches[nameof(UseACallAimp)].BooleanValue;
                return false;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(UseACallAimp));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
                switchToAdd.DisplayName = "Use acall/aimp instead of lcall/limp";
                switchToAdd.Description = "Use acall/aimp instead of lcall/limp.     (--acall-aimp)";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "--acall-aimp";
                switchToAdd.Name = nameof(UseACallAimp);
                switchToAdd.BooleanValue = value;
                this.ActiveToolSwitches.Add(nameof(UseACallAimp), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }
        public string UseAssembler
        {
            get
            {
                if (this.IsPropertySet(nameof(UseAssembler)))
                    return this.ActiveToolSwitches[nameof(UseAssembler)].Value;
                return null;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(UseAssembler));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.String);
                switchToAdd.DisplayName = "Use Alternative Assembler";
                switchToAdd.Description = "Use alternative assembler.    (--asm=<assembler>)";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "--asm=";
                switchToAdd.Name = nameof(UseAssembler);
                switchToAdd.Value = value;
                this.ActiveToolSwitches.Add(nameof(UseAssembler), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }
        public bool CalleeSavesBC
        {
            get
            {
                if (this.IsPropertySet(nameof(CalleeSavesBC)))
                    return this.ActiveToolSwitches[nameof(CalleeSavesBC)].BooleanValue;
                return false;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(CalleeSavesBC));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
                switchToAdd.DisplayName = "Function Calls Save BC";
                switchToAdd.Description = "Force a called function to always save BC.    (--callee-saves-bc)";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "--callee-saves-bc";
                switchToAdd.Name = nameof(CalleeSavesBC);
                switchToAdd.BooleanValue = value;
                this.ActiveToolSwitches.Add(nameof(CalleeSavesBC), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }
        public bool DumpCallTree
        {
            get
            {
                if (this.IsPropertySet(nameof(DumpCallTree)))
                    return this.ActiveToolSwitches[nameof(DumpCallTree)].BooleanValue;
                return false;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(DumpCallTree));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
                switchToAdd.DisplayName = "Dump Call Tree File";
                switchToAdd.Description = "Dump Call Tree in .calltree file.     (--calltree)";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "--calltree";
                switchToAdd.Name = nameof(DumpCallTree);
                switchToAdd.BooleanValue = value;
                this.ActiveToolSwitches.Add(nameof(DumpCallTree), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }
        public bool DebugRalloc
        {
            get
            {
                if (this.IsPropertySet(nameof(DebugRalloc)))
                    return this.ActiveToolSwitches[nameof(DebugRalloc)].BooleanValue;
                return false;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(DebugRalloc));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
                switchToAdd.DisplayName = "Debug Register Allocator";
                switchToAdd.Description = "Dump register allocator debug file *.d.     (--debug-ralloc)";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "--debug-ralloc";
                switchToAdd.Name = nameof(DebugRalloc);
                switchToAdd.BooleanValue = value;
                this.ActiveToolSwitches.Add(nameof(DebugRalloc), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }
        public bool DebugExtra
        {
            get
            {
                if (this.IsPropertySet(nameof(DebugExtra)))
                    return this.ActiveToolSwitches[nameof(DebugExtra)].BooleanValue;
                return false;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(DebugExtra));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
                switchToAdd.DisplayName = "Debug Xtra";
                switchToAdd.Description = "Show more debug info in assembly output.     (--debug-xtra)";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "--debug-xtra";
                switchToAdd.Name = nameof(DebugExtra);
                switchToAdd.BooleanValue = value;
                this.ActiveToolSwitches.Add(nameof(DebugExtra), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }
        public bool EnablePeeps
        {
            get
            {
                if (this.IsPropertySet(nameof(EnablePeeps)))
                    return this.ActiveToolSwitches[nameof(EnablePeeps)].BooleanValue;
                return false;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(EnablePeeps));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
                switchToAdd.DisplayName = "Enable Peepholes";
                switchToAdd.Description = "Explict enable of peepholes.     (--denable-peeps)";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "--denable-peeps";
                switchToAdd.Name = nameof(EnablePeeps);
                switchToAdd.BooleanValue = value;
                this.ActiveToolSwitches.Add(nameof(EnablePeeps), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }
        public bool EmitExterns
        {
            get
            {
                if (this.IsPropertySet(nameof(EmitExterns)))
                    return this.ActiveToolSwitches[nameof(EmitExterns)].BooleanValue;
                return false;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(EmitExterns));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
                switchToAdd.DisplayName = "Emit Externs";
                switchToAdd.Description = "Emit externs list in generated asm.     (--emit-externs)";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "";
                switchToAdd.Name = nameof(EmitExterns);
                switchToAdd.BooleanValue = value;
                this.ActiveToolSwitches.Add(nameof(EmitExterns), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }
        public bool NoEmitFramePointer
        {
            get
            {
                if (this.IsPropertySet(nameof(NoEmitFramePointer)))
                    return this.ActiveToolSwitches[nameof(NoEmitFramePointer)].BooleanValue;
                return false;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(NoEmitFramePointer));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
                switchToAdd.DisplayName = "No Omit Frame Pointer";
                switchToAdd.Description = "Do not omit frame pointer.     (--fno-omit-frame-pointer)";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "--fno-omit-frame-pointer";
                switchToAdd.Name = nameof(NoEmitFramePointer);
                switchToAdd.BooleanValue = value;
                this.ActiveToolSwitches.Add(nameof(NoEmitFramePointer), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }
        public bool GStack
        {
            get
            {
                if (this.IsPropertySet(nameof(GStack)))
                    return this.ActiveToolSwitches[nameof(GStack)].BooleanValue;
                return false;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(GStack));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
                switchToAdd.DisplayName = "Trace Stack Pointer";
                switchToAdd.Description = "Trace stack pointer push/pop to overflow.    (--gstack)";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "--gstack";
                switchToAdd.Name = nameof(GStack);
                switchToAdd.BooleanValue = value;
                this.ActiveToolSwitches.Add(nameof(GStack), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }
        public string IVTAddress
        {
            get
            {
                if (this.IsPropertySet(nameof(IVTAddress)))
                    return this.ActiveToolSwitches[nameof(IVTAddress)].Value;
                return null;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(IVTAddress));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.String);
                switchToAdd.DisplayName = "Interrupt Vector Table Address";
                switchToAdd.Description = "Set address of interrupt vector table.     (--ivt-loc=<address>)";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "--ivt-loc=";
                switchToAdd.Name = nameof(IVTAddress);
                switchToAdd.Value = value;
                this.ActiveToolSwitches.Add(nameof(IVTAddress), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }
        public string AltLinker
        {
            get
            {
                if (this.IsPropertySet(nameof(AltLinker)))
                    return this.ActiveToolSwitches[nameof(AltLinker)].Value;
                return null;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(AltLinker));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.String);
                switchToAdd.DisplayName = "Alternative Linker";
                switchToAdd.Description = "Use alternative Linker.     (--link=<linker>)";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "--link=";
                switchToAdd.Name = nameof(AltLinker);
                switchToAdd.Value = value;
                this.ActiveToolSwitches.Add(nameof(AltLinker), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }
        public string MemoryModel
        {
            get
            {
                if (this.IsPropertySet(nameof(MemoryModel)))
                    return this.ActiveToolSwitches[nameof(MemoryModel)].Value;
                return null;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(MemoryModel));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.String);
                switchToAdd.DisplayName = "Memory Model";
                switchToAdd.Description = "The memory model.     (--model-<model>";
                switchToAdd.ArgumentRelationList = new ArrayList();
                string[][] switchMap = new string[][]
                {
                    new string[2]{ "Small", "--model-small" },
                    new string[2]{ "Medium", "--model-medium" },
                    new string[2]{ "Large", "--model-large" },
                    new string[2]{ "Huge", "--model-huge" },
                    new string[2]{ "Flat24", "--model-flat24" },
                };
                switchToAdd.SwitchValue = this.ReadSwitchMap(nameof(MemoryModel), switchMap, value);
                switchToAdd.Name = nameof(MemoryModel);
                switchToAdd.Value = value;
                switchToAdd.MultipleValues = true;
                this.ActiveToolSwitches.Add(nameof(MemoryModel), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }
        public bool EnableMPLABCompatibilityMode
        {
            get
            {
                if (this.IsPropertySet(nameof(EnableMPLABCompatibilityMode)))
                    return this.ActiveToolSwitches[nameof(EnableMPLABCompatibilityMode)].BooleanValue;
                return false;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(EnableMPLABCompatibilityMode));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
                switchToAdd.DisplayName = "Enable MPLAB Compatibility Mode";
                switchToAdd.Description = "Enable compatibility mode for MPLAB utilities (MPASM/MPLINK).    (--mplab-comp)";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "--mplab-comp";
                switchToAdd.Name = nameof(EnableMPLABCompatibilityMode);
                switchToAdd.BooleanValue = value;
                this.ActiveToolSwitches.Add(nameof(EnableMPLABCompatibilityMode), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }
        public bool NoCrt
        {
            get
            {
                if (this.IsPropertySet(nameof(NoCrt)))
                    return this.ActiveToolSwitches[nameof(NoCrt)].BooleanValue;
                return false;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(NoCrt));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
                switchToAdd.DisplayName = "No CRT";
                switchToAdd.Description = "Do not link any default run-time initialization module.     (--no-crt)";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "--no-crt";
                switchToAdd.Name = nameof(NoCrt);
                switchToAdd.BooleanValue = value;
                this.ActiveToolSwitches.Add(nameof(NoCrt), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }
        public bool NoExtendedInstructions
        {
            get
            {
                if (this.IsPropertySet(nameof(NoExtendedInstructions)))
                    return this.ActiveToolSwitches[nameof(NoExtendedInstructions)].BooleanValue;
                return false;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(NoExtendedInstructions));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
                switchToAdd.DisplayName = "No Extended Instruction Set";
                switchToAdd.Description = "Forbid use of the extended instruction set (e.g., ADDFSR).     (--no-extended-instructions)";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "--no-extended-instructions";
                switchToAdd.Name = nameof(NoExtendedInstructions);
                switchToAdd.BooleanValue = value;
                this.ActiveToolSwitches.Add(nameof(NoExtendedInstructions), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }
        public bool NoOptimizeGoTo
        {
            get
            {
                if (this.IsPropertySet(nameof(NoOptimizeGoTo)))
                    return this.ActiveToolSwitches[nameof(NoOptimizeGoTo)].BooleanValue;
                return false;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(NoOptimizeGoTo));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
                switchToAdd.DisplayName = "No Optimize GoTo";
                switchToAdd.Description = "Do NOT use (conditional) BRA insetad of GOTO.     (--no-optimize-goto)";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "--no-optimize-goto";
                switchToAdd.Name = nameof(NoOptimizeGoTo);
                switchToAdd.BooleanValue = value;
                this.ActiveToolSwitches.Add(nameof(NoOptimizeGoTo), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }
        public bool NoPackIRam
        {
            get
            {
                if (this.IsPropertySet(nameof(NoPackIRam)))
                    return this.ActiveToolSwitches[nameof(NoPackIRam)].BooleanValue;
                return false;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(NoPackIRam));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
                switchToAdd.DisplayName = "No Pack IRAM";
                switchToAdd.Description = "Deprecated: Tells the linker not to pack variables in internal ram.     (--no-pack-iram)";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "--no-pack-iram";
                switchToAdd.Name = nameof(NoPackIRam);
                switchToAdd.BooleanValue = value;
                this.ActiveToolSwitches.Add(nameof(NoPackIRam), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }
        public bool NoPCodeOptimization
        {
            get
            {
                if (this.IsPropertySet(nameof(NoPCodeOptimization)))
                    return this.ActiveToolSwitches[nameof(NoPCodeOptimization)].BooleanValue;
                return false;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(NoPCodeOptimization));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
                switchToAdd.DisplayName = "Disable pCode optimization";
                switchToAdd.Description = "Disable (slightly faulty) optimization on pCode.     (--no-pcode-opt)";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "--no-pcode-opt";
                switchToAdd.Name = nameof(NoPCodeOptimization);
                switchToAdd.BooleanValue = value;
                this.ActiveToolSwitches.Add(nameof(NoPCodeOptimization), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }
        public bool NoRetWithoutCall
        {
            get
            {
                if (this.IsPropertySet(nameof(NoRetWithoutCall)))
                    return this.ActiveToolSwitches[nameof(NoRetWithoutCall)].BooleanValue;
                return false;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(NoRetWithoutCall));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
                switchToAdd.DisplayName = "No RET without Call";
                switchToAdd.Description = "Do not use ret independent of acall/lcall.     (--no-ret-without-call)";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "--no-ret-without-call";
                switchToAdd.Name = nameof(NoRetWithoutCall);
                switchToAdd.BooleanValue = value;
                this.ActiveToolSwitches.Add(nameof(NoRetWithoutCall), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }
        public bool NoStdCrt
        {
            get
            {
                if (this.IsPropertySet(nameof(NoStdCrt)))
                    return this.ActiveToolSwitches[nameof(NoStdCrt)].BooleanValue;
                return false;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(NoStdCrt));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
                switchToAdd.DisplayName = "No STD Crt";
                switchToAdd.Description = "Do not link default crt0.rel.     (--no-std-crt0)";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "--no-std-crt0";
                switchToAdd.Name = nameof(NoStdCrt);
                switchToAdd.BooleanValue = value;
                this.ActiveToolSwitches.Add(nameof(NoStdCrt), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }
        public bool NoWarnNonFree
        {
            get
            {
                if (this.IsPropertySet(nameof(NoWarnNonFree)))
                    return this.ActiveToolSwitches[nameof(NoWarnNonFree)].BooleanValue;
                return false;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(NoWarnNonFree));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
                switchToAdd.DisplayName = "Suppress --use-non-free Warning";
                switchToAdd.Description = "Suppress warning on absent --use-non-free option.     (--no-warn-non-free)";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "--no-warn-non-free";
                switchToAdd.Name = nameof(NoWarnNonFree);
                switchToAdd.BooleanValue = value;
                this.ActiveToolSwitches.Add(nameof(NoWarnNonFree), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }
        public bool NoDefaultLibs
        {
            get
            {
                if (this.IsPropertySet(nameof(NoDefaultLibs)))
                    return this.ActiveToolSwitches[nameof(NoDefaultLibs)].BooleanValue;
                return false;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(NoDefaultLibs));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
                switchToAdd.DisplayName = "No Default Libs";
                switchToAdd.Description = "Do not link default libraries when linking.     (--nodefaultlibs)";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "--nodefaultlibs";
                switchToAdd.Name = nameof(NoDefaultLibs);
                switchToAdd.BooleanValue = value;
                this.ActiveToolSwitches.Add(nameof(NoDefaultLibs), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }
        public int BankSelectOptimizationLevel
        {
            get
            {
                if (this.IsPropertySet(nameof(BankSelectOptimizationLevel)))
                    return this.ActiveToolSwitches[nameof(BankSelectOptimizationLevel)].Number;
                return 0;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(BankSelectOptimizationLevel));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Integer);
                switchToAdd.DisplayName = "BANKSEL Optimization Level";
                switchToAdd.Description = "Set BANKSEL optimization level (default=0 no).     (--obanksel=<level>)";
                switchToAdd.IsValid = this.ValidateInteger(nameof(BankSelectOptimizationLevel), int.MinValue, int.MaxValue, value);
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "--obanksel=";
                switchToAdd.Name = nameof(BankSelectOptimizationLevel);
                switchToAdd.Number = value;
                this.ActiveToolSwitches.Add(nameof(BankSelectOptimizationLevel), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }
        public bool UseOldRegisterAllocator
        {
            get
            {
                if (this.IsPropertySet(nameof(UseOldRegisterAllocator)))
                    return this.ActiveToolSwitches[nameof(UseOldRegisterAllocator)].BooleanValue;
                return false;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(UseOldRegisterAllocator));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
                switchToAdd.DisplayName = "Use Old Register Allocator";
                switchToAdd.Description = "Use old regisetr allocator.     (--oldralloc)";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "--oldralloc";
                switchToAdd.Name = nameof(UseOldRegisterAllocator);
                switchToAdd.BooleanValue = value;
                this.ActiveToolSwitches.Add(nameof(UseOldRegisterAllocator), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }
        public bool OptimizeCmp
        {
            get
            {
                if (this.IsPropertySet(nameof(OptimizeCmp)))
                    return this.ActiveToolSwitches[nameof(OptimizeCmp)].BooleanValue;
                return false;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(OptimizeCmp));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
                switchToAdd.DisplayName = "Optimize Compares";
                switchToAdd.Description = "Try to optimize some compares.     (--optimize-cmp)";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "--optimize-cmp";
                switchToAdd.Name = nameof(OptimizeCmp);
                switchToAdd.BooleanValue = value;
                this.ActiveToolSwitches.Add(nameof(OptimizeCmp), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }
        public bool OptimizeDataFlow
        {
            get
            {
                if (this.IsPropertySet(nameof(OptimizeDataFlow)))
                    return this.ActiveToolSwitches[nameof(OptimizeDataFlow)].BooleanValue;
                return false;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(OptimizeDataFlow));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
                switchToAdd.DisplayName = "Analyze Data Flow";
                switchToAdd.Description = "Throughly analyze data flow (memory and time intensive!).     (--optimize-df)";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "--optimize-df";
                switchToAdd.Name = nameof(OptimizeDataFlow);
                switchToAdd.BooleanValue = value;
                this.ActiveToolSwitches.Add(nameof(OptimizeDataFlow), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }
        public bool PackIRam
        {
            get
            {
                if (this.IsPropertySet(nameof(PackIRam)))
                    return this.ActiveToolSwitches[nameof(PackIRam)].BooleanValue;
                return false;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(PackIRam));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
                switchToAdd.DisplayName = "Pack IRAM";
                switchToAdd.Description = "Tells the linker to pack variables in internal ram (default).     (--pack-iram)";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "--pack-iram";
                switchToAdd.Name = nameof(PackIRam);
                switchToAdd.BooleanValue = value;
                this.ActiveToolSwitches.Add(nameof(PackIRam), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }
        public bool ParamsInBank1
        {
            get
            {
                if (this.IsPropertySet(nameof(ParamsInBank1)))
                    return this.ActiveToolSwitches[nameof(ParamsInBank1)].BooleanValue;
                return false;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(ParamsInBank1));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
                switchToAdd.DisplayName = "Bank1 Param Passing";
                switchToAdd.Description = "Use Bank1 for parameter passing.     (--parms-in-bank1)";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "--parms-in-bank1";
                switchToAdd.Name = nameof(ParamsInBank1);
                switchToAdd.BooleanValue = value;
                this.ActiveToolSwitches.Add(nameof(ParamsInBank1), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }
        public bool PCodeVerbose
        {
            get
            {
                if (this.IsPropertySet(nameof(PCodeVerbose)))
                    return this.ActiveToolSwitches[nameof(PCodeVerbose)].BooleanValue;
                return false;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(PCodeVerbose));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
                switchToAdd.DisplayName = "Dump PCODE Info";
                switchToAdd.Description = "Dump PCODE related info.     (--pcode-verbose)";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "--pcode-verbose";
                switchToAdd.Name = nameof(PCodeVerbose);
                switchToAdd.BooleanValue = value;
                this.ActiveToolSwitches.Add(nameof(PCodeVerbose), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }
        public bool NoBankSelect
        {
            get
            {
                if (this.IsPropertySet(nameof(NoBankSelect)))
                    return this.ActiveToolSwitches[nameof(NoBankSelect)].BooleanValue;
                return false;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(NoBankSelect));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
                switchToAdd.DisplayName = "No BANKSEL Directives";
                switchToAdd.Description = "Do not generate BANKSEL assembler directives.    (--pno-banksel)";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "--pno-banksel";
                switchToAdd.Name = nameof(NoBankSelect);
                switchToAdd.BooleanValue = value;
                this.ActiveToolSwitches.Add(nameof(NoBankSelect), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }
        public string IOPortMode
        {
            get
            {
                if (this.IsPropertySet(nameof(IOPortMode)))
                    return this.ActiveToolSwitches[nameof(IOPortMode)].Value;
                return null;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(IOPortMode));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.String);
                switchToAdd.DisplayName = "I/O Port Mode";
                switchToAdd.Description = "Determine Port I/O mode (z80/z180).     (--portmode=<mode>)";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "--portmode=";
                switchToAdd.Name = nameof(IOPortMode);
                switchToAdd.Value = value;
                this.ActiveToolSwitches.Add(nameof(IOPortMode), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }
        public string ReplaceUDataWith
        {
            get
            {
                if (this.IsPropertySet(nameof(ReplaceUDataWith)))
                    return this.ActiveToolSwitches[nameof(ReplaceUDataWith)].Value;
                return null;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(ReplaceUDataWith));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.String);
                switchToAdd.DisplayName = "UData Replacement";
                switchToAdd.Description = "Place udata variables at another section: udata_acs, udata_ovr, udata_shr.     (--preplace-udata-with=<udata>)";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "--preplace-udata-with=";
                switchToAdd.Name = nameof(ReplaceUDataWith);
                switchToAdd.Value = value;
                this.ActiveToolSwitches.Add(nameof(ReplaceUDataWith), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }
        public bool ProtectSPUpdate
        {
            get
            {
                if (this.IsPropertySet(nameof(ProtectSPUpdate)))
                    return this.ActiveToolSwitches[nameof(ProtectSPUpdate)].BooleanValue;
                return false;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(ProtectSPUpdate));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
                switchToAdd.DisplayName = "Disable Int during ESP:SP Updates";
                switchToAdd.Description = "Disable interrupts during ESP:SP updates.     (--protect-sp-update)";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "--protect-sp-update";
                switchToAdd.Name = nameof(ProtectSPUpdate);
                switchToAdd.BooleanValue = value;
                this.ActiveToolSwitches.Add(nameof(ProtectSPUpdate), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }
        public string StackModel
        {
            get
            {
                if (this.IsPropertySet(nameof(StackModel)))
                    return this.ActiveToolSwitches[nameof(StackModel)].Value;
                return null;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(StackModel));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.String);
                switchToAdd.DisplayName = "Stack Model";
                switchToAdd.Description = "Use stack model 'small' (default) or 'large'.     (--pstack-model=<model>)";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "--pstack-model=";
                switchToAdd.Name = nameof(StackModel);
                switchToAdd.Value = value;
                this.ActiveToolSwitches.Add(nameof(StackModel), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }
        public bool ReserveRegsIY
        {
            get
            {
                if (this.IsPropertySet(nameof(ReserveRegsIY)))
                    return this.ActiveToolSwitches[nameof(ReserveRegsIY)].BooleanValue;
                return false;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(ReserveRegsIY));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
                switchToAdd.DisplayName = "Do Not Use IY";
                switchToAdd.Description = "Do not use IY (incompatible with --fomit-frame-pointer)    (--reserve-regs-iv)";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "--reserve-regs-iv";
                switchToAdd.Name = nameof(ReserveRegsIY);
                switchToAdd.BooleanValue = value;
                this.ActiveToolSwitches.Add(nameof(ReserveRegsIY), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }
        public string StackBits
        {
            get
            {
                if (this.IsPropertySet(nameof(StackBits)))
                    return this.ActiveToolSwitches[nameof(StackBits)].Value;
                return null;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(StackBits));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.String);
                switchToAdd.DisplayName = "Stack Bits";
                switchToAdd.Description = "Number of bits used by the stack.";
                switchToAdd.ArgumentRelationList = new ArrayList();
                string[][] switchMap = new string[][]
                {
                    new string[2]{ "10 Bit", "--stack-10bit" },
                    new string[2]{ "8 Bit", "--stack-8bit" },
                };
                switchToAdd.SwitchValue = this.ReadSwitchMap(nameof(StackBits), switchMap, value);
                switchToAdd.Name = nameof(StackBits);
                switchToAdd.Value = value;
                switchToAdd.MultipleValues = true;
                this.ActiveToolSwitches.Add(nameof(StackBits), switchToAdd);
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
                switchToAdd.DisplayName = "Stack Size";
                switchToAdd.Description = "Tells the linker to allocate this space for stack.     (--stack-size <nnnn>)";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "--stack-size ";
                switchToAdd.Name = nameof(StackSize);
                switchToAdd.Value = value;
                this.ActiveToolSwitches.Add(nameof(StackSize), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }
        public int TINILibraryId
        {
            get
            {
                if (this.IsPropertySet(nameof(TINILibraryId)))
                    return this.ActiveToolSwitches[nameof(TINILibraryId)].Number;
                return 0;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(TINILibraryId));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Integer);
                switchToAdd.DisplayName = "LibraryID";
                switchToAdd.Description = "LibraryID used in -mTININative.     (--tini-libid <nnnn>)";
                switchToAdd.IsValid = this.ValidateInteger(nameof(TINILibraryId), int.MinValue, int.MaxValue, value);
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "--tini-libid ";
                switchToAdd.Name = nameof(TINILibraryId);
                switchToAdd.Number = value;
                this.ActiveToolSwitches.Add(nameof(TINILibraryId), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }
        public bool UseAccelerator
        {
            get
            {
                if (this.IsPropertySet(nameof(UseAccelerator)))
                    return this.ActiveToolSwitches[nameof(UseAccelerator)].BooleanValue;
                return false;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(UseAccelerator));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
                switchToAdd.DisplayName = "Generate Code for Arithmetic Accelerator";
                switchToAdd.Description = "Genreate Code for the ds390/ds400 arithmetic accelerator.     (--use-accelerator)";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "--use-accelerator";
                switchToAdd.Name = nameof(UseAccelerator);
                switchToAdd.BooleanValue = value;
                this.ActiveToolSwitches.Add(nameof(UseAccelerator), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }
        public string UseCRT
        {
            get
            {
                if (this.IsPropertySet(nameof(UseCRT)))
                    return this.ActiveToolSwitches[nameof(UseCRT)].Value;
                return null;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(UseCRT));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.String);
                switchToAdd.DisplayName = "Use CRT";
                switchToAdd.Description = "Use <crt-0> run-time initialization module.     (--use-crt=<crt-0>)";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "--use-crt=";
                switchToAdd.Name = nameof(UseCRT);
                switchToAdd.Value = value;
                this.ActiveToolSwitches.Add(nameof(UseCRT), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }
        public bool ExtendedInstructionSet
        {
            get
            {
                if (this.IsPropertySet(nameof(ExtendedInstructionSet)))
                    return this.ActiveToolSwitches[nameof(ExtendedInstructionSet)].BooleanValue;
                return false;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(ExtendedInstructionSet));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Boolean);
                switchToAdd.DisplayName = "Enable Extended Instruction Set";
                switchToAdd.Description = "Enable exteneded instruction set/Literal offset addressing mode.     (-y)";
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.SwitchValue = "-y";
                switchToAdd.Name = nameof(ExtendedInstructionSet);
                switchToAdd.BooleanValue = value;
                this.ActiveToolSwitches.Add(nameof(ExtendedInstructionSet), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }

        public int CodeBank
        {
            get
            {
                if (this.IsPropertySet(nameof(CodeBank)))
                    return this.ActiveToolSwitches[nameof(CodeBank)].Number;
                return 0;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(CodeBank));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Integer);
                switchToAdd.DisplayName = "Code Bank";
                switchToAdd.Description = "Use Code Bank <num>    (-bo<num>)";
                //switchToAdd.Parents.AddLast("");
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.IsValid = this.ValidateInteger(nameof(CodeBank), int.MinValue, int.MaxValue, value);
                switchToAdd.SwitchValue = "-bo";
                switchToAdd.Name = nameof(CodeBank);
                switchToAdd.Number = value;
                this.ActiveToolSwitches.Add(nameof(CodeBank), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }
        public int DataBank
        {
            get
            {
                if (this.IsPropertySet(nameof(DataBank)))
                    return this.ActiveToolSwitches[nameof(DataBank)].Number;
                return 0;
            }
            set
            {
                this.ActiveToolSwitches.Remove(nameof(DataBank));
                ToolSwitch switchToAdd = new ToolSwitch(ToolSwitchType.Integer);
                switchToAdd.DisplayName = "Data Bank";
                switchToAdd.Description = "Use Data Bank <num>    (-ba<num>)";
                //switchToAdd.Parents.AddLast("");
                switchToAdd.ArgumentRelationList = new ArrayList();
                switchToAdd.IsValid = this.ValidateInteger(nameof(DataBank), int.MinValue, int.MaxValue, value);
                switchToAdd.SwitchValue = "-ba";
                switchToAdd.Name = nameof(DataBank);
                switchToAdd.Number = value;
                this.ActiveToolSwitches.Add(nameof(DataBank), switchToAdd);
                this.AddActiveSwitchToolValue(switchToAdd);
            }
        }

        #endregion

        public SDCCCompile()
        {
            this.EnableErrorListRegex = true;
            
            this.errorListRegexList = new List<Regex>
            {
                new Regex(@"(?'FILENAME'.+):(?'LINE'\d+): (syntax )?(?'CATEGORY'Error|error|warning|info)( (?'CODE'\d+))?:(?'TEXT'.+?)( %3B column (?'COLUMN'\d+))?$"),
            };

            SwitchList = new ArrayList();
            SwitchList.Add(nameof(AlwaysAppend));
            SwitchList.Add(nameof(PortType));
            SwitchList.Add(nameof(ProcessorType));

            SwitchList.Add(nameof(Verbose));
            SwitchList.Add(nameof(OutputMacros));
            SwitchList.Add(nameof(PreprocessorDefinitions));
            SwitchList.Add(nameof(AdditionalIncludeDirectories));
            SwitchList.Add(nameof(AssertAnswer));
            SwitchList.Add(nameof(PreprocessorUndefines));
            SwitchList.Add(nameof(DependencyRule));
            SwitchList.Add(nameof(PreprocessorOptions));
            SwitchList.Add(nameof(AssemblerOptions));
            SwitchList.Add(nameof(LinkerOptions));
            SwitchList.Add(nameof(PrintSearchDirs));
            SwitchList.Add(nameof(NoStdLib));
            SwitchList.Add(nameof(NoStdInc));
            SwitchList.Add(nameof(LessPedantic));
            SwitchList.Add(nameof(DisableWarning));
            SwitchList.Add(nameof(WarnErrors));
            SwitchList.Add(nameof(Debug));
            SwitchList.Add(nameof(Cyclomatic));
            SwitchList.Add(nameof(CStandard));
            SwitchList.Add(nameof(DollarsInId));
            SwitchList.Add(nameof(SignedChar));
            SwitchList.Add(nameof(UseNonFree));

            SwitchList.Add(nameof(StackAuto));
            SwitchList.Add(nameof(ExternalStack));
            SwitchList.Add(nameof(IntLongReent));
            SwitchList.Add(nameof(FloatReent));
            SwitchList.Add(nameof(XRamMovec));
            SwitchList.Add(nameof(CalleeSaves));
            SwitchList.Add(nameof(Profile));
            SwitchList.Add(nameof(OmitFramePointer));
            SwitchList.Add(nameof(AllCalleeSaves));
            SwitchList.Add(nameof(StackProbe));
            SwitchList.Add(nameof(NoXinitOpt));
            SwitchList.Add(nameof(NoCCodeInAsm));
            SwitchList.Add(nameof(NoPeepComments));
            SwitchList.Add(nameof(CodeSeg));
            SwitchList.Add(nameof(ConstSeg));
            SwitchList.Add(nameof(DataSeg));

            SwitchList.Add(nameof(NoOverlay));
            SwitchList.Add(nameof(NoGcse));
            SwitchList.Add(nameof(NoLabelOpt));
            SwitchList.Add(nameof(NoInvariant));
            SwitchList.Add(nameof(NoInduction));
            SwitchList.Add(nameof(NoLoopReverse));
            SwitchList.Add(nameof(NoPeep));
            SwitchList.Add(nameof(NoRegParams));
            SwitchList.Add(nameof(PeepAsm));
            SwitchList.Add(nameof(PeepReturn));
            SwitchList.Add(nameof(NoPeepReturn));
            SwitchList.Add(nameof(PeepFile));
            SwitchList.Add(nameof(OptimizeCode));
            SwitchList.Add(nameof(MaxAllocsPerNode));
            SwitchList.Add(nameof(NoLospre));
            SwitchList.Add(nameof(AllowUnsafeRead));
            SwitchList.Add(nameof(NoStdLibCalls));

            SwitchList.Add(nameof(DumpAst));
            SwitchList.Add(nameof(DumpICode));
            SwitchList.Add(nameof(DumpGraphs));
            SwitchList.Add(nameof(ICodeInAsm));
            SwitchList.Add(nameof(VerboseAsm));

            SwitchList.Add(nameof(IncludeLibraries));
            SwitchList.Add(nameof(AdditionalLibraryPath));
            SwitchList.Add(nameof(OutputFormat));
            SwitchList.Add(nameof(XRamLocation));
            SwitchList.Add(nameof(XRamSize));
            SwitchList.Add(nameof(IRamSize));
            SwitchList.Add(nameof(XStackLocation));
            SwitchList.Add(nameof(CodeLocation));
            SwitchList.Add(nameof(CodeSize));
            SwitchList.Add(nameof(StackLocation));
            SwitchList.Add(nameof(DataLocation));
            SwitchList.Add(nameof(IDataLocation));
            SwitchList.Add(nameof(NoOptSdccInAsm));

            SwitchList.Add(nameof(KeepComments));
            SwitchList.Add(nameof(PreprocessorOutput));
            SwitchList.Add(nameof(PedanticParseNum));

            SwitchList.Add(nameof(UseACallAimp));
            SwitchList.Add(nameof(UseAssembler));
            SwitchList.Add(nameof(CalleeSavesBC));
            SwitchList.Add(nameof(DumpCallTree));
            SwitchList.Add(nameof(DebugRalloc));
            SwitchList.Add(nameof(DebugExtra));
            SwitchList.Add(nameof(EnablePeeps));
            SwitchList.Add(nameof(EmitExterns));
            SwitchList.Add(nameof(NoEmitFramePointer));
            SwitchList.Add(nameof(GStack));
            SwitchList.Add(nameof(IVTAddress));
            SwitchList.Add(nameof(AltLinker));
            SwitchList.Add(nameof(MemoryModel));
            SwitchList.Add(nameof(EnableMPLABCompatibilityMode));
            SwitchList.Add(nameof(NoCrt));
            SwitchList.Add(nameof(NoExtendedInstructions));
            SwitchList.Add(nameof(NoOptimizeGoTo));
            SwitchList.Add(nameof(NoPackIRam));
            SwitchList.Add(nameof(NoPCodeOptimization));
            SwitchList.Add(nameof(NoRetWithoutCall));
            SwitchList.Add(nameof(NoStdCrt));
            SwitchList.Add(nameof(NoWarnNonFree));
            SwitchList.Add(nameof(NoDefaultLibs));
            SwitchList.Add(nameof(BankSelectOptimizationLevel));
            SwitchList.Add(nameof(UseOldRegisterAllocator));
            SwitchList.Add(nameof(OptimizeCmp));
            SwitchList.Add(nameof(OptimizeDataFlow));
            SwitchList.Add(nameof(PackIRam));
            SwitchList.Add(nameof(ParamsInBank1));
            SwitchList.Add(nameof(PCodeVerbose));
            SwitchList.Add(nameof(NoBankSelect));
            SwitchList.Add(nameof(IOPortMode));
            SwitchList.Add(nameof(ReplaceUDataWith));
            SwitchList.Add(nameof(ProtectSPUpdate));
            SwitchList.Add(nameof(StackModel));
            SwitchList.Add(nameof(ReserveRegsIY));
            SwitchList.Add(nameof(StackBits));
            SwitchList.Add(nameof(StackSize));
            SwitchList.Add(nameof(TINILibraryId));
            SwitchList.Add(nameof(UseAccelerator));
            SwitchList.Add(nameof(UseCRT));
            SwitchList.Add(nameof(ExtendedInstructionSet));
            SwitchList.Add(nameof(CodeBank));
            SwitchList.Add(nameof(DataBank));

            SwitchList.Add(nameof(AdditionalOptions));

            SwitchList.Add(nameof(OutputFile));
            SwitchList.Add(nameof(SourceInput));
        }

        protected override string ToolName
        {
            get
            {
                return "SDCC.exe";
            }
        }
        
    }
}
