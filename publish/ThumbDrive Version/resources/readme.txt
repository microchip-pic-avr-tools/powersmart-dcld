Digital Control Library Designer SDK (DCLD) for Microchip dsPIC33®
==================================================================
Version 0.9.8.104 Release Notes:
-------------------------------

New Features:

1) Control Output Mirroring on Multiple Output Ports

Description:
When configuring the controll loop to support multiple output ports (target and alternate target), a new option has been added allowing to write the most recent control output to both ports simultaneously.

Optimizations:

1) Accumulator merging in Dual Bit-Shift Mode

Description:
In Dual Bit-Shift Number Scaling Mode the order in which accumulator A and B get merged in the assembly code was changed to increase the effective number resolution of the result by pushing backwards normalization to the end of the computation. The effective increase in number resolution achieved by this change is equal to the number of bits shifted during backwards normalization of the B-term.


Bugfixes:

1) CONTROLLER_STATUS_FLAGS enumeration caused MPLAB X real time parser to occasionally break code highlighting.

***Description:***
The controller status flags of the cNPNZ16b_t data object are supported by enumerations for better readability. This enumeration was declared as CONTROLLER_STATUS_FLAGS_t, which seem to have confused the MPALB X real time parser, causing it to occasionally fail. As a result, MPLAB X showed unresolved items everywhere the enumeration was used.
Refactoring the enumeration type to CONTROLLER_STATUS_FLAGS_e solved the problem.

Status: Fixed

2) PWM Timing Update triggered by ADC Trigger placement in Dual ADC Trigger Mode

Description:
DCLD supports auto-positioning of up to two ADC triggers A & B simultaneously during one control loop execution. This directly supports the two ADC triggers of dsPIC33C devices.
If, however, the PWM configuraiton of the target device is set to PWM Auto-Update By ADC Trigger A, ADC trigger placement B set by DCLD was ignored.
Therefore the ADC trigger placement sequence in the generated code has been changed to ensure Trigger B gets updated together with Trigger A.

Status: Fixed

3) Converter Type of P-Term Controller Configuration did not get saved

Description:
When configuration a P-Term controller for plant measurements, users can specify the converter type to calculate nominal control outputs at user-specific operating conditions.
This converter type setting occasionally did not get saved and loaded properly and users had to re-adjust this setting again.

Status: Fixed

4) Configuration Corruption when switching between DCLD config files

Description:
When users switched from one open configuration to another, if occasionally happened, that feedbak settings of the previously opened configuraiton remained in memory, corrupting the newly opened file.

Status: Fixed



============================================
(c) 2020, Microchip Technology Inc.
