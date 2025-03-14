using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;
using ZXing;
using System.Data.OleDb;
using System.Media;


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
            refTable = "Registration";
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
                    
                    
                    string sql = $"SELECT ID from {refTable} WHERE FullName = '{name}'";
                    int UID = transact.SearchProfile(sql); //check if profile is in registered

                    if (UID != 0) { // ADD IF ALREADY EXISTS
                        sql = $"INSERT INTO {refTable}(FullName,Designation,Designation1,RDate,RTime) VALUES('{name}','{position}','{position1}','{DateTime.Now.ToShortDateString()}','{currDateTime:hh:mm:s tt}')";
                        try {
                            transact.AddRecord(sql);
                            scn.validSound("./audio/Correct Sound.wav");
                            lblVerified.Text = name;
                            
                        } catch (Exception ex) {
                            scn.validSound("./audio/Error Alert.wav");
                            lblVerified.Text = "Please try again!";
                        }

                    } else {
                        scn.validSound("./audio/Correct Sound.wav");
                        lblName.Text = "Already Registered!";
                        lblVerified.Text = name;
                        
                    }


                    if (timerRefresh.Enabled) {
                        timerRefresh.Stop();
                        lblName.Text = "Welcome";
                        timerRefresh.Start();
                    } else {

                        timerRefresh.Start();
                    }
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
        }


        private void cmbCamera_SelectionChangeCommitted(object sender, EventArgs e) {
            scn.cameraRestart();
            btnStart.Text = scn.btnLabel;
            userScrn = scn.picBox; 
            timer1.Stop();
            
            scn.cameraInit(cmbCamera.SelectedIndex);
        }
    }
}
