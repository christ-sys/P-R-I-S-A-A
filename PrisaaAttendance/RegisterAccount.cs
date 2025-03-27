using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZXing;
using ZXing.Common;
using ZXing.QrCode.Internal;
using ZXing.Rendering;

namespace PrisaaAttendance {
    public partial class RegisterAccount : Form {
        Bitmap bitmap;
        private String path;
        public RegisterAccount() {
            InitializeComponent();
            path = Path.Combine(Application.StartupPath, "Generated QR");

            // Create directory if it doesn't exist
            if (!Directory.Exists(path)) {
                Directory.CreateDirectory(path);
            }

        }

        private void BtnGenerateQr_Click(object sender, EventArgs e) {
            BarcodeWriter barcodeWriter = new BarcodeWriter();
            EncodingOptions encodingOptions = new EncodingOptions() { Width = 500, Height = 500, Margin = 1, PureBarcode = false };
            encodingOptions.Hints.Add(EncodeHintType.ERROR_CORRECTION, ErrorCorrectionLevel.H);
            barcodeWriter.Renderer = new BitmapRenderer();
            barcodeWriter.Options = encodingOptions;
            barcodeWriter.Format = BarcodeFormat.QR_CODE;
            bitmap = barcodeWriter.Write(TxtInfo.Text);
            Bitmap logo = new Bitmap($"{Application.StartupPath}/img/NatlPrisaaLogo.png");
            logo = new Bitmap(logo, 200, 200);
            //Bitmap logo = new Bitmap($"{Application.StartupPath}/foxlearn.png");
            Graphics g = Graphics.FromImage(bitmap);
            g.DrawImage(logo, new Point((bitmap.Width - logo.Width) / 2, (bitmap.Height - logo.Height) / 2));
            PicQr.Image = bitmap;
        }

        private void BtnDownload_Click(object sender, EventArgs e) {
            

            using (SaveFileDialog saveDialog = new SaveFileDialog()) {
                saveDialog.Filter = "Image file|*.jpeg";
                saveDialog.Title = "Save an image";
                String txtName = TxtInfo.Lines[0];
                saveDialog.FileName = $"{txtName}.jpg";
                saveDialog.InitialDirectory =path;
                
                DialogResult res = saveDialog.ShowDialog();

                if (saveDialog.FileName != "" && res == DialogResult.OK) {
                    bitmap.Save(saveDialog.FileName.ToString(), ImageFormat.Jpeg);
                }

            }
            
        }


        private void PicQr_Paint(object sender, PaintEventArgs e) {
            Padding p = new Padding();
            p.Left = 500 / 4;
            p.Top = 500 / 4;
            PicQr.Padding = p;
        }

        private void BtnClear_Click(object sender, EventArgs e) {
            TxtInfo.Clear();
            PicQr.Image = null;
        }

    }
}
