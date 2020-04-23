namespace dcld
{
    partial class frmMPLABXProject
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMPLABXProject));
            this.cmdCancel = new System.Windows.Forms.Button();
            this.cmdOK = new System.Windows.Forms.Button();
            this.lblProjectDir = new System.Windows.Forms.Label();
            this.txtMPLABXProjectDir = new System.Windows.Forms.TextBox();
            this.cmdMPLABXProjectBrowse = new System.Windows.Forms.Button();
            this.cmbActiveConfig = new System.Windows.Forms.ComboBox();
            this.lblActiveConfig = new System.Windows.Forms.Label();
            this.grpProjectDirectories = new System.Windows.Forms.GroupBox();
            this.chkMakefileIncludeDirectory = new System.Windows.Forms.CheckBox();
            this.cmbIncludeDirectories = new System.Windows.Forms.ComboBox();
            this.lblIncludeDirectories = new System.Windows.Forms.Label();
            this.txtASMIncludeDir = new System.Windows.Forms.TextBox();
            this.txtSpecialIncludeDir = new System.Windows.Forms.TextBox();
            this.txtCommonIncludeDir = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtMakefileLocation = new System.Windows.Forms.TextBox();
            this.txtActiveTargetDevice = new System.Windows.Forms.TextBox();
            this.lblCommonIncludeDir = new System.Windows.Forms.Label();
            this.lblSpecialIncludeDir = new System.Windows.Forms.Label();
            this.lblActiveTargetDevice = new System.Windows.Forms.Label();
            this.lblASMIncludeDir = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblDescription = new System.Windows.Forms.Label();
            this.grpDCLDConfiguration = new System.Windows.Forms.GroupBox();
            this.lblControllerNamePrefix = new System.Windows.Forms.Label();
            this.txtControllerNamePrefix = new System.Windows.Forms.TextBox();
            this.txtControllerNameLabel = new System.Windows.Forms.TextBox();
            this.lblFinalNamePrefixOutput = new System.Windows.Forms.Label();
            this.lblFinalNamePrefix = new System.Windows.Forms.Label();
            this.lblDCLDProjectDir = new System.Windows.Forms.Label();
            this.cmdDCLDProjectBrowse = new System.Windows.Forms.Button();
            this.txtDCLDProjectDir = new System.Windows.Forms.TextBox();
            this.picMPLABXLogo = new System.Windows.Forms.PictureBox();
            this.chkShowatStartup = new System.Windows.Forms.CheckBox();
            this.picConfigSuccess = new System.Windows.Forms.PictureBox();
            this.picConfigFailure = new System.Windows.Forms.PictureBox();
            this.tabProjectDirectories = new System.Windows.Forms.TabControl();
            this.tabRefDir = new System.Windows.Forms.TabPage();
            this.grpProjectDirectories.SuspendLayout();
            this.grpDCLDConfiguration.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picMPLABXLogo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picConfigSuccess)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picConfigFailure)).BeginInit();
            this.tabProjectDirectories.SuspendLayout();
            this.tabRefDir.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(569, 620);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(96, 35);
            this.cmdCancel.TabIndex = 2;
            this.cmdCancel.Text = "&Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            // 
            // cmdOK
            // 
            this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.Enabled = false;
            this.cmdOK.Location = new System.Drawing.Point(467, 620);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(96, 35);
            this.cmdOK.TabIndex = 1;
            this.cmdOK.Text = "&OK";
            this.cmdOK.UseVisualStyleBackColor = true;
            // 
            // lblProjectDir
            // 
            this.lblProjectDir.AutoSize = true;
            this.lblProjectDir.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProjectDir.Location = new System.Drawing.Point(38, 25);
            this.lblProjectDir.Name = "lblProjectDir";
            this.lblProjectDir.Size = new System.Drawing.Size(155, 15);
            this.lblProjectDir.TabIndex = 8;
            this.lblProjectDir.Text = "MPLAB X® Project Location:";
            // 
            // txtMPLABXProjectDir
            // 
            this.txtMPLABXProjectDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMPLABXProjectDir.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMPLABXProjectDir.Location = new System.Drawing.Point(199, 22);
            this.txtMPLABXProjectDir.Name = "txtMPLABXProjectDir";
            this.txtMPLABXProjectDir.Size = new System.Drawing.Size(359, 23);
            this.txtMPLABXProjectDir.TabIndex = 2;
            this.txtMPLABXProjectDir.TextChanged += new System.EventHandler(this.CheckProjectPathsValid);
            // 
            // cmdMPLABXProjectBrowse
            // 
            this.cmdMPLABXProjectBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdMPLABXProjectBrowse.Location = new System.Drawing.Point(556, 21);
            this.cmdMPLABXProjectBrowse.Name = "cmdMPLABXProjectBrowse";
            this.cmdMPLABXProjectBrowse.Size = new System.Drawing.Size(57, 25);
            this.cmdMPLABXProjectBrowse.TabIndex = 3;
            this.cmdMPLABXProjectBrowse.Text = "&Browse";
            this.cmdMPLABXProjectBrowse.UseVisualStyleBackColor = true;
            this.cmdMPLABXProjectBrowse.Click += new System.EventHandler(this.cmdMPLABXProjectBrowse_Click);
            // 
            // cmbActiveConfig
            // 
            this.cmbActiveConfig.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbActiveConfig.Enabled = false;
            this.cmbActiveConfig.FormattingEnabled = true;
            this.cmbActiveConfig.Location = new System.Drawing.Point(199, 51);
            this.cmbActiveConfig.Name = "cmbActiveConfig";
            this.cmbActiveConfig.Size = new System.Drawing.Size(359, 23);
            this.cmbActiveConfig.TabIndex = 4;
            this.cmbActiveConfig.SelectedIndexChanged += new System.EventHandler(this.LoadActiveProjectConfig);
            // 
            // lblActiveConfig
            // 
            this.lblActiveConfig.AutoSize = true;
            this.lblActiveConfig.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblActiveConfig.Location = new System.Drawing.Point(32, 54);
            this.lblActiveConfig.Name = "lblActiveConfig";
            this.lblActiveConfig.Size = new System.Drawing.Size(161, 15);
            this.lblActiveConfig.TabIndex = 12;
            this.lblActiveConfig.Text = "Active Project Configuration:";
            // 
            // grpProjectDirectories
            // 
            this.grpProjectDirectories.BackColor = System.Drawing.Color.Transparent;
            this.grpProjectDirectories.Controls.Add(this.chkMakefileIncludeDirectory);
            this.grpProjectDirectories.Controls.Add(this.cmbIncludeDirectories);
            this.grpProjectDirectories.Controls.Add(this.lblIncludeDirectories);
            this.grpProjectDirectories.Controls.Add(this.txtASMIncludeDir);
            this.grpProjectDirectories.Controls.Add(this.txtSpecialIncludeDir);
            this.grpProjectDirectories.Controls.Add(this.txtCommonIncludeDir);
            this.grpProjectDirectories.Controls.Add(this.label1);
            this.grpProjectDirectories.Controls.Add(this.txtMakefileLocation);
            this.grpProjectDirectories.Controls.Add(this.txtActiveTargetDevice);
            this.grpProjectDirectories.Controls.Add(this.lblCommonIncludeDir);
            this.grpProjectDirectories.Controls.Add(this.lblSpecialIncludeDir);
            this.grpProjectDirectories.Controls.Add(this.lblActiveTargetDevice);
            this.grpProjectDirectories.Controls.Add(this.lblASMIncludeDir);
            this.grpProjectDirectories.Controls.Add(this.txtMPLABXProjectDir);
            this.grpProjectDirectories.Controls.Add(this.lblProjectDir);
            this.grpProjectDirectories.Controls.Add(this.cmdMPLABXProjectBrowse);
            this.grpProjectDirectories.Controls.Add(this.cmbActiveConfig);
            this.grpProjectDirectories.Controls.Add(this.lblActiveConfig);
            this.grpProjectDirectories.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpProjectDirectories.Location = new System.Drawing.Point(3, 3);
            this.grpProjectDirectories.Name = "grpProjectDirectories";
            this.grpProjectDirectories.Size = new System.Drawing.Size(646, 299);
            this.grpProjectDirectories.TabIndex = 1;
            this.grpProjectDirectories.TabStop = false;
            this.grpProjectDirectories.Text = "MPLAB X® Project Directories";
            // 
            // chkMakefileIncludeDirectory
            // 
            this.chkMakefileIncludeDirectory.AutoSize = true;
            this.chkMakefileIncludeDirectory.Enabled = false;
            this.chkMakefileIncludeDirectory.Location = new System.Drawing.Point(252, 109);
            this.chkMakefileIncludeDirectory.Name = "chkMakefileIncludeDirectory";
            this.chkMakefileIncludeDirectory.Size = new System.Drawing.Size(269, 19);
            this.chkMakefileIncludeDirectory.TabIndex = 6;
            this.chkMakefileIncludeDirectory.Text = "&Reference include paths to Makefile location";
            this.chkMakefileIncludeDirectory.UseVisualStyleBackColor = true;
            // 
            // cmbIncludeDirectories
            // 
            this.cmbIncludeDirectories.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbIncludeDirectories.FormattingEnabled = true;
            this.cmbIncludeDirectories.Location = new System.Drawing.Point(199, 80);
            this.cmbIncludeDirectories.Name = "cmbIncludeDirectories";
            this.cmbIncludeDirectories.Size = new System.Drawing.Size(358, 23);
            this.cmbIncludeDirectories.TabIndex = 5;
            this.cmbIncludeDirectories.SelectedIndexChanged += new System.EventHandler(this.cmbIncludeDirectories_SelectedIndexChanged);
            // 
            // lblIncludeDirectories
            // 
            this.lblIncludeDirectories.AutoSize = true;
            this.lblIncludeDirectories.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIncludeDirectories.Location = new System.Drawing.Point(45, 83);
            this.lblIncludeDirectories.Name = "lblIncludeDirectories";
            this.lblIncludeDirectories.Size = new System.Drawing.Size(148, 15);
            this.lblIncludeDirectories.TabIndex = 24;
            this.lblIncludeDirectories.Text = "Default Include Directory:";
            // 
            // txtASMIncludeDir
            // 
            this.txtASMIncludeDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtASMIncludeDir.Enabled = false;
            this.txtASMIncludeDir.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtASMIncludeDir.Location = new System.Drawing.Point(372, 260);
            this.txtASMIncludeDir.Name = "txtASMIncludeDir";
            this.txtASMIncludeDir.Size = new System.Drawing.Size(186, 23);
            this.txtASMIncludeDir.TabIndex = 11;
            // 
            // txtSpecialIncludeDir
            // 
            this.txtSpecialIncludeDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSpecialIncludeDir.Enabled = false;
            this.txtSpecialIncludeDir.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSpecialIncludeDir.Location = new System.Drawing.Point(372, 231);
            this.txtSpecialIncludeDir.Name = "txtSpecialIncludeDir";
            this.txtSpecialIncludeDir.Size = new System.Drawing.Size(186, 23);
            this.txtSpecialIncludeDir.TabIndex = 10;
            // 
            // txtCommonIncludeDir
            // 
            this.txtCommonIncludeDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCommonIncludeDir.Enabled = false;
            this.txtCommonIncludeDir.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCommonIncludeDir.Location = new System.Drawing.Point(372, 202);
            this.txtCommonIncludeDir.Name = "txtCommonIncludeDir";
            this.txtCommonIncludeDir.Size = new System.Drawing.Size(186, 23);
            this.txtCommonIncludeDir.TabIndex = 9;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(87, 147);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 15);
            this.label1.TabIndex = 19;
            this.label1.Text = "Makefile Location:";
            // 
            // txtMakefileLocation
            // 
            this.txtMakefileLocation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMakefileLocation.Enabled = false;
            this.txtMakefileLocation.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMakefileLocation.Location = new System.Drawing.Point(199, 144);
            this.txtMakefileLocation.Name = "txtMakefileLocation";
            this.txtMakefileLocation.Size = new System.Drawing.Size(359, 23);
            this.txtMakefileLocation.TabIndex = 7;
            // 
            // txtActiveTargetDevice
            // 
            this.txtActiveTargetDevice.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtActiveTargetDevice.Enabled = false;
            this.txtActiveTargetDevice.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtActiveTargetDevice.Location = new System.Drawing.Point(372, 173);
            this.txtActiveTargetDevice.Name = "txtActiveTargetDevice";
            this.txtActiveTargetDevice.Size = new System.Drawing.Size(186, 23);
            this.txtActiveTargetDevice.TabIndex = 8;
            this.txtActiveTargetDevice.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtActiveTargetDevice.TextChanged += new System.EventHandler(this.txtActiveTargetDevice_TextChanged);
            // 
            // lblCommonIncludeDir
            // 
            this.lblCommonIncludeDir.AutoSize = true;
            this.lblCommonIncludeDir.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCommonIncludeDir.Location = new System.Drawing.Point(238, 205);
            this.lblCommonIncludeDir.Name = "lblCommonIncludeDir";
            this.lblCommonIncludeDir.Size = new System.Drawing.Size(130, 15);
            this.lblCommonIncludeDir.TabIndex = 14;
            this.lblCommonIncludeDir.Text = "Common Include Path:";
            // 
            // lblSpecialIncludeDir
            // 
            this.lblSpecialIncludeDir.AutoSize = true;
            this.lblSpecialIncludeDir.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSpecialIncludeDir.Location = new System.Drawing.Point(246, 234);
            this.lblSpecialIncludeDir.Name = "lblSpecialIncludeDir";
            this.lblSpecialIncludeDir.Size = new System.Drawing.Size(122, 15);
            this.lblSpecialIncludeDir.TabIndex = 15;
            this.lblSpecialIncludeDir.Text = "Special Include Path:";
            // 
            // lblActiveTargetDevice
            // 
            this.lblActiveTargetDevice.AutoSize = true;
            this.lblActiveTargetDevice.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblActiveTargetDevice.Location = new System.Drawing.Point(249, 176);
            this.lblActiveTargetDevice.Name = "lblActiveTargetDevice";
            this.lblActiveTargetDevice.Size = new System.Drawing.Size(117, 15);
            this.lblActiveTargetDevice.TabIndex = 13;
            this.lblActiveTargetDevice.Text = "Active Target Device:";
            // 
            // lblASMIncludeDir
            // 
            this.lblASMIncludeDir.AutoSize = true;
            this.lblASMIncludeDir.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblASMIncludeDir.Location = new System.Drawing.Point(229, 263);
            this.lblASMIncludeDir.Name = "lblASMIncludeDir";
            this.lblASMIncludeDir.Size = new System.Drawing.Size(139, 15);
            this.lblASMIncludeDir.TabIndex = 18;
            this.lblASMIncludeDir.Text = "Assembler Include Path:";
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Calibri", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(160, 12);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(381, 33);
            this.lblTitle.TabIndex = 21;
            this.lblTitle.Text = "(no MPLAB X® project associated)";
            // 
            // lblDescription
            // 
            this.lblDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDescription.BackColor = System.Drawing.Color.Transparent;
            this.lblDescription.Location = new System.Drawing.Point(163, 56);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(502, 108);
            this.lblDescription.TabIndex = 22;
            this.lblDescription.Text = "To set up the configuration of this controller, please follow these steps:";
            // 
            // grpDCLDConfiguration
            // 
            this.grpDCLDConfiguration.BackColor = System.Drawing.Color.Transparent;
            this.grpDCLDConfiguration.Controls.Add(this.lblControllerNamePrefix);
            this.grpDCLDConfiguration.Controls.Add(this.txtControllerNamePrefix);
            this.grpDCLDConfiguration.Controls.Add(this.txtControllerNameLabel);
            this.grpDCLDConfiguration.Controls.Add(this.lblFinalNamePrefixOutput);
            this.grpDCLDConfiguration.Controls.Add(this.lblFinalNamePrefix);
            this.grpDCLDConfiguration.Controls.Add(this.lblDCLDProjectDir);
            this.grpDCLDConfiguration.Controls.Add(this.cmdDCLDProjectBrowse);
            this.grpDCLDConfiguration.Controls.Add(this.txtDCLDProjectDir);
            this.grpDCLDConfiguration.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpDCLDConfiguration.Location = new System.Drawing.Point(3, 302);
            this.grpDCLDConfiguration.Name = "grpDCLDConfiguration";
            this.grpDCLDConfiguration.Size = new System.Drawing.Size(646, 113);
            this.grpDCLDConfiguration.TabIndex = 12;
            this.grpDCLDConfiguration.TabStop = false;
            this.grpDCLDConfiguration.Text = "DCLD Project Configuration";
            // 
            // lblControllerNamePrefix
            // 
            this.lblControllerNamePrefix.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblControllerNamePrefix.AutoSize = true;
            this.lblControllerNamePrefix.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblControllerNamePrefix.Location = new System.Drawing.Point(275, 55);
            this.lblControllerNamePrefix.Name = "lblControllerNamePrefix";
            this.lblControllerNamePrefix.Size = new System.Drawing.Size(75, 15);
            this.lblControllerNamePrefix.TabIndex = 35;
            this.lblControllerNamePrefix.Text = "Name Prefix:";
            // 
            // txtControllerNamePrefix
            // 
            this.txtControllerNamePrefix.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtControllerNamePrefix.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtControllerNamePrefix.Location = new System.Drawing.Point(356, 52);
            this.txtControllerNamePrefix.MaxLength = 255;
            this.txtControllerNamePrefix.Name = "txtControllerNamePrefix";
            this.txtControllerNamePrefix.Size = new System.Drawing.Size(97, 23);
            this.txtControllerNamePrefix.TabIndex = 15;
            this.txtControllerNamePrefix.TextChanged += new System.EventHandler(this.txtControllerNamePrefix_TextChanged);
            // 
            // txtControllerNameLabel
            // 
            this.txtControllerNameLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtControllerNameLabel.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtControllerNameLabel.Location = new System.Drawing.Point(461, 52);
            this.txtControllerNameLabel.MaxLength = 255;
            this.txtControllerNameLabel.Name = "txtControllerNameLabel";
            this.txtControllerNameLabel.Size = new System.Drawing.Size(97, 23);
            this.txtControllerNameLabel.TabIndex = 16;
            this.txtControllerNameLabel.TextChanged += new System.EventHandler(this.txtControllerNameLabel_TextChanged);
            // 
            // lblFinalNamePrefixOutput
            // 
            this.lblFinalNamePrefixOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblFinalNamePrefixOutput.AutoSize = true;
            this.lblFinalNamePrefixOutput.Enabled = false;
            this.lblFinalNamePrefixOutput.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblFinalNamePrefixOutput.Location = new System.Drawing.Point(458, 80);
            this.lblFinalNamePrefixOutput.Name = "lblFinalNamePrefixOutput";
            this.lblFinalNamePrefixOutput.Size = new System.Drawing.Size(80, 15);
            this.lblFinalNamePrefixOutput.TabIndex = 37;
            this.lblFinalNamePrefixOutput.Text = "{Name Prefix}";
            // 
            // lblFinalNamePrefix
            // 
            this.lblFinalNamePrefix.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblFinalNamePrefix.AutoSize = true;
            this.lblFinalNamePrefix.Enabled = false;
            this.lblFinalNamePrefix.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblFinalNamePrefix.Location = new System.Drawing.Point(365, 80);
            this.lblFinalNamePrefix.Name = "lblFinalNamePrefix";
            this.lblFinalNamePrefix.Size = new System.Drawing.Size(87, 15);
            this.lblFinalNamePrefix.TabIndex = 36;
            this.lblFinalNamePrefix.Text = "Name Preview:";
            // 
            // lblDCLDProjectDir
            // 
            this.lblDCLDProjectDir.AutoSize = true;
            this.lblDCLDProjectDir.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDCLDProjectDir.Location = new System.Drawing.Point(29, 26);
            this.lblDCLDProjectDir.Name = "lblDCLDProjectDir";
            this.lblDCLDProjectDir.Size = new System.Drawing.Size(164, 15);
            this.lblDCLDProjectDir.TabIndex = 20;
            this.lblDCLDProjectDir.Text = "DCLD Configuration Location:";
            // 
            // cmdDCLDProjectBrowse
            // 
            this.cmdDCLDProjectBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdDCLDProjectBrowse.Location = new System.Drawing.Point(556, 22);
            this.cmdDCLDProjectBrowse.Name = "cmdDCLDProjectBrowse";
            this.cmdDCLDProjectBrowse.Size = new System.Drawing.Size(57, 25);
            this.cmdDCLDProjectBrowse.TabIndex = 14;
            this.cmdDCLDProjectBrowse.Text = "&Browse";
            this.cmdDCLDProjectBrowse.UseVisualStyleBackColor = true;
            this.cmdDCLDProjectBrowse.Click += new System.EventHandler(this.cmdDCLDProjectBrowse_Click);
            // 
            // txtDCLDProjectDir
            // 
            this.txtDCLDProjectDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDCLDProjectDir.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDCLDProjectDir.Location = new System.Drawing.Point(199, 23);
            this.txtDCLDProjectDir.Name = "txtDCLDProjectDir";
            this.txtDCLDProjectDir.Size = new System.Drawing.Size(359, 23);
            this.txtDCLDProjectDir.TabIndex = 13;
            this.txtDCLDProjectDir.TextChanged += new System.EventHandler(this.CheckProjectPathsValid);
            // 
            // picMPLABXLogo
            // 
            this.picMPLABXLogo.Image = ((System.Drawing.Image)(resources.GetObject("picMPLABXLogo.Image")));
            this.picMPLABXLogo.Location = new System.Drawing.Point(18, 12);
            this.picMPLABXLogo.Name = "picMPLABXLogo";
            this.picMPLABXLogo.Size = new System.Drawing.Size(121, 123);
            this.picMPLABXLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picMPLABXLogo.TabIndex = 23;
            this.picMPLABXLogo.TabStop = false;
            // 
            // chkShowatStartup
            // 
            this.chkShowatStartup.AccessibleName = "ShowWinAtStartup";
            this.chkShowatStartup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkShowatStartup.AutoSize = true;
            this.chkShowatStartup.Checked = true;
            this.chkShowatStartup.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkShowatStartup.Location = new System.Drawing.Point(24, 629);
            this.chkShowatStartup.Name = "chkShowatStartup";
            this.chkShowatStartup.Size = new System.Drawing.Size(361, 19);
            this.chkShowatStartup.TabIndex = 3;
            this.chkShowatStartup.Text = "Show this window at startup when no configuration is loaded";
            this.chkShowatStartup.UseVisualStyleBackColor = true;
            // 
            // picConfigSuccess
            // 
            this.picConfigSuccess.Image = ((System.Drawing.Image)(resources.GetObject("picConfigSuccess.Image")));
            this.picConfigSuccess.Location = new System.Drawing.Point(547, 8);
            this.picConfigSuccess.Name = "picConfigSuccess";
            this.picConfigSuccess.Size = new System.Drawing.Size(43, 41);
            this.picConfigSuccess.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picConfigSuccess.TabIndex = 25;
            this.picConfigSuccess.TabStop = false;
            this.picConfigSuccess.Visible = false;
            // 
            // picConfigFailure
            // 
            this.picConfigFailure.Image = ((System.Drawing.Image)(resources.GetObject("picConfigFailure.Image")));
            this.picConfigFailure.Location = new System.Drawing.Point(596, 8);
            this.picConfigFailure.Name = "picConfigFailure";
            this.picConfigFailure.Size = new System.Drawing.Size(43, 41);
            this.picConfigFailure.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picConfigFailure.TabIndex = 26;
            this.picConfigFailure.TabStop = false;
            this.picConfigFailure.Visible = false;
            // 
            // tabProjectDirectories
            // 
            this.tabProjectDirectories.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabProjectDirectories.Controls.Add(this.tabRefDir);
            this.tabProjectDirectories.Location = new System.Drawing.Point(12, 164);
            this.tabProjectDirectories.Name = "tabProjectDirectories";
            this.tabProjectDirectories.SelectedIndex = 0;
            this.tabProjectDirectories.Size = new System.Drawing.Size(660, 450);
            this.tabProjectDirectories.TabIndex = 0;
            // 
            // tabRefDir
            // 
            this.tabRefDir.Controls.Add(this.grpDCLDConfiguration);
            this.tabRefDir.Controls.Add(this.grpProjectDirectories);
            this.tabRefDir.Location = new System.Drawing.Point(4, 24);
            this.tabRefDir.Name = "tabRefDir";
            this.tabRefDir.Padding = new System.Windows.Forms.Padding(3);
            this.tabRefDir.Size = new System.Drawing.Size(652, 422);
            this.tabRefDir.TabIndex = 0;
            this.tabRefDir.Text = "Directory References";
            this.tabRefDir.UseVisualStyleBackColor = true;
            // 
            // frmMPLABXProject
            // 
            this.AcceptButton = this.cmdOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.cmdCancel;
            this.ClientSize = new System.Drawing.Size(684, 665);
            this.Controls.Add(this.picConfigFailure);
            this.Controls.Add(this.picConfigSuccess);
            this.Controls.Add(this.chkShowatStartup);
            this.Controls.Add(this.picMPLABXLogo);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdOK);
            this.Controls.Add(this.tabProjectDirectories);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(700, 704);
            this.Name = "frmMPLABXProject";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "MPLAB X Project Configuration";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMPLABXProject_FormClosing);
            this.Load += new System.EventHandler(this.frmMPLABXProject_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmMPLABXProject_KeyDown);
            this.grpProjectDirectories.ResumeLayout(false);
            this.grpProjectDirectories.PerformLayout();
            this.grpDCLDConfiguration.ResumeLayout(false);
            this.grpDCLDConfiguration.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picMPLABXLogo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picConfigSuccess)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picConfigFailure)).EndInit();
            this.tabProjectDirectories.ResumeLayout(false);
            this.tabRefDir.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.Label lblProjectDir;
        private System.Windows.Forms.TextBox txtMPLABXProjectDir;
        private System.Windows.Forms.Button cmdMPLABXProjectBrowse;
        private System.Windows.Forms.ComboBox cmbActiveConfig;
        private System.Windows.Forms.Label lblActiveConfig;
        private System.Windows.Forms.GroupBox grpProjectDirectories;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.TextBox txtActiveTargetDevice;
        private System.Windows.Forms.Label lblActiveTargetDevice;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtMakefileLocation;
        private System.Windows.Forms.GroupBox grpDCLDConfiguration;
        private System.Windows.Forms.Label lblDCLDProjectDir;
        private System.Windows.Forms.Button cmdDCLDProjectBrowse;
        private System.Windows.Forms.TextBox txtDCLDProjectDir;
        private System.Windows.Forms.Label lblControllerNamePrefix;
        private System.Windows.Forms.TextBox txtControllerNamePrefix;
        private System.Windows.Forms.TextBox txtControllerNameLabel;
        private System.Windows.Forms.Label lblFinalNamePrefixOutput;
        private System.Windows.Forms.Label lblFinalNamePrefix;
        private System.Windows.Forms.TextBox txtASMIncludeDir;
        private System.Windows.Forms.TextBox txtSpecialIncludeDir;
        private System.Windows.Forms.TextBox txtCommonIncludeDir;
        private System.Windows.Forms.Label lblCommonIncludeDir;
        private System.Windows.Forms.Label lblSpecialIncludeDir;
        private System.Windows.Forms.Label lblASMIncludeDir;
        private System.Windows.Forms.PictureBox picMPLABXLogo;
        private System.Windows.Forms.CheckBox chkShowatStartup;
        private System.Windows.Forms.PictureBox picConfigSuccess;
        private System.Windows.Forms.PictureBox picConfigFailure;
        private System.Windows.Forms.TabControl tabProjectDirectories;
        private System.Windows.Forms.TabPage tabRefDir;
        private System.Windows.Forms.ComboBox cmbIncludeDirectories;
        private System.Windows.Forms.Label lblIncludeDirectories;
        private System.Windows.Forms.CheckBox chkMakefileIncludeDirectory;
    }
}