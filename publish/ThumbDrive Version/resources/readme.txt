Digital Control Library Designer SDK (DCLD) for Microchip dsPIC33®
==================================================================
Version 0.9.9.403 Release Notes:
--------------------------------

New Features:

1) Independent Setting for Assembly and C Include Paths

    Description:
    Previous versions of DCLD only supported the selection of one C include directory for all generated files. With the introduciton of an additional Assembly include file, a second, individual selection of a user-defined Assembly Include Path was required. 
    This setting has been added to the MPLAB X Project Configuration dialog.

2) Include Path Override Option
    
    Description:
    DCLD allows the specification of user defined include paths for C and Assembly files. The include paths available are imported from the MPLAB X project. If users do not select any include files, the Makefile location becoes the include path reference for all #include and .include pre-compiler directives. 
    The new override feature is located in the MPLAB Project Configuration dialog and allows users to effectively disable the include path handling by always using the Makefile location an absolute refrence for all #include and .include pre-compiler directives generation.


Optimizations:

(none)


Bugfixes:

1) Compiler Error after Assembly Generation with Include File Reference

    Description:
    Previous versions of DCLD treated the assembly include file like a comon C header file. Hence, when a user-defined C include path was specified, the include declaration generated in the assembly library file also referenced to the C header include directory. However, the compiler does not consider this include directory when assembing the Assembly source file and the buld process failed.
    With the introduction of a user-defined Assembly include path (see above), this issue has been fixed.

    Status: fixed



============================================
(c) 2020, Microchip Technology Inc.
