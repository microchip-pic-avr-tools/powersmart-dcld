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
            this.lblParam1 = new System.Windows.Forms.Label();
            this.txtParam1 = new System.Windows.Forms.TextBox();
            this.txtParam2 = new System.Windows.Forms.TextBox();
            this.lblParam2 = new System.Windows.Forms.Label();
            this.cmdOK = new System.Windows.Forms.Button();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.lblUnit3 = new System.Windows.Forms.Label();
            this.txtParam3 = new System.Windows.Forms.TextBox();
            this.lblParam3 = new System.Windows.Forms.Label();
            this.lblUnit2 = new System.Windows.Forms.Label();
            this.lblUnit1 = new System.Windows.Forms.Label();
            this.lblControlTarget = new System.Windows.Forms.Label();
            this.cmbControlTarget = new System.Windows.Forms.ComboBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.lblOutputGain = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblParam1
            // 
            this.lblParam1.AutoSize = true;
            this.lblParam1.Location = new System.Drawing.Point(126, 78);
            this.lblParam1.Name = "lblParam1";
            this.lblParam1.Size = new System.Drawing.Size(65, 15);
            this.lblParam1.TabIndex = 0;
            this.lblParam1.Text = "lblParam1";
            this.lblParam1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtParam1
            // 
            this.txtParam1.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtParam1.Location = new System.Drawing.Point(215, 75);
            this.txtParam1.Name = "txtParam1";
            this.txtParam1.Size = new System.Drawing.Size(123, 23);
            this.txtParam1.TabIndex = 1;
            this.txtParam1.Text = "65535";
            this.txtParam1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtParam1.TextChanged += new System.EventHandler(this.CalculateOutputGain);
            // 
            // txtParam2
            // 
            this.txtParam2.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtParam2.Location = new System.Drawing.Point(215, 104);
            this.txtParam2.Name = "txtParam2";
            this.txtParam2.Size = new System.Drawing.Size(123, 23);
            this.txtParam2.TabIndex = 2;
            this.txtParam2.Text = "10000";
            this.txtParam2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtParam2.TextChanged += new System.EventHandler(this.CalculateOutputGain);
            // 
            // lblParam2
            // 
            this.lblParam2.AutoSize = true;
            this.lblParam2.Location = new System.Drawing.Point(126, 106);
            this.lblParam2.Name = "lblParam2";
            this.lblParam2.Size = new System.Drawing.Size(65, 15);
            this.lblParam2.TabIndex = 3;
            this.lblParam2.Text = "lblParam2";
            this.lblParam2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // cmdOK
            // 
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.Location = new System.Drawing.Point(220, 246);
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
            this.cmdCancel.Location = new System.Drawing.Point(308, 246);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(82, 30);
            this.cmdCancel.TabIndex = 5;
            this.cmdCancel.Text = "&Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(1, 2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(414, 242);
            this.tabControl1.TabIndex = 6;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.lblOutputGain);
            this.tabPage1.Controls.Add(this.lblUnit3);
            this.tabPage1.Controls.Add(this.txtParam3);
            this.tabPage1.Controls.Add(this.lblParam3);
            this.tabPage1.Controls.Add(this.lblUnit2);
            this.tabPage1.Controls.Add(this.lblUnit1);
            this.tabPage1.Controls.Add(this.lblControlTarget);
            this.tabPage1.Controls.Add(this.cmbControlTarget);
            this.tabPage1.Controls.Add(this.txtParam2);
            this.tabPage1.Controls.Add(this.lblParam1);
            this.tabPage1.Controls.Add(this.txtParam1);
            this.tabPage1.Controls.Add(this.lblParam2);
            this.tabPage1.Location = new System.Drawing.Point(4, 24);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(406, 214);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // lblUnit3
            // 
            this.lblUnit3.AutoSize = true;
            this.lblUnit3.Location = new System.Drawing.Point(344, 136);
            this.lblUnit3.Name = "lblUnit3";
            this.lblUnit3.Size = new System.Drawing.Size(52, 15);
            this.lblUnit3.TabIndex = 10;
            this.lblUnit3.Text = "lblUnit3";
            // 
            // txtParam3
            // 
            this.txtParam3.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtParam3.Location = new System.Drawing.Point(215, 133);
            this.txtParam3.Name = "txtParam3";
            this.txtParam3.Size = new System.Drawing.Size(123, 23);
            this.txtParam3.TabIndex = 8;
            this.txtParam3.Text = "10000";
            this.txtParam3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtParam3.TextChanged += new System.EventHandler(this.CalculateOutputGain);
            // 
            // lblParam3
            // 
            this.lblParam3.AutoSize = true;
            this.lblParam3.Location = new System.Drawing.Point(126, 135);
            this.lblParam3.Name = "lblParam3";
            this.lblParam3.Size = new System.Drawing.Size(65, 15);
            this.lblParam3.TabIndex = 9;
            this.lblParam3.Text = "lblParam3";
            this.lblParam3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblUnit2
            // 
            this.lblUnit2.AutoSize = true;
            this.lblUnit2.Location = new System.Drawing.Point(344, 107);
            this.lblUnit2.Name = "lblUnit2";
            this.lblUnit2.Size = new System.Drawing.Size(52, 15);
            this.lblUnit2.TabIndex = 7;
            this.lblUnit2.Text = "lblUnit2";
            // 
            // lblUnit1
            // 
            this.lblUnit1.AutoSize = true;
            this.lblUnit1.Location = new System.Drawing.Point(343, 78);
            this.lblUnit1.Name = "lblUnit1";
            this.lblUnit1.Size = new System.Drawing.Size(52, 15);
            this.lblUnit1.TabIndex = 6;
            this.lblUnit1.Text = "lblUnit1";
            // 
            // lblControlTarget
            // 
            this.lblControlTarget.AutoSize = true;
            this.lblControlTarget.Location = new System.Drawing.Point(34, 23);
            this.lblControlTarget.Name = "lblControlTarget";
            this.lblControlTarget.Size = new System.Drawing.Size(92, 15);
            this.lblControlTarget.TabIndex = 5;
            this.lblControlTarget.Text = "Control Output:";
            // 
            // cmbControlTarget
            // 
            this.cmbControlTarget.FormattingEnabled = true;
            this.cmbControlTarget.Items.AddRange(new object[] {
            "1-Duty Cycle at Fixed Frequency",
            "2-Phase Shift at Fixed Frequency",
            "3-Period of Variable Frequency",
            "4-Reference of Inner Control Loop"});
            this.cmbControlTarget.Location = new System.Drawing.Point(140, 20);
            this.cmbControlTarget.Name = "cmbControlTarget";
            this.cmbControlTarget.Size = new System.Drawing.Size(245, 23);
            this.cmbControlTarget.TabIndex = 4;
            this.cmbControlTarget.SelectedIndexChanged += new System.EventHandler(this.cmbControlTarget_SelectedIndexChanged);
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 24);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(406, 214);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // lblOutputGain
            // 
            this.lblOutputGain.AutoSize = true;
            this.lblOutputGain.Enabled = false;
            this.lblOutputGain.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOutputGain.Location = new System.Drawing.Point(300, 176);
            this.lblOutputGain.Name = "lblOutputGain";
            this.lblOutputGain.Size = new System.Drawing.Size(38, 15);
            this.lblOutputGain.TabIndex = 11;
            this.lblOutputGain.Text = "1.000";
            this.lblOutputGain.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Enabled = false;
            this.label1.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(113, 176);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 15);
            this.label1.TabIndex = 12;
            this.label1.Text = "Output Gain:";
            // 
            // frmCalculateOutputGain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(413, 285);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdOK);
            this.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmCalculateOutputGain";
            this.Text = "Code Generation";
            this.Load += new System.EventHandler(this.frmCalculateOutputGain_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblParam1;
        private System.Windows.Forms.TextBox txtParam1;
        private System.Windows.Forms.TextBox txtParam2;
        private System.Windows.Forms.Label lblParam2;
        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Label lblControlTarget;
        private System.Windows.Forms.ComboBox cmbControlTarget;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label lblUnit3;
        private System.Windows.Forms.TextBox txtParam3;
        private System.Windows.Forms.Label lblParam3;
        private System.Windows.Forms.Label lblUnit2;
        private System.Windows.Forms.Label lblUnit1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblOutputGain;
    }
}