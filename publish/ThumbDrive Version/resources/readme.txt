Digital Control Library Designer SDK (DCLD) for Microchip dsPIC33®
==================================================================
Version 0.9.9.262 Release Notes:
-------------------------------

New Features:

1) New Data Provider Source for compensated Input Value

Description:
In bidirectional converters the current feedback zero line is usually set at half of the ADC range. Within the application code, this offset must be subtracted from the most recent sample to track the effective forward or backward current feedback. These additional steps in calculating the effective current are already performed by the control loop itself and thus can be provided to the user application, easing the use of this value throughout proprietary code. The compensated control input will have no offset and will be inverted if feedback inversion is activated in DCLD.

Optimizations:

1) Regional Number Format Support

Description:
Various regional number formats supported by the computer's operating system are now better supported by DCLD. in the past, uncommon number formats have led to data I/O conflicts. These have been resolved and support for more uncommon number formats have been added by increasing the text-to-number conversion capabilities of the input text boxes of the graphical user interface.

Bugfixes:

1) Bidirectional Feedback Compensation

Description:
When feedback offset compensation and feedback inversion were activated simultaneously, occasional number overruns occurred under certain circumstances. 

Status: Fixed

2) Bode Plot not Updating Ruler Positions when entering Pole & Zero Frequencies

Description:
Occasionally the pole & zero ruler positions of the Bode plot did freeze when a set of frequencies got loaded from the history list.

Status: Fixed

3) In Timing chart, the user trigger offset value did not get loaded from files correctly

Description:
Occasionally, the user trigger offset value did not get loaded correctly into the text box while the saved trigger value got applied to the timing chart correctly.

Status: Fixed


============================================
(c) 2020, Microchip Technology Inc.
