Digital Control Library Designer SDK (DCLD) for Microchip dsPIC33®
==================================================================
Version 0.9.9.452 Release Notes:
--------------------------------

New Features:

(none)

Optimizations:

1) File Manager Update

    Description:
    File export is now managed by a path sanity check class ensuring include paths and file locations are correct. In case of user configuration mismatches, error warning will be created guiding the user to the corrupted settings for quick resolution.

Bugfixes:

1) File Export may throw an exception instead of producing an error message if a file path has not been declared

    Description:
    If one of the file paths was not declared, an exception may be thrown instead of handling the exception and producing an error message with guidance on what happened and how to fix it.

    Status: fixed

2) When Pole or Zero Location was set = 0 Hz, code generation and Bode plot update locks

    Description:
    When a pole or zero location was set to = 0 by the user, the code generator and Bode plot would display the fequency as = 1 Hz. Any new change of the pole or zero location by the user would go without effect until the application is restarted.

    Status: fixed



============================================
(c) 2020, Microchip Technology Inc.
