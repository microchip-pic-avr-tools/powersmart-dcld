using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;

namespace dcld
{
    public partial class frmCalculateOutputGain : Form
    {
        public double _OutputGain = 1.000;
        public double OutputGain
        {
            get { return (_OutputGain); }
            set { _OutputGain = value; return; }
        }

        private double _PWMFrequency = 0.0;
        public double PWMFrequency
        {
            get { return (_PWMFrequency); }
            set { _PWMFrequency = value; return; }
        }

        public frmCalculateOutputGain()
        {
            InitializeComponent();
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            _OutputGain = Convert.ToDouble(lblOutputGain.Text);
            this.Close();
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmCalculateOutputGain_Load(object sender, EventArgs e)
        {
            cmbControlTarget.SelectedIndex = 0;
        }

        private void cmbControlTarget_SelectedIndexChanged(object sender, EventArgs e)
        {

            switch (cmbControlTarget.SelectedIndex)
            {
                case 0:

                    lblParam1.Text = "PWM Frequency:";
                    txtParam1.Text = _PWMFrequency.ToString();
                    lblUnit1.Text = "kHz";

                    lblParam1.Visible = true;
                    txtParam1.Visible = true;
                    lblUnit1.Visible = true;

                    lblParam2.Text = "PWM Resolution:";
                    txtParam2.Text = "0.250";
                    lblUnit2.Text = "ns";

                    lblParam2.Visible = true;
                    txtParam2.Visible = true;
                    lblUnit2.Visible = true;


                    lblParam3.Visible = false;
                    txtParam3.Visible = false;
                    lblUnit3.Visible = false;

                    break;

                case 1:

                    lblParam1.Text = "PWM Frequency:";
                    txtParam1.Text = _PWMFrequency.ToString();
                    lblUnit1.Text = "kHz";

                    lblParam1.Visible = true;
                    txtParam1.Visible = true;
                    lblUnit1.Visible = true;

                    lblParam2.Text = "PWM Resolution:";
                    txtParam2.Text = "0.250";
                    lblUnit2.Text = "ns";

                    lblParam2.Visible = true;
                    txtParam2.Visible = true;
                    lblUnit2.Visible = true;


                    lblParam3.Visible = false;
                    txtParam3.Visible = false;
                    lblUnit3.Visible = false;

                    break;

                case 2:

                    lblParam1.Text = "Minimum PWM Frequency:";
                    txtParam1.Text = _PWMFrequency.ToString();
                    lblUnit1.Text = "kHz";

                    lblParam1.Visible = true;
                    txtParam1.Visible = true;
                    lblUnit1.Visible = true;

                    lblParam2.Text = "PWM Resolution:";
                    txtParam2.Text = "0.250";
                    lblUnit2.Text = "ns";

                    lblParam2.Visible = true;
                    txtParam2.Visible = true;
                    lblUnit2.Visible = true;


                    lblParam3.Visible = false;
                    txtParam3.Visible = false;
                    lblUnit3.Visible = false;

                    break;

                case 3:

                    lblParam1.Text = "Feedback Resolution:";
                    txtParam1.Text = "12";
                    lblUnit1.Text = "bit";

                    lblParam1.Visible = true;
                    txtParam1.Visible = true;
                    lblUnit1.Visible = true;

                    lblParam2.Visible = false;
                    txtParam2.Visible = false;
                    lblUnit2.Visible = false;


                    lblParam3.Visible = false;
                    txtParam3.Visible = false;
                    lblUnit3.Visible = false;

                    break;
            
            }

            lblParam1.Left = (txtParam1.Left - lblParam1.Width - 24);
            lblParam2.Left = (txtParam2.Left - lblParam2.Width - 24);
            lblParam3.Left = (txtParam3.Left - lblParam3.Width - 24);

            CalculateOutputGain(sender, e);

        }

        private void CalculateOutputGain(object sender, EventArgs e) 
        {

            double ddum = 0.0;

            switch (cmbControlTarget.SelectedIndex)
            {
                case 0:
                    ddum = (1.0 / (Convert.ToDouble(txtParam1.Text) * 1000)); // PWM Period in [sec]
                    ddum = (ddum / (Convert.ToDouble(txtParam2.Text) * Convert.ToDouble(1e-9))); // PWM Period in [ticks]
                    ddum = (ddum / Math.Pow(2.0, 16.0));
                    break;

                case 1:
                    ddum = (1.0 / (Convert.ToDouble(txtParam1.Text) * 1000)); // PWM Period in [sec]
                    ddum = ((ddum / (Convert.ToDouble(txtParam2.Text) * Convert.ToDouble(1e-9))) / 2.0); // PWM Period in [ticks]
                    ddum = (ddum / Math.Pow(2.0, 16.0));
                    break;

                case 2:
                    ddum = (1.0 / (Convert.ToDouble(txtParam1.Text) * 1000)); // PWM Period in [sec]
                    ddum = (ddum / (Convert.ToDouble(txtParam2.Text) * Convert.ToDouble(1e-9))); // PWM Period in [ticks]
                    ddum = (ddum / Math.Pow(2.0, 16.0));
                    break;

                case 3:
                    ddum = Math.Pow(2.0, Convert.ToDouble(txtParam1.Text));
                    ddum = (ddum / Math.Pow(2.0, 16.0));
                    break;
                
                default:
                    ddum = 1.000;
                    break;

            }

            lblOutputGain.Text = ddum.ToString("#0.000000", CultureInfo.CurrentCulture);

        }

        
    }
}
