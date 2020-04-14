using System;
using System.Globalization;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Windows.Forms.DataVisualization.Charting;
using System.Numerics;

using System.Diagnostics;

namespace dcld
{

    [System.Runtime.InteropServices.GuidAttribute("3952432A-6B37-4A47-8A68-A42D879C5A0D")]
    public partial class frmMain : Form
    {
        // Data Class to calculate control coefficients
        clsCompensatorNPNZ cNPNZ = new clsCompensatorNPNZ();

        const int MAX_FILTER_ORDER = 6;
        clsPoleZeroObject[] UserPoleSettings = new clsPoleZeroObject[MAX_FILTER_ORDER];
        clsPoleZeroObject[] UserZeroSettings = new clsPoleZeroObject[MAX_FILTER_ORDER];

        clsFeedbackDeclaration feedback = new clsFeedbackDeclaration();
        clsOutputDeclaration ctrl_output = new clsOutputDeclaration();

        clsMPLABXHandler MPLABXProject = new clsMPLABXHandler();
        clsFilePathHandler ConvertFilePath = new clsFilePathHandler();

        // Value table formating
        Color WarningBackground = Color.FromArgb(255, 255, 120);
        Color AlertBackground = Color.FromArgb(255, 200, 200);

        string Q7Format  = "{0:0.00000000}";
        string Q15Format = "{0:0.000000000000000}";
        string Q23Format = "{0:0.000000000000000000000}";
        string Q31Format = "{0:0.000000000000000000000000}";
        string QFormatStr = "{0:0.000000000000000}";


        // Win API Function Calls
        [DllImport("Kernel32.dll")]
            static extern bool Beep(Int32 dwFreq, Int32 dwDuration);

        // Fundamental calculation parameters
        double MaxError = 0.0, WarnError = 0.0;
        int LagElements = 0, Q_format = 0, ScalingMode = 0;
        int preLagElements = -1, preScalingMode = -1, preQFormat = -1;
        string DefaultFilePrefix = "";
        string DefaultVariablePrefix = "";
        string _targetDevice = ""; // dsPIC-Product Number as string, e.g. dsPIC33CK256MP506
        string _targetDeviceType = ""; // Reduced type indication, e.g. dsPIC33C

        // GUI status flags
        bool ExternalFileOpenEvent = false;
        bool FilterTypeChanged = true;
        bool ScalingChanged = true;
        bool UpdateComplete = false;
        bool UpdateWarning = false;
        bool ApplicationStartUp = true;
        bool ApplicationShutDown = false;
        bool ProjectFileLoadActive = false;
        bool ProjectFileChanged = false;
        bool ShowSDomainTF = true;

        bool showMPLABXconfigWindowAtStartup = true;

        // Project files
        string DefaultProjectFileNameExtension = ".dcld";
        string DefaultProjectFileName = "MyCtrlLoop";
        string NewProjectFilenameDummy = "";
        string ExternalFileOpenPath = "";
        string UserGuideFileName = "";

        string rootPath = Application.StartupPath;
        string resourcePath = Application.StartupPath;

        const string DEFAULT_INI_FILE = "dcld.ini";
        const string ASM_GEN_FILE = "assembly.gen";
        const string C_GEN_FILE = "c-code.gen";

        string _dsp = System.IO.Path.DirectorySeparatorChar.ToString();
        string _adsp = System.IO.Path.AltDirectorySeparatorChar.ToString();

        clsINIFileHandler SettingsFile = new clsINIFileHandler();
        clsINIFileHandler ProjectFile = new clsINIFileHandler();
        clsINIFileHandler AsmGeneratorScript = new clsINIFileHandler();
        clsINIFileHandler CCodeGeneratorScript = new clsINIFileHandler();

        // GUI controls groups
        TextBox[] txtPole = null, txtZero = null;
        Label[] lblPole = null, lblZero = null;
        Label[] lblPoleUnit = null, lblZeroUnit = null;

        // Bode Plot Properties
        bool MoveAnnotation = false;
        KeyEventArgs ChartKeyEvents;
        MouseEventArgs ChartMouseEvents;
        MouseEventArgs RelativeMouseMoveStart;
        MouseEventArgs RelativeMouseMoveStop;
        MouseEventArgs AbsoluteMouseMoveStart;
        MouseEventArgs AbsoluteMouseMoveStop;

        double DefaultXMin = 10.0;
        double DefaultXMax = 1000000.0;
        double DefaultY1Min = -60.0;
        double DefaultY1Max =  60.0;
        double DefaultY2Min = -180.0;
        double DefaultY2Max = 180.0;

        // Timing chart properties
        bool FreezeTimingCursor = false;  // Mouse-Click toggles FreezeTimingCursor
        bool FreezeBodeCursor = false;  // Mouse-Click toggles FreezeBodeCursor

        enum chartRegions : byte
        { chartPrimaryYAxis, chartSecondaryYAxes, chartXAxes, chartLegend, chartPrintArea }

        // Code generator options variables
        int GroupFolding_MinHeight = 20;
 //       int GroupFolding_VDistance = 7; (not used)
        int GroupFolding_grpContextSavingHeight = 100;
        int GroupFolding_grpCodeFeatureOptionsHeight = 100;
        int GroupFolding_grpCodeFeatureDataIOHeight = 80;
        int GroupFolding_grpDataProviderSourcesHeight = 100;
        int GroupFolding_grpAntiWindupHeight = 100;
//        int GroupFolding_grpDevelopmentToolsHeight = 100;

        public frmMain(string[] args)
        {
            int i = 0;
            string str_path = "";
            string[] str_arr;
            string[] dum_sep = new string[1];


            InitializeComponent();

            if (args.Length != 0)
            {

                dum_sep[0] = (_dsp);
                str_path = args[0].ToString().Trim();
                str_arr = str_path.Split(dum_sep, StringSplitOptions.RemoveEmptyEntries);

                for (i = 0; i < (str_arr.Length-1); i++)
                {
                    ExternalFileOpenPath = ExternalFileOpenPath + str_arr[i] + dum_sep[0];
                }
                ExternalFileOpenPath = ExternalFileOpenPath + str_arr[i];

                ExternalFileOpenEvent = true;

            }
            else
            {
                ExternalFileOpenEvent = false;
                ExternalFileOpenPath = "";
            }

            return;
        }

        private void DebugInfoPrintLine(string str_debug)
        {
            if ((txtOutput.TextLength + str_debug.Length) > txtOutput.MaxLength) 
                txtOutput.Clear();
            txtOutput.AppendText(str_debug + "\r\n");
            txtOutput.SelectionStart = txtOutput.TextLength;
            txtOutput.ScrollToCaret();
        }

        private bool frmMain_SetAppEnvironment()
        {
            string _dps = ""; // Default Path Separator
            bool f_res = false; // function return value

            try 
            {

                // Determine App Product Version Key
                string str_dum = "";
                string[] dum_sep = new string[1];
                dum_sep[0] = ".";
                string[] str_arr = Application.ProductVersion.Split(dum_sep, StringSplitOptions.RemoveEmptyEntries); ;

                str_dum = Convert.ToInt32(str_arr[0]).ToString("#0") +
                          Convert.ToInt32(str_arr[1]).ToString("#0") +
                          Convert.ToInt32(str_arr[2]).ToString("#00");
                dcldGlobals.APP_VERSION_KEY = Convert.ToInt32(str_dum);
                
                // Initialize debug output
                // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
                DebugInfoPrintLine(
                    "================================================================\r\n" + 
                    ">Application start up..." + "\r\n" +
                    "\r\n" +
                    "Product Name:                    " + Application.ProductName.ToString() + "\r\n" +
                    "Application Version:             " + Application.ProductVersion.ToString() + " (" + dcldGlobals.APP_VERSION_KEY.ToString() + ")\r\n" +
                    "Company Name:                    " + Application.CompanyName.ToString() + "\r\n" +
                    "\r\n" +
                    "Operating System:\r\n" +
                    "    - OS Version:                " + Environment.OSVersion.ToString() + "\r\n" +
                    "    - 64-bit OS Type:            " + Environment.Is64BitOperatingSystem.ToString() + "\r\n" +
                    "    - 64-bit Process:            " + Environment.Is64BitProcess.ToString() + "\r\n" +
                    "    - .net Version:              " + Environment.Version.Major.ToString() + "." + Environment.Version.Minor.ToString() + " (" + Environment.Version.ToString() + ")" + "\r\n" +
                    "\r\nHardware Information:\r\n" +
                    "    - Machine Name:              " + Environment.MachineName.ToString() + "\r\n" +
                    "    - No of CPUs:                " + Environment.ProcessorCount.ToString() + "\r\n" +
                    "    - Mapped Memory:             " + Environment.WorkingSet.ToString() + "\r\n" +
                    "    - System Page Size:          " + Environment.SystemPageSize.ToString() + "\r\n" +
                    "\r\nSoftware Environment Information:\r\n" +
                    "    - User Domain Name:          " + Environment.UserDomainName.ToString() + "\r\n" +
                    "    - User Name:                 " + Environment.UserName.ToString() + "\r\n" +
                    "    - Current Managed Thread-ID: " + Environment.CurrentManagedThreadId.ToString() + "\r\n" +
                    "    - System Directory:          " + Environment.SystemDirectory.ToString() + "\r\n" +
                    "    - My Documents Directory:    " + Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\r\n" +
                    "    - User Interactive:          " + Environment.UserInteractive.ToString() + "\r\n" +
                    "    - Version:                   " + Environment.Version.ToString() + "\r\n" +
                    "\r\nRegional Settings:\r\n" +
                    "    - Culture Info Name:         " + CultureInfo.CurrentCulture.NativeName + "\r\n" +
                    "    - Keyboard Language:         " + Application.CurrentInputLanguage.Culture.DisplayName + "\r\n" +
                    "    - Digit Substitution:        " + CultureInfo.CurrentCulture.NumberFormat.DigitSubstitution + "\r\n" +
                    "    - NaN Symbol:                " + CultureInfo.CurrentCulture.NumberFormat.NaNSymbol + "\r\n" +
                    "    - Negative Sign:             " + CultureInfo.CurrentCulture.NumberFormat.NegativeSign + "\r\n" +
                    "    - Positive Sign:             " + CultureInfo.CurrentCulture.NumberFormat.PositiveSign + "\r\n" +
                    "    - Decimal Separator:         " + CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator + "\r\n" +
                    "    - Group Separator:           " + CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator + "\r\n" +
                    "    - Group Sizes:               " + CultureInfo.CurrentCulture.NumberFormat.NumberGroupSizes + "\r\n" +
                    "    - Floating Point Number:     " + Convert.ToDouble((float)1.234567).ToString("G") + "\r\n" +
                    "    - Decimal Point Match:       " +
                    (Convert.ToDouble((float)1.234567).ToString("G", CultureInfo.InvariantCulture) == Convert.ToDouble((float)1.234567).ToString("G", CultureInfo.CurrentCulture)).ToString() + "\r\n" +
                    "    - Large Integer Number:      " + Convert.ToInt64(1234567).ToString("G") + "\r\n" +
                    "\r\nApplication Environment Information:\r\n" +
                    "    - Application Startup Path:  " + Application.StartupPath + "\r\n" +
                    "    - Command Line:              " + Environment.CommandLine.ToString() + "\r\n" +
                    "    - Current Directory:         " + Environment.CurrentDirectory.ToString() + "\r\n" +
                    "    - Common AppData Path:       " + Application.CommonAppDataPath + "\r\n" +
                    "    - User AppData Path:         " + Application.UserAppDataPath + "\r\n" + 
                    "    - Executable Location:       " + Application.ExecutablePath + "\r\n" +
                    "    - Start-up Path:             " + Application.StartupPath + "\r\n" +
                    "================================================================\r\n" +
                    "\r\n"
                    );
                // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


                DebugInfoPrintLine(">Loading Application Components...");

                // Set ROOT DIRECTORY
                _dps = System.IO.Path.DirectorySeparatorChar.ToString();
                rootPath = Application.StartupPath;
                if (rootPath.Substring(rootPath.Length - _dps.Length, _dps.Length) != _dps) { rootPath = rootPath + _dps; }
                if(System.IO.Directory.Exists(rootPath)) 
                {
                    DebugInfoPrintLine(">Set Root Path: " + rootPath);  
                }
                else
                {
                    DebugInfoPrintLine(
                        ">Set Root Path: unable to access root directory " + rootPath + "\r\n" +
                        ">    -> Abort setting application environment component dependnecies"
                        );
                    return(false);
                }

                // Set RESOURCE DIRECTORY
                if (System.IO.Directory.Exists(rootPath + "Resources")) 
                { 
                    resourcePath = rootPath + "Resources" + _dps; 
                    DebugInfoPrintLine(">Set Resource Path: " + resourcePath);  
                }
                else
                {
                    DebugInfoPrintLine(
                        ">Set Resource Path: unable to access resource directory " + resourcePath + "\r\n" +
                        ">    -> Abort setting application environment component dependnecies"
                        );
                    return(false);
                }

                // Set APPLICATION SETTINGS FILE
                f_res = (System.IO.File.Exists(rootPath + DEFAULT_INI_FILE) && SettingsFile.SetFilename(rootPath + DEFAULT_INI_FILE));
                if (f_res)
                {
                    DebugInfoPrintLine(">Load Application Settings File: " + SettingsFile.FileName); 
                }
                else
                {
                    DebugInfoPrintLine(
                        ">Load Application Settings File: unable to access settings file " + rootPath + DEFAULT_INI_FILE + "\r\n" +
                        ">    -> Abort setting application environment component dependnecies"
                        );
                    return(false);
                }

                // Set ASSEMBLY GENERATOR SCRIPT FILE
                f_res = (System.IO.File.Exists(resourcePath + ASM_GEN_FILE) && AsmGeneratorScript.SetFilename(resourcePath + ASM_GEN_FILE));
                if (f_res)
                {
                    DebugInfoPrintLine(
                        ">Load Assembly Generator Script: " + AsmGeneratorScript.FileName + "\r\n" +
                        ">    - AGS Version: " + AsmGeneratorScript.ReadKey("generic", "Version", "N/A") + "\r\n" +
                        ">    - AGS Date:    " + AsmGeneratorScript.ReadKey("generic", "Date", "N/A") 
                        );

                }
                else
                {
                    DebugInfoPrintLine(
                        ">Load Assembly Generator Script: unable to access assembly generator script " + resourcePath + ASM_GEN_FILE + "\r\n" +
                        ">    -> Abort setting application environment component dependnecies"
                        );
                    return(false);
                }

                // Set C-CODE GENERATOR SCRIPT FILE
                f_res = (System.IO.File.Exists(resourcePath + C_GEN_FILE) && CCodeGeneratorScript.SetFilename(resourcePath + C_GEN_FILE));
                if (f_res)
                {
                    DebugInfoPrintLine(
                        ">Load ANSI C Generator Script: " + CCodeGeneratorScript.FileName + "\r\n" +
                        ">    - CGS Version: " + CCodeGeneratorScript.ReadKey("generic", "Version", "N/A") + "\r\n" +
                        ">    - CGS Date:    " + CCodeGeneratorScript.ReadKey("generic", "Date", "N/A") 
                        );

                }
                else
                {
                    DebugInfoPrintLine(
                        ">Load ANSI C Generator Script: unable to access ANSI C generator script " + resourcePath + C_GEN_FILE + "\r\n" +
                        ">    -> Abort setting application environment component dependnecies"
                        );
                    return(false);
                }

                // Set ASSEMBLY PARSER FILE
                f_res = System.IO.File.Exists(resourcePath + "ActiproSoftware.dsPICAssembly.xml");
                if (f_res)
                {
                    DebugInfoPrintLine(">Load Assembly Parser Script: " + resourcePath + "ActiproSoftware.dsPICAssembly.xml");
                    this.txtSyntaxEditorAssembly.Document.Language = ActiproSoftware.SyntaxEditor.Addons.Dynamic.DynamicSyntaxLanguage.LoadFromXml(resourcePath + "ActiproSoftware.dsPICAssembly.xml", 0);
                    this.txtSyntaxEditorAssembly.LineNumberMarginVisible = true;
                    this.txtSyntaxEditorAssembly.HideSelection = false;
                    DebugInfoPrintLine(">    -> loaded successfully");
                }
                else
                {
                    DebugInfoPrintLine(
                        ">Load Assembly Parser Script: unable to find/access ASM parser script " + resourcePath + "ActiproSoftware.dsPICAssembly.xml" + "\r\n" +
                        ">    -> Assembly code highlighting might not be available."
                        );
                }

                // Set XC16 C-CODE PARSER FILE
                f_res = System.IO.File.Exists(resourcePath + "ActiproSoftware.dsPICXC16.xml");
                if (f_res)
                {
                    DebugInfoPrintLine(">Load XC16 C-Code Parser Script: " + resourcePath + "ActiproSoftware.dsPICXC16.xml");
                    
                    this.txtSyntaxEditorCSource.Document.Language = ActiproSoftware.SyntaxEditor.Addons.Dynamic.DynamicSyntaxLanguage.LoadFromXml(resourcePath + "ActiproSoftware.dsPICXC16.xml", 0);
                    this.txtSyntaxEditorCSource.LineNumberMarginVisible = true;
                    this.txtSyntaxEditorCSource.HideSelection = false;
                    DebugInfoPrintLine(">    -> C-Source Editor: loaded successfully");

                    this.txtSyntaxEditorCHeader.Document.Language = ActiproSoftware.SyntaxEditor.Addons.Dynamic.DynamicSyntaxLanguage.LoadFromXml(resourcePath + "ActiproSoftware.dsPICXC16.xml", 0);
                    this.txtSyntaxEditorCHeader.LineNumberMarginVisible = true;
                    this.txtSyntaxEditorCHeader.HideSelection = false;
                    DebugInfoPrintLine(">    -> C-Header Editor: loaded successfully");

                    this.txtSyntaxEditorCLibHeader.Document.Language = ActiproSoftware.SyntaxEditor.Addons.Dynamic.DynamicSyntaxLanguage.LoadFromXml(resourcePath + "ActiproSoftware.dsPICXC16.xml", 0);
                    this.txtSyntaxEditorCLibHeader.LineNumberMarginVisible = true;
                    this.txtSyntaxEditorCLibHeader.HideSelection = false;
                    DebugInfoPrintLine(">    -> Library C-Header Editor: loaded successfully");

                }
                else
                {
                    DebugInfoPrintLine(
                        ">Load XC16 C-Code Parser Script: unable to find/access XC16 C-Code parser script " + resourcePath + "ActiproSoftware.dsPICXC16.xml" + "\r\n" +
                        ">    -> Assembly code highlighting might not be available."
                        );
                }

                
                // Create Default DCLD Project File 
                f_res = ProjectFile.Clear();
                NewProjectFilenameDummy = DefaultProjectFileName + DefaultProjectFileNameExtension;

                // Set USER GUIDE/HELP FILE
                UserGuideFileName = SettingsFile.ReadKey("common", "UserGuideFileName", "");
                f_res = (System.IO.File.Exists(resourcePath + UserGuideFileName) && (UserGuideFileName.Trim().Length > 4));
                if (!f_res) f_res = (System.IO.File.Exists(rootPath + UserGuideFileName) && (UserGuideFileName.Trim().Length > 4));

                if (f_res)
                {
                    UserGuideFileName = resourcePath + UserGuideFileName;
                    DebugInfoPrintLine(">Load User Guide: " + UserGuideFileName + "\r\n" +
                                   ">    -> loaded successfully");
                }
                else
                {
                    DebugInfoPrintLine(">Load User Guide: " + resourcePath + UserGuideFileName + "\r\n" +
                                   ">    -> unable to find/access user guide. File might not be availble.");
                }

                // Set return value to indicate success
                f_res = true;

            }   
            catch (Exception ex)
            {
                MessageBox.Show(
                    this, 
                    "Critical error (0x" + ex.HResult.ToString("X") + ") occured while initializing app body generator." + "\r\n" +
                    "Error Message: " + ex.Message + "\r\n" + 
                    "Check output window for details.", 
                    "Critical Error",
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error
                    );

                // Set return value to indicate failure
                f_res = false;
            }

            // Return success/failure
            return(f_res);
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            double[,] GainSeries = new double[2, 100];
            Control[] AllControls;
            int i = 0;

            // Prepare frmMain...
            this.Hide();
            Application.DoEvents();

            // Load application envirnment components
            if (!frmMain_SetAppEnvironment())
            {
                MessageBox.Show(
                    this,
                    Application.ProductName + " was not able to load all environment dependencies." + "\r\n" +
                    "Check the messages in the output window for details.",
                    "Critical Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                    );
            }

            // Create array of text boxes for pole & zero editing
            txtPole = new TextBox[MAX_FILTER_ORDER];
            txtZero = new TextBox[MAX_FILTER_ORDER];
            lblPole = new Label[MAX_FILTER_ORDER];
            lblZero = new Label[MAX_FILTER_ORDER];
            lblPoleUnit = new Label[MAX_FILTER_ORDER];
            lblZeroUnit = new Label[MAX_FILTER_ORDER];

            for (i = 0; i < txtPole.Length; i++)
            {
                AllControls = Controls.Find(("txtFP" + i.ToString()), true);
                if (AllControls.Length > 0)
                {
                    txtPole[i] = (System.Windows.Forms.TextBox)AllControls[0];
                    txtPole[i].Text = NumberTextBox_ToDouble(txtPole[i]).ToString();
                }

                AllControls = Controls.Find(("lblFP" + i.ToString()), true);
                if (AllControls.Length > 0)
                    lblPole[i] = (System.Windows.Forms.Label)AllControls[0];

                AllControls = Controls.Find(("lblFP" + i.ToString() + "Unit"), true);
                if (AllControls.Length > 0)
                    lblPoleUnit[i] = (System.Windows.Forms.Label)AllControls[0];

            }

            for (i = 1; i < txtZero.Length; i++)
            {
                AllControls = Controls.Find(("txtFZ" + i.ToString()), true);
                if (AllControls.Length > 0)
                { 
                    txtZero[i] = (System.Windows.Forms.TextBox)AllControls[0];
                    txtZero[i].Text = NumberTextBox_ToDouble(txtZero[i]).ToString();
                }

                AllControls = Controls.Find(("lblFZ" + i.ToString()), true);
                if (AllControls.Length > 0)
                    lblZero[i] = (System.Windows.Forms.Label)AllControls[0];

                AllControls = Controls.Find(("lblFZ" + i.ToString() + "Unit"), true);
                if (AllControls.Length > 0)
                    lblZeroUnit[i] = (System.Windows.Forms.Label)AllControls[0];

            }

            // Preventing number conflicts with default settings 
            txtInputGain.Text = NumberTextBox_ToDouble(txtInputGain).ToString("#0.000", CultureInfo.CurrentCulture);
            txtOutputGain.Text = NumberTextBox_ToDouble(txtOutputGain).ToString("#0.000", CultureInfo.CurrentCulture);
            txtInputDataResolution.Text = NumberTextBox_ToDouble(txtInputDataResolution).ToString();
            txtSamplingFrequency.Text = NumberTextBox_ToDouble(txtSamplingFrequency).ToString();

            // Set startup-status
            lvCoefficients.HideSelection = false;
            lvCoefficients.MultiSelect = false;
           
            // capture foldable object sizes of code generator option catalog
            GroupFolding_grpContextSavingHeight = grpContextManagement.Height;
            GroupFolding_grpCodeFeatureOptionsHeight = grpCodeFeatureOptions.Height;
            GroupFolding_grpCodeFeatureDataIOHeight = grpCodeFeatureDataIO.Height;
            GroupFolding_grpDataProviderSourcesHeight = grpDataProviderSources.Height;
            GroupFolding_grpAntiWindupHeight = grpAntiWindup.Height;
//            GroupFolding_grpDevelopmentToolsHeight = grpDevelopmentTools.Height;

            // reload last Bode chart settings
            DefaultXMin = Convert.ToDouble(SettingsFile.ReadKey("bode_plot", "x_min", DefaultXMin.ToString()));
            DefaultXMax = Convert.ToDouble(SettingsFile.ReadKey("bode_plot", "x_max", DefaultXMax.ToString()));
            DefaultY1Min = Convert.ToDouble(SettingsFile.ReadKey("bode_plot", "y1_min", DefaultY1Min.ToString()));
            DefaultY1Max = Convert.ToDouble(SettingsFile.ReadKey("bode_plot", "y1_max", DefaultY1Max.ToString()));
            DefaultY2Min = Convert.ToDouble(SettingsFile.ReadKey("bode_plot", "y2_min", DefaultY2Min.ToString()));
            DefaultY2Max = Convert.ToDouble(SettingsFile.ReadKey("bode_plot", "y2_max", DefaultY2Max.ToString()));

            // reload data table status
            showCoeffficientDataTableToolStripMenuItem.Checked = Convert.ToBoolean(Convert.ToInt32(SettingsFile.ReadKey("data_table", "visible", "1")));
            showCoeffficientDataTableToolStripMenuItem_CheckedChanged(sender, e);

            // reload body generation and timing analysis window status
            // (none)

            // reload compensator settings status
            showSourceCodeTimingToolStripMenuItem.Checked = Convert.ToBoolean(Convert.ToUInt32(SettingsFile.ReadKey("code_generator", "timing_visible", "1")));
            showSourceCodeTimingToolStripMenuItem_CheckedChanged(sender, e);

            // Set default options
            cmbQFormat.SelectedIndex = 1;   // Always select Q15 format as default
            cmbQScalingMethod.SelectedIndex = 0;   // Always select single bit-shift scaling as default
            cmbCompType.SelectedIndex = 2;  // Always select 3P3Z controller as default
            cmbLoopTriggerOption.SelectedIndex = 0; // Always select loop trigger option (ADC-Triggered) as default
            cmbTriggerPlacement.SelectedIndex = 0; // Always select 50% On-Time as default

            ShowSDomainTF = Convert.ToBoolean(Convert.ToInt32(SettingsFile.ReadKey("bode_plot", "show_s_domain", "1")));
            showSDomainTransferFunctionToolStripMenuItem.Checked = ShowSDomainTF;

            // Load dynamic control properties
            toolStripButtonMPLABXPathWarning.AutoToolTip = false;
            toolStripButtonMPLABXPathWarning.ToolTipText =
                                "The recent declaration of the MPLAB X® project directory is not valid. \r\n" +
                                "Please make sure this path is valid or empty or code generation will fail.";


            // Initialize generator file paths...
            this.txtMPLABXProjectDir.Text = "";
            this.txtASMSourcePath.Text = ""; // rootPath;
            this.txtCSourcePath.Text = ""; // rootPath;
            this.txtCHeaderPath.Text = ""; // rootPath;
            this.txtCLibPath.Text = ""; // rootPath;


            // Refresh GUI
            ApplicationStartUp = false;
            UpdateTransferFunction(this, EventArgs.Empty);
            GenerateCode(this, EventArgs.Empty);

            // create debugging output
            try
            {

                // ensure visibility of all active controls
                for (i = 0; i < cNPNZ.FilterOrder; i++)
                {
                    lblPole[i].Visible = true;
                    txtPole[i].Visible = true;
                    lblPoleUnit[i].Visible = true;
                    if (i > 0)
                    { 
                        lblZero[i].Visible = true;
                        txtZero[i].Visible = true;
                        lblZeroUnit[i].Visible = true;
                    }
                }
                this.Show();

                // Restore last user settings
                this.WindowState = (FormWindowState)Convert.ToInt32(SettingsFile.ReadKey("main_window", "winstate", "0"));
                if (WindowState == System.Windows.Forms.FormWindowState.Normal)
                {
                    this.Width = Convert.ToInt32(SettingsFile.ReadKey("main_window", "width", "1000"));
                    this.Height = Convert.ToInt32(SettingsFile.ReadKey("main_window", "height", "1000"));
                }
                
                splitContainerCoefficients.SplitterDistance = (splitContainerCoefficients.Panel1.Height + splitContainerCoefficients.Panel2.Height) - Convert.ToInt32(SettingsFile.ReadKey("data_table", "splitter_pos", "500"));
                splitContainerContents.SplitterDistance = Convert.ToInt32(SettingsFile.ReadKey("code_generator", "settings_splitter_pos", "375"));
                splitContainerTiming.SplitterDistance = (splitContainerTiming.Panel1.Height + splitContainerTiming.Panel2.Height) - Convert.ToInt32(SettingsFile.ReadKey("code_generator", "timing_splitter_pos", "248"));

                DebugInfoPrintLine(">Loading Application completed");

                Application.DoEvents();
                this.Refresh();
                Application.DoEvents();


            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    this, 
                    "Critical error (0x" + ex.HResult.ToString("X") + ") occured while initializing application." + "\r\n" +
                    "Error Message: " + ex.Message + "\r\n" + 
                    "Check output window for details.", 
                    "Critical Error",
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error
                    );
            }

            // if external file is loaded, read configuration here
            if (ExternalFileOpenEvent)
            {
                if (System.IO.File.Exists(ExternalFileOpenPath))
                {
                    DebugInfoPrintLine(">Open file by external call: " + ExternalFileOpenPath);

                    if (OpenFile(ExternalFileOpenPath))
                        DebugInfoPrintLine(">    -> File loaded successfully");
                    else
                        DebugInfoPrintLine(">    -> File loaded with errors");

                    UpdateTransferFunction(this, EventArgs.Empty);
                    GenerateCode(this, EventArgs.Empty);

                    if (this.txtMPLABXProjectDir.Text.Trim().Length == 0)
                        DebugInfoPrintLine(">MPLAB X Project Directory: " + this.txtMPLABXProjectDir.Text + "(not set)");
                    else
                        DebugInfoPrintLine(">MPLAB X Project Directory: " + this.txtMPLABXProjectDir.Text);


                }
                else
                {
                    DebugInfoPrintLine(">    -> Cannot open/access external file");
                }
                ExternalFileOpenPath = "";
                ExternalFileOpenEvent = false;
            }

            // Check if Configuraiton window should be shown by default
            showMPLABXconfigWindowAtStartup = Convert.ToBoolean(Convert.ToInt32(SettingsFile.ReadKey("config_window", "show", "1")));

            // I no MPLAB project has been specified, open project configuraition
            if (MPLABXProject.MPLABXProjectDirectory == string.Empty)
                if (showMPLABXconfigWindowAtStartup)
                    OpenProjectConfigWindow();


            return;
        }

        private void eventProjectFileChanged(object sender, EventArgs e)
        {
            ProjectFileChanged = true;
            saveToolStripMenuItem.Enabled = true;
            toolStripButtonSave.Enabled = saveToolStripMenuItem.Enabled;

            return;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                frmMain.ActiveForm.Close();
            }
            catch { /* Do nothing */ }
        }

        private void BuildOutputTable(object sender, EventArgs e)
        {
            int i=0;
            ListViewItem lvX;
            ListViewItem.ListViewSubItem lvSX;

            if (cmbCompType.Text.Trim().Length == 0) return;
            LagElements = Convert.ToInt32(cmbCompType.Text.Substring(0, 1));

            if (lblPole != null)
            { 

                for (i = 0; i < lblPole.Length; i++)
                {
                    lblPole[i].Visible = (bool)(LagElements >= (i+1));
                    txtPole[i].Visible = lblPole[i].Visible; lblPoleUnit[i].Visible = lblPole[i].Visible;
                    if (i > 0) { lblZero[i].Visible = lblPole[i].Visible; txtZero[i].Visible = lblZero[i].Visible; lblZeroUnit[i].Visible = lblZero[i].Visible; }
                }
            }

            lvCoefficients.Items.Clear();
            lvCoefficients.Groups.Add("ACoeff", "A-Coefficients");

            for (i = 1; i <= LagElements; i++)
            {
                lvX = lvCoefficients.Items.Add("A" + Convert.ToString(i));
                lvX.Name = "A" + Convert.ToString(i);
                lvSX = lvX.SubItems.Add("0.0000");     // Floating Point Coefficient
                lvSX.Name = "floatA" + Convert.ToString(i);
                lvSX = lvX.SubItems.Add("0");          // Bit-shifting log_decade_scaler of this coefficient (according to Q-Format selected)
                lvSX.Name = "scalerA" + Convert.ToString(i);
                lvSX = lvX.SubItems.Add("0.0000");     // Scaled Floating Point Coefficient (according to Q-Format selected)
                lvSX.Name = "sclfloatA" + Convert.ToString(i);
                lvSX = lvX.SubItems.Add("0.0000");     // Fractional Coefficient (according to Q-Format selected)
                lvSX.Name = "fractA" + Convert.ToString(i);
                lvSX = lvX.SubItems.Add("0.000");     // Fixed Point Error after conversion
                lvSX.Name = "fperrA" + Convert.ToString(i);
                lvSX = lvX.SubItems.Add("0");          // Singed Integer value of the scaled coefficient (according to Q-Format selected)
                lvSX.Name = "intA" + Convert.ToString(i);
                lvSX = lvX.SubItems.Add("0");          // Unsinged Integer value of the scaled coefficient (according to Q-Format selected)
                lvSX.Name = "uintA" + Convert.ToString(i);
                lvSX = lvX.SubItems.Add("0x0000");     // Hexadecimal value of the scaled coefficient (according to Q-Format selected)
                lvSX.Name = "hexA" + Convert.ToString(i);
                lvSX = lvX.SubItems.Add("00000000 00000000");     // Binary body of the scaled coefficient (according to Q-Format selected)
                lvSX.Name = "binA" + Convert.ToString(i);
                lvX.Group = lvCoefficients.Groups["ACoeff"];
            }
            
            lvCoefficients.Groups.Add("BCoeff", "B-Coefficients");

            for (i = 0; i <= LagElements; i++)
            {
                lvX = lvCoefficients.Items.Add("B" + Convert.ToString(i));
                lvX.Name = "B" + Convert.ToString(i);
                lvSX = lvX.SubItems.Add("0.0000");     // Floating Point Coefficient
                lvSX.Name = "floatB" + Convert.ToString(i);
                lvSX = lvX.SubItems.Add("0");          // Bit-shifting log_decade_scaler of this coefficient (according to Q-Format selected)
                lvSX.Name = "scalerB" + Convert.ToString(i);
                lvSX = lvX.SubItems.Add("0.0000");     // Scaled Floating Point Coefficient (according to Q-Format selected)
                lvSX.Name = "sclfloatB" + Convert.ToString(i);
                lvSX = lvX.SubItems.Add("0.0000");     // Fractional Coefficient (according to Q-Format selected)
                lvSX.Name = "fractB" + Convert.ToString(i);
                lvSX = lvX.SubItems.Add("0.000");     // Fixed Point Error after conversion
                lvSX.Name = "fperrB" + Convert.ToString(i);
                lvSX = lvX.SubItems.Add("0");          // Singed Integer value of the scaled coefficient (according to Q-Format selected)
                lvSX.Name = "intB" + Convert.ToString(i);
                lvSX = lvX.SubItems.Add("0");          // Unsinged Integer value of the scaled coefficient (according to Q-Format selected)
                lvSX.Name = "uintB" + Convert.ToString(i);
                lvSX = lvX.SubItems.Add("0x0000");     // Hexadecimal value of the scaled coefficient (according to Q-Format selected)
                lvSX.Name = "hexB" + Convert.ToString(i);
                lvSX = lvX.SubItems.Add("00000000 00000000");     // Binary body of the scaled coefficient (according to Q-Format selected)
                lvSX.Name = "binB" + Convert.ToString(i);
                lvX.Group = lvCoefficients.Groups["BCoeff"];
            }

            if ((clsCompensatorNPNZ.dcldScalingMethod)Convert.ToInt32(cmbQScalingMethod.Text.Substring(0, 1)) == clsCompensatorNPNZ.dcldScalingMethod.DCLD_SCLMOD_OUTPUT_SCALING_FACTOR)
            {
                lvCoefficients.Groups.Add("OutputSclFct", "Output Scaling Factor");

                lvX = lvCoefficients.Items.Add("Scale Factor");
                lvX.Name = "OutSclFct";
                lvSX = lvX.SubItems.Add("0.0000");     // Floating Point Coefficient
                lvSX.Name = "floatOSF";
                lvSX = lvX.SubItems.Add("0");          // Bit-shifting log_decade_scaler of this coefficient (according to Q-Format selected)
                lvSX.Name = "scalerOSF";
                lvSX = lvX.SubItems.Add("0.0000");     // Scaled Floating Point Coefficient (according to Q-Format selected)
                lvSX.Name = "sclfloatOSF";
                lvSX = lvX.SubItems.Add("0.0000");     // Fractional Coefficient (according to Q-Format selected)
                lvSX.Name = "fractOSF";
                lvSX = lvX.SubItems.Add("0.000");     // Fixed Point Error after conversion
                lvSX.Name = "fperrOSF";
                lvSX = lvX.SubItems.Add("0");          // Singed Integer value of the scaled coefficient (according to Q-Format selected)
                lvSX.Name = "intOSF";
                lvSX = lvX.SubItems.Add("0");          // Unsinged Integer value of the scaled coefficient (according to Q-Format selected)
                lvSX.Name = "uintOSF";
                lvSX = lvX.SubItems.Add("0x0000");     // Hexadecimal value of the scaled coefficient (according to Q-Format selected)
                lvSX.Name = "hexOSF";
                lvSX = lvX.SubItems.Add("00000000 00000000");     // Binary body of the scaled coefficient (according to Q-Format selected)
                lvSX.Name = "binOSF";
                lvX.Group = lvCoefficients.Groups["OutputSclFct"];
            
            }

            lvCoefficients.ShowGroups = true;
    
        }

        private void UpdatePTermController(object sender, EventArgs e)
        {
            cNPNZ.PTermNominalFeedback = Convert.ToInt32(txtPTermNominalFeedback.Text);
            cNPNZ.PTermNominalControlOutput = Convert.ToInt32(Convert.ToDouble(txtPTermNominalOutput.Text));
            txtPTermFactor.Text = NumberBaseConverter.Dec2Fractional(cNPNZ.PTermFactor, Q_format).ToString(CultureInfo.CurrentCulture);
            txtPTermScaler.Text = cNPNZ.PTermScaler.ToString(CultureInfo.CurrentCulture);
        }

        private void UpdateTransferFunction(object sender, EventArgs e)
        {
            int i = 0;
            bool valid_data_entry = false;
            string str_buf = "";

            var watch = System.Diagnostics.Stopwatch.StartNew();

            try
            {
            
                if (ApplicationStartUp) return;     // During the startup-phase o fhte application, suppress all updates
                if (ProjectFileLoadActive) return;         // If settings are loaded from a file, suppress all updates
                if (cmbCompType.Text.Trim().Length == 0) return;

                // Reset flags
                UpdateWarning = false;
                UpdateComplete = false;
                cNPNZ.AutoUpdate = false;

                eventProjectFileChanged(sender, e);
                stbProgressBarLabel.Text = "Updating Results:";
                stbProgressBarLabel.Visible = true;
                stbProgressBar.Visible = true;
                stbProgressBar.Value = 0;

                Application.DoEvents();

                // determine lag elements and detect changes to previous settings
                LagElements = Convert.ToInt32(cmbCompType.Text.Substring(0, 1));
                if (preLagElements != LagElements) FilterTypeChanged = true;
                preLagElements = LagElements;

                // determine scaling mode and number format and detect changes to previous settings
                ScalingMode = (cmbQScalingMethod.SelectedIndex);
                Q_format = (int)(((cmbQFormat.SelectedIndex + 1) * 8) - 1);
                if ((preQFormat != Q_format) || (preScalingMode != ScalingMode)) ScalingChanged = true;
                preQFormat = Q_format;
                preScalingMode = ScalingMode;

                // read error tolerance levels for coefficient fixed-point error analysis
                MaxError = Convert.ToDouble(taboptMaxFPError.Text) / 100.0;
                WarnError = Convert.ToDouble(taboptMaxFPErrorWarning.Text) / 100.0;

                // set filter order
                cNPNZ.FilterOrder = LagElements;

                // if filter order has changed, rebuild enter mask for poles and zeros
                for (i = 0; i < MAX_FILTER_ORDER; i++)
                {
                    
                    // Destroy and rebuild user pole&zero settings if filter type has changed
                    if (FilterTypeChanged)
                    {
                        UserPoleSettings[i] = new clsPoleZeroObject();
                        if (i > 0) UserZeroSettings[i] = new clsPoleZeroObject();
                    }

                    // Update user pole & zero frequencies and conversion settings
                    UserPoleSettings[i].Frequency = NumberTextBox_ToDouble(txtPole[i]); 
                    if (i > 0) UserZeroSettings[i].Frequency = NumberTextBox_ToDouble(txtZero[i]); 
                }

                stbProgressBar.Value = 10;
                Application.DoEvents();

                // if scaling mode has changed, set output format
                if (ScalingChanged)
                { 
                    switch (Q_format)
                    {
                        case 7:  QFormatStr = Q7Format; break;
                        case 15: QFormatStr = Q15Format; break;
                        case 23: QFormatStr = Q23Format; break;
                        case 31: QFormatStr = Q31Format; break;
                        default: QFormatStr = Q15Format; break;
                    }
                    // Set Q-Number format
                    cNPNZ.QFormat = Q_format; 

                    // if scaling mode is not set, through error message to user
                    if (cmbQScalingMethod.Text == "")
                    {
                        MessageBox.Show(this, "Please select a factor scaling method.\t", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        cmbQScalingMethod.Select();
                        return;
                    }

                }

                stbProgressBar.Value = 20;
                Application.DoEvents();

                // In case another compensator or scaling method has been selected, update data table format
                if (FilterTypeChanged || ScalingChanged)
                { 
                    // update file prefix if automatied prefix is used (user labels won't get overwritten
                    if ((txtControllerNamePrefix.Text == DefaultFilePrefix) || (txtControllerNamePrefix.Text.Trim() == string.Empty))
                        txtControllerNamePrefix.Text = "c" + LagElements + "p" + LagElements + "z";

                    // update default name prefixes for body generation
                    DefaultFilePrefix = "c" + LagElements + "p" + LagElements + "z";
                    DefaultVariablePrefix = "c" + LagElements + "P" + LagElements + "Z";

                    // Destroy and rebuild the coefficient output table
                    BuildOutputTable(sender, e);

                }

                stbProgressBar.Value = 30;
                Application.DoEvents();

                // if number of lag elements is within range of supported filter orders, configure cNPNZ class accordingly 
                if (LagElements<=MAX_FILTER_ORDER)
                {

                    cNPNZ.ScalingMethod = (clsCompensatorNPNZ.dcldScalingMethod)Convert.ToInt32(cmbQScalingMethod.Text.Substring(0, 1));
                    cNPNZ.QFormat = Convert.ToInt32(((cmbQFormat.SelectedIndex + 1) * 8) - 1);

                    lblTickRate.Text = (1000000.0 / NumberTextBox_ToDouble(txtSamplingFrequency)).ToString("#0.000", CultureInfo.CurrentCulture);
                    lblBDInputGain.Text = (NumberTextBox_ToDouble(txtInputGain)).ToString("#0.000", CultureInfo.CurrentCulture);
                    lblBDInputResolution.Text = (NumberTextBox_ToDouble(txtInputDataResolution)).ToString("#0", CultureInfo.CurrentCulture);
                    lblBDReferenceResolution.Text = (16.0).ToString("#0", CultureInfo.CurrentCulture);
                    lblPreScaler.Text = (cNPNZ.QFormat - cNPNZ.InputDataResolution).ToString();
                    str_buf = cmbCompType.Text;
                    lblCompTypeDescr.Text = str_buf.Substring(0, str_buf.IndexOf("-")).Trim();
                    lblFilterOrder.Text = cNPNZ.FilterOrder.ToString("#0", CultureInfo.CurrentCulture);

                    cNPNZ.SamplingFrequency = NumberTextBox_ToDouble(txtSamplingFrequency); // Convert.ToDouble(txtSamplingFrequency.Text);
                    cNPNZ.InputDataResolution = NumberTextBox_ToDouble(txtInputDataResolution); // Convert.ToDouble(txtInputDataResolution.Text);
                    cNPNZ.InputGainNormalization = chkNormalizeInputGain.Checked;
                    cNPNZ.InputGain = NumberTextBox_ToDouble(txtInputGain); // Convert.ToDouble(txtInputGain.Text);
                    cNPNZ.IsBidirectional = chkBiDirectionalFeedback.Checked;
                    if (cNPNZ.IsBidirectional)
                        cNPNZ.FeedbackRecitification = (chkFeedbackRectification.Checked & chkFeedbackRectification.Enabled);
                    else
                        cNPNZ.FeedbackRecitification = false;

                    cNPNZ.OutputGainNormalization = chkNormalizeOutputGain.Checked;
                    cNPNZ.OutputGain = NumberTextBox_ToDouble(txtOutputGain);

                    for (i = 0; i < LagElements; i++)
                    {
                        valid_data_entry = Convert.ToBoolean(UserPoleSettings[i].Frequency > 0);
                        if (valid_data_entry) cNPNZ.Pole[i].Frequency = UserPoleSettings[i].Frequency;
                        else break;

                        if (i > 0)
                        {
                            valid_data_entry = Convert.ToBoolean(UserZeroSettings[i].Frequency > 0);
                            if (valid_data_entry) cNPNZ.Zero[i].Frequency = UserZeroSettings[i].Frequency;
                            else break;
                        }
                    }

                    if (!valid_data_entry)
                    {
                        // skip data update if data entry is not complet.
                        stbMainStatusLabel.Text = "Invalid data entry. Data update terminated.";
                        stbMainStatusLabel.Image = dcld.Properties.Resources.icon_exclamation.ToBitmap();
                        stbMainStatusLabel.BackColor = stbMain.BackColor;

                        // Reset control flags
                        FilterTypeChanged = false;
                        ScalingChanged = false;
                        UpdateComplete = true;

                        return;
                    }

                    stbProgressBar.Value = 60;
                    Application.DoEvents();

                    ForceCoefficientsUpdate(sender, e);

                    // output data on Block Diagram view
                    switch(cNPNZ.ScalingMethod)
                    {
                        case  clsCompensatorNPNZ.dcldScalingMethod.DCLD_SCLMOD_SINGLE_BIT_SHIFT:
                            lblPostScaler.Text = cNPNZ.CoeffB[0].QScaler.ToString("#0", CultureInfo.CurrentCulture);
                            str_buf = "1";
                            break;

                        case clsCompensatorNPNZ.dcldScalingMethod.DCLD_SCLMOD_OUTPUT_SCALING_FACTOR:
                            lblPostScaler.Text = cNPNZ.OutputScalingFactor.Float64.ToString("#0.000", CultureInfo.CurrentCulture);
                            str_buf = "1";
                            break;

                        case clsCompensatorNPNZ.dcldScalingMethod.DCLD_SCLMOD_DUAL_BIT_SHIFT:
                            lblPostScaler.Text = cNPNZ.CoeffA[1].QScaler.ToString("#0", CultureInfo.CurrentCulture) + "/" + cNPNZ.CoeffB[1].QScaler.ToString("#0", CultureInfo.CurrentCulture);
                            str_buf = "2";
                            break;

                        case clsCompensatorNPNZ.dcldScalingMethod.DCLD_SCLMOD_DBLSCL_FLOAT:
                            lblPostScaler.Text = "n/a";
                            str_buf = "2";
                            break;

                        default:
                            lblPostScaler.Text = "n/a";
                            str_buf = "n/a";
                            break;
                    }

                    lblWorkflowInfo.Text =
                        "Filter Steps: " + (2 * cNPNZ.FilterOrder + 1).ToString("#0", CultureInfo.CurrentCulture) + "\r\n" +
                        "Number of Accumulators used: " + str_buf + "\r\n" +
                        "Sample History Depth: " + (cNPNZ.FilterOrder + 1).ToString("#0", CultureInfo.CurrentCulture) + "\r\n" +
                        "Sample History Timespan: " + (1e+6 * (cNPNZ.FilterOrder + 1) * cNPNZ.SamplingInterval).ToString("#0.000 usec", CultureInfo.CurrentCulture) + "\r\n";

                    // plot debug info to output window
                    DebugInfoPrintLine(">" + cNPNZ.DebugInfo.ToString().Replace("\r\n", "\r\n>"));

                    // Update displayed data => A-Coefficients
                    for (i = 1; i <= LagElements; i++)
                    {
                        // Print coefficient values in float, float- and Q-scaled format
                        lvCoefficients.Items["A" + i.ToString()].SubItems["floatA" + i.ToString()].Text = String.Format(QFormatStr, cNPNZ.CoeffA[i].Float64);
                        lvCoefficients.Items["A" + i.ToString()].SubItems["sclfloatA" + i.ToString()].Text = String.Format(QFormatStr, cNPNZ.CoeffA[i].FloatScaledFixedPoint);
                        lvCoefficients.Items["A" + i.ToString()].SubItems["fractA" + i.ToString()].Text = String.Format(QFormatStr, cNPNZ.CoeffA[i].QFractional);

                        // Print coefficient values in signed and unsigned integer, hex and binary format
                        lvCoefficients.Items["A" + i.ToString()].SubItems["intA" + i.ToString()].Text = String.Format("{0:#,#}", NumberBaseConverter.Fractional2Dec(cNPNZ.CoeffA[i].QFractional, (int)Q_format, true));
                        lvCoefficients.Items["A" + i.ToString()].SubItems["uintA" + i.ToString()].Text = String.Format("{0:#,#}", NumberBaseConverter.Fractional2Dec(cNPNZ.CoeffA[i].QFractional, (int)Q_format, false));

                        if (cNPNZ.ScalingMethod == clsCompensatorNPNZ.dcldScalingMethod.DCLD_SCLMOD_DBLSCL_FLOAT)
                        {
                            lvCoefficients.Items["A" + i.ToString()].SubItems["hexA" + i.ToString()].Text = NumberBaseConverter.Fractional2Hex(cNPNZ.CoeffA[i].QFractional, (int)Q_format, (int)(4 * cNPNZ.QHexWidth), true, true) + NumberBaseConverter.Dec2Hex(cNPNZ.CoeffA[i].QScaler, 16, true, false);
                            lvCoefficients.Items["A" + i.ToString()].SubItems["binA" + i.ToString()].Text = NumberBaseConverter.Fractional2Bin(cNPNZ.CoeffA[i].QFractional, (int)Q_format, (int)(cNPNZ.QBinWidth), true, true) + " " + NumberBaseConverter.Dec2Bin(cNPNZ.CoeffA[i].QScaler, 16, true, false);
                        }
                        else
                        {
                            lvCoefficients.Items["A" + i.ToString()].SubItems["hexA" + i.ToString()].Text = NumberBaseConverter.Fractional2Hex(cNPNZ.CoeffA[i].QFractional, (int)Q_format, (int)(4 * cNPNZ.QHexWidth), true, true);
                            lvCoefficients.Items["A" + i.ToString()].SubItems["binA" + i.ToString()].Text = NumberBaseConverter.Fractional2Bin(cNPNZ.CoeffA[i].QFractional, (int)Q_format, (int)(cNPNZ.QBinWidth), true, true);
                        }

                        lvCoefficients.Items["A" + i.ToString()].SubItems["scalerA" + i.ToString()].Text = cNPNZ.CoeffA[i].QScaler.ToString();

                        // Plot error analysis data with colored warnings
                        lvCoefficients.Items["A" + i.ToString()].SubItems["fperrA" + i.ToString()].Text = String.Format("{0:#0.000%}", cNPNZ.CoeffA[i].FixedPointErr);
                        if (Math.Abs(cNPNZ.CoeffA[i].FixedPointErr) > MaxError) { lvCoefficients.Items["A" + i.ToString()].BackColor = AlertBackground; UpdateWarning = true; }
                        else if (Math.Abs(cNPNZ.CoeffA[i].FixedPointErr) >= WarnError) { lvCoefficients.Items["A" + i.ToString()].BackColor = WarningBackground; }
                        else { lvCoefficients.Items["A" + i.ToString()].BackColor = lvCoefficients.BackColor; }
                    }

                    stbProgressBar.Value = 70;
                    Application.DoEvents();

                    for (i = 0; i <= LagElements; i++)
                    {
                        // Print coefficient values in float, float- and Q-scaled format
                        lvCoefficients.Items["B" + i.ToString()].SubItems["floatB" + i.ToString()].Text = String.Format(QFormatStr, cNPNZ.CoeffB[i].Float64);
                        lvCoefficients.Items["B" + i.ToString()].SubItems["sclfloatB" + i.ToString()].Text = String.Format(QFormatStr, cNPNZ.CoeffB[i].FloatScaledFixedPoint);
                        lvCoefficients.Items["B" + i.ToString()].SubItems["fractB" + i.ToString()].Text = String.Format(QFormatStr, cNPNZ.CoeffB[i].QFractional);

                        // Print coefficient values in signed and unsigned integer, hex and binary format
                        lvCoefficients.Items["B" + i.ToString()].SubItems["intB" + i.ToString()].Text = String.Format("{0:#,#}", NumberBaseConverter.Fractional2Dec(cNPNZ.CoeffB[i].QFractional, (int)Q_format, true));
                        lvCoefficients.Items["B" + i.ToString()].SubItems["uintB" + i.ToString()].Text = String.Format("{0:#,#}", NumberBaseConverter.Fractional2Dec(cNPNZ.CoeffB[i].QFractional, (int)Q_format, false));

                        if (cNPNZ.ScalingMethod == clsCompensatorNPNZ.dcldScalingMethod.DCLD_SCLMOD_DBLSCL_FLOAT)
                        {
                            lvCoefficients.Items["B" + i.ToString()].SubItems["hexB" + i.ToString()].Text = NumberBaseConverter.Fractional2Hex(cNPNZ.CoeffB[i].QFractional, (int)Q_format, (int)(4 * cNPNZ.QHexWidth), true, true) + NumberBaseConverter.Dec2Hex(cNPNZ.CoeffB[i].QScaler, 16, true, false);
                            lvCoefficients.Items["B" + i.ToString()].SubItems["binB" + i.ToString()].Text = NumberBaseConverter.Fractional2Bin(cNPNZ.CoeffB[i].QFractional, (int)Q_format, (int)(cNPNZ.QBinWidth), true, true) + " " + NumberBaseConverter.Dec2Bin(cNPNZ.CoeffB[i].QScaler, 16, true, false);
                        }
                        else
                        {
                            lvCoefficients.Items["B" + i.ToString()].SubItems["hexB" + i.ToString()].Text = NumberBaseConverter.Fractional2Hex(cNPNZ.CoeffB[i].QFractional, (int)Q_format, (int)(4 * cNPNZ.QHexWidth), true, true);
                            lvCoefficients.Items["B" + i.ToString()].SubItems["binB" + i.ToString()].Text = NumberBaseConverter.Fractional2Bin(cNPNZ.CoeffB[i].QFractional, (int)Q_format, (int)(cNPNZ.QBinWidth), true, true);
                        }

                        lvCoefficients.Items["B" + i.ToString()].SubItems["scalerB" + i.ToString()].Text = cNPNZ.CoeffB[i].QScaler.ToString();

                        // Plot error analysis data with colored warnings
                        lvCoefficients.Items["B" + i.ToString()].SubItems["fperrB" + i.ToString()].Text = String.Format("{0:#0.000%}", cNPNZ.CoeffB[i].FixedPointErr);
                        if (Math.Abs(cNPNZ.CoeffB[i].FixedPointErr) > MaxError) { lvCoefficients.Items["B" + i.ToString()].BackColor = AlertBackground; UpdateWarning = true; }
                        else if (Math.Abs(cNPNZ.CoeffB[i].FixedPointErr) >= WarnError) { lvCoefficients.Items["B" + i.ToString()].BackColor = WarningBackground; }
                        else { lvCoefficients.Items["B" + i.ToString()].BackColor = lvCoefficients.BackColor; }
                    }

                    stbProgressBar.Value = 80;
                    Application.DoEvents();

                    if (cNPNZ.ScalingMethod == clsCompensatorNPNZ.dcldScalingMethod.DCLD_SCLMOD_OUTPUT_SCALING_FACTOR)
                    { 
                        lvCoefficients.Items["OutSclFct"].SubItems["floatOSF"].Text = String.Format(QFormatStr, cNPNZ.OutputScalingFactor.Float64);
                        lvCoefficients.Items["OutSclFct"].SubItems["sclfloatOSF"].Text = String.Format(QFormatStr, cNPNZ.OutputScalingFactor.FloatScaledFixedPoint);
                        lvCoefficients.Items["OutSclFct"].SubItems["fractOSF"].Text = String.Format(QFormatStr, cNPNZ.OutputScalingFactor.QFractional);
                        lvCoefficients.Items["OutSclFct"].SubItems["fperrOSF"].Text = String.Format("{0:#0.000%}", cNPNZ.OutputScalingFactor.FixedPointErr);
                        if (Math.Abs(cNPNZ.OutputScalingFactor.FixedPointErr) > MaxError) { lvCoefficients.Items["OutSclFct"].BackColor = AlertBackground; UpdateWarning = true; }
                        else if (Math.Abs(cNPNZ.OutputScalingFactor.FixedPointErr) >= WarnError) { lvCoefficients.Items["OutSclFct"].BackColor = WarningBackground; }
                        else { lvCoefficients.Items["OutSclFct"].BackColor = lvCoefficients.BackColor; }

                        lvCoefficients.Items["OutSclFct"].SubItems["intOSF"].Text = String.Format("{0:#,#}", NumberBaseConverter.Fractional2Dec(cNPNZ.OutputScalingFactor.QFractional, (int)Q_format, true));
                        lvCoefficients.Items["OutSclFct"].SubItems["uintOSF"].Text = String.Format("{0:#,#}", NumberBaseConverter.Fractional2Dec(cNPNZ.OutputScalingFactor.QFractional, (int)Q_format, false));
                        lvCoefficients.Items["OutSclFct"].SubItems["hexOSF"].Text = NumberBaseConverter.Fractional2Hex(cNPNZ.OutputScalingFactor.QFractional, (int)Q_format, (int)(4 * cNPNZ.QHexWidth), true, true);
                        lvCoefficients.Items["OutSclFct"].SubItems["binOSF"].Text = NumberBaseConverter.Fractional2Bin(cNPNZ.OutputScalingFactor.QFractional, (int)Q_format, (int)(cNPNZ.QBinWidth), true, true);

                        lvCoefficients.Items["OutSclFct"].SubItems["scalerOSF"].Text = cNPNZ.OutputScalingFactor.QScaler.ToString();
                    }      

                }
                else
                {
                    MessageBox.Show(this, "Selected controller not supported yet.", "Beta Limitation Warning", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }

                // show success message in status bar
                if (!UpdateWarning)
                { 
                    stbMainStatusLabel.Text = "Coefficients generated successfully";
                    stbMainStatusLabel.Image = dcld.Properties.Resources.icon_ready.ToBitmap();
                    stbMainStatusLabel.BackColor = stbMain.BackColor;
                }
                else
                {
                    stbMainStatusLabel.Text = "Coefficients have been generated with warnings";
                    stbMainStatusLabel.Image = dcld.Properties.Resources.icon_exclamation.ToBitmap();
                    stbMainStatusLabel.BackColor = stbMain.BackColor;
                }

                // Update Bode Plot
                if (FilterTypeChanged)
                {
                    SetBodePlotData(sender, e, true, FilterTypeChanged);
                    if (FreezeBodeCursor)
                    {
                        chartBode_UpdateCursorMeasurement(sender, false, false);
                    }
                    else 
                    {
                        chartBode_ResetCursorMeasurement(sender, true);
                    }
                }
                else
                {
                    UpdateBodePlot(sender, e, true);
                }

            
            }   // end of try
            catch 
            {
                // show failure message in status bar
                stbMainStatusLabel.Text = "Invalid number format detected - results may be corrupted";
                stbMainStatusLabel.Image = dcld.Properties.Resources.icon_critical.ToBitmap();
                stbMainStatusLabel.BackColor = AlertBackground;
            }
            
            
            // Reset control flags
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds.ToString(CultureInfo.CurrentCulture);
            stbMainProgressTime.Text = "Refresh Period: " + elapsedMs + " ms";

            FilterTypeChanged = false;
            ScalingChanged = false;
            UpdateComplete = true;

            stbProgressBar.Value = 100;
            Application.DoEvents();
            stbProgressBar.Visible = false;
            stbProgressBarLabel.Visible = false;

            return;
        
        }

        private bool SetBodePlotData(object sender, EventArgs e, bool ForceScales, bool ForceAnnotations)
        {

            int i = 0;
            double decades = 0;
            HorizontalLineAnnotation h_anno = null;
            VerticalLineAnnotation v_anno = null;

            // Load gain and phase plots
            chartBode.Series[0].Points.DataBindXY(cNPNZ.TransferFunction.FrequencyPoint, cNPNZ.TransferFunction.MagGain_z);
            chartBode.Series[1].Points.DataBindXY(cNPNZ.TransferFunction.FrequencyPoint, cNPNZ.TransferFunction.MagPhase_z);
            if (ShowSDomainTF)
            {
                chartBode.Series[2].Points.DataBindXY(cNPNZ.TransferFunction.FrequencyPoint, cNPNZ.TransferFunction.MagGain_s);
                chartBode.Series[3].Points.DataBindXY(cNPNZ.TransferFunction.FrequencyPoint, cNPNZ.TransferFunction.MagPhase_s);
            }
            else
            {
                chartBode.Series[2].Points.Clear();
                chartBode.Series[3].Points.Clear();
            }

            if (ForceScales)
            {
                // Set scales to default
                DefaultXMax = cNPNZ.SamplingFrequency/2;
                chartBode.ChartAreas["GainPhase"].AxisX.Minimum = DefaultXMin;
                chartBode.ChartAreas["GainPhase"].AxisX.Maximum = DefaultXMax;
                chartBode.ChartAreas["GainPhase"].AxisX.IsLogarithmic = true;
                chartBode.ChartAreas["GainPhase"].AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dash;
                chartBode.ChartAreas["GainPhase"].AxisX.MinorGrid.LineWidth = 1;
                chartBode.ChartAreas["GainPhase"].AxisX.MinorGrid.LineDashStyle = ChartDashStyle.Dot;
                chartBode.ChartAreas["GainPhase"].AxisX.MinorGrid.LineWidth = 1;

                chartBode.ChartAreas["GainPhase"].AxisY.Minimum = DefaultY1Min;
                chartBode.ChartAreas["GainPhase"].AxisY.Maximum = DefaultY1Max;
                chartBode.ChartAreas["GainPhase"].AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dash;
                chartBode.ChartAreas["GainPhase"].AxisY.MinorGrid.LineWidth = 1;
                chartBode.ChartAreas["GainPhase"].AxisY.MinorGrid.LineDashStyle = ChartDashStyle.Dot;
                chartBode.ChartAreas["GainPhase"].AxisY.MinorGrid.LineWidth = 1;

                chartBode.ChartAreas["GainPhase"].AxisY2.Minimum = DefaultY2Min;
                chartBode.ChartAreas["GainPhase"].AxisY2.Maximum = DefaultY2Max;
                chartBode.ChartAreas["GainPhase"].AxisY2.MajorGrid.LineDashStyle = ChartDashStyle.Dash;
                chartBode.ChartAreas["GainPhase"].AxisY2.MinorGrid.LineWidth = 1;
                chartBode.ChartAreas["GainPhase"].AxisY2.MinorGrid.LineDashStyle = ChartDashStyle.Dot;
                chartBode.ChartAreas["GainPhase"].AxisY2.MinorGrid.LineWidth = 1;

            }

            if (ForceAnnotations)
            { 

                chartBode.Annotations.Clear();

                h_anno = GetHline(chartBode, 0, "ZeroMarker", Color.DarkGray, 2, ChartDashStyle.Solid, 0, false);
                chartBode.Annotations.Add(h_anno);

                for (i = 0; i < LagElements; i++)
                {
                    v_anno = GetVline(chartBode, 0, "Pole" + i.ToString(), Color.Teal, 2, ChartDashStyle.DashDotDot, cNPNZ.Pole[i].Frequency);
                    v_anno.Tag = txtPole[i];
                    chartBode.Annotations.Add(v_anno);
                }
                for (i = 1; i < LagElements; i++)
                {
                    v_anno = GetVline(chartBode, 0, "Zero" + i.ToString(), Color.Green, 2, ChartDashStyle.DashDotDot, cNPNZ.Zero[i].Frequency);
                    v_anno.Tag = txtZero[i];
                    chartBode.Annotations.Add(v_anno);
                }

            }

            // Set Cursors
            chartBode.ChartAreas["GainPhase"].CursorX.AxisType = AxisType.Primary;
            chartBode.ChartAreas["GainPhase"].CursorX.IsUserEnabled = true;
            chartBode.ChartAreas["GainPhase"].CursorX.LineWidth = 1;
            chartBode.ChartAreas["GainPhase"].CursorX.LineDashStyle = ChartDashStyle.DashDotDot;
            chartBode.ChartAreas["GainPhase"].CursorX.LineColor = Color.LimeGreen;

            decades = Math.Log10(cNPNZ.TransferFunction.FrequencyPoint[cNPNZ.TransferFunction.DataPoints-1] - cNPNZ.TransferFunction.FrequencyPoint[0]);

            chartBode.ChartAreas["GainPhase"].CursorX.Interval = decades / (cNPNZ.TransferFunction.FrequencyPoint.Length);
            chartBode.ChartAreas["GainPhase"].CursorX.AutoScroll = true;

            chartBode.ChartAreas["GainPhase"].CursorY.AxisType = AxisType.Primary;
            chartBode.ChartAreas["GainPhase"].CursorY.IsUserEnabled = true;
            chartBode.ChartAreas["GainPhase"].CursorY.LineWidth = 1;
            chartBode.ChartAreas["GainPhase"].CursorY.LineDashStyle = ChartDashStyle.DashDotDot;
            chartBode.ChartAreas["GainPhase"].CursorY.LineColor = Color.LimeGreen;


            return (true);

        }

        private bool UpdateBodePlot(object sender, EventArgs e, bool ForceAnnotationUpdate = false)
        {
            int i = 0;
            Annotation v_anno;

            try
            {
                if (chartBode == null) return (false);
                if (chartBode.Series == null) return (false);
                if (chartBode.Series.Count == 0) return (false);

                // Load data into data series
                chartBode.Series[0].Points.DataBindXY(cNPNZ.TransferFunction.FrequencyPoint, cNPNZ.TransferFunction.MagGain_z);
                chartBode.Series[1].Points.DataBindXY(cNPNZ.TransferFunction.FrequencyPoint, cNPNZ.TransferFunction.MagPhase_z);

                if (ShowSDomainTF)
                {
                    chartBode.Series[2].Points.DataBindXY(cNPNZ.TransferFunction.FrequencyPoint, cNPNZ.TransferFunction.MagGain_s);
                    chartBode.Series[3].Points.DataBindXY(cNPNZ.TransferFunction.FrequencyPoint, cNPNZ.TransferFunction.MagPhase_s);
                }
                else
                {
                    chartBode.Series[2].Points.Clear(); // .Points.DataBindXY(cNPNZ.TransferFunction.FrequencyPoint, cNPNZ.TransferFunction.MagGain_s);
                    chartBode.Series[3].Points.Clear(); // .Points.DataBindXY(cNPNZ.TransferFunction.FrequencyPoint, cNPNZ.TransferFunction.MagPhase_s);
                }

                if (ForceAnnotationUpdate)
                {
                    for (i = 0; i < cNPNZ.Pole.Length; i++)
                    {
                        v_anno = chartBode.Annotations.FindByName("Pole" + i.ToString());
                        if (v_anno != null)
                            if (v_anno.Name.Contains("Pole"))
                                if (v_anno.X != cNPNZ.Pole[i].Frequency)
                                    v_anno.X = cNPNZ.Pole[i].Frequency;
                    }
                    for (i = 1; i < cNPNZ.Zero.Length; i++)
                    {
                        v_anno = chartBode.Annotations.FindByName("Zero" + i.ToString());
                        if (v_anno != null)
                            if (v_anno.Name.Contains("Zero"))
                                if (v_anno.X != cNPNZ.Zero[i].Frequency)
                                    v_anno.X = cNPNZ.Zero[i].Frequency;
                    }

                }
            }
            catch
            {
                DebugInfoPrintLine("UpdateBodePlot(" + ForceAnnotationUpdate.ToString() + ") Exception");
            }

            return (true);
        }

        private HorizontalLineAnnotation GetHline(Chart chartControl, int chartArea, string AnnotationName, Color LineColor, int LineWidth = 1, ChartDashStyle LineDashStyle = ChartDashStyle.Solid, double YPos = 0.0, bool movable = true)
        {

            HorizontalLineAnnotation h = new HorizontalLineAnnotation();

            h.Name = AnnotationName;
            h.ClipToChartArea = chartControl.ChartAreas[chartArea].Name;
            h.IsSizeAlwaysRelative = true;
            h.AllowSelecting = false; 
            h.AllowMoving = movable;

            h.AxisX = chartControl.ChartAreas[chartArea].AxisX;
            h.AxisY = chartControl.ChartAreas[chartArea].AxisY;

            h.LineColor = LineColor;
            h.LineWidth = LineWidth;
            h.LineDashStyle = LineDashStyle;
            h.IsInfinitive = true;

            h.X = 0;
            h.Y = YPos;

            return h;
        }

        private VerticalLineAnnotation GetVline(Chart chartControl, int chartArea, string AnnotationName, Color LineColor, int LineWidth = 1, ChartDashStyle LineDashStyle = ChartDashStyle.Solid, double XPos = 0.0, bool movable = true)
        {

            VerticalLineAnnotation v = new VerticalLineAnnotation();

            v.Name = AnnotationName;
            v.ClipToChartArea = chartControl.ChartAreas[chartArea].Name;
            v.IsSizeAlwaysRelative = true;
            v.AllowSelecting = false; 
            v.AllowMoving = movable;

            v.AxisX = chartControl.ChartAreas[chartArea].AxisX;
            v.AxisY = chartControl.ChartAreas[chartArea].AxisY;

            v.LineColor = LineColor;
            v.LineWidth = LineWidth;
            v.LineDashStyle = LineDashStyle;
            v.IsInfinitive = true;

            v.X = XPos;
            v.Y = 0;

            return v;
        }


        /* ***************************************************************************************** 
         * When the application or a file is closed or a new file is opened while project changes
         * are still pending, throw a message box asking if the project should be saved.
         * ***************************************************************************************** */
        private bool AskForFileSave(object sender, EventArgs e)
        {
            bool b_res = true;
            string str_dum = "";
            DialogResult dlgResult = 0;

            // If there are still unsaved project changes pending and the most recent project file is valid
            if ((ProjectFileChanged) && (ProjectFile.Directory.Length > 7))
            {
                // capture project file 
                str_dum = ProjectFile.FileName;

                // ask if the pending changes should be saved
                dlgResult = MessageBox.Show("File '" + str_dum + "' has been changed.\r\nDo you wish to save these changes?", Application.ProductName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                switch (dlgResult)
                {
                    // User agreed to save the changes
                    case System.Windows.Forms.DialogResult.Yes:
                        SaveFile(sender, e);
                        if (ProjectFileChanged) b_res = false;
                        else b_res = true;
                        break;

                    // User approved to discard the changes
                    case System.Windows.Forms.DialogResult.No:
                        b_res = true;
                        break;

                    // User clicked 'Cancel' to go back to application
                    default:
                        b_res = false;
                        break;
                }

            }

            return (b_res);

        }

        /* ***************************************************************************************** 
         * Save the most recent settings to project file
         * ***************************************************************************************** */
        private bool SaveFile(object sender, EventArgs e)
        {
            SaveFileDialog sfdlg = new SaveFileDialog();
            string str_dum = "";
            string[] str_arr;
            string[] dum_sep = new string[1];
            int i = 0;

            // Prepare the status bar progress bar
            stbMainStatusLabel.Text = "Saving File:";
            stbMainStatusLabel.Visible = true;
            stbProgressBar.Visible = true;
            stbProgressBar.Value = 10;
            Application.DoEvents();

            // Save parameter file
            if ((ProjectFile.FileName.Trim().Length == 0) || (sender == saveAsToolStripMenuItem) || (ProjectFile.FileName == NewProjectFilenameDummy))
            {
                // Show "Save As..." Dialog
                sfdlg.FileName = NewProjectFilenameDummy;
                sfdlg.Filter = "Microchip Digital Control Loop Designer files (*.dcld)|*.dcld|All files (*.*)|*.*";
                sfdlg.FilterIndex = 1;

                // If project file is valid
                if (ProjectFile.FileName.Trim().Length > 7)
                { sfdlg.InitialDirectory = ProjectFile.Directory; }
                else if (MPLABXProject.MPLABXProjectDirectory.Trim().Length > 7)
                { sfdlg.InitialDirectory = MPLABXProject.MPLABXProjectDirectory; }

                // Showing the 'save file' dialog 
                if (sfdlg.ShowDialog() == DialogResult.OK)
                {
                    // if user clicked OK...
                    try
                    {
                        // When the selected path is valid
                        if (sfdlg.CheckPathExists)
                        {
                            // Capture user-defined filename & path
                            ProjectFile.SetFilename(sfdlg.FileName.Trim());

                            // Display recent project file in window title bar
                            dum_sep[0] = (_dsp);
                            str_arr = sfdlg.FileName.Split(dum_sep, StringSplitOptions.RemoveEmptyEntries); ;
                            str_dum = str_arr[Convert.ToInt32(str_arr.GetUpperBound(0))];
                            this.Text = Application.ProductName + " v" + Application.ProductVersion + " - [" + str_dum + "]";

                            // Rescan and adopt code generator file path declarations
                            if (MPLABXProject.MPLABXProjectDirectory.Trim().Length == 0)
                            {
                                MPLABXProject.SetMPLABXProject("");
                                UpdateFilePaths();

                                txtMPLABXProjectDir.ToolTipText = MPLABXProject.MPLABXProjectDirectory;
                            }
                            else { txtMPLABXProjectDir.ToolTipText = "(not specified)";  }

                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error (0x" + ex.HResult.ToString("X") + "):" + ex.Message + "\r\n" +
                                        "File path could not be identified."
                                        ,"Save File" , 
                                        MessageBoxButtons.OK, 
                                        MessageBoxIcon.Exclamation, 
                                        MessageBoxDefaultButton.Button1
                                        );
                        // Restore output window
                        sfdlg.Dispose();
                        stbMainStatusLabel.Visible = false;
                        stbProgressBar.Visible = false;
                        ProjectFile.Clear();

                        return (false);

                    }
                }
                else
                {
                    // Restore output window
                    sfdlg.Dispose();
                    stbMainStatusLabel.Visible = false;
                    stbProgressBar.Visible = false;

                    return (false);
                }

            }

            // Save output window to file
            try
            {
                // Status Bar Progress Indication
                stbProgressBar.Value = 15;
                Application.DoEvents();
                
                // File Info
                if (Convert.ToString(ProjectFile.ReadKey("GUI", "CreateDate", "")) == "")
                    ProjectFile.WriteKey("GUI", "CreateDate", System.DateTime.Now.ToString());

                // GUI Version
                ProjectFile.WriteKey("GUI", "SaveDate", Convert.ToString(System.DateTime.Now));
                ProjectFile.WriteKey("GUI", "Name", Application.ProductName);
                ProjectFile.WriteKey("GUI", "Version", Application.ProductVersion);
                ProjectFile.WriteKey("GUI", "VersionKey", dcldGlobals.APP_VERSION_KEY.ToString());
                ProjectFile.WriteKey("GUI", "AGS Version", AsmGeneratorScript.ReadKey("generic", "Version", "N/A"));
                ProjectFile.WriteKey("GUI", "CGS Version", CCodeGeneratorScript.ReadKey("generic", "Version", "N/A"));

                // Status Bar Progress Indication
                stbProgressBar.Value = 20;
                Application.DoEvents();

                // GUI settings
                ProjectFile.WriteKey("GUI", "WindoWState", this.WindowState.ToString());
                ProjectFile.WriteKey("GUI", "Top", this.Top.ToString());
                ProjectFile.WriteKey("GUI", "Left", this.Left.ToString());
                ProjectFile.WriteKey("GUI", "Height", this.Height.ToString());
                ProjectFile.WriteKey("GUI", "Width", this.Width.ToString());
                ProjectFile.WriteKey("GUI", "TabHeight", splitContainerCoefficients.Panel2.ClientSize.Height.ToString());

                // Status Bar Progress Indication
                stbProgressBar.Value = 25;
                Application.DoEvents();

                // Control Type and Mode
                ProjectFile.WriteKey("ControlSetup", "ControlType", cmbCompType.SelectedIndex.ToString());
                ProjectFile.WriteKey("ControlSetup", "ScalingMode", cmbQScalingMethod.SelectedIndex.ToString());
                ProjectFile.WriteKey("ControlSetup", "QFormat", cmbQFormat.SelectedIndex.ToString());
                ProjectFile.WriteKey("ControlSetup", "SamplingFrequency", txtSamplingFrequency.Text);
                ProjectFile.WriteKey("ControlSetup", "InputGain", txtInputGain.Text);
                ProjectFile.WriteKey("ControlSetup", "InputGainNormalization", Math.Abs(Convert.ToInt32(chkNormalizeInputGain.Checked)).ToString());
                ProjectFile.WriteKey("ControlSetup", "BiDirectionalFeedback", Math.Abs(Convert.ToInt32(chkBiDirectionalFeedback.Checked)).ToString());
                ProjectFile.WriteKey("ControlSetup", "FeedbackRectification", Math.Abs(Convert.ToInt32(chkFeedbackRectification.Checked & chkBiDirectionalFeedback.Checked)).ToString());
                ProjectFile.WriteKey("ControlSetup", "OutputGain", txtOutputGain.Text);
                ProjectFile.WriteKey("ControlSetup", "OutputGainNormalization", Math.Abs(Convert.ToInt32(chkNormalizeOutputGain.Checked)).ToString());

                ProjectFile.WriteKey("ControlSetup", "EnableAGC", Math.Abs(Convert.ToInt32(chkEnableAdaptiveGainControl.Checked)).ToString());
                ProjectFile.WriteKey("ControlSetup", "AddAGCEnableSwitch", Math.Abs(Convert.ToInt32(chkAGCAddEnable.Checked)).ToString());
                ProjectFile.WriteKey("ControlSetup", "AddAGCGetModFunCall", Math.Abs(Convert.ToInt32(chkAGCAddGetModFactorFunCall.Checked)).ToString());

                ProjectFile.WriteKey("ControlSetup", "PTermNomFeedback", cNPNZ.PTermNominalFeedback.ToString());
                ProjectFile.WriteKey("ControlSetup", "PTermNomOutput", cNPNZ.PTermNominalControlOutput.ToString());
                ProjectFile.WriteKey("ControlSetup", "PTermFactor", cNPNZ.PTermFactor.ToString());
                ProjectFile.WriteKey("ControlSetup", "PTermScaler", cNPNZ.PTermScaler.ToString());

                for (i = 0; i < txtPole.Length; i++)
                { 
                    ProjectFile.WriteKey("ControlSetup", "FrequencyP" + i.ToString(), txtPole[i].Text);
                    if (i > 0) ProjectFile.WriteKey("ControlSetup", "FrequencyZ" + i.ToString(), txtZero[i].Text);

                }

                ProjectFile.WriteKey("ControlSetup", "InputDataResolution", txtInputDataResolution.Text);
                ProjectFile.WriteKey("ControlSetup", "FixedPointErrorMsg", taboptMaxFPError.Text);
                ProjectFile.WriteKey("ControlSetup", "FixedPointErrorWarning", taboptMaxFPErrorWarning.Text);

                // Feedback Definition
                ProjectFile.WriteKey("FeedbackDeclaration", "Type", Convert.ToInt32(feedback.FeedbackType).ToString());
                ProjectFile.WriteKey("FeedbackDeclaration", "ADCref", feedback.ADCReference.ToString(CultureInfo.InvariantCulture));
                ProjectFile.WriteKey("FeedbackDeclaration", "ADCres", feedback.ADCResolution.ToString(CultureInfo.InvariantCulture));
                ProjectFile.WriteKey("FeedbackDeclaration", "ADCdiff", Convert.ToInt32(feedback.ADCIsDifferential).ToString(CultureInfo.InvariantCulture));
                ProjectFile.WriteKey("FeedbackDeclaration", "VDR1", feedback.VoltageDividerR1.ToString(CultureInfo.InvariantCulture));
                ProjectFile.WriteKey("FeedbackDeclaration", "VDR2", feedback.VoltageDividerR2.ToString(CultureInfo.InvariantCulture));
                ProjectFile.WriteKey("FeedbackDeclaration", "VDGA", feedback.VoltageDividerAmplifierGain.ToString(CultureInfo.InvariantCulture));
                ProjectFile.WriteKey("FeedbackDeclaration", "CSNomValue", feedback.VoltageDividerSenseVoltage.ToString(CultureInfo.InvariantCulture));
                ProjectFile.WriteKey("FeedbackDeclaration", "CSRS", feedback.CurrentSenseRshunt.ToString(CultureInfo.InvariantCulture));
                ProjectFile.WriteKey("FeedbackDeclaration", "CSGA", feedback.CurrentSenseAmplifierGain.ToString(CultureInfo.InvariantCulture));
                ProjectFile.WriteKey("FeedbackDeclaration", "CSNomValue", feedback.CurrentSenseSenseCurrent.ToString(CultureInfo.InvariantCulture));
                ProjectFile.WriteKey("FeedbackDeclaration", "CTRB", feedback.CurrentTransformerBurdenResistance.ToString(CultureInfo.InvariantCulture));
                ProjectFile.WriteKey("FeedbackDeclaration", "CTWR", feedback.CurrentTransformerWindingRatio.ToString(CultureInfo.InvariantCulture));
                ProjectFile.WriteKey("FeedbackDeclaration", "CTNomValue", feedback.CurrentTransformerSenseCurrent.ToString(CultureInfo.InvariantCulture));
                ProjectFile.WriteKey("FeedbackDeclaration", "DSRES", feedback.DigitalSourceResolution.ToString(CultureInfo.InvariantCulture));
                ProjectFile.WriteKey("FeedbackDeclaration", "DSNomValue", feedback.DigitalSourceValue.ToString(CultureInfo.InvariantCulture));

                // Ouptput Definition
                ProjectFile.WriteKey("OutputDeclaration", "Type", Convert.ToInt32(ctrl_output.OutputType).ToString());
                ProjectFile.WriteKey("OutputDeclaration", "PCLK", ctrl_output.PWMClock.ToString(CultureInfo.InvariantCulture));
                ProjectFile.WriteKey("OutputDeclaration", "PCLKDIV", ctrl_output.PWMClockDivider.ToString(CultureInfo.InvariantCulture));
                ProjectFile.WriteKey("OutputDeclaration", "PWMFREQ", ctrl_output.PWMFrequency.ToString(CultureInfo.InvariantCulture));
                ProjectFile.WriteKey("OutputDeclaration", "NomVin", ctrl_output.NominalInputVoltage.ToString(CultureInfo.InvariantCulture));
                ProjectFile.WriteKey("OutputDeclaration", "NomVout", ctrl_output.NominalOutputVoltage.ToString(CultureInfo.InvariantCulture));
                ProjectFile.WriteKey("OutputDeclaration", "WindingRatioPrimary", ctrl_output.WindingRatioPrimary.ToString(CultureInfo.InvariantCulture));
                ProjectFile.WriteKey("OutputDeclaration", "WindingRatioSecondary", ctrl_output.WindingRatioSecondary.ToString(CultureInfo.InvariantCulture));

                // Status Bar Progress Indication
                stbProgressBar.Value = 30;
                Application.DoEvents();

                // Code Generator Tab
                ProjectFile.WriteKey("AssemblyGenerator", "UserPrefix1", txtControllerNamePrefix.Text);
                ProjectFile.WriteKey("AssemblyGenerator", "UserPrefix2", txtControllerNameLabel.Text);

                ProjectFile.WriteKey("AssemblyGenerator", "ContextSaving", Convert.ToUInt16(this.chkContextSaving.Checked).ToString());
                ProjectFile.WriteKey("AssemblyGenerator", "ContextSavingShadowRegisters", Convert.ToUInt16(this.chkSaveRestoreShadowRegisters.Checked).ToString());
                ProjectFile.WriteKey("AssemblyGenerator", "ContextSavingMACRegisters", Convert.ToUInt16(this.chkSaveRestoreMACRegisters.Checked).ToString());
                ProjectFile.WriteKey("AssemblyGenerator", "ContextSavingAccumulatorRegisters", Convert.ToUInt16(this.chkSaveRestoreAccumulators.Checked).ToString());
                ProjectFile.WriteKey("AssemblyGenerator", "ContextSavingCoreConfigRegister", Convert.ToUInt16(this.chkSaveRestoreCoreConfig.Checked).ToString());
                ProjectFile.WriteKey("AssemblyGenerator", "ContextSavingCoreStatusRegister", Convert.ToUInt16(this.chkSaveRestoreCoreStatus.Checked).ToString());

                ProjectFile.WriteKey("AssemblyGenerator", "CodeFeatureOptions", Convert.ToUInt16(this.chkCodeFeatureOptions.Checked).ToString());
                ProjectFile.WriteKey("AssemblyGenerator", "EnforceCoreConfiguration", Convert.ToUInt16(this.chkAddCoreConfig.Checked).ToString());
                ProjectFile.WriteKey("AssemblyGenerator", "AddEnableDisableFeature", Convert.ToUInt16(this.chkAddEnableDisable.Checked).ToString());
                ProjectFile.WriteKey("AssemblyGenerator", "AddDisableDummyReadFeature", Convert.ToUInt16(this.chkAddDisableDummyRead.Checked).ToString());
                ProjectFile.WriteKey("AssemblyGenerator", "AddErrorNormalization", Convert.ToUInt16(this.chkAddErrorNormalization.Checked).ToString());
                ProjectFile.WriteKey("AssemblyGenerator", "AddADCTriggerAPlacement", Convert.ToUInt16(this.chkAddADCTriggerAPlacement.Checked).ToString());
                ProjectFile.WriteKey("AssemblyGenerator", "AddADCTriggerBPlacement", Convert.ToUInt16(this.chkAddADCTriggerBPlacement.Checked).ToString());
                ProjectFile.WriteKey("AssemblyGenerator", "AddCascadedFunctionCall", Convert.ToUInt16(this.chkAddCascadedFunctionCall.Checked).ToString());

                ProjectFile.WriteKey("AssemblyGenerator", "AddAutomatedDataInterface", Convert.ToUInt16(this.chkAutomatedDataInterface.Checked).ToString());
                ProjectFile.WriteKey("AssemblyGenerator", "AddAlternateSource", Convert.ToUInt16(this.chkAddAlternateSource.Checked).ToString());
                ProjectFile.WriteKey("AssemblyGenerator", "AddAlternateTarget", Convert.ToUInt16(this.chkAddAlternateTarget.Checked).ToString());

                ProjectFile.WriteKey("AssemblyGenerator", "DataProviderSource", Convert.ToUInt16(this.chkDataProviderSource.Checked).ToString());
                ProjectFile.WriteKey("AssemblyGenerator", "AddDataProviderControlInput", Convert.ToUInt16(this.chkAddDataProviderControlInput.Checked).ToString());
                ProjectFile.WriteKey("AssemblyGenerator", "AddDataProviderErrorInput", Convert.ToUInt16(this.chkAddDataProviderErrorInput.Checked).ToString());
                ProjectFile.WriteKey("AssemblyGenerator", "AddDataProviderControlOutput", Convert.ToUInt16(this.chkAddDataProviderControlOutput.Checked).ToString());

                ProjectFile.WriteKey("AssemblyGenerator", "AntiWindup", Convert.ToUInt16(this.chkAntiWindup.Checked).ToString());
                ProjectFile.WriteKey("AssemblyGenerator", "AntiWindupSoftDesaturation", Convert.ToUInt16(this.chkAntiWindupSoftDesaturationFlag.Checked).ToString());
                ProjectFile.WriteKey("AssemblyGenerator", "AddAntiWindupMaximumClamping", Convert.ToUInt16(this.chkAntiWindupClampMax.Checked).ToString());
                ProjectFile.WriteKey("AssemblyGenerator", "AddAntiWindupMaximumClampingStatusFlag", Convert.ToUInt16(this.chkAntiWindupMaxStatusFlag.Checked).ToString());
                ProjectFile.WriteKey("AssemblyGenerator", "AddAntiWindupMinimumClamping", Convert.ToUInt16(this.chkAntiWindupClampMin.Checked).ToString());
                ProjectFile.WriteKey("AssemblyGenerator", "AddAntiWindupMinimumClampingStatusFlag", Convert.ToUInt16(this.chkAntiWindupMinStatusFlag.Checked).ToString());

                ProjectFile.WriteKey("AssemblyGenerator", "AddPTermLoop", Convert.ToUInt16(this.chkAddPTermLoop.Checked).ToString());

                // Status Bar Progress Indication
                stbProgressBar.Value = 50;
                Application.DoEvents();

                // source code pages
                ProjectFile.WriteKey("CodeGenerationPaths", "ExportASMSource", Convert.ToUInt32(assemblyLibraryExportToolStripMenuItem.Checked).ToString());
                ProjectFile.WriteKey("CodeGenerationPaths", "ExportCSource", Convert.ToUInt32(libraryCSourceFileExportToolStripMenuItem.Checked).ToString());
                ProjectFile.WriteKey("CodeGenerationPaths", "ExportCHeader", Convert.ToUInt32(libraryCHeaderExportToolStripMenuItem.Checked).ToString());
                ProjectFile.WriteKey("CodeGenerationPaths", "ExportCLib", Convert.ToUInt32(genericControlLibraryHeaderExportToolStripMenuItem.Checked).ToString());
                ProjectFile.WriteKey("CodeGenerationPaths", "EnableOneClickExport", Convert.ToUInt32(generateCodeBeforeExportToolStripMenuItem.Checked).ToString());
                ProjectFile.WriteKey("CodeGenerationPaths", "MPLABX_ProjectDirectory", ConvertFilePath.ToRelativeFilePath(MPLABXProject.MPLABXProjectDirectory, ProjectFile.Directory));
                ProjectFile.WriteKey("CodeGenerationPaths", "ASMSourcePath", ConvertFilePath.Unix2Win(txtASMSourcePath.Text));
                ProjectFile.WriteKey("CodeGenerationPaths", "CSourcePath", ConvertFilePath.Unix2Win(txtCSourcePath.Text));
                ProjectFile.WriteKey("CodeGenerationPaths", "CHeaderPath", ConvertFilePath.Unix2Win(txtCHeaderPath.Text));
                ProjectFile.WriteKey("CodeGenerationPaths", "CLibPath", ConvertFilePath.Unix2Win(txtCLibPath.Text));
                ProjectFile.WriteKey("CodeGenerationPaths", "IncludeCHeaderPathInCSource", Convert.ToUInt32(chkCSourceIncludePath.Checked).ToString());
                ProjectFile.WriteKey("CodeGenerationPaths", "IncludeCLibPathInCHeader", Convert.ToUInt32(chkCHeaderIncludePath.Checked).ToString());

                // Status Bar Progress Indication
                stbProgressBar.Value = 65;
                Application.DoEvents();

                // Bode Plot settings
                ProjectFile.WriteKey("BodePlot", "XScale-NyquistShannonLimit", Convert.ToInt32(nyquistShannonLimitToolStripMenuItem.Checked).ToString());
                ProjectFile.WriteKey("BodePlot", "XScale-SamplingFrequency", Convert.ToInt32(samplingFrequencyToolStripMenuItem.Checked).ToString());
                ProjectFile.WriteKey("BodePlot", "XScale-FullScale", Convert.ToInt32(fullScaleToolStripMenuItem.Checked).ToString());
                ProjectFile.WriteKey("BodePlot", "YScale-UnwrapPhase", Convert.ToInt32(unwrapPhaseToolStripMenuItem.Checked).ToString());
                    
                // Timing graph
                ProjectFile.WriteKey("TimingGraph", "LoopTriggerOption", this.cmbLoopTriggerOption.SelectedIndex.ToString());

                ProjectFile.WriteKey("TimingGraph", "CPUClock", this.txtCPUClock.Text);
                ProjectFile.WriteKey("TimingGraph", "ADCLatency", this.txtADCLatency.Text);
                ProjectFile.WriteKey("TimingGraph", "PWMFrequency", this.txtPWMFrequency.Text);
                ProjectFile.WriteKey("TimingGraph", "PWMDutyCycle", this.txtPWMDutyCycle.Text);
                ProjectFile.WriteKey("TimingGraph", "ControlLoopLatency", this.txtISRLatency.Text);

                ProjectFile.WriteKey("TimingGraph", "TriggerPlacement", this.cmbTriggerPlacement.SelectedIndex.ToString());
                ProjectFile.WriteKey("TimingGraph", "UserTriggerPosition", this.chartTiming.Annotations["annADCTrigger"].AnchorX.ToString(CultureInfo.InvariantCulture));

                // Parameter listing complete

                // Status Bar Progress Indication
                stbProgressBar.Value = 100;
                Application.DoEvents();

                ProjectFileChanged = false;
                saveToolStripMenuItem.Enabled = false;
                toolStripButtonSave.Enabled = saveToolStripMenuItem.Enabled;

                // Restore output window
                sfdlg.Dispose();
                stbMainStatusLabel.Visible = false;
                stbProgressBar.Visible = false;

                return (true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error (0x" + ex.HResult.ToString("X") + "): Could not write file to disk. Original error: " + ex.Message, 
                    Application.ProductName, 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error
                    );

                // Restore output window
                sfdlg.Dispose();
                stbMainStatusLabel.Visible = false;
                stbProgressBar.Visible = false;

                return (false);
            }

        }

        private bool OpenFile(string Filename)
        {
            bool project_conflicts = false;
            int i = 0, int_dum = 0;
            string str_dum = ""; //, str_file = "";

            stbProgressBarLabel.Text = "Load Configuration:";
            stbProgressBarLabel.Visible = true;
            stbProgressBar.Visible = true;
            stbProgressBar.Value = 5;
            Application.DoEvents();

            try
            {

                // write debugging info to output window
                DebugInfoPrintLine(">Open File '" + Filename + "'...");
                
                // Set Project File
                if (!ProjectFile.SetFilename(Filename.Trim()))
                {
                    DebugInfoPrintLine(">File loading error. Could not set project file.");
                    return (false);
                }
                else
                    DebugInfoPrintLine(">File found");

                // Set flag
                ProjectFileLoadActive = true;
                cNPNZ.AutoUpdate = false;

                stbProgressBar.Value = 10;
                Application.DoEvents();


                // Control Type and Mode
                cmbCompType.SelectedIndex = Convert.ToInt32(ProjectFile.ReadKey("ControlSetup", "ControlType", "2"));
                cmbQScalingMethod.SelectedIndex = Convert.ToInt32(ProjectFile.ReadKey("ControlSetup", "ScalingMode", "0"));
                cmbQFormat.SelectedIndex = Convert.ToInt32(ProjectFile.ReadKey("ControlSetup", "QFormat", "1"));

                txtSamplingFrequency.Text = ProjectFile.ReadKey("ControlSetup", "SamplingFrequency", "50000");
                txtInputGain.Text = ProjectFile.ReadKey("ControlSetup", "InputGain", "1.000");
                chkNormalizeInputGain.Checked = Convert.ToBoolean(Convert.ToInt32(ProjectFile.ReadKey("ControlSetup", "InputGainNormalization", "1")));
                chkBiDirectionalFeedback.Checked = Convert.ToBoolean(Convert.ToInt32(ProjectFile.ReadKey("ControlSetup", "BiDirectionalFeedback", "0")));
                chkFeedbackRectification.Checked = Convert.ToBoolean(Convert.ToInt32(ProjectFile.ReadKey("ControlSetup", "FeedbackRectification", "0")));
                txtOutputGain.Text = ProjectFile.ReadKey("ControlSetup", "OutputGain", "1.000");
                chkNormalizeOutputGain.Checked = Convert.ToBoolean(Convert.ToInt32(ProjectFile.ReadKey("ControlSetup", "OutputGainNormalization", "1")));

                chkEnableAdaptiveGainControl.Checked = Convert.ToBoolean(Convert.ToInt32(ProjectFile.ReadKey("ControlSetup", "EnableAGC", "0")));
                chkAGCAddEnable.Checked = Convert.ToBoolean(Convert.ToInt32(ProjectFile.ReadKey("ControlSetup", "AddAGCEnableSwitch", "0")));
                chkAGCAddGetModFactorFunCall.Checked = Convert.ToBoolean(Convert.ToInt32(ProjectFile.ReadKey("ControlSetup", "AddAGCGetModFunCall", "0")));

                txtPTermNominalFeedback.Text = Convert.ToInt32(ProjectFile.ReadKey("ControlSetup", "PTermNomFeedback", "0")).ToString(CultureInfo.CurrentCulture);
                txtPTermNominalOutput.Text = Convert.ToInt32(ProjectFile.ReadKey("ControlSetup", "PTermNomOutput", "0")).ToString(CultureInfo.CurrentCulture);

                for (i = 0; i < txtPole.Length; i++)
                {
                    txtPole[i].Text = ProjectFile.ReadKey("ControlSetup", "FrequencyP" + i.ToString(), "1");
                    if (i > 0) txtZero[i].Text = ProjectFile.ReadKey("ControlSetup", "FrequencyZ" + i.ToString(), "1");
                }

                txtInputDataResolution.Text = ProjectFile.ReadKey("ControlSetup", "InputDataResolution", "12");

                taboptMaxFPError.Text = ProjectFile.ReadKey("ControlSetup", "FixedPointErrorMsg", "0.5");
                taboptMaxFPErrorWarning.Text = ProjectFile.ReadKey("ControlSetup", "FixedPointErrorWarning", "0.1");

                stbProgressBar.Value = 20;
                Application.DoEvents();

                // Feedback Definition
                int_dum = Convert.ToInt32(ProjectFile.ReadKey("FeedbackDeclaration", "Type", "1"));
                feedback.FeedbackType = (clsFeedbackDeclaration.dcldFeedbackType)int_dum;
                feedback.ADCReference = Convert.ToDouble(ProjectFile.ReadKey("FeedbackDeclaration", "ADCref", "3.3"));
                feedback.ADCResolution = Convert.ToDouble(ProjectFile.ReadKey("FeedbackDeclaration", "ADCres", "12.0"));
                feedback.ADCIsDifferential = Convert.ToBoolean(Convert.ToInt32(ProjectFile.ReadKey("FeedbackDeclaration", "ADCdiff", "0")));
                feedback.VoltageDividerR1 = Convert.ToDouble(ProjectFile.ReadKey("FeedbackDeclaration", "VDR1", "12000"));
                feedback.VoltageDividerR2 = Convert.ToDouble(ProjectFile.ReadKey("FeedbackDeclaration", "VDR2", "2200"));
                feedback.VoltageDividerAmplifierGain = Convert.ToDouble(ProjectFile.ReadKey("FeedbackDeclaration", "VDGA", "1.0"));
                feedback.VoltageDividerSenseVoltage = Convert.ToDouble(ProjectFile.ReadKey("FeedbackDeclaration", "VDNomVal", "5.0"));
                feedback.CurrentSenseRshunt = Convert.ToDouble(ProjectFile.ReadKey("FeedbackDeclaration", "CSRS", "0.010"));
                feedback.CurrentSenseAmplifierGain = Convert.ToDouble(ProjectFile.ReadKey("FeedbackDeclaration", "CSGA", "20"));
                feedback.CurrentSenseSenseCurrent = Convert.ToDouble(ProjectFile.ReadKey("FeedbackDeclaration", "CSNomVal", "5.0"));
                feedback.CurrentTransformerBurdenResistance = Convert.ToDouble(ProjectFile.ReadKey("FeedbackDeclaration", "CTRB", "10"));
                feedback.CurrentTransformerWindingRatio = Convert.ToDouble(ProjectFile.ReadKey("FeedbackDeclaration", "CTWR", "50"));
                feedback.CurrentTransformerSenseCurrent = Convert.ToDouble(ProjectFile.ReadKey("FeedbackDeclaration", "CTNomVal", "5.0"));
                feedback.DigitalSourceResolution = Convert.ToDouble(ProjectFile.ReadKey("FeedbackDeclaration", "DSRES", "12.0"));
                feedback.DigitalSourceValue = Convert.ToDouble(ProjectFile.ReadKey("FeedbackDeclaration", "DSNomValue", "2048.0"));


                // Ouptput Definition
                int_dum = Convert.ToInt32(ProjectFile.ReadKey("OutputDeclaration", "Type", "1"));
                ctrl_output.OutputType = (clsOutputDeclaration.dcldOutputType)int_dum;
                ctrl_output.PWMClock = Convert.ToDouble(ProjectFile.ReadKey("OutputDeclaration", "PCLK", "4000000000"));
                ctrl_output.PWMClockDivider = Convert.ToDouble(ProjectFile.ReadKey("OutputDeclaration", "PCLKDIV", "1.0"));
                ctrl_output.PWMFrequency = Convert.ToDouble(ProjectFile.ReadKey("OutputDeclaration", "PWMFREQ", "250000.0"));
                ctrl_output.NominalInputVoltage = Convert.ToDouble(ProjectFile.ReadKey("OutputDeclaration", "NomVin", "0.0"));
                ctrl_output.NominalOutputVoltage = Convert.ToDouble(ProjectFile.ReadKey("OutputDeclaration", "NomVout", "0.0"));
                ctrl_output.WindingRatioPrimary = Convert.ToDouble(ProjectFile.ReadKey("OutputDeclaration", "WindingRatioPrimary", "1.0"));
                ctrl_output.WindingRatioSecondary = Convert.ToDouble(ProjectFile.ReadKey("OutputDeclaration", "WindingRatioSecondary", "1.0"));

                // Code Generator Tab
                txtControllerNamePrefix.Text = ProjectFile.ReadKey("AssemblyGenerator", "UserPrefix1", "");
                txtControllerNameLabel.Text = ProjectFile.ReadKey("AssemblyGenerator", "UserPrefix2", "");

                stbProgressBar.Value = 30;
                Application.DoEvents();

                this.chkContextSaving.Checked = Convert.ToBoolean(Convert.ToUInt16(ProjectFile.ReadKey("AssemblyGenerator", "ContextSaving", "1")));
                this.chkSaveRestoreShadowRegisters.Checked = Convert.ToBoolean(Convert.ToUInt16(ProjectFile.ReadKey("AssemblyGenerator", "ContextSavingShadowRegisters", "1")));
                this.chkSaveRestoreMACRegisters.Checked = Convert.ToBoolean(Convert.ToUInt16(ProjectFile.ReadKey("AssemblyGenerator", "ContextSavingMACRegisters", "1")));
                this.chkSaveRestoreAccumulators.Checked = Convert.ToBoolean(Convert.ToUInt16(ProjectFile.ReadKey("AssemblyGenerator", "ContextSavingAccumulatorRegisters", "1")));
                this.chkSaveRestoreCoreConfig.Checked = Convert.ToBoolean(Convert.ToUInt16(ProjectFile.ReadKey("AssemblyGenerator", "ContextSavingCoreConfigRegister", "1")));
                this.chkSaveRestoreCoreStatus.Checked = Convert.ToBoolean(Convert.ToUInt16(ProjectFile.ReadKey("AssemblyGenerator", "ContextSavingCoreStatusRegister", "1")));

                stbProgressBar.Value = 40;
                Application.DoEvents();

                this.chkCodeFeatureOptions.Checked = Convert.ToBoolean(Convert.ToUInt16(ProjectFile.ReadKey("AssemblyGenerator", "CodeFeatureOptions", "1")));
                this.chkAddCoreConfig.Checked = Convert.ToBoolean(Convert.ToUInt16(ProjectFile.ReadKey("AssemblyGenerator", "EnforceCoreConfiguration", "1")));
                this.chkAddEnableDisable.Checked = Convert.ToBoolean(Convert.ToUInt16(ProjectFile.ReadKey("AssemblyGenerator", "AddEnableDisableFeature", "1")));
                this.chkAddDisableDummyRead.Checked = Convert.ToBoolean(Convert.ToUInt16(ProjectFile.ReadKey("AssemblyGenerator", "AddDisableDummyReadFeature", "1")));
                this.chkAddErrorNormalization.Checked = Convert.ToBoolean(Convert.ToUInt16(ProjectFile.ReadKey("AssemblyGenerator", "AddErrorNormalization", "1")));
                this.chkAddADCTriggerAPlacement.Checked = Convert.ToBoolean(Convert.ToUInt16(ProjectFile.ReadKey("AssemblyGenerator", "AddADCTriggerAPlacement", "1")));
                this.chkAddADCTriggerBPlacement.Checked = Convert.ToBoolean(Convert.ToUInt16(ProjectFile.ReadKey("AssemblyGenerator", "AddADCTriggerBPlacement", "0")));
                this.chkAddCascadedFunctionCall.Checked = Convert.ToBoolean(Convert.ToUInt16(ProjectFile.ReadKey("AssemblyGenerator", "AddCascadedFunctionCall", "0")));

                this.chkAutomatedDataInterface.Checked = Convert.ToBoolean(Convert.ToUInt16(ProjectFile.ReadKey("AssemblyGenerator", "AddAutomatedDataInterface", "0")));
                this.chkAddAlternateSource.Checked = Convert.ToBoolean(Convert.ToUInt16(ProjectFile.ReadKey("AssemblyGenerator", "AddAlternateSource", "0")));
                this.chkAddAlternateTarget.Checked = Convert.ToBoolean(Convert.ToUInt16(ProjectFile.ReadKey("AssemblyGenerator", "AddAlternateTarget", "0")));

                this.chkDataProviderSource.Checked = Convert.ToBoolean(Convert.ToUInt16(ProjectFile.ReadKey("AssemblyGenerator", "DataProviderSource", "0")));
                this.chkAddDataProviderControlInput.Checked = Convert.ToBoolean(Convert.ToUInt16(ProjectFile.ReadKey("AssemblyGenerator", "AddDataProviderControlInput", "0")));
                this.chkAddDataProviderErrorInput.Checked = Convert.ToBoolean(Convert.ToUInt16(ProjectFile.ReadKey("AssemblyGenerator", "AddDataProviderErrorInput", "0")));
                this.chkAddDataProviderControlOutput.Checked = Convert.ToBoolean(Convert.ToUInt16(ProjectFile.ReadKey("AssemblyGenerator", "AddDataProviderControlOutput", "0")));

                stbProgressBar.Value = 50;
                Application.DoEvents();

                this.chkAntiWindup.Checked = Convert.ToBoolean(Convert.ToUInt16(ProjectFile.ReadKey("AssemblyGenerator", "AntiWindup", "1")));
                this.chkAntiWindupSoftDesaturationFlag.Checked = Convert.ToBoolean(Convert.ToUInt16(ProjectFile.ReadKey("AssemblyGenerator", "AntiWindupSoftDesaturation", "0")));
                this.chkAntiWindupClampMax.Checked = Convert.ToBoolean(Convert.ToUInt16(ProjectFile.ReadKey("AssemblyGenerator", "AddAntiWindupMaximumClamping", "1")));
                this.chkAntiWindupMaxStatusFlag.Checked = Convert.ToBoolean(Convert.ToUInt16(ProjectFile.ReadKey("AssemblyGenerator", "AddAntiWindupMaximumClampingStatusFlag", "0")));
                this.chkAntiWindupClampMin.Checked = Convert.ToBoolean(Convert.ToUInt16(ProjectFile.ReadKey("AssemblyGenerator", "AddAntiWindupMinimumClamping", "1")));
                this.chkAntiWindupMinStatusFlag.Checked = Convert.ToBoolean(Convert.ToUInt16(ProjectFile.ReadKey("AssemblyGenerator", "AddAntiWindupMinimumClampingStatusFlag", "0")));

                this.chkAddPTermLoop.Checked = Convert.ToBoolean(Convert.ToUInt16(ProjectFile.ReadKey("AssemblyGenerator", "AddPTermLoop", "0")));



                stbProgressBar.Value = 60;
                Application.DoEvents();

                // Source code pages
                this.txtASMSourcePath.Text = ProjectFile.ReadKey("CodeGenerationPaths", "ASMSourcePath", ""); //str_dum + _dsp + str_file + "_asm.s");
                this.txtCSourcePath.Text = ProjectFile.ReadKey("CodeGenerationPaths", "CSourcePath", ""); //str_dum + _dsp + str_file + ".c");
                this.txtCHeaderPath.Text = ProjectFile.ReadKey("CodeGenerationPaths", "CHeaderPath", ""); //str_dum + _dsp + str_file + ".h");
                this.txtCLibPath.Text = ProjectFile.ReadKey("CodeGenerationPaths", "CLibPath", ""); //_dsp + "npnz16b.h");

                stbProgressBar.Value = 70;
                Application.DoEvents();

                this.assemblyLibraryExportToolStripMenuItem.Checked = Convert.ToBoolean(Convert.ToUInt32(ProjectFile.ReadKey("CodeGenerationPaths", "ExportASMSource", "1")));
                this.libraryCSourceFileExportToolStripMenuItem.Checked = Convert.ToBoolean(Convert.ToUInt32(ProjectFile.ReadKey("CodeGenerationPaths", "ExportCSource", "1")));
                this.libraryCHeaderExportToolStripMenuItem.Checked = Convert.ToBoolean(Convert.ToUInt32(ProjectFile.ReadKey("CodeGenerationPaths", "ExportCHeader", "1")));
                this.genericControlLibraryHeaderExportToolStripMenuItem.Checked = Convert.ToBoolean(Convert.ToUInt32(ProjectFile.ReadKey("CodeGenerationPaths", "ExportCLib", "1")));
                this.generateCodeBeforeExportToolStripMenuItem.Checked = Convert.ToBoolean(Convert.ToUInt32(ProjectFile.ReadKey("CodeGenerationPaths", "EnableOneClickExport", "1")));
                //this.txtMPLABXProjectDir.Text = ProjectFile.ReadKey("CodeGenerationPaths", "MPLABX_ProjectDirectory", ""); // Is loaded separately when project file is opened
                this.chkCSourceIncludePath.Checked = Convert.ToBoolean(Convert.ToUInt32(ProjectFile.ReadKey("CodeGenerationPaths", "IncludeCHeaderPathInCSource", "1")));
                this.chkCHeaderIncludePath.Checked = Convert.ToBoolean(Convert.ToUInt32(ProjectFile.ReadKey("CodeGenerationPaths", "IncludeCLibPathInCHeader", "1")));

                stbProgressBar.Value = 80;
                Application.DoEvents();

                // Bode Plot settings
                unwrapPhaseToolStripMenuItem.Checked = Convert.ToBoolean(Convert.ToUInt32(ProjectFile.ReadKey("BodePlot", "YScale-UnwrapPhase", "1")));

                if (Convert.ToBoolean(Convert.ToUInt32(ProjectFile.ReadKey("BodePlot", "XScale-NyquistShannonLimit", "1"))))
                    chartBode_ScaleOptionChanged(nyquistShannonLimitToolStripMenuItem, EventArgs.Empty);

                if ( Convert.ToBoolean(Convert.ToUInt32(ProjectFile.ReadKey("BodePlot", "XScale-SamplingFrequency", "0"))) )
                    chartBode_ScaleOptionChanged(samplingFrequencyToolStripMenuItem, EventArgs.Empty);
                
                if ( Convert.ToBoolean(Convert.ToUInt32(ProjectFile.ReadKey("BodePlot", "XScale-FullScale", "0"))) )
                    chartBode_ScaleOptionChanged(fullScaleToolStripMenuItem, EventArgs.Empty);

                // Timing graph
                this.cmbLoopTriggerOption.SelectedIndex = Convert.ToInt16(ProjectFile.ReadKey("TimingGraph", "LoopTriggerOption", "0"));

                this.txtCPUClock.Text = ProjectFile.ReadKey("TimingGraph", "CPUClock", "70");
                this.txtADCLatency.Text = ProjectFile.ReadKey("TimingGraph", "ADCLatency", "310");
                this.txtPWMFrequency.Text = ProjectFile.ReadKey("TimingGraph", "PWMFrequency", "250");
                this.txtPWMDutyCycle.Text = ProjectFile.ReadKey("TimingGraph", "PWMDutyCycle", "30");
                this.txtISRLatency.Text = ProjectFile.ReadKey("TimingGraph", "ControlLoopLatency", "171");
                this.cmbTriggerPlacement.SelectedIndex = Convert.ToInt32(ProjectFile.ReadKey("TimingGraph", "TriggerPlacement", "0"));
                this.chartTiming.Annotations["annADCTrigger"].AnchorX = Convert.ToDouble(ProjectFile.ReadKey("TimingGraph", "UserTriggerPosition", "0"));
                
                chartTimingSetAnnotationLabelPositions(this, EventArgs.Empty);

                stbProgressBar.Value = 90;
                Application.DoEvents();

                // ==================================================================================
                // Parameter listing complete

                // Load user history
                LoadHistorySettingsList(ProjectFile.FileName);
                ForceCoefficientsUpdate(this, EventArgs.Empty);

                // First load recent project name into title bar
                this.Text = Application.ProductName + " v" + Application.ProductVersion + " - [" + ProjectFile.FileName + "]";

                // Construct absolute MPLAB X project directory path
                str_dum = ProjectFile.ReadKey("CodeGenerationPaths", "MPLABX_ProjectDirectory", "");
                str_dum = ConvertFilePath.ToAbsoluteFilePath(str_dum, ProjectFile.Directory);

                if ((str_dum.Trim().Length > 3) && (System.IO.Directory.Exists(str_dum)))
                {
                    if (MPLABXProject.SetMPLABXProject(str_dum))
                    {
                        txtMPLABXProjectDir.Text = MPLABXProject.MPLABXProjectDirectory;
                        txtMPLABXProjectDir.ToolTipText = MPLABXProject.MPLABXProjectDirectory;
                    }
                    else
                    {
                        project_conflicts = true;
                    }
                }
                else
                {
                    project_conflicts = true;
                }

                // Check for project loading errors
                if (project_conflicts) { 
                MessageBox.Show(
                    "The location of the assiciated MPLAB x project is not valid anymore. " +
                    "Please review the project configuration to make sure the associated  " +
                    "MPLAB X project does exist and is accessible.",
                    "Project Loading Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }

                // write debugging info to output window
                DebugInfoPrintLine(">Code Generator File Paths:");
                DebugInfoPrintLine(">    - DCLD Project Directory:         " + ProjectFile.Directory);
                DebugInfoPrintLine(">    - MPLAB X Project Directory:      " + MPLABXProject.MPLABXProjectDirectory);
                DebugInfoPrintLine(">    - ASM Source File Directory:      " + ConvertFilePath.ToAbsoluteFilePath(txtASMSourcePath.Text, MPLABXProject.MPLABXProjectDirectory));
                DebugInfoPrintLine(">    - C Source File Directory:        " + ConvertFilePath.ToAbsoluteFilePath(txtCSourcePath.Text, MPLABXProject.MPLABXProjectDirectory));
                DebugInfoPrintLine(">    - C Header File Directory:        " + ConvertFilePath.ToAbsoluteFilePath(txtCHeaderPath.Text, MPLABXProject.MPLABXProjectDirectory));
                DebugInfoPrintLine(">    - Library Header File Directory:  " + ConvertFilePath.ToAbsoluteFilePath(txtCLibPath.Text, MPLABXProject.MPLABXProjectDirectory));

                // Set Flags
                ProjectFileLoadActive = false;
                ProjectFileChanged = false;
                saveToolStripMenuItem.Enabled = false;
                toolStripButtonSave.Enabled = saveToolStripMenuItem.Enabled;

                stbProgressBar.Value = 100;
                Application.DoEvents();
                stbProgressBar.Visible = false;
                stbProgressBarLabel.Visible = false;

                if (project_conflicts)
                    OpenProjectConfigWindow();

                return (true);

            }
            catch (Exception ex)
            {
                DebugInfoPrintLine(
                    ">Open File Error (0x" + ex.HResult.ToString("X") + ")\r\n" + 
                    ">Original error: " + ex.Message
                    );
                MessageBox.Show(
                    "Error (0x" + ex.HResult.ToString("X") + "): Could not read file from disk.\r\n" + 
                    "Original error: " + ex.Message, 
                    Application.ProductName, 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error
                    );

                // Clear Project File
                ProjectFile.Clear();

                stbProgressBar.Value = 100;
                Application.DoEvents();
                stbProgressBarLabel.Visible = false;
                stbProgressBar.Visible = false;
                return (false);
            }
        }

        private void UpdateFilePaths()
        {
            string asm_source_buf = "", c_cource_buf = "", c_head_buf = "", c_lib_buf = "";

            asm_source_buf = ConvertFilePath.ToAbsoluteFilePath(txtASMSourcePath.Text, MPLABXProject.MPLABXIncludeDirectoryAbsolute);
            c_cource_buf = ConvertFilePath.ToAbsoluteFilePath(txtCSourcePath.Text, MPLABXProject.MPLABXIncludeDirectoryAbsolute);
            c_head_buf = ConvertFilePath.ToAbsoluteFilePath(txtCHeaderPath.Text, MPLABXProject.MPLABXIncludeDirectoryAbsolute);
            c_lib_buf = ConvertFilePath.ToAbsoluteFilePath(txtCLibPath.Text, MPLABXProject.MPLABXIncludeDirectoryAbsolute);

            // Make file locations relative
            txtASMSourcePath.Text = ConvertFilePath.Unix2Win(ConvertFilePath.ToRelativeFilePath(asm_source_buf, MPLABXProject.MPLABXIncludeDirectoryAbsolute));
            txtCSourcePath.Text = ConvertFilePath.Unix2Win(ConvertFilePath.ToRelativeFilePath(c_cource_buf, MPLABXProject.MPLABXIncludeDirectoryAbsolute));
            txtCHeaderPath.Text = ConvertFilePath.Unix2Win(ConvertFilePath.ToRelativeFilePath(c_head_buf, MPLABXProject.MPLABXIncludeDirectoryAbsolute));
            txtCLibPath.Text = ConvertFilePath.Unix2Win(ConvertFilePath.ToRelativeFilePath(c_lib_buf, MPLABXProject.MPLABXIncludeDirectoryAbsolute));

            return;

        }

        private void NumberTextBox_KeyDownNoScaling(object sender, KeyEventArgs e)
        {

            if (
                ((48 <= e.KeyValue) && (e.KeyValue <= 57)) ||    // Numbers from 0-9 on keyboard
                ((96 <= e.KeyValue) && (e.KeyValue <= 105)) ||   // Number from 0-9 on NumPad
                (e.KeyValue == 110) || (e.KeyValue == 188) || (e.KeyValue == 190) ||                      // Comma and Period
                (e.KeyValue == 8) || (e.KeyValue == 46) || ((37 <= e.KeyValue) && (e.KeyValue <= 40)) ||  // DEL, Backspace, Left-Right-Up-Down Arrows
                (e.KeyValue == 35) || (e.KeyValue == 36) || (e.KeyValue == 45)    // POS1, END, INS
                )
            {
                // u=85
                return;
            }
            else
            {
                e.SuppressKeyPress = true;
                return;
            }


        }

        public void NumberTextBox_KeyDownWithScaling(object sender, KeyEventArgs e)
        {
            string tval = "";
            TextBox tBox;

            if (
                ((e.KeyValue == 84) && (e.Shift))  || ((e.KeyValue == 71) && (e.Shift)) || ((e.KeyValue == 77) && (e.Shift)) ||     // allow scaling using T=Tera, G=Giga, M=Mega, ...
                ((e.KeyValue == 75) && (!e.Shift)) || ((e.KeyValue == 77) && (!e.Shift)) || ((e.KeyValue == 85) && (!e.Shift)) ||   // ... k=Kilo, m=Milli, u=Micro, ...
                ((e.KeyValue == 78) && (!e.Shift)) || ((e.KeyValue == 80) && (!e.Shift)) || ((e.KeyValue == 70) && (!e.Shift))      // ... n=Nano, p=Piko, freq=Femto
               )
            {
                try
                {
                    if (sender.GetType().ToString() == "System.Windows.Forms.TextBox")
                    {
                        tBox = (TextBox)sender;
                        tval = tBox.Text.Trim();

                             if ((e.KeyValue == 84) && (e.Shift))  tval = tval + " T";   // Tera
                        else if ((e.KeyValue == 71) && (e.Shift))  tval = tval + " G";   // Giga
                        else if ((e.KeyValue == 77) && (e.Shift))  tval = tval + " M";   // Mega
                        else if ((e.KeyValue == 75) && (!e.Shift)) tval = tval + " k";   // Kilo
                        else if ((e.KeyValue == 77) && (!e.Shift)) tval = tval + " m";   // Milli
                        else if ((e.KeyValue == 85) && (!e.Shift)) tval = tval + " u";   // Mikro
                        else if ((e.KeyValue == 78) && (!e.Shift)) tval = tval + " n";   // Nano
                        else if ((e.KeyValue == 80) && (!e.Shift)) tval = tval + " p";   // Piko
                        else if ((e.KeyValue == 70) && (!e.Shift)) tval = tval + " f";   // Femto
                        {
                            //tBox.Text = tval;
                        }
                    }
                }
                catch { /* do nothing */ }

                //e.SuppressKeyPress = true;
            }
            else if (
                ((48 <= e.KeyValue) && (e.KeyValue <= 57)) ||    // Numbers from 0-9 on keyboard
                ((96 <= e.KeyValue) && (e.KeyValue <= 105)) ||   // Number from 0-9 on NumPad
                (e.KeyValue == 110) || (e.KeyValue == 188) || (e.KeyValue == 190) ||                      // Comma and Period
                (e.KeyValue == 8) || (e.KeyValue == 46) || ((37 <= e.KeyValue) && (e.KeyValue <= 40)) ||  // DEL, Backspace, Left-Right-Up-Down Arrows
                (e.KeyValue == 35) || (e.KeyValue == 36) || (e.KeyValue == 45)    // POS1, END, INS
                )
            {
                // u=85
                return;
            }
            else
            { 
                e.SuppressKeyPress = true; 
                return;
            }


        }

        private void NumberTextBox_Enter(object sender, EventArgs e)
        {
            System.Windows.Forms.TextBox tBox;

            if (sender.GetType().ToString() == "System.Windows.Forms.TextBox")
            {
                tBox = (System.Windows.Forms.TextBox)sender;
                tBox.Select(0, (tBox.Text.Length));
            }

            return;
        }

        private double NumberTextBox_ToDouble(TextBox tBoxToValidate)
        {
            double dval = 0.0, dblres = 0.0;
            string tval = "";
            System.Windows.Forms.TextBox tBox;

            if (tBoxToValidate.GetType().ToString() == "System.Windows.Forms.TextBox")
            {
                tBox = (System.Windows.Forms.TextBox)tBoxToValidate;
                tval = tBox.Text.Trim();

                     if (tval.Contains("T")) { dval = (1e+12); tval = tval.Replace("G", "").Trim(); }   // Tera
                else if (tval.Contains("G")) { dval = (1e+9);  tval = tval.Replace("G", "").Trim(); }   // Giga
                else if (tval.Contains("M")) { dval = (1e+6);  tval = tval.Replace("M", "").Trim(); }   // Mega
                else if (tval.Contains("k")) { dval = (1e+3);  tval = tval.Replace("k", "").Trim(); }   // Kilo
                else if (tval.Contains("m")) { dval = (1e-3);  tval = tval.Replace("m", "").Trim(); }   // Milli
                else if (tval.Contains("u")) { dval = (1e-6);  tval = tval.Replace("u", "").Trim(); }   // Mikro
                else if (tval.Contains("n")) { dval = (1e-9);  tval = tval.Replace("n", "").Trim(); }   // Nano
                else if (tval.Contains("p")) { dval = (1e-12); tval = tval.Replace("p", "").Trim(); }   // Piko
                else if (tval.Contains("f")) { dval = (1e-15); tval = tval.Replace("f", "").Trim(); }   // Femto
                else { dval = (1e+0); }

                     try { dblres = Convert.ToDouble(tval) * dval; }
                     catch
                     { dblres = 1.0; }

            }

            return (dblres);
        }

        private void CopyCoeffDeclaration2Clipboard(object sender, EventArgs e)
        {
            // try to cature body into clipboard
            try
            {
                GenerateCode(sender, e);
                Clipboard.SetText(txtSyntaxEditorCSource.Text);
                MessageBox.Show("Coefficient Declaration has been successfully copied into Clipboard", Application.ProductName,
                                                MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            catch
            {
                MessageBox.Show("An unknown error occured during access to clipboard.\t\r\n" +
                                "values could not be copied.", Application.ProductName + " Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return;
        }

        private void CopySDomainBodeData2Clipboard(object sender, EventArgs e)
        {

            // try to cature body into clipboard
            try
            {
                int i = 0;
                StringBuilder output = new StringBuilder();

                output.Append("Frequency [Hz]\t" + "Magnitude(s) [dB]\t" + "Phase(s) [°]\r\n");

                for (i = 0; i < cNPNZ.TransferFunction.DataPoints; i++)
                {
                    output.Append(
                        cNPNZ.TransferFunction.FrequencyPoint[i].ToString("F14") + "\t" +
                        cNPNZ.TransferFunction.MagGain_s[i].ToString("F14") + "\t" +
                        cNPNZ.TransferFunction.MagPhase_s[i].ToString("F14")
                        );

                    output.Append("\r\n");
                }

                Clipboard.SetText(output.ToString());

                MessageBox.Show("s-Domain Bode diagram data has been successfully copied into Clipboard", Application.ProductName,
                                MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

            }
            catch
            {
                MessageBox.Show("An unknown error occured during access to clipboard.\t\r\n" +
                                "values could not be copied.", Application.ProductName + " Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return;

        }

        private void CopyZDomainBodeData2Clipboard(object sender, EventArgs e)
        {

            // try to cature body into clipboard
            try
            {
                int i = 0;
                StringBuilder output = new StringBuilder();

                output.Append("Frequency [Hz]\t" + "Magnitude(s) [dB]\t" + "Phase(s) [°]\r\n");

                for (i = 0; i < cNPNZ.TransferFunction.DataPoints; i++)
                {
                    output.Append(
                        cNPNZ.TransferFunction.FrequencyPoint[i].ToString("F14") + "\t" +
                        cNPNZ.TransferFunction.MagGain_z[i].ToString("F14") + "\t" +
                        cNPNZ.TransferFunction.MagPhase_z[i].ToString("F14")
                        );

                    output.Append("\r\n");
                }

                Clipboard.SetText(output.ToString());

                MessageBox.Show("z-Domain Bode diagram data has been successfully copied into Clipboard", Application.ProductName,
                                MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

            }
            catch
            {
                MessageBox.Show("An unknown error occured during access to clipboard.\t\r\n" +
                                "values could not be copied.", Application.ProductName + " Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return;

        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string copyright, version;
            Assembly asm = Assembly.GetExecutingAssembly();

            copyright = ((AssemblyCopyrightAttribute)asm.GetCustomAttribute(typeof(AssemblyCopyrightAttribute))).Copyright;
            version = ((AssemblyFileVersionAttribute)asm.GetCustomAttribute(typeof(AssemblyFileVersionAttribute))).Version;

            MessageBox.Show(
                this,
                //Application.ProductName + " v" + Application.ProductVersion + "\t\r\n\r\n" + Application.CompanyName + "\r\n" +
                Application.ProductName + " v" + version + "\r\n" +
                copyright + ", " + Application.CompanyName,
                "About " + Application.ProductName,
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
                );

            return;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofdlg = new OpenFileDialog();

            if (!AskForFileSave(sender, e)) return;

            stbProgressBarLabel.Text = "Opening File:";
            stbProgressBarLabel.Visible = true;
            stbProgressBar.Visible = true;
            stbProgressBar.Value = 3;
            Application.DoEvents();

            ofdlg.Filter = "Microchip Digital Control Loop Designer files (*.dcld)|*.dcld|All files (*.*)|*.*";
            ofdlg.FilterIndex = 1;

            if (ofdlg.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if (ofdlg.CheckPathExists)
                    {
                        OpenFile(ofdlg.FileName.Trim());

                        UpdateTransferFunction(sender, e);

                        ProjectFileChanged = false;
                        saveToolStripMenuItem.Enabled = false;
                        toolStripButtonSave.Enabled = saveToolStripMenuItem.Enabled;

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        "Error (0x" + ex.HResult.ToString("X") + "): Could not read file from disk.\r\n" + 
                        "Original error: " + ex.Message, 
                        Application.ProductName, 
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Error);
                }
            }

            ofdlg.Dispose();

            stbProgressBar.Value = 100;
            Application.DoEvents();
            stbProgressBar.Visible = false;
            stbProgressBarLabel.Visible = false;

        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFile(sender, e);
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            SaveFile(sender, e);
        }

        private void exportHeaderFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GenerateCode(sender, e);
            return;

        }

        private void CopyCoeffTable2Clipboard(object sender, EventArgs e)
        {
            int i = 0;
            StringBuilder str_Buffer = new StringBuilder(); ;

            str_Buffer.Clear();
            str_Buffer.Append("Name" + "\t" + 
                              "Float" + "\t" +
                              "BSFT Scaler" + "\t" +
                              "Scaled Float" + "\t" + 
                              "Q15 Fractional" + "\t" + 
                              "Fixed Point Error" + "\t" + 
                              "Unsigned Integer" + "\t" + 
                              "Integer" + "\t" + 
                              "Hexadecimal" + "\t" +
                              "Binary" + 
                              "\r\n");

            for (i = 1; i <= cNPNZ.FilterOrder; i++)
            {
                str_Buffer.Append(
                        "Coeff A" + i + "\t" +
                        cNPNZ.CoeffA[i].Float64.ToString() + "\t" +
                        cNPNZ.CoeffA[i].QScaler.ToString() + "\t" +
                        cNPNZ.CoeffA[i].FloatScaledFixedPoint.ToString() + "\t" +
                        cNPNZ.CoeffA[i].QFractional.ToString() + "\t" +
                        cNPNZ.CoeffA[i].FixedPointErr.ToString() + "\t" +
                        cNPNZ.CoeffA[i].UInt.ToString() + "\t" +
                        cNPNZ.CoeffA[i].Int.ToString() + "\t" +
                        cNPNZ.CoeffA[i].Hex.ToString() + "\t" +
                        cNPNZ.CoeffA[i].Binary.ToString() + 
                        "\r\n"
                    );

            }

            for (i = 0; i <= cNPNZ.FilterOrder; i++)
            {
                str_Buffer.Append(
                        "Coeff B" + i + "\t" +
                        cNPNZ.CoeffB[i].Float64.ToString() + "\t" +
                        cNPNZ.CoeffB[i].QScaler.ToString() + "\t" +
                        cNPNZ.CoeffB[i].FloatScaledFixedPoint.ToString() + "\t" +
                        cNPNZ.CoeffB[i].QFractional.ToString() + "\t" +
                        cNPNZ.CoeffB[i].FixedPointErr.ToString() + "\t" +
                        cNPNZ.CoeffB[i].UInt.ToString() + "\t" +
                        cNPNZ.CoeffB[i].Int.ToString() + "\t" +
                        cNPNZ.CoeffB[i].Hex.ToString() + "\t" +
                        cNPNZ.CoeffB[i].Binary.ToString() +
                        "\r\n"
                    );

            }

            if (cNPNZ.ScalingMethod == clsCompensatorNPNZ.dcldScalingMethod.DCLD_SCLMOD_OUTPUT_SCALING_FACTOR)
            {
                str_Buffer.Append(
                        "Output Scaling Factor" + "\t" +
                        cNPNZ.OutputScalingFactor.Float64.ToString() + "\t" +
                        cNPNZ.OutputScalingFactor.QScaler.ToString() + "\t" +
                        cNPNZ.OutputScalingFactor.FloatScaledFixedPoint.ToString() + "\t" +
                        cNPNZ.OutputScalingFactor.QFractional.ToString() + "\t" +
                        cNPNZ.OutputScalingFactor.FixedPointErr.ToString() + "\t" +
                        cNPNZ.OutputScalingFactor.UInt.ToString() + "\t" +
                        cNPNZ.OutputScalingFactor.Int.ToString() + "\t" +
                        cNPNZ.OutputScalingFactor.Hex.ToString() + "\t" +
                        cNPNZ.OutputScalingFactor.Binary.ToString() +
                        "\r\n"
                    );            
            }

            Clipboard.SetText(str_Buffer.ToString());

            MessageBox.Show("Coefficient table output has been successfully copied into Clipboard", Application.ProductName,
                                            MessageBoxButtons.OK, MessageBoxIcon.Asterisk); 
            
            return;
        }

        private void ExportGeneratedFiles(object sender, EventArgs e)
        {
            int i = 0;
            string str_path = "", str_dum = "", str_msg = "";
            DialogResult result;
            SaveFileDialog sfdlg = new SaveFileDialog();

            // Export is only allowed if the project file has been saved by the user, to prevent that files end up in Nirvana...
            if (!File.Exists(ProjectFile.FileName))
            {
                result = MessageBox.Show(this,
                        "The recent configuration has not been copleted yet. \r\n" +
                        "\r\n" + 
                        "Features such as relative path declaration in source files and the user setting " +
                        "hitory log, will not be supported until this configuration has been saved. \r\n"+
                        "\r\n" +
                        "Please complete the configuraiton of the most recent project on the following " +
                        "window  before you continue.\r\n" +
                        "\r\n",
                        "Project Configuration Warning", 
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                OpenProjectConfigWindow();
                return;

            }

            // Generate code before export
            if (generateCodeBeforeExportToolStripMenuItem.Checked)
            {
                str_msg = "Generating following code files:\r\n\r\n";
                GenerateCode(sender, e);
            }
            else { str_msg = ""; }

            // Lease here is application is shutting down while code was updated
            if (ApplicationShutDown) 
                return;

            // Show Status bar
            stbProgressBarLabel.Text = "Exporting Generated Files:";
            stbProgressBarLabel.Visible = true;
            stbProgressBar.Visible = true;

            // Export files...
            for (i = 0; i < 4; i++)
            {
                stbProgressBar.Value = (int)(100*(i+1)/4);
                Application.DoEvents();

                // capture filename
                switch (i)
                {
                    case 0: str_path = txtASMSourcePath.Text.Trim(); break; // save assembly file
                    case 1: str_path = txtCHeaderPath.Text.Trim(); break; // save C-header file
                    case 2: str_path = txtCSourcePath.Text.Trim(); break; // save C-source file
                    case 3: str_path = txtCLibPath.Text.Trim(); break; // save CLib Header file
                    default:
                        break;
                }

                // check if filename is valid
                str_path = ConvertFilePath.Unix2Win(str_path);   // Always switch path into Windows path format

                if (str_path.Contains("." + _dsp))  // path contains relative path items
                {
                    str_path = ConvertFilePath.Unix2Win(ConvertFilePath.ToAbsoluteFilePath(str_path, MPLABXProject.MPLABXIncludeDirectoryAbsolute));
                }
                
                else if ((str_path.Length < 3) && (str_path.Substring(0, 2) != "." + _dsp) && (str_path.Substring(0, 3) != ".." + _dsp))
                {
                    MessageBox.Show(
                        "No target directory has been defined yet.\r\n\r\n" +
                        "Please specify a directory where you'd like to store the generated code files.\t",
                        "Code Generation Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);

                    return;
                }
                 

                if ((str_path.Substring(str_path.Length - 2, 2).ToLower() == ".s") ||
                    (str_path.Substring(str_path.Length - 2, 2).ToLower() == ".h") ||
                    (str_path.Substring(str_path.Length - 2, 2).ToLower() == ".c"))
                { 
                    string[] str_arr;
                    string[] dum_sep = new string[1];

                    dum_sep[0] = (_dsp);
                    str_arr = str_path.Split(dum_sep, StringSplitOptions.RemoveEmptyEntries); ;
                    str_dum = str_arr[Convert.ToInt32(str_arr.GetUpperBound(0))];
                    str_path = str_path.Substring(0, str_path.IndexOf(str_dum));
                }

                if (str_path.Substring(str_path.Length - _dsp.Length, _dsp.Length) != _dsp)
                { str_path = str_path + _dsp; }


                switch (i)
                {
                    case 0: // save assembly file

                        str_msg += "  - Assembly Library File '" + lblFinalNamePrefixOutput.Text + "_asm.s'...";
                        if (assemblyLibraryExportToolStripMenuItem.Checked)
                        {
                            sfdlg.FileName = str_path + lblFinalNamePrefixOutput.Text + "_asm.s";
                            DebugInfoPrintLine(">Generating file " + sfdlg.FileName);
                            if (System.IO.Directory.Exists(str_path))
                            {
                                System.IO.File.WriteAllText(sfdlg.FileName, txtSyntaxEditorAssembly.Text);
                                str_msg += "OK" + "\r\n";
                            }
                            else
                            {

                                str_msg += "failed" + "\r\n";
                                MessageBox.Show(
                                    str_msg + "\r\n" +
                                    "Specified target directory does not exists.\r\n\r\n" +
                                    sfdlg.FileName + "\t\r\n\r\n" +
                                    "Please specify a directory where you'd like to store the generated Assembly Library file.\t",
                                    "Code Generation Error",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Exclamation);
                                return;
                            }
                        }
                        else
                        {
                            str_msg += "(skipped)" + "\r\n";
                        }
                        break;

                    case 1: // save C-header file

                        str_msg += "  - C-Header File '" + lblFinalNamePrefixOutput.Text + ".h'...";
                        if (libraryCHeaderExportToolStripMenuItem.Checked)
                        {
                            sfdlg.FileName = str_path + lblFinalNamePrefixOutput.Text + ".h";
                            DebugInfoPrintLine(">Generating file " + sfdlg.FileName);
                            if (System.IO.Directory.Exists(str_path)) 
                            {
                                System.IO.File.WriteAllText(sfdlg.FileName, txtSyntaxEditorCHeader.Text);
                                str_msg += "OK" + "\r\n";
                            }
                            else
                            {
                                str_msg += "failed" + "\r\n";
                                MessageBox.Show(
                                    str_msg + "\r\n" +
                                    "Specified target directory does not exists.\r\n\r\n" +
                                    sfdlg.FileName + "\t\r\n\r\n" +
                                    "Please specify a directory where you'd like to store the generated C-Header file.\t",
                                    "Code Generation Error",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Exclamation);
                                return;
                            }
                        }
                        else
                        {
                            str_msg += "(skipped)" + "\r\n";
                        }
                        break;

                    case 2: // save C-source file

                        str_msg += "  - C-Source File '" + lblFinalNamePrefixOutput.Text + ".c'...";
                        if (libraryCSourceFileExportToolStripMenuItem.Checked)
                        { 
                            sfdlg.FileName = str_path + lblFinalNamePrefixOutput.Text + ".c";
                            DebugInfoPrintLine(">Generating file " + sfdlg.FileName);
                            if (System.IO.Directory.Exists(str_path)) 
                            {
                                System.IO.File.WriteAllText(sfdlg.FileName, txtSyntaxEditorCSource.Text);
                                str_msg += "OK" + "\r\n";
                            }
                            else
                            {
                                str_msg += "failed" + "\r\n";
                                MessageBox.Show(
                                    str_msg + "\r\n" +
                                    "Specified target directory does not exists.\r\n\r\n" +
                                    sfdlg.FileName + "\t\r\n\r\n" +
                                    "Please specify a directory where you'd like to store the generated C-Source file.\t",
                                    "Code Generation Error",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Exclamation);
                                return;
                            }
                        }
                        else
                        {
                            str_msg += "(skipped)" + "\r\n";
                        }
                        break;

                    case 3: // save CLib Header file

                        str_msg += "  - Generic Header File 'npnz16b.h'...";
                        if (genericControlLibraryHeaderExportToolStripMenuItem.Checked)
                        {
                            sfdlg.FileName = str_path + "npnz16b.h";
                            DebugInfoPrintLine(">Generating file " + sfdlg.FileName);
                            if (System.IO.Directory.Exists(str_path))
                            { 
                                System.IO.File.WriteAllText(sfdlg.FileName, txtSyntaxEditorCLibHeader.Text);
                                str_msg += "OK" + "\r\n";
                            }
                            else
                            {
                                str_msg += "failed" + "\r\n";
                                MessageBox.Show(
                                    str_msg + "\r\n" +
                                    "Specified target directory does not exists.\r\n\r\n" +
                                    sfdlg.FileName + "\t\r\n\r\n" +
                                    "Please specify a directory where you'd like to store the generated Control Library Header file.\t",
                                    "Code Generation Error",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Exclamation);
                                return;
                            }

                        }
                        else
                        {
                            str_msg += "(skipped)" + "\r\n";
                        }
                        break;

                    default:
                        break;
                }

                

            }

            // Add controller settings to user history
            AddHistorySettings(sender, e);

            // Acknowledge successfully executed command
            if (generateCodeBeforeExportToolStripMenuItem.Checked)
            {
                MessageBox.Show(
                    str_msg + "\r\n" +
                    "All selected files have been successfully generated and exported.", 
                    Application.ProductName,
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Asterisk
                    );

            }
            else
            {

                str_msg = "File to be exported:\r\n\r\n" + str_msg;
                MessageBox.Show(
                    str_msg + "\r\n" +
                    "All generated files have been successfully exported.", 
                    Application.ProductName,
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Asterisk);
            }


            stbProgressBar.Value = 100;
            Application.DoEvents();
            stbProgressBar.Visible = false;
            stbProgressBarLabel.Visible = false;
            return;

        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {

            // Check for unsaved changes
            if (!AskForFileSave(sender, e))
            {
                e.Cancel = true;
                return;
            }

            // Set FORM CLOSING flag
            ApplicationShutDown = true;

            // Save last user settings
            SettingsFile.WriteKey("main_window", "winstate", Convert.ToInt32(WindowState).ToString());
            if (WindowState == System.Windows.Forms.FormWindowState.Normal)
            {
               SettingsFile.WriteKey("main_window", "width", Convert.ToString(Width));
               SettingsFile.WriteKey("main_window", "height", Convert.ToString(Height));
            }

            // save last Bode axis settings
           SettingsFile.WriteKey("bode_plot", "x_min", Convert.ToString(chartBode.ChartAreas["GainPhase"].AxisX.Minimum));
           SettingsFile.WriteKey("bode_plot", "x_max", Convert.ToString(chartBode.ChartAreas["GainPhase"].AxisX.Maximum));
           SettingsFile.WriteKey("bode_plot", "y1_min", Convert.ToString(chartBode.ChartAreas["GainPhase"].AxisY.Minimum));
           SettingsFile.WriteKey("bode_plot", "y1_max", Convert.ToString(chartBode.ChartAreas["GainPhase"].AxisY.Maximum));
           SettingsFile.WriteKey("bode_plot", "y2_min", Convert.ToString(chartBode.ChartAreas["GainPhase"].AxisY2.Minimum));
           SettingsFile.WriteKey("bode_plot", "y2_max", Convert.ToString(chartBode.ChartAreas["GainPhase"].AxisY2.Maximum));
           SettingsFile.WriteKey("bode_plot", "show_s_domain", Convert.ToString(Convert.ToInt32(showSDomainTransferFunctionToolStripMenuItem.Checked)));

            // save data table status
           SettingsFile.WriteKey("data_table", "visible", Convert.ToUInt16(showCoeffficientDataTableToolStripMenuItem.Checked).ToString());
           SettingsFile.WriteKey("data_table", "splitter_pos", splitContainerCoefficients.Panel2.Height.ToString());

            // save compenstor setting status
           SettingsFile.WriteKey("compensator", "visible", Convert.ToUInt16(showSourceCodeTimingToolStripMenuItem.Checked).ToString());

            // save body generation and timing analysis window status
           SettingsFile.WriteKey("code_generator", "settings_splitter_pos", splitContainerContents.SplitterDistance.ToString());
           SettingsFile.WriteKey("code_generator", "timing_visible", Convert.ToUInt16(showSourceCodeTimingToolStripMenuItem.Checked).ToString());
           SettingsFile.WriteKey("code_generator", "timing_splitter_pos", splitContainerTiming.Panel2.Height.ToString());
            

        }

        private void chartBode_AnnotationPositionChanging(object sender, AnnotationPositionChangingEventArgs e)
        {
            TextBox txtPZ = (TextBox)e.Annotation.Tag;
            double scaler = 0.0;

            MoveAnnotation = true;

            if (ChartKeyEvents != null)
            {
                if (ChartKeyEvents.Shift)
                {// Finest positioning resolution including three fractional digits
                    if (txtPZ != null) txtPZ.Text = String.Format("{0:0.000}", Math.Round(e.NewLocationX, 3));
                }
                else if (ChartKeyEvents.Control)
                {// Coarse, rounded scaling at 1/10th of the decade
                    scaler = Math.Pow(10, Math.Floor(Math.Log10(e.NewLocationX))) / 1;
                    if (txtPZ != null) txtPZ.Text = (scaler * Math.Round((e.NewLocationX / scaler), 0)).ToString();
                }
                else
                {// Normal, rounded scaling at 1/100th of the decade
                    scaler = Math.Pow(10, Math.Floor(Math.Log10(e.NewLocationX))) / 10;
                    if (txtPZ != null) txtPZ.Text = (scaler * Math.Round((e.NewLocationX / scaler), 0)).ToString();
                }

            }
            else
            {// Normal, rounded scaling at 1/100th of the decade
                scaler = Math.Pow(10, Math.Floor(Math.Log10(e.NewLocationX))) / 10;
                if (txtPZ != null) txtPZ.Text = (scaler * Math.Round((e.NewLocationX / scaler), 0)).ToString();
            }

            if (txtPZ != null) e.NewLocationX = Convert.ToDouble(txtPZ.Text);

            if (!timRefresh.Enabled)
            { 
                timRefresh.Interval = 30;
                timRefresh.Enabled = true;
            }
            return;
        }

        private void timRefresh_Tick(object sender, EventArgs e)
        {
            if (UpdateComplete)
            {
                UpdateBodePlot(sender, e, false);
                timRefresh.Enabled = false;
            }
        }

        private void frmMain_KeyDown(object sender, KeyEventArgs e)
        {
            ChartKeyEvents = new KeyEventArgs(e.KeyData);
        }

        private void frmMain_KeyUp(object sender, KeyEventArgs e)
        {
            ChartKeyEvents = null;
        }

        private void chartBode_MouseDown(object sender, MouseEventArgs e)
        {
            ChartMouseEvents = e;
            RelativeMouseMoveStart = e;
            AbsoluteMouseMoveStart = e;
            RelativeMouseMoveStop = null;
            AbsoluteMouseMoveStop = null;
        }

        private void chartBode_MouseUp(object sender, MouseEventArgs e)
        {
            MoveAnnotation = false;
            ChartMouseEvents = e;
            RelativeMouseMoveStart = null;
            AbsoluteMouseMoveStart = null;
            RelativeMouseMoveStop = e;
            AbsoluteMouseMoveStop = e;
        }

        private void chartBode_UpdateCursorMeasurement(object sender, bool ForceCursorPositionX = false, bool ForceCursorPositionY = false, double CursorX = 1.0)
        {
            double cursorX_decade = 0.0, cursorY_scaler = 0.0;
            string f_cursor_precision = "";
            PointF cursor_pos = new PointF();

            try { 

                if (ForceCursorPositionX)
                { 
                    cursor_pos.X = (float)CursorX;
                    chartBode.ChartAreas["GainPhase"].CursorX.SetCursorPixelPosition(cursor_pos, true);
                }

                cursorY_scaler = (chartBode.ChartAreas["GainPhase"].CursorX.Position / chartBode.ChartAreas["GainPhase"].CursorX.Interval);
                if ((int)cursorY_scaler < 0) cursorY_scaler = cNPNZ.TransferFunction.DataPoints-1;
                if (cursorY_scaler > cNPNZ.TransferFunction.DataPoints - 1) cursorY_scaler = cNPNZ.TransferFunction.DataPoints - 1;
                cursor_pos.Y = (float)cNPNZ.TransferFunction.MagGain_z[(int)cursorY_scaler];

                if (ForceCursorPositionY) { chartBode.ChartAreas["GainPhase"].CursorY.SetCursorPosition((double)cursor_pos.Y); }

                cursorX_decade = Math.Log10(cNPNZ.TransferFunction.FrequencyPoint[(int)cursorY_scaler]);

                if (cursorX_decade > 3) cursorX_decade = 3;
                f_cursor_precision = ("N" + (3 - (int)cursorX_decade).ToString());
                txtBodeCursorFrequency.Text = cNPNZ.TransferFunction.FrequencyPoint[(int)cursorY_scaler].ToString(f_cursor_precision, CultureInfo.InvariantCulture);
                txtBodeCursorMagnitude.Text = cursor_pos.Y.ToString("0.0");
                txtBodeCursorPhase.Text = cNPNZ.TransferFunction.MagPhase_z[(int)cursorY_scaler].ToString("0.0");
                txtBodePhaseErrosion.Text = (cNPNZ.TransferFunction.MagPhase_z[(int)cursorY_scaler] - cNPNZ.TransferFunction.MagPhase_s[(int)cursorY_scaler]).ToString("0.0");

            }
            catch
            {
                DebugInfoPrintLine(">chartBode_UpdateCursorMeasurement(" + ForceCursorPositionX.ToString() + ", " + ForceCursorPositionY.ToString() + ", " + CursorX.ToString() + ") Exception");
            }

            return;
        }

        private void chartBode_ResetCursorMeasurement(object sender, bool HideCursor = false)
        {
            double nan = Double.NaN; 
            PointF cursor_pos = new PointF();

            try { 

                if (HideCursor)
                { 
                cursor_pos.X = (float)nan;
                cursor_pos.Y = (float)nan;
                chartBode.ChartAreas["GainPhase"].CursorX.SetCursorPosition((double)cursor_pos.Y);
                chartBode.ChartAreas["GainPhase"].CursorY.SetCursorPosition((double)cursor_pos.Y);
                } 
            
                txtBodeCursorFrequency.Text = "0";
                txtBodeCursorMagnitude.Text = "0";
                txtBodeCursorPhase.Text = "0";
                txtBodePhaseErrosion.Text = "0";

            }
            catch
            {
                DebugInfoPrintLine(">chartBode_ResetCursorMeasurement(" + HideCursor.ToString() + ") Exception");
            }

            return;
        }

        private void chartBode_MouseMove(object sender, MouseEventArgs e)
        {
            int i = 0;
            double log_decade_scaler = 0.0, lin_gain_scaler = 0.0, lin_phase_scaler = 0.0;
            double min_decade = 0.0, max_decade = 0.0, new_setting=0.0;
            double min_scale = 0.0, max_scale = 0.0, anno_pos = 0.0;
            int mouse_drag_filter = 50;

            bool ResetCursor = false;
            Point chartArea_location= new Point();
            Size chartArea_size = new Size();
            chartRegions ActiveChartRegion = new chartRegions();
            bool SetAnnotationCursor = false; 

            try
            {

                // Copy MoseMoveEvents for other processes
                ChartMouseEvents = e;

                // capture chart location and size
                chartArea_location.X = (int)Math.Round((chartBode.ChartAreas["GainPhase"].AxisX.GetPosition(chartBode.ChartAreas["GainPhase"].AxisX.Minimum) / 100) * chartBode.Width, 0);
                chartArea_location.Y = (int)Math.Round((chartBode.ChartAreas["GainPhase"].AxisY.GetPosition(chartBode.ChartAreas["GainPhase"].AxisY.Maximum) / 100) * chartBode.Height, 0);
                chartArea_size.Width = (int)Math.Round((chartBode.ChartAreas["GainPhase"].AxisX.GetPosition(chartBode.ChartAreas["GainPhase"].AxisX.Maximum) / 100) * chartBode.Width, 0) - chartArea_location.X;
                chartArea_size.Height = (int)Math.Round((chartBode.ChartAreas["GainPhase"].AxisY.GetPosition(chartBode.ChartAreas["GainPhase"].AxisY.Minimum) / 100) * chartBode.Height, 0) - chartArea_location.Y;

                // Dividing chart area into valid regions to identify axes operations
                if ((chartArea_location.X < e.X) && (e.X < (chartArea_location.X + chartArea_size.Width)) && (e.Y > (chartArea_location.Y + chartArea_size.Height)))
                {// Mouse is within X-Axes region
                    ActiveChartRegion = chartRegions.chartXAxes;
                    chartBode.Cursor = Cursors.Hand;
                }
                else if ((e.X < chartArea_location.X) && (chartArea_location.Y < e.Y) && (e.Y < (chartArea_location.Y + chartArea_size.Height)))
                {// Mouse is within the primary Y-Axes region
                    ActiveChartRegion = chartRegions.chartPrimaryYAxis;
                    chartBode.Cursor = Cursors.Hand;
                }
                else if ((e.X > (chartArea_location.X + chartArea_size.Width)) && (chartArea_location.Y < e.Y) && (e.Y < (chartArea_location.Y + chartArea_size.Height)))
                {// Mouse is within the secondary Y-Axes region
                    ActiveChartRegion = chartRegions.chartSecondaryYAxes;
                    chartBode.Cursor = Cursors.Hand;
                }
                else if ((chartArea_location.X < e.X) && (e.X < (chartArea_location.X + chartArea_size.Width)) && (e.Y < chartArea_location.Y))
                {// Mouse is within the legend region above the chart
                    ActiveChartRegion = chartRegions.chartLegend;
                    chartBode.Cursor = Cursors.Default;
                }
                else if ((chartArea_location.X < e.X) && (e.X < (chartArea_location.X + chartArea_size.Width)) && (chartArea_location.Y < e.Y) && (e.Y < chartArea_location.Y + chartArea_size.Height))
                {// Mouse is within the printing region of the chart
                    ActiveChartRegion = chartRegions.chartPrintArea;

                    for (i=0; i<chartBode.Annotations.Count; i++)
                    {
                        anno_pos = Math.Round(chartBode.ChartAreas["GainPhase"].AxisX.GetPosition(chartBode.Annotations[i].X)/100 * chartBode.Width, 0);

                        if (((anno_pos - 5) < e.X) && (e.X < (anno_pos + 5)))
                        {
                            SetAnnotationCursor = true;
                            break;
                        }
                    }

                    if (SetAnnotationCursor) chartBode.Cursor = Cursors.VSplit;
                    else chartBode.Cursor = Cursors.Cross;

                    // Set measurement cursor
                    if ((!FreezeBodeCursor) && (chkBodeCursor.Checked))
                    {
                        chartBode_UpdateCursorMeasurement(sender, true, true, e.X);
                        ResetCursor = false;
                    }
                    else if (FreezeBodeCursor) { ResetCursor = false; }
                    else { ResetCursor = true; }
                }
                else 
                {// mouse is in some other region{
                    chartBode.Cursor = Cursors.Default;
                    return;
                }

                if (((ActiveChartRegion != chartRegions.chartPrintArea) && (!FreezeBodeCursor)) || (ResetCursor))
                {
                    chartBode_ResetCursorMeasurement(sender, true);
                }

                if (RelativeMouseMoveStart == null) return;
                if (MoveAnnotation) return;

                // to make mouse operations more convenient, the movement is filtered to minimum distances before a response is triggered
                mouse_drag_filter = chartBode.Width / 20;


                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {

                    // Default scaling factors for X, Y and Y2 axes
                    log_decade_scaler = 1.0;
                    lin_gain_scaler = 10.0;
                    lin_phase_scaler = 30.0;

                    switch (ActiveChartRegion)
                    {
                        case chartRegions.chartXAxes:   // Mouse is within X-Axes region

                            min_decade = Math.Floor(Math.Log10(chartBode.ChartAreas["GainPhase"].AxisX.Minimum));
                            max_decade = Math.Floor(Math.Log10(chartBode.ChartAreas["GainPhase"].AxisX.Maximum));

                            if (RelativeMouseMoveStart.X < chartArea_location.X + (chartArea_size.Width / 2))
                            { // Adjusting X-axes minimum 

                                if ((RelativeMouseMoveStart.X - e.X) > mouse_drag_filter)
                                {
                                    // Move X-Axis minimum to the right (shift scale to the left) => Minimum gets greater
                                    if ((min_decade + 1) < max_decade)
                                    {
                                        new_setting = Math.Pow(10, (min_decade + log_decade_scaler));
                                        chartBode.ChartAreas["GainPhase"].AxisX.Minimum = new_setting;
                                        RelativeMouseMoveStart = e;
                                    }
                                }
                                else if ((RelativeMouseMoveStart.X - e.X) < -mouse_drag_filter)
                                {
                                    // Move X-Axis minimum to the left (shift scale to the left) => Minimum gets smaller
                                    if ((min_decade + 1) <= max_decade)
                                    {
                                        new_setting = Math.Pow(10, (min_decade - log_decade_scaler));
                                        chartBode.ChartAreas["GainPhase"].AxisX.Minimum = new_setting;
                                        RelativeMouseMoveStart = e;
                                    }
                                }

                            }
                            else if (RelativeMouseMoveStart.X > chartArea_location.X + (chartArea_size.Width / 2))
                            { // Adjusting X-axes maximum 

                                if ((RelativeMouseMoveStart.X - e.X) > mouse_drag_filter)
                                {
                                    // Move X-Axis maximum to the right (shift scale to the left) => Maximum gets greater
                                    if ((min_decade + 1) <= max_decade)
                                    {
                                        new_setting = Math.Pow(10, (max_decade + log_decade_scaler));
                                        chartBode.ChartAreas["GainPhase"].AxisX.Maximum = new_setting;
                                        RelativeMouseMoveStart = e;
                                    }
                                }
                                else if ((RelativeMouseMoveStart.X - e.X) < -mouse_drag_filter)
                                {
                                    // Move X-Axis maximum to the left  (shift scale to the right) => Maximum gets smaller
                                    if ((min_decade + 1) < max_decade)
                                    {
                                        new_setting = Math.Pow(10, max_decade - log_decade_scaler);
                                        chartBode.ChartAreas["GainPhase"].AxisX.Maximum = new_setting;
                                        RelativeMouseMoveStart = e;
                                    }
                                }

                            }

                                break;

                        case chartRegions.chartPrimaryYAxis:    // Mouse is within the primary Y-Axes region

                            min_scale = chartBode.ChartAreas["GainPhase"].AxisY.Minimum;
                            max_scale = chartBode.ChartAreas["GainPhase"].AxisY.Maximum;

                            if (RelativeMouseMoveStart.Y > chartArea_location.Y + (chartArea_size.Height / 2))
                            { // Adjusting primary Y-axes minimum 

                                if ((e.Y - RelativeMouseMoveStart.Y) > mouse_drag_filter)
                                {// Move Y-Axis minimum down (shift scale up) => Minimum gets greater

                                    if ((min_scale + lin_gain_scaler) < max_scale)
                                    {
                                        new_setting = min_scale + lin_gain_scaler;
                                        chartBode.ChartAreas["GainPhase"].AxisY.Minimum = new_setting;
                                        RelativeMouseMoveStart = e;
                                    }
                                }
                                else if ((e.Y - RelativeMouseMoveStart.Y) < -mouse_drag_filter)
                                {// Move Y-Axis minimum up (shift scale down) => Minimum gets smaller

                                    if ((min_scale + lin_gain_scaler) <= max_scale)
                                    {
                                        new_setting = min_scale - lin_gain_scaler;
                                        chartBode.ChartAreas["GainPhase"].AxisY.Minimum = new_setting;
                                        RelativeMouseMoveStart = e;
                                    }
                                }

                            }
                            else if (RelativeMouseMoveStart.Y < chartArea_location.Y + (chartArea_size.Height / 2))
                            { // Adjusting primary Y-axes maximum  

                                if ((e.Y - RelativeMouseMoveStart.Y) > mouse_drag_filter)
                                {// Move Y-Axis maximum down (shift scale up) => Maximum gets greater

                                    if ((min_scale + lin_gain_scaler) < max_scale)
                                    {
                                        new_setting = max_scale + lin_gain_scaler;
                                        chartBode.ChartAreas["GainPhase"].AxisY.Maximum = new_setting;
                                        RelativeMouseMoveStart = e;
                                    }
                                }
                                else if ((e.Y - RelativeMouseMoveStart.Y) < -mouse_drag_filter)
                                {// Move Y-Axis maximum up (shift scale down) => Maximum gets smaller

                                    if ((min_scale + lin_gain_scaler) <= max_scale)
                                    {
                                        new_setting = max_scale - lin_gain_scaler;
                                        chartBode.ChartAreas["GainPhase"].AxisY.Maximum = new_setting;
                                        RelativeMouseMoveStart = e;
                                    }
                                }

                            }
                            break;

                        case chartRegions.chartSecondaryYAxes:    // Mouse is within the secondary Y-Axes region
                        
                            min_scale = chartBode.ChartAreas["GainPhase"].AxisY2.Minimum;
                            max_scale = chartBode.ChartAreas["GainPhase"].AxisY2.Maximum;

                            if (RelativeMouseMoveStart.Y > chartArea_location.Y + (chartArea_size.Height / 2))
                            { // Adjusting secondary Y-axes minimum 

                                if ((e.Y - RelativeMouseMoveStart.Y) > mouse_drag_filter)
                                {// Move Y-Axis minimum down (shift scale up) => Minimum gets greater

                                    if ((min_scale + lin_phase_scaler) < max_scale)
                                    {
                                        new_setting = min_scale + lin_phase_scaler;
                                        chartBode.ChartAreas["GainPhase"].AxisY2.Minimum = new_setting;
                                        RelativeMouseMoveStart = e;
                                    }
                                }
                                else if ((e.Y - RelativeMouseMoveStart.Y) < -mouse_drag_filter)
                                {// Move Y-Axis minimum up (shift scale down) => Minimum gets smaller

                                    if ((min_scale + lin_phase_scaler) <= max_scale)
                                    {
                                        new_setting = min_scale - lin_phase_scaler;
                                        chartBode.ChartAreas["GainPhase"].AxisY2.Minimum = new_setting;
                                        RelativeMouseMoveStart = e;
                                    }

                                }
                            
                            }
                            else if (RelativeMouseMoveStart.Y < chartArea_location.Y + (chartArea_size.Height / 2))
                            { // Adjusting secondary Y-axes maximum  

                                if ((e.Y - RelativeMouseMoveStart.Y) > mouse_drag_filter)
                                {// Move Y-Axis maximum down (shift scale up) => Maximum gets greater

                                    if ((min_scale + lin_phase_scaler) < max_scale)
                                    {
                                        new_setting = max_scale + lin_phase_scaler;
                                        chartBode.ChartAreas["GainPhase"].AxisY2.Maximum = new_setting;
                                        RelativeMouseMoveStart = e;
                                    }
                                }
                                else if ((e.Y - RelativeMouseMoveStart.Y) < -mouse_drag_filter)
                                {// Move Y-Axis maximum up (shift scale down) => Maximum gets smaller

                                    if ((min_scale + lin_phase_scaler) <= max_scale)
                                    {
                                        new_setting = max_scale - lin_phase_scaler;
                                        chartBode.ChartAreas["GainPhase"].AxisY2.Maximum = new_setting;
                                        RelativeMouseMoveStart = e;
                                    }
                                }

                            }

                            break;
                    }



                }
            }
            catch
            {
                DebugInfoPrintLine(">chartBode_MouseMove() Exception");
            }


            return;
        }

        private void splitContainerContent_DoubleClick(object sender, EventArgs e)
        {

            if (splitContainerCoefficients.Panel2Collapsed) splitContainerCoefficients.Panel2Collapsed = false;
            else splitContainerCoefficients.Panel2Collapsed = true;

            showCoeffficientDataTableToolStripMenuItem.Checked = (!splitContainerCoefficients.Panel2Collapsed);

            return;
        }

        private void showCoeffficientDataTableToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            splitContainerCoefficients.Panel2Collapsed = !showCoeffficientDataTableToolStripMenuItem.Checked;
            toolStripButtonShowCoefficientTable.Checked = showCoeffficientDataTableToolStripMenuItem.Checked;
            return;
        }

        private void showSourceCodeTimingToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            splitContainerTiming.Panel2Collapsed = !showSourceCodeTimingToolStripMenuItem.Checked;
            toolStripButtonShowTimingTable.Checked = showSourceCodeTimingToolStripMenuItem.Checked;
            return;
        }

        private void showCoeffficientDataTableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showCoeffficientDataTableToolStripMenuItem.Checked = !showCoeffficientDataTableToolStripMenuItem.Checked;
            return;
        }

        private void showSourceCodeTimingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showSourceCodeTimingToolStripMenuItem.Checked = !showSourceCodeTimingToolStripMenuItem.Checked;
            return;
        }

        private void frmMain_ResizeEnd(object sender, EventArgs e)
        {
            return;
        }

        private bool BrowseCodeGeneratorPath(object sender, EventArgs e)
        {

            SaveFileDialog sfdlg = new SaveFileDialog();
            string str_path = "", str_dum = "", sender_name = "";
            string[] str_arr;
            string[] dum_sep = new string[1];
            Button cmd;

            // Show "Save As..." Dialog

            sfdlg.ValidateNames = true;
            sfdlg.ValidateNames = true;
            sfdlg.CheckPathExists = true;
            sfdlg.OverwritePrompt = true;

            sfdlg.Filter = "Assembly files (*.s)|*.s|C-Header files (*.h)|*.h|C-Source files (*.c)|*.c|All files (*.*)|*.*";

            try { cmd = (Button)sender; }
            catch{ return(false); }

            sender_name = cmd.Name;
            switch (sender_name) 
            { 
                case "cmdASMSourcePath":
                    sfdlg.InitialDirectory = ConvertFilePath.Unix2Win(ConvertFilePath.ToAbsoluteFilePath(txtASMSourcePath.Text, MPLABXProject.MPLABXIncludeDirectoryAbsolute));
                    sfdlg.FileName = lblFinalNamePrefixOutput.Text + "_asm";
                    sfdlg.DefaultExt = ".s";
                    sfdlg.FilterIndex = 1;
                    break;

                case "cmdCHeaderPath":
                    sfdlg.InitialDirectory = ConvertFilePath.Unix2Win(ConvertFilePath.ToAbsoluteFilePath(txtCHeaderPath.Text, MPLABXProject.MPLABXIncludeDirectoryAbsolute));
                    sfdlg.FileName = lblFinalNamePrefixOutput.Text;
                    sfdlg.DefaultExt = ".h";
                    sfdlg.FilterIndex = 2;
                    break;

                case "cmdCLibPath":
                    sfdlg.InitialDirectory = ConvertFilePath.Unix2Win(ConvertFilePath.ToAbsoluteFilePath(txtCLibPath.Text, MPLABXProject.MPLABXIncludeDirectoryAbsolute));
                    sfdlg.FileName = "npnz16b.h";
                    sfdlg.DefaultExt = ".h";
                    sfdlg.FilterIndex = 2;
                    break;

                case "cmdCSourcePath":
                    sfdlg.InitialDirectory = ConvertFilePath.Unix2Win(ConvertFilePath.ToAbsoluteFilePath(txtCSourcePath.Text, MPLABXProject.MPLABXIncludeDirectoryAbsolute));
                    sfdlg.FileName = lblFinalNamePrefixOutput.Text;
                    sfdlg.DefaultExt = ".c";
                    sfdlg.FilterIndex = 3;
                    break;

                default:
                    break;
            }


            if (sfdlg.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if (sfdlg.CheckPathExists)
                    {
                        str_path = sfdlg.FileName.Trim();

                        dum_sep[0] = (_dsp);
                        str_arr = str_path.Split(dum_sep, StringSplitOptions.RemoveEmptyEntries); ;
                        str_dum = str_arr[Convert.ToInt32(str_arr.GetUpperBound(0))];
                        str_path = str_path.Substring(0, str_path.IndexOf(str_dum));

                        switch (sender_name)
                        {
                                
                            case "cmdASMSourcePath":
                                txtASMSourcePath.Text = ConvertFilePath.Unix2Win(ConvertFilePath.ToRelativeFilePath(str_path, MPLABXProject.MPLABXIncludeDirectoryAbsolute));
                                break;

                            case "cmdCHeaderPath":
                                txtCHeaderPath.Text = ConvertFilePath.Unix2Win(ConvertFilePath.ToRelativeFilePath(str_path, MPLABXProject.MPLABXIncludeDirectoryAbsolute));
                                break;

                            case "cmdCLibPath":
                                txtCLibPath.Text = ConvertFilePath.Unix2Win(ConvertFilePath.ToRelativeFilePath(str_path, MPLABXProject.MPLABXIncludeDirectoryAbsolute));
                                break;

                            case "cmdCSourcePath":
                                txtCSourcePath.Text = ConvertFilePath.Unix2Win(ConvertFilePath.ToRelativeFilePath(str_path, MPLABXProject.MPLABXIncludeDirectoryAbsolute));
                                break;

                            default:
                                break;
                        }

                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        "Error (0x" + ex.HResult.ToString("X") + "): Could not identify path.\r\n" + 
                        "Original error: " + ex.Message,
                        Application.ProductName,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation,
                        MessageBoxDefaultButton.Button1
                        );
                    // Restore output window
                    sfdlg.Dispose();

                    return (false);

                }
            }
            else
            {
                // Restore output window
                sfdlg.Dispose();

                return (false);
            }

            return (false);

        }

        private void ForceCoefficientsUpdate(object sender, EventArgs e)
        {

            // Now refresh controller coefficients and calculate transfer function data
            cNPNZ.AutoUpdate = true;

            // show code generation update warning in source code windows
            tsbCodeGenUpdateWarningAssembly.Visible = true;
            tsbCodeGenUpdateWarningCSource.Visible = true;
            tsbCodeGenUpdateWarningCHeader.Visible = true;
            tsbCodeGenUpdateWarningLibHeader.Visible = true;
            tsbCodeGenUpdateWarningTiming.Visible = true;

            // Now refresh controller coefficients and calculate transfer function data
            cNPNZ.AutoUpdate = false;

        }

        public static Control FindByTag(Control root, string tag)
        {
            if (root == null) return null;
            if (root.Tag is string && (string)root.Tag == tag) return root;
            return (from Control control in root.Controls
                    select FindByTag(control, tag)).FirstOrDefault(c => c != null);
        }

        private void GenerateCode(object sender, EventArgs e)
        {
            int _i = 0;
            string tagName = "";
            int EditorASMScrollPos = 0;

            if (ApplicationStartUp) return;     // During the startup-phase o fhte application, suppress all updates
            if (ProjectFileLoadActive) return;  // If settings are loaded from a file, suppress all updates
            if (cNPNZ.QFormat != 15) return;    // Q15-Format supported only

            try
            {
                stbProgressBarLabel.Text = "Generating Source Code:";
                stbProgressBarLabel.Visible = true;
                stbProgressBar.Visible = true;

                stbProgressBar.Value = 10;
                Application.DoEvents();

                // capture editor windows vertical scroll status
                EditorASMScrollPos = txtSyntaxEditorAssembly.SelectedView.CurrentDisplayLine.Index;   //SelectedView.DisplayLines.IndexOf();  //.VerticalScroll.Value;

                // ========================================================================

                // Set common C-Code Generator Settings
                clsCCodeGenerator cGen = new clsCCodeGenerator();

                cGen.dcldProjectFile = ProjectFile;

                cGen.FileNamePattern = txtControllerNamePrefix.Text.Trim() + txtControllerNameLabel.Text.Trim();
                cGen.PreShift = (int)(Convert.ToUInt32(cmbQFormat.Text.Substring(1, (int)cmbQFormat.Text.Length - 1)) - Convert.ToUInt32(txtInputDataResolution.Text));

                if (txtControllerNamePrefix.Text.Trim().Length > 0)
                { cGen.PreFix = txtControllerNamePrefix.Text.Trim() + "_"; }
                else { cGen.PreFix = DefaultVariablePrefix + "_"; }

                if (chkCSourceIncludePath.Checked)
                { cGen.CHeaderIncludePath = ConvertFilePath.Win2Unix(GetIncludePath(txtCHeaderPath.Text)); }
                else
                { cGen.CHeaderIncludePath = ""; }

                if (chkCHeaderIncludePath.Checked)
                { cGen.LibHeaderIncludePath = ConvertFilePath.Win2Unix(GetIncludePath(txtCLibPath.Text)); }
                else
                { cGen.LibHeaderIncludePath = ""; }

                cGen.GeneratorScript = CCodeGeneratorScript;
                cGen.CGS_Version = CCodeGeneratorScript.ReadKey("generic", "Version", "n/a");
                cGen.CGS_VersionDate = CCodeGeneratorScript.ReadKey("generic", "Date", "n/a");
                cGen.CompTypeName = cmbCompType.Text;
                cGen.ScalingMethodName = cmbQScalingMethod.Text;

                // Set Code Generation Options
                //Control parent = new Control();
                CheckBox check = new CheckBox();

                for (_i = 0; _i < cGen.Tokens.Items.Count(); _i++)
                {
                    foreach (Control _tab in tabConfig.Controls)
                    {
                        if (_tab.GetType().ToString() == "System.Windows.Forms.TabPage")
                        {
                            foreach (Control parent in _tab.Controls)
                            {
                                if (parent.GetType().ToString() == "System.Windows.Forms.GroupBox")
                                {
                                    tagName = cGen.Tokens.Items[_i].Key;
                                    var items = parent.Controls;
                                    var item = items.Cast<Control>().FirstOrDefault(control => String.Equals(control.Tag, tagName));

                                    if (item != null)
                                    {
                                        if (item.GetType().ToString() == "System.Windows.Forms.CheckBox")
                                        {
                                            check = (CheckBox)item;
                                            cGen.Tokens.Items[_i].Enabled = (bool)(check.Checked && check.Enabled);
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                // Generate C-Source incorporating coefficients

                txtSyntaxEditorCSource.Text = cGen.BuildSource(cNPNZ).ToString(); // GenerateCSource(sender, e).ToString();
                stbProgressBar.Value = 20;
                Application.DoEvents();

                // Generate C-Header
                txtSyntaxEditorCHeader.Text = cGen.BuildCHeader(cNPNZ).ToString(); // GenerateCHeader(sender, e).ToString();
                stbProgressBar.Value = 30;
                Application.DoEvents();

                // Generate C Library Header
                txtSyntaxEditorCLibHeader.Text = cGen.BuildCLibHeader(cNPNZ).ToString(); //  GenerateCLibHeader(sender, e).ToString();
                stbProgressBar.Value = 35;
                Application.DoEvents();

                // ========================================================================

                // Generate assembly body
                txtSyntaxEditorAssembly.Document.Text = GenerateAssembly(sender, e).ToString();

                stbProgressBar.Value = 70;
                Application.DoEvents();

                // ========================================================================

                // Hide code generation update warnings
                tsbCodeGenUpdateWarningAssembly.Visible = false;
                tsbCodeGenUpdateWarningCSource.Visible = false;
                tsbCodeGenUpdateWarningCHeader.Visible = false;
                tsbCodeGenUpdateWarningLibHeader.Visible = false;
                tsbCodeGenUpdateWarningTiming.Visible = false;

                stbProgressBar.Value = 90;
                Application.DoEvents();

                // Update execution timing chart and data
                UpdateExecutionRuntime(sender, e);

                // Reset editor windows vertial scroll status
                if (EditorASMScrollPos > 0)
                    txtSyntaxEditorAssembly.SelectedView.GoToLine(EditorASMScrollPos - 1);

                stbProgressBar.Value = 100;
                Application.DoEvents();
                stbProgressBar.Visible = false;
                stbProgressBarLabel.Visible = false;
            }
            catch 
            {
                if (!ApplicationShutDown) 
                {
                    stbMainStatusLabel.Text = "Unexpected exception during code generation. The generated code may be incomplete or corrupted.";
                    stbMainStatusLabel.Image = dcld.Properties.Resources.icon_critical.ToBitmap();
                    stbMainStatusLabel.BackColor = stbMain.BackColor;
                }
            }
            
            return;
        }

        private string GetIncludePath(string PathDeclaration)
        {
            string str_dum = "";
            string ref_dum = "";

            // Trim parameter
            PathDeclaration = PathDeclaration.Trim();

            // If nothing has been specified yet, return empty string
            if (PathDeclaration == string.Empty)
                return (""); // Return Empty Path
            else
                str_dum = PathDeclaration; // Copy path declaration


            // Check for include paths
            // All files are reference to the common include directory declared in MPLAB X
            // If nothing has been declared by the user, the MPLAB X project cirectory
            // is set (location of MAKEFILE)

            if ((MPLABXProject.MPLABXIncludeDirectory.Trim() == string.Empty) &&
                (MPLABXProject.MPLABXProjectDirectory.Trim() != string.Empty))
                ref_dum = MPLABXProject.MPLABXProjectDirectory.Trim();
            else if (MPLABXProject.MPLABXIncludeDirectory.Trim() != string.Empty)
                ref_dum = MPLABXProject.MPLABXIncludeDirectory.Trim();
            else
                ref_dum = str_dum;

            // Get relative path with regards to reference path of MPLAB X include path
            str_dum = ConvertFilePath.ToRelativeFilePath(str_dum, ref_dum);

            if ((str_dum.Length > 1) && (str_dum.Substring(0, 1) == "."))
            {
                str_dum = str_dum.Replace(_dsp, _adsp);
                if ((str_dum.Length > 1) && (str_dum.Substring(str_dum.Length - _adsp.Length, _adsp.Length) != _adsp)) str_dum = str_dum + _adsp;
            }
            else
            {
                if ((str_dum.Length > 1) && (str_dum.Substring(str_dum.Length - _dsp.Length, _dsp.Length) != _dsp)) str_dum = str_dum + _dsp;
            }

            // Return corrected include path
            return (str_dum);
            
        }

        private StringBuilder GenerateAssembly(object sender, EventArgs e)
        {

            int _i = 0; // Runtime indices for loading option catalog
            string tagName = ""; // Tag name to identify related user control on frmMain

            string prefix = "";
            StringBuilder code = new StringBuilder();
            clsAssemblyGenerator AssGen = new clsAssemblyGenerator();
            //long CursorPositionLine = 0, CursorPositionColumn = 0;

            if (ApplicationStartUp) return(code);

            // Save recent cursor position
            txtSyntaxEditorAssembly.SelectionMovesOnRightClick = false;

            //CursorPositionLine = txtSyntaxEditorAssembly.Caret.EditPosition.Line;
            //CursorPositionColumn = txtSyntaxEditorAssembly.Caret.CharacterColumn;

            // Determine file _Prefix
            prefix = txtControllerNamePrefix.Text.Trim() + txtControllerNameLabel.Text.Trim();
            if (prefix.Length == 0)
            {
                MessageBox.Show("Please enter a valid controller name _Prefix or disable the user customization option.\t", "Assembly Code Generator", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                try
                {
                    txtControllerNamePrefix.Select();
                }
                catch { }
                return (code);
            }

            // Configure body generator

            // Set basic options
            AssGen.Prefix = prefix;
            AssGen.GeneratorScript = AsmGeneratorScript;
            AssGen.ScalingMethod = (int)(cNPNZ.ScalingMethod);
            AssGen.FilterOrder = (int)(cNPNZ.FilterOrder);
            AssGen.CodeOptimizationLevel = 0;
            AssGen.BidirectionalFeedback = cNPNZ.IsBidirectional;
            if (AssGen.BidirectionalFeedback) // feedback rectification option only allowed with bi-directional feedbacks
                AssGen.FeedbackRectification = cNPNZ.FeedbackRecitification; 
            else
                AssGen.FeedbackRectification = false;


            // Set Code Generation Options
            //Control parent = new Control();
            CheckBox check = new CheckBox();
            

            for (_i = 0; _i < AssGen.Tokens.Items.Count(); _i++)
            {
                foreach (Control _tab in tabConfig.Controls)
                {
                    if (_tab.GetType().ToString() == "System.Windows.Forms.TabPage") 
                    {
                        foreach (Control parent in _tab.Controls)
                        {
                            if (parent.GetType().ToString() == "System.Windows.Forms.GroupBox")
                            { 
                                tagName = AssGen.Tokens.Items[_i].Key;
                                var items = parent.Controls;
                                var item = items.Cast<Control>().FirstOrDefault(control => String.Equals(control.Tag, tagName));

                                if (item != null)
                                {
                                    if (item.GetType().ToString() == "System.Windows.Forms.CheckBox")
                                    {
                                        check = (CheckBox)item;
                                        AssGen.Tokens.Items[_i].Enabled = (bool)(check.Checked && check.Enabled);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }


            //// Set Adaptive Gain Control options
            //AssGen.AdaptiveGainModulationEnable = (chkEnableAdaptiveGainControl.Checked && chkEnableAdaptiveGainControl.Enabled);
            //AssGen.AdaptiveGainModulationAddEnableSwitch = (chkAGCAddEnable.Checked && chkAGCAddEnable.Enabled);
            //AssGen.AdaptiveGainModulationAddFunctionCall = (chkAGCAddGetModFactorFunCall.Checked && chkAGCAddGetModFactorFunCall.Enabled);

            //// set dynamic execution options
            //AssGen.SaveRestoreContext = this.chkContextSaving.Checked;
            //AssGen.SaveRestoreShadowRegisters = ((this.chkSaveRestoreShadowRegisters.Checked) && (this.chkContextSaving.Checked));
            //AssGen.SaveRestoreMACRegisters = ((this.chkSaveRestoreMACRegisters.Checked) && (this.chkContextSaving.Checked));
            //AssGen.SaveRestoreAccumulators = ((this.chkSaveRestoreAccumulators.Checked) && (this.chkContextSaving.Checked));
            //AssGen.SaveRestoreAccumulatorA = ((this.chkSaveRestoreAccumulators.Checked) && (this.chkContextSaving.Checked) && (this.chkSaveRestoreAccumulatorA.Checked));
            //AssGen.SaveRestoreAccumulatorB = ((this.chkSaveRestoreAccumulators.Checked) && (this.chkContextSaving.Checked) && (this.chkSaveRestoreAccumulatorB.Checked));
            //AssGen.SaveRestoreCoreConfig = ((this.chkSaveRestoreCoreConfig.Checked) && (this.chkContextSaving.Checked));
            //AssGen.SaveRestoreCoreStatusRegister = ((this.chkSaveRestoreCoreStatus.Checked) && (this.chkContextSaving.Checked));

            //AssGen.AddAlternateSource = ((this.chkAddAlternateSource.Checked) && (this.chkAddAlternateSource.Enabled));
            //AssGen.AddAlternateTarget = ((this.chkAddAlternateTarget.Checked) && (this.chkAddAlternateTarget.Enabled));
            //AssGen.AddADCTriggerAPlacement = ((this.chkCodeFeatureOptions.Checked) && (this.chkAddADCTriggerAPlacement.Checked));
            //AssGen.AddADCTriggerBPlacement = ((this.chkCodeFeatureOptions.Checked) && (this.chkAddADCTriggerBPlacement.Checked));
            //AssGen.AddCascadedFunctionCall = ((this.chkCodeFeatureOptions.Checked) && (this.chkAddCascadedFunctionCall.Checked));
            //AssGen.AddErrorInputNormalization = ((this.chkCodeFeatureOptions.Checked) && (this.chkAddErrorNormalization.Checked));
            //AssGen.AddEnableDisableFeature = ((this.chkCodeFeatureOptions.Checked) && (this.chkAddEnableDisable.Checked));
            //AssGen.AddDisableDummyReadFeature = ((this.chkCodeFeatureOptions.Checked) && (this.chkAddEnableDisable.Checked) && (this.chkAddDisableDummyRead.Checked));
            //AssGen.AddCoreConfig = ((this.chkCodeFeatureOptions.Checked) && (this.chkAddCoreConfig.Checked));

            //AssGen.AddAntiWindup = this.chkAntiWindup.Checked;
            //AssGen.AntiWindupSoftDesaturationFlag = ((this.chkAntiWindupSoftDesaturationFlag.Checked) && ((this.chkAntiWindupClampMax.Checked) || (this.chkAntiWindupClampMin.Checked)));
            //AssGen.AntiWindupClampMax = this.chkAntiWindupClampMax.Checked;
            //AssGen.AntiWindupClampMaxWithStatusFlag = ((this.chkAntiWindupMaxStatusFlag.Checked) && (this.chkAntiWindupClampMax.Checked));
            //AssGen.AntiWindupClampMin = this.chkAntiWindupClampMin.Checked;
            //AssGen.AntiWindupClampMinWithStatusFlag = ((this.chkAntiWindupMinStatusFlag.Checked) && (this.chkAntiWindupClampMin.Checked));

            //AssGen.CreateCopyOfMostRecentControlInput = ((this.chkCodeFeatureOptions.Checked) && (this.chkAddDataProviderControlInput.Checked) && (chkDataProviderSource.Checked));
            //AssGen.CreateCopyOfMostRecentErrorInput = ((this.chkCodeFeatureOptions.Checked) && (this.chkAddDataProviderErrorInput.Checked) && (chkDataProviderSource.Checked));
            //AssGen.CreateCopyOfMostRecentControlOutput = ((this.chkCodeFeatureOptions.Checked) && (this.chkAddDataProviderControlOutput.Checked) && (chkDataProviderSource.Checked));

            //AssGen.StoreReloadAccLevel1 = ((this.chkCodeFeatureOptions.Checked) && (this.chkStoreReloadAccLevel1.Checked) && (this.chkStoreReloadAccLevel1.Enabled) && (AssGen.CodeOptimizationLevel == 1));

            // Start body generation by adding generator header

            AssGen.CustomComment = "; **********************************************************************************" + "\r\n" +
                                   ";  SDK Version: " + Application.ProductName + " v" + Application.ProductVersion + "\r\n" +
                                   ";  AGS Version: " + AsmGeneratorScript.ReadKey("generic", "Name", "") +
                                                    " v" + AsmGeneratorScript.ReadKey("generic", "Version", "") +
                                                    " (" + AsmGeneratorScript.ReadKey("generic", "Date", "") + ")" + "\r\n" +
                                   ";  Author:      " + Environment.UserName + "\r\n" +
                                   ";  Date/Time:   " + System.DateTime.Now.ToString() + "\r\n" +
                                   "; **********************************************************************************" + "\r\n" +
                                   ";  " + cNPNZ.FilterOrder.ToString() + "P" + cNPNZ.FilterOrder.ToString() + "Z Control Library File (" + AssGen.ScalingMethodDescription + ")" + "\r\n" +
                                   "; **********************************************************************************" + "\r\n";
                                    
            // add body from the body generator
            code.Append(AssGen.BuildCode());

            lblNumberOfInstructionCycles.Text = AssGen.CycleCountTotal.ToString();
            lblNumberOfInstructionCyclesRead.Text = AssGen.CycleCountToDataCapture.ToString();
            lblNumberOfInstructionCyclesResponse.Text = AssGen.CycleCountToWriteback.ToString();

            return (code);

        }

        private void UpdateExecutionRuntime(object sender, EventArgs e)
        {
            int i = 0;
            double pwm_freq = 0.0, pwm_per = 0.0, pwm_dc = 0.0;
            double adc_latency = 0.0, adc_trig = 0.0;
            double cpu_clk = 0.0, cpu_load = 0.0, control_start_delay = 0.0, control_latency = 0.0, control_read = 0.0, control_write = 0.0, user_trigger_delay = 0.0;
            double execution_time = 0.0, read_delay = 0.0, response_delay = 0.0;
            double pwm_fragment_start = 0.0, pwm_fragment_end = 0.0;
            int pwm_cycles = 0;
            bool edge_rising = false;
            double[] pwm_data_time;
            double[] pwm_data_level;
            double nan = Double.NaN;

            try 
            { 
                // Print execution time results
                if (txtCPUClock.TextLength > 0)
                {
                    // calculate timing table parameters
                    cpu_clk = NumberTextBox_ToDouble(txtCPUClock);
                    control_latency = 1e+3 * Convert.ToDouble(this.lblNumberOfInstructionCycles.Text) * (1.0 / cpu_clk);
                    control_read = 1e+3 * Convert.ToDouble(lblNumberOfInstructionCyclesRead.Text) * (1.0 / cpu_clk);
                    control_write = 1e+3 * Convert.ToDouble(lblNumberOfInstructionCyclesResponse.Text) * (1.0 / cpu_clk);

                }
                else { return; }


                // Set user, control and ADC delays
                if (this.txtISRLatency.Text.Length > 0) 
                    control_start_delay = Convert.ToDouble(this.txtISRLatency.Text);
                if (txtPWMDutyCycle.TextLength > 0)
                    pwm_dc = (Convert.ToDouble(txtPWMDutyCycle.Text) / 100.0);
                if (this.txtADCLatency.Text.Length > 0)
                    adc_latency = Convert.ToDouble(txtADCLatency.Text);
                if (this.txtPWMFrequency.TextLength > 0)
                {
                    pwm_freq = Convert.ToDouble(txtPWMFrequency.Text);
                    if (pwm_freq == 0.0) return;
                    pwm_per = (1e+6 / pwm_freq);
                }
                if (this.txtUserTriggerDelay.Text.Length > 0)
                    user_trigger_delay = Convert.ToDouble(this.txtUserTriggerDelay.Text);


                if ((control_latency < 0) || (float.IsInfinity((float)control_latency)))
                { return; }

                // Capture ADC trigger point

                // If annotation has been moved by the user, AnchorX = NaN and X holds the position of the adc-trigger
                adc_trig = chartTiming.Annotations["annADCTrigger"].X;
                if ((int)adc_trig < 0)
                {
                    // If AnchorX holds a number (not NaN), adc-trigger is set automatically
                    adc_trig = chartTiming.Annotations["annADCTrigger"].AnchorX;
                    if ((int)adc_trig >= 0)
                    {
                        // If both settings = NaN, set the adc-trigger by drop-down setting
                        switch(cmbTriggerPlacement.SelectedIndex)
                        {
                            case 0:     // Set trigger at 50% On-Time
                                adc_trig = (pwm_per * pwm_dc) / 2.0; //(default)
                                break;
                            case 1:     // Set trigger at 50% Off-Time
                                adc_trig = (pwm_per * pwm_dc) + (pwm_per - (pwm_per * pwm_dc)) / 2.0; //(default)
                                break;
                            case 2:     // Set trigger at 50% period
                                adc_trig = pwm_per / 2.0; //(default)
                                break;
                            case 3:     // Set trigger at rising edge
                                adc_trig = 0.0; //(default)
                                break;
                            case 4:     // Set trigger at falling edge
                                adc_trig = (pwm_per * pwm_dc); //(default)
                                break;
                            default:
                                adc_trig = chartTiming.Annotations[0].X;
                                user_trigger_delay = 0;
                                break;
                        }
                        
                        
                    }
                    else{ adc_trig = (pwm_per * pwm_dc) / 2.0; } // set to default
                }

                // Incorporate user-defined trigger delay
                adc_trig += user_trigger_delay;

                // Update timing chart 
                // Build PWM Singal
            
                // determine number of PWM cycles which need to be displayed
                switch (cmbLoopTriggerOption.SelectedIndex)
                {
                    case 0: // Control loop is called by the ADC interrupt
                        pwm_cycles = Convert.ToInt32(Math.Floor((adc_trig + control_start_delay + control_latency) / pwm_per) + 1.0);
                        break;

                    case 1: // Control loop is called by the PWM interupt synchronous with ADC trigger
                        pwm_cycles = Convert.ToInt32(Math.Floor((control_start_delay + control_latency) / pwm_per) + 1.0);
                        break;
                }

                // Build time scale of PWM signal

                pwm_fragment_start = -Math.Ceiling(pwm_dc * pwm_per);
                if (pwm_fragment_start < -200.0) pwm_fragment_start = -200.0;
                pwm_fragment_end = Math.Abs(pwm_fragment_start);
            
                pwm_data_time = new double[(pwm_cycles * 4) + 4];
                pwm_data_time[0] = pwm_fragment_start;    // pre-segment
            
                for (i = 0; i < pwm_cycles; i++)
                { 
                    // Data list needs to be created
                    pwm_data_time[(i * 4) + 1] = (i *pwm_per);
                    pwm_data_time[(i * 4) + 2] = pwm_data_time[(i * 4) + 1];
                    pwm_data_time[(i * 4) + 3] = (i * pwm_per) + (pwm_dc * pwm_per);
                    pwm_data_time[(i * 4) + 4] = pwm_data_time[(i * 4) + 3];
                }

                pwm_data_time[pwm_data_time.Length - 3] = pwm_cycles * pwm_per;        // Following cycle rising edge
                pwm_data_time[pwm_data_time.Length - 2] = pwm_cycles * pwm_per;        // Following cycle rising edge
                pwm_data_time[pwm_data_time.Length - 1] = pwm_cycles * pwm_per + pwm_fragment_end;  // Following cycle post segment

                // Build data array of PWM signal level
                pwm_data_level = new double[(pwm_cycles * 4) + 4];

                edge_rising = true;
                pwm_data_level[0] = (double)Math.Abs(Convert.ToInt32(!edge_rising));

                for (i = 1; i < pwm_data_level.Length; i+=2)
                {
                    // Data list needs to be created
                    if (edge_rising)
                    {
                        pwm_data_level[i + 0] = 0;
                        if ((i + 1) < pwm_data_level.Length) pwm_data_level[i + 1] = 1;
                    }
                    else 
                    {
                        pwm_data_level[i + 0] = 1;
                        if ((i + 1) < pwm_data_level.Length) pwm_data_level[i + 1] = 0;
                    }
                    edge_rising = !edge_rising; 
                }

                chartTiming.Series["PWM"].Points.DataBindXY(pwm_data_time, pwm_data_level);

                // Create signal timing plot of ADC activity
                chartTiming.Series["ADC"].Points[1].XValue = adc_trig;
                chartTiming.Series["ADC"].Points[2].XValue = chartTiming.Series["ADC"].Points[1].XValue;
                chartTiming.Series["ADC"].Points[3].XValue = adc_trig + adc_latency;
                chartTiming.Series["ADC"].Points[4].XValue = chartTiming.Series["ADC"].Points[3].XValue;
                chartTiming.Series["ADC"].Points[5].XValue = pwm_per + pwm_fragment_end;

                // Create signal timing plot of Control Loop activity

                double control_start = 0.0, control_end = 0.0, control_read_delay = 0.0, control_write_delay = 0.0;

                switch (cmbLoopTriggerOption.SelectedIndex)
                {
                    case 0: // Control loop is called by the ADC interrupt
                        control_start = adc_trig + control_start_delay;
                        control_read_delay = adc_trig + control_start_delay + control_read;
                        control_write_delay = adc_trig + control_start_delay + control_write;
                        control_end = adc_trig + control_start_delay + control_latency;

                        break;

                    case 1: // Control loop is called by the PWM interupt synchronous with ADC trigger

                        control_start = adc_trig + adc_latency + control_start_delay;
                        control_read_delay = adc_trig + adc_latency + control_start_delay + control_read;
                        control_write_delay = adc_trig + adc_latency + control_start_delay + control_write;
                        control_end = adc_trig + adc_latency + control_start_delay + control_latency;
                        break;
                }

                chartTiming.Series["Control"].Points[1].XValue = control_start;
                chartTiming.Series["Control"].Points[2].XValue = chartTiming.Series["Control"].Points[1].XValue;
                chartTiming.Series["Control"].Points[3].XValue = control_end;
                chartTiming.Series["Control"].Points[4].XValue = chartTiming.Series["Control"].Points[3].XValue;
                chartTiming.Series["Control"].Points[5].XValue = pwm_per + pwm_fragment_end;

                // Set chart time scale
                chartTiming.ChartAreas["ControlTiming"].AxisY.Maximum = 1.2;
                chartTiming.ChartAreas["ControlTiming"].AxisX.Minimum = pwm_fragment_start;
                chartTiming.ChartAreas["ControlTiming"].AxisX.Maximum = Math.Round((pwm_cycles * pwm_per) + pwm_fragment_end, 0);
                chartTiming.ChartAreas["ControlTiming"].AxisX.IntervalOffset = Math.Abs(pwm_fragment_start);
                chartTiming.ChartAreas["ControlTiming"].AxisX.Interval = (Math.Round(1000 * Math.Ceiling((pwm_cycles * pwm_per + pwm_fragment_end) / 1000.0), 0) / 10.0);

                // Set timing event markers
                chartTiming.Annotations["annADCTrigger"].X = nan;
                chartTiming.Annotations["annADCTrigger"].AnchorX = (float)adc_trig;

                chartTiming.Annotations["annDataCapture"].X = nan;
                chartTiming.Annotations["annDataCapture"].AnchorX = (float)(control_read_delay); // (adc_trig + control_start_delay + control_read);

                chartTiming.Annotations["annDataWriteBack"].X = nan;
                chartTiming.Annotations["annDataWriteBack"].AnchorX = (float)(control_write_delay); //(adc_trig + control_start_delay + control_write);

                // Update timing table
                execution_time = (adc_trig + control_start_delay + control_latency) - adc_trig;
                read_delay = (adc_trig + control_start_delay + control_read) - adc_trig;
                response_delay = (adc_trig + control_start_delay + control_write) - adc_trig;

                cpu_load = (double)(control_start_delay + control_latency) / (1e+9 * cNPNZ.SamplingInterval);
                lblCPULoad.Text = Math.Round(100.0 * cpu_load, 1).ToString("#0.0", CultureInfo.CurrentCulture);
                lblCPULoadRatio.Text = lblCPULoad.Text + " %";
                lblCPULoadRatio.Height = Convert.ToInt32(cpu_load * pnlCPULoadRatio.Height);

                lblExecutionPeriod.Text = Math.Round(execution_time / 1e+3, 3).ToString("0.000", CultureInfo.CurrentCulture);
                lblDataReadDelay.Text = Math.Round(read_delay / 1e+3, 3).ToString("0.000", CultureInfo.CurrentCulture);
                lblResponseDelay.Text = Math.Round(response_delay / 1e+3, 3).ToString("0.000", CultureInfo.CurrentCulture);

                eventProjectFileChanged(sender, e);

                return;
            }
            catch{ return; }
        }

        private void chartTimingSetAnnotationLabelPositions(object sender, EventArgs e)
        {
            double lbl_top = 0.0;
            double ddum = 0.0;

            // Set annotation labels (triggered by PostPaint-Event to ensure all data is ready)
            try
            {

                lbl_top = (chartTiming.ChartAreas["ControlTiming"].AxisY.ValueToPixelPosition(chartTiming.ChartAreas["ControlTiming"].AxisY.Maximum));

                ddum = chartTiming.Annotations["annADCTrigger"].AnchorX;
                if ((int)ddum < 0) ddum = chartTiming.Annotations["annADCTrigger"].X;
                lblAnnoADCTrigger.Left = (int)(chartTiming.ChartAreas["ControlTiming"].AxisX.ValueToPixelPosition(ddum) - (double)lblAnnoADCTrigger.Width / 2.0);
                lblAnnoADCTrigger.Top = (int)lbl_top + lblAnnoADCTrigger.Height + 2;

                ddum = chartTiming.Annotations["annDataCapture"].AnchorX;
                if ((int)ddum < 0) ddum = chartTiming.Annotations["annDataCapture"].X;
                lblAnnoDataRead.Left = (int)(chartTiming.ChartAreas["ControlTiming"].AxisX.ValueToPixelPosition(ddum) - (double)lblAnnoDataRead.Width / 2.0);
                if (((lblAnnoADCTrigger.Left + lblAnnoADCTrigger.Width) >= lblAnnoDataRead.Left) || (lblAnnoDataWrite.Left <= (lblAnnoDataRead.Left + lblAnnoDataRead.Width)))
                { lblAnnoDataRead.Top = (int)lbl_top + 2 * (lblAnnoDataRead.Height + 2); }
                else
                { lblAnnoDataRead.Top = (int)lbl_top + lblAnnoDataRead.Height + 2; }

                ddum = chartTiming.Annotations["annDataWriteBack"].AnchorX;
                if ((int)ddum < 0) ddum = chartTiming.Annotations["annDataWriteBack"].X;
                lblAnnoDataWrite.Left = (int)(chartTiming.ChartAreas["ControlTiming"].AxisX.ValueToPixelPosition(ddum) - (double)lblAnnoDataWrite.Width / 2.0);
                if ((lblAnnoADCTrigger.Left + lblAnnoADCTrigger.Width) >= lblAnnoDataWrite.Left)
                { lblAnnoDataWrite.Top = (int)lbl_top; }
                else
                { lblAnnoDataWrite.Top = (int)lbl_top + lblAnnoDataRead.Height + 2; }

            }
            catch 
            {
                //MessageBox.Show("bleep!!!"); }
                return;
            }

            return;
        }

        private void txtControllerNamePrefix_TextChanged(object sender, EventArgs e)
        {
            lblFinalNamePrefixOutput.Text = txtControllerNamePrefix.Text.Trim() + txtControllerNameLabel.Text.Trim();
            return;
        }

        private void codeGeneratorConfig_GroupFolding(object sender, EventArgs e)
        {
            if (!chkContextSaving.Checked){ grpContextManagement.Height = GroupFolding_MinHeight; }
            else { grpContextManagement.Height = GroupFolding_grpContextSavingHeight; }

            if (!chkCodeFeatureOptions.Checked) { grpCodeFeatureOptions.Height = GroupFolding_MinHeight; }
            else { grpCodeFeatureOptions.Height = GroupFolding_grpCodeFeatureOptionsHeight; }

            if (!chkAutomatedDataInterface.Checked) { grpCodeFeatureDataIO.Height = GroupFolding_MinHeight; }
            else { grpCodeFeatureDataIO.Height = GroupFolding_grpCodeFeatureDataIOHeight; }

            if (!chkDataProviderSource.Checked) { grpDataProviderSources.Height = GroupFolding_MinHeight; }
            else { grpDataProviderSources.Height = GroupFolding_grpDataProviderSourcesHeight; }

            if (!chkAntiWindup.Checked) { grpAntiWindup.Height = GroupFolding_MinHeight; }
            else { grpAntiWindup.Height = GroupFolding_grpAntiWindupHeight; }

            return;
        }

        private void CodeGeneratorOptions_CheckedChanged(object sender, EventArgs e)
        {
            codeGeneratorConfig_GroupFolding(sender, e);
            
            GenerateCode(sender, e);
            //ForceCoefficientsUpdate(sender, e);

            eventProjectFileChanged(sender, e);
            
            return;
        }

        private void FoldingBoxEnable_CheckChanged(object sender, EventArgs e)
        {
            // generic variables
            int i = 0;
            CheckBox check, chkItem;
            GroupBox group;
            Button btn;


            // If function was called from other control than a check box, exit here
            if (sender.GetType().ToString() != "System.Windows.Forms.CheckBox")
                return;

            check = (CheckBox)sender;

            // If parent of selected check box is not a group box, exit here
            if (check.Parent.GetType().ToString() != "System.Windows.Forms.GroupBox")
                return;

            group = (GroupBox)check.Parent;

            // Let the Check Box respond before updating the GUI
            Application.DoEvents();

            // enable/disable child controls within a group box
            for (i = 0; i < group.Controls.Count; i++)
            {

                switch (group.Controls[i].GetType().ToString())
                {

                    // If control is a check box
                    case "System.Windows.Forms.CheckBox":

                        chkItem = (CheckBox)group.Controls[i];
                        

                        // ~~~~~~~~~ Handle individual exceptions ~~~~~~~~~~~~~~~~~~

                        // Context Management Box
                        if (check.Name == chkSaveRestoreAccumulatorB.Name) 
                        {
                            bool ACCBIsValid = (cNPNZ.ScalingMethod == clsCompensatorNPNZ.dcldScalingMethod.DCLD_SCLMOD_DBLSCL_FLOAT) ||
                                               (cNPNZ.ScalingMethod == clsCompensatorNPNZ.dcldScalingMethod.DCLD_SCLMOD_DUAL_BIT_SHIFT);

                            check.Enabled = (chkContextSaving.Checked & ACCBIsValid);
                        }


                        // ~~~~~~~~~
                        else if (chkItem.Name != check.Name)
                            chkItem.Enabled = check.Checked;
                        else
                        {  } // ??? //


                        break;

                    // If control is a command button
                    case "System.Windows.Forms.Button":

                        btn = (Button)group.Controls[i];

                        // ~~~~~~~~~ Handle individual exceptions ~~~~~~~~~~~~~~~~~~
    
                        // Development Tools
                        //if (check.Name == chkEnableDevelopmentTools.Name)
                        //    btn.Enabled = (chkAddPTermLoop.Checked & chkAddPTermLoop.Enabled & chkEnableDevelopmentTools.Checked);


                        // ~~~~~~~~~
                        //else
                            btn.Enabled = check.Checked;


                        break;

                    default:
                        break;
                }

            }

            // Update project settings
            CodeGeneratorOptions_CheckedChanged(sender, e);
            return;

        }


        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UpdateTransferFunction(sender, e);
            chartTimingSetAnnotationLabelPositions(sender, e);
            return;
        }

        private void chartTiming_PostPaint(object sender, ChartPaintEventArgs e)
        {
            chartTimingSetAnnotationLabelPositions(sender, e);
            return;
        }

        private void chkClampControlOutputMaximum_CheckedChanged(object sender, EventArgs e)
        {
            chkAntiWindupMaxStatusFlag.Enabled = chkAntiWindupClampMax.Checked;
            chkAntiWindupSoftDesaturationFlag.Enabled = (chkAntiWindupClampMin.Checked || chkAntiWindupClampMax.Checked);
            CodeGeneratorOptions_CheckedChanged(sender, e);
        }

        private void chkClampControlOutputMinimum_CheckedChanged(object sender, EventArgs e)
        {
            chkAntiWindupMinStatusFlag.Enabled = chkAntiWindupClampMin.Checked;
            chkAntiWindupSoftDesaturationFlag.Enabled = (chkAntiWindupClampMin.Checked || chkAntiWindupClampMax.Checked);
            CodeGeneratorOptions_CheckedChanged(sender, e);
        }

        private void chkSaveRestoreAccumulators_CheckedChanged(object sender, EventArgs e)
        {
            chkSaveRestoreAccumulatorA.Enabled = chkSaveRestoreAccumulators.Checked;
            chkSaveRestoreAccumulatorB.Enabled = chkSaveRestoreAccumulators.Checked;

            CodeGeneratorOptions_CheckedChanged(sender, e);
            return;

        }

        private void cmbQScalingMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateTransferFunction(sender, e);
            chkSaveRestoreAccumulators_CheckedChanged(sender, e);

            return;
        }

        private void cmbLoopTriggerOption_SelectedIndexChanged(object sender, EventArgs e)
        {

            UpdateExecutionRuntime(sender, e);
            return;
        }

        private void chartTiming_MouseMove(object sender, MouseEventArgs e)
        {
            double nan = Double.NaN;
            double adc_trigger = 0.0, pwm_dc = 0.0;

            PointF cursor_pos = new PointF();
            Point chartArea_location = new Point();
            Size chartArea_size = new Size();


            if (cmbTriggerPlacement.SelectedIndex < 0) cmbTriggerPlacement.SelectedIndex = 0;

            adc_trigger = chartTiming.Annotations["annADCTrigger"].AnchorX;
            if ((int)adc_trigger < 0) adc_trigger = chartTiming.Annotations["annADCTrigger"].X;
            if (adc_trigger == nan) adc_trigger = 0.0;

            if ((txtPWMDutyCycle.Text.Length > 0) && (txtPWMFrequency.Text.Length > 0))
            {
                pwm_dc = 1e+6 * (Convert.ToDouble(this.txtPWMDutyCycle.Text) / 100) / Convert.ToDouble(this.txtPWMFrequency.Text);
            }
            else{ pwm_dc = 0.0; }

            chartArea_location.X = (int)Math.Round((chartTiming.ChartAreas["ControlTiming"].AxisX.GetPosition(chartTiming.ChartAreas["ControlTiming"].AxisX.Minimum) / 100) * chartTiming.Width, 0);
            chartArea_location.Y = (int)Math.Round((chartTiming.ChartAreas["ControlTiming"].AxisY.GetPosition(chartTiming.ChartAreas["ControlTiming"].AxisY.Maximum) / 100) * chartTiming.Height, 0);
            chartArea_size.Width = (int)Math.Round((chartTiming.ChartAreas["ControlTiming"].AxisX.GetPosition(chartTiming.ChartAreas["ControlTiming"].AxisX.Maximum) / 100) * chartTiming.Width, 0) - chartArea_location.X;
            chartArea_size.Height = (int)Math.Round((chartTiming.ChartAreas["ControlTiming"].AxisY.GetPosition(chartTiming.ChartAreas["ControlTiming"].AxisY.Minimum) / 100) * chartTiming.Height, 0) - chartArea_location.Y;

            // Dividing chart area into valid regions to identify axes operations
            if ((chartArea_location.X < e.X) && (e.X < (chartArea_location.X + chartArea_size.Width)) && (chartArea_location.Y < e.Y) && (e.Y < chartArea_location.Y + chartArea_size.Height))
            {// Mouse is within the printing region of the chart

                double anno_pos = 0;

                anno_pos = Math.Round(chartTiming.ChartAreas["ControlTiming"].AxisX.GetPosition(adc_trigger) / 100 * chartTiming.Width, 0);

                if (((anno_pos - 5) < e.X) && (e.X < (anno_pos + 5)))
                { chartTiming.Cursor = Cursors.VSplit; }
                else
                { chartTiming.Cursor = Cursors.Cross; }

                if ((!FreezeTimingCursor) && (chkTimingCursor.Checked))
                {
                    // Set Cursor to Mouse Position
                    cursor_pos.X = (float)e.X;
                    cursor_pos.Y = (float)e.Y;

                    chartTiming.ChartAreas["ControlTiming"].CursorX.SetCursorPixelPosition(cursor_pos, true);
                    chartTiming.ChartAreas["ControlTiming"].CursorY.SetCursorPixelPosition(cursor_pos, true);

                    // Display position data
                    txtTimingAbsolutePosition.Text = chartTiming.ChartAreas["ControlTiming"].CursorX.Position.ToString("N", CultureInfo.InvariantCulture);
                    txtTimingRelativePosToTrigger.Text = (chartTiming.ChartAreas["ControlTiming"].CursorX.Position - adc_trigger).ToString("N", CultureInfo.InvariantCulture);
                    txtTimingRelativePosToFallingEdge.Text = (chartTiming.ChartAreas["ControlTiming"].CursorX.Position - pwm_dc).ToString("N", CultureInfo.InvariantCulture);
                }

            }
            else
            {
                chartTiming.Cursor = Cursors.Default;
                if (!FreezeTimingCursor)
                {
                    txtTimingAbsolutePosition.Text = "0";
                    txtTimingRelativePosToTrigger.Text = "0";
                    txtTimingRelativePosToFallingEdge.Text = "0";

                    cursor_pos.X = (float)nan;
                    cursor_pos.Y = (float)nan;
                    chartTiming.ChartAreas["ControlTiming"].CursorX.SetCursorPixelPosition(cursor_pos, false);
                    chartTiming.ChartAreas["ControlTiming"].CursorY.SetCursorPixelPosition(cursor_pos, false);

                    // Clear position data
                    txtTimingAbsolutePosition.Text = "0";
                    txtTimingRelativePosToTrigger.Text = "0";
                    txtTimingRelativePosToFallingEdge.Text = "0";
                }
            }

            return;

        }

        private void chartTiming_MouseLeave(object sender, EventArgs e)
        {

            if (!FreezeTimingCursor)
            {
                txtTimingAbsolutePosition.Text="0";
                txtTimingRelativePosToTrigger.Text = "0";
                txtTimingRelativePosToFallingEdge.Text = "0";
            }
            return;

        }

        private void chartTiming_CursorPositionChanged(object sender, CursorEventArgs e)
        {
            return;
        }

        private void chartTiming_Click(object sender, EventArgs e)
        {
            FreezeTimingCursor = !FreezeTimingCursor;
            return;
        }

        private void chartTiming_AnnotationPositionChanged(object sender, EventArgs e)
        {
         
            // Set Trigger Placement
            cmbTriggerPlacement.SelectedIndex = (cmbTriggerPlacement.Items.Count - 1);  // set selection to (user defined)
            
            // Update timing chart (aligning control timing to ADC trigger
            chartTimingSetAnnotationLabelPositions(sender, e);
            UpdateExecutionRuntime(sender, e);

            eventProjectFileChanged(sender, e);

            return;
        }

        private void cmbTriggerPlacement_SelectedIndexChanged(object sender, EventArgs e)
        {
            double nan = Double.NaN;
            PointF cursor_pos = new PointF();
            
            //User has manually moved trigger-annotation
            if (cmbTriggerPlacement.SelectedIndex == (cmbTriggerPlacement.Items.Count - 1)) return;

            // Reset Annotation
            cursor_pos.Y = (float)0.0;
            chartTiming.Annotations["annADCTrigger"].X = nan;
            chartTiming.Annotations["annADCTrigger"].AnchorX = (float)cursor_pos.X;

            UpdateExecutionRuntime(sender, e);
            chartTimingSetAnnotationLabelPositions(sender, e);

            eventProjectFileChanged(sender, e);            
            
            return;
        }

        private void chartBode_SetScales(int XAxesLimitType = 0)
        {

            try
            {
                // Set scales to show all available data
                chartBode.ChartAreas["GainPhase"].AxisX.IsLogarithmic = true;

                switch (XAxesLimitType)
                {
                    case 1:
                        chartBode.ChartAreas["GainPhase"].AxisX.Maximum = cNPNZ.SamplingFrequency / 2;
                        break;

                    case 2:
                        chartBode.ChartAreas["GainPhase"].AxisX.Maximum = cNPNZ.SamplingFrequency;
                        break;

                    case 3:
                        chartBode.ChartAreas["GainPhase"].AxisX.Minimum = cNPNZ.TransferFunction.StartFrequency;
                        chartBode.ChartAreas["GainPhase"].AxisX.Maximum = cNPNZ.TransferFunction.StopFrequency;
                        break;

                    default:
                        if (cNPNZ.TransferFunction.StartFrequency > 0) chartBode.ChartAreas["GainPhase"].AxisX.Minimum = cNPNZ.TransferFunction.StartFrequency;
                        else chartBode.ChartAreas["GainPhase"].AxisX.Minimum = 1;

                        if (cNPNZ.TransferFunction.StopFrequency > cNPNZ.TransferFunction.StartFrequency) chartBode.ChartAreas["GainPhase"].AxisX.Maximum = cNPNZ.TransferFunction.StopFrequency;
                        else chartBode.ChartAreas["GainPhase"].AxisX.Maximum = 1e+6;

                        // set Y-Scales
                        chartBode.ChartAreas["GainPhase"].AxisY.Minimum = -60;
                        chartBode.ChartAreas["GainPhase"].AxisY.Maximum = 60;
                        chartBode.ChartAreas["GainPhase"].AxisY2.Minimum = -180;
                        chartBode.ChartAreas["GainPhase"].AxisY2.Maximum = 180;

                        break;
                }

            }
            catch {
                DebugInfoPrintLine(">chartBode_SetScales(" + XAxesLimitType.ToString() + ") Exception");
            }
            return;
        }

        private void cmbResetBodeChart_Click(object sender, EventArgs e)
        {

            return;
        }

        private void chkBodeCursor_CheckedChanged(object sender, EventArgs e)
        {

            if (chkBodeCursor.Checked)
            { chkBodeCursor.Text = "Cursor (on):"; }
            else
            {
                chkBodeCursor.Text = "Cursor (off):";
                chartBode_ResetCursorMeasurement(sender, false);
            }
            return;
        }

        private void chkTimingCursor_CheckedChanged(object sender, EventArgs e)
        {
            double nan = Double.NaN;
            PointF cursor_pos = new PointF();

            if (chkTimingCursor.Checked) { chkTimingCursor.Text = "Cursor (on):"; }
            else
            {
                chkTimingCursor.Text = "Cursor (off):";

                cursor_pos.X = (float)nan;
                cursor_pos.Y = (float)nan;

                chartTiming.ChartAreas["ControlTiming"].CursorX.SetCursorPixelPosition(cursor_pos, false);
                chartTiming.ChartAreas["ControlTiming"].CursorY.SetCursorPixelPosition(cursor_pos, false);

                // Display position data
                txtTimingAbsolutePosition.Text = "0";
                txtTimingRelativePosToTrigger.Text = "0";
                txtTimingRelativePosToFallingEdge.Text = "0";
            }
            return;
        }

        private void chartBode_Click(object sender, EventArgs e)
        {
            FreezeBodeCursor = !FreezeBodeCursor;
            chartBode_UpdateCursorMeasurement(sender, false, true);
            return;
        }

        private void chartBode_ScaleOptionChanged(object sender, EventArgs e)
        {
            ToolStripMenuItem itm = new ToolStripMenuItem();

            itm = (ToolStripMenuItem)sender;
            if (itm != null)
            {
                nyquistShannonLimitToolStripMenuItem.Checked = false;
                fullScaleToolStripMenuItem.Checked = false;
                samplingFrequencyToolStripMenuItem.Checked = false;

                itm.Checked = true;
                switch(itm.Name)
                {
                    case "nyquistShannonLimitToolStripMenuItem":
                        chartBode_SetScales(1);
                        break;

                    case "samplingFrequencyToolStripMenuItem":
                        chartBode_SetScales(2);
                        break;

                    case "fullScaleToolStripMenuItem":
                        chartBode_SetScales(3);
                        break;
                }

            }

            return;
        }

        private void unwrapPhaseToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            cNPNZ.TransferFunction.UnwrapPhase = unwrapPhaseToolStripMenuItem.Checked;
            cNPNZ.Update();
            UpdateBodePlot(sender, e, false);

            return;
        }

        private void GetCodeGeneratorPath(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            Application.DoEvents();
            BrowseCodeGeneratorPath(sender, e);
            this.Cursor = Cursors.Default;
            Application.DoEvents();

            return;
        }

        private void SourcePathTextBox_TextChanged(object sender, EventArgs e)
        {
            string txtName = "";

            // If a new source or header file path has been declared
            if (sender.GetType().ToString() == "System.Windows.Forms.TextBox")
            {
                TextBox txt = (TextBox)sender;
                txtName = txt.Name;

                if (txtName == txtCLibPath.Name)
                { genericControlLibraryHeaderExportToolStripMenuItem.Checked = true; }
            }
            else
            { return; }

            // Respond to changes to project settings
            eventProjectFileChanged(sender, e);

            return;
        }

        private void CopyConfigFileLocation2Clipboard(object sender, EventArgs e)
        {
            Clipboard.SetText(AsmGeneratorScript.FileName);

            MessageBox.Show(this, "Configuraiton file path has been successfully copied into the clipboard.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void FileExportItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem mnuitm;

            mnuitm = (ToolStripMenuItem)sender;
            mnuitm.Checked = !mnuitm.Checked;

            eventProjectFileChanged(sender, e);

            return;
        }

        private void DropDownList_LockingKeyDown(object sender, KeyEventArgs e)
        {
            ComboBox cmb = new ComboBox();

            if (sender.GetType().ToString() == "System.Windows.Forms.ComboBox")
            {
                cmb = (System.Windows.Forms.ComboBox)sender;

                // filter navigation keys
                if ((e.KeyCode == Keys.Up) || (e.KeyCode == Keys.Down)  || (e.KeyCode == Keys.Left) || (e.KeyCode == Keys.Right))
                {
                    cmb.DroppedDown = true;
                    return;
                }
                if ((e.KeyCode == Keys.Enter) || (e.KeyCode == Keys.Space))
                {
                    cmb.DroppedDown = false;
                    return;
                }

                e.SuppressKeyPress = true;
            }

            return;
        }

        private void IncludePathCheckBox_CheckedChanged(object sender, EventArgs e)
        {

            GenerateCode(sender, e);
            eventProjectFileChanged(sender, e);

            return;

        }

        private void chkBiDirectionalFeedback_CheckedChanged(object sender, EventArgs e)
        {
            chkFeedbackRectification.Enabled = chkBiDirectionalFeedback.Checked;
            UpdateTransferFunction(sender, e);
        }

        private void ctxCoefficientsHistory_Opening(object sender, CancelEventArgs e)
        {
            if (lstCoefficientsHistory.Items.Count == 0)
            {
                ctxCoeffSetLoad.Enabled = false;
                ctxCoeffSetRename.Enabled = false;
                ctxCoeffSetDelete.Enabled = false;
            }
            else
            {
                ctxCoeffSetLoad.Enabled = (lstCoefficientsHistory.SelectedIndices.Count > 0);
                ctxCoeffSetRename.Enabled = ctxCoeffSetLoad.Enabled;
                ctxCoeffSetDelete.Enabled = ctxCoeffSetLoad.Enabled;
            }
        }

        private void ctxCoefficientsHistory_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

            switch (e.ClickedItem.Name) 
            {
                case "ctxCoeffSetLoad":
                    LoadHistorySettings(sender, e);
                    break;

                case "ctxCoeffSetRename":
                    RenameHistorySettings(sender, e);
                    break;

                case "ctxCoeffSetDelete":
                    DeleteHistorySettings(sender, e);
                    break;

                default:
                    break;

            }
        }

        private void LoadHistorySettingsList(string project_file)
        {
            string str_dum = "";
            string[] str_arr;
            string[] dum_sep = new string[1];
            int lst_count = 0, active_item = 0, i = 0;
            ListViewItem itm = new ListViewItem();

            if (!File.Exists(project_file)) return;

            lstCoefficientsHistory.Items.Clear();

            lst_count = Convert.ToInt32(ProjectFile.ReadKey("generator_history", "count", "0"));
            active_item = Convert.ToInt32(ProjectFile.ReadKey("generator_history", "active_item", "0"));

            if (lst_count > 0)
            {
                for (i = 1; i <= lst_count; i++)
                {
                    str_dum = ProjectFile.ReadKey("generator_history", i.ToString(), "");

                    if ((str_dum.Length > 0) && (str_dum.Contains("||")))
                    {
                        dum_sep[0] = ("||");
                        str_arr = str_dum.Split(dum_sep, StringSplitOptions.RemoveEmptyEntries);

                        if (str_arr.Length >= 4)
                        {
                            itm = lstCoefficientsHistory.Items.Add(i.ToString());
                            itm.SubItems.Add(str_arr[0]);
                            itm.SubItems.Add(str_arr[1]);
                            itm.SubItems.Add(str_arr[2]);
                            itm.SubItems.Add(str_arr[3]);

                            if (active_item == i)
                            { itm.BackColor = SystemColors.ActiveCaption; }
                        }
                    }
                }
            }
            
            return;
        }

        private void AddHistorySettings(object sender, EventArgs e)
        {
            int i = 0;
            ListViewItem itm = new ListViewItem();
            string id = "", key = "", user = "", label = "", settings = "";
            Int32 save_item = 0;

            if (ProjectFile == null) return;
            if (!File.Exists(ProjectFile.FileName)) return;

            // Add time stamp
            save_item = (Convert.ToInt32(ProjectFile.ReadKey("generator_history", "count", "0")) + 1);

            id = save_item.ToString();
            key = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); // Datecode identifies the item
            label = "(Autosafe)";
            user = Environment.UserName.ToString();
            settings =
                cmbCompType.SelectedIndex.ToString() + "; " +
                cmbQScalingMethod.SelectedIndex.ToString() + "; " +
                txtInputDataResolution.Text + "; " +
                txtInputGain.Text + "; " +
                Convert.ToInt32(chkNormalizeInputGain.Checked).ToString() + "; " +
                Convert.ToInt32(chkBiDirectionalFeedback.Checked).ToString() + "; " +
                Convert.ToInt32(chkFeedbackRectification.Checked).ToString() + "; " +
                txtSamplingFrequency.Text + "; " +
                txtFP0.Text + "; " +
                txtFP1.Text + "; " +
                txtFP2.Text + "; " +
                txtFP3.Text + "; " +
                txtFP4.Text + "; " +
                txtFP5.Text + "; " +
                txtFZ1.Text + "; " +
                txtFZ2.Text + "; " +
                txtFZ3.Text + "; " +
                txtFZ4.Text + "; " +
                txtFZ5.Text;


            itm = lstCoefficientsHistory.Items.Add(save_item.ToString());
            itm.SubItems.Add(key);
            itm.SubItems.Add(user);
            itm.SubItems.Add(label);
            itm.SubItems.Add(settings);

            for (i = 0; i < lstCoefficientsHistory.Items.Count; i++)
            { lstCoefficientsHistory.Items[i].BackColor = SystemColors.Window; }
            itm.BackColor = SystemColors.ActiveCaption;

            ProjectFile.WriteKey("generator_history", "count", id);
            ProjectFile.WriteKey("generator_history", id.Trim(), key.Trim() + "||" + user.Trim() + "||" + label.Trim() + "||" + settings.Trim());
            ProjectFile.WriteKey("generator_history", "active_item", id);

            return;
        }

        private void RenameHistorySettings(object sender, EventArgs e)
        {
            string label = "";
            DialogResult result = new DialogResult();
            ListViewItem itm = new ListViewItem();

            itm = lstCoefficientsHistory.SelectedItems[0];
            label = itm.SubItems[3].Text;

            result = ShowInputDialog(this, this.Font, ref label,
                    "New Label:", "Please enter a new user-defined text for this history point:");

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                itm.SubItems[3].Text = label;
                ProjectFile.WriteKey("generator_history",
                    itm.SubItems[0].Text.Trim(),
                    itm.SubItems[1].Text.Trim() + "||" +
                    itm.SubItems[2].Text.Trim() + "||" + 
                    label.Trim() + "||" +
                    itm.SubItems[4].Text.Trim());
            }

            return;
        }

        private void DeleteHistorySettings(object sender, EventArgs e)
        {
            string id = "", aid = "";
            int i = 0, del_cnt = 0;
            ListViewItem itm = new ListViewItem();

            for (i = (lstCoefficientsHistory.SelectedItems.Count); i > 0;  i--)
            {
                itm = lstCoefficientsHistory.SelectedItems[i-1];
                id = itm.SubItems[0].Text.Trim();

                aid = ProjectFile.ReadKey("generator_history", "active_item", "0");
                if (aid.Trim().ToLower() == id.Trim().ToLower())
                    ProjectFile.WriteKey("generator_history", "active_item", "0");

                ProjectFile.DeleteKey("generator_history", id); // Remove file entry
                itm.Remove(); // Remove table item
                del_cnt++;
            
            }

            // Adjust item count
            aid = ProjectFile.ReadKey("generator_history", "count", "0");
            del_cnt = (Convert.ToInt32(aid) - del_cnt);
            if (del_cnt < 0) del_cnt = 0;
            ProjectFile.WriteKey("generator_history", "count", del_cnt.ToString());
                
            return;
        }

        private void LoadHistorySettings(object sender, EventArgs e)
        {
            int i = 0;
            string str_dum = "";
            string[] str_arr;
            string[] dum_sep = new string[1];
            ListViewItem itm = new ListViewItem();

            itm = lstCoefficientsHistory.SelectedItems[0];

            for (i = 0; i < lstCoefficientsHistory.Items.Count; i++)
            { lstCoefficientsHistory.Items[i].BackColor = SystemColors.Window; }
            itm.BackColor = SystemColors.ActiveCaption;

            if (itm.SubItems.Count < 5) return;

            // Clear the clutter generated by the ListView
            str_dum = itm.SubItems[4].ToString();
            str_dum = str_dum.Replace("ListViewSubItem", "");
            str_dum = str_dum.Replace(":", "");
            str_dum = str_dum.Replace("{", "");
            str_dum = str_dum.Replace("}", "");

            if (str_dum.Length > 0)
            {
                dum_sep[0] = (";");
                str_arr = str_dum.Split(dum_sep, StringSplitOptions.RemoveEmptyEntries);
                if (str_arr.Length >= 4)
                {
                    ApplicationStartUp = true;  // Prevent repeated GUI updates

                    // Loading transfer function settings
                    if (str_arr.Length > 0) cmbCompType.SelectedIndex = Convert.ToInt32(str_arr[0].Trim());
                    if (str_arr.Length > 1) cmbQScalingMethod.SelectedIndex = Convert.ToInt32(str_arr[1].Trim());
                    if (str_arr.Length > 2) txtInputDataResolution.Text = str_arr[2].Trim();
                    if (str_arr.Length > 3) txtInputGain.Text = str_arr[3].Trim();
                    if (str_arr.Length > 4) chkNormalizeInputGain.Checked = Convert.ToBoolean(Convert.ToInt32(str_arr[4].Trim()));
                    if (str_arr.Length > 5) chkBiDirectionalFeedback.Checked = Convert.ToBoolean(Convert.ToInt32(str_arr[5].Trim()));
                    if (str_arr.Length > 6) chkFeedbackRectification.Checked = Convert.ToBoolean(Convert.ToInt32(str_arr[6].Trim()));

                    if (str_arr.Length > 7) txtSamplingFrequency.Text = str_arr[7].Trim();
                    if (str_arr.Length > 8) txtFP0.Text = str_arr[8].Trim();
                    if (str_arr.Length > 9) txtFP1.Text = str_arr[9].Trim();
                    if (str_arr.Length > 10) txtFP2.Text = str_arr[10].Trim();
                    if (str_arr.Length > 11) txtFP3.Text = str_arr[11].Trim();
                    if (str_arr.Length > 12) txtFP4.Text = str_arr[12].Trim();
                    if (str_arr.Length > 13) txtFP5.Text = str_arr[13].Trim();
                    if (str_arr.Length > 14) txtFZ1.Text = str_arr[14].Trim();
                    if (str_arr.Length > 15) txtFZ2.Text = str_arr[15].Trim();
                    if (str_arr.Length > 16) txtFZ3.Text = str_arr[16].Trim();
                    if (str_arr.Length > 17) txtFZ4.Text = str_arr[17].Trim();
                    if (str_arr.Length > 18) txtFZ5.Text = str_arr[18].Trim();

                    if (str_arr.Length > 19) txtOutputGain.Text = str_arr[19].Trim();
                    if (str_arr.Length > 20) chkNormalizeOutputGain.Checked = Convert.ToBoolean(Convert.ToInt32(str_arr[20].Trim()));

                    ApplicationStartUp = false; //Enable repeated GUI updates
                    UpdateTransferFunction(this, EventArgs.Empty);
                    GenerateCode(this, EventArgs.Empty);

                    if (File.Exists(ProjectFile.FileName))
                    {
                        str_dum = itm.SubItems[0].Text.ToString().Trim();
                        ProjectFile.WriteKey("generator_history", "active_item", str_dum); 
                    }

                }

            }

            return;
        }

        private static DialogResult ShowInputDialog(IWin32Window owner, Font font, ref string input, string caption, string descr)
        {
            System.Drawing.Size size = new System.Drawing.Size(400, 90);
            Form inputBox = new Form();

            inputBox.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            inputBox.ClientSize = size;
            inputBox.Font = font;
            inputBox.Text = caption;

            System.Windows.Forms.Label labelLabel = new Label();
            labelLabel.TextAlign = ContentAlignment.TopLeft;
            labelLabel.Size = new System.Drawing.Size(size.Width - 10, 23);
            labelLabel.Location = new System.Drawing.Point(5, 5);
            labelLabel.Font = font;
            labelLabel.Text = descr;
            inputBox.Controls.Add(labelLabel);

            System.Windows.Forms.TextBox textBox = new TextBox();
            textBox.Size = new System.Drawing.Size(size.Width - 10, 23);
            textBox.Location = new System.Drawing.Point(labelLabel.Left, labelLabel.Top + labelLabel.Height + 5);
            textBox.Font = font;
            textBox.Text = input;
            inputBox.Controls.Add(textBox);

            Button cancelButton = new Button();
            cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new System.Drawing.Size(75, 23);
            cancelButton.Text = "&Cancel";
            cancelButton.Location = new System.Drawing.Point(size.Width - 100, textBox.Top + textBox.Height + 9);
            inputBox.Controls.Add(cancelButton);

            Button okButton = new Button();
            okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            okButton.Name = "okButton";
            okButton.Size = new System.Drawing.Size(75, 23);
            okButton.Text = "&OK";
            okButton.Location = new System.Drawing.Point(cancelButton.Left - okButton.Width - 2, cancelButton.Top);
            inputBox.Controls.Add(okButton);

            inputBox.Height = okButton.Top + okButton.Height + 49;

            inputBox.AcceptButton = okButton;
            inputBox.CancelButton = cancelButton;
            inputBox.StartPosition = FormStartPosition.CenterParent;

            DialogResult result = inputBox.ShowDialog(owner);
            input = textBox.Text;
            return result;
        }

        private void lstCoefficientsHistory_KeyDown(object sender, KeyEventArgs e)
        {

            if (lstCoefficientsHistory.SelectedItems.Count == 0) return;

            if (e.KeyCode == Keys.Delete)
            { DeleteHistorySettings(sender, e); }
            else if (e.KeyCode == Keys.F2)
            { RenameHistorySettings(sender, e); }
            else if ((e.KeyCode == Keys.Enter) && (e.Shift == true))
            { LoadHistorySettings(sender, e); }
            
            return;

        }

        private void lstCoefficientsHistory_DoubleClick(object sender, EventArgs e)
        {
            if (lstCoefficientsHistory.SelectedItems.Count == 0) return;
            LoadHistorySettings(sender, e);

            return;
        }

        private void chkAddEnableDisable_CheckedChanged(object sender, EventArgs e)
        {
            chkAddDisableDummyRead.Enabled = chkAddEnableDisable.Checked;
            CodeGeneratorOptions_CheckedChanged(sender, e);
        }

        private void chkAddPTermLoop_CheckedChanged(object sender, EventArgs e)
        {
            DialogResult msg_res = new DialogResult();

            if (!ProjectFileLoadActive)
            {
                // When enabled by user, show warning message
                if (chkAddPTermLoop.Checked)
                {
                    msg_res = MessageBox.Show(
                        "Attention!\r\n\r\n" +
                        "This P-Term control loop is by default unstable and is not suited to be used to regulate the power supply stage. " +
                        "It only serves the sole purpose of being used as regulator during measurements of the plant transfer function.\r\n\r\n" +
                        "By enabling this option, a P-Term Control Loop will be added to the assembly library file. This P-Term controller " +
                        "is meant to replace the main control loop."
                        , "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);

                    // If user denies, uncheck this option
                    chkAddPTermLoop.Checked = (bool)(msg_res == System.Windows.Forms.DialogResult.OK);

                }

                // Update project settings
                CodeGeneratorOptions_CheckedChanged(sender, e);
            }

            // Enable configuration command button
            cmdGetPTermNominalFeedback.Enabled = chkAddPTermLoop.Checked;
            cmdGetPTermNominalOutput.Enabled = chkAddPTermLoop.Checked;
            
            txtPTermNominalFeedback.Enabled = chkAddPTermLoop.Checked;
            lblPTermNominalFeedback.Enabled = chkAddPTermLoop.Checked;
            txtPTermNominalOutput.Enabled = chkAddPTermLoop.Checked;
            lblPTermNominalOutput.Enabled = chkAddPTermLoop.Checked;

        }

        private void showSDomainTransferFunctionToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            chartBode.Series[2].IsVisibleInLegend = ShowSDomainTF;
            chartBode.Series[3].IsVisibleInLegend = ShowSDomainTF;
            UpdateBodePlot(sender, e, false);
        }

        private void showSDomainTransferFunctionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (showSDomainTransferFunctionToolStripMenuItem.Checked)
                ShowSDomainTF = false;
            else
                ShowSDomainTF = true;

            showSDomainTransferFunctionToolStripMenuItem.Checked = ShowSDomainTF;
        }


        private void userGuideToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (System.IO.File.Exists(UserGuideFileName))
                System.Diagnostics.Process.Start(UserGuideFileName);
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            userGuideToolStripMenuItem.Enabled = System.IO.File.Exists(UserGuideFileName);
        }

        private void visitURLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(SettingsFile.ReadKey("common", "URL", ""));
        }

        private void cmdGetOutputGain_Click(object sender, EventArgs e)
        {
            // Create a new instance of the form class
            frmCalculateOutputGain frm = new frmCalculateOutputGain();

            // Show the settings form
            if (ctrl_output.OutputType == clsOutputDeclaration.dcldOutputType.DCLD_OUT_TYPE_UNDEFINED)
                ctrl_output.OutputType = clsOutputDeclaration.dcldOutputType.DCLD_OUT_TYPE_FIXED_FREQUENCY;

            frm.output = ctrl_output;

            frm.output.SetDeviceType(_targetDeviceType);
            //frm.output.PWMFrequency = (Convert.ToDouble(this.txtPWMFrequency.Text) * 1000.0);
            //frm.output.PWMClock = (4e+9);
            //frm.output.PWMClockDivider = 1.0;

            frm.EnableNominalControlEdits = true;

            if(frm.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                double outgain = 0.0;
                string str_outgain = "";

                outgain = frm.output.Gain;
                str_outgain = outgain.ToString("#0.000000", CultureInfo.CurrentCulture);
                txtOutputGain.Text = str_outgain;
                txtPWMFrequency.Text = (frm.output.PWMFrequency / 1000.0).ToString(CultureInfo.CurrentCulture);

                ctrl_output = frm.output;
            }
        }

        private void chkNormalizeOutputGain_CheckedChanged(object sender, EventArgs e)
        {
            chkNormalizeOutputGain.Checked = false;
            grpOutputDataNormalization.Enabled = chkNormalizeOutputGain.Checked;
            txtOutputGain.Enabled = chkNormalizeOutputGain.Checked;
            lblOutputGain.Enabled = chkNormalizeOutputGain.Checked;
            cmdGetOutputGain.Enabled = chkNormalizeOutputGain.Checked;
            UpdateTransferFunction(sender, e);
        }

        private void chkNormalizeInputGain_CheckedChanged(object sender, EventArgs e)
        {
            txtInputGain.Enabled = chkNormalizeInputGain.Checked;
            lblInputGain.Enabled = chkNormalizeInputGain.Checked;
            cmdGetInputGain.Enabled = chkNormalizeInputGain.Checked;
            UpdateTransferFunction(sender, e);
        }

        private void cmdGetInputGain_Click(object sender, EventArgs e)
        {
            // Create a new instance of the form class
            frmCalculateInputGain frm = new frmCalculateInputGain();

            // Show the settings form
            frm.feedback = feedback;
            frm.feedback.ADCResolution = cNPNZ.InputDataResolution;
            //frm.feedback.FeedbackGain = cNPNZ.InputGain;

            if (frm.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                txtInputGain.Text = frm.feedback.FeedbackGain.ToString("#0.000000", CultureInfo.CurrentCulture); ;
                txtInputDataResolution.Text = frm.feedback.ADCResolution.ToString("#0.#", CultureInfo.CurrentCulture);

                txtPTermNominalFeedback.Text = frm.feedback.FeedbackValue.ToString(CultureInfo.CurrentCulture);

                feedback = frm.feedback;
            }

        }

        private void cmdGetPTermNominalFeedback_Click(object sender, EventArgs e)
        {
            // Create a new instance of the form class
            frmCalculateInputGain frm = new frmCalculateInputGain();

            // Show the settings form
            frm.feedback = feedback;

            if (frm.feedback.ADCResolution != cNPNZ.InputDataResolution) feedback.ADCResolution = cNPNZ.InputDataResolution;

            if (frm.feedback.FeedbackType == clsFeedbackDeclaration.dcldFeedbackType.DCLD_FB_TYPE_VOLTAGE_DIVIDER)
                frm.feedback.VoltageDividerSenseVoltage = (Convert.ToDouble(txtPTermNominalFeedback.Text) * frm.feedback.ADCGranularity) / frm.feedback.VoltageDividerFeedbackGain;
            else if (frm.feedback.FeedbackType == clsFeedbackDeclaration.dcldFeedbackType.DCLD_FB_TYPE_SHUNT_AMPLIFIER)
                frm.feedback.CurrentSenseSenseCurrent = (Convert.ToDouble(txtPTermNominalFeedback.Text) * frm.feedback.ADCGranularity) / frm.feedback.CurrentSenseFeedbackGain;
            else if (frm.feedback.FeedbackType == clsFeedbackDeclaration.dcldFeedbackType.DCLD_FB_TYPE_CURRENT_TRANSFORMER)
                frm.feedback.CurrentTransformerSenseCurrent = (Convert.ToDouble(txtPTermNominalFeedback.Text) * frm.feedback.ADCGranularity) / frm.feedback.CurrentTransformerFeedbackGain;
            else if (frm.feedback.FeedbackType == clsFeedbackDeclaration.dcldFeedbackType.DCLD_FB_TYPE_DIGITAL_SOURCE)
                frm.feedback.DigitalSourceValue = (Convert.ToDouble(txtPTermNominalFeedback.Text));

            frm.EnableInputValueEdits = true;


            if (frm.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                // Set maximum feedback error value
                txtPTermNominalFeedback.Text = frm.feedback.FeedbackValue.ToString(CultureInfo.CurrentCulture);

                // Keep Input Gain and Input Resolution in Sync
                if (chkNormalizeInputGain.Checked)
                { 
                    txtInputGain.Text = frm.feedback.FeedbackGain.ToString("#0.000000", CultureInfo.CurrentCulture); ;
                    txtInputDataResolution.Text = frm.feedback.ADCResolution.ToString("#0.#", CultureInfo.CurrentCulture);
                    feedback = frm.feedback;
                }

                // Sync settings across output and input objects
                if ((frm.feedback.VoltageDividerSenseVoltage != ctrl_output.NominalOutputVoltage) && 
                    (frm.feedback.FeedbackType == clsFeedbackDeclaration.dcldFeedbackType.DCLD_FB_TYPE_VOLTAGE_DIVIDER))
                {
                    ctrl_output.NominalOutputVoltage = frm.feedback.VoltageDividerSenseVoltage;
                    txtPTermNominalOutput.Text = Math.Round(ctrl_output.NominalOutput, 0).ToString(CultureInfo.CurrentCulture);
                }
                    



                CodeGeneratorOptions_CheckedChanged(chkAddPTermLoop, e);
            }

        }


        private void cmdGetPTermNominalOutput_Click(object sender, EventArgs e)
        {
            // Create a new instance of the form class
            frmCalculateOutputGain frm = new frmCalculateOutputGain();

            // Show the settings form
            if (ctrl_output.OutputType == clsOutputDeclaration.dcldOutputType.DCLD_OUT_TYPE_UNDEFINED)
                ctrl_output.OutputType = clsOutputDeclaration.dcldOutputType.DCLD_OUT_TYPE_FIXED_FREQUENCY;

            // Sync Parameters across application
            if ((Convert.ToDouble(txtPWMFrequency.Text) * 1000) != ctrl_output.PWMFrequency)
                ctrl_output.PWMFrequency = (Convert.ToDouble(txtPWMFrequency.Text) * 1000);
            
            // Set frm objects
            frm.output = ctrl_output;
            frm.output.SetDeviceType(_targetDeviceType);
            frm.EnableNominalControlEdits = true;

            if (frm.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                // Update control output object
                txtPTermNominalOutput.Text = Math.Round(frm.output.NominalOutput, 0).ToString(CultureInfo.CurrentCulture);

                // Sync settings across output and input objects
                if ((feedback.VoltageDividerSenseVoltage != ctrl_output.NominalOutputVoltage) &&
                    (feedback.FeedbackType == clsFeedbackDeclaration.dcldFeedbackType.DCLD_FB_TYPE_VOLTAGE_DIVIDER))
                {
                    feedback.VoltageDividerSenseVoltage = frm.output.NominalOutputVoltage;
                    txtPTermNominalFeedback.Text = feedback.FeedbackValue.ToString(CultureInfo.CurrentCulture);
                }
                  
                // Capture output configuration
                ctrl_output = frm.output;

                // Update Timing Graph parameters
                txtPWMFrequency.Text = (ctrl_output.PWMFrequency / 1000.0).ToString(CultureInfo.CurrentCulture);
                txtPWMDutyCycle.Text = Math.Round((100.0 * ctrl_output.PWMDutyCycle), 1).ToString(CultureInfo.CurrentCulture);

                // Update code output
                CodeGeneratorOptions_CheckedChanged(cmdGetPTermNominalOutput, e);
            }

        }

        private void chkEnableAdaptiveGainControl_CheckedChanged(object sender, EventArgs e)
        {
            chkAGCAddGetModFactorFunCall.Enabled = (chkEnableAdaptiveGainControl.Enabled && chkEnableAdaptiveGainControl.Checked);
            chkAGCAddEnable.Enabled = (chkEnableAdaptiveGainControl.Enabled && chkEnableAdaptiveGainControl.Checked);
            CodeGeneratorOptions_CheckedChanged(sender, e);
        }


        private void toolStripButtonConfig_Click(object sender, EventArgs e)
        {
            OpenProjectConfigWindow();
        }

        private void OpenProjectConfigWindow()
        {
            // Create a new instance of the form class
            frmMPLABXProject frm = new frmMPLABXProject();

            // Show the settings form
            frm.MPLABXProject = MPLABXProject;

            // Set DCLD default filename in case no DCLD configuraiton is available
            frm.dcldDefaultFilename = DefaultProjectFileName;

            // Check if project file available
            if (ProjectFile.Directory != string.Empty)
                frm.dcldProject = ProjectFile;

            // Load Filename and Variable Prefix
            if (txtControllerNamePrefix.Text.Trim().Length > 0)
            { frm.dcldVariablePrefix = txtControllerNamePrefix.Text.Trim(); }
            else { frm.dcldVariablePrefix = DefaultVariablePrefix; }

            // Load Controller Name Label 
            if (txtControllerNameLabel.Text.Trim().Length > 0)
            { frm.dcldControllerNameLabel = txtControllerNameLabel.Text.Trim(); }
            else { frm.dcldControllerNameLabel = ""; }

            // Set option if window should be shown at startup by default
            frm.ShowWinAtStartup = showMPLABXconfigWindowAtStartup;

            // Open Window...
            if (frm.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                MPLABXProject = frm.MPLABXProject;

                // If no DCLD Project File has been defined yet...
                if (frm.dcldProject != null)
                    ProjectFile.SetFilename(frm.dcldProject.FileName);
                else if (frm.dcldDefaultFilename != string.Empty)
                    ProjectFile.Create(frm.dcldDefaultFilename, "GUI", "CreateDate", DateTime.Now.ToString());

                // Update controller names
                txtControllerNamePrefix.Text = frm.dcldVariablePrefix;
                txtControllerNameLabel.Text = frm.dcldControllerNameLabel;

                // capture selected target device
                _targetDevice = MPLABXProject.MPLABXConfiguration[MPLABXProject.ActiveConfiguration].TargetDevice;
                if (_targetDevice.Length >= 7)
                { 
                    if (_targetDevice.Substring(5, 1) == "C")
                        _targetDeviceType = _targetDevice.Substring(0, 6);
                    else
                        _targetDeviceType = _targetDevice.Substring(0, 7);

                }
                ProjectFile.WriteKey("ControlSetup", "TargetDevice", _targetDevice);

                // If both objects have been set, update path information
                if ((ProjectFile != null) && (MPLABXProject != null))
                {
                    txtMPLABXProjectDir.Text = MPLABXProject.MPLABXProjectDirectory;

                    if (txtASMSourcePath.Text.Trim() == string.Empty)
                        txtASMSourcePath.Text = ConvertFilePath.ToRelativeFilePath(MPLABXProject.MPLABXIncludeDirectoryAbsolute, MPLABXProject.MPLABXProjectDirectory);

                    if (txtCSourcePath.Text.Trim() == string.Empty)
                        txtCSourcePath.Text = ConvertFilePath.ToRelativeFilePath(MPLABXProject.MPLABXIncludeDirectoryAbsolute, MPLABXProject.MPLABXProjectDirectory);

                    if (txtCHeaderPath.Text.Trim() == string.Empty)
                        txtCHeaderPath.Text = ConvertFilePath.ToRelativeFilePath(MPLABXProject.MPLABXIncludeDirectoryAbsolute, MPLABXProject.MPLABXProjectDirectory);

                    if (txtCLibPath.Text.Trim() == string.Empty)
                        txtCLibPath.Text = ConvertFilePath.ToRelativeFilePath(MPLABXProject.MPLABXIncludeDirectoryAbsolute, MPLABXProject.MPLABXProjectDirectory);
                    
                    SaveFile(this, EventArgs.Empty);

                }

            }

            // Save if Window should be shown next time
            showMPLABXconfigWindowAtStartup = frm.ShowWinAtStartup;
            SettingsFile.WriteKey("config_window", "show", Convert.ToInt32(showMPLABXconfigWindowAtStartup).ToString());


        }

        private void ToolTip_Show(object sender, EventArgs e, string tool_tip_text)
        {
            // guarding condition...
            if (sender.GetType().ToString() != "System.Windows.Forms.PictureBox")
                return;

            PictureBox TTpic = (PictureBox)sender;
            frmToolTip TTip = new frmToolTip();

            TTip.BackColor = SystemColors.Info;
            TTip.ToolTipText = tool_tip_text;
            TTip.StartPosition = FormStartPosition.Manual;
            TTip.WinPos = TTpic.PointToScreen(Point.Empty); ;
            TTip.Show(this);

            return;

        }

        private void picInfo_MouseHover(object sender, EventArgs e)
        {
            PictureBox pic = new PictureBox();
            string msg = "";

            // If function was call from other control than a picture box, exit here
            if (sender.GetType().ToString() != "System.Windows.Forms.PictureBox")
                return;

            pic = (PictureBox)sender;

            if (pic.Name == picInfoADCLatency.Name)
                msg = "ADC Latency is the time delay between the trigger event, and the time the result is " +
                      "loaded into the respective ADC result buffer register (ADCBUFx). This time depends on " +
                      "the settings for sampling and conversion time as well as the ADC resolution. \r\n" +
                      "\r\n" +
                      "Please refer to the device data sheet for more details.";

            else if (pic.Name == picInfoAGC.Name)
                msg = "This function adds a modulation factor to the control loop code allowing runtime tuning " +
                      "of the loop gain. \r\n" +
                      "\r\n" +
                      "This option is only available for fast number scaling modes.\r\n" +
                      "Please refer to the user guide for more details.";

            else if (pic.Name == picInfoAlternateInputSource.Name)
                msg = "The NPNZ controller xupport up to two input ports. For common feedback loops only one port " +
                      "is required. The second 'Alternate Source' port is provided to support enhanced functoins where " +
                      "control inputs need to be swapped during runtime (e.g. bi-directional control loops). \r\n" +
                      "Alternatively, a second input port is required for control modes where the controller result " +
                      "has further dependencies. (e.g. Adaptive Gain Modulation) \r\n" +
                      "\r\n" +
                      "Please note:\r\n" +
                      "This option is locked when option\r\n'Adaptive Gain Control' is enabled.";

            else if (pic.Name == picInfoCascadeFunctionCall.Name)
                msg = "By enabling this function, you can specify a function pointer" + "\r\n" +
                      "to an additional function which should be executed right after" + "\r\n" +
                      "the execution of this loop has been completed." + "\r\n" +
                      "\r\n" +
                      "Please refer to the user guide for more details.";

            else if (pic.Name == picInfoCycleStats.Name)
                msg = "The number of cycles and related timings shown in this statistics is provided by the code generator " +
                      "and the number of cycles counted across the code options selected. However, these numbers are not " +
                      "considering conditional code excution such as branches and are referring to the maximum number of cycles. \r\n" +
                      "\r\n" +
                      "Hence, the total number of cycles shown may therefore differ from the real number of executions " +
                      "and this statics should be used for estimations only.";

            else if (pic.Name == picInfoDSPConfig.Name)
                msg = Application.ProductName + " uses a specific DSP core configuration to execute control loops at optimal performance.\r\n" +
                      "\r\n" +
                      "Enable this option if the DSP core is also used by other software instances with a different configuration." + "\r\n" +
                      "\r\n" +
                      "If the DSP is only used to execute control loop generated by " + Application.ProductName + ", it is recommended " +
                      "to configure the DSP in a separated routine (e.g. during device initialization) and disable this option";

            else if (pic.Name == picInfoErrNorm.Name)
                msg = "Error Normalization is required when data is read \r\n" +
                      "in integer format from the source (e.g. ADC converter)\r\n" +
                      "If the source is providing data in Q15 number format \r\n" +
                      "this option can be disabled.";

            else if (pic.Name == picInfoISRLatency.Name)
                msg = "The Interrupt Latency is the delay between an interrupt event being triggered and the " +
                      "time were the first instruction of the Interrupt Service Routine (ISR) is executed. " +
                      "This delay depends on the device clock, device architecture but also on compiler settings " +
                      "such as utilization of Alternate Working Registers (AltWREG / CTXTx) as well as interrupt " +
                      "priorities." + "\r\n" +
                      "\r\n" +
                      "Please refer to the user guide for more details.";

            else if (pic.Name == picInfoTimingPWMFrequency.Name)
                msg = "The switching frequency settingis only used to create the above waveform (blue) and does not " +
                      "influence sampling frequency and thus also does not influence CPU load estimation.";

            else if (pic.Name == picInfoUserTriggerDelay.Name)
                msg = "Most designs need to specify a defined ADC trigger delay to compensate for the propagation delay of " +
                      "internally generated PWM signals and the delayed point in time where switches will be turned on/off " +
                      "and/or feedback signal delays. \r\n" +
                      "\r\n" +
                      "This setting is highly hardware dependent and would have to be determined for each design individually";

            else if (pic.Name == picInfoPTermController.Name)
                msg = "This option enables the configuration of a simple proportional error controller (P-Term Controller). \r\n" +
                      "This control loop serves the sole purpose of regulating the output voltage during plant measurements. \r\n\r\n" + 
                      "Please note: \r\n" + 
                      "This control loop is highly unstable and requires absolutely stable operating conditions to remain working. \r\n" +
                      "Do not use this controller for regulation under normal operating conditions!" +
                      "\r\n" +
                      "";

            else
                msg = "(help message missing)";

            // Show ToolTip Window
            ToolTip_Show(sender, e, msg);
        }


    }

}
