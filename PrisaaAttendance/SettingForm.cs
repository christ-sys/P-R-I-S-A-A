using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace PrisaaAttendance {
    public partial class SettingForm : Form {
        ScannerImplementation sci;
        DbTransactions tr;
        public SettingForm() {
            InitializeComponent();
            sci = new ScannerImplementation();
            tr = new DbTransactions();
            initCmb();
        }

        private void cmbRegType_SelectedIndexChanged(object sender, EventArgs e) {
            SharedData.regType = cmbRegType.SelectedIndex;
            checkInput();
        }

        private void initCmb() {
            List<string> tab = tr.getTableNames();
            cmbTable.Items.Clear();
            foreach (string i in tab) {
                cmbTable.Items.Add(i);
            }
        }

        private void btnApply_Click(object sender, EventArgs e) {
            sci.createTable(txtEvent.Text);
            this.Close();
        }

        private void checkInput() {
            if(cmbRegType.SelectedItem != null && !string.IsNullOrEmpty(txtEvent.Text)) {
                btnApply.Enabled = true;
            } else {
                btnApply.Enabled = false;
            }
            
        }

        private void button2_Click(object sender, EventArgs e) {
            this.Close();
        }


        private void txtEvent_KeyPress(object sender, KeyPressEventArgs e) {
            if(!char.IsControl(e.KeyChar) && !char.IsLetterOrDigit(e.KeyChar) && e.KeyChar!='_') {
                e.Handled = true;
                errorProvider1.SetError(txtEvent, "Special characters is not allowed");
            } else {
                errorProvider1.Clear();
            }
            
        }

        private void txtEvent_TextChanged(object sender, EventArgs e) {
            checkInput();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e) {
            if(checkBox1.CheckState == CheckState.Checked) {
                grpExisting.Enabled = true;
                grpCreate.Enabled = false;
                
            } else {
                grpExisting.Enabled = false;
                grpCreate.Enabled = true;
            }
        }

        private void btnSelect_Click(object sender, EventArgs e) {
            try {
                tr.identifyRegType(cmbTable.SelectedItem.ToString());
                this.Close();
            }catch(Exception ey) {
                MessageBox.Show(ey.Message, "Something went wrong!", MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }
    }
}
