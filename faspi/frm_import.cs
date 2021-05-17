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

namespace faspi
{
    public partial class frm_import : Form
    {
        public frm_import()
        {
            InitializeComponent();
        }

        private void SideFill()
        {
            flowLayoutPanel1.Controls.Clear();
            DataTable dtsidefill = new DataTable();

            dtsidefill.Columns.Add("Name", typeof(string));
            dtsidefill.Columns.Add("DisplayName", typeof(string));
            dtsidefill.Columns.Add("ShortcutKey", typeof(string));
            dtsidefill.Columns.Add("Visible", typeof(bool));

            //save
            dtsidefill.Rows.Add();
            dtsidefill.Rows[0]["Name"] = "import";
            dtsidefill.Rows[0]["DisplayName"] = "Import";
            dtsidefill.Rows[0]["ShortcutKey"] = "^S";
            dtsidefill.Rows[0]["Visible"] = true;

            //close
            dtsidefill.Rows.Add();
            dtsidefill.Rows[1]["Name"] = "quit";
            dtsidefill.Rows[1]["DisplayName"] = "Quit";
            dtsidefill.Rows[1]["ShortcutKey"] = "Esc";
            dtsidefill.Rows[1]["Visible"] = true;

            for (int i = 0; i < dtsidefill.Rows.Count; i++)
            {
                if (bool.Parse(dtsidefill.Rows[i]["Visible"].ToString()) == true)
                {
                    Button btn = new Button();
                    btn.Size = new Size(150, 30);
                    btn.Name = dtsidefill.Rows[i]["Name"].ToString();
                    btn.Text = "";
                    Bitmap bmp = new Bitmap(btn.ClientRectangle.Width, btn.ClientRectangle.Height);
                    Graphics G = Graphics.FromImage(bmp);
                    G.Clear(btn.BackColor);
                    string line1 = dtsidefill.Rows[i]["ShortcutKey"].ToString();
                    string line2 = dtsidefill.Rows[i]["DisplayName"].ToString();
                    StringFormat SF = new StringFormat();
                    SF.Alignment = StringAlignment.Near;
                    SF.LineAlignment = StringAlignment.Center;
                    Rectangle RC = btn.ClientRectangle;
                    Font font = new Font("Arial", 12);
                    G.DrawString(line1, font, Brushes.Red, RC, SF);
                    G.DrawString("".PadLeft(line1.Length * 2 + 1) + line2, font, Brushes.Black, RC, SF);
                    btn.Image = bmp;
                    btn.Click += new EventHandler(btn_Click);
                    flowLayoutPanel1.Controls.Add(btn);
                }
            }
        }

        void btn_Click(object sender, EventArgs e)
        {
            Button tbtn = (Button)sender;
            string name = tbtn.Name.ToString();

            if (name == "import")
            {
                import();
            }
            else if (name == "quit")
            {
                this.Close();
                this.Dispose();
            }
        }

        public void LoadData()
        {
            dataGridView1.Rows.Add();
            dataGridView1.Rows[0].Cells["master"].Value = "Basic Accounts";
            dataGridView1.Rows[0].Cells["check"].Value = true;
            dataGridView1.Rows[0].Cells["change"].Value = false;

            dataGridView1.Rows.Add();
            dataGridView1.Rows[1].Cells["master"].Value = "TaxCategory";
            dataGridView1.Rows[1].Cells["check"].Value = true;
            dataGridView1.Rows[1].Cells["change"].Value = false;

            dataGridView1.Rows.Add();
            dataGridView1.Rows[2].Cells["master"].Value = "Charges/Discount";
            dataGridView1.Rows[2].Cells["check"].Value = true;
            dataGridView1.Rows[2].Cells["change"].Value = false;

            String address = "http://www.faspi.in/faspidata/companylist.php";
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

            str = str.Replace("\n", "");
            str = str.Replace("\t", "");
            if (str.Trim() == "")
            {
                MessageBox.Show("Data Not Found on Server.");
            }
            else
            {
                string[] ar = str.Split('|');
                for (int i = 3; i < ar.Length +3; i++)
                {
                    dataGridView1.Rows.Add();
                    dataGridView1.Rows[i].Cells["master"].Value = ar[i-3];
                    dataGridView1.Rows[i].Cells["check"].Value = false;
                    dataGridView1.Rows[i].Cells["change"].Value = true;
                }
            }
            this.Size = this.MdiParent.Size;
            SideFill();                
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show(DateTime.Now.TimeOfDay.ToString());

            AccImport();
            CommCodeImport();
            CommCodeDetailsImport();
            ChargesImport();

            for (int i = 4; i < dataGridView1.Rows.Count; i++)
            {
                if (bool.Parse(dataGridView1.Rows[i].Cells["check"].Value.ToString()) == true)
                {
                    DescImport(dataGridView1.Rows[i].Cells["master"].Value.ToString());                    
                }
            }

            MessageBox.Show(DateTime.Now.TimeOfDay.ToString());
            MessageBox.Show("Done");
            this.Close();
            this.Dispose();
        }

        private void DescImport(string strCom)
        {
             String address = "http://www.faspi.in/faspidata/items.php?list=" + strCom;
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
            str = str.Replace("\n", "");
            str = str.Replace("\t", "");
            if (str.Trim() == "")
            {
                MessageBox.Show("Charges Item Not Found on Server.");
            }
            else
            {
                DataTable dtServer = new DataTable();
                dtServer.Columns.Add("desc", typeof(string));
                dtServer.Columns.Add("packing", typeof(string));
                dtServer.Columns.Add("company", typeof(string));
                dtServer.Columns.Add("item", typeof(string));
                dtServer.Columns.Add("pricegrp", typeof(string));
                dtServer.Columns.Add("color", typeof(string));
                dtServer.Columns.Add("taxcat", typeof(string));
                dtServer.Columns.Add("sku", typeof(string));
                dtServer.Columns.Add("pvalue", typeof(string));
                dtServer.Columns.Add("rate_unit", typeof(string));
                dtServer.Columns.Add("State", typeof(string));
                string[] ar = str.Split('~');
                for (int i = 0; i < ar.Length - 1; i++)
                {
                    string[] Dcell = ar[i].Split('|');
                    dtServer.Rows.Add();
                    dtServer.Rows[i][0] = Dcell[1];
                    dtServer.Rows[i][1] = Dcell[2];
                    dtServer.Rows[i][2] = Dcell[3];
                    dtServer.Rows[i][3] = Dcell[4];
                    dtServer.Rows[i][4] = Dcell[5];
                    dtServer.Rows[i][5] = Dcell[6];
                    dtServer.Rows[i][6] = Dcell[7];
                    dtServer.Rows[i][7] = Dcell[8];
                    dtServer.Rows[i][8] = Dcell[9];
                    dtServer.Rows[i][9] = Dcell[10];
                    dtServer.Rows[i][10] = "Added";                  
                }

                DataTable LOther = new DataTable("Other");
                Database.GetSqlData("select * from Other", LOther);

                DataTable SCompany =  dtServer.DefaultView.ToTable(true, "company");
                for (int y = 0; y < SCompany.Rows.Count;y++)
                {
                    if (LOther.Select("Name='" + SCompany.Rows[y][0] + "' and Type='SER14'").Length == 0)
                    {
                        LOther.Rows.Add();
                        LOther.Rows[LOther.Rows.Count-1]["Name"] = SCompany.Rows[y][0];
                        LOther.Rows[LOther.Rows.Count-1]["Type"] = "SER14";
                    }
                }

                DataTable SItem = dtServer.DefaultView.ToTable(true, "item");
                for (int y = 0; y < SItem.Rows.Count; y++)
                {
                    if (LOther.Select("Name='" + SItem.Rows[y][0] + "' and Type='SER15'").Length == 0)
                    {
                        LOther.Rows.Add();
                        LOther.Rows[LOther.Rows.Count-1]["Name"] = SItem.Rows[y][0];
                        LOther.Rows[LOther.Rows.Count-1]["Type"] = "SER15";
                    }
                }

                DataTable SPriceGrp = dtServer.DefaultView.ToTable(true, "pricegrp");
                for (int y = 0; y < SPriceGrp.Rows.Count; y++)
                {
                    if (LOther.Select("Name='" + SPriceGrp.Rows[y][0] + "' and Type='SER16'").Length == 0)
                    {
                        LOther.Rows.Add();
                        LOther.Rows[LOther.Rows.Count-1]["Name"] = SPriceGrp.Rows[y][0];
                        LOther.Rows[LOther.Rows.Count-1]["Type"] = "SER16";
                    }
                }

                DataTable SColor = dtServer.DefaultView.ToTable(true, "color");
                for (int y = 0; y < SColor.Rows.Count; y++)
                {
                    if (LOther.Select("Name='" + SColor.Rows[y][0] + "' and Type='SER18'").Length == 0)
                    {
                        LOther.Rows.Add();
                        LOther.Rows[LOther.Rows.Count-1]["Name"] = SColor.Rows[y][0];
                        LOther.Rows[LOther.Rows.Count-1]["Type"] = "SER18";
                    }
                }

                Database.SaveData(LOther);
                Database.GetSqlData("select * from Other", LOther);

                DataTable dtDesc = new DataTable("Description");
                Database.GetSqlData("Select * from Description where Des_id='SER0'",dtDesc);

                DataTable dtTaxCat = new DataTable("TAXCATEGORY");
                Database.GetSqlData("Select * from TAXCATEGORY",dtTaxCat);

                for (int s = 0; s < dtServer.Rows.Count;s++ )
                {
                    dtDesc.Rows.Add();
                    dtDesc.Rows[s]["Description"] = dtServer.Rows[s]["desc"];
                    dtDesc.Rows[s]["Pack"] = dtServer.Rows[s]["packing"];
                    dtDesc.Rows[s]["rate_unit"] = dtServer.Rows[s]["rate_unit"];
                    dtDesc.Rows[s]["pvalue"] = dtServer.Rows[s]["pvalue"];
                    dtDesc.Rows[s]["Retail"] = 0;
                    dtDesc.Rows[s]["Wholesale"] = 0;
                    dtDesc.Rows[s]["Company_id"] = LOther.Select("Name='" + dtServer.Rows[s]["company"] + "' and Type='SER14'")[0][0];
                    dtDesc.Rows[s]["Item_id"] = LOther.Select("Name='" + dtServer.Rows[s]["item"] + "' and Type='SER15'")[0][0];
                    dtDesc.Rows[s]["Col_id"] = LOther.Select("Name='" + dtServer.Rows[s]["color"] + "' and Type='SER18'")[0][0];
                    dtDesc.Rows[s]["Group_id"] = LOther.Select("Name='" + dtServer.Rows[s]["pricegrp"] + "' and Type='SER16'")[0][0];
                    dtDesc.Rows[s]["Tax_Cat_id"] = dtTaxCat.Select("Category_Name='" + dtServer.Rows[s]["taxcat"] + "'")[0][0];
                    dtDesc.Rows[s]["Purchase_rate"] = 0;
                    dtDesc.Rows[s]["Open_stock"] = 0;
                    dtDesc.Rows[s]["Open_stock2"] = 0;
                    dtDesc.Rows[s]["Mark"] = "No";
                    dtDesc.Rows[s]["Wlavel"] = 0;
                    dtDesc.Rows[s]["Commission%"] = 0;
                    dtDesc.Rows[s]["Commission@"] = 0;
                    dtDesc.Rows[s]["ShortCode"] = 0;
                    dtDesc.Rows[s]["box_quantity"] = 1;
                    dtDesc.Rows[s]["weight"] = 1;
                    dtDesc.Rows[s]["discount_qty"] = 1;
                    dtDesc.Rows[s]["Rate_X"] = 0;
                    dtDesc.Rows[s]["Rate_Y"] = 0;
                    dtDesc.Rows[s]["Rate_Z"] = 0;
                    dtDesc.Rows[s]["MRP"] = 0;
                    dtDesc.Rows[s]["Skucode"] = dtServer.Rows[s]["sku"];
                    dtDesc.Rows[s]["State"] = dtServer.Rows[s]["state"];
                    dtDesc.Rows[s]["remarkreq"] = false;
                    dtDesc.Rows[s]["StkMaintain"] = true; ;
                }
                Database.SaveData(dtDesc);
            }
           Master.UpdateOther();
           Master.UpdateDecription();
           Master.UpdateDecriptionInfo();
        }
        
        private void CommCodeDetailsImport()
        {
            String address = "http://localhost1/faspidata/commcode.php";
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

            str = str.Replace("\n", "");
            str = str.Replace("\t", "");
            if (str.Trim() == "")
            {
                MessageBox.Show("Tax Catagory Details Master Not Found on Server.");
            }
            else
            {
                DataTable DtSCat = new DataTable("TAXCATEGORY");
                DtSCat.Columns.Add("Category_Id", typeof(int));
                DtSCat.Columns.Add("SubCategory_Name", typeof(string));
                DtSCat.Columns.Add("Sireal", typeof(int));
                DtSCat.Columns.Add("Sale_Pur_Acc_id", typeof(int));
                DtSCat.Columns.Add("Tax_Acc_id", typeof(int));
                DtSCat.Columns.Add("Tax_Name", typeof(string));
                DtSCat.Columns.Add("Tax_Rate", typeof(double));
                string[] ar = str.Split('~');
                DataTable DtAcc = new DataTable();
                Database.GetSqlData("select Ac_id,Name from account", DtAcc);

                DataTable DtCat = new DataTable();
                Database.GetSqlData("select Category_Id,Category_Name from TAXCATEGORY", DtCat);

                for (int i = 0; i < ar.Length - 1; i++)
                {

                    string[] Dcell = ar[i].Split('|');
                    DtSCat.Rows.Add();
                    DtSCat.Rows[DtSCat.Rows.Count - 1][0] = DtCat.Select("Category_Name='" + Dcell[0] + "'")[0]["Category_Id"].ToString();
                    DtSCat.Rows[DtSCat.Rows.Count - 1][1] = "Local Purchase";
                    DtSCat.Rows[DtSCat.Rows.Count - 1][2] = "1";
                    DtSCat.Rows[DtSCat.Rows.Count - 1][3] = DtAcc.Select("Name='" + Dcell[4] + "'")[0]["Ac_id"].ToString();
                    DtSCat.Rows[DtSCat.Rows.Count - 1][4] = DtAcc.Select("Name='" + Dcell[6] + "'")[0]["Ac_id"].ToString();
                    DtSCat.Rows[DtSCat.Rows.Count - 1][5] = "VAT";
                    DtSCat.Rows[DtSCat.Rows.Count - 1][6] = Dcell[2];

                    DtSCat.Rows.Add();
                    DtSCat.Rows[DtSCat.Rows.Count - 1][0] = DtCat.Select("Category_Name='" + Dcell[0] + "'")[0]["Category_Id"].ToString();
                    DtSCat.Rows[DtSCat.Rows.Count - 1][1] = "Local Purchase";
                    DtSCat.Rows[DtSCat.Rows.Count - 1][2] = "2";
                    DtSCat.Rows[DtSCat.Rows.Count - 1][3] = DtAcc.Select("Name='" + Dcell[4] + "'")[0]["Ac_id"].ToString();
                    DtSCat.Rows[DtSCat.Rows.Count - 1][4] = DtAcc.Select("Name='" + Dcell[6] + "'")[0]["Ac_id"].ToString();
                    DtSCat.Rows[DtSCat.Rows.Count - 1][5] = "SAT";
                    DtSCat.Rows[DtSCat.Rows.Count - 1][6] = Dcell[3];

                    DtSCat.Rows.Add();
                    DtSCat.Rows[DtSCat.Rows.Count - 1][0] = DtCat.Select("Category_Name='" + Dcell[0] + "'")[0]["Category_Id"].ToString();
                    DtSCat.Rows[DtSCat.Rows.Count - 1][1] = "Local Sale";
                    DtSCat.Rows[DtSCat.Rows.Count - 1][2] = "1";
                    DtSCat.Rows[DtSCat.Rows.Count - 1][3] = DtAcc.Select("Name='" + Dcell[5] + "'")[0]["Ac_id"].ToString();
                    DtSCat.Rows[DtSCat.Rows.Count - 1][4] = DtAcc.Select("Name='" + Dcell[7] + "'")[0]["Ac_id"].ToString();
                    DtSCat.Rows[DtSCat.Rows.Count - 1][5] = "VAT";
                    DtSCat.Rows[DtSCat.Rows.Count - 1][6] = Dcell[2];

                    DtSCat.Rows.Add();
                    DtSCat.Rows[DtSCat.Rows.Count - 1][0] = DtCat.Select("Category_Name='" + Dcell[0] + "'")[0]["Category_Id"].ToString();
                    DtSCat.Rows[DtSCat.Rows.Count - 1][1] = "Local Sale";
                    DtSCat.Rows[DtSCat.Rows.Count - 1][2] = "2";
                    DtSCat.Rows[DtSCat.Rows.Count - 1][3] = DtAcc.Select("Name='" + Dcell[5] + "'")[0]["Ac_id"].ToString();
                    DtSCat.Rows[DtSCat.Rows.Count - 1][4] = DtAcc.Select("Name='" + Dcell[7] + "'")[0]["Ac_id"].ToString();
                    DtSCat.Rows[DtSCat.Rows.Count - 1][5] = "SAT";
                    DtSCat.Rows[DtSCat.Rows.Count - 1][6] = Dcell[3];
                }

                DataTable DtLPacking = new DataTable();
                string StrSql = "select * from  TAXCATEGORYDETAIL";
                Database.GetSqlData(StrSql, DtLPacking);
                if (DtLPacking.Rows.Count == 0)
                {
                    Database.SaveData(DtSCat, StrSql);
                    Master.UpdateTaxCategory();                  
                    Master.UpdateDecriptionInfo();
                }
                else
                {
                    return;
                }
            }
        }

        private void CommCodeImport()
        {
          String address = "http://www.faspi.in/faspidata/CommCode.php";
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

            str = str.Replace("\n", "");
            str = str.Replace("\t", "");

            if (str.Trim() == "")
            {
                MessageBox.Show("Tax Catagory Master Not Found on Server.");
            }
            else
            {
                DataTable DtSCat = new DataTable("TAXCATEGORY");
                DtSCat.Columns.Add("Category_Name", typeof(string));
                DtSCat.Columns.Add("Commodity_Code", typeof(string));
                DtSCat.Columns.Add("Item_Type", typeof(string));
                DtSCat.Columns.Add("PA", typeof(string));
                DtSCat.Columns.Add("SA", typeof(string));
                DtSCat.Columns.Add("PAEx", typeof(string));
                DtSCat.Columns.Add("SAEx", typeof(string));
                DtSCat.Columns.Add("PTA1", typeof(string));
                DtSCat.Columns.Add("PTA2", typeof(string));
                DtSCat.Columns.Add("PTA3", typeof(string));
                DtSCat.Columns.Add("STA1", typeof(string));
                DtSCat.Columns.Add("STA2", typeof(string));
                DtSCat.Columns.Add("STA3", typeof(string));
                DtSCat.Columns.Add("PTR1", typeof(double));
                DtSCat.Columns.Add("PTR2", typeof(double));
                DtSCat.Columns.Add("PTR3", typeof(double));
                DtSCat.Columns.Add("STR1", typeof(double));
                DtSCat.Columns.Add("STR2", typeof(double));
                DtSCat.Columns.Add("STR3", typeof(double));
                DtSCat.Columns.Add("PCA", typeof(string));
                DtSCat.Columns.Add("PCAEx", typeof(string));
                DtSCat.Columns.Add("SCA", typeof(string));
                DtSCat.Columns.Add("SCAEx", typeof(string));
                DtSCat.Columns.Add("PCR", typeof(double));
                DtSCat.Columns.Add("PCREx", typeof(double));
                DtSCat.Columns.Add("SCR", typeof(double));
                DtSCat.Columns.Add("SCREx", typeof(double));
                DtSCat.Columns.Add("RCMPay", typeof(string));
                DtSCat.Columns.Add("RCMITC", typeof(string));
                DtSCat.Columns.Add("RCMEli", typeof(string));

                string[] ar = str.Split('~');
           
                DataTable DtAcc = new DataTable();
                Database.GetSqlData("select Ac_id,Name from account", DtAcc);
                for (int i = 0; i < ar.Length - 1; i++)
                {
                    DtSCat.Rows.Add();
                    string[] Dcell = ar[i].Split('|');
                    DtSCat.Rows[i][0] = Dcell[0];
                    DtSCat.Rows[i][1] = Dcell[1];
                    DtSCat.Rows[i][2] = "Goods";

                    DtSCat.Rows[i][3] = DtAcc.Select("Name='" + Dcell[2] + "'")[0]["Ac_id"].ToString();
                    DtSCat.Rows[i][4] = DtAcc.Select("Name='" + Dcell[3] + "'")[0]["Ac_id"].ToString();
                    DtSCat.Rows[i][5] = DtAcc.Select("Name='" + Dcell[2] + "'")[0]["Ac_id"].ToString();
                    DtSCat.Rows[i][6] = DtAcc.Select("Name='" + Dcell[3] + "'")[0]["Ac_id"].ToString();
                    DtSCat.Rows[i][7] = DtAcc.Select("Name='" + Dcell[7] + "'")[0]["Ac_id"].ToString();
                    DtSCat.Rows[i][8] = DtAcc.Select("Name='" + Dcell[8] + "'")[0]["Ac_id"].ToString(); 
                    DtSCat.Rows[i][9] = DtAcc.Select("Name='" + Dcell[9] + "'")[0]["Ac_id"].ToString();
                    DtSCat.Rows[i][10] = DtAcc.Select("Name='" + Dcell[7] + "'")[0]["Ac_id"].ToString();
                    DtSCat.Rows[i][11] = DtAcc.Select("Name='" + Dcell[8] + "'")[0]["Ac_id"].ToString();
                    DtSCat.Rows[i][12] = DtAcc.Select("Name='" + Dcell[9] + "'")[0]["Ac_id"].ToString();

                    DtSCat.Rows[i][13] = Dcell[4];
                    DtSCat.Rows[i][14] = Dcell[5];
                    DtSCat.Rows[i][15] = Dcell[6];
                    DtSCat.Rows[i][16] = Dcell[4];
                    DtSCat.Rows[i][17] = Dcell[5];
                    DtSCat.Rows[i][18] = Dcell[6];
                    DtSCat.Rows[i][19] = 0;
                    DtSCat.Rows[i][20] = 0;
                    DtSCat.Rows[i][21] = 0;
                    DtSCat.Rows[i][22] = 0;
                    DtSCat.Rows[i][23] = 0;
                    DtSCat.Rows[i][24] = 0;
                    DtSCat.Rows[i][25] = 0;
                    DtSCat.Rows[i][26] = 0;
                    DtSCat.Rows[i][27] = 0;
                    DtSCat.Rows[i][28] = 0;
                    DtSCat.Rows[i][29] = 0;
                }

                DataTable DtLPacking = new DataTable();
                string StrSql = "select Category_Name,Commodity_Code,Item_Type,Pa,Paex,Sa,Saex,pta1,pta2,pta3,sta1,sta2,sta3,ptr1,ptr2,ptr3,str1,str2,str3,pca,pcaex,sca,scaex,pcr,scr,pcrex,screx,RCMPay,RCMITC,RCMEli from TAXCATEGORY";
                Database.GetSqlData(StrSql, DtLPacking);
                if (DtLPacking.Rows.Count == 0)
                {
                    Database.SaveData(DtSCat, StrSql);
                    Master.UpdateTaxCategory();                   
                    Master.UpdateDecriptionInfo();
                }
                else
                {
                    return;
                }
            }
        }
        private void ChargesImport()
        {
            String address = "http://www.faspi.in/faspidata/charges.php";
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

            str = str.Replace("\n", "");
            str = str.Replace("\t", "");
            if (str.Trim() == "")
            {
                MessageBox.Show("Charges Master Not Found on Server.");
            }
            else
            {
                DataTable DtSPacking = new DataTable("CHARGES");
                DtSPacking.Columns.Add("Name", typeof(string));
                DtSPacking.Columns.Add("Ac_id", typeof(string));
                DtSPacking.Columns.Add("Charge_type", typeof(int));
                DtSPacking.Columns.Add("Add_sub", typeof(int));

                string[] ar = str.Split('~');
                DataTable DtAcc = new DataTable();
                Database.GetSqlData("select Ac_id,Name from account", DtAcc);
                for (int i = 0; i < ar.Length - 1; i++)
                {
                    DtSPacking.Rows.Add();
                    string[] Dcell = ar[i].Split('|');
                    DtSPacking.Rows[i][0] = Dcell[0];
                    if (Dcell[1] == "0")
                    {
                        DtSPacking.Rows[i][1] = 0;
                    }
                    else
                    {
                        DtSPacking.Rows[i][1] = DtAcc.Select("Name='" + Dcell[1] + "'")[0]["Ac_id"].ToString();
                    }
                    DtSPacking.Rows[i][2] = Dcell[2];
                    DtSPacking.Rows[i][3] = Dcell[3];
                }

                DataTable DtLPacking = new DataTable();
                string StrSql = "select Name,Ac_id,Charge_type,Add_sub from charges";
                Database.GetSqlData(StrSql, DtLPacking);
                if (DtLPacking.Rows.Count == 0)
                {
                    Database.SaveData(DtSPacking, StrSql);
                    Master.UpdateCharge();
                }
                else
                {
                    return;
                }
            }
        }

        private void AccImport()
        {
             String address = "http://www.faspi.in/faspidata/account.php";
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
            str = str.Replace("\n", "");
            str = str.Replace("\t", "");
            if (str.Trim() == "")
            {
                MessageBox.Show("Account Master Not Found on Server.");
            }
            else
            {
                DataTable DtSPacking = new DataTable("ACCOUNT");

                DtSPacking.Columns.Add("Act_id", typeof(string));
                DtSPacking.Columns.Add("Name", typeof(string));
                DtSPacking.Columns.Add("Tin_number", typeof(string));
                DtSPacking.Columns.Add("RegStatus", typeof(string));               
                DtSPacking.Columns.Add("Balance", typeof(double));
                DtSPacking.Columns.Add("Balance2", typeof(double));
                DtSPacking.Columns.Add("Status", typeof(bool));
                DtSPacking.Columns.Add("PrintName", typeof(string));              
                DtSPacking.Columns.Add("State_id", typeof(string));
                string[] ar = str.Split('~');

                for (int i = 0; i < ar.Length - 1; i++)
                {
                    DtSPacking.Rows.Add();
                    string[] Dcell = ar[i].Split('|');
                    Dcell[0] = Dcell[0].Replace("\r", "");
                    DtSPacking.Rows[i][0] = funs.Select_act_id(Dcell[0]);
                    DtSPacking.Rows[i][1] = Dcell[1];
                    DtSPacking.Rows[i][2] = Dcell[2];
                    DtSPacking.Rows[i][3] = Dcell[3];
                    DtSPacking.Rows[i][4] = 0;
                    DtSPacking.Rows[i][5] = 0;
                    DtSPacking.Rows[i][7] = true;
                    DtSPacking.Rows[i][8] = Dcell[1];  
                    DtSPacking.Rows[i][9] = Database.CompanyState_id;
                }

                DataTable DtLPacking = new DataTable();
                string StrSql = "select Act_id,Name,Tin_number,RegStatus,Balance,Balance2,State,Status,PrintName,State_id from account";

                Database.GetSqlData(StrSql, DtLPacking);

                if (DtLPacking.Rows.Count == 4)
                {
                    Database.SaveData(DtSPacking, StrSql);
                    Database.CommandExecutor("Update Account set AllowPS=" + access_sql.Singlequote + "false" + access_sql.Singlequote);
                    Database.CommandExecutor("Update Account set AllowPS=" + access_sql.Singlequote + "true" + access_sql.Singlequote + " where Name='Customer-Supplier'");
                    Master.UpdateAccount();
                    Master.UpdateAccountinfo();
                }
                else
                {
                    return;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        private void frm_import_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
            {
                import();
            }

            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
                this.Dispose();
            }
        }

        private void import()
        {
            MessageBox.Show(DateTime.Now.TimeOfDay.ToString());
            for (int i = 0; i < 3; i++)
            {
                if (bool.Parse(dataGridView1.Rows[i].Cells["check"].Value.ToString()) == true)
                {
                    AccImport();
                    CommCodeImport();                  
                    ChargesImport();                    
                }
            }
            for (int i = 3; i < dataGridView1.Rows.Count; i++)
            {
                if (bool.Parse(dataGridView1.Rows[i].Cells["check"].Value.ToString()) == true)
                {
                    DescImport(dataGridView1.Rows[i].Cells["master"].Value.ToString());
                }
            }
            MessageBox.Show(DateTime.Now.TimeOfDay.ToString());
            this.Close();
            this.Dispose();
        }

        private void frm_import_Load(object sender, EventArgs e)
        {
            SideFill();
        }
    }
}
