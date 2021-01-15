MPLAB PowerSmart™ SDK for Microchip dsPIC33® Digital Signal Controllers
Digital Control Library Designer (PS-DCLD)
=======================================================================

Version 0.9.12.657 Release Notes:
---------------------------------

New Features:

1) Extension Function Call Hooks

Four new optional hooks for extension functions have been added to the Advanced Tab of the GUI. These hooks add function pointers at 
specific locations within the NPNZ controller assembly routine allowing to tie in proprietary user code into the NPNZ controller 
execution. The new hooks have been placed 

    - Beginning of the 'Update' routine, after context has been saved and the Enable bit has been passed
    - After the source address has been read and offset and polarity compensation has been performed but before the error is calculated
    - After the compensation filter computation has completed and before anti-windup clamping is applied
    - After anti-windup clamping has been applied and before the most recent control output is written to the target address
    - At the end of active 'Update' routine before context is restored and included in the 'Enable' bypass (funciton will not be called when controller is disabled)
    - At the very end of the 'Update' routine after the Enable bypass, This is the replacement of the previous Cascade Function Call feature

Each user function can be called as 'viod' function without parameter, with one integer parameter and may or may not return a value.
These extension function hooks are automatically added to the main control Update routine as well as the PTerm update routine.

2) Increased User Parameter Data Space

With increased number of extension functions the need arose to also increase the user data space within the NPNZ data structure. 
Now this data space supports up to eight unassigned word variables users can use as data space for proprietary user functions.


Optimizations:

(none)

Bugfixes:

1) Number Scaling Limit

Rating: Medium
Status: Fixed

At very low pole and zero locations absolute values of coefficients tend to get very small. 
In extreme cases number scaling could exceed the specified bit-width of the fractional, 
resulting in a math trap error when executed on dsPIC33 working registers. Hence, in this 
version the maximum number scaling has been limited to the specified bit-width of the fractional.

2) Assembler Include Path Format

Rating: Medium
Status: Fixed

On some Windows operating systems the file path conversion from Windows format using a backslash "\" 
to separate file system directory levels to Unix-style path format using a slash "/" did get corrupted
leaving backslash formats in place causing builds to fail on Linux or Unix operating systems.

3) Assembler Code Comments 

Rating: Low
Status: Fixed

Operant array index comment in the first multiply of a A- or B-filter caclculation using floating point 
number scaling was broken, leaving %INDEX% tokens in the comment instead of showing the correct index number.




Important Change Notes - (Impact on Existing Projects):

1)  API Changes

Section 'CascadeFunction' of NPNZ16b_t data object has been changed to 'ExtensionHooks' and now supports 
up to six individual user function call hooks. The previous data fields 'ptrCascadeFunction' and 'CascadeFunctionParam'
have been replaced by 'ptrExtHookEndFunction' and 'ExtHookEndFunctionParam'.

2)  Execution of Generated Code / Timing
(none)



============================================
(c) 2021, Microchip Technology Inc.
