using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;

using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Data.OleDb;

namespace faspi
{
    class access_sql
    {
        public static string accbalq = "";
        public static String Hash = "";
        public static String Singlequote = "";
        public static String QryJournal = "";
        public static String Docnumber = "";
        public static String Docnumber1 = "";
        public static String EditVoucher = "";
        public static String Svnum = "";
        public static String IsNull = "";
        public static String Concat = "";
        public static String DateFormat = "";

        public static void setconnectionold()
        {
            try
            {
                FileInfo fInfo = new FileInfo(Application.StartupPath + "\\connect.ini");
                if (fInfo.Exists)
                {
                    Database.inipathfile = System.IO.File.ReadAllText(Application.StartupPath + "\\connect.ini");
                }
                else
                {
                    //create ini file with text
                    File.Create(Application.StartupPath + "\\connect.ini").Dispose();
                    TextWriter tw = new StreamWriter(Application.StartupPath + "\\connect.ini");
                    tw.WriteLine("access;");
                    tw.Close();
                    Database.inipathfile = System.IO.File.ReadAllText(Application.StartupPath + "\\connect.ini");
                }
                String[] val = Database.inipathfile.Split(';');
                Database.DatabaseType = val[0];

                if (Database.DatabaseType == "sql")
                {
                    Database.inipath = val[1];
                    Database.sqlseverpwd = val[2];
                }
                else
                {
                    Database.inipath = "";
                    Database.sqlseverpwd = "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Connection File is not Available.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                Environment.Exit(0);
            }
        }

        public static void setconnection()
        {
            try
            {
                FileInfo fInfo = new FileInfo(Application.StartupPath + "\\connect.ini");
                if (fInfo.Exists)
                {
                    Database.inipathfile = System.IO.File.ReadAllText(Application.StartupPath + "\\connect.ini").Replace("\n", "");
                    Database.inipathfile = Database.inipathfile.Replace("\r", "");

                    string stradd = System.IO.File.ReadAllText(Application.StartupPath + "\\connect.ini").Replace("\n", "");
                    String[] val = stradd.Replace("\r", "").Split(';');
                    Database.DatabaseType = val[0];

                    if (Database.DatabaseType == "access")
                    {
                        if (Database.inipathfile == "access;")
                        {
                            TextWriter tw = new StreamWriter(Application.StartupPath + "\\connect.ini");
                            tw.WriteLine("access;loginfo;SER;");
                            tw.Close();
                        }
                    }
                    else if (Database.DatabaseType == "sql")
                    {
                        string stradd1 = System.IO.File.ReadAllText(Application.StartupPath + "\\connect.ini").Replace("\n", "");
                        String[] val1 = stradd1.Replace("\r", "").Split(';');

                        if (val1.Length == 3)
                        {

                            File.AppendAllText(Application.StartupPath + "\\connect.ini", ";loginfo;SER;");
                        }
                    }
                }
                else
                {
                    //create ini file with text
                    File.Create(Application.StartupPath + "\\connect.ini").Dispose();
                    TextWriter tw = new StreamWriter(Application.StartupPath + "\\connect.ini");
                    tw.WriteLine("access;loginfo;SER;");
                    tw.Close();
                    Database.inipathfile = System.IO.File.ReadAllText(Application.StartupPath + "\\connect.ini").Replace("\n", "");
                    Database.inipathfile = Database.inipathfile.Replace("\r", "");
                }
                string stradd2 = System.IO.File.ReadAllText(Application.StartupPath + "\\connect.ini").Replace("\n", "");
                String[] val2 = stradd2.Replace("\r", "").Split(';');
                Database.DatabaseType = val2[0];
                if (Database.DatabaseType == "sql")
                {
                    Database.inipath = val2[1];
                    Database.sqlseverpwd = val2[2];
                    Database.loginfoName = val2[3];
                    Database.LocationId = val2[4];
                }
                else
                {
                    Database.inipath = "";
                    Database.sqlseverpwd = "";
                    Database.loginfoName = val2[1];
                    Database.LocationId = val2[2];
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Connection File is not Available.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                Environment.Exit(0);
            }
        }
        public static void fnhashSinglequote()
        {
            if (Database.DatabaseType == "access")
            {
                Hash = "#";
                Singlequote = "";
                Docnumber = " VOUCHERTYPE.Short & ' ' & Format(VOUCHERINFO.Vdate,'yyyymmdd' & ' ' & VOUCHERINFO.Vnumber) ";
                Svnum = "' Bill No.' & Svnum & ' Dt. ' & Format(Svdate,'dd-mmm-yyyy')";
                DateFormat = " Format$(Voucherinfo.Vdate,'dd-mmm-yyyy')";
                Docnumber1 = " VOUCHERTYPE_1.Short & ' ' & Format(JOURNAL_1.Vdate,'yyyymmdd' & ' ' & VOUCHERINFO_1.Vnumber) AS DocNumber2 ";
                EditVoucher = "select act_id & '' as Code,Name as AccountType from accountype where type='Account'";
                IsNull = "=";
                Concat = "&";
            }
            else
            {
                Hash = "'";
                Singlequote = "'";
                Docnumber = " VOUCHERTYPE.Short + ' ' + CONVERT(nvarchar, VOUCHERINFO.Vdate, 112) + ' ' + CAST(VOUCHERINFO.Vnumber AS nvarchar(10)) ";
                Svnum = "' Bill No.' + Svnum + ' Dt. ' + CONVERT(nvarchar,Svdate, 106)";
                DateFormat = "CONVERT(nvarchar,Voucherinfo.Vdate, 112)";
                Docnumber1 = " VOUCHERTYPE_1.Short + ' ' + CONVERT(nvarchar,JOURNAL_1.Vdate, 112) + ' ' + CAST(VOUCHERINFO_1.Vnumber AS nvarchar(10)) AS DocNumber2 ";
                EditVoucher = "select CAST(act_id AS nvarchar(10)) As Code,Name as AccountType from accountype where type='Account'";
                IsNull = " Is ";
                Concat = " + ";
            }
        }

        public static string fnaccbal()
        {
            if (Database.DatabaseType == "access")
            {
                accbalq = " iif(Sum(balance.Dr)>Sum(balance.Cr),Format(Sum(balance.Dr)-Sum(balance.Cr),'Standard') & ' Dr.',Format(Sum(balance.Cr)-Sum(balance.Dr),'Standard') & ' Cr.') as Balance ";
            }
            else
            {
                accbalq = " Case when Sum(balance.Dr)>Sum(balance.Cr) then cast((Sum(balance.Dr)-Sum(balance.Cr)) as nvarchar(20)) + ' Dr.'  else cast((Sum(balance.Cr)-Sum(balance.Dr))  as nvarchar(20)) + ' Cr.'  End as Balance ";
            }
            return accbalq;
        }

        public static string fnstring(string con, string first, string second)
        {
            string res = "";
            if (Database.DatabaseType == "access")
            {
                res = "iif(" + con + ", " + first + ", " + second + ")";
            }
            else
            {
                res = "case when " + con + " then " + first + " Else " + second + " End ";
            }
            return res;
        }

        public static string fnDatFormatting(string Fieldname, string format)
        {
            string res = "";
            if (Database.DatabaseType == "access")
            {
                res = "Format(" + Fieldname + ", '" + format + "')";
            }
            else
            {
                res = "CONVERT(nvarchar, " + Fieldname + ", 106) ";

            }
            return res;
        }
    }
}
