namespace dcld
{
    partial class frmToolTip
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
            this.components = new System.ComponentModel.Container();
            this.timToolHelp = new System.Windows.Forms.Timer(this.components);
            this.lblToolTipText = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // timToolHelp
            // 
            this.timToolHelp.Interval = 4000;
            this.timToolHelp.Tick += new System.EventHandler(this.timToolHelp_Tick);
            // 
            // lblToolTipText
            // 
            this.lblToolTipText.BackColor = System.Drawing.Color.Transparent;
            this.lblToolTipText.Location = new System.Drawing.Point(12, 9);
            this.lblToolTipText.MaximumSize = new System.Drawing.Size(410, 0);
            this.lblToolTipText.MinimumSize = new System.Drawing.Size(0, 15);
            this.lblToolTipText.Name = "lblToolTipText";
            this.lblToolTipText.Size = new System.Drawing.Size(410, 15);
            this.lblToolTipText.TabIndex = 0;
            this.lblToolTipText.Text = "lblToolTipText";
            this.lblToolTipText.MouseMove += new System.Windows.Forms.MouseEventHandler(this.frmToolTip_KeepVisible);
            // 
            // frmToolTip
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(434, 182);
            this.ControlBox = false;
            this.Controls.Add(this.lblToolTipText);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(450, 400);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(240, 90);
            this.Name = "frmToolTip";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "frmToolTip";
            this.TopMost = true;
            this.Deactivate += new System.EventHandler(this.frmToolTip_Deactivate);
            this.Load += new System.EventHandler(this.frmToolTip_Load);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.frmToolTip_KeepVisible);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timToolHelp;
        private System.Windows.Forms.Label lblToolTipText;

    }
}