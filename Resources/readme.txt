Digital Control Library Designer SDK (DCLD) for Microchip dsPIC33®
==================================================================
Version 0.9.5.96 Release Notes:
-------------------------------

This is a major version upgrade with a couple of disruptive changes in the code generator.

New controller features enforced significant changes to the cNPNZ16b_t data structure, where parameters have been grouped into sub-structures helping to better structure and manage the steadily growing number of parameters.
These changes were necessary to better manage code changes, add new features and options while making the cNPNZ16b_t controller object data structure more intuitive for users.

Unfortunately, this will inevitably create conflicts with existing user code modules using previous versions of DCLD.


Code Migration Guide:
---------------------

1) Unified Controller Object Data Structure

In previous versions, two different data structures were available

  * cNPNZ16b_t used for all controller types with 16-bit fixed-point scaling modes
  * cNPNZ3216b_t used for all controller types with 32-bit fast floating-point scaling modes

These two data structures have been merged into one, unified structure called cNPNZ16b_t to allow users to write code, independent from scaling modes.
As a consequence, all filter coefficients are now declared as 32-bit number. If 16-bit fixed-point scaling modes are used for highest execution performance, the number format of coefficients will have ZERO in the high word and the 16-bit coefficient value in the low-word of the 32-bit number (e.g. CoeffB0 = 0x00002A52)
This change does not influence the execution time of the compensation filter computation.

2) Controller Input and Output Ports Sub-Groups

Advanced controllers need to be able to work with values from multiple different input sources resp. access to multiple output targets. 
Although internal number normalization covers most of the mathamtical challenges already, normalization of physical domains is still required.
For this purpose each input and output port of the controller has been grouped into a sub-structure with 

  * Pointer to Object Address: (e.g. Special Function Register (SFR) or global variable)
  * Signal Offset: helping to scale analog signals
  * Normalization Factor: formated as fractional
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

All coefficients and related number scaling paramters have been grouped into 'Filter'.
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
These settings have now been grouped in sub group 'ADCTriggerControl':
Effects on user code:

Specifying the controller input source in user code with previous versions were written like this:
	
	my_controller.ptrADCTriggerARegister = 2800; // Limit maximum control output to 2800 ticks
	my_controller.ADCTriggerAOffset = 600;	     // Delay generated trigger 150 ns

The new structure requires to change this like to 
	
	my_controller.ADCTriggerControl.ptrADCTriggerARegister = 2800; // Limit maximum control output to 2800 ticks
	my_controller.ADCTriggerControl.ADCTriggerAOffset = 600;	   // Delay generated trigger 150 ns

6) Controller Data Provider Sub-Group

When Data Providers are enabled, internal controller data is pushed to user-specified target variables or registers.
These target addresses need to specified in code. The Data Provider parameters have now been grouped in sub-group 'DataProviders'
Effects on user code:

Specifying the controller input source in user code with previous versions were written like this:
	
	my_controller.ptrDataProviderControlInput = &my_vout; // push most recent output voltage sample to user variable

The new structure requires to change this like to 
	
	my_controller.DataProviders.ptrDProvControlInput = &my_vout; // push most recent output voltage sample to user variable


7) Controller Cascade Trigger Sub-Group

When Data Providers are enabled, internal controller data is pushed to user-specified target variables or registers.
These target addresses need to specified in code. The Data Provider parameters have now been grouped in sub-group 'DataProviders'
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

This sub group 'GainControl' specifies all parameters required to perform the cycle-by-cycle gain modulation, but excludes the determination of these parameters.


8) Controller Advanced Sub-Group (new)

This version of DCLD adds a new parameter sub group 'Advanced' to the controller object.
This parameter group may be used as data buffer by advanced features. 



* New Features:

	MPLAB X® Project Parsing
	DCLD now integrates much deeper into MPLAB X® by adoting user settings from the most recent project configuration. 
	You can still use DCLD independent from MPLAB X®, but some of the advanced control features and code generator options may not be available

	Project Configuration Window 
	The inclusion of a specific MPLAB X® project is supported by a configuration window.
	Users can open the porjec configuration at any time from the tools bar. 
	This window will open automatically every time conflicts are detected. 
	
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
