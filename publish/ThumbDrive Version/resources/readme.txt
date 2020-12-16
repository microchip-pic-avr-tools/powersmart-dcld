PowerSmart™ SDK for Microchip dsPIC33® Digital Signal Controllers
Digital Control Library Designer (PS-DCLD)
=================================================================

Version 0.9.12.645 Release Notes:
---------------------------------

New Features:

1) Application Rebranding
in this release the Digital Control Library Designer SDK got rebranded as MPLAB® PowerSmart™ SDK for Microchip dsPIC33® Digital Signal Controllers.
The known Digital Control Library Designer is a sub-component of the MPLAB® PowerSmart™ Software Development Kit eco-system. As the development of the SDK moves forward the branding got required to support the future release of additional SDK sub-components.


Optimizations:

1) Bode Plot Export Frequency Range
The frequency range used by PS-DCLD to generate output data to be imported in or being merged with data from third party tools appeared to create conflicts.
The PS-DCLD frequency range of exported data stopped short one frequency step below the given end of the frequency range while most tools seem to include this point.
Hence, the frequency range of PS-DCLD now also includes the stop-frequency point as last frequency entry in the data table.

2) Bode Plot Phase Unwrapping
PS-DCLD unwrapped the phase graph between PI and -PI only. However, across a wider frequency range and when going around the unit cycle multiple times, the phase wraps alternating between PI and -PI and PI/2 and -PI/2. Thus the 90° steps of the phase did not get unwrapped properly.
Phase unwrapping might still fail far above the Nyquist-Shannon limit and at lower graph resolution (number of data points). If a proper unwrapping of the phase is required, make sure the data resolution is high enough.

Bugfixes:

1) s-Domain Reference Transfer Function in Bode Plot

Rating: Low

By default, the s-domain reference transfer function is not shown in the Bode plot until users enable this feature. At startup, however, the legend still showed the s-domain data series while the chart did not.
By enforcing an update of the legend at startup after settings have been loaded fixes the problem.

2) Broken Update link in Controller Configuration Code Example

Rating: Low

The controller configuration code example output on the right side of the main window shows a bar with an update link, when controller types or scaling modes have changed, and no new code was automatically generated yet. The link for triggering this code update on this bar (temporarily visible only) was broken and no code was generated.

3) File Export Settings Test in Configuration Dialog

Rating: Low

When PS-DCLD project configuration and file output were both identical with the MPLAB X project root directory, the Export File settings wrongly showed that the directory could not be found.
Neither file export settings nor the file export itself was affected.

Change Notes - Impact on Existing Projects:

1)  API Changes
In this release version the API did not change, and existing user code will not be affected.

2)  Execution of Generated Code / Timing
(none)



============================================
(c) 2020, Microchip Technology Inc.
