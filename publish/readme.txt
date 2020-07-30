Digital Control Library Designer SDK (DCLD) for Microchip dsPIC33®
==================================================================
Version 0.9.9.368 Release Notes:
--------------------------------

New Features:

1) NPNZ16b_t Data Structure Reference Separation in Assembly Include File

    Description:
    The DCLD code generator now features a fifth source code output window where the assembly declaration of the NPNZ16b_t data structure is stored. Users can select to generate the declaration directly into the assembly library file or out-source it to a new file called npnz16b.inc and include it in the Assembly file instead.
    Although this makes no difference for the generated assembly library, it allows users to include and access the NPNZ16b_t data structure declaration in custom assembly modules.

2) File Generation Full Path Display

    Description:
    When hoovering the mouse over one of the tabs of the code generator views, a tool-tip text will be displayed, showing the full path of the respective generated file.


Optimizations:

1) Default Include Path Selection

    Description:
    DCLD imports include directories declared in the specified MPLAB X project. Users can select one of the defined include paths in the Configuration dialog to optimize include path declarations in generated code.    

    Example:
    - Project files are located in '<my_project>/<source_files>/'. 
    - The DCLD generated files should be located in '<my_project>/<source_files>/<sub_folder1>/<sub_folder2>/'
    - The path './ <source_files>/' has been added to MPLAB X Project Properties -> XC16 -> Common Include Dirs.
    - In DCLD Configuration dialog this include path can be selected as Default Include Path
    - When generating code, the code generator will add the include path #include "./<sub_folder1>/<sub_folder2>/<my_file>" 
      instead of #include "./<source_files>/<sub_folder1>/<sub_folder2>/<my_file>"


2) Assembly Data Structure Label Names Cleanup

    Description:
    The labels used in assembly code referencing to their counterpart of the C-domain NPNZ16b_t data structure have deviated from related C-domain labels over time and have been synchronized to prevent confusions.


Bugfixes:

(none)



============================================
(c) 2020, Microchip Technology Inc.
