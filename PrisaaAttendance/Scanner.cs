using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrisaaAttendance {
    public abstract class Scanner {
        public abstract void cameraInit(int x); /*SET UP/PREPARATION FOR VIDEO CAPTURING*/

        public abstract void cameraList(ComboBox cmb); /*POPULATE CAMERA ON THE CMB BOX*/
        public abstract void scanStart();  /*START SCANNING / OPEN CAMERA */
        public abstract void scanStop(); /*OFF CAMERA */
        public abstract void cameraRestart();
        public abstract string validateData(string data);
        public abstract string readQR();

        public abstract void playSound(string path);
    }
}
