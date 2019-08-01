using System;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace dcld
{
    public class clsCCodeGenerator
    {

        private bool _AddCHeaderIncludePath = false;
        internal bool AddCHeaderIncludePath
        {
            get { return (_AddCHeaderIncludePath); }
            set { _AddCHeaderIncludePath = value; return; }
        }

        private string _CHeaderIncludePath = "";
        internal string CHeaderIncludePath
        {
            get { return (_CHeaderIncludePath); }
            set { _CHeaderIncludePath = value; return; }
        }

        private bool _AddLibHeaderIncludePath = false;
        internal bool AddLibHeaderIncludePath
        {
            get { return (_AddLibHeaderIncludePath); }
            set { _AddLibHeaderIncludePath = value; return; }
        }

        private string _LibHeaderIncludePath = "";
        internal string LibHeaderIncludePath
        {
            get { return (_LibHeaderIncludePath); }
            set { _LibHeaderIncludePath = value; return; }
        }

        private string _FileNamePattern = "";
        internal string FileNamePattern
        {
            get { return (_FileNamePattern); }
            set { _FileNamePattern = value; return; }
        }

        private string _CompTypeName = "";
        internal string CompTypeName
        {
            get { return (_CompTypeName); }
            set { _CompTypeName = value; return; }
        }

        private string _ScalingMethodName = "";
        internal string ScalingMethodName
        {
            get { return (_ScalingMethodName); }
            set { _ScalingMethodName = value; return; }
        }

        private string _PreFix = "";
        internal string PreFix
        {
            get { return (_PreFix); }
            set { _PreFix = value; return; }
        }

        private string _PostFix = "";
        internal string PostFix
        {
            get { return (_PostFix); }
            set { _PostFix = value; return; }
        }

        private int _PreShift = 0;
        internal int PreShift
        {
            get { return (_PreShift); }
            set { _PreShift = value; return; }
        }

        /*  */
        public StringBuilder BuildCode(clsCompensatorNPNZ compFilter, bool IsHeader, bool IsGenericLibraryHeader=false)
        {
            string hdrdef_pattern = "";
            StringBuilder strBuffer = new StringBuilder();

            //file header top-line with generator version information
            strBuffer.Append(
                        "/* ***************************************************************************************\r\n" +
                        " * " + Application.ProductName + " Version " + Application.ProductVersion.ToString() + "." + "\r\n"
                        );


            if (IsHeader)   // a header file needs to be generated
            {

                if (IsGenericLibraryHeader) // the header file to be generated is the generic NPNZ data structure header
                {
                    //file header top-line with generator version information
                    strBuffer.Append(
                                " * ***************************************************************************************\r\n" +
                                " * Generic library header for z-domain compensation filter assembly functions\r\n" +
                                " * ***************************************************************************************/\r\n" +
                                "\r\n");

                    // header 
                    hdrdef_pattern = "__SPECIAL_FUNCTION_LAYER_LIB_NPNZ_H__";
                    strBuffer.Append("#ifndef " + hdrdef_pattern + "\r\n" + "#define " + hdrdef_pattern + "\r\n\r\n");

                    // Include section
                    strBuffer.Append("#include <xc.h>\r\n" + 
                                     "#include <dsp.h>\r\n" + 
                                     "#include <stdint.h>\r\n" + 
                                     "\r\n");

                    // Generate the contorl library contents
                    strBuffer.Append(BuildCLibHeader(compFilter));

                    // add header file end 
                    strBuffer.Append("#endif\t// end of " + hdrdef_pattern + " header file section\r\n");

    
                }
                else
                {
                    //file c-header top-line with generator version information
                    strBuffer.Append(
                                " * ***************************************************************************************\r\n" +
                                " * " + compFilter.FilterOrder + "p" + compFilter.FilterOrder + "z compensation filter coefficients derived for following operating conditions:\r\n" +
                                " * ***************************************************************************************\r\n" +
                                " *\r\n" +
                                " * \tController Type:\t" + _CompTypeName + "\r\n" +
                                " * \tSampling Frequency:\t" + compFilter.SamplingFrequency.ToString() + " Hz \r\n" +
                                " * \tFixed Point Format:\t" + compFilter.QFormat + "\r\n" +
                                " * \tScaling Mode:\t\t" + _ScalingMethodName + "\r\n" +
                                " * \tInput Gain:\t\t\t" + compFilter.InputGain.ToString() + "\r\n" +
                                " * \r\n" +
                                " * ***************************************************************************************/\r\n" +
                                "\r\n");

                    // Include section
                    // header 
                    hdrdef_pattern = "__SPECIAL_FUNCTION_LAYER_" + _PreFix.ToUpper() + "H__";

                    strBuffer.Append("#ifndef " + hdrdef_pattern + "\r\n" +
                                     "#define " + hdrdef_pattern + "\r\n" +
                                     "\r\n");

                    strBuffer.Append("#include <xc.h>" + "\r\n" +
                                     "#include <dsp.h>" + "\r\n" +
                                     "#include <stdint.h>" + "\r\n" +
                                     "\r\n");

                    strBuffer.Append("#include " + (char)34 + _LibHeaderIncludePath + "npnz16b.h" + (char)34 + "\r\n" +
                                    "\r\n");

                    strBuffer.Append(
                                "/* ***************************************************************************************\r\n" +
                                " * Data Arrays:\r\n" +
                                " * The cNPNZ_t data structure contains a pointer to derived coefficients in X-space and" + "\r\n" +
                                " * other pointers to controller and error history in Y-space.\r\n" +
                                " * This header file holds public declarations for variables and arrays defined in \r\n" + 
                                " * " + _FileNamePattern.ToLower() + ".c\r\n" +
                                " * \r\n" +
                                " * Type definition for A- and B- coefficient arrays and error- and control-history arrays, \r\n" +
                                " * which are aligned in memory for optimized addressing during DSP computations.           \r\n" +
                                " * These data structures need to be placed in specific memory locations to allow direct    \r\n" +
                                " * X/Y-access from the DSP. (coefficients in x-space, histories in y-space)                \r\n" +
                                " * ***************************************************************************************/\r\n" +
                                "\r\n");


                    // Generate the contorl header contents
                    strBuffer.Append(BuildHeader(compFilter));
                    
                    strBuffer.Append("#endif" + "\t// end of " + hdrdef_pattern + " header file section\r\n");

                }


            }
            else 
            {


                //file header top-line with generator version information
                strBuffer.Append(
                            " * ***************************************************************************************\r\n" +
                            " * " + compFilter.FilterOrder + "p" + compFilter.FilterOrder + "z compensation filter coefficients derived for following operating conditions:\r\n" +
                            " * ***************************************************************************************\r\n" +
                            " *\r\n" +
                            " * \tController Type:\t" + _CompTypeName + "\r\n" +
                            " * \tSampling Frequency:\t" + compFilter.SamplingFrequency.ToString() + " Hz \r\n" +
                            " * \tFixed Point Format:\t" + compFilter.QFormat + "\r\n" +
                            " * \tScaling Mode:\t\t" + _ScalingMethodName + "\r\n" +
                            " * \tInput Gain:\t\t\t" + compFilter.InputGain.ToString() + "\r\n" +
                            " * \r\n" +
                            " * ***************************************************************************************/\r\n" +
                            "\r\n");

                strBuffer.Append("#include " + (char)34 + _CHeaderIncludePath + _FileNamePattern.ToLower() + ".h" + (char)34 + "\r\n\r\n");

                strBuffer.Append(
                            "/* ***************************************************************************************\r\n" +
                            " * Data Arrays:\r\n" +
                            " * The cNPNZ_t data structure contains a pointer to derived coefficients in X-space and" + "\r\n" +
                            " * other pointers to controller and error history in Y-space.\r\n" +
                            " * This source file declares the default parameters of the z-domain compensation filter.\r\n" +
                            " * These declarations are made publicly accessible through defines in " + _FileNamePattern.ToLower() + ".h\r\n" + 
                            " * ***************************************************************************************/\r\n" +
                            "\r\n"
                            );

                strBuffer.Append(BuildSource(compFilter));
                strBuffer.Append("\r\n");
            }
        
            return(strBuffer);
        
        }

        private StringBuilder BuildCLibHeader(clsCompensatorNPNZ compFilter)
        {
            StringBuilder strBuffer = new StringBuilder();
            string _str_coeff_datatype = "";

            try
            {

                strBuffer.Append(
                    "/* Status flags (Single Bit) */\r\n" + 
                    "#define NPNZ16_STATUS_LSAT_SET		1\r\n" +
                    "#define NPNZ16_STATUS_LSAT_RESET	0\r\n" +
                    "#define NPNZ16_STATUS_USAT_SET		1\r\n" +
                    "#define NPNZ16_STATUS_USAT_RESET	0\r\n" +
                    "#define NPNZ16_STATUS_ENABLED		1\r\n" +
                    "#define NPNZ16_STATUS_DISABLED		0\r\n" +
                    "\r\n"
                );

                strBuffer.Append(
                    "/* Status flags (bit-field) */\r\n" + 
                    "typedef enum {\r\n" + 
                    "    CONTROLLER_STATUS_CLEAR = 0b0000000000000000,\r\n" + 
                    "    CONTROLLER_STATUS_SATUATION_MSK = 0b0000000000000011,\r\n" + 
                    "    CONTROLLER_STATUS_LSAT_ON = 0b0000000000000001,\r\n" + 
                    "    CONTROLLER_STATUS_LSAT_OFF = 0b0000000000000000,\r\n" + 
                    "    CONTROLLER_STATUS_USAT_ON = 0b0000000000000010,\r\n" + 
                    "    CONTROLLER_STATUS_USAT_OFF = 0b0000000000000000,\r\n" + 
                    "    CONTROLLER_STATUS_ENABLE_OFF = 0b0000000000000000,\r\n" + 
                    "    CONTROLLER_STATUS_ENABLE_ON = 0b1000000000000000\r\n" + 
                    "} CONTROLLER_STATUS_FLAGS_t;\r\n" + 
                    "\r\n"
                    );

                strBuffer.Append(
                    "typedef struct {\r\n" + 
                    "    volatile unsigned flt_clamp_min : 1; // Bit 0: control loop is clamped at minimum output level\r\n" + 
                    "    volatile unsigned flt_clamp_max : 1; // Bit 1: control loop is clamped at maximum output level\r\n" + 
                    "    volatile unsigned : 1; // Bit 2: reserved\r\n" + 
                    "    volatile unsigned : 1; // Bit 3: reserved\r\n" + 
                    "    volatile unsigned : 1; // Bit 4: reserved\r\n" + 
                    "    volatile unsigned : 1; // Bit 5: reserved\r\n" + 
                    "    volatile unsigned : 1; // Bit 6: reserved\r\n" + 
                    "    volatile unsigned : 1; // Bit 7: reserved\r\n" + 
                    "    volatile unsigned : 1; // Bit 8: reserved\r\n" + 
                    "    volatile unsigned : 1; // Bit 9: reserved\r\n" + 
                    "    volatile unsigned : 1; // Bit 11: reserved\r\n" + 
                    "    volatile unsigned : 1; // Bit 11: reserved\r\n" + 
                    "    volatile unsigned : 1; // Bit 12: reserved\r\n" + 
                    "    volatile unsigned : 1; // Bit 13: reserved\r\n" + 
                    "    volatile unsigned : 1; // Bit 14: reserved\r\n" + 
                    "    volatile unsigned enable : 1; // Bit 15: enables/disables control loop execution\r\n" + 
                    "} __attribute__((packed))CONTROLLER_STATUS_BIT_FIELD_t;\r\n" + 
                    "\r\n"
                );

                strBuffer.Append(
                    "/* status items data structure to monitor a power converter */\r\n" +
                    "typedef union {\r\n" +
                    "    volatile CONTROLLER_STATUS_BIT_FIELD_t flags;\r\n" +
                    "    volatile uint16_t value;\r\n" +
                    "} __attribute__((packed))CONTROLLER_STATUS_t;\r\n" +
                    "\r\n"
                );

                if (compFilter.ScalingMethod == clsCompensatorNPNZ.dcldScalingMethod.DCLD_SCLMOD_DBLSCL_FLOAT) { _str_coeff_datatype = "int32_t"; }
                else { _str_coeff_datatype = "fractional"; }

                strBuffer.Append(
                    "typedef struct {\r\n" +
                    "    // External control and monitoring\r\n" +
                    "    volatile CONTROLLER_STATUS_t status;" + " // Control Loop Status flags" + "\r\n" +
                    "\r\n");

                strBuffer.Append(
                    "    // Input/Output to controller\r\n" +
                    "    volatile uint16_t* ptrSource; // Pointer to source register or variable where the input value is read from (e.g. ADCBUF0)\r\n" +
                    "    volatile uint16_t* ptrTarget; // Pointer to target register or variable where the control output is written to (e.g. PCD1)\r\n" +
                    "    volatile uint16_t* ptrControlReference; // Pointer to global variable of input register holding the controller reference value (e.g. uint16_t my_ref)" + "\r\n" +
                    "\r\n");

                strBuffer.Append(
                    "    // Filter coefficients and input/output histories\r\n" +
                    "    volatile " + _str_coeff_datatype + "* ptrACoefficients; // Pointer to A coefficients located in X-space \r\n" +
                    "    volatile " + _str_coeff_datatype + "* ptrBCoefficients; // Pointer to B coefficients located in X-space \r\n" +
                    "    volatile fractional* ptrControlHistory; // Pointer to n delay-line samples located in Y-space with first sample being the most recent \r\n" +
                    "    volatile fractional* ptrErrorHistory; // Pointer to n+1 delay-line samples located in Y-space with first sample being the most recent \r\n" +
                    "\r\n"
                    );

                strBuffer.Append(
                    "    // Array size information" + "\r\n" +
                    "    volatile uint16_t ACoefficientsArraySize;" + " // Size of the A coefficients array in X-space" + "\r\n" +
                    "    volatile uint16_t BCoefficientsArraySize; // Size of the B coefficients array in X-space" + "\r\n" +
                    "    volatile uint16_t ControlHistoryArraySize; // Size of the control history array in Y-space" + "\r\n" +
                    "    volatile uint16_t ErrorHistoryArraySize; // Size of the error history array in Y-space" + "\r\n" +
                    "\r\n");

                strBuffer.Append(
                    "    // Feedback scaling Input/Output Normalization\r\n" +
                    "    volatile int16_t normPreShift; // Normalization of ADC-resolution to Q15 (R/W)\r\n" +
                    "    volatile int16_t normPostShiftA; // Normalization of A-term control output to Q15 (R/W)\r\n" +
                    "    volatile int16_t normPostShiftB; // Normalization of B-term control output to Q15 (R/W)\r\n" +
                    "    volatile int16_t normPostScaler; // Control output normalization factor (Q15) (R/W)\r\n" +
                    "\r\n"
                    );

                strBuffer.Append(
                    "    // Feedback conditioning\r\n" +
                    "    volatile int16_t InputOffset; // Control input source offset value (R/W)\r\n" +
                    "\r\n"
                    );

                strBuffer.Append(
                    "    // System clamping/Anti-windup\r\n" +
                    "    volatile int16_t MinOutput; // Minimum output value used for clamping (R/W)\r\n" +
                    "    volatile int16_t MaxOutput; // Maximum output value used for clamping (R/W)\r\n" +
                        "\r\n" +
                    "    // Voltage/Average Current Mode Control Trigger handling\r\n" +
                    "    volatile uint16_t* ptrADCTriggerRegister; // Pointer to ADC trigger register (e.g. TRIG1)\r\n" +
                    "    volatile uint16_t ADCTriggerOffset; // ADC trigger offset to compensate propagation delays \r\n" +
                    "    \r\n" +
                    "} __attribute__((packed))cNPNZ16b_t;\r\n" +
                    "\r\n"
                    );

                strBuffer.Append("\r\n/* ***************************************************************************************/\r\n");

            }
            catch
            {
                strBuffer.Append("\r\n\r\n#error [Invalid values detected during body generation]\r\n\r\n");
            }

            return (strBuffer);

        }

        private StringBuilder BuildHeader(clsCompensatorNPNZ compFilter)
        {

            StringBuilder strBuffer = new StringBuilder();
            string _str_coeff_datatype = "";

            try
            {

                if (compFilter.ScalingMethod == clsCompensatorNPNZ.dcldScalingMethod.DCLD_SCLMOD_DBLSCL_FLOAT) { _str_coeff_datatype = "int32_t"; }
                else { _str_coeff_datatype = "fractional"; }

                strBuffer.Append(
                        "\t" + "typedef struct" + "\r\n" +
                        "\t" + "{" + "\r\n" +
                        "\t" + "	" + "volatile " + _str_coeff_datatype + " ACoefficients[" + (compFilter.FilterOrder).ToString() + "]; // A-Coefficients" + "\r\n" +
                        "\t" + "	" + "volatile " + _str_coeff_datatype + " BCoefficients[" + (compFilter.FilterOrder + 1).ToString() + "]; // B-Coefficients" + "\r\n" +
                        "\t" + "} __attribute__((packed))" + _PreFix.ToUpper() + "CONTROL_LOOP_COEFFICIENTS_t;" + "\r\n" +
                        "\r\n" +
                        "\t" + "typedef struct" + "\r\n" +
                        "\t" + "{" + "\r\n" +
                        "\t" + "	" + "volatile " + "fractional ControlHistory[" + (compFilter.FilterOrder).ToString() + "];  // Control History" + "\r\n" +
                        "\t" + "	" + "volatile " + "fractional ErrorHistory[" + (compFilter.FilterOrder + 1).ToString() + "];    // Error History" + "\r\n" +
                        "\t" + "} __attribute__((packed))" + _PreFix.ToUpper() + "CONTROL_LOOP_HISTORIES_t;" + "\r\n" +
                        "\r\n"
                        );

                strBuffer.Append( "\r\n");

                strBuffer.Append("\t" + "extern volatile cNPNZ16b_t " + FileNamePattern + ";" + " // user-controller data object" +
                    "\r\n");

                strBuffer.Append("\r\n/* ***************************************************************************************/\r\n" +
                    "\r\n");

                strBuffer.Append(
                    "// Function call prototypes for initialization routines and control loops" +
                    "\r\n");

                strBuffer.Append("\r\n");

                strBuffer.Append(
                    "extern uint16_t " + FileNamePattern + "_Init(void);" + 
                    " " + "// Loads default coefficients into " + compFilter.FilterOrder + "P" + compFilter.FilterOrder + "Z" + " controller and resets histories to zero" + 
                    "\r\n");

                strBuffer.Append("\r\n");

                strBuffer.Append(
                    "extern void " + FileNamePattern + "_Reset(" + " // Resets the " + compFilter.FilterOrder + "P" + compFilter.FilterOrder + "Z" + " controller histories" + "\r\n" +
                    "\t" + "volatile cNPNZ16b_t* controller" + " // Pointer to nPnZ data structure" + "\r\n" +
                    "\t" + ");" +
                    "\r\n");

                strBuffer.Append("\r\n");

                strBuffer.Append(
                    "extern void " + FileNamePattern + "_Precharge(" + " // Pre-charges histories of the " + compFilter.FilterOrder + "P" + compFilter.FilterOrder + "Z" + " with defined steady-state data" + "\r\n" +
                    "\t" + "volatile cNPNZ16b_t* controller," + " // Pointer to nPnZ data structure" + "\r\n" +
                    "\t" + "volatile uint16_t ctrl_input," + " // user-defined, constant error history value" + "\r\n" +
                    "\t" + "volatile uint16_t ctrl_output" + " // user-defined, constant control output history value" + "\r\n" +
                    "\t" + ");" +
                    "\r\n");

                strBuffer.Append("\r\n");

                strBuffer.Append(
                    "extern void " + FileNamePattern + "_Update(" + " // Calls the " + compFilter.FilterOrder + "P" + compFilter.FilterOrder + "Z" + " controller" + "\r\n" +
                    "\t" + "volatile cNPNZ16b_t* controller" + " // Pointer to nPnZ data structure" + "\r\n" +
                    "\t" + ");" +
                    "\r\n");


                strBuffer.Append("\r\n");

            }
            catch
            {
                strBuffer.Append("\r\n\r\n#error [Invalid values detected during body generation]\r\n\r\n");
            }

            return(strBuffer);

        }

        private StringBuilder BuildSource(clsCompensatorNPNZ compFilter)
        {
            int i = 0;
            StringBuilder strBuffer = new StringBuilder();
            string _str_coeff_datatype = "";

            try
            {

                if (compFilter.ScalingMethod == clsCompensatorNPNZ.dcldScalingMethod.DCLD_SCLMOD_DBLSCL_FLOAT) { _str_coeff_datatype = "int32_t"; }
                else { _str_coeff_datatype = "fractional"; }

                strBuffer.Append(
                    "\t" + "volatile " + _PreFix.ToUpper() + "CONTROL_LOOP_COEFFICIENTS_t" + " " + "__attribute__((space(xmemory), near))" + " " + _PreFix + "coefficients; // A/B-Coefficients \r\n" +
                    "\t" + "volatile " + "uint16_t " + _PreFix + "ACoefficients_size = (sizeof(" + _PreFix + "coefficients.ACoefficients)/sizeof(" + _PreFix + "coefficients.ACoefficients[0])); // A-coefficient array size" + "\r\n" +
                    "\t" + "volatile " + "uint16_t " + _PreFix + "BCoefficients_size = (sizeof(" + _PreFix + "coefficients.BCoefficients)/sizeof(" + _PreFix + "coefficients.BCoefficients[0])); // B-coefficient array size" + "\r\n"
                    );
                strBuffer.Append("\r\n");
                strBuffer.Append(
                    "\t" + "volatile " + _PreFix.ToUpper() + "CONTROL_LOOP_HISTORIES_t" + " " + "__attribute__((space(ymemory), far))" + " " + _PreFix + "histories; // Control/Error Histories \r\n" +
                    "\t" + "volatile " + "uint16_t " + _PreFix + "ControlHistory_size = (sizeof(" + _PreFix + "histories.ControlHistory)/sizeof(" + _PreFix + "histories.ControlHistory[0])); // Control history array size" + "\r\n" +
                    "\t" + "volatile " + "uint16_t " + _PreFix + "ErrorHistory_size = (sizeof(" + _PreFix + "histories.ErrorHistory)/sizeof(" + _PreFix + "histories.ErrorHistory[0])); // Error history array size" + "\r\n"
                    );
                strBuffer.Append("\r\n");


                strBuffer.Append(
                            "/* ***************************************************************************************\r\n" +
                            " * \tPole&Zero Placement:\r\n" +
                            " * ***************************************************************************************\r\n" +
                            " *\r\n"
                            );

                if ((compFilter.FilterOrder >= 1) && (compFilter.Pole[0] != null)) strBuffer.Append(" * \tfP0:\t" + compFilter.Pole[0].Frequency.ToString() + " Hz \r\n");

                for (i = 1; i < compFilter.FilterOrder; i++)
                {
                    strBuffer.Append(
                                " * \tfP" + i.ToString() + ":\t" + compFilter.Pole[i].Frequency.ToString() + " Hz \r\n" +
                                " * \tfZ" + i.ToString() + ":\t" + compFilter.Zero[i].Frequency.ToString() + " Hz \r\n"
                                );
                }

                strBuffer.Append(
                            " *\r\n" +
                            " * ***************************************************************************************\r\n" +
                            " * \tFilter Coefficients and Parameters:\r\n" +
                            " * ***************************************************************************************/\r\n" +
                            "\r\n"
                            );

                strBuffer.Append(
                    "\t" + "volatile " + _str_coeff_datatype + " " + _PreFix + "ACoefficients [" + (compFilter.FilterOrder).ToString() + "] = " + "\r\n" +
                    "\t" + "{" + "\r\n");

                for (i = 1; i <= compFilter.FilterOrder; i++)
                {
                    strBuffer.Append("\t" + "\t" + "0x" + compFilter.CoeffA[i].Hex);
                    if (i < compFilter.FilterOrder) strBuffer.Append(",");
                    strBuffer.Append("\t// Coefficient A" + i.ToString() + " will be multiplied with controller output u(n-" + i.ToString() + ")\r\n");
                }

                strBuffer.Append("\t" + "};" + "\r\n" + "\r\n");

                strBuffer.Append(
                    "\t" + "volatile " + _str_coeff_datatype + " " + _PreFix + "BCoefficients [" + (compFilter.FilterOrder + 1).ToString() + "] = " + "\r\n" +
                    "\t" + "{" + "\r\n");

                for (i = 0; i <= compFilter.FilterOrder; i++)
                {
                    strBuffer.Append("\t" + "\t" + "0x" + compFilter.CoeffB[i].Hex);
                    if(i<compFilter.FilterOrder) strBuffer.Append(",");
                    strBuffer.Append("\t// Coefficient B" + i.ToString());

                    if (i == 0) { strBuffer.Append(" will be multiplied with error input e(n)\r\n"); }
                    else { strBuffer.Append(" will be multiplied with error input e(n-" + i.ToString() + ")\r\n"); }
                }
                strBuffer.Append("\t" + "};" + "\r\n" + "\r\n");

                strBuffer.Append(
                            "\r\n" +
                            "\t" + "volatile " + "int16_t " + _PreFix + "pre_scaler = " + _PreShift.ToString() + ";" + "\r\n"
                            );

                switch(compFilter.ScalingMethod)
                {
                    case clsCompensatorNPNZ.dcldScalingMethod.DCLD_SCLMOD_SINGLE_BIT_SHIFT:
                        strBuffer.Append("\t" + "volatile " + "int16_t " + _PreFix + "post_shift_A = " + compFilter.CoeffA[1].QScaler.ToString() + ";" + "\r\n");
                        strBuffer.Append("\t" + "volatile " + "int16_t " + _PreFix + "post_shift_B = " + "0" + ";" + "\r\n");
                        strBuffer.Append("\t" + "volatile " + "fractional " + _PreFix + "post_scaler = 0x" + "0000" + ";" + "\r\n");
                        break;

                    case clsCompensatorNPNZ.dcldScalingMethod.DCLD_SCLMOD_DUAL_BIT_SHIFT:
                        strBuffer.Append("\t" + "volatile " + "int16_t " + _PreFix + "post_shift_A = " + compFilter.CoeffA[1].QScaler.ToString() + ";" + "\r\n");
                        strBuffer.Append("\t" + "volatile " + "int16_t " + _PreFix + "post_shift_B = " + compFilter.CoeffB[0].QScaler.ToString() + ";" + "\r\n");
                        strBuffer.Append("\t" + "volatile " + "fractional " + _PreFix + "post_scaler = 0x" + "0000" + ";" + "\r\n");
                        break;

                    case clsCompensatorNPNZ.dcldScalingMethod.DCLD_SCLMOD_OUTPUT_SCALING_FACTOR:
                        strBuffer.Append("\t" + "volatile " + "int16_t " + _PreFix + "post_shift_A = " + compFilter.OutputScalingFactor.QScaler.ToString() + ";" + "\r\n");
                        strBuffer.Append("\t" + "volatile " + "int16_t " + _PreFix + "post_shift_B = " + "0" + ";" + "\r\n");
                        strBuffer.Append("\t" + "volatile " + "fractional " + _PreFix + "post_scaler = 0x" + compFilter.OutputScalingFactor.Hex.ToString() + ";" + "\r\n");
                        break;
                    
                    case clsCompensatorNPNZ.dcldScalingMethod.DCLD_SCLMOD_DBLSCL_FLOAT:
                        strBuffer.Append("\t" + "volatile " + "int16_t " + _PreFix + "post_shift_A = " + "0" + ";" + "\r\n");
                        strBuffer.Append("\t" + "volatile " + "int16_t " + _PreFix + "post_shift_B = " + "0" + ";" + "\r\n");
                        strBuffer.Append("\t" + "volatile " + "fractional " + _PreFix + "post_scaler = 0x" + "0000" + ";" + "\r\n");
                        break;

                    default:
                        break;
                }

                strBuffer.Append("\r\n");

                strBuffer.Append("\t" + "volatile " + "cNPNZ16b_t " + FileNamePattern + ";" + " // user-controller data object" +
                    "\r\n");

                strBuffer.Append("\r\n/* ***************************************************************************************/\r\n\r\n");

                strBuffer.Append(
                      "uint16_t " + FileNamePattern + "_Init(void)" + "\r\n" +
                      "{" + "\r\n" +
                      "\t" + "volatile " + "uint16_t i = 0;" + "\r\n" +
                      "\r\n");

                strBuffer.Append(
                    "\t" + "// Initialize controller data structure at runtime with pre-defined default values" + "\r\n" +
                    "\t" + FileNamePattern + ".status.flags = CONTROLLER_STATUS_CLEAR;  // clear all status flag bits (will turn off execution))" + "\r\n" +
                    "\r\n" +
                    "\t" + FileNamePattern + ".ptrACoefficients = &" + _PreFix + "coefficients.ACoefficients[0]; // initialize pointer to A-coefficients array" + "\r\n" +
                    "\t" + FileNamePattern + ".ptrBCoefficients = &" + _PreFix + "coefficients.BCoefficients[0]; // initialize pointer to B-coefficients array" + "\r\n" +
                    "\t" + FileNamePattern + ".ptrControlHistory = &" + _PreFix + "histories.ControlHistory[0]; // initialize pointer to control history array" + "\r\n" +
                    "\t" + FileNamePattern + ".ptrErrorHistory = &" + _PreFix + "histories.ErrorHistory[0]; // initialize pointer to error history array" + "\r\n" +
                    "\t" + FileNamePattern + ".normPostShiftA = " + _PreFix + "post_shift_A; // initialize A-coefficients/single bit-shift scaler" + "\r\n" +
                    "\t" + FileNamePattern + ".normPostShiftB = " + _PreFix + "post_shift_B; // initialize B-coefficients/dual/post scale factor bit-shift scaler" + "\r\n" +
                    "\t" + FileNamePattern + ".normPostScaler = " + _PreFix + "post_scaler; // initialize control output value normalization scaling factor" + "\r\n" +
                    "\t" + FileNamePattern + ".normPreShift = " + _PreFix + "pre_scaler; // initialize A-coefficients/single bit-shift scaler" + "\r\n" +
                    "\r\n" +
                    "\t" + FileNamePattern + ".ACoefficientsArraySize = " + _PreFix + "ACoefficients_size; // initialize A-coefficients array size" + "\r\n" +
                    "\t" + FileNamePattern + ".BCoefficientsArraySize = " + _PreFix + "BCoefficients_size; // initialize A-coefficients array size" + "\r\n" +
                    "\t" + FileNamePattern + ".ControlHistoryArraySize = " + _PreFix + "ControlHistory_size; // initialize control history array size" + "\r\n" +
                    "\t" + FileNamePattern + ".ErrorHistoryArraySize = " + _PreFix + "ErrorHistory_size; // initialize error history array size" + "\r\n" +
                    "\r\n");

                strBuffer.Append(
                      "\r\n" +
                      "\t" + "// Load default set of A-coefficients from user RAM into X-Space controller A-array" + "\r\n" +
                      "\t" + "for(i=0; i<" + FileNamePattern + ".ACoefficientsArraySize; i++)" + "\r\n" +
                      "\t" + "{" + "\r\n" +
                      "\t" + "\t" + _PreFix + "coefficients.ACoefficients[i] = " + _PreFix + "ACoefficients[i];" + "\r\n" +
                      "\t" + "}" + "\r\n" +
                      "\r\n" +
                      "\t" + "// Load default set of B-coefficients from user RAM into X-Space controller B-array" + "\r\n" +
                      "\t" + "for(i=0; i<" + FileNamePattern + ".BCoefficientsArraySize; i++)" + "\r\n" +
                      "\t" + "{" + "\r\n" +
                      "\t" + "\t" + _PreFix + "coefficients.BCoefficients[i] = " + _PreFix + "BCoefficients[i];" + "\r\n" +
                      "\t" + "}" + "\r\n" +
                      "\r\n"
                      );

                strBuffer.Append(
                      "\t" + "// Clear error and control histories of the " + compFilter.FilterOrder + "P" + compFilter.FilterOrder + "Z" + " controller" + "\r\n" +
                      "\t" + FileNamePattern + "_Reset(&" + FileNamePattern + ");" + "\r\n" +
                      "\r\n" +
                      "\t" + "return(1);" + "\r\n" +
                      "}" + "\r\n" +
                      "\r\n"
                      );

            }
            catch
            {
                strBuffer.Append("\r\n\r\n#error [Invalid values detected during body generation]\r\n\r\n");
            }

            return (strBuffer);

        }

    
    }

}