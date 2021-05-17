using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Web.Script.Serialization;

namespace faspi
{
    public partial class frm_Cashier : Form
    {
        List<UsersFeature> permission;

        string gstr = "";

        public frm_Cashier()
        {
            InitializeComponent();
        }

        public void Loaddata(string str)
        {
            gstr = str;
            DataTable dt = new DataTable();
            string sql="";


            if (str == "Cashier")
            {
                groupBox3.Visible = false;
                groupBox4.Visible = false;
                textBox1.Visible = false;
                button4.Visible = false;
                label3.Visible = false;
                sql = "SELECT      VOUCHERTYPE.Name,  VOUCHERTYPE.Short + ' ' + CONVERT(nvarchar,  VOUCHERINFO.Vdate, 112)   + ' ' + CAST( VOUCHERINFO.Vnumber AS nvarchar(10)) AS DocNumber,  Journal.Vdate,  VOUCHERINFO.Vi_id,  ACCOUNT.Name AS Debit,                       ACCOUNT_2.Name AS Credit, SUM( Journal.Amount) AS totalamount FROM          VOUCHERTYPE RIGHT OUTER JOIN                       ACCOUNT AS ACCOUNT_2 RIGHT OUTER JOIN   ACCOUNT RIGHT OUTER JOIN                        VOUCHERINFO ON  ACCOUNT.Ac_id =  VOUCHERINFO.Dr_Ac_id ON ACCOUNT_2.Ac_id =  VOUCHERINFO.Cr_Ac_id ON  VOUCHERTYPE.Vt_id =  VOUCHERINFO.Vt_id RIGHT OUTER JOIN Journal LEFT OUTER JOIN   ACCOUNT AS ACCOUNT_1 LEFT OUTER JOIN  ACCOUNTYPE ON ACCOUNT_1.Act_id =  ACCOUNTYPE.Act_id ON  Journal.Ac_id = ACCOUNT_1.Ac_id ON                       VOUCHERINFO.Vi_id =  Journal.Vi_id WHERE     ( ACCOUNTYPE.Name = 'CASH-IN-HAND') AND ( VOUCHERINFO.Branch_id = '" + Database.BranchId + "') AND ( VOUCHERINFO.Cashier_approved = 'false') AND ( VOUCHERTYPE." + Database.BMode + " = 'true') GROUP BY  VOUCHERTYPE.Name,  VOUCHERTYPE.Short + ' ' + CONVERT(nvarchar,  VOUCHERINFO.Vdate, 112)   + ' ' + CAST( VOUCHERINFO.Vnumber AS nvarchar(10)),  Journal.Vdate,  VOUCHERINFO.Vi_id, ACCOUNT_2.Name,  ACCOUNT.Name";
                //}
                //else
                //{
                //    //sql = "SELECT VOUCHERTYPE.Name, VOUCHERTYPE.Short + ' ' + CONVERT(nvarchar, VOUCHERINFO.Vdate, 112) + ' ' + CAST(VOUCHERINFO.Vnumber AS nvarchar(10)) AS DocNumber, Journal.Vdate, SUM(Journal.Amount) AS Totalamount, VOUCHERINFO.Vi_id, ACCOUNT.Name AS party FROM ACCOUNT RIGHT OUTER JOIN Journal ON ACCOUNT.Ac_id = Journal.Opp_acid LEFT OUTER JOIN VOUCHERINFO LEFT OUTER JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id ON Journal.Vi_id = VOUCHERINFO.Vi_id LEFT OUTER JOIN ACCOUNTYPE RIGHT OUTER JOIN ACCOUNT AS ACCOUNT_1 ON ACCOUNTYPE.Act_id = ACCOUNT_1.Act_id ON Journal.Ac_id = ACCOUNT_1.Ac_id WHERE (ACCOUNTYPE.Name = N'CASH-IN-HAND') AND (VOUCHERINFO.Branch_id = '" + Database.BranchId + "') AND (VOUCHERINFO.Cashier_approved = 'false') AND (VOUCHERTYPE.B = 'true') GROUP BY VOUCHERTYPE.Name, VOUCHERTYPE.Short + ' ' + CONVERT(nvarchar, VOUCHERINFO.Vdate, 112) + ' ' + CAST(VOUCHERINFO.Vnumber AS nvarchar(10)), Journal.Vdate, VOUCHERINFO.Vi_id, ACCOUNT.Name ORDER BY VOUCHERTYPE.Short + ' ' + CONVERT(nvarchar, VOUCHERINFO.Vdate, 112) + ' ' + CAST(VOUCHERINFO.Vnumber AS nvarchar(10)) DESC";
                //    //sql = "SELECT VOUCHERTYPE.Name, VOUCHERTYPE.Short + ' ' + CONVERT(nvarchar, VOUCHERINFO.Vdate, 112) + ' ' + CAST(VOUCHERINFO.Vnumber AS nvarchar(10)) AS DocNumber, Journal.Vdate, VOUCHERINFO.Vi_id, ACCOUNT.Name AS party,                       ACCOUNT_1.Name AS [Opp A/c], SUM(Journal.Amount) AS TotalAmount FROM         ACCOUNTYPE RIGHT OUTER JOIN VOUCHERTYPE RIGHT OUTER JOIN                       VOUCHERINFO LEFT OUTER JOIN ACCOUNT ON VOUCHERINFO.Cr_Ac_id = ACCOUNT.Ac_id ON VOUCHERTYPE.Vt_id = VOUCHERINFO.Vt_id RIGHT OUTER JOIN                       ACCOUNT AS ACCOUNT_1 RIGHT OUTER JOIN Journal ON ACCOUNT_1.Ac_id = Journal.Ac_id ON VOUCHERINFO.Vi_id = Journal.Vi_id ON ACCOUNTYPE.Act_id = ACCOUNT_1.Act_id WHERE     (ACCOUNTYPE.Name = N'CASH-IN-HAND') AND (VOUCHERINFO.Branch_id = '" + Database.BranchId + "') AND (VOUCHERINFO.Cashier_approved = 'false') AND    (VOUCHERTYPE.B = 'true') GROUP BY VOUCHERTYPE.Name, VOUCHERTYPE.Short + ' ' + CONVERT(nvarchar, VOUCHERINFO.Vdate, 112)      + ' ' + CAST(VOUCHERINFO.Vnumber AS nvarchar(10)), Journal.Vdate, VOUCHERINFO.Vi_id, ACCOUNT.Name, ACCOUNT_1.Name ORDER BY DocNumber DESC";
                //    sql = "SELECT      VOUCHERTYPE.Name,  VOUCHERTYPE.Short + ' ' + CONVERT(nvarchar,  VOUCHERINFO.Vdate, 112)   + ' ' + CAST( VOUCHERINFO.Vnumber AS nvarchar(10)) AS DocNumber,  Journal.Vdate,  VOUCHERINFO.Vi_id,  ACCOUNT.Name AS Debit,                       ACCOUNT_2.Name AS Credit, SUM( Journal.Amount) AS totalamount FROM          VOUCHERTYPE RIGHT OUTER JOIN  ACCOUNT AS ACCOUNT_2 RIGHT OUTER JOIN   ACCOUNT RIGHT OUTER JOIN                        VOUCHERINFO ON  ACCOUNT.Ac_id =  VOUCHERINFO.Dr_Ac_id ON ACCOUNT_2.Ac_id =  VOUCHERINFO.Cr_Ac_id ON  VOUCHERTYPE.Vt_id =  VOUCHERINFO.Vt_id RIGHT OUTER JOIN Journal LEFT OUTER JOIN   ACCOUNT AS ACCOUNT_1 LEFT OUTER JOIN  ACCOUNTYPE ON ACCOUNT_1.Act_id =  ACCOUNTYPE.Act_id ON  Journal.Ac_id = ACCOUNT_1.Ac_id ON                       VOUCHERINFO.Vi_id =  Journal.Vi_id WHERE     ( ACCOUNTYPE.Name = 'CASH-IN-HAND') AND ( VOUCHERINFO.Branch_id = '" + Database.BranchId + "') AND ( VOUCHERINFO.Cashier_approved = 'false') AND ( VOUCHERTYPE.B = 'true') GROUP BY  VOUCHERTYPE.Name,  VOUCHERTYPE.Short + ' ' + CONVERT(nvarchar,  VOUCHERINFO.Vdate, 112)   + ' ' + CAST( VOUCHERINFO.Vnumber AS nvarchar(10)),  Journal.Vdate,  VOUCHERINFO.Vi_id, ACCOUNT_2.Name,  ACCOUNT.Name";
                //}
                this.Text = "Cashier Approval";
                ansGridView5.Columns["party"].Visible = true;
                ansGridView5.Columns["oppac"].Visible = true;
            }
            else if (str == "Approve")
            {
                //ansGridView5.Columns["party"].Visible = false;
                //ansGridView5.Columns["oppac"].Visible = false;
                ansGridView5.Columns["party"].Visible = true;
                ansGridView5.Columns["oppac"].Visible = true;
                string str1 = "";
                if (textBox1.Text != "")
                {
                 str1 =   " And (VOUCHERTYPE.Name='" + textBox1.Text + "')"; 
                }

                //if (Database.IsKacha == false)
                //{

                //sql = "SELECT      VOUCHERTYPE.Name,  VOUCHERTYPE.Short + ' ' + CONVERT(nvarchar,  VOUCHERINFO.Vdate, 112)   + ' ' + CAST( VOUCHERINFO.Vnumber AS nvarchar(10)) AS DocNumber,  Journal.Vdate,  VOUCHERINFO.Vi_id,  ACCOUNT.Name AS Debit,     ACCOUNT_2.Name AS Credit, SUM( Journal.Amount) AS totalamount FROM          VOUCHERTYPE RIGHT OUTER JOIN                       ACCOUNT AS ACCOUNT_2 RIGHT OUTER JOIN   ACCOUNT RIGHT OUTER JOIN                        VOUCHERINFO ON  ACCOUNT.Ac_id =  VOUCHERINFO.Dr_Ac_id ON ACCOUNT_2.Ac_id =  VOUCHERINFO.Cr_Ac_id ON  VOUCHERTYPE.Vt_id =  VOUCHERINFO.Vt_id RIGHT OUTER JOIN Journal LEFT OUTER JOIN   ACCOUNT AS ACCOUNT_1 LEFT OUTER JOIN  ACCOUNTYPE ON ACCOUNT_1.Act_id =  ACCOUNTYPE.Act_id ON  Journal.Ac_id = ACCOUNT_1.Ac_id ON                       VOUCHERINFO.Vi_id =  Journal.Vi_id WHERE     ( ACCOUNTYPE.Name = 'CASH-IN-HAND') AND ( VOUCHERINFO.Branch_id = '" + Database.BranchId + "') AND ( VOUCHERINFO.Cashier_approved = 'false') AND ( VOUCHERTYPE." + Database.BMode + " = 'true') GROUP BY  VOUCHERTYPE.Name,  VOUCHERTYPE.Short + ' ' + CONVERT(nvarchar,  VOUCHERINFO.Vdate, 112)   + ' ' + CAST( VOUCHERINFO.Vnumber AS nvarchar(10)),  Journal.Vdate,  VOUCHERINFO.Vi_id, ACCOUNT_2.Name,  ACCOUNT.Name";
                
               // sql = "SELECT VOUCHERTYPE.Name, VOUCHERINFO.Vdate, VOUCHERTYPE.Short + ' ' + CONVERT(nvarchar, VOUCHERINFO.Vdate, 112) + ' ' + CAST(VOUCHERINFO.Vnumber AS nvarchar(10)) AS DocNumber, VOUCHERINFO.Vnumber, VOUCHERINFO.Totalamount, VOUCHERINFO.Vi_id, VOUCHERINFO.Branch_id, VOUCHERTYPE.A, VOUCHERINFO.Approved FROM VOUCHERINFO LEFT OUTER JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id WHERE (VOUCHERINFO.Branch_id = '" + Database.BranchId + "') AND (VOUCHERTYPE." + Database.BMode + " = 1) AND (VOUCHERINFO.Approved = 0) And (VOUCHERINFO.Vdate >= '" + dateTimePicker1.Value.Date.ToString(Database.dformat) + "') AND (VOUCHERINFO.Vdate <= '" + dateTimePicker2.Value.Date.ToString(Database.dformat) + "') " + str1 + "     ORDER BY VOUCHERINFO.Vdate DESC, DocNumber, VOUCHERTYPE.Name";

                //sql = "SELECT VOUCHERTYPE.Name, VOUCHERTYPE.Short + ' ' + CONVERT(nvarchar, VOUCHERINFO.Vdate, 112)   + ' ' + CAST(VOUCHERINFO.Vnumber AS nvarchar(10)) AS DocNumber, Journal.Vdate, VOUCHERINFO.Vi_id, ACCOUNT.Name AS Debit,  ACCOUNT_2.Name AS Credit, SUM(Journal.Amount) AS totalamount FROM ACCOUNT AS ACCOUNT_2 RIGHT OUTER JOIN  VOUCHERINFO LEFT OUTER JOIN  Journal LEFT OUTER JOIN  ACCOUNT ON Journal.Ac_id = ACCOUNT.Ac_id ON VOUCHERINFO.Dr_Ac_id = Journal.Ac_id AND VOUCHERINFO.Vi_id = Journal.Vi_id ON   ACCOUNT_2.Ac_id = VOUCHERINFO.Cr_Ac_id LEFT OUTER JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id WHERE (VOUCHERINFO.Branch_id = '" + Database.BranchId + "') AND (VOUCHERTYPE." + Database.BMode + " = 'true') AND (VOUCHERINFO.Approved = 'false') AND  (JOURNAL.Vdate >=  '" + dateTimePicker1.Value.Date.ToString(Database.dformat) + "') AND (JOURNAL.Vdate <=  '" + dateTimePicker2.Value.Date.ToString(Database.dformat) + "') " + str1 + "  GROUP BY VOUCHERTYPE.Name, VOUCHERTYPE.Short + ' ' + CONVERT(nvarchar, VOUCHERINFO.Vdate, 112)   + ' ' + CAST(VOUCHERINFO.Vnumber AS nvarchar(10)), Journal.Vdate, VOUCHERINFO.Vi_id, ACCOUNT_2.Name, ACCOUNT.Name  ORDER BY Journal.Vdate DESC, DocNumber, VOUCHERTYPE.Name";
                sql = "SELECT VOUCHERTYPE.Name, VOUCHERTYPE.Short + ' ' + CONVERT(nvarchar, VOUCHERINFO.Vdate, 112) + ' ' + CAST(VOUCHERINFO.Vnumber AS nvarchar(10))    AS DocNumber, Journal.Vdate, VOUCHERINFO.Vi_id, ACCOUNT.Name AS Debit, ACCOUNT_2.Name AS Credit, Voucherinfo.totalamount AS totalamount FROM VOUCHERTYPE RIGHT OUTER JOIN   ACCOUNT AS ACCOUNT_2 RIGHT OUTER JOIN   ACCOUNT RIGHT OUTER JOIN   VOUCHERINFO ON ACCOUNT.Ac_id = VOUCHERINFO.Dr_Ac_id ON ACCOUNT_2.Ac_id = VOUCHERINFO.Cr_Ac_id ON    VOUCHERTYPE.Vt_id = VOUCHERINFO.Vt_id RIGHT OUTER JOIN   Journal LEFT OUTER JOIN   ACCOUNT AS ACCOUNT_1 LEFT OUTER JOIN   ACCOUNTYPE ON ACCOUNT_1.Act_id = ACCOUNTYPE.Act_id ON Journal.Ac_id = ACCOUNT_1.Ac_id ON VOUCHERINFO.Vi_id = Journal.Vi_id WHERE (ACCOUNTYPE.Name <> '') AND (VOUCHERINFO.Branch_id = '" + Database.BranchId + "') AND (VOUCHERTYPE." + Database.BMode + " = 'true') AND (VOUCHERINFO.Approved = 'false') AND  (JOURNAL.Vdate >=  '" + dateTimePicker1.Value.Date.ToString(Database.dformat) + "') AND (JOURNAL.Vdate <=  '" + dateTimePicker2.Value.Date.ToString(Database.dformat) + "') " + str1 + " GROUP BY VOUCHERTYPE.Name, VOUCHERTYPE.Short + ' ' + CONVERT(nvarchar, VOUCHERINFO.Vdate, 112) + ' ' + CAST(VOUCHERINFO.Vnumber AS nvarchar(10)),    Journal.Vdate, VOUCHERINFO.Vi_id, ACCOUNT_2.Name, ACCOUNT.Name, Voucherinfo.totalamount ORDER BY Journal.Vdate DESC, DocNumber, VOUCHERTYPE.Name";

                //}
                //else
                //{
                //    sql = "SELECT VOUCHERTYPE.Name, VOUCHERINFO.Vdate, VOUCHERTYPE.Short + ' ' + CONVERT(nvarchar, VOUCHERINFO.Vdate, 112) + ' ' + CAST(VOUCHERINFO.Vnumber AS nvarchar(10)) AS DocNumber, VOUCHERINFO.Vnumber, VOUCHERINFO.Totalamount, VOUCHERINFO.Vi_id, VOUCHERINFO.Branch_id, VOUCHERTYPE.A, VOUCHERINFO.Approved FROM VOUCHERINFO LEFT OUTER JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id WHERE (VOUCHERINFO.Branch_id = '" + Database.BranchId + "') AND (VOUCHERTYPE.B = 1) AND (VOUCHERINFO.Approved = 0) ORDER BY VOUCHERINFO.Vdate DESC, DocNumber, VOUCHERTYPE.Name";
                //}
                this.Text = "Approval";
            }

            Database.GetSqlData(sql,dt);
         
            ansGridView5.Rows.Clear();

            double total = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                total = total + double.Parse(dt.Rows[i]["Totalamount"].ToString());
               
            }

            label1.Text = funs.IndianCurr(total);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ansGridView5.Rows.Add();
                ansGridView5.Rows[i].Cells["sno"].Value = (i + 1);
                ansGridView5.Rows[i].Cells["Vid"].Value = dt.Rows[i]["Vi_id"].ToString();
                ansGridView5.Rows[i].Cells["amount"].Value = funs.DecimalPoint(double.Parse(dt.Rows[i]["totalamount"].ToString()),2);
                ansGridView5.Rows[i].Cells["vdate"].Value = DateTime.Parse(dt.Rows[i]["Vdate"].ToString()).ToString(Database.dformat);
                ansGridView5.Rows[i].Cells["DocNumber"].Value = dt.Rows[i]["DocNumber"].ToString();
                //if (str == "Cashier")
                //{
                    ansGridView5.Rows[i].Cells["party"].Value = dt.Rows[i]["debit"].ToString();
                    ansGridView5.Rows[i].Cells["oppac"].Value = dt.Rows[i]["credit"].ToString();
                //}
                //else
                //{
                //    ansGridView5.Columns["party"].Visible =false;
                //    ansGridView5.Columns["oppac"].Visible = false;
                //}
                ansGridView5.Rows[i].Cells["select"].Value = false;
            }
        }

        private string IsDocumentNumber(String str)
        {
            //return Database.GetScalarText("SELECT DISTINCT VOUCHERINFO.Vi_id, " + access_sql.Docnumber + " AS DocNumber FROM (VOUCHERINFO LEFT JOIN ACCOUNT ON VOUCHERINFO.Ac_id = ACCOUNT.Ac_id) LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id WHERE (((VOUCHERINFO.Vt_id)=[VOUCHERTYPE].[Vt_id]) AND (" + access_sql.Docnumber + "='" + str + "'))");
            return Database.GetScalarText("SELECT DISTINCT VOUCHERINFO.Vi_id, " + access_sql.Docnumber + " AS DocNumber FROM (VOUCHERINFO LEFT JOIN ACCOUNT ON VOUCHERINFO.Ac_id = ACCOUNT.Ac_id) LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id WHERE (((VOUCHERINFO.Vt_id)=[VOUCHERTYPE].[Vt_id]) AND (" + access_sql.Docnumber + "='" + str + "')) AND (VOUCHERINFO.Branch_id = '" + Database.BranchId + "')");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (gstr == "Cashier")
            {
                for (int i = 0; i < ansGridView5.Rows.Count; i++)
                {
                    if (bool.Parse(ansGridView5.Rows[i].Cells["select"].Value.ToString()) == true)
                    {
                        Database.CommandExecutor("Update Voucherinfo set Cashier_approved=" + access_sql.Singlequote + "true" + access_sql.Singlequote + ",Cashier_id='" + Database.user_id + "' where Vi_id='" + ansGridView5.Rows[i].Cells["Vid"].Value.ToString() + "' ");
                    }
                }
                MessageBox.Show("Done");
            }

            else if (gstr == "Approve")
            {

                for (int i = 0; i < ansGridView5.Rows.Count; i++)
                {
                    if (bool.Parse(ansGridView5.Rows[i].Cells["select"].Value.ToString()) == true)
                    {
                        Database.CommandExecutor("Update Voucherinfo set Approved=" + access_sql.Singlequote + "true" + access_sql.Singlequote + ", ApprovedBy='" + Database.user_id + "' where Vi_id='" + ansGridView5.Rows[i].Cells["Vid"].Value.ToString() + "' ");
                        Sendsms(ansGridView5.Rows[i].Cells["Vid"].Value.ToString());
                    }
                }

                MessageBox.Show("Done");
               
            }

            Loaddata(gstr);
        }
        private void Sendsms(string vid)
        {
            permission = funs.GetPermissionKey("SMS Setup");

            UsersFeature ob = permission.Where(w => w.FeatureName == "Send SMS").FirstOrDefault();

            if (ob != null && ob.SelectedValue == "No")
            {
                return;
            }
            else if (ob != null && ob.SelectedValue == "Ask")
            {
                if (MessageBox.Show("Are you want to send SMS?", "SMS", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
                {
                    return;
                }
            }

          
            string vt_id = Database.GetScalarText("Select vt_id from Voucherinfo where vi_id='"+vid+"'");
            string gtype = Database.GetScalarText("Select type from Vouchertype where vt_id='" + vt_id + "'");
            DataTable dtcompany = new DataTable();
            Database.GetSqlData("Select name , Address1,address2 from Company", dtcompany);
            if (gtype == "Sale" ||  gtype=="Return" )
            {
                string ac_id = Database.GetScalarText("Select Ac_id from Voucherinfo where Vi_id='" + vid + "'");

                if (funs.Select_AccTypeids(ac_id) != "SER3")
                {
                    if (funs.Select_Mobile(funs.Select_ac_nm(ac_id)) != "0")
                    {
                       
                        DataTable dtcontent = new DataTable();
                        Database.GetSqlData("SELECT VOUCHERTYPE.Name, VOUCHERINFO.Invoiceno, VOUCHERINFO.Vdate, VOUCHERINFO.Totalamount as amount FROM VOUCHERINFO LEFT OUTER JOIN  VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id WHERE ( VOUCHERINFO.Vi_id = '" + vid + "')", dtcontent);



                        if (dtcontent.Rows.Count > 0)
                        {
                            double bal = Database.GetScalarDecimal("SELECT SUM(Balance) AS Balance FROM    (SELECT     Balance+Balance2 as Balance FROM          dbo.ACCOUNT  WHERE      (Ac_id = '" + ac_id + "') UNION ALL  SELECT     SUM(dbo.Journal.Amount) AS SumOfAmount  FROM         dbo.VOUCHERINFO LEFT OUTER JOIN  dbo.Journal ON dbo.VOUCHERINFO.Vi_id = dbo.Journal.Vi_id  WHERE     (dbo.Journal.Ac_id = '" + ac_id + "') AND (dbo.Journal.Vdate <= " + access_sql.Hash + DateTime.Parse(dtcontent.Rows[0]["vdate"].ToString()).ToString(Database.dformat) + access_sql.Hash + ")  AND (dbo.Journal.AB = 'true')) AS res");
                            string balan = "";
                                if (bal == 0)
                                {
                                    balan = "0";
                                }
                                else if (bal > 0)
                                {
                                    balan = bal.ToString() + " Dr.";
                                }
                                else
                                {
                                    balan = (-1 * bal).ToString() + " Cr.";
                                }


                                string msg = "Dear Sir, " + dtcontent.Rows[0]["Name"].ToString() + " No: " + dtcontent.Rows[0]["Invoiceno"].ToString() + " Dated: " + DateTime.Parse(dtcontent.Rows[0]["Vdate"].ToString()).ToString(Database.dformat) + ", Amt: " + funs.IndianCurr(double.Parse(dtcontent.Rows[0]["Amount"].ToString())) + ", Current Bal is: "+balan+", from " + dtcompany.Rows[0]["Name"].ToString() + " " + dtcompany.Rows[0]["Address2"].ToString();
                          
                            msg = msg.Replace("\r", "");

                            if (funs.isDouble(funs.Select_Mobile(funs.Select_ac_nm(ac_id))) == true)
                            {


                                sms objsms = new sms();
                                //MessageBox.Show(msg);
                                objsms.send(msg, funs.Select_Mobile(funs.Select_ac_nm(ac_id)), funs.Select_ac_nm(ac_id));
                            }


                        }
                    }
                    
                }

            }
            else if(gtype=="Receipt" || gtype=="Cnote") 
            {
                DataTable dtcontent = new DataTable();
                Database.GetSqlData("SELECT VOUCHERTYPE.Name, VOUCHERINFO.Invoiceno, VOUCHERINFO.Vdate, VOUCHERACTOTAL.Accid, VOUCHERACTOTAL.Amount FROM VOUCHERINFO LEFT OUTER JOIN   VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id LEFT OUTER JOIN   VOUCHERACTOTAL ON VOUCHERINFO.Vi_id = VOUCHERACTOTAL.Vi_id WHERE VOUCHERINFO.Vi_id = '"+ vid+"'", dtcontent);
                for (int k = 0; k < dtcontent.Rows.Count; k++)
                {
                    string ac_id = dtcontent.Rows[k]["Accid"].ToString();

                    if (funs.Select_AccTypeids(ac_id) != "SER3")
                    {
                        if (funs.Select_Mobile(funs.Select_ac_nm(ac_id)) != "0")
                        {

                          
                                double bal = Database.GetScalarDecimal("SELECT SUM(Balance) AS Balance FROM         (SELECT     Balance+Balance2 as Balance FROM          dbo.ACCOUNT  WHERE      (Ac_id = '" + ac_id + "') UNION ALL  SELECT     SUM(dbo.Journal.Amount) AS SumOfAmount  FROM         dbo.VOUCHERINFO LEFT OUTER JOIN  dbo.Journal ON dbo.VOUCHERINFO.Vi_id = dbo.Journal.Vi_id  WHERE     (dbo.Journal.Ac_id = '" + ac_id + "') AND (dbo.Journal.Vdate <= " + access_sql.Hash + DateTime.Parse(dtcontent.Rows[0]["vdate"].ToString()).ToString(Database.dformat) + access_sql.Hash + ")  AND (dbo.Journal.AB = 'true')) AS res");
                                string balan = "";

                                if (bal == 0)
                                {
                                    balan = "0";
                                }
                                else if (bal > 0)
                                {
                                    balan = bal.ToString() + " Dr.";
                                }
                                else
                                {
                                    balan = (-1 * bal).ToString() + " Cr.";
                                }


                               // string msg = "Dear Sir, " + dtcontent.Rows[0]["Type"].ToString() + ",Invoice No:" + dtcontent.Rows[0]["Invoiceno"].ToString() + ",Invoice Date:" + DateTime.Parse(dtcontent.Rows[0]["Vdate"].ToString()).ToString(Database.dformat) + ",Invoice Amt: " + funs.IndianCurr(double.Parse(dtcontent.Rows[0]["Amount"].ToString())) + ",Now Current Bal is: " + balan + ", from " + dtcompany.Rows[0]["Name"].ToString() + " " + dtcompany.Rows[0]["Address1"].ToString() + " " + dtcompany.Rows[0]["Address2"].ToString();
                                string msg = "Dear Sir, " + dtcontent.Rows[0]["Name"].ToString() + " No: " + dtcontent.Rows[0]["Invoiceno"].ToString() + " Dated: " + DateTime.Parse(dtcontent.Rows[0]["Vdate"].ToString()).ToString(Database.dformat) + ", Amt: " + funs.IndianCurr(double.Parse(dtcontent.Rows[0]["Amount"].ToString())) + ", Current Bal is: " + balan + ", from " + dtcompany.Rows[0]["Name"].ToString() + " " + dtcompany.Rows[0]["Address2"].ToString();
                          
                                msg = msg.Replace("\r", "");

                                if (funs.isDouble(funs.Select_Mobile(funs.Select_ac_nm(ac_id))) == true)
                                {


                                    sms objsms = new sms();
                                   // MessageBox.Show(msg);
                                    objsms.send(msg, funs.Select_Mobile(funs.Select_ac_nm(ac_id)), funs.Select_ac_nm(ac_id));
                                }


                            
                        }

                    }
                }

            }




         

           





        }
        private void frm_Cashier_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
                this.Dispose();
            }
        }

        private void frm_Cashier_Load(object sender, EventArgs e)
        {
            dateTimePicker1.CustomFormat = Database.dformat;
            dateTimePicker2.CustomFormat = Database.dformat;
            dateTimePicker1.MinDate = Database.stDate;
            dateTimePicker1.MaxDate = Database.ldate;
            dateTimePicker2.MinDate = Database.stDate;
            dateTimePicker2.MaxDate = Database.ldate;
            dateTimePicker1.Value = Database.ldate;
            dateTimePicker2.Value = Database.ldate;
            this.Size = this.MdiParent.Size;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Loaddata(gstr);
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            string strCombo = "";
           
            strCombo = "SELECT Name AS VoucherName FROM Vouchertype where "+Database.BMode+"='true' And active=" + access_sql.Singlequote + "True" + access_sql.Singlequote;
           
            char cg = ' ';
            textBox1.Text = SelectCombo.ComboKeypress(this, cg, strCombo, "", 0);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Loaddata(gstr);
        }

        private void ansGridView5_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            String clkStr = "";
            if (ansGridView5.CurrentCell.Value != null)
            {
                clkStr = ansGridView5.CurrentCell.Value.ToString();
            }
            if (IsDocumentNumber(clkStr) != "")
            {
                funs.OpenFrm(this, IsDocumentNumber(clkStr), false);
            }
        }
    }
}
