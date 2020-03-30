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
    public partial class frmToolTip : Form
    {
        private double _opacity = 1.0;      // Opacity animation variable

        internal string ToolTipText
        {
            get { return (lblToolTipText.Text); }
            set {
                lblToolTipText.AutoSize = true;
                lblToolTipText.Text = value;
//                lblToolTipText.AutoSize = false;
                this.Width = lblToolTipText.Width + 40;
                this.Height = lblToolTipText.Height + 70;
                return; 
            }
        }

        // Form position left
        private Point _win_pos;
        internal Point WinPos 
        {
            get { return (_win_pos); }
            set { _win_pos = value;  this.Location = value; return; }
        }

        // Fade interval in ms
        private int _fadeout_interval = 50;
        internal int FadeoutInterval
        {
            get { return (_fadeout_interval); }
            set { _fadeout_interval = value; return; }
        }

        // Fading ratio in fractions/tick
        private double _fadeout_ratio = 0.020;
        internal double FadeoutRatio
        {
            get { return (_fadeout_ratio); }
            set { _fadeout_ratio = value; return; }
        }

        // Fadeout cut off ratio in fractions at which the form will be unloaded
        private double _fadeout_cutoff = 0.200;
        internal double FadeoutCutOff
        {
            get { return (_fadeout_cutoff); }
            set { _fadeout_cutoff = value; return; }
        }

        // Visible period in ms
        private int _visible_period = 4000;
        internal int VisiblePeriod
        {
            get { return (_visible_period); }
            set { _visible_period = value; return; }
        }


        public frmToolTip()
        {
            InitializeComponent();
        }

        private void frmToolTip_Load(object sender, EventArgs e)
        {
            this.Text = "Help";
            _opacity = 1.0;
            timToolHelp.Interval = _visible_period;
            timToolHelp.Enabled = true;
        }

        private void timToolHelp_Tick(object sender, EventArgs e)
        {

            timToolHelp.Interval = _fadeout_interval;
            this.Opacity = _opacity;

            if ((_opacity -= _fadeout_ratio) < _fadeout_cutoff)
            {
                timToolHelp.Enabled = false;
                this.Close();
            }
        }

        private void frmToolTip_Deactivate(object sender, EventArgs e)
        {
            timToolHelp.Enabled = false;
            this.Close();
        }

        private void frmToolTip_KeepVisible(object sender, MouseEventArgs e)
        {
            timToolHelp.Stop();
            timToolHelp.Enabled = false;
            _opacity = 1.000;
            this.Opacity = _opacity;
            timToolHelp.Interval = _visible_period;
            timToolHelp.Enabled = true;
            timToolHelp.Start();
        }


    }
}
