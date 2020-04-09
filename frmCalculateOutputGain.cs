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
        bool ParameterUpdate = false;
        internal clsOutputDeclaration output = new clsOutputDeclaration();
        internal bool EnableNominalControlEdits = false;


        //internal double OutputGain
        //{
        //    get { return (output.Gain); }
        //}

        //internal string DeviceTypeName
        //{
        //    get { return (output.DeviceType); }
        //    set { output.DeviceType = value; return; }
        //}
        
        //internal double PWMFrequency
        //{
        //    get { return (output.PWMFrequency); }
        //    set { output.PWMFrequency = value; return; }
        //}

        //internal double PWMDutyCycle
        //{
        //    get { return (output.PWMDutyCycle); }
        //    set { output.PWMDutyCycle = value; return; }
        //}

        //internal double PWMClock
        //{
        //    get { return (output.PWMClock); }
        //    set { output.PWMClock = value; return; }
        //}

        //internal double PWMClockDivider
        //{
        //    get { return (output.PWMClockDivider); }
        //    set { output.PWMClockDivider = value; return; }
        //}

        //internal Int64 PWMCount
        //{
        //    get { return (output.PWMCount); }
        //}

        public frmCalculateOutputGain()
        {
            InitializeComponent();
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmCalculateOutputGain_Load(object sender, EventArgs e)
        {
            // Initialize controls
            tabPWMMode_SelectedIndexChanged(this, e);
            cmbDeviceType.SelectedIndex = (int)(output.DeviceType-1);

            // Set Window Startup Position
            this.StartPosition = FormStartPosition.CenterParent;

            // Enable Key Preview for Hot Key support
            this.KeyPreview = true;
        }

        private void tabPWMMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            

            if (tabPWMMode.SelectedTab == tabFixedFrequency) 
            {

                output.OutputType = clsOutputDeclaration.dcldOutputType.DCLD_OUT_TYPE_FIXED_FREQUENCY;

                lblNominalOutputValue.Text = "Nominal Duty Ratio:";
                txtNominalOutputValue.Text = (100.0 * output.PWMDutyCycle).ToString(CultureInfo.CurrentCulture);

                lblParam1.Text = "PWM Frequency:";
                txtParam1.Text =output.PWMFrequency.ToString();
                NumberTextBox_ToString(txtParam1);
                lblUnit1.Text = "Hz";

                lblParam2.Text = "PWM Period:";
                txtParam2.Text = output.PWMPeriod.ToString();
                NumberTextBox_ToString(txtParam2);
                lblUnit2.Text = "sec";

                lblParam3.Text = "PWM Period Count:";
                txtParam3.Text = output.PWMPeriodCount.ToString(CultureInfo.CurrentCulture);
                lblUnit3.Text = "ticks";

                txtParam1.Enabled = true;
                txtParam2.Enabled = false;
                txtParam3.Enabled = false;
                txtParam4.Enabled = false;

                txtParam1.Visible = true;
                txtParam2.Visible = true;
                txtParam3.Visible = true;
                txtParam4.Visible = true;

                // Show/Hide additional input field
                lblNominalOutputValue.Enabled = EnableNominalControlEdits;
                txtNominalOutputValue.Enabled = EnableNominalControlEdits;
                lblNominalOutputValueUnit.Enabled = EnableNominalControlEdits;
                lblNominalOutputValue.Visible = EnableNominalControlEdits;
                txtNominalOutputValue.Visible = EnableNominalControlEdits;
                lblNominalOutputValueUnit.Visible = EnableNominalControlEdits;
                cmdGetPTermNominalOutput.Enabled = EnableNominalControlEdits;
                cmdGetPTermNominalOutput.Visible = EnableNominalControlEdits;

            }
            else if (tabPWMMode.SelectedTab == tabPhaseShifted) 
            {

                    output.OutputType = clsOutputDeclaration.dcldOutputType.DCLD_OUT_TYPE_PHASE_SHIFTED_PWM;

                    lblNominalOutputValue.Text = "Phase-Shift Ratio:";
                    lblNominalOutputValueUnit.Text = "%";
                    
                    lblParam1.Text = "PWM Frequency:";
                    txtParam1.Text = output.PWMFrequency.ToString();
                    NumberTextBox_ToString(txtParam1);
                    lblUnit1.Text = "Hz";

                    lblParam2.Text = "PWM Period:";
                    txtParam2.Text = output.PWMPeriod.ToString();
                    NumberTextBox_ToString(txtParam2);
                    lblUnit2.Text = "sec";

                    lblParam3.Text = "PWM Period Count:";
                    txtParam3.Text = output.PWMPeriodCount.ToString(CultureInfo.CurrentCulture);
                    lblUnit3.Text = "ticks";

                    txtParam1.Enabled = true;
                    txtParam2.Enabled = false;
                    txtParam3.Enabled = false;
                    txtParam4.Enabled = false;

                    txtParam1.Visible = true;
                    txtParam2.Visible = true;
                    txtParam3.Visible = true;
                    txtParam4.Visible = true;

                    // Show/Hide additional input field
                    lblNominalOutputValue.Enabled = EnableNominalControlEdits;
                    txtNominalOutputValue.Enabled = EnableNominalControlEdits;
                    lblNominalOutputValueUnit.Enabled = EnableNominalControlEdits;
                    lblNominalOutputValue.Visible = EnableNominalControlEdits;
                    txtNominalOutputValue.Visible = EnableNominalControlEdits;
                    lblNominalOutputValueUnit.Visible = EnableNominalControlEdits;
                    cmdGetPTermNominalOutput.Enabled = false;
                    cmdGetPTermNominalOutput.Visible = false;

            }
            
            else if (tabPWMMode.SelectedTab == tabVariableFrequency) 
            {

                output.OutputType = clsOutputDeclaration.dcldOutputType.DCLD_OUT_TYPE_VARIABLE_FREQUENCY;

                lblNominalOutputValue.Text = "Load Ratio:";
                lblNominalOutputValueUnit.Text = "%";

                if (!EnableNominalControlEdits)
                    lblParam1.Text = "Min. PWM Frequency:";
                else
                    lblParam1.Text = "Nominal PWM Frequency:";

                txtParam1.Text = output.PWMFrequency.ToString();
                NumberTextBox_ToString(txtParam1);
                lblUnit1.Text = "Hz";

                lblParam2.Text = "PWM Period:";
                txtParam2.Text = output.PWMPeriod.ToString();
                NumberTextBox_ToString(txtParam2);
                lblUnit2.Text = "sec";

                lblParam3.Text = "PWM Period Count:";
                txtParam3.Text = output.PWMPeriodCount.ToString(CultureInfo.CurrentCulture);
                lblUnit3.Text = "ticks";

                txtParam1.Enabled = true;
                txtParam2.Enabled = false;
                txtParam3.Enabled = false;
                txtParam4.Enabled = false;

                txtParam1.Visible = true;
                txtParam2.Visible = true;
                txtParam3.Visible = true;
                txtParam4.Visible = true;

                // Show/Hide additional input field
                lblNominalOutputValue.Enabled = false;
                txtNominalOutputValue.Enabled = false;
                lblNominalOutputValueUnit.Enabled = false;
                lblNominalOutputValue.Visible = false;
                txtNominalOutputValue.Visible = false;
                lblNominalOutputValueUnit.Visible = false;
                cmdGetPTermNominalOutput.Enabled = false;
                cmdGetPTermNominalOutput.Visible = false;

            }

            else 
            {
                output.OutputType = clsOutputDeclaration.dcldOutputType.DCLD_OUT_TYPE_UNDEFINED;

                lblParam1.Text = "Feedback Resolution:";
                txtParam1.Text = "12";
                lblUnit1.Text = "bit";

                txtParam1.Visible = true;
                txtParam2.Visible = false;
                txtParam3.Visible = false;
                txtParam4.Visible = true;

            }

            lblParam1.Left = (txtParam1.Left - lblParam1.Width - 6);
            lblParam2.Left = (txtParam2.Left - lblParam2.Width - 6);
            lblParam3.Left = (txtParam3.Left - lblParam3.Width - 6);
            lblParam4.Left = (txtParam4.Left - lblParam4.Width - 6);
            lblNominalOutputValue.Left = (txtNominalOutputValue.Left - lblNominalOutputValue.Width - 6);

            lblParam1.Enabled = txtParam1.Enabled;
            lblParam2.Enabled = txtParam2.Enabled;
            lblParam3.Enabled = txtParam3.Enabled;
            lblParam4.Enabled = txtParam4.Enabled;

            lblUnit1.Enabled = txtParam1.Enabled;
            lblUnit2.Enabled = txtParam2.Enabled;
            lblUnit3.Enabled = txtParam3.Enabled;
            lblUnit4.Enabled = txtParam4.Enabled;

            lblParam1.Visible = txtParam1.Visible;
            lblParam2.Visible = txtParam2.Visible;
            lblParam3.Visible = txtParam3.Visible;
            lblParam4.Visible = txtParam4.Visible;

            lblUnit1.Visible = txtParam1.Visible;
            lblUnit2.Visible = txtParam2.Visible;
            lblUnit3.Visible = txtParam3.Visible;
            lblUnit4.Visible = txtParam4.Visible;

            CalculateOutputGain(sender, e);

        }

        private void CalculateOutputGain(object sender, EventArgs e) 
        {

            double ddum = 0.0;
            System.Windows.Forms.TextBox tBox = new TextBox();

            if (ParameterUpdate) return;
            ParameterUpdate = true;

            // Capture calling control
            if (sender.GetType().ToString() == "System.Windows.Forms.TextBox")
            { tBox = (System.Windows.Forms.TextBox)sender; }

            try
            {
                // Set parameters
                output.PWMClock = NumberTextBox_ToDouble(txtSourceClock);
                output.PWMClockDivider = Convert.ToInt32(NumberTextBox_ToDouble(txtClockDivider));

                txtResolution.Text = output.PWMResolution.ToString(CultureInfo.CurrentCulture);
                NumberTextBox_ToString(txtResolution);
                txtMaximum.Text = (Math.Pow(2.0, output.PWMBitResolution)-1).ToString(CultureInfo.CurrentCulture);


                if (tabPWMMode.SelectedTab == tabFixedFrequency)
                {
                    output.PWMFrequency = NumberTextBox_ToDouble(txtParam1);
                }

                else if (tabPWMMode.SelectedTab == tabPhaseShifted)
                {
                    output.PWMFrequency = NumberTextBox_ToDouble(txtParam1);
                }

                else if (tabPWMMode.SelectedTab == tabVariableFrequency)
                {
                    if (!EnableNominalControlEdits)
                    { // always calcualte maximum period/minimum frequency
                        txtParam1.Text = (1.0 / (output.PWMResolution * Math.Pow(2.0, output.PWMBitResolution) * output.PWMClockDivider)).ToString(CultureInfo.CurrentCulture);
                        NumberTextBox_ToString(txtParam1);
                    }

                    // Update period
                    output.PWMFrequency = NumberTextBox_ToDouble(txtParam1);
                }

                else
                { output.PWMFrequency = NumberTextBox_ToDouble(txtParam1); }

                // Update period, period count and effective resolution
                txtParam2.Text = output.PWMPeriod.ToString(CultureInfo.CurrentCulture);
                txtParam3.Text = output.PWMPeriodCount.ToString(CultureInfo.CurrentCulture);
                txtParam4.Text = Math.Log(output.PWMPeriodCount, 2.0).ToString("#0.00#", CultureInfo.CurrentCulture);

                NumberTextBox_ToString(txtParam2);
                NumberTextBox_ToString(txtParam3);
                NumberTextBox_ToString(txtParam4);

                // Format Text Box in standard color
                tBox.BackColor = SystemColors.Window;
                ddum = output.Gain;
            }
            catch
            {
                // Format Text Box in ERROR color
                tBox.BackColor = Color.LightCoral;
                ddum = 1.000;

            }

            ParameterUpdate = false;
            lblOutputGain.Text = ddum.ToString("#0.000000", CultureInfo.CurrentCulture);

        }

        

        private void NumberTextBox_KeyDownWithScaling(object sender, KeyEventArgs e)
        {
            string tval = "";
            TextBox tBox;

            if (
                ((e.KeyValue == 84) && (e.Shift)) || ((e.KeyValue == 71) && (e.Shift)) || ((e.KeyValue == 77) && (e.Shift)) ||     // allow scaling using T=Tera, G=Giga, M=Mega, ...
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

                        if ((e.KeyValue == 84) && (e.Shift)) tval = tval + " T";   // Tera
                        else if ((e.KeyValue == 71) && (e.Shift)) tval = tval + " G";   // Giga
                        else if ((e.KeyValue == 77) && (e.Shift)) tval = tval + " M";   // Mega
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
                else if (tval.Contains("G")) { dval = (1e+9); tval = tval.Replace("G", "").Trim(); }   // Giga
                else if (tval.Contains("M")) { dval = (1e+6); tval = tval.Replace("M", "").Trim(); }   // Mega
                else if (tval.Contains("k")) { dval = (1e+3); tval = tval.Replace("k", "").Trim(); }   // Kilo
                else if (tval.Contains("m")) { dval = (1e-3); tval = tval.Replace("m", "").Trim(); }   // Milli
                else if (tval.Contains("u")) { dval = (1e-6); tval = tval.Replace("u", "").Trim(); }   // Mikro
                else if (tval.Contains("n")) { dval = (1e-9); tval = tval.Replace("n", "").Trim(); }   // Nano
                else if (tval.Contains("p")) { dval = (1e-12); tval = tval.Replace("p", "").Trim(); }   // Piko
                else if (tval.Contains("f")) { dval = (1e-15); tval = tval.Replace("f", "").Trim(); }   // Femto
                else { dval = (1e+0); }

                try { dblres = Convert.ToDouble(tval) * dval; }
                catch
                { dblres = 1.0; }

            }

            return (dblres);
        }

        private string NumberTextBox_ToString(TextBox tBoxToValidate)
        {
            System.Windows.Forms.TextBox tBox;
            string strres = "";
            double ddum = 0.0;

            if (tBoxToValidate.GetType().ToString() == "System.Windows.Forms.TextBox")
            {
                tBox = (System.Windows.Forms.TextBox)tBoxToValidate;

                try
                {
                    ddum = Convert.ToDouble(tBox.Text);

                    if (ddum < (1e-12)) strres = (ddum / (1e-15)).ToString("#0.0##f", CultureInfo.CurrentCulture);
                    else if (ddum < (1e-9)) strres = (ddum / (1e-12)).ToString("#0.0##p", CultureInfo.CurrentCulture);
                    else if (ddum < (1e-6)) strres = (ddum / (1e-9)).ToString("#0.0##n", CultureInfo.CurrentCulture);
                    else if (ddum < (1e-3)) strres = (ddum / (1e-6)).ToString("#0.0##u", CultureInfo.CurrentCulture);
                    else if (ddum < (1e-0)) strres = (ddum / (1e-3)).ToString("#0.0##m", CultureInfo.CurrentCulture);
                    else if (ddum > (1e+3)) strres = (ddum / (1e+3)).ToString("#0.0##k", CultureInfo.CurrentCulture);
                    else if (ddum > (1e+6)) strres = (ddum / (1e+6)).ToString("#0.0##M", CultureInfo.CurrentCulture);
                    else if (ddum > (1e+9)) strres = (ddum / (1e+9)).ToString("#0.0##G", CultureInfo.CurrentCulture);
                    else if (ddum > (1e+12)) strres = (ddum / (1e+12)).ToString("#0.0##T", CultureInfo.CurrentCulture);
                    else strres = ddum.ToString("#0.0##", CultureInfo.CurrentCulture);

                    tBox.Text = strres.Trim();
                }
                catch
                { }
            }

            return (strres.Trim());
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

        private void frmCalculateOutputGain_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Escape)
                this.Close();
            return;
        }

        private void txtNominalOutputValue_TextChanged(object sender, EventArgs e)
        {
            double dbl_dum = 0.0;

            try
            {
                dbl_dum = Convert.ToDouble(txtNominalOutputValue.Text);
                if (dbl_dum > 100.0) dbl_dum = 100.0;
                if (dbl_dum < 0.0) dbl_dum = 0.0;
                string _str_dum = dbl_dum.ToString(CultureInfo.CurrentCulture);
                if (_str_dum.Length > 5)
                {
                    _str_dum = Math.Round(dbl_dum, 3).ToString(CultureInfo.CurrentCulture);
                    txtNominalOutputValue.Text = _str_dum; return; // This function will call itself here
                }

                output.PWMDutyCycle = (dbl_dum / 100.0);
                txtNominalOutputValue.BackColor = SystemColors.Window;
            }
            catch
            {
                txtNominalOutputValue.BackColor = Color.LightCoral;
            }
            
        }

        private void cmdGetPTermNominalOutput_Click(object sender, EventArgs e)
        {
            double _dbl_output = 0.0;
            string _str_dum = "";

            // Create a new instance of the form class
            frmGetNominalControlOutput frm = new frmGetNominalControlOutput();

            frm.output = output;

            if (frm.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {

                switch(output.OutputType)
                {
                    case clsOutputDeclaration.dcldOutputType.DCLD_OUT_TYPE_FIXED_FREQUENCY: 
                    // In fixed frequency converters the Duty Cycle is the control value
                    
                        if (((!Double.IsNaN(frm.output.PWMDutyCycle)) && (!Double.IsInfinity(frm.output.PWMDutyCycle))))
                            _dbl_output = (frm.output.PWMDutyCycle * 100.0);
                        else
                            _dbl_output = Double.NaN;
                        break;

                    case clsOutputDeclaration.dcldOutputType.DCLD_OUT_TYPE_VARIABLE_FREQUENCY:
                        // In variable frequency converters the Period is the control value

                        if (((!Double.IsNaN(frm.output.PWMPeriodCount)) && (!Double.IsInfinity(frm.output.PWMPeriodCount))))
                            _dbl_output = frm.output.PWMPeriodCount;
                        else
                            _dbl_output = Double.NaN;
                        break;
                
                }

                _str_dum = _dbl_output.ToString(CultureInfo.CurrentCulture);
                if (_str_dum.Length > 6) _str_dum = Math.Round(_dbl_output, 6).ToString(CultureInfo.CurrentCulture);
                txtNominalOutputValue.Text = _str_dum;

                // If result is not valid, mark back color
                if (Double.IsNaN(frm.output.NominalOutput))
                {
                    txtNominalOutputValue.BackColor = Color.LightCoral;
                }

                else
                {
                    output = frm.output;
                    txtNominalOutputValue.BackColor = SystemColors.Window;
                }
                    

            }

        }
        
    }

}
