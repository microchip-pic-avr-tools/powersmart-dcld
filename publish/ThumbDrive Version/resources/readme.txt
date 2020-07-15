Digital Control Library Designer SDK (DCLD) for Microchip dsPIC33®
==================================================================
Version 0.9.9.262 Release Notes:
-------------------------------

New Features:

1) New Data Provider Source for compensated Input Value

Description:
In bidirectional converters the current feedback zero line is usually set at half of the ADC range. Within the application code, this offset must be subtracted from the most recent sample to track the effective forward or backward current feedback. These additional steps in calculating the effective current are already performed by the control loop itself and thus can be provided to the user application, easing the use of this value throughout proprietary code. The compensated control input will have no offset and will be inverted if feedback inversion is activated in DCLD.

2) New Anti-Windup Option "Allow Control Output Saturation"

Description:
This option allows the control loop history to further integrate the error beyond the user-defined control output limits. The value written to the defined output target remains to be clamped to user thresholds. This option emulates the behavior of an analog error amplifier circuit.

3) Modified Anti-Windup Option "Enable Limit Debouncing"

Description:
Clamping the control output to hard thresholds with no hysteresis can sometimes result in undesired behavior, usually manifesting as uncontrolled bouncing. The new option zeros the most recent control error together with overwriting the control output value to damp the desaturation and thus the bouncing.

Optimizations:

1) NPNZ16b_t Data Structure Declarations

Description:
Nested data structure declarations of the NPNZ16b_t data structure was changed to better support the MPLAB X source code parser by adding labels in addition to structure type definition names. Each structure now has a label of the form NPNZ_xxx_s and a type definition name of the form NPNZ_xxx_t. When used as data type in variable and function parameter declarations in user code, the expression struct NPNZ16_s is used instead of the pure data type NPNZ16b_t to prevent the MPLAB X code parser from breaking.

Bugfixes:

1) Regional Number Format Support

Description:
Various regional number formats supported by the computer's operating system are now better supported by DCLD. in the past, uncommon number formats have led to data I/O conflicts. These have been resolved and support for more uncommon number formats have been added by increasing the text-to-number conversion capabilities of the input text boxes of the graphical user interface.

2) Bidirectional Feedback Compensation

Description:
When feedback offset compensation and feedback inversion were activated simultaneously, occasional number overruns occurred under certain circumstances. 

Status: Fixed

3) Bode Plot not Updating Ruler Positions when entering Pole & Zero Frequencies

Description:
Occasionally the pole & zero ruler positions of the Bode plot did freeze when a set of frequencies got loaded from the history list.

Status: Fixed

4) In Timing chart, the user trigger offset value did not get loaded from files correctly

Description:
Occasionally, the user trigger offset value did not get loaded correctly into the text box while the saved trigger value got applied to the timing chart correctly.

Status: Fixed

5) Syntax Code Windows Scrolling

Description:
When users scrolled down in code view windows and generated code, the scroll position reset to the beginning of the file. Now it keeps the scroll position of the code view window selected, even if contents change substantially.

Status: Fixed


============================================
(c) 2020, Microchip Technology Inc.
