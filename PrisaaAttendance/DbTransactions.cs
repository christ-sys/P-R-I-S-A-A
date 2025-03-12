using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Data;
using System.Windows.Forms;

namespace PrisaaAttendance {
    
   // Provider=Microsoft.ACE.OLEDB.12.0;Data Source = C:\Users\Ichan\Desktop\PrisaaAttendance\PrisaaAttendance\PrisaaAttendance\bin\Debug\Attendance.accdb
    public class DbTransactions {
        OleDbConnection conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source = .\\Attendance.accdb");
        DataTable dtable;

        public void AddRecord(string sql) {
            OleDbCommand dbCommand = new OleDbCommand();
            dbCommand.Connection = conn;
            dbCommand.CommandText = sql;
            try {
                conn.Open();
                dbCommand.ExecuteNonQuery();
                conn.Close();
            } catch(Exception ex) {
                MessageBox.Show(ex.Message);
                conn.Close();
            }
            
        }
        public void UpdateRecord(string sql) {
            OleDbCommand dbCommand = new OleDbCommand();
            dbCommand.Connection = conn;
            dbCommand.CommandText = sql;

            try {
                conn.Open();
                dbCommand.ExecuteNonQuery();
                conn.Close();
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
                conn.Close();
            }
        }
        public int SearchProfile(string sql) {
           // string sql = $"SELECT ID from Profiles WHERE Name = '{item}'";
            OleDbDataAdapter da = new OleDbDataAdapter(sql, conn);
            dtable = new DataTable();
            da.Fill(dtable);

            if (dtable.Rows.Count > 0) {
                foreach(DataRow row in dtable.Rows) {
                    return (int)row["ID"];
                }
            }

            return 0;


        }

        public DataTable fetch_rowData(string q) {
            try {
                using (OleDbDataAdapter myDataAdapter = new OleDbDataAdapter(q, conn)) {
                    dtable = new DataTable();
                    myDataAdapter.Fill(dtable);
                }
            } catch (Exception ex) {
                MessageBox.Show(ex.Message.ToString(), "Something Went Wrong!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            return dtable;
        }
    }
}
