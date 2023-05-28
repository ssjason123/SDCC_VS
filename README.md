# Refactor Project Notes
This refactor is a complete rewrite of the msbuild implementation. The goal is to follow the c++ toolchain integration described at: https://learn.microsoft.com/en-us/visualstudio/extensibility/visual-cpp-project-extensibility?view=vs-2022. By adding new platforms (gbz80, etc.) and platform toolsets (sdcc). The end goal is a cleaner and more complete integration that follows the visual studio c++ standard for future compatibility.


# SDCC_VS
Visual Studio package to add SDCC toolchain support. SDCC is not included in the package but can be downloaded from: http://sdcc.sourceforge.net/.

# Release Notes
# Version 2.6
Fix Bug where the AreaAddressBase was not correctly updated with modified values.
Add support for Emulicious 'remotedebug' command line argument.
Update the legacy additional source folder junction process to reduce the number of sleep calls to avoid launch timeouts when using multiple additional source folders.

# Version 2.5
Update to add support for the emulcious "additionalSrcFolders" option.
Major code cleanup.
Rework Json modification and make several fixes legacy.

# Version 2.1
Update vsix build projects to include Microsoft.VisualStudio transitive dependencies.

# Version 2.0
This version breaks compatibility with past versions of the plugin. To repair the issue you will need to update the SDCC paths to use MSBuild paths:
* Original

  * ```<Import Project="$(VCTargetsPath)SDCC/SDCC.Common.props" />```

  * ```<Import Project="$(VCTargetsPath)SDCC/SDCC.Common.targets" />```

* Updated
  * ```<Import Project="$(MSBuildExtensionsPath)\SDCC\SDCC.Common.props" />```
  * ```<Import Project="$(MSBuildExtensionsPath)\SDCC\SDCC.Common.targets" />```

# Version 1.0
Initial development on the package has been focused on GameBoy support. Future support will extend to other platforms as requested.

# SDCC Configuration
By default the SDCC_VS plugin looks for SDCC installation using the "SDCC_HOME" environment variable. This can be overrriden per project by going to the project properties and updating the property under "General -> SDCC -> Path to SDCC". The latest version of SDCC can be installed from: http://sdcc.sourceforge.net/

# Game Boy Development Kit Configuration
The SDCC_VS plugin supports two versions of the GBDK library, GBDK-2020 (https://github.com/Zal0/gbdk-2020) and GBDK-N (https://github.com/andreasjhkarlsson/gbdk-n). The version can be selected using the property under "GBDK -> General -> GBDK Type". By default the SDCC_VS plugin looks for the GBDK installation using the "GBDK_HOME" environment variable. This can be overriden per project by going to the project properties and updating the property under "GBDK -> General -> GBDK Install Path". GBDK support can also be disabled by updating the property under "GBDK -> General -> Supported".

The SDCC_VS plugin can be used to build the GBDK libraries. Sample solutions are available at:
* GBDK-2020: https://github.com/ssjason123/GBDK-2020-VS
* GBDK-N: https://github.com/ssjason123/GBDK-N-VS

# Emulicious Debugger
The SDCC_VS plugin supports debugging using the Emulicious debugger. By default the SDCC_VS plugin uses the "EmuliciousExecutable" environment variable to launch the Emulicious application. This can be overriden per project by upating the property under "Debugging -> "Debugger to launch:" Emulicious Debugger -> Command". 

For more details on using the Emulicious Debugger see the official documentation at: https://marketplace.visualstudio.com/items?itemName=emulicious.emulicious-debugger#Usage

# Local Debugger
The SDCC_VS plugin exposes the native visual studio debuggers. These can be used to launch other applications for testing but do not provide debugging support.

# Known Issues
* Building libraries with the debug flag enabled will result in the following SDCC known issue: https://sourceforge.net/p/sdcc/bugs/2970/
* Currently there is no support for "Attach to Process" for the emulicious debugger.

