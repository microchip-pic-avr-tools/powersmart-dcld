using System;
using System.Text;
using System.Runtime.InteropServices;

namespace dcld
{
    public class clsAssemblyGenerator
    {

        // Class Properties

        private clsINIFileHandler _GenScript = new clsINIFileHandler();
        internal clsINIFileHandler GeneratorScript
        {
            get { return (_GenScript); }
            set
            {
                _GenScript = value;
                if (_tokens == null)
                    _tokens = new clsConditionalCode();
                _tokens.GetTokenList(_GenScript);
                return;
            }
        }

        private bool _BidirectionalFeedback = false;
        internal bool BidirectionalFeedback        // Specifies if input value has a static offset
        {
            get { return _BidirectionalFeedback; }
            set { _BidirectionalFeedback = value; return; }
        }

        private bool _FeedbackRectification = false;
        internal bool FeedbackRectification        // In bi-directional feedback systems with defined offsets, "reverse control" is enabled by rectifying input data
        {
            get { return _FeedbackRectification; }
            set { _FeedbackRectification = value; return; }
        }

        private int _CodeOptimizationLevel = 0;
        internal int CodeOptimizationLevel
        {
            get { return (_CodeOptimizationLevel); }
            set { _CodeOptimizationLevel = value; return; }
        }

        //----------------------------------------------------------------------------------------------

        private string _Prefix = "";
        internal string Prefix
        {
            get { return (_Prefix); }
            set { _Prefix = value; return; }
        }

        private string _Postfix = "";
        internal string Postfix
        {
            get { return (_Postfix); }
        }

        private int _DeviceType = 0;
        internal int DeviceType
        {
            get { return (_DeviceType); }
            set { _DeviceType = value; return; }
        }

        private int _ScalingMethod = 0;
        internal int ScalingMethod
        {
            get { return (_ScalingMethod); }
            set 
            { 
                string str_dum = "";

                _ScalingMethod = value;

                str_dum = _GenScript.ReadKey("filter_block_scaling_modes", _ScalingMethod.ToString(), "");
                if (str_dum.Trim().Length > 0)
                { _ScalingMethodDescription = _GenScript.ReadKey("filter_block_scaling_modes_descritpion", str_dum, ""); }
                else 
                { _ScalingMethodDescription = ""; }
                
                return; 
            }
        }

        private string _ScalingMethodDescription = "";
        internal string ScalingMethodDescription
        {
            get { return (_ScalingMethodDescription); }
        }

        private int _FilterOrder = 0;
        internal int FilterOrder
        {
            get { return (_FilterOrder); }
            set { _FilterOrder = value; return; }
        }

        private bool _CycleCountEnable = false;

        private int _CycleCountTotal = 0;
        internal int CycleCountTotal
        {
            get { return (_CycleCountTotal); }
        }

        private int _CycleCountToDataCapture = 0;
        internal int CycleCountToDataCapture
        {
            get { return (_CycleCountToDataCapture); }
        }

        private int _CycleCountToWriteback = 0;
        internal int CycleCountToWriteback
        {
            get { return (_CycleCountToWriteback); }
        }

        private int _NumberSizeInByteCoeff = 4;
        internal int NumberSizeInByte_Coefficients
        {
            get { return (_NumberSizeInByteCoeff); }
        }

        private int _NumberSizeInByteData = 2;
        internal int NumberSizeInByte_Data
        {
            get { return (_NumberSizeInByteData); }
        }

        private string _AccumulatorUsage = "ab";
        internal string AccumulatorUsage
        {
            get { return (_AccumulatorUsage); }
            private set { _AccumulatorUsage = value.ToLower().Trim(); return; }
        }

        private string _WREGUsage = "4,5,8,10";
        internal string WREGUsage
        {
            get { return (_WREGUsage); }
            private set { _WREGUsage = value.ToLower().Trim(); return; }
        }

        private string _custom_comment = "";
        internal string CustomComment
        {
            get { return (_custom_comment); }
            set { _custom_comment = value; return; }
        }

        /* Code Generation Options */

        private clsConditionalCode _tokens;
        internal clsConditionalCode Tokens
        {
            get { return (_tokens); }
            set { _tokens = value; return; }
        }


        //----------------------------------------------------------------------------------------------
        //----------------------------------------------------------------------------------------------
        public string BuildCode()
        {
            StringBuilder code = new StringBuilder();

            code.Append(BuildCodeBody());

            return (code.ToString());
        }

        //----------------------------------------------------------------------------------------------
        //----------------------------------------------------------------------------------------------

        // This function reads all Token-IDs and Token Keys from the code generator script
        private bool GetConditionalCodeTokenList()
        {
            bool fres = false;
            int _i = 0, _token_count = 0;
            string text_line = "";

            try
            {
                _token_count = Convert.ToInt32(_GenScript.ReadKey("option_ids", "count", "0"));

                for (_i = 0; _i < _token_count; _i++)
                {
                    text_line = _GenScript.ReadKey("option_ids", (_i.ToString()), "");
                    if (_tokens == null)
                        _tokens = new clsConditionalCode();
                    fres = _tokens.Add(text_line);
                    if (!fres) break;
                }

                return (fres);
            }
            catch
            { return (false); }

        }

        //----------------------------------------------------------------------------------------------
        //----------------------------------------------------------------------------------------------



        private string ReplaceTokens(string user_string)
        {
            string str_dum = "";

            // Copy parameter
            str_dum = user_string.Trim();

            // Comment Blocks
            str_dum = str_dum.Replace("%SEPARATOR%", "\r\n;------------------------------------------------------------------------------\r\n");
            str_dum = str_dum.Replace("%EMPTY%", "");

            str_dum = str_dum.Replace("%PREFIX%", _Prefix.ToLower().Trim());
            str_dum = str_dum.Replace("%PREFIXG%", "_" + _Prefix.Trim());
            str_dum = str_dum.Replace("%PREFIXU%", _Prefix.ToUpper().Trim());
            str_dum = str_dum.Replace("%PREFIXL%", _Prefix.ToLower().Trim());
            str_dum = str_dum.Replace("%POSTFIX%", _Postfix.ToLower().Trim());
            str_dum = str_dum.Replace("%POSTFIXG%", _Postfix.Trim());
            str_dum = str_dum.Replace("%POSTFIXU%", _Postfix.ToUpper().Trim());
            str_dum = str_dum.Replace("%POSTFIXL%", _Postfix.ToLower().Trim());

            // Check for Option Tokens
            str_dum = _tokens.GetTokenResult(str_dum).CodeLine;

            return (str_dum);
        }


        private string BuildCodeBody()
        {
            int block_count = 0, i = 0;
            int _addr_offset_coeff = 0;
            int _addr_offset_data = 0;
            //bool bool_dummy = false;
            string str_dum = "", command = "";
            StringBuilder body = new StringBuilder();
            StringBuilder header = new StringBuilder();

            //try 
            {

                _Postfix = _GenScript.ReadKey("filter_block_scaling_modes", _ScalingMethod.ToString(), "");
                if (_Postfix.Length == 0) return ("; (no code template available)");

                block_count = Convert.ToInt32(_GenScript.ReadKey("blockset_" + _Postfix + "_" + _CodeOptimizationLevel, "count", "0"));
                if (block_count == 0) return ("; (no code template available)");

                _AccumulatorUsage = _GenScript.ReadKey("blockset_" + _Postfix + "_" + _CodeOptimizationLevel, "accu_usage", "ab");
                _WREGUsage = _GenScript.ReadKey("blockset_" + _Postfix + "_" + _CodeOptimizationLevel, "wreg_usage", "4,6,8,10");

                _addr_offset_coeff = Convert.ToInt16(_GenScript.ReadKey("filter_block_array_addressing_coeff", "addr_offset_" + _Postfix, "4"));
                _addr_offset_data = Convert.ToInt16(_GenScript.ReadKey("filter_block_array_addressing_data", "addr_offset_" + _Postfix, "2"));

                _NumberSizeInByteData = _addr_offset_data;
                _NumberSizeInByteCoeff = _addr_offset_coeff;

                _CycleCountToWriteback = 0;
                _CycleCountTotal = 0;

                for (i = 0; i < block_count; i++)
                {
                    // clear string buffer and read next command set
                    str_dum = "";
                    command = ReplaceTokens(_GenScript.ReadKey("blockset_" + _Postfix + "_" + _CodeOptimizationLevel, i.ToString(), "(not found)"));

                    // Parse command set option tokens to add/skip command
                    if (!_GenScript.KeyExists(command, "lines") && (command.Length>0))
                    { // If command cannot be found of token parsing failed, print error message
                        str_dum = command;  
                    }
                    else 
                    { // if command is an empty line or script request returned "(not found)", add line to code
                    
                        switch (command.ToLower().Trim())
                        {
                            case "(not found)":
                                str_dum = "\r\n\t#error: label '" + "blockset_" + _Postfix + "_" + _CodeOptimizationLevel + "[" + i.ToString() + "]' could not be found\r\n";
                                break;

                            case "disclaimer":
                                str_dum = BuildCodeBlock(command);
                                str_dum = str_dum + _custom_comment;
                                break;

                            case "exec_function_head":
                                str_dum = BuildCodeBlock(command);
                                _CycleCountEnable = true; // Setting flag starting instruction cycle count of main loop
                                break;

                            case "return":
                                str_dum = BuildCodeBlock(command);
                                _CycleCountEnable = false; // Clearing flag ending instruction cycle count of main loop
                                break;

                            case "comp_writeback":
                                str_dum = BuildCodeBlock(command);
                                if (_CycleCountEnable) _CycleCountToWriteback = _CycleCountTotal;  // Capture value from most recent cycle count
                                break;

                            case "comp_read_input":
                                str_dum = BuildCodeBlock(command);
                                if (_CycleCountEnable) _CycleCountToDataCapture = _CycleCountTotal;  // Capture value from most recent cycle count
                                break;

                            default:
                                str_dum = BuildCodeBlock(command);
                                break;
                        }

                    }

                    // format line ending, set instruction line ident
                    if (str_dum.Trim().Length > 0)
                    {
                        str_dum = str_dum.Replace("\r\n", "\r\n\t"); // at every lime end, set ident of following line
                        str_dum = str_dum.Replace("\t;", ";"); // if no line ending, set ident of comment line

                        // Move labels to the outer left of the line (no ident)
                        if (str_dum.Contains(":") && str_dum.Contains("_"))
                            str_dum = str_dum.Replace("\t_", "_");

                        // Remove accidential doubble idents
                        str_dum = str_dum.Replace("\t\t", "\t");

                        // Add code line to code
                        body.Append(str_dum);
                    }
                
                }

                // return code line
                return (body.ToString());
            
            }
            // catch { return ("(error while executing code body generation of command '" + command + "')"); }
        }

        /* *********************************************************************************************
         * ********************************************************************************************/

        private string BuildCodeBlock(string block_name)
        {
            int i = 0, k = 0, cycles = 0, cycle_count = 0, line_cnt = 0;
            string hcomm = "";

            StringBuilder code_line = new StringBuilder();
            StringBuilder code_block = new StringBuilder();

            // read and insert head comment of this code block
            hcomm = ReplaceTokens(_GenScript.ReadKey(block_name, "head_comment", ""));
            if(hcomm.Length > 0) hcomm = hcomm + "\r\n";

            // get line count and execution cycles
            line_cnt = Convert.ToInt32(_GenScript.ReadKey(block_name, "lines", "0"));
            if (_CycleCountEnable) 
                cycles = Convert.ToInt32(_GenScript.ReadKey(block_name, "cycles", "0"));
            else
                cycles = 0;

            // ~~~~~~ Insert code lines with comments ~~~~~~ 

            code_line.Clear();  // clear buffer


            // if code block needs to be generated in loops...
            if (_GenScript.ReadKey(block_name, "filter_order_loop", "0").ToLower().Trim() == "as")
            {   // Loop Instruction on filter order

                for (i = 1; i < _FilterOrder-1; i++)
                {
                    for (k = 0; k < line_cnt; k++)
                    { code_line.Append(BuildCodeBlockLine(block_name, k, i)); }
                    cycle_count += cycles;                          // keep counting instruction cycles
                }
            }
            else if (_GenScript.ReadKey(block_name, "filter_order_loop", "0").ToLower().Trim() == "bs")
            {   // Loop Instruction on filter order

                for (i = 0; i < _FilterOrder-1; i++)
                {
                    for (k = 0; k < line_cnt; k++)
                    { code_line.Append(BuildCodeBlockLine(block_name, k, i)); }
                    cycle_count += cycles;                          // keep counting instruction cycles 
                }
            }
            else if (_GenScript.ReadKey(block_name, "filter_order_loop", "0").ToLower().Trim() == "a")
            {   // Loop Instruction on filter order

                for (i = 1; i < _FilterOrder; i++)
                {
                    for (k = 0; k < line_cnt; k++)
                    { code_line.Append(BuildCodeBlockLine(block_name, k, i)); }
                    cycle_count += cycles;                          // keep counting instruction cycles
                }
            }
            else if (_GenScript.ReadKey(block_name, "filter_order_loop", "0").ToLower().Trim() == "b")
            {   // Loop Instruction on filter order

                for (i = 0; i < _FilterOrder; i++)
                {
                    for (k = 0; k < line_cnt; k++)
                    { code_line.Append(BuildCodeBlockLine(block_name, k, i)); }
                    cycle_count += cycles;                          // keep counting instruction cycles 
                }
            }
            else if (_GenScript.ReadKey(block_name, "address_loop", "0").ToLower().Trim() == "a")
            {   // Loop Instruction on filter order

                for (i = (_FilterOrder - 1); i > 0; i--)
                {
                    for (k = 0; k < line_cnt; k++)
                    { code_line.Append(BuildCodeBlockLine(block_name, k, i)); }
                    cycle_count += cycles;                          // keep counting instruction cycles
                }
            }
            else if (_GenScript.ReadKey(block_name, "address_loop", "0").ToLower().Trim() == "b")
            {   // Loop Instruction on filter order

                for (i = _FilterOrder; i > 0; i--)
                {
                    for (k = 0; k < line_cnt; k++)
                    { code_line.Append(BuildCodeBlockLine(block_name, k, i)); }
                    cycle_count += cycles;                          // keep counting instruction cycles
                }
            }
            else
            {   // common code line is inserted (no loop)
                for (k = 0; k < line_cnt; k++)
                { code_line.Append(BuildCodeBlockLine(block_name, k)); }
                cycle_count = cycles;                          // determine instruction cycles
            }

            // add new line to code block
            code_block.Append(code_line.ToString());

            // Add block head comment
            if (hcomm.Length > 0) { code_block.Insert(0, hcomm); }

            // Update total execution cycle count
            _CycleCountTotal += cycle_count;

            return (code_block.ToString());
        }

        /* *********************************************************************************************
         * ********************************************************************************************/

        private string BuildCodeBlockLine(string block_name, int line_index, int loop_index = -1)
        {
            int fo_ptr = 0, num_dum = 0, loops = 0;
            string str_dum = "", str_flag = "", str_replace = "";
            StringBuilder code_line = new StringBuilder();
            StringBuilder comm_line = new StringBuilder();
            StringBuilder code_block = new StringBuilder();

            // Read code line and skip if it cannot be found (empty lines pass through)
            code_line.Append(ReplaceTokens(_GenScript.ReadKey(block_name, "code" + line_index.ToString(), "")));
            if (code_line.ToString().ToLower().Trim() == "(not found)")
            { return ("\r\n\t#error: label '" + block_name + "' " + "code" + line_index.ToString() + " unknown"); }

            // Read comment line
            comm_line.Append(ReplaceTokens(_GenScript.ReadKey(block_name, "comment" + line_index.ToString(), "")));
            comm_line.Replace("%LINE%", line_index.ToString());

            // Merge code line and comment into block line
            if ((code_line.ToString().Trim().Length > 0) && (comm_line.ToString().Trim().Length > 0))
            { code_block.AppendLine(code_line.ToString().Trim() + "    ; " + comm_line.ToString().Trim()); }
            else if ((code_line.ToString().Trim().Length > 0) && (comm_line.ToString().Trim().Length == 0))
            { code_block.AppendLine(code_line.ToString().Trim()); }
            else if ((code_line.ToString().Trim().Length == 0) && (comm_line.ToString().Trim().Length > 0))
            { code_block.AppendLine("; " + comm_line.ToString().Trim()); }
            else { return (""); }

            // Parse and replace flags
            fo_ptr = _FilterOrder;

            code_block.Replace("%FO%", fo_ptr.ToString());   // Replace address offsets within the captured string
            code_block.Replace("%FO*ADDR_DATA%", (fo_ptr * _NumberSizeInByteData).ToString());   // Replace address offsets within the captured string
            code_block.Replace("%FO*ADDR_DATA-ADDR_DATA%", ((fo_ptr * _NumberSizeInByteData) - _NumberSizeInByteData).ToString());   // Replace address offsets within the captured string
            code_block.Replace("%FO+ADDR_DATA%", (fo_ptr * _NumberSizeInByteData).ToString());   // Replace address offsets within the captured string
            code_block.Replace("%FO-ADDR_DATA%", (fo_ptr * _NumberSizeInByteData + 2).ToString());   // Replace address offsets within the captured string
            code_block.Replace("%ADDR_DATA%", _NumberSizeInByteData.ToString());   // Replace address offsets within the captured string
            code_block.Replace("%FO*ADDR_COEF%", (fo_ptr * _NumberSizeInByteCoeff).ToString());   // Replace address offsets within the captured string
            code_block.Replace("%FO*ADDR_COEF-ADDR_COEF%", ((fo_ptr * _NumberSizeInByteCoeff) - _NumberSizeInByteCoeff).ToString());   // Replace address offsets within the captured string
            code_block.Replace("%FO+ADDR_COEF%", (fo_ptr * _NumberSizeInByteCoeff).ToString());   // Replace address offsets within the captured string
            code_block.Replace("%FO-ADDR_COEF%", (fo_ptr * _NumberSizeInByteCoeff + 2).ToString());   // Replace address offsets within the captured string
            code_block.Replace("%ADDR_COEF%", _NumberSizeInByteCoeff.ToString());   // Replace address offsets within the captured string


            str_dum=code_block.ToString();
            if (str_dum.Contains("%ADDR_DATA"))
            {
                // extract flag
                while ((str_dum.Length > 0) && (loops++ < 32))
                {
                    str_dum = str_dum.Substring(str_dum.IndexOf("%ADDR_DATA") + ("%ADDR_DATA").Length);

                    str_flag = str_dum.TrimStart();
                    str_flag = str_flag.Substring(0, str_flag.IndexOf("%"));
                    num_dum = Convert.ToInt32(str_flag.Substring(1, str_flag.Length - 1));
                    str_flag = "%ADDR_DATA" + str_flag + "%";

                    switch(str_dum.Substring(0,1))
                    {
                        case "*":
                            str_replace = (_NumberSizeInByteData * num_dum).ToString();
                            break;
                        case "/":
                            str_replace = (_NumberSizeInByteData / num_dum).ToString();
                            break;
                        case "+":
                            str_replace = (_NumberSizeInByteData + num_dum).ToString();
                            break;
                        case "-":
                            str_replace = (_NumberSizeInByteData - num_dum).ToString();
                            break;
                    }

                    code_block.Replace(str_flag, str_replace);   // Replace address offsets within the captured string

                    if (!str_dum.Contains("%ADDR_DATA")) { break; }
                    else { str_dum = str_dum.Substring(str_dum.IndexOf("%") + 1); }
                }

            }
            
            if (str_dum.Contains("%ADDR_COEF"))
            {
                // extract flag
                while ((str_dum.Length > 0) && (loops++ < 32))
                {
                    str_dum = str_dum.Substring(str_dum.IndexOf("%ADDR_COEF") + ("%ADDR_COEF").Length);

                    str_flag = str_dum.TrimStart();
                    str_flag = str_flag.Substring(0, str_flag.IndexOf("%"));
                    num_dum = Convert.ToInt32(str_flag.Substring(1, str_flag.Length - 1));
                    str_flag = "%ADDR_COEF" + str_flag + "%";

                    switch (str_dum.Substring(0, 1))
                    {
                        case "*":
                            str_replace = (_NumberSizeInByteCoeff * num_dum).ToString();
                            break;
                        case "/":
                            str_replace = (_NumberSizeInByteCoeff / num_dum).ToString();
                            break;
                        case "+":
                            str_replace = (_NumberSizeInByteCoeff + num_dum).ToString();
                            break;
                        case "-":
                            str_replace = (_NumberSizeInByteCoeff - num_dum).ToString();
                            break;
                    }

                    code_block.Replace(str_flag, str_replace);   // Replace address offsets within the captured string

                    if (!str_dum.Contains("%ADDR_COEFF")) { break; }
                    else { str_dum = str_dum.Substring(str_dum.IndexOf("%") + 1); }
                }

            }
            // if code is not generated in loop (standard instruction only)
            if (loop_index >= 0)
            {
                code_block.Replace("%INDEX*ADDR_DATA-ADDR_DATA%", (loop_index * _NumberSizeInByteData - _NumberSizeInByteData).ToString());
                code_block.Replace("%INDEX*ADDR_DATA+ADDR_DATA%", (loop_index * _NumberSizeInByteData + _NumberSizeInByteData).ToString());
                code_block.Replace("%INDEX*ADDR_DATA%", (loop_index * _NumberSizeInByteData).ToString());
                code_block.Replace("%INDEX*ADDR_COEF-ADDR_COEF%", (loop_index * _NumberSizeInByteCoeff - _NumberSizeInByteCoeff).ToString());
                code_block.Replace("%INDEX*ADDR_COEF+ADDR_COEF%", (loop_index * _NumberSizeInByteCoeff + _NumberSizeInByteCoeff).ToString());
                code_block.Replace("%INDEX*ADDR_COEF%", (loop_index * _NumberSizeInByteCoeff).ToString());
                code_block.Replace("%FO-INDEX%", (_FilterOrder - loop_index).ToString());
                code_block.Replace("%INDEX%", loop_index.ToString());   // Replace indices within the captured string (usually in comments)
            }

            if (_Prefix.Length > 0)
            {
                code_block.Replace("%PREFIX%", _Prefix.ToLower().Trim());
                code_block.Replace("%PREFIXG%", "_" + _Prefix.Trim());
                code_block.Replace("%PREFIXU%", _Prefix.ToUpper().Trim());
                code_block.Replace("%PREFIXL%", _Prefix.ToLower().Trim());
                code_block.Replace("%POSTFIX%", _Postfix.ToLower().Trim());
                code_block.Replace("%POSTFIXG%", _Postfix.Trim());
                code_block.Replace("%POSTFIXU%", _Postfix.ToUpper().Trim());
                code_block.Replace("%POSTFIXL%", _Postfix.ToLower().Trim());
            }
            else { code_block.Replace("%PREFIX%", ""); }


            return (code_block.ToString());
        }
    }


}