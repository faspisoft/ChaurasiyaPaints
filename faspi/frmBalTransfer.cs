using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Data.SqlClient;

namespace faspi
{
    public partial class frmBalTransfer : Form
    {
        public String frmBalTrans;
        OleDbConnection AccessCnnSource;
        OleDbConnection AccessCnnDest;
        OleDbDataAdapter da;
        OleDbCommand comm;
        OleDbDataReader dr;
        SqlConnection SqlCnnSource;
        SqlConnection SqlCnnDest;
        SqlDataAdapter Sqlda;
        SqlCommand Sqlcomm;
        SqlDataAdapter Sqldr;
        double totdr = 0;
        double totcr = 0;
        DateTime sourceend;

        public frmBalTransfer()
        {
            InitializeComponent();
        }

        public void LoadData()
        {
            DataTable dtFirmBal1 = new DataTable();
            DataTable dtFirmBal2 = new DataTable();

            Database.GetOtherSqlData("SELECT FIRMINFO.Firm_name,FIRMINFO.Firm_Period_name,FIRMINFO.Firm_name+'['+FIRMINFO.Firm_Period_name+']' as BaseFirm FROM FIRMINFO  where Gststatus=" + access_sql.Singlequote + "True" + access_sql.Singlequote + " ORDER BY FIRMINFO.Firm_name DESC , FIRMINFO.Firm_Period_name DESC", dtFirmBal1);
            ansGridView1.DataSource = dtFirmBal1;
            ansGridView1.Columns["BaseFirm"].Width = 250;
            ansGridView1.Columns["Firm_name"].Visible = false;
            ansGridView1.Columns["Firm_Period_name"].Visible = false;
            Database.GetOtherSqlData("SELECT FIRMINFO.Firm_name,FIRMINFO.Firm_Period_name,FIRMINFO.Firm_name+'['+FIRMINFO.Firm_Period_name+']' as TargetFirm FROM FIRMINFO  where Gststatus=" + access_sql.Singlequote + "True" + access_sql.Singlequote + "  ORDER BY FIRMINFO.Firm_name DESC , FIRMINFO.Firm_Period_name DESC", dtFirmBal2);
            ansGridView2.DataSource = dtFirmBal2;
            ansGridView2.Columns["TargetFirm"].Width = 250;
            ansGridView2.Columns["Firm_name"].Visible = false;
            ansGridView2.Columns["Firm_Period_name"].Visible = false;
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        //private void Button1_Click(object sender, EventArgs e)
        //{
        //    if (ansGridView1.Rows[ansGridView1.SelectedCells[0].RowIndex].Cells["BaseFirm"].Value.ToString() != ansGridView2.Rows[ansGridView2.SelectedCells[0].RowIndex].Cells["TargetFirm"].Value.ToString())
        //    {
        //        DataTable dtTot = new DataTable();
        //        DataTable dtnew = new DataTable();
        //        DataTable dtFirm = new DataTable();
        //        Database.GetOtherSqlData("select Firm_database from firminfo where Firm_name='" + ansGridView1.Rows[ansGridView1.SelectedCells[0].RowIndex].Cells["Firm_name"].Value + "' and Firm_Period_name='" + ansGridView1.Rows[ansGridView1.SelectedCells[0].RowIndex].Cells["Firm_Period_name"].Value + "'", dtFirm);
        //        String SourcedbName = dtFirm.Rows[0][0].ToString();

        //        if (Database.DatabaseType == "access")
        //        {
        //            AccessCnnSource = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Application.StartupPath + "\\Database\\" + SourcedbName + ".mdb;Persist Security Info=true;Jet OLEDB:Database Password=ptsoft9358524971");
        //            AccessCnnSource.Open();
        //        }
        //        else
        //        {
        //            SqlCnnSource = new SqlConnection("Data Source=" + Database.inipath + ";Initial Catalog=" + SourcedbName + ";Persist Security Info=True;User ID=sa;password=" + Database.sqlseverpwd + ";Connection Timeout=100");
        //            SqlCnnSource.Open();
        //        }
        //        dtTot.Clear();
        //        ansGridView3.Rows.Clear();
        //        if (frmBalTrans == "Account")
        //        {

        //            string str = "";

        //            if (Database.IsKacha == false)
        //            {
        //                str = "SELECT res.Name, Sum(res.Dr) AS Dr, Sum(res.Cr) AS Cr  FROM ((SELECT X.Name, Sum(X.Dr) AS Dr, Sum(X.Cr) AS Cr FROM (SELECT Name, sum(Dr) as Dr, sum(Cr) as Cr From QryJournal  WHERE (((A)=" + access_sql.Singlequote + "True" + access_sql.Singlequote + ")) GROUP BY Name UNION ALL SELECT Name, Dr as Dr, Cr as Cr FROM QryAccountinfo)  AS X GROUP BY X.Name)  AS res LEFT JOIN ACCOUNT ON res.Name = ACCOUNT.Name) LEFT JOIN ACCOUNTYPE ON ACCOUNT.Act_id = ACCOUNTYPE.Act_id GROUP BY res.Name, ACCOUNTYPE.Name HAVING  (((ACCOUNTYPE.Name)='CAPITAL ACCOUNT')) OR (((ACCOUNTYPE.Name)='CURRENT LIABILITIES')) OR (((ACCOUNTYPE.Name)='DUTIES & TAXES')) OR  (((ACCOUNTYPE.Name)='SUNDRY CREDITORS'))  OR (((ACCOUNTYPE.Name)='RESERVES  & SURPLUS')) OR (((ACCOUNTYPE.Name)='SUSPENSE ACCOUNT (Temporary A/C)')) OR (((ACCOUNTYPE.Name)='BANK OCC A/C')) OR (((ACCOUNTYPE.Name)='PROVISIONS')) OR (((ACCOUNTYPE.Name)='SECURE LOANS')) OR (((ACCOUNTYPE.Name)='UNSECURE LOANS')) OR (((ACCOUNTYPE.Name)='FIXED ASSETS'))  OR  (((ACCOUNTYPE.Name)='INVESTMENTS')) OR (((ACCOUNTYPE.Name)='SUNDRY DEBTORS'))  OR (((ACCOUNTYPE.Name)='SECURITY & DEPOSITS (Assets)')) OR (((ACCOUNTYPE.Name)='LOAN & ADVANCES (Assests)')) OR (((ACCOUNTYPE.Name)='CASH-IN-HAND')) OR (((ACCOUNTYPE.Name)='BANK ACCOUNTS'))  OR (((ACCOUNTYPE.Name)='CURRENT ASSETS'))";
        //            }
        //            else
        //            {
        //                str = "SELECT res.Name, Sum(res.Dr) AS Dr, Sum(res.Cr) AS Cr  FROM ( (SELECT X.Name, Sum(X.Dr) AS Dr, Sum(X.Cr) AS Cr FROM (SELECT Name, sum(Dr) as Dr, sum(Cr) as Cr From QryJournal  WHERE (((B)=" + access_sql.Singlequote + "True" + access_sql.Singlequote + ")) GROUP BY Name UNION ALL SELECT Name, Dr2 as Dr, Cr2 as Cr FROM QryAccountinfo)  AS X GROUP BY X.Name)  AS res LEFT JOIN ACCOUNT ON res.Name = ACCOUNT.Name) LEFT JOIN ACCOUNTYPE ON ACCOUNT.Act_id = ACCOUNTYPE.Act_id GROUP BY res.Name, ACCOUNTYPE.Name HAVING  (((ACCOUNTYPE.Name)='CAPITAL ACCOUNT')) OR (((ACCOUNTYPE.Name)='CURRENT LIABILITIES')) OR (((ACCOUNTYPE.Name)='DUTIES & TAXES')) OR  (((ACCOUNTYPE.Name)='SUNDRY CREDITORS')) OR (((ACCOUNTYPE.Name)='RESERVES  & SURPLUS')) OR (((ACCOUNTYPE.Name)='SUSPENSE ACCOUNT (Temporary A/C)')) OR (((ACCOUNTYPE.Name)='BANK OCC A/C')) OR (((ACCOUNTYPE.Name)='PROVISIONS')) OR (((ACCOUNTYPE.Name)='SECURE LOANS')) OR (((ACCOUNTYPE.Name)='UNSECURE LOANS')) OR (((ACCOUNTYPE.Name)='FIXED ASSETS')) OR  (((ACCOUNTYPE.Name)='INVESTMENTS')) OR (((ACCOUNTYPE.Name)='SUNDRY DEBTORS'))  OR (((ACCOUNTYPE.Name)='SECURITY & DEPOSITS (Assets)')) OR (((ACCOUNTYPE.Name)='LOAN & ADVANCES (Assests)')) OR (((ACCOUNTYPE.Name)='CASH-IN-HAND')) OR (((ACCOUNTYPE.Name)='BANK ACCOUNTS')) OR (((ACCOUNTYPE.Name)='CURRENT ASSETS')) ";

        //            }

        //            if (Database.DatabaseType == "access")
        //            {
        //                da = new OleDbDataAdapter(str, AccessCnnSource);

        //                da.Fill(dtTot);
        //            }
        //            else
        //            {

        //               Sqlda = new SqlDataAdapter(str, SqlCnnSource);

        //               Sqlda.Fill(dtTot);
        //            }
                  
        //            ansGridView3.Columns.Add("Name", "Name");
        //            ansGridView3.Columns.Add("Dr", "Dr");
        //            ansGridView3.Columns.Add("Cr", "Cr");
                    
            
        //            ansGridView3.Columns["Name"].Width = 150;
        //            ansGridView3.Columns["Dr"].Width = 100;
        //            ansGridView3.Columns["Cr"].Width = 100;
                  
        //            int j = 0;
              
        //            for (int i = 0; i < dtTot.Rows.Count; i++)
        //            {

        //                if (Database.IsKacha == false)
        //                {


        //                    if ((double.Parse(dtTot.Rows[i]["Dr"].ToString()) - double.Parse(dtTot.Rows[i]["Cr"].ToString())) == 0 && (double.Parse(dtTot.Rows[i]["Cr"].ToString()) - double.Parse(dtTot.Rows[i]["Dr"].ToString())) == 0)
        //                    {
        //                        continue;
        //                    }

        //                    ansGridView3.Rows.Add();

        //                    ansGridView3.Rows[j].Cells["Name"].Value = dtTot.Rows[i]["Name"];
        //                    if ((double.Parse(dtTot.Rows[i]["Dr"].ToString()) - double.Parse(dtTot.Rows[i]["Cr"].ToString())) > 0)
        //                    {
        //                        ansGridView3.Rows[j].Cells["Dr"].Value = funs.DecimalPoint(double.Parse(dtTot.Rows[i]["Dr"].ToString()) - double.Parse(dtTot.Rows[i]["Cr"].ToString()));
        //                    }
        //                    else
        //                    {
        //                        ansGridView3.Rows[j].Cells["Dr"].Value = "0.00";
        //                    }




        //                    if ((double.Parse(dtTot.Rows[i]["Cr"].ToString()) - double.Parse(dtTot.Rows[i]["Dr"].ToString())) > 0)
        //                    {
        //                        ansGridView3.Rows[j].Cells["Cr"].Value = funs.DecimalPoint(double.Parse(dtTot.Rows[i]["Cr"].ToString()) - double.Parse(dtTot.Rows[i]["Dr"].ToString()));
        //                    }
        //                    else
        //                    {
        //                        ansGridView3.Rows[j].Cells["Cr"].Value = "0.00";
        //                    }
        //                }
        //                else
        //                {
        //                    if ((double.Parse(dtTot.Rows[i]["Dr"].ToString()) - double.Parse(dtTot.Rows[i]["Cr"].ToString())) == 0 && (double.Parse(dtTot.Rows[i]["Cr"].ToString()) - double.Parse(dtTot.Rows[i]["Dr"].ToString())) == 0)
        //                    {
        //                        continue;
        //                    }

        //                    ansGridView3.Rows.Add();

        //                    ansGridView3.Rows[j].Cells["Name"].Value = dtTot.Rows[i]["Name"];

        //                    if ((double.Parse(dtTot.Rows[i]["Dr"].ToString()) - double.Parse(dtTot.Rows[i]["Cr"].ToString())) > 0)
        //                    {
        //                        ansGridView3.Rows[j].Cells["Dr"].Value = funs.DecimalPoint(double.Parse(dtTot.Rows[i]["Dr"].ToString()) - double.Parse(dtTot.Rows[i]["Cr"].ToString()));
        //                    }
        //                    else
        //                    {
        //                        ansGridView3.Rows[j].Cells["Dr"].Value = "0.00";
        //                    }




        //                    if ((double.Parse(dtTot.Rows[i]["Cr"].ToString()) - double.Parse(dtTot.Rows[i]["Dr"].ToString())) > 0)
        //                    {
        //                        ansGridView3.Rows[j].Cells["Cr"].Value = funs.DecimalPoint(double.Parse(dtTot.Rows[i]["Cr"].ToString()) - double.Parse(dtTot.Rows[i]["Dr"].ToString()));
        //                    }
        //                    else
        //                    {
        //                        ansGridView3.Rows[j].Cells["Cr"].Value = "0.00";
        //                    }

        //                }
        //                j++;
        //            }
        //            dtTot.Clear();
        //            string ssql = "";
        //            if (Database.IsKacha == false)
        //            {

        //                ssql = "SELECT final.Name, Sum(final.Total) AS Balance, final.Closing_Bal FROM (SELECT re.Name, (re.Dr-re.Cr) AS Total ,'' as Closing_Bal FROM (SELECT res.Name, Sum(res.Dr) AS Dr, Sum(res.Cr) AS Cr  FROM ((SELECT X.Name, Sum(X.Dr) AS Dr, Sum(X.Cr) AS Cr FROM (SELECT Name, sum(Dr) as Dr, sum(Cr) as Cr From QryJournal  WHERE (((A)=" + access_sql.Singlequote + "True" + access_sql.Singlequote + ")) GROUP BY Name UNION ALL SELECT Name, Dr as Dr, Cr as Cr FROM QryAccountinfo)  AS X GROUP BY X.Name)  AS res LEFT JOIN ACCOUNT ON res.Name = ACCOUNT.Name) LEFT JOIN ACCOUNTYPE ON ACCOUNT.Act_id = ACCOUNTYPE.Act_id GROUP BY res.Name, ACCOUNTYPE.Name HAVING  (((ACCOUNTYPE.Name)='STOCK-IN-HAND')) )  AS re  Union all   SELECT ACCOUNT.Name, 0 AS Total, ACCOUNT.Closing_Bal AS Closing_Bal FROM ACCOUNT LEFT JOIN ACCOUNTYPE ON ACCOUNT.Act_id = ACCOUNTYPE.Act_id WHERE (((ACCOUNTYPE.Name)='STOCK-IN-HAND')) )  AS final GROUP BY final.Name, final.Closing_Bal";
        //               // da = new OleDbDataAdapter("SELECT final.Name, Sum(final.Total) AS Balance, final.Closing_Bal FROM (SELECT re.Name, (re.Dr-re.Cr) AS Total ,'' as Closing_Bal FROM (SELECT res.Name, Sum(res.Dr) AS Dr, Sum(res.Cr) AS Cr  FROM ((SELECT X.Name, Sum(X.Dr) AS Dr, Sum(X.Cr) AS Cr FROM (SELECT Name, sum(Dr) as Dr, sum(Cr) as Cr From QryJournal  WHERE (((QryJournal.A)=True)) GROUP BY QryJournal.ACCOUNT.Name UNION ALL SELECT QryAccountinfo.Name, QryAccountinfo.Dr as Dr, QryAccountinfo.Cr as Cr FROM QryAccountinfo)  AS X GROUP BY X.Name)  AS res LEFT JOIN ACCOUNT ON res.Name = ACCOUNT.Name) LEFT JOIN ACCOUNTYPE ON ACCOUNT.Act_id = ACCOUNTYPE.Act_id GROUP BY res.Name, ACCOUNTYPE.Name HAVING  (((ACCOUNTYPE.Name)='Stock')) )  AS re  Union all   SELECT ACCOUNT.Name,0 as Total, ACCOUNT.Closing_Bal AS Closing_Bal FROM ACCOUNT WHERE (((ACCOUNT.Act_id)=28)) )  AS final GROUP BY final.Name, final.Closing_Bal", AccessCnnSource);

        //            }
        //            else
        //            {
        //                ssql = "SELECT final.Name, Sum(final.Total) AS Balance, final.Closing_Bal2 FROM (SELECT re.Name, (re.Dr-re.Cr) AS Total ,'' as Closing_Bal2 FROM (SELECT res.Name, Sum(res.Dr) AS Dr, Sum(res.Cr) AS Cr  FROM ((SELECT X.Name, Sum(X.Dr) AS Dr, Sum(X.Cr) AS Cr FROM (SELECT Name, sum(Dr) as Dr, sum(Cr) as Cr From QryJournal  WHERE (((B)=" + access_sql.Singlequote + "True" + access_sql.Singlequote + ")) GROUP BY Name UNION ALL SELECT Name, Dr2 as Dr, Cr2 as Cr FROM QryAccountinfo)  AS X GROUP BY X.Name)  AS res LEFT JOIN ACCOUNT ON res.Name = ACCOUNT.Name) LEFT JOIN ACCOUNTYPE ON ACCOUNT.Act_id = ACCOUNTYPE.Act_id GROUP BY res.Name, ACCOUNTYPE.Name HAVING  (((ACCOUNTYPE.Name)='STOCK-IN-HAND')) )  AS re  Union all   SELECT ACCOUNT.Name, 0 AS Total, ACCOUNT.Closing_Bal2 AS Closing_Bal FROM ACCOUNT LEFT JOIN ACCOUNTYPE ON ACCOUNT.Act_id = ACCOUNTYPE.Act_id WHERE (((ACCOUNTYPE.Name)='STOCK-IN-HAND')) )  AS final GROUP BY final.Name, final.Closing_Bal2";
        //                //da = new OleDbDataAdapter("SELECT final.Name, Sum(final.Total) AS Balance, final.Closing_Bal2 FROM (SELECT re.Name, (re.Dr-re.Cr) AS Total ,'' as Closing_Bal2 FROM (SELECT res.Name, Sum(res.Dr) AS Dr, Sum(res.Cr) AS Cr  FROM ((SELECT X.Name, Sum(X.Dr) AS Dr, Sum(X.Cr) AS Cr FROM (SELECT Name, sum(Dr) as Dr, sum(Cr) as Cr From QryJournal  WHERE (((QryJournal.B)=True)) GROUP BY QryJournal.ACCOUNT.Name UNION ALL SELECT QryAccountinfo.Name, QryAccountinfo.Dr2 as Dr, QryAccountinfo.Cr2 as Cr FROM QryAccountinfo)  AS X GROUP BY X.Name)  AS res LEFT JOIN ACCOUNT ON res.Name = ACCOUNT.Name) LEFT JOIN ACCOUNTYPE ON ACCOUNT.Act_id = ACCOUNTYPE.Act_id GROUP BY res.Name, ACCOUNTYPE.Name HAVING  (((ACCOUNTYPE.Name)='Stock')) )  AS re  Union all   SELECT ACCOUNT.Name,0 as Total, ACCOUNT.Closing_Bal2 AS Closing_Bal2 FROM ACCOUNT WHERE (((ACCOUNT.Act_id)=28)) )  AS final GROUP BY final.Name, final.Closing_Bal2", AccessCnnSource);
        //            }

        //            if (Database.DatabaseType == "access")
        //            {
        //                da = new OleDbDataAdapter(ssql, AccessCnnSource);

        //                da.Fill(dtTot);
        //            }
        //            else
        //            {
        //                Sqlda = new SqlDataAdapter(ssql, SqlCnnSource);
        //                Sqlda.Fill(dtTot);
        //            }
        //            for (int i = 0; i < dtTot.Rows.Count; i++)
        //            {
        //                ansGridView3.Rows.Add();
                       
        //                ansGridView3.Rows[j].Cells["Name"].Value = dtTot.Rows[i]["Name"];
        //                ansGridView3.Rows[j].Cells["Cr"].Value = 0;
        //                if (Database.IsKacha == false)
        //                {
        //                    if (dtTot.Rows[i]["Closing_Bal"].ToString() == "")
        //                    {
        //                        dtTot.Rows[i]["Closing_Bal"] = double.Parse(dtTot.Rows[i]["Balance"].ToString());
        //                    }
                           
        //                    else
        //                    {
        //                        dtTot.Rows[i]["Closing_Bal"] = double.Parse(dtTot.Rows[i]["Closing_Bal"].ToString());
        //                    }


        //                    ansGridView3.Rows[j].Cells["Dr"].Value = funs.DecimalPoint(double.Parse(dtTot.Rows[i]["Closing_Bal"].ToString()));
        //                }
        //                else
        //                {

        //                    if (dtTot.Rows[i]["Closing_Bal2"].ToString() == "")
        //                    {
        //                        dtTot.Rows[i]["Closing_Bal2"] = double.Parse(dtTot.Rows[i]["Balance"].ToString());
        //                    }

        //                    else
        //                    {
        //                        dtTot.Rows[i]["Closing_Bal2"] = double.Parse(dtTot.Rows[i]["Closing_Bal2"].ToString());
        //                    }

        //                    ansGridView3.Rows[j].Cells["Dr"].Value = funs.DecimalPoint(double.Parse(dtTot.Rows[i]["Closing_Bal2"].ToString()));
        //                }
        //                j++;
        //            }
        //            tabControl1.SelectedIndex = 1;
        //        }


               
        //        else if (frmBalTrans == "Stock")
        //        {
        //            string str = "";
        //            if (Database.IsKacha == false)
        //            {

        //                str = "SELECT DESCRIPTION.Description AS Descr, DESCRIPTION.Pack AS Packing, Sum(Stock.Receive)-Sum(Stock.Issue) AS Stock, Stock.godown_id FROM Stock LEFT JOIN DESCRIPTION ON Stock.Did = DESCRIPTION.Des_id WHERE (((Stock.marked)=" + access_sql.Singlequote + "False" + access_sql.Singlequote + ") AND ((Stock.Did)<>0)) GROUP BY DESCRIPTION.Description, DESCRIPTION.Pack, Stock.godown_id HAVING (((Sum([Stock].[Receive])-Sum([Stock].[Issue]))<>0))";
                      
        //            }
        //            else
        //            {
        //                str = "SELECT DESCRIPTION.Description AS Descr, DESCRIPTION.Pack AS Packing, Sum(Stock.Receive)-Sum(Stock.Issue) AS Stock, Stock.godown_id FROM Stock LEFT JOIN DESCRIPTION ON Stock.Did = DESCRIPTION.Des_id WHERE (((Stock.marked)=" + access_sql.Singlequote + "True" + access_sql.Singlequote + ") AND ((Stock.Did)<>0)) GROUP BY DESCRIPTION.Description, DESCRIPTION.Pack, Stock.godown_id HAVING      (SUM(Stock.Receive) - SUM(Stock.Issue) <> 0)";
        //            }
        //            if (Database.DatabaseType == "access")
        //            {
        //                da = new OleDbDataAdapter(str, AccessCnnSource);

        //                da.Fill(dtTot);
        //            }
        //            else
        //            {
        //                Sqlda = new SqlDataAdapter(str, SqlCnnSource);
        //                Sqlda.Fill(dtTot);
        //            }
        //            //comm = new OleDbCommand(str, AccessCnnSource);
        //            //dr = comm.ExecuteReader();

        //            ansGridView3.Columns.Add("desc", "Description");
        //            ansGridView3.Columns.Add("nm", "Name");
        //            ansGridView3.Columns.Add("Open_stock", "Open_stock");
        //            ansGridView3.Columns.Add("godown_id", "Godown");

        //            ansGridView3.Columns["godown_id"].Visible = false;
        //            ansGridView3.Columns["nm"].Width = 50;
        //            ansGridView3.Columns["desc"].Width = 250;
               

        //            for (int k = 0; k < dtTot.Rows.Count; k++)
        //            {

        //                ansGridView3.Rows.Add();

        //                ansGridView3.Rows[k].Cells["desc"].Value = dtTot.Rows[k]["descr"].ToString();
        //                ansGridView3.Rows[k].Cells["nm"].Value = dtTot.Rows[k]["Packing"].ToString();
        //                ansGridView3.Rows[k].Cells["Open_stock"].Value = dtTot.Rows[k]["Stock"].ToString();
        //                ansGridView3.Rows[k].Cells["godown_id"].Value = dtTot.Rows[k]["godown_id"].ToString();
        //                label3.Text = k.ToString();
        //            }
                    
        //            tabControl1.SelectedIndex = 1;
        //        }
            
        //    }


        //}

        //private void button4_Click(object sender, EventArgs e)
        //{
        //    DataTable dtAccmatch = new DataTable();
        //    DataTable dtFirmNew = new DataTable();
        //    Database.GetOtherSqlData("select Firm_database from firminfo where Firm_name='" + ansGridView2.Rows[ansGridView2.SelectedCells[0].RowIndex].Cells["Firm_name"].Value + "' and Firm_Period_name='" + ansGridView2.Rows[ansGridView2.SelectedCells[0].RowIndex].Cells["Firm_Period_name"].Value + "'", dtFirmNew);
        //    String DestdbName = dtFirmNew.Rows[0][0].ToString();
        //    if (Database.DatabaseType == "access")
        //    {
        //        AccessCnnDest = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Application.StartupPath + "\\Database\\" + DestdbName + ".mdb;Persist Security Info=true;Jet OLEDB:Database Password=ptsoft9358524971");
        //        AccessCnnDest.Open();
        //    }
        //    else
        //    {
        //        SqlCnnDest = new SqlConnection("Data Source=" + Database.inipath + ";Initial Catalog=" + DestdbName + ";Persist Security Info=True;User ID=sa;password=" + Database.sqlseverpwd + ";Connection Timeout=100");
        //        SqlCnnDest.Open();
        //    }
        //    ansGridView4.Columns.Clear();

         

        //    if (frmBalTrans == "Account")
        //    {
        //        ansGridView4.Columns.Add("nm", "Name");
        //        ansGridView4.Columns.Add("dr", "Dr");
        //        ansGridView4.Columns.Add("cr", "Cr");
              
        //        ansGridView4.Columns["nm"].Width = 175;
        //        ansGridView4.Columns["dr"].Width = 150;
        //        ansGridView4.Columns["cr"].Width = 150;
        //    }
        //    else
        //    {
        //        ansGridView4.Columns.Add("desc", "Description");
        //        ansGridView4.Columns.Add("Open_stock", "Open_stock");
             
        //        ansGridView4.Columns["desc"].Width = 250;
        //    }

        //    int j = 0;
        //    for (int i = 0;i <= ansGridView3.RowCount -1 ;i++)
        //    {
        //        if (frmBalTrans == "Account")
        //        {
        //            if (Database.DatabaseType == "access")
        //            {
        //                da = new OleDbDataAdapter("select name from account where name='" + ansGridView3.Rows[i].Cells["Name"].Value + "'", AccessCnnDest);
        //                dtAccmatch.Clear();
        //                da.Fill(dtAccmatch);
        //            }
        //            else
        //            {
        //                Sqlda = new SqlDataAdapter("select name from account where name='" + ansGridView3.Rows[i].Cells["Name"].Value + "'", SqlCnnDest);
        //                dtAccmatch.Clear();
        //                Sqlda.Fill(dtAccmatch);
        //            }

        //                if (dtAccmatch.Rows.Count == 0)
        //                {
        //                    ansGridView4.Rows.Add();
        //                    ansGridView4.Rows[j].Cells["nm"].Value = ansGridView3.Rows[i].Cells["Name"].Value;
        //                    ansGridView4.Rows[j].Cells["dr"].Value = ansGridView3.Rows[i].Cells["Dr"].Value;
        //                    ansGridView4.Rows[j].Cells["cr"].Value = ansGridView3.Rows[i].Cells["Cr"].Value;
        //                    j++;
        //                }
                   
        //        }
        //        else if (frmBalTrans == "Stock")
        //        {


        //            if (Database.DatabaseType == "access")
        //            {
        //                da = new OleDbDataAdapter("SELECT DESCRIPTION.Description, DESCRIPTION.Pack FROM DESCRIPTION WHERE (((DESCRIPTION.Description)='" + ansGridView3.Rows[i].Cells["desc"].Value + "') AND ((DESCRIPTION.Pack)='" + ansGridView3.Rows[i].Cells["nm"].Value + "'))", AccessCnnDest);
        //                dtAccmatch.Clear();
        //                da.Fill(dtAccmatch);
        //            }
        //            else
        //            {
        //                Sqlda = new SqlDataAdapter("SELECT DESCRIPTION.Description, DESCRIPTION.Pack FROM DESCRIPTION WHERE (((DESCRIPTION.Description)='" + ansGridView3.Rows[i].Cells["desc"].Value + "') AND ((DESCRIPTION.Pack)='" + ansGridView3.Rows[i].Cells["nm"].Value + "'))", SqlCnnDest);
        //                dtAccmatch.Clear();
        //                Sqlda.Fill(dtAccmatch);
        //            }


              

        //            if (dtAccmatch.Rows.Count == 0)
        //            {
        //                ansGridView4.Rows.Add();
        //                ansGridView4.Rows[j].Cells["desc"].Value = ansGridView3.Rows[i].Cells["desc"].Value + "(" + ansGridView3.Rows[i].Cells["nm"].Value + ")";
        //                //if (double.Parse(ansGridView3.Rows[i].Cells["Open_stock"].Value.ToString()) == 30)
        //                //{

        //                //}

        //                ansGridView4.Rows[j].Cells["Open_stock"].Value = ansGridView3.Rows[i].Cells["Open_stock"].Value;
        //                label4.Text = j.ToString();
        //                j++;
                        
        //            }   
        //        }
   
        //    }
  
        //    tabControl1.SelectedIndex = 2;
        //}

        //private void button6_Click(object sender, EventArgs e)
        //{
        //    if (frmBalTrans == "Account")
        //    {
        //        if (Database.DatabaseType == "access")
        //        {
        //            if (Database.IsKacha == false)
        //            {
        //                comm = new OleDbCommand("update account set Balance=0", AccessCnnDest);
        //                comm.ExecuteNonQuery();
        //            }
        //            else
        //            {
        //                comm = new OleDbCommand("update account set Balance2=0", AccessCnnDest);
        //                comm.ExecuteNonQuery();
        //            }
        //        }
        //        else
        //        {
        //            if (Database.IsKacha == false)
        //            {
        //                Sqlcomm = new SqlCommand("update account set Balance=0", SqlCnnDest);
        //               Sqlcomm.ExecuteNonQuery();
        //            }
        //            else
        //            {
        //                Sqlcomm = new SqlCommand("update account set Balance2=0", SqlCnnDest);
        //                Sqlcomm.ExecuteNonQuery();
        //            }
        //        }

        //        for (int i = 0; i <= ansGridView3.RowCount - 1; i++)
        //        {
        //            double bal = 0;
        //            bal = double.Parse(ansGridView3.Rows[i].Cells["Dr"].Value.ToString()) - double.Parse(ansGridView3.Rows[i].Cells["Cr"].Value.ToString());
        //            if (Database.DatabaseType == "access")
        //            {


        //                if (Database.IsKacha == false)
        //                {
                           
        //                    comm = new OleDbCommand("update account set Balance=" + bal + " where [name]='" + ansGridView3.Rows[i].Cells["Name"].Value + "'", AccessCnnDest);
        //                    comm.ExecuteNonQuery();
        //                }
        //                else
        //                {
        //                    comm = new OleDbCommand("update account set Balance2=" + bal + " where [name]='" + ansGridView3.Rows[i].Cells["Name"].Value + "'", AccessCnnDest);
        //                    comm.ExecuteNonQuery();
        //                }
        //            }
        //            else 
        //            {
        //                if (Database.IsKacha == false)
        //                {

        //                    Sqlcomm = new SqlCommand("update account set Balance=" + bal + " where [name]='" + ansGridView3.Rows[i].Cells["Name"].Value + "'", SqlCnnDest);
        //                    Sqlcomm.ExecuteNonQuery();
        //                }
        //                else
        //                {
        //                    Sqlcomm = new SqlCommand("update account set Balance2=" + bal + " where [name]='" + ansGridView3.Rows[i].Cells["Name"].Value + "'", SqlCnnDest);
        //                    Sqlcomm.ExecuteNonQuery();
        //                }
        //            }
                 
                    
        //        }
        //    }
        //    else if (frmBalTrans == "Stock")
        //    {

        //        try
        //        {
        //            Database.BeginTran();
        //            string str = "";
        //            DataTable dtstock = new DataTable("Stock");

        //            if (Database.IsKacha == false)
        //            {
        //                str = "Delete from Stock where Vid=0 and marked=" + access_sql.Singlequote + "false" + access_sql.Singlequote ;
        //                if (Database.DatabaseType == "access")
        //                {


        //                    comm = new OleDbCommand(str, AccessCnnDest);
        //                    comm.ExecuteNonQuery();
        //                    da = new OleDbDataAdapter("select * from Stock where vid='' and marked=" + access_sql.Singlequote + "false" + access_sql.Singlequote, AccessCnnDest);
        //                    dtstock.Clear();
        //                    da.Fill(dtstock);
        //                }
        //                else
        //                {
        //                    Sqlcomm = new SqlCommand(str, SqlCnnDest);
        //                    Sqlcomm.ExecuteNonQuery();
        //                    Sqlda = new SqlDataAdapter("select * from Stock where vid='' and marked=" + access_sql.Singlequote + "false" + access_sql.Singlequote, SqlCnnDest);
        //                    dtstock.Clear();
        //                    Sqlda.Fill(dtstock);
        //                }
                       
        //            }
        //            else
        //            {
        //                str = "Delete from Stock where Vid='' and marked="+ access_sql.Singlequote+"true"+ access_sql.Singlequote;
        //                if (Database.DatabaseType == "access")
        //                {


        //                    comm = new OleDbCommand(str, AccessCnnDest);
        //                    comm.ExecuteNonQuery();
        //                    da = new OleDbDataAdapter("select * from Stock where vid='' and marked=" + access_sql.Singlequote + "true" + access_sql.Singlequote, AccessCnnDest);
        //                    dtstock.Clear();
        //                    da.Fill(dtstock);
        //                }
        //                else
        //                {
        //                    Sqlcomm = new SqlCommand(str, SqlCnnDest);
        //                    Sqlcomm.ExecuteNonQuery();
        //                    Sqlda = new SqlDataAdapter("select * from Stock where vid='' and marked=" + access_sql.Singlequote + "true" + access_sql.Singlequote, SqlCnnDest);
        //                    dtstock.Clear();
        //                    Sqlda.Fill(dtstock);
        //                }
                       
        //            }
        //            for (int i = 0; i <= ansGridView3.RowCount - 1; i++)
        //            {
        //                int desid = 0;
        //                str = "Select Des_id from Description where Description ='" + ansGridView3.Rows[i].Cells["desc"].Value.ToString() + "' and Pack='" + ansGridView3.Rows[i].Cells["nm"].Value.ToString() + "'";
        //                if (Database.DatabaseType == "access")
        //                {
        //                    comm = new OleDbCommand(str, AccessCnnDest);


        //                    if (comm.ExecuteScalar() != null && comm.ExecuteScalar().ToString() != "")
        //                    {


        //                        desid = int.Parse(comm.ExecuteScalar().ToString());

        //                    }

        //                }
        //                else
        //                {
        //                   Sqlcomm = new SqlCommand(str, SqlCnnDest);



        //                   if (Sqlcomm.ExecuteScalar() != null && Sqlcomm.ExecuteScalar().ToString() != "")
        //                   {

        //                       desid = int.Parse(Sqlcomm.ExecuteScalar().ToString());
        //                   }
        //                }


        //                if (desid!=0)
        //                {

        //                    dtstock.Rows.Add();
        //                    dtstock.Rows[dtstock.Rows.Count - 1]["Vid"] = 0;
        //                    dtstock.Rows[dtstock.Rows.Count - 1]["Did"] = desid;
        //                    dtstock.Rows[dtstock.Rows.Count - 1]["Itemsr"] = dtstock.Rows.Count;

        //                    dtstock.Rows[dtstock.Rows.Count - 1]["Receive"] = double.Parse(ansGridView3.Rows[i].Cells["Open_stock"].Value.ToString());
        //                    dtstock.Rows[dtstock.Rows.Count - 1]["Issue"] = 0;
        //                    dtstock.Rows[dtstock.Rows.Count - 1]["ReceiveAmt"] = 0;
        //                    dtstock.Rows[dtstock.Rows.Count - 1]["IssueAmt"] = 0;


        //                    dtstock.Rows[dtstock.Rows.Count - 1]["godown_id"] = ansGridView3.Rows[i].Cells["godown_id"].Value.ToString();
        //                    if (Database.IsKacha == false)
        //                    {

        //                        dtstock.Rows[dtstock.Rows.Count - 1]["marked"] = false;
        //                    }
        //                    else
        //                    {
        //                        dtstock.Rows[dtstock.Rows.Count - 1]["marked"] = true;
        //                    }
        //                }
        //            }
        //            //Database.CommitTran();
        //            if (Database.DatabaseType == "access")
        //            {
                     

        //                da = new OleDbDataAdapter("select * from stock", AccessCnnDest);
        //                OleDbCommandBuilder cb = new OleDbCommandBuilder();
        //                cb.QuotePrefix = "[";
        //                cb.QuoteSuffix = "]";
        //                cb.DataAdapter = da;
        //                da.Update(dtstock);
        //            }
        //            else
        //            {
                      

                       
        //                Sqlda = new SqlDataAdapter("select * from stock", SqlCnnDest );
        //                SqlCommandBuilder cb = new SqlCommandBuilder();
        //                cb.QuotePrefix = "[";
        //                cb.QuoteSuffix = "]";
        //                cb.DataAdapter = Sqlda;
        //                Sqlda.Update(dtstock);
        //            }
        //            Database.CommitTran();
        //            MessageBox.Show("Transfered successfully.");
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show("Error Occured..");
        //            Database.RollbackTran();
        //        }
        //    }
           
        //    this.Close();
        //    this.Dispose();
         
        //}



        private void Button1_Click(object sender, EventArgs e)
        {
            if (ansGridView1.Rows[ansGridView1.SelectedCells[0].RowIndex].Cells["BaseFirm"].Value.ToString() != ansGridView2.Rows[ansGridView2.SelectedCells[0].RowIndex].Cells["TargetFirm"].Value.ToString())
            {
                DataTable dtTot = new DataTable();
                DataTable dtnew = new DataTable();
                DataTable dtFirm = new DataTable();
                Database.GetOtherSqlData("select Firm_database,Firm_odate,Firm_edate from firminfo where Firm_name='" + ansGridView1.Rows[ansGridView1.SelectedCells[0].RowIndex].Cells["Firm_name"].Value + "' and Firm_Period_name='" + ansGridView1.Rows[ansGridView1.SelectedCells[0].RowIndex].Cells["Firm_Period_name"].Value + "'", dtFirm);
                String SourcedbName = dtFirm.Rows[0][0].ToString();
                sourceend = DateTime.Parse(dtFirm.Rows[0][2].ToString());

                if (Database.DatabaseType == "access")
                {
                    AccessCnnSource = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Application.StartupPath + "\\Database\\" + SourcedbName + ".mdb;Persist Security Info=true;Jet OLEDB:Database Password=ptsoft9358524971");
                    AccessCnnSource.Open();
                }
                else
                {
                    SqlCnnSource = new SqlConnection("Data Source=" + Database.inipath + ";Initial Catalog=" + SourcedbName + ";Persist Security Info=True;User ID=sa;password=" + Database.sqlseverpwd + ";Connection Timeout=100");
                    SqlCnnSource.Open();
                }
                dtTot.Clear();

                ansGridView3.Rows.Clear();
                if (frmBalTrans == "Account")
                {
                    string str = "";
                    string sql = "SELECT Nid, Name FROM ACCOUNTYPE WHERE (Nature = 'L') OR   (Nature = 'A') AND (Name <> 'STOCK-IN-HAND') order by nid";
                    //string sql = "SELECT Act_id, Name FROM ACCOUNTYPE WHERE (Nature = 'L') OR   (Nature = 'A') AND (Name <> 'STOCK-IN-HAND')";
                    DataTable dt = new DataTable();
                    Database.GetSqlData(sql, dt);
                    if (Database.DatabaseType == "sql")
                    {
                        if (Database.BMode == "A")
                        {
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                // str = "SELECT  CASE WHEN SUM(res.Dr) > SUM(res.Cr) THEN SUM(res.Dr) - SUM(res.Cr) ELSE 0 END AS Dr, CASE WHEN SUM(res.Cr) > SUM(res.Dr) THEN SUM(res.Cr) - SUM(res.Dr) ELSE 0 END AS Cr, ACCOUNT.Name FROM          ACCOUNT LEFT OUTER JOIN                       ACCOUNTYPE ON  ACCOUNT.Act_id =  ACCOUNTYPE.Act_id RIGHT OUTER JOIN (SELECT     Ac_id,  " + access_sql.fnstring("Balance>0", "Balance", "0") + " AS Dr, " + access_sql.fnstring("Balance<0", "-1*(Balance)", "0") + " AS Cr FROM ACCOUNT AS ACCOUNT_1 UNION ALL SELECT     Ac_id,  " + access_sql.fnstring("JOURNAL.Amount>0", "JOURNAL.Amount", "0") + " AS Dr, " + access_sql.fnstring("JOURNAL.Amount<0", "-1*(JOURNAL.Amount)", "0") + " AS Cr          FROM          JOURNAL) AS res ON  ACCOUNT.Ac_id = res.Ac_id WHERE     ( ACCOUNTYPE.Path LIKE '" + dt.Rows[i]["act_id"].ToString() + ";%') GROUP BY  ACCOUNT.Name";

                                str = "SELECT CASE WHEN SUM(res.Dr) > SUM(res.Cr) THEN SUM(res.Dr) - SUM(res.Cr) ELSE 0 END AS Dr, CASE WHEN SUM(res.Cr) > SUM(res.Dr) THEN SUM(res.Cr) - SUM(res.Dr)  ELSE 0 END AS Cr, ACCOUNT.Name FROM ACCOUNT LEFT OUTER JOIN  ACCOUNTYPE ON ACCOUNT.Act_id = ACCOUNTYPE.Act_id RIGHT OUTER JOIN  (SELECT Ac_id, CASE WHEN Balance > 0 THEN Balance ELSE 0 END AS Dr, CASE WHEN Balance < 0 THEN - 1 * (Balance) ELSE 0 END AS Cr  FROM ACCOUNT AS ACCOUNT_1 where Branch_id='" + Database.BranchId + "'  UNION ALL  SELECT Journal.Ac_id, CASE WHEN JOURNAL.Amount > 0 THEN JOURNAL.Amount ELSE 0 END AS Dr,  CASE WHEN JOURNAL.Amount < 0 THEN - 1 * (JOURNAL.Amount) ELSE 0 END AS Cr  FROM VOUCHERTYPE RIGHT OUTER JOIN  VOUCHERINFO ON VOUCHERTYPE.Vt_id = VOUCHERINFO.Vt_id RIGHT OUTER JOIN  Journal ON VOUCHERINFO.Vi_id = Journal.Vi_id  WHERE ( VOUCHERTYPE.A = 'true') and Branch_id='" + Database.BranchId + "') AS res ON ACCOUNT.Ac_id = res.Ac_id WHERE     ( ACCOUNTYPE.Path LIKE '" + dt.Rows[i]["Nid"].ToString() + ";%') GROUP BY ACCOUNT.Name ";
                                Sqlda = new SqlDataAdapter(str, SqlCnnSource);
                                Sqlda.Fill(dtTot);
                            }
                        }
                        else if (Database.BMode == "B")
                        {
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                str = "SELECT CASE WHEN SUM(res.Dr) > SUM(res.Cr) THEN SUM(res.Dr) - SUM(res.Cr) ELSE 0 END AS Dr, CASE WHEN SUM(res.Cr) > SUM(res.Dr) THEN SUM(res.Cr) - SUM(res.Dr)  ELSE 0 END AS Cr, ACCOUNT.Name FROM ACCOUNT LEFT OUTER JOIN  ACCOUNTYPE ON ACCOUNT.Act_id = ACCOUNTYPE.Act_id RIGHT OUTER JOIN  (SELECT Ac_id, CASE WHEN Balance2 > 0 THEN Balance2 ELSE 0 END AS Dr, CASE WHEN Balance2 < 0 THEN - 1 * (Balance2) ELSE 0 END AS Cr  FROM ACCOUNT AS ACCOUNT_1 where Branch_id='" + Database.BranchId + "'  UNION ALL  SELECT Journal.Ac_id, CASE WHEN JOURNAL.Amount > 0 THEN JOURNAL.Amount ELSE 0 END AS Dr,  CASE WHEN JOURNAL.Amount < 0 THEN - 1 * (JOURNAL.Amount) ELSE 0 END AS Cr  FROM VOUCHERTYPE RIGHT OUTER JOIN  VOUCHERINFO ON VOUCHERTYPE.Vt_id = VOUCHERINFO.Vt_id RIGHT OUTER JOIN  Journal ON VOUCHERINFO.Vi_id = Journal.Vi_id  WHERE ( VOUCHERTYPE.B = 'true') and Branch_id='" + Database.BranchId + "') AS res ON ACCOUNT.Ac_id = res.Ac_id WHERE     ( ACCOUNTYPE.Path LIKE '" + dt.Rows[i]["Nid"].ToString() + ";%')  GROUP BY ACCOUNT.Name ";
                                Sqlda = new SqlDataAdapter(str, SqlCnnSource);
                                Sqlda.Fill(dtTot);
                            }
                        }
                    }
                  

                    if (Database.DatabaseType == "access")
                    {
                        da = new OleDbDataAdapter(str, AccessCnnSource);
                        da.Fill(dtTot);
                    }
                    else
                    {
                        Sqlda = new SqlDataAdapter(str, SqlCnnSource);

                        Sqlda.Fill(dtTot);
                    }

                    ansGridView3.Columns.Add("Name", "Name");
                    ansGridView3.Columns.Add("Dr", "Dr");
                    ansGridView3.Columns.Add("Cr", "Cr");

                    ansGridView3.Columns["Name"].Width = 150;
                    ansGridView3.Columns["Dr"].Width = 100;
                    ansGridView3.Columns["Cr"].Width = 100;

                    int j = 0;

                    for (int i = 0; i < dtTot.Rows.Count; i++)
                    {

                        if (Database.BMode == "A")
                        {
                            if ((double.Parse(dtTot.Rows[i]["Dr"].ToString()) - double.Parse(dtTot.Rows[i]["Cr"].ToString())) == 0 && (double.Parse(dtTot.Rows[i]["Cr"].ToString()) - double.Parse(dtTot.Rows[i]["Dr"].ToString())) == 0)
                            {
                                continue;
                            }

                            ansGridView3.Rows.Add();

                            ansGridView3.Rows[j].Cells["Name"].Value = dtTot.Rows[i]["Name"];
                            if ((double.Parse(dtTot.Rows[i]["Dr"].ToString()) - double.Parse(dtTot.Rows[i]["Cr"].ToString())) > 0)
                            {
                                ansGridView3.Rows[j].Cells["Dr"].Value = funs.DecimalPoint(double.Parse(dtTot.Rows[i]["Dr"].ToString()) - double.Parse(dtTot.Rows[i]["Cr"].ToString()));
                            }
                            else
                            {
                                ansGridView3.Rows[j].Cells["Dr"].Value = "0.00";
                            }

                            if ((double.Parse(dtTot.Rows[i]["Cr"].ToString()) - double.Parse(dtTot.Rows[i]["Dr"].ToString())) > 0)
                            {
                                ansGridView3.Rows[j].Cells["Cr"].Value = funs.DecimalPoint(double.Parse(dtTot.Rows[i]["Cr"].ToString()) - double.Parse(dtTot.Rows[i]["Dr"].ToString()));
                            }
                            else
                            {
                                ansGridView3.Rows[j].Cells["Cr"].Value = "0.00";
                            }
                        }
                        else if (Database.BMode == "B")
                        {
                            if ((double.Parse(dtTot.Rows[i]["Dr"].ToString()) - double.Parse(dtTot.Rows[i]["Cr"].ToString())) == 0 && (double.Parse(dtTot.Rows[i]["Cr"].ToString()) - double.Parse(dtTot.Rows[i]["Dr"].ToString())) == 0)
                            {
                                continue;
                            }

                            ansGridView3.Rows.Add();

                            ansGridView3.Rows[j].Cells["Name"].Value = dtTot.Rows[i]["Name"];

                            if ((double.Parse(dtTot.Rows[i]["Dr"].ToString()) - double.Parse(dtTot.Rows[i]["Cr"].ToString())) > 0)
                            {
                                ansGridView3.Rows[j].Cells["Dr"].Value = funs.DecimalPoint(double.Parse(dtTot.Rows[i]["Dr"].ToString()) - double.Parse(dtTot.Rows[i]["Cr"].ToString()));
                            }
                            else
                            {
                                ansGridView3.Rows[j].Cells["Dr"].Value = "0.00";
                            }




                            if ((double.Parse(dtTot.Rows[i]["Cr"].ToString()) - double.Parse(dtTot.Rows[i]["Dr"].ToString())) > 0)
                            {
                                ansGridView3.Rows[j].Cells["Cr"].Value = funs.DecimalPoint(double.Parse(dtTot.Rows[i]["Cr"].ToString()) - double.Parse(dtTot.Rows[i]["Dr"].ToString()));
                            }
                            else
                            {
                                ansGridView3.Rows[j].Cells["Cr"].Value = "0.00";
                            }

                        }
                        j++;
                    }
                    dtTot.Clear();
                    string ssql = "";
                    if (Database.BMode == "A")
                    {
                        if (Database.DatabaseType == "sql")
                        {
                            ssql = "SELECT     Name, Total AS Balance, Closing_Bal as ClosingBal FROM         (SELECT     ACCOUNT.Name, 0 AS Total, CASE WHEN ACCOUNT.Closing_Bal IS NULL THEN '' ELSE ACCOUNT.Closing_Bal END AS Closing_Bal  FROM          ACCOUNT LEFT OUTER JOIN     ACCOUNTYPE ON ACCOUNT.Act_id = ACCOUNTYPE.Act_id wHERE      (ACCOUNTYPE.Name = 'STOCK-IN-HAND')  and  Branch_id='" + Database.BranchId + "'  ) AS final";
                        }
                        else
                        {
                            ssql = "SELECT     Name, SUM(Total) AS Balance,  iif(Closing_Bal is null,'',Closing_Bal) as ClosingBal FROM         (SELECT    ACCOUNT.Name, 0 AS Total, ACCOUNT.Closing_Bal   FROM         ACCOUNT LEFT OUTER JOIN ACCOUNTYPE ON ACCOUNT.Act_id = ACCOUNTYPE.Act_id  WHERE      (ACCOUNTYPE.Name = 'STOCK-IN-HAND') and Branch_id='" + Database.BranchId + "') AS final GROUP BY Name, Closing_Bal";
                        }

                    }
                    else if (Database.BMode == "B")
                    {
                        if (Database.DatabaseType == "sql")
                        {
                            ssql = "SELECT     Name, SUM(Total) AS Balance,CASE WHEN Closing_Bal2 IS NULL THEN '' ELSE Closing_Bal2 END AS ClosingBal2   FROM         (SELECT     ACCOUNT.Name, 0 AS Total,ACCOUNT.Closing_Bal2 FROM         ACCOUNT LEFT OUTER JOIN  ACCOUNTYPE ON ACCOUNT.Act_id = ACCOUNTYPE.Act_id WHERE     (ACCOUNTYPE.Name = 'STOCK-IN-HAND') and Branch_id='" + Database.BranchId + "') AS final GROUP BY Name, Closing_Bal2";
                        }
                        else
                        {
                            ssql = "SELECT     Name, SUM(Total) AS Balance,  iif(Closing_Bal2 = null,'',Closing_Bal2) as ClosingBal2 FROM         (SELECT    ACCOUNT.Name, 0 AS Total, ACCOUNT.Closing_Bal2   FROM         ACCOUNT LEFT OUTER JOIN ACCOUNTYPE ON ACCOUNT.Act_id = ACCOUNTYPE.Act_id  WHERE      (ACCOUNTYPE.Name = 'STOCK-IN-HAND') and Branch_id='" + Database.BranchId + "') AS final GROUP BY Name, Closing_Bal2";
                        }
                    }

                    if (Database.DatabaseType == "access")
                    {
                        da = new OleDbDataAdapter(ssql, AccessCnnSource);

                        da.Fill(dtTot);
                    }
                    else
                    {
                        Sqlda = new SqlDataAdapter(ssql, SqlCnnSource);
                        Sqlda.Fill(dtTot);
                    }
                    for (int i = 0; i < dtTot.Rows.Count; i++)
                    {
                        ansGridView3.Rows.Add();

                        ansGridView3.Rows[j].Cells["Name"].Value = dtTot.Rows[i]["Name"];
                        ansGridView3.Rows[j].Cells["Cr"].Value = 0;
                        if (Database.IsKacha == false)
                        {
                            if (dtTot.Rows[i]["ClosingBal"].ToString() == "")
                            {
                                dtTot.Rows[i]["ClosingBal"] = double.Parse(dtTot.Rows[i]["Balance"].ToString());
                            }

                            else
                            {
                                dtTot.Rows[i]["ClosingBal"] = double.Parse(dtTot.Rows[i]["ClosingBal"].ToString());
                            }


                            ansGridView3.Rows[j].Cells["Dr"].Value = funs.DecimalPoint(double.Parse(dtTot.Rows[i]["ClosingBal"].ToString()));
                        }
                        else
                        {

                            if (dtTot.Rows[i]["ClosingBal2"].ToString() == "")
                            {
                                dtTot.Rows[i]["ClosingBal2"] = double.Parse(dtTot.Rows[i]["Balance"].ToString());
                            }

                            else
                            {
                                dtTot.Rows[i]["ClosingBal2"] = double.Parse(dtTot.Rows[i]["ClosingBal2"].ToString());
                            }

                            ansGridView3.Rows[j].Cells["Dr"].Value = funs.DecimalPoint(double.Parse(dtTot.Rows[i]["ClosingBal2"].ToString()));
                        }
                        j++;
                    }
                    tabControl1.SelectedIndex = 1;
                }
                else if (frmBalTrans == "Stock")
                {


                    string valuation ="LastPurchaseRate";
                    DataTable dtTot1 = new DataTable();
                    DataTable dt2 = new DataTable();
                    string sql = "";
                    string sql2 = "";

                    if (Database.BMode == "A")
                    {
                        //sql = "SELECT case when ACCOUNT.Name Is Null then '<MAIN>' Else ACCOUNT.Name End AS Godown, Description.Description as descr, Description.Pack as Packing, Sum(Receive)-Sum(Issue) AS Stock, Sum(0.01) AS Amount,Stock.godown_id, Stock.Did as Did FROM (Stock LEFT JOIN Description ON Stock.Did = Description.Des_id) LEFT JOIN ACCOUNT ON Stock.godown_id = ACCOUNT.Ac_id WHERE (((Stock.marked)= " + access_sql.Singlequote + "False " + access_sql.Singlequote + ")) AND (Description.StkMaintain = 'true') AND (dbo.Stock.Branch_id = '"+Database.BranchId+"') GROUP BY Description.Description, Description.Pack, ACCOUNT.Name, Stock.godown_id, Stock.Did HAVING (((Sum([Receive])-Sum([Issue]))>0))";
                        sql="SELECT  CASE WHEN ACCOUNT.Name IS NULL THEN '<MAIN>' ELSE ACCOUNT.Name END AS Godown,  Description.Description AS descr,    Description.Pack AS Packing, SUM( Stock.Receive) - SUM( Stock.Issue) AS Stock, SUM(0.01) AS Amount,  Stock.godown_id,  Stock.Did FROM  VOUCHERTYPE RIGHT OUTER JOIN   VOUCHERINFO ON  VOUCHERTYPE.Vt_id =  VOUCHERINFO.Vt_id RIGHT OUTER JOIN  Stock ON  VOUCHERINFO.Vi_id =  Stock.Vid LEFT OUTER JOIN  Description ON  Stock.Did =  Description.Des_id LEFT OUTER JOIN  ACCOUNT ON  Stock.godown_id =  ACCOUNT.Ac_id WHERE  ( Description.StkMaintain = 'true') AND (VOUCHERTYPE."+Database.BMode+" ='true' )  AND (dbo.Voucherinfo.Branch_id = '"+Database.BranchId+"') GROUP BY  Description.Description,  Description.Pack,  ACCOUNT.Name,  Stock.godown_id,  Stock.Did HAVING   (SUM( Stock.Receive) - SUM( Stock.Issue) > 0)";

                        sql2 = "SELECT  Stock.Did, Stock.ReceiveAmt as Amount ,  Stock.Receive AS Qty, Stock.ReceiveAmt /  Stock.Receive AS Rate, CASE WHEN VOUCHERINFO.Vdate IS NULL  THEN datediff(day,-1," + Database.stDate.ToString("dd-mm-yyyy") + ") ELSE VOUCHERINFO.Vdate END AS Vdate, Stock.godown_id FROM          Stock LEFT OUTER JOIN   VOUCHERINFO ON  Stock.Vid =  VOUCHERINFO.Vi_id WHERE     ( Stock.Receive <> 0) AND ( Stock.ReceiveAmt <> 0) AND ( Stock.marked = " + access_sql.Singlequote + "false" + access_sql.Singlequote + ") AND (dbo.Stock.Branch_id = '" + Database.BranchId + "')  ORDER BY  Stock.Did, Vdate DESC, VOUCHERINFO.Vnumber DESC , Stock.Itemsr DESC";
                    }
                    else if (Database.BMode == "B")
                    {
                        sql = "SELECT  CASE WHEN ACCOUNT.Name IS NULL THEN '<MAIN>' ELSE ACCOUNT.Name END AS Godown,  Description.Description AS descr,    Description.Pack AS Packing, SUM( Stock.Receive) - SUM( Stock.Issue) AS Stock, SUM(0.01) AS Amount,  Stock.godown_id,  Stock.Did FROM  VOUCHERTYPE RIGHT OUTER JOIN   VOUCHERINFO ON  VOUCHERTYPE.Vt_id =  VOUCHERINFO.Vt_id RIGHT OUTER JOIN  Stock ON  VOUCHERINFO.Vi_id =  Stock.Vid LEFT OUTER JOIN  Description ON  Stock.Did =  Description.Des_id LEFT OUTER JOIN  ACCOUNT ON  Stock.godown_id =  ACCOUNT.Ac_id WHERE  ( Description.StkMaintain = 'true') AND (VOUCHERTYPE." + Database.BMode + " ='true' )  AND (dbo.Voucherinfo.Branch_id = '" + Database.BranchId + "') GROUP BY  Description.Description,  Description.Pack,  ACCOUNT.Name,  Stock.godown_id,  Stock.Did HAVING   (SUM( Stock.Receive) - SUM( Stock.Issue) > 0)";


                        sql2 = "SELECT  Stock.Did, Stock.ReceiveAmt as Amount ,  Stock.Receive AS Qty, Stock.ReceiveAmt /  Stock.Receive AS Rate, CASE WHEN VOUCHERINFO.Vdate IS NULL  THEN datediff(day,-1," + Database.stDate.ToString("dd-mm-yyyy") + ") ELSE VOUCHERINFO.Vdate END AS Vdate, Stock.godown_id FROM          Stock LEFT OUTER JOIN   VOUCHERINFO ON  Stock.Vid =  VOUCHERINFO.Vi_id WHERE     ( Stock.Receive <> 0) AND ( Stock.ReceiveAmt <> 0) AND ( Stock.marked = " + access_sql.Singlequote + "true" + access_sql.Singlequote + ") AND (dbo.Stock.Branch_id = '" + Database.BranchId + "')  ORDER BY  Stock.Did, Vdate DESC, VOUCHERINFO.Vnumber DESC , Stock.Itemsr DESC";
                    }


                
                  
                        Sqlda = new SqlDataAdapter(sql, SqlCnnSource);
                        Sqlda.Fill(dtTot1);

                        Sqlda = new SqlDataAdapter(sql2, SqlCnnSource);
                        Sqlda.Fill(dt2);

                     //   dt2 = dt2.Select().CopyToDataTable();
                    ansGridView3.Columns.Add("godown", "Godown");
                    ansGridView3.Columns.Add("godown_id", "godown_id");
                    ansGridView3.Columns.Add("Des_id", "Des_id");
                    ansGridView3.Columns.Add("desc", "Description");
                    ansGridView3.Columns.Add("nm", "Name");
                 
                    ansGridView3.Columns.Add("Open_stock", "Open_stock");
                  
                    ansGridView3.Columns.Add("ValuationAmt", "ValuationAmt");
                 



                    ansGridView3.Columns["godown_id"].Visible = false;
                    ansGridView3.Columns["Des_id"].Visible = false;
                    ansGridView3.Columns["nm"].Width = 50;
                    ansGridView3.Columns["desc"].Width = 250;
                    double amt = 0;
                    for (int k = 0; k < dtTot1.Rows.Count; k++)
                    {
                        ansGridView3.Rows.Add();
                        ansGridView3.Rows[k].Cells["desc"].Value = dtTot1.Rows[k]["descr"].ToString();
                        ansGridView3.Rows[k].Cells["nm"].Value = dtTot1.Rows[k]["Packing"].ToString();
                     

                        ansGridView3.Rows[k].Cells["Open_stock"].Value = dtTot1.Rows[k]["Stock"].ToString();
                     
                        ansGridView3.Rows[k].Cells["Godown"].Value = dtTot1.Rows[k]["Godown"].ToString();
                       
                      
                            if (dt2.Select("Did='" + dtTot1.Rows[k]["Did"].ToString() + "' And godown_id='" + dtTot1.Rows[k]["godown_id"].ToString()+"'", "").Length <= 0)
                            {

                                dtTot1.Rows[k]["Amount"] = 0;
                            }
                            else
                            {
                                double sumqty = 0, sumamt = 0;
                                double rate = 0;
                                sumqty = double.Parse(dt2.Compute("Sum(Qty)", "Did='" + dtTot1.Rows[k]["Did"].ToString() + "' And godown_id='" + dtTot1.Rows[k]["godown_id"].ToString()+"'").ToString());
                                sumamt = double.Parse(dt2.Compute("Sum(Amount)", "Did='" + dtTot1.Rows[k]["Did"].ToString() + "' And godown_id='" + dtTot1.Rows[k]["godown_id"].ToString() + "'").ToString());
                                if (sumqty == 0)
                                {
                                    rate = 0;
                                }

                                //else if (valuation == "AverageRate")
                                //{


                                //    rate = double.Parse(dt2.Compute("Sum(Amount)/Sum(Qty)", "Did=" + dtTot1.Rows[k]["Did"] + " And godown_id=" + dtTot1.Rows[k]["godown_id"] + " And  MRP=" + double.Parse(dtTot1.Rows[k]["MRP"].ToString()) + "  And Batch_no='" + dtTot.Rows[k]["Batch_no"].ToString() + "'").ToString());
                                //}
                                else
                                {
                                    rate = double.Parse(dt2.Select("Did='" + dtTot1.Rows[k]["Did"].ToString() + "' And godown_id='" + dtTot1.Rows[k]["godown_id"].ToString() + "'", "").FirstOrDefault()["Rate"].ToString());
                                }
                               
                                dtTot1.Rows[k]["Amount"] = double.Parse(funs.DecimalPoint(rate * double.Parse(dtTot1.Rows[k]["Stock"].ToString()), 2));
                                amt += double.Parse(funs.DecimalPoint(dtTot1.Rows[k]["Amount"].ToString(), 2));
                            }
                       
                        ansGridView3.Rows[k].Cells["ValuationAmt"].Value = dtTot1.Rows[k]["Amount"].ToString();
                        label3.Text = k.ToString();
                       // label6.Text = amt.ToString();
                        tabControl1.SelectedIndex = 1;
                    }



                    //DataTable dt2 = new DataTable();
                    //string sql = "";
                    //string sql2 = "";
                    //string godown = "SER596";

                  
                    //  sql = "SELECT CASE WHEN ACCOUNT.Name IS NULL THEN '<MAIN>' ELSE ACCOUNT.Name END AS Godown, Description_3.Description, Description_3.Pack AS Packing, SUM(final.Opening + final.Purchase - final.Sale) AS Stock, SUM(final.OpeningAmt + final.PurchaseAmt - final.SaleAmt) AS Amount, final.Did                 FROM ACCOUNT RIGHT OUTER JOIN (SELECT 'Opening Balance' AS Type, Did, SUM(Qty) AS Opening, SUM(Amount) AS OpeningAmt, 0 AS Purchase, 0 AS PurchaseAmt, 0 AS Sale, 0 AS SaleAmt,  godown_id FROM (SELECT Stock.Did, SUM(Stock.Issue) * - 1 AS Qty, - (1 * SUM(Stock.IssueAmt)) AS Amount, Stock.godown_id FROM Description RIGHT OUTER JOIN  Stock ON Description.Des_id = Stock.Did LEFT OUTER JOIN  VOUCHERTYPE RIGHT OUTER JOIN  VOUCHERINFO ON VOUCHERTYPE.Vt_id = VOUCHERINFO.Vt_id ON Stock.Vid = VOUCHERINFO.Vi_id WHERE (VOUCHERINFO.Vdate < '01-Jan-2018') AND (Description.StkMaintain = 'True')   GROUP BY Stock.Did, Stock.godown_id  UNION ALL  SELECT Stock_2.Did, SUM(Stock_2.Receive) AS Qty, SUM(Stock_2.ReceiveAmt) AS Amount, Stock_2.godown_id  FROM Description AS Description_2 RIGHT OUTER JOIN  Stock AS Stock_2 ON Description_2.Des_id = Stock_2.Did LEFT OUTER JOIN  VOUCHERTYPE AS VOUCHERTYPE_2 RIGHT OUTER JOIN  VOUCHERINFO AS VOUCHERINFO_2 ON VOUCHERTYPE_2.Vt_id = VOUCHERINFO_2.Vt_id ON   Stock_2.Vid = VOUCHERINFO_2.Vi_id  WHERE (VOUCHERINFO_2.Vdate < '01-Jan-2018') AND (Description_2.StkMaintain = 'true')  GROUP BY Stock_2.Did, Stock_2.godown_id) AS opn  GROUP BY Did, godown_id  UNION ALL  SELECT '' AS Type, Stock_1.Did, 0 AS Opening, 0 AS OpeningAmt, SUM(Stock_1.Receive) AS Purchase, SUM(Stock_1.ReceiveAmt) AS PurchaseAmt,   SUM(Stock_1.Issue) AS Sale, SUM(Stock_1.IssueAmt) AS SaleAmt, Stock_1.godown_id  FROM Description AS Description_1 RIGHT OUTER JOIN  Stock AS Stock_1 ON Description_1.Des_id = Stock_1.Did RIGHT OUTER JOIN  VOUCHERINFO AS VOUCHERINFO_1 LEFT OUTER JOIN  VOUCHERTYPE AS VOUCHERTYPE_1 ON VOUCHERINFO_1.Vt_id = VOUCHERTYPE_1.Vt_id ON Stock_1.Vid = VOUCHERINFO_1.Vi_id  WHERE (VOUCHERINFO_1.Vdate >= '01-Jan-2018') AND  Description_1.StkMaintain = 'true' AND   (VOUCHERINFO_1.Vdate <= '31-Mar-2018')  GROUP BY Stock_1.Did, Stock_1.godown_id) AS final ON ACCOUNT.Ac_id = final.godown_id LEFT OUTER JOIN  Description AS Description_3 ON final.Did = Description_3.Des_id  GROUP BY Description_3.Description, Description_3.Pack, final.Did, ACCOUNT.Name ORDER BY Description_3.Description ";
  
                    //    Sqlda = new SqlDataAdapter(sql, SqlCnnSource);
                    //    Sqlda.Fill(dtTot);

                       
                       
                    //ansGridView3.Columns.Add("godown", "Godown");
                    //ansGridView3.Columns.Add("godown_id", "godown_id");
                    //ansGridView3.Columns.Add("Des_id", "Des_id");
                    //ansGridView3.Columns.Add("desc", "Description");
                    //ansGridView3.Columns.Add("nm", "Name");
                    //ansGridView3.Columns.Add("Open_stock", "Open_stock");
                    //ansGridView3.Columns.Add("ValuationAmt", "ValuationAmt");


                    //ansGridView3.Columns["godown_id"].Visible = false;
                    //ansGridView3.Columns["Des_id"].Visible = false;
                    //ansGridView3.Columns["nm"].Width = 50;
                    //ansGridView3.Columns["desc"].Width = 250;

                    //dtTot = dtTot.Select("Stock<>0").CopyToDataTable();


                 
                    //for (int k = 0; k < dtTot.Rows.Count; k++)
                    //{
                    //    ansGridView3.Rows.Add();
                    //    ansGridView3.Rows[k].Cells["desc"].Value = dtTot.Rows[k]["Description"].ToString();
                    //    ansGridView3.Rows[k].Cells["nm"].Value = dtTot.Rows[k]["Packing"].ToString();
                    //    ansGridView3.Rows[k].Cells["Open_stock"].Value = dtTot.Rows[k]["Stock"].ToString();
                    //    ansGridView3.Rows[k].Cells["Godown"].Value = funs.Select_ac_nm(godown); ;

                      
                    //    ansGridView3.Rows[k].Cells["ValuationAmt"].Value = dtTot.Rows[k]["Amount"].ToString();
                    //    label3.Text = k.ToString();
                    //}

                    //tabControl1.SelectedIndex = 1;
                }

            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DataTable dtAccmatch = new DataTable();
            DataTable dtFirmNew = new DataTable();
            Database.GetOtherSqlData("select Firm_database from firminfo where Firm_name='" + ansGridView2.Rows[ansGridView2.SelectedCells[0].RowIndex].Cells["Firm_name"].Value + "' and Firm_Period_name='" + ansGridView2.Rows[ansGridView2.SelectedCells[0].RowIndex].Cells["Firm_Period_name"].Value + "'", dtFirmNew);
            String DestdbName = dtFirmNew.Rows[0][0].ToString();
            if (Database.DatabaseType == "access")
            {
                AccessCnnDest = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Application.StartupPath + "\\Database\\" + DestdbName + ".mdb;Persist Security Info=true;Jet OLEDB:Database Password=ptsoft9358524971");
                AccessCnnDest.Open();
            }
            else
            {
                SqlCnnDest = new SqlConnection("Data Source=" + Database.inipath + ";Initial Catalog=" + DestdbName + ";Persist Security Info=True;User ID=sa;password=" + Database.sqlseverpwd + ";Connection Timeout=100");
                SqlCnnDest.Open();
            }
            ansGridView4.Columns.Clear();



            if (frmBalTrans == "Account")
            {
                ansGridView4.Columns.Add("nm", "Name");
                ansGridView4.Columns.Add("dr", "Dr");
                ansGridView4.Columns.Add("cr", "Cr");

                ansGridView4.Columns["nm"].Width = 175;
                ansGridView4.Columns["dr"].Width = 150;
                ansGridView4.Columns["cr"].Width = 150;
            }
            else
            {
                ansGridView4.Columns.Add("desc", "Description");
                ansGridView4.Columns.Add("Open_stock", "Open_stock");

                ansGridView4.Columns["desc"].Width = 250;
            }



            int j = 0;
            DataTable dtno = new DataTable();
            dtno.Columns.Add("description",typeof(string));
            dtno.Columns.Add("packing", typeof(string));
            for (int i = 0; i <= ansGridView3.RowCount - 1; i++)
            {
                if (frmBalTrans == "Account")
                {
                    if (Database.DatabaseType == "access")
                    {
                        da = new OleDbDataAdapter("select name from account where name='" + ansGridView3.Rows[i].Cells["Name"].Value + "'", AccessCnnDest);
                        dtAccmatch.Clear();
                        da.Fill(dtAccmatch);
                    }
                    else
                    {
                        Sqlda = new SqlDataAdapter("select name from account where name='" + ansGridView3.Rows[i].Cells["Name"].Value + "' and Branch_id='" + Database.BranchId + "'", SqlCnnDest);
                        dtAccmatch.Clear();
                        Sqlda.Fill(dtAccmatch);
                    }

                    if (dtAccmatch.Rows.Count == 0)
                    {
                        ansGridView4.Rows.Add();
                        ansGridView4.Rows[j].Cells["nm"].Value = ansGridView3.Rows[i].Cells["Name"].Value;
                        ansGridView4.Rows[j].Cells["dr"].Value = ansGridView3.Rows[i].Cells["Dr"].Value;
                        ansGridView4.Rows[j].Cells["cr"].Value = ansGridView3.Rows[i].Cells["Cr"].Value;
                        j++;
                    }

                }
                else if (frmBalTrans == "Stock")
                {

                    if (Database.DatabaseType == "access")
                    {
                        da = new OleDbDataAdapter("SELECT DESCRIPTION.Description, DESCRIPTION.Pack FROM DESCRIPTION WHERE (((DESCRIPTION.Description)='" + ansGridView3.Rows[i].Cells["desc"].Value + "') AND ((DESCRIPTION.Pack)='" + ansGridView3.Rows[i].Cells["nm"].Value + "'))", AccessCnnDest);
                        dtAccmatch.Clear();
                        da.Fill(dtAccmatch);
                    }
                    else
                    {
                        Sqlda = new SqlDataAdapter("SELECT DESCRIPTION.Description, DESCRIPTION.Pack FROM DESCRIPTION WHERE (((DESCRIPTION.Description)='" + ansGridView3.Rows[i].Cells["desc"].Value + "') AND ((DESCRIPTION.Pack)='" + ansGridView3.Rows[i].Cells["nm"].Value + "'))", SqlCnnDest);
                        dtAccmatch.Clear();
                        Sqlda.Fill(dtAccmatch);
                    }

                    if (dtAccmatch.Rows.Count == 0)
                    {
                        ansGridView4.Rows.Add();
                        ansGridView4.Rows[j].Cells["desc"].Value = ansGridView3.Rows[i].Cells["desc"].Value + "(" + ansGridView3.Rows[i].Cells["nm"].Value + ")";

                        ansGridView4.Rows[j].Cells["Open_stock"].Value = ansGridView3.Rows[i].Cells["Open_stock"].Value;
                        label4.Text = j.ToString();
                        j++;

                    }

                    //if (Database.DatabaseType == "access")
                    //{
                    //    da = new OleDbDataAdapter("SELECT DESCRIPTION.Description, DESCRIPTION.Pack FROM DESCRIPTION WHERE (((DESCRIPTION.Description)='" + ansGridView3.Rows[i].Cells["desc"].Value + "') AND ((DESCRIPTION.Pack)='" + ansGridView3.Rows[i].Cells["nm"].Value + "'))", AccessCnnDest);
                    //    dtAccmatch.Clear();
                    //    da.Fill(dtAccmatch);
                    //}
                    //else
                    //{
                    //    Sqlda = new SqlDataAdapter("SELECT DESCRIPTION.Description, DESCRIPTION.Pack FROM DESCRIPTION WHERE (((DESCRIPTION.Description)='" + ansGridView3.Rows[i].Cells["desc"].Value + "') AND ((DESCRIPTION.Pack)='" + ansGridView3.Rows[i].Cells["nm"].Value + "'))", SqlCnnDest);
                    //    dtAccmatch.Clear();
                    //    Sqlda.Fill(dtAccmatch);
                    //}

                    //if (dtAccmatch.Rows.Count == 0)
                    //{

                    //    ansGridView4.Rows.Add();
                    //    ansGridView4.Rows[j].Cells["desc"].Value = ansGridView3.Rows[i].Cells["desc"].Value + "(" + ansGridView3.Rows[i].Cells["nm"].Value + ")";

                    //    ansGridView4.Rows[j].Cells["Open_stock"].Value = ansGridView3.Rows[i].Cells["Open_stock"].Value;
                    //    label4.Text = j.ToString();

                    //    dtno.Rows.Add();
                    //    dtno.Rows[dtno.Rows.Count - 1]["description"] = ansGridView3.Rows[i].Cells["desc"].Value.ToString();
                    //    dtno.Rows[dtno.Rows.Count - 1]["packing"] = ansGridView3.Rows[i].Cells["nm"].Value.ToString();


                    //    j++;

                    //}
                }
               

            }
          
            tabControl1.SelectedIndex = 2;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (frmBalTrans == "Account")
            {
              
                    if (Database.BMode == "A")
                    {
                        Sqlcomm = new SqlCommand("update account set Balance=0 where Branch_id='" + Database.BranchId + "'", SqlCnnDest);
                        Sqlcomm.ExecuteNonQuery();
                    }
                    else if (Database.BMode == "B")
                    {
                        Sqlcomm = new SqlCommand("update account set Balance2=0 where Branch_id='" + Database.BranchId + "'", SqlCnnDest);
                        Sqlcomm.ExecuteNonQuery();
                    }
              

                for (int i = 0; i <= ansGridView3.RowCount - 1; i++)
                {
                    double bal = 0;
                    bal = double.Parse(ansGridView3.Rows[i].Cells["Dr"].Value.ToString()) - double.Parse(ansGridView3.Rows[i].Cells["Cr"].Value.ToString());
                    if (Database.DatabaseType == "access")
                    {
                        if (Database.BMode == "A")
                        {
                            comm = new OleDbCommand("update account set Balance=" + bal + " where [name]='" + ansGridView3.Rows[i].Cells["Name"].Value + "'", AccessCnnDest);
                            comm.ExecuteNonQuery();
                        }
                        else if (Database.BMode == "B")
                        {
                            comm = new OleDbCommand("update account set Balance2=" + bal + " where [name]='" + ansGridView3.Rows[i].Cells["Name"].Value + "'", AccessCnnDest);
                            comm.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        if (Database.BMode == "A")
                        {

                            Sqlcomm = new SqlCommand("update account set Balance=" + bal + " where [name]='" + ansGridView3.Rows[i].Cells["Name"].Value + "' and Branch_id='" + Database.BranchId + "'", SqlCnnDest);
                            Sqlcomm.ExecuteNonQuery();
                        }
                        else if (Database.BMode == "B")
                        {
                            Sqlcomm = new SqlCommand("update account set Balance2=" + bal + " where [name]='" + ansGridView3.Rows[i].Cells["Name"].Value + "' and Branch_id='" + Database.BranchId + "'", SqlCnnDest);
                            Sqlcomm.ExecuteNonQuery();
                        }
                    }


                }
            }
            else if (frmBalTrans == "Stock")
            {
                string str = "";
                DataTable dtstock = new DataTable("Stock");
                string sql = "";
                string sql2 = "";
                DataTable dtp = new DataTable();
                DataTable dt2 = new DataTable();

                if(Database.DatabaseType=="sql")
                {
                    if (Database.BMode == "A")
                    {
                        Database.IsKacha = false;
                    }
                    else if (Database.BMode == "B")
                    {
                        Database.IsKacha = true;
                    }
                 //   sql = "SELECT case when ACCOUNT.Name Is Null then '<MAIN>' Else ACCOUNT.Name End AS Godown, Description.Description as descr, Description.Pack as Packing, Sum(Receive)-Sum(Issue) AS Stock, Sum(0.01) AS Amount,Stock.godown_id, Stock.Did as Did FROM (Stock LEFT JOIN Description ON Stock.Did = Description.Des_id) LEFT JOIN ACCOUNT ON Stock.godown_id = ACCOUNT.Ac_id WHERE (((Stock.marked)= " + access_sql.Singlequote + Database.IsKacha + access_sql.Singlequote + ")) AND (Description.StkMaintain = 'true') AND (dbo.Stock.Branch_id = '" + Database.BranchId + "') GROUP BY Description.Description, Description.Pack, ACCOUNT.Name, Stock.godown_id, Stock.Did HAVING (((Sum([Receive])-Sum([Issue]))>0))";
                    sql = "SELECT  CASE WHEN ACCOUNT.Name IS NULL THEN '<MAIN>' ELSE ACCOUNT.Name END AS Godown,  Description.Description AS descr,    Description.Pack AS Packing, SUM( Stock.Receive) - SUM( Stock.Issue) AS Stock, SUM(0.01) AS Amount,  Stock.godown_id,  Stock.Did FROM  VOUCHERTYPE RIGHT OUTER JOIN   VOUCHERINFO ON  VOUCHERTYPE.Vt_id =  VOUCHERINFO.Vt_id RIGHT OUTER JOIN  Stock ON  VOUCHERINFO.Vi_id =  Stock.Vid LEFT OUTER JOIN  Description ON  Stock.Did =  Description.Des_id LEFT OUTER JOIN  ACCOUNT ON  Stock.godown_id =  ACCOUNT.Ac_id WHERE  ( Description.StkMaintain = 'true') AND (VOUCHERTYPE." + Database.BMode + " ='true' )  AND (dbo.Voucherinfo.branch_id = '" + Database.BranchId + "') GROUP BY  Description.Description,  Description.Pack,  ACCOUNT.Name,  Stock.godown_id,  Stock.Did";


                    sql2 = "SELECT  Stock.Did, Stock.ReceiveAmt as Amount ,  Stock.Receive AS Qty, Stock.ReceiveAmt /  Stock.Receive AS Rate, CASE WHEN VOUCHERINFO.Vdate IS NULL  THEN datediff(day,-1," + Database.stDate.ToString("dd-mm-yyyy") + ") ELSE VOUCHERINFO.Vdate END AS Vdate, Stock.godown_id FROM  Stock LEFT OUTER JOIN   VOUCHERINFO ON  Stock.Vid =  VOUCHERINFO.Vi_id WHERE     ( Stock.Receive <> 0) AND ( Stock.ReceiveAmt <> 0) AND ( Stock.marked = " + access_sql.Singlequote + Database.IsKacha + access_sql.Singlequote + ") AND (dbo.Stock.Branch_id = '" + Database.BranchId + "')  ORDER BY  Stock.Did, Vdate DESC, VOUCHERINFO.Vnumber DESC , Stock.Itemsr DESC";

                  
                    Sqlda = new SqlDataAdapter(sql, SqlCnnSource);
                    dtp.Clear();
                    Sqlda.Fill(dtp);

                    Sqlda = new SqlDataAdapter(sql2, SqlCnnSource);
                    Sqlda.Fill(dt2);

                    for (int k = 0; k < dtp.Rows.Count; k++)
                    {
                    
                            if (dt2.Select("Did='" + dtp.Rows[k]["Did"].ToString() + "' And godown_id='" + dtp.Rows[k]["godown_id"].ToString() + "'", "").Length <= 0)
                            {
                                dtp.Rows[k]["Amount"] = 0;
                            }
                            else
                            {
                                double sumqty = 0, sumamt = 0;
                                double rate = 0;
                                sumqty = double.Parse(dt2.Compute("Sum(Qty)", "Did='" + dtp.Rows[k]["Did"].ToString() + "' And godown_id='" + dtp.Rows[k]["godown_id"].ToString() + "'").ToString());
                                sumamt = double.Parse(dt2.Compute("Sum(Amount)", "Did='" + dtp.Rows[k]["Did"].ToString() + "' And godown_id='" + dtp.Rows[k]["godown_id"].ToString() + "'").ToString());
                                // sumamt = double.Parse(dt2.Compute("Sum(Amount)", "Did=" + dtp.Rows[k]["Did"] + " And godown_id=" + dtp.Rows[k]["godown_id"]).ToString());
                                if (sumqty == 0)
                                {
                                    rate = 0;
                                }

                                //else if (textBox1.Text == "AverageRate")
                                //{

                                //    rate = double.Parse(dt2.Compute("Sum(Amount)/Sum(Qty)", "Did=" + dtp.Rows[k]["Did"] + " And godown_id=" + dtp.Rows[k]["godown_id"] + " And MRP=" + double.Parse(dtp.Rows[k]["MRP"].ToString()) + "  And Batch_no='" + dtp.Rows[k]["Batch_no"].ToString() + "'").ToString());
                                //}
                                else
                                {
                                    rate = double.Parse(funs.DecimalPoint(dt2.Select("Did='" + dtp.Rows[k]["Did"].ToString() + "' And godown_id='" + dtp.Rows[k]["godown_id"].ToString() + "'", "").FirstOrDefault()["Rate"].ToString()));
                                }

                                dtp.Rows[k]["Amount"] = double.Parse(funs.DecimalPoint(rate * double.Parse(dtp.Rows[k]["Stock"].ToString()), 2));

                            }
                     




                    }

                    
                    if (dtp.Rows.Count > 0)
                    {
                        string vtid = "";
                        if (Database.BMode == "A")
                        {

                            str = "select vt_id from Vouchertype where name='Opening Stock'";
                        }
                        else if (Database.BMode == "B")
                        {
                            str = "select vt_id from Vouchertype where name='Opening Stock K'";
                        }
                        Sqlcomm = new SqlCommand(str, SqlCnnDest);
                        if (Sqlcomm.ExecuteScalar() != null)
                        {
                            vtid = Sqlcomm.ExecuteScalar().ToString();

                        }


                        DataTable dtstkvo = new DataTable();

                        str = "Select Vi_id from Voucherinfo where Vt_id='" + vtid + "' AND (Branch_id = '" + Database.BranchId + "')";
                        Sqlda = new SqlDataAdapter(str, SqlCnnDest);
                        dtstkvo.Clear();
                        Sqlda.Fill(dtstkvo);



                        DataTable dtmarked = new DataTable();
                        str = "SELECT COUNT(*) FROM Stock WHERE (marked = '" + Database.IsKacha + "') ";
                        Sqlda = new SqlDataAdapter(str, SqlCnnDest);
                        dtmarked.Clear();
                        Sqlda.Fill(dtmarked);



                        if (dtstkvo.Rows.Count > 0)
                        {
                            DialogResult chk = MessageBox.Show("You will be deleted all Opening Stock data", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                            if (chk == DialogResult.Yes)
                            {
                                for (int l = 0; l < dtstkvo.Rows.Count; l++)
                                {
                                    string strdelvo = "Delete from Voucherinfo where Vi_id='" + dtstkvo.Rows[l]["Vi_id"].ToString()+"'";
                                    Sqlcomm = new SqlCommand(strdelvo, SqlCnnDest);
                                    Sqlcomm.ExecuteNonQuery();


                                    string strdelvod = "Delete from Voucherdet where Vi_id='" + dtstkvo.Rows[l]["Vi_id"].ToString() + "'";
                                    Sqlcomm = new SqlCommand(strdelvod, SqlCnnDest);
                                    Sqlcomm.ExecuteNonQuery();

                                    string strdelstk = "Delete from Stock where Vid='" + dtstkvo.Rows[l]["Vi_id"].ToString() + "'";
                                    Sqlcomm = new SqlCommand(strdelstk, SqlCnnDest);
                                    Sqlcomm.ExecuteNonQuery();


                                    string strdeljou = "Delete from Journal where Vi_id='" + dtstkvo.Rows[l]["Vi_id"].ToString() + "'";
                                    Sqlcomm = new SqlCommand(strdeljou, SqlCnnDest);
                                    Sqlcomm.ExecuteNonQuery();

                                }
                            }
                            else
                            {
                                return;
                            }

                        }

                        str = "Select Vi_id from Voucherinfo where Vt_id='" + vtid+"'";
                        Sqlda = new SqlDataAdapter(str, SqlCnnDest);
                        dtstkvo.Clear();
                        Sqlda.Fill(dtstkvo);

                        DateTime dt = sourceend;
                        DataTable dt1 = dtp.DefaultView.ToTable(true, "godown_id");


                        for (int i = 0; i < dt1.Rows.Count; i++)
                        {
                            DataTable dtVoucherInfo = new DataTable("Voucherinfo");
                            str = "select * from Voucherinfo where Vi_id='0'";
                            Sqlda = new SqlDataAdapter(str, SqlCnnDest);
                            dtVoucherInfo.Clear();
                            Sqlda.Fill(dtVoucherInfo);



                            DataTable dtitems = dtp.Select("godown_id='" + dt1.Rows[i][0].ToString() + "'").CopyToDataTable();



                            DataTable dtvno = new DataTable("Voucherinfo");
                            str = "select max(vnumber) from Voucherinfo where Vt_id='" + vtid + "'";
                            Sqlda = new SqlDataAdapter(str, SqlCnnDest);
                            dtvno.Clear();
                            Sqlda.Fill(dtvno);

                            int vno = 0;

                            if (dtvno.Rows[0][0].ToString() != "")
                            {
                                vno = int.Parse(dtvno.Rows[0][0].ToString()) + 1;
                            }
                            else
                            {
                                vno = i + 1;
                            }

                            int nid = 0;
                            string vid = "";


                            DataTable dtcnt = new DataTable();
                            str = "select count(*) from VOUCHERINFO where locationid='" + Database.LocationId + "'";
                            Sqlda = new SqlDataAdapter(str, SqlCnnDest);
                            dtcnt.Clear();
                            Sqlda.Fill(dtcnt);

                            if (int.Parse(dtcnt.Rows[0][0].ToString()) == 0)
                            {
                                nid = 1;
                            }
                            else
                            {
                                DataTable dtid = new DataTable();
                                str = "select max(Nid) as Nid from VOUCHERINFO where locationid='" + Database.LocationId + "'";
                                Sqlda = new SqlDataAdapter(str, SqlCnnDest);
                                dtid.Clear();
                                Sqlda.Fill(dtid);
                                nid = int.Parse(dtid.Rows[0][0].ToString()) + 1;
                            }

                            vid = Database.LocationId + nid.ToString();






                            //voucherinfo
                            dtVoucherInfo.Rows.Add();

                            string prefix = "";
                            string postfix = "";
                            int padding = 0;
                            string fix = "Select prefix from Vouchertype where vt_id='" + vtid+"'";
                            Sqlcomm = new SqlCommand(fix, SqlCnnDest);
                            if (Sqlcomm.ExecuteScalar() != null)
                            {
                                prefix = Sqlcomm.ExecuteScalar().ToString();

                            }
                            fix = "Select postfix from Vouchertype where vt_id='" + vtid+"'";
                            Sqlcomm = new SqlCommand(fix, SqlCnnDest);
                            if (Sqlcomm.ExecuteScalar() != null)
                            {
                                postfix = Sqlcomm.ExecuteScalar().ToString();

                            }
                            //prefix = Database.GetScalarText("Select prefix from Vouchertype where vt_id=" + vtid + " ");
                            // postfix = Database.GetScalarText("Select postfix from Vouchertype where vt_id=" + vtid + " ");

                            fix = "Select padding from Vouchertype where vt_id='" + vtid+"'";
                            Sqlcomm = new SqlCommand(fix, SqlCnnDest);
                            if (Sqlcomm.ExecuteScalar() != null)
                            {
                                padding = int.Parse(Sqlcomm.ExecuteScalar().ToString());
                            }

                            // padding = Database.GetScalarInt("Select padding from Vouchertype where vt_id=" + vtid + " ");
                            string invoiceno = vno.ToString();
                            dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count - 1]["Invoiceno"] = prefix + invoiceno.PadLeft(padding, '0') + postfix;
                            dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count - 1]["Vt_id"] = vtid;
                            dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count - 1]["Vnumber"] = vno;
                            dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count - 1]["user_id"] = Database.user_id;
                            if (dt1.Rows[i][0].ToString() == "0.0000")
                            {
                                dt1.Rows[i][0] = 0;
                            }

                            dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count - 1]["Nid"] = nid;
                            dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count - 1]["Vi_id"] = vid;
                            dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count - 1]["Branch_id"] = Database.BranchId;
                            dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count - 1]["CreTime"] = System.DateTime.Now.ToString("HH:mm:ss");
                            dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count - 1]["ModTime"] = System.DateTime.Now.ToString("HH:mm:ss");

                            if (Database.utype.ToUpper() == "USER")
                            {
                                dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count - 1]["NApproval"] = true;
                            }
                            else
                            {
                                dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count - 1]["NApproval"] = false;
                            }

                            dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count - 1]["Cash_Pending"] = false;


                            dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count - 1]["ac_id"] = dt1.Rows[i][0].ToString();
                            dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count - 1]["Ac_id2"] = 0;
                            dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count - 1]["Vdate"] = dt.ToString();
                            dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count - 1]["Svdate"] = sourceend.ToString("dd-MMM-yyyy");
                            dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count - 1]["Duedate"] = sourceend.ToString("dd-MMM-yyyy");
                            dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count - 1]["Narr"] = "Opening Stock";
                            dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count - 1]["Reffno"] = "";
                            dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count - 1]["TaxableAmount"] = double.Parse(dtp.Compute("Sum(Amount)", "godown_id='" + dt1.Rows[i][0].ToString()+"'").ToString());
                            dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count - 1]["Totalamount"] = double.Parse(dtp.Compute("Sum(Amount)", "godown_id='" + dt1.Rows[i][0].ToString() + "'").ToString());
                           
                            dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count - 1]["rate"] = 0;
                            dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count - 1]["Roff"] = 0;
                            dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count - 1]["Tdtype"] = true;
                            dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count - 1]["DirectChanged"] = false;
                            dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count - 1]["RCM"] = false;
                            dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count - 1]["5000Allowed"] = false;
                            dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count - 1]["ITC"] = false;
                            dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count - 1]["RoffChanged"] = false;
                            dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count - 1]["TaxChanged"] = false;
                            dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count - 1]["Svnum"] = "0";
                            dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count - 1]["Transport1"] = "";
                            dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count - 1]["Transport2"] = "";
                            dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count - 1]["Grno"] = "";
                            dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count - 1]["DeliveryAt"] = "";
                            dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count - 1]["Transport3"] = "";
                            dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count - 1]["Transport4"] = "";
                            dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count - 1]["Transport5"] = "";
                            dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count - 1]["Transport6"] = "";
                            dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count - 1]["ShiptoAddress1"] = "";
                            dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count - 1]["ShiptoAddress2"] = "";
                            dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count - 1]["ShiptoEmail"] = "";
                            dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count - 1]["ShiptoTIN"] = "";
                            dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count - 1]["ShiptoPhone"] = "";
                            dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count - 1]["ShiptoStateid"] = 0;
                            dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count - 1]["Shipto"] = "";
                            dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count - 1]["ShiptoPAN"] = "";
                            dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count - 1]["ShiptoAadhar"] = "";
                            dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count - 1]["FormC"] = false;
                            dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count - 1]["Conn_id"] = 0;
                            dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count - 1]["Iscancel"] = false;
                          
                            dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count - 1]["LocationId"] = Database.LocationId;



                            Sqlda = new SqlDataAdapter("select * from " + dtVoucherInfo.TableName, SqlCnnDest);
                            SqlCommandBuilder cb = new SqlCommandBuilder();
                            cb.QuotePrefix = "[";
                            cb.QuoteSuffix = "]";
                            cb.DataAdapter = Sqlda;
                            Sqlda.Update(dtVoucherInfo);

                            //int vid = 0;
                            //str = "select max(Vi_id) from voucherinfo";
                            //Sqlcomm = new SqlCommand(str, SqlCnnDest);
                            //if (Sqlcomm.ExecuteScalar() != null)
                            //{
                            //    vid = int.Parse(Sqlcomm.ExecuteScalar().ToString());

                            //}

                            DataTable dtVoucherDet = new DataTable("VOUCHERDET");

                            str = "select * from VOUCHERDET where Vi_id='0'";
                            Sqlda = new SqlDataAdapter(str, SqlCnnDest);
                            dtVoucherDet.Clear();
                            Sqlda.Fill(dtVoucherDet);


                            if (dtVoucherDet.Rows.Count > 0)
                            {
                                for (int m = 0; m < dtVoucherDet.Rows.Count; m++)
                                {
                                    dtVoucherDet.Rows[m].Delete();
                                }

                                //Database.SaveData(dtVoucherDet);
                                Sqlda = new SqlDataAdapter("select * from " + dtVoucherDet.TableName, SqlCnnDest);
                                cb = new SqlCommandBuilder();
                                cb.QuotePrefix = "[";
                                cb.QuoteSuffix = "]";
                                cb.DataAdapter = Sqlda;
                                Sqlda.Update(dtVoucherDet);

                                dtVoucherDet = new DataTable("VOUCHERDET");
                                str = "select * from VOUCHERDET where vi_id='" + vid+"'";
                                Sqlda = new SqlDataAdapter(str, SqlCnnDest);
                                dtVoucherDet.Clear();
                                Sqlda.Fill(dtVoucherDet);
                                // Database.GetSqlData("select * from VOUCHERDET where vi_id=" + vid, dtVoucherDet);
                            }


                            DataTable dtfinal = new DataTable("Stock");
                            str = "select * from Stock where vid='" + vid+"'";
                            Sqlda = new SqlDataAdapter(str, SqlCnnDest);
                            dtfinal.Clear();
                            Sqlda.Fill(dtfinal);
                            // Database.GetSqlData("select * from Stock where vid=" + vid, dtfinal);
                            DataTable dtJou = new DataTable("Journal");
                            str = "select * from Journal where vi_id='" + vid+"'";
                            Sqlda = new SqlDataAdapter(str, SqlCnnDest);
                            dtJou.Clear();
                            Sqlda.Fill(dtJou);
                            if (dtfinal.Rows.Count > 0)
                            {
                                for (int m = 0; m < dtfinal.Rows.Count; m++)
                                {
                                    dtfinal.Rows[m].Delete();
                                }

                                // Database.SaveData(dtfinal);
                                Sqlda = new SqlDataAdapter("select * from " + dtfinal.TableName, SqlCnnDest);
                                cb = new SqlCommandBuilder();
                                cb.QuotePrefix = "[";
                                cb.QuoteSuffix = "]";
                                cb.DataAdapter = Sqlda;
                                Sqlda.Update(dtfinal);

                                dtfinal = new DataTable("Stock");
                                str = "select * from Stock where vid='" + vid+"'";
                                Sqlda = new SqlDataAdapter(str, SqlCnnDest);
                                dtfinal.Clear();
                                Sqlda.Fill(dtfinal);
                                //   Database.GetSqlData("select * from Stock where vid=" + vid, dtfinal);
                            }
                            if (dtJou.Rows.Count > 0)
                            {
                                for (int m = 0; m < dtJou.Rows.Count; m++)
                                {
                                    dtJou.Rows[m].Delete();
                                }

                                // Database.SaveData(dtfinal);
                                Sqlda = new SqlDataAdapter("select * from " + dtJou.TableName, SqlCnnDest);
                                cb = new SqlCommandBuilder();
                                cb.QuotePrefix = "[";
                                cb.QuoteSuffix = "]";
                                cb.DataAdapter = Sqlda;
                                Sqlda.Update(dtJou);

                                dtJou = new DataTable("Journal");
                                str = "select * from Journal where vi_id='" + vid+"'";
                                Sqlda = new SqlDataAdapter(str, SqlCnnDest);
                                dtJou.Clear();
                                Sqlda.Fill(dtJou);

                                //   Database.GetSqlData("select * from Stock where vid=" + vid, dtfinal);
                            }
                            //voucherDetails

                            Feature.Available("Type of Discount1");
                            string qdtype = Feature.Available("Type of Discount1");
                            string cdtype = Feature.Available("Type of Discount2");
                            string fdtype = Feature.Available("Type of Discount3");

                            for (int j = 0; j < dtitems.Rows.Count; j++)
                            {
                                DataTable dtdes = new DataTable();
                                str = "select * from Description where Des_id='" + dtitems.Rows[j]["Did"].ToString()+"'";
                                Sqlda = new SqlDataAdapter(str, SqlCnnSource);
                                Sqlda.Fill(dtdes);
                                string desname = dtdes.Rows[0]["description"].ToString();
                                string packname = dtdes.Rows[0]["Pack"].ToString();

                                fix = "Select des_id from Description where description='" + desname + "' and Pack='" + packname + "'";
                                Sqlcomm = new SqlCommand(fix, SqlCnnDest);
                                string desid = "";
                                if (Sqlcomm.ExecuteScalar() != null)
                                {
                                    desid = Sqlcomm.ExecuteScalar().ToString();

                                }


                                fix = "Select Tax_Cat_id from Description where des_id='" + desid+"'";
                                Sqlcomm = new SqlCommand(fix, SqlCnnDest);
                                string taxcatid = "";
                                if (Sqlcomm.ExecuteScalar() != null)
                                {
                                    taxcatid = Sqlcomm.ExecuteScalar().ToString();

                                }
                                if (desid != "")
                                {
                                    dtVoucherDet.Rows.Add();
                                    dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["vi_id"] = vid;
                                    
                                    dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["LocationId"] = Database.LocationId;

                                    dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["Itemsr"] = j + 1;

                                    //    string des = Database.GetScalarText("SELECT Description FROM Description WHERE Des_id = " + int.Parse(dtitems.Rows[j]["Did"].ToString()) + "");

                                    dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["Description"] = desname;
                                    dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["Quantity"] = dtitems.Rows[j]["stock"].ToString();
                                    dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["comqty"] = dtitems.Rows[j]["stock"].ToString();
                                    //dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["Quantity"] = dtitems.Rows[j]["Receive"].ToString();
                                    //dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["comqty"] = dtitems.Rows[j]["Receive"].ToString();
                                    decimal rate = 0;
                                    if (decimal.Parse((dtitems.Rows[j]["Amount"].ToString())) != 0)
                                    {
                                        rate = decimal.Parse((dtitems.Rows[j]["Amount"].ToString())) / decimal.Parse((dtitems.Rows[j]["Stock"].ToString()));
                                    }

                                    dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["Rate_am"] = rate;
                                    dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["Amount"] = double.Parse(dtitems.Rows[j]["Amount"].ToString());
                                    //dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["Amount"] = dtitems.Rows[j]["ReceiveAmt"].ToString();
                                    dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["Des_ac_id"] = desid;
                                    dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["Category_Id"] = taxcatid;
                                    dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["Taxabelamount"] = double.Parse(dtitems.Rows[j]["Amount"].ToString());
                                    //dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["Taxabelamount"] = dtitems.Rows[j]["ReceiveAmt"].ToString();
                                    dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["Rvi_id"] = "0";
                                    dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["RItemsr"] = "0";
                                    dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["Batch_Code"] ="";
                                    dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["weight"] =0 ;
                                    dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["Cost"] = rate;
                                    dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["MRP"] = 0;
                                    dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["Commission%"] = "0";
                                    dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["packing"] = packname;
                                    dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["orgpacking"] = packname;
                                    dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["pvalue"] = dtdes.Rows[0]["Pvalue"].ToString();
                                    dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["Rate_unit"] = dtdes.Rows[0]["Rate_Unit"].ToString();
                                    dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["remark1"] = "";
                                    dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["remark2"] = "";
                                    dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["remark3"] = "";
                                    dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["remark4"] = "";
                                    dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["remarkreq"] = "false";
                                    dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["Type"] = 0;
                                    dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["flatdis"] = 0;
                                    dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["godown_id"] = dt1.Rows[i][0].ToString();
                                    dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["qd"] = 0;
                                    dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["cd"] = 0;
                                    dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["RCMac_id"] = "0";

                                    dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["Cost"] = rate;

                                    dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["Commission@"] = 0;

                                    fix = "Select pa from taxcategory where Category_Id='" + taxcatid+"'";
                                    Sqlcomm = new SqlCommand(fix, SqlCnnDest);
                                    string pa ="";

                                    if (Sqlcomm.ExecuteScalar() != null)
                                    {
                                        pa = Sqlcomm.ExecuteScalar().ToString();
                                    }

                                    //new fields
                                    dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["pur_sale_acc"] = pa;
                                    dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["tax1"] = 0;
                                    dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["tax2"] = 0;
                                    dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["tax3"] = 0;
                                    dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["tax4"] = 0;
                                    dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["rate1"] = 0;
                                    dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["rate2"] = 0;
                                    dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["rate3"] = 0;
                                    dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["rate4"] = 0;
                                   
                                    dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["taxamt1"] = 0;
                                    dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["taxamt2"] = 0;
                                    dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["taxamt3"] = 0;
                                    dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["taxamt4"] = 0;
                                   
                                    dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["bottomdis"] = 0;
                                    dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["Amount0"] = double.Parse(dtitems.Rows[j]["Amount"].ToString());

                                    //dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["Amount0"] = dtitems.Rows[j]["ReceiveAmt"].ToString();
                                    dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["QDType"] = qdtype;
                                    dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["QDAmount"] = 0;
                                    dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["Amount1"] = 0;

                                    //dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["Amount1"] = dtitems.Rows[j]["ReceiveAmt"].ToString();

                                    dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["CDType"] = cdtype;
                                    dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["CDAmount"] = 0;
                                    dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["Amount2"] = 0;

                                    //dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["Amount2"] = dtitems.Rows[j]["ReceiveAmt"].ToString();

                                    dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["FDType"] = fdtype;
                                    dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["FDAmount"] = 0;
                                    dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["Amount3"] = 0;
                                    //dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["Amount3"] = dtitems.Rows[j]["ReceiveAmt"].ToString();
                                    dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["GridDis"] = 0;
                                    dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["TotalDis"] = 0;
                                    dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["Amount4"] = 0;
                                    //dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["Amount4"] = dtitems.Rows[j]["ReceiveAmt"].ToString();
                                    dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["TotTaxPer"] = 0;
                                    dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["TotTaxAmount"] = 0;
                                    dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["Amount5"] = 0;
                                    //dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["Amount5"] = dtitems.Rows[j]["ReceiveAmt"].ToString();
                                    dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["ExpAmount"] = 0;
                                    dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["SQUARE_FT"] = dtdes.Rows[0]["SQUARE_FT"].ToString();
                                    dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["SQUARE_MT"] = dtdes.Rows[0]["SQUARE_MT"].ToString();
                               
                                    //stock
                                    dtfinal.Rows.Add();
                                    dtfinal.Rows[dtfinal.Rows.Count - 1]["Vid"] = vid;
                                    dtfinal.Rows[dtfinal.Rows.Count - 1]["Itemsr"] = j + 1;
                                    dtfinal.Rows[dtfinal.Rows.Count - 1]["Did"] = desid;
                                    dtfinal.Rows[dtfinal.Rows.Count - 1]["Issue"] = 0;
                                 
                                    dtfinal.Rows[dtfinal.Rows.Count - 1]["IssueAmt"] = 0;
                                    dtfinal.Rows[dtfinal.Rows.Count - 1]["Receive"] = dtitems.Rows[j]["stock"].ToString();
                                    dtfinal.Rows[dtfinal.Rows.Count - 1]["marked"] = true;
                               
                                  
                                   
                                    dtfinal.Rows[dtfinal.Rows.Count - 1]["Batch_no"] = "";
                                    dtfinal.Rows[dtfinal.Rows.Count - 1]["ReceiveAmt"] = double.Parse(dtitems.Rows[j]["Amount"].ToString());
                                    dtfinal.Rows[dtfinal.Rows.Count - 1]["godown_id"] = dt1.Rows[i][0].ToString();
                                    dtfinal.Rows[dtfinal.Rows.Count - 1]["marked"] =Database.IsKacha;
                                    dtfinal.Rows[dtfinal.Rows.Count - 1]["Branch_id"] = Database.BranchId;
                                    dtfinal.Rows[dtfinal.Rows.Count - 1]["LocationId"] = Database.LocationId;

                                   

                                    //Journal

                                    dtJou.Rows.Add();
                                    dtJou.Rows[dtJou.Rows.Count - 1]["Vi_id"] = vid;
                                    dtJou.Rows[dtJou.Rows.Count - 1]["Vdate"] = dt.ToString();
                                    dtJou.Rows[dtJou.Rows.Count - 1]["Ac_id"] = pa;
                                    dtJou.Rows[dtJou.Rows.Count - 1]["Narr"] = "Opening Stock";
                                    dtJou.Rows[dtJou.Rows.Count - 1]["Sno"] = j + 1;
                                    dtJou.Rows[dtJou.Rows.Count - 1]["Amount"] = double.Parse(dtitems.Rows[j]["Amount"].ToString()); ;
                                    dtJou.Rows[dtJou.Rows.Count - 1]["Opp_acid"] = 0;
                                    dtJou.Rows[dtJou.Rows.Count - 1]["Narr2"] = "Opening Stock";
                                    dtJou.Rows[dtJou.Rows.Count - 1]["Reffno"] = "";
                                    dtJou.Rows[dtJou.Rows.Count - 1]["LocationId"] = Database.LocationId;
                                    if (Database.IsKacha == false)
                                    {
                                        dtJou.Rows[dtJou.Rows.Count - 1]["A"] = true;
                                        dtJou.Rows[dtJou.Rows.Count - 1]["B"] = false;
                                        dtJou.Rows[dtJou.Rows.Count - 1]["AB"] = true;
                                    }
                                    else
                                    {
                                        dtJou.Rows[dtJou.Rows.Count - 1]["A"] =false;
                                        dtJou.Rows[dtJou.Rows.Count - 1]["B"] = true;
                                        dtJou.Rows[dtJou.Rows.Count - 1]["AB"] = true;
                                    }
                                }
                            }

                            // Database.SaveData(dtVoucherDet);
                            Sqlda = new SqlDataAdapter("select * from " + dtVoucherDet.TableName, SqlCnnDest);
                            cb = new SqlCommandBuilder();
                            cb.QuotePrefix = "[";
                            cb.QuoteSuffix = "]";
                            cb.DataAdapter = Sqlda;
                            Sqlda.Update(dtVoucherDet);



                            //  Database.SaveData(dtfinal);

                            Sqlda = new SqlDataAdapter("select * from " + dtfinal.TableName, SqlCnnDest);
                            cb = new SqlCommandBuilder();
                            cb.QuotePrefix = "[";
                            cb.QuoteSuffix = "]";
                            cb.DataAdapter = Sqlda;
                            Sqlda.Update(dtfinal);


                            Sqlda = new SqlDataAdapter("select * from " + dtJou.TableName, SqlCnnDest);
                            cb = new SqlCommandBuilder();
                            cb.QuotePrefix = "[";
                            cb.QuoteSuffix = "]";
                            cb.DataAdapter = Sqlda;
                            Sqlda.Update(dtJou);
                        }
                    }







                    //sql = "SELECT CASE WHEN ACCOUNT.Name IS NULL THEN '<MAIN>' ELSE ACCOUNT.Name END AS Godown, Description_3.Description, Description_3.Pack AS Packing, SUM(final.Opening + final.Purchase - final.Sale) AS Stock, SUM(final.OpeningAmt + final.PurchaseAmt - final.SaleAmt) AS Amount, final.Did FROM ACCOUNT RIGHT OUTER JOIN (SELECT 'Opening Balance' AS Type, Did, SUM(Qty) AS Opening, SUM(Amount) AS OpeningAmt, 0 AS Purchase, 0 AS PurchaseAmt, 0 AS Sale, 0 AS SaleAmt,  godown_id FROM (SELECT Stock.Did, SUM(Stock.Issue) * - 1 AS Qty, - (1 * SUM(Stock.IssueAmt)) AS Amount, Stock.godown_id FROM Description RIGHT OUTER JOIN  Stock ON Description.Des_id = Stock.Did LEFT OUTER JOIN  VOUCHERTYPE RIGHT OUTER JOIN  VOUCHERINFO ON VOUCHERTYPE.Vt_id = VOUCHERINFO.Vt_id ON Stock.Vid = VOUCHERINFO.Vi_id WHERE (VOUCHERINFO.Vdate < '01-Jan-2018') AND (Description.StkMaintain = 'True')   GROUP BY Stock.Did, Stock.godown_id  UNION ALL  SELECT Stock_2.Did, SUM(Stock_2.Receive) AS Qty, SUM(Stock_2.ReceiveAmt) AS Amount, Stock_2.godown_id  FROM Description AS Description_2 RIGHT OUTER JOIN  Stock AS Stock_2 ON Description_2.Des_id = Stock_2.Did LEFT OUTER JOIN  VOUCHERTYPE AS VOUCHERTYPE_2 RIGHT OUTER JOIN  VOUCHERINFO AS VOUCHERINFO_2 ON VOUCHERTYPE_2.Vt_id = VOUCHERINFO_2.Vt_id ON   Stock_2.Vid = VOUCHERINFO_2.Vi_id  WHERE (VOUCHERINFO_2.Vdate < '01-Jan-2018') AND (Description_2.StkMaintain = 'true')  GROUP BY Stock_2.Did, Stock_2.godown_id) AS opn  GROUP BY Did, godown_id  UNION ALL  SELECT '' AS Type, Stock_1.Did, 0 AS Opening, 0 AS OpeningAmt, SUM(Stock_1.Receive) AS Purchase, SUM(Stock_1.ReceiveAmt) AS PurchaseAmt,   SUM(Stock_1.Issue) AS Sale, SUM(Stock_1.IssueAmt) AS SaleAmt, Stock_1.godown_id  FROM Description AS Description_1 RIGHT OUTER JOIN  Stock AS Stock_1 ON Description_1.Des_id = Stock_1.Did RIGHT OUTER JOIN  VOUCHERINFO AS VOUCHERINFO_1 LEFT OUTER JOIN  VOUCHERTYPE AS VOUCHERTYPE_1 ON VOUCHERINFO_1.Vt_id = VOUCHERTYPE_1.Vt_id ON Stock_1.Vid = VOUCHERINFO_1.Vi_id  WHERE (VOUCHERINFO_1.Vdate >= '01-Jan-2018') AND  Description_1.StkMaintain = 'true' AND   (VOUCHERINFO_1.Vdate <= '31-Mar-2018')  GROUP BY Stock_1.Did, Stock_1.godown_id) AS final ON ACCOUNT.Ac_id = final.godown_id LEFT OUTER JOIN  Description AS Description_3 ON final.Did = Description_3.Des_id  GROUP BY Description_3.Description, Description_3.Pack, final.Did, ACCOUNT.Name ORDER BY Description_3.Description ";

                    //Sqlda = new SqlDataAdapter(sql, SqlCnnSource);
                    //dtp.Clear();
                    //Sqlda.Fill(dtp);


                   
                    //if (dtp.Rows.Count > 0)
                    //{
                    //    string vtid = "";
                    //    str = "select vt_id from Vouchertype where name='Opening Stock'";
                    //    Sqlcomm = new SqlCommand(str, SqlCnnDest);
                    //    if (Sqlcomm.ExecuteScalar() != null)
                    //    {
                    //        //vtid = int.Parse(Sqlcomm.ExecuteScalar().ToString());
                    //        vtid = Sqlcomm.ExecuteScalar().ToString();
                    //    }

                    //    DataTable dtstkvo = new DataTable();

                    //    str = "Select Vi_id from Voucherinfo where Vt_id='" + vtid + "' ";
                    //    Sqlda = new SqlDataAdapter(str, SqlCnnDest);
                    //    dtstkvo.Clear();
                    //    Sqlda.Fill(dtstkvo);

                    //    DataTable dtmarked = new DataTable();
                    //    str = "SELECT COUNT(*) FROM Stock WHERE (marked = '" + Database.IsKacha + "')";
                    //    Sqlda = new SqlDataAdapter(str, SqlCnnDest);
                    //    dtmarked.Clear();
                    //    Sqlda.Fill(dtmarked);

                    //    if (dtstkvo.Rows.Count != 0)
                    //        {
                    //            DialogResult chk = MessageBox.Show("You will be deleted all Opening Stock data", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                    //            if (chk == DialogResult.Yes)
                    //            {
                    //                for (int l = 0; l < dtstkvo.Rows.Count; l++)
                    //                {
                    //                    string strdelvo = "Delete from Voucherinfo where Vi_id='" + dtstkvo.Rows[l]["Vi_id"].ToString() + "' ";
                    //                    Sqlcomm = new SqlCommand(strdelvo, SqlCnnDest);
                    //                    Sqlcomm.ExecuteNonQuery();


                    //                    string strdelvod = "Delete from Voucherdet where Vi_id='" + dtstkvo.Rows[l]["Vi_id"].ToString() + "' ";
                    //                    Sqlcomm = new SqlCommand(strdelvod, SqlCnnDest);
                    //                    Sqlcomm.ExecuteNonQuery();


                    //                    string strdelstk = "Delete from Stock where Vid='" + dtstkvo.Rows[l]["Vi_id"].ToString() + "' ";
                    //                    Sqlcomm = new SqlCommand(strdelstk, SqlCnnDest);
                    //                    Sqlcomm.ExecuteNonQuery();
                    //                }
                    //            }
                    //            else
                    //            {
                    //                return;
                    //            }
                    //        }

                    //    str = "Select Vi_id from Voucherinfo where Vt_id='" + vtid + "' ";
                    //    Sqlda = new SqlDataAdapter(str, SqlCnnDest);
                    //    dtstkvo.Clear();
                    //    Sqlda.Fill(dtstkvo);

                    //    DateTime dt = sourceend;
                    //    DataTable dt1 = dtp.DefaultView.ToTable(true, "godown");

                    //    for (int i = 0; i < dt1.Rows.Count; i++)
                    //    {
                           
                    //        DataTable dtitems = new DataTable();
                    //        sql = "SELECT CASE WHEN ACCOUNT.Name IS NULL THEN '<MAIN>' ELSE ACCOUNT.Name END AS Godown, Description_3.Description, Description_3.Pack AS Packing, SUM(final.Opening + final.Purchase - final.Sale) AS Stock, SUM(final.OpeningAmt + final.PurchaseAmt - final.SaleAmt) AS Amount, final.Did FROM ACCOUNT RIGHT OUTER JOIN (SELECT 'Opening Balance' AS Type, Did, SUM(Qty) AS Opening, SUM(Amount) AS OpeningAmt, 0 AS Purchase, 0 AS PurchaseAmt, 0 AS Sale, 0 AS SaleAmt,  godown_id FROM (SELECT Stock.Did, SUM(Stock.Issue) * - 1 AS Qty, - (1 * SUM(Stock.IssueAmt)) AS Amount, Stock.godown_id FROM Description RIGHT OUTER JOIN  Stock ON Description.Des_id = Stock.Did LEFT OUTER JOIN  VOUCHERTYPE RIGHT OUTER JOIN  VOUCHERINFO ON VOUCHERTYPE.Vt_id = VOUCHERINFO.Vt_id ON Stock.Vid = VOUCHERINFO.Vi_id WHERE (VOUCHERINFO.Vdate < '01-Jan-2018') AND (Description.StkMaintain = 'True')  GROUP BY Stock.Did, Stock.godown_id  UNION ALL  SELECT Stock_2.Did, SUM(Stock_2.Receive) AS Qty, SUM(Stock_2.ReceiveAmt) AS Amount, Stock_2.godown_id  FROM Description AS Description_2 RIGHT OUTER JOIN  Stock AS Stock_2 ON Description_2.Des_id = Stock_2.Did LEFT OUTER JOIN  VOUCHERTYPE AS VOUCHERTYPE_2 RIGHT OUTER JOIN  VOUCHERINFO AS VOUCHERINFO_2 ON VOUCHERTYPE_2.Vt_id = VOUCHERINFO_2.Vt_id ON   Stock_2.Vid = VOUCHERINFO_2.Vi_id  WHERE (VOUCHERINFO_2.Vdate < '01-Jan-2018') AND (Description_2.StkMaintain = 'true')  GROUP BY Stock_2.Did, Stock_2.godown_id) AS opn  GROUP BY Did, godown_id  UNION ALL  SELECT '' AS Type, Stock_1.Did, 0 AS Opening, 0 AS OpeningAmt, SUM(Stock_1.Receive) AS Purchase, SUM(Stock_1.ReceiveAmt) AS PurchaseAmt,   SUM(Stock_1.Issue) AS Sale, SUM(Stock_1.IssueAmt) AS SaleAmt, Stock_1.godown_id  FROM Description AS Description_1 RIGHT OUTER JOIN  Stock AS Stock_1 ON Description_1.Des_id = Stock_1.Did RIGHT OUTER JOIN  VOUCHERINFO AS VOUCHERINFO_1 LEFT OUTER JOIN  VOUCHERTYPE AS VOUCHERTYPE_1 ON VOUCHERINFO_1.Vt_id = VOUCHERTYPE_1.Vt_id ON Stock_1.Vid = VOUCHERINFO_1.Vi_id  WHERE (VOUCHERINFO_1.Vdate >= '01-Jan-2018') AND  Description_1.StkMaintain = 'true' AND   (VOUCHERINFO_1.Vdate <= '31-Mar-2018')  GROUP BY Stock_1.Did, Stock_1.godown_id) AS final ON ACCOUNT.Ac_id = final.godown_id LEFT OUTER JOIN  Description AS Description_3 ON final.Did = Description_3.Des_id  GROUP BY Description_3.Description, Description_3.Pack, final.Did,ACCOUNT.Name   HAVING      (CASE WHEN dbo.ACCOUNT.Name IS NULL THEN '<MAIN>' ELSE dbo.ACCOUNT.Name END = '" + dt1.Rows[i][0].ToString() + "') ORDER BY Description_3.Description ";
                    //        Sqlda = new SqlDataAdapter(sql, SqlCnnSource);
                    //        Sqlda.Fill(dtitems);

                    //        if(dtitems.Select("Stock<>0").Length!=0)

                    //        dtitems = dtitems.Select("Stock<>0").CopyToDataTable();

                    //        int nid = 0;
                    //        string vid = "";


                    //        DataTable dtcnt = new DataTable();
                    //        str = "select count(*) from VOUCHERINFO where locationid='" + Database.LocationId + "'";
                    //        Sqlda = new SqlDataAdapter(str, SqlCnnDest);
                    //        dtcnt.Clear();
                    //        Sqlda.Fill(dtcnt);

                    //        if (int.Parse(dtcnt.Rows[0][0].ToString()) == 0)
                    //        {
                    //            nid = 1;
                    //        }
                    //        else
                    //        {
                    //            DataTable dtid = new DataTable();
                    //            str = "select max(Nid) as Nid from VOUCHERINFO where locationid='" + Database.LocationId + "'";
                    //            Sqlda = new SqlDataAdapter(str, SqlCnnDest);
                    //            dtid.Clear();
                    //            Sqlda.Fill(dtid);
                    //            nid = int.Parse(dtid.Rows[0][0].ToString()) + 1;
                    //        }

                    //        vid = Database.LocationId + nid.ToString();

                    //        DataTable dtVoucherInfo = new DataTable("Voucherinfo");
                    //        str = "select * from Voucherinfo where Vi_id='0'";
                    //        Sqlda = new SqlDataAdapter(str, SqlCnnDest);
                    //        dtVoucherInfo.Clear();
                    //        Sqlda.Fill(dtVoucherInfo);

                    //        DataTable dtvno = new DataTable("Voucherinfo");
                    //        str = "select max(vnumber) from Voucherinfo where Vt_id='" + vtid + "'";
                    //        Sqlda = new SqlDataAdapter(str, SqlCnnDest);
                    //        dtvno.Clear();
                    //        Sqlda.Fill(dtvno);

                    //        int vno = 0;

                    //        if (dtvno.Rows[0][0].ToString()!="")
                    //        {
                    //            vno = int.Parse(dtvno.Rows[0][0].ToString()) + 1;
                    //        }
                    //        else
                    //        {
                    //            vno = i + 1;
                    //        }
                             
                    //        //voucherinfo
                    //        dtVoucherInfo.Rows.Add();

                    //        string prefix = "";
                    //        string postfix = "";
                    //        int padding = 0;

                    //        string fix = "Select prefix from Vouchertype where vt_id='" + vtid + "' ";
                    //        Sqlcomm = new SqlCommand(fix, SqlCnnDest);
                    //        if (Sqlcomm.ExecuteScalar() != null)
                    //        {
                    //            prefix = Sqlcomm.ExecuteScalar().ToString();
                    //        }

                    //        fix = "Select postfix from Vouchertype where vt_id='" + vtid + "' ";
                    //        Sqlcomm = new SqlCommand(fix, SqlCnnDest);
                    //        if (Sqlcomm.ExecuteScalar() != null)
                    //        {
                    //            postfix = Sqlcomm.ExecuteScalar().ToString();

                    //        }

                    //        fix = "Select padding from Vouchertype where vt_id='" + vtid + "' ";
                    //        Sqlcomm = new SqlCommand(fix, SqlCnnDest);
                    //        if (Sqlcomm.ExecuteScalar() != null)
                    //        {
                    //            padding = int.Parse(Sqlcomm.ExecuteScalar().ToString());
                    //        }

                    //        string invoiceno = vno.ToString();
                    //        dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count - 1]["Invoiceno"] = prefix + invoiceno.PadLeft(padding, '0') + postfix;
                    //        dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count - 1]["Vt_id"] = vtid;
                    //        dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count - 1]["Vnumber"] = vno;
                    //        dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count - 1]["user_id"] = Database.user_id;

                    //        if (dt1.Rows[i][0].ToString() == "0.0000")
                    //        {
                    //            dt1.Rows[i][0] = 0;
                    //        }

                    //        dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count - 1]["Nid"] = nid;
                    //        dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count - 1]["Vi_id"] = vid;
                    //        dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count - 1]["Branch_id"] = Database.BranchId;
                    //        dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count - 1]["CreTime"] = System.DateTime.Now.ToString("HH:mm:ss");
                    //        dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count - 1]["ModTime"] = System.DateTime.Now.ToString("HH:mm:ss");

                    //        if (Database.utype.ToUpper() == "USER")
                    //        {
                    //            dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count - 1]["NApproval"] = true;
                    //        }
                    //        else
                    //        {
                    //            dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count - 1]["NApproval"] = false;
                    //        }

                    //        dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count - 1]["Cash_Pending"] = false;
                    //        dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count - 1]["ac_id"] = funs.Select_ac_id(dt1.Rows[i][0].ToString());
                    //        dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count - 1]["Ac_id2"] = "0";
                    //        dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count - 1]["Vdate"] = dt.ToString();
                    //        dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count - 1]["Svdate"] = sourceend.ToString("dd-MMM-yyyy");
                    //        dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count - 1]["Duedate"] = sourceend.ToString("dd-MMM-yyyy");
                    //        dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count - 1]["Narr"] = "Opening Stock";
                    //        dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count - 1]["Reffno"] = "";
                    //        dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count - 1]["TaxableAmount"] = double.Parse(dtitems.Compute("Sum(Amount)", "Stock<>0").ToString());
                    //        dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count - 1]["Totalamount"] = double.Parse(dtitems.Compute("Sum(Amount)", "Stock<>0").ToString());
                    //        dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count - 1]["rate"] = 0;
                    //        dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count - 1]["Roff"] = 0;
                    //        dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count - 1]["Tdtype"] = "true";
                    //        dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count - 1]["DirectChanged"] = "false";
                    //        dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count - 1]["RCM"] = false;
                    //        dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count - 1]["5000Allowed"] = false;
                    //        dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count - 1]["ITC"] = false;
                    //        dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count - 1]["RoffChanged"] = "false";
                    //        dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count - 1]["TaxChanged"] = "false";
                    //        dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count - 1]["Svnum"] = "0";
                    //        dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count - 1]["Transport1"] = "";
                    //        dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count - 1]["Transport2"] = "";
                    //        dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count - 1]["Grno"] = "";
                    //        dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count - 1]["DeliveryAt"] = "";
                    //        dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count - 1]["Transport3"] = "";
                    //        dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count - 1]["Transport4"] = "";
                    //        dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count - 1]["Transport5"] = "";
                    //        dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count - 1]["Transport6"] = "";
                    //        dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count - 1]["ShiptoAddress1"] = "";
                    //        dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count - 1]["ShiptoAddress2"] = "";
                    //        dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count - 1]["ShiptoEmail"] = "";
                    //        dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count - 1]["ShiptoTIN"] = "";
                    //        dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count - 1]["ShiptoPhone"] = "";
                    //        dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count - 1]["ShiptoStateid"] = 0;
                    //        dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count - 1]["Shipto"] = "";
                    //        dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count - 1]["ShiptoPAN"] = "";
                    //        dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count - 1]["ShiptoAadhar"] = "";
                    //        dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count - 1]["FormC"] = "false";
                    //        dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count - 1]["Conn_id"] = 0;
                    //        dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count - 1]["Iscancel"] = false;
                   
                    //        dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count - 1]["LocationId"] = Database.LocationId;

                    //        Sqlda = new SqlDataAdapter("select * from " + dtVoucherInfo.TableName, SqlCnnDest);
                    //        SqlCommandBuilder cb = new SqlCommandBuilder();
                    //        cb.QuotePrefix = "[";
                    //        cb.QuoteSuffix = "]";
                    //        cb.DataAdapter = Sqlda;
                    //        Sqlda.Update(dtVoucherInfo);

                           
                    //        DataTable dtVoucherDet = new DataTable("VOUCHERDET");

                    //        str = "select * from VOUCHERDET where Vi_id='0'";
                    //        Sqlda = new SqlDataAdapter(str, SqlCnnDest);
                    //        dtVoucherDet.Clear();
                    //        Sqlda.Fill(dtVoucherDet);

                    //        if (dtVoucherDet.Rows.Count > 0)
                    //        {
                    //            for (int m = 0; m < dtVoucherDet.Rows.Count; m++)
                    //            {
                    //                dtVoucherDet.Rows[m].Delete();
                    //            }

                    //            //Database.SaveData(dtVoucherDet);
                    //            Sqlda = new SqlDataAdapter("select * from " + dtVoucherDet.TableName, SqlCnnDest);
                    //            cb = new SqlCommandBuilder();
                    //            cb.QuotePrefix = "[";
                    //            cb.QuoteSuffix = "]";
                    //            cb.DataAdapter = Sqlda;
                    //            Sqlda.Update(dtVoucherDet);

                    //            dtVoucherDet = new DataTable("VOUCHERDET");
                    //            str = "select * from VOUCHERDET where vi_id='" + vid + "' ";
                    //            Sqlda = new SqlDataAdapter(str, SqlCnnDest);
                    //            dtVoucherDet.Clear();
                    //            Sqlda.Fill(dtVoucherDet);
                               
                    //        }


                    //        DataTable dtfinal = new DataTable("Stock");
                    //        str = "select * from Stock where vid='" + vid + "' ";
                    //        Sqlda = new SqlDataAdapter(str, SqlCnnDest);
                    //        dtfinal.Clear();
                    //        Sqlda.Fill(dtfinal);
                          
                    //        if (dtfinal.Rows.Count > 0)
                    //        {
                    //            for (int m = 0; m < dtfinal.Rows.Count; m++)
                    //            {
                    //                dtfinal.Rows[m].Delete();
                    //            }

                    //            // Database.SaveData(dtfinal);
                    //            Sqlda = new SqlDataAdapter("select * from " + dtfinal.TableName, SqlCnnDest);
                    //            cb = new SqlCommandBuilder();
                    //            cb.QuotePrefix = "[";
                    //            cb.QuoteSuffix = "]";
                    //            cb.DataAdapter = Sqlda;
                    //            Sqlda.Update(dtfinal);

                    //            dtfinal = new DataTable("Stock");
                    //            str = "select * from Stock where vid='" + vid + "' ";
                    //            Sqlda = new SqlDataAdapter(str, SqlCnnDest);
                    //            dtfinal.Clear();
                    //            Sqlda.Fill(dtfinal);
                    //            //   Database.GetSqlData("select * from Stock where vid=" + vid, dtfinal);
                    //        }

                    //        //voucherDetails
                    //        Feature.Available("Type of Discount1");
                    //        string qdtype = Feature.Available("Type of Discount1");
                    //        string cdtype = Feature.Available("Type of Discount2");
                    //        string fdtype = Feature.Available("Type of Discount3");

                    //        for (int j = 0; j < dtitems.Rows.Count; j++)
                    //        {
                    //            DataTable dtdes = new DataTable();
                    //            str = "select * from Description where Des_id='" + dtitems.Rows[j]["Did"].ToString() + "' ";
                    //            Sqlda = new SqlDataAdapter(str, SqlCnnSource);
                    //            Sqlda.Fill(dtdes);


                    //            string desname = dtdes.Rows[0]["description"].ToString();
                    //            string packname = dtdes.Rows[0]["Pack"].ToString();

                    //            fix = "Select des_id from Description where description='" + desname + "' and Pack='" + packname + "'";
                    //            Sqlcomm = new SqlCommand(fix, SqlCnnDest);
                    //            string desid = "";

                    //            if (Sqlcomm.ExecuteScalar() != null)
                    //            {
                    //                desid = Sqlcomm.ExecuteScalar().ToString();
                    //            }

                    //            if (desid == "")
                    //            {
                    //                continue;
                    //            }
                    //            // Database.GetSqlData("select * from Description where Des_id=" + dtitems.Rows[j]["Did"].ToString() + "", dtdes);

                    //            fix = "Select Tax_Cat_id from Description where des_id='" + desid+"' ";
                    //            Sqlcomm = new SqlCommand(fix, SqlCnnDest);
                    //            string taxcatid = "";
                    //            if (Sqlcomm.ExecuteScalar() != null)
                    //            {
                    //                taxcatid = Sqlcomm.ExecuteScalar().ToString();
                    //            }

                    //            dtVoucherDet.Rows.Add();
                    //            dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["vi_id"] = vid;
                    //            dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["LocationId"] = Database.LocationId;
                    //            dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["Itemsr"] = j + 1;
                    //            dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["Description"] = desname;
                    //            dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["Quantity"] = dtitems.Rows[j]["Stock"].ToString();
                    //            dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["comqty"] = dtitems.Rows[j]["Stock"].ToString();


                    //            decimal rate = 0;
                    //            if (decimal.Parse((dtitems.Rows[j]["Amount"].ToString())) != 0)
                    //            {
                    //                rate = decimal.Parse((dtitems.Rows[j]["Amount"].ToString())) / decimal.Parse((dtitems.Rows[j]["Stock"].ToString()));
                    //            }

                    //            dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["Rate_am"] = rate;
                    //            dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["Amount"] = double.Parse(dtitems.Rows[j]["Amount"].ToString());
                                
                    //            dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["Des_ac_id"] = desid;
                    //            dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["Category_Id"] = taxcatid;
                    //            dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["Taxabelamount"] = double.Parse(dtitems.Rows[j]["Amount"].ToString());
                                
                    //            dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["Rvi_id"] = "0";
                    //            dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["RItemsr"] = "0";
                    //            dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["Batch_Code"] = "";
                    //            dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["Commission%"] = "0";
                    //            dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["packing"] = packname;
                    //            dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["orgpacking"] = packname;
                    //            dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["pvalue"] = dtdes.Rows[0]["Pvalue"].ToString();
                    //            dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["Rate_unit"] = dtdes.Rows[0]["Rate_Unit"].ToString();
                    //            dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["remark1"] = "";
                    //            dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["remark2"] = "";
                    //            dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["remark3"] = "";
                    //            dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["remark4"] = "";
                    //            dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["remarkreq"] = "false";
                    //            dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["Type"] = 0;
                    //            dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["flatdis"] = 0;
                                
                    //            dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["godown_id"] = funs.Select_ac_id(dt1.Rows[i][0].ToString());
                    //            dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["qd"] = 0;
                    //            dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["cd"] = 0;
                    //            dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["dattype"] = "";
                    //            dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["datamount"] = 0;
                    //            dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["dat"] = 0;
                    //            dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["RCMac_id"] = "0";
                    //            dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["weight"] = 0;
                    //            dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["Cost"] = rate;
                    //            dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["MRP"] = dtdes.Rows[0]["MRP"];
                    //            dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["Commission@"] = 0;

                    //            fix = "Select PA from taxcategory where Category_Id='" + taxcatid+"' ";
                    //            Sqlcomm = new SqlCommand(fix, SqlCnnDest);
                    //            string pa = "";
                    //            if (Sqlcomm.ExecuteScalar() != null)
                    //            {
                    //                pa = Sqlcomm.ExecuteScalar().ToString();

                    //            }
                    //            //new fields
                    //            dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["pur_sale_acc"] = pa;
                    //            dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["tax1"] = 0;
                    //            dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["tax2"] = 0;
                    //            dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["tax3"] = 0;
                    //            dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["tax4"] = 0;
                    //            dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["rate1"] = 0;
                    //            dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["rate2"] = 0;
                    //            dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["rate3"] = 0;
                    //            dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["rate4"] = 0;
                    //            dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["taxamt1"] = 0;
                    //            dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["taxamt2"] = 0;
                    //            dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["taxamt3"] = 0;
                    //            dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["taxamt4"] = 0;
                    //            dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["bottomdis"] = 0;
                    //            dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["Amount0"] = double.Parse(dtitems.Rows[j]["Amount"].ToString());
                                
                    //            dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["QDType"] = qdtype;
                    //            dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["QDAmount"] = 0;
                    //            dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["Amount1"] = 0;
                                
                    //            dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["CDType"] = cdtype;
                    //            dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["CDAmount"] = 0;
                    //            dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["Amount2"] = 0;
                                
                    //            dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["FDType"] = fdtype;
                    //            dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["FDAmount"] = 0;
                    //            dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["Amount3"] = 0;
                                
                    //            dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["GridDis"] = 0;
                    //            dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["TotalDis"] = 0;
                    //            dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["Amount4"] = 0;
                                
                    //            dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["TotTaxPer"] = 0;
                    //            dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["TotTaxAmount"] = 0;
                    //            dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["Amount5"] = 0;
                                
                    //            dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["ExpAmount"] = 0;


                    //            dtfinal.Rows.Add();
                    //            dtfinal.Rows[dtfinal.Rows.Count - 1]["Vid"] = vid;
                    //            dtfinal.Rows[dtfinal.Rows.Count - 1]["Itemsr"] = j + 1;
                    //            dtfinal.Rows[dtfinal.Rows.Count - 1]["Did"] = desid;
                    //            dtfinal.Rows[dtfinal.Rows.Count - 1]["Issue"] = 0;
                    //            dtfinal.Rows[dtfinal.Rows.Count - 1]["IssueAmt"] = 0;
                    //            dtfinal.Rows[dtfinal.Rows.Count - 1]["Receive"] = dtitems.Rows[j]["Stock"].ToString();
                    //            dtfinal.Rows[dtfinal.Rows.Count - 1]["ReceiveAmt"] = double.Parse(dtitems.Rows[j]["Amount"].ToString());
                    //            dtfinal.Rows[dtfinal.Rows.Count - 1]["godown_id"] = funs.Select_ac_id(dt1.Rows[i][0].ToString());
                    //            dtfinal.Rows[dtfinal.Rows.Count - 1]["marked"] = true;
                    //            dtfinal.Rows[dtfinal.Rows.Count - 1]["Batch_no"] = "";
                    //            dtfinal.Rows[dtfinal.Rows.Count - 1]["LocationId"] = Database.LocationId;
                    //            dtfinal.Rows[dtfinal.Rows.Count - 1]["Branch_id"] = Database.BranchId;
                    //        }

                    //        // Database.SaveData(dtVoucherDet);
                    //        Sqlda = new SqlDataAdapter("select * from " + dtVoucherDet.TableName, SqlCnnDest);
                    //        cb = new SqlCommandBuilder();
                    //        cb.QuotePrefix = "[";
                    //        cb.QuoteSuffix = "]";
                    //        cb.DataAdapter = Sqlda;
                    //        Sqlda.Update(dtVoucherDet);



                    //        //  Database.SaveData(dtfinal);

                    //        Sqlda = new SqlDataAdapter("select * from " + dtfinal.TableName, SqlCnnDest);
                    //        cb = new SqlCommandBuilder();
                    //        cb.QuotePrefix = "[";
                    //        cb.QuoteSuffix = "]";
                    //        cb.DataAdapter = Sqlda;
                    //        Sqlda.Update(dtfinal);
                    //    }
                    //}




                }                
            }
            MessageBox.Show("Transfered successfully.");
            this.Close();
            this.Dispose();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            ansGridView3.Rows.Clear();
            ansGridView3.Columns.Clear();
            tabControl1.SelectedIndex = 0;
          
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
          
        }

        private void button8_Click(object sender, EventArgs e)
        {
            ansGridView4.Rows.Clear();
            ansGridView4.Columns.Clear();
            tabControl1.SelectedIndex = 1;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        private void ansGridView3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button4_Click(sender, e);
            }
        }

        private void ansGridView4_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button6_Click(sender, e);
            }
        }

        private void frmBalTransfer_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2 || (e.Control && e.KeyCode == Keys.W))
            {
                button6_Click(sender, e);
            }
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{tab}");
            }
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
                this.Dispose();

            }
        }
    }
}
