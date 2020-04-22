using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dcld
{
    public partial class frmMPLABXProject : Form
    {
        private bool _project_loading = false;
        internal bool ShowWinAtStartup = true;

        private clsMPLABXHandler _mplabx_project = new clsMPLABXHandler();
        internal clsMPLABXHandler MPLABXProject
        {
            get { return (_mplabx_project); }
            set { _mplabx_project = value; return; }
        }

        private clsINIFileHandler _dcld_project;
        internal clsINIFileHandler dcldProject
        {
            get { return (_dcld_project); }
            set { _dcld_project = value; return; }
        }

        private string _dcld_default_filename = "";
        internal string dcldDefaultFilename
        {
            get { return (_dcld_default_filename); }
            set { _dcld_default_filename = value; return; }
        }

        private string _dcld_variable_prefix = "";
        internal string dcldVariablePrefix
        {
            get { return (_dcld_variable_prefix); }
            set { _dcld_variable_prefix = value; return; }
        }

        private string _dcld_controller_name_label = "";
        internal string dcldControllerNameLabel
        {
            get { return (_dcld_controller_name_label);  }
            set { _dcld_controller_name_label = value; return; }
        }


        public frmMPLABXProject()
        {
            InitializeComponent();
        }

        private void frmMPLABXProject_Load(object sender, EventArgs e)
        {
            // Set Window Properties
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ShowInTaskbar = false;
            
            chkShowatStartup.Checked = ShowWinAtStartup;
            LoadMPLABXProjectFile(sender, e);
            this.KeyPreview = true;
        }

        private void LoadMPLABXProjectFile(object sender, EventArgs e)
        {
            int _i = 0;

            _project_loading = true;    // Set LOAD MPLAB X PROJECT flag

            // Set MPLAB X project directory
            txtMPLABXProjectDir.Text = _mplabx_project.MPLABXProjectDirectory;
            if (_mplabx_project.MPLABXProjectName.Length == 0)
                lblTitle.Text = "(no MPLAB X® Project selected)";
            else
                lblTitle.Text = "MPLAB X® Project: " + _mplabx_project.MPLABXProjectName;

            // Load MPLAB X project configurations
            if (_mplabx_project.MPLABXConfiguration != null)
            {
                for (_i = 0; _i < _mplabx_project.MPLABXConfiguration.Length; _i++)
                    cmbActiveConfig.Items.Add(_mplabx_project.MPLABXConfiguration[_i].Name);
                cmbActiveConfig.SelectedIndex = _mplabx_project.ActiveConfiguration;
            }

            // Load Makefile path
            txtMakefileLocation.Text = _mplabx_project.MakefilePath;

            // Load DCLD project path
            if (_dcld_project != null)
                txtDCLDProjectDir.Text = dcldProject.Directory;

            // Load code generator variable prefix
            txtControllerNamePrefix.Text = _dcld_variable_prefix;

            // Load code generator controller name label
            txtControllerNameLabel.Text = _dcld_controller_name_label;

            // Load MPLAB X configurations
            _project_loading = false;
            LoadActiveProjectConfig(sender, e);

            // Run settings validation
            CheckProjectPathsValid(sender, e);

        }
        
        private void LoadActiveProjectConfig(object sender, EventArgs e)
        {
            int _c = _mplabx_project.ActiveConfiguration;
            int _i = 0;

            if (_project_loading) return;
            if (_mplabx_project.MPLABXConfiguration == null) return;

            txtCommonIncludeDir.Text = string.Empty;
            cmbIncludeDirectories.Items.Clear();
            for (_i = 0; _i < _mplabx_project.MPLABXConfiguration[_c].CommonIncludeDirectories.Length; _i++) { 
                txtCommonIncludeDir.AppendText(_mplabx_project.MPLABXConfiguration[_c].CommonIncludeDirectories[_i] + "; ");
                cmbIncludeDirectories.Items.Add(_mplabx_project.MPLABXConfiguration[_c].CommonIncludeDirectories[_i]);
            }

            txtSpecialIncludeDir.Text = string.Empty;
            for (_i = 0; _i < _mplabx_project.MPLABXConfiguration[_c].ExtraIncludeDirectories.Length; _i++) {
                txtSpecialIncludeDir.AppendText(_mplabx_project.MPLABXConfiguration[_c].ExtraIncludeDirectories[_i] + "; ");
                cmbIncludeDirectories.Items.Add(_mplabx_project.MPLABXConfiguration[_c].ExtraIncludeDirectories[_i]);
            }

            txtASMIncludeDir.Text = string.Empty;
            for (_i = 0; _i < _mplabx_project.MPLABXConfiguration[_c].ExtraIncludeDirectoriesAssembler.Length; _i++)
                txtASMIncludeDir.AppendText(_mplabx_project.MPLABXConfiguration[_c].ExtraIncludeDirectoriesAssembler[_i] + "; ");
        
            txtActiveTargetDevice.Text = _mplabx_project.MPLABXConfiguration[_c].TargetDevice;

            if (cmbIncludeDirectories.Items.Count == 0)
                cmbIncludeDirectories.Items.Add(_mplabx_project.MPLABXProjectDirectory);
            cmbIncludeDirectories.SelectedIndex = 0;


            return;
        }



        private void cmdMPLABXProjectBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofdlg = new OpenFileDialog();
            System.Windows.Forms.DialogResult ans = new System.Windows.Forms.DialogResult();
            string str_path = "";

            // Determine target directory of folder dialog
            if (_mplabx_project.MPLABXProjectDirectory.Trim().Length > 3)   // MPLAB X project location is available
            { str_path = _mplabx_project.MPLABXProjectDirectory.Trim(); }
            else 
            {
                if (_dcld_project == null) // DCLD project location is not available
                { str_path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments); }
                else if (_dcld_project != null)
                { str_path = _dcld_project.Directory; }  // DCLD project location is available
            }

            if (str_path.Substring(str_path.Length - 1, 1) != "\\") 
                str_path += "\\";

            // Set target directory of folder dialog
            ofdlg.Filter = "MPLAB X® Project File (project.xml)|project.xml";
            ofdlg.FilterIndex = 0;
            ofdlg.FileName = str_path + "project.xml";

            while ((ans == System.Windows.Forms.DialogResult.None) || (ans == System.Windows.Forms.DialogResult.Retry))
            {
                // Show Folder Browse Dialog
                if (ofdlg.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // Read selected directory path
                        str_path = ofdlg.FileName;

                        if (_mplabx_project.SetMPLABXProject(str_path))
                        {
                            LoadMPLABXProjectFile(sender, e); 
                            ans = System.Windows.Forms.DialogResult.OK;
                        }
                        else
                        {

                            ans = MessageBox.Show(
                                        "Selected directory \r\n\r\n'" + str_path + "' is not a valid MPLAB X® project directory.\r\n\r\n" +
                                        "Please select a valid MPLAB X® project directory.",
                                        Application.ProductName,
                                        MessageBoxButtons.RetryCancel,
                                        MessageBoxIcon.Exclamation,
                                        MessageBoxDefaultButton.Button1
                                        );
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(
                            "Error (0x" + ex.HResult.ToString("X") + "): Could not identify path or selected directory is not valid.\r\n" +
                            "Original error: " + ex.Message,
                            Application.ProductName,
                            MessageBoxButtons.RetryCancel,
                            MessageBoxIcon.Exclamation,
                            MessageBoxDefaultButton.Button1
                            );

                    }
                }
                else
                {
                    ans = System.Windows.Forms.DialogResult.Cancel;
                    break;
                }
            }

            // Restore output window
            ofdlg.Dispose();

            return;
                        
        }


        private void cmdDCLDProjectBrowse_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfdlg = new SaveFileDialog();
            System.Windows.Forms.DialogResult ans = new System.Windows.Forms.DialogResult();
            string str_path = "";

            // Determine target directory of folder dialog
            if ((_dcld_project == null) && (_mplabx_project.MPLABXProjectDirectory != string.Empty))
            { // DCLD project location is not available => use MPLAB X project directory
                str_path = _mplabx_project.MPLABXProjectDirectory + _dcld_default_filename; 
            }
            else if ((_dcld_project == null) && (_mplabx_project.MPLABXProjectDirectory == string.Empty))
            { // If neither MPLAB X directory nor DCLD directory is available, select user MyDocuments
                str_path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                if (str_path.Substring(str_path.Length - 1, 1) != "\\") str_path += "\\";
                str_path += _dcld_default_filename;
            } 
            else
            { // DCLD project location is available
                str_path = _dcld_project.Directory; 
            } 

            // Set target directory of folder dialog
            sfdlg.Filter = "Microchip Digital Control Loop Designer files (*.dcld)|*.dcld|All files (*.*)|*.*";
            sfdlg.FilterIndex = 1;
            sfdlg.FileName = str_path;
            sfdlg.OverwritePrompt = true;
            sfdlg.DefaultExt = "dcld";

            while ((ans == System.Windows.Forms.DialogResult.None) || (ans == System.Windows.Forms.DialogResult.Retry))
            {
                // Show Folder Browse Dialog
                if (sfdlg.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // Read selected directory path
                        str_path = sfdlg.FileName;

                        if (_dcld_project == null)
                        { // DCLD Project has not been defined yet => Just filename here. Project will be crqted by Main Window
                            _dcld_default_filename = str_path;
                        }
                        else
                        { // If project file was already defined, 

                            if (System.IO.File.Exists(str_path))
                            {
                                _dcld_project.SetFilename(str_path);

                                txtControllerNamePrefix.Text = _dcld_project.ReadKey("AssemblyGenerator", "UserPrefix1", _dcld_variable_prefix);
                                txtControllerNameLabel.Text = _dcld_project.ReadKey("AssemblyGenerator", "UserPrefix2", _dcld_controller_name_label);
                            }
                        }

                        // Dialog Result is OK
                        txtDCLDProjectDir.Text = str_path;
                        ans = System.Windows.Forms.DialogResult.OK;

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(
                            "Error (0x" + ex.HResult.ToString("X") + "): Could not identify path or selected directory is not valid.\r\n" +
                            "Original error: " + ex.Message,
                            Application.ProductName,
                            MessageBoxButtons.RetryCancel,
                            MessageBoxIcon.Exclamation,
                            MessageBoxDefaultButton.Button1
                            );

                    }
                }
                else
                {
                    ans = System.Windows.Forms.DialogResult.Cancel;
                    break;
                }
            }

            // Restore output window
            sfdlg.Dispose();

            return;
        }


        private void txtControllerNamePrefix_TextChanged(object sender, EventArgs e)
        {
            _dcld_variable_prefix = txtControllerNamePrefix.Text.Trim();
            lblFinalNamePrefixOutput.Text = _dcld_variable_prefix + _dcld_controller_name_label;
        }

        private void txtControllerNameLabel_TextChanged(object sender, EventArgs e)
        {
            _dcld_controller_name_label = txtControllerNameLabel.Text.Trim();
            lblFinalNamePrefixOutput.Text = _dcld_variable_prefix + _dcld_controller_name_label;
        }

        private void frmMPLABXProject_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Escape)
                this.Close();
            return;
        }

        private void frmMPLABXProject_FormClosing(object sender, FormClosingEventArgs e)
        {
            ShowWinAtStartup = chkShowatStartup.Checked;
        }


        private void CheckProjectPathsValid(object sender, EventArgs e)
        {
            bool config_complete = false;
            bool _mplabx_project_declared = false;
            bool _dcld_project_file_declared = false;
            StringBuilder str_dum = new StringBuilder();

            // Check if MPLAB X Project Configuration is valid
            if (_mplabx_project != null)
                if (System.IO.Directory.Exists(_mplabx_project.MPLABXProjectDirectory))
                    _mplabx_project_declared = true;

            // Check if DCLD Project Configuration is valid
            if (_dcld_project != null)
                _dcld_project_file_declared =  (System.IO.Directory.Exists(_dcld_project.Directory));
            else
                if(txtDCLDProjectDir.Text.Trim().Length > 7)
                    _dcld_project_file_declared = System.IO.Directory.Exists(System.IO.Directory.GetParent(txtDCLDProjectDir.Text).FullName);
            

            // Check if total configuration is complete
            config_complete = (bool)((_mplabx_project_declared) && (_dcld_project_file_declared));

            // Update decription label
            if (!config_complete)
            {
                str_dum.Append("Your controller configuration is incomplete: \r\n" +
                "\r\n");

                if (!_mplabx_project_declared)
                {
                    str_dum.Append(
                    "- Select the MPLAB X® project associated with this controller configuration \r\n" +
                    "  by clicking the 'Browse' button in 'MPLAB X® Project Directories' \r\n\r\n");
                }

                if (!_dcld_project_file_declared)
                {
                    str_dum.Append(
                    "- Select where this DCLD configuration should be stored by clicking the \r\n" +
                    "  'Browse' button in 'DCLD Project Configuration' \r\n\r\n");
                }

            }
            else
            {
                str_dum.Append(
                    "Your controller configuration has been successfully completed. \r\n\r\n" + 
                    "Please don't forget to specify the desired location of the generated code files \r\n" +
                    "in the 'Source Code Output' tab on the right of the main window."
                    );

            }

            lblDescription.Text = str_dum.ToString();
            cmdOK.Enabled = config_complete;

            picConfigSuccess.Left = lblTitle.Left + lblTitle.Width + 8;
            picConfigFailure.Left = picConfigSuccess.Left;

            if (config_complete)
            {
                picConfigFailure.Visible = false;
                picConfigSuccess.Visible = true;
            }
            else
            {
                picConfigFailure.Visible = true;
                picConfigSuccess.Visible = false;
            }

            return;
        }

        private void cmbIncludeDirectories_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbIncludeDirectories.Text.Trim().Length > 0)
                _mplabx_project.MPLABXIncludeDirectory = cmbIncludeDirectories.Text;
        }

        private void txtActiveTargetDevice_TextChanged(object sender, EventArgs e)
        {
            
        }

    }
}
