using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Web.Script.Serialization;

namespace faspi
{
    class  funs
    {
        OleDbCommand cmd = new OleDbCommand();   
        public static System.Windows.Forms.NotifyIcon notifyIcon=new System.Windows.Forms.NotifyIcon();

        public static void ShowBalloonTip(String BalloonTipTitle, string BalloonTipText)
        {
            GC.Collect();
            System.Drawing.Icon appIcon = System.Drawing.Icon.ExtractAssociatedIcon(Database.ServerPath + "\\Marwari.exe");
            notifyIcon.Icon = appIcon;
            notifyIcon.Visible = true;
            notifyIcon.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            notifyIcon.BalloonTipTitle = BalloonTipTitle;

            notifyIcon.BalloonTipText = BalloonTipText;
            notifyIcon.ShowBalloonTip(1000);
        }
        public static string Select_Pincode(String name)
        {

            if (Master.Account.Select("[Name]='" + name + "'").Length == 0)
            {
                return "";
            }
            else
            {
                return Master.Account.Select("[Name]='" + name + "'").FirstOrDefault()["Pincode"].ToString();
            }

        }


        public static string Select_ac_City_id(String accname)
        {
            if (Master.Account.Select("[Name]='" + accname + "'").Length == 0)
            {
                return "";
            }
            else
            {
                return Master.Account.Select("[Name]='" + accname + "'").FirstOrDefault()["City_id"].ToString();
            }
        }
        public static bool isDouble(String str)
        {
            double mydouble ;
            bool isnumber=double.TryParse(str, out mydouble);
            return isnumber;
        }

        public static string Select_MainAccTypeName(string AccountName)
        {

            string actid= Database.GetScalarText("Select Act_id from account where name='" + AccountName + "'");

             string mainactname = Database.GetScalarText("Select Name from Accountype where Act_id='" + actid + "'");
          
            if (Select_act_fixed(mainactname) == false)
            {
                bool fix = false;
                while (fix == false)
                {
                    if (mainactname == "")
                    {
                        break;
                    }
                    string under = funs.Select_act_under(mainactname);
                    mainactname = funs.Select_act_nm(under);
                    fix = funs.Select_act_fixed(mainactname);

                }
            }

            return mainactname;
        }
        public static string DocumentNumber(string vid)
        {


            // return Database.GetScalarText("SELECT DISTINCT  dbo.VOUCHERTYPE.Short + ' ' + CONVERT(nvarchar, dbo.VOUCHERINFO.Vdate, 112) + ' ' + CAST(dbo.VOUCHERINFO.Vnumber AS nvarchar(10)) AS DocNumber FROM         dbo.VOUCHERINFO INNER JOIN dbo.VOUCHERTYPE ON dbo.VOUCHERINFO.Vt_id = dbo.VOUCHERTYPE.Vt_id WHERE     (dbo.VOUCHERINFO.Vi_id = "+vid+")");
            return Database.GetScalarText("SELECT DISTINCT VOUCHERTYPE.Short + ' ' + CONVERT(nvarchar, VOUCHERINFO.Vdate, 112) + ' ' + CAST(VOUCHERINFO.Vnumber AS nvarchar(10)) AS DocNumber FROM VOUCHERINFO LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id WHERE (((VOUCHERINFO.Vi_id)='" + vid + "'))");
        }
        //public static int Select_AccTypeid(string AccountName)
        //{
          
        //    return Database.GetScalarInt("Select Act_id from account where name='" + AccountName + "'");
        //}
        public static string GetFixedLengthString(string input, int length)
        {
            string result = string.Empty;
            if (string.IsNullOrEmpty(input))
            {
                result = new string(' ', length);
            }
            else if (input.Length > length)
            {
                result = input.Substring(0, length);
            }
            else
            {
                result = input.PadRight(length);
            }
            return result;
        }
        public static string Select_city_id(String ctname)
        {

            return Database.GetScalarText("Select City_id from City where Cname='" + ctname + "'");

        }


        public static String Select_city_name(string cityid)
        {

            return Database.GetScalarText("Select Cname from City where City_id='" + cityid+"'");

        }
        public static String AddCity()
        {
            String citynm;
            frm_City frm = new frm_City();
            frm.calledIndirect = true;
            frm.LoadData("0", "City");
            frm.ShowDialog();
            citynm = frm.cityName;
            return citynm;
        }

        public static String EditCity(String ciname)
        {
            String newChnm;
            String Chid;
            DataTable dtCheckChg = new DataTable();
            Database.GetSqlData("select * from City where [cname]='" + ciname + "'", dtCheckChg);
            if (dtCheckChg.Rows.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("City does not exist");
                return "";
            }
            frm_City frm = new frm_City();

            Chid = Select_city_id(ciname).ToString();
            frm.calledIndirect = true;
            frm.LoadData(Chid, "Edit City");
            frm.ShowDialog();
            newChnm = frm.cityName;
            if (newChnm == "" || newChnm == null)
            {
                return ciname;
            }
            else
            {
                return newChnm;
            }
        }


        public static String GetStrComboled(string AccountType)
        {
            string strCombo = "";

            if (Database.BMode == "AB" && AccountType == "*")
            {
               // strCombo = "SELECT Name,CASE WHEN Balance > 0 THEN CAST(Balance AS nvarchar(20)) + ' Dr.' ELSE CAST(- 1 * Balance AS nvarchar(20)) + ' Cr.' END AS Balance, Address1, Address2,  Phone, Tin_number, Accounttype AS [Account Group],  AccountGroup AS [Payment Collector], Agent, SalesMan FROM (SELECT Account.Name, Balance +Balance2+ ISNULL  ((select sum(Amount) from journal where  Journal.Ac_id=Account.ac_id and Journal.AB='true'), 0) AS Balance, Address1, Address2, Phone,  Tin_number,   (SELECT Name FROM dbo.ACCOUNTYPE  WHERE (Act_id = dbo.ACCOUNT.Act_id)) AS Accounttype, (SELECT Name  FROM dbo.OTHER  WHERE (Oth_id = dbo.ACCOUNT.Loc_id)) AS AccountGroup,  (SELECT Name FROM dbo.Account WHERE (Ac_id = dbo.ACCOUNT.Con_id)) AS Agent, (SELECT Name FROM dbo.Salesman WHERE (S_id = dbo.ACCOUNT.Salesman_id)) AS SalesMan FROM dbo.ACCOUNT WHERE (Branch_id = '" + Database.BranchId + "') ) AS MyQry order by Name";
                strCombo = "SELECT Name,CASE WHEN Balance > 0 THEN CAST(Balance AS nvarchar(20)) + ' Dr.' ELSE CAST(- 1 * Balance AS nvarchar(20)) + ' Cr.' END AS Balance, Address1, Address2,Code,  Phone, Tin_number, Accounttype AS [Account Group],  AccountGroup AS [Payment Collector],  SalesMan FROM (SELECT Account.Name, Balance +Balance2+ ISNULL  ((select sum(Amount) from journal where  Journal.Ac_id=Account.ac_id and Journal.AB='true'), 0) AS Balance, Address1, Address2,Code, Phone,  Tin_number,   (SELECT Name FROM dbo.ACCOUNTYPE  WHERE (Act_id = dbo.ACCOUNT.Act_id)) AS Accounttype, (SELECT Name  FROM dbo.OTHER  WHERE (Oth_id = dbo.ACCOUNT.Loc_id)) AS AccountGroup,   (SELECT Name FROM dbo.Salesman WHERE (S_id = dbo.ACCOUNT.Salesman_id)) AS SalesMan FROM dbo.ACCOUNT WHERE (Branch_id = '" + Database.BranchId + "') ) AS MyQry order by Name";
            }
            else if (Database.BMode == "A" && AccountType == "*")
            {
               // strCombo = "SELECT Name,CASE WHEN Balance > 0 THEN CAST(Balance AS nvarchar(20)) + ' Dr.' ELSE CAST(- 1 * Balance AS nvarchar(20)) + ' Cr.' END AS Balance, Address1, Address2,  Phone, Tin_number, Accounttype AS [Account Group],  AccountGroup AS [Payment Collector], Agent, SalesMan FROM (SELECT Account.Name, Balance + ISNULL  ((select sum(Amount) from journal where  Journal.Ac_id=Account.ac_id and Journal.A='true'), 0) AS Balance, Address1, Address2, Phone,  Tin_number,   (SELECT Name FROM dbo.ACCOUNTYPE  WHERE (Act_id = dbo.ACCOUNT.Act_id)) AS Accounttype, (SELECT Name  FROM dbo.OTHER  WHERE (Oth_id = dbo.ACCOUNT.Loc_id)) AS AccountGroup,  (SELECT Name FROM dbo.Account WHERE (Ac_id = dbo.ACCOUNT.Con_id)) AS Agent, (SELECT Name FROM dbo.Salesman WHERE (S_id = dbo.ACCOUNT.Salesman_id)) AS SalesMan FROM dbo.ACCOUNT WHERE (Branch_id = '" + Database.BranchId + "') ) AS MyQry order by Name";
                strCombo = "SELECT Name,CASE WHEN Balance > 0 THEN CAST(Balance AS nvarchar(20)) + ' Dr.' ELSE CAST(- 1 * Balance AS nvarchar(20)) + ' Cr.' END AS Balance, Address1, Address2, Code, Phone, Tin_number, Accounttype AS [Account Group],  AccountGroup AS [Payment Collector],  SalesMan FROM (SELECT Account.Name, Balance + ISNULL  ((select sum(Amount) from journal where  Journal.Ac_id=Account.ac_id and Journal.A='true'), 0) AS Balance, Address1, Address2, Code,Phone,  Tin_number,   (SELECT Name FROM dbo.ACCOUNTYPE  WHERE (Act_id = dbo.ACCOUNT.Act_id)) AS Accounttype, (SELECT Name  FROM dbo.OTHER  WHERE (Oth_id = dbo.ACCOUNT.Loc_id)) AS AccountGroup,   (SELECT Name FROM dbo.Salesman WHERE (S_id = dbo.ACCOUNT.Salesman_id)) AS SalesMan FROM dbo.ACCOUNT WHERE (Branch_id = '" + Database.BranchId + "') ) AS MyQry order by Name";
            }
            else if (Database.BMode == "B" && AccountType == "*")
            {
               // strCombo = "SELECT Name,CASE WHEN Balance > 0 THEN CAST(Balance AS nvarchar(20)) + ' Dr.' ELSE CAST(- 1 * Balance AS nvarchar(20)) + ' Cr.' END AS Balance, Address1, Address2,  Phone, Tin_number, Accounttype AS [Account Group],  AccountGroup AS [Payment Collector], Agent, SalesMan FROM (SELECT Account.Name, Balance +Balance2+ ISNULL  ((select sum(Amount) from journal where  Journal.Ac_id=Account.ac_id and Journal.B='true'), 0) AS Balance, Address1, Address2, Phone,  Tin_number,   (SELECT Name FROM dbo.ACCOUNTYPE  WHERE (Act_id = dbo.ACCOUNT.Act_id)) AS Accounttype, (SELECT Name  FROM dbo.OTHER  WHERE (Oth_id = dbo.ACCOUNT.Loc_id)) AS AccountGroup,  (SELECT Name FROM dbo.Account WHERE (Ac_id = dbo.ACCOUNT.Con_id)) AS Agent, (SELECT Name FROM dbo.Salesman WHERE (S_id = dbo.ACCOUNT.Salesman_id)) AS SalesMan FROM dbo.ACCOUNT WHERE (Branch_id = '" + Database.BranchId + "') ) AS MyQry order by Name";
                strCombo = "SELECT Name,CASE WHEN Balance > 0 THEN CAST(Balance AS nvarchar(20)) + ' Dr.' ELSE CAST(- 1 * Balance AS nvarchar(20)) + ' Cr.' END AS Balance, Address1, Address2,Code,  Phone, Tin_number, Accounttype AS [Account Group],  AccountGroup AS [Payment Collector],  SalesMan FROM (SELECT Account.Name, Balance2+ ISNULL  ((select sum(Amount) from journal where  Journal.Ac_id=Account.ac_id and Journal.B='true'), 0) AS Balance, Address1, Address2,Code, Phone,  Tin_number,   (SELECT Name FROM dbo.ACCOUNTYPE  WHERE (Act_id = dbo.ACCOUNT.Act_id)) AS Accounttype, (SELECT Name  FROM dbo.OTHER  WHERE (Oth_id = dbo.ACCOUNT.Loc_id)) AS AccountGroup, (SELECT Name FROM dbo.Salesman WHERE (S_id = dbo.ACCOUNT.Salesman_id)) AS SalesMan FROM dbo.ACCOUNT WHERE (Branch_id = '" + Database.BranchId + "') ) AS MyQry order by Name";
            }





            return strCombo;
        }

        public static String GetStrCombo(string AccountType)
        {
            string strCombo = "";

            if (Database.BMode == "A" && AccountType == "*")
            {
                strCombo = "SELECT ACCOUNT.Name," + access_sql.accbalq + ", ACCOUNT.Address1, ACCOUNT.Address2,ACCOUNT.Code, ACCOUNT.Phone, ACCOUNT.Tin_number, ACCOUNTYPE.Name AS [Account Group], OTHER.Name AS [Payment Collector], CONTRACTOR.Name AS Agent FROM ((((SELECT ACCOUNT.Ac_id,  " + access_sql.fnstring("ACCOUNT.Balance>0", "ACCOUNT.Balance", "0") + " AS Dr, " + access_sql.fnstring("ACCOUNT.Balance<0", "-1*(ACCOUNT.Balance)", "0") + " AS Cr FROM ACCOUNT union all SELECT JOURNAL.Ac_id, " + access_sql.fnstring("JOURNAL.Amount>0", "JOURNAL.Amount", "0") + " AS Dr, " + access_sql.fnstring("JOURNAL.Amount<0", "-1*(JOURNAL.Amount)", "0") + " AS Cr FROM JOURNAL,  VOUCHERINFO , VOUCHERTYPE where JOURNAL.Vi_id = VOUCHERINFO.Vi_id  and VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id and VOUCHERTYPE.A=" + access_sql.Singlequote + "True" + access_sql.Singlequote + ")  AS balance LEFT JOIN ACCOUNT ON balance.Ac_id = ACCOUNT.Ac_id) LEFT JOIN ACCOUNTYPE ON ACCOUNT.Act_id = ACCOUNTYPE.Act_id) LEFT JOIN OTHER ON ACCOUNT.Loc_id = OTHER.Oth_id) LEFT JOIN CONTRACTOR ON ACCOUNT.Con_id = CONTRACTOR.Con_id GROUP BY ACCOUNT.Name, ACCOUNT.Address1, ACCOUNT.Address2,ACCOUNT.Code, ACCOUNT.Phone, ACCOUNT.Tin_number, ACCOUNTYPE.Name, OTHER.Name, CONTRACTOR.Name";
            }
            else if (Database.BMode == "B" && AccountType == "*")
            {
                strCombo = "SELECT ACCOUNT.Name, " + access_sql.accbalq + " , ACCOUNT.Address1, ACCOUNT.Address2,ACCOUNT.Code, ACCOUNT.Phone, ACCOUNT.Tin_number, ACCOUNTYPE.Name AS [Account Group], OTHER.Name AS [Payment Collector], CONTRACTOR.Name AS Agent FROM ((((SELECT ACCOUNT.Ac_id,  " + access_sql.fnstring("ACCOUNT.Balance2>0", "ACCOUNT.Balance2", "0") + " AS Dr, " + access_sql.fnstring("ACCOUNT.Balance2<0", "-1*(ACCOUNT.Balance2)", "0") + " AS Cr FROM ACCOUNT union all SELECT JOURNAL.Ac_id, " + access_sql.fnstring("JOURNAL.Amount>0", "JOURNAL.Amount", "0") + " AS Dr, " + access_sql.fnstring("JOURNAL.Amount<0", "-1*(JOURNAL.Amount)", "0") + " AS Cr FROM JOURNAL,  VOUCHERINFO , VOUCHERTYPE where JOURNAL.Vi_id = VOUCHERINFO.Vi_id  and VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id and VOUCHERTYPE.B=" + access_sql.Singlequote + "True" + access_sql.Singlequote + ")  AS balance LEFT JOIN ACCOUNT ON balance.Ac_id = ACCOUNT.Ac_id) LEFT JOIN ACCOUNTYPE ON ACCOUNT.Act_id = ACCOUNTYPE.Act_id) LEFT JOIN OTHER ON ACCOUNT.Loc_id = OTHER.Oth_id) LEFT JOIN CONTRACTOR ON ACCOUNT.Con_id = CONTRACTOR.Con_id GROUP BY ACCOUNT.Name, ACCOUNT.Address1, ACCOUNT.Address2,ACCOUNT.Code, ACCOUNT.Phone, ACCOUNT.Tin_number, ACCOUNTYPE.Name, OTHER.Name, CONTRACTOR.Name";
            }
            else if (Database.BMode == "A" && AccountType != "*")
            {
                strCombo = "SELECT ACCOUNT.Name," + access_sql.accbalq + " , ACCOUNT.Address1, ACCOUNT.Address2,ACCOUNT.Code, ACCOUNT.Phone, ACCOUNT.Tin_number, ACCOUNTYPE.Name AS [Account Group], OTHER.Name AS [Payment Collector], CONTRACTOR.Name AS Agent FROM ((((SELECT ACCOUNT.Ac_id,  " + access_sql.fnstring("ACCOUNT.Balance>0", "ACCOUNT.Balance", "0") + " AS Dr, " + access_sql.fnstring("ACCOUNT.Balance<0", "-1*(ACCOUNT.Balance)", "0") + " AS Cr FROM ACCOUNT union all SELECT JOURNAL.Ac_id, " + access_sql.fnstring("JOURNAL.Amount>0", "JOURNAL.Amount", "0") + " AS Dr, " + access_sql.fnstring("JOURNAL.Amount<0", "-1*(JOURNAL.Amount)", "0") + " AS Cr FROM JOURNAL,  VOUCHERINFO , VOUCHERTYPE where JOURNAL.Vi_id = VOUCHERINFO.Vi_id  and VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id and VOUCHERTYPE.A=" + access_sql.Singlequote + "True" + access_sql.Singlequote + ")  AS balance LEFT JOIN ACCOUNT ON balance.Ac_id = ACCOUNT.Ac_id) LEFT JOIN ACCOUNTYPE ON ACCOUNT.Act_id = ACCOUNTYPE.Act_id) LEFT JOIN OTHER ON ACCOUNT.Loc_id = OTHER.Oth_id) LEFT JOIN CONTRACTOR ON ACCOUNT.Con_id = CONTRACTOR.Con_id WHERE     ( " + AccountType + ") GROUP BY ACCOUNT.Name, ACCOUNT.Address1, ACCOUNT.Address2, ACCOUNT.Code,ACCOUNT.Phone, ACCOUNT.Tin_number, ACCOUNTYPE.Name, OTHER.Name, CONTRACTOR.Name";
            }
            else if (Database.BMode == "B" && AccountType != "*")
            {
                strCombo = "SELECT ACCOUNT.Name, " + access_sql.accbalq + ", ACCOUNT.Address1, ACCOUNT.Address2, ACCOUNT.Code,ACCOUNT.Phone, ACCOUNT.Tin_number, ACCOUNTYPE.Name AS [Account Group], OTHER.Name AS [Payment Collector], CONTRACTOR.Name AS Agent FROM ((((SELECT ACCOUNT.Ac_id,  " + access_sql.fnstring("ACCOUNT.Balance2>0", "ACCOUNT.Balance2", "0") + " AS Dr, " + access_sql.fnstring("ACCOUNT.Balance2<0", "-1*(ACCOUNT.Balance2)", "0") + " AS Cr FROM ACCOUNT union all SELECT JOURNAL.Ac_id, " + access_sql.fnstring("JOURNAL.Amount>0", "JOURNAL.Amount", "0") + " AS Dr, " + access_sql.fnstring("JOURNAL.Amount<0", "-1*(JOURNAL.Amount)", "0") + " AS Cr FROM JOURNAL,  VOUCHERINFO , VOUCHERTYPE where JOURNAL.Vi_id = VOUCHERINFO.Vi_id  and VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id and VOUCHERTYPE.B=" + access_sql.Singlequote + "True" + access_sql.Singlequote + ")  AS balance LEFT JOIN ACCOUNT ON balance.Ac_id = ACCOUNT.Ac_id) LEFT JOIN ACCOUNTYPE ON ACCOUNT.Act_id = ACCOUNTYPE.Act_id) LEFT JOIN OTHER ON ACCOUNT.Loc_id = OTHER.Oth_id) LEFT JOIN CONTRACTOR ON ACCOUNT.Con_id = CONTRACTOR.Con_id WHERE     ( " + AccountType + ") GROUP BY ACCOUNT.Name, ACCOUNT.Address1, ACCOUNT.Address2, ACCOUNT.Code,ACCOUNT.Phone, ACCOUNT.Tin_number, ACCOUNTYPE.Name, OTHER.Name, CONTRACTOR.Name";
            }
            else if (Database.BMode == "AB" && AccountType != "*")
            {
                strCombo = "SELECT ACCOUNT.Name, " + access_sql.accbalq + ", ACCOUNT.Address1, ACCOUNT.Address2,ACCOUNT.Code, ACCOUNT.Phone, ACCOUNT.Tin_number, ACCOUNTYPE.Name AS [Account Group], OTHER.Name AS [Payment Collector], CONTRACTOR.Name AS Agent FROM ((((SELECT ACCOUNT.Ac_id,  " + access_sql.fnstring("(ACCOUNT.Balance+ACCOUNT.Balance2)>0", "ACCOUNT.Balance+ACCOUNT.Balance2", "0") + " AS Dr, " + access_sql.fnstring("(ACCOUNT.Balance+ACCOUNT.Balance2)<0", "-1*(ACCOUNT.Balance2+ACCOUNT.Balance)", "0") + " AS Cr FROM ACCOUNT union all SELECT JOURNAL.Ac_id, " + access_sql.fnstring("JOURNAL.Amount>0", "JOURNAL.Amount", "0") + " AS Dr, " + access_sql.fnstring("JOURNAL.Amount<0", "-1*(JOURNAL.Amount)", "0") + " AS Cr FROM JOURNAL,  VOUCHERINFO , VOUCHERTYPE where JOURNAL.Vi_id = VOUCHERINFO.Vi_id  and VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id and VOUCHERTYPE." + Database.BMode + "=" + access_sql.Singlequote + "True" + access_sql.Singlequote + ")  AS balance LEFT JOIN ACCOUNT ON balance.Ac_id = ACCOUNT.Ac_id) LEFT JOIN ACCOUNTYPE ON ACCOUNT.Act_id = ACCOUNTYPE.Act_id) LEFT JOIN OTHER ON ACCOUNT.Loc_id = OTHER.Oth_id) LEFT JOIN CONTRACTOR ON ACCOUNT.Con_id = CONTRACTOR.Con_id WHERE     ( " + AccountType + ") GROUP BY ACCOUNT.Name, ACCOUNT.Address1, ACCOUNT.Address2,ACCOUNT.Code, ACCOUNT.Phone, ACCOUNT.Tin_number, ACCOUNTYPE.Name, OTHER.Name, CONTRACTOR.Name";
            }
            else if (Database.BMode == "AB" && AccountType == "*")
            {
                strCombo = "SELECT ACCOUNT.Name," + access_sql.accbalq + ", ACCOUNT.Address1, ACCOUNT.Address2,ACCOUNT.Code, ACCOUNT.Phone, ACCOUNT.Tin_number, ACCOUNTYPE.Name AS [Account Group], OTHER.Name AS [Payment Collector], CONTRACTOR.Name AS Agent FROM ((((SELECT ACCOUNT.Ac_id,  " + access_sql.fnstring("(ACCOUNT.Balance+ACCOUNT.Balance2)>0", "(ACCOUNT.Balance+ACCOUNT.Balance2)", "0") + " AS Dr, " + access_sql.fnstring("(ACCOUNT.Balance+ACCOUNT.Balance2)<0", "-1*(ACCOUNT.Balance+ACCOUNT.Balance2)", "0") + " AS Cr FROM ACCOUNT union all SELECT JOURNAL.Ac_id, " + access_sql.fnstring("JOURNAL.Amount>0", "JOURNAL.Amount", "0") + " AS Dr, " + access_sql.fnstring("JOURNAL.Amount<0", "-1*(JOURNAL.Amount)", "0") + " AS Cr FROM JOURNAL,  VOUCHERINFO , VOUCHERTYPE where JOURNAL.Vi_id = VOUCHERINFO.Vi_id  and VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id and VOUCHERTYPE." + Database.BMode + "=" + access_sql.Singlequote + "True" + access_sql.Singlequote + ")  AS balance LEFT JOIN ACCOUNT ON balance.Ac_id = ACCOUNT.Ac_id) LEFT JOIN ACCOUNTYPE ON ACCOUNT.Act_id = ACCOUNTYPE.Act_id) LEFT JOIN OTHER ON ACCOUNT.Loc_id = OTHER.Oth_id) LEFT JOIN CONTRACTOR ON ACCOUNT.Con_id = CONTRACTOR.Con_id GROUP BY ACCOUNT.Name, ACCOUNT.Address1, ACCOUNT.Address2,ACCOUNT.Code, ACCOUNT.Phone, ACCOUNT.Tin_number, ACCOUNTYPE.Name, OTHER.Name, CONTRACTOR.Name";
            }
            return strCombo;
        }

        public static string Select_act_path(String name)
        {
            if (Master.AccountType.Select("[name]='" + name + "'").Length == 0)
            {
                return "";
            }
            else
            {
                return Master.AccountType.Select("[name]='" + name + "'").FirstOrDefault()["Path"].ToString();
            }
        }

        public static string Select_vt_Narrtemplate(string vt_id)
        {
            if (Master.VoucherType.Select("[Vt_id]='" + vt_id + "'").Length == 0)
            {
                return "";
            }
            else
            {
                return Master.VoucherType.Select("[Vt_id]='" + vt_id + "'").FirstOrDefault()["NarrTemplate"].ToString();
            }
        }
        public static int Select_act_regsqn(String name)
        {
            if (Master.AccountType.Select("[name]='" + name + "'").Length == 0)
            {
                return 0;
            }
            else
            {
                return int.Parse(Master.AccountType.Select("[name]='" + name + "'").FirstOrDefault()["Regsqn"].ToString());
            }
        }

        public static string Select_act_under(String name)
        {
            if (Master.AccountType.Select("[name]='" + name + "'").Length == 0)
            {
                return "";
            }
            else
            {
                return Master.AccountType.Select("[name]='" + name + "'").FirstOrDefault()["under"].ToString();
            }
        }

        public static int Select_act_level(String subname)
        {
            if (Master.AccountType.Select("[name]='" + subname + "'").Length == 0)
            {
                return 0;
            }
            else
            {
                return int.Parse(Master.AccountType.Select("[name]='" + subname + "'").FirstOrDefault()["level"].ToString());
            }
        }

        public static bool Select_act_fixed(String name)
        {
            if (Master.AccountType.Select("[name]='" + name + "'").Length == 0)
            {
                return false;
            }
            else
            {
                return bool.Parse(Master.AccountType.Select("[name]='" + name + "'").FirstOrDefault()["fixed"].ToString());
            }
        }

        public static string Select_act_nature(String name)
        {
            if (Master.AccountType.Select("[name]='" + name + "'").Length == 0)
            {
                return "";
            }
            else
            {
                return Master.AccountType.Select("[name]='" + name + "'").FirstOrDefault()["Nature"].ToString();
            }
        }

        public static string Select_AccType_id(string AccountTypeName)
        {
            if (Master.AccountType.Select("[Name]='" + AccountTypeName + "'").Length == 0)
            {
                return "";
            }
            else
            {
                return Master.AccountType.Select("[Name]='" + AccountTypeName + "'").FirstOrDefault()["Act_id"].ToString();
            }
        }

        public static string Select_user_id(String uname)
        {
            return Database.GetScalarText("Select U_id from Userinfo where uname='" + uname + "'");
        }
        public static string Select_Role_Name(int rid)
        {
            return Database.GetScalarText("Select RoleName from SYS_Role where role_id=" + rid);
        }
        public static int Select_Role_id(String rname)
        {
            return Database.GetScalarInt("Select Role_id from SYS_Role where rolename='" + rname + "'");
        }
        public static string Select_Container_name(string cid)
        {
            return Database.GetScalarText("Select Cname from Container where id='" + cid + "'");
        }

        public static string Select_Container_id(string cname)
        {
            return Database.GetScalarText("Select id from Container where Cname='" + cname + "'");
        }
        //public static List<UsersFeature> GetPermission(string pagename)
        //{

        //    string str = Database.GetScalarText("SELECT  WinPageRole.Feature FROM   WinPage RIGHT OUTER JOIN  WinPageRole ON WinPage.PageID = WinPageRole.Page_id LEFT OUTER JOIN SYS_Role ON WinPageRole.Role_id = SYS_Role.Role_id WHERE     (SYS_Role.RoleName = '" + Database.utype + "') AND (WinPage.PageName = '" + pagename + "')");
        //    List<UsersFeature> objlist = new List<UsersFeature>();
        //    JavaScriptSerializer obj = new JavaScriptSerializer();

        //    objlist = obj.Deserialize<List<UsersFeature>>(str);
        //    return objlist;

        //}
        public static List<UsersFeature> GetPermissionKey(string KeyValue)
        {

            string str = Database.GetScalarText("SELECT  WinPageRole.Feature FROM   WinPage RIGHT OUTER JOIN  WinPageRole ON WinPage.PageID = WinPageRole.Page_id LEFT OUTER JOIN SYS_Role ON WinPageRole.Role_id = SYS_Role.Role_id WHERE     (SYS_Role.RoleName = '" + Database.utype + "') AND (WinPage.KeyValue = '" + KeyValue + "')");
            List<UsersFeature> objlist = new List<UsersFeature>();
            JavaScriptSerializer obj = new JavaScriptSerializer();

            objlist = obj.Deserialize<List<UsersFeature>>(str);
            return objlist;

        }

        public static string Select_destax_id(string desid)
        {
            if (Master.Description.Select("des_id='" + desid+"' ").Length == 0)
            {
                return "";
            }
            else
            {
                return Master.Description.Select("des_id='" + desid+"' ").FirstOrDefault()["Tax_Cat_id"].ToString();
            }
        }

        public static String GetStrCombonew(string wherestr, string Having)
        {
            string strCombo = "";
            if (Database.BMode=="A")
            {

                strCombo = "SELECT Name,  CASE WHEN Balance > 0 THEN CAST(Balance AS nvarchar(20)) + ' Dr.' ELSE CAST(- 1 * Balance AS nvarchar(20)) + ' Cr.' END AS Balance,Address1, Address2, Code, Phone, Tin_number, Accounttype AS [Account Group],  AccountGroup AS [Payment Collector],  SalesMan FROM (SELECT Account.Name, Balance + ISNULL  ((select sum(Amount) from journal where  Journal.Ac_id=Account.ac_id and Journal.A='true'), 0) AS Balance, Address1, Address2,Code, Phone,   Tin_number,  (SELECT Name  FROM dbo.ACCOUNTYPE  WHERE (Act_id = dbo.ACCOUNT.Act_id)) AS Accounttype,  (SELECT Name FROM dbo.OTHER  WHERE (Oth_id = dbo.ACCOUNT.Loc_id)) AS AccountGroup,  (SELECT Name FROM dbo.Salesman WHERE (S_id = dbo.ACCOUNT.Salesman_id)) AS SalesMan FROM dbo.ACCOUNT,Accountype WHERE account.Act_id=accountype.Act_id And (" + wherestr + ")  and " + Having + " ) AS MyQry";
             //   strCombo = " SELECT   ISNULL(X.AccountGroup, N'') AS AccountGroup, X.AccountName, ACCOUNT_1.Phone, SUM(X.Dr) AS Dr, SUM(X.Cr) AS Cr FROM         (SELECT     AccountGroup, AccountName, SUM(Dr) AS Dr, SUM(Cr) AS Cr FROM         (SELECT     OTHER.Name AS AccountGroup, ACCOUNT.Name AS AccountName,          CASE WHEN Journal.Amount >= 0 THEN Journal.Amount ELSE 0 END AS Dr,      CASE WHEN Journal.Amount < 0 THEN - 1 * Journal.Amount ELSE 0 END AS Cr FROM          ACCOUNT LEFT OUTER JOIN         OTHER ON ACCOUNT.Loc_id = OTHER.Oth_id RIGHT OUTER JOIN       Journal ON ACCOUNT.Ac_id = Journal.Ac_id WHERE      (Journal.Vdate <= '31-Mar-2019') AND (Journal."+Database.BMode+" = 'true') AND (ACCOUNT.Branch_id = 'MAN')) AS XY GROUP BY AccountGroup, AccountName   UNION ALL    SELECT     OTHER_1.Name AS AccountGroup, QryAccountinfo.Name AS AccountName, QryAccountinfo.Dr, QryAccountinfo.Cr                FROM         QryAccountinfo LEFT OUTER JOIN           OTHER AS OTHER_1 ON QryAccountinfo.Loc_id = OTHER_1.Oth_id  WHERE     (QryAccountinfo.Branch_id = 'MAN')) AS X LEFT OUTER JOIN     ACCOUNT AS ACCOUNT_1 ON X.AccountName = ACCOUNT_1.Name GROUP BY X.AccountGroup, X.AccountName, ACCOUNT_1.Phone HAVING      (NOT (SUM(X.Dr) = 0)) OR     (NOT (SUM(X.Cr) = 0)) ORDER BY AccountGroup, X.AccountName";
            }
            else if (Database.BMode == "B")
            {
               // strCombo = "SELECT Name,  CASE WHEN Balance > 0 THEN CAST(Balance AS nvarchar(20)) + ' Dr.' ELSE CAST(- 1 * Balance AS nvarchar(20)) + ' Cr.' END AS Balance,Address1, Address2,  Phone, Tin_number, Accounttype AS [Account Group],  AccountGroup AS [Payment Collector], Agent, SalesMan FROM (SELECT Account.Name, Balance2 + ISNULL  ((select sum(Amount) from journal where  Journal.Ac_id=Account.ac_id and Journal.B='true'), 0) AS Balance, Address1, Address2, Phone,   Tin_number,  (SELECT Name  FROM dbo.ACCOUNTYPE  WHERE (Act_id = dbo.ACCOUNT.Act_id)) AS Accounttype,  (SELECT Name FROM dbo.OTHER  WHERE (Oth_id = dbo.ACCOUNT.Loc_id)) AS AccountGroup, (SELECT Name  FROM dbo.Account  WHERE (Ac_id = dbo.ACCOUNT.Con_id)) AS Agent, (SELECT Name FROM dbo.Salesman WHERE (S_id = dbo.ACCOUNT.Salesman_id)) AS SalesMan FROM dbo.ACCOUNT,Accountype WHERE account.Act_id=accountype.Act_id And (" + wherestr + ")  and " + Having + " ) AS MyQry";
                strCombo = "SELECT Name,  CASE WHEN Balance > 0 THEN CAST(Balance AS nvarchar(20)) + ' Dr.' ELSE CAST(- 1 * Balance AS nvarchar(20)) + ' Cr.' END AS Balance,Address1, Address2, Code, Phone, Tin_number, Accounttype AS [Account Group],  AccountGroup AS [Payment Collector],  SalesMan FROM (SELECT Account.Name, Balance2 + ISNULL  ((select sum(Amount) from journal where  Journal.Ac_id=Account.ac_id and Journal.B='true'), 0) AS Balance, Address1, Address2,Code, Phone,   Tin_number,  (SELECT Name  FROM dbo.ACCOUNTYPE  WHERE (Act_id = dbo.ACCOUNT.Act_id)) AS Accounttype,  (SELECT Name FROM dbo.OTHER  WHERE (Oth_id = dbo.ACCOUNT.Loc_id)) AS AccountGroup, (SELECT Name FROM dbo.Salesman WHERE (S_id = dbo.ACCOUNT.Salesman_id)) AS SalesMan FROM dbo.ACCOUNT,Accountype WHERE account.Act_id=accountype.Act_id And (" + wherestr + ")  and " + Having + " ) AS MyQry";
             
            }
            else if (Database.BMode == "AB")
            {
              //  strCombo = "SELECT Name,  CASE WHEN Balance > 0 THEN CAST(Balance AS nvarchar(20)) + ' Dr.' ELSE CAST(- 1 * Balance AS nvarchar(20)) + ' Cr.' END AS Balance,Address1, Address2,  Phone, Tin_number, Accounttype AS [Account Group],  AccountGroup AS [Payment Collector], Agent, SalesMan FROM (SELECT Account.Name, Balance + balance2 + ISNULL  ((select sum(Amount) from journal where  Journal.Ac_id=Account.ac_id and Journal.AB='true'), 0) AS Balance, Address1, Address2, Phone,   Tin_number,  (SELECT Name  FROM dbo.ACCOUNTYPE  WHERE (Act_id = dbo.ACCOUNT.Act_id)) AS Accounttype,  (SELECT Name FROM dbo.OTHER  WHERE (Oth_id = dbo.ACCOUNT.Loc_id)) AS AccountGroup, (SELECT Name  FROM dbo.Account  WHERE (Ac_id = dbo.ACCOUNT.Con_id)) AS Agent, (SELECT Name FROM dbo.Salesman WHERE (S_id = dbo.ACCOUNT.Salesman_id)) AS SalesMan FROM dbo.ACCOUNT,Accountype WHERE account.Act_id=accountype.Act_id And (" + wherestr + ")  and " + Having + " ) AS MyQry";
                strCombo = "SELECT Name,  CASE WHEN Balance > 0 THEN CAST(Balance AS nvarchar(20)) + ' Dr.' ELSE CAST(- 1 * Balance AS nvarchar(20)) + ' Cr.' END AS Balance,Address1, Address2,  Code,Phone, Tin_number, Accounttype AS [Account Group],  AccountGroup AS [Payment Collector],  SalesMan FROM (SELECT Account.Name, Balance + balance2 + ISNULL  ((select sum(Amount) from journal where  Journal.Ac_id=Account.ac_id and Journal.AB='true'), 0) AS Balance, Address1, Address2,Code, Phone,   Tin_number,  (SELECT Name  FROM dbo.ACCOUNTYPE  WHERE (Act_id = dbo.ACCOUNT.Act_id)) AS Accounttype,  (SELECT Name FROM dbo.OTHER  WHERE (Oth_id = dbo.ACCOUNT.Loc_id)) AS AccountGroup,  (SELECT Name FROM dbo.Salesman WHERE (S_id = dbo.ACCOUNT.Salesman_id)) AS SalesMan FROM dbo.ACCOUNT,Accountype WHERE account.Act_id=accountype.Act_id And (" + wherestr + ")  and " + Having + " ) AS MyQry";
              
            }
            return strCombo;
        }

        public static String accbal(string ac_id, DateTime dt1)
        {
            String curbal;
            double opbal = 0, bal = 0;
            DataTable dtOpenBal = new DataTable();
            if (Database.BMode=="A")
            {
                Database.GetSqlData("select Balance from account where Ac_id='" + ac_id+"' ", dtOpenBal);
            }
            else if (Database.BMode == "B")
            {
                Database.GetSqlData("select Balance2 as Balance from account where Ac_id='" + ac_id+"' ", dtOpenBal);
            }
            else if (Database.BMode == "AB")
            {
                Database.GetSqlData("select Balance+Balance2 as Balance from account where Ac_id='" + ac_id + "' ", dtOpenBal);
            }
            if (dtOpenBal.Rows.Count > 0)
            {
                opbal = double.Parse(dtOpenBal.Rows[0]["Balance"].ToString());
            }
            string acname = funs.Select_ac_nm(ac_id);
            DataTable dtBal = new DataTable();

            //if (Database.IsKacha == false)
            //{

                Database.GetSqlData("SELECT Sum(QryJournal.Dr) AS Dramt, Sum(QryJournal.Cr) AS Cramt FROM QryJournal WHERE (((QryJournal.Name)='" + acname + "') AND ((QryJournal.Vdate)<="+access_sql.Hash +dt1.ToString(Database.dformat) +  access_sql.Hash +")) GROUP BY QryJournal."+Database.BMode+" HAVING (((QryJournal."+Database.BMode+")=" + access_sql.Singlequote + "True" + access_sql.Singlequote + "))", dtBal);
            //}
            //else
            //{
            //    Database.GetSqlData("SELECT Sum(QryJournal.Dr) AS Dramt, Sum(QryJournal.Cr) AS Cramt FROM QryJournal WHERE (((QryJournal.Name)='" + acname + "') AND ((QryJournal.Vdate)<=" + access_sql.Hash + dt1.ToString(Database.dformat) + access_sql.Hash + ")) GROUP BY QryJournal.B HAVING (((QryJournal.B)=" + access_sql.Singlequote + "True" + access_sql.Singlequote + "))", dtBal);
            //}


            if (dtBal.Rows.Count > 0)
            {
                if (dtBal.Rows[0]["Cramt"].ToString() == "" || dtBal.Rows[0]["Dramt"].ToString() == "")
                {
                    dtBal.Rows[0]["Cramt"] = 0;
                }

                if (double.Parse(dtBal.Rows[0]["Dramt"].ToString()) > double.Parse(dtBal.Rows[0]["Cramt"].ToString()))
                {
                    bal = double.Parse(dtBal.Rows[0]["Dramt"].ToString()) - double.Parse(dtBal.Rows[0]["Cramt"].ToString());
                }
                else
                {
                    bal = -(double.Parse(dtBal.Rows[0]["Cramt"].ToString()) - double.Parse(dtBal.Rows[0]["Dramt"].ToString()));
                }
            }

            curbal = (opbal + bal).ToString();
            if (double.Parse(curbal) >= 0)
            {
                curbal += " Dr.";
            }
            else
            {
                curbal = (-1 * double.Parse(curbal)).ToString();
                curbal += " Cr.";
            }
            return curbal;
        }

        public static String accbal(string ac_id)
        {
            String curbal;
            double opbal = 0, bal = 0;

            DataTable dtOpenBal = new DataTable();
            if (Database.BMode=="A")
            {
                Database.GetSqlData("select Balance from account where Ac_id='" + ac_id + "'", dtOpenBal);
            }
            else if (Database.BMode == "B")
            {
                Database.GetSqlData("select Balance2 as Balance from account where Ac_id='" + ac_id + "'", dtOpenBal);
            }
            else if (Database.BMode == "AB")
            {
                Database.GetSqlData("select (Balance+Balance2) as Balance from account where Ac_id='" + ac_id + "'", dtOpenBal);
            }
            if (dtOpenBal.Rows.Count > 0)
            {
                
               opbal = double.Parse(dtOpenBal.Rows[0]["Balance"].ToString());  
            }
            string acname = funs.Select_ac_nm(ac_id);
            DataTable dtBal = new DataTable();
            //if (Database.IsKacha == false)
            //{
            Database.GetSqlData("SELECT Sum(QryJournal.Dr) AS Dramt, Sum(QryJournal.Cr) AS Cramt FROM QryJournal WHERE (((QryJournal.Name)='" + acname + "')) GROUP BY QryJournal." + Database.BMode + " HAVING (((QryJournal." + Database.BMode + ")=" + access_sql.Singlequote + "True" + access_sql.Singlequote + "))", dtBal);
            //}
            //else
            //{
            //    Database.GetSqlData("SELECT Sum(QryJournal.Dr) AS Dramt, Sum(QryJournal.Cr) AS Cramt FROM QryJournal WHERE (((QryJournal.Name)='" + acname + "')) GROUP BY QryJournal.B HAVING (((QryJournal.B)=" + access_sql.Singlequote + "True" + access_sql.Singlequote + "))", dtBal);
            //}

            if (dtBal.Rows.Count > 0)
            {
                if (dtBal.Rows[0]["Cramt"].ToString() == "" || dtBal.Rows[0]["Dramt"].ToString() == "")
                {
                    dtBal.Rows[0]["Cramt"] = 0;
                }

                if (double.Parse(dtBal.Rows[0]["Dramt"].ToString()) > double.Parse(dtBal.Rows[0]["Cramt"].ToString()))
                {
                    bal = double.Parse(dtBal.Rows[0]["Dramt"].ToString()) - double.Parse(dtBal.Rows[0]["Cramt"].ToString());
                }

                else
                {
                    bal = -(double.Parse(dtBal.Rows[0]["Cramt"].ToString()) - double.Parse(dtBal.Rows[0]["Dramt"].ToString()));
                }

            }

            curbal = (opbal + bal).ToString();

            if (double.Parse(curbal) >= 0)
            {
                curbal += " Dr.";
            }
            else
            {
                curbal = (-1 * double.Parse(curbal)).ToString(); 
                curbal += " Cr.";
            }
            return curbal;
        }

        public static String AddAccount()
        {
            String accnm;
            frm_NewAcc frm = new frm_NewAcc();
            frm.calledIndirect = true;
            frm.LoadData("0", "Account");
            frm.ShowDialog();
            accnm = frm.AccName;
            return accnm;
        }

        public static String AddAccount(String acctyp)
        {
            String accnm;
            frm_NewAcc frm = new frm_NewAcc();
            frm.calledIndirect = true;
            frm.AccType = acctyp;
            frm.LoadData("0", "Account");
            frm.ShowDialog();
            accnm = frm.AccName;
            return accnm;
        }

        public static string getmonth(int Month)
        {
            string month = new DateTime(1900, Month, 1).ToString("MMMM");
            return month;
        }

        public static String EditAccount(String accnm)
        {
            String newAccnm;
            String acid;
            DataTable dtCheckAcc = new DataTable();
            Database.GetSqlData("select * from account where [name]='" + accnm + "'", dtCheckAcc);
            if (dtCheckAcc.Rows.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("Account does not exist");
                return "";
            }
            else
            {
                frm_NewAcc frm = new frm_NewAcc();
                frm.calledIndirect = true;
                acid = Select_ac_id(accnm).ToString();
                frm.LoadData(acid, "Edit Account");
                frm.ShowDialog();
                newAccnm = frm.AccName;
                if (newAccnm == "" || newAccnm == null)
                {
                    return accnm;
                }
                else
                {
                    return newAccnm;
                }
            }
        }

        public static String EditAccount(String accnm, String acctyp)
        {
            String newAccnm;
            String acid;
            DataTable dtCheckAcc = new DataTable();
            Database.GetSqlData("select * from account where [name]='" + accnm + "'", dtCheckAcc);
            if (dtCheckAcc.Rows.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("Account does not exist");
                return "";
            }
            frm_NewAcc frm = new frm_NewAcc();
            frm.calledIndirect = true;
            frm.AccType = acctyp;
            acid = Select_ac_id(accnm).ToString();
            frm.LoadData(acid, "Edit Account");
            frm.ShowDialog();
            newAccnm = frm.AccName;
            return newAccnm;
        }

        public static String AddDescription()
        {
            String Descriptionid;
            frmDescription frm = new frmDescription();
            frm.calledIndirect = true;
            frm.LoadData("0", "Description");
            frm.ShowDialog();
            Descriptionid = frm.DescriptionName;
            return Descriptionid;
        }

        public static String EditDescription(String descnm)
        {
            String newDesc;
            String Description;
            DataTable dtCheckDesc = new DataTable();
            Database.GetSqlData("select * from description where Description='" + descnm + "' ", dtCheckDesc);
            if (dtCheckDesc.Rows.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("Description or Packing does not exist");
                return "";
            }
            frmDescription frm = new frmDescription();
            frm.calledIndirect = true;
            Description = descnm.ToString();
            frm.LoadData(descnm, "Edit Description");
            frm.ShowDialog();
            newDesc = frm.DescriptionName;
            if (newDesc == "" || newDesc == null)
            {
                return descnm;
            }
            else
            {
                return newDesc;
            }
        }

        public static String AddBroker()
        {
            String bronm;
            frmBroker frm = new frmBroker();
            frm.calledIndirect = true;
            frm.LoadData("0", "Broker");
            frm.ShowDialog();
            bronm = frm.BrokerName;
            return bronm;
        }

        public static String AddCharge()
        {
            String chname;
            frmCharges frm = new frmCharges();
            frm.calledIndirect = true;
            frm.LoadData("0", "Charges");
            frm.ShowDialog();
            chname = frm.chrgname;
            return chname;
        }

        public static String AddSalesman()
        {
            String name;
            frm_salesman frm = new frm_salesman();
            frm.calledIndirect = true;
            frm.LoadData("0", "Salesman");
            frm.ShowDialog();
            name = frm.salesmanname;
            return name;
        }


        public static String EditSalesman(String name)
        {
            String newChnm;
            String Chid;
            DataTable dtCheckChg = new DataTable();
            Database.GetSqlData("select * from Salesman where [name]='" + name + "'", dtCheckChg);
            if (dtCheckChg.Rows.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("Salesman does not exist");
                return "";
            }
            frm_salesman frm = new frm_salesman();

            Chid = Select_salesman_id(name).ToString();
            frm.calledIndirect = true;
            frm.LoadData(Chid, "Edit Salesman");
            frm.ShowDialog();
            newChnm = frm.salesmanname;
            if (newChnm == "" || newChnm == null)
            {
                return newChnm;
            }
            else
            {
                return newChnm;
            }
        }

        public static String EditCharge(String chname)
        {
            String newChnm;
            String Chid;
            DataTable dtCheckChg = new DataTable();
            Database.GetSqlData("select * from Charges where [name]='" + chname + "'", dtCheckChg);
            if (dtCheckChg.Rows.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("Charges does not exist");
                return "";
            }
           frmCharges frm = new frmCharges();
       
           Chid = Select_ch_id(chname).ToString();
           frm.calledIndirect = true;
            frm.LoadData(Chid, "Edit Charges");
            frm.ShowDialog();
            newChnm = frm.chrgname;
            if (newChnm == "" || newChnm == null)
            {
                return chname;
            }
            else
            {
                return newChnm;
            }
        }

        public static String EditBroker(String bronm)
        {
            String newBronm;
            String Broid;
            DataTable dtCheckBro = new DataTable();
            Database.GetSqlData("select * from contractor where [name]='" + bronm + "'", dtCheckBro);
            if (dtCheckBro.Rows.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("Broker does not exist");
                return "";
            }
            frmBroker frm = new frmBroker();
            frm.calledIndirect = true;
            Broid = Select_con_id(bronm);
            frm.LoadData(Broid, "Edit Broker");
            frm.ShowDialog();
            newBronm = frm.BrokerName;
            if (newBronm == "" || newBronm == null)
            {
                return bronm;
            }
            else
            {
                return newBronm;
            }            
        }

        public static String AddGroup()
        {
            String Gpnm;
            frm_NewGroup frm = new frm_NewGroup();
            frm.calledIndirect = true;
            frm.LoadData("0", "New Group");
            frm.ShowDialog();
            Gpnm = frm.GrpName;
            return Gpnm;
        }

        public static String AddState()
        {
            String Stnm;
            frm_state frm = new frm_state();
            frm.calledIndirect= true;
            frm.LoadData("0", "New State");
            frm.ShowDialog();
            Stnm = frm.statename;
            return Stnm;
        }

        public static String EditState(String stnm)
        {
            String newstnm;
            String stid;
            DataTable dtCheckst = new DataTable();
            Database.GetSqlData("select State_id from State where [Sname]='" + stnm + "'", dtCheckst);
            if (dtCheckst.Rows.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("State does not exist");
                return "";
            }
            frm_state frm = new frm_state();
            frm.calledIndirect = true;
            stid = Select_state_id(stnm).ToString();
            frm.LoadData(stid, "Edit state");
            frm.ShowDialog();
            newstnm = frm.statename;
            if (newstnm == "" || newstnm == null)
            {
                return stnm;
            }
            else
            {
                return newstnm;
            }
        }

        public static String EditGroup(String gpnm)
        {
            String newGpnm;
            String gpid;
            DataTable dtCheckGp = new DataTable();
            Database.GetSqlData("select * from other where [name]='" + gpnm + "' and [type]=17", dtCheckGp);
            if (dtCheckGp.Rows.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("Group does not exist");
                return "";
            }
            frm_NewGroup frm = new frm_NewGroup();
            frm.calledIndirect = true;
            gpid = Select_oth_id(gpnm).ToString();
            frm.LoadData(gpid, "Edit Group");
            frm.ShowDialog();
            newGpnm = frm.GrpName;
            if (newGpnm == "" || newGpnm == null)
            {
                return gpnm;
            }
            else
            {
                return newGpnm;
            }    
        }

        public static String AddItem()
        {
            String Itemnm;
            frmItem frm = new frmItem();
            frm.calledIndirect = true;
            frm.LoadData("0", "New Item");
            frm.ShowDialog();
            Itemnm = frm.ItemName;
            return Itemnm;
        }

        public static String EditItem(String itemnm)
        {
            String newItemnm;
            String Itemid;
            DataTable dtCheckItem = new DataTable();
            Database.GetSqlData("select * from other where [name]='" + itemnm + "' and [type]=15", dtCheckItem);
            if (dtCheckItem.Rows.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("Item does not exist");
                return "";
            }
            frmItem frm = new frmItem();
            frm.calledIndirect = true;
            Itemid = Select_oth_id(itemnm).ToString();
            frm.LoadData(Itemid, "Edit Item");
            frm.ShowDialog();
            newItemnm = frm.ItemName;
            if (newItemnm == "" || newItemnm == null)
            {
                return itemnm;
            }
            else
            {
                return newItemnm;
            }
        }

        public static String AddColor()
        {
            String Colornm;
            frmItem frm = new frmItem();
            frm.calledIndirect = true;
            frm.LoadData("0", "New Color");
            frm.ShowDialog();
            Colornm = frm.ItemName;
            return Colornm;
        }

        public static String EditColor(String colornm)
        {
            String newColornm;
            String Colorid;
            DataTable dtCheckColor = new DataTable();
            Database.GetSqlData("select * from other where [name]='" + colornm + "' and [type]=18", dtCheckColor);
            if (dtCheckColor.Rows.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("Color does not exist");
                return "";
            } 
            frmItem frm = new frmItem();
            frm.calledIndirect = true;
            Colorid = Select_oth_id(colornm).ToString();
            frm.LoadData(Colorid, "Edit Color");
            frm.ShowDialog();
            newColornm = frm.ItemName;
            if (newColornm == "" || newColornm == null)
            {
                return colornm;
            }
            else
            {
                return newColornm;
            }
        }

        public static String AddItemGroup()
        {
            String Companynm;
            frmItem frm = new frmItem();
            frm.calledIndirect = true;
            frm.LoadData("0", "New PriceGroup");
            frm.ShowDialog();
            Companynm = frm.ItemName;
            return Companynm;
        }

        public static String AddDepartment()
        {
            String Companynm;
            frmItem frm = new frmItem();
            frm.calledIndirect = true;
            frm.LoadData("0", "New Department");
            frm.ShowDialog();
            Companynm = frm.ItemName;
            return Companynm;
        }

        public static String AddCompany()
        {
            String Companynm;
            frmItem frm = new frmItem();
            frm.calledIndirect = true;
            frm.LoadData("0", "New Company");
            frm.ShowDialog();
            Companynm = frm.ItemName;
            return Companynm;
        }

        public static String EditCompany(String companynm)
        {
            String newCompanynm;
            String Companyid;
            DataTable dtCheckCompany = new DataTable();
            Database.GetSqlData("select * from other where [name]='" + companynm + "' and [type]=14", dtCheckCompany);
            if (dtCheckCompany.Rows.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("Company does not exist");
                return "";
            } 
            frmItem frm = new frmItem();
            frm.calledIndirect = true;
            Companyid = Select_oth_id(companynm).ToString();
            frm.LoadData(Companyid, "Edit Company");
            frm.ShowDialog();
            newCompanynm = frm.ItemName;
            if (newCompanynm == "" || newCompanynm == null)
            {
                return companynm;
            }
            else
            {
                return newCompanynm;
            }
        }

        public static String EditPricegroup(String companynm)
        {
            String newCompanynm;
            String Companyid;
            DataTable dtCheckCompany = new DataTable();
            Database.GetSqlData("select * from other where [name]='" + companynm + "' and [type]=16", dtCheckCompany);
            if (dtCheckCompany.Rows.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("PriceGroup does not exist");
                return "";
            }
            frmItem frm = new frmItem();
            frm.calledIndirect = true;
            Companyid = Select_oth_id(companynm).ToString();
            frm.LoadData(Companyid, "Edit PriceGroup");
            frm.ShowDialog();
            newCompanynm = frm.ItemName;
            if (newCompanynm == "" || newCompanynm == null)
            {
                return companynm;
            }
            else
            {
                return newCompanynm;
            }
        }

        public static String EditDepartment(String Depnm)
        {
            int depactid = Database.GetScalarInt("Select act_id from accountype where Name='Department'");
            String newCompanynm;
            String Companyid;
            DataTable dtCheckCompany = new DataTable();
            Database.GetSqlData("select * from other where [name]='" + Depnm + "' and [type]=" + depactid, dtCheckCompany);
            if (dtCheckCompany.Rows.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("Department does not exist");
                return "";
            }
            frmItem frm = new frmItem();
            frm.calledIndirect = true;
            Companyid = Select_oth_id(Depnm).ToString();
            frm.LoadData(Companyid, "Edit Department");
            frm.ShowDialog();
            newCompanynm = frm.ItemName;
            if (newCompanynm == "" || newCompanynm == null)
            {
                return Depnm;
            }
            else
            {
                return newCompanynm;
            }
        }

        public static String AddTax()
        {
            String Taxnm;
            frm_tax frm = new frm_tax();
            frm.calledIndirect = true;
            frm.LoadData("0", "New Tax Category");
            frm.ShowDialog();
            Taxnm = frm.TaxCategoryName;
            return Taxnm;
        }

        public static String EditTax(String taxnm)
        {
            String newTaxnm;
            String Taxid;
            DataTable dtCheckTax = new DataTable();
            Database.GetSqlData("select * from taxcategory where Category_Name='" + taxnm + "'", dtCheckTax);
            if (dtCheckTax.Rows.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("Tax Category does not exist");
                return "";
            }
            frm_tax frm = new frm_tax();
            frm.calledIndirect = true;
            Taxid = Select_tax_cat_id(taxnm).ToString();
          
            frm.LoadData(Taxid, "Edit Tax Category");
            frm.ShowDialog();
            newTaxnm = frm.TaxCategoryName;
            if (newTaxnm == "" || newTaxnm == null)
            {
                return taxnm;
            }
            else
            {
                return newTaxnm;
            }
        }

        public static String AddPriceGp()
        {
            String PriceGpnm;
            frmItem frm = new frmItem();
            frm.calledIndirect = true;
            frm.LoadData("0", "New Price Group");
            frm.ShowDialog();
            frm.Type = "";
            PriceGpnm = frm.ItemName;
            return PriceGpnm;
        }
        
        public static String EditPriceGp(String priceGpnm)
        {
            String newPriceGpnm;
            String PriceGpid;
            DataTable dtCheckPriceGp = new DataTable();
            Database.GetSqlData("select * from other where [name]='" + priceGpnm + "' and [type]=16", dtCheckPriceGp);
            if (dtCheckPriceGp.Rows.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("Price Group does not exist");
                return "";
            } 
            frmItem frm = new frmItem();
            frm.calledIndirect = true;
            PriceGpid = Select_oth_id(priceGpnm).ToString();
            frm.LoadData(PriceGpid, "Edit Price Group");
            frm.ShowDialog();
            newPriceGpnm = frm.ItemName;
            if (newPriceGpnm == "" || newPriceGpnm == null)
            {
                return priceGpnm;
            }
            else
            {
                return newPriceGpnm;
            }
        }

        public static String AddProduct(String Type)
        {
            String Pname;
            frmItem frm = new frmItem();
            frm.calledIndirect = true;
            frm.Type = Type;
            frm.LoadData("0", "New");   
            frm.ShowDialog();
            Pname = frm.ItemName;
            return Pname;  
        }

        public static string Select_tax_cat_id(String catnm)
        {
            if (Master.TaxCategory.Select("[Category_Name]='" + catnm + "'").Length == 0)
            {
                return "";
            }
            else
            {
                return Master.TaxCategory.Select("[Category_Name]='" + catnm + "'").FirstOrDefault()["Category_Id"].ToString();
            }
        }

        public static int Select_AccTypeRegsqn(string Actname)
        {
            if (Master.AccountType.Select("[Name]='" + Actname + "'").Length == 0)
            {
                return 0;
            }
            else
            {
                return int.Parse(Master.AccountType.Select("[Name]='" + Actname + "'").FirstOrDefault()["Regsqn"].ToString());
            }
        }

        public static string Select_tax_cat_code(String catnm)
        {
            if (Master.TaxCategory.Select("[Category_Name]='" + catnm + "'").Length == 0)
            {
                return "";
            }
            else
            {
                return Master.TaxCategory.Select("[Category_Name]='" + catnm + "'").FirstOrDefault()["Commodity_Code"].ToString();
            }
        }

        public static double Select_tax_cat_purcgst(String catnm)
        {
            return Database.GetScalarDecimal("select sum(STR1+STR2) from TaxCategory where Category_Name='" + catnm + "'");
        }

        public static double Select_tax_cat_salecgst(String catnm)
        {
            return Database.GetScalarDecimal("select sum(PTR1+PTR2) from TaxCategory where Category_Name='" + catnm + "'");
        }

        public static double Select_tax_cat_purigst(String catnm)
        {
            return Database.GetScalarDecimal("select STR3 from TaxCategory where Category_Name='" + catnm + "'");
        }

        public static double Select_tax_cat_saleigst(String catnm)
        {
            return Database.GetScalarDecimal("select PTR3 from TaxCategory where Category_Name='" + catnm + "'");
        }

        public static String Select_tax_cat_nm(string tax_cat_id)
        {
            return Database.GetScalarText("select Category_Name from TaxCategory where Category_Id='" + tax_cat_id+"' ");
        }

        public static int Select_sale_pur_id(int tax_cat_id,String subtaxcat)
        {
            return Database.GetScalarInt("select Sale_Pur_Acc_id from taxcategorydetail where Category_Id=" + tax_cat_id + " and SubCategory_Name='" + subtaxcat + "'");
        }

        public static double Select_tax_sum(int tax_cat_id, String subtaxcat)
        {
            return Database.GetScalarDecimal("select Sum(TAXCATEGORYDETAIL.Tax_Rate) AS SumOfTax FROM TAXCATEGORYDETAIL GROUP BY TAXCATEGORYDETAIL.Category_Id, TAXCATEGORYDETAIL.SubCategory_Name HAVING (((TAXCATEGORYDETAIL.Category_Id)=" + tax_cat_id + " ) AND ((TAXCATEGORYDETAIL.SubCategory_Name)='" + subtaxcat + "'))");
        }

        public static string Select_ac_id(String accname)
        {
            if (Master.Account.Select("[Name]='" + accname + "'").Length == 0)
            {
                return "";
            }
            else
            {
                return Master.Account.Select("[Name]='" + accname + "'").FirstOrDefault()["ac_id"].ToString();
            }
        }

        public static string Select_branch_id(String brname)
        {
            return Database.GetScalarText("select id from Branch where Bname='" + brname + "'");  
        }

        public static string Select_branch_name(string brid)
        {
            return Database.GetScalarText("select Bname from Branch where id='" + brid + "' ");
        }

        public static string Select_ac_regstatus(string accid)
        {
            if (Master.Account.Select("[Ac_id]='" + accid+ "'").Length == 0)
            {
                return "";
            }
            else
            {
                return Master.Account.Select("[Ac_id]='" + accid+"'").FirstOrDefault()["Regstatus"].ToString();
            }
        }

        public static string Select_state_id(String statename)
        {
            if (Master.State.Select("[Sname]='" + statename + "'").Length == 0)
            {
                return "";
            }
            else
            {
                return Master.State.Select("[Sname]='" + statename + "'").FirstOrDefault()["State_id"].ToString();
            }
        }

        public static int Select_ac_dlimit(String accname)
        {
            if (Master.Account.Select("[Name]='" + accname + "'").Length == 0)
            {
                return 0;
            }
            else
            {
                return int.Parse(Master.Account.Select("[Name]='" + accname + "'").FirstOrDefault()["Dlimit"].ToString());
            }
        }

        public static string Select_Mobile(String name)
        {
            if (Master.Account.Select("[Name]='" + name + "'").Length == 0)
            {
                return "";
            }
            else
            {
                return Master.Account.Select("[Name]='" + name + "'").FirstOrDefault()["Phone"].ToString();
            }
        }

        public static string Select_ac_state_id(String accname)
        {
            if (Master.Account.Select("[Name]='" + accname + "'").Length == 0)
            {
                return "";
            }
            else
            {
                return Master.Account.Select("[Name]='" + accname + "'").FirstOrDefault()["State_id"].ToString();
            }
        }

        public static string Select_Print(String name)
        {
            if (Master.Account.Select("[Name]='" + name + "'").Length == 0)
            {
                return "";
            }
            else
            {
                return Master.Account.Select("[Name]='" + name + "'").FirstOrDefault()["Printname"].ToString();
            }
        }

        public static string Select_TIN(String name)
        {
            if (Master.Account.Select("[Name]='" + name + "'").Length == 0)
            {
                return "";
            }
            else
            {
                return Master.Account.Select("[Name]='" + name + "'").FirstOrDefault()["Tin_number"].ToString();
            }
        }

        public static string Select_PAN(String name)
        {
            if (Master.Account.Select("[Name]='" + name + "'").Length == 0)
            {
                return "";
            }
            else
            {
                return Master.Account.Select("[Name]='" + name + "'").FirstOrDefault()["PAN"].ToString();
            }
        }

        public static string Select_AAdhar(String name)
        {
            return Database.GetScalarText("select Aadhaarno from ACCOUNT where [Name]='" + name + "'");
        }

        public static string Select_vt_id_nm(string name)
        {
            if (Master.VoucherType.Select("[Name]='" + name + "'").Length == 0)
            {
                return "0";
            }
            else
            {
                return Master.VoucherType.Select("[Name]='" + name + "'").FirstOrDefault()["Vt_id"].ToString();
            }
        }

        public static string Select_Email(String name)
        {
            return Database.GetScalarText("select Email from ACCOUNT where [Name]='" + name + "'");
        }

        public static string Select_Address1(String name)
        {
            return Database.GetScalarText("select Address1 from ACCOUNT where [Name]='" + name + "'");
        }

        public static string Select_Address2(String name)
        {
            return Database.GetScalarText("select Address2 from ACCOUNT where [Name]='" + name + "'");
        }

        public static String Select_ac_nm(string ac_id)
        {
            return Database.GetScalarText("select Name from ACCOUNT where Ac_id='" + ac_id + "'");
        }

        public static String Select_state_nm(string state_id)
        {
            return Database.GetScalarText("select SName from State where State_id='" + state_id + "'");
        }

        public static String Select_state_GST(string statename)
        {
            return Database.GetScalarText("select GSTCode from State where [Sname]='" + statename + "'");
        }

        public static string Select_act_id(String name)
        {
            return Database.GetScalarText("select act_id from ACCOUNTYPE where [name]='" + name + "'");
        }

        public static string Select_Refineact_id(String name)
        {
            return Database.GetScalarText("select act_id from ACCOUNTYPE where [refinename]='" + name + "'");
        }

        //public static String Select_act_nm(string act_id)
        //{
        //    return Database.GetScalarText("select name from ACCOUNTYPE where act_id='" + act_id + "'");
        //}
        public static String Select_act_nm(string act_id)
        {
            if (Master.AccountType.Select("[act_id]='" + act_id+"' ").Length == 0)
            {
                return "0";
            }
            else
            {
                return Master.AccountType.Select("[act_id]='" + act_id + "'").FirstOrDefault()["name"].ToString();
            }
        }
        
        public static String Select_Refineact_nm(string act_id)
        {

            if (Master.AccountType.Select("[act_id]='" + act_id + "' ").Length == 0)
            {
                return "";
            }
            else
            {
                return Master.AccountType.Select("[act_id]='" + act_id + "'").FirstOrDefault()["refinename"].ToString();
            }
        }

        public static string Select_ch_id(String chname)
        {
            return Database.GetScalarText("select ch_id from CHARGES where [name]='" + chname + "'");
        }

        public static string Select_salesman_id(String name)
        {
            return Database.GetScalarText("select s_id from Salesman where [name]='" + name + "'");
        }

        public static String Select_salesman_nm(string s_id)
        {
            return Database.GetScalarText("select [name] from Salesman where s_id='" + s_id + "'");
        }

        public static int Select_impdates_id(String title)
        {
            return Database.GetScalarInt("select [id] from importantdate where [title]='" + title + "'");
        }

        public static String Select_ch_nm(int ch_id)
        {
            return Database.GetScalarText("select [name] from CHARGES where ch_id='" + ch_id + "'");
        }

        public static string Select_oth_id(String OtherName)
        {
            if (Master.Other.Select("[name]='" + OtherName + "'").Length == 0)
            {
                return "";
            }
            else
            {
                return Master.Other.Select("[name]='" + OtherName + "'").FirstOrDefault()["oth_id"].ToString();
            }
        }

        public static String Select_oth_nm(string other_id)
        {
            if (Master.Other.Select("[oth_id]='" + other_id + "' ").Length == 0)
            {
                return "";
            }
            else
            {
                return Master.Other.Select("[oth_id]='" + other_id + "'").FirstOrDefault()["name"].ToString();
            }
        }

        public static string Select_con_id(String con_name)
        {
            if (Master.Agent.Select("[name]='" + con_name + "'").Length == 0)
            {
                return "";
            }
            else
            {
                return Master.Agent.Select("[name]='" + con_name + "'").FirstOrDefault()["con_id"].ToString();
            }
        }

        public static String Select_con_nm(string con_id)
        {
            if (Master.Agent.Select("[con_id]='" + con_id + "'").Length == 0)
            {
                return "";
            }
            else
            {
                return Master.Agent.Select("[con_id]='" + con_id + "' ").FirstOrDefault()["name"].ToString();
            }
        }

        public static int Select_controlroom_id(String Feature)
        {
            if (Master.Controlroom.Select("[Features]='" + Feature + "'").Length == 0)
            {
                return 0;
            }
            else
            {
                return int.Parse(Master.Controlroom.Select("[Features]='" + Feature + "'").FirstOrDefault()["ID"].ToString());
            }
        }

        public static double Select_des_Wlavel(string des_id)
        {
            if (Master.Description.Select("[Des_id]='" + des_id+"' ").Length == 0)
            {
                return 0;
            }
            else
            {
                return double.Parse(Master.Description.Select("[Des_id]='" + des_id+"' ").FirstOrDefault()["Wlavel"].ToString());
            }
        }

        public static String Select_pack_nm(string des_id)
        {
            if (Master.Description.Select("[Des_id]='" + des_id+"' ").Length == 0)
            {
                return "";
            }
            else
            {
                return Master.Description.Select("[Des_id]='" + des_id+"' ").FirstOrDefault()["pack"].ToString();
            }
        }

        public static string Select_category_id(String category_name)
        {
            if (Master.TaxCategory.Select("[category_name]='" + category_name + "' ").Length == 0)
            {
                return "";
            }
            else
            {
                return Master.TaxCategory.Select("[category_name]='" + category_name + "' ").FirstOrDefault()["category_id"].ToString();
            }
        }

        public static String Select_category_nm(string category_id)
        {
            return Database.GetScalarText("select category_name from TaxCategory where category_id='" + category_id + "'");
        }

        public static string Select_vt_id_vid(string vi_id)
        {
            return Database.GetScalarText("Select Vt_id from Voucherinfo where  Vi_id='" + vi_id + "' ");
        }

        public static string Select_vt_id_vnm(String vt_name)
        {
            return Database.GetScalarText("select vt_id from VOUCHERTYPE where [name]='" + vt_name + "'");
        }

        public static string Select_vt_short(String vt_name)
        {
            if (Master.VoucherType.Select("[name]='" + vt_name + "'").Length == 0)
            {
                return "";
            }
            else
            {
                return Master.VoucherType.Select("[name]='" + vt_name + "'").FirstOrDefault()["Short"].ToString();
            }
        }

        public static int Select_NumType(string vt_id)
        {
            if (Master.VoucherType.Select("[Vt_id]='" + vt_id + "'").Length == 0)
            {
                return 0;
            }
            else
            {
                return int.Parse(Master.VoucherType.Select("[Vt_id]='" + vt_id + "'").FirstOrDefault()["Numtype"].ToString());
            }
        }
        public static string Select_vt_id(string vi_id)
        {

            return Database.GetScalarText("Select Vt_id from Voucherinfo where  Vi_id='" + vi_id+"'");
        }

        public static string Select_AccTypeid(string AccountName)
        {
            if (Master.Account.Select("[Name]='" + AccountName + "'").Length == 0)
            {
                return "";
            }
            else
            {
                return Master.Account.Select("[Name]='" + AccountName + "'").FirstOrDefault()["Act_id"].ToString();
            }
        }

        public static string Select_AccTypeids(string Ac_id)
        {
            if (Master.Account.Select("[Ac_id]='" + Ac_id + "'").Length == 0)
            {
                return "";
            }
            else
            {
                return Master.Account.Select("[Ac_id]='" + Ac_id + "'").FirstOrDefault()["Act_id"].ToString();
            }
        }

        public static string Select_Agentid(string AccountName)
        {
            if (Master.Account.Select("[Name]='" + AccountName + "'").Length == 0)
            {
                return "";
            }
            else
            {
                return Master.Account.Select("[Name]='" + AccountName + "'").FirstOrDefault()["Con_id"].ToString();
            }
        }

        public static string Select_Groupid(string AccountName)
        {
            if (Master.Account.Select("[Name]='" + AccountName + "'").Length == 0)
            {
                return "";
            }
            else
            {
                return Master.Account.Select("[Name]='" + AccountName + "'").FirstOrDefault()["Loc_id"].ToString();
            }
        }

        public static string Select_vt_RateType(string vt_id)
        {

            if (Master.VoucherType.Select("[Vt_id]='" + vt_id + "'").Length == 0)
            {
                return "";
            }
            else
            {
                return Master.VoucherType.Select("[Vt_id]='" + vt_id + "'").FirstOrDefault()["Ratetype"].ToString();
            }
        }

        public static bool Select_vt_taxinvoice(string vt_id)
        {
            return Database.GetScalarBool("select TaxInvoice from VOUCHERTYPE where Vt_id='" + vt_id + "'");
        }

        public static bool Select_vt_Exstate(string vt_id)
        {
            if (Master.VoucherType.Select("[Vt_id]='" + vt_id + "'").Length == 0)
            {
                return false;
            }
            else
            {
                return bool.Parse(Master.VoucherType.Select("[Vt_id]='" + vt_id + "'").FirstOrDefault()["ExState"].ToString());
            }
        }

        public static bool Select_des_stkMaintain(string desc_id)
        {
            if (Master.Description.Select("[Des_id]='" + desc_id+"' ").Length == 0)
            {
                return false;
            }
            else
            {
                return bool.Parse(Master.Description.Select("[Des_id]='" + desc_id+"' ").FirstOrDefault()["StkMaintain"].ToString());
            }
        }

        public static string Select_vt_Exempted(string vt_id)
        {
            if (Master.VoucherType.Select("[Vt_id]='" + vt_id + "'").Length == 0)
            {
                return "No";
            }
            else
            {
                return Master.VoucherType.Select("[Vt_id]='" + vt_id + "'").FirstOrDefault()["Exempted"].ToString();
            }
        }

        public static string Select_Rates_Value(string RatesId)
        {
            if (Master.DtRates.Select("[RateId]='" + RatesId + "'").Length == 0)
            {
                return "0";
            }
            else
            {
                return Master.DtRates.Select("[RateId]='" + RatesId + "'").FirstOrDefault()["RateValue"].ToString();
            }
        }

        public static string Select_Rates_Id(string RatesValue)
        {
            if (Master.DtRates.Select("[RateValue]='" + RatesValue + "'").Length == 0)
            {
                return "";
            }
            else
            {
                return Master.DtRates.Select("[RateValue]='" + RatesValue + "'").FirstOrDefault()["RateId"].ToString();
            }
        }

        public static bool Select_vt_Excludungtax(string vt_id)
        {
            if (Master.VoucherType.Select("[Vt_id]='" + vt_id + "'").Length == 0)
            {
                return false;
            }
            else
            {
                return bool.Parse(Master.VoucherType.Select("[Vt_id]='" + vt_id + "'").FirstOrDefault()["ExcludingTax"].ToString());
            }
        }

        public static string Select_vt_CalculationType(string vt_id)
        {
            if (Master.VoucherType.Select("[Vt_id]='" + vt_id + "'").Length == 0)
            {
                return "0";
            }
            else
            {
                return Master.VoucherType.Select("[Vt_id]='" + vt_id + "'").FirstOrDefault()["Calculation"].ToString();
            }
        }

        public static string Select_vt_Cashtran(string vt_id)
        {

            if (Master.VoucherType.Select("[Vt_id]='" + vt_id + "'").Length == 0)
            {
                return "0";
            }
            else
            {
                return Master.VoucherType.Select("[Vt_id]='" + vt_id + "'").FirstOrDefault()["CashTransaction"].ToString();
            }
        }

        public static bool Select_vt_Includingtax(string vt_id)
        {
            if (Master.VoucherType.Select("[Vt_id]='" + vt_id + "'").Length == 0)
            {
                return false;
            }
            else
            {
                return bool.Parse(Master.VoucherType.Select("[Vt_id]='" + vt_id + "'").FirstOrDefault()["IncludingTax"].ToString());
            }
        }

        public static bool Select_vt_Unregistered(string vt_id)
        {
            if (Master.VoucherType.Select("[Vt_id]='" + vt_id + "'").Length == 0)
            {
                return false;
            }
            else
            {
                return bool.Parse(Master.VoucherType.Select("[Vt_id]='" + vt_id + "'").FirstOrDefault()["Unregistered"].ToString());
            }
        }

        public static String Select_vt_nm(string vt_id)
        {

            if (Master.VoucherType.Select("[Vt_id]='" + vt_id + "'").Length == 0)
            {
                return "0";
            }
            else
            {
                return Master.VoucherType.Select("[Vt_id]='" + vt_id + "'").FirstOrDefault()["name"].ToString();
            }  
        }

        public static String Select_vt_Alias(string vt_id)
        {
            if (Master.VoucherType.Select("[Vt_id]='" + vt_id + "'").Length == 0)
            {
                return "0";
            }
            else
            {
                return Master.VoucherType.Select("[Vt_id]='" + vt_id + "'").FirstOrDefault()["AliasName"].ToString();
            }
        }

        public static string Select_des_id(String des_name, String packnm)
        {
            if (Master.DescriptionInfo.Select("Description='" + des_name + "' and packing='" + packnm + "' ").Length == 0)
            {
                return "0";
            }
            else
            {
                return Master.DescriptionInfo.Select("Description='" + des_name + "' and packing='" + packnm + "' ").FirstOrDefault()["des_id"].ToString();
            }
        }

        public static String Select_des_nm(string des_id)
        {
            if (Master.Description.Select("des_id='" + des_id+"' " ).Length == 0)
            {
                return "0";
            }
            else
            {
                return Master.Description.Select("des_id='" + des_id+"' ").FirstOrDefault()["Description"].ToString();
            }
        }

        public static int Select_vi_id(int vnm, int id, String dt)
        {
            return Database.GetScalarInt("select vi_id voucherinfo where vnumber=" + vnm + " and vt_id=" + id);
        }

        public static double Select_pack_value(string des_id)
        {
            if (Master.Description.Select("[Des_id]='" + des_id+"'").Length == 0)
            {
                return 0;
            }
            else
            {
                return double.Parse(Master.Description.Select("[Des_id]='" + des_id + "' ").FirstOrDefault()["Pvalue"].ToString());
            }
        }

        public static String DecimalPoint(Object o, int count)
        {
            string str=".";
            for (int i = 0; i < count; i++)
            {
                str += "0"; 
            }
            if (count == 0)
            {
                str = "";
            }
            String conVal;
            conVal = String.Format("{0:0" + str + "}", o);
            return conVal;
        }

        public static String DecimalPoint(Object o)
        {
            return DecimalPoint(o, 2);
        }

        public static string IndianCurr(double o)
        {
            System.Globalization.CultureInfo cuInfo = new System.Globalization.CultureInfo("hi-IN");
            return (o.ToString("C", cuInfo)).Remove(0, Database.trimno).Trim();
        }

        public static int chkNumType(string vtid)
        {
            return Database.GetScalarInt("select Numtype from vouchertype where vt_id='" + vtid + "' ");
        }

        public static int GenerateVno(string vtid, String dt, string vid)
        {
             string wherstr = "";
            int prospective = 0;
            int numtype = funs.Select_NumType(vtid);

            if (numtype == 1)//yearly
            {
                wherstr = ""; 
            }
            else if (numtype == 2) //monthly
            {
                wherstr= " and (month(vdate)=" + DateTime.Parse(dt).Month + ")";
            }
            else if (numtype == 3) //daily
            {
                wherstr = " and Vdate= " + access_sql.Hash + dt + access_sql.Hash ;
            }

            prospective = Database.GetScalarInt("SELECT Max(Vnumber) AS Expr1 FROM VOUCHERINFO, VOUCHERTYPE WHERE VOUCHERINFO.Vt_id=VOUCHERTYPE.Vt_id AND Voucherinfo.Branch_id='" + Database.BranchId + "'  And VOUCHERTYPE.Code= (SELECT VOUCHERTYPE.Code FROM VOUCHERTYPE WHERE VOUCHERTYPE.Vt_id='" + vtid + "'" + wherstr + ")") + 1;

            //date verification
            String Pre = "";
            String nex = "";

            if (numtype == 1)  //yearly
            {
                Pre = Database.GetScalarDate("SELECT Max(VOUCHERINFO.Vdate) As Vdate FROM VOUCHERINFO WHERE (((VOUCHERINFO.Vnumber)<" + prospective + ") AND ((VOUCHERINFO.Vt_id)='" + vtid + "') and vi_id <>'" + vid + "' and Voucherinfo.Branch_id='" + Database.BranchId + "' )");
                nex = Database.GetScalarDate("SELECT Min(VOUCHERINFO.Vdate) AS Vdate FROM VOUCHERINFO WHERE (((VOUCHERINFO.Vnumber)>" + prospective + ") AND ((VOUCHERINFO.Vt_id)='" + vtid + "') and vi_id <>'" + vid + "' and Voucherinfo.Branch_id='" + Database.BranchId + "')");
            }

            else if (numtype == 2) //monthly
            {
                Pre = Database.GetScalarDate("SELECT Max(VOUCHERINFO.Vdate) As Vdate FROM VOUCHERINFO WHERE VOUCHERINFO.Vnumber<" + prospective + " AND VOUCHERINFO.Vt_id='" + vtid + "' and (month(vdate)=" + DateTime.Parse(dt).Month + ") and vi_id <>'" + vid + "' and Voucherinfo.Branch_id='" + Database.BranchId + "'");
                nex = Database.GetScalarDate("SELECT Min(VOUCHERINFO.Vdate) As Vdate FROM VOUCHERINFO WHERE VOUCHERINFO.Vnumber>" + prospective + " AND VOUCHERINFO.Vt_id='" + vtid + "' and (month(vdate)=" + DateTime.Parse(dt).Month + ") and vi_id <>'" + vid + "' and Voucherinfo.Branch_id='" + Database.BranchId + ";");
            }

            else if (numtype == 3) //daily
            {
                Pre = "";
                nex = "";
            }

            if (Pre == "" && nex == "")
            {
                return prospective;
            }
            else if (DateTime.Parse(dt) >= DateTime.Parse(Pre) && nex == "")
            {
                return prospective;
            }
            else if (DateTime.Parse(dt) >= DateTime.Parse(Pre) && DateTime.Parse(dt) <= DateTime.Parse(nex))
            {
                return prospective;
            }
            else
            {
                return 0;
            }
        }

        public static String Stock(string desc_id)
        {
            String stck;
            double opstck=0,sale=0,saleret=0,pur=0,puret=0;
            
            DataTable dtOpenStock = new DataTable();
            Database.GetSqlData("select Open_stock from description where Des_id='" + desc_id + "' ", dtOpenStock);
            if (dtOpenStock.Rows.Count > 0)
            {
                opstck = double.Parse(dtOpenStock.Rows[0]["Open_stock"].ToString());
            }
            DataTable dtSale = new DataTable();
            Database.GetSqlData("SELECT Sum(VOUCHERDET.Quantity) AS Qty FROM VOUCHERINFO INNER JOIN VOUCHERDET ON VOUCHERINFO.Vi_id = VOUCHERDET.Vi_id WHERE (((VOUCHERINFO.Vt_id)='SER1' Or (VOUCHERINFO.Vt_id)='SER2' Or (VOUCHERINFO.Vt_id)='SER3' Or (VOUCHERINFO.Vt_id)='SER4')) GROUP BY VOUCHERDET.Des_ac_id HAVING (((VOUCHERDET.Des_ac_id)='" + desc_id + "'))", dtSale);
            if (dtSale.Rows.Count > 0)
            {
                sale = double.Parse(dtSale.Rows[0]["Qty"].ToString());
            }
            DataTable dtSaleRet = new DataTable();
            Database.GetSqlData("SELECT Sum(VOUCHERDET.Quantity) AS Qty FROM VOUCHERINFO INNER JOIN VOUCHERDET ON VOUCHERINFO.Vi_id = VOUCHERDET.Vi_id WHERE (((VOUCHERINFO.Vt_id)='SER16' Or (VOUCHERINFO.Vt_id)='SER5' Or (VOUCHERINFO.Vt_id)='SER15' Or (VOUCHERINFO.Vt_id)='SER17')) GROUP BY VOUCHERDET.Des_ac_id HAVING (((VOUCHERDET.Des_ac_id)='" + desc_id + "'))", dtSaleRet);
            if (dtSaleRet.Rows.Count > 0)
            {
                saleret = double.Parse(dtSaleRet.Rows[0]["Qty"].ToString());
            }
            DataTable dtPurchase = new DataTable();
            Database.GetSqlData("SELECT Sum(VOUCHERDET.Quantity) AS Qty FROM VOUCHERINFO INNER JOIN VOUCHERDET ON VOUCHERINFO.Vi_id = VOUCHERDET.Vi_id WHERE (((VOUCHERINFO.Vt_id)='SER18' Or (VOUCHERINFO.Vt_id)='SER24' Or (VOUCHERINFO.Vt_id)='SER60')) GROUP BY VOUCHERDET.Des_ac_id HAVING (((VOUCHERDET.Des_ac_id)='" + desc_id + "'))", dtPurchase);
            if (dtPurchase.Rows.Count > 0)
            {
                pur = double.Parse(dtPurchase.Rows[0]["Qty"].ToString());
            }
            DataTable dtPurchaseRet = new DataTable();
            Database.GetSqlData("SELECT Sum(VOUCHERDET.Quantity) AS Qty FROM VOUCHERINFO INNER JOIN VOUCHERDET ON VOUCHERINFO.Vi_id = VOUCHERDET.Vi_id WHERE (((VOUCHERINFO.Vt_id)='SER19' Or (VOUCHERINFO.Vt_id)='SER25' Or (VOUCHERINFO.Vt_id)='SER61')) GROUP BY VOUCHERDET.Des_ac_id HAVING (((VOUCHERDET.Des_ac_id)='" + desc_id + "'))", dtPurchaseRet);
            if (dtPurchaseRet.Rows.Count > 0)
            {
                puret = double.Parse(dtPurchaseRet.Rows[0]["Qty"].ToString());
            }
            stck = (opstck + pur - puret - (sale - saleret)).ToString();
            return stck;
        }

        public static string Select_color_group_id(String name)
        {
            if (Master.Other.Select("[name]='" + name + "'").Length == 0)
            {
                return "";
            }
            else
            {
                return Master.Other.Select("[name]='" + name + "'").FirstOrDefault()["oth_id"].ToString();
            }
        }

        public static String AccountBalance(string Name)
        {
            if (Master.Accountinfo.Select("[Name]='" + Name + "'").Length == 0)
            {
                return "0";
            }
            else
            {
                return Master.Accountinfo.Select("[Name]='" + Name + "' ").FirstOrDefault()["Balance"].ToString();
            }
        }

        public static String accbal2(string ac_id)
        {
            String curbal;
            double opbal = 0, bal = 0;
            
            DataTable dtOpenBal = new DataTable();
            if (Database.IsKacha == false)
            {
                Database.GetSqlData("select dr,cr from account where Ac_id=" + ac_id, dtOpenBal);
            }
            else
            {
                Database.GetSqlData("select dr2 as dr ,cr2 as cr from account where Ac_id=" + ac_id, dtOpenBal);
            }

            if (dtOpenBal.Rows.Count > 0)
            {
                if (double.Parse(dtOpenBal.Rows[0]["Dr"].ToString()) != 0)
                {
                    opbal = double.Parse(dtOpenBal.Rows[0]["Dr"].ToString());
                }
                else
                {
                    opbal = -(double.Parse(dtOpenBal.Rows[0]["Cr"].ToString()));
                }
            }
            
            string acname= funs.Select_ac_nm(ac_id);
            DataTable dtBal = new DataTable();
            if (Database.IsKacha == false)
            {
                Database.GetSqlData("SELECT Sum(QryJournal.Dr) AS Dramt, Sum(QryJournal.Cr) AS Cramt FROM QryJournal WHERE (((QryJournal.Name)='"+ acname +"')) GROUP BY QryJournal.A HAVING (((QryJournal.A)=True))",dtBal);
            }
            else
            {
                Database.GetSqlData("SELECT Sum(QryJournal.Dr) AS Dramt, Sum(QryJournal.Cr) AS Cramt FROM QryJournal WHERE (((QryJournal.Name)='" + acname + "')) GROUP BY QryJournal.B HAVING (((QryJournal.B)=True))", dtBal);
            }

            if (dtBal.Rows.Count > 0)
            {
                if( dtBal.Rows[0]["Cramt"].ToString() == "" || dtBal.Rows[0]["Dramt"].ToString() == "")
                {
                    dtBal.Rows[0]["Cramt"] = 0;
                }

                if (double.Parse(dtBal.Rows[0]["Dramt"].ToString()) > double.Parse(dtBal.Rows[0]["Cramt"].ToString()))
                {
                    bal = double.Parse(dtBal.Rows[0]["Dramt"].ToString()) - double.Parse(dtBal.Rows[0]["Cramt"].ToString());
                }

                else
                {
                    bal = -(double.Parse(dtBal.Rows[0]["Cramt"].ToString()) - double.Parse(dtBal.Rows[0]["Dramt"].ToString()));
                }

            }
            curbal = (opbal + bal).ToString();

            if (double.Parse(curbal) >= 0)
            {
                curbal += " Dr.";
            }
            else
            {
                curbal += " Cr.";
            }
            return curbal;
        }

        public static String select_rpt_copy(int vtid, int cpy)
        { 
            string columnname = "";
            DataTable dtOptions = new DataTable();
            dtOptions.Clear();
            if (cpy == 1)
            {
                columnname = "Default1";
            }
            else if (cpy == 2)
            {
                columnname = "Default2";
            }
            else if (cpy == 3)
            {
                columnname = "Default3";
            }

            if (Master.VoucherType.Select("[Vt_id]=" + vtid).Length == 0)
            {
                return "0";
            }
            else
            {
                return Master.VoucherType.Select("[Vt_id]=" + vtid).FirstOrDefault()[columnname].ToString();
            }
        }

        public static int Select_disaftertax_id(String taxname)
        {
            return Database.GetScalarInt("Select Tax_id from DisAfterTax where taxname='" + taxname + "'");
        }

        public static string Select_packcat_id(String taxname)
        {
            return Database.GetScalarText("Select PackCat_id from PackCategory where name='" + taxname + "'");
        }

        public static string Select_packcat_name(String tax_id)
        {
            return Database.GetScalarText("Select name from PackCategory where PackCat_id='" + tax_id+"'");
        }

        public static string Select_disaftertax_name(String tax_id)
        {
            return Database.GetScalarText("Select taxname from DisAfterTax where tax_id=" + tax_id);
        }


        public static double Roundoff(String tempamt)
        {
            double amt = 0;
            amt = double.Parse(tempamt);
            amt = Math.Round(amt);
            return amt;


        }




        private static bool validateopnfrm(string vtype)
        {
            string originalvtype = vtype;
            if(vtype=="P Return")
            {

                originalvtype = "PURCHASE RETURN";
            }
            else if (vtype == "Return")
            {
                originalvtype = "SALE RETURN";

            }
            else if (vtype == "Transfer")
            {
                originalvtype = "STOCK JOU";

            }


            for (int k = 0; k < Master.SideMenu.Rows.Count; k++)
            {

                if (Master.SideMenu.Rows[k]["Menuoption"].ToString().ToUpper() == originalvtype.ToUpper())
                {

                  
                    if (bool.Parse(Master.SideMenu.Rows[k]["Display"].ToString()) == false)
                    {
                        return false;
                    }
                }


            }

            return true;
        }

        public static void OpenFrm(System.Windows.Forms.Form thisfrm,string v_id,bool resave)
        {
            Boolean TdType = false;
            
            string frmName = "";
            DataTable dtTdType = new DataTable();
            Database.GetSqlData("select tdtype,vouchertype.type as vname from voucherinfo,vouchertype  where voucherinfo.vt_id=vouchertype.vt_id and vi_id='" + v_id.ToString()+"' " , dtTdType);
            if (dtTdType.Rows.Count > 0)
            {
             
                TdType = Boolean.Parse(dtTdType.Rows[0][0].ToString());
                frmName = dtTdType.Rows[0][1].ToString();
            }
            if (validateopnfrm(frmName) == true)
            {


                string vid = v_id;

                if (frmName == "Receipt")
                {
                    frmCashRec frm = new frmCashRec();
                    frm.recpay = "Receipt";
                    frm.cmdnm = "edit";
                    frm.Text = "Edit Receipt";
                    frm.MdiParent = thisfrm.MdiParent;
                    frm.gresave = resave;
                    frm.LoadData(vid.ToString(), frm.Text);
                    if (resave == true)
                    {
                    }
                    else
                    {
                        frm.Show();
                    }
                 
                }

                else if (frmName == "Payment")
                {
                    frmCashRec frm = new frmCashRec();
                    frm.recpay = "Payment";
                    frm.cmdnm = "edit";
                    frm.Text = "Edit Payment";
                    frm.gresave = resave;
                    frm.MdiParent = thisfrm.MdiParent;
                    frm.LoadData(vid.ToString(), frm.Text);
                    if (resave == true)
                    {
                    }
                    else
                    {
                            frm.Show();
                    }
                    
                }

                else if (frmName == "Contra")
                {
                    frmCashRec frm = new frmCashRec();
                    frm.recpay = "Contra";
                    frm.cmdnm = "edit";
                    frm.Text = "Edit Contra";
                    frm.gresave = resave;
                    frm.MdiParent = thisfrm.MdiParent;
                    frm.LoadData(vid.ToString(), frm.Text);
                    if (resave == true)
                    {
                    }
                    else
                    {
                        frm.Show();
                    }
                    
                }

                else if (frmName == "Journal")
                {
                    frmJournal frm = new frmJournal();
                    frm.Text = "Journal Voucher";
                    frm.cmdmode = "edit";
                    frm.MdiParent = thisfrm.MdiParent;
                    frm.gresave = resave;
                    frm.LoadData(vid.ToString(), frm.Text);
                    if (resave == true)
                    {
                    }
                    else
                    {
                        frm.Show();
                    }
                   
                }

                else if (frmName == "Cnote")
                {
                    frmDebitCredit frm = new frmDebitCredit();
                    frm.dr_cr_note = "Credit Note";
                    frm.MdiParent = thisfrm.MdiParent;
                    frm.cmdnm = "edit";
                    frm.gresave = resave;
                    frm.LoadData(vid.ToString(), "Credit Note");
                  
                    if (resave == true)
                    {
                    }
                    else
                    {
                        frm.Show();
                    }
                   
                }

                else if (frmName == "Dnote")
                {
                    frmDebitCredit frm = new frmDebitCredit();
                    frm.dr_cr_note = "Debit Note";
                    frm.cmdnm = "edit";
                    frm.MdiParent = thisfrm.MdiParent;
                    frm.gresave = resave;
                    frm.LoadData(vid.ToString(), "Debit Note");
                    
                    if (resave == true)
                    {
                    }
                    else
                    {
                        frm.Show();
                    }
                   
                }

                else if (frmName == "Transfer")
                {
                    frm_stkjournal frm = new frm_stkjournal();
                    frm.MdiParent = thisfrm.MdiParent;
                    frm.gresave = resave;

                    frm.LoadData(vid.ToString(), "Stock Journal");
                    if (resave == true)
                    {
                    }
                    else
                    {
                        frm.Show();
                    }
                }

                else if (frmName == "issue" || frmName == "receive")
                {
                    return;
                }

                else
                {
                    DataTable dtalter = new DataTable();
                    Database.GetSqlData("SELECT VOUCHERTYPE.Type, VOUCHERINFO.Tdtype, VOUCHERTYPE.ExState, VOUCHERTYPE.Unregistered FROM VOUCHERINFO,VOUCHERTYPE WHERE VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id and VOUCHERINFO.Vi_id='" + vid + "' ", dtalter);
                    frmTransaction frm = new frmTransaction();
                    frm.cmdmode = "edit";
                    frm.MdiParent = thisfrm.MdiParent;
                    frm.gresave = resave;
                    frm.LoadData(vid, dtalter.Rows[0]["Type"].ToString(), bool.Parse(dtalter.Rows[0]["Tdtype"].ToString()), bool.Parse(dtalter.Rows[0]["ExState"].ToString()), bool.Parse(dtalter.Rows[0]["Unregistered"].ToString()));
                    if (resave == true)
                    {
                    }
                    else
                    {
                        frm.Show();
                    }
                }

                if (Feature.Available("Close Form After Report") == "Yes")
                {
                    thisfrm.Close();
                }
            }
            else
            {
                System.Windows.MessageBox.Show("You don't have Permission to access this..");
            }
        }

        public static string Get_Company_id()
        {
            return Database.GetScalarText("Select act_id from Accountype where Name='Company'");
        }

        public static string Get_Item_id()
        {
            return Database.GetScalarText("Select act_id from Accountype where Name='Item'");
        }

        public static string Get_Colour_id()
        {
            return Database.GetScalarText("Select act_id from Accountype where Name='Colour'");
        }

        public static string Get_Department_id()
        {
            return Database.GetScalarText("Select act_id from Accountype where Name='Department'");
        }

        public static string Get_Group_id()
        {
            return Database.GetScalarText("Select act_id from Accountype where Name='Group'");
        }
    }
}
