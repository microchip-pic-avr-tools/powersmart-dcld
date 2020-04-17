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
    public partial class frmGetNominalControlOutput : Form
    {

        internal clsOutputDeclaration output = new clsOutputDeclaration();
        bool _WindowLoading = false;

        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public frmGetNominalControlOutput()
        {
            InitializeComponent();
        }

        private void frmGetNominalControlOutput_Load(object sender, EventArgs e)
        {

            _WindowLoading = true;

            // Set Window Properties
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ShowInTaskbar = false;
            this.HelpButton = false;

            cmbConverterType.SelectedIndex = Convert.ToInt32(output.ConverterType);
            txtWindingRatioPrimary.Text = output.WindingRatioPrimary.ToString(CultureInfo.CurrentCulture);
            txtWindingRatioSecondary.Text = output.WindingRatioSecondary.ToString(CultureInfo.CurrentCulture);
            txtNomInputVoltage.Text = output.NominalInputVoltage.ToString(CultureInfo.CurrentCulture);
            txtNomOutputVoltage.Text = output.NominalOutputVoltage.ToString(CultureInfo.CurrentCulture);
            txtNomEfficiency.Text = (100.0 * output.NominalEfficiency).ToString(CultureInfo.CurrentCulture);
            _WindowLoading = false;

            CalculateNominalOutput(sender, e);

            // Set Window Startup Position
            this.StartPosition = FormStartPosition.CenterParent;

            // Enable Key Preview for Hot Key support
            this.KeyPreview = true;
        }

         private void frmGetNominalControlOutput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Escape)
                this.Close();
            return;
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            CalculateNominalOutput(sender, e);
            this.Close();
        }

        private void CalculateNominalOutput(object sender, EventArgs e)
        {

            bool _sanity_check = false;
            TextBox txt = new TextBox();
            ComboBox cmb = new ComboBox();

            if (_WindowLoading) return;

            if (sender.GetType().ToString() == "System.Windows.Forms.TextBox") 
            {
                txt = (TextBox)sender;
                cmb = null;
            }
            else if (sender.GetType().ToString() == "System.Windows.Forms.ComboBox")
            {
                txt = null;
                cmb = (ComboBox)sender;
            } 

            try
            {
                output.ConverterType = (clsOutputDeclaration.dcldConverterType)cmbConverterType.SelectedIndex;
                output.NominalOutputVoltage = Convert.ToDouble(txtNomOutputVoltage.Text);
                output.NominalInputVoltage = Convert.ToDouble(txtNomInputVoltage.Text);
                output.NominalEfficiency = (Convert.ToDouble(txtNomEfficiency.Text) / 100.0);;
                output.WindingRatioPrimary = Convert.ToDouble(txtWindingRatioPrimary.Text);
                output.WindingRatioSecondary = Convert.ToDouble(txtWindingRatioSecondary.Text);

                lblOutputResult.Text = (Math.Round((100.0 * output.PWMDutyCycle), 3)).ToString(CultureInfo.CurrentCulture);

                _sanity_check = (bool)((0.0 <= output.PWMDutyCycle) && (output.PWMDutyCycle <= 1.0));

                // Reset all controls to default background color
                foreach (Control ctrl in grpNominalOutput.Controls)
                {
                    if (ctrl.GetType().ToString() == "System.Windows.Forms.TextBox")
                        ctrl.BackColor = SystemColors.Window;
                }
                cmdOK.Enabled = true; //Enable users to apply settings.

                // If any error occurred...
                if ((!_sanity_check) && (txt != null)) { // particular error
                    txt.BackColor = Color.LightCoral;
                    cmdOK.Enabled = false;
                }
                else if ((!_sanity_check) && (txt == null)) // general error (converter type mismatch with data
                {
                    foreach (Control ctrl in grpNominalOutput.Controls)
                    {
                        if (ctrl.GetType().ToString() == "System.Windows.Forms.TextBox")
                            ctrl.BackColor = Color.LightCoral;
                    }
                    cmdOK.Enabled = false;

                }

            }
            catch 
            {
                cmdOK.Enabled = false;
                if (txt != null)
                    txt.BackColor = Color.LightCoral; 
            }
        }


    }
}
