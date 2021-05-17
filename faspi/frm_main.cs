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
using Microsoft.Office.Core;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Common;

using System.Data.SqlClient;
using System.Net.Mail;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using System.IO.Compression;
using System.Web.Script.Serialization;



namespace faspi
{
    public partial class frm_main : Form
    {
        public static DataTable dtDisplay1 = new DataTable();
        public static DataTable dtDisplay2 = new DataTable();
        public int random;
        public string createledger = "";
        FlowLayoutPanel flp;
        List<UsersFeature> permission;
        public ToolStripProgressBar ProgrBar = new ToolStripProgressBar();

        public frm_main()
        {
            InitializeComponent();
        }

        private void brokerReportCustomerWiseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Report gg = new Report();
            //string strCombo = "SELECT ACCOUNT.Name FROM  ACCOUNT LEFT OUTER JOIN  ACCOUNTYPE ON ACCOUNT.Act_id = ACCOUNTYPE.Act_id WHERE (ACCOUNTYPE.Name = 'Agent') AND (ACCOUNT.Branch_id = '" + Database.BranchId + "') ORDER BY ACCOUNT.Name";
            //char cg = 'a';
            //string selected = SelectCombo.ComboKeypress(this, cg, strCombo, "", 1);
            //gg.BrokerDetailCustomerWise(Database.stDate, Database.enDate, selected);
            //gg.MdiParent = this;
            //gg.Show();
        }

        private void accountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmMaster frm = new frmMaster();
            frm.MdiParent = this;
            frm.ProgrBar = toolStripProgressBar1;
            frm.LoadData("Account", "Account");
            frm.WindowState = FormWindowState.Maximized;
            frm.Show();
        }

        private void journalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DOSReport gg = new DOSReport();
            gg.Journal("abc", DateTime.Parse("15-July-2016"), DateTime.Parse("15-July-2016"));
        }

        private void cashBookToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report gg = new Report();
            gg.CashBook(Database.stDate, Database.ldate);
            gg.MdiParent = this;
            gg.Show();
        }

        private void ledgerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report gg = new Report();
            string strCombo = funs.GetStrComboled("*");
            char cg = 'a';
            string selected = SelectCombo.ComboKeypress(this, cg, strCombo, "", 1);
            if (Feature.Available("Ledger with Remarks") == "Yes")
            {
                gg.LedgerRemark(Database.ldate, Database.ldate, selected);
            }
            else
            {
                gg.LedgerNew(Database.ldate, Database.ldate, selected);
            }
            gg.MdiParent = this;
            gg.Show();
        }

        private void movedAccountSummaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report gg = new Report();
            gg.MovedAccountSummary(Database.stDate, Database.ldate);
            gg.MdiParent = this;
            gg.Show();
        }

        private void balanceSheetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Report gg = new Report();
            //gg.AccountGroupBalance(Database.stDate, Database.ldate);
            //gg.MdiParent = this;
            //gg.Show();

            string strCombo = "SELECT Name FROM OTHER WHERE Type = 'SER17' ORDER BY Name";
            char cg = 'a';
            string selected = SelectCombo.ComboKeypress(this, cg, strCombo, "", 0);


            //if (selected != "")
            //{
                Report gg = new Report();
                gg.AccountGroupBalance(Database.stDate, Database.ldate, selected);
                gg.MdiParent = this;
                gg.Show();
            //}

        }

        private void customerDetailBillWiseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report gg = new Report();
            //string strCombo = funs.GetStrCombo("*");
            string strCombo = funs.GetStrComboled("*");
            char cg = 'a';
            string selected = SelectCombo.ComboKeypress(this, cg, strCombo, "", 1);


            if (selected == "")
            {
                gg.CustomerDetailBillWise(Database.ldate, Database.ldate, selected);
            }
            else
            {
                gg.CustomerDetailBillWise(Database.stDate, Database.ldate, selected);
            }
            gg.MdiParent = this;
            gg.Show();
        }

        private void customerDetailItemWiseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report gg = new Report();
            //string strCombo = funs.GetStrCombo("*");
            string strCombo = funs.GetStrComboled("*");
            char cg = 'a';
            string selected = SelectCombo.ComboKeypress(this, cg, strCombo, "", 1);
            if (selected == "")
            {
                gg.CustomerDetailItemWise(Database.ldate, Database.ldate, selected);
            }
            else
            {
                gg.CustomerDetailItemWise(Database.stDate, Database.ldate, selected);
            }
            gg.MdiParent = this;
            gg.Show();
        }

        private void annexureAToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report gg = new Report();
            gg.AnnexureA(Database.stDate, Database.ldate);
            gg.MdiParent = this;
            gg.Show();
        }

        private void annexureBToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report gg = new Report();
            gg.AnnexureB(Database.stDate, Database.ldate);
            gg.MdiParent = this;
            gg.Show();
        }

        private void unregisteredPurchaseListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report gg = new Report();
            gg.UnRegisteredPurchaseList(Database.stDate, Database.ldate);
            gg.MdiParent = this;
            gg.Show();
        }

        private void commoditySumaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report gg = new Report();
            gg.CommoditySummary(Database.ldate, Database.ldate);
            gg.MdiParent = this;
            gg.Show();
        }

        private void customerBrokerageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report gg = new Report();
            // string strCombo = funs.GetStrCombo("*");
            string strCombo = funs.GetStrComboled("*");
            char cg = 'a';
            string selected = SelectCombo.ComboKeypress(this, cg, strCombo, "", 1);
            gg.CustomerBrokerage(Database.stDate, Database.ldate, selected);
            gg.MdiParent = this;
            gg.Show();
        }

        private void supplierDetailBillWiseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report gg = new Report();



            // string strCombo = funs.GetStrCombo("*");
            string strCombo = funs.GetStrComboled("*");
            char cg = 'a';
            string selected = SelectCombo.ComboKeypress(this, cg, strCombo, "", 1);
            gg.SupplierDetailBillWise(Database.stDate, Database.ldate, selected);
            gg.MdiParent = this;
            gg.Show();
        }


        private void supplierDetailItemWiseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report gg = new Report();
            string strCombo = funs.GetStrComboled("*");
            char cg = 'a';
            string selected = SelectCombo.ComboKeypress(this, cg, strCombo, "", 1);
            gg.SupplierDetailItemWise(Database.stDate, Database.ldate, selected);
            gg.MdiParent = this;
            gg.Show();
        }

        private void modifyRateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frm_updaterates frm = new frm_updaterates();
            frm.MdiParent = this;
            frm.type = "Rates";
            frm.Show();
        }

        private void listOfDescriptionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmCompanyColor frm = new frmCompanyColor();
            frm.MdiParent = this;
            frm.gCap = "List of Description";
            frm.Text = "List of Description";
            frm.Show();
        }

        private void customerSupplierRateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmMaster frm = new frmMaster();
            frm.MdiParent = this;
            frm.LoadData("Customer/Supplier Rate", "Customer/Supplier Rate");
            frm.Show();
        }

        private void eFilingUPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frm_eFiling frm = new frm_eFiling();
            frm.MdiParent = this;
            frm.Show();
        }

        private void inBillChargesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report gg = new Report();
            string strCombo = funs.GetStrComboled("*");
            char cg = 'a';
            string selected = SelectCombo.ComboKeypress(this, cg, strCombo, "", 1);
            gg.InBillCharges(Database.stDate, Database.ldate, selected);
            gg.MdiParent = this;
            gg.Show();
        }

        private void brokerReportItemWiseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report gg = new Report();
            string strCombo = "SELECT name from Contractor order by name";
            char cg = 'a';
            string selected = SelectCombo.ComboKeypress(this, cg, strCombo, "", 1);
            gg.BrokerDetailItemWise(Database.stDate, Database.enDate, selected);
            gg.MdiParent = this;
            gg.Show();
        }

        private void standardTrialBalanceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report gg = new Report();
            gg.StandardTrial(Database.stDate, Database.ldate);
            gg.MdiParent = this;
            gg.Show();
        }

        public void frm_main_Load(object sender, EventArgs e)
        {
            DirectoryInfo dInfo = new System.IO.DirectoryInfo(Application.StartupPath + "\\System");

            if (dInfo.Exists == false)
            {
                Directory.CreateDirectory(Application.StartupPath + "\\System");
            }

            Master.UpdateAll();
            Database.TextCase = Feature.Available("Text Case");

            if (Feature.Available("OTP Based Login") == "Yes")
            {
                string msg = "OTP is: " + random;
                string companycon = Database.GetScalarText("Select Contactno from Company");
                InputBox box = new InputBox("Type OTP", "", false);
                box.outStr = "OTP";
                Database.OTP = random;
                box.ShowInTaskbar = false;
                box.ShowDialog(this);
                // MessageBox.Show("ABC");

                if (funs.isDouble(companycon) == true)
                {
                    if (companycon != "0")
                    {
                        sms objsms = new sms();
                        objsms.send(msg, companycon, "");
                    }
                }
                setMenu();

            }
            else
            {
       
                setUserMenu();
            }

            if (Feature.AvailableLogin("Base Rate") == true)
            {
                DataTable dtMaxRate = new DataTable("baserate");
                Database.GetSqlData("select * from baserate where LoginDate =(SELECT Max(baserate.LoginDate) AS Dt FROM baserate) ", dtMaxRate);
                double rate = 0;
                if (dtMaxRate.Rows.Count > 0)
                {
                    rate = double.Parse(dtMaxRate.Rows[0]["rate"].ToString());
                }
                InputBox input = new InputBox("Enter Base Rate", rate.ToString(), false);
                input.ShowDialog();

                DataTable dtCurrentRate = new DataTable("baserate");
                Database.GetSqlData("select * from baserate where LoginDate=#" + Database.ldate + "#", dtCurrentRate);
                if (dtCurrentRate.Rows.Count == 0)
                {
                    dtCurrentRate.Rows.Add();
                }
                dtCurrentRate.Rows[0]["LoginDate"] = Database.ldate;
                if (input.outStr != "" && input.outStr != null)
                {
                    dtCurrentRate.Rows[0]["rate"] = input.outStr;
                    statusStrip1.Items[11].Text = input.outStr;
                }
                else
                {
                    dtCurrentRate.Rows[0]["rate"] = "0.00";
                }
                Database.SaveData(dtCurrentRate);
                statusStrip1.Items[12].Text = "Base Rate";
            }
            else
            {
                statusStrip1.Items[12].Text = "";
            }

            statusStrip1.Items[0].Text = "Faspi Enterprises Pvt. Ltd.";
            this.Text = Database.fname + "[" + Database.fyear + "]";
            statusStrip1.Items[2].Text = Database.ExeDate.ToString("yy.M.d");
            statusStrip1.Items[4].Text = Database.uname;
            statusStrip1.Items[6].Text = Database.ldate.ToString(Database.dformat);

            statusStrip1.Items[9].Text = "+91 83070 71699";
            statusStrip1.Items[11].Text = Database.fyear;

            statusStrip1.Items["Bname"].Text = funs.Select_branch_name(Database.BranchId);

            mode.Text = "Business";

            if (mode.Text == "Business")
            {
                Database.BMode = "A";
            }
            else if (mode.Text == "Personal")
            {
                Database.BMode = "B";
            }
            else if (mode.Text == "Both")
            {
                Database.BMode = "AB";
            }
            FileInfo fInfo = new FileInfo(Application.StartupPath + "\\System\\" + Database.fname + ".jpg");
            if (fInfo.Exists)
            {
                this.BackgroundImage = new Bitmap(Application.StartupPath + "\\System\\" + Database.fname + ".jpg");
                this.BackgroundImageLayout = ImageLayout.Stretch;
            }

            fInfo = null;
        }

        private void statusStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.ToString() == statusStrip1.Items[0].Text)
            {
                InputBox box = new InputBox("It is for Maintenance purpose and need permission. Please give Maintenance Password", "", true);
                box.ShowInTaskbar = false;
                box.ShowDialog(this);
                String pass = box.outStr;
                if (pass.ToLower() == "admin")
                {
                    frmQuery frm = new frmQuery();
                    frm.MdiParent = this;
                    frm.Show();
                }
                else
                {
                    MessageBox.Show("Access Denied");

                   
                }
            }
        }

        bool IsExit = false;
        private void frm_main_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (IsExit)
            {
                return;
            }

            DialogResult ch = MessageBox.Show(null, "Are you sure to exit?", "Confirm", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (ch != DialogResult.OK)
            {
                e.Cancel = true;
                return;
            }

            string[] files = System.IO.Directory.GetFiles(Application.StartupPath + "\\System", "*.pdf");
            foreach (string file in files)
            {
                try
                {
                    System.IO.File.Delete(file);
                }
                catch (Exception e1) { }
            }

            if (Database.databaseName != "")
            {
                string strBackOption = Feature.Available("Auto Backup").ToUpper();

                if (strBackOption == "YES" || strBackOption == "ASK")
                {
                    if (strBackOption == "ASK")
                    {
                        DialogResult chbackup = MessageBox.Show(null, "Are you want to take Backup of this Firm?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (chbackup == DialogResult.No)
                        {
                            IsExit = true;
                            Application.Exit();
                            return;
                        }
                    }


                    int remainder = int.Parse(DateTime.Now.ToString("dd")) % 2;
                    string strBackType = Feature.Available("Auto Backup Style").ToUpper();
                    if (strBackType == "SMART" || strBackType == "EVEN-ODD")
                    {
                        if (Feature.Available("IP Backup").ToUpper() == "YES")
                        {
                            DownloadBackup obj = new DownloadBackup();
                            obj.strFoldePath = Application.StartupPath + "\\Backup";
                            obj.dbName = Database.databaseName;
                            obj.BackType = strBackType;
                            obj.ShowDialog(this);
                        }
                        else
                        {
                            try
                            {

                                if (strBackType == "SMART")
                                {
                                    string mm = "S" + Database.databaseName + "M" + DateTime.Now.ToString("MM");
                                    string dd = "S" + Database.databaseName + "D" + DateTime.Now.ToString("dd");


                                    Database.CommandExecutor("Backup database " + Database.databaseName + " to disk='" + Application.StartupPath + "\\Backup\\" + mm + "' ", false);
                                    Database.CommandExecutor("Backup database " + Database.databaseName + " to disk='" + Application.StartupPath + "\\Backup\\" + dd + "' ", false);
                                    string filePath = Application.StartupPath + "\\Backup\\" + mm;
                                    string zipfilem = Application.StartupPath + "\\Backup\\" + mm + ".zip";

                                    if (File.Exists(zipfilem) == true)
                                    {
                                        File.Delete(zipfilem);
                                    }
                                    ZipArchive zip = ZipFile.Open(zipfilem, ZipArchiveMode.Create);
                                    zip.CreateEntryFromFile(filePath, Path.GetFileName(filePath));

                                    zip.Dispose();

                                    string filePath2 = Application.StartupPath + "\\Backup\\" + dd;
                                    string zipfiled = Application.StartupPath + "\\Backup\\" + dd + ".zip";

                                    if (File.Exists(zipfiled) == true)
                                    {
                                        File.Delete(zipfiled);
                                    }
                                    ZipArchive zip1 = ZipFile.Open(zipfiled, ZipArchiveMode.Create);
                                    zip1.CreateEntryFromFile(filePath2, Path.GetFileName(filePath2));
                                    zip1.Dispose();
                                    File.Delete(filePath);
                                    File.Delete(filePath2);
                                }
                                else
                                {
                                    string mm = "S" + Database.databaseName + remainder;
                                    Database.CommandExecutor("Backup database " + Database.databaseName + " to disk='" + Application.StartupPath + "\\Backup\\" + mm + "' ", false);

                                    string filePath = Application.StartupPath + "\\Backup\\" + mm;
                                    string zipfilem = Application.StartupPath + "\\Backup\\" + mm + ".zip";

                                    if (File.Exists(zipfilem) == true)
                                    {
                                        File.Delete(zipfilem);
                                    }
                                    ZipArchive zip = ZipFile.Open(zipfilem, ZipArchiveMode.Create);
                                    zip.CreateEntryFromFile(filePath, Path.GetFileName(filePath));

                                    zip.Dispose();
                                    File.Delete(filePath);

                                }

                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Backup Not generated");
                            }

                            //if (strBackType == "SMART")
                            //{
                            //    DataSet ds = new DataSet();
                            //    DataTable dtalltable = new DataTable();

                            //    Database.GetSqlData("SELECT Table_Name as tablename FROM  " + Database.databaseName + ".INFORMATION_SCHEMA.TABLES WHERE  TABLE_TYPE = 'BASE TABLE' order by Table_Name", dtalltable);

                            //    for (int i = 0; i < dtalltable.Rows.Count; i++)
                            //    {
                            //        DataTable dtsingletable = new DataTable(dtalltable.Rows[i][0].ToString());
                            //        Database.GetSqlData("select * from " + dtalltable.Rows[i][0].ToString(), dtsingletable);
                            //        ds.Tables.AddRange(new DataTable[] { dtsingletable });
                            //    }
                            //    // folderBrowserDialog1.ShowDialog();
                            //    string filename = "S" + Database.databaseName + "D" + DateTime.Now.ToString("dd");
                            //    string filePath = Application.StartupPath + "\\Backup\\" + filename;
                            //    ds.WriteXml(filePath);

                            //    string zipfiled = Application.StartupPath + "\\Backup\\S" + Database.databaseName + "D" + DateTime.Now.ToString("dd") + ".zip";

                            //    if (File.Exists(zipfiled) == true)
                            //    {
                            //        File.Delete(zipfiled);
                            //    }
                            //    ZipArchive zip = ZipFile.Open(zipfiled, ZipArchiveMode.Create);
                            //    zip.CreateEntryFromFile(filePath, Path.GetFileName(filePath));

                            //    zip.Dispose();


                            //    string zipfilem = Application.StartupPath + "\\Backup\\S" + Database.databaseName + "M" + DateTime.Now.ToString("MM") + ".zip";

                            //    if (File.Exists(zipfilem) == true)
                            //    {
                            //        File.Delete(zipfilem);
                            //    }
                            //    ZipArchive zipm = ZipFile.Open(zipfilem, ZipArchiveMode.Create);
                            //    zipm.CreateEntryFromFile(filePath, Path.GetFileName(filePath));

                            //    zipm.Dispose();
                            //    File.Delete(filePath);
                            //}
                            //else
                            //{
                            //    DataSet ds = new DataSet();
                            //    DataTable dtalltable = new DataTable();
                            //    Database.GetSqlData("SELECT Table_Name as tablename FROM  " + Database.databaseName + ".INFORMATION_SCHEMA.TABLES WHERE  TABLE_TYPE = 'BASE TABLE' order by Table_Name", dtalltable);
                            //    for (int i = 0; i < dtalltable.Rows.Count; i++)
                            //    {
                            //        DataTable dtsingletable = new DataTable(dtalltable.Rows[i][0].ToString());
                            //        Database.GetSqlData("select * from " + dtalltable.Rows[i][0].ToString(), dtsingletable);
                            //        ds.Tables.AddRange(new DataTable[] { dtsingletable });
                            //    }
                            //    // folderBrowserDialog1.ShowDialog();
                            //    string filename = "S" + Database.databaseName + remainder;

                            //    string filePath = Application.StartupPath + "\\Backup\\" + filename;
                            //    ds.WriteXml(filePath);



                            //    string zipfiled = Application.StartupPath + "\\Backup\\S" + Database.databaseName + remainder + ".zip";

                            //    if (File.Exists(zipfiled) == true)
                            //    {
                            //        File.Delete(zipfiled);
                            //    }
                            //    ZipArchive zip = ZipFile.Open(zipfiled, ZipArchiveMode.Create);
                            //    zip.CreateEntryFromFile(filePath, Path.GetFileName(filePath));

                            //    zip.Dispose();





                            //    File.Delete(filePath);
                            //}
                       
                        }
                    }
                }
            }

            IsExit = true;
            Application.Exit();
        }


        private void balanceSheetToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Report gg = new Report();
            gg.BalanceSheet(Database.stDate, Database.ldate);
            gg.MdiParent = this;
            gg.Show();
        }

        private void openingTrialBalanceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report gg = new Report();
            gg.OpeningTrial(Database.stDate, Database.stDate);
            gg.MdiParent = this;
            gg.Show();
        }

        private void groupedTrialBalanceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report gg = new Report();
            gg.GroupedTrial(Database.stDate, Database.ldate);
            gg.MdiParent = this;
            gg.Show();
        }

        private void profitLossStatementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report gg = new Report();
            gg.ProfitAndLoss(Database.stDate, Database.ldate);
            gg.MdiParent = this;
            gg.Show();
        }

        private void stockSummaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report gg = new Report();
            if (Feature.Available("Company Colour") == "No")
            {
                string godown = "";
                string godownname = "";
                char cg = 'a';
                if (Feature.Available("Multi-Godown") == "Yes")
                {
                    godown = "select distinct '<MAIN>' as name from account union all SELECT ACCOUNT.Name as name FROM ACCOUNT LEFT JOIN ACCOUNTYPE ON ACCOUNT.Act_id = ACCOUNTYPE.Act_id WHERE ACCOUNTYPE.Name='Godown' and Account.Branch_id='" + Database.BranchId + "' GROUP BY ACCOUNT.Name";
                    godownname = SelectCombo.ComboKeypress(this, cg, godown, "", 0);
                }
                gg.Stock(Database.stDate, Database.ldate, godownname);
            }
            else
            {
                string godown = "";
                string godownname = "";
                char cg = 'a';

                if (Feature.Available("Multi-Godown") == "Yes")
                {

                    godown = "select distinct '<MAIN>' as name from account union all SELECT ACCOUNT.Name as name FROM ACCOUNT LEFT JOIN ACCOUNTYPE ON ACCOUNT.Act_id = ACCOUNTYPE.Act_id WHERE ACCOUNTYPE.Name='Godown' and Account.Branch_id='" + Database.BranchId + "' GROUP BY ACCOUNT.Name";
                    godownname = SelectCombo.ComboKeypress(this, cg, godown, "", 0);
                }

                string company = "Select Name from Other where Type='SER14' order by Name";
                string companyname = SelectCombo.ComboKeypress(this, cg, company, "", 0);
                if (Feature.Available("Stock Report in Crosss-Tab") == "No")
                {
                    gg.StockSummary(Database.stDate, Database.ldate, godownname, companyname);
                }
                else
                {
                    gg.StockSummarycross(Database.stDate, Database.ldate, godownname, companyname);
                }
            }
            gg.MdiParent = this;
            gg.Show();

        }

        private void belowStockWarningToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report gg = new Report();
            string strCombo = "Select Name from Other where Type='SER14' order by Name";
            char cg = 'a';
            string selected = SelectCombo.ComboKeypress(this, cg, strCombo, "", 1);
            gg.BelowStockWarning(Database.stDate, Database.ldate, selected);
            gg.MdiParent = this;
            gg.Show();
        }

        private void priceListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report gg = new Report();
            string strCombo = "Select Name from Other where Type='SER14' order by Name";
            char cg = 'a';
            string selected = SelectCombo.ComboKeypress(this, cg, strCombo, "", 0);

            DataTable dtcombo = new DataTable();

            dtcombo.Columns.Add("PriceList", typeof(string));
            if (Feature.Available("Name of PriceList1") != "Purchase Rate")
            {

                dtcombo.Rows.Add();
                dtcombo.Rows[dtcombo.Rows.Count - 1][0] = Feature.Available("Name of PriceList1");
            }

            dtcombo.Rows.Add();
            dtcombo.Rows[dtcombo.Rows.Count-1][0] = Feature.Available("Name of PriceList2");

            dtcombo.Rows.Add();
            dtcombo.Rows[dtcombo.Rows.Count - 1][0] = Feature.Available("Name of PriceList3");

            dtcombo.Rows.Add();
            dtcombo.Rows[dtcombo.Rows.Count - 1][0] = Feature.Available("Name of PriceList4");

            dtcombo.Rows.Add();
            dtcombo.Rows[dtcombo.Rows.Count - 1][0] = Feature.Available("Name of PriceList5");

            dtcombo.Rows.Add();
            dtcombo.Rows[dtcombo.Rows.Count - 1][0] = Feature.Available("Name of PriceList6");


            dtcombo.Rows.Add();
            dtcombo.Rows[dtcombo.Rows.Count - 1][0] = "MRP";
            string ratesid = "";
            string ratesvalue = SelectCombo.ComboDt(this, dtcombo, 0);
            if (ratesvalue != "")
            {
                ratesid = funs.Select_Rates_Id(ratesvalue);
            }
            else
            {
                ratesid = ratesvalue;
            }

            gg.PriceList(Database.stDate, Database.ldate, selected, ratesid);
            gg.MdiParent = this;
            gg.Show();
        }

        private void itemLedgerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Report gg = new Report();
            //string godown = "";
            //string godownname = "";
            //char cg = 'a';
            //if (Feature.Available("Multi-Godown") == "Yes")
            //{
            //    godown = "select distinct '<MAIN>' as name from account union all SELECT ACCOUNT.Name as name FROM ACCOUNT LEFT JOIN ACCOUNTYPE ON ACCOUNT.Act_id = ACCOUNTYPE.Act_id WHERE ACCOUNTYPE.Name='Godown' and Account.Branch_id='"+Database.BranchId+"' GROUP BY ACCOUNT.Name";
            //    godownname = SelectCombo.ComboKeypress(this, cg, godown, "", 0);
            //}
            //string strCombo = "SELECT DESCRIPTION.Description " + access_sql.Concat + " ' (' " + access_sql.Concat + " PACK  As Description  FROM DESCRIPTION WHERE (((DESCRIPTION.StkMaintain)="+ access_sql.Singlequote+"True"+access_sql.Singlequote+")) order by Description,Pack ";
            //string selected = SelectCombo.ComboKeypress(this, cg, strCombo, "", 2);
            //if (selected == "")
            //{
            //    return;
            //}
            //string[] Des_Name = selected.Split('(');
            //string des_id = funs.Select_des_id(Des_Name[0], Des_Name[1]);
            //if (des_id == "")
            //{
            //    return;
            //}
            //gg.ItemLedger(Database.stDate, Database.ldate, godownname, des_id);
            //gg.MdiParent = this;
            //gg.Show();



            frm_itemledger frm = new frm_itemledger();
            frm.Show();
        }

        private void addressPrintingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string addname = SelectCombo.ComboKeypress(this, ' ', "select [name] from account where act_id in ('SER4','SER5','SER11','SER13','SER20','SER22','SER23')", "", 0);
            if (funs.Select_ac_id(addname) != "")
            {
                Report gg = new Report();
                gg.AddressPrinting(addname);
            }
        }

        public static void clearDisplay()
        {
            dtDisplay1.Clear();
            dtDisplay2.Clear();
            dtDisplay1.Columns.Clear();
            dtDisplay2.Columns.Clear();
        }

        public static void clearDisplay1()
        {
            dtDisplay1.Clear();
            dtDisplay1.Columns.Clear();
        }

        public static void clearDisplay2()
        {
            dtDisplay2.Clear();
            dtDisplay2.Columns.Clear();
        }

        private void newFirmToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frm_NewCompany frm = new frm_NewCompany();
            frm.MdiParent = this;
            frm.frmMenuTyp = "New Company";
            frm.LoadData(0, "New Company");
            frm.Show();
        }

        private void databaseBackupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmbackup frm = new frmbackup();
            frm.MdiParent = this;
            frm.frmMenuTyp = "Backup";
            frm.Text = "Backup Firm";
            frm.Show();
        }

        private void deleteFirmToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmbackup frm = new frmbackup();
            frm.MdiParent = this;
            frm.frmMenuTyp = "Delete";
            frm.Text = "Delete Firm";
            frm.Show();
        }

        private void importRateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmImpRate frm = new frmImpRate();
            frm.ProgrBar = toolStripProgressBar1;
            frm.MdiParent = this;
            frm.Show();
        }

        private void modifyItemsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmModifyGroup frm = new frmModifyGroup();
            frm.MdiParent = this;
            frm.Show();
        }

        private void tintingSystemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Search frm = new Search();
            frm.MdiParent = this;
            frm.Show();
        }

        private void activateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Dongle.cllogin(true);
            string dong = Dongle.getDongleNumber();
            String address = "http://www.faspi.in/admin/software/dongleinfo.php?dongle=" + dong;
            // String address = "http://localhost/faspidata/dongleinfo.php?dongle=" + dong;
            WebRequest webRequest = WebRequest.Create(address);
            webRequest.CachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);
            WebResponse webResponse;

            try
            {
                webResponse = webRequest.GetResponse();
            }
            catch (Exception e1)
            {
                MessageBox.Show("Check Your Network Connectivity.");
                return;
            }


            Stream stream = webResponse.GetResponseStream();
            StreamReader reader = new StreamReader(stream, Encoding.UTF8);
            String str = reader.ReadToEnd();

            int stindex = str.IndexOf("<body>") + 6;

            int edindex = str.IndexOf("</body>");

            str = str.Substring(stindex, edindex - stindex).Replace("\n", "");
            str = str.Replace("\t", "");


            if (str.Trim() == "")
            {
                MessageBox.Show("Dongle Information is not Found On Server. Please Contact Your Vendor");
            }
            else
            {
                string[] ar = str.Split('|');
                string DongleNo = ar[0].Trim();
                string CustomerName = ar[1].Trim() + " " + ar[2].Trim();
                string Firmname = ar[3].Trim();
                string AddressFirm = ar[4].Trim();
                string City = ar[5].Trim();
                string Contact = ar[6].Trim();
                DateTime Doa = DateTime.Parse(ar[7].Trim());
                DateTime Doamc = DateTime.Parse(ar[8].Trim());
                string Note = ar[9].Trim();

                // address = "http://localhost/faspidata/currentdate.php";
                address = "http://www.faspi.in/admin/software/currentdate.php";
                webRequest = WebRequest.Create(address);
                webRequest.CachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);
                try
                {
                    webResponse = webRequest.GetResponse();
                }
                catch (Exception e1)
                {
                    MessageBox.Show("Check Your Network Connectivity.");
                    return;
                }

                stream = webResponse.GetResponseStream();
                reader = new StreamReader(stream, Encoding.UTF8);
                str = reader.ReadToEnd();

                DateTime Today = DateTime.Parse(str);
                frmRegistration frm = new frmRegistration();

                frm.LoadData(DongleNo, CustomerName, Firmname, AddressFirm, City, Contact, Doa, Doamc, Today);
                frm.ShowDialog();
                setMenu();
                //  Fillchart();
            }
        }

        private void toolStripMenuItem26_Click(object sender, EventArgs e)
        {
            frm_voutype frm = new frm_voutype();
            frm.MdiParent = this;
            frm.Show();
        }


        public void setUserMenu()
        {
            if (Database.databaseName != "")
            {
               
                statusStrip1.Items[6].Text = Database.ldate.ToString(Database.dformat);
                statusStrip1.Items[11].Text = Database.fyear;
                this.Text = Database.fname;
                statusStrip1.Items[2].Text = Database.ExeDate.ToString("yy.M.d");
                statusStrip1.Items[4].Text = Database.uname;
                setupToolStripMenuItem.Visible = true;
              

            DataTable dtpagerole= new DataTable();
            Database.GetSqlData("SELECT WinPage.PageName, dbo.WinPageRole.Visible FROM  WinPageRole LEFT OUTER JOIN SYS_Role ON dbo.WinPageRole.Role_id = dbo.SYS_Role.Role_ID LEFT OUTER JOIN WinPage ON dbo.WinPageRole.Page_id = dbo.WinPage.PageID WHERE (dbo.SYS_Role.RoleName = '"+ Database.utype+"')", dtpagerole);
            for (int i = 0; i < dtpagerole.Rows.Count; i++)
            {
                if (dtpagerole.Rows[i]["PageName"].ToString() == "")
                {
                    continue;
                }
                ToolStripItem[] obj = menuStrip1.Items.Find(dtpagerole.Rows[i]["PageName"].ToString(),true);
                if (obj.Length > 0)
                {
                    for (int j = 0; j < obj.Length; j++)
                    {
                        obj[0].Visible = bool.Parse(dtpagerole.Rows[i]["Visible"].ToString());
                    }
                }
                else
                {
                    obj = menuStrip2.Items.Find(dtpagerole.Rows[i]["PageName"].ToString(), true);
                    for (int k = 0; k < obj.Length; k++)
                    {
                        obj[0].Visible = bool.Parse(dtpagerole.Rows[i]["Visible"].ToString());
                    }
                }

                if(dtpagerole.Rows[i]["PageName"].ToString()=="accountToolStripMenuItem")
                {
                    accountToolStripMenuItem1.Visible = bool.Parse(dtpagerole.Rows[i]["Visible"].ToString());
                }
                if (dtpagerole.Rows[i]["PageName"].ToString() == "stockItemToolStripMenuItem")
                {
                    stockItemsToolStripMenuItem.Visible = bool.Parse(dtpagerole.Rows[i]["Visible"].ToString());
                } 
                if (dtpagerole.Rows[i]["PageName"].ToString() == "paymentToolStripMenuItem")
                {
                  
                    for(int k=0;k<Master.SideMenu.Rows.Count;k++)
                    {

                        if (Master.SideMenu.Rows[k]["Menuoption"].ToString().ToUpper()  =="PAYMENT")
                        {
                           
                            Master.SideMenu.Rows[k]["Display"] = bool.Parse(dtpagerole.Rows[i]["Visible"].ToString());
                            if (bool.Parse(dtpagerole.Rows[i]["Visible"].ToString()) == false)
                            {
                                paymentToolStripMenuItem.ShowShortcutKeys = false;

                                paymentToolStripMenuItem.ShortcutKeyDisplayString = "";




                                paymentToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.None;
                            }
                        }
                       

                    }
                }

                if (dtpagerole.Rows[i]["PageName"].ToString() == "receiptToolStripMenuItem")
                {

                    for (int k = 0; k < Master.SideMenu.Rows.Count; k++)
                    {

                        if (Master.SideMenu.Rows[k]["Menuoption"].ToString().ToUpper() == "RECEIPT")
                        {

                            Master.SideMenu.Rows[k]["Display"] = bool.Parse(dtpagerole.Rows[i]["Visible"].ToString());
                            if (bool.Parse(dtpagerole.Rows[i]["Visible"].ToString()) == false)
                            {
                                receiptToolStripMenuItem.ShowShortcutKeys = false;
                                receiptToolStripMenuItem.ShortcutKeyDisplayString = "";
                                receiptToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.None;
                            }
                        }
                    }
                }

                if (dtpagerole.Rows[i]["PageName"].ToString() == "purchaseToolStripMenuItem")
                {
                    for (int k = 0; k < Master.SideMenu.Rows.Count; k++)
                    {
                        if (Master.SideMenu.Rows[k]["Menuoption"].ToString().ToUpper() == "PURCHASE")
                        {

                            Master.SideMenu.Rows[k]["Display"] = bool.Parse(dtpagerole.Rows[i]["Visible"].ToString());
                            if (bool.Parse(dtpagerole.Rows[i]["Visible"].ToString()) == false)
                            {
                                purchaseToolStripMenuItem.ShowShortcutKeys = false;
                                purchaseToolStripMenuItem.ShortcutKeyDisplayString = "";
                                purchaseToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.None;
                            }
                        }
                    }
                }


                if (dtpagerole.Rows[i]["PageName"].ToString() == "purchaseReturnToolStripMenuItem")
                {
                    for (int k = 0; k < Master.SideMenu.Rows.Count; k++)
                    {

                        if (Master.SideMenu.Rows[k]["Menuoption"].ToString().ToUpper() == "PURCHASE RETURN")
                        {

                            Master.SideMenu.Rows[k]["Display"] = bool.Parse(dtpagerole.Rows[i]["Visible"].ToString());
                            if (bool.Parse(dtpagerole.Rows[i]["Visible"].ToString()) == false)
                            {
                                purchaseReturnToolStripMenuItem.ShowShortcutKeys = false;
                                purchaseReturnToolStripMenuItem.ShortcutKeyDisplayString = "";
                                purchaseReturnToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.None;
                            }
                        }
                    }
                }

                if (dtpagerole.Rows[i]["PageName"].ToString() == "saleToolStripMenuItem1")
                {

                    for (int k = 0; k < Master.SideMenu.Rows.Count; k++)
                    {

                        if (Master.SideMenu.Rows[k]["Menuoption"].ToString().ToUpper() == "SALE")
                        {

                            Master.SideMenu.Rows[k]["Display"] = bool.Parse(dtpagerole.Rows[i]["Visible"].ToString());
                            if (bool.Parse(dtpagerole.Rows[i]["Visible"].ToString()) == false)
                            {
                                saleToolStripMenuItem1.ShowShortcutKeys = false;
                                saleToolStripMenuItem1.ShortcutKeyDisplayString = "";
                                saleToolStripMenuItem1.ShortcutKeys = System.Windows.Forms.Keys.None;
                            }
                        }


                    }
                }
                if (dtpagerole.Rows[i]["PageName"].ToString() == "saleReturnToolStripMenuItem")
                {

                    for (int k = 0; k < Master.SideMenu.Rows.Count; k++)
                    {

                        if (Master.SideMenu.Rows[k]["Menuoption"].ToString().ToUpper() == "SALE RETURN")
                        {

                            Master.SideMenu.Rows[k]["Display"] = bool.Parse(dtpagerole.Rows[i]["Visible"].ToString());
                            if (bool.Parse(dtpagerole.Rows[i]["Visible"].ToString()) == false)
                            {
                                saleReturnToolStripMenuItem.ShowShortcutKeys = false;
                                saleReturnToolStripMenuItem.ShortcutKeyDisplayString = "";
                                saleReturnToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.None;
                            }
                        }


                    }
                }
                if (dtpagerole.Rows[i]["PageName"].ToString() == "contraToolStripMenuItem")
                {

                    for (int k = 0; k < Master.SideMenu.Rows.Count; k++)
                    {

                        if (Master.SideMenu.Rows[k]["Menuoption"].ToString().ToUpper() == "CONTRA")
                        {

                            Master.SideMenu.Rows[k]["Display"] = bool.Parse(dtpagerole.Rows[i]["Visible"].ToString());
                            if (bool.Parse(dtpagerole.Rows[i]["Visible"].ToString()) == false)
                            {
                                contraToolStripMenuItem.ShowShortcutKeys = false;
                                contraToolStripMenuItem.ShortcutKeyDisplayString = "";
                                contraToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.None;
                            }
                        }


                    }
                }
                if (dtpagerole.Rows[i]["PageName"].ToString() == "stockJournalToolStripMenuItem")
                {

                    for (int k = 0; k < Master.SideMenu.Rows.Count; k++)
                    {

                        if (Master.SideMenu.Rows[k]["Menuoption"].ToString().ToUpper() == "STOCK JOU")
                        {

                            Master.SideMenu.Rows[k]["Display"] = bool.Parse(dtpagerole.Rows[i]["Visible"].ToString());
                            if (bool.Parse(dtpagerole.Rows[i]["Visible"].ToString()) == false)
                            {
                                stockJournalToolStripMenuItem.ShowShortcutKeys = false;
                                stockJournalToolStripMenuItem.ShortcutKeyDisplayString = "";
                                stockJournalToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.None;
                            }
                        }


                    }
                }

                if (dtpagerole.Rows[i]["PageName"].ToString() == "journalToolStripMenuItem2")
                {

                    for (int k = 0; k < Master.SideMenu.Rows.Count; k++)
                    {

                        if (Master.SideMenu.Rows[k]["Menuoption"].ToString().ToUpper() == "JOURNAL")
                        {

                            Master.SideMenu.Rows[k]["Display"] = bool.Parse(dtpagerole.Rows[i]["Visible"].ToString());
                            if (bool.Parse(dtpagerole.Rows[i]["Visible"].ToString()) == false)
                            {
                                journalToolStripMenuItem2.ShowShortcutKeys = false;
                                journalToolStripMenuItem2.ShortcutKeyDisplayString = "";
                                journalToolStripMenuItem2.ShortcutKeys = System.Windows.Forms.Keys.None;
                            }
                        }


                    }
                }


            }

            frm_flowlayout frmn = new frm_flowlayout();
            frmn.MdiParent = this;
            frmn.ProgrBar = toolStripProgressBar1;
            frmn.Show();

            }

        }
        public void setMenu()
        {
            // Master.UpdateAll();
            if (Database.databaseName != "")
            {
                frm_flowlayout frmn = new frm_flowlayout();
                frmn.MdiParent = this;


                frmn.ProgrBar = toolStripProgressBar1;
                frmn.Show();

                statusStrip1.Items[6].Text = Database.ldate.ToString(Database.dformat);
                statusStrip1.Items[11].Text = Database.fyear;
                this.Text = Database.fname;
                statusStrip1.Items[2].Text = Database.ExeDate.ToString("yy.M.d");
                statusStrip1.Items[4].Text = Database.uname;
                setupToolStripMenuItem.Visible = true;
              
                saleToolStripMenuItem.Visible = true;
                toolToolStripMenuItem.Visible = true;
                exitToolStripMenuItem.Visible = true;
                settingsToolStripMenuItem.Visible = true;
                activateToolStripMenuItem.Visible = true;
               
                if (flp != null)
                {
                    flp.Dispose();
                }
                flp = new FlowLayoutPanel();
                //    taxCategoyToolStripMenuItem
                if (Feature.Available("Taxation Applicable") == "VAT")
                {
                    newToolStripMenuItem.Text = "Tax Category";
                }
                else
                {
                    newToolStripMenuItem.Text = "HSN";
                }


                userManagementToolStripMenuItem.Visible = true;

                if (Feature.Available("Required AccountGroup").ToUpper() == "YES")
                {
                    accountGroupToolStripMenuItem1.Visible = true;
                    tBalanceSheetToolStripMenuItem.Visible = false;
                    tProfitLossToolStripMenuItem.Visible = false;
                }
                else
                {
                    accountGroupToolStripMenuItem1.Visible = false;
                    tBalanceSheetToolStripMenuItem.Visible = true;
                    tProfitLossToolStripMenuItem.Visible = true;
                }

        
                if (Feature.Available("Summarized Registers").ToUpper() == "YES")
                {
                    summarizedSaleRegisterToolStripMenuItem.Visible = true;
                    summarizedPurchaseRegisterToolStripMenuItem.Visible = true;
                }
                else
                {
                    summarizedSaleRegisterToolStripMenuItem.Visible = false;
                    summarizedPurchaseRegisterToolStripMenuItem.Visible = false;
                }
                if (Database.DatabaseType == "sql")
                {
                    crossTabSaleRegisterToolStripMenuItem.Visible = false;
                }
                if (Feature.AvailableLogin("Accounts") == true)
                {
                    if (Feature.Available("Cash Book") == "No")
                    {
                        cashBookToolStripMenuItem.Visible = false;
                    }
                    if (Feature.Available("Trail Balance") == "No")
                    {
                        trialBalanceToolStripMenuItem.Visible = false;
                    }
                    if (Feature.Available("Statement Of Account") == "No")
                    {
                        ledgerToolStripMenuItem.Visible = false;
                    }
                    if (Feature.Available("Final Accounts") == "No")
                    {
                        tProfitLossToolStripMenuItem.Visible = false;
                        tBalanceSheetToolStripMenuItem.Visible = false;
                    }
                }
                else
                {
                    accountReportToolStripMenuItem.Visible = false;
                }

                if (Feature.Available("Enable Order Management") == "Yes")
                {
                    saleOrderToolStripMenuItem.Visible = true;
                    pendingOrderToolStripMenuItem.Visible = true;
                }
                else
                {
                    saleOrderToolStripMenuItem.Visible = false;
                    pendingOrderToolStripMenuItem.Visible = false;
                }
                if (Feature.Available("Required Debit Note With GST") == "Yes")
                {
                    purchaseWithDebitNoteToolStripMenuItem.Visible = true;
                    toolStripMenuItem29.Visible = true;
                }
                else
                {
                    purchaseWithDebitNoteToolStripMenuItem.Visible = false;
                    toolStripMenuItem29.Visible = false;
                }
               

                if (Feature.Available("Taxation Applicable") == "GST")
                {

                    if (Feature.AvailableLogin("Vat") == false)
                    {
                        gSTReportToolStripMenuItem.Visible = false;
                    }
                    else
                    {
                        gSTReportToolStripMenuItem.Visible = true;
                    }
                }
                else
                {
                    gSTReportToolStripMenuItem.Visible = false;
                }

                if (Feature.Available("Broker Wise Report") == "No")
                {
                    brokerReportToolStripMenuItem.Visible = false;
                }

              
                if (Feature.Available("Trail Balance") == "No")
                {
                    trialBalanceToolStripMenuItem.Visible = false;
                }

                if (Feature.Available("Cash Book") == "No")
                {
                    cashBookToolStripMenuItem.Visible = false;
                }

                if (Feature.Available("Statement Of Account") == "No")
                {
                    ledgerToolStripMenuItem.Visible = false;
                }

                if (Feature.Available("Final Accounts") == "No")
                {
                    tProfitLossToolStripMenuItem.Visible = false;
                    tBalanceSheetToolStripMenuItem.Visible = false;
                    toolStripMenuItem24.Visible = false;
                }

                if (Feature.Available("Stock Status") == "No")
                {
                    stockReportToolStripMenuItem.Visible = false;
                }


                if (Feature.Available("Item Ledger") == "No")
                {
                    itemLedgerToolStripMenuItem.Visible = false;
                }

                if (Feature.Available("Item Group Modify") == "No")
                {
                    modifyItemsToolStripMenuItem.Visible = false;
                }

                if (Feature.Available("Item Lifting Report") == "No")
                {
                    itemLiftingReportToolStripMenuItem1.Visible = false;
                    inBillChargesToolStripMenuItem.Visible = false;
                }

               

                if (Feature.Available("Item Stock") == "No")
                {
                    stockReportToolStripMenuItem.Visible = false;
                }


                if (Feature.Available("Discount After Tax") == "No")
                {
                    discountAfterTaxToolStripMenuItem.Visible = false;
                }
                else
                {
                    discountAfterTaxToolStripMenuItem.Visible = true;
                }

                if (Feature.AvailableLogin("Activated") == false)
                {
                    activateToolStripMenuItem.Visible = true;
                }
                else
                {
                    activateToolStripMenuItem.Visible = false;
                }

                if (Feature.Available("Company Colour") == "No")
                {
                    companyManufacturerToolStripMenuItem.Visible = false;
                    brandItemGroupToolStripMenuItem.Visible = false;
                    colorVariantToolStripMenuItem.Visible = false;
                    modifyItemsToolStripMenuItem.Visible = false;
                    modifyRateToolStripMenuItem.Visible = false;
                  
                    toolStripMenuItem15.Visible = false;
                    itemLiftingReportToolStripMenuItem1.Visible = false;
                    inBillChargesToolStripMenuItem.Visible = false;
                    //stockValuationToolStripMenuItem.Visible = false;
                    belowStockWarningToolStripMenuItem.Visible = false;
                    priceListToolStripMenuItem.Visible = false;
                    stockLiquidationToolStripMenuItem.Visible = false;
                }


                if (Feature.Available("Send Mail") == "Yes")
                {
                    bulkUpdatesToolStripMenuItem.Visible = true;
                    eToolStripMenuItem.Visible = true;
                }
                else
                {
                    bulkUpdatesToolStripMenuItem.Visible = false;
                    eToolStripMenuItem.Visible = false;
                }

                if (Database.utype.ToUpper() == "USER")
                {
                    userManagementToolStripMenuItem.Visible = false;
                    needApprovalToolStripMenuItem.Visible = false;
                    if (Feature.UserPower("Master") == false)
                    {
                        setupToolStripMenuItem.Visible = false;
                    }
                    else
                    {
                        setupToolStripMenuItem.Visible = true;
                    }


                    if (Feature.UserPower("Tool") == false)
                    {
                        toolToolStripMenuItem.Visible = false;
                    }
                    else
                    {
                        toolToolStripMenuItem.Visible = true;
                    }
                    if (Feature.UserPower("Report") == false)
                    {
                        exitToolStripMenuItem.Visible = false;
                    }
                    else
                    {
                        exitToolStripMenuItem.Visible = true;
                    }

                    if (Feature.UserPower("AccountReport") == false)
                    {
                        accountReportToolStripMenuItem.Visible = false;
                    }
                    else if (Feature.AvailableLogin("Accounts") == false)
                    {
                        accountReportToolStripMenuItem.Visible = false;
                    }
                    else
                    {
                        accountReportToolStripMenuItem.Visible = true;
                    }

                    if (Feature.Available("Cash Book") == "No")
                    {
                        cashBookToolStripMenuItem.Visible = false;
                    }

                    if (Feature.Available("Trail Balance") == "No")
                    {
                        trialBalanceToolStripMenuItem.Visible = false;
                    }

                    if (Feature.Available("Statement Of Account") == "No")
                    {
                        ledgerToolStripMenuItem.Visible = false;
                    }

                    if (Feature.Available("Final Accounts") == "No")
                    {
                        tProfitLossToolStripMenuItem.Visible = false;
                        tBalanceSheetToolStripMenuItem.Visible = false;
                    }

                 

                    if (Feature.UserPower("StockReport") == false)
                    {
                        stockReportToolStripMenuItem.Visible = false;
                    }
                    else
                    {
                        stockReportToolStripMenuItem.Visible = true;
                    }
                    if (Feature.UserPower("CustomerReport") == false)
                    {
                        customerReportToolStripMenuItem.Visible = false;
                    }
                    else
                    {
                        customerReportToolStripMenuItem.Visible = true;
                    }
                    if (Feature.UserPower("SuppilierReport") == false)
                    {
                        supplierReportToolStripMenuItem.Visible = false;
                    }
                    else
                    {
                        supplierReportToolStripMenuItem.Visible = true;
                    }
                    if (Feature.UserPower("BrokerReport") == false)
                    {
                        brokerReportToolStripMenuItem.Visible = false;
                    }
                    else
                    {
                        brokerReportToolStripMenuItem.Visible = true;
                    }
                    if (Feature.UserPower("OtherReport") == false)
                    {
                        otherReportToolStripMenuItem.Visible = false;
                    }
                    else
                    {
                        otherReportToolStripMenuItem.Visible = true;
                    }

                }

                if (Feature.Available("Discount on Grid") == "Yes")
                {
                    inBillChargesToolStripMenuItem.Visible = true;
                }
                else
                {
                    inBillChargesToolStripMenuItem.Visible = false;
                }

                //if (Feature.Available("Send SMS").ToUpper() == "YES")
                //{
                //    sMSTakadaToolStripMenuItem.Visible = true;
                //    sMSLogToolStripMenuItem.Visible = true;
                //    removeSMSLogToolStripMenuItem.Visible = true;
                //}
                //else
                //{
                //    sMSTakadaToolStripMenuItem.Visible = false;
                //    sMSLogToolStripMenuItem.Visible = false;
                //    removeSMSLogToolStripMenuItem.Visible = false;
                //}
          

                int firms = Database.GetOtherScalarInt("Select count(*) as firms from Firminfo");
                if (firms == 1)
                {
                    deleteFirmToolStripMenuItem.Enabled = false;
                }
                else
                {
                    deleteFirmToolStripMenuItem.Enabled = true;
                }

                if (Database.DatabaseType == "sql")
                {
                    createNewFinancialYearToolStripMenuItem.Visible = false;
                    deleteFirmToolStripMenuItem.Visible = false;
                    dataRestoreToolStripMenuItem.Visible = false;
                }
                else
                {
                    createNewFinancialYearToolStripMenuItem.Visible = true;
                    deleteFirmToolStripMenuItem.Visible = true;
                    dataRestoreToolStripMenuItem.Visible = true;
                }


                if (Database.utype.ToUpper() == "SUPERADMIN")
                {
                    userManagementToolStripMenuItem.Visible = true;
                    controlRoomToolStripMenuItem1.Visible = true;
                    tranjectionSetupToolStripMenuItem.Visible = true;
                }
                else
                {
                    userManagementToolStripMenuItem.Visible = false;
                    controlRoomToolStripMenuItem1.Visible = false;
                    tranjectionSetupToolStripMenuItem.Visible = false;
                }

                if (Database.utype.ToUpper() == "CASHIER" || Database.utype.ToUpper() == "SUPERADMIN")
                {
                    cashierToolStripMenuItem.Visible = true;
                }
                else
                {
                    cashierToolStripMenuItem.Visible = false;
                }

            }

            else if (Database.databaseName == "")
            {
                saleToolStripMenuItem.Visible = false;
                setupToolStripMenuItem.Visible = false;
                toolToolStripMenuItem.Visible = false;
                exitToolStripMenuItem.Visible = false;
                activateToolStripMenuItem.Visible = false;
                settingsToolStripMenuItem.Visible = false;
                
            }


            if (Feature.AvailableLogin("Activated") == false)
            {
                if (Database.databaseName != "")
                {
                    DataTable dtdiff = new DataTable();
                    Database.GetSqlData("select distinct Vdate from voucherinfo", dtdiff);
                    int count = 0;
                    count = dtdiff.Rows.Count;

                    if (count >= 30)
                    {
                        setupToolStripMenuItem.Visible = false;
                        settingsToolStripMenuItem.Visible = false;
                        toolToolStripMenuItem.Visible = false;
                        exitToolStripMenuItem.Visible = false;
                    }

                }
            }
        }

        private void importDescriptionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmImportDesc frm = new frmImportDesc();
            frm.MdiParent = this;
            frm.Show();
        }

        private void smartDocumentFinderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmSmartDocumentFinder frm = new frmSmartDocumentFinder();
            frm.MdiParent = this;
            frm.Show();
        }

        private void stockTransferToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmBalTransfer frm = new frmBalTransfer();
            frm.frmBalTrans = "Stock";
            frm.Text = "Stock Transfer";
            frm.MdiParent = this;
            frm.LoadData();
            frm.Show();
        }



        private void faspiUpdatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmSoftwareUpdates frm = new frmSoftwareUpdates();
            frm.MdiParent = this;
            frm.Show();
        }

        private void eFilingUPCSTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frm_eFilingCST frm = new frm_eFilingCST();
            frm.MdiParent = this;
            frm.Show();
        }

        private void mailerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmMailer frm = new frmMailer();
            frm.MdiParent = this;
            frm.Show();
        }

        private void desktopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ofd.Filter = "JPEG Files(*.jpg) | *.jpg";
            if (DialogResult.OK == ofd.ShowDialog())
            {
                this.BackgroundImage = new Bitmap(ofd.FileName);
                this.BackgroundImageLayout = ImageLayout.Stretch;
                GC.Collect();
                File.Copy(ofd.FileName, Application.StartupPath + "\\System\\" + Database.fname + ".jpg", true);
                MessageBox.Show("Done");
            }
        }

        private void sMSSetupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmSmsSetting frm = new frmSmsSetting();
            frm.MdiParent = this;
            frm.Show();
        }

        private void interestVoucherToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmDebitCredit frm = new frmDebitCredit();
            frm.dr_cr_note = "Credit Note";
            frm.MdiParent = this;
            frm.LoadData("0", "Credit Note");
            frm.Show();
        }

        private void commonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmDebitCredit frm = new frmDebitCredit();
            frm.dr_cr_note = "Debit Note";
            frm.MdiParent = this;
            frm.LoadData("0", "Debit Note");
            frm.Show();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            statusStrip1.Items[7].Text = DateTime.Now.ToLongTimeString();
        }

        private void checkForUpdateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AutoUpdater.Start("http://faspi.in/MarwariGstUpdate/FaspiPaintsPro.xml");
        }

        private void calculatorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("calc");
        }

        private void Restore()
        {
            DialogResult val = ofd.ShowDialog(this);
            DataTable dt = new DataTable();
            if (val == DialogResult.OK)
            {
                System.Data.OleDb.OleDbConnection Conn = new System.Data.OleDb.OleDbConnection();
                Conn.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + ofd.FileName + ";Persist Security Info=true;Jet OLEDB:Database Password=ptsoft9358524971";
                Conn.Open();
                string str = "select * from company";
                System.Data.OleDb.OleDbDataAdapter da = new System.Data.OleDb.OleDbDataAdapter(str, Conn);
                da.Fill(dt);
                Conn.Close();
                DataTable dtckeck = new DataTable();
                Database.GetOtherSqlData("select * from firminfo where Firm_name= '" + dt.Rows[0]["Name"] + "' and Firm_Period_name= '" + dt.Rows[0]["Firm_Period_name"] + "'", dtckeck);
                if (dtckeck.Rows.Count != 0)
                {
                    DirectoryInfo dir = new DirectoryInfo(Application.StartupPath + "\\System");
                    bool ch = dir.Exists;
                    if (ch == false)
                    {
                        dir.Create();
                    }
                    string PathtoRestoreFrom;
                    PathtoRestoreFrom = ofd.FileName;
                    File.Copy(Application.StartupPath + "\\Database\\" + dtckeck.Rows[0]["Firm_database"] + ".mdb", Application.StartupPath + "\\System\\" + DateTime.Now.ToString("yyyyMMddHHmmss"));
                    File.Copy(PathtoRestoreFrom, Application.StartupPath + "\\Database\\" + dtckeck.Rows[0]["Firm_database"] + ".mdb", true);
                    MessageBox.Show("Restore Successfull");

                }
                else
                {
                    MessageBox.Show("Firm/Company Not Found in Database" + Environment.NewLine + "Firm Name: " + dt.Rows[0]["Name"] + Environment.NewLine + "Period: " + dt.Rows[0]["Firm_Period_name"]);
                }
            }
        }

        private void exportLedgerMasterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frm_tally frm = new frm_tally();
            frm.MdiParent = this;
            frm.Show();
        }

        private void exportJournalToolStripMenuItem_Click(object sender, EventArgs e)
        {

            frm_tallydate frm = new frm_tallydate();
            frm.MdiParent = this;
            frm.type = "Receipt";
            frm.Show();




        }

        private void exportSalesPurchasrVoucherToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frm_tallydate frm = new frm_tallydate();
            frm.MdiParent = this;
            frm.type = "Sale";
            frm.Show();

        }

        private void exportJournalVoucherToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Tally objtally = new Tally();

            DataTable dtvou = new DataTable();

            Database.GetSqlData("SELECT VOUCHERINFO.Vi_id FROM VOUCHERINFO INNER JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id WHERE (((VOUCHERTYPE.Type)='Journal')) OR (((VOUCHERTYPE.Type)='Journal'))", dtvou);
            for (int i = 0; i < dtvou.Rows.Count; i++)
            {
                DataTable dtdet = new DataTable();
                Database.GetSqlData("SELECT ACCOUNT.Name, [JOURNAL].[Dr]+([JOURNAL].[Cr]*-1) AS Amount, JOURNAL.Narr, JOURNAL.Vdate, VOUCHERTYPE.AliasName, VOUCHERINFO.Vnumber FROM ((JOURNAL INNER JOIN ACCOUNT ON JOURNAL.Ac_id = ACCOUNT.Ac_id) INNER JOIN VOUCHERINFO ON JOURNAL.Vi_id = VOUCHERINFO.Vi_id) INNER JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id WHERE (((JOURNAL.Vi_id)='" + dtvou.Rows[i]["Vi_id"].ToString() + "'))", dtdet);
                DateTime Vdate = DateTime.Parse(dtdet.Rows[0]["Vdate"].ToString());
                objtally.CreateVoucher("Journal", Vdate.ToString("yyyyMMdd"), dtdet.Rows[0]["Name"].ToString(), dtdet.Rows[0]["AliasName"].ToString() + " " + dtdet.Rows[0]["Vnumber"].ToString(), dtdet.Rows[0]["AliasName"].ToString() + " " + dtdet.Rows[0]["Vnumber"].ToString() + " " + dtdet.Rows[0]["Narr"].ToString(), dtdet);
            }
            MessageBox.Show("Done");
        }

        private void exportPurchaseReturnVoucherToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Tally objtally = new Tally();

            DataTable dtvou = new DataTable();

            Database.GetSqlData("SELECT VOUCHERINFO.Vi_id FROM VOUCHERINFO INNER JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id WHERE (((VOUCHERTYPE.Type)='Return')) OR (((VOUCHERTYPE.Type)='Return'))", dtvou);
            for (int i = 0; i < dtvou.Rows.Count; i++)
            {
                DataTable dtdet = new DataTable();
                Database.GetSqlData("SELECT ACCOUNT.Name, [JOURNAL].[Dr]+([JOURNAL].[Cr]*-1) AS Amount, JOURNAL.Narr, JOURNAL.Vdate, VOUCHERTYPE.AliasName, VOUCHERINFO.Vnumber FROM ((JOURNAL INNER JOIN ACCOUNT ON JOURNAL.Ac_id = ACCOUNT.Ac_id) INNER JOIN VOUCHERINFO ON JOURNAL.Vi_id = VOUCHERINFO.Vi_id) INNER JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id WHERE (((JOURNAL.Vi_id)='" + dtvou.Rows[i]["Vi_id"].ToString() + "'))", dtdet);
                DateTime Vdate = DateTime.Parse(dtdet.Rows[0]["Vdate"].ToString());
                objtally.CreateVoucher("CreditNote", Vdate.ToString("yyyyMMdd"), dtdet.Rows[0]["Name"].ToString(), dtdet.Rows[0]["AliasName"].ToString() + " " + dtdet.Rows[0]["Vnumber"].ToString(), dtdet.Rows[0]["AliasName"].ToString() + " " + dtdet.Rows[0]["Vnumber"].ToString() + " " + dtdet.Rows[0]["Narr"].ToString(), dtdet);
            }
        }

        private void missprintingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frm_updatelevel frm = new frm_updatelevel();
            frm.MdiParent = this;
            frm.Size = this.Size;
            frm.Show();


        }

        private void asianMyAwazToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Frm_PurImp frm = new Frm_PurImp();
            frm.MdiParent = this;
            frm.Show();
        }

        private void testToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PdfReader frm = new PdfReader();
            frm.Show();
        }

        private void particularCommoditySummaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report gg = new Report();
            string strCombo = "SELECT TAXCATEGORY.Category_Name, TAXCATEGORY.Commodity_Code FROM TAXCATEGORY ORDER BY TAXCATEGORY.Category_Name";
            char cg = 'a';
            string selected = SelectCombo.ComboKeypress(this, cg, strCombo, "", 1);
            gg.ParticularCommoditySummary(Database.ldate, Database.ldate, selected);
            gg.MdiParent = this;
            gg.Show();

        }

        private void customerProfitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report gg = new Report();
            string strCombo = funs.GetStrComboled("*");
            char cg = 'a';
            string selected = SelectCombo.ComboKeypress(this, cg, strCombo, "", 1);
            gg.CustomerProfit(Database.stDate, Database.ldate, selected);
            gg.MdiParent = this;
            gg.Show();

        }

        private void customerwiseProfitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report gg = new Report();
            gg.CustomerwiseProfit(Database.stDate, Database.ldate);
            gg.MdiParent = this;
            gg.Show();
        }

        private void sMSTakadaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SmsTakada frm = new SmsTakada();
            frm.MdiParent = this;
            frm.LoadData();
            frm.Show();
        }

        private void importFromFaspiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frm_import frm = new frm_import();
            frm.MdiParent = this;
            frm.LoadData();
            frm.Show();
        }

        private void groupLedgerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report gg = new Report();
            gg.GroupLedger(Database.stDate, Database.ldate);
            gg.MdiParent = this;
            gg.Show();
        }

        private void mToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frm_modifyopn frm = new frm_modifyopn();
            frm.MdiParent = this;
            frm.LoadData();
            frm.Show();
        }

        private void supplierLiftingAmountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report gg = new Report();
            string strCombo = funs.GetStrComboled("*");

            char cg = 'a';
            string selected = SelectCombo.ComboKeypress(this, cg, strCombo, "", 1);
            gg.SupplierLifting(Database.stDate, Database.ldate, selected);
            gg.MdiParent = this;
            gg.Show();
        }

        private void stockValuationToolStripMenuItem_Click(object sender, EventArgs e)
        {

            Form1 frm = new Form1();
            frm.Show();
        }

        private void frm_main_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control == false && e.KeyCode == Keys.F11)
            {
                frmMaster frm = new frmMaster();
                frm.MdiParent = this;
                frm.LoadData("Control Room", "Control Room");
                frm.Show();
            }
            else if (e.Control == false && e.KeyCode == Keys.F12)
            {
                frmMaster frm = new frmMaster();
                frm.MdiParent = this;
                frm.LoadData("TransactionSetup", "TransactionSetup");
                frm.Show();
            }
            if (e.Control == false && e.Alt==false &&    e.KeyCode == Keys.F4)
            {
                frmMasterVou frm = new frmMasterVou();
                frm.MdiParent = this;
                frm.LoadData("Contra", "Contra Voucher");
                frm.Show();
            }
            else if (e.Control && e.Alt && e.KeyCode == Keys.A)
            {

                DataTable dtcombo = new DataTable();
                dtcombo.Columns.Add("Mode", typeof(string));
                dtcombo.Rows.Add();
                dtcombo.Rows[dtcombo.Rows.Count - 1]["Mode"] = "Business";

                permission = funs.GetPermissionKey("Transactions");

                UsersFeature ob = permission.Where(w => w.FeatureName == "Personal Mode").FirstOrDefault();


                if (ob != null && ob.SelectedValue == "Allowed")
                {
                    dtcombo.Rows.Add();
                    dtcombo.Rows[dtcombo.Rows.Count - 1]["Mode"] = "Personal";
                }

                permission = funs.GetPermissionKey("Transactions");

                UsersFeature ob1 = permission.Where(w => w.FeatureName == "Both Mode").FirstOrDefault();

                if (ob1 != null && ob1.SelectedValue == "Allowed")
                {
                    dtcombo.Rows.Add();
                    dtcombo.Rows[dtcombo.Rows.Count - 1]["Mode"] = "Both";
                }

                //if (Feature.Available("Personal Mode Allowed").ToUpper() == "YES")
                //{
                //    dtcombo.Rows.Add();
                //    dtcombo.Rows[dtcombo.Rows.Count - 1]["Mode"] = "Personal";
                //}
                //if (Feature.Available("Both Mode Allowed").ToUpper() == "YES")
                //{
                //    dtcombo.Rows.Add();
                //    dtcombo.Rows[dtcombo.Rows.Count - 1]["Mode"] = "Both";
                //}
                string smode = SelectCombo.ComboDt(this, dtcombo, 1);
                if (smode == "")
             
                
                {
                    smode = "Business";
                }
                mode.Text = smode;
                if (smode == "Business")
                {
                    Database.IsKacha = false;
                    Database.BMode = "A";
                }
                else if (smode == "Personal")
                {
                    Database.IsKacha = true;
                    Database.BMode = "B";
                }
                else if (smode == "Both")
                {
                    Database.IsKacha = true;
                    Database.BMode = "AB";
                }

                Master.UpdateAll();
            }
            else if (e.Control && e.Alt && e.KeyCode == Keys.U)
            {
                InputBox box = new InputBox("Enter Password", "", true);
                box.outStr = "";
                box.ShowInTaskbar = false;
                box.ShowDialog(this);

                if (box.outStr == "admin")
                {
                    if (Database.DatabaseType == "access")
                    {
                        Database.CommandExecutor("UPDATE Voucherdet LEFT JOIN Description ON Voucherdet.Des_ac_id = Description.Des_id SET Voucherdet.Rate_Unit = [description].[Rate_Unit],Voucherdet.Pvalue = [description].[Pvalue]");
                    }
                    else
                    {
                        Database.CommandExecutor("UPDATE Voucherdet SET Voucherdet.Rate_Unit = [description].[Rate_Unit] FROM   description, Voucherdet WHERE description.des_id = voucherdet.des_ac_id,,Voucherdet.Pvalue = [description].[Pvalue]");
                    }
                    MessageBox.Show("Done");
                }
                else
                {
                    MessageBox.Show("Wrong Password.");
                }
            }

            else if (e.Control && e.Alt && e.KeyCode == Keys.C)
            {
                DataTable dtnew = new DataTable();
                Database.GetSqlData("SELECT OBJECT_NAME(object_id) AS NameofConstraint, SCHEMA_NAME(schema_id) AS SchemaName, OBJECT_NAME(parent_object_id) AS TableName, type_desc FROM  sys.objects WHERE (type_desc LIKE '%CONSTRAINT')", dtnew);

                for (int i = 0; i < dtnew.Rows.Count; i++)
                {
                    Database.CommandExecutor("ALTER TABLE [dbo].[" + dtnew.Rows[i]["TableName"].ToString() + "] DROP CONSTRAINT [" + dtnew.Rows[i]["NameofConstraint"].ToString() + "]");
                }
                MessageBox.Show("Done");
            }
            else if (e.Control && e.Alt && e.KeyCode == Keys.D)
            {   
                Database.CommandExecutor("Delete from FirmSetup where features='Send SMS'");
                Database.CommandExecutor("Delete from FirmSetup where features='Action On ChangeRate'");
                Database.CommandExecutor("Delete from FirmSetup where features='Personal Mode'");
                Database.CommandExecutor("Delete from FirmSetup where features='Both Mode'");
                Database.CommandExecutor("Delete from FirmSetup where features='Voucher Editing Power'");
                Database.CommandExecutor("Delete from FirmSetup where features='Voucher Delete Permission'");
                Database.CommandExecutor("Delete from FirmSetup where features='Required UpdateRate Option'");
                Master.UpdateFeature();
                MessageBox.Show("Done");
            }
            else if (e.Control && e.Alt && e.KeyCode == Keys.R)
            {
                InputBox box = new InputBox("Enter Password", "", true);
                box.outStr = "";

                box.ShowInTaskbar = false;
                box.ShowDialog(this);

                if (box.outStr == "SURE")
                {
                    Database.CommandExecutor("Delete from FirmSetup where Features='Barcode'");
                    Database.CommandExecutor("Delete from FirmSetup where Features='Pending Invoice'");
                    Database.CommandExecutor("Delete from FirmSetup where Features='Batch Code'");
                    Database.CommandExecutor("Delete from FirmSetup where Features='Amt=Weight*Qty*Rate'");
                    Database.CommandExecutor("Delete from FirmSetup where Features='Weight required in Billing'");
                    Database.CommandExecutor("Delete from FirmSetup where Features='Purchase Invoice (Ex-State)'");
                    Database.CommandExecutor("Delete from FirmSetup where Features='Price Variation Report'");
                    Database.CommandExecutor("Delete from FirmSetup where Features='Grid Report'");
                    Database.CommandExecutor("Delete from FirmSetup where Features='Unregistered Purchase'");
                    Database.CommandExecutor("Delete from FirmSetup where Features='Sale Including Tax'");
                    Database.CommandExecutor("Delete from FirmSetup where Features='Dot Matrix'");
                    Database.CommandExecutor("Delete from FirmSetup where Features='Transaction Report in Crystal Report'");
                    Database.CommandExecutor("Delete from FirmSetup where Features='Production'");
                    Database.CommandExecutor("Update FirmSetup set selected_value='GST' where Features='Taxation Applicable'  ");
                    Database.CommandExecutor("Drop table items");
                    Database.CommandExecutor("Drop table ITEMTAX");
                    Database.CommandExecutor("Drop table Voucherdet1");
                    Database.CommandExecutor("Drop table Journal1");
                    Database.CommandExecutor("Drop table Packing");
                    Database.CommandExecutor("Drop table BASERATE");
                    Database.CommandExecutor("Drop table TAXCATEGORYDETAIL");
                    Database.CommandExecutor("Drop table USERACC");
                    Database.CommandExecutor("Delete from Vouchertype where type='Report' and Name<>'Ledger'");
                    Database.CommandExecutor("Delete from Vouchertype where Type='Sale' and A=" + access_sql.Singlequote + "true" + access_sql.Singlequote + "");
                    Database.CommandExecutor("Delete from Vouchertype where Type='Return' and A=" + access_sql.Singlequote + "true" + access_sql.Singlequote + " ");
                    Database.CommandExecutor("Delete from Vouchertype where Name='Stock Issue'");
                    Database.CommandExecutor("Delete from Vouchertype where Name='Stock Receive'");
                    Database.CommandExecutor("Delete from Vouchertype where Name='Bank Payment'");
                    Database.CommandExecutor("Delete from Vouchertype where Name='Bank Receipt'");
                    Database.CommandExecutor("Delete from Vouchertype where Name='Purchase(Ex State)'");
                    Database.CommandExecutor("Delete from Vouchertype where Name='Purchase Return(Ex State)'");
                    Database.CommandExecutor("Update Vouchertype set Name='Payment Voucher',AliasName='Payment Voucher' where Name='Cash Payment'");
                    Database.CommandExecutor("Update Vouchertype set Name='Receipt Voucher',AliasName='Receipt Voucher' where Name='Cash Receipt'");
                    Database.CommandExecutor("Update Vouchertype set Name='Bill of Supply',AliasName='Bill of Supply',CashTransaction='Only Allowed',Prefix='B-',Padding=6,printcopy='Original for Recipient,True;Duplicate for Transporter,True;Triplicate for Supplier,True;',Code='SLB',Short='SLB',ReportName='GSTBOSA4.rpt' where Name='Bill'");
                    Database.CommandExecutor("Update Vouchertype set CashTransaction='Not Allowed',printcopy='Original for Recipient,True;Duplicate for Transporter,True;Triplicate for Supplier,True;',Prefix='T-',padding=6,Code='SLT',Short='SLT',ReportName='GSTTIA4.rpt' where Name='Tax Invoice'");
                    Database.CommandExecutor("Update Vouchertype set Name='Contra Voucher',AliasName='Contra Voucher' where Name='Contra'");
                    Database.CommandExecutor("Alter table Vouchertype Drop AllowedAcc");
                    Database.CommandExecutor("insert into VOUCHERTYPE ([Name],[Type],[Stationary],[Numtype],[Short],[AliasName],[Default1],[Default2],[Default3],[ReportName],[Code],[Effect_On_Stock],[Effect_On_Acc],[IncludingTax],[ExcludingTax],[ExState],[TaxInvoice],[Unregistered],[Active],[PaperSize],[SmsTemplate],[A],[B],[printcopy],[CashTransaction],[Calculation],[Postfix],[Prefix],[Padding],[Exempted]) values('Bill of Supply','Sale'," + access_sql.Singlequote + "true" + access_sql.Singlequote + ",1,'SLB','Bill of Supply','Original Copy','Duplicate Copy','Office Copy','GSTBOSA4.rpt','SLB','Y','Y'," + access_sql.Singlequote + "true" + access_sql.Singlequote + "," + access_sql.Singlequote + "true" + access_sql.Singlequote + "," + access_sql.Singlequote + "false" + access_sql.Singlequote + "," + access_sql.Singlequote + "false" + access_sql.Singlequote + "," + access_sql.Singlequote + "false" + access_sql.Singlequote + "," + access_sql.Singlequote + "true" + access_sql.Singlequote + ",'A4','Dear Customer, Thanks For Purchasing With Us, Your Last Bill No is: {Vno}'," + access_sql.Singlequote + "true" + access_sql.Singlequote + "," + access_sql.Singlequote + "true" + access_sql.Singlequote + ",'Original for Recipient,True;Duplicate for Transporter,True;Triplicate for Supplier,True;','Only Allowed','Default Excluding Tax','','B-',6,'Allowed')");
                    Database.CommandExecutor("insert into VOUCHERTYPE ([Name],[Type],[Stationary],[Numtype],[Short],[AliasName],[Default1],[Default2],[Default3],[ReportName],[Code],[Effect_On_Stock],[Effect_On_Acc],[IncludingTax],[ExcludingTax],[ExState],[TaxInvoice],[Unregistered],[Active],[PaperSize],[SmsTemplate],[A],[B],[printcopy],[CashTransaction],[Calculation],[Postfix],[Prefix],[Padding],[Exempted]) values('Bill of Supply Return','Return'," + access_sql.Singlequote + "true" + access_sql.Singlequote + ",1,'REB','Bill of Supply Return','Original Copy','Duplicate Copy','Office Copy','GSTBOSA4.rpt','REB','Y','Y'," + access_sql.Singlequote + "true" + access_sql.Singlequote + "," + access_sql.Singlequote + "true" + access_sql.Singlequote + "," + access_sql.Singlequote + "false" + access_sql.Singlequote + "," + access_sql.Singlequote + "false" + access_sql.Singlequote + "," + access_sql.Singlequote + "false" + access_sql.Singlequote + "," + access_sql.Singlequote + "true" + access_sql.Singlequote + ",'A4','Dear Customer, Thanks For Purchasing With Us, Your Last Bill No is: {Vno}'," + access_sql.Singlequote + "true" + access_sql.Singlequote + "," + access_sql.Singlequote + "true" + access_sql.Singlequote + ",'Original for Recipient,True;Duplicate for Transporter,True;Triplicate for Supplier,True;','Only Allowed','Default Excluding Tax','','BR-',6,'Allowed')");
                    Database.CommandExecutor("insert into VOUCHERTYPE ([Name],[Type],[Stationary],[Numtype],[Short],[AliasName],[Default1],[Default2],[Default3],[ReportName],[Code],[Effect_On_Stock],[Effect_On_Acc],[IncludingTax],[ExcludingTax],[ExState],[TaxInvoice],[Unregistered],[Active],[PaperSize],[SmsTemplate],[A],[B],[printcopy],[CashTransaction],[Calculation],[Postfix],[Prefix],[Padding],[Exempted]) values('Tax Invoice','Sale'," + access_sql.Singlequote + "true" + access_sql.Singlequote + ",1,'SLT','Tax Invoice','Original Copy','Duplicate Copy','Office Copy','GSTTIA4.rpt','SLT','Y','Y'," + access_sql.Singlequote + "true" + access_sql.Singlequote + "," + access_sql.Singlequote + "true" + access_sql.Singlequote + "," + access_sql.Singlequote + "false" + access_sql.Singlequote + "," + access_sql.Singlequote + "true" + access_sql.Singlequote + "," + access_sql.Singlequote + "false" + access_sql.Singlequote + "," + access_sql.Singlequote + "true" + access_sql.Singlequote + ",'A4','Dear Customer, Thanks For Purchasing With Us, Your Last Bill No is: {Vno}'," + access_sql.Singlequote + "true" + access_sql.Singlequote + "," + access_sql.Singlequote + "true" + access_sql.Singlequote + ",'Original for Recipient,True;Duplicate for Transporter,True;Triplicate for Supplier,True;','Not Allowed','Default Excluding Tax','','T-',6,'Not Allowed')");
                    Database.CommandExecutor("insert into VOUCHERTYPE ([Name],[Type],[Stationary],[Numtype],[Short],[AliasName],[Default1],[Default2],[Default3],[ReportName],[Code],[Effect_On_Stock],[Effect_On_Acc],[IncludingTax],[ExcludingTax],[ExState],[TaxInvoice],[Unregistered],[Active],[PaperSize],[SmsTemplate],[A],[B],[printcopy],[CashTransaction],[Calculation],[Postfix],[Prefix],[Padding],[Exempted]) values('Tax Invoice Return','Return'," + access_sql.Singlequote + "true" + access_sql.Singlequote + ",1,'RET','Tax Invoice Return','Original Copy','Duplicate Copy','Office Copy','GSTTIA4.rpt','RET','Y','Y'," + access_sql.Singlequote + "true" + access_sql.Singlequote + "," + access_sql.Singlequote + "true" + access_sql.Singlequote + "," + access_sql.Singlequote + "false" + access_sql.Singlequote + "," + access_sql.Singlequote + "true" + access_sql.Singlequote + "," + access_sql.Singlequote + "false" + access_sql.Singlequote + "," + access_sql.Singlequote + "true" + access_sql.Singlequote + ",'A4','Dear Customer, Thanks For Purchasing With Us, Your Last Bill No is: {Vno}'," + access_sql.Singlequote + "true" + access_sql.Singlequote + "," + access_sql.Singlequote + "true" + access_sql.Singlequote + ",'Original for Recipient,True;Duplicate for Transporter,True;Triplicate for Supplier,True;','Not Allowed','Default Excluding Tax','','TR-',6,'Not Allowed')");
                    Master.UpdateControlRoom();
                    Master.UpdateVoucherType();

                    MessageBox.Show("Repaired");
                }
            }
        }

        private void companyWiseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report gg = new Report();
            string strCombo = "Select Name from Other where Type='SER14' order by Name";
            char cg = 'a';
            string selected = SelectCombo.ComboKeypress(this, cg, strCombo, "", 1);
            gg.CompanyWise(Database.ldate, Database.ldate, selected);
            gg.MdiParent = this;
            gg.Show();
        }

        private void saleRegisterToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Report gg = new Report();
            gg.SaleRegisterTax(Database.stDate, Database.ldate);
            gg.MdiParent = this;
            gg.Show();
        }

        private void purchseRegisterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report gg = new Report();
            gg.PurchaseRegisterTax(Database.stDate, Database.ldate);
            gg.MdiParent = this;
            gg.Show();
        }

        private void itemLiftingReportToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Report gg = new Report();
            string strCombo = "Select Name from Other where Type='SER14' order by Name";
            char cg = 'a';
            string selected = SelectCombo.ComboKeypress(this, cg, strCombo, "", 1);
            gg.ItemLifting(Database.stDate, Database.ldate, selected);
            gg.MdiParent = this;
            gg.Show();
        }

        private void commoditysSaleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report gg = new Report();
            string strCombo = "SELECT TAXCATEGORY.Category_Name, TAXCATEGORY.Commodity_Code FROM TAXCATEGORY ORDER BY TAXCATEGORY.Category_Name";
            char cg = 'a';
            string selected = SelectCombo.ComboKeypress(this, cg, strCombo, "", 1);
            gg.CommoditySale(Database.ldate, Database.ldate, selected);
            gg.MdiParent = this;
            gg.Show();

        }

        private void commoditysSaleRegisterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report gg = new Report();
            string strCombo = "SELECT TAXCATEGORY.Category_Name, TAXCATEGORY.Commodity_Code FROM TAXCATEGORY ORDER BY TAXCATEGORY.Category_Name";
            char cg = 'a';
           
            string selected = SelectCombo.ComboKeypress(this, cg, strCombo, "", 1);
            gg.CommodityPurchase(Database.ldate, Database.ldate, selected);
            gg.MdiParent = this;
            gg.Show();
        }

        private void detailLedgerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report gg = new Report();
            string strCombo = funs.GetStrComboled("*");
            char cg = 'a';
            string selected = SelectCombo.ComboKeypress(this, cg, strCombo, "", 1);
            gg.DetailLedger(Database.stDate, Database.ldate, selected);
            gg.MdiParent = this;
            gg.Show();
        }

        private void tBalanceSheetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report gg = new Report();
            gg.BalanceSheet(Database.stDate, Database.ldate);
            gg.MdiParent = this;
            gg.Show();
        }

        private void tProfitLossToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report gg = new Report();
            gg.ProfitAndLoss(Database.stDate, Database.ldate);
            gg.MdiParent = this;
            gg.Show();
        }

        private void cashReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report gg = new Report();
            string strCombo = "SELECT VOUCHERINFO.Formno FROM VOUCHERTYPE LEFT JOIN ((VOUCHERINFO LEFT JOIN ACCOUNT ON VOUCHERINFO.Ac_id = ACCOUNT.Ac_id) LEFT JOIN ACCOUNTYPE ON ACCOUNT.Act_id = ACCOUNTYPE.Act_id) ON VOUCHERTYPE.Vt_id = VOUCHERINFO.Vt_id GROUP BY VOUCHERINFO.Formno, ACCOUNTYPE.Act_id, VOUCHERTYPE.Vt_id HAVING (((VOUCHERINFO.Formno)<>'') AND ((ACCOUNTYPE.Act_id)='SER3') AND ((VOUCHERTYPE.Vt_id)='SER15' Or (VOUCHERTYPE.Vt_id)='SER3'))";
            char cg = 'a';
            string selected = SelectCombo.ComboKeypress(this, cg, strCombo, "", 1);
            gg.CashReport(Database.stDate, Database.ldate, selected);
            gg.MdiParent = this;
            gg.Show();
        }

        private void stiockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report gg = new Report();

            string godown = "";
            string godownname = "";
            char cg = 'a';

            if (Feature.Available("Multi-Godown") == "Yes")
            {
                godown = "select distinct '<MAIN>' as name from account union all SELECT ACCOUNT.Name as name FROM ACCOUNT LEFT JOIN ACCOUNTYPE ON ACCOUNT.Act_id = ACCOUNTYPE.Act_id WHERE (((ACCOUNTYPE.Name)='Godown')) GROUP BY ACCOUNT.Name";
                godownname = SelectCombo.ComboKeypress(this, cg, godown, "", 0);
            }

            string strCombo = "";
            if (Database.DatabaseType == "access")
            {
                strCombo = "Select Distinct(TaxSlab) " + access_sql.Concat + " '%'  AS Tax_Rate from (SELECT [PTR1]+[PTR2] + [PCR] AS TaxSlab FROM TAXCATEGORY Union all SELECT PTR3  + [PCREX] AS TaxSlab FROM TAXCATEGORY Union all SELECT [STR1]+[STR2] + [SCR] AS TaxSlab FROM TAXCATEGORY Union all SELECT [STR3] + [SCREX] as TaxSlab FROM TAXCATEGORY ) as test ";
            }
            else
            {
                strCombo = "Select Distinct  CAST(TaxSlab AS nvarchar(10)) + '%'  AS Tax_Rate from (SELECT [PTR1]+[PTR2] + [PCR] AS TaxSlab FROM TAXCATEGORY Union all SELECT PTR3  + [PCREX] AS TaxSlab FROM TAXCATEGORY Union all SELECT [STR1]+[STR2] + [SCR] AS TaxSlab FROM TAXCATEGORY Union all SELECT [STR3] + [SCREX] as TaxSlab FROM TAXCATEGORY ) as test ";
            }
            string taxslab = SelectCombo.ComboKeypress(this, cg, strCombo, "", 0);
            gg.StockTaxSlabWise(Database.stDate, Database.ldate, godownname, taxslab);

            gg.MdiParent = this;
            gg.Show();
        }

        private void billByBillAdjustmentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frm_BillByBill frm = new frm_BillByBill();
            frm.MdiParent = this;
            frm.Show();
        }

        private void billByBillPendingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report gg = new Report();
            string strCombo = funs.GetStrComboled("*");
            char cg = 'a';
            string selected = SelectCombo.ComboKeypress(this, cg, strCombo, "", 1);
            gg.CustomerPending(Database.stDate, Database.ldate, selected);
            gg.MdiParent = this;
            gg.Show();
        }

        private void customersBillWiseAdjustmentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report gg = new Report();
            string strCombo = funs.GetStrComboled("*");
            char cg = 'a';
            string selected = SelectCombo.ComboKeypress(this, cg, strCombo, "", 1);
            gg.CustomerBillwise(Database.stDate, Database.ldate, selected);
            gg.MdiParent = this;
            gg.Show();
        }

        private void outstandingBillsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report gg = new Report();
            string strCombo = funs.GetStrComboled("*");
            char cg = 'a';
            string selected = SelectCombo.ComboKeypress(this, cg, strCombo, "", 1);
            gg.CustomerOutstanding(Database.stDate, Database.ldate, selected);
            gg.MdiParent = this;
            gg.Show();
        }

        private void updaterate(string companyname)
        {
            if (companyname != "")
            {

                Object misValue = System.Reflection.Missing.Value;
                Excel.Application apl = new Microsoft.Office.Interop.Excel.Application();

                Excel.Workbook wb;
                wb = (Excel.Workbook)apl.Workbooks.Add(misValue);

                //Rate_Y
                Excel.Worksheet ws = (Excel.Worksheet)wb.Worksheets[1];
                ws.Name = "Rate_Y";
                DataTable dtupdate = new DataTable();
                Database.GetSqlData("TRANSFORM Max(DESCRIPTION.Rate_Y) AS MaxOfRate_Y SELECT DESCRIPTION.Description FROM (DESCRIPTION LEFT JOIN PACKING ON DESCRIPTION.Pack_id = PACKING.Pack_id) LEFT JOIN OTHER ON DESCRIPTION.Company_id = OTHER.Oth_id WHERE (((OTHER.Name)='" + companyname + "')) GROUP BY DESCRIPTION.Description, OTHER.Name ORDER BY PACKING.Pvalue DESC  PIVOT PACKING.Pvalue", dtupdate);


                string StrCon = "";

                StrCon = "";
                //columnheader
                int coln = 1;
                ws.get_Range(ws.Cells[1, 1], ws.Cells[1, dtupdate.Columns.Count]).Font.Bold = true;
                ws.get_Range(ws.Cells[2, 2], ws.Cells[dtupdate.Rows.Count + 1, dtupdate.Columns.Count]).NumberFormat = "#00.00";
                for (int j = 0; j < dtupdate.Columns.Count; j++)
                {
                    StrCon += dtupdate.Columns[j].ColumnName.Replace('_', '.') + "\t";

                    coln++;
                }
                StrCon += "\n";


                //rowsdata
                int lno = 2;
                for (int i = 0; i < dtupdate.Rows.Count; i++)
                {
                    int col = 1;
                    for (int j = 0; j < dtupdate.Columns.Count; j++)
                    {
                        StrCon += dtupdate.Rows[i][j].ToString() + "\t";
                        col++;
                    }
                    StrCon += "\n";
                    lno++;
                }

                Clipboard.SetText(StrCon);

                ws.Paste(misValue, misValue);
                Clipboard.Clear();
                ws.Columns.AutoFit();

                ws.Cells.Locked = false;

                ws.UsedRange.Cells.Borders.Color = System.Drawing.Color.Black.ToArgb();

                Microsoft.Office.Interop.Excel.FormatCondition format = (Microsoft.Office.Interop.Excel.FormatCondition)(ws.get_Range(ws.Cells[2, 2], ws.Cells[dtupdate.Rows.Count + 1, dtupdate.Columns.Count]).FormatConditions.Add(Microsoft.Office.Interop.Excel.XlFormatConditionType.xlExpression, Microsoft.Office.Interop.Excel.XlFormatConditionOperator.xlEqual, "=ISBLANK(A1)=TRUE", misValue, misValue, misValue, misValue, misValue));
                format.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Pink);




                Microsoft.Office.Interop.Excel.FormatCondition formatfcell = (Microsoft.Office.Interop.Excel.FormatCondition)(ws.get_Range(ws.Cells[2, 2], ws.Cells[dtupdate.Rows.Count + 1, dtupdate.Columns.Count]).FormatConditions.Add(Microsoft.Office.Interop.Excel.XlFormatConditionType.xlExpression, Microsoft.Office.Interop.Excel.XlFormatConditionOperator.xlEqual, "=ISBLANK(A1)=FALSE", misValue, misValue, misValue, misValue, misValue));
                formatfcell.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGreen);


                ws.get_Range(ws.Cells[1, 1], ws.Cells[dtupdate.Rows.Count + 1, 1]).Locked = true;
                ws.get_Range(ws.Cells[1, 1], ws.Cells[dtupdate.Rows.Count + 1, 1]).Cells.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightBlue);
                ws.get_Range(ws.Cells[1, 1], ws.Cells[dtupdate.Rows.Count + 1, 1]).Borders.Color = System.Drawing.Color.Black.ToArgb();
                ws.get_Range(ws.Cells[1, 1], ws.Cells[1, dtupdate.Columns.Count]).Locked = true;
                ws.get_Range(ws.Cells[1, 1], ws.Cells[1, dtupdate.Columns.Count]).Cells.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Blue);
                ws.get_Range(ws.Cells[1, 1], ws.Cells[1, dtupdate.Columns.Count]).Borders.Color = System.Drawing.Color.Black.ToArgb();
                string Password = "abc";
                ws.Protect(Password, misValue, misValue, misValue, misValue, misValue,
                misValue, misValue, misValue, misValue, misValue, misValue, misValue,
                misValue, misValue, misValue);



                //Rate_Z
                StrCon = "";
                ws = (Excel.Worksheet)wb.Worksheets[2];
                ws.Name = "Rate_Z";

                dtupdate.Clear();
                Database.GetSqlData("TRANSFORM Max(DESCRIPTION.Rate_Z) AS MaxOfRate_Z SELECT DESCRIPTION.Description FROM (DESCRIPTION LEFT JOIN PACKING ON DESCRIPTION.Pack_id = PACKING.Pack_id) LEFT JOIN OTHER ON DESCRIPTION.Company_id = OTHER.Oth_id WHERE (((OTHER.Name)='" + companyname + "')) GROUP BY DESCRIPTION.Description, OTHER.Name ORDER BY PACKING.Pvalue DESC  PIVOT PACKING.Pvalue", dtupdate);
                //columnheader
                coln = 1;
                ws.get_Range(ws.Cells[1, 1], ws.Cells[1, dtupdate.Columns.Count]).Font.Bold = true;
                ws.get_Range(ws.Cells[2, 2], ws.Cells[dtupdate.Rows.Count + 1, dtupdate.Columns.Count]).NumberFormat = "#00.00";
                for (int j = 0; j < dtupdate.Columns.Count; j++)
                {
                    StrCon += dtupdate.Columns[j].ColumnName.Replace('_', '.') + "\t";
                    coln++;
                }
                StrCon += "\n";

                //rowsdata
                lno = 2;
                for (int i = 0; i < dtupdate.Rows.Count; i++)
                {
                    int col = 1;
                    for (int j = 0; j < dtupdate.Columns.Count; j++)
                    {
                        StrCon += dtupdate.Rows[i][j].ToString() + "\t";
                        col++;
                    }
                    StrCon += "\n";
                    lno++;
                }

                Clipboard.SetText(StrCon);

                ws.Paste(misValue, misValue);
                Clipboard.Clear();
                ws.Columns.AutoFit();

                ws.Cells.Locked = false;

                ws.UsedRange.Cells.Borders.Color = System.Drawing.Color.Black.ToArgb();

                format = (Microsoft.Office.Interop.Excel.FormatCondition)(ws.get_Range(ws.Cells[2, 2], ws.Cells[dtupdate.Rows.Count + 1, dtupdate.Columns.Count]).FormatConditions.Add(Microsoft.Office.Interop.Excel.XlFormatConditionType.xlExpression, Microsoft.Office.Interop.Excel.XlFormatConditionOperator.xlEqual, "=ISBLANK(A1)=TRUE", misValue, misValue, misValue, misValue, misValue));
                format.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Pink);




                formatfcell = (Microsoft.Office.Interop.Excel.FormatCondition)(ws.get_Range(ws.Cells[2, 2], ws.Cells[dtupdate.Rows.Count + 1, dtupdate.Columns.Count]).FormatConditions.Add(Microsoft.Office.Interop.Excel.XlFormatConditionType.xlExpression, Microsoft.Office.Interop.Excel.XlFormatConditionOperator.xlEqual, "=ISBLANK(A1)=FALSE", misValue, misValue, misValue, misValue, misValue));
                formatfcell.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGreen);



                ws.get_Range(ws.Cells[1, 1], ws.Cells[dtupdate.Rows.Count + 1, 1]).Locked = true;
                ws.get_Range(ws.Cells[1, 1], ws.Cells[dtupdate.Rows.Count + 1, 1]).Cells.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightBlue);
                ws.get_Range(ws.Cells[1, 1], ws.Cells[dtupdate.Rows.Count + 1, 1]).Borders.Color = System.Drawing.Color.Black.ToArgb();
                ws.get_Range(ws.Cells[1, 1], ws.Cells[1, dtupdate.Columns.Count]).Locked = true;
                ws.get_Range(ws.Cells[1, 1], ws.Cells[1, dtupdate.Columns.Count]).Cells.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Blue);
                ws.get_Range(ws.Cells[1, 1], ws.Cells[1, dtupdate.Columns.Count]).Borders.Color = System.Drawing.Color.Black.ToArgb();
                ws.Protect(Password, misValue, misValue, misValue, misValue, misValue,
                misValue, misValue, misValue, misValue, misValue, misValue, misValue,
                misValue, misValue, misValue);




                //MRP
                StrCon = "";
                ws = (Excel.Worksheet)wb.Worksheets[3];
                ws.Name = "MRP";

                dtupdate.Clear();
                Database.GetSqlData("TRANSFORM Max(DESCRIPTION.MRP) AS MaxOfMRP SELECT DESCRIPTION.Description FROM (DESCRIPTION LEFT JOIN PACKING ON DESCRIPTION.Pack_id = PACKING.Pack_id) LEFT JOIN OTHER ON DESCRIPTION.Company_id = OTHER.Oth_id WHERE (((OTHER.Name)='" + companyname + "')) GROUP BY DESCRIPTION.Description, OTHER.Name ORDER BY PACKING.Pvalue DESC  PIVOT PACKING.Pvalue", dtupdate);
                //columnheader
                coln = 1;
                ws.get_Range(ws.Cells[1, 1], ws.Cells[1, dtupdate.Columns.Count]).Font.Bold = true;
                ws.get_Range(ws.Cells[2, 2], ws.Cells[dtupdate.Rows.Count, dtupdate.Columns.Count]).NumberFormat = "#00.00";
                for (int j = 0; j < dtupdate.Columns.Count; j++)
                {
                    StrCon += dtupdate.Columns[j].ColumnName.Replace('_', '.') + "\t";
                    coln++;
                }
                StrCon += "\n";

                //rowsdata
                lno = 2;
                for (int i = 0; i < dtupdate.Rows.Count; i++)
                {
                    int col = 1;
                    for (int j = 0; j < dtupdate.Columns.Count; j++)
                    {
                        StrCon += dtupdate.Rows[i][j].ToString() + "\t";
                        col++;
                    }
                    StrCon += "\n";
                    lno++;
                }
                Clipboard.SetText(StrCon);

                ws.Paste(misValue, misValue);
                Clipboard.Clear();
                ws.Columns.AutoFit();

                ws.Cells.Locked = false;

                ws.UsedRange.Cells.Borders.Color = System.Drawing.Color.Black.ToArgb();

                format = (Microsoft.Office.Interop.Excel.FormatCondition)(ws.get_Range(ws.Cells[2, 2], ws.Cells[dtupdate.Rows.Count + 1, dtupdate.Columns.Count]).FormatConditions.Add(Microsoft.Office.Interop.Excel.XlFormatConditionType.xlExpression, Microsoft.Office.Interop.Excel.XlFormatConditionOperator.xlEqual, "=ISBLANK(A1)=TRUE", misValue, misValue, misValue, misValue, misValue));
                format.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Pink);




                formatfcell = (Microsoft.Office.Interop.Excel.FormatCondition)(ws.get_Range(ws.Cells[2, 2], ws.Cells[dtupdate.Rows.Count + 1, dtupdate.Columns.Count]).FormatConditions.Add(Microsoft.Office.Interop.Excel.XlFormatConditionType.xlExpression, Microsoft.Office.Interop.Excel.XlFormatConditionOperator.xlEqual, "=ISBLANK(A1)=FALSE", misValue, misValue, misValue, misValue, misValue));
                formatfcell.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGreen);



                ws.get_Range(ws.Cells[1, 1], ws.Cells[dtupdate.Rows.Count + 1, 1]).Locked = true;
                ws.get_Range(ws.Cells[1, 1], ws.Cells[dtupdate.Rows.Count + 1, 1]).Cells.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightBlue);
                ws.get_Range(ws.Cells[1, 1], ws.Cells[dtupdate.Rows.Count + 1, 1]).Borders.Color = System.Drawing.Color.Black.ToArgb();
                ws.get_Range(ws.Cells[1, 1], ws.Cells[1, dtupdate.Columns.Count]).Locked = true;
                ws.get_Range(ws.Cells[1, 1], ws.Cells[1, dtupdate.Columns.Count]).Cells.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Blue);
                ws.get_Range(ws.Cells[1, 1], ws.Cells[1, dtupdate.Columns.Count]).Borders.Color = System.Drawing.Color.Black.ToArgb();
                ws.Protect(Password, misValue, misValue, misValue, misValue, misValue,
                misValue, misValue, misValue, misValue, misValue, misValue, misValue,
                misValue, misValue, misValue);



                //Rate_X
                StrCon = "";
                ws = (Excel.Worksheet)wb.Worksheets.Add(Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                ws.Name = "Rate_X";

                dtupdate.Clear();
                Database.GetSqlData("TRANSFORM Max(DESCRIPTION.Rate_X) AS MaxOfRate_X SELECT DESCRIPTION.Description FROM (DESCRIPTION LEFT JOIN PACKING ON DESCRIPTION.Pack_id = PACKING.Pack_id) LEFT JOIN OTHER ON DESCRIPTION.Company_id = OTHER.Oth_id WHERE (((OTHER.Name)='" + companyname + "')) GROUP BY DESCRIPTION.Description, OTHER.Name ORDER BY PACKING.Pvalue DESC  PIVOT PACKING.Pvalue", dtupdate);
                // columnheader
                coln = 1;
                ws.get_Range(ws.Cells[1, 1], ws.Cells[1, dtupdate.Columns.Count]).Font.Bold = true;
                ws.get_Range(ws.Cells[2, 2], ws.Cells[dtupdate.Rows.Count + 1, dtupdate.Columns.Count]).NumberFormat = "#00.00";
                for (int j = 0; j < dtupdate.Columns.Count; j++)
                {
                    StrCon += dtupdate.Columns[j].ColumnName.Replace('_', '.') + "\t";
                    coln++;
                }
                StrCon += "\n";

                //rowsdata
                lno = 2;
                for (int i = 0; i < dtupdate.Rows.Count; i++)
                {
                    int col = 1;
                    for (int j = 0; j < dtupdate.Columns.Count; j++)
                    {
                        StrCon += dtupdate.Rows[i][j].ToString() + "\t";
                        col++;
                    }
                    StrCon += "\n";
                    lno++;
                }
                Clipboard.SetText(StrCon);

                ws.Paste(misValue, misValue);
                Clipboard.Clear();
                ws.Columns.AutoFit();

                ws.Cells.Locked = false;

                ws.UsedRange.Cells.Borders.Color = System.Drawing.Color.Black.ToArgb();

                format = (Microsoft.Office.Interop.Excel.FormatCondition)(ws.get_Range(ws.Cells[2, 2], ws.Cells[dtupdate.Rows.Count + 1, dtupdate.Columns.Count]).FormatConditions.Add(Microsoft.Office.Interop.Excel.XlFormatConditionType.xlExpression, Microsoft.Office.Interop.Excel.XlFormatConditionOperator.xlEqual, "=ISBLANK(A1)=TRUE", misValue, misValue, misValue, misValue, misValue));
                format.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Pink);




                formatfcell = (Microsoft.Office.Interop.Excel.FormatCondition)(ws.get_Range(ws.Cells[2, 2], ws.Cells[dtupdate.Rows.Count + 1, dtupdate.Columns.Count]).FormatConditions.Add(Microsoft.Office.Interop.Excel.XlFormatConditionType.xlExpression, Microsoft.Office.Interop.Excel.XlFormatConditionOperator.xlEqual, "=ISBLANK(A1)=FALSE", misValue, misValue, misValue, misValue, misValue));
                formatfcell.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGreen);


                ws.get_Range(ws.Cells[1, 1], ws.Cells[dtupdate.Rows.Count + 1, 1]).Locked = true;
                ws.get_Range(ws.Cells[1, 1], ws.Cells[dtupdate.Rows.Count + 1, 1]).Cells.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightBlue);
                ws.get_Range(ws.Cells[1, 1], ws.Cells[dtupdate.Rows.Count + 1, 1]).Borders.Color = System.Drawing.Color.Black.ToArgb();
                ws.get_Range(ws.Cells[1, 1], ws.Cells[1, dtupdate.Columns.Count]).Locked = true;
                ws.get_Range(ws.Cells[1, 1], ws.Cells[1, dtupdate.Columns.Count]).Cells.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Blue);
                ws.get_Range(ws.Cells[1, 1], ws.Cells[1, dtupdate.Columns.Count]).Borders.Color = System.Drawing.Color.Black.ToArgb();
                ws.Protect(Password, misValue, misValue, misValue, misValue, misValue,
                misValue, misValue, misValue, misValue, misValue, misValue, misValue,
                misValue, misValue, misValue);



                //Retail Rate
                StrCon = "";
                ws = (Excel.Worksheet)wb.Worksheets.Add(Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                ws.Name = "Retail Rate";

                dtupdate.Clear();
                Database.GetSqlData("TRANSFORM Max(DESCRIPTION.Retail) AS MaxOfRetail SELECT DESCRIPTION.Description FROM (DESCRIPTION LEFT JOIN PACKING ON DESCRIPTION.Pack_id = PACKING.Pack_id) LEFT JOIN OTHER ON DESCRIPTION.Company_id = OTHER.Oth_id WHERE (((OTHER.Name)='" + companyname + "')) GROUP BY DESCRIPTION.Description, OTHER.Name ORDER BY PACKING.Pvalue DESC  PIVOT PACKING.Pvalue", dtupdate);
                //columnheader
                coln = 1;
                ws.get_Range(ws.Cells[1, 1], ws.Cells[1, dtupdate.Columns.Count]).Font.Bold = true;
                ws.get_Range(ws.Cells[2, 2], ws.Cells[dtupdate.Rows.Count + 1, dtupdate.Columns.Count]).NumberFormat = "#00.00";
                for (int j = 0; j < dtupdate.Columns.Count; j++)
                {
                    StrCon += dtupdate.Columns[j].ColumnName.Replace('_', '.') + "\t";
                    coln++;
                }
                StrCon += "\n";

                //rowsdata
                lno = 2;
                for (int i = 0; i < dtupdate.Rows.Count; i++)
                {
                    int col = 1;
                    for (int j = 0; j < dtupdate.Columns.Count; j++)
                    {
                        StrCon += dtupdate.Rows[i][j].ToString() + "\t";
                        col++;
                    }
                    StrCon += "\n";
                    lno++;
                }
                Clipboard.SetText(StrCon);
                ws.Paste(misValue, misValue);
                Clipboard.Clear();
                ws.Columns.AutoFit();
                ws.Cells.Locked = false;
                ws.UsedRange.Cells.Borders.Color = System.Drawing.Color.Black.ToArgb();
                format = (Microsoft.Office.Interop.Excel.FormatCondition)(ws.get_Range(ws.Cells[2, 2], ws.Cells[dtupdate.Rows.Count + 1, dtupdate.Columns.Count]).FormatConditions.Add(Microsoft.Office.Interop.Excel.XlFormatConditionType.xlExpression, Microsoft.Office.Interop.Excel.XlFormatConditionOperator.xlEqual, "=ISBLANK(A1)=TRUE", misValue, misValue, misValue, misValue, misValue));
                format.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Pink);
                formatfcell = (Microsoft.Office.Interop.Excel.FormatCondition)(ws.get_Range(ws.Cells[2, 2], ws.Cells[dtupdate.Rows.Count + 1, dtupdate.Columns.Count]).FormatConditions.Add(Microsoft.Office.Interop.Excel.XlFormatConditionType.xlExpression, Microsoft.Office.Interop.Excel.XlFormatConditionOperator.xlEqual, "=ISBLANK(A1)=FALSE", misValue, misValue, misValue, misValue, misValue));
                formatfcell.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGreen);
                ws.get_Range(ws.Cells[1, 1], ws.Cells[dtupdate.Rows.Count + 1, 1]).Locked = true;
                ws.get_Range(ws.Cells[1, 1], ws.Cells[dtupdate.Rows.Count + 1, 1]).Cells.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightBlue);
                ws.get_Range(ws.Cells[1, 1], ws.Cells[dtupdate.Rows.Count + 1, 1]).Borders.Color = System.Drawing.Color.Black.ToArgb();
                ws.get_Range(ws.Cells[1, 1], ws.Cells[1, dtupdate.Columns.Count]).Locked = true;
                ws.get_Range(ws.Cells[1, 1], ws.Cells[1, dtupdate.Columns.Count]).Cells.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Blue);
                ws.get_Range(ws.Cells[1, 1], ws.Cells[1, dtupdate.Columns.Count]).Borders.Color = System.Drawing.Color.Black.ToArgb();
                ws.Protect(Password, misValue, misValue, misValue, misValue, misValue,
                misValue, misValue, misValue, misValue, misValue, misValue, misValue,
                misValue, misValue, misValue);

                //Wholesale Rate
                StrCon = "";
                ws = (Excel.Worksheet)wb.Worksheets.Add(Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                ws.Name = "Wholesale Rate";

                dtupdate.Clear();
                Database.GetSqlData("TRANSFORM Max(DESCRIPTION.Wholesale) AS MaxOfWholesale SELECT DESCRIPTION.Description FROM (DESCRIPTION LEFT JOIN PACKING ON DESCRIPTION.Pack_id = PACKING.Pack_id) LEFT JOIN OTHER ON DESCRIPTION.Company_id = OTHER.Oth_id WHERE (((OTHER.Name)='" + companyname + "')) GROUP BY DESCRIPTION.Description, OTHER.Name ORDER BY PACKING.Pvalue DESC  PIVOT PACKING.Pvalue", dtupdate);
                //columnheader
                coln = 1;
                ws.get_Range(ws.Cells[1, 1], ws.Cells[1, dtupdate.Columns.Count]).Font.Bold = true;
                ws.get_Range(ws.Cells[2, 2], ws.Cells[dtupdate.Rows.Count + 1, dtupdate.Columns.Count]).NumberFormat = "#00.00";
                for (int j = 0; j < dtupdate.Columns.Count; j++)
                {
                    StrCon += dtupdate.Columns[j].ColumnName.Replace('_', '.') + "\t";
                    coln++;
                }
                StrCon += "\n";

                //rowsdata
                lno = 2;
                for (int i = 0; i < dtupdate.Rows.Count; i++)
                {
                    int col = 1;
                    for (int j = 0; j < dtupdate.Columns.Count; j++)
                    {
                        StrCon += dtupdate.Rows[i][j].ToString() + "\t";
                        col++;
                    }
                    StrCon += "\n";
                    lno++;
                }
                Clipboard.SetText(StrCon);
                ws.Paste(misValue, misValue);
                Clipboard.Clear();
                ws.Columns.AutoFit();
                ws.Cells.Locked = false;
                ws.UsedRange.Cells.Borders.Color = System.Drawing.Color.Black.ToArgb();
                format = (Microsoft.Office.Interop.Excel.FormatCondition)(ws.get_Range(ws.Cells[2, 2], ws.Cells[dtupdate.Rows.Count + 1, dtupdate.Columns.Count]).FormatConditions.Add(Microsoft.Office.Interop.Excel.XlFormatConditionType.xlExpression, Microsoft.Office.Interop.Excel.XlFormatConditionOperator.xlEqual, "=ISBLANK(A1)=TRUE", misValue, misValue, misValue, misValue, misValue));
                format.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Pink);
                formatfcell = (Microsoft.Office.Interop.Excel.FormatCondition)(ws.get_Range(ws.Cells[2, 2], ws.Cells[dtupdate.Rows.Count + 1, dtupdate.Columns.Count]).FormatConditions.Add(Microsoft.Office.Interop.Excel.XlFormatConditionType.xlExpression, Microsoft.Office.Interop.Excel.XlFormatConditionOperator.xlEqual, "=ISBLANK(A1)=FALSE", misValue, misValue, misValue, misValue, misValue));
                formatfcell.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGreen);
                ws.get_Range(ws.Cells[1, 1], ws.Cells[dtupdate.Rows.Count + 1, 1]).Locked = true;
                ws.get_Range(ws.Cells[1, 1], ws.Cells[dtupdate.Rows.Count + 1, 1]).Cells.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightBlue);
                ws.get_Range(ws.Cells[1, 1], ws.Cells[dtupdate.Rows.Count + 1, 1]).Borders.Color = System.Drawing.Color.Black.ToArgb();
                ws.get_Range(ws.Cells[1, 1], ws.Cells[1, dtupdate.Columns.Count]).Locked = true;
                ws.get_Range(ws.Cells[1, 1], ws.Cells[1, dtupdate.Columns.Count]).Cells.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Blue);
                ws.get_Range(ws.Cells[1, 1], ws.Cells[1, dtupdate.Columns.Count]).Borders.Color = System.Drawing.Color.Black.ToArgb();
                ws.Protect(Password, misValue, misValue, misValue, misValue, misValue,
                misValue, misValue, misValue, misValue, misValue, misValue, misValue,
                misValue, misValue, misValue);

                //Purchase Rate
                StrCon = "";
                ws = (Excel.Worksheet)wb.Worksheets.Add(Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                ws.Name = "Purchase Rate";

                dtupdate.Clear();
                Database.GetSqlData("TRANSFORM Max(DESCRIPTION.Purchase_rate) AS MaxOfPurchase_rate SELECT DESCRIPTION.Description FROM (DESCRIPTION LEFT JOIN PACKING ON DESCRIPTION.Pack_id = PACKING.Pack_id) LEFT JOIN OTHER ON DESCRIPTION.Company_id = OTHER.Oth_id WHERE (((OTHER.Name)='" + companyname + "')) GROUP BY DESCRIPTION.Description, OTHER.Name ORDER BY PACKING.Pvalue DESC  PIVOT PACKING.Pvalue", dtupdate);
                //columnheader
                coln = 1;
                ws.get_Range(ws.Cells[1, 1], ws.Cells[1, dtupdate.Columns.Count]).Font.Bold = true;
                ws.get_Range(ws.Cells[2, 2], ws.Cells[dtupdate.Rows.Count + 1, dtupdate.Columns.Count]).NumberFormat = "#00.00";

                for (int j = 0; j < dtupdate.Columns.Count; j++)
                {
                    StrCon += dtupdate.Columns[j].ColumnName.Replace('_', '.') + "\t";
                    coln++;
                }
                StrCon += "\n";

                //rowsdata
                lno = 2;
                for (int i = 0; i < dtupdate.Rows.Count; i++)
                {
                    int col = 1;
                    for (int j = 0; j < dtupdate.Columns.Count; j++)
                    {
                        StrCon += dtupdate.Rows[i][j].ToString() + "\t";
                        col++;
                    }
                    StrCon += "\n";
                    lno++;
                }

                Clipboard.SetText(StrCon);
                ws.Paste(misValue, misValue);
                Clipboard.Clear();
                ws.Columns.AutoFit();
                ws.Cells.Locked = false;
                ws.UsedRange.Cells.Borders.Color = System.Drawing.Color.Black.ToArgb();
                format = (Microsoft.Office.Interop.Excel.FormatCondition)(ws.get_Range(ws.Cells[2, 2], ws.Cells[dtupdate.Rows.Count + 1, dtupdate.Columns.Count]).FormatConditions.Add(Microsoft.Office.Interop.Excel.XlFormatConditionType.xlExpression, Microsoft.Office.Interop.Excel.XlFormatConditionOperator.xlEqual, "=ISBLANK(A1)=TRUE", misValue, misValue, misValue, misValue, misValue));
                format.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Pink);
                formatfcell = (Microsoft.Office.Interop.Excel.FormatCondition)(ws.get_Range(ws.Cells[2, 2], ws.Cells[dtupdate.Rows.Count + 1, dtupdate.Columns.Count]).FormatConditions.Add(Microsoft.Office.Interop.Excel.XlFormatConditionType.xlExpression, Microsoft.Office.Interop.Excel.XlFormatConditionOperator.xlEqual, "=ISBLANK(A1)=FALSE", misValue, misValue, misValue, misValue, misValue));
                formatfcell.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGreen);
                ws.get_Range(ws.Cells[1, 1], ws.Cells[dtupdate.Rows.Count + 1, 1]).Locked = true;
                ws.get_Range(ws.Cells[1, 1], ws.Cells[dtupdate.Rows.Count + 1, 1]).Cells.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightBlue);
                ws.get_Range(ws.Cells[1, 1], ws.Cells[dtupdate.Rows.Count + 1, 1]).Borders.Color = System.Drawing.Color.Black.ToArgb();
                ws.get_Range(ws.Cells[1, 1], ws.Cells[1, dtupdate.Columns.Count]).Locked = true;
                ws.get_Range(ws.Cells[1, 1], ws.Cells[1, dtupdate.Columns.Count]).Cells.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Blue);
                ws.get_Range(ws.Cells[1, 1], ws.Cells[1, dtupdate.Columns.Count]).Borders.Color = System.Drawing.Color.Black.ToArgb();
                ws.Protect(Password, misValue, misValue, misValue, misValue, misValue,
                misValue, misValue, misValue, misValue, misValue, misValue, misValue,
                misValue, misValue, misValue);
                apl.Visible = true;
            }
        }

        private void getColumnNameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataTable dtcombo = new DataTable();
            String strCombo = "Select Description from Description where Des_id=0";
            Database.GetSqlData(strCombo, dtcombo);
            dtcombo.Columns["Description"].ColumnName = "Rates";
            dtcombo.Rows.Add();
            dtcombo.Rows[0][0] = "Purchase_rate";

            dtcombo.Rows.Add();
            dtcombo.Rows[1][0] = "Retail";

            dtcombo.Rows.Add();
            dtcombo.Rows[2][0] = "Wholesale";

            dtcombo.Rows.Add();
            dtcombo.Rows[3][0] = "Rate_X";

            dtcombo.Rows.Add();
            dtcombo.Rows[4][0] = "Rate_Y";

            dtcombo.Rows.Add();
            dtcombo.Rows[5][0] = "Rate_Z";

            dtcombo.Rows.Add();
            dtcombo.Rows[6][0] = "MRP";

            object obj = SelectCombo.ComboDt(this, dtcombo, 1);
        }

        private void sMSLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frm_smslog frm = new frm_smslog();
            frm.MdiParent = this;
            frm.Loaddata();
            frm.Show();
        }

        private void removeSMSLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frm_removesmslog frm = new frm_removesmslog();
            frm.MdiParent = this;
            frm.Show();
        }

        private void importantDatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frm_impdates frm = new frm_impdates();
            frm.MdiParent = this;
            frm.Loaddata("0", "ReminderDates");
            frm.Show();
        }

        private void partysPriceListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report gg = new Report();
            string strCombo = "SELECT DISTINCT ACCOUNT.Name FROM PARTYRATE LEFT JOIN ACCOUNT ON PARTYRATE.Ac_id = ACCOUNT.Ac_id ORDER BY ACCOUNT.Name";

            char cg = 'a';
            string selected = SelectCombo.ComboKeypress(this, cg, strCombo, "", 1);
            gg.PartyPrice(Database.stDate, Database.ldate, selected);
            gg.MdiParent = this;
            gg.Show();
        }

        private void stockItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmMaster frm = new frmMaster();
            frm.MdiParent = this;
            frm.ProgrBar = toolStripProgressBar1;
            frm.LoadData("StockItem", "StockItem");
            frm.Show();
        }

        private void exitToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmChangePass frm = new frmChangePass();
            frm.MdiParent = this;
            frm.LoadData(Database.uname, "Change Password");
            frm.Show();
        }


        private void accountToolStripMenuItem1_Click_1(object sender, EventArgs e)
        {
            frmMaster frm = new frmMaster();
            frm.MdiParent = this;
            frm.ProgrBar = toolStripProgressBar1;
            frm.LoadData("Account", "Account");
            frm.Show();
        }

        private void accountGroupToolStripMenuItem1_Click_1(object sender, EventArgs e)
        {
            frmMaster frm = new frmMaster();
            frm.MdiParent = this;
            frm.LoadData("Account Group", "Account Group");
            frm.Show();
        }

        private void agentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmMaster frm = new frmMaster();
            frm.MdiParent = this;
            frm.LoadData("Broker", "Broker");
            frm.Show();
        }

        private void stockItemsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmMaster frm = new frmMaster();
            frm.MdiParent = this;
            frm.ProgrBar = toolStripProgressBar1;
            frm.LoadData("StockItem", "StockItem");
            frm.Show();
        }

        private void logoffToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Form[] frms = this.MdiChildren;
            foreach (Form frmC in frms)
            {
                frmC.Dispose();
            }
            Database.uname = "";
            Database.fname = "";
            Database.fyear = "";
            this.Text = "";
            statusStrip1.Items[2].Text = "";
            statusStrip1.Items[4].Text = "";
            statusStrip1.Items[9].Text = "+91 83070 71699";
            frmLogin frm = new frmLogin();
            frm.ShowInTaskbar = false;
            frm.ShowDialog(this);
            this.Hide();
        }

        private void importantDatesToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmMaster frm = new frmMaster();
            frm.MdiParent = this;
            frm.LoadData("ReminderDates", "ReminderDates");
            frm.Show();
        }

        private void packingToolStripMenuItem1_Click_1(object sender, EventArgs e)
        {

            frmMaster frm = new frmMaster();
            frm.MdiParent = this;
            frm.LoadData("Packing", "Packing");
            frm.Show();
        }

        private void taxCategoyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmMaster frm = new frmMaster();
            frm.MdiParent = this;
            frm.LoadData("TaxCategory", "TaxCategory");
            frm.Show();

        }

        private void discountChargesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmMaster frm = new frmMaster();
            frm.MdiParent = this;
            frm.LoadData("Charges", "Charges");
            frm.Show();
        }

        private void dataRestoreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Restore();
        }

        private void changeBackgroundImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ofd.Filter = "JPEG Files(*.jpg) | *.jpg";
            if (DialogResult.OK == ofd.ShowDialog())
            {
                this.BackgroundImage = new Bitmap(ofd.FileName);
                this.BackgroundImageLayout = ImageLayout.Stretch;
                GC.Collect();
                File.Copy(ofd.FileName, Application.StartupPath + "\\System\\" + Database.fname + ".jpg", true);
                MessageBox.Show("Done");
            }
        }

        private void createNewFinancialYearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frm_NewCompany frm = new frm_NewCompany();
            frm.frmMenuTyp = "New Financial Year";
            frm.NewFinancial("New Fianncial Year");
            frm.ShowDialog();

            Form[] frms = this.MdiChildren;
            foreach (Form frm1 in frms)
            {
                frm1.Dispose();
            }

            setMenu();
            statusStrip1.Items[2].Text = Database.ExeDate.ToString("yy.M.d");
            statusStrip1.Items[4].Text = Database.ldate.ToString(Database.dformat);
            statusStrip1.Items[9].Text = "+91 83070 71699";
            this.Text = Database.fname + "[" + Database.fyear + "]";
        }

        private void firmInformationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frm_NewCompany frm = new frm_NewCompany();
            DataTable dtFirm = new DataTable("FirmInfo");
            Database.GetOtherSqlData("select * from firminfo where Firm_name='" + Database.fname + "' And Firm_Period_name='" + Database.fyear + "'", dtFirm);
            if (dtFirm.Rows.Count > 0)
            {
                frm.LoadData(int.Parse(dtFirm.Rows[0]["f_id"].ToString()), "Modify Company");
                frm.ShowDialog(this);
                this.Text = Database.fname + "[" + Database.fyear + "]";
            }
        }

        public void backupsql()
        {


            // folderBrowserDialog1.ShowDialog();
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                DataSet ds = new DataSet();
                DataTable dtalltable = new DataTable();

                Database.GetSqlData("SELECT Table_Name as tablename FROM  " + Database.databaseName + ".INFORMATION_SCHEMA.TABLES WHERE  TABLE_TYPE = 'BASE TABLE' order by Table_Name", dtalltable);

                for (int i = 0; i < dtalltable.Rows.Count; i++)
                {
                    DataTable dtsingletable = new DataTable(dtalltable.Rows[i][0].ToString());
                    Database.GetSqlData("select * from " + dtalltable.Rows[i][0].ToString(), dtsingletable);
                    ds.Tables.AddRange(new DataTable[] { dtsingletable });
                }
                try
                {
                    string filename = Database.databaseName + DateTime.Now.ToString("yyyyMMddHHmmss");
                    string filePath = folderBrowserDialog1.SelectedPath + "\\" + filename;
                    ds.WriteXml(filePath);
                    ZipArchive zip = ZipFile.Open(filePath + ".zip", ZipArchiveMode.Create);
                    zip.CreateEntryFromFile(filePath, Path.GetFileName(filePath));
                    zip.Dispose();
                    File.Delete(filePath);

                    MessageBox.Show("Database BackUp has been created successful.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }





            //ds = new DataSet();

            //ds.ReadXml(filePath1);






        }
        private void dataBackupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //CreateBackup(); 
            int remainder = int.Parse(DateTime.Now.ToString("dd")) % 2;
          

                if (Feature.Available("IP Backup").ToUpper() == "YES")
                {
                    if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                    {
                        DownloadBackup obj = new DownloadBackup();
                        obj.strFoldePath = folderBrowserDialog1.SelectedPath;
                        obj.dbName = Database.databaseName;
                        obj.BackType = "MANUAL";
                        obj.ShowDialog(this);
                    }
                }
                else
                {
                   
                      

                      MessageBox.Show(" Database Backup Can Not Take On Window Drive ");
                    saveFileDialog1.Filter = "Text files (*.bak)|*.bak|All files (*.*)|*.*";
                    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            Database.CommandExecutor("Backup database " + Database.databaseName + " to disk='" + saveFileDialog1.FileName + "'",false);
                            MessageBox.Show("Database BackUp has been created successful.");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }


                 
                  
                  
                }


           
            //backupsql();            
        }

        private void controlRoomToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmMaster frm = new frmMaster();
            frm.LoadData("Control Room", "Control Room");
            frm.MdiParent = this;
            frm.Show();
        }

        private void tranjectionSetupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmMaster frm = new frmMaster();
            frm.LoadData("TransactionSetup", "TransactionSetup");
            frm.MdiParent = this;
            frm.Show();
        }

        private void eMailSetupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmMailer frm = new frmMailer();
            frm.MdiParent = this;
            frm.Show();
        }

        private void sMSSetupToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmSmsSetting frm = new frmSmsSetting();
            frm.MdiParent = this;
            frm.Show();
        }



        private void switchFirmToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form[] frms = this.MdiChildren;
            foreach (Form frm in frms)
            {
                frm.Dispose();
            }
            Database.prevUsr = statusStrip1.Items[2].Text;
            Database.fyear = "";
            this.Text = "";
            statusStrip1.Items[2].Text = "";
            statusStrip1.Items[4].Text = "";
            statusStrip1.Items[9].Text = "+91 83070 71699";
            Database.databaseName = "";
            //setMenu();
            Database.CloseConnection();
            frmbackup frm1 = new frmbackup();

            frm1.frmMenuTyp = "Use";
            frm1.Text = "Login as";
            frm1.ShowDialog(this);
            bool ch = frm1.ret;
            if (ch == true)
            {


              //  setMenu();
                setUserMenu();
                statusStrip1.Items[2].Text = Database.ExeDate.ToString("yy.M.d");
                statusStrip1.Items[4].Text = Database.ldate.ToString(Database.dformat);
                statusStrip1.Items[9].Text = "+91 83070 71699";
                this.Text = Database.fname + "[" + Database.fyear + "]";

            }

        }

        private void accountBalanceImportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmBalTransfer frm = new frmBalTransfer();
            frm.frmBalTrans = "Account";
            frm.MdiParent = this;
            frm.Text = "Balance Transfer";
            frm.LoadData();
            frm.Show();
        }

        private void stockItemBalanceImportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmBalTransfer frm = new frmBalTransfer();
            frm.frmBalTrans = "Stock";
            frm.Text = "Stock Transfer";
            frm.MdiParent = this;
            frm.LoadData();
            frm.Show();
        }

        private void companyManufacturerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmMaster frm = new frmMaster();
            frm.MdiParent = this;
            frm.LoadData("Company", "Company/Manufacturer");
            frm.Show();
        }

        private void brandItemGroupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmMaster frm = new frmMaster();
            frm.MdiParent = this;
            frm.LoadData("Item", "Item/Brand");
            frm.Show();
        }

        private void colorVariantToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmMaster frm = new frmMaster();
            frm.MdiParent = this;
            frm.LoadData("Colour", "Colour/Variant");
            frm.Show();
        }



        private void newFinancialYearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frm_NewCompany frm = new frm_NewCompany();
            frm.frmMenuTyp = "New Financial Year";
            frm.NewFinancial("New Fianncial Year");
            frm.ShowDialog();
            setMenu();
            statusStrip1.Items[2].Text = Database.ExeDate.ToString("yy.M.d");
            statusStrip1.Items[4].Text = Database.ldate.ToString(Database.dformat);
            statusStrip1.Items[9].Text = "+91 83070 71699";
            this.Text = Database.fname + "[" + Database.fyear + "]";
        }

        private void deleteFirmToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            DialogResult ch = MessageBox.Show(null, "Are you sure to Delete? \n All Data must be Lost of Current Login Firm.", "Confirm", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (ch == DialogResult.OK)
            {
                if (Database.databaseName != "")
                {
                    if (Database.DatabaseType == "access")
                    {
                        int fid = Database.GetOtherScalarInt("Select F_id from Firminfo where Firm_name='" + Database.fname + "' and Firm_Period_name='" + Database.fyear + "'");
                        Database.CommandExecutorOther("Delete from Firminfo where F_id=" + fid);
                        Database.CloseConnection();
                        if (Database.AccessCnn.State == ConnectionState.Open)
                        {
                            Database.CloseConnection();
                        }
                        File.Move(Application.StartupPath + "\\Database\\" + Database.databaseName + ".mdb", Application.StartupPath + "\\System\\" + DateTime.Now.ToString("yyyyMMddhmmff"));
                    }
                    else
                    {
                        if (Database.SqlCnn.State == ConnectionState.Open)
                        {
                            Database.CloseConnection();
                        }
                        string pathbackup = Application.StartupPath + "\\System\\" + Database.databaseName + DateTime.Now.ToString("yyyyMMddhmmff") + ".bak";
                        Database.CommandExecutor("Backup database " + Database.databaseName + " to disk='" + pathbackup + "'",false);
                    }
                }
                funs.notifyIcon.Visible = false;
                GC.Collect();
                Environment.Exit(0);
            }
        }

        private void vATAnnexureCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report gg = new Report();
            gg.AnnexureC(Database.stDate, Database.ldate);
            gg.MdiParent = this;
            gg.Show();
        }

        private void stockItemWiseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report gg = new Report();
            // gg.Stock(Database.stDate, Database.ldate);
            gg.MdiParent = this;
            gg.Show();
        }

        private void itemLiftingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report gg = new Report();
            string strCombo = "Select Name from Other where Type='SER14' order by Name";
            char cg = 'a';
            string selected = SelectCombo.ComboKeypress(this, cg, strCombo, "", 1);
            gg.ItemSold(Database.stDate, Database.ldate, selected);
            gg.MdiParent = this;
            gg.Show();
        }

        private void itemLiftingReportDetailedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report gg = new Report();
            string strCombo = "Select Name from Other where Type='SER14' order by Name";
            char cg = 'a';
            string selected = SelectCombo.ComboKeypress(this, cg, strCombo, "", 1);
            gg.ItemSoldDetail(Database.stDate, Database.ldate, selected);
            gg.MdiParent = this;
            gg.Show();
        }

        private void stockLiquidationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report gg = new Report();
            gg.StockLiquid(Database.ldate, Database.ldate);
            gg.MdiParent = this;
            gg.Show();
        }

        private void copyRatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frm_updaterate frm = new frm_updaterate();
            frm.MdiParent = this;
            frm.Show();
        }

        private void exportAllVouchersToolStripMenuItem_Click(object sender, EventArgs e)
        {

            frm_tallydate frm = new frm_tallydate();
            frm.MdiParent = this;
            frm.type = "Journal";
            frm.Show();
        }

        private void openingStockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmMasterVou frm = new frmMasterVou();
            frm.MdiParent = this;
            frm.LoadData("Opening", "Opening Stock Vouchers");
            frm.Show();

            //frmTransaction frm = new frmTransaction();
            //string vid = "";
            //vid = Database.GetScalarText("Select Vi_id from Voucherinfo where vt_id='" + funs.Select_vt_id("Opening Stock") + "'");
            //frm.LoadData(vid, "Opening", true, false, false);
            //frm.MdiParent = this;
            //frm.Show();

            //frm_openingstock frm = new frm_openingstock();
            //frm.MdiParent = this;
            //frm.WindowState = FormWindowState.Maximized;
            //frm.Show();
        }
        private void controlRoomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmMaster frm = new frmMaster();
            frm.MdiParent = this;
            frm.LoadData("Control Room", "Control Room");
            frm.Show();
        }

        private void transactionSetupToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            frmMaster frm = new frmMaster();
            frm.MdiParent = this;
            frm.LoadData("TransactionSetup", "TransactionSetup");
            frm.Show();
        }

        private void receiptToolStripMenuItem_Click(object sender, EventArgs e)
        {

            frmMasterVou frm = new frmMasterVou();
            frm.MdiParent = this;
            frm.LoadData("Receipt", "Receipt Vouchers");
            frm.Show();
        }

        private void paymentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmMasterVou frm = new frmMasterVou();
            frm.MdiParent = this;
            frm.LoadData("Payment", "Payment Vouchers");
            frm.Show();
        }

        private void creditNoteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmMasterVou frm = new frmMasterVou();
            frm.MdiParent = this;
            frm.LoadData("Cnote", "Credit Note Vouchers");
            frm.Show();
        }

        private void debitNoteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmMasterVou frm = new frmMasterVou();
            frm.MdiParent = this;
            frm.LoadData("Dnote", "Debit Note Vouchers");
            frm.Show();
        }

        private void purchaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmMasterVou frm = new frmMasterVou();
            frm.MdiParent = this;
            frm.LoadData("Purchase", "Purchase Vouchers");
            frm.ProgrBar = toolStripProgressBar1;
            frm.Show();
        }

        private void purchaseReturnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmMasterVou frm = new frmMasterVou();
            frm.MdiParent = this;
            frm.LoadData("P Return", "Purchase Return Vouchers");
            frm.ProgrBar = toolStripProgressBar1;
            frm.Show();
        }

        private void saleToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmMasterVou frm = new frmMasterVou();
            frm.MdiParent = this;
            frm.LoadData("Sale", "Sale Vouchers");
            frm.ProgrBar = toolStripProgressBar1;
            frm.Show();
        }

        private void saleReturnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmMasterVou frm = new frmMasterVou();
            frm.MdiParent = this;
            frm.LoadData("Return", "Sale Return Vouchers");
            frm.ProgrBar = toolStripProgressBar1;
            frm.Show();
        }

        private void journalToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            frmMasterVou frm = new frmMasterVou();
            frm.MdiParent = this;
            frm.LoadData("Journal", "Journal Vouchers");
            frm.Show();
        }

        private void stateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmMaster frm = new frmMaster();

            frm.MdiParent = this;
            frm.LoadData("State", "State");
            frm.Show();
        }

        private void pendingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmMasterVou frm = new frmMasterVou();
            frm.MdiParent = this;
            frm.LoadData("Pending", "Pending Vouchers");
            frm.Show();

        }

        private void stockReceiveToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmMasterVou frm = new frmMasterVou();
            frm.MdiParent = this;
            frm.LoadData("receive", "Stock Receive Vouchers");
            frm.Show();

        }

        private void stockIssueToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmMasterVou frm = new frmMasterVou();
            frm.MdiParent = this;
            frm.LoadData("issue", "Stock Issue Vouchers");
            frm.Show();
        }

        private void godownTransferToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmMasterVou frm = new frmMasterVou();
            frm.MdiParent = this;
            frm.LoadData("Transfer", "Godown Transfer Vouchers");
            frm.Show();
        }

        private void eToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frm_EmailLog frm = new frm_EmailLog();
            frm.MdiParent = this;
            frm.LoadData();
            frm.Show();
        }

        private void bulkUpdatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frm_sendmail frm = new frm_sendmail();
            frm.MdiParent = this;
            frm.LoadData();
            frm.Show();

        }

        private void copyRatesToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            frmMaster frm = new frmMaster();
            frm.MdiParent = this;
            frm.ProgrBar = toolStripProgressBar1;
            frm.LoadData("Copy Rate", "Copy Rate");
            frm.WindowState = FormWindowState.Maximized;
            frm.Show();
            //frm_updaterate frm = new frm_updaterate();
            //frm.MdiParent = this;
            //frm.Show();
        }

        private void otherDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frm_gridotherdet frm = new frm_gridotherdet();
            frm.MdiParent = this;
            frm.Show();
        }

        private void contraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmMasterVou frm = new frmMasterVou();
            frm.MdiParent = this;
            frm.LoadData("Contra", "Contra Vouchers");
            frm.Show();
        }

        private void stockJournalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmMasterVou frm = new frmMasterVou();
            frm.MdiParent = this;
            frm.LoadData("Transfer", "Stock Journal Vouchers");
            frm.Show();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmMaster frm = new frmMaster();
            frm.MdiParent = this;
            frm.LoadData("Tax", "TaxCategory");
            frm.Show();
        }

        private void paymentCollectorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmMaster frm = new frmMaster();
            frm.MdiParent = this;
            frm.LoadData("Payment Collector", "Payment Collector");
            frm.Show();
        }

        private void summarizedSaleRegisterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report gg = new Report();
            gg.SummSaleRegister(Database.stDate, Database.ldate);
            gg.MdiParent = this;
            gg.Show();
        }

        private void summarizedPurchaseRegisterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report gg = new Report();
            gg.SummPurchaseRegister(Database.stDate, Database.ldate);
            gg.MdiParent = this;
            gg.Show();
        }

        private void discountAfterTaxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmMaster frm = new frmMaster();
            frm.MdiParent = this;
            frm.LoadData("DAT", "Discount After Tax");
            frm.Show();
        }

        private void expensesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmMasterVou frm = new frmMasterVou();
            frm.MdiParent = this;
            frm.LoadData("RCM", "RCM Voucher");
            frm.Show();

        }

        private void gSTR3BToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frm_GSTR_3B frm = new frm_GSTR_3B();
            frm.MdiParent = this;
            frm.formattype = "pdf";
            frm.Show();
        }

        private void b2BInterStateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report gg = new Report();
            gg.B2BInterState(Database.cmonthFst, Database.ldate);
            gg.MdiParent = this;
            gg.Show();
        }

        private void b2BIntraStateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report gg = new Report();
            gg.B2BIntraState(Database.cmonthFst, Database.ldate);
            gg.MdiParent = this;
            gg.Show();
        }

        private void b2CIntraStateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report gg = new Report();
            gg.B2CIntraState(Database.cmonthFst, Database.ldate);
            gg.MdiParent = this;
            gg.Show();
        }

        private void b2CInterStateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report gg = new Report();
            gg.B2CInterState(Database.cmonthFst, Database.ldate);
            gg.MdiParent = this;
            gg.Show();
        }


        private void testToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            
        }

        private void purchaseUnregisteredIntrastateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report gg = new Report();
            gg.PUnRegisteredIntra(Database.cmonthFst, Database.ldate);
            gg.MdiParent = this;
            gg.Show();
        }

        private void purchaseRegisteredIntrastateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report gg = new Report();
            gg.PRegisteredIntra(Database.cmonthFst, Database.ldate);
            gg.MdiParent = this;
            gg.Show();
        }

        private void purchaseCompositionDealerIntrastateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report gg = new Report();
            gg.PCompositionIntra(Database.cmonthFst, Database.ldate);
            gg.MdiParent = this;
            gg.Show();
        }

        private void purchaseUnregisteredInterstateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report gg = new Report();
            gg.PUnRegisteredInter(Database.cmonthFst, Database.ldate);
            gg.MdiParent = this;
            gg.Show();
        }

        private void purchaseRegisteredInterstateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report gg = new Report();
            gg.PRegisteredInter(Database.cmonthFst, Database.ldate);
            gg.MdiParent = this;
            gg.Show();
        }

        private void purchaseCompositionDealerInterstateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report gg = new Report();
            gg.PCompositionInter(Database.cmonthFst, Database.ldate);
            gg.MdiParent = this;
            gg.Show();
        }

        private void needApprovalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //frm_needApp frm = new frm_needApp();
            //frm.MdiParent = this;
            //frm.Loaddata();
            //frm.Show();

            frm_Cashier frm = new frm_Cashier();
            frm.MdiParent = this;
            frm.Loaddata("Approve");
            frm.Show();
        }

        private void commoditySummaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report gg = new Report();
            gg.CommoditySummary(Database.cmonthFst, Database.ldate);
            gg.MdiParent = this;
            gg.Show();
        }

        private void commodityDetailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report gg = new Report();
            string strCombo = "SELECT TAXCATEGORY.Category_Name as HSNName, TAXCATEGORY.Commodity_Code as HSNCode FROM TAXCATEGORY ORDER BY TAXCATEGORY.Category_Name";
            char cg = 'a';
            string selected = SelectCombo.ComboKeypress(this, cg, strCombo, "", 1);
            gg.ParticularCommoditySummary(Database.cmonthFst, Database.ldate, selected);
            gg.MdiParent = this;
            gg.Show();
        }

        private void gSTR1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string filename1 = Application.StartupPath + "\\efile\\GSTR1.xlsx";

            if (File.Exists(filename1) == false)
            {
                MessageBox.Show("Please download the GSTR1 Template File.To download the file Follow these Steps: " + Environment.NewLine + "1. Please Make Sure Your System is connected to the Internet Connection." + Environment.NewLine + "2. Open the Browser and Write www.faspi.in in the address bar." + Environment.NewLine + "3. Now click on the Downloads Menu." + Environment.NewLine + "4. Select GSTR1 Efiling Template to download it." + Environment.NewLine + "5. After Downloading this File,Copy this File and Paste it under the Efile folder in your S/w.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                frm_GSTR1 frm = new frm_GSTR1();
                frm.MdiParent = this;
                frm.Show();
            }
        }

        private void changeVnoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int taxvno = 1;
            DataTable dtvou = new DataTable();
            Database.GetSqlData("SELECT * FROM VOUCHERINFO WHERE Vt_id = 'SER70' and branch_id='" + Database.BranchId + "' ORDER BY Vdate,Vnumber", dtvou);
            for (int i = 0; i < dtvou.Rows.Count; i++)
            {
                Database.CommandExecutor("Update Voucherinfo set vnumber=" + taxvno + " ,Invoiceno=" + taxvno.ToString() + " where Vi_id='" + dtvou.Rows[i]["Vi_id"].ToString() + "' ");
                taxvno++;
            }
            MessageBox.Show("Done");
        }

        private void hSNSummaryPurchaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report gg = new Report();
            gg.HSNPur(Database.cmonthFst, Database.ldate);
            gg.MdiParent = this;
            gg.Show();
        }

        private void hSNSummarySaleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report gg = new Report();
            gg.HSNSale(Database.cmonthFst, Database.ldate);
            gg.MdiParent = this;
            gg.Show();
        }

        private void saleRegisterHSNWiseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report gg = new Report();
            gg.SaleRegisterHsn(Database.cmonthFst, Database.ldate);
            gg.MdiParent = this;
            gg.Show();
        }

        private void purchaseRegisterHSNWiseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report gg = new Report();
            gg.PurchaseRegisterHsn(Database.cmonthFst, Database.ldate);
            gg.MdiParent = this;
            gg.Show();
        }

        private void debtorsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report gg = new Report();
            gg.Debtors(Database.stDate, Database.ldate);
            gg.MdiParent = this;
            gg.Show();
        }

        private void creditorsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report gg = new Report();
            gg.Creditors(Database.stDate, Database.ldate);
            gg.MdiParent = this;
            gg.Show();
        }

        private void debtorsAddressBookToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report gg = new Report();
            gg.Debtors(Database.stDate, Database.ldate);
            gg.MdiParent = this;
            gg.Show();
        }

        private void debToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report gg = new Report();
            gg.Debtors(Database.stDate, Database.ldate);
            gg.MdiParent = this;
            gg.Show();
        }

        private void creditorsToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            Report gg = new Report();
            gg.Creditors(Database.stDate, Database.ldate);
            gg.MdiParent = this;
            gg.Show();
        }

        private void continuousBillPrintingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string strCombo = "";
            //if (Database.BMode==)
            //{
            strCombo = "SELECT Name AS VoucherName FROM Vouchertype where Type='Sale' and active=" + access_sql.Singlequote + "True" + access_sql.Singlequote + " and " + Database.BMode + "=" + access_sql.Singlequote + "true" + access_sql.Singlequote;
            //}
            //else
            //{
            //    strCombo = "SELECT Name AS VoucherName FROM Vouchertype where Type='Sale' and active=" + access_sql.Singlequote + "True" + access_sql.Singlequote + " and B=" + access_sql.Singlequote + "true" + access_sql.Singlequote;
            //}
            char cg = ' ';
            string selected = SelectCombo.ComboKeypress(this, cg, strCombo, "", 2);

            if (selected != "")
            {
                frm_billprint frm = new frm_billprint();
                frm.MdiParent = this;
                frm.LoadData(selected, "Print Vouchers");
                frm.Show();
            }
        }

        private void graphicalReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                frmchart frm = new frmchart();
                frm.MdiParent = this;
                frm.frmtype = "purchase-sale";
                frm.Show();

            }
            catch (Exception ex)
            {
                if (Feature.Available("Display Chart") == "Yes")
                {
                    MessageBox.Show("Please install Chart control from the following Link : www.faspi.in/admin/downloads/MSChart.exe", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void receiptRegistorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report gg = new Report();
           string   wheresrt = "(Path LIKE '1;3;%')  OR   (Path LIKE '1;2;%')";

            string strCombo = funs.GetStrCombonew(wheresrt, "1=1");

            char cg = 'a';
            string selected = SelectCombo.ComboKeypress(this, cg, strCombo, "", 1);
            gg.ReceiptRegister(Database.ldate, Database.ldate,selected);
            gg.MdiParent = this;
            gg.Show();
        }

        private void paymentRegisterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report gg = new Report();
            string wheresrt = "(Path LIKE '1;3;%')  OR   (Path LIKE '1;2;%')";

            string strCombo = funs.GetStrCombonew(wheresrt, "1=1");

            char cg = 'a';
            string selected = SelectCombo.ComboKeypress(this, cg, strCombo, "", 1);
            gg.PaymentRegister(Database.ldate, Database.ldate, selected);


            gg.MdiParent = this;
            gg.Show();
        }

        private void saleOrderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmMasterVou frm = new frmMasterVou();
            frm.MdiParent = this;
            frm.LoadData("Sale Order", "Sale Order Vouchers");
            frm.Show();
        }

        private void pendingOrderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report gg = new Report();

            gg.PendingOrder(Database.stDate, Database.ldate);
            gg.MdiParent = this;
            gg.Show();
        }

        private void rCMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report gg = new Report();

            gg.RCMRegister(Database.stDate, Database.ldate);
            gg.MdiParent = this;
            gg.Show();
        }

        private void gSTR2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string filename1 = Application.StartupPath + "\\efile\\GSTR2.xlsx";

            if (File.Exists(filename1) == false)
            {
                MessageBox.Show("Please download the GSTR2 Template File.To download the file Follow these Steps: " + Environment.NewLine + "1. Please Make Sure Your System is connected to the Internet Connection." + Environment.NewLine + "2. Open the Browser and Write www.faspi.in in the address bar." + Environment.NewLine + "3. Now click on the Downloads Menu." + Environment.NewLine + "4. Select GSTR2 Efiling Template to download it." + Environment.NewLine + "5. After Downloading this File,Copy this File and Paste it under the Efile folder in your S/w.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                frm_GSTR2 frm = new frm_GSTR2();
                frm.MdiParent = this;
                frm.Show();
            }
        }

        private void gSTR2AMatchingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frm_Gstr2A frm = new frm_Gstr2A();
            frm.MdiParent = this;
            frm.Show();
        }

        private void exportPaymentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frm_tallydate frm = new frm_tallydate();
            frm.MdiParent = this;
            frm.type = "Payment";
            frm.Show();
        }

        private void exportPurchaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frm_tallydate frm = new frm_tallydate();
            frm.MdiParent = this;
            frm.type = "Purchase";
            frm.Show();
        }

        private void exportContraVouchersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frm_tallydate frm = new frm_tallydate();
            frm.MdiParent = this;
            frm.type = "Contra";
            frm.Show();
        }

        private void exportDebiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frm_tallydate frm = new frm_tallydate();
            frm.MdiParent = this;
            frm.type = "Dnote";
            frm.Show();
        }

        private void exportCreditNoteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frm_tallydate frm = new frm_tallydate();
            frm.MdiParent = this;
            frm.type = "Cnote";
            frm.Show();
        }

        private void daySummaryToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Report frm = new Report();
            frm.DaySummary(Database.ldate, Database.ldate);
            frm.MdiParent = this;
            frm.Show();
        }

        private void purchaseWithDebitNoteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmMasterVou frm = new frmMasterVou();
            frm.MdiParent = this;
            frm.LoadData("PWDebitNote", "Purchase With Debit Note Vouchers");
            frm.Show();
        }

        private void crossTabSaleRegisterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report gg = new Report();

            gg.PartyWiseSale(Database.ldate, Database.ldate);
            gg.MdiParent = this;
            gg.Show();
        }

        private void bBankBookToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report gg = new Report();
            string wheresrt = "(Path LIKE '1;3;%')  OR   (Path LIKE '1;2;%')";

            string strCombo = funs.GetStrCombonew(wheresrt, "1=1");
            char cg = 'a';
            string selected = SelectCombo.ComboKeypress(this, cg, strCombo, "", 1);
            gg.BankBook(Database.stDate, Database.ldate, selected);
            gg.MdiParent = this;
            gg.Show();
        }

        private void newAcshBookToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report gg = new Report();
            string wheresrt = "(Path LIKE '1;3;%')  OR   (Path LIKE '1;2;%')";

            string strCombo = funs.GetStrCombonew(wheresrt, "1=1");
            char cg = 'a';
            string selected = SelectCombo.ComboKeypress(this, cg, strCombo, "", 1);
            gg.NewBook(Database.stDate, Database.ldate, selected);
            gg.MdiParent = this;
            gg.Show();
        }

        private void gSTR3BExcelToolStripMenuItem_Click(object sender, EventArgs e)
        {

            string filename1 = Application.StartupPath + "\\efile\\GSTR3B.xls";

            if (File.Exists(filename1) == false)
            {
                MessageBox.Show("Please download the GSTR3B Template File.To download the file Follow these Steps: " + Environment.NewLine + "1. Please Make Sure Your System is connected to the Internet Connection." + Environment.NewLine + "2. Open the Browser and Write www.faspi.in in the address bar." + Environment.NewLine + "3. Now click on the Downloads Menu." + Environment.NewLine + "4. Select GSTR3B Template to download it." + Environment.NewLine + "5. After Downloading this File,Copy this File and Paste it under the Efile folder in your S/w.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                frm_GSTR_3B frm = new frm_GSTR_3B();
                frm.MdiParent = this;
                frm.formattype = "excel";
                frm.Show();
            }



        }

        private void userManagementToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
        }

        private void statementOfAffairToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report gg = new Report();
            gg.StatementofAffair(Database.stDate, Database.ldate);
            gg.MdiParent = this;
            gg.Show();
        }

        private void crossTabPurchaseRegisterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report gg = new Report();
            gg.PartyWisePurchase(Database.ldate, Database.ldate);
            gg.MdiParent = this;
            gg.Show();
        }

        private void cashBankSummaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report gg = new Report();
            gg.Register(Database.ldate, Database.ldate);
            gg.MdiParent = this;
            gg.Show();
        }

        private void saleRegisterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report gg = new Report();
            gg.SaleRegisterHsnNew(Database.ldate, Database.ldate);
            gg.MdiParent = this;
            gg.Show();
        }

        private void purchaseRegisterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report gg = new Report();
            gg.PurchaseRegisterHsnNew(Database.stDate, Database.ldate);
            gg.MdiParent = this;
            gg.Show();
        }

        private void priceGroupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmMaster frm = new frmMaster();
            frm.MdiParent = this;
            frm.LoadData("PriceGroup", "Group");
            frm.Show();
        }

        private void cashierToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frm_Cashier frm = new frm_Cashier();
            frm.MdiParent = this;
            frm.Loaddata("Cashier");
            frm.Show();
        }

        private void godownInOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report gg = new Report();
            if (Feature.Available("Company Colour") == "No")
            {
                string godown = "";
                string godownname = "";
                char cg = 'a';
                if (Feature.Available("Multi-Godown") == "Yes")
                {
                    godown = "select distinct '<MAIN>' as name from account union all SELECT ACCOUNT.Name as name FROM ACCOUNT LEFT JOIN ACCOUNTYPE ON ACCOUNT.Act_id = ACCOUNTYPE.Act_id WHERE ACCOUNTYPE.Name='Godown' and Account.Branch_id='" + Database.BranchId + "' GROUP BY ACCOUNT.Name";
                    godownname = SelectCombo.ComboKeypress(this, cg, godown, "", 0);
                    gg.GodownInOut(Database.ldate, Database.ldate, godownname);
                }
            }
            else
            {
                string godown = "";
                string godownname = "";
                char cg = 'a';
                if (Feature.Available("Multi-Godown") == "Yes")
                {
                    godown = "select distinct '<MAIN>' as name from account union all SELECT ACCOUNT.Name as name FROM ACCOUNT LEFT JOIN ACCOUNTYPE ON ACCOUNT.Act_id = ACCOUNTYPE.Act_id WHERE ACCOUNTYPE.Name='Godown' and Account.Branch_id='" + Database.BranchId + "' GROUP BY ACCOUNT.Name";
                    godownname = SelectCombo.ComboKeypress(this, cg, godown, "", 0);
                    gg.GodownInOut(Database.ldate, Database.ldate, godownname);
                }
            }
            gg.MdiParent = this;
            gg.Show();
        }

        private void cashCreditSaleToolStripMenuItem_Click(object sender, EventArgs e)
        {

            Report gg = new Report();
            DataTable dt = new DataTable();
            dt.Columns.Add("Type", typeof(string));
            dt.Rows.Add();
            dt.Rows[0][0] = "Cash";
            dt.Rows.Add();
            dt.Rows[1][0] = "Credit";
            dt.Rows.Add();
            dt.Rows[2][0] = "Both";
            string selected = SelectCombo.ComboDt(this, dt, 0);
            gg.CashCreditSale(Database.ldate, Database.ldate, selected);
            gg.MdiParent = this;
            gg.Show();
        }

        private void dcToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void testToolStripMenuItem1_Click(object sender, EventArgs e)
        {

            var SqlCnnClient = @"Data Source=" + Database.inipath + ";Initial Catalog=" + Database.databaseName + ";Persist Security Info=True;User ID=sa;password=" + Database.sqlseverpwd + "";
            var SqlCnnSer = @"Data Source=" + Database.SHostname + ";Initial Catalog=" + Database.SDbname + ";Persist Security Info=True;User ID=" + Database.SUsername + ";password=" + Database.SPwd;
            DataTable dt = new DataTable();
            using (SqlConnection connectionClient = new SqlConnection(SqlCnnClient))
            {
                using (SqlDataAdapter da = new SqlDataAdapter("select * from Syncronizer order by id", connectionClient))
                {
                    da.Fill(dt);
                }
            }

            using (SqlConnection connection = new SqlConnection(SqlCnnSer))
            {
                SqlCommand command;
                connection.Open();

                    DataTable dtbatch = new DataTable();
                    dtbatch.Columns.Add("query",typeof(string));
                    string sql = "";
                    int rowno = 0;
                    for (int i = rowno; rowno < dt.Rows.Count; rowno++)
                    {
                        int count = 0;
                        dtbatch.Rows.Add();
                        for(int k=0;k<10;k++)
                        {

                            dtbatch.Rows[dtbatch.Rows.Count - 1]["query"] += dt.Rows[rowno]["query"].ToString() + "; ";

                          
                            if (k < 9 && dt.Rows.Count-1 != rowno)
                            {

                                rowno++;
                            }
                        }
                        
                        //sql += dt.Rows[i]["query"].ToString() + ";";
                        
                    }

                    SqlTransaction sqlTran = connection.BeginTransaction();
                    try
                    {
                        for (int i = 0; i < dtbatch.Rows.Count; i++)
                        {

                            try
                            {
                                command = new SqlCommand(dtbatch.Rows[i]["query"].ToString(), connection);
                                command.Transaction = sqlTran;
                                command.ExecuteNonQuery();

                            }
                            catch (Exception exb)
                            {
                                MessageBox.Show(exb.Message + Environment.NewLine + " Not Done... Try Again Line No");
                            }
                        }

                      //  int rowsAffected = command.ExecuteNonQuery();
                   
                    sqlTran.Commit();
                }
                catch (Exception exp)
                {
                    MessageBox.Show(exp.Message + Environment.NewLine + " Not Done... Try Again Line No");
                    sqlTran.Rollback();
                    return;
                }

            }
            using (SqlConnection connection = new SqlConnection(SqlCnnClient))
            {
                connection.Open();
                SqlTransaction sqlTran = connection.BeginTransaction();
                SqlCommand command = connection.CreateCommand();
                command.Transaction = sqlTran;

                try
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        command.CommandText = "delete from Syncronizer where id=" + dt.Rows[i]["id"].ToString();
                        int rowsAffected = command.ExecuteNonQuery();
                    }
                    sqlTran.Commit();
                    MessageBox.Show("Done");
                }
                catch (Exception exp)
                {
                    MessageBox.Show(exp.Message);
                    sqlTran.Rollback();
                    MessageBox.Show("Not Done... Try Again");
                }

            }
        }

        private void cashierReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report frm = new Report();
          //  string strCombo = "Select  distinct  'All' as Name from Account Union all SELECT Uname as Name FROM Userinfo WHERE  (Branch_id = '" + Database.BranchId + "') AND (Utype = 'SUPERADMIN') OR (Utype = 'CASHIER')";
            string strCombo = "Select  distinct  'All' as Name from Account Union all SELECT Uname as Name FROM Userinfo WHERE  (Branch_id = '" + Database.BranchId + "') ";
            char cg = 'a';
            string selected = SelectCombo.ComboKeypress(this, cg, strCombo, "", 1);
            frm.CashierReport(Database.ldate, Database.ldate, selected);
            frm.MdiParent = this;
            frm.Show();
        }

        private void dayWiseStockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form1 frm = new Form1();
            frm.ReportName = "StockSummary";
            frm.Show();


            //Report gg = new Report();
            //gg.MdiParent = this;
            //gg.DayWiseReport(Database.ldate, Database.ldate);
            //gg.Show();
        }

        private void descriptionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OleDbConnection AccessConn1 = new OleDbConnection();
            AccessConn1.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Database.ServerPath + "\\Database\\ATC1718GST.mdb;Persist Security Info=true;Jet OLEDB:Database Password=ptsoft9358524971";
            AccessConn1.Open();
            DataTable dt = new DataTable();
            DataTable dtsql = new DataTable("description");
            Database.GetSqlData("Select * from description", dtsql);
            OleDbDataAdapter da = new OleDbDataAdapter("Select * from description", AccessConn1);

            da.Fill(dt);

            for (int i = 0; i < dtsql.Rows.Count; i++)
            {
                if (dt.Select("description='" + dtsql.Rows[i]["description"].ToString() + "' and pack='" + dtsql.Rows[i]["pack"].ToString() + "'").Length != 0)
                {

                    dtsql.Rows[i]["retail"] = double.Parse(dt.Select("description='" + dtsql.Rows[i]["description"].ToString() + "'  and pack='" + dtsql.Rows[i]["pack"].ToString() + "'").FirstOrDefault()["retail"].ToString());


                }
            }
            Database.SaveData(dtsql);

        }

        private void containerToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void customerBillDueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report gg = new Report();
            gg.MdiParent = this;
            gg.CustomerBillDue(Database.ldate, Database.ldate, "");
            gg.Show();
        }

        private void supplierBillDueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report gg = new Report();
            gg.MdiParent = this;
            gg.SupplierBillDue(Database.ldate, Database.ldate, "");
            gg.Show();
        }

        private void containerToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            frmMaster frm = new frmMaster();
            frm.MdiParent = this;
            frm.LoadData("Container", "Container");
            frm.Show();
        }

        private void saleRegisterNewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sale_register frm = new sale_register();
            frm.calledindirect = false;
            frm.frmtext = "Sale Register";
            frm.ShowDialog();
        }

        private void purchaseRegisterToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            sale_register frm = new sale_register();
            frm.frmtext = "Purchase Register";
            frm.calledindirect = false;
            frm.MdiParent = this;
            frm.Show();
        }



        private void searchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frm_search frm = new frm_search();
            frm.ShowDialog();
        }

        private void priceListToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frm_priceList frm = new frm_priceList();
            frm.MdiParent = this;
            frm.Show();
        }

        private void reorderManagementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frm_price_list frm = new frm_price_list();
            frm.MdiParent = this;
            frm.typ = "Reorder";
            frm.Show();
        }

        private void brokersListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report gg = new Report();
            gg.MdiParent = this.MdiParent;
            gg.Broker(Database.stDate, Database.ldate);
            gg.Show();
        }

        private void salesManToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmMaster frm = new frmMaster();

            frm.MdiParent = this;
            frm.LoadData("Salesman", "Salesman");
            frm.Show();
        }

        private void pakingCategoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmMaster frm = new frmMaster();

            frm.MdiParent = this;
            frm.LoadData("PackCategory", "PackCategory");
            frm.Show();


        }

        private void customerWiseToolStripMenuItem_Click(object sender, EventArgs e)
        {

            //Report gg = new Report();
            //string strCombo = "SELECT name from SalesMan order by name";
            //char cg = 'a';
            //string selected = SelectCombo.ComboKeypress(this, cg, strCombo, "", 1);
            //gg.SalesManDetailCustomerWise(Database.stDate, Database.enDate, selected);
            //gg.MdiParent = this;
            //gg.Show();
        }

        private void productionFormulaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmMaster frm = new frmMaster();
            frm.MdiParent = this;
            frm.LoadData("ProductFormula", "ProductFormula");
            frm.Show();
        }

        private void productionReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void branchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Database.CommandExecutor("Update Branch set id='MAN' where id='SER1'");

            //Database.CommandExecutor("Alter table Branch Add oldid nvarchar(4) ");

            //Database.CommandExecutor("Update Branch set id='MAN',oldid='SER1'  where id='SER1'");
            //Database.CommandExecutor("Update Branch set id='INM',oldid='SER2' where id='SER2'");
            DataTable dtbranch = new DataTable();
            Database.GetSqlData("Select * from Branch order by oldid", dtbranch);

            //for (int i = 0; i < dtbranch.Rows.Count; i++)
            //{
            //    DataTable dtAccount = new DataTable("Account");
            //    Database.GetSqlData("Select * from Account where Branch_id='" + dtbranch.Rows[i]["oldid"].ToString() + "'", dtAccount);
            //    for (int j = 0; j < dtAccount.Rows.Count; j++)
            //    {

            //        string oldac_id = dtAccount.Rows[j]["Ac_id"].ToString();

            //        string oldlocid = dtbranch.Rows[i]["oldid"].ToString();
            //        string newlocid = dtbranch.Rows[i]["id"].ToString();
            //        string newac_id = newlocid + dtAccount.Rows[j]["Nid"].ToString();
            //        Database.CommandExecutor("Update account set Branch_id='" + newlocid + "', locationid='" + newlocid + "',Ac_id='" + newac_id + "' where Branch_id='" + oldlocid + "' and Ac_id='" + oldac_id + "' ");
            //        Database.CommandExecutor("Update BILLBYBILL set Ac_id='" + newac_id + "' where Ac_id='" + oldac_id + "'");
            //        Database.CommandExecutor("Update CHARGES set Ac_id='" + newac_id + "' where Ac_id='" + oldac_id + "'");
            //        Database.CommandExecutor("Update DisAfterTax set Ac_id='" + newac_id + "' where Ac_id='" + oldac_id + "'");
            //        Database.CommandExecutor("Update importantdate set Ac_id='" + newac_id + "' where Ac_id='" + oldac_id + "'");
            //        Database.CommandExecutor("Update ITEMCHARGES set Accid='" + newac_id + "' where Accid='" + oldac_id + "'");
            //        Database.CommandExecutor("Update Journal set Ac_id='" + newac_id + "' where Ac_id='" + oldac_id + "'");


            //        Database.CommandExecutor("Update PARTYRATE set Ac_id='" + newac_id + "' where Ac_id='" + oldac_id + "'");
            //        Database.CommandExecutor("Update Stock set godown_id='" + newac_id + "' where godown_id='" + oldac_id + "'");
            //        Database.CommandExecutor("Update TAXCATEGORY set PA='" + newac_id + "' where PA='" + oldac_id + "'");
            //        Database.CommandExecutor("Update TAXCATEGORY set SA='" + newac_id + "' where SA='" + oldac_id + "'");
            //        Database.CommandExecutor("Update TAXCATEGORY set PAEX='" + newac_id + "' where PAEX='" + oldac_id + "'");
            //        Database.CommandExecutor("Update TAXCATEGORY set SAEX='" + newac_id + "' where SAEX='" + oldac_id + "'");
            //        Database.CommandExecutor("Update TAXCATEGORY set PCA='" + newac_id + "' where PCA='" + oldac_id + "'");
            //        Database.CommandExecutor("Update TAXCATEGORY set SCA='" + newac_id + "' where SCA='" + oldac_id + "'");
            //        Database.CommandExecutor("Update TAXCATEGORY set PCAEX='" + newac_id + "' where PCAEX='" + oldac_id + "'");
            //        Database.CommandExecutor("Update TAXCATEGORY set SCAEX='" + newac_id + "' where SCAEX='" + oldac_id + "'");
            //        Database.CommandExecutor("Update TAXCATEGORY set PTA1='" + newac_id + "' where PTA1='" + oldac_id + "'");
            //        Database.CommandExecutor("Update TAXCATEGORY set PTA2='" + newac_id + "' where PTA2='" + oldac_id + "'");
            //        Database.CommandExecutor("Update TAXCATEGORY set PTA3='" + newac_id + "' where PTA3='" + oldac_id + "'");
            //        Database.CommandExecutor("Update TAXCATEGORY set STA1='" + newac_id + "' where STA1='" + oldac_id + "'");
            //        Database.CommandExecutor("Update TAXCATEGORY set STA2='" + newac_id + "' where STA2='" + oldac_id + "'");
            //        Database.CommandExecutor("Update TAXCATEGORY set STA3='" + newac_id + "' where STA3='" + oldac_id + "'");
            //        Database.CommandExecutor("Update TAXCATEGORY set RCMPay=" + newac_id + " where RCMPay='" + oldac_id + "'");
            //        Database.CommandExecutor("Update TAXCATEGORY set RCMITC='" + newac_id + "' where RCMITC='" + oldac_id + "'");
            //        Database.CommandExecutor("Update TAXCATEGORY set RCMEli='" + newac_id + "' where RCMEli='" + oldac_id + "'");
            //        Database.CommandExecutor("Update VOUCHERDET set godown_id='" + newac_id + "' where godown_id='" + oldac_id + "'");
            //        Database.CommandExecutor("Update VOUCHERDET set pur_sale_acc='" + newac_id + "' where pur_sale_acc='" + oldac_id + "'");
            //        Database.CommandExecutor("Update VOUCHERDET set tax1='" + newac_id + "' where tax1='" + oldac_id + "'");
            //        Database.CommandExecutor("Update VOUCHERDET set tax2='" + newac_id + "' where tax2='" + oldac_id + "'");
            //        Database.CommandExecutor("Update VOUCHERDET set tax3='" + newac_id + "' where tax3='" + oldac_id + "'");
            //        Database.CommandExecutor("Update VOUCHERDET set tax4='" + newac_id + "' where tax4='" + oldac_id + "'");
            //        Database.CommandExecutor("Update VOUCHERDET set DATac_id='" + newac_id + "' where DATac_id='" + oldac_id + "'");
            //        Database.CommandExecutor("Update VOUCHERDET set RCMac_id='" + newac_id + "' where RCMac_id='" + oldac_id + "'");
            //        Database.CommandExecutor("Update VOUCHARGES set Accid='" + newac_id + "' where Accid='" + oldac_id + "'");
            //        Database.CommandExecutor("Update VOUCHERACTOTAL set Accid='" + newac_id + "' where Accid='" + oldac_id + "'");
            //        Database.CommandExecutor("Update Voucherinfo set ac_id='" + newac_id + "' where ac_id='" + oldac_id + "'");
            //        Database.CommandExecutor("Update Voucherpaydet set acc_id='" + newac_id + "' where acc_id='" + oldac_id + "'");
            //        Database.CommandExecutor("Update Voucherinfo set ac_id2='" + newac_id + "' where ac_id2='" + oldac_id + "'");

            //        Database.CommandExecutor("Update Journal set opp_Acid='" + newac_id + "' where opp_Acid='" + oldac_id + "'");
            //        Database.CommandExecutor("Update Voucherinfo set Dr_Ac_id='" + newac_id + "' where Dr_Ac_id='" + oldac_id + "'");
            //        Database.CommandExecutor("Update Voucherinfo set Cr_Ac_id='" + newac_id + "' where Cr_Ac_id='" + oldac_id + "'");
            //        Database.CommandExecutor("Update Description set godown_id='" + newac_id + "' where godown_id='" + oldac_id + "'");
            //        Database.CommandExecutor("Update Contractor set Reff_id='" + newac_id + "' where Reff_id='" + oldac_id + "'");



            //    }





            //}
            //DataTable dtcontractor = new DataTable();
            //Database.GetSqlData("Select * from contractor", dtcontractor);
            //for (int k = 0; k < dtcontractor.Rows.Count; k++)
            //{
            //    string newlocid = "MAN";
            //    string newconid = newlocid + dtcontractor.Rows[k]["Nid"].ToString();
            //    string oldconid = dtcontractor.Rows[k]["Con_id"].ToString();
            //    Database.CommandExecutor("update contractor set Con_id='" + newconid + "', locationid='" + newlocid + "' where Con_id='" + oldconid + "' ");
            //    Database.CommandExecutor("update Account set Con_id='" + newconid + "' where Con_id='" + oldconid + "' ");
            //    Database.CommandExecutor("update Voucherinfo set Conn_id='" + newconid + "' where Conn_id='" + oldconid + "' ");
            //}
            for (int i = 0; i < dtbranch.Rows.Count; i++)
            {
                DataTable dtvoucherinfo = new DataTable();
                Database.GetSqlData("Select * from Voucherinfo where branch_id='" + dtbranch.Rows[i]["oldid"].ToString() + "'", dtvoucherinfo);
                for (int j = 0; j < dtvoucherinfo.Rows.Count; j++)
                {
                    string oldvi_id = dtvoucherinfo.Rows[j]["Vi_id"].ToString();
                    string oldlocid = dtbranch.Rows[i]["oldid"].ToString();
                    string newlocid = dtbranch.Rows[i]["id"].ToString();
                    string newvi_id = newlocid + dtvoucherinfo.Rows[j]["Nid"].ToString();
                    Database.CommandExecutor("Update Voucherinfo set Branch_id='" + newlocid + "', locationid='" + newlocid + "',Vi_id='" + newvi_id + "' where Branch_id='" + oldlocid + "' and Vi_id='" + oldvi_id + "' ");
                    Database.CommandExecutor("Update Voucherdet set Vi_id='" + newvi_id + "' where  Vi_id='" + oldvi_id + "' ");
                    Database.CommandExecutor("Update Journal set Vi_id='" + newvi_id + "' where  Vi_id='" + oldvi_id + "' ");
                    Database.CommandExecutor("Update Voucherpaydet set Vi_id='" + newvi_id + "' where  Vi_id='" + oldvi_id + "' ");
                    Database.CommandExecutor("Update Voucheractotal set Vi_id='" + newvi_id + "' where  Vi_id='" + oldvi_id + "' ");
                    Database.CommandExecutor("Update Voucharges set Vi_id='" + newvi_id + "' where  Vi_id='" + oldvi_id + "' ");
                    Database.CommandExecutor("Update Stock set Vid='" + newvi_id + "' where  Vid='" + oldvi_id + "' ");
                }
            }

            MessageBox.Show("Done");
        }

        private void branch2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataTable dtvou = new DataTable("Voucherinfo");
            Database.GetSqlData("SELECT * FROM  VOUCHERINFO WHERE     (Nid > 26506) AND (Conn_id <> '0') AND (Conn_id <> '') ORDER BY Nid", dtvou);
            for (int k = 0; k < dtvou.Rows.Count; k++)
            {

                string conid = dtvou.Rows[k]["Conn_id"].ToString();
                int con_id = int.Parse(conid.Replace("MAN", ""));
                if (con_id > 150)
                {
                    dtvou.Rows[k]["Conn_id"] = "INM" + con_id;
                }


            }
            Database.SaveData(dtvou);
        }

        private void dailyB2CReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report gg = new Report();
            gg.B2CDaily(Database.cmonthFst, Database.ldate);
            gg.MdiParent = this;
            gg.Show();
        }

        private void customersContactNoSearchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmMobileno frm = new frmMobileno();
            frm.ShowDialog();
        }

        private void summarizedReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report gg = new Report();
            gg.Productiondet(Database.cmonthFst, Database.ldate);
            gg.MdiParent = this;
            gg.Show();
        }

        private void detailedReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report gg = new Report();
            string strCombo = "SELECT Distinct DESCRIPTION.Description   FROM DESCRIPTION order by Description";
            char cg = 'a';
            string selected = SelectCombo.ComboKeypress(this, cg, strCombo, "", 1);
            gg.DetailedProduction(Database.cmonthFst, Database.ldate, selected);
            gg.MdiParent = this;
            gg.Show();
        }


        public void backup()
        {
            DataSet ds = new DataSet();
            DataTable dtalltable = new DataTable();
            Database.GetSqlData("SELECT Table_Name as tablename FROM  " + Database.databaseName + ".INFORMATION_SCHEMA.TABLES WHERE  TABLE_TYPE = 'BASE TABLE' order by Table_Name", dtalltable);
            for (int i = 0; i < dtalltable.Rows.Count; i++)
            {
                DataTable dtsingletable = new DataTable(dtalltable.Rows[i][0].ToString());
                Database.GetSqlData("select * from " + dtalltable.Rows[i][0].ToString(), dtsingletable);
                ds.Tables.AddRange(new DataTable[] { dtsingletable });
            }

            // 
            saveFileDialog1.Filter = "Text files (*.bak)|*.bak|All files (*.*)|*.*";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    //   DateTime dt1 = DateTime.Now;
                    string filePath1 = saveFileDialog1.FileName;
                    ds.WriteXml(filePath1);


                    //MemoryStream obj = new MemoryStream();
                    //ds.WriteXml(obj);

                    // string abc=ds.GetXml();

                    //System.IO.Compression.GZipStream sw = new GZipStream(obj, CompressionMode.Compress);


                    //string res =   Compress(result);


                    //File.Create(Application.StartupPath + "\\bak.txt").Dispose();
                    //TextWriter tw = new StreamWriter(Application.StartupPath + "\\bak.txt");
                    //tw.WriteLine(res);
                    //tw.Close();

                    //DateTime dt2 = DateTime.Now;
                    //System.Windows.Forms.MessageBox.Show((dt2 - dt1).TotalSeconds.ToString());
                    MessageBox.Show("Database BackUp has been created successful.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }





            //ds = new DataSet();

            //ds.ReadXml(filePath1);






        }

        private string Compress(string text)
        {
            string inputStr = text;
            byte[] inputBytes = Encoding.UTF8.GetBytes(inputStr);

            using (var outputStream = new MemoryStream())
            {
                using (var gZipStream = new GZipStream(outputStream, CompressionMode.Compress))
                    gZipStream.Write(inputBytes, 0, inputBytes.Length);

                var outputBytes = outputStream.ToArray();

                var outputStr = Encoding.UTF8.GetString(outputBytes);

                return outputStr;
            }
        }

        private void newBackupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DateTime dt1 = DateTime.Now;
            backup();
            DateTime dt2 = DateTime.Now;
            System.Windows.Forms.MessageBox.Show((dt2 - dt1).TotalSeconds.ToString());
        }

        private void ofd_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void sanitaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataTable dtAccount = new DataTable("Account");
            Database.GetSqlData("SELECT    * FROM         dbo.ACCOUNT WHERE     (oldid IN ('MAN1', 'MAN2', 'MAN3', 'MAN4', 'MAN5', 'MAN6'))", dtAccount);
            for (int j = 0; j < dtAccount.Rows.Count; j++)
            {

                // string oldac_id = dtAccount.Rows[j]["Ac_id"].ToString();
                string oldac_id = dtAccount.Rows[j]["oldid"].ToString();
                //string oldlocid = dtbranch.Rows[i]["oldid"].ToString();
                //string newlocid = dtbranch.Rows[i]["id"].ToString();
                string newac_id = dtAccount.Rows[j]["ac_id"].ToString();
                // Database.CommandExecutor("Update account set where Branch_id='" + oldlocid + "' and Ac_id='" + oldac_id + "' ");
                Database.CommandExecutor("Update BILLBYBILL set Ac_id='" + newac_id + "' where Ac_id='" + oldac_id + "'");
                Database.CommandExecutor("Update CHARGES set Ac_id='" + newac_id + "' where Ac_id='" + oldac_id + "'");
                Database.CommandExecutor("Update DisAfterTax set Ac_id='" + newac_id + "' where Ac_id='" + oldac_id + "'");
                Database.CommandExecutor("Update importantdate set Ac_id='" + newac_id + "' where Ac_id='" + oldac_id + "'");
                Database.CommandExecutor("Update ITEMCHARGES set Accid='" + newac_id + "' where Accid='" + oldac_id + "'");
                Database.CommandExecutor("Update Journal set Ac_id='" + newac_id + "' where Ac_id='" + oldac_id + "'");


                Database.CommandExecutor("Update PARTYRATE set Ac_id='" + newac_id + "' where Ac_id='" + oldac_id + "'");
                Database.CommandExecutor("Update Stock set godown_id='" + newac_id + "' where godown_id='" + oldac_id + "'");
                Database.CommandExecutor("Update TAXCATEGORY set PA='" + newac_id + "' where PA='" + oldac_id + "'");
                Database.CommandExecutor("Update TAXCATEGORY set SA='" + newac_id + "' where SA='" + oldac_id + "'");
                Database.CommandExecutor("Update TAXCATEGORY set PAEX='" + newac_id + "' where PAEX='" + oldac_id + "'");
                Database.CommandExecutor("Update TAXCATEGORY set SAEX='" + newac_id + "' where SAEX='" + oldac_id + "'");
                Database.CommandExecutor("Update TAXCATEGORY set PCA='" + newac_id + "' where PCA='" + oldac_id + "'");
                Database.CommandExecutor("Update TAXCATEGORY set SCA='" + newac_id + "' where SCA='" + oldac_id + "'");
                Database.CommandExecutor("Update TAXCATEGORY set PCAEX='" + newac_id + "' where PCAEX='" + oldac_id + "'");
                Database.CommandExecutor("Update TAXCATEGORY set SCAEX='" + newac_id + "' where SCAEX='" + oldac_id + "'");
                Database.CommandExecutor("Update TAXCATEGORY set PTA1='" + newac_id + "' where PTA1='" + oldac_id + "'");
                Database.CommandExecutor("Update TAXCATEGORY set PTA2='" + newac_id + "' where PTA2='" + oldac_id + "'");
                Database.CommandExecutor("Update TAXCATEGORY set PTA3='" + newac_id + "' where PTA3='" + oldac_id + "'");
                Database.CommandExecutor("Update TAXCATEGORY set STA1='" + newac_id + "' where STA1='" + oldac_id + "'");
                Database.CommandExecutor("Update TAXCATEGORY set STA2='" + newac_id + "' where STA2='" + oldac_id + "'");
                Database.CommandExecutor("Update TAXCATEGORY set STA3='" + newac_id + "' where STA3='" + oldac_id + "'");
                Database.CommandExecutor("Update TAXCATEGORY set RCMPay=" + newac_id + " where RCMPay='" + oldac_id + "'");
                Database.CommandExecutor("Update TAXCATEGORY set RCMITC='" + newac_id + "' where RCMITC='" + oldac_id + "'");
                Database.CommandExecutor("Update TAXCATEGORY set RCMEli='" + newac_id + "' where RCMEli='" + oldac_id + "'");
                Database.CommandExecutor("Update VOUCHERDET set godown_id='" + newac_id + "' where godown_id='" + oldac_id + "'");
                Database.CommandExecutor("Update VOUCHERDET set pur_sale_acc='" + newac_id + "' where pur_sale_acc='" + oldac_id + "'");
                Database.CommandExecutor("Update VOUCHERDET set tax1='" + newac_id + "' where tax1='" + oldac_id + "'");
                Database.CommandExecutor("Update VOUCHERDET set tax2='" + newac_id + "' where tax2='" + oldac_id + "'");
                Database.CommandExecutor("Update VOUCHERDET set tax3='" + newac_id + "' where tax3='" + oldac_id + "'");
                Database.CommandExecutor("Update VOUCHERDET set tax4='" + newac_id + "' where tax4='" + oldac_id + "'");
                Database.CommandExecutor("Update VOUCHERDET set DATac_id='" + newac_id + "' where DATac_id='" + oldac_id + "'");
                Database.CommandExecutor("Update VOUCHERDET set RCMac_id='" + newac_id + "' where RCMac_id='" + oldac_id + "'");
                Database.CommandExecutor("Update VOUCHARGES set Accid='" + newac_id + "' where Accid='" + oldac_id + "'");
                Database.CommandExecutor("Update VOUCHERACTOTAL set Accid='" + newac_id + "' where Accid='" + oldac_id + "'");
                Database.CommandExecutor("Update Voucherinfo set ac_id='" + newac_id + "' where ac_id='" + oldac_id + "'");
                Database.CommandExecutor("Update Voucherpaydet set acc_id='" + newac_id + "' where acc_id='" + oldac_id + "'");
                Database.CommandExecutor("Update Voucherinfo set ac_id2='" + newac_id + "' where ac_id2='" + oldac_id + "'");

                Database.CommandExecutor("Update Journal set opp_Acid='" + newac_id + "' where opp_Acid='" + oldac_id + "'");
                Database.CommandExecutor("Update Voucherinfo set Dr_Ac_id='" + newac_id + "' where Dr_Ac_id='" + oldac_id + "'");
                Database.CommandExecutor("Update Voucherinfo set Cr_Ac_id='" + newac_id + "' where Cr_Ac_id='" + oldac_id + "'");
                Database.CommandExecutor("Update Description set godown_id='" + newac_id + "' where godown_id='" + oldac_id + "'");
                Database.CommandExecutor("Update Contractor set Reff_id='" + newac_id + "' where Reff_id='" + oldac_id + "'");



            }
            MessageBox.Show("");

        }

        private void sanitary2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataTable dtvoucherinfo = new DataTable();
            Database.GetSqlData("Select * from Voucherinfo where nid>6000", dtvoucherinfo);
            for (int j = 0; j < dtvoucherinfo.Rows.Count; j++)
            {
                string oldvi_id = dtvoucherinfo.Rows[j]["oldid"].ToString();

                string newvi_id = dtvoucherinfo.Rows[j]["vi_id"].ToString();
                // Database.CommandExecutor("Update Voucherinfo set Branch_id='" + Vi_id='" + newvi_id + "' where Branch_id='" + oldlocid + "' and Vi_id='" + oldvi_id + "' ");
                Database.CommandExecutor("Update Voucherdet set Vi_id='" + newvi_id + "' where  Vi_id='" + oldvi_id + "' ");
                Database.CommandExecutor("Update Journal set Vi_id='" + newvi_id + "' where  Vi_id='" + oldvi_id + "' ");
                Database.CommandExecutor("Update Voucherpaydet set Vi_id='" + newvi_id + "' where  Vi_id='" + oldvi_id + "' ");
                Database.CommandExecutor("Update Voucheractotal set Vi_id='" + newvi_id + "' where  Vi_id='" + oldvi_id + "' ");
                Database.CommandExecutor("Update Voucharges set Vi_id='" + newvi_id + "' where  Vi_id='" + oldvi_id + "' ");
                Database.CommandExecutor("Update Stock set Vid='" + newvi_id + "' where  Vid='" + oldvi_id + "' ");
            }
            MessageBox.Show("done");
        }

        private void bulkUpdateRebateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frm_bulkupdreb frm = new frm_bulkupdreb();
            frm.MdiParent = this;
            frm.WindowState = FormWindowState.Maximized;
            frm.Show();

        }

        private void brokerReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frm_rpt frm = new frm_rpt();
            frm.MdiParent = this;
            frm.frmtext = "Agent";
            frm.Show();
        }

        private void salesManReportsToolStripMenuItem_Click(object sender, EventArgs e)
        {

            frm_rpt frm = new frm_rpt();
            frm.MdiParent = this;
            frm.frmtext = "Salesman";
            frm.Show();
        }



        private void timer1_Tick_1(object sender, EventArgs e)
        {
            if (!backgroundWorker1.IsBusy)
            {
                backgroundWorker1.RunWorkerAsync();
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                Master.UpdateAll();
                funs.ShowBalloonTip("Updated", "Updated Successfully");
            }
            catch (Exception ex)
            {

                MessageBox.Show("Timer " + ex.ToString());
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        private void accountGroupLedgerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report gg = new Report();
            string strCombo = "SELECT  Name FROM ACCOUNTYPE WHERE Type = 'Account' ORDER BY Name";
            char cg = 'a';
            string selected = SelectCombo.ComboKeypress(this, cg, strCombo, "", 1);


            gg.AccGroupLedger(Database.stDate, Database.ldate, selected);


            gg.MdiParent = this;
            gg.Show();
        }

        private void mailTestingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frm_treemode frm = new frm_treemode();
            frm.MdiParent = this;
            frm.Show();
          
        }

        private void pageFillToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //DataTable dtwinpag = new DataTable();
            //Database.GetSqlData("Select PageId from WinPage", dtwinpag);
            //DataTable dtwinpagerole = new DataTable("WinPageRole");
            //Database.GetSqlData("Select * from WinPageRole", dtwinpagerole);
            //for (int i = 0; i < dtwinpag.Rows.Count; i++)
            //{
            //    dtwinpagerole.Rows.Add();
            //    dtwinpagerole.Rows[dtwinpagerole.Rows.Count - 1]["Role_Id"] = 1;
            //    dtwinpagerole.Rows[dtwinpagerole.Rows.Count - 1]["Page_Id"] = dtwinpag.Rows[i]["PageId"];
            //    dtwinpagerole.Rows[dtwinpagerole.Rows.Count - 1]["Visible"] = true;


            //    dtwinpagerole.Rows.Add();
            //    dtwinpagerole.Rows[dtwinpagerole.Rows.Count - 1]["Role_Id"] = 2;
            //    dtwinpagerole.Rows[dtwinpagerole.Rows.Count - 1]["Page_Id"] = dtwinpag.Rows[i]["PageId"];
            //    dtwinpagerole.Rows[dtwinpagerole.Rows.Count - 1]["Visible"] = true;
            //}
            //Database.SaveData(dtwinpagerole);
            //MessageBox.Show("Done");



            List<UsersFeature>  obj= new List<UsersFeature>();
            List<string>  objpv= new List<string>();
            objpv.Add("Allowed");
             objpv.Add("Not Allowed");

             List<string> objpv2 = new List<string>();
             objpv2.Add("Allowed");
             objpv2.Add("Not Allowed");
             objpv2.Add("Count Restricted");
             objpv2.Add("Days Restricted");

            obj.Add(new UsersFeature(){ FeatureName="Create",FeatureType="MultiValue",PossibleValues=objpv,SelectedValue="Allowed"});
            obj.Add(new UsersFeature() { FeatureName = "Alter", FeatureType = "MultiValue", PossibleValues = objpv2, SelectedValue = "Allowed" });

            obj.Add(new UsersFeature() { FeatureName = "Alter Restrictions", FeatureType = "SingleValue", SelectedValue = "0" });
            obj.Add(new UsersFeature() { FeatureName = "Delete", FeatureType = "MultiValue", PossibleValues = objpv2, SelectedValue = "Allowed" });

            obj.Add(new UsersFeature() { FeatureName = "Delete  Restrictions", FeatureType = "SingleValue", SelectedValue = "0" });



            JavaScriptSerializer obj2 = new JavaScriptSerializer();
            string str=obj2.Serialize(obj);

        }

        private void usersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmMaster frm = new frmMaster();
            frm.MdiParent = this;
            frm.LoadData("User", "User");
            frm.Show();
        }

        private void roleManagementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmMaster frm = new frmMaster();
            frm.MdiParent = this;
            frm.LoadData("Role", "Role");
            frm.Show();
        }

        private void newCodeSynToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var SqlCnnClient = @"Data Source=" + Database.inipath + ";Initial Catalog=" + Database.databaseName + ";Persist Security Info=True;User ID=sa;password=" + Database.sqlseverpwd + "";
            var SqlCnnSer = @"Data Source=" + Database.SHostname + ";Initial Catalog=" + Database.SDbname + ";Persist Security Info=True;User ID=" + Database.SUsername + ";password=" + Database.SPwd;
            DataTable dt = new DataTable();
            //using (SqlConnection connectionClient = new SqlConnection(SqlCnnClient))
            //{
            //    using (SqlDataAdapter da = new SqlDataAdapter("select * from Syncronizer order by id", connectionClient))
            //    {
            //        da.Fill(dt);
            //    }
            //}

            string scriptDirectory = Application.StartupPath;
           
            DirectoryInfo di = new DirectoryInfo(scriptDirectory);
            FileInfo[] rgFiles = di.GetFiles("*.sql");
            foreach (FileInfo fi in rgFiles)
            {
                FileInfo fileInfo = new FileInfo(fi.FullName);
                string script = fileInfo.OpenText().ReadToEnd();
                //using (SqlConnection connection = new SqlConnection(SqlCnnClient))
                //{
                    Server server = new Server(new ServerConnection(SqlCnnSer));
                    server.ConnectionContext.ExecuteNonQuery(script);
               // }
            }


            //Server server = new Server(new ServerConnection(SqlCnnSer));

            //using (SqlConnection connection = new SqlConnection(SqlCnnSer))
            //{
            //    connection.Open();
            //    SqlTransaction sqlTran = connection.BeginTransaction();
            //    SqlCommand command = connection.CreateCommand();
            //    command.Transaction = sqlTran;


            //    try
            //    {
            //        for (int i = 0; i < dt.Rows.Count; i++)
            //        {
            //            command.CommandText = dt.Rows[i]["query"].ToString();
            //            int rowsAffected = command.ExecuteNonQuery();
            //        }
            //        sqlTran.Commit();
            //    }
            //    catch (Exception exp)
            //    {
            //        MessageBox.Show(exp.Message + Environment.NewLine + " Not Done... Try Again Line No");
            //        sqlTran.Rollback();
            //        return;
            //    }

            //}


        }

        private void rebateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frm_rebate frm = new frm_rebate();
            frm.LoadData();
            frm.MdiParent = this;
            frm.Show();
        }

        private void otherReportToolStripMenuItem_Click(object sender, EventArgs e)
        {


        }

        private void grToolStripMenuItem_Click(object sender, EventArgs e)
        {

            string strcombo = "SELECT Name FROM  ACCOUNTYPE WHERE  (Type = 'Account') ORDER BY Name";
            char cg = 'a';
            string selected = SelectCombo.ComboKeypress(this, cg, strcombo, "", 1);

            frm_gradeoption frm = new frm_gradeoption();
            frm.acctype = selected;
            frm.Show();

        }

        private void outstandingReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report gg = new Report();
            string strCombo = funs.GetStrCombonew(" (Path LIKE '1;39;%') or (Path LIKE '1;38;%')   or   (Path LIKE '8;40;%')  or   (Path LIKE '8;39;%' ) ", " Branch_id='" + Database.BranchId + "' ");
            char cg = ' ';
            string selected = SelectCombo.ComboKeypress(this, cg, strCombo, "", 2);

            gg.adjtest(Database.stDate, Database.ldate, selected);
            gg.MdiParent = this;
            gg.Show();
        }

        private void gSTRegisterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frm_filtergst frm = new frm_filtergst();
            frm.MdiParent = this;
            frm.Show();
        }

        private void accountantSaleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report gg = new Report();

            gg.AccSale(Database.cmonthFst, Database.ldate);
            gg.MdiParent = this;
            gg.Show();
        }

        private void customerReportToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void cityToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmMaster frm = new frmMaster();
            frm.MdiParent = this;
            frm.LoadData("City", "City");
            frm.WindowState = FormWindowState.Maximized;
            frm.Show();
        }

        private void contactNoWiseListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report gg = new Report();
            gg.ContactList(Database.ldate, Database.ldate);
            gg.MdiParent = this;
            gg.Show();
        }
    }
}

