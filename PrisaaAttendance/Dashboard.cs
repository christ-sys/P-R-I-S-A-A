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
    public partial class Dashboard : Form {
        public Dashboard() {
            InitializeComponent();
            frm();
        }

        public void frm() {
            
            //loop per distinct record and count each
            for(int i=0; i<5; i++) {
                addGrb();
            }
            

        }

        public void addGrb() {
            FlowLayoutPanel fl = mainGrb;
            fl.Padding = new Padding(10,5,10,5);
            GroupBox grb = new GroupBox();
           // grb.Text = "Sample";
            grb.BackColor = Color.Honeydew;
            grb.Width = 271;
            grb.Height = 196;
            grb.Margin = new Padding(5);

            Label lbl = new Label();
            lbl.Text = "100";
            lbl.ForeColor = Color.SteelBlue;
            lbl.Location = new Point(0, 11);
            lbl.Size = new Size(271, 97);
            lbl.AutoSize = false;
            lbl.Font = new Font("Microsoft Sans Serif", 45);
            lbl.Margin = new Padding(3, 0, 3, 0);
            lbl.TextAlign = ContentAlignment.MiddleCenter;
            grb.Controls.Add(lbl);

            Label lblCaption = new Label();
            lblCaption.Text = "bsvfd nhdfgh sdfjhsgghjggjgdgsy sdyfjgsydgfjsdfsbdjh";
            lblCaption.ForeColor = Color.SlateGray;
            lblCaption.Location = new Point(0, 119);
            lblCaption.Size = new Size(271, 61);
            lblCaption.Font = new Font("Microsoft Sans Serif", 15);
            lblCaption.TextAlign = ContentAlignment.TopCenter;
            lblCaption.AutoEllipsis = true;
            grb.Controls.Add(lblCaption);




            fl.Controls.Add(grb);
        }

        private void label3_Click(object sender, EventArgs e) {

        }

        private void label4_Click(object sender, EventArgs e) {

        }
    }
}
