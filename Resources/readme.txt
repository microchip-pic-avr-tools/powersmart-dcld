Digital Control Library Designer SDK (DCLD) for Microchip dsPIC33®
==================================================================
Version 0.9.7.99 Release Notes:
-------------------------------

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

IMPORTANT NOTE:
===============
If you are upgrading your software from an earlier version than v0.9.5.96, your firmware will not build successfully due to major changes in the cNPNZ16b_t data structure as well as function call names.

Please read the Code Migration Guidelines below carefully following the recommendations!

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

This is a gradual version upgrade with a couple of minor changes in the code generator and refined and optimized features introduced in version 0.9.5.96.

1) Proportional Controller for Plant Measurements

This feature now supports calculation tools helping user to quickly and easily determine the proportional gain factor for the P-Term control loop used in plant measurements. Please refer to the user guide for more information about how P-Term controllers are used to measure the plant frequency response of a power supply and how to set up controller and its coefficient.

When the P-Term control loop option is enabled, DCLD now also generates initialization code for this controller. It gets initialized automatically when the common controller initialization routine [my_controller]_Initialize(&my-controller) is called. No further configuration in user code is required.

2) Feedback Gain Modulation (Adaptive Gain Control)

This is one of the latest features added to DCLD allowing the modulation of the feedback gain during runtime on a cycle-by-cycle basis. This feature is very powerful but needs some attention when used. To better support user-defined functions used to determine the gain modulation factor during runtime, an additional function pointer has been added to the cNPNZ16b_t data structure allowing users to call their function from the Assembly routine before the modulation factor is applied.

Please read the sections in new user guide for more information.

3) Code generation output window

A new output window has been added to the code generator view providing more detailed debugging information. Errors and data violations occurring during internal calculations and code generation are now provided with dedicated error messages and codes to make troubleshooting mode effective.

4) List of Most Recently Opened Files

Inside the 'File' menu you now find a sub-menu where previously opened files are stored. You can open of of the DCLD configuration files by clicking on the regarding list entry and the file will be loaded automatically. The list can be cleared by clicking on 'Clear Recent File List'. The maximum number of recently opened files is currently limited to eight. You can adjust the number of files by editing the key '[recent_files]->max' in the application settings file dcld.ini located in the program folder. The absolute maximum number of files allowed is limited to 128. Setting the maximum number of files =0 will disable this feature and no file history will be stored.

5) Bugfixes

  - solved conflicts with relative path declarations in #include paths
  - solved a calculation error in input gain calculations of digital sources
  - solved a bug accidentally overwriting the ADC gain in input gain calculations when using digital input sources

6) 'Under-The-Hood' changes

With increasing number of code generation options, feature management has been optimized by introducing combinatorial tokens in code generator scripts. These changes are not visible and noticeable for common users but may be of interest for users who manipulate/tweak code generator scripts to better meet their own requirements. More detailed information on code generator scripts is available on request.


============================================

Code Migration Guide from versions prior to Version 0.9.5.96:
-------------------------------------------------------------

With version 0.9.5.96, new controller features were enforced, which significantly changed the cNPNZ16b_t data structure, where parameters have been grouped into sub-structures helping to better structure, organize and manage the steadily growing number of feature parameters. These changes were necessary to better manage code changes, add new features and options while making the cNPNZ16b_t controller object data structure more intuitive and manageable for users.

1) Unified Controller Object Data Structure

In previous versions, two different data structures were available

  * cNPNZ16b_t used for all controller types with 16-bit fixed-point scaling modes
  * cNPNZ3216b_t used for all controller types with 32-bit fast floating-point scaling modes

These two data structures have been merged into one, unified structure called cNPNZ16b_t to allow users to write code, independent from scaling modes.
Consequently, all filter coefficients are now declared as 32-bit number. If 16-bit fixed-point scaling modes are used for highest execution performance, the number format of coefficients will have ZERO in the high word and the 16-bit coefficient value in the low-word of the 32-bit number (e.g. CoeffB0 = 0x00002A52)
This change does not influence the execution time of the compensation filter computation.

2) Controller Input and Output Ports Sub-Groups

Advanced controllers need to be able to work with values from multiple different input sources resp. access to multiple output targets. 
Although internal number normalization covers most of the mathematical challenges already, normalization of physical domains is still required.
For this purpose, each input and output port of the controller has been grouped into a sub-structure with 

  * Pointer to Object Address: (e.g. Special Function Register (SFR) or global variable)
  * Signal Offset: helping to scale analog signals
  * Normalization Factor: formatted as fractional
  * Normalization Scaler: Additional bit-shift scaler of fractional Normalization Factor

Offset and Normalization is optional and related features are not enabled by default. 
DCLD will add related code only if specific options/features are enabled/selected by the user.

This release version supports controllers with up to with two input ports and two output ports.


Effects on user code:

Specifying the controller input source in user code with previous versions were written like this:
	
	my_controller.ptrSource = &ADCBUF2; // assigning ADC buffer of AN2 as data input
	my_controller.InputOffset = 125;	// feedback voltage has 100mV Offset

The new structure requires to change this like to 
	
	my_controller.Ports.Source.ptrAddress = &ADCBUF2; // assigning ADC buffer of AN2 as data input
	my_controller.Ports.Source.Offset = 125;		  // feedback voltage has 100mV Offset


3) Controller Filter Sub-Group

All coefficients and related number scaling parameters have been grouped into 'Filter'.
Effects on user code:

Specifying the controller input source in user code with previous versions were written like this:
	
	my_controller.PostShiftA = -2; // Backward normalization of Q15 fractional result with factor 4

The new structure requires to change this like to 
	
	my_controller.Filter.PostShiftA = -2; // Backward normalization of Q15 fractional result with factor 4

4) Controller Limits Sub-Group

Threshold values of Anti-Windup limits are now grouped in 'Limits'. Furthermore, a second set of output limits has been introduced to support alternate output port configuration.
Effects on user code:

Specifying the controller input source in user code with previous versions were written like this:
	
	my_controller.MaxOutput = 2800; // Limit maximum control output to 2800 ticks

The new structure requires to change this like to 
	
	my_controller.Limits.MaxOutput = 2800; // Limit maximum control output to 2800 ticks

5) Controller ADC Trigger Control Sub-Group

The function Automated ADC Trigger Placement supports up to two ADC triggers, which can be automatically positioned during runtime.
The configuration of this feature requires the declaration of the ADC trigger compare register and an additional, user-specified offset value (usually used to compensate for hardware related delays).
These settings have now been grouped in sub-group 'ADCTriggerControl':
Effects on user code:

Specifying the controller input source in user code with previous versions were written like this:
	
	my_controller.ptrADCTriggerARegister = 2800; // Limit maximum control output to 2800 ticks
	my_controller.ADCTriggerAOffset = 600;	     // Delay generated trigger 150 ns

The new structure requires to change this like to 
	
	my_controller.ADCTriggerControl.ptrADCTriggerARegister = 2800; // Limit maximum control output to 2800 ticks
	my_controller.ADCTriggerControl.ADCTriggerAOffset = 600;	   // Delay generated trigger 150 ns

6) Controller Data Provider Sub-Group

When Data Providers are enabled, internal controller data is pushed to user-specified target variables or registers.
These target addresses need to be specified in code. The Data Provider parameters have now been grouped in sub-group 'DataProviders'
Effects on user code:

Specifying the controller input source in user code with previous versions were written like this:
	
	my_controller.ptrDataProviderControlInput = &my_vout; // push most recent output voltage sample to user variable

The new structure requires to change this like to 
	
	my_controller.DataProviders.ptrDProvControlInput = &my_vout; // push most recent output voltage sample to user variable


7) Controller Cascade Trigger Sub-Group

When Data Providers are enabled, internal controller data is pushed to user-specified target variables or registers.
These target addresses need to be specified in code. The Data Provider parameters have now been grouped in sub-group 'DataProviders'
Effects on user code:

Specifying the controller input source in user code with previous versions were written like this:
	
	my_controller.CascadedFunction = &my_function; // Call user function at the end of controller execution
	my_controller.CascadedFunParam = 650; // Parameter of function call 

The new structure requires to change this like to 
	
	my_controller.CascadeTrigger.prtCascadedFunction = &my_function; // Call user function at the end of controller execution
	my_controller.CascadeTrigger.CascadedFunParam = 650; // Parameter of function call 


8) Controller Gain Control Sub-Group (new)

This version of DCLD introduces a new feature called Adaptive Gain Control, which is a specific, highly generic form of digital feed forward control.
This feature breaks down into two parts:

  * Applying gain modulation to the compensation filter on a cycle-by-cycle basis
  * Determining the most recent gain modulation factor 

This subgroup 'GainControl' specifies all parameters required to perform the cycle-by-cycle gain modulation but excludes the determination of these parameters. A function pointer to a user-defined function can be set to include the factor determination routine.


8) Controller Advanced Sub-Group (new)

This version of DCLD adds a new parameter subgroup 'Advanced' to the controller object.
This parameter group may be used as data buffer by advanced features. 



* New Features:

	MPLAB X® Project Parsing
	DCLD now integrates much deeper into MPLAB X® by adopting user settings from the most recent project configuration. 
	You can still use DCLD independent from MPLAB X®, but some of the advanced control features and code generator options may not be available

	Project Configuration Window 
	The inclusion of a specific MPLAB X® project is supported by a configuration window.
	Users can open the project configuration at any time from the tools bar. 
	This window will automatically open every time conflicts are detected. 
	
	Input Gain Calculation Tool
	DCLD supports the inclusion of signal gain dependencies. To calculate the related gains more easily, new calculation tools have been introduced, making it easier to calculate the figures for 
	
	* Voltage Dividers 
	* Current Sense Amplifiers
	* Current Sense Transformers
	* Digital Sources (e.g. Input Capture)
	
	Adaptive Gain Control
	This new feature adds a modulation factor to the control loop. However, by enabling this function only the required hooks are added to the control code.
	The effective adaptive algorithms required to perform the runtime gain modulation are added in later releases.
	
	Seamless Controller Migration
	By unifying the cNPNZ16b_t and cNPNZ3216b_t data structure into one, controllers and all their features can now be seamlessly migrated across all number scales removing previous limitation or required, minor user code edits.

	Code Generator
	By introducing support of MPLAB X® project assignments, users can now select the include path, which should be used while generating pre-compiler file inclusion declarations. 

	Timing Chart: Added User-Specified ADC Trigger Offset
	The cNPNZ16b_t controller object supports user-defined trigger offsets to allow compensation of hardware related propagation delays.
	This feature is now also supported by the timing chart where users can specify the trigger offset value below the chart.
	
	Timing Chart: CPU Load Indicator
	The Timing Chart now also displays a rough CPU load estimation as percentage. The parameters used to calculate this figure is based on sampling frequency x execution cycles of the most recent controller.
	

* Bugfixes: 
	File associations and path declarations in DCLD are relative by default, but absolute paths are supported.
	Especially in configurations where mixed absolute and relative paths declarations were used, users occasionally encountered issues with DCLD not being able to properly resolve directory information.
	These issues have been solved by introducing a completely new directory management structure, incorporating information stored in the associated MPLAB X® user project.


============================================
(c) 2020, Microchip Technology Inc.
