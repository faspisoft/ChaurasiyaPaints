using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;
using System.Globalization;


namespace faspi
{
    class Database
    {
<<<<<<< HEAD
        public static DateTime ExeDate = DateTime.Parse("05-May-2021");
=======
        public static DateTime ExeDate = DateTime.Parse("29-Jul-2020");
>>>>>>> 112c82ae3816a7f8e7eb91f18a82ff39cf0bf5b2
        public static string BranchGodown_id;
        public static string prevUsr;
        public static string fname;
        public static string fyear;
        public static string uname;
        public static string BMode;
        public static string Dongleno;
        public static string utype;
        public static string upass;
        public static string databaseName;
        public static string SoftwareName;
        public static string DatabaseType = "";

        public static int OTP;
        public static string Depaccesstouser;
        public static string LocationId;
        public static string BranchId;
        public static String dformat = "dd-MMM-yyyy";
        public static DateTime ldate = new DateTime();
        public static DateTime stDate = new DateTime();

        public static DateTime enDate = new DateTime();
        public static DateTime cmonthFst = new DateTime();
        public static DateTime lmonthFst = new DateTime();
        public static DateTime lmonthLst = new DateTime();
        public static SqlConnection SqlConn = new SqlConnection();
        public static SqlConnection SqlCnn = new SqlConnection();


        public static OleDbConnection AccessConn = new OleDbConnection();
        public static OleDbConnection AccessCnn = new OleDbConnection();
        public static OleDbConnection MultiConn = new OleDbConnection();



        private static SqlCommand sqlcmd;
        public static string inipathfile = "";
        public static String loginfoName;
        public static string inipath = "";
        public static string sqlseverpwd = "";
        public static string user_id;
        private static OleDbCommand accesscmd;
        public static SqlTransaction sqlTran;

        private static SqlTransaction sqlTrana;
        private static OleDbTransaction AccessTran;
        private static OleDbTransaction AccessTrana;




        public static string SHostname = "";
        public static string SUsername = "";
        public static string SDbname = "";
        public static string SPwd = "";
        public static string ServerPath = "";
        public static string LastError = "";
        public static string CompanyState_id = "";
        public static bool IsKacha = false;
        public static bool LoginbyDb = false;
        public static string TextCase = "";
        public static int trimno = 1;


        public static long SyncBatchNo = 0;

        public static void setVariable(String fnm, String fyr, String unm, String upss, String utyp, String dbName, DateTime dt1, DateTime dt2)
        {
            fname = fnm;
            fyear = fyr;
            uname = unm;
            utype = utyp;
            upass = upss;
            databaseName = dbName;
            stDate = DateTime.Parse(dt1.ToString("dd-MMM-yyyy"));
            enDate = DateTime.Parse(dt2.ToString("dd-MMM-yyyy"));


            cmonthFst = new DateTime(ldate.Year, ldate.Month, 1);
            lmonthFst = cmonthFst.AddMonths(-1);
            lmonthLst = cmonthFst.AddDays(-1);

            access_sql.fnhashSinglequote();
            access_sql.fnaccbal();

            try
            {
                double.Parse(funs.IndianCurr(123));
                trimno = 1;
            }
            catch (Exception e)
            {
                trimno = 2;
            }


            BranchId = Database.GetScalarText("Select Branch_id from Userinfo where uname='" + uname + "' and Upass='" + upass + "'");
            frmSoftwareUpdates updt = new frmSoftwareUpdates();
            updt.Update();
            CompanyState_id = GetScalarText("SELECT CState_id FROM COMPANY");
            user_id = Database.GetScalarText("Select U_id from Userinfo where Uname='" + uname + "'");
            Depaccesstouser = Database.GetScalarText("Select Department_id from Userinfo where Uname='" + uname + "'");
            BranchGodown_id = Database.GetScalarText("select Godown_id from Branch where id='" + BranchId + "'");
            SHostname = GetScalarText("SELECT hostname FROM COMPANY");
            SUsername = GetScalarText("SELECT username FROM COMPANY");
            SDbname = GetScalarText("SELECT dbname FROM COMPANY");
            SPwd = GetScalarText("SELECT pwd FROM COMPANY");
            Dongleno = Database.GetOtherScalarText("Select Value from Activate where [Column]='Dongle'");


        }

        public static void OpenConnection()
        {
            if (DatabaseType == "sql")
            {

                if (SqlCnn.State == ConnectionState.Closed)
                {
                    SqlCnn.ConnectionString = @"Data Source=" + inipath + ";Initial Catalog=loginfo;Persist Security Info=True;User ID=sa;password=" + sqlseverpwd + ";Connection Timeout=100";
                    SqlCnn.Open();
                }
                else if (SqlConn.State == ConnectionState.Closed && databaseName != null && databaseName != "")
                {
                    SqlConn.ConnectionString = @"Data Source=" + inipath + ";Initial Catalog=" + Database.databaseName + ";Persist Security Info=True;User ID=sa;password=" + sqlseverpwd + ";Connection Timeout=1800";
                    SqlConn.Open();
                }
            }
            else
            {
                SetPath();
                if (AccessCnn.State == ConnectionState.Closed)
                {
                    AccessCnn.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + ServerPath + "\\loginfo\\loginfo.mdb;Persist Security Info=true;Jet OLEDB:Database Password=ptsoft9358524971";
                    AccessCnn.Open();
                }
                else if (AccessConn.State == ConnectionState.Closed && databaseName != null && databaseName != "")
                {
                    AccessConn.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + ServerPath + "\\Database\\" + databaseName + ".mdb;Persist Security Info=true;Jet OLEDB:Database Password=ptsoft9358524971";
                    AccessConn.Open();
                }
            }
        }

        public static void SetPath()
        {
            ServerPath = Application.StartupPath;
        }
        public static void CloseConnection()
        {
            if (DatabaseType == "sql")
            {
                SqlConn.Close();
            }
            else
            {
                AccessConn.Close();
            }
        }

        public static bool CommandExecutor(String str,bool syn= true)
        {
            OpenConnection();
            if (DatabaseType == "sql")
            {
                sqlcmd = new SqlCommand(str, SqlConn);
                try
                {                    

                    sqlcmd.Transaction = sqlTran;
                    if (sqlcmd.ExecuteScalar() != null)
                    {
                        sqlcmd.ExecuteNonQuery();
                    }
                    
                    if (Feature.Available("Active Syncronizer").ToUpper() == "YES" && syn==true)
                    {

                        //DataTable dtSync = new DataTable("Syncronizer");
                        //dtSync.Rows.Add();
                        //dtSync.Columns.Add("Id", typeof(int));
                        //dtSync.Columns.Add("Location", typeof(string));
                        //dtSync.Columns.Add("query", typeof(string));
                        //dtSync.Columns.Add("Updated", typeof(Boolean));
                        //dtSync.Columns.Add("timespan", typeof(string));
                        //DataTable dtCount = new DataTable();
                        //Database.GetSqlData("select count(*) from Syncronizer", dtCount);

                        //dtSync.Rows[dtSync.Rows.Count - 1]["Location"] = Database.LocationId;
                        //dtSync.Rows[dtSync.Rows.Count - 1]["query"] = str;
                        //dtSync.Rows[dtSync.Rows.Count - 1]["Updated"] = false;
                        //dtSync.Rows[dtSync.Rows.Count - 1]["timespan"] = DateTime.Now;
                        //SqlDataAdapter da1 = new SqlDataAdapter("select * from " + dtSync.TableName, SqlConn);
                        //SqlCommandBuilder cb1 = new SqlCommandBuilder();
                        //cb1.QuotePrefix = "[";
                        //cb1.QuoteSuffix = "]";
                        //cb1.DataAdapter = da1;
                        //da1.SelectCommand.Transaction = sqlTran;
                        //da1.Update(dtSync);

                        if (sqlTran == null || sqlTran.Connection == null)
                        {
                            SyncBatchNo = DateTime.Now.ToFileTime();
                        }

                        InsertInSync(str);
                       
                    }
                    
                    return true;
                }
                catch (SqlException ex)
                {
                    if (ex.Message.Substring(0, 5) != "Table" && (ex.Message != "Primary key already exists.") && ex.Message.Substring(0, 5) != "Field" && ex.Message.Substring(0, 10) == "The change")
                    {

                    }
                    return false;
                }

            }
            else
            {
                accesscmd = new OleDbCommand(str, AccessConn);
                try
                {
                    accesscmd.Transaction = AccessTran;
                    accesscmd.ExecuteNonQuery();
                    return true;
                }
                catch (OleDbException ex)
                {
                    if (ex.Message.Substring(0, 5) != "Table" && (ex.Message != "Primary key already exists.") && ex.Message.Substring(0, 5) != "Field" && ex.Message.Substring(0, 10) == "The change")
                    {
                        // System.Windows.Forms.MessageBox.Show(ex.Message);
                    }
                    return false;
                }

            }
            CloseConnection();
        }

        private static bool InsertInSync(string strSql)
        {

            if (DatabaseType == "sql")
            {
                using (SqlCommand _sqlcmd = new SqlCommand("Insert into Syncronizer ([location],[query],[Updated],[timespan],[batchno]) values(@location,@query,@Updated,@timespan,@batchno)", SqlConn, sqlTran))
                {
                    if (SqlConn.State == ConnectionState.Closed) { SqlConn.Open(); }

                    _sqlcmd.Parameters.Add(new SqlParameter("@location", Database.LocationId));
                    _sqlcmd.Parameters.Add(new SqlParameter("@query", strSql));
                    _sqlcmd.Parameters.Add(new SqlParameter("@Updated", false));
                    _sqlcmd.Parameters.Add(new SqlParameter("@timespan", DateTime.Now));
                    _sqlcmd.Parameters.Add(new SqlParameter("@batchno", SyncBatchNo));
                    int iRes = _sqlcmd.ExecuteNonQuery();
                    return true;
                }
            }
            else {

                using (OleDbCommand _sqlcmd = new OleDbCommand("Insert into Syncronizer ([location],[query],[Updated],[timespan],[batchno]) values(?,?,?,?,?)", AccessConn, AccessTran))
                {

                    if (AccessConn.State == ConnectionState.Closed) { AccessConn.Open(); }

                    _sqlcmd.Parameters.Add(new OleDbParameter("?", Database.LocationId));
                    _sqlcmd.Parameters.Add(new OleDbParameter("?", strSql));
                    _sqlcmd.Parameters.Add(new OleDbParameter("?", false));
                    _sqlcmd.Parameters.Add(new OleDbParameter("?", DateTime.Now));
                    _sqlcmd.Parameters.Add(new OleDbParameter("?", SyncBatchNo));
                    int iRes = _sqlcmd.ExecuteNonQuery();
                    return true;
                }
            
            }
            return false;
        }

        public static bool CommandExecutorOther(String str)
        {
            OpenConnection();
            if (DatabaseType == "sql")
            {
                sqlcmd = new SqlCommand(str, SqlCnn);
                try
                {
                    sqlcmd.ExecuteNonQuery();
                    return true;
                }
                catch (OleDbException ex)
                {
                    if (ex.Message.Substring(0, 5) != "Table" && (ex.Message != "Primary key already exists.") && ex.Message.Substring(0, 5) != "Field" && ex.Message.Substring(0, 10) == "The change")
                    {
                        MessageBox.Show(ex.Message);
                    }
                    return false;
                }
                sqlcmd.Dispose();
            }
            else
            {
                accesscmd = new OleDbCommand(str, AccessCnn);
                try
                {
                    accesscmd.ExecuteNonQuery();
                    return true;
                }
                catch (OleDbException ex)
                {
                    if (ex.Message.Substring(0, 5) != "Table" && (ex.Message != "Primary key already exists.") && ex.Message.Substring(0, 5) != "Field" && ex.Message.Substring(0, 10) == "The change")
                    {

                    }
                    return false;
                }
                accesscmd.Dispose();
            }
            CloseConnection();
        }
        public static int GetOtherScalarInt(String str)
        {
            int res = 0;
            OpenConnection();
            if (DatabaseType == "sql")
            {

                SqlCommand cmd = new SqlCommand(str, SqlCnn);
                cmd.Transaction = sqlTrana;
                if (cmd.ExecuteScalar() != null && cmd.ExecuteScalar().ToString() != "")
                {
                    res = int.Parse(cmd.ExecuteScalar().ToString());
                }
                else
                {
                    res = 0;
                }
                if (sqlTrana == null || sqlTrana.Connection == null)
                {
                    CloseConnection();
                }
                cmd.Dispose();

            }
            else
            {
                OleDbCommand cmd = new OleDbCommand(str, AccessCnn);
                cmd.Transaction = AccessTrana;
                if (cmd.ExecuteScalar() != null && cmd.ExecuteScalar().ToString() != "")
                {
                    res = int.Parse(cmd.ExecuteScalar().ToString());
                }
                else
                {
                    res = 0;
                }
                if (AccessTrana == null || AccessTrana.Connection == null)
                {
                    CloseConnection();
                }
                cmd.Dispose();
            }
            return res;
        }

        public static long GetScalarLong(String str)
        {
            long res = 0;
            OpenConnection();
            if (DatabaseType == "sql")
            {
                SqlCommand cmd = new SqlCommand(str, SqlConn);
                cmd.Transaction = sqlTran;
                if (cmd.ExecuteScalar() != null)
                {
                    long x = 0;
                    if (long.TryParse(cmd.ExecuteScalar().ToString(), out x) == true)
                    {
                        res = long.Parse(cmd.ExecuteScalar().ToString());
                    }
                }
                cmd.Dispose();
            }
            else
            {
                OleDbCommand cmd = new OleDbCommand(str, AccessConn);
                cmd.Transaction = AccessTran;
                if (cmd.ExecuteScalar() != null)
                {
                    long x = 0;
                    if (long.TryParse(cmd.ExecuteScalar().ToString(), out x) == true)
                    {
                        res = long.Parse(cmd.ExecuteScalar().ToString());
                    }
                    else
                    {
                        res = 0;
                    }

                }
                cmd.Dispose();
            }
            if (AccessTran == null || AccessTran.Connection == null)
            {
                CloseConnection();
            }


            if (sqlTran == null || sqlTran.Connection == null)
            {
                CloseConnection();
            }
            return res;
        }


        public static int CommandExecutorInt(String str)
        {
            OpenConnection();
            if (DatabaseType == "sql")
            {
                sqlcmd = new SqlCommand(str, SqlConn);
                try
                {
                    sqlcmd.Transaction = sqlTran;
                    return sqlcmd.ExecuteNonQuery();
                    if (Feature.Available("Active Syncronizer").ToUpper() == "YES")
                    {
                        //DataTable dtSync = new DataTable("Syncronizer");
                        //dtSync.Rows.Add();
                        //dtSync.Columns.Add("Id", typeof(int));
                        //dtSync.Columns.Add("Location", typeof(string));
                        //dtSync.Columns.Add("query", typeof(string));
                        //dtSync.Columns.Add("Updated", typeof(Boolean));
                        //dtSync.Columns.Add("timespan", typeof(string));
                        //DataTable dtCount = new DataTable();
                        //Database.GetSqlData("select count(*) from Syncronizer", dtCount);

                        //dtSync.Rows[dtSync.Rows.Count - 1]["Location"] = Database.LocationId;
                        //dtSync.Rows[dtSync.Rows.Count - 1]["query"] = str;
                        //dtSync.Rows[dtSync.Rows.Count - 1]["Updated"] = false;
                        //dtSync.Rows[dtSync.Rows.Count - 1]["timespan"] = DateTime.Now;
                        //SqlDataAdapter da1 = new SqlDataAdapter("select * from " + dtSync.TableName, SqlConn);
                        //SqlCommandBuilder cb1 = new SqlCommandBuilder();
                        //cb1.QuotePrefix = "[";
                        //cb1.QuoteSuffix = "]";
                        //cb1.DataAdapter = da1;
                        //da1.SelectCommand.Transaction = sqlTran;
                        //da1.Update(dtSync);

                        if (sqlTran == null || sqlTran.Connection == null)
                        {
                            SyncBatchNo = DateTime.Now.ToFileTime();
                        }

                        InsertInSync(str);
                    }




                }
                catch (SqlException ex)
                {
                    //System.Windows.Forms.MessageBox.Show(ex.Message);
                    return 0;
                }
            }
            else
            {
                accesscmd = new OleDbCommand(str, AccessConn);
                try
                {
                    accesscmd.Transaction = AccessTran;
                    return accesscmd.ExecuteNonQuery();
                }
                catch (OleDbException ex)
                {
                    return 0;
                }
            }

        }
        public static bool OtherCommandExecutor(String str)
        {
            OpenConnection();
            accesscmd = new OleDbCommand(str, AccessCnn);
            try
            {
                accesscmd.ExecuteNonQuery();
                return true;
            }
            catch (OleDbException ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
            return false;
        }

        public static void SaveOtherData(DataTable dt)
        {
            if (DatabaseType == "sql")
            {
                SqlDataAdapter da = new SqlDataAdapter("select * from " + dt.TableName, SqlCnn);
                SqlCommandBuilder cb = new SqlCommandBuilder();
                cb.QuotePrefix = "[";
                cb.QuoteSuffix = "]";
                cb.DataAdapter = da;
                da.Update(dt);
            }
            else
            {
                OleDbDataAdapter da = new OleDbDataAdapter("select * from " + dt.TableName, AccessCnn);
                OleDbCommandBuilder cb = new OleDbCommandBuilder();
                cb.QuotePrefix = "[";
                cb.QuoteSuffix = "]";
                cb.DataAdapter = da;
                da.Update(dt);
            }
        }




        public static void SaveData(DataTable dt)
        {
            if (dt == null) return;
            if (DatabaseType == "sql")
            {

                SqlDataAdapter da = new SqlDataAdapter("select * from " + dt.TableName, SqlConn);
                SqlCommandBuilder cb = new SqlCommandBuilder();
                cb.ConflictOption = ConflictOption.CompareRowVersion;
                cb.QuotePrefix = "[";
                cb.QuoteSuffix = "]";
                cb.DataAdapter = da;
                da.SelectCommand.Transaction = sqlTran;
                if (Feature.Available("Active Syncronizer").ToUpper() == "YES")
                {
                    if (sqlTran == null || sqlTran.Connection == null)
                    {
                        SyncBatchNo = DateTime.Now.ToFileTime();
                    }

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string Sql = "";

                        if (dt.Rows[i].RowState == DataRowState.Added)
                        {
                            Sql = cb.GetInsertCommand().CommandText;
                            for (int j = cb.GetInsertCommand().Parameters.Count - 1; j >= 0; j--)
                            {
                                if (cb.GetInsertCommand().Parameters[j].DbType.ToString() == "Double" || cb.GetInsertCommand().Parameters[j].DbType.ToString() == "Currency" || cb.GetInsertCommand().Parameters[j].DbType.ToString() == "Int32")
                                {
                                    if (dt.Rows[i][cb.GetInsertCommand().Parameters[j].SourceColumn].ToString() != "")
                                    {
                                        Sql = Sql.Replace(cb.GetInsertCommand().Parameters[j].ParameterName, dt.Rows[i][cb.GetInsertCommand().Parameters[j].SourceColumn].ToString());
                                    }
                                    else
                                    {
                                        Sql = Sql.Replace(cb.GetInsertCommand().Parameters[j].ParameterName, "0");
                                    }
                                }
                                else if (cb.GetInsertCommand().Parameters[j].DbType.ToString() == "DateTime")
                                {

                                    Sql = Sql.Replace(cb.GetInsertCommand().Parameters[j].ParameterName, "'" + DateTime.Parse(dt.Rows[i][cb.GetInsertCommand().Parameters[j].SourceColumn].ToString()).ToString("dd-MMM-yyyy") + "'");
                                }
                                else
                                {
                                    Sql = Sql.Replace(cb.GetInsertCommand().Parameters[j].ParameterName, "'" + dt.Rows[i][cb.GetInsertCommand().Parameters[j].SourceColumn].ToString().Replace("'", "'+char(39)+'") + "'");
                                }
                            }
                        }
                        else if (dt.Rows[i].RowState == DataRowState.Modified)
                        {
                            Sql = cb.GetUpdateCommand().CommandText;
                            for (int j = cb.GetUpdateCommand().Parameters.Count - 1; j >= 0; j--)
                            {
                                if (cb.GetUpdateCommand().Parameters[j].GetType().FullName == "System.Double" || cb.GetUpdateCommand().Parameters[j].GetType().FullName == "System.Int32")
                                {
                                    if (j >= dt.Columns.Count)
                                    {
                                        Sql = Sql.Replace(cb.GetUpdateCommand().Parameters[j].ParameterName, dt.Rows[i][cb.GetUpdateCommand().Parameters[j].SourceColumn, DataRowVersion.Original].ToString());
                                    }
                                    else
                                    {
                                        Sql = Sql.Replace(cb.GetUpdateCommand().Parameters[j].ParameterName, dt.Rows[i][cb.GetUpdateCommand().Parameters[j].SourceColumn].ToString());
                                    }
                                }
                                else if (cb.GetUpdateCommand().Parameters[j].DbType.ToString() == "DateTime")
                                {
                                    if (j >= dt.Columns.Count)
                                    {
                                        Sql = Sql.Replace(cb.GetUpdateCommand().Parameters[j].ParameterName, "'" + DateTime.Parse(dt.Rows[i][cb.GetUpdateCommand().Parameters[j].SourceColumn, DataRowVersion.Original].ToString()).ToString("dd-MMM-yyyy") + "'");
                                    }
                                    else
                                    {
                                        //if (dt.Rows[i][cb.GetUpdateCommand().Parameters[j].SourceColumn].ToString() == "")
                                        //{
                                        //    dt.Rows[i][cb.GetUpdateCommand().Parameters[j].SourceColumn] = "02-01-1801";
                                        //}

                                        Sql = Sql.Replace(cb.GetUpdateCommand().Parameters[j].ParameterName, "'" + DateTime.Parse(dt.Rows[i][cb.GetUpdateCommand().Parameters[j].SourceColumn].ToString()).ToString("dd-MMM-yyyy") + "'");
                                    }
                                }
                                else
                                {
                                    if (j >= dt.Columns.Count - 1)
                                    {
                                        Sql = Sql.Replace(cb.GetUpdateCommand().Parameters[j].ParameterName, "'" + dt.Rows[i][cb.GetUpdateCommand().Parameters[j].SourceColumn, DataRowVersion.Original].ToString() + "'");
                                    }
                                    else
                                    {
                                        Sql = Sql.Replace(cb.GetUpdateCommand().Parameters[j].ParameterName, "'" + dt.Rows[i][cb.GetUpdateCommand().Parameters[j].SourceColumn].ToString() + "'");
                                    }
                                }
                            }

                        }
                        else if (dt.Rows[i].RowState == DataRowState.Deleted)
                        {
                            Sql = cb.GetDeleteCommand().CommandText;
                            for (int j = cb.GetDeleteCommand().Parameters.Count - 1; j >= 0; j--)
                            {
                                if (cb.GetDeleteCommand().Parameters[j].GetType().FullName == "System.Double" || cb.GetDeleteCommand().Parameters[j].GetType().FullName == "System.Int32")
                                {
                                    Sql = Sql.Replace(cb.GetDeleteCommand().Parameters[j].ParameterName, dt.Rows[i][cb.GetDeleteCommand().Parameters[j].SourceColumn, DataRowVersion.Original].ToString());
                                }
                                else if (cb.GetDeleteCommand().Parameters[j].DbType.ToString() == "DateTime")
                                {

                                    Sql = Sql.Replace(cb.GetDeleteCommand().Parameters[j].ParameterName, "'" + DateTime.Parse(dt.Rows[i][cb.GetDeleteCommand().Parameters[j].SourceColumn, DataRowVersion.Original].ToString()).ToString("dd-MMM-yyyy") + "'");
                                }
                                else
                                {
                                    Sql = Sql.Replace(cb.GetDeleteCommand().Parameters[j].ParameterName, "'" + dt.Rows[i][cb.GetDeleteCommand().Parameters[j].SourceColumn, DataRowVersion.Original].ToString() + "'");
                                }
                            }
                        }

                        //dt.Rows[i][j].ToString() == "" ? null :
                        //Database.CommandExecutor("insert into Syncronizer(Location,query) values('" + Database.terminal + "','" + Sql + "')");

                        if (dt.Rows[i].RowState != DataRowState.Unchanged)
                        {
                            //DataTable dtSync = new DataTable("Syncronizer");
                            //dtSync.Rows.Add();
                            //dtSync.Columns.Add("Id", typeof(int));
                            //dtSync.Columns.Add("Location", typeof(string));
                            //dtSync.Columns.Add("query", typeof(string));
                            //dtSync.Columns.Add("Updated", typeof(Boolean));
                            //dtSync.Columns.Add("timespan", typeof(string));
                            //DataTable dtCount = new DataTable();
                            //Database.GetSqlData("select count(*) from Syncronizer", dtCount);

                            //DataRow dtNewRow = dtSync.Rows.Add();

                            //dtNewRow["Location"] = Database.LocationId;
                            //dtNewRow["query"] = Sql;
                            //dtNewRow["Updated"] = false;
                            //dtNewRow["timespan"] = DateTime.Now;
                            //dtNewRow["BatchNo"] = SyncBatchNo;

                            //dtSync.TableName = "Syncronizer";

                            //SqlDataAdapter da1 = new SqlDataAdapter("select * from " + dtSync.TableName, SqlConn);
                            //SqlCommandBuilder cb1 = new SqlCommandBuilder();
                            //cb1.QuotePrefix = "[";
                            //cb1.QuoteSuffix = "]";
                            //cb1.DataAdapter = da1;
                            //da1.SelectCommand.Transaction = sqlTran;
                            //da1.Update(dtSync);

                            InsertInSync(Sql);
                        }
                    }


                }

                da.Update(dt);
            }
            else
            {
                OleDbDataAdapter da = new OleDbDataAdapter("select * from " + dt.TableName, AccessConn);
                OleDbCommandBuilder cb = new OleDbCommandBuilder();
                cb.ConflictOption = ConflictOption.CompareRowVersion;
                cb.QuotePrefix = "[";
                cb.QuoteSuffix = "]";
                cb.DataAdapter = da;
                da.SelectCommand.Transaction = AccessTran;

                if (Feature.Available("Active Syncronizer").ToUpper() == "YES")
                {

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string Sql = "";
                        if (dt.Rows[i].RowState == DataRowState.Added)
                        {
                            Sql = cb.GetInsertCommand().CommandText;
                            for (int j = cb.GetInsertCommand().Parameters.Count - 1; j >= 0; j--)
                            {
                                if (cb.GetInsertCommand().Parameters[j].GetType().FullName == "System.Double" || cb.GetInsertCommand().Parameters[j].GetType().FullName == "System.Int32")
                                {
                                    Sql = Sql.Replace(cb.GetInsertCommand().Parameters[j].ParameterName, dt.Rows[i][cb.GetInsertCommand().Parameters[j].SourceColumn].ToString());
                                }
                                else
                                {
                                    Sql = Sql.Replace(cb.GetInsertCommand().Parameters[j].ParameterName, "'" + dt.Rows[i][cb.GetInsertCommand().Parameters[j].SourceColumn].ToString() + "'");
                                }
                            }
                        }
                        else if (dt.Rows[i].RowState == DataRowState.Modified)
                        {
                            Sql = cb.GetUpdateCommand().CommandText;
                            for (int j = cb.GetUpdateCommand().Parameters.Count - 1; j >= 0; j--)
                            {
                                if (cb.GetUpdateCommand().Parameters[j].GetType().FullName == "System.Double" || cb.GetUpdateCommand().Parameters[j].GetType().FullName == "System.Int32")
                                {
                                    if (j >= dt.Columns.Count)
                                    {
                                        Sql = Sql.Replace(cb.GetUpdateCommand().Parameters[j].ParameterName, dt.Rows[i][cb.GetUpdateCommand().Parameters[j].SourceColumn, DataRowVersion.Original].ToString());
                                    }
                                    else
                                    {
                                        Sql = Sql.Replace(cb.GetUpdateCommand().Parameters[j].ParameterName, dt.Rows[i][cb.GetUpdateCommand().Parameters[j].SourceColumn].ToString());
                                    }
                                }
                                else
                                {
                                    if (j >= dt.Columns.Count)
                                    {
                                        Sql = Sql.Replace(cb.GetUpdateCommand().Parameters[j].ParameterName, "'" + dt.Rows[i][cb.GetUpdateCommand().Parameters[j].SourceColumn, DataRowVersion.Original].ToString() + "'");
                                    }
                                    else
                                    {
                                        Sql = Sql.Replace(cb.GetUpdateCommand().Parameters[j].ParameterName, "'" + dt.Rows[i][cb.GetUpdateCommand().Parameters[j].SourceColumn].ToString() + "'");
                                    }
                                }
                            }

                        }
                        else if (dt.Rows[i].RowState == DataRowState.Deleted)
                        {
                            Sql = cb.GetDeleteCommand().CommandText;
                            for (int j = cb.GetDeleteCommand().Parameters.Count - 1; j >= 0; j--)
                            {
                                if (cb.GetDeleteCommand().Parameters[j].GetType().FullName == "System.Double" || cb.GetDeleteCommand().Parameters[j].GetType().FullName == "System.Int32")
                                {
                                    Sql = Sql.Replace(cb.GetDeleteCommand().Parameters[j].ParameterName, dt.Rows[i][cb.GetDeleteCommand().Parameters[j].SourceColumn, DataRowVersion.Original].ToString());
                                }
                                else
                                {
                                    Sql = Sql.Replace(cb.GetDeleteCommand().Parameters[j].ParameterName, "'" + dt.Rows[i][cb.GetDeleteCommand().Parameters[j].SourceColumn, DataRowVersion.Original].ToString() + "'");
                                }
                            }
                        }

                        if (dt.Rows[i].RowState != DataRowState.Unchanged)
                        {

                            //DataTable dtSync = new DataTable("Syncronizer");
                            //dtSync.Rows.Add();

                            //dtSync.Columns.Add("Location", typeof(string));
                            //dtSync.Columns.Add("query", typeof(string));
                            //dtSync.Columns.Add("Updated", typeof(Boolean));
                            //dtSync.Columns.Add("timespan", typeof(string));
                            //DataTable dtCount = new DataTable();
                            //Database.GetSqlData("select count(*) from Syncronizer", dtCount);

                            //DataRow dtNewRow = dtSync.Rows.Add();

                            //dtNewRow["Location"] = Database.LocationId;
                            //dtNewRow["query"] = Sql;
                            //dtNewRow["Updated"] = false;
                            //dtNewRow["timespan"] = DateTime.Now;
                            //dtNewRow["BatchNo"] = SyncBatchNo;

                            //OleDbDataAdapter da1 = new OleDbDataAdapter("select * from " + dtSync.TableName, AccessConn);
                            //OleDbCommandBuilder cb1 = new OleDbCommandBuilder();
                            //cb1.QuotePrefix = "[";
                            //cb1.QuoteSuffix = "]";
                            //cb1.DataAdapter = da1;
                            //da1.SelectCommand.Transaction = AccessTran;
                            //da1.Update(dtSync);

                            InsertInSync(Sql);
                        }
                    }

                }


                da.Update(dt);
            }
        }


        public static void SaveData(DataTable dt, String str)
        {
            if (DatabaseType == "sql")
            {
                SqlDataAdapter da = new SqlDataAdapter(str, SqlConn);
                SqlCommandBuilder cb = new SqlCommandBuilder(da);
                da.SelectCommand.Transaction = sqlTran;
                da.Update(dt);
            }
            else
            {
                OleDbDataAdapter da = new OleDbDataAdapter(str, AccessConn);
                OleDbCommandBuilder cb = new OleDbCommandBuilder(da);
                da.SelectCommand.Transaction = AccessTran;
                da.Update(dt);
            }
        }

        public static void CommitTran()
        {
            if (DatabaseType == "sql")
            {
                sqlTran.Commit();
            }
            else
            {
                AccessTran.Commit();
            }

        }

        public static void RollbackTran()
        {
            if (DatabaseType == "sql")
            {
                sqlTran.Rollback();
            }
            else
            {
                AccessTran.Rollback();
            }
        }

        public static void BeginTran()
        {
            if (DatabaseType == "sql")
            {
                if (SqlConn.State == ConnectionState.Closed)
                {
                    SqlConn.Open();
                }
                sqlTran = SqlConn.BeginTransaction();
                SyncBatchNo = DateTime.Now.ToFileTime();
            }
            else
            {
                if (AccessConn.State == ConnectionState.Closed)
                {
                    AccessConn.Open();
                }
                AccessTran = AccessConn.BeginTransaction();
                SyncBatchNo = DateTime.Now.ToFileTime();
            }
        }

        public static void GetSqlDataNotClear(String str, DataTable dt)
        {

            if (DatabaseType == "sql")
            {
                SqlDataAdapter da = new SqlDataAdapter(str, SqlConn);
                da.SelectCommand.CommandTimeout = 180;
                da.SelectCommand.Transaction = sqlTran;
                da.Fill(dt);



            }
            else
            {
                OleDbDataAdapter da = new OleDbDataAdapter(str, AccessConn);
                da.SelectCommand.Transaction = AccessTran;
                da.Fill(dt);
            }
        }
        public static void GetSqlData(String str, DataTable dt)
        {
            dt.Clear();
            if (DatabaseType == "sql")
            {
                SqlDataAdapter da = new SqlDataAdapter(str, SqlConn);
                da.SelectCommand.CommandTimeout = 360;
                da.SelectCommand.Transaction = sqlTran;

                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {

                }
                else
                {

                    da.Dispose();
                }

            }
            else
            {
                OleDbDataAdapter da = new OleDbDataAdapter(str, AccessConn);
                da.SelectCommand.Transaction = AccessTran;
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                }
                else
                {

                    da.Dispose();
                }
            }

        }


        public static void GetOtherSqlData(String str, DataTable dt)
        {
            dt.Clear();
            SetPath();
            if (DatabaseType == "sql")
            {
                if (SqlCnn.ConnectionString == "")
                {
                    SqlCnn.ConnectionString = @"Data Source=" + inipath + ";Initial Catalog=loginfo;Persist Security Info=True;User ID=sa;password=" + sqlseverpwd + ";Connection Timeout=100";
                }


                SqlDataAdapter da = new SqlDataAdapter(str, SqlCnn);
                da.Fill(dt);
            }
            else
            {
                if (AccessCnn.ConnectionString == "")
                {
                    AccessCnn.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + ServerPath + "\\loginfo\\loginfo.mdb;Persist Security Info=true;Jet OLEDB:Database Password=ptsoft9358524971";
                }
                OleDbDataAdapter da = new OleDbDataAdapter(str, AccessCnn);
                da.Fill(dt);
            }
        }

        public static int GetScalar(String str)
        {
            int res;
            OpenConnection();
            if (DatabaseType == "sql")
            {
                SqlCommand cmd = new SqlCommand(str, SqlConn);
                cmd.Transaction = sqlTran;
                res = int.Parse(cmd.ExecuteScalar().ToString());
                cmd.Dispose();

                if (sqlTran == null || sqlTran.Connection == null)
                {
                    CloseConnection();
                }
            }
            else
            {
                OleDbCommand cmd = new OleDbCommand(str, AccessConn);
                cmd.Transaction = AccessTran;
                res = int.Parse(cmd.ExecuteScalar().ToString());
                cmd.Dispose();
                if (AccessTran == null || AccessTran.Connection == null)
                {
                    CloseConnection();
                }
            }

            return res;
        }


        public static String GetScalarText(String str)
        {
            String res = "";
            OpenConnection();
            if (DatabaseType == "sql")
            {
                SqlCommand cmd = new SqlCommand(str, SqlConn);
                cmd.Transaction = sqlTran;
                if (cmd.ExecuteScalar() != null)
                {
                    res = cmd.ExecuteScalar().ToString();
                }
                cmd.Dispose();
                if (sqlTran == null || sqlTran.Connection == null)
                {
                    CloseConnection();
                }
            }
            else
            {
                OleDbCommand cmd = new OleDbCommand(str, AccessConn);
                cmd.Transaction = AccessTran;
                if (cmd.ExecuteScalar() != null)
                {
                    res = cmd.ExecuteScalar().ToString();

                }
                cmd.Dispose();
                if (AccessTran == null || AccessTran.Connection == null)
                {
                    CloseConnection();
                }

            }




            return res;
        }



        public static String GetOtherScalarText(String str)
        {
            String res = "";

            if (DatabaseType == "sql")
            {
                SqlCommand cmd = new SqlCommand(str, SqlCnn);

                if (cmd.ExecuteScalar() != null)
                {
                    res = cmd.ExecuteScalar().ToString();
                }

                cmd.Dispose();
            }
            else
            {

                OleDbCommand cmd = new OleDbCommand(str, AccessCnn);

                if (cmd.ExecuteScalar() != null)
                {
                    res = cmd.ExecuteScalar().ToString();
                }

                cmd.Dispose();
            }
            return res;
        }


        public static int GetScalarInt(String str)
        {
            int res = 0;
            OpenConnection();
            if (DatabaseType == "sql")
            {
                // OpenConnection();
                SqlCommand cmd = new SqlCommand(str, SqlConn);
                cmd.Transaction = sqlTran;

                if (cmd.ExecuteScalar() != null && cmd.ExecuteScalar().ToString() != "")
                {
                    res = int.Parse(cmd.ExecuteScalar().ToString());
                }
                else
                {
                    res = 0;
                }
                cmd.Dispose();

                if (sqlTran == null || sqlTran.Connection == null)
                {
                    CloseConnection();

                }
            }
            else
            {
                //OpenConnection();
                OleDbCommand cmd = new OleDbCommand(str, AccessConn);
                cmd.Transaction = AccessTran;
                if (cmd.ExecuteScalar() != null && cmd.ExecuteScalar().ToString() != "")
                {
                    res = int.Parse(cmd.ExecuteScalar().ToString());
                }
                else
                {
                    res = 0;
                }
                cmd.Dispose();
                if (AccessTran == null || AccessTran.Connection == null)
                {
                    CloseConnection();
                }
            }


            return res;
        }


        public static String GetScalarDate(String str)
        {
            String res = "";
            OpenConnection();
            if (DatabaseType == "sql")
            {
                SqlCommand cmd = new SqlCommand(str, SqlConn);
                cmd.Transaction = sqlTran;
                // res = DateTime.Parse(cmd.ExecuteScalar().ToString()).ToString("dd-MMM-yyyy");
                if (cmd.ExecuteScalar().ToString() != null && cmd.ExecuteScalar().ToString() != "")
                {

                    res = DateTime.Parse(cmd.ExecuteScalar().ToString()).ToString("dd-MMM-yyyy");
                }
                else
                {
                    res = "";
                }
                cmd.Dispose();
                if (sqlTran == null || sqlTran.Connection == null)
                {
                    CloseConnection();
                }
            }
            else
            {
                OleDbCommand cmd = new OleDbCommand(str, AccessConn);
                cmd.Transaction = AccessTran;
                if (cmd.ExecuteScalar() != null && cmd.ExecuteScalar().ToString() != "")
                {

                    res = DateTime.Parse(cmd.ExecuteScalar().ToString()).ToString("dd-MMM-yyyy");
                }
                else
                {
                    res = "";
                }
                cmd.Dispose();
                if (AccessTran == null || AccessTran.Connection == null)
                {
                    CloseConnection();
                }
            }




            return res;
        }

        public static bool GetScalarBool(String str)
        {
            bool res = false;
            OpenConnection();
            if (DatabaseType == "sql")
            {
                SqlCommand cmd = new SqlCommand(str, SqlConn);
                cmd.Transaction = sqlTran;
                if (cmd.ExecuteScalar() != null && cmd.ExecuteScalar().ToString() != "")
                {

                    res = bool.Parse(cmd.ExecuteScalar().ToString());
                }
                else
                {
                    res = false;
                }
                cmd.Dispose();
                if (sqlTran == null || sqlTran.Connection == null)
                {
                    CloseConnection();
                }
            }
            else
            {
                OleDbCommand cmd = new OleDbCommand(str, AccessConn);
                cmd.Transaction = AccessTran;
                if (cmd.ExecuteScalar() != null && cmd.ExecuteScalar().ToString() != "")
                {

                    res = bool.Parse(cmd.ExecuteScalar().ToString());
                }
                else
                {
                    res = false;
                }
                cmd.Dispose();
                if (AccessTran == null || AccessTran.Connection == null)
                {
                    CloseConnection();
                }
            }


            return res;
        }

        public static bool GetOtherScalarBool(String str)
        {


            bool res = false;
            OpenConnection();
            if (DatabaseType == "sql")
            {
                SqlCommand cmd = new SqlCommand(str, SqlCnn);
                cmd.Transaction = sqlTran;
                if (cmd.ExecuteScalar() != null && cmd.ExecuteScalar().ToString() != "")
                {

                    res = bool.Parse(cmd.ExecuteScalar().ToString());
                }
                else
                {
                    res = false;
                }
                cmd.Dispose();
                if (sqlTran == null || sqlTran.Connection == null)
                {
                    CloseConnection();
                }
            }
            else
            {
                OleDbCommand cmd = new OleDbCommand(str, AccessCnn);
                cmd.Transaction = AccessTran;
                if (cmd.ExecuteScalar() != null && cmd.ExecuteScalar().ToString() != "")
                {

                    res = bool.Parse(cmd.ExecuteScalar().ToString());
                }
                else
                {
                    res = false;
                }
                cmd.Dispose();
                if (AccessTran == null || AccessTran.Connection == null)
                {
                    CloseConnection();
                }
            }


            return res;
        }


        public static Double GetScalarDecimal(String str)
        {
            Double res = 0;

            OpenConnection();
            if (DatabaseType == "sql")
            {
                SqlCommand cmd = new SqlCommand(str, SqlConn);
                cmd.Transaction = sqlTran;
                if (cmd.ExecuteScalar() != null)
                {
                    if (funs.isDouble(cmd.ExecuteScalar().ToString()))
                    {

                        res = Double.Parse(cmd.ExecuteScalar().ToString());
                    }
                    else
                    {
                        res = 0;
                    }
                }
                cmd.Dispose();
                if (sqlTran == null || sqlTran.Connection == null)
                {
                    CloseConnection();
                }
            }
            else
            {
                OleDbCommand cmd = new OleDbCommand(str, AccessConn);
                cmd.Transaction = AccessTran;
                if (cmd.ExecuteScalar() != null)
                {
                    if (funs.isDouble(cmd.ExecuteScalar().ToString()))
                    {

                        res = Double.Parse(cmd.ExecuteScalar().ToString());
                    }
                    else
                    {
                        res = 0;
                    }
                }
                cmd.Dispose();
                if (AccessTran == null || AccessTran.Connection == null)
                {
                    CloseConnection();
                }
            }


            return res;

        }

        public static void setFocus(TextBox tb)
        {
            tb.BackColor = System.Drawing.Color.AntiqueWhite;
            tb.ForeColor = System.Drawing.Color.Black;
        }


        public static void setFocus(RadioButton tb)
        {
            tb.BackColor = System.Drawing.Color.AntiqueWhite;
            tb.ForeColor = System.Drawing.Color.Black;
        }





        public static void lostFocus(RadioButton tb)
        {
            tb.BackColor = System.Drawing.Color.White;
            tb.ForeColor = System.Drawing.Color.Black;
        }

        public static void lostFocus(TextBox tb)
        {
            if (TextCase == "To UpperCase")
            {
                tb.Text = CultureInfo.CurrentCulture.TextInfo.ToUpper(tb.Text);
            }
            else if (TextCase == "To LowerCase")
            {
                tb.Text = CultureInfo.CurrentCulture.TextInfo.ToLower(tb.Text);
            }
            else if (TextCase == "To CamelCase")
            {
                tb.Text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(tb.Text);
            }
            tb.BackColor = System.Drawing.Color.White;
            tb.ForeColor = System.Drawing.Color.Black;
        }

        public static void setFocus(DateTimePicker dtp)
        {
            dtp.BackColor = System.Drawing.Color.AntiqueWhite;
            dtp.CalendarMonthBackground = System.Drawing.Color.Black;
        }

        public static void lostFocus(DateTimePicker dtp)
        {
            dtp.BackColor = System.Drawing.Color.White;
            dtp.ForeColor = System.Drawing.Color.Black;
        }

        public static void setFocus(DataGridViewCell cell)
        {
            cell.Style.BackColor = System.Drawing.Color.AntiqueWhite;
            cell.Style.ForeColor = System.Drawing.Color.Black;
        }

        public static void lostFocus(DataGridViewCell cell)
        {
            cell.Style.BackColor = System.Drawing.Color.White;
            cell.Style.ForeColor = System.Drawing.Color.Black;
        }


        public static void FillList(ListBox lb, String str)
        {
            DataTable dtList = new DataTable();
            dtList.Clear();
            GetSqlData(str, dtList);
            lb.DataSource = dtList;
            lb.DisplayMember = dtList.Columns[0].ColumnName;
        }

        public static void FillCombo(ComboBox cb, String str)
        {
            DataTable dtCombo = new DataTable();
            dtCombo.Clear();
            GetSqlData(str, dtCombo);
            cb.DataSource = dtCombo;
            cb.DisplayMember = dtCombo.Columns[0].ColumnName;
        }

        public static void FillCombo(DataGridViewComboBoxColumn gvcb, String str)
        {
            DataTable dtCombo = new DataTable();
            dtCombo.Clear();
            GetSqlData(str, dtCombo);
            gvcb.DataSource = dtCombo;
            gvcb.DisplayMember = dtCombo.Columns[0].ColumnName;
        }

        public static void FillCombo(ComboBox cb, DataTable dtStr, String colName)
        {
            cb.DataSource = dtStr;
            cb.DisplayMember = dtStr.Columns[colName].ColumnName;
        }
    }
}
