﻿using System;
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
        FilterInfoCollection filterInfoCollection;
        VideoCaptureDevice videoCaptureDevice;
        DbTransactions transact = new DbTransactions();
        string currDateTime = DateTime.Now.ToString("hh:mm:s tt");
        string today = DateTime.Now.ToString("MMMM dd, yyyy");
        string today1 = DateTime.Now.ToShortDateString();
        public FrmMain() {
            InitializeComponent();
            timerTimeRef.Start();
            lblVerified.Text = "to UCV!";
        }


        private void FrmMain_Load(object sender, EventArgs e) {
             lblVerified.Text = "to UCV!";
            
            filterInfoCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach(FilterInfo item in filterInfoCollection) {
                cmbCamera.Items.Add(item.Name);
            }
            try {
                cmbCamera.SelectedIndex = 1;
            }catch(Exception x) {
                cmbCamera.SelectedIndex = 0;
            }
            
            lblDate.Text = today;

            videoCaptureDevice = new VideoCaptureDevice(filterInfoCollection[cmbCamera.SelectedIndex].MonikerString);
            videoCaptureDevice.NewFrame += VideoCaptureDevice_NewFrame;
        }

        private void btnStart_Click(object sender, EventArgs e) {

            if (videoCaptureDevice.IsRunning) {
                btnStart.Text = "Start";
                timer1.Stop();
                videoCaptureDevice.Stop();
                userScrn.ImageLocation = "./img/Web.png";
                //userScrn.Image = "./img/Web.png";

            } else {
                
                btnStart.Text = "Stop";
                lblVerified.Text = "to UCV!";
                userScrn.Image = null;
                timer1.Start();
                videoCaptureDevice.Start();
                
            }


        }

        private void restartVideo() {
            if (videoCaptureDevice.IsRunning) {
                btnStart.Text = "Start";
                timer1.Stop();
                videoCaptureDevice.Stop();
                userScrn.ImageLocation = "./img/Web.png";

            }
        }

        private void VideoCaptureDevice_NewFrame(object sender, NewFrameEventArgs eventArgs) {
            userScrn.Image = (Bitmap)eventArgs.Frame.Clone();

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
                    //lblName.Text = txtQrContent.Lines[0];
                    
                    try {
                        name = txtQrContent.Lines[0];
                        if (name == "Dr. Edward Y. Chua") {
                            name = "Dr. Edison Y. Chua";
                        }
                        position = txtQrContent.Lines[1]; 
                        position1 = txtQrContent.Lines[2]; 
                    }catch(Exception ex) { /*none*/ }
                    
                    
                    string sql = $"SELECT ID from Registration WHERE FullName = '{name}'";
                    int UID = transact.SearchProfile(sql); //check if profile is in registered

                    if (UID == 0) {
                        sql = $"INSERT INTO Registration(FullName,Designation,Designation1,RDate,RTime) VALUES('{name}','{position}','{position1}','{DateTime.Now.ToShortDateString()}','{currDateTime:hh:mm:s tt}')";
                        try {
                            transact.AddRecord(sql);
                            using (var soundPlayer = new SoundPlayer(@"./audio/Correct Sound.wav")) {
                                soundPlayer.Play(); // can also use soundPlayer.PlaySync()
                            }
                            lblVerified.Text = name;
                            //sound here
                            
                        } catch (Exception ex) {
                            using (var soundPlayer = new SoundPlayer(@"./audio/Error Alert.wav")) {
                                soundPlayer.Play(); // can also use soundPlayer.PlaySync()
                            }
                            lblVerified.Text = "Please try again!";
                        }

                    } else {
                        using (var soundPlayer = new SoundPlayer(@"./audio/Correct Sound.wav")) {
                            soundPlayer.Play(); // can also use soundPlayer.PlaySync()
                        }
                        lblName.Text = "Already Registered!";
                        lblVerified.Text = name;
                        

                    }


                    




                    /* if (UID != 0) {

                         sql = $"SELECT ID from Record WHERE UID = {UID} AND Date1 = '{DateTime.Now.ToShortDateString()}'";
                         int myID = transact.SearchProfile(sql);

                         if (myID != 0) {
                             //Record Found update in and out

                             sql = $"SELECT * FROM Record WHERE ID={myID} AND Date1 = '{today1}'";
                             DataTable recordData = transact.fetch_rowData(sql);

                             //update the selected record
                             foreach (DataRow row in recordData.Rows) {
                                 foreach (DataColumn col in recordData.Columns) {
                                     if (row.IsNull(col)) {
                                         //MessageBox.Show($"{col.ColumnName} is empty");
                                         sql = $"UPDATE Record SET {col.ColumnName} = '{currDateTime:hh:mm:s tt}'  WHERE ID = {myID}";
                                         transact.UpdateRecord(sql);
                                         if (col.ColumnName=="Out0" || col.ColumnName == "Out1" || col.ColumnName == "Out2") {
                                             lblVerified.Text = "Timed Out";
                                         } else {
                                             lblVerified.Text = "Timed In";
                                         }

                                         break;
                                     }
                                     if (!row.IsNull(col)) {
                                         lblVerified.Text = "Reached 3 scans";
                                     }
                                 }
                             }

                         } else {
                             //new record must add

                             sql = $"INSERT INTO Record(UID,Date1,In0) VALUES({UID},'{DateTime.Now.ToShortDateString()}','{currDateTime:hh:mm:s tt}')";
                            // MessageBox.Show(sql);
                            transact.AddRecord(sql);
                            // MessageBox.Show(UID+" "+today + " "+currDateTime);
                         }


                     } else {
                         lblVerified.Text = "UNVERIFIED";
                     }*/

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
            
            /*try {
                MessageBox.Show("Eyyy Successs");

            }catch(Exception ex) {
                MessageBox.Show("Database is inaccessible","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }*/

        }

        private void timerTimeRef_Tick(object sender, EventArgs e) {
            currDateTime = DateTime.Now.ToString("hh:mm:s tt");
            lblTime.Text = currDateTime.ToString();
        }

        private void FrmMain_FormClosed(object sender, FormClosedEventArgs e) {
            if (videoCaptureDevice.IsRunning) {
                videoCaptureDevice.Stop();
            }
            Application.Exit();
        }


        private void FrmMain_FormClosing_1(object sender, FormClosingEventArgs e) {
            if (videoCaptureDevice.IsRunning) {
                videoCaptureDevice.Stop();
            }
        }

        private void timerRefresh_Tick(object sender, EventArgs e) {
            lblVerified.Text = "to UCV!";
            //lblName.Text = "Welcome,";
        }


        private void cmbCamera_SelectionChangeCommitted(object sender, EventArgs e) {
            restartVideo();
            videoCaptureDevice = new VideoCaptureDevice(filterInfoCollection[cmbCamera.SelectedIndex].MonikerString);
            videoCaptureDevice.NewFrame += VideoCaptureDevice_NewFrame;
        }
    }
}
