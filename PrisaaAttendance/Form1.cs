﻿using System;
using System.Drawing;
using System.Windows.Forms;
using ZXing;
using System.Data.OleDb;


namespace PrisaaAttendance {
    public partial class FrmMain : Form {
        //FilterInfoCollection filterInfoCollection;
        DbTransactions transact = new DbTransactions();
        string currDateTime = DateTime.Now.ToString("hh:mm:s tt");
        string today = DateTime.Now.ToString("MMMM dd, yyyy");
        ScannerImplementation scn;
        string refTable;
        public FrmMain() {
            InitializeComponent();
            timerTimeRef.Start();
            //lblVerified.Text = "to UCV!";
            scn = new ScannerImplementation(userScrn);
            lblVerified.Text = scn.lblWelcome;
            SharedData.tableRef = "temp1";
            refTable = SharedData.tableRef;
            SharedData.regType = 0;
        }


        private void FrmMain_Load(object sender, EventArgs e) {
            lblVerified.Text = scn.lblWelcome;
            scn.cameraList(cmbCamera);//populate combobox for camera
            scn.cameraInit(); // initialize frame/picturebox for capturing
            lblDate.Text = today; //display current date
        }

        private void btnStart_Click(object sender, EventArgs e) {
            scn.scanStart(); //Camera start capturing
            btnStart.Text = scn.btnLabel;
            if (scn.videoCaptureDevice.IsRunning) {
                timer1.Start();
                lblVerified.Text = scn.lblWelcome;

            } else {
                userScrn.Image = null; 
                timer1.Stop();
            }
            
        }

        private void timer1_Tick(object sender, EventArgs e) {
            if (userScrn.Image != null) {
                BarcodeReader barcodeReader = new BarcodeReader();
                Result result = barcodeReader.Decode((Bitmap)userScrn.Image); //ICHN
                string name = "";
                string position = "";
                string position1 = "";
                if (result != null) {
                    txtQrContent.Text = result.ToString();

                    try {
                        name = scn.validateData(txtQrContent.Lines[0]);
                        position = scn.validateData(txtQrContent.Lines[1]); 
                        position1 = scn.validateData(txtQrContent.Lines[2]); 
                    }catch(Exception ex) { /*none*/ }
                    
                    
                    string sql = $"SELECT ID from {refTable} WHERE Name = '{name}' AND Curr_Date ='{DateTime.Now.ToShortDateString()}'";
                    int UID = transact.SearchProfile(sql); //check if profile is in registered

                    switch (SharedData.regType) { //testing type of registration (0-simple, 1-in/iout, 2-2 session)
                        case 0:
                            if (UID == 0) { //unique record
                                try {
                                    transact.regtype1(name, DateTime.Now.ToShortDateString(), DateTime.Now.ToString("hh:mm:s tt"), position, position1);
                                    success(name);
                                } catch (Exception) {error();}
                            } else {
                                duplicate(name, "Already Registered!");
                            }

                            break;
                        case 1:
                            if (UID == 0) { //unique record
                                try {
                                    transact.regtype2(name, DateTime.Now.ToShortDateString(), DateTime.Now.ToString("hh:mm:s tt"),position, position1);
                                    success(name);
                                } catch (Exception) { error(); }
                            } else { //update for time out
                                transact.regtype2(name, DateTime.Now.ToShortDateString(), DateTime.Now.ToString("hh:mm:s tt"));
                                duplicate(name, "Thank you for coming!");
                            }

                            break;
                        case 2:
                            if (UID == 0) { //unique record
                                try {
                                    transact.regtype2(name, DateTime.Now.ToShortDateString(), DateTime.Now.ToString("hh:mm:s tt"), position, position1);
                                    success(name);
                                } catch (Exception) { error(); }
                            } else { //update for time out
                                transact.regtype3(name, DateTime.Now.ToShortDateString(), DateTime.Now.ToString("hh:mm:s tt"));
                                duplicate(name, "Thank you for coming!");
                            }
                            break;

                    }

                    if (timerRefresh.Enabled) {
                        timerRefresh.Stop();
                    } 
                    timerRefresh.Start();
                    
                }
            }
        }

       

        private void btnRegister_Click(object sender, EventArgs e) {
            OleDbConnection conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source = .\\Attendance.accdb");
            RegisterAccount regFrm = new RegisterAccount();
            regFrm.ShowDialog();
        }

        private void timerTimeRef_Tick(object sender, EventArgs e) {
            currDateTime = DateTime.Now.ToString("hh:mm:s tt");
            lblTime.Text = currDateTime.ToString();
        }

        private void FrmMain_FormClosed(object sender, FormClosedEventArgs e) {
            scn.scanStop();
            Application.Exit();
        }


        private void FrmMain_FormClosing_1(object sender, FormClosingEventArgs e) {
            scn.scanStop();
        }

        private void timerRefresh_Tick(object sender, EventArgs e) {
            lblVerified.Text = scn.lblWelcome;
            lblName.Text = "Welcome";
        }


        private void cmbCamera_SelectionChangeCommitted(object sender, EventArgs e) {
            scn.cameraRestart();
            btnStart.Text = scn.btnLabel;
            userScrn = scn.picBox; 
            timer1.Stop();
            
            scn.cameraInit(cmbCamera.SelectedIndex);
        }

        private void btnSetting_Click(object sender, EventArgs e) {
            SettingForm setting = new SettingForm();
            DialogResult res = DialogResult.None;
            res = setting.ShowDialog();
            refTable = SharedData.tableRef;
        }

        private void error() {
            scn.validSound("./audio/Error Alert.wav");
            lblVerified.Text = "Please try again!";
        }
        private void duplicate(string name,string txt) {
            scn.validSound("./audio/Correct Sound.wav");
            lblName.Text = txt;
            lblVerified.Text = name;
        }
        private void success(string name) {
            lblName.Text = "Welcome";
            scn.validSound("./audio/Correct Sound.wav");
            lblVerified.Text = name.Replace("''", "'");
        }

        private void btnDashboard_Click(object sender, EventArgs e) {
            Dashboard dashbrd = new Dashboard();
            dashbrd.ShowDialog();
        }
    }
}
