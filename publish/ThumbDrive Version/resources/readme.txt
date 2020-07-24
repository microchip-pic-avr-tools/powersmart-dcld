Digital Control Library Designer SDK (DCLD) for Microchip dsPIC33®
==================================================================
Version 0.9.9.312 Release Notes:
--------------------------------

Optimizations:

1) Unified Assembly Data Structure

    Description:
    The list of data structure address offsets representing the NPNZ16b_t data structure in Assembly has been unified, always exactly representing the data struture declared in C. Any previously tailored, generated versions have been eliminated, making the data structure more generic for user code modules.


Bugfixes:

1) MPLAB X Common Include Path Import Issue

    Description:
    Directory declarations in include paths specified in MPLAB X with a folder name of less than 2 characters length caused DCLD to throw an error message and the given include directory was ignored during code generation.

    Status: Fixed

2) DCLD Program Settings Storage Issue

    Description:
    Saving/restoring program settings such as Bode Graph scales or the file history did not get updated correctly or were corrupted when application closed.

    Status: Fixed

3) Number Conversion Conflict

    Description:
    When storing negative numbers in program settings, some negative numbers got accidentially rounded to 1. This only affected application parameters. User configurations (*.dcld files) were not affected.

    Status: Fixed



============================================
(c) 2020, Microchip Technology Inc.
