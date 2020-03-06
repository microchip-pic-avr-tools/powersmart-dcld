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
            set { _GenScript = value; return; }
        }

        private bool _SaveRestoreContext = true;
        internal bool SaveRestoreContext
        {
            get { return (_SaveRestoreContext); }
            set { _SaveRestoreContext = value; return; }
        }

        private bool _SaveRestoreShadowRegisters = true;
        internal bool SaveRestoreShadowRegisters
        {
            get { return (_SaveRestoreShadowRegisters); }
            set { _SaveRestoreShadowRegisters = value; return; }
        }

        private bool _SaveRestoreMACRegisters = true;
        internal bool SaveRestoreMACRegisters
        {
            get { return (_SaveRestoreMACRegisters); }
            set { _SaveRestoreMACRegisters = value; return; }
        }

        private bool _SaveRestoreAccumulators = true;
        internal bool SaveRestoreAccumulators
        {
            get { return (_SaveRestoreAccumulators); }
            set { _SaveRestoreAccumulators = value; return; }
        }

        private bool _SaveRestoreAccumulatorA = true;
        internal bool SaveRestoreAccumulatorA
        {
            get { return (_SaveRestoreAccumulatorA); }
            set { _SaveRestoreAccumulatorA = value; return; }
        }

        private bool _SaveRestoreAccumulatorB = true;
        internal bool SaveRestoreAccumulatorB
        {
            get { return (_SaveRestoreAccumulatorB); }
            set { _SaveRestoreAccumulatorB = value; return; }
        }

        private bool _SaveRestoreCoreConfig = true;
        internal bool SaveRestoreCoreConfig
        {
            get { return (_SaveRestoreCoreConfig); }
            set { _SaveRestoreCoreConfig = value; return; }
        }

        private bool _SaveRestoreCoreStatusRegister = true;
        internal bool SaveRestoreCoreStatusRegister
        {
            get { return (_SaveRestoreCoreStatusRegister); }
            set { _SaveRestoreCoreStatusRegister = value; return; }
        }

        private bool _AddCoreConfig = true;
        internal bool AddCoreConfig
        {
            get { return (_AddCoreConfig); }
            set { _AddCoreConfig = value; return; }
        }

        private bool _AddEnableDisableFeature = true;
        internal bool AddEnableDisableFeature
        {
            get { return (_AddEnableDisableFeature); }
            set { _AddEnableDisableFeature = value; return; }
        }

        private bool _AddDisableDummyReadFeature = true;
        internal bool AddDisableDummyReadFeature
        {
            get { return (_AddDisableDummyReadFeature); }
            set { _AddDisableDummyReadFeature = value; return; }
        }

        private bool _AddErrorInputNormalization = true;
        internal bool AddErrorInputNormalization
        {
            get { return (_AddErrorInputNormalization); }
            set { _AddErrorInputNormalization = value; return; }
        }

        private bool _AddAlternateSource = false;
        internal bool AddAlternateSource
        {
            get { return (_AddAlternateSource); }
            set { _AddAlternateSource = value; return; }
        }

        private bool _AddAlternateTarget = false;
        internal bool AddAlternateTarget
        {
            get { return (_AddAlternateTarget); }
            set { _AddAlternateTarget = value; return; }
        }

        private bool _AddADCTriggerAPlacement = true;
        internal bool AddADCTriggerAPlacement
        {
            get { return (_AddADCTriggerAPlacement); }
            set { _AddADCTriggerAPlacement = value; return; }
        }

        private bool _AddADCTriggerBPlacement = false;
        internal bool AddADCTriggerBPlacement
        {
            get { return (_AddADCTriggerBPlacement); }
            set { _AddADCTriggerBPlacement = value; return; }
        }

        private bool _AddCascadedFunctionCall = false;
        internal bool AddCascadedFunctionCall
        {
            get { return _AddCascadedFunctionCall; }
            set { _AddCascadedFunctionCall = value; return; }
        }

        private bool _AddAntiWindup = true;
        internal bool AddAntiWindup
        {
            get { return (_AddAntiWindup); }
            set { _AddAntiWindup = value; return; }
        }

        private bool _AntiWindupSoftDesaturationFlag = false;
        internal bool AntiWindupSoftDesaturationFlag
        {
            get { return (_AntiWindupSoftDesaturationFlag); }
            set { _AntiWindupSoftDesaturationFlag = value; return; }
        }

        private bool _AntiWindupClampMax = true;
        internal bool AntiWindupClampMax
        {
            get { return (_AntiWindupClampMax); }
            set { _AntiWindupClampMax = value; return; }
        }

        private bool _AntiWindupClampMaxWithStatusFlag = true;
        internal bool AntiWindupClampMaxWithStatusFlag
        {
            get { return (_AntiWindupClampMaxWithStatusFlag); }
            set { _AntiWindupClampMaxWithStatusFlag = value; return; }
        }

        private bool _AntiWindupClampMin = true;
        internal bool AntiWindupClampMin
        {
            get { return (_AntiWindupClampMin); }
            set { _AntiWindupClampMin = value; return; }
        }

        private bool _AntiWindupClampMinWithStatusFlag = true;
        internal bool AntiWindupClampMinWithStatusFlag
        {
            get { return (_AntiWindupClampMinWithStatusFlag); }
            set { _AntiWindupClampMinWithStatusFlag = value; return; }
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


        private bool _CreateCopyOfMostRecentControlInput = true;
        internal bool CreateCopyOfMostRecentControlInput
        {
            get { return (_CreateCopyOfMostRecentControlInput); }
            set { _CreateCopyOfMostRecentControlInput = value; return; }
        }

        private bool _CreateCopyOfMostRecentErrorInput = true;
        internal bool CreateCopyOfMostRecentErrorInput
        {
            get { return (_CreateCopyOfMostRecentErrorInput); }
            set { _CreateCopyOfMostRecentErrorInput = value; return; }
        }

        private bool _CreateCopyOfMostRecentControlOutput = true;
        internal bool CreateCopyOfMostRecentControlOutput
        {
            get { return (_CreateCopyOfMostRecentControlOutput); }
            set { _CreateCopyOfMostRecentControlOutput = value; return; }
        }

        private int _CodeOptimizationLevel = 0;
        internal int CodeOptimizationLevel
        {
            get { return (_CodeOptimizationLevel); }
            set { _CodeOptimizationLevel = value; return; }
        }


        private bool _StoreReloadAccLevel1 = true;
        internal bool StoreReloadAccLevel1
        {
            get { return (_StoreReloadAccLevel1); }
            set { _StoreReloadAccLevel1 = value; return; }
        }

        private bool _SpreadSpectrumModulation = false;
        internal bool SpreadSpectrumModulation
        {
            get { return (_SpreadSpectrumModulation); }
            set { _SpreadSpectrumModulation = value; return; }
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

        private int _NumberSizeInByte = 2;
        internal int NumberSizeInByte
        {
            get { return (_NumberSizeInByte); }
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

        private string _CustomComment = "";
        internal string CustomComment
        {
            get { return (_CustomComment); }
            set { _CustomComment = value; return; }
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
        private string ReplaceTokens(string user_string)
        {
            string str_dum = "";

            // Copy parameter
            str_dum = user_string.Trim();

            // Comment Blocks
            str_dum = str_dum.Replace("%SEPARATOR%", "\r\n;------------------------------------------------------------------------------\r\n");
            str_dum = str_dum.Replace("%EMPTY%", "");


            return (str_dum);
        }


        private string BuildCodeBody()
        {
            int block_count = 0, i = 0;
            int addr_offset = 0;
            bool bool_dummy = false;
            string str_dum = "", command = "";
            StringBuilder body = new StringBuilder();
            StringBuilder header = new StringBuilder();

            try 
            {

                _Postfix = _GenScript.ReadKey("filter_block_scaling_modes", _ScalingMethod.ToString(), "");
                if (_Postfix.Length == 0) return ("; (no code template available)");

                block_count = Convert.ToInt32(_GenScript.ReadKey("blockset_" + _Postfix + "_" + _CodeOptimizationLevel, "count", "0"));
                if (block_count == 0) return ("; (no code template available)");

                AccumulatorUsage = _GenScript.ReadKey("blockset_" + _Postfix + "_" + _CodeOptimizationLevel, "accu_usage", "ab");
                WREGUsage = _GenScript.ReadKey("blockset_" + _Postfix + "_" + _CodeOptimizationLevel, "wreg_usage", "4,5,8,10");

                addr_offset = Convert.ToInt16(_GenScript.ReadKey("filter_block_array_addressing", "addr_offset_" + _Postfix, "2"));
                _NumberSizeInByte = addr_offset;

                _CycleCountToWriteback = 0;
                _CycleCountTotal = 0;

                for (i = 0; i < block_count; i++)
                {
                    // Print recent execution step for debugging purposes
                    //body.Append("\r\n;execute block #" + i.ToString() + "\t" + ReadConfigString(_TemplateFile, "blockset_" + _Postfix + "_" + _CodeOptimizationLevel, i.ToString(), "(not found)") + "\r\n");

                    str_dum = "";
                    command = ReplaceTokens(_GenScript.ReadKey("blockset_" + _Postfix + "_" + _CodeOptimizationLevel, i.ToString(), "(not found)"));
                    
                    switch (command.ToLower().Trim())
                    {
                        case "(not found)":
                            str_dum = "\r\n\t#error: label '" + "blockset_" + _Postfix + "_" + _CodeOptimizationLevel + "[" + i.ToString() + "]' could not be found\r\n";
                            break;

                        case "disclaimer":
                            str_dum = BuildCodeBlock(command);
                            str_dum = str_dum + _CustomComment;
                            break;

                        case "context_save":
                            if (_SaveRestoreContext) str_dum = BuildCodeBlock(command);
                            break;

                        case "context_save_shadow":
                            if (_SaveRestoreShadowRegisters) str_dum = BuildCodeBlock(command);
                            break;

                        case "context_save_mac_registers":
                            if (_SaveRestoreMACRegisters) str_dum = BuildCodeBlock(command);
                            break;

                        case "fscl_context_save_mac_registers":
                            if (_SaveRestoreMACRegisters) str_dum = BuildCodeBlock(command);
                            break;

                        case "context_save_status_word_wreg":
                            if (((_AddAntiWindup) && ((_AntiWindupClampMaxWithStatusFlag) || (_AntiWindupClampMinWithStatusFlag))) || (_AddEnableDisableFeature))
                            { str_dum = BuildCodeBlock(command); }
                            break;

                        case "context_save_accumulator_a":
                            if ((_SaveRestoreAccumulatorA) && (_AccumulatorUsage.Contains("a"))) str_dum = BuildCodeBlock(command);
                            break;

                        case "context_save_accumulator_b":
                            if ((_SaveRestoreAccumulatorB) && (_AccumulatorUsage.Contains("b"))) str_dum = BuildCodeBlock(command);
                            break;

                        case "context_save_core_config":
                            if (_SaveRestoreCoreConfig) str_dum = BuildCodeBlock(command);
                            break;

                        case "context_save_core_status":
                            if (_SaveRestoreCoreStatusRegister) str_dum = BuildCodeBlock(command);
                            break;

                        case "context_restore":
                            if (_SaveRestoreContext) str_dum = BuildCodeBlock(command);
                            break;

                        case "context_restore_shadow":
                            if (_SaveRestoreShadowRegisters) str_dum = BuildCodeBlock(command);
                            break;

                        case "context_restore_mac_registers":
                            if (_SaveRestoreMACRegisters) str_dum = BuildCodeBlock(command);
                            break;

                        case "fscl_context_restore_mac_registers":
                            if (_SaveRestoreMACRegisters) str_dum = BuildCodeBlock(command);
                            break;

                        case "context_restore_status_word_wreg":
                            if (((_AddAntiWindup) && ((_AntiWindupClampMaxWithStatusFlag) || (_AntiWindupClampMinWithStatusFlag))) || (_AddEnableDisableFeature))
                            { str_dum = BuildCodeBlock(command); }
                            break;

                        case "context_restore_accumulator_a":
                            if ((_SaveRestoreAccumulatorA) && (_AccumulatorUsage.Contains("a"))) str_dum = BuildCodeBlock(command);
                            break;

                        case "context_restore_accumulator_b":
                            if ((_SaveRestoreAccumulatorB) && (_AccumulatorUsage.Contains("b"))) str_dum = BuildCodeBlock(command);
                            break;

                        case "context_restore_core_config":
                            if (_SaveRestoreCoreConfig) str_dum = BuildCodeBlock(command);
                            break;

                        case "context_restore_core_status":
                            if (_SaveRestoreCoreStatusRegister) str_dum = BuildCodeBlock(command);
                            break;

                        case "enable_disable_start":
                            if (_AddEnableDisableFeature) str_dum = BuildCodeBlock(command);
                            break;

                        case "enable_disable_end":
                            if ((_AddEnableDisableFeature) && (!_AddDisableDummyReadFeature)) str_dum = BuildCodeBlock(command);
                            break;

                        case "enable_disable_end_dummy_read":
                            if ((_AddEnableDisableFeature) && (_AddDisableDummyReadFeature))
                            {
                                str_dum = BuildCodeBlock(command);
                                if (_CreateCopyOfMostRecentControlInput) str_dum = str_dum + BuildCodeBlock("shadow_copy_control_input");
                            }
                            break;

                        case "enable_disable_end_dummy_read_end":
                            if ((_AddEnableDisableFeature) && (_AddDisableDummyReadFeature)) str_dum = BuildCodeBlock(command);
                            break;

                        case "core_config":
                            if (_AddCoreConfig) str_dum = BuildCodeBlock(command);
                            break;

                        case "shadow_copy_control_input":
                            if (_CreateCopyOfMostRecentControlInput) str_dum = BuildCodeBlock(command);
                            break;

                        case "shadow_copy_error_input":
                            if (_CreateCopyOfMostRecentErrorInput) str_dum = BuildCodeBlock(command);
                            break;

                        case "shadow_copy_control_output":
                            if (_CreateCopyOfMostRecentControlOutput) str_dum = BuildCodeBlock(command);
                            break;

                        case "comp_zero_input":
                            if (_BidirectionalFeedback) str_dum = BuildCodeBlock(command);
                            break;

                        case "comp_invert_input":
                            if (_FeedbackRectification) str_dum = BuildCodeBlock(command);
                            break;

                        case "comp_mac_load32b_a":
                            if (_StoreReloadAccLevel1) str_dum = BuildCodeBlock(command);
                            break;

                        case "comp_mac_load32b_b":
                            if (_StoreReloadAccLevel1) str_dum = BuildCodeBlock(command);
                            break;

                        case "comp_mac_store32b_a":
                            if (_StoreReloadAccLevel1) str_dum = BuildCodeBlock(command);
                            break;

                        case "comp_mac_store32b_b":
                            if (_StoreReloadAccLevel1) str_dum = BuildCodeBlock(command);
                            break;

                        case "comp_writeback":
                            if (_SpreadSpectrumModulation) { command = command + "_ssm"; }
                            if (_AddAlternateTarget) { command = command + "_with_alt_target_switch"; }
                            str_dum = BuildCodeBlock(command);
                            _CycleCountToWriteback = _CycleCountTotal;  // Capture value from most recent cycle count
                            break;

                        case "comp_read_input":
                            if (_AddAlternateSource) { command = command + "_with_alt_source_switch"; }
                            str_dum = BuildCodeBlock(command);
                            _CycleCountToDataCapture = _CycleCountTotal;  // Capture value from most recent cycle count
                            break;

                        case "comp_norm_error":
                            if (_AddErrorInputNormalization) str_dum = BuildCodeBlock(command);
                            break;

                        case "anti_windup":
                            if (_AddAntiWindup) str_dum = BuildCodeBlock(command);
                            break;

                        case "anti_windup_max":
                            if (_AntiWindupClampMax)
                            {
                                bool needs_bypass = false;

                                bool_dummy = ((_AntiWindupClampMaxWithStatusFlag) || (_AntiWindupSoftDesaturationFlag));    // elements added when output is overwritten (IF-statement)
                                needs_bypass = (_AntiWindupClampMaxWithStatusFlag); // elements added when output is overwritten (ELSE-statement)

                                str_dum = BuildCodeBlock("anti_windup_max_start");
                                if (bool_dummy) str_dum = str_dum + BuildCodeBlock("anti_windup_max_options_start");
                                if (_AntiWindupClampMaxWithStatusFlag) str_dum = str_dum + BuildCodeBlock("anti_windup_max_clear_status_flag");
                                if (bool_dummy) str_dum = str_dum + BuildCodeBlock("anti_windup_max_options_override_bypass");
                                if (bool_dummy) str_dum = str_dum + BuildCodeBlock("anti_windup_max_options_override_start");
                                str_dum = str_dum + BuildCodeBlock("anti_windup_max_override");
                                if (_AntiWindupSoftDesaturationFlag) str_dum = str_dum + BuildCodeBlock("anti_windup_soft_desaturation");
                                if (_AntiWindupClampMaxWithStatusFlag) str_dum = str_dum + BuildCodeBlock("anti_windup_max_set_status_flag");
                                if (bool_dummy) str_dum = str_dum + BuildCodeBlock("anti_windup_max_options_end");

                            }
                            break;

                        case "anti_windup_min":
                            if (_AntiWindupClampMin)
                            {

                                bool needs_bypass = false;

                                bool_dummy = ((_AntiWindupClampMinWithStatusFlag) || (_AntiWindupSoftDesaturationFlag));    // elements added when output is overwritten (IF-statement)
                                needs_bypass = (_AntiWindupClampMinWithStatusFlag); // elements added when output is overwritten (ELSE-statement)

                                str_dum = BuildCodeBlock("anti_windup_min_start");
                                if (bool_dummy) str_dum = str_dum + BuildCodeBlock("anti_windup_min_options_start");
                                if (_AntiWindupClampMinWithStatusFlag) str_dum = str_dum + BuildCodeBlock("anti_windup_min_clear_status_flag");
                                if (bool_dummy) str_dum = str_dum + BuildCodeBlock("anti_windup_min_options_override_bypass");
                                if (bool_dummy) str_dum = str_dum + BuildCodeBlock("anti_windup_min_options_override_start");
                                str_dum = str_dum + BuildCodeBlock("anti_windup_min_override");
                                if (_AntiWindupSoftDesaturationFlag) str_dum = str_dum + BuildCodeBlock("anti_windup_soft_desaturation");
                                if (_AntiWindupClampMinWithStatusFlag) str_dum = str_dum + BuildCodeBlock("anti_windup_min_set_status_flag");
                                if (bool_dummy) str_dum = str_dum + BuildCodeBlock("anti_windup_min_options_end");

                            }
                            break;

                        case "adc_trigger_a_placement":
                            if (_AddADCTriggerAPlacement)
                            {
                                if (_SpreadSpectrumModulation) { command = command + "_ssm"; }
                                str_dum = BuildCodeBlock(command);
                            }
                            break;

                        case "adc_trigger_b_placement":
                            if (_AddADCTriggerBPlacement)
                            {
                                if (_SpreadSpectrumModulation) { command = command + "_ssm"; }
                                str_dum = BuildCodeBlock(command);
                            }
                            break;

                        case "cascaded_function_call":
                            if (_AddCascadedFunctionCall) str_dum = BuildCodeBlock(command);
                            break;

                        case "update_status_bitfield":
                            if ((_AddAntiWindup) && ((_AntiWindupClampMaxWithStatusFlag) || (_AntiWindupClampMinWithStatusFlag))) str_dum = BuildCodeBlock(command); 
                            break;

                        default:
                            str_dum = BuildCodeBlock(command);
                            break;
                    }

                    if (str_dum.Trim().Length > 0)
                    {
                        str_dum = str_dum.Replace("\r\n", "\r\n\t");
                        str_dum = str_dum.Replace("\t;", ";");

                        // Move labels to the outer lefft of the line
                        if (str_dum.Contains(":") && str_dum.Contains("_"))
                        {
                            str_dum = str_dum.Replace("\t_", "_");
                        }
                        str_dum = str_dum.Replace("\t\t", "\t");

                        
                        body.Append(str_dum);
                    }
                
                }

                return (body.ToString());
            
            }
            catch { return ("(error while executing code body generation)"); }
        }

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
            cycles = Convert.ToInt32(_GenScript.ReadKey(block_name, "cycles", "0"));

            // Insert code lines with comments

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
            code_block.Replace("%FO*ADDR%", (fo_ptr * _NumberSizeInByte).ToString());   // Replace address offsets within the captured string
            code_block.Replace("%FO*ADDR-ADDR%", ((fo_ptr * _NumberSizeInByte) - _NumberSizeInByte).ToString());   // Replace address offsets within the captured string
            code_block.Replace("%FO+ADDR%", (fo_ptr * _NumberSizeInByte).ToString());   // Replace address offsets within the captured string
            code_block.Replace("%FO-ADDR%", (fo_ptr * _NumberSizeInByte + 2).ToString());   // Replace address offsets within the captured string
            code_block.Replace("%ADDR%", _NumberSizeInByte.ToString());   // Replace address offsets within the captured string


            str_dum=code_block.ToString();
            if (str_dum.Contains("%ADDR"))
            {
                // extract flag
                while ((str_dum.Length > 0) && (loops++ < 32))
                { 
                    str_dum = str_dum.Substring(str_dum.IndexOf("%ADDR") + 5);

                    str_flag = str_dum.TrimStart();
                    str_flag = str_flag.Substring(0, str_flag.IndexOf("%"));
                    num_dum = Convert.ToInt32(str_flag.Substring(1, str_flag.Length - 1));
                    str_flag = "%ADDR" + str_flag + "%";

                    switch(str_dum.Substring(0,1))
                    {
                        case "*": 
                            str_replace = (_NumberSizeInByte * num_dum).ToString();
                            break;
                        case "/": 
                            str_replace = (_NumberSizeInByte * num_dum).ToString();
                            break;
                        case "+": 
                            str_replace = (_NumberSizeInByte * num_dum).ToString();
                            break;
                        case "-": 
                            str_replace = (_NumberSizeInByte * num_dum).ToString();
                            break;
                    }

                    code_block.Replace(str_flag, str_replace);   // Replace address offsets within the captured string

                    if (!str_dum.Contains("%ADDR")) { break; }
                    else { str_dum = str_dum.Substring(str_dum.IndexOf("%") + 1); }
                }

            }

            // if code is not generated in loop (standard instruction only)
            if (loop_index >= 0)
            {
                code_block.Replace("%INDEX*ADDR-ADDR%", (loop_index * _NumberSizeInByte - _NumberSizeInByte).ToString());
                code_block.Replace("%INDEX*ADDR+ADDR%", (loop_index * _NumberSizeInByte + _NumberSizeInByte).ToString());
                code_block.Replace("%INDEX*ADDR%", (loop_index * _NumberSizeInByte).ToString());
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