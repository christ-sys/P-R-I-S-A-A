using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Data;
using System.Windows.Forms;
using System.Collections;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace PrisaaAttendance {
    
   // Provider=Microsoft.ACE.OLEDB.12.0;Data Source = C:\Users\Ichan\Desktop\PrisaaAttendance\PrisaaAttendance\PrisaaAttendance\bin\Debug\Attendance.accdb
    public class DbTransactions {
        OleDbConnection conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source = .\\Attendance.accdb; Mode=ReadWrite");
        DataTable dtable;
        ScannerImplementation sc = new ScannerImplementation();
        

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


        public void regtype1(string name, string curr_date, string curr_time, string info = "", string details = "") {

             string sql = $"INSERT INTO {SharedData.tableRef}(Name,Curr_Date,Time_in,Info,Details) VALUES('{name}','{curr_date}','{curr_time}','{info}','{details}')";

            /* string sql = $"INSERT INTO {SharedData.tableRef}(Name,Curr_Date,Time_in,Info,Details)VALUES(@name,@date,@time,@info,@details)";
             using (OleDbCommand cmd = new OleDbCommand(sql, conn)) {
                 cmd.Parameters.AddWithValue("@name", string.IsNullOrEmpty(name)? (object)DBNull.Value: name);
                 cmd.Parameters.AddWithValue("@date", string.IsNullOrEmpty(curr_date)?(object)DBNull.Value: curr_date);
                 cmd.Parameters.AddWithValue("@time", string.IsNullOrEmpty(curr_time) ? (object)DBNull.Value : curr_time);
                 cmd.Parameters.AddWithValue("@info", string.IsNullOrEmpty(info) ? (object)DBNull.Value : info);
                 cmd.Parameters.AddWithValue("@details", string.IsNullOrEmpty(details) ? (object)DBNull.Value : details);
                // cmd.ExecuteNonQuery();
             }*/
            AddRecord(sql);
            
          //  return sql;
        }


        public Boolean isExisting(string tab) {
            DataTable schema = conn.GetSchema("Tables");
            foreach(DataRow row in schema.Rows) {
                if (row["TABLE_NAME"].ToString().Equals(tab, StringComparison.OrdinalIgnoreCase)) {
                    return true;
                }
            }

            return false;
        }

        public void createTable(string query, string tab) {
            conn.Open();
            if (!isExisting(tab)) {
                OleDbCommand dbCommand = new OleDbCommand(query, conn);

                try {
                    
                    dbCommand.ExecuteNonQuery();
                    MessageBox.Show("Table created successfully","Table creation",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    SharedData.tableRef = tab;
                    //sc.setReferenceTable(tab);
                } catch (Exception ex) {
                    MessageBox.Show(ex.Message,"Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
            } else { 
                DialogResult res= MessageBox.Show("Table is already existing!\nDo you want to use the existing table?", "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (res == DialogResult.OK) {
                    SharedData.tableRef = tab;
                   // sc.setReferenceTable(tab);
                }

            }
            conn.Close();
        }

        public void simpleTable(string tab) {
            string query = $"CREATE TABLE {tab}(ID autoincrement primary key, Name varchar(100),Info varchar(100), Details varchar(200), Curr_Date varchar(32), Time_in datetime)"; 
            createTable(query, tab);
            SharedData.query = $"INSERT INTO {tab} (Name,Curr_Date,Time_in)";
        }

        public void sessionTable(string tab) {
            string query = $"CREATE TABLE {tab}(ID autoincrement primary key, Name varchar(100),Info varchar(100), Details varchar(200), Curr_Date varchar(32), In0 datetime, Out0 datetime)";
            createTable(query, tab);
        }
        public void doubleSessionTable(string tab) {
            string query = $"CREATE TABLE {tab}(ID autoincrement primary key, Name varchar(100),Info varchar(100), Details varchar(200),Curr_Date varchar(32), In0 datetime, Out0 datetime, In1 datetime, Out1 datetime)";
            createTable(query, tab);
        }


    }
}
