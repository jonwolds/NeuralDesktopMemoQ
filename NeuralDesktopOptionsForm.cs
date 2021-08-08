using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lexorama.NeuralDesktopMemoQ
{
    public partial class NeuralDesktopOptionsForm : Form
    {

        public NeuralDesktopOptions Options
        {
            get;
            set;
        }

        public NeuralDesktopOptionsForm(NeuralDesktopOptions options)
        {
            Options = options;
            InitializeComponent();

        }
        public NeuralDesktopOptionsForm()
        {
            InitializeComponent();
        }

        private void NeuralDesktopConfDialog_Load(object sender, EventArgs e)
        {

            if (Options.GeneralSettings.framework == "lua")
            {
                this.rButtonLua.Checked = true;
                this.groupBoxCon.Enabled = true;
            }
            else if (Options.GeneralSettings.framework == "wizard")
            {
                this.rButtonWizard.Checked = true;
                this.groupBoxCon.Enabled = true;
            }
            else
            {
                this.rButtonLocal.Checked = true;
                this.groupBoxCon.Enabled = false;
            }

            //if (Options.featurePosition == "start")
            //    this.rButtonFeatBeg.Checked = true;
            //else if (Options.featurePosition == "end")
            //    this.rButtonFeatEnd.Checked = true;
            //else
            //    this.rButtonFeatTok.Checked = true;

            this.address_txtbox.Text = Options.GeneralSettings.serverAddress;
            this.port_txtbox.Text = Options.GeneralSettings.port;
            //this.textBoxCustomer.Text = Options.client;
            //this.textBoxSubject.Text = Options.subject;
            //this.textBoxOtherFeatures.Text = Options.otherFeatures;
        }

        private void Save_btn_Click(object sender, EventArgs e)
        {


            if (this.rButtonLua.Checked)
                Options.GeneralSettings.framework = "lua";
            else if (this.rButtonWizard.Checked)
                Options.GeneralSettings.framework = "wizard";
            else
                Options.GeneralSettings.framework = "local";

            //if (this.rButtonFeatBeg.Checked)
            //    Options.featurePosition = "start";
            //else if (this.rButtonFeatEnd.Checked)
            //    Options.GeneralSettings.featurePosition = "end";
            //else
            //    Options.GeneralSettings.featurePosition = "token";

            Options.GeneralSettings.serverAddress = this.address_txtbox.Text.Trim();
            Options.GeneralSettings.port = this.port_txtbox.Text.Trim();

            this.DialogResult = DialogResult.OK;

        }

        private void Cancel_btn_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }




        private void ServerPort_label_Click(object sender, EventArgs e)
        {

        }

        private void ErrorMsgLabel_Click(object sender, EventArgs e)
        {

        }

        private void ServerUrl_textBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void toolTipOtherFeatures_Popup(object sender, PopupEventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            this.groupBoxCon.Enabled = true;
        }

        private void groupBoxFeats_Enter(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged_1(object sender, EventArgs e)
        {
            this.groupBoxCon.Enabled = false;
        }

        private void rButtonWizard_CheckedChanged(object sender, EventArgs e)
        {
            this.groupBoxCon.Enabled = true;
        }

        private void Save_btn_Click_1(object sender, EventArgs e)
        {

        }
    }
}
