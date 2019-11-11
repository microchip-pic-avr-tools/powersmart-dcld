Digital Control Library Designer SDK
====================================

z-Domain Loop Configuration Tool & Code Generator Module


This is a beta version showcasing the essential functions of the z-domain configuration block of the Digital Control Library SDK.
The main purpose of this tool is to allow the generation of customized, tailored digital SMPS controllers without the need to write DSP code.
A standardized API ensures seamless integration into the final firmware, supporting multiple, co-existing controllers in one firmware.
The unified API also allows seamless migration between controller orders and selecting functional option by simple mouse clicks.


Currently supported features:

  - Supports z-Domain Compensation Filters from 1st to 6th Order (1P1Z, 2P2Z, 3P3Z, 4P4Z, 5P5P, 6P6Z)
  - Fixed-Point and Floating-Point DSP Library Support
      o Fixed Point Number scaling options:
	  * Single-bit shift scaler
	  * Dual bit-shift scaler (one for A-, one for B-coefficients)
          * Coefficient Scaling Factor (Microchip SMPS standard libraries)

      o Fast Floating Point Scaling:
	  * Input currently limited to 16-bit controller input with 32-bit coefficients
          * Error and control histories are 16-bit wide

	Please note:
	Up to know only Q15 format is supported for all scaling methods named above. 
        Extended 32-bit scaling options including full 32-bit wide fast floating point are in preparation

  - Built-In Number Resolution Analysis and Optimization for optiized performance tuning

  - Graphic Loop Adjustment
      o Bode plot with movable markers for all filter poles and zeros
      o Comparison between s- and z-domain filter performance over frequency
      o Bode data export (text table) optimized for direct Copy&Paste into MS Excel

  - Graphic Execution Timing Analysis
      o Timing Optimization recently only supports mode 0 - PWM Interrupt Trigger (Minimum CPU Load) which is the linear code execution (calculate A-term, read error, update error history, calculate B-term, anti-windup, write-back, trigger positioning, control history update)
	The fast response mode (where the most recent error is read, the last MAC is calculated, fed into anti-windup and immediately written back - all necessary updates are done at the end of the control routine) is not supported yet. 
	This method has a couple of fundamentl downsides:
	  * Histories are distorted between control steps
	  * Total CPU workload is higher (no linear execution)

	Usually you can achieve equal or better results if the controller is optimized using the Timing view and then using a PWM trigger to position the control loop execution over ADC conversion time.
	Eventually it is the option between handling the overhead at the beginning or at the end. Due to the lower total CPU load the linear execution with PWM interrupts is preferred.

  - ANSI C/DSP Assembly Code Generation
      o Generation of assembly DSP library, C-source and header files providing unified API
          * User-defined naming options for support of multiple, co-existing loops
          * Each file can be generated to an independent, user defined directory
          * Code generation of each file can be selected/skipped by user
      o Selectable controller features
          * DSP/MCU Context Save/Restore:
            Depending on features provided by the silicon and related configuration, users can select the context which should be saved/restored when the control interrupt is called
	  * Features:
              ~ Alternative code configuration allows co-existance of various DSP routines co-existing with the control library
              ~ Enable/Disable status bit features allows to enable/suspend the control loop execution during runtime
              ~ Manual error input normalization to support different ADC architectures across dsPIC device family generations
              ~ ADC trigger placement for accurate average value tracking in voltage and average current mode control
              ~ Generation of shadow copies of control input, recent error, control output publishing these values within the firmware
              ~ Anti Windup (control output clamping) with hard and soft saturation options
              ~ Generation of Anti Windup status bits to allow detection saturation conditions

  - MPLAB X IDE integration
      o Configuration file can be included in firmware project
          * Click on integrated configuration file aloows to open DCLD directly from the MPLAB X IDE Project Manager
          * Changes can be tracked by Git/Mercurial


Getting started:

  - Open the GUI
  - Go to the Source Code tab on the right
  - Specify the target paths for every generated file (assembly, C-source, C-header, Library header)
  - Choose a name Prefix for the controller on the Source Code tab on the left
  - Go to Controller tab on the left and select the Bode Plot tab on the right
  - Select your controller and adjust poles and zeros
  - Check the rounding error of the coefficients (A-coefficients are the most critical ones) => The coefficient table should not show warnings (yellow or red) on A-coefficients!
  - Select the Source Code tab on the left and the Timing tab on the right
  - Choose your code options (context saving, anti-windup, etc.)
  - Click Tools => Generate Code (ALT+G)
  - Click Tools => Export Generated Files => Export Files (SHIFT+F5)

	Please note:  
	The Generic Library File npnz16b.h is only needed once in the firmware.
	If you are configuring more than one controller you only need to generate this file once. There are no dependencies to any other settings. 
	No harm is done if this file gets overwritten multiple times by different configurations as its contents are truly generic.

  - Save the configuration by clicking File => Save/Save As… (CTRL+S)

Special Features:

  - Data export using menu Tools => Copy To Clipboard
      o Transfer function (Bode Plot Data)
          ~ Copies the bode plot data table into the clipboard (s-domain only)  as tab-separated text table with column headers. This data can directly be pasted into MS Excel.
            The data table is set to 1601 points ranging from 1Hz to 1MHz. In this version this cannot be changed (but is on the backlog list).
	    For the time being I’d recommend to copy & paste the data into Excel and merge it with your plant simulation / measurement data to derive the open loop gain transfer function

            For multi-loop systems I am working on a dedicated MS Excel export which is generating complex formulas in Excel so that you can multiply/add multiple transfer functions for multiphase converters.

      o Coefficient Declaration
          ~ This is practically the content of the C-source file and therefore just another way to export generated code
            You could as well just select the generated code from the C-Source window and copy it from there or generate this file only to a specified location

      o Coefficient Table
          ~ All coefficients are exported as table covering floating point, scaled floating point, fixed point, hex, int and binary number format and the rounding error. 
	    This is mailny meant for documentation purposes.

      o Config File Location
          ~ Copies the file location of the op-code file for the assembly code generation. This is the script used to build the assembly library. 
	    Unfortunately the C-code is not script based yet. So any larger modification affecting names or data structure elements would result in necessary changes to the DCLD code. 
            In future versions beyond v1.0 both (assembly and C) will be fully script based to allow anyone to add and modify supported controllers without the need to change to the main program.




PlEASE SEND YOUR FEEDBACK AND QUESTIONS DIRECTLY TO andreas.reiter@microchip.com.


HISTORY:
========
v0.9.0.23	07/26/18	Initial release

v0.9.0.25	07/27/18	Bug Fixes/Features
				
	- change the data structure element ptrADCTriggerOffset, which was a pointer to a 16-bit variable to ADCTriggerOffset, which is now a true 16-bit value
	- internal, absolute path information used to work with internal resource files have been changed to become relative paths. (performance improvement)

v0.9.0.32	08/19/18	Bug Fixes/Features

	- resolved a bug in the variable declaration in the header file for fast floating point scaling 
	- resolved a bug in the code generation of register save/restore instructions added to assembly code, where the status register bit was always added, regardless of the option setting
	- resolved the issue of the gain folding upwards instead of dropping into the Nyquist abyss due to unresolved inversion in the conversion equation
	- Bode plot export now supports selection of s-domain and z-domain transfer function
	- Files can now directly be opened by double-click on a project file, however the file association is not properly set during installation or will not be present when you use a ThumbDrive version. under these circumstances, right-click on a *.dcld project file in Windows Explorer and choose the DCLD.exe application as "Open With..." default target.

v0.9.0.33	09/10/18	modification

	- MPLAB XC16 v1.35 Linker crashed occatsionally when a C-code and assembly source file have the same name. Now the generated assembly library gets the extension "xxx_asm.s" by default
	- after startup the generator target path in source file windows shows the filename. As this is not required (it's a directory path only) these have been removed.

v0.9.0.34	09/19/18	features

	- code generation in previous versions was acknowledged by a simple message box claiming that all files were generated successfully. Now a list of all files is shown indicating which files have been generated or skipped in accordance with user settings.
	- file "npnz16b.h" will always be generated  when selected. The auto-deselect feature for this generic file was removed on request.

v0.9.0.35	09/24/18	bug fixes

	- added exception handlers to calculation routines to handle math errors like divide-by-zero or multiply-by-infinity
	- added status bar output when entered numbers result in non-processible result

v0.9.0.36	09/29/18	bug fixes/features

	- added warning bar to source code windows to indicate when pole & zero locations have been changed and a code generation process is pending/has not been performed yet
	- added debuggin information about operating system, hardware and regional settings, which is printed to the output window at startup
	- BugFix: resolved a bug with the value of the input gain, which resulted in a wrong valueif regional settings of decimal point and grouping letter differed from generic English settings
	- BugFix: optimized startup behavior and solved occasional problems with invisible controls (pole and zero entry boxes)

v0.9.0.38	09/29/18	bug fixes/features

	- changed the Copy Coefficient Table To Clipboard feature. It now copies a table in the form of the table displayed below the Bode plot with columns separated with tabs to be directly pasted in MS Excel
	- added scaled frequency entry. Frequencies can now be entered in the form e.g. xxxk for kHz. Supported scaling letters: T=Tera, G=Giga, k=kilo, m=milli, u=micro, n=nano, p=piko, f=femto
	- Bugfix: Updated references for code-highlighting libraries preventing machine-dependent .net version conflicts when opening the application.

v0.9.0.39	09/30/18	optimization

	- minor internal codeexecution optimization without visible impact to the user

v0.9.0.40	10/01/18	features

	- added a tool bar to enhance user experience

v0.9.0.44	10/02/18	bug fixes/features

	- added a progress bar function to the status bar (lower edge of the main window) to indicate activity during long processes such as code generation
	- replaced internal components to stay compatible with earlier versions of the Windows .net framework. This helped to resolve issues with setup programs and especially the thumb-drive version without installation process

v0.9.0.45	10/02/18	major feature change

	- the input gain setting will not affect the coefficient generation anymore. It now only adjusts the transfer function results so that results are correctly representing the physical system.
	  Any decisions on gain adjustments do now have to be made by the user.

v0.9.0.46	10/03/18	features

	- the right side of the main window got a new tab showing standardized block diagrams, z-transform equations and flow charts of the selected controller type (read only)
	- The timing chart got a new analysis value indicating the expected CPU load, depending on selected code features, CPU performance and sampling frequency

v0.9.0.47	10/04/18	feature change

	- there is a new check-box allowing the selection of internal Input Gain Normalization. When the input gain is set to non-"1" values and the normalization is enabled, the control loop gain will be changed to compensate the input gain variation. When this option is disabled (default) then the gain in the bode chart will have an offset and coefficients won't be affected.
	- Preparation for bi-directional feedback signal scaling (not enabled yet)

v0.9.0.48	10/05/18	bugfix/feature enhancement

	- Bugfix: "Save" button remained disabled when parameters were changed under some conditions.
	- Progress bar in the lower right corner of the status bar indicating loading/saving/code generation processes got a label

v0.9.0.49	10/05/18	bugfix

	- Bugfix: Code Generator Update warning within Timing Chart Tab was not functional
	- Bugfix: Timing results table was overlapping on some screens. These items have been re-positioned.

v0.9.0.50	11/09/18	bugfix

	- Bugfix: When opening DCLD by doubleclick on a configuration file, transfer function parameters may not get set correctly. This issue is now resolved.

v0.9.0.51	11/11/18	features

	- Feature: Anti-windup settings now support soft desaturation where the most significant error in the error history array is reset to zero (no error saturation)
	- Feature: Anti-Windup saturation flag bits are now reset automatically by the controller code library.

	Please note: Code generation of anti-windup in assembly flie has changed. Please make sure the auto-reset of the saturation flag bits does not create conflicts in diagnostics engines or fualt handlers. 


v0.9.0.52	11/14/18	features

	- Feature: (Mini-adder) CPU load calculation result in Timing view seems to be misunderstood some times. CPU load is only created when the MCU executes code and therefore the only value of the sampling frequency is of interest.
	  This value, however, is adjusted outside the Timing window under control parameters. 

v0.9.0.53	11/28/18	features

	- Feature: The source code file path defining the directory in which generated code files will be placed, now supports relative file paths to ease the use when moving/exchanging MPLAB X projects or when copying DCLD config files to new projects.

v0.9.0.54	11/29/18	bugfix

	- Bugfix: The relative/absolute path translators introduced in version 0.9.0.53 generated flie paths with double backslashes under certain circumstances. This bug has been fixed in this release.

v0.9.0.55	12/13/18	bugfix

	- Bugfix: Wrong s-domain transfer function in the Block Diagram View was replaced with the correct equation.

v0.9.0.58	04/17/19	feature update

	- Feature: Feedback input offset is now supported by enabling the "Bi-directional Feedback" option on the 'Controller" tab. This will add lines to the assembly library subtracting a user defined value from the most recent input value.
	- Feature: Adding file location to the #include "xxx" preprocessor declaration in C-source and C-header files is now possible by selecting "Add file location in generated code #include path" option on the individual code generator tab

v0.9.0.60	07/12/19	bugfix

	- Bugfix: on some systems the installation failed to complete successfully due to certificate conflicts. 
                  No funcitonal changes have been made to the main application

v0.9.0.64	10/19/19	feature update

	- Feature: the previously introduced input offset compensation showed some short comings when the input voltage
		was unexpectedly lower than the given offset value. The new implementation now supports true bi-directional 
		feedbacks by reliably generating seamless error signals in the positive and negative range. If a zero-line
		cut-off is required, this would have to be done in user code prior to calling the control library code.
		(This feature extension is on the list but not supported yet)

v0.9.0.70	10/20/19	feature update

	- Feature: Input signal offset compensation now fully supports controlled feedback inversion, allowing the design of bi-directional controllers which can seanlessly switch between quadrants.
		
	- Feature: Settings, which have been used to generate code files, are now stored in a istory list located below the Bode plot.
		Users can use this list to reload previously used settings by doubble-click, SHIFT+ENTER hot key or a mouse context menu.
		Users can also label and rename or delete entries in this list.

v0.9.0.72	11/07/19	feature update

	- Feature: Added {Dummy-Read when Disabled} Option This option has been added for dsPIC33CH and dsPIC33CK devices where an 
		ADC buffer always needs to be accessed when its ADC interrupt is triggered and the interrupt service routine (ISR) 
		is called. This new option will add code for reading the specified data source register/variable even when the controller 
		is disabled and the control loop execution is bypassed to prevent the CPU from stalling on dsPIC33CH and dsPIC3CK devices. 
		This option is not required for dsPIC33FJ and dsPIC33EP devices.

	- Feature: Added {Second ADC Trigger} Option** In a multi-loop system control modes such as average current mode control, an outer 
		loop (e.g. voltage loop) generates reference values for the inner loop (e.g. current loop). The duty cycle, frequency or 
		phase-shift adjustment, however, is computed by the inner loop only. Any ADC trigger placement which needs to be aligned 
		and synchronized with the switching waveform can therefore only be set once the inner control loop execution has been completed. 
		As outer and inner loop may require two independent ADC trigger locations within one switching cycle, an option for placing a second 
		ADC trigger has been added. The two available triggers ADC_Trigger_A and ADC_Trigger_B are equal and can be used as desired. 
		When no second ADC trigger is required and the option is disabled, ADC_Trigger_B will be ignored by the control loop.

v0.9.0.75	11/11/19	feature update

	- Feature: Added three Data Provider Sources to control loop. These data providers are pointers to external data buffers where
		users can decide to push internal runtime data to. Users can select most recent input value (from source), most recent error
		and most recent control output. If the most recent data input (from source) is selected, data is also pushed when the 
		control loop is disabled.

	- Internal Upgrade: C-Code generation is now fully based on an external code generator script, which allow users to modify the generated
		C-Code when desired. 
______________________________
(C) Microchip Technology Inc.
Date/Time: 12:41 PM 07/26/18

