namespace dcld
{
    partial class frmCalculateOutputGain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCalculateOutputGain));
            this.cmdOK = new System.Windows.Forms.Button();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.tabPWMMode = new System.Windows.Forms.TabControl();
            this.tabFixedFrequency = new System.Windows.Forms.TabPage();
            this.picFFreq = new System.Windows.Forms.PictureBox();
            this.tabVariableFrequency = new System.Windows.Forms.TabPage();
            this.picVPhase = new System.Windows.Forms.PictureBox();
            this.tabPhaseShifted = new System.Windows.Forms.TabPage();
            this.picPSFT = new System.Windows.Forms.PictureBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblDeviceType = new System.Windows.Forms.Label();
            this.cmbDeviceType = new System.Windows.Forms.ComboBox();
            this.txtSourceClock = new System.Windows.Forms.TextBox();
            this.lblSourceClockUnit = new System.Windows.Forms.Label();
            this.txtClockDivider = new System.Windows.Forms.TextBox();
            this.lblResolutionUnit = new System.Windows.Forms.Label();
            this.lblSourceClock = new System.Windows.Forms.Label();
            this.txtResolution = new System.Windows.Forms.TextBox();
            this.lblMaximum = new System.Windows.Forms.Label();
            this.lblClockDivider = new System.Windows.Forms.Label();
            this.lblResolution = new System.Windows.Forms.Label();
            this.txtMaximum = new System.Windows.Forms.TextBox();
            this.grpVoltageDividerCalculation = new System.Windows.Forms.GroupBox();
            this.cmdGetPTermNominalOutput = new System.Windows.Forms.Button();
            this.lblNominalOutputValueUnit = new System.Windows.Forms.Label();
            this.lblNominalOutputValue = new System.Windows.Forms.Label();
            this.txtNominalOutputValue = new System.Windows.Forms.TextBox();
            this.txtParam4 = new System.Windows.Forms.TextBox();
            this.lblUnit4 = new System.Windows.Forms.Label();
            this.lblParam4 = new System.Windows.Forms.Label();
            this.txtParam3 = new System.Windows.Forms.TextBox();
            this.lblUnit3 = new System.Windows.Forms.Label();
            this.lblParam3 = new System.Windows.Forms.Label();
            this.txtParam2 = new System.Windows.Forms.TextBox();
            this.lblUnit2 = new System.Windows.Forms.Label();
            this.lblParam2 = new System.Windows.Forms.Label();
            this.txtParam1 = new System.Windows.Forms.TextBox();
            this.lblUnit1 = new System.Windows.Forms.Label();
            this.lblParam1 = new System.Windows.Forms.Label();
            this.grmOutputGain = new System.Windows.Forms.GroupBox();
            this.lblOutputGain = new System.Windows.Forms.Label();
            this.lblInputGainUnit = new System.Windows.Forms.Label();
            this.tabPWMMode.SuspendLayout();
            this.tabFixedFrequency.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picFFreq)).BeginInit();
            this.tabVariableFrequency.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picVPhase)).BeginInit();
            this.tabPhaseShifted.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picPSFT)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.grpVoltageDividerCalculation.SuspendLayout();
            this.grmOutputGain.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmdOK
            // 
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.Location = new System.Drawing.Point(363, 514);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(82, 30);
            this.cmdOK.TabIndex = 3;
            this.cmdOK.Text = "&OK";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // cmdCancel
            // 
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(457, 514);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(82, 30);
            this.cmdCancel.TabIndex = 4;
            this.cmdCancel.Text = "&Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // tabPWMMode
            // 
            this.tabPWMMode.Controls.Add(this.tabFixedFrequency);
            this.tabPWMMode.Controls.Add(this.tabVariableFrequency);
            this.tabPWMMode.Controls.Add(this.tabPhaseShifted);
            this.tabPWMMode.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabPWMMode.Location = new System.Drawing.Point(0, 0);
            this.tabPWMMode.Name = "tabPWMMode";
            this.tabPWMMode.SelectedIndex = 0;
            this.tabPWMMode.Size = new System.Drawing.Size(551, 281);
            this.tabPWMMode.TabIndex = 0;
            this.tabPWMMode.SelectedIndexChanged += new System.EventHandler(this.tabPWMMode_SelectedIndexChanged);
            // 
            // tabFixedFrequency
            // 
            this.tabFixedFrequency.Controls.Add(this.picFFreq);
            this.tabFixedFrequency.Location = new System.Drawing.Point(4, 24);
            this.tabFixedFrequency.Name = "tabFixedFrequency";
            this.tabFixedFrequency.Padding = new System.Windows.Forms.Padding(3);
            this.tabFixedFrequency.Size = new System.Drawing.Size(543, 253);
            this.tabFixedFrequency.TabIndex = 0;
            this.tabFixedFrequency.Text = "Fixed Frequency";
            this.tabFixedFrequency.UseVisualStyleBackColor = true;
            // 
            // picFFreq
            // 
            this.picFFreq.BackgroundImage = global::dcld.Properties.Resources.FFreq;
            this.picFFreq.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.picFFreq.Location = new System.Drawing.Point(27, 0);
            this.picFFreq.Name = "picFFreq";
            this.picFFreq.Size = new System.Drawing.Size(489, 247);
            this.picFFreq.TabIndex = 0;
            this.picFFreq.TabStop = false;
            // 
            // tabVariableFrequency
            // 
            this.tabVariableFrequency.Controls.Add(this.picVPhase);
            this.tabVariableFrequency.Location = new System.Drawing.Point(4, 24);
            this.tabVariableFrequency.Name = "tabVariableFrequency";
            this.tabVariableFrequency.Size = new System.Drawing.Size(543, 253);
            this.tabVariableFrequency.TabIndex = 2;
            this.tabVariableFrequency.Text = "Variable Frequency";
            this.tabVariableFrequency.UseVisualStyleBackColor = true;
            // 
            // picVPhase
            // 
            this.picVPhase.BackgroundImage = global::dcld.Properties.Resources.VFreq;
            this.picVPhase.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.picVPhase.Location = new System.Drawing.Point(27, 3);
            this.picVPhase.Name = "picVPhase";
            this.picVPhase.Size = new System.Drawing.Size(489, 247);
            this.picVPhase.TabIndex = 2;
            this.picVPhase.TabStop = false;
            // 
            // tabPhaseShifted
            // 
            this.tabPhaseShifted.Controls.Add(this.picPSFT);
            this.tabPhaseShifted.Location = new System.Drawing.Point(4, 24);
            this.tabPhaseShifted.Name = "tabPhaseShifted";
            this.tabPhaseShifted.Padding = new System.Windows.Forms.Padding(3);
            this.tabPhaseShifted.Size = new System.Drawing.Size(543, 253);
            this.tabPhaseShifted.TabIndex = 1;
            this.tabPhaseShifted.Text = "Phase Shifted PWM";
            this.tabPhaseShifted.UseVisualStyleBackColor = true;
            // 
            // picPSFT
            // 
            this.picPSFT.BackgroundImage = global::dcld.Properties.Resources.PSFT;
            this.picPSFT.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.picPSFT.Location = new System.Drawing.Point(27, 3);
            this.picPSFT.Name = "picPSFT";
            this.picPSFT.Size = new System.Drawing.Size(489, 247);
            this.picPSFT.TabIndex = 1;
            this.picPSFT.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblDeviceType);
            this.groupBox1.Controls.Add(this.cmbDeviceType);
            this.groupBox1.Controls.Add(this.txtSourceClock);
            this.groupBox1.Controls.Add(this.lblSourceClockUnit);
            this.groupBox1.Controls.Add(this.txtClockDivider);
            this.groupBox1.Controls.Add(this.lblResolutionUnit);
            this.groupBox1.Controls.Add(this.lblSourceClock);
            this.groupBox1.Controls.Add(this.txtResolution);
            this.groupBox1.Controls.Add(this.lblMaximum);
            this.groupBox1.Controls.Add(this.lblClockDivider);
            this.groupBox1.Controls.Add(this.lblResolution);
            this.groupBox1.Controls.Add(this.txtMaximum);
            this.groupBox1.Location = new System.Drawing.Point(4, 283);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(225, 225);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "PWM Time Base";
            // 
            // lblDeviceType
            // 
            this.lblDeviceType.AutoSize = true;
            this.lblDeviceType.BackColor = System.Drawing.Color.Transparent;
            this.lblDeviceType.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDeviceType.Location = new System.Drawing.Point(39, 24);
            this.lblDeviceType.Name = "lblDeviceType";
            this.lblDeviceType.Size = new System.Drawing.Size(73, 15);
            this.lblDeviceType.TabIndex = 10;
            this.lblDeviceType.Text = "Device Type:";
            this.lblDeviceType.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // cmbDeviceType
            // 
            this.cmbDeviceType.Enabled = false;
            this.cmbDeviceType.FormattingEnabled = true;
            this.cmbDeviceType.Items.AddRange(new object[] {
            "dsPIC33FJ",
            "dsPIC33EP",
            "dsPIC33C"});
            this.cmbDeviceType.Location = new System.Drawing.Point(118, 20);
            this.cmbDeviceType.Name = "cmbDeviceType";
            this.cmbDeviceType.Size = new System.Drawing.Size(95, 23);
            this.cmbDeviceType.TabIndex = 5;
            // 
            // txtSourceClock
            // 
            this.txtSourceClock.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSourceClock.Location = new System.Drawing.Point(118, 51);
            this.txtSourceClock.Name = "txtSourceClock";
            this.txtSourceClock.Size = new System.Drawing.Size(64, 22);
            this.txtSourceClock.TabIndex = 1;
            this.txtSourceClock.Text = "4.0G";
            this.txtSourceClock.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtSourceClock.WordWrap = false;
            this.txtSourceClock.TextChanged += new System.EventHandler(this.CalculateOutputGain);
            this.txtSourceClock.Enter += new System.EventHandler(this.NumberTextBox_Enter);
            this.txtSourceClock.KeyDown += new System.Windows.Forms.KeyEventHandler(this.NumberTextBox_KeyDownWithScaling);
            // 
            // lblSourceClockUnit
            // 
            this.lblSourceClockUnit.AutoSize = true;
            this.lblSourceClockUnit.BackColor = System.Drawing.Color.Transparent;
            this.lblSourceClockUnit.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSourceClockUnit.Location = new System.Drawing.Point(188, 54);
            this.lblSourceClockUnit.Name = "lblSourceClockUnit";
            this.lblSourceClockUnit.Size = new System.Drawing.Size(20, 15);
            this.lblSourceClockUnit.TabIndex = 2;
            this.lblSourceClockUnit.Text = "Hz";
            // 
            // txtClockDivider
            // 
            this.txtClockDivider.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtClockDivider.Location = new System.Drawing.Point(118, 80);
            this.txtClockDivider.Name = "txtClockDivider";
            this.txtClockDivider.Size = new System.Drawing.Size(64, 22);
            this.txtClockDivider.TabIndex = 4;
            this.txtClockDivider.Text = "1";
            this.txtClockDivider.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtClockDivider.WordWrap = false;
            this.txtClockDivider.TextChanged += new System.EventHandler(this.CalculateOutputGain);
            this.txtClockDivider.Enter += new System.EventHandler(this.NumberTextBox_Enter);
            this.txtClockDivider.KeyDown += new System.Windows.Forms.KeyEventHandler(this.NumberTextBox_KeyDownWithScaling);
            // 
            // lblResolutionUnit
            // 
            this.lblResolutionUnit.AutoSize = true;
            this.lblResolutionUnit.BackColor = System.Drawing.Color.Transparent;
            this.lblResolutionUnit.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblResolutionUnit.Location = new System.Drawing.Point(188, 112);
            this.lblResolutionUnit.Name = "lblResolutionUnit";
            this.lblResolutionUnit.Size = new System.Drawing.Size(25, 15);
            this.lblResolutionUnit.TabIndex = 7;
            this.lblResolutionUnit.Text = "sec";
            // 
            // lblSourceClock
            // 
            this.lblSourceClock.AutoSize = true;
            this.lblSourceClock.BackColor = System.Drawing.Color.Transparent;
            this.lblSourceClock.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSourceClock.Location = new System.Drawing.Point(13, 53);
            this.lblSourceClock.Name = "lblSourceClock";
            this.lblSourceClock.Size = new System.Drawing.Size(99, 15);
            this.lblSourceClock.TabIndex = 0;
            this.lblSourceClock.Text = "Clock Frequency:";
            this.lblSourceClock.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtResolution
            // 
            this.txtResolution.Enabled = false;
            this.txtResolution.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtResolution.Location = new System.Drawing.Point(118, 109);
            this.txtResolution.Name = "txtResolution";
            this.txtResolution.Size = new System.Drawing.Size(64, 22);
            this.txtResolution.TabIndex = 6;
            this.txtResolution.Text = "250p";
            this.txtResolution.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtResolution.WordWrap = false;
            this.txtResolution.Enter += new System.EventHandler(this.NumberTextBox_Enter);
            this.txtResolution.KeyDown += new System.Windows.Forms.KeyEventHandler(this.NumberTextBox_KeyDownWithScaling);
            // 
            // lblMaximum
            // 
            this.lblMaximum.AutoSize = true;
            this.lblMaximum.BackColor = System.Drawing.Color.Transparent;
            this.lblMaximum.Enabled = false;
            this.lblMaximum.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMaximum.Location = new System.Drawing.Point(47, 142);
            this.lblMaximum.Name = "lblMaximum";
            this.lblMaximum.Size = new System.Drawing.Size(65, 15);
            this.lblMaximum.TabIndex = 8;
            this.lblMaximum.Text = "Maximum:";
            this.lblMaximum.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblClockDivider
            // 
            this.lblClockDivider.AutoSize = true;
            this.lblClockDivider.BackColor = System.Drawing.Color.Transparent;
            this.lblClockDivider.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblClockDivider.Location = new System.Drawing.Point(62, 83);
            this.lblClockDivider.Name = "lblClockDivider";
            this.lblClockDivider.Size = new System.Drawing.Size(50, 15);
            this.lblClockDivider.TabIndex = 3;
            this.lblClockDivider.Text = "Divider:";
            this.lblClockDivider.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblResolution
            // 
            this.lblResolution.AutoSize = true;
            this.lblResolution.BackColor = System.Drawing.Color.Transparent;
            this.lblResolution.Enabled = false;
            this.lblResolution.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblResolution.Location = new System.Drawing.Point(44, 112);
            this.lblResolution.Name = "lblResolution";
            this.lblResolution.Size = new System.Drawing.Size(68, 15);
            this.lblResolution.TabIndex = 5;
            this.lblResolution.Text = "Resolution:";
            this.lblResolution.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtMaximum
            // 
            this.txtMaximum.Enabled = false;
            this.txtMaximum.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMaximum.Location = new System.Drawing.Point(118, 139);
            this.txtMaximum.Name = "txtMaximum";
            this.txtMaximum.Size = new System.Drawing.Size(64, 22);
            this.txtMaximum.TabIndex = 9;
            this.txtMaximum.Text = "65535";
            this.txtMaximum.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtMaximum.WordWrap = false;
            this.txtMaximum.Enter += new System.EventHandler(this.NumberTextBox_Enter);
            this.txtMaximum.KeyDown += new System.Windows.Forms.KeyEventHandler(this.NumberTextBox_KeyDownWithScaling);
            // 
            // grpVoltageDividerCalculation
            // 
            this.grpVoltageDividerCalculation.Controls.Add(this.cmdGetPTermNominalOutput);
            this.grpVoltageDividerCalculation.Controls.Add(this.lblNominalOutputValueUnit);
            this.grpVoltageDividerCalculation.Controls.Add(this.lblNominalOutputValue);
            this.grpVoltageDividerCalculation.Controls.Add(this.txtNominalOutputValue);
            this.grpVoltageDividerCalculation.Controls.Add(this.txtParam4);
            this.grpVoltageDividerCalculation.Controls.Add(this.lblUnit4);
            this.grpVoltageDividerCalculation.Controls.Add(this.lblParam4);
            this.grpVoltageDividerCalculation.Controls.Add(this.txtParam3);
            this.grpVoltageDividerCalculation.Controls.Add(this.lblUnit3);
            this.grpVoltageDividerCalculation.Controls.Add(this.lblParam3);
            this.grpVoltageDividerCalculation.Controls.Add(this.txtParam2);
            this.grpVoltageDividerCalculation.Controls.Add(this.lblUnit2);
            this.grpVoltageDividerCalculation.Controls.Add(this.lblParam2);
            this.grpVoltageDividerCalculation.Controls.Add(this.txtParam1);
            this.grpVoltageDividerCalculation.Controls.Add(this.lblUnit1);
            this.grpVoltageDividerCalculation.Controls.Add(this.lblParam1);
            this.grpVoltageDividerCalculation.Controls.Add(this.grmOutputGain);
            this.grpVoltageDividerCalculation.Location = new System.Drawing.Point(235, 283);
            this.grpVoltageDividerCalculation.Name = "grpVoltageDividerCalculation";
            this.grpVoltageDividerCalculation.Size = new System.Drawing.Size(312, 225);
            this.grpVoltageDividerCalculation.TabIndex = 2;
            this.grpVoltageDividerCalculation.TabStop = false;
            this.grpVoltageDividerCalculation.Text = "Calculation";
            // 
            // cmdGetPTermNominalOutput
            // 
            this.cmdGetPTermNominalOutput.Enabled = false;
            this.cmdGetPTermNominalOutput.Image = global::dcld.Properties.Resources.calculate;
            this.cmdGetPTermNominalOutput.Location = new System.Drawing.Point(265, 140);
            this.cmdGetPTermNominalOutput.Name = "cmdGetPTermNominalOutput";
            this.cmdGetPTermNominalOutput.Size = new System.Drawing.Size(24, 24);
            this.cmdGetPTermNominalOutput.TabIndex = 16;
            this.cmdGetPTermNominalOutput.UseVisualStyleBackColor = true;
            this.cmdGetPTermNominalOutput.Click += new System.EventHandler(this.cmdGetPTermNominalOutput_Click);
            // 
            // lblNominalOutputValueUnit
            // 
            this.lblNominalOutputValueUnit.AutoSize = true;
            this.lblNominalOutputValueUnit.BackColor = System.Drawing.Color.Transparent;
            this.lblNominalOutputValueUnit.Enabled = false;
            this.lblNominalOutputValueUnit.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNominalOutputValueUnit.Location = new System.Drawing.Point(243, 144);
            this.lblNominalOutputValueUnit.Name = "lblNominalOutputValueUnit";
            this.lblNominalOutputValueUnit.Size = new System.Drawing.Size(16, 15);
            this.lblNominalOutputValueUnit.TabIndex = 15;
            this.lblNominalOutputValueUnit.Text = "%";
            this.lblNominalOutputValueUnit.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblNominalOutputValue
            // 
            this.lblNominalOutputValue.AutoSize = true;
            this.lblNominalOutputValue.Location = new System.Drawing.Point(51, 144);
            this.lblNominalOutputValue.Name = "lblNominalOutputValue";
            this.lblNominalOutputValue.Size = new System.Drawing.Size(116, 15);
            this.lblNominalOutputValue.TabIndex = 14;
            this.lblNominalOutputValue.Text = "Nominal Duty Ratio:";
            // 
            // txtNominalOutputValue
            // 
            this.txtNominalOutputValue.Location = new System.Drawing.Point(173, 141);
            this.txtNominalOutputValue.Name = "txtNominalOutputValue";
            this.txtNominalOutputValue.Size = new System.Drawing.Size(64, 23);
            this.txtNominalOutputValue.TabIndex = 13;
            this.txtNominalOutputValue.Text = "50.0";
            this.txtNominalOutputValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtNominalOutputValue.TextChanged += new System.EventHandler(this.txtNominalOutputValue_TextChanged);
            // 
            // txtParam4
            // 
            this.txtParam4.Enabled = false;
            this.txtParam4.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtParam4.Location = new System.Drawing.Point(173, 110);
            this.txtParam4.Name = "txtParam4";
            this.txtParam4.Size = new System.Drawing.Size(86, 22);
            this.txtParam4.TabIndex = 10;
            this.txtParam4.Text = "12.966";
            this.txtParam4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtParam4.WordWrap = false;
            this.txtParam4.Enter += new System.EventHandler(this.NumberTextBox_Enter);
            this.txtParam4.KeyDown += new System.Windows.Forms.KeyEventHandler(this.NumberTextBox_KeyDownWithScaling);
            // 
            // lblUnit4
            // 
            this.lblUnit4.AutoSize = true;
            this.lblUnit4.BackColor = System.Drawing.Color.Transparent;
            this.lblUnit4.Enabled = false;
            this.lblUnit4.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUnit4.Location = new System.Drawing.Point(265, 113);
            this.lblUnit4.Name = "lblUnit4";
            this.lblUnit4.Size = new System.Drawing.Size(22, 15);
            this.lblUnit4.TabIndex = 11;
            this.lblUnit4.Text = "bit";
            this.lblUnit4.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblParam4
            // 
            this.lblParam4.AutoSize = true;
            this.lblParam4.BackColor = System.Drawing.Color.Transparent;
            this.lblParam4.Enabled = false;
            this.lblParam4.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblParam4.Location = new System.Drawing.Point(51, 113);
            this.lblParam4.Name = "lblParam4";
            this.lblParam4.Size = new System.Drawing.Size(116, 15);
            this.lblParam4.TabIndex = 9;
            this.lblParam4.Text = "Effective Resolution:";
            this.lblParam4.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtParam3
            // 
            this.txtParam3.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtParam3.Location = new System.Drawing.Point(173, 80);
            this.txtParam3.Name = "txtParam3";
            this.txtParam3.Size = new System.Drawing.Size(86, 22);
            this.txtParam3.TabIndex = 7;
            this.txtParam3.Text = "8000";
            this.txtParam3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtParam3.WordWrap = false;
            this.txtParam3.Enter += new System.EventHandler(this.NumberTextBox_Enter);
            this.txtParam3.KeyDown += new System.Windows.Forms.KeyEventHandler(this.NumberTextBox_KeyDownWithScaling);
            // 
            // lblUnit3
            // 
            this.lblUnit3.AutoSize = true;
            this.lblUnit3.BackColor = System.Drawing.Color.Transparent;
            this.lblUnit3.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUnit3.Location = new System.Drawing.Point(265, 83);
            this.lblUnit3.Name = "lblUnit3";
            this.lblUnit3.Size = new System.Drawing.Size(32, 15);
            this.lblUnit3.TabIndex = 8;
            this.lblUnit3.Text = "ticks";
            this.lblUnit3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblParam3
            // 
            this.lblParam3.AutoSize = true;
            this.lblParam3.BackColor = System.Drawing.Color.Transparent;
            this.lblParam3.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblParam3.Location = new System.Drawing.Point(81, 83);
            this.lblParam3.Name = "lblParam3";
            this.lblParam3.Size = new System.Drawing.Size(86, 15);
            this.lblParam3.TabIndex = 6;
            this.lblParam3.Text = "PWM Counter:";
            this.lblParam3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtParam2
            // 
            this.txtParam2.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtParam2.Location = new System.Drawing.Point(173, 51);
            this.txtParam2.Name = "txtParam2";
            this.txtParam2.Size = new System.Drawing.Size(86, 22);
            this.txtParam2.TabIndex = 4;
            this.txtParam2.Text = "4.0u";
            this.txtParam2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtParam2.WordWrap = false;
            this.txtParam2.Enter += new System.EventHandler(this.NumberTextBox_Enter);
            this.txtParam2.KeyDown += new System.Windows.Forms.KeyEventHandler(this.NumberTextBox_KeyDownWithScaling);
            // 
            // lblUnit2
            // 
            this.lblUnit2.AutoSize = true;
            this.lblUnit2.BackColor = System.Drawing.Color.Transparent;
            this.lblUnit2.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUnit2.Location = new System.Drawing.Point(265, 54);
            this.lblUnit2.Name = "lblUnit2";
            this.lblUnit2.Size = new System.Drawing.Size(25, 15);
            this.lblUnit2.TabIndex = 5;
            this.lblUnit2.Text = "sec";
            this.lblUnit2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblParam2
            // 
            this.lblParam2.AutoSize = true;
            this.lblParam2.BackColor = System.Drawing.Color.Transparent;
            this.lblParam2.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblParam2.Location = new System.Drawing.Point(88, 54);
            this.lblParam2.Name = "lblParam2";
            this.lblParam2.Size = new System.Drawing.Size(79, 15);
            this.lblParam2.TabIndex = 3;
            this.lblParam2.Text = "PWM Period:";
            this.lblParam2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtParam1
            // 
            this.txtParam1.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtParam1.Location = new System.Drawing.Point(173, 21);
            this.txtParam1.Name = "txtParam1";
            this.txtParam1.Size = new System.Drawing.Size(86, 22);
            this.txtParam1.TabIndex = 1;
            this.txtParam1.Text = "250k";
            this.txtParam1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtParam1.WordWrap = false;
            this.txtParam1.TextChanged += new System.EventHandler(this.CalculateOutputGain);
            this.txtParam1.Enter += new System.EventHandler(this.NumberTextBox_Enter);
            this.txtParam1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.NumberTextBox_KeyDownWithScaling);
            // 
            // lblUnit1
            // 
            this.lblUnit1.AutoSize = true;
            this.lblUnit1.BackColor = System.Drawing.Color.Transparent;
            this.lblUnit1.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUnit1.Location = new System.Drawing.Point(265, 24);
            this.lblUnit1.Name = "lblUnit1";
            this.lblUnit1.Size = new System.Drawing.Size(20, 15);
            this.lblUnit1.TabIndex = 2;
            this.lblUnit1.Text = "Hz";
            this.lblUnit1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblParam1
            // 
            this.lblParam1.AutoSize = true;
            this.lblParam1.BackColor = System.Drawing.Color.Transparent;
            this.lblParam1.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblParam1.Location = new System.Drawing.Point(68, 24);
            this.lblParam1.Name = "lblParam1";
            this.lblParam1.Size = new System.Drawing.Size(99, 15);
            this.lblParam1.TabIndex = 0;
            this.lblParam1.Text = "PWM Frequency:";
            this.lblParam1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // grmOutputGain
            // 
            this.grmOutputGain.Controls.Add(this.lblOutputGain);
            this.grmOutputGain.Controls.Add(this.lblInputGainUnit);
            this.grmOutputGain.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grmOutputGain.Location = new System.Drawing.Point(6, 170);
            this.grmOutputGain.Name = "grmOutputGain";
            this.grmOutputGain.Size = new System.Drawing.Size(286, 48);
            this.grmOutputGain.TabIndex = 12;
            this.grmOutputGain.TabStop = false;
            this.grmOutputGain.Text = "Signal Gain:";
            // 
            // lblOutputGain
            // 
            this.lblOutputGain.BackColor = System.Drawing.Color.Transparent;
            this.lblOutputGain.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOutputGain.Location = new System.Drawing.Point(25, 19);
            this.lblOutputGain.Name = "lblOutputGain";
            this.lblOutputGain.Size = new System.Drawing.Size(197, 15);
            this.lblOutputGain.TabIndex = 13;
            this.lblOutputGain.Text = "1.000";
            this.lblOutputGain.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblInputGainUnit
            // 
            this.lblInputGainUnit.AutoSize = true;
            this.lblInputGainUnit.BackColor = System.Drawing.Color.Transparent;
            this.lblInputGainUnit.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInputGainUnit.Location = new System.Drawing.Point(228, 19);
            this.lblInputGainUnit.Name = "lblInputGainUnit";
            this.lblInputGainUnit.Size = new System.Drawing.Size(51, 15);
            this.lblInputGainUnit.TabIndex = 14;
            this.lblInputGainUnit.Text = "tick/tick";
            // 
            // frmCalculateOutputGain
            // 
            this.AcceptButton = this.cmdOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cmdCancel;
            this.ClientSize = new System.Drawing.Size(551, 552);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.grpVoltageDividerCalculation);
            this.Controls.Add(this.tabPWMMode);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdOK);
            this.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmCalculateOutputGain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Output Gain";
            this.Load += new System.EventHandler(this.frmCalculateOutputGain_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmCalculateOutputGain_KeyDown);
            this.tabPWMMode.ResumeLayout(false);
            this.tabFixedFrequency.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picFFreq)).EndInit();
            this.tabVariableFrequency.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picVPhase)).EndInit();
            this.tabPhaseShifted.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picPSFT)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.grpVoltageDividerCalculation.ResumeLayout(false);
            this.grpVoltageDividerCalculation.PerformLayout();
            this.grmOutputGain.ResumeLayout(false);
            this.grmOutputGain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.TabControl tabPWMMode;
        private System.Windows.Forms.TabPage tabFixedFrequency;
        private System.Windows.Forms.TabPage tabPhaseShifted;
        private System.Windows.Forms.TabPage tabVariableFrequency;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtSourceClock;
        private System.Windows.Forms.Label lblSourceClockUnit;
        private System.Windows.Forms.TextBox txtClockDivider;
        private System.Windows.Forms.Label lblResolutionUnit;
        private System.Windows.Forms.Label lblSourceClock;
        private System.Windows.Forms.TextBox txtResolution;
        private System.Windows.Forms.Label lblMaximum;
        private System.Windows.Forms.Label lblClockDivider;
        private System.Windows.Forms.Label lblResolution;
        private System.Windows.Forms.TextBox txtMaximum;
        private System.Windows.Forms.GroupBox grpVoltageDividerCalculation;
        private System.Windows.Forms.GroupBox grmOutputGain;
        private System.Windows.Forms.Label lblOutputGain;
        private System.Windows.Forms.Label lblInputGainUnit;
        private System.Windows.Forms.Label lblUnit1;
        private System.Windows.Forms.Label lblParam1;
        private System.Windows.Forms.TextBox txtParam2;
        private System.Windows.Forms.Label lblUnit2;
        private System.Windows.Forms.Label lblParam2;
        private System.Windows.Forms.TextBox txtParam1;
        private System.Windows.Forms.TextBox txtParam3;
        private System.Windows.Forms.Label lblUnit3;
        private System.Windows.Forms.Label lblParam3;
        private System.Windows.Forms.PictureBox picFFreq;
        private System.Windows.Forms.PictureBox picPSFT;
        private System.Windows.Forms.PictureBox picVPhase;
        private System.Windows.Forms.TextBox txtParam4;
        private System.Windows.Forms.Label lblUnit4;
        private System.Windows.Forms.Label lblParam4;
        private System.Windows.Forms.Label lblNominalOutputValue;
        private System.Windows.Forms.TextBox txtNominalOutputValue;
        private System.Windows.Forms.Label lblNominalOutputValueUnit;
        private System.Windows.Forms.Label lblDeviceType;
        private System.Windows.Forms.ComboBox cmbDeviceType;
        private System.Windows.Forms.Button cmdGetPTermNominalOutput;
    }
}