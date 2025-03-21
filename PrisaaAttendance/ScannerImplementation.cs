﻿using AForge.Video;
using AForge.Video.DirectShow;
using System;
using System.Drawing;
using System.Media;
using System.Windows.Forms;
using ZXing;

namespace PrisaaAttendance {
    public class ScannerImplementation : Scanner {
        public VideoCaptureDevice videoCaptureDevice;
        private FilterInfoCollection filterInfoCollection;
        private int cameraSelected;
        public PictureBox picBox;
        public string btnLabel;
        public string lblWelcome;
        private Bitmap capturedImage;
        private int registrationType;

       

        public ScannerImplementation(PictureBox p) {
            cameraSelected = 0;
            this.picBox = p;
            picBox.ImageLocation = "./img/Web.png";
            lblWelcome = "to UCV!";
            btnLabel = "Start";
            capturedImage = null;
        }
        public ScannerImplementation() {
            registrationType = 0;
        }


        /*POPULATE CAMERA ON THE CMB BOX*/
        public override void cameraList(ComboBox cmb) {
            filterInfoCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo item in filterInfoCollection) {
                cmb.Items.Add(item.Name);
            }
            try {
                cmb.SelectedIndex = 1;
            } catch (Exception x) {
                cmb.SelectedIndex = 0;
            }
        }

        /*SET UP/PREPARATION FOR VIDEO CAPTURING*/
        public override void cameraInit(int cameraSelected =0) {
            /*this.picBox = p;*/
            videoCaptureDevice = new VideoCaptureDevice(filterInfoCollection[cameraSelected].MonikerString);
            videoCaptureDevice.NewFrame += VideoCaptureDevice_NewFrame;
            



        }

        /*CAPTURE EVERY FRAME*/
        private void VideoCaptureDevice_NewFrame(object sender, NewFrameEventArgs eventArgs) {
            
            capturedImage = (Bitmap)eventArgs.Frame.Clone();
            capturedImage.RotateFlip(RotateFlipType.RotateNoneFlipX); //TO MIRROR CAMERA
            picBox.Image = capturedImage;
            

            /*try {
                picBox.Image = (Bitmap)eventArgs.Frame.Clone();
            }catch(Exception ex) {
                cameraRestart();
            }*/

        }

        /*START SCANNING / OPEN CAMERA */
        public override void scanStart() {
            if (videoCaptureDevice.IsRunning) {
                videoCaptureDevice.Stop();
                btnLabel = "Start";
                picBox.ImageLocation = "./img/Web.png";
            } else {
                picBox.Image = null;
                videoCaptureDevice.Start();
                btnLabel = "Stop";
            }

        }


        /*OFF CAMERA */
        public override void scanStop() {
            if (videoCaptureDevice.IsRunning) {
                videoCaptureDevice.Stop();
            }
        }

        public override void cameraRestart() {
            if (videoCaptureDevice.IsRunning) {
                btnLabel = "Start";
                videoCaptureDevice.Stop();
                picBox.ImageLocation = "./img/Web.png";

            }
        }

        public override string validateData(string data) {
            string text = "";
            text = data.Replace("'", "''");

            return text;
        }

        //READING QR CONTENT
        public override string readQR() {
            //string txt = "";
            BarcodeReader barcodeReader = new BarcodeReader();
            Result result = barcodeReader.Decode((Bitmap)picBox.Image); //ICHN



            return result.ToString();
        }

        public override void validSound(string soundPath) {
            using (var soundPlayer = new SoundPlayer(@soundPath)) {
                soundPlayer.Play(); // can also use soundPlayer.PlaySync()
            }
        }

       /* public void setRegistrationType(int type) {
            this.registrationType = type;
        }
        public int getRegistrationType() {
            return registrationType;
        }*/

        public void createTable(string tablename) {
            DbTransactions transact =  new DbTransactions();
            switch (SharedData.regType) {
                case 0:
                     transact.simpleTable(tablename);
                     break;
                case 1:
                    transact.sessionTable(tablename);
                    break;
                case 2:
                    transact.doubleSessionTable(tablename);
                    break;
            }
            
        }

        
    }
}
