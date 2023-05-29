﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SDCCTask {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Strings {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Strings() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("SDCCTask.Strings", typeof(Strings).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Element &lt;{0}&gt; has an invalid value of &quot;{1}&quot;..
        /// </summary>
        internal static string ArgumentOutOfRange {
            get {
                return ResourceManager.GetString("ArgumentOutOfRange", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Switch &lt;{0}&gt; requires an argument..
        /// </summary>
        internal static string ArgumentRequired {
            get {
                return ResourceManager.GetString("ArgumentRequired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Forcing recompile of all source files due to missing PDB &quot;{0}&quot;..
        /// </summary>
        internal static string CL_RebuildingNoPDB {
            get {
                return ResourceManager.GetString("CL.RebuildingNoPDB", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Expected &quot;{0}&quot; but got &quot;{1}&quot;..
        /// </summary>
        internal static string CommandLineDiffer {
            get {
                return ResourceManager.GetString("CommandLineDiffer", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to MSB8017: A circular dependency has been detected while executing custom build commands for item &quot;{0}&quot;. This may cause incremental build to work incorrectly..
        /// </summary>
        internal static string CustomBuild_CircularDepedencyDetected {
            get {
                return ResourceManager.GetString("CustomBuild.CircularDepedencyDetected", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to MSB8018: No outputs specified for item &quot;{0}&quot;. Its custom build command will be skipped..
        /// </summary>
        internal static string CustomBuild_NoOutputs {
            get {
                return ResourceManager.GetString("CustomBuild.NoOutputs", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Required file &quot;{0}&quot; is missing..
        /// </summary>
        internal static string Error_MissingFile {
            get {
                return ResourceManager.GetString("Error.MissingFile", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unable to open file {0} because {1}.
        /// </summary>
        internal static string FileNotOpen {
            get {
                return ResourceManager.GetString("FileNotOpen", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to MSB3098: &quot;{1}&quot; task received an invalid value for the &quot;{0}&quot; parameter..
        /// </summary>
        internal static string General_InvalidValue {
            get {
                return ResourceManager.GetString("General.InvalidValue", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &quot;The build of &apos;{0}&apos; depends on &apos;{1}&apos; which is produced by the build of &apos;{2}&apos;. The items cannot be built in parallel.&quot;.
        /// </summary>
        internal static string GetOutOfDateItems_ItemDependsOnAnotherItemOutput {
            get {
                return ResourceManager.GetString("GetOutOfDateItems.ItemDependsOnAnotherItemOutput", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Source file &apos;{0}&apos; is not up-to-date: list of dependencies has changed since the last build..
        /// </summary>
        internal static string GetOutOfDateItems_RebuildingSourceDependenciesChanged {
            get {
                return ResourceManager.GetString("GetOutOfDateItems.RebuildingSourceDependenciesChanged", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Source file &apos;{0}&apos; is not up-to-date: list of outputs has changed since the last build..
        /// </summary>
        internal static string GetOutOfDateItems_RebuildingSourceOutputsChanged {
            get {
                return ResourceManager.GetString("GetOutOfDateItems.RebuildingSourceOutputsChanged", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Element &lt;{0}&gt; has invalid type of &quot;{1}&quot;..
        /// </summary>
        internal static string ImproperType {
            get {
                return ResourceManager.GetString("ImproperType", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Internal Error has occurred. Most likely a bug..
        /// </summary>
        internal static string InternalError {
            get {
                return ResourceManager.GetString("InternalError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Expecting &quot;{0}&quot;..
        /// </summary>
        internal static string InvalidType {
            get {
                return ResourceManager.GetString("InvalidType", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to MSB3511: &quot;{0}&quot; is an invalid value for the &quot;Importance&quot; parameter. Valid values are: High, Normal and Low..
        /// </summary>
        internal static string Message_InvalidImportance {
            get {
                return ResourceManager.GetString("Message.InvalidImportance", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to To improve incremental build performance for managed components, please make sure that the &apos;VC++ Directories-&gt;Reference Directories&apos; points to all the paths which contain the referenced managed assemblies..
        /// </summary>
        internal static string MetaGenInfoReferenceDirectories {
            get {
                return ResourceManager.GetString("MetaGenInfoReferenceDirectories", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Element &lt;{0}&gt; does not contain the required attribute &quot;{1}&quot;..
        /// </summary>
        internal static string MissingAttribute {
            get {
                return ResourceManager.GetString("MissingAttribute", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Missing required argument &lt;{0}&gt; for property &lt;{1}&gt;..
        /// </summary>
        internal static string MissingRequiredArgument {
            get {
                return ResourceManager.GetString("MissingRequiredArgument", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Missing required argument &lt;{0}&gt; for property &lt;{1}&gt; which is set to {2}..
        /// </summary>
        internal static string MissingRequiredArgumentWithValue {
            get {
                return ResourceManager.GetString("MissingRequiredArgumentWithValue", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Xml file must start with the root element &lt;{0}&gt;..
        /// </summary>
        internal static string MissingRootElement {
            get {
                return ResourceManager.GetString("MissingRootElement", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unable to move file &quot;{0}&quot; to &quot;{1}&quot;. {2}.
        /// </summary>
        internal static string Move_Error {
            get {
                return ResourceManager.GetString("Move.Error", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Moving file from &quot;{0}&quot; to &quot;{1}&quot;..
        /// </summary>
        internal static string Move_FileComment {
            get {
                return ResourceManager.GetString("Move.FileComment", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} is building {1} file(s)..
        /// </summary>
        internal static string MultiTool_AddDone {
            get {
                return ResourceManager.GetString("MultiTool.AddDone", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Adding source {0}..
        /// </summary>
        internal static string MultiTool_AddSource {
            get {
                return ResourceManager.GetString("MultiTool.AddSource", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Adding source {0} with dependency on {1}..
        /// </summary>
        internal static string MultiTool_AddSourceWithDep {
            get {
                return ResourceManager.GetString("MultiTool.AddSourceWithDep", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Building with {0}..
        /// </summary>
        internal static string MultiTool_BuildingWith {
            get {
                return ResourceManager.GetString("MultiTool.BuildingWith", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to All Sources must have the same TrackerLogDirectory ({0},{1})..
        /// </summary>
        internal static string MultiTool_SameTrackerLogDirectory {
            get {
                return ResourceManager.GetString("MultiTool.SameTrackerLogDirectory", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Source {0} doesn&apos;t match previous command line..
        /// </summary>
        internal static string MultiTool_SourceNotMatchCommand {
            get {
                return ResourceManager.GetString("MultiTool.SourceNotMatchCommand", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Source {0} is out of date..
        /// </summary>
        internal static string MultiTool_SourceOutOfDate {
            get {
                return ResourceManager.GetString("MultiTool.SourceOutOfDate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} task found..
        /// </summary>
        internal static string MultiTool_TaskFound {
            get {
                return ResourceManager.GetString("MultiTool.TaskFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Tracking command:.
        /// </summary>
        internal static string Native_TrackingCommandMessage {
            get {
                return ResourceManager.GetString("Native_TrackingCommandMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to MSB5003: Failed to create a temporary file. Temporary files folder is full or its path is incorrect. {0}.
        /// </summary>
        internal static string Shared_FailedCreatingTempFile {
            get {
                return ResourceManager.GetString("Shared.FailedCreatingTempFile", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Parameter &quot;{0}&quot; cannot be null..
        /// </summary>
        internal static string Shared_ParameterCannotBeNull {
            get {
                return ResourceManager.GetString("Shared.ParameterCannotBeNull", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Parameter &quot;{0}&quot; cannot have zero length..
        /// </summary>
        internal static string Shared_ParameterCannotHaveZeroLength {
            get {
                return ResourceManager.GetString("Shared.ParameterCannotHaveZeroLength", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Parameters &quot;{0}&quot; and &quot;{1}&quot; must have the same number of elements..
        /// </summary>
        internal static string Shared_ParametersMustHaveTheSameLength {
            get {
                return ResourceManager.GetString("Shared.ParametersMustHaveTheSameLength", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to MSB8016: Can not turn on Unicode output for &quot;{0}&quot;. Some Unicode characters will be improperly displayed..
        /// </summary>
        internal static string TrackedVCToolTask_CreateUnicodeOutputPipeFailed {
            get {
                return ResourceManager.GetString("TrackedVCToolTask.CreateUnicodeOutputPipeFailed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to All source files are not up-to-date:  command line has changed since the last build..
        /// </summary>
        internal static string TrackedVCToolTask_RebuildingAllSourcesCommandLineChanged {
            get {
                return ResourceManager.GetString("TrackedVCToolTask.RebuildingAllSourcesCommandLineChanged", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to MSB8014: All source files are not up-to-date:  forcing a rebuild of all sources due to an error with the tracking logs. {0}.
        /// </summary>
        internal static string TrackedVCToolTask_RebuildingDueToInvalidTLog {
            get {
                return ResourceManager.GetString("TrackedVCToolTask.RebuildingDueToInvalidTLog", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to MSB8015: All source files are not up-to-date:  forcing a rebuild of all source files due to the contents of &apos;{0}&apos; being invalid..
        /// </summary>
        internal static string TrackedVCToolTask_RebuildingDueToInvalidTLogContents {
            get {
                return ResourceManager.GetString("TrackedVCToolTask.RebuildingDueToInvalidTLogContents", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to All source files are not up-to-date: missing command TLog &quot;{0}&quot;..
        /// </summary>
        internal static string TrackedVCToolTask_RebuildingNoCommandTLog {
            get {
                return ResourceManager.GetString("TrackedVCToolTask.RebuildingNoCommandTLog", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Source file &quot;{0}&quot; is not up-to-date: command line has changed since the last build..
        /// </summary>
        internal static string TrackedVCToolTask_RebuildingSourceCommandLineChanged {
            get {
                return ResourceManager.GetString("TrackedVCToolTask.RebuildingSourceCommandLineChanged", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to MSB8001: &quot;{0}&quot; is an invalid value for the &quot;Code&quot; parameter..
        /// </summary>
        internal static string VCMessage_InvalidCode {
            get {
                return ResourceManager.GetString("VCMessage.InvalidCode", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to MSB8000: &quot;{0}&quot; is an invalid value for the &quot;Type&quot; parameter. Valid values are: Warning and Error..
        /// </summary>
        internal static string VCMessage_InvalidType {
            get {
                return ResourceManager.GetString("VCMessage.InvalidType", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to MSB8002: Specified platform toolset ({0}) is not compatible with the targeted .NET Framework version ({1}). Please set TargetFrameworkVersion to one of the supported values (&apos;v2.0&apos;, &apos;v3.0&apos;, &apos;v3.5&apos;)..
        /// </summary>
        internal static string VCMessage_MSB8002 {
            get {
                return ResourceManager.GetString("VCMessage.MSB8002", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to MSB8003: Could not find {0} variable from the registry.  TargetFrameworkVersion or PlatformToolset may be set to an invalid version number..
        /// </summary>
        internal static string VCMessage_MSB8003 {
            get {
                return ResourceManager.GetString("VCMessage.MSB8003", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to MSB8004: {0} Directory does not end with a trailing slash.  This build instance will add the slash as it is required to allow proper evaluation of the {1} Directory..
        /// </summary>
        internal static string VCMessage_MSB8004 {
            get {
                return ResourceManager.GetString("VCMessage.MSB8004", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to MSB8005: The property &apos;{0}&apos; doesn&apos;t exist.  Skipping....
        /// </summary>
        internal static string VCMessage_MSB8005 {
            get {
                return ResourceManager.GetString("VCMessage.MSB8005", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to MSB8006: The Platform for project &apos;{0}&apos; is invalid.  Platform=&apos;{1}&apos;. This error may also appear if some other project is trying to follow a project-to-project reference to this project, this project has been unloaded or is not included in the solution, and the referencing project does not build using the same or an equivalent Platform..
        /// </summary>
        internal static string VCMessage_MSB8006 {
            get {
                return ResourceManager.GetString("VCMessage.MSB8006", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to MSB8007: The Platform for project &apos;{0}&apos; is invalid.  Platform=&apos;{1}&apos;. You may be seeing this message because you are trying to build a project without a solution file, and have specified a non-default Platform that doesn&apos;t exist for this project..
        /// </summary>
        internal static string VCMessage_MSB8007 {
            get {
                return ResourceManager.GetString("VCMessage.MSB8007", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to MSB8008: Specified platform toolset ({0}) is not installed or invalid. Please make sure that a supported PlatformToolset value is selected..
        /// </summary>
        internal static string VCMessage_MSB8008 {
            get {
                return ResourceManager.GetString("VCMessage.MSB8008", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to MSB8009: .NET Framework 2.0/3.0/3.5 target the v90 platform toolset. Please make sure that Visual Studio 2008 is installed on the machine..
        /// </summary>
        internal static string VCMessage_MSB8009 {
            get {
                return ResourceManager.GetString("VCMessage.MSB8009", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to MSB8010: Specified platform toolset (v90) requires Visual Studio 2008. Please make sure that Visual Studio 2008 is installed on the machine..
        /// </summary>
        internal static string VCMessage_MSB8010 {
            get {
                return ResourceManager.GetString("VCMessage.MSB8010", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to MSB8011: Failed to register output. Please try enabling Per-user Redirection or register the component from a command prompt with elevated permissions..
        /// </summary>
        internal static string VCMessage_MSB8011 {
            get {
                return ResourceManager.GetString("VCMessage.MSB8011", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to MSB8012: {0}({1}) does not match the {2}&apos;s OutputFile property value ({3}). This may cause your project to build incorrectly. To correct this, please make sure that $(OutDir), $(TargetName) and $(TargetExt) property values match the value specified in %({4}.OutputFile)..
        /// </summary>
        internal static string VCMessage_MSB8012 {
            get {
                return ResourceManager.GetString("VCMessage.MSB8012", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to MSB8013: This project doesn&apos;t contain the Configuration and Platform combination of {0}..
        /// </summary>
        internal static string VCMessage_MSB8013 {
            get {
                return ResourceManager.GetString("VCMessage.MSB8013", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to MSB8016: This project does not support the current Configuration Type ({0})..
        /// </summary>
        internal static string VCMessage_MSB8016 {
            get {
                return ResourceManager.GetString("VCMessage.MSB8016", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to MSB8019: This build is consuming a component &quot;{0}&quot; that is not packaged because the component is not coming from a Windows Store app project &quot;{1}&quot;..
        /// </summary>
        internal static string VCMessage_MSB8019 {
            get {
                return ResourceManager.GetString("VCMessage.MSB8019", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to MSB8020: The build tools for {0} (Platform Toolset = &apos;{1}&apos;) cannot be found. To build using the {1} build tools, please install {0} build tools.  Alternatively, you may upgrade to the current Visual Studio tools by selecting the Project menu or right-click the solution, and then selecting &quot;Retarget solution&quot;..
        /// </summary>
        internal static string VCMessage_MSB8020 {
            get {
                return ResourceManager.GetString("VCMessage.MSB8020", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to MSB8021: The value &apos;{0}&apos; of the variable &apos;{1}&apos; is incompatible with the value &apos;{2}&apos; of the variable &apos;{3}&apos;..
        /// </summary>
        internal static string VCMessage_MSB8021 {
            get {
                return ResourceManager.GetString("VCMessage.MSB8021", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to MSB8022: Building Desktop applications for the {0} platform is not supported..
        /// </summary>
        internal static string VCMessage_MSB8022 {
            get {
                return ResourceManager.GetString("VCMessage.MSB8022", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to MSB8023: Execution path ({0}) could not be found..
        /// </summary>
        internal static string VCMessage_MSB8023 {
            get {
                return ResourceManager.GetString("VCMessage.MSB8023", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to MSB8024: Using static version of the C++ runtime library is not supported..
        /// </summary>
        internal static string VCMessage_MSB8024 {
            get {
                return ResourceManager.GetString("VCMessage.MSB8024", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to MSB8025: Using legacy manifest embedding because of {0} on Manifest Tool is set..
        /// </summary>
        internal static string VCMessage_MSB8025 {
            get {
                return ResourceManager.GetString("VCMessage.MSB8025", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to MSB8026: Static analysis is not supported with the current platform toolset..
        /// </summary>
        internal static string VCMessage_MSB8026 {
            get {
                return ResourceManager.GetString("VCMessage.MSB8026", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to MSB8027: Two or more files with the name of {0} will produce outputs to the same location. This can lead to an incorrect build result.  The files involved are {1}..
        /// </summary>
        internal static string VCMessage_MSB8027 {
            get {
                return ResourceManager.GetString("VCMessage.MSB8027", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to MSB8028: The intermediate directory ({1}) contains files shared from another project ({0}).  This can lead to incorrect clean and rebuild behavior..
        /// </summary>
        internal static string VCMessage_MSB8028 {
            get {
                return ResourceManager.GetString("VCMessage.MSB8028", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to MSB8029: The Intermediate directory or Output directory cannot reside under the Temporary directory as it could lead to issues with incremental build..
        /// </summary>
        internal static string VCMessage_MSB8029 {
            get {
                return ResourceManager.GetString("VCMessage.MSB8029", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to MSB8030: The linker switch &quot;Minimum Required Version&quot; requires &quot;SubSystem&quot; to be set.  Without &quot;SubSystem&quot;, the &quot;Minimum Required Version&quot; would not be passed to linker and could prevent to the output binary from running on older Operating Systems..
        /// </summary>
        internal static string VCMessage_MSB8030 {
            get {
                return ResourceManager.GetString("VCMessage.MSB8030", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to MSB8031: Building an MFC project for a non-Unicode character set is deprecated. You must change the project property to Unicode or download an additional library. See https://go.microsoft.com/fwlink/p/?LinkId=286820 for more information..
        /// </summary>
        internal static string VCMessage_MSB8031 {
            get {
                return ResourceManager.GetString("VCMessage.MSB8031", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to MSB8032: The Platform or PlatformToolset is not available from a 64bit environment.  Consider building from 32bit environment instead..
        /// </summary>
        internal static string VCMessage_MSB8032 {
            get {
                return ResourceManager.GetString("VCMessage.MSB8032", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} -&gt; {1} (Partial PDB).
        /// </summary>
        internal static string VCMessage_MSB8033 {
            get {
                return ResourceManager.GetString("VCMessage.MSB8033", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to MSB8033: Cannot determine a remote location corresponding to {0} directory on local machine. Some files might not be found during remote build. Please change the properties to use relative paths or add a local-remote folders mapping in Tools - Options - Cross Platform – C++ - iOS - Mapping..
        /// </summary>
        internal static string VCMessage_MSB8033_RemoteBuild_MissingDirectoryMapping {
            get {
                return ResourceManager.GetString("VCMessage.MSB8033.RemoteBuild.MissingDirectoryMapping", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} -&gt; {1} (Full PDB).
        /// </summary>
        internal static string VCMessage_MSB8034 {
            get {
                return ResourceManager.GetString("VCMessage.MSB8034", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} -&gt; {1} (Partial -&gt; Full PDB).
        /// </summary>
        internal static string VCMessage_MSB8035 {
            get {
                return ResourceManager.GetString("VCMessage.MSB8035", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to MSB8035: Root folder for local relative paths is not defined, but relative path {0} is used. Please change it to a full path or define root folder..
        /// </summary>
        internal static string VCMessage_MSB8035_RemoteBuild_MissingRootFolderForLocalRelativePaths {
            get {
                return ResourceManager.GetString("VCMessage.MSB8035.RemoteBuild.MissingRootFolderForLocalRelativePaths", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to MSB8036:The Windows SDK version {0} was not found. Install the required version of Windows SDK or change the SDK version in the project property pages or by right-clicking the solution and selecting &quot;Retarget solution&quot;..
        /// </summary>
        internal static string VCMessage_MSB8036 {
            get {
                return ResourceManager.GetString("VCMessage.MSB8036", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to MSB8037:The Windows SDK version {0} for Desktop C++ {1} Apps was not found. Install the required version of Windows SDK or change the SDK version in the project property pages or by right-clicking the solution and selecting &quot;Retarget solution&quot;. .
        /// </summary>
        internal static string VCMessage_MSB8037 {
            get {
                return ResourceManager.GetString("VCMessage.MSB8037", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to MSB8038: Platform Toolset is not defined. Please select one of the available Platform Toolsets in the Project Properties UI..
        /// </summary>
        internal static string VCMessage_MSB8038 {
            get {
                return ResourceManager.GetString("VCMessage.MSB8038", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Warning: Full debug symbol generation from partial PDBs is not supported for static libraries..
        /// </summary>
        internal static string VCMessage_MSB8039 {
            get {
                return ResourceManager.GetString("VCMessage.MSB8039", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to MSB8038: Spectre mitigation is enabled but Spectre mitigated libraries are not found.  Verify that the Visual Studio Workload includes the Spectre mitigated libraries.  See https://aka.ms/Ofhn4c for more information..
        /// </summary>
        internal static string VCMessage_MSB8040 {
            get {
                return ResourceManager.GetString("VCMessage.MSB8040", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to MSB8051: Support for targeting Windows XP is deprecated and will not be present in future releases of Visual Studio. Please see https://go.microsoft.com/fwlink/?linkid=2023588 for more information..
        /// </summary>
        internal static string VCMessage_MSB8051 {
            get {
                return ResourceManager.GetString("VCMessage.MSB8051", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unable to create Xaml task.  Compilation failed.  {0}.
        /// </summary>
        internal static string XamlTaskCreationFailed {
            get {
                return ResourceManager.GetString("XamlTaskCreationFailed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unable to parse Xaml task.  {0}.
        /// </summary>
        internal static string XamlTaskParseFailed {
            get {
                return ResourceManager.GetString("XamlTaskParseFailed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Xml Error: {0}..
        /// </summary>
        internal static string XmlError {
            get {
                return ResourceManager.GetString("XmlError", resourceCulture);
            }
        }
    }
}