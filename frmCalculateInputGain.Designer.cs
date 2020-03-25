namespace dcld
{
    partial class frmCalculateInputGain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCalculateInputGain));
            this.lblParam1Label = new System.Windows.Forms.Label();
            this.txtParam1 = new System.Windows.Forms.TextBox();
            this.txtParam2 = new System.Windows.Forms.TextBox();
            this.lblParam2Label = new System.Windows.Forms.Label();
            this.cmdOK = new System.Windows.Forms.Button();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.tabFeedback = new System.Windows.Forms.TabControl();
            this.tabVD = new System.Windows.Forms.TabPage();
            this.grpVoltageDividerCircuit = new System.Windows.Forms.GroupBox();
            this.picVD = new System.Windows.Forms.PictureBox();
            this.tabCS = new System.Windows.Forms.TabPage();
            this.grpCSCircuit = new System.Windows.Forms.GroupBox();
            this.picCS = new System.Windows.Forms.PictureBox();
            this.tabCT = new System.Windows.Forms.TabPage();
            this.grpCTCircuit = new System.Windows.Forms.GroupBox();
            this.picCT = new System.Windows.Forms.PictureBox();
            this.tabDS = new System.Windows.Forms.TabPage();
            this.grpDSCircuit = new System.Windows.Forms.GroupBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lblInputGain = new System.Windows.Forms.Label();
            this.lblParam3Unit = new System.Windows.Forms.Label();
            this.txtParam3 = new System.Windows.Forms.TextBox();
            this.lblParam3Label = new System.Windows.Forms.Label();
            this.lblParam2Unit = new System.Windows.Forms.Label();
            this.lblParam1Unit = new System.Windows.Forms.Label();
            this.grpVoltageDividerCalculation = new System.Windows.Forms.GroupBox();
            this.lblParam0Unit = new System.Windows.Forms.Label();
            this.txtParam0 = new System.Windows.Forms.TextBox();
            this.lblParam0Label = new System.Windows.Forms.Label();
            this.grmInputGain = new System.Windows.Forms.GroupBox();
            this.lblInputGainUnit = new System.Windows.Forms.Label();
            this.txtInputMaximum = new System.Windows.Forms.TextBox();
            this.lblADCMaximum = new System.Windows.Forms.Label();
            this.txtInputReference = new System.Windows.Forms.TextBox();
            this.txtInputResolution = new System.Windows.Forms.TextBox();
            this.lblInputReferenceUnit = new System.Windows.Forms.Label();
            this.lblInputResolutionUnit = new System.Windows.Forms.Label();
            this.lblInputReference = new System.Windows.Forms.Label();
            this.lblInputResolution = new System.Windows.Forms.Label();
            this.txtInputMinimum = new System.Windows.Forms.TextBox();
            this.lblADCMinimum = new System.Windows.Forms.Label();
            this.chkInputSigned = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tabFeedback.SuspendLayout();
            this.tabVD.SuspendLayout();
            this.grpVoltageDividerCircuit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picVD)).BeginInit();
            this.tabCS.SuspendLayout();
            this.grpCSCircuit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picCS)).BeginInit();
            this.tabCT.SuspendLayout();
            this.grpCTCircuit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picCT)).BeginInit();
            this.tabDS.SuspendLayout();
            this.grpDSCircuit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.grpVoltageDividerCalculation.SuspendLayout();
            this.grmInputGain.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblParam1Label
            // 
            this.lblParam1Label.AutoSize = true;
            this.lblParam1Label.BackColor = System.Drawing.Color.Transparent;
            this.lblParam1Label.Location = new System.Drawing.Point(121, 56);
            this.lblParam1Label.Name = "lblParam1Label";
            this.lblParam1Label.Size = new System.Drawing.Size(24, 15);
            this.lblParam1Label.TabIndex = 0;
            this.lblParam1Label.Text = "R1:";
            this.lblParam1Label.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtParam1
            // 
            this.txtParam1.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtParam1.Location = new System.Drawing.Point(151, 52);
            this.txtParam1.Name = "txtParam1";
            this.txtParam1.Size = new System.Drawing.Size(95, 23);
            this.txtParam1.TabIndex = 1;
            this.txtParam1.Text = "12000";
            this.txtParam1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtParam1.TextChanged += new System.EventHandler(this.CalculateInputGain);
            this.txtParam1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.NumberTextBox_KeyDownWithScaling);
            // 
            // txtParam2
            // 
            this.txtParam2.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtParam2.Location = new System.Drawing.Point(151, 81);
            this.txtParam2.Name = "txtParam2";
            this.txtParam2.Size = new System.Drawing.Size(95, 23);
            this.txtParam2.TabIndex = 2;
            this.txtParam2.Text = "2200";
            this.txtParam2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtParam2.TextChanged += new System.EventHandler(this.CalculateInputGain);
            // 
            // lblParam2Label
            // 
            this.lblParam2Label.AutoSize = true;
            this.lblParam2Label.BackColor = System.Drawing.Color.Transparent;
            this.lblParam2Label.Location = new System.Drawing.Point(121, 85);
            this.lblParam2Label.Name = "lblParam2Label";
            this.lblParam2Label.Size = new System.Drawing.Size(24, 15);
            this.lblParam2Label.TabIndex = 3;
            this.lblParam2Label.Text = "R2:";
            this.lblParam2Label.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // cmdOK
            // 
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.Location = new System.Drawing.Point(363, 488);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(82, 30);
            this.cmdOK.TabIndex = 4;
            this.cmdOK.Text = "&OK";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // cmdCancel
            // 
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(457, 488);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(82, 30);
            this.cmdCancel.TabIndex = 5;
            this.cmdCancel.Text = "&Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // tabFeedback
            // 
            this.tabFeedback.Controls.Add(this.tabVD);
            this.tabFeedback.Controls.Add(this.tabCS);
            this.tabFeedback.Controls.Add(this.tabCT);
            this.tabFeedback.Controls.Add(this.tabDS);
            this.tabFeedback.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabFeedback.Location = new System.Drawing.Point(0, 0);
            this.tabFeedback.Name = "tabFeedback";
            this.tabFeedback.SelectedIndex = 0;
            this.tabFeedback.Size = new System.Drawing.Size(551, 281);
            this.tabFeedback.TabIndex = 6;
            this.tabFeedback.SelectedIndexChanged += new System.EventHandler(this.tabFeedback_SelectedIndexChanged);
            // 
            // tabVD
            // 
            this.tabVD.Controls.Add(this.grpVoltageDividerCircuit);
            this.tabVD.Location = new System.Drawing.Point(4, 24);
            this.tabVD.Name = "tabVD";
            this.tabVD.Padding = new System.Windows.Forms.Padding(3);
            this.tabVD.Size = new System.Drawing.Size(543, 253);
            this.tabVD.TabIndex = 0;
            this.tabVD.Text = "Voltage Feedback";
            this.tabVD.UseVisualStyleBackColor = true;
            // 
            // grpVoltageDividerCircuit
            // 
            this.grpVoltageDividerCircuit.Controls.Add(this.picVD);
            this.grpVoltageDividerCircuit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpVoltageDividerCircuit.Location = new System.Drawing.Point(3, 3);
            this.grpVoltageDividerCircuit.Name = "grpVoltageDividerCircuit";
            this.grpVoltageDividerCircuit.Size = new System.Drawing.Size(537, 247);
            this.grpVoltageDividerCircuit.TabIndex = 13;
            this.grpVoltageDividerCircuit.TabStop = false;
            this.grpVoltageDividerCircuit.Text = "Circuit";
            // 
            // picVD
            // 
            this.picVD.BackgroundImage = global::dcld.Properties.Resources.VD;
            this.picVD.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.picVD.Location = new System.Drawing.Point(26, 22);
            this.picVD.Name = "picVD";
            this.picVD.Size = new System.Drawing.Size(483, 216);
            this.picVD.TabIndex = 0;
            this.picVD.TabStop = false;
            // 
            // tabCS
            // 
            this.tabCS.Controls.Add(this.grpCSCircuit);
            this.tabCS.Location = new System.Drawing.Point(4, 24);
            this.tabCS.Name = "tabCS";
            this.tabCS.Padding = new System.Windows.Forms.Padding(3);
            this.tabCS.Size = new System.Drawing.Size(543, 253);
            this.tabCS.TabIndex = 2;
            this.tabCS.Text = "Shunt Amplifier";
            this.tabCS.UseVisualStyleBackColor = true;
            // 
            // grpCSCircuit
            // 
            this.grpCSCircuit.Controls.Add(this.picCS);
            this.grpCSCircuit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpCSCircuit.Location = new System.Drawing.Point(3, 3);
            this.grpCSCircuit.Name = "grpCSCircuit";
            this.grpCSCircuit.Size = new System.Drawing.Size(537, 247);
            this.grpCSCircuit.TabIndex = 15;
            this.grpCSCircuit.TabStop = false;
            this.grpCSCircuit.Text = "Circuit";
            // 
            // picCS
            // 
            this.picCS.BackgroundImage = global::dcld.Properties.Resources.CS;
            this.picCS.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.picCS.Location = new System.Drawing.Point(27, 14);
            this.picCS.Name = "picCS";
            this.picCS.Size = new System.Drawing.Size(475, 219);
            this.picCS.TabIndex = 0;
            this.picCS.TabStop = false;
            // 
            // tabCT
            // 
            this.tabCT.Controls.Add(this.grpCTCircuit);
            this.tabCT.Location = new System.Drawing.Point(4, 24);
            this.tabCT.Name = "tabCT";
            this.tabCT.Padding = new System.Windows.Forms.Padding(3);
            this.tabCT.Size = new System.Drawing.Size(543, 253);
            this.tabCT.TabIndex = 1;
            this.tabCT.Text = "Current Transformer";
            this.tabCT.UseVisualStyleBackColor = true;
            // 
            // grpCTCircuit
            // 
            this.grpCTCircuit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.grpCTCircuit.Controls.Add(this.picCT);
            this.grpCTCircuit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpCTCircuit.Location = new System.Drawing.Point(3, 3);
            this.grpCTCircuit.Name = "grpCTCircuit";
            this.grpCTCircuit.Size = new System.Drawing.Size(537, 247);
            this.grpCTCircuit.TabIndex = 17;
            this.grpCTCircuit.TabStop = false;
            this.grpCTCircuit.Text = "Circuit";
            // 
            // picCT
            // 
            this.picCT.BackgroundImage = global::dcld.Properties.Resources.CT;
            this.picCT.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.picCT.Location = new System.Drawing.Point(6, 39);
            this.picCT.Name = "picCT";
            this.picCT.Size = new System.Drawing.Size(503, 168);
            this.picCT.TabIndex = 0;
            this.picCT.TabStop = false;
            // 
            // tabDS
            // 
            this.tabDS.Controls.Add(this.grpDSCircuit);
            this.tabDS.Location = new System.Drawing.Point(4, 24);
            this.tabDS.Name = "tabDS";
            this.tabDS.Padding = new System.Windows.Forms.Padding(3);
            this.tabDS.Size = new System.Drawing.Size(543, 253);
            this.tabDS.TabIndex = 3;
            this.tabDS.Text = "Digital Source";
            this.tabDS.UseVisualStyleBackColor = true;
            // 
            // grpDSCircuit
            // 
            this.grpDSCircuit.Controls.Add(this.pictureBox1);
            this.grpDSCircuit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpDSCircuit.Location = new System.Drawing.Point(3, 3);
            this.grpDSCircuit.Name = "grpDSCircuit";
            this.grpDSCircuit.Size = new System.Drawing.Size(537, 247);
            this.grpDSCircuit.TabIndex = 14;
            this.grpDSCircuit.TabStop = false;
            this.grpDSCircuit.Text = "Circuit";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = global::dcld.Properties.Resources.DS;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox1.Location = new System.Drawing.Point(14, 42);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(501, 134);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // lblInputGain
            // 
            this.lblInputGain.BackColor = System.Drawing.Color.Transparent;
            this.lblInputGain.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInputGain.Location = new System.Drawing.Point(25, 19);
            this.lblInputGain.Name = "lblInputGain";
            this.lblInputGain.Size = new System.Drawing.Size(215, 15);
            this.lblInputGain.TabIndex = 11;
            this.lblInputGain.Text = "1.000";
            this.lblInputGain.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblParam3Unit
            // 
            this.lblParam3Unit.AutoSize = true;
            this.lblParam3Unit.BackColor = System.Drawing.Color.Transparent;
            this.lblParam3Unit.Location = new System.Drawing.Point(252, 113);
            this.lblParam3Unit.Name = "lblParam3Unit";
            this.lblParam3Unit.Size = new System.Drawing.Size(25, 15);
            this.lblParam3Unit.TabIndex = 10;
            this.lblParam3Unit.Text = "V/V";
            // 
            // txtParam3
            // 
            this.txtParam3.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtParam3.Location = new System.Drawing.Point(151, 110);
            this.txtParam3.Name = "txtParam3";
            this.txtParam3.Size = new System.Drawing.Size(95, 23);
            this.txtParam3.TabIndex = 8;
            this.txtParam3.Text = "1.000";
            this.txtParam3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtParam3.TextChanged += new System.EventHandler(this.CalculateInputGain);
            // 
            // lblParam3Label
            // 
            this.lblParam3Label.AutoSize = true;
            this.lblParam3Label.BackColor = System.Drawing.Color.Transparent;
            this.lblParam3Label.Location = new System.Drawing.Point(56, 113);
            this.lblParam3Label.Name = "lblParam3Label";
            this.lblParam3Label.Size = new System.Drawing.Size(89, 15);
            this.lblParam3Label.TabIndex = 9;
            this.lblParam3Label.Text = "Amplifier Gain:";
            this.lblParam3Label.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblParam2Unit
            // 
            this.lblParam2Unit.AutoSize = true;
            this.lblParam2Unit.BackColor = System.Drawing.Color.Transparent;
            this.lblParam2Unit.Font = new System.Drawing.Font("Symbol", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.lblParam2Unit.Location = new System.Drawing.Point(252, 84);
            this.lblParam2Unit.Name = "lblParam2Unit";
            this.lblParam2Unit.Size = new System.Drawing.Size(19, 16);
            this.lblParam2Unit.TabIndex = 7;
            this.lblParam2Unit.Text = "W";
            // 
            // lblParam1Unit
            // 
            this.lblParam1Unit.AutoSize = true;
            this.lblParam1Unit.BackColor = System.Drawing.Color.Transparent;
            this.lblParam1Unit.Font = new System.Drawing.Font("Symbol", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.lblParam1Unit.Location = new System.Drawing.Point(251, 55);
            this.lblParam1Unit.Name = "lblParam1Unit";
            this.lblParam1Unit.Size = new System.Drawing.Size(19, 16);
            this.lblParam1Unit.TabIndex = 6;
            this.lblParam1Unit.Text = "W";
            // 
            // grpVoltageDividerCalculation
            // 
            this.grpVoltageDividerCalculation.Controls.Add(this.lblParam0Unit);
            this.grpVoltageDividerCalculation.Controls.Add(this.txtParam0);
            this.grpVoltageDividerCalculation.Controls.Add(this.lblParam0Label);
            this.grpVoltageDividerCalculation.Controls.Add(this.grmInputGain);
            this.grpVoltageDividerCalculation.Controls.Add(this.txtParam1);
            this.grpVoltageDividerCalculation.Controls.Add(this.lblParam2Label);
            this.grpVoltageDividerCalculation.Controls.Add(this.lblParam1Label);
            this.grpVoltageDividerCalculation.Controls.Add(this.lblParam3Unit);
            this.grpVoltageDividerCalculation.Controls.Add(this.txtParam2);
            this.grpVoltageDividerCalculation.Controls.Add(this.txtParam3);
            this.grpVoltageDividerCalculation.Controls.Add(this.lblParam1Unit);
            this.grpVoltageDividerCalculation.Controls.Add(this.lblParam3Label);
            this.grpVoltageDividerCalculation.Controls.Add(this.lblParam2Unit);
            this.grpVoltageDividerCalculation.Location = new System.Drawing.Point(235, 283);
            this.grpVoltageDividerCalculation.Name = "grpVoltageDividerCalculation";
            this.grpVoltageDividerCalculation.Size = new System.Drawing.Size(298, 199);
            this.grpVoltageDividerCalculation.TabIndex = 14;
            this.grpVoltageDividerCalculation.TabStop = false;
            this.grpVoltageDividerCalculation.Text = "Calculation";
            // 
            // lblParam0Unit
            // 
            this.lblParam0Unit.AutoSize = true;
            this.lblParam0Unit.BackColor = System.Drawing.Color.Transparent;
            this.lblParam0Unit.Location = new System.Drawing.Point(252, 24);
            this.lblParam0Unit.Name = "lblParam0Unit";
            this.lblParam0Unit.Size = new System.Drawing.Size(14, 15);
            this.lblParam0Unit.TabIndex = 55;
            this.lblParam0Unit.Text = "V";
            // 
            // txtParam0
            // 
            this.txtParam0.Enabled = false;
            this.txtParam0.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtParam0.Location = new System.Drawing.Point(151, 21);
            this.txtParam0.Name = "txtParam0";
            this.txtParam0.Size = new System.Drawing.Size(95, 23);
            this.txtParam0.TabIndex = 53;
            this.txtParam0.Text = "1.000";
            this.txtParam0.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblParam0Label
            // 
            this.lblParam0Label.AutoSize = true;
            this.lblParam0Label.BackColor = System.Drawing.Color.Transparent;
            this.lblParam0Label.Location = new System.Drawing.Point(48, 24);
            this.lblParam0Label.Name = "lblParam0Label";
            this.lblParam0Label.Size = new System.Drawing.Size(97, 15);
            this.lblParam0Label.TabIndex = 54;
            this.lblParam0Label.Text = "Maximum Input:";
            this.lblParam0Label.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // grmInputGain
            // 
            this.grmInputGain.Controls.Add(this.lblInputGain);
            this.grmInputGain.Controls.Add(this.lblInputGainUnit);
            this.grmInputGain.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grmInputGain.Location = new System.Drawing.Point(6, 145);
            this.grmInputGain.Name = "grmInputGain";
            this.grmInputGain.Size = new System.Drawing.Size(286, 48);
            this.grmInputGain.TabIndex = 52;
            this.grmInputGain.TabStop = false;
            this.grmInputGain.Text = "Signal Gain:";
            // 
            // lblInputGainUnit
            // 
            this.lblInputGainUnit.AutoSize = true;
            this.lblInputGainUnit.BackColor = System.Drawing.Color.Transparent;
            this.lblInputGainUnit.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInputGainUnit.Location = new System.Drawing.Point(246, 19);
            this.lblInputGainUnit.Name = "lblInputGainUnit";
            this.lblInputGainUnit.Size = new System.Drawing.Size(28, 15);
            this.lblInputGainUnit.TabIndex = 13;
            this.lblInputGainUnit.Text = "V/V";
            // 
            // txtInputMaximum
            // 
            this.txtInputMaximum.Enabled = false;
            this.txtInputMaximum.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtInputMaximum.Location = new System.Drawing.Point(114, 110);
            this.txtInputMaximum.Name = "txtInputMaximum";
            this.txtInputMaximum.Size = new System.Drawing.Size(64, 22);
            this.txtInputMaximum.TabIndex = 48;
            this.txtInputMaximum.Text = "4095";
            this.txtInputMaximum.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtInputMaximum.WordWrap = false;
            // 
            // lblADCMaximum
            // 
            this.lblADCMaximum.AutoSize = true;
            this.lblADCMaximum.BackColor = System.Drawing.Color.Transparent;
            this.lblADCMaximum.Enabled = false;
            this.lblADCMaximum.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblADCMaximum.Location = new System.Drawing.Point(43, 113);
            this.lblADCMaximum.Name = "lblADCMaximum";
            this.lblADCMaximum.Size = new System.Drawing.Size(65, 15);
            this.lblADCMaximum.TabIndex = 49;
            this.lblADCMaximum.Text = "Maximum:";
            this.lblADCMaximum.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtInputReference
            // 
            this.txtInputReference.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtInputReference.Location = new System.Drawing.Point(114, 22);
            this.txtInputReference.Name = "txtInputReference";
            this.txtInputReference.Size = new System.Drawing.Size(64, 22);
            this.txtInputReference.TabIndex = 45;
            this.txtInputReference.Text = "3.300";
            this.txtInputReference.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtInputReference.WordWrap = false;
            this.txtInputReference.TextChanged += new System.EventHandler(this.CalculateInputGain);
            // 
            // txtInputResolution
            // 
            this.txtInputResolution.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtInputResolution.Location = new System.Drawing.Point(114, 51);
            this.txtInputResolution.Name = "txtInputResolution";
            this.txtInputResolution.Size = new System.Drawing.Size(64, 22);
            this.txtInputResolution.TabIndex = 42;
            this.txtInputResolution.Text = "12";
            this.txtInputResolution.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtInputResolution.WordWrap = false;
            this.txtInputResolution.TextChanged += new System.EventHandler(this.CalculateInputGain);
            // 
            // lblInputReferenceUnit
            // 
            this.lblInputReferenceUnit.AutoSize = true;
            this.lblInputReferenceUnit.BackColor = System.Drawing.Color.Transparent;
            this.lblInputReferenceUnit.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInputReferenceUnit.Location = new System.Drawing.Point(184, 25);
            this.lblInputReferenceUnit.Name = "lblInputReferenceUnit";
            this.lblInputReferenceUnit.Size = new System.Drawing.Size(14, 15);
            this.lblInputReferenceUnit.TabIndex = 47;
            this.lblInputReferenceUnit.Text = "V";
            // 
            // lblInputResolutionUnit
            // 
            this.lblInputResolutionUnit.AutoSize = true;
            this.lblInputResolutionUnit.BackColor = System.Drawing.Color.Transparent;
            this.lblInputResolutionUnit.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInputResolutionUnit.Location = new System.Drawing.Point(184, 55);
            this.lblInputResolutionUnit.Name = "lblInputResolutionUnit";
            this.lblInputResolutionUnit.Size = new System.Drawing.Size(22, 15);
            this.lblInputResolutionUnit.TabIndex = 44;
            this.lblInputResolutionUnit.Text = "Bit";
            // 
            // lblInputReference
            // 
            this.lblInputReference.AutoSize = true;
            this.lblInputReference.BackColor = System.Drawing.Color.Transparent;
            this.lblInputReference.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInputReference.Location = new System.Drawing.Point(15, 25);
            this.lblInputReference.Name = "lblInputReference";
            this.lblInputReference.Size = new System.Drawing.Size(95, 15);
            this.lblInputReference.TabIndex = 46;
            this.lblInputReference.Text = "Input Reference:";
            this.lblInputReference.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblInputResolution
            // 
            this.lblInputResolution.AutoSize = true;
            this.lblInputResolution.BackColor = System.Drawing.Color.Transparent;
            this.lblInputResolution.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInputResolution.Location = new System.Drawing.Point(10, 54);
            this.lblInputResolution.Name = "lblInputResolution";
            this.lblInputResolution.Size = new System.Drawing.Size(100, 15);
            this.lblInputResolution.TabIndex = 43;
            this.lblInputResolution.Text = "Input Resolution:";
            this.lblInputResolution.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtInputMinimum
            // 
            this.txtInputMinimum.Enabled = false;
            this.txtInputMinimum.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtInputMinimum.Location = new System.Drawing.Point(114, 80);
            this.txtInputMinimum.Name = "txtInputMinimum";
            this.txtInputMinimum.Size = new System.Drawing.Size(64, 22);
            this.txtInputMinimum.TabIndex = 50;
            this.txtInputMinimum.Text = "0";
            this.txtInputMinimum.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtInputMinimum.WordWrap = false;
            // 
            // lblADCMinimum
            // 
            this.lblADCMinimum.AutoSize = true;
            this.lblADCMinimum.BackColor = System.Drawing.Color.Transparent;
            this.lblADCMinimum.Enabled = false;
            this.lblADCMinimum.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblADCMinimum.Location = new System.Drawing.Point(43, 83);
            this.lblADCMinimum.Name = "lblADCMinimum";
            this.lblADCMinimum.Size = new System.Drawing.Size(63, 15);
            this.lblADCMinimum.TabIndex = 51;
            this.lblADCMinimum.Text = "Minimum:";
            this.lblADCMinimum.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // chkInputSigned
            // 
            this.chkInputSigned.AutoSize = true;
            this.chkInputSigned.Location = new System.Drawing.Point(29, 146);
            this.chkInputSigned.Name = "chkInputSigned";
            this.chkInputSigned.Size = new System.Drawing.Size(135, 19);
            this.chkInputSigned.TabIndex = 53;
            this.chkInputSigned.Text = "&Differential (signed)";
            this.chkInputSigned.UseVisualStyleBackColor = true;
            this.chkInputSigned.CheckedChanged += new System.EventHandler(this.CalculateInputGain);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtInputReference);
            this.groupBox1.Controls.Add(this.lblInputReferenceUnit);
            this.groupBox1.Controls.Add(this.chkInputSigned);
            this.groupBox1.Controls.Add(this.txtInputResolution);
            this.groupBox1.Controls.Add(this.lblInputResolutionUnit);
            this.groupBox1.Controls.Add(this.lblInputReference);
            this.groupBox1.Controls.Add(this.txtInputMinimum);
            this.groupBox1.Controls.Add(this.lblADCMaximum);
            this.groupBox1.Controls.Add(this.lblInputResolution);
            this.groupBox1.Controls.Add(this.lblADCMinimum);
            this.groupBox1.Controls.Add(this.txtInputMaximum);
            this.groupBox1.Location = new System.Drawing.Point(4, 283);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(225, 199);
            this.groupBox1.TabIndex = 15;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Input Scaling";
            // 
            // frmCalculateInputGain
            // 
            this.AcceptButton = this.cmdOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cmdCancel;
            this.ClientSize = new System.Drawing.Size(551, 530);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.grpVoltageDividerCalculation);
            this.Controls.Add(this.tabFeedback);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdOK);
            this.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmCalculateInputGain";
            this.Text = "Input Gain";
            this.Load += new System.EventHandler(this.frmCalculateInputGain_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmCalculateInputGain_KeyDown);
            this.tabFeedback.ResumeLayout(false);
            this.tabVD.ResumeLayout(false);
            this.grpVoltageDividerCircuit.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picVD)).EndInit();
            this.tabCS.ResumeLayout(false);
            this.grpCSCircuit.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picCS)).EndInit();
            this.tabCT.ResumeLayout(false);
            this.grpCTCircuit.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picCT)).EndInit();
            this.tabDS.ResumeLayout(false);
            this.grpDSCircuit.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.grpVoltageDividerCalculation.ResumeLayout(false);
            this.grpVoltageDividerCalculation.PerformLayout();
            this.grmInputGain.ResumeLayout(false);
            this.grmInputGain.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblParam1Label;
        private System.Windows.Forms.TextBox txtParam1;
        private System.Windows.Forms.TextBox txtParam2;
        private System.Windows.Forms.Label lblParam2Label;
        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.TabControl tabFeedback;
        private System.Windows.Forms.TabPage tabVD;
        private System.Windows.Forms.TabPage tabCT;
        private System.Windows.Forms.Label lblParam3Unit;
        private System.Windows.Forms.TextBox txtParam3;
        private System.Windows.Forms.Label lblParam3Label;
        private System.Windows.Forms.Label lblParam2Unit;
        private System.Windows.Forms.Label lblParam1Unit;
        private System.Windows.Forms.Label lblInputGain;
        private System.Windows.Forms.TabPage tabCS;
        private System.Windows.Forms.GroupBox grpVoltageDividerCircuit;
        private System.Windows.Forms.GroupBox grpVoltageDividerCalculation;
        private System.Windows.Forms.GroupBox grpCSCircuit;
        private System.Windows.Forms.Label lblInputGainUnit;
        private System.Windows.Forms.GroupBox grpCTCircuit;
        private System.Windows.Forms.PictureBox picCT;
        private System.Windows.Forms.PictureBox picCS;
        private System.Windows.Forms.PictureBox picVD;
        private System.Windows.Forms.TabPage tabDS;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TextBox txtInputResolution;
        private System.Windows.Forms.Label lblInputResolutionUnit;
        private System.Windows.Forms.Label lblInputResolution;
        private System.Windows.Forms.TextBox txtInputReference;
        private System.Windows.Forms.Label lblInputReferenceUnit;
        private System.Windows.Forms.Label lblInputReference;
        private System.Windows.Forms.GroupBox grpDSCircuit;
        private System.Windows.Forms.TextBox txtInputMaximum;
        private System.Windows.Forms.Label lblADCMaximum;
        private System.Windows.Forms.TextBox txtInputMinimum;
        private System.Windows.Forms.Label lblADCMinimum;
        private System.Windows.Forms.GroupBox grmInputGain;
        private System.Windows.Forms.CheckBox chkInputSigned;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblParam0Unit;
        private System.Windows.Forms.TextBox txtParam0;
        private System.Windows.Forms.Label lblParam0Label;
    }
}