
using System.Data.OleDb;

namespace PrisaaAttendance {
    public class SharedData {
        public static string tableRef { get; set; } //global access using the static modifier

        public static int regType { get; set; }

    }
}
