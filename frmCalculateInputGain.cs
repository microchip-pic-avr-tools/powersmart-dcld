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
    public partial class frmCalculateInputGain : Form
    {

        // Private Variables
        private bool ParameterUpdate = false;
        internal clsFeedbackDeclaration feedback = new clsFeedbackDeclaration();


        public frmCalculateInputGain()
        {
            InitializeComponent();
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            feedback.FeedbackGain = Convert.ToDouble(lblInputGain.Text);
            this.Close();
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmCalculateInputGain_Load(object sender, EventArgs e)
        {
            txtInputResolution.Text = feedback.ADCResolution.ToString(CultureInfo.CurrentCulture);
            tabFeedback_SelectedIndexChanged(sender, e);
        }

        private void tabFeedback_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Guarding condition start
            ParameterUpdate = true;

            switch (tabFeedback.SelectedIndex)
            {
                case 0: // Voltage Divider

                    feedback.FeedbackType = clsFeedbackDeclaration.dcldFeedbackType.DCLD_FB_TYPE_VOLTAGE_DIVIDER;

                    txtParam0.Text = feedback.VoltageDividerSourceMaximum.ToString("#0.###", CultureInfo.CurrentCulture);
                    txtParam1.Text = feedback.VoltageDividerR1.ToString("#0.###", CultureInfo.CurrentCulture);
                    txtParam2.Text = feedback.VoltageDividerR2.ToString("#0.###", CultureInfo.CurrentCulture);
                    txtParam3.Text = feedback.VoltageDividerAmplifierGain.ToString("#0.0##", CultureInfo.CurrentCulture);

                    lblInputResolution.Text = "ADC Resolution";
                    lblInputResolution.Visible = true;
                    lblInputReference.Text = "ADC Reference";
                    txtInputReference.Visible = true;
                    lblInputReference.Visible = true;
                    lblInputReferenceUnit.Visible = true;

                    lblParam0Label.Text = "Maximum Input:";
                    lblParam0Unit.Text = "V";
                    lblParam0Unit.Font = this.Font;

                    lblParam1Label.Text = "R1:";
                    lblParam1Unit.Text = "W";
                    lblParam1Unit.Font = new Font("Symbol", 10);

                    lblParam2Label.Text = "R2:";
                    lblParam2Unit.Text = "W";
                    lblParam2Unit.Font = new Font("Symbol", 10);

                    lblParam3Label.Text = "Amplifier Gain:";
                    lblParam3Unit.Text = "V/V";
                    lblParam3Unit.Font = this.Font;

                    txtParam2.Visible = true;
                    lblParam2Label.Visible = true;
                    lblParam2Unit.Visible = true;

                    txtParam3.Visible = true;
                    lblParam3Label.Visible = true;
                    lblParam3Unit.Visible = true;

                    lblInputGainUnit.Text = "V/V";

                    break;

                case 1: // Shunt Amplifier

                    feedback.FeedbackType = clsFeedbackDeclaration.dcldFeedbackType.DCLD_FB_TYPE_SHUNT_AMPLIFIER;

                    txtParam0.Text = feedback.CurrentSenseSourceMaximum.ToString("#0.###", CultureInfo.CurrentCulture);
                    txtParam1.Text = feedback.CurrentSenseRshunt.ToString("#0.0##", CultureInfo.CurrentCulture);
                    txtParam2.Text = feedback.CurrentSenseAmplifierGain.ToString("#0.0##", CultureInfo.CurrentCulture);

                    lblInputResolution.Text = "ADC Resolution";
                    lblInputResolution.Visible = true;
                    lblInputReference.Text = "ADC Reference";
                    txtInputReference.Visible = true;
                    lblInputReference.Visible = true;
                    lblInputReferenceUnit.Visible = true;

                    lblParam0Label.Text = "Maximum Input:";
                    lblParam0Unit.Text = "A";
                    lblParam0Unit.Font = this.Font;

                    lblParam1Label.Text = "Shunt Resistance:";
                    lblParam1Unit.Text = "W";
                    lblParam1Unit.Font = new Font("Symbol", 10);

                    lblParam2Label.Text = "Amplifier Gain:";
                    lblParam2Unit.Text = "V/V";
                    lblParam2Unit.Font = this.Font;

                    txtParam2.Visible = true;
                    lblParam2Label.Visible = true;
                    lblParam2Unit.Visible = true;

                    txtParam3.Visible = false;
                    lblParam3Label.Visible = false;
                    lblParam3Unit.Visible = false;
                    
                    lblInputGainUnit.Text = "V/A";

                    break;

                case 2: // Current Sense Transformer

                    feedback.FeedbackType = clsFeedbackDeclaration.dcldFeedbackType.DCLD_FB_TYPE_CURRENT_TRANSFORMER;

                    txtParam0.Text = feedback.CurrentTransformerSourceMaximum.ToString("#0.###", CultureInfo.CurrentCulture);
                    txtParam1.Text = feedback.CurrentTransformerBurdenResistance.ToString("#0.0##", CultureInfo.CurrentCulture);
                    txtParam2.Text = feedback.CurrentTransformerWindingRatio.ToString("#0.###", CultureInfo.CurrentCulture);

                    lblInputResolution.Text = "ADC Resolution";
                    lblInputResolution.Visible = true;
                    lblInputReference.Text = "ADC Reference";
                    txtInputReference.Visible = true;
                    lblInputReference.Visible = true;
                    lblInputReferenceUnit.Visible = true;

                    lblParam0Label.Text = "Maximum Input:";
                    lblParam0Unit.Text = "A";
                    lblParam0Unit.Font = this.Font;

                    lblParam1Label.Text = "Burden Resistance:";
                    lblParam1Unit.Text = "W";
                    lblParam1Unit.Font = new Font("Symbol", 10);

                    lblParam2Label.Text = "Winding Ratio [P/S] = 1:n";
                    lblParam2Unit.Text = "";
                    lblParam2Unit.Font = this.Font;

                    txtParam2.Visible = true;
                    lblParam2Label.Visible = true;
                    lblParam2Unit.Visible = true;

                    txtParam3.Visible = false;
                    lblParam3Label.Visible = false;
                    lblParam3Unit.Visible = false;
                    
                    lblInputGainUnit.Text = "V/A";

                    break;

                case 3: // Digital Soruce

                    feedback.FeedbackType = clsFeedbackDeclaration.dcldFeedbackType.DCLD_FB_TYPE_DIGITAL_SOURCE;

                    txtParam0.Text = feedback.DigitalSourceSourceMaximum.ToString("#0.###", CultureInfo.CurrentCulture);
                    txtParam1.Text = feedback.DigitalSourceResolution.ToString("#0.0##", CultureInfo.CurrentCulture);

                    lblInputResolution.Text = "Input Resolution";
                    lblInputResolution.Visible = true;
                    lblInputReference.Text = "Input Reference";
                    txtInputReference.Visible = false;
                    lblInputReference.Visible = false;
                    lblInputReferenceUnit.Visible = false;

                    lblParam0Label.Text = "Maximum Input:";
                    lblParam0Unit.Text = "";
                    lblParam0Unit.Font = this.Font;

                    lblParam1Label.Text = "Value Resolution:";
                    lblParam1Unit.Text = "bit";
                    lblParam1Unit.Font = this.Font;

                    lblParam2Label.Text = "Maximum Input Value:";
                    lblParam2Unit.Text = "";
                    lblParam2Unit.Font = this.Font;

                    txtParam2.Visible = false;
                    lblParam2Label.Visible = false;
                    lblParam2Unit.Visible = false;

                    txtParam3.Visible = false;
                    lblParam3Label.Visible = false;
                    lblParam3Unit.Visible = false;

                    lblInputGainUnit.Text = "tick/tick";

                    break;

                default:

                    feedback.FeedbackType = clsFeedbackDeclaration.dcldFeedbackType.DCLD_FB_TYPE_UNDEFINED;
                    break;
            }

            // Adjust label positions
            lblInputReference.Left = (txtInputReference.Left - lblInputReference.Width-6);
            lblInputResolution.Left = (txtInputResolution.Left - lblInputResolution.Width - 6);

            lblParam0Label.Left = (txtParam0.Left - lblParam0Label.Width - 6);
            lblParam1Label.Left = (txtParam1.Left - lblParam1Label.Width - 6);
            lblParam2Label.Left = (txtParam2.Left - lblParam2Label.Width - 6);
            lblParam3Label.Left = (txtParam3.Left - lblParam3Label.Width - 6);

            lblParam0Unit.Left = (txtParam0.Left + txtParam0.Width + 6);
            lblParam1Unit.Left = (txtParam1.Left + txtParam1.Width + 6);
            lblParam2Unit.Left = lblParam1Unit.Left;
            lblParam3Unit.Left = lblParam1Unit.Left;

            // Format Test Box values
            if (tabFeedback.SelectedIndex != 3)
            { 
                NumberTextBox_ToString(txtParam0);
                NumberTextBox_ToString(txtParam1);
                NumberTextBox_ToString(txtParam2);
                NumberTextBox_ToString(txtParam3);
            }

            // Guarding condition end
            ParameterUpdate = false;

            // Run calculation
            CalculateInputGain(sender, e);
            
        }

        private void CalculateInputGain(object sender, EventArgs e) 
        {

            double ddum = 0.0;
            System.Windows.Forms.TextBox tBox = new TextBox();

            // Guarding condition start
            if (ParameterUpdate) return;
            ParameterUpdate = true;

            // Capture calling control
            if (sender.GetType().ToString() == "System.Windows.Forms.TextBox")
            { tBox = (System.Windows.Forms.TextBox)sender; }

            // Calculate input gain

            try 
            {

                // Calculate ADC/Input Resolution and Granularity
                feedback.ADCIsDifferential = chkInputSigned.Checked;
                feedback.ADCReference = Convert.ToDouble(txtInputReference.Text);
                feedback.ADCResolution = Convert.ToDouble(txtInputResolution.Text);

                // Set Parameters
                switch (tabFeedback.SelectedIndex)
                {
                    case 0: // Voltage Divider

                        feedback.FeedbackType = clsFeedbackDeclaration.dcldFeedbackType.DCLD_FB_TYPE_VOLTAGE_DIVIDER;

                        feedback.VoltageDividerR1 = NumberTextBox_ToDouble(txtParam1);
                        feedback.VoltageDividerR2 = NumberTextBox_ToDouble(txtParam2);
                        feedback.VoltageDividerAmplifierGain = NumberTextBox_ToDouble(txtParam3);

                        // Update Maximum Value
                        txtParam0.Text = feedback.VoltageDividerSourceMaximum.ToString("#0.0##", CultureInfo.CurrentCulture);
                        break;

                    case 1: // Shunt Amplifier

                        feedback.FeedbackType = clsFeedbackDeclaration.dcldFeedbackType.DCLD_FB_TYPE_SHUNT_AMPLIFIER;

                        feedback.CurrentSenseRshunt = NumberTextBox_ToDouble(txtParam1);
                        feedback.CurrentSenseAmplifierGain = NumberTextBox_ToDouble(txtParam2);

                        // Update Maximum Value
                        txtParam0.Text = feedback.CurrentSenseSourceMaximum.ToString("#0.0##", CultureInfo.CurrentCulture);
                        break;

                    case 2: // Current Sense Transformer

                        feedback.FeedbackType = clsFeedbackDeclaration.dcldFeedbackType.DCLD_FB_TYPE_CURRENT_TRANSFORMER;

                        feedback.CurrentTransformerBurdenResistance = NumberTextBox_ToDouble(txtParam1);
                        feedback.CurrentTransformerWindingRatio = NumberTextBox_ToDouble(txtParam2);

                        // Update Maximum Value
                        txtParam0.Text = feedback.CurrentTransformerSourceMaximum.ToString("#0.0##", CultureInfo.CurrentCulture);
                        break;

                    case 3: // Digital Source

                        feedback.FeedbackType = clsFeedbackDeclaration.dcldFeedbackType.DCLD_FB_TYPE_DIGITAL_SOURCE;

                        feedback.DigitalSourceResolution = NumberTextBox_ToDouble(txtParam1);

                        // Update Maximum Value
                        txtParam0.Text = feedback.DigitalSourceSourceMaximum.ToString("#0.0##", CultureInfo.CurrentCulture);
                        break;

                    default: // Undefined
                        return;
                }

                // Format Text Box in standard color
                tBox.BackColor = SystemColors.Window;
                ddum = feedback.FeedbackGain;
            }
            catch
            {
                // Format Text Box in ERROR color
                tBox.BackColor = Color.LightCoral;
                ddum = 1.000;

            }

            // Plot Input Number Range
            txtInputMinimum.Text = feedback.ADCMinimum.ToString(CultureInfo.CurrentCulture);
            txtInputMaximum.Text = feedback.ADCMaximum.ToString(CultureInfo.CurrentCulture);

            // Set Input Gain Value
            lblInputGain.Text = ddum.ToString("#0.000000", CultureInfo.CurrentCulture);

            // Guarding condition end
            ParameterUpdate = false;


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

        
    }
}
