using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrisaaAttendance {
    public partial class SettingForm : Form {
        ScannerImplementation sci;
        public SettingForm() {
            InitializeComponent();
            sci = new ScannerImplementation();
        }

        private void cmbRegType_SelectedIndexChanged(object sender, EventArgs e) {
            //MessageBox.Show(cmbRegType.SelectedIndex.ToString());
            //sci.setRegistrationType(cmbRegType.SelectedIndex);
            SharedData.regType = cmbRegType.SelectedIndex;
        }

        private void btnApply_Click(object sender, EventArgs e) { 
            sci.createTable(txtEvent.Text);
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e) {
            this.Close();
        }
    }
}
