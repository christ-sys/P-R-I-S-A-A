using System;
using System.Data.OleDb;
using System.Data;
using System.Windows.Forms;
using System.Collections.Generic;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace PrisaaAttendance {
    
    public class DbTransactions {
        OleDbConnection conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source = .\\Attendance.accdb; Mode=ReadWrite");
        DataTable dtable;
        ScannerImplementation sc = new ScannerImplementation();
        
        public void dbError(string txt) {
            string warning = "";
            if (txt.Contains("No value given")) {
                warning = "Required fields are missing";
            }else if(txt.Contains("syntax error")) {
                warning = "Query is incorrect";
            }else if (txt.Contains("duplicate")) {
                warning = "Duplicate entry detected! This record is already existing";
            } else {
                warning = "An error occured!";
            }
            MessageBox.Show(warning, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        public void AddRecord(string sql) {
            if (conn.State == ConnectionState.Closed) {
                conn.Open();
            }
            
            using (OleDbCommand dbCommand = new OleDbCommand(sql, conn)) {
                try {
                    dbCommand.ExecuteNonQuery();
                } catch (Exception ex) {
                    dbError(ex.Message);
                }
            }
        }
        public int SearchProfile(string sql) {
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
            AddRecord(sql);
        }

        public void regtype2(string name, string curr_date, string curr_time,string info = "", string details = "") {
            string sql = $"INSERT INTO {SharedData.tableRef}(Name,Curr_Date,In0,Info,Details) VALUES('{name}','{curr_date}','{curr_time}','{info}','{details}')";
            AddRecord(sql);  
        }

        public void regtype2(string name, string curr_date, string curr_time) {
            string sql = $"UPDATE {SharedData.tableRef} SET Out0='{curr_time}' WHERE Name = '{name}' AND Curr_Date = '{curr_date}'";
            AddRecord(sql);
        }


        public void regtype3(string name, string curr_date, string curr_time) {
            string sql = $"SELECT * FROM {SharedData.tableRef}  WHERE Name = '{name}' AND Curr_Date = '{curr_date}'";

            if(conn.State == ConnectionState.Closed) {
                conn.Open();
            }
            
            using (OleDbCommand cmd = new OleDbCommand(sql, conn)) {
                var reader = cmd.ExecuteReader();

                if (reader.Read()) {
                    string out0 = reader["Out0"]?.ToString();
                    string in1 = reader["In1"]?.ToString();
                    string out1 = reader["Out1"]?.ToString();
                    string field = "";

                    if (string.IsNullOrEmpty(out0)) {
                        field = "Out0";
                    }else if (string.IsNullOrEmpty(in1)) {
                        field = "In1";
                    }else if (string.IsNullOrEmpty(out1)) {
                        field = "Out1";
                    } else {
                        MessageBox.Show("Slots are filled","Information",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    }

                    if(!string.IsNullOrEmpty(field)){
                        string q = $"UPDATE {SharedData.tableRef} SET {field} = '{curr_time}' WHERE Name = '{name}' AND Curr_Date = '{curr_date}' ";
                        conn.Close();
                        AddRecord(q);
                    }

                }

            }
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

        public int getColumnCount(string tab) {
            int count = 0;
            DataTable schemaTab = conn.GetSchema("Columns",new string[] {null, null, tab, null});
            foreach(DataRow row in schemaTab.Rows) {
                count++;
            }
            return count;
        }

        public List<string> getTableNames() {

            conn.Open();
            DataTable schema = conn.GetSchema("Tables");
            List<string> tables = new List<string>();

            
            foreach (DataRow row in schema.Rows) {
                string tableType = row["TABLE_TYPE"].ToString();
                string tableName = row["TABLE_NAME"].ToString();

                // Filter for user tables (not system tables or views)
                if (tableType == "TABLE" && !tableName.StartsWith("MSys")) {
                    tables.Add(tableName);
                }
            }
            conn.Close();
            return tables;
        }

        public int getRegCount(string sql) {
            int count = 0;

            if(conn.State == ConnectionState.Closed) {
                conn.Open();
            }

            using(OleDbCommand cmd = new OleDbCommand(sql, conn)) {
                count = Convert.ToInt32(cmd.ExecuteScalar());
            }

            return count;
        }

        


        public void createTable(string query, string tab) {
            
            if(conn.State == ConnectionState.Closed) {
                conn.Open();
            }
            if (!isExisting(tab)) {
                using (OleDbCommand dbCommand = new OleDbCommand(query, conn)) {
                    try {

                        dbCommand.ExecuteNonQuery();
                        MessageBox.Show("Table created successfully", "Table creation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        SharedData.tableRef = tab;
                    } catch (Exception ex) {
                        dbError(ex.Message);
                    }
                }

                    
            } else { 
                DialogResult res= MessageBox.Show("Table is already existing!\nDo you want to use the existing table?", "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (res == DialogResult.OK) {
                    identifyRegType(tab);
                }
            }
        }

        public void identifyRegType(string tab) {
            if (conn.State == ConnectionState.Closed) {
                conn.Open();
            }

            SharedData.tableRef = tab;
            int i = getColumnCount(SharedData.tableRef);
            switch (i) {
                case 6:
                    SharedData.regType = 0;
                    break;
                case 7:
                    SharedData.regType = 1;
                    break;
                case 9:
                    SharedData.regType = 2;
                    break;
            }

            conn.Close();
        }

        public void simpleTable(string tab) {
            string query = $"CREATE TABLE {tab}(ID autoincrement primary key, Name varchar(100),Info varchar(100), Details varchar(200), Curr_Date varchar(32), Time_in datetime)"; 
            createTable(query, tab);
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
