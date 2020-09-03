Digital Control Library Designer SDK (DCLD) for Microchip dsPIC33®
==================================================================
Version 0.9.10.501 Release Notes:
---------------------------------

In this major feature release, many visible and invisible optimizations have been implemented. The most significant changes affect the Bode plot and mathematical transfer function approximation, project configuration and export procedure of generated code files. The code generator has been optimized and the assembly control library templates have been optimized for more efficient code execution as well as higher number precision.

New Features:

1)  Bode Plot
a.  X-/Y-axis scaling and display option panel
The previous mouse functions of the Bode plot control allowing users to directly move scales up and down to adjust the visible range of the diagram seemed to be too confusing and well hidden to be useful. Hence, we decided to it the classic way by integrating an axis scale configuration panel next to the chart.
b.  One-Click transfer function export with selectable frequency range and resolution
The new Bode plot axis configuration panel opened up the option for a more intuitive way in adjusting the data range of the transfer function. When the transfer function data table is copied to the clipboard, the most recent frequency scale settings and data resolution (number of data points) is used. Hence, if you need to adjust the data range to fit data tables of other tools, you can now use the axis configuration panel to make your adjustments before copying the data table to the clipboard.
c.  Discrete time domain transfer function approximation improvement
Previous versions of DCLD used a very simple approximation of the z-domain transfer function for the Bode plot. This approximation was only valid up to the Nyquist frequency, incorporating the Zero Order Halt (ZOH) of the control loop to estimate the phase and gain erosion over frequency. However, with broader application of DCLD in various applications it turned out, that this approximation often deviated significantly in the higher frequency range. These deviations were by default negative, showing a better gain margin in simulation than in real-work bench tests, occasionally resulting in gain-margin related stability issues requiring unnecessary, manual retuning of the loop. 
Improvements of the approximation of the z-domain transfer function fixes these problems by providing reliable data in the high frequency range also representing the commonly expected gain convolution beyond the Nyquist frequency.
d.  Enhanced display options
The new Bode diagram axis configuration panel also provides simplified access to functions like Show/Hide s-Domain Transfer Function, Unwrap Phase, Show/Hide Legend and Copy data right on the surface reachable with a single mouse click.
2)  Project Configuration Dialog
a.  Extended project configuration dialog, now covering target file path declarations of generated source code files in one place
One of the major changes made to DCLD in this release was the contraction of the file location declarations in the Project Configuration window. So instead of having to click through all source file output windows entering file location declarations individually, all individual file locations now are declared on a single tab of the Project Configuration dialog. If any of the file declarations is inaccessible or cannot be resolved, warnings are generated to prevent issues during file export.
3)  New Source Code File Export Dialog 
a.  Allows review of file export settings before export
The simple “Export Files” function was replaced by a new dialog, which allows a last review of file names and locations, file status and potential export issues as well as selecting which files should be exported without having to roam through window menus. All settings made will be stored in the configuration file and will be recalled for the next file export.
b.  Output window allowing more detailed troubleshooting
During the export detailed output messages are generated in the output window of the File Export dialog enhancing debugging and troubleshooting.
c.  Autosave Entry Naming in Configuration History (Bode plot view)
When files are exported, it is assumed by DCLD that these files will be programmed and thus the most recent filter settings will be applied to hardware, which triggers the generation of a configuration history entry. In previous versions these entries have been named with a simple ‘(Autosave)’ label. This turned out to come short when trying to find a certain configuration in a longer list of history entries. Now ‘Autosave’ entries also captures the selected filter type, number scaling option and sampling frequency in the entry label to help users to better identify settings.
4)  Code Generator Options
a.  Simplified Controller Name Label entry
Previous versions of DCLD used two labels for naming of files and variables. This was originally implemented to keep variable names short while allowing more detailed file names. Although this seemed to be a good idea at the time, it turned out to be too confusing and the voices voting for reducing it to one unified naming of files and variables increased and eventually this was changed in this release and controller naming now only uses one single label for everything.
b.  Enhanced support for controller Data Provider Sources in Bypass mode
The code generator offers options for adding so-called Data Provider pointers to the assembly routine. These Data Providers will push runtime data such as most recent input value, control error or output value to user specified variables/memory addresses automatically while the control loop is running. The previous implementation, however, was occasionally coming short. One gap was that data providers stopped updating data when the controller got turned off. In particular, when the automatic update stopped when the controller was turned off, the update of the most recent control output did not get reset to zero, which eventually caused challenges when other software instances were using this value driving other algorithms.
In the recent release all Data Providers get updated when the controller is turned off and the most recent control output is automatically reset to zero. This function still only works if the Enable/Disable feature is turned on together with option ‘Always read when disabled’.
c.  Adaptive Gain Control accuracy and data resolution improvement option
Adaptive gain control multiplies a modulation factor with the partial filter result of the LDE B-term. During computation of the compensation filter result, this B-term intermediate result is up to 40-bit wide, being stored in the accumulator. The additional modulation computation requires to convert this value into a Q15 compatible number, inevitably resulting in a loss of resolution. This resolution limitation becomes significant when the intermediate result is small. The new option converts the contents of the accumulator into a fast floating point number to dynamically increase the data resolution of this number before the modulation multiply, eventually preventing resolution related modulation inaccuracy issues.
In general, it is recommended to enable this high-resolution mode but there might be applications where execution speed is superior over accuracy, in which case this option can be disabled.
5)  Timing Chart:
a.  Extended selection of control loop trigger modes (PWM interrupt or ADC interrupt)
The controller timing vs. ADC activity period was tuned to be used for a control loop call by the PWM interrupt only in previous versions. Users had to tweak the control loop latency setting to align ADC and control loop. In this new release users can choose between the control loop trigger being the PWM interrupt or ADC interrupt, accounting for the timing differences automatically. However, users might still need to tweak the control loop latency timing setting to account for specific features of the ADC such as Early Interrupts (please read the dsPIC33 data sheet for more details on this feature)
b.  Runtime instruction cycle count estimation provides additional information about 'normal' runtime CPU load considering code flow branches
With increasing number of code generator options, the code execution flow splits up into more and more paths. Hence it became necessary to determine the major code execution path, count its cycles and provide an additional, alternative timing estimation pointing out the total number of CPU cycles of the loop code AND the maximum number of cycles which will be executed under worst case conditions during operation. This new assessment result is also used to estimate the CPU load created by the loop.
c.  Timing Conflict Warning in Timing Chart
The DATA READ ruler of the timing chart will show a warning in form of an exclamation mark when the ADC period collides with the DATA READ event of the control loop to draw the attention of the user to potential issues with sampling time and related, increase phase erosion.

Optimizations:

1)  DCLD Graphical User Interface
a.  Improved performance of code generation
By introducing extensive debugging information during code generation, the overall performance dropped resulting in occasional update issues and kind-of sloppy performance of the code generator option catalog on slower computers. By contracting debug messages in junks, this behavior has been improved without reducing the number of debugging messages produced I the output window.
b.  Improved performance of transfer function data series calculation
The computation of the transfer function data series has been revised and improved to make the Bode diagram control more agile when adjusting filter poles & zeros
c.  Improved accuracy of Bode Plot cursor measurement
The mouse position resolution has been increased to improve the accuracy of the cursor measurement inside the Bode diagram.
d.  Improved accuracy of Timing Chart cursor management
The mouse position resolution has been increased to improve the accuracy of the cursor measurement inside the Timing chart.
e.  Include path sanity check with error indication
each source code output window features an option to include the file relative path with respect to the project include directory in the #include pre-compiler directive. As part of the sanity checks implemented in the file location declaration and file export window, these sanity checks are now also performed on #include paths before generating code.
f.  Revised layout of graphical user interface
The DCLD GUI got a little face lift to make it more intuitive and to make it easier for users to find information. This was mainly done to tackle the steadily increasing number of features. 
2)  Code Generation
a.  Improved performance of generated assembly routine by optimizing working register usage of status word (all modes and controllers)
By reviewing the possible code execution flows dependent on controller type selection, number scaling option and optional feature selections, we were able to optimize the usage of working registers across all controllers helping to reduce the total number of instruction cycles of the generated code. In most cases this will only result in minor improvements of three to four instruction cycles but … nevertheless.
b.  Improved performance of generated assembly routine by optimizing pointer management (all modes and controllers)
(Same as above)
b.  Improved number resolution in Single Bit-Shift Scaling mode with Output Factor Scaling
In single-bit shift mode with output scaling factor the final output of the control loop gets multiplied with a backwards scaling factor. This mode is generally providing a slightly better fixed-point number resolution than the faster pure single bit-shift mode. However, at the end of filter computation the 40-bit wide result gets converted into a Q15 compatible factor inevitably resulting in loss of resolution. In most cases the output factor multiplication increases the resolution, which is why this was considered to be a feasible implementation. Most recent analysis of low frequency error integration characteristics, however, showed some unexpected shortcomings with specific combinations of filter coefficient values. Therefore, it was decided to add code for dynamic resolution improvement before the factor multiplication, similar to the AGC resolution improvement described above.
Please note:
This increases the total instruction cycle count of this number scaling mode, which might impact controller execution timing by prolonging the control response. If you use this number scaling mode in your application with tight timing requirements, it is recommended to review the overall execution timing still works out in your setup.

Bugfixes:

1)  DCLD Graphical User Interface
a.  Unhandled exception during code generation triggered by corrupted file path 
By generating code without resolving and checking relative file path declarations ahead of the code generation occasionally resulted in a failing file export after code has been generated. This exception was not handled in code resulting into an unhandled exception error message popping u pat the end of the file export procedure.
b.  Unhandled exception in numeric text boxes
A floor in the value conversion of user inputs in numeric text boxes resulted in an unhandled exception error message to be thrown. This error was triggered when two physical SI unit scalers followed sequentially (e.g. kM or mk). These conditions could be provoked while data was typed into the test box. This bug has been fixed by skipping the value update whith non-sensical inputs.
c.  Resolution of relative file path declarations occasionally mixed up C- and Assembly include directory declarations, causing the compiler to fail after files got exported
The #include path declaration accidentally got mixed up under certain conditions, resulting in the wrong file path being generated in assembly files. As a result, code generation and file export passed successful but code compilation failed inside MPLAB X as the compiler could not resolve the given #include path.
2)  Code Generator
a.  Coefficients occasionally did not get updated before code generation due to bug in internal event handling
Performance issues introduced by increasing number of debugging messages being generated during the code generation process resulted in conflicts with the internal event handler when users kept changing options and values while the generator was running in the background. As a result, coefficient values did not get applied/updated as expected.
b.  Control Output Data Provider pushed wrong data value during runtime
In single-bit shift mode with output factor scaling, the Control Output Data Provider accessed the wrong working register when pushing data to user variables. 
d.  When context management was enabled, working registers of MAC instructions got restored in the wrong sequence
Working registers pushed to the stack need to be restored in reverse order to end up in the same place again. An error in the code generation script, however, restored the MAC working registers in the same sequence as they were saved, eventually scrambling the context.
e.  In Fast Floating Point Scaling mode with Adaptive Gain Control enabled, the factor multiplication corrupted data pointers causing the filter computation to fail
In some implementations the adaptive gain modulation routine accidentally overwrote pointers used by the fast floating point filter computation corrupting the result.
f.  In Single-Bit Shifting mode with Output Factor Scaling, conditions occurred, where coefficients got rescaled by one bit but this rescaling was not included in the output scaler computation
A floor in the coefficient scaler calculation of DCLD resulted in two different results for each coefficient scaler. As a result, each coefficient got scaled a second time by one bit by which the coefficient value output on the GUI showed non-sensical results. The coefficients generated in code, however, were correct.

Change Notes - Impact on Existing Projects:

1)  API Changes
In this release version the API did not change, and existing user code will not be affected.

2)  Code Execution
Several optimizations and new features may have influence on the execution time of generated controller code modules. 
While optimization of working register usage, pointer handling optimization are reducing the overall execution time, data resolution improvement of AGC factor multiplication and Single Bit-Shift with Output Factor Scaling are slightly increasing the total instruction cycle count. 
Even if these changes only have minor impact on the overall code execution timing, it is recommended to use the timing chart to verify the new control timing does not create conflicts in existing projects, if any of these options are used.


A BIG Thank You to all Beta users who spent all the time and efforts digging into features and issues helping to make this tool better and better!
We deeply appreciate your support!

============================================
(c) 2020, Microchip Technology Inc.
