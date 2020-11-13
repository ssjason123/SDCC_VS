# SDCC_VS
Visual Studio package to add SDCC toolchain support. SDCC is not included in the package but can be downloaded from: http://sdcc.sourceforge.net/.

# Notes
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

