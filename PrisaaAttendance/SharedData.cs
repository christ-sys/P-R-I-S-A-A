using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace PrisaaAttendance {
    public class SharedData {
        public static string tableRef { get; set; } //global access using the static modifier

        public static int regType { get; set; }

        public static string query { get; set;}

        public static string setQuery1(string name, string info, string details, string curr_date, string curr_time) {
            string sql = $"INSERT INTO {tableRef}(Name,Info,Details,Curr_Date,Time_in) VALUES(@name,@info,@details,@date,@time)";
            using(OleDbCommand cmd = new OleDbCommand()) {
                cmd.Parameters.AddWithValue("@name",name);
                cmd.Parameters.AddWithValue("@info",info);
                cmd.Parameters.AddWithValue("@details", details);
                cmd.Parameters.AddWithValue("@date", curr_date);
                cmd.Parameters.AddWithValue("@time", curr_time);
            }
            return sql;
        }

        public static string setQuery1() {

            return "";
        }
    }
}
