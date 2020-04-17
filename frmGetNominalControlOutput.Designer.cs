namespace dcld
{
    partial class frmGetNominalControlOutput
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmGetNominalControlOutput));
            this.cmbConverterType = new System.Windows.Forms.ComboBox();
            this.picInfoConverterType = new System.Windows.Forms.PictureBox();
            this.lblConverterType = new System.Windows.Forms.Label();
            this.txtWindingRatioPrimary = new System.Windows.Forms.TextBox();
            this.lblWindingRatio = new System.Windows.Forms.Label();
            this.lblWindingRatioUnit = new System.Windows.Forms.Label();
            this.lblNomInputVoltageUnit = new System.Windows.Forms.Label();
            this.lblNomInputVoltage = new System.Windows.Forms.Label();
            this.txtNomInputVoltage = new System.Windows.Forms.TextBox();
            this.lblNomOutputVoltageUnit = new System.Windows.Forms.Label();
            this.lblNomOutputVoltage = new System.Windows.Forms.Label();
            this.txtNomOutputVoltage = new System.Windows.Forms.TextBox();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.cmdOK = new System.Windows.Forms.Button();
            this.grpNominalOutput = new System.Windows.Forms.GroupBox();
            this.txtWindingRatioSecondary = new System.Windows.Forms.TextBox();
            this.lblOutputResultLabel = new System.Windows.Forms.Label();
            this.lblOutputResultUnit = new System.Windows.Forms.Label();
            this.lblOutputResult = new System.Windows.Forms.Label();
            this.lblNomEfficiencyUnit = new System.Windows.Forms.Label();
            this.lblNomEfficiency = new System.Windows.Forms.Label();
            this.txtNomEfficiency = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.picInfoConverterType)).BeginInit();
            this.grpNominalOutput.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmbConverterType
            // 
            this.cmbConverterType.FormattingEnabled = true;
            this.cmbConverterType.Items.AddRange(new object[] {
            "0 - Buck/Forward Converter",
            "1 - Boost Converter",
            "2 - Buck/Boost Converter"});
            this.cmbConverterType.Location = new System.Drawing.Point(126, 28);
            this.cmbConverterType.Name = "cmbConverterType";
            this.cmbConverterType.Size = new System.Drawing.Size(208, 23);
            this.cmbConverterType.TabIndex = 2;
            this.cmbConverterType.SelectedIndexChanged += new System.EventHandler(this.CalculateNominalOutput);
            // 
            // picInfoConverterType
            // 
            this.picInfoConverterType.Image = ((System.Drawing.Image)(resources.GetObject("picInfoConverterType.Image")));
            this.picInfoConverterType.InitialImage = ((System.Drawing.Image)(resources.GetObject("picInfoConverterType.InitialImage")));
            this.picInfoConverterType.Location = new System.Drawing.Point(342, 30);
            this.picInfoConverterType.Name = "picInfoConverterType";
            this.picInfoConverterType.Size = new System.Drawing.Size(19, 18);
            this.picInfoConverterType.TabIndex = 71;
            this.picInfoConverterType.TabStop = false;
            // 
            // lblConverterType
            // 
            this.lblConverterType.AutoSize = true;
            this.lblConverterType.Location = new System.Drawing.Point(30, 31);
            this.lblConverterType.Name = "lblConverterType";
            this.lblConverterType.Size = new System.Drawing.Size(90, 15);
            this.lblConverterType.TabIndex = 1;
            this.lblConverterType.Text = "Converter Type:";
            // 
            // txtWindingRatioPrimary
            // 
            this.txtWindingRatioPrimary.Location = new System.Drawing.Point(218, 69);
            this.txtWindingRatioPrimary.Name = "txtWindingRatioPrimary";
            this.txtWindingRatioPrimary.Size = new System.Drawing.Size(47, 23);
            this.txtWindingRatioPrimary.TabIndex = 4;
            this.txtWindingRatioPrimary.Text = "1";
            this.txtWindingRatioPrimary.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtWindingRatioPrimary.TextChanged += new System.EventHandler(this.CalculateNominalOutput);
            // 
            // lblWindingRatio
            // 
            this.lblWindingRatio.AutoSize = true;
            this.lblWindingRatio.Location = new System.Drawing.Point(96, 72);
            this.lblWindingRatio.Name = "lblWindingRatio";
            this.lblWindingRatio.Size = new System.Drawing.Size(116, 15);
            this.lblWindingRatio.TabIndex = 3;
            this.lblWindingRatio.Text = "Winding Ratio (P/S):";
            // 
            // lblWindingRatioUnit
            // 
            this.lblWindingRatioUnit.Location = new System.Drawing.Point(268, 73);
            this.lblWindingRatioUnit.Name = "lblWindingRatioUnit";
            this.lblWindingRatioUnit.Size = new System.Drawing.Size(16, 17);
            this.lblWindingRatioUnit.TabIndex = 5;
            this.lblWindingRatioUnit.Text = ":";
            this.lblWindingRatioUnit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblNomInputVoltageUnit
            // 
            this.lblNomInputVoltageUnit.AutoSize = true;
            this.lblNomInputVoltageUnit.Location = new System.Drawing.Point(338, 103);
            this.lblNomInputVoltageUnit.Name = "lblNomInputVoltageUnit";
            this.lblNomInputVoltageUnit.Size = new System.Drawing.Size(14, 15);
            this.lblNomInputVoltageUnit.TabIndex = 9;
            this.lblNomInputVoltageUnit.Text = "V";
            // 
            // lblNomInputVoltage
            // 
            this.lblNomInputVoltage.AutoSize = true;
            this.lblNomInputVoltage.Location = new System.Drawing.Point(80, 103);
            this.lblNomInputVoltage.Name = "lblNomInputVoltage";
            this.lblNomInputVoltage.Size = new System.Drawing.Size(132, 15);
            this.lblNomInputVoltage.TabIndex = 7;
            this.lblNomInputVoltage.Text = "Nominal Input Voltage:";
            // 
            // txtNomInputVoltage
            // 
            this.txtNomInputVoltage.Location = new System.Drawing.Point(218, 99);
            this.txtNomInputVoltage.Name = "txtNomInputVoltage";
            this.txtNomInputVoltage.Size = new System.Drawing.Size(116, 23);
            this.txtNomInputVoltage.TabIndex = 8;
            this.txtNomInputVoltage.Text = "0";
            this.txtNomInputVoltage.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtNomInputVoltage.TextChanged += new System.EventHandler(this.CalculateNominalOutput);
            // 
            // lblNomOutputVoltageUnit
            // 
            this.lblNomOutputVoltageUnit.AutoSize = true;
            this.lblNomOutputVoltageUnit.Location = new System.Drawing.Point(338, 133);
            this.lblNomOutputVoltageUnit.Name = "lblNomOutputVoltageUnit";
            this.lblNomOutputVoltageUnit.Size = new System.Drawing.Size(14, 15);
            this.lblNomOutputVoltageUnit.TabIndex = 12;
            this.lblNomOutputVoltageUnit.Text = "V";
            // 
            // lblNomOutputVoltage
            // 
            this.lblNomOutputVoltage.AutoSize = true;
            this.lblNomOutputVoltage.Location = new System.Drawing.Point(71, 133);
            this.lblNomOutputVoltage.Name = "lblNomOutputVoltage";
            this.lblNomOutputVoltage.Size = new System.Drawing.Size(141, 15);
            this.lblNomOutputVoltage.TabIndex = 10;
            this.lblNomOutputVoltage.Text = "Nominal Output Voltage:";
            // 
            // txtNomOutputVoltage
            // 
            this.txtNomOutputVoltage.Location = new System.Drawing.Point(218, 129);
            this.txtNomOutputVoltage.Name = "txtNomOutputVoltage";
            this.txtNomOutputVoltage.Size = new System.Drawing.Size(116, 23);
            this.txtNomOutputVoltage.TabIndex = 11;
            this.txtNomOutputVoltage.Text = "0";
            this.txtNomOutputVoltage.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtNomOutputVoltage.TextChanged += new System.EventHandler(this.CalculateNominalOutput);
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdCancel.Location = new System.Drawing.Point(284, 244);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(96, 35);
            this.cmdCancel.TabIndex = 17;
            this.cmdCancel.Text = "&Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            // 
            // cmdOK
            // 
            this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdOK.Location = new System.Drawing.Point(181, 244);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(96, 35);
            this.cmdOK.TabIndex = 16;
            this.cmdOK.Text = "&OK";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // grpNominalOutput
            // 
            this.grpNominalOutput.Controls.Add(this.lblNomEfficiencyUnit);
            this.grpNominalOutput.Controls.Add(this.lblNomEfficiency);
            this.grpNominalOutput.Controls.Add(this.txtNomEfficiency);
            this.grpNominalOutput.Controls.Add(this.txtWindingRatioSecondary);
            this.grpNominalOutput.Controls.Add(this.lblOutputResultLabel);
            this.grpNominalOutput.Controls.Add(this.lblOutputResultUnit);
            this.grpNominalOutput.Controls.Add(this.lblOutputResult);
            this.grpNominalOutput.Controls.Add(this.lblConverterType);
            this.grpNominalOutput.Controls.Add(this.cmbConverterType);
            this.grpNominalOutput.Controls.Add(this.picInfoConverterType);
            this.grpNominalOutput.Controls.Add(this.lblNomOutputVoltageUnit);
            this.grpNominalOutput.Controls.Add(this.txtWindingRatioPrimary);
            this.grpNominalOutput.Controls.Add(this.lblNomOutputVoltage);
            this.grpNominalOutput.Controls.Add(this.lblWindingRatio);
            this.grpNominalOutput.Controls.Add(this.txtNomOutputVoltage);
            this.grpNominalOutput.Controls.Add(this.lblWindingRatioUnit);
            this.grpNominalOutput.Controls.Add(this.lblNomInputVoltageUnit);
            this.grpNominalOutput.Controls.Add(this.txtNomInputVoltage);
            this.grpNominalOutput.Controls.Add(this.lblNomInputVoltage);
            this.grpNominalOutput.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpNominalOutput.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpNominalOutput.Location = new System.Drawing.Point(0, 0);
            this.grpNominalOutput.Name = "grpNominalOutput";
            this.grpNominalOutput.Size = new System.Drawing.Size(392, 238);
            this.grpNominalOutput.TabIndex = 0;
            this.grpNominalOutput.TabStop = false;
            this.grpNominalOutput.Text = "Nominal Control Output";
            // 
            // txtWindingRatioSecondary
            // 
            this.txtWindingRatioSecondary.Location = new System.Drawing.Point(287, 69);
            this.txtWindingRatioSecondary.Name = "txtWindingRatioSecondary";
            this.txtWindingRatioSecondary.Size = new System.Drawing.Size(47, 23);
            this.txtWindingRatioSecondary.TabIndex = 6;
            this.txtWindingRatioSecondary.Text = "1";
            this.txtWindingRatioSecondary.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtWindingRatioSecondary.TextChanged += new System.EventHandler(this.CalculateNominalOutput);
            // 
            // lblOutputResultLabel
            // 
            this.lblOutputResultLabel.AutoSize = true;
            this.lblOutputResultLabel.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOutputResultLabel.Location = new System.Drawing.Point(47, 199);
            this.lblOutputResultLabel.Name = "lblOutputResultLabel";
            this.lblOutputResultLabel.Size = new System.Drawing.Size(117, 15);
            this.lblOutputResultLabel.TabIndex = 16;
            this.lblOutputResultLabel.Text = "Nominal Duty Ratio:";
            // 
            // lblOutputResultUnit
            // 
            this.lblOutputResultUnit.AutoSize = true;
            this.lblOutputResultUnit.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOutputResultUnit.Location = new System.Drawing.Point(338, 199);
            this.lblOutputResultUnit.Name = "lblOutputResultUnit";
            this.lblOutputResultUnit.Size = new System.Drawing.Size(16, 15);
            this.lblOutputResultUnit.TabIndex = 18;
            this.lblOutputResultUnit.Text = "%";
            // 
            // lblOutputResult
            // 
            this.lblOutputResult.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOutputResult.Location = new System.Drawing.Point(215, 199);
            this.lblOutputResult.Name = "lblOutputResult";
            this.lblOutputResult.Size = new System.Drawing.Size(120, 17);
            this.lblOutputResult.TabIndex = 17;
            this.lblOutputResult.Text = "0";
            this.lblOutputResult.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblNomEfficiencyUnit
            // 
            this.lblNomEfficiencyUnit.AutoSize = true;
            this.lblNomEfficiencyUnit.Location = new System.Drawing.Point(338, 162);
            this.lblNomEfficiencyUnit.Name = "lblNomEfficiencyUnit";
            this.lblNomEfficiencyUnit.Size = new System.Drawing.Size(16, 15);
            this.lblNomEfficiencyUnit.TabIndex = 15;
            this.lblNomEfficiencyUnit.Text = "%";
            // 
            // lblNomEfficiency
            // 
            this.lblNomEfficiency.AutoSize = true;
            this.lblNomEfficiency.Location = new System.Drawing.Point(96, 162);
            this.lblNomEfficiency.Name = "lblNomEfficiency";
            this.lblNomEfficiency.Size = new System.Drawing.Size(112, 15);
            this.lblNomEfficiency.TabIndex = 13;
            this.lblNomEfficiency.Text = "Nominal Efficiency:";
            // 
            // txtNomEfficiency
            // 
            this.txtNomEfficiency.Location = new System.Drawing.Point(218, 158);
            this.txtNomEfficiency.Name = "txtNomEfficiency";
            this.txtNomEfficiency.Size = new System.Drawing.Size(116, 23);
            this.txtNomEfficiency.TabIndex = 14;
            this.txtNomEfficiency.Text = "0";
            this.txtNomEfficiency.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtNomEfficiency.TextChanged += new System.EventHandler(this.CalculateNominalOutput);
            // 
            // frmGetNominalControlOutput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(392, 286);
            this.Controls.Add(this.grpNominalOutput);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdOK);
            this.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(408, 269);
            this.Name = "frmGetNominalControlOutput";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Nominal Control Output Calculator";
            this.Load += new System.EventHandler(this.frmGetNominalControlOutput_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmGetNominalControlOutput_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.picInfoConverterType)).EndInit();
            this.grpNominalOutput.ResumeLayout(false);
            this.grpNominalOutput.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbConverterType;
        private System.Windows.Forms.PictureBox picInfoConverterType;
        private System.Windows.Forms.Label lblConverterType;
        private System.Windows.Forms.TextBox txtWindingRatioPrimary;
        private System.Windows.Forms.Label lblWindingRatio;
        private System.Windows.Forms.Label lblWindingRatioUnit;
        private System.Windows.Forms.Label lblNomInputVoltageUnit;
        private System.Windows.Forms.Label lblNomInputVoltage;
        private System.Windows.Forms.TextBox txtNomInputVoltage;
        private System.Windows.Forms.Label lblNomOutputVoltageUnit;
        private System.Windows.Forms.Label lblNomOutputVoltage;
        private System.Windows.Forms.TextBox txtNomOutputVoltage;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.GroupBox grpNominalOutput;
        private System.Windows.Forms.Label lblOutputResultLabel;
        private System.Windows.Forms.Label lblOutputResultUnit;
        private System.Windows.Forms.Label lblOutputResult;
        private System.Windows.Forms.TextBox txtWindingRatioSecondary;
        private System.Windows.Forms.Label lblNomEfficiencyUnit;
        private System.Windows.Forms.Label lblNomEfficiency;
        private System.Windows.Forms.TextBox txtNomEfficiency;
    }
}