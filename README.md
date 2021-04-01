![image](microchip.jpg) 

# MPLAB&reg; PowerSmart&trade; SDK for Microchip dsPIC33&reg; Digital Signal Controllers 

The MPLAB&reg; PowerSmart&trade; SDK is a Software Development Kit (SDK) comprised of multiple, individual stand-alone tools for system definition, system modeling, code generation, control system fine tuning and real-time debugging of fully digital control systems for Switched-Mode Power Supplies (SMPS) supporting Microchip Technology's dsPIC&reg; Digital Signal Controllers (DSC).

The major scope of this tool set is the rapid design of a digital power supply control stage rather than the power supply itself. This allows to simplify the design process to models based on interconnected transfer functions. These transfer functions are defined and configured in individual configuration windows. A transfer function can be based on generic Laplace-domain functions, being calculated at runtime or being defined by external data coming from network analyzer measurements or other third-party simulation tools such as MATLAB, SciLab, Simplis/SiMetrix, LTSpice, etc.

# PowerSmart&trade; Digital Control Library Designer (PS-DCLD)
### z-Domain Control Loop Configuration Tool & Code Generator Module Overview

The Digital Control Library Designer allows the graphical design of discrete compensation filters from the 1st to the 6th order (1P1Z to 6P6Z). Analysis results on timing, number accuracy and resolution and support of alternative fixed- and floating point number scaling options help to tune and optimize the final SMPS controller firmware for specific needs.

The output of this tool the generation of customized/tailored digital SMPS controller source code libraries with standardized API, taking away the need to manually write DSP-specific source code. 

The standardized API ensures seamless integration into the final firmware, supporting multiple, co-existing controllers in one firmware and seamless exchange between different controller types and scaling methods helping to solve typical performance vs. accuracy and feature tradeoffs.
For simplified use during code development, the Digital Control Library Designer can be called directly from the MPLAB&reg; X Integrated Development Environment (IDE) to make changes. 

(Please refer to the [PDF User Guide](./docs/181026n_dcld_beta_user_guide.pdf) for details)

### Core Features:
* **Supports z-Domain Compensation Filters from 1st to 6th Order**
* **Fixed-Point and Floating-Point DSP Library Support**
* **Graphic Loop Adjustment**
* **Transfer Function Export**
* **Built-In Number Resolution Analysis and Optimization**
* **Graphic Execution Timing Analysis**
* **ANSI C/DSP Assembly Code Generation**

### Special Features:
* **Advanced Control Options**  
PS-DCLD provides code generator options injecting code into the real-time high speed loop allowing advanced control algorithms manipulative access to the compensation filter computation as well as data provider sources to track and monitor internal processing data at runtime.

* **System Design Options**  
PS-DCLD offers alternative feedback loops enabling power supply plant measurements supporting power plant model verification and/or directly deriving essential, unknown plant transfer function information through bench tests using vector network analyzers.

* **MPLAB X Support**  
PS-DCLD was developed as control library generator for Microchip dsPIC33 product families. To allow the code generator derive project settings like C include directories and selected device part number, each controller project is tightly coupled to a user-specified MPLAB® X project. For most convenient use, DCLD can be opened from the MPLAB® X project manager context menu when the project file is included in the related MPLAB® X project.

* **Data export**  
Export of s-Domain and z-Domain Transfer Function (Bode Plot Data) copies the bode plot data table into the clipboard as tab-separated text table with column headers. This data can directly be pasted into external applications such as MS Excel.   


### Further Information:

Please visit the WIKI site of this repository for more information: [https://github.com/areiter128/DCLD/wiki](https://github.com/areiter128/DCLD/wiki)

### Download Software

Please visit the RELEASE websiteto download the latest release version: [https://github.com/areiter128/DCLD/releases](https://github.com/areiter128/DCLD/releases)


---
&copy; 2021 Microchip Technology Inc.

