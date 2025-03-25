namespace PrisaaAttendance {
    partial class Dashboard {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.mainGrb = new System.Windows.Forms.FlowLayoutPanel();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.grbContent = new System.Windows.Forms.GroupBox();
            this.panel1.SuspendLayout();
            this.grbContent.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainGrb
            // 
            this.mainGrb.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mainGrb.AutoScroll = true;
            this.mainGrb.BackColor = System.Drawing.SystemColors.Control;
            this.mainGrb.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mainGrb.Location = new System.Drawing.Point(67, 229);
            this.mainGrb.Name = "mainGrb";
            this.mainGrb.Size = new System.Drawing.Size(667, 316);
            this.mainGrb.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 80.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Navy;
            this.label3.Location = new System.Drawing.Point(0, 28);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(800, 116);
            this.label3.TabIndex = 1;
            this.label3.Text = "200";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(0, 154);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(800, 39);
            this.label4.TabIndex = 1;
            this.label4.Text = "Total Registers";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(800, 223);
            this.panel1.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoEllipsis = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.SlateGray;
            this.label2.Location = new System.Drawing.Point(0, 133);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(271, 47);
            this.label2.TabIndex = 1;
            this.label2.Text = "bsvfd nhdfgh sdfjhsgghjggjgdgsy sdyfjgsydgfjsdfsbdjh\r\n\r\n";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 45F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.SteelBlue;
            this.label1.Location = new System.Drawing.Point(0, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(271, 97);
            this.label1.TabIndex = 1;
            this.label1.Text = "123456";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // grbContent
            // 
            this.grbContent.BackColor = System.Drawing.Color.Honeydew;
            this.grbContent.Controls.Add(this.label1);
            this.grbContent.Controls.Add(this.label2);
            this.grbContent.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.grbContent.Location = new System.Drawing.Point(18, 332);
            this.grbContent.Margin = new System.Windows.Forms.Padding(20);
            this.grbContent.Name = "grbContent";
            this.grbContent.Size = new System.Drawing.Size(271, 196);
            this.grbContent.TabIndex = 0;
            this.grbContent.TabStop = false;
            this.grbContent.Visible = false;
            // 
            // Dashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.ClientSize = new System.Drawing.Size(800, 557);
            this.Controls.Add(this.grbContent);
            this.Controls.Add(this.mainGrb);
            this.Controls.Add(this.panel1);
            this.Name = "Dashboard";
            this.Text = "Dashboard";
            this.panel1.ResumeLayout(false);
            this.grbContent.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel mainGrb;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox grbContent;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}