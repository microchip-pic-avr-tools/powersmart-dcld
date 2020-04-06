using System;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace dcld
{
    public class clsCCodeGenerator
    {

        private clsINIFileHandler _GenScript = new clsINIFileHandler();
        internal clsINIFileHandler GeneratorScript
        {
            get { return (_GenScript); }
            set { 
                _GenScript = value;
                if (_tokens == null)
                    _tokens = new clsConditionalCode();
                _tokens.GetTokenList(_GenScript);
                return; 
            }
        }

        private clsINIFileHandler _dcldProjectFile = new clsINIFileHandler();
        internal clsINIFileHandler dcldProjectFile
        {
            get { return (_dcldProjectFile); }
            set { _dcldProjectFile = value; return; }
        }

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

        private string _CGS_Version = "n/a";
        internal string CGS_Version
        {
            get { return(_CGS_Version); }
            set { _CGS_Version = value; return; }
        }

        private string _CGS_VersionDate = "n/a";
        internal string CGS_VersionDate
        {
            get { return (_CGS_VersionDate); }
            set { _CGS_VersionDate = value; return; }
        }

        /* Code Generation Options */

        private dcld.clsConditionalCode _tokens;
        internal dcld.clsConditionalCode Tokens
        {
            get { return (_tokens); }
            set { _tokens = value; return; }
        }


        private string ReplaceConfigStringTokens(string text_line, clsCompensatorNPNZ compFilter)
        {

            string sDum = "";
            string _str_coeff_datatype = "", _str_hist_datatype = "", _str_struct_label = "";


            switch (compFilter.ScalingMethod)
            { 
                //case clsCompensatorNPNZ.dcldScalingMethod.DCLD_SCLMOD_DBLSCL_FLOAT:
                //    _str_coeff_datatype = "int32_t";
                //    _str_hist_datatype = "fractional";
                //    _str_struct_label = "cNPNZ3216b_t";
                //    break;
                default:
                    _str_coeff_datatype = "int32_t";
                    _str_hist_datatype = "fractional";
                    _str_struct_label = "cNPNZ16b_t";
                    break;
                    //_str_coeff_datatype = "fractional";
                    //_str_struct_label = "cNPNZ16b_t";
                    //_str_hist_datatype = "fractional";
                    //break;
            }

            sDum = text_line.Trim();

            if (sDum.Length > 0)
            {
                // Check for constant tokens to replace
                sDum = sDum.Replace("\\n", "\r\n");
                sDum = sDum.Replace("%PREFIX%", _PreFix.ToLower().Trim());
                sDum = sDum.Replace("%PREFIXG%", "_" + _PreFix.Trim());
                sDum = sDum.Replace("%PREFIXU%", _PreFix.ToUpper().Trim());
                sDum = sDum.Replace("%PREFIXL%", _PreFix.ToLower().Trim());
                sDum = sDum.Replace("%SEPARATOR%", "\r\n;------------------------------------------------------------------------------\r\n");
                sDum = sDum.Replace("%EMPTY%", "");
                sDum = sDum.Replace("%IDENT%", "    ");
                sDum = sDum.Replace("%SPACE%", " ");
                sDum = sDum.Replace("%APP_PRODUCT_NAME%", Application.ProductName);
                sDum = sDum.Replace("%APP_PRODUCT_VERSION%", Application.ProductVersion.ToString());
                sDum = sDum.Replace("%APP_PRODUCT_VERSION_KEY%", dcldGlobals.APP_VERSION_KEY.ToString());
                sDum = sDum.Replace("%CGS_VERSION%", _CGS_Version);
                sDum = sDum.Replace("%CGS_VERSION_DATE%", _CGS_VersionDate);
                sDum = sDum.Replace("%COMP_TYPE_NAME%", _CompTypeName);
                sDum = sDum.Replace("%SAMPLING_FREQUENCY%", compFilter.SamplingFrequency.ToString());
                sDum = sDum.Replace("%Q_FORMAT%", compFilter.QFormat.ToString());
                sDum = sDum.Replace("%SCALING_MODE%", _ScalingMethodName);
                sDum = sDum.Replace("%INPUT_GAIN%", compFilter.InputGain.ToString());
                sDum = sDum.Replace("%FILTER_ORDER%", compFilter.FilterOrder.ToString());
                sDum = sDum.Replace("%FILTER_ORDER+1%", (compFilter.FilterOrder + 1).ToString());
                sDum = sDum.Replace("%FILTER_ORDER-1%", (compFilter.FilterOrder - 1).ToString());
                sDum = sDum.Replace("%FILENAME_PATTERN%", _FileNamePattern.Trim());
                sDum = sDum.Replace("%FILENAME_PATTERN_U%", _FileNamePattern.ToUpper().Trim());
                sDum = sDum.Replace("%FILENAME_PATTERN_L%", _FileNamePattern.ToLower().Trim());
                sDum = sDum.Replace("%LIB_HEADER_INCLUDE_PATH%", _LibHeaderIncludePath);
                sDum = sDum.Replace("%C_HEADER_INCLUDE_PATH%", _CHeaderIncludePath + _FileNamePattern.ToLower() + ".h");
                sDum = sDum.Replace("%COEFF_DATA_TYPE%", _str_coeff_datatype);
                sDum = sDum.Replace("%PRESCALER%", compFilter.PreScaler.ToString());
                sDum = sDum.Replace("%POSTSHIFT_A%", compFilter.PostShiftA.ToString());
                sDum = sDum.Replace("%POSTSHIFT_B%", compFilter.PostShiftB.ToString());
                sDum = sDum.Replace("%POSTSCALER%", NumberBaseConverter.Dec2Hex(compFilter.PostScaler, compFilter.QFormat, true, true));
                sDum = sDum.Replace("%PTERMFACTOR%", NumberBaseConverter.Dec2Hex(compFilter.PTermFactor, compFilter.QFormat, true, true));
                sDum = sDum.Replace("%PTERMSCALER%", NumberBaseConverter.Dec2Hex(compFilter.PTermScaler, compFilter.QFormat, true, true));
                sDum = sDum.Replace("%HISTORY_DATA_TYPE%", _str_hist_datatype);
                sDum = sDum.Replace("%STRUCTURE_LABEL%", _str_struct_label);
                sDum = sDum.Replace("%USER_NAME%", Environment.UserName);
                sDum = sDum.Replace("%DATE_TODAY%", System.DateTime.Now.ToString());
                sDum = sDum.Replace("%SUPPORT_URL%", _GenScript.ReadKey("labels", "%SUPPORT_URL%", "").Trim());
                sDum = sDum.Replace("%VENDOR_URL%", _GenScript.ReadKey("labels", "%VENDOR_URL%", "").Trim());
                sDum = sDum.Replace("%TOOL_HOME_URL%", _GenScript.ReadKey("labels", "%TOOL_HOME_URL%", "").Trim());
                sDum = sDum + "\r\n";

                // Check for Option Tokens
                sDum = _tokens.GetTokenResult(sDum).CodeLine;

            }

            return (sDum);
        }


        internal StringBuilder BuildCLibHeader(clsCompensatorNPNZ compFilter)
        {
            string text_line = "", block_name = "";
            UInt32 text_elements = 0, i = 0;
            StringBuilder strBuffer = new StringBuilder();

            try
            {
                block_name = "library_header";
                text_elements = Convert.ToUInt32(_GenScript.ReadKey(block_name, "count", "0"));
                for(i=0; i<text_elements; i++)
                {
                    text_line = _GenScript.ReadKey(block_name, ("line" + i.ToString()), "");
                    text_line = ReplaceConfigStringTokens(text_line, compFilter);
                    if(text_line.Length > 0)
                        strBuffer.Append(text_line);
                }
                

            }
            catch
            {
                strBuffer.Append("\r\n\r\n#error [Invalid tokens detected during header body generation]\r\n\r\n");
            }

            return (strBuffer);

        }

        internal StringBuilder BuildCHeader(clsCompensatorNPNZ compFilter)
        {
            string text_line = "", block_name = "";
            UInt32 text_elements = 0, i = 0;
            StringBuilder strBuffer = new StringBuilder();

            try
            {
                block_name = "comp_header";
                text_elements = Convert.ToUInt32(_GenScript.ReadKey(block_name, "count", "0"));
                for (i = 0; i < text_elements; i++)
                {
                    text_line = _GenScript.ReadKey(block_name, ("line" + i.ToString()), "");
                    text_line = ReplaceConfigStringTokens(text_line, compFilter);
                    strBuffer.Append(text_line);
                }

            }
            catch
            {
                strBuffer.Append("\r\n\r\n#error [Invalid tokens detected during header body generation]\r\n\r\n");
            }

            return (strBuffer);

        }

        internal StringBuilder BuildSource(clsCompensatorNPNZ compFilter)
        {
            string _coeff_fill = "";
            string text_line = "", block_name = "";
            UInt32 functions = 0;
            UInt32 text_elements = 0, i = 0, k = 0, m = 0;
            StringBuilder strBuffer = new StringBuilder();

            try
            {
                // The coefficient format is unified to a 32-bit wide number. In normal Q15 bit-shift scaling modes
                // the high-word is set = zero while the low-word holds the Q15 coefficient.
                // In Fast Floating Point, the high-word holds the Q15 coefficient while the low-word holds the scaler.
                switch (compFilter.ScalingMethod)
                {
                    case clsCompensatorNPNZ.dcldScalingMethod.DCLD_SCLMOD_DBLSCL_FLOAT:
                        _coeff_fill = "";
                        break;
                    default:
                        _coeff_fill = "0000";
                        break;
                }

                block_name = "comp_source_head";
                text_elements = Convert.ToUInt32(_GenScript.ReadKey(block_name, "count", "0"));
                for (i = 0; i < text_elements; i++)
                {
                    text_line = _GenScript.ReadKey(block_name, ("line" + i.ToString()), "");
                    if (text_line.Contains("%LOOP_POLE_LOCATION_LIST%"))
                    {
                        for (k = 0; k < compFilter.FilterOrder; k++)
                        {
                            text_line = _GenScript.ReadKey(block_name, ("line" + i.ToString()), ""); 
                            text_line = ReplaceConfigStringTokens(text_line, compFilter);
                            text_line = text_line.Replace("%INDEX%", k.ToString());
                            text_line = text_line.Replace("%LOOP_POLE_LOCATION_LIST%", compFilter.Pole[k].Frequency.ToString());
                            strBuffer.Append(text_line);
                        }
                    }
                    else if (text_line.Contains("%LOOP_ZERO_LOCATION_LIST%"))
                    {
                        for (k = 1; k < compFilter.FilterOrder; k++)
                        {
                            text_line = _GenScript.ReadKey(block_name, ("line" + i.ToString()), "");
                            text_line = ReplaceConfigStringTokens(text_line, compFilter);
                            text_line = text_line.Replace("%INDEX%", k.ToString());
                            text_line = text_line.Replace("%LOOP_ZERO_LOCATION_LIST%", compFilter.Zero[k].Frequency.ToString());
                            strBuffer.Append(text_line);
                        }
                    }
                    else if (text_line.Contains("%LOOP_A_COEFFICIENTS_LIST%"))
                    {
                        for (k = 1; k < compFilter.FilterOrder; k++)
                        {
                            text_line = _GenScript.ReadKey(block_name, ("line" + i.ToString()), "");
                            text_line = ReplaceConfigStringTokens(text_line, compFilter);
                            text_line = text_line.Replace("%INDEX%", k.ToString());
                            text_line = text_line.Replace("%LOOP_A_COEFFICIENTS_LIST%", "0x" + _coeff_fill + compFilter.CoeffA[k].Hex.ToString() + ",");
                            strBuffer.Append(text_line);
                        }

                        // Last coefficient is added to array without tailing comma
                        text_line = _GenScript.ReadKey(block_name, ("line" + i.ToString()), "");
                        text_line = ReplaceConfigStringTokens(text_line, compFilter);
                        text_line = text_line.Replace("%INDEX%", k.ToString());
                        text_line = text_line.Replace("%LOOP_A_COEFFICIENTS_LIST%", "0x" + _coeff_fill + compFilter.CoeffA[k].Hex.ToString() + " ");
                        strBuffer.Append(text_line);
                    }
                    else if (text_line.Contains("%LOOP_B_COEFFICIENTS_LIST%"))
                    {
                        for (k = 0; k < compFilter.FilterOrder; k++)
                        {
                            text_line = _GenScript.ReadKey(block_name, ("line" + i.ToString()), "");
                            text_line = ReplaceConfigStringTokens(text_line, compFilter);
                            text_line = text_line.Replace("%INDEX%", k.ToString());
                            text_line = text_line.Replace("%LOOP_B_COEFFICIENTS_LIST%", "0x" + _coeff_fill + compFilter.CoeffB[k].Hex.ToString() + ",");
                            strBuffer.Append(text_line);
                        }

                        // Last coefficient is added to array without tailing comma
                        text_line = _GenScript.ReadKey(block_name, ("line" + i.ToString()), "");
                        text_line = ReplaceConfigStringTokens(text_line, compFilter);
                        text_line = text_line.Replace("%INDEX%", k.ToString());
                        text_line = text_line.Replace("%LOOP_B_COEFFICIENTS_LIST%", "0x" + _coeff_fill + compFilter.CoeffB[k].Hex.ToString() + " ");
                        strBuffer.Append(text_line);
                    
                    }
                    else
                    {
                        text_line = ReplaceConfigStringTokens(text_line, compFilter);
                        strBuffer.Append(text_line);
                    }
                }


                functions = Convert.ToUInt32(_GenScript.ReadKey("comp_source_functions", "count", "0"));

                for (m = 0; m < functions; m++)
                {

                    block_name = _GenScript.ReadKey("comp_source_functions", "function" + m.ToString(), "0");
                    text_elements = Convert.ToUInt32(_GenScript.ReadKey(block_name, "count", "0"));

                    for (i = 0; i < text_elements; i++)
                    {
                        text_line = _GenScript.ReadKey(block_name, ("line" + i.ToString()), "");
                        text_line = ReplaceConfigStringTokens(text_line, compFilter);
                        strBuffer.Append(text_line);
                    }

                }
            }
            catch
            {
                strBuffer.Append("\r\n\r\n#error [Invalid tokens detected during body generation]\r\n");
                strBuffer.Append("       => error triggered by " + _GenScript.FileTitle + ", " + block_name + ", line " + i.ToString() + "\r\n\r\n");
            }

            return (strBuffer);

        }

    
    }

}