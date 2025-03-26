using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace PrisaaAttendance {
    public partial class Dashboard : Form {
        DbTransactions transact;
        FlowLayoutPanel fl;
        public Dashboard() {
            InitializeComponent();
            transact = new DbTransactions();
            fl = mainGrb;
            frm();

        }

        public void frm() {
            string sql = $"SELECT COUNT(*) FROM {SharedData.tableRef}"; 
            lblTotalReg.Text = transact.getRegCount(sql).ToString("D4"); //TOTAL REGISTERS

            sql = $"SELECT DISTINCT Info FROM {SharedData.tableRef} ORDER BY Info ASC"; 
            DataTable rec = transact.fetch_rowData(sql); //DISTINCT RECORDS ON INFO COL
            int x = 0;

            if (rec.Rows.Count == 0) {
                emptyRec();
            } else {
                
                //loop per distinct record and count each
                foreach (DataRow row in rec.Rows) {
                    sql = $"SELECT COUNT(*) FROM {SharedData.tableRef} WHERE Info = '{row["Info"].ToString()}'";
                    if (string.IsNullOrEmpty(row["Info"].ToString())) {
                        sql = $"SELECT COUNT(*) FROM {SharedData.tableRef} WHERE Info IS NULL";
                    }
                    x = transact.getRegCount(sql);
                    addGrb(row["Info"].ToString(), x);
                }
            }
        }

        public void addGrb(string info, int count) {
            if (string.IsNullOrEmpty(info)) {
                info = "(No details)";
            }

            fl.Padding = new Padding(10,5,10,5);
            GroupBox grb = new GroupBox();
            grb.BackColor = Color.Honeydew;
            grb.Width = 271;
            grb.Height = 196;
            grb.Margin = new Padding(5);

            Label lbl = new Label();
            lbl.Text = count.ToString("D3");
            lbl.ForeColor = Color.SteelBlue;
            lbl.Location = new Point(0, 11);
            lbl.Size = new Size(271, 97);
            lbl.AutoSize = false;
            lbl.Font = new Font("Microsoft Sans Serif", 45);
            lbl.Margin = new Padding(3, 0, 3, 0);
            lbl.TextAlign = ContentAlignment.MiddleCenter;
            grb.Controls.Add(lbl);

            Label lblCaption = new Label();
            lblCaption.Text = info;
            lblCaption.ForeColor = Color.SlateGray;
            lblCaption.Location = new Point(0, 119);
            lblCaption.Size = new Size(271, 61);
            lblCaption.Font = new Font("Microsoft Sans Serif", 15);
            lblCaption.TextAlign = ContentAlignment.TopCenter;
            lblCaption.AutoEllipsis = true;
            grb.Controls.Add(lblCaption);

            fl.Controls.Add(grb);
        }

        public void emptyRec(string text = "No records found") {
            fl.Padding = new Padding(10, 5, 10, 5);
            GroupBox grb = new GroupBox();
            grb.BackColor = Color.Honeydew;
            grb.Width = 271;
            grb.Height = 100;
            grb.Margin = new Padding(5);

            Label lbl = new Label();
            lbl.Text = text;
            lbl.ForeColor = Color.SteelBlue;
            lbl.Location = new Point(0, 11);
            lbl.Size = new Size(271, 97);
            lbl.AutoSize = false;
            lbl.Font = new Font("Microsoft Sans Serif", 20);
            lbl.Margin = new Padding(3, 0, 3, 0);
            lbl.TextAlign = ContentAlignment.MiddleCenter;
            grb.Controls.Add(lbl);

            fl.Controls.Add(grb);
        }
    }
}
