using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Office.Core;
using Excel = Microsoft.Office.Interop.Excel;

namespace faspi
{
    public partial class frmImportDesc : Form
    {
        String fName = "";
        static Object misValue = System.Reflection.Missing.Value;
        static Excel.Application apl = new Microsoft.Office.Interop.Excel.Application();
        Excel.Workbook wb;
        Excel.Worksheet ws;


        public frmImportDesc()
        {
            InitializeComponent();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (DialogResult.OK == ofd.ShowDialog())
            {
                fName = ofd.FileName;
                textBox2.Text = fName;
            }
        }

        private void frmImportDesc_Load(object sender, EventArgs e)
        {
            ofd.Filter = "Excel Files(*.xls)|*.xlsx";
            dataGridView1.Rows.Add();
            dataGridView1.Rows[0].Cells["Column1"].Value = "ASIAN PGE DAWN";
            dataGridView1.Rows[0].Cells["Column2"].Value = "1 LT.";
            dataGridView1.Rows[0].Cells["Unit"].Value = "Lt.";
            dataGridView1.Rows[0].Cells["Pvalue"].Value = "1.00";

            dataGridView1.Rows[0].Cells["Column3"].Value = "ASIAN";
            dataGridView1.Rows[0].Cells["Column4"].Value = "PGE";
            dataGridView1.Rows[0].Cells["Column5"].Value = "2";
            dataGridView1.Rows[0].Cells["Column6"].Value = "CASCADE GREEN";
            dataGridView1.Rows[0].Cells["Column7"].Value = "Paints etc.";
            dataGridView1.Rows[0].Cells["Column8"].Value = "002121";
            dataGridView1.Rows[0].Cells["Column9"].Value = "ASIAN123";
           
            dataGridView1.Rows.Add(1);
            dataGridView1.Rows[1].Cells["Column1"].Value = "ASIAN PGE OFF WHITE";
            dataGridView1.Rows[1].Cells["Column2"].Value = "500 ML.";
            dataGridView1.Rows[1].Cells["Unit"].Value = "Lt.";
            dataGridView1.Rows[1].Cells["Pvalue"].Value = "0.50";

            dataGridView1.Rows[1].Cells["Column3"].Value = "ASIAN";
            dataGridView1.Rows[1].Cells["Column4"].Value = "PGE";
            dataGridView1.Rows[1].Cells["Column5"].Value = "2";
            dataGridView1.Rows[1].Cells["Column6"].Value = "OFF WHITE";
            dataGridView1.Rows[1].Cells["Column7"].Value = "Paints etc.";
            dataGridView1.Rows[1].Cells["Column8"].Value = "002124";
            dataGridView1.Rows[1].Cells["Column9"].Value = "ASIAN456";
         

            this.Size = this.MdiParent.Size;
            SideFill();

        }
        private void SideFill()
        {
            flowLayoutPanel1.Controls.Clear();
            DataTable dtsidefill = new DataTable();

            dtsidefill.Columns.Add("Name", typeof(string));
            dtsidefill.Columns.Add("DisplayName", typeof(string));
            dtsidefill.Columns.Add("ShortcutKey", typeof(string));
            dtsidefill.Columns.Add("Visible", typeof(bool));


            if (tabControl1.SelectedIndex == 0)
            {
                //save
                dtsidefill.Rows.Add();
                dtsidefill.Rows[0]["Name"] = "next";
                dtsidefill.Rows[0]["DisplayName"] = "Next";
                dtsidefill.Rows[0]["ShortcutKey"] = "";
                dtsidefill.Rows[0]["Visible"] = true;

                //close
                dtsidefill.Rows.Add();
                dtsidefill.Rows[1]["Name"] = "quit";
                dtsidefill.Rows[1]["DisplayName"] = "Quit";
                dtsidefill.Rows[1]["ShortcutKey"] = "Esc";
                dtsidefill.Rows[1]["Visible"] = true;
            }
            else if (tabControl1.SelectedIndex == 1)
            {
                //back
                dtsidefill.Rows.Add();
                dtsidefill.Rows[0]["Name"] = "back";
                dtsidefill.Rows[0]["DisplayName"] = "Back";
                dtsidefill.Rows[0]["ShortcutKey"] = "";
                dtsidefill.Rows[0]["Visible"] = true;
                //save
                dtsidefill.Rows.Add();
                dtsidefill.Rows[1]["Name"] = "next2";
                dtsidefill.Rows[1]["DisplayName"] = "Next";
                dtsidefill.Rows[1]["ShortcutKey"] = "";
                dtsidefill.Rows[1]["Visible"] = true;

                //close
                dtsidefill.Rows.Add();
                dtsidefill.Rows[2]["Name"] = "quit";
                dtsidefill.Rows[2]["DisplayName"] = "Quit";
                dtsidefill.Rows[2]["ShortcutKey"] = "Esc";
                dtsidefill.Rows[2]["Visible"] = true;

            }

            else if (tabControl1.SelectedIndex == 2)
            {

                //back
                dtsidefill.Rows.Add();
                dtsidefill.Rows[0]["Name"] = "back2";
                dtsidefill.Rows[0]["DisplayName"] = "Back";
                dtsidefill.Rows[0]["ShortcutKey"] = "";
                dtsidefill.Rows[0]["Visible"] = true;
                //save
                dtsidefill.Rows.Add();
                dtsidefill.Rows[1]["Name"] = "finish";
                dtsidefill.Rows[1]["DisplayName"] = "Finish";
                dtsidefill.Rows[1]["ShortcutKey"] = "";
                dtsidefill.Rows[1]["Visible"] = true;

                //close
                dtsidefill.Rows.Add();
                dtsidefill.Rows[2]["Name"] = "quit";
                dtsidefill.Rows[2]["DisplayName"] = "Quit";
                dtsidefill.Rows[2]["ShortcutKey"] = "Esc";
                dtsidefill.Rows[2]["Visible"] = true;

            }



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

            if (name == "next")
            {
                if (textBox2.Text == "")
                {
                    textBox2.BackColor = Color.Aqua;
                }
                else
                {
                    Cursor.Current = Cursors.WaitCursor;
                    wb = (Excel.Workbook)apl.Workbooks.Open(ofd.FileName, true, true, misValue, null, null, false, misValue, null, false, false, misValue, misValue, misValue, false);
                    foreach (Excel.Worksheet ws in wb.Worksheets)
                    {
                        listBox8.Items.Add(ws.Name);
                    }
                    listBox1.Text = "Column A";
                    listBox2.Text = "Column B";
                    listBox11.Text = "Column C";
                    listBox12.Text = "Column D";
                    listBox3.Text = "Column E";
                    listBox4.Text = "Column F";
                    listBox5.Text = "Column G";
                    listBox6.Text = "Column H";
                    listBox7.Text = "Column I";
                    listBox8.SelectedIndex = 0;
                    listBox9.Text = "<None>";
                    listBox10.Text = "<None>";
                    listBox10.Text = "<None>";
                    Cursor.Current = Cursors.Default;
                    tabControl1.SelectedIndex = 1;
                    SideFill();
                }
            }
            else if (name == "back")
            {

                tabControl1.SelectedIndex = 0;
                SideFill();
            }


            else if (name == "next2")
            {
                if (listBox1.Text == "" || listBox2.Text == "" || listBox3.Text == "" || listBox4.Text == "" || listBox5.Text == "" || listBox6.Text == "")
                {
                    return;
                }

                dataGridView2.Rows.Clear();
                wb = (Excel.Workbook)apl.Workbooks.Open(ofd.FileName, true, true, misValue, null, null, false, misValue, null, false, false, misValue, misValue, misValue, false);
                ws = (Excel.Worksheet)wb.Worksheets[listBox8.SelectedIndex + 1];
                int i = 0;
                Excel.Range range;
                range = ws.UsedRange;
                Cursor.Current = Cursors.WaitCursor;
                while ((range.Cells[(i + 1), 1] as Excel.Range).Value2 != null)
                {

                    dataGridView2.Rows.Add();
                    dataGridView2.Rows[i].Cells["sno"].Value = (i + 1);
                    dataGridView2.Rows[i].Cells["desc"].Value = (range.Cells[(i + 1), listBox1.SelectedIndex + 1] as Excel.Range).Value2.ToString().Replace("  ", " ").Trim();
                    dataGridView2.Rows[i].Cells["pack"].Value = (range.Cells[(i + 1), listBox2.SelectedIndex + 1] as Excel.Range).Value2.ToString().Replace("  ", " ").Trim();
                    dataGridView2.Rows[i].Cells["unt"].Value = (range.Cells[(i + 1), listBox11.SelectedIndex + 1] as Excel.Range).Value2.ToString().Replace("  ", " ").Trim();
                    dataGridView2.Rows[i].Cells["pvalue1"].Value = (range.Cells[(i + 1), listBox12.SelectedIndex + 1] as Excel.Range).Value2.ToString().Replace("  ", " ").Trim();

                    
                    dataGridView2.Rows[i].Cells["company"].Value = (range.Cells[(i + 1), listBox3.SelectedIndex + 1] as Excel.Range).Value2.ToString().Replace("  ", " ").Trim();
                    dataGridView2.Rows[i].Cells["item"].Value = (range.Cells[(i + 1), listBox4.SelectedIndex + 1] as Excel.Range).Value2.ToString().Replace("  ", " ").Trim();
                    dataGridView2.Rows[i].Cells["pg"].Value = (range.Cells[(i + 1), listBox5.SelectedIndex + 1] as Excel.Range).Value2.ToString().Replace("  ", " ").Trim();
                    dataGridView2.Rows[i].Cells["col"].Value = (range.Cells[(i + 1), listBox6.SelectedIndex + 1] as Excel.Range).Value2.ToString().Replace("  ", " ").Trim();
                    dataGridView2.Rows[i].Cells["tax_cat"].Value = (range.Cells[(i + 1), listBox7.SelectedIndex + 1] as Excel.Range).Value2.ToString().Replace("  ", " ").Trim();
                    if (listBox9.Text != "<None>")
                    {
                        dataGridView2.Columns["Shortcod"].Visible = true;
                        dataGridView2.Rows[i].Cells["Shortcod"].Value = (range.Cells[(i + 1), listBox9.SelectedIndex] as Excel.Range).Value2.ToString().Replace("  ", " ").Trim();
                    }
                    else
                    {
                        dataGridView2.Columns["Shortcod"].Visible = false;
                    }
                    if (listBox10.Text != "<None>")
                    {
                        dataGridView2.Columns["skucode"].Visible = true;

                        dataGridView2.Rows[i].Cells["skucode"].Value = (range.Cells[(i + 1), listBox10.SelectedIndex] as Excel.Range).Value2.ToString().Replace("  ", " ").Trim();
                    }
                    else
                    {
                        dataGridView2.Columns["skucode"].Visible = false;
                    }


                    i++;
                }
                FindError();
                Cursor.Current = Cursors.Default;
                tabControl1.SelectedIndex = 2;
                SideFill();
            }
            else if (name == "back2")
            {
                tabControl1.SelectedIndex = 1;
                SideFill();
            }
            else if (name == "finish")
            {
                Cursor.Current = Cursors.WaitCursor;
                if (validate() == true)
                {
                    //int company, item, pg, col, descrip;

                    //DataTable dtOther = new DataTable("Other");
                    //Database.GetSqlData("Select * from other", dtOther);
                    //DataTable dtDescription = new DataTable("Description");
                    //Database.GetSqlData("Select * from Description", dtDescription);


                    //for (int i = 0; i < dataGridView2.Rows.Count; i++)
                    //{
                    //    company = select_other_id(dataGridView2.Rows[i].Cells["company"].Value.ToString(), 14);
                    //    item = select_other_id(dataGridView2.Rows[i].Cells["item"].Value.ToString(), 15);
                    //    pg = select_other_id(dataGridView2.Rows[i].Cells["pg"].Value.ToString(), 16);
                    //    col = select_other_id(dataGridView2.Rows[i].Cells["col"].Value.ToString(), 18);
                    //    descrip = funs.Select_des_id(dataGridView2.Rows[i].Cells["desc"].Value.ToString(), dataGridView2.Rows[i].Cells["pack"].Value.ToString());

                    //    if (company == 0)
                    //    {
                    //        //DataTable SCompany = dtServer.DefaultView.ToTable(true, "company");
                    //        //for (int y = 0; y < SCompany.Rows.Count; y++)
                    //        //{
                    //        //    if (dtOther.Select("Name='" + SCompany.Rows[y][0] + "' and Type=14").Length == 0)
                    //        //    {
                    //        //        dtOther.Rows.Add();
                    //        //        dtOther.Rows[LOther.Rows.Count - 1]["Name"] = SCompany.Rows[y][0];
                    //        //        dtOther.Rows[LOther.Rows.Count - 1]["Type"] = 14;
                    //        //    }
                    //        //}
                    //        Database.CommandExecutor("insert into Other(Name,Type,Blimit,Dlimit) values('" + dataGridView2.Rows[i].Cells["company"].Value.ToString() + "', 14,0,0)");
                    //        company = select_other_id(dataGridView2.Rows[i].Cells["company"].Value.ToString(), 14);
                    //    }

                    //    if (item == 0)
                    //    {
                    //        Database.CommandExecutor("insert into Other(Name,Type,Blimit,Dlimit) values('" + dataGridView2.Rows[i].Cells["item"].Value.ToString() + "', 15,0,0)");
                    //        item = select_other_id(dataGridView2.Rows[i].Cells["item"].Value.ToString(), 15);
                    //    }

                    //    if (pg == 0)
                    //    {
                    //        Database.CommandExecutor("insert into Other(Name,Type,Blimit,Dlimit) values('" + dataGridView2.Rows[i].Cells["pg"].Value.ToString() + "', 16,0,0)");
                    //        pg = select_other_id(dataGridView2.Rows[i].Cells["pg"].Value.ToString(), 16);
                    //    }

                    //    if (col == 0)
                    //    {
                    //        Database.CommandExecutor("insert into Other(Name,Type,Blimit,Dlimit) values('" + dataGridView2.Rows[i].Cells["col"].Value.ToString() + "', 18,0,0)");
                    //        col = select_other_id(dataGridView2.Rows[i].Cells["col"].Value.ToString(), 18);
                    //    }


                    //    if (descrip == 0)
                    //    {
                    //        dtDescription.Rows.Add(0);
                    //        dtDescription.Rows[dtDescription.Rows.Count - 1]["Pack_id"] = funs.Select_pack_id(dataGridView2.Rows[i].Cells["pack"].Value.ToString());
                    //        dtDescription.Rows[dtDescription.Rows.Count - 1]["Description"] = dataGridView2.Rows[i].Cells["desc"].Value.ToString();
                    //        dtDescription.Rows[dtDescription.Rows.Count - 1]["Retail"] = 0;
                    //        dtDescription.Rows[dtDescription.Rows.Count - 1]["Wholesale"] = 0;
                    //        dtDescription.Rows[dtDescription.Rows.Count - 1]["Purchase_rate"] = 0;
                    //        dtDescription.Rows[dtDescription.Rows.Count - 1]["Rate_X"] = 0;
                    //        dtDescription.Rows[dtDescription.Rows.Count - 1]["Rate_Y"] = 0;
                    //        dtDescription.Rows[dtDescription.Rows.Count - 1]["Rate_Z"] = 0;
                    //        dtDescription.Rows[dtDescription.Rows.Count - 1]["Wlavel"] = 0;
                    //        dtDescription.Rows[dtDescription.Rows.Count - 1]["Open_stock"] = 0;
                    //        dtDescription.Rows[dtDescription.Rows.Count - 1]["weight"] = 1;
                    //        dtDescription.Rows[dtDescription.Rows.Count - 1]["box_quantity"] = 1;
                    //        dtDescription.Rows[dtDescription.Rows.Count - 1]["discount_qty"] = 1;

                    //        dtDescription.Rows[dtDescription.Rows.Count - 1]["Company_id"] = company;
                    //        dtDescription.Rows[dtDescription.Rows.Count - 1]["Item_id"] = item;
                    //        dtDescription.Rows[dtDescription.Rows.Count - 1]["Col_id"] = col;
                    //        dtDescription.Rows[dtDescription.Rows.Count - 1]["Group_id"] = pg;
                    //        dtDescription.Rows[dtDescription.Rows.Count - 1]["Tax_Cat_id"] = funs.Select_tax_cat_id(dataGridView2.Rows[i].Cells["tax_cat"].Value.ToString());

                    //        dtDescription.Rows[dtDescription.Rows.Count - 1]["Commission%"] = 0;
                    //        dtDescription.Rows[dtDescription.Rows.Count - 1]["Commission@"] = 0;
                    //        dtDescription.Rows[dtDescription.Rows.Count - 1]["Mark"] = "None";
                    //        if (dataGridView2.Rows[i].Cells["Shortcod"].Value != null)
                    //        {
                    //            dtDescription.Rows[dtDescription.Rows.Count - 1]["Shortcode"] = dataGridView2.Rows[i].Cells["Shortcod"].Value.ToString();
                    //        }
                    //        else
                    //        {
                    //            dtDescription.Rows[dtDescription.Rows.Count - 1]["Shortcode"] = "";
                    //        }

                    //        if (dataGridView2.Rows[i].Cells["skucode"].Value != null)
                    //        {
                    //            dtDescription.Rows[dtDescription.Rows.Count - 1]["Skucode"] = dataGridView2.Rows[i].Cells["skucode"].Value.ToString();
                    //        }
                    //        else
                    //        {
                    //            dtDescription.Rows[dtDescription.Rows.Count - 1]["Skucode"] = null;
                    //        }
                    //    }
                    //    else
                    //    {
                    //        DataRow row = dtDescription.Select("des_id=" + descrip).FirstOrDefault();
                    //        row["Company_id"] = company;
                    //        row["Item_id"] = item;
                    //        row["Col_id"] = col;
                    //        row["Group_id"] = pg;
                    //        row["Tax_Cat_id"] = funs.Select_tax_cat_id(dataGridView2.Rows[i].Cells["tax_cat"].Value.ToString());
                    //        if (listBox9.Text != "<None>")
                    //        {
                    //            row["Shortcode"] = dataGridView2.Rows[i].Cells["Shortcode"].Value.ToString();
                    //        }
                    //        if (listBox10.Text != "<None>")
                    //        {
                    //            row["Skucode"] = dataGridView2.Rows[i].Cells["skucode"].Value.ToString();
                    //        }

                    //    }
                    //}
                    //Database.SaveData(dtDescription);
                    //Cursor.Current = Cursors.Default;
                    //MessageBox.Show("Description imported successfully");


                    string str = "";
                    DataTable dtServer = new DataTable();
                    dtServer.Columns.Add("desc", typeof(string));
                    dtServer.Columns.Add("packing", typeof(string));
                    dtServer.Columns.Add("unit", typeof(string));
                    dtServer.Columns.Add("pvalue", typeof(string));
                    dtServer.Columns.Add("company", typeof(string));
                    dtServer.Columns.Add("item", typeof(string));
                    dtServer.Columns.Add("pricegrp", typeof(string));
                    dtServer.Columns.Add("color", typeof(string));
                    dtServer.Columns.Add("taxcat", typeof(string));
                    dtServer.Columns.Add("Shortcod", typeof(string));
                    dtServer.Columns.Add("sku", typeof(string));
                    dtServer.Columns.Add("box_quantity", typeof(int));
                    for (int i = 0; i < dataGridView2.Rows.Count; i++)
                    {
                        str += dataGridView2.Rows[i].Cells["desc"].Value + "|";
                        str += dataGridView2.Rows[i].Cells["pack"].Value + "|";
                        str += dataGridView2.Rows[i].Cells["unt"].Value + "|";
                        str += dataGridView2.Rows[i].Cells["pvalue1"].Value + "|";
                        str += dataGridView2.Rows[i].Cells["company"].Value + "|";
                        str += dataGridView2.Rows[i].Cells["item"].Value + "|";
                        str += dataGridView2.Rows[i].Cells["pg"].Value + "|";
                        str += dataGridView2.Rows[i].Cells["col"].Value + "|";
                        str += dataGridView2.Rows[i].Cells["tax_cat"].Value + "|";
                        str += dataGridView2.Rows[i].Cells["Shortcod"].Value + "|";
                        str += dataGridView2.Rows[i].Cells["skucode"].Value + "~";
                      
                    }


                    string[] ar = str.Split('~');
                    for (int i = 0; i < ar.Length - 1; i++)
                    {
                        string[] Dcell = ar[i].Split('|');
                        dtServer.Rows.Add();
                        dtServer.Rows[i][0] = Dcell[0];
                        dtServer.Rows[i][1] = Dcell[1];
                        dtServer.Rows[i][2] = Dcell[2];
                        dtServer.Rows[i][3] = Dcell[3];
                        dtServer.Rows[i][4] = Dcell[4];
                        dtServer.Rows[i][5] = Dcell[5];
                        dtServer.Rows[i][6] = Dcell[6];
                        dtServer.Rows[i][7] = Dcell[7];
                        dtServer.Rows[i][8] = Dcell[8];
                        dtServer.Rows[i][9] = Dcell[9];
                        dtServer.Rows[i][10] = Dcell[10];
                       
                    }

                    DataTable LOther = new DataTable("Other");
                    Database.GetSqlData("select * from Other", LOther);

                    DataTable SCompany = dtServer.DefaultView.ToTable(true, "company");
                    for (int y = 0; y < SCompany.Rows.Count; y++)
                    {
                        if (LOther.Select("Name='" + SCompany.Rows[y][0] + "' and Type=14").Length == 0)
                        {
                            LOther.Rows.Add();
                            LOther.Rows[LOther.Rows.Count - 1]["Name"] = SCompany.Rows[y][0];
                            LOther.Rows[LOther.Rows.Count - 1]["Type"] = 14;
                        }
                    }

                    DataTable SItem = dtServer.DefaultView.ToTable(true, "item");
                    for (int y = 0; y < SItem.Rows.Count; y++)
                    {
                        if (LOther.Select("Name='" + SItem.Rows[y][0] + "' and Type=15").Length == 0)
                        {
                            LOther.Rows.Add();
                            LOther.Rows[LOther.Rows.Count - 1]["Name"] = SItem.Rows[y][0];
                            LOther.Rows[LOther.Rows.Count - 1]["Type"] = 15;
                        }
                    }

                    DataTable SPriceGrp = dtServer.DefaultView.ToTable(true, "pricegrp");
                    for (int y = 0; y < SPriceGrp.Rows.Count; y++)
                    {
                        if (LOther.Select("Name='" + SPriceGrp.Rows[y][0] + "' and Type=16").Length == 0)
                        {
                            LOther.Rows.Add();
                            LOther.Rows[LOther.Rows.Count - 1]["Name"] = SPriceGrp.Rows[y][0];
                            LOther.Rows[LOther.Rows.Count - 1]["Type"] = 16;
                        }
                    }

                    DataTable SColor = dtServer.DefaultView.ToTable(true, "color");
                    for (int y = 0; y < SColor.Rows.Count; y++)
                    {
                        if (LOther.Select("Name='" + SColor.Rows[y][0] + "' and Type=18").Length == 0)
                        {
                            LOther.Rows.Add();
                            LOther.Rows[LOther.Rows.Count - 1]["Name"] = SColor.Rows[y][0];
                            LOther.Rows[LOther.Rows.Count - 1]["Type"] = 18;
                        }
                    }

                    Database.SaveData(LOther);
                    Database.GetSqlData("select * from Other", LOther);

                   

                    DataTable dtDesc = new DataTable("Description");
                    Database.GetSqlData("Select * from Description where Des_id=0", dtDesc);

                    DataTable dtTaxCat = new DataTable("TAXCATEGORY");
                    Database.GetSqlData("Select * from TAXCATEGORY", dtTaxCat);


                    for (int s = 0; s < dtServer.Rows.Count; s++)
                    {
                        dtDesc.Rows.Add();
                        dtDesc.Rows[s]["Pack"] = dtServer.Rows[s]["Packing"];
                        dtDesc.Rows[s]["Description"] = dtServer.Rows[s]["desc"];

                        dtDesc.Rows[s]["rate_unit"] = dtServer.Rows[s]["unit"];
                        dtDesc.Rows[s]["pvalue"] = dtServer.Rows[s]["pvalue"];
                        dtDesc.Rows[s]["Retail"] = 0;
                        dtDesc.Rows[s]["Wholesale"] = 0;
                        dtDesc.Rows[s]["Company_id"] = LOther.Select("Name='" + dtServer.Rows[s]["company"] + "' and Type=14")[0][0];
                        dtDesc.Rows[s]["Item_id"] = LOther.Select("Name='" + dtServer.Rows[s]["item"] + "' and Type=15")[0][0];
                        dtDesc.Rows[s]["Col_id"] = LOther.Select("Name='" + dtServer.Rows[s]["color"] + "' and Type=18")[0][0];
                        dtDesc.Rows[s]["Group_id"] = LOther.Select("Name='" + dtServer.Rows[s]["pricegrp"] + "' and Type=16")[0][0];
                        dtDesc.Rows[s]["Tax_Cat_id"] = dtTaxCat.Select("Category_Name='" + dtServer.Rows[s]["taxcat"] + "'")[0][0];
                        dtDesc.Rows[s]["Purchase_rate"] = 0;
                        dtDesc.Rows[s]["Open_stock"] = 0;
                        dtDesc.Rows[s]["Mark"] = "No";
                        dtDesc.Rows[s]["Wlavel"] = 0;
                        dtDesc.Rows[s]["Commission%"] = 0;
                        dtDesc.Rows[s]["Commission@"] = 0;
                        dtDesc.Rows[s]["ShortCode"] = dtServer.Rows[s]["Shortcod"];
                        dtDesc.Rows[s]["box_quantity"] = 1;
                        dtDesc.Rows[s]["weight"] = 1;
                        dtDesc.Rows[s]["discount_qty"] = 1;
                        dtDesc.Rows[s]["Rate_X"] = 0;
                        dtDesc.Rows[s]["Rate_Y"] = 0;
                        dtDesc.Rows[s]["Rate_Z"] = 0;
                        dtDesc.Rows[s]["Skucode"] = dtServer.Rows[s]["sku"];
                        dtDesc.Rows[s]["box_quantity"] = dtServer.Rows[s]["box_quantity"];
                    }
                    Database.SaveData(dtDesc);
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show("Description imported successfully");

                    Master.UpdateDecription();
                    Master.UpdateDecriptionInfo();

                    apl.Quit();
                    this.Close();
                    this.Dispose();
                }


            }

            if (name == "quit")
            {
                this.Close();
                this.Dispose();
            }


        }

        private void button8_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox2.Text == "")
            {
                textBox2.BackColor = Color.Aqua;
            }
            else
            {
                Cursor.Current = Cursors.WaitCursor;
                wb = (Excel.Workbook)apl.Workbooks.Open(ofd.FileName, true, true, misValue, null, null, false, misValue, null, false, false, misValue, misValue, misValue, false);
                foreach (Excel.Worksheet ws in wb.Worksheets)
                {
                    listBox8.Items.Add(ws.Name);
                }
                listBox1.Text = "Column A";
                listBox2.Text = "Column B";
                listBox3.Text = "Column C";
                listBox4.Text = "Column D";
                listBox5.Text = "Column E";
                listBox6.Text = "Column F";
                listBox7.Text = "Column G";
                listBox8.SelectedIndex = 0;
                listBox9.Text = "<None>";
                listBox10.Text = "<None>";
                Cursor.Current = Cursors.Default;
                tabControl1.SelectedIndex = 1;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (listBox1.Text == "" || listBox2.Text == "" || listBox3.Text == "" || listBox4.Text == "" || listBox5.Text == "" || listBox6.Text == "")
            {
                return;
            }

            dataGridView2.Rows.Clear();
            wb = (Excel.Workbook)apl.Workbooks.Open(ofd.FileName, true, true, misValue, null, null, false, misValue, null, false, false, misValue, misValue, misValue, false);
            ws = (Excel.Worksheet)wb.Worksheets[listBox8.SelectedIndex + 1];
            int i = 0;
            Excel.Range range;
            range = ws.UsedRange;
            Cursor.Current = Cursors.WaitCursor;
            while ((range.Cells[(i + 1), 1] as Excel.Range).Value2 != null)
            {

                dataGridView2.Rows.Add();
                dataGridView2.Rows[i].Cells["sno"].Value = (i + 1);
                dataGridView2.Rows[i].Cells["desc"].Value = (range.Cells[(i + 1), listBox1.SelectedIndex + 1] as Excel.Range).Value2.ToString().Replace("  ", " ").Trim();
                dataGridView2.Rows[i].Cells["pack"].Value = (range.Cells[(i + 1), listBox2.SelectedIndex + 1] as Excel.Range).Value2.ToString().Replace("  ", " ").Trim();
                dataGridView2.Rows[i].Cells["company"].Value = (range.Cells[(i + 1), listBox3.SelectedIndex + 1] as Excel.Range).Value2.ToString().Replace("  ", " ").Trim();
                dataGridView2.Rows[i].Cells["item"].Value = (range.Cells[(i + 1), listBox4.SelectedIndex + 1] as Excel.Range).Value2.ToString().Replace("  ", " ").Trim();
                dataGridView2.Rows[i].Cells["pg"].Value = (range.Cells[(i + 1), listBox5.SelectedIndex + 1] as Excel.Range).Value2.ToString().Replace("  ", " ").Trim();
                dataGridView2.Rows[i].Cells["col"].Value = (range.Cells[(i + 1), listBox6.SelectedIndex + 1] as Excel.Range).Value2.ToString().Replace("  ", " ").Trim();
                dataGridView2.Rows[i].Cells["tax_cat"].Value = (range.Cells[(i + 1), listBox7.SelectedIndex + 1] as Excel.Range).Value2.ToString().Replace("  ", " ").Trim();
                if (listBox9.Text != "<None>")
                {
                    dataGridView2.Columns["Shortcod"].Visible = true;
                    dataGridView2.Rows[i].Cells["Shortcod"].Value = (range.Cells[(i + 1), listBox9.SelectedIndex] as Excel.Range).Value2.ToString().Replace("  ", " ").Trim();
                }
                else
                {
                    dataGridView2.Columns["Shortcod"].Visible = false;
                }
                if (listBox10.Text != "<None>")
                {
                    dataGridView2.Columns["skucode"].Visible = true;

                    dataGridView2.Rows[i].Cells["skucode"].Value = (range.Cells[(i + 1), listBox10.SelectedIndex] as Excel.Range).Value2.ToString().Replace("  ", " ").Trim();
                }
                else
                {
                    dataGridView2.Columns["skucode"].Visible = false;
                }


                i++;
            }
            FindError();
            Cursor.Current = Cursors.Default;
            tabControl1.SelectedIndex = 2;

        }

        private void button6_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 1;
        }


        private void button9_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (validate() == true)
            {
                //int company, item, pg, col, descrip;

                //DataTable dtOther = new DataTable("Other");
                //Database.GetSqlData("Select * from other", dtOther);
                //DataTable dtDescription = new DataTable("Description");
                //Database.GetSqlData("Select * from Description", dtDescription);


                //for (int i = 0; i < dataGridView2.Rows.Count; i++)
                //{
                //    company = select_other_id(dataGridView2.Rows[i].Cells["company"].Value.ToString(), 14);
                //    item = select_other_id(dataGridView2.Rows[i].Cells["item"].Value.ToString(), 15);
                //    pg = select_other_id(dataGridView2.Rows[i].Cells["pg"].Value.ToString(), 16);
                //    col = select_other_id(dataGridView2.Rows[i].Cells["col"].Value.ToString(), 18);
                //    descrip = funs.Select_des_id(dataGridView2.Rows[i].Cells["desc"].Value.ToString(), dataGridView2.Rows[i].Cells["pack"].Value.ToString());

                //    if (company == 0)
                //    {
                //        //DataTable SCompany = dtServer.DefaultView.ToTable(true, "company");
                //        //for (int y = 0; y < SCompany.Rows.Count; y++)
                //        //{
                //        //    if (dtOther.Select("Name='" + SCompany.Rows[y][0] + "' and Type=14").Length == 0)
                //        //    {
                //        //        dtOther.Rows.Add();
                //        //        dtOther.Rows[LOther.Rows.Count - 1]["Name"] = SCompany.Rows[y][0];
                //        //        dtOther.Rows[LOther.Rows.Count - 1]["Type"] = 14;
                //        //    }
                //        //}
                //        Database.CommandExecutor("insert into Other(Name,Type,Blimit,Dlimit) values('" + dataGridView2.Rows[i].Cells["company"].Value.ToString() + "', 14,0,0)");
                //        company = select_other_id(dataGridView2.Rows[i].Cells["company"].Value.ToString(), 14);
                //    }

                //    if (item == 0)
                //    {
                //        Database.CommandExecutor("insert into Other(Name,Type,Blimit,Dlimit) values('" + dataGridView2.Rows[i].Cells["item"].Value.ToString() + "', 15,0,0)");
                //        item = select_other_id(dataGridView2.Rows[i].Cells["item"].Value.ToString(), 15);
                //    }

                //    if (pg == 0)
                //    {
                //        Database.CommandExecutor("insert into Other(Name,Type,Blimit,Dlimit) values('" + dataGridView2.Rows[i].Cells["pg"].Value.ToString() + "', 16,0,0)");
                //        pg = select_other_id(dataGridView2.Rows[i].Cells["pg"].Value.ToString(), 16);
                //    }

                //    if (col == 0)
                //    {
                //        Database.CommandExecutor("insert into Other(Name,Type,Blimit,Dlimit) values('" + dataGridView2.Rows[i].Cells["col"].Value.ToString() + "', 18,0,0)");
                //        col = select_other_id(dataGridView2.Rows[i].Cells["col"].Value.ToString(), 18);
                //    }


                //    if (descrip == 0)
                //    {
                //        dtDescription.Rows.Add(0);
                //        dtDescription.Rows[dtDescription.Rows.Count - 1]["Pack_id"] = funs.Select_pack_id(dataGridView2.Rows[i].Cells["pack"].Value.ToString());
                //        dtDescription.Rows[dtDescription.Rows.Count - 1]["Description"] = dataGridView2.Rows[i].Cells["desc"].Value.ToString();
                //        dtDescription.Rows[dtDescription.Rows.Count - 1]["Retail"] = 0;
                //        dtDescription.Rows[dtDescription.Rows.Count - 1]["Wholesale"] = 0;
                //        dtDescription.Rows[dtDescription.Rows.Count - 1]["Purchase_rate"] = 0;
                //        dtDescription.Rows[dtDescription.Rows.Count - 1]["Rate_X"] = 0;
                //        dtDescription.Rows[dtDescription.Rows.Count - 1]["Rate_Y"] = 0;
                //        dtDescription.Rows[dtDescription.Rows.Count - 1]["Rate_Z"] = 0;
                //        dtDescription.Rows[dtDescription.Rows.Count - 1]["Wlavel"] = 0;
                //        dtDescription.Rows[dtDescription.Rows.Count - 1]["Open_stock"] = 0;
                //        dtDescription.Rows[dtDescription.Rows.Count - 1]["weight"] = 1;
                //        dtDescription.Rows[dtDescription.Rows.Count - 1]["box_quantity"] = 1;
                //        dtDescription.Rows[dtDescription.Rows.Count - 1]["discount_qty"] = 1;

                //        dtDescription.Rows[dtDescription.Rows.Count - 1]["Company_id"] = company;
                //        dtDescription.Rows[dtDescription.Rows.Count - 1]["Item_id"] = item;
                //        dtDescription.Rows[dtDescription.Rows.Count - 1]["Col_id"] = col;
                //        dtDescription.Rows[dtDescription.Rows.Count - 1]["Group_id"] = pg;
                //        dtDescription.Rows[dtDescription.Rows.Count - 1]["Tax_Cat_id"] = funs.Select_tax_cat_id(dataGridView2.Rows[i].Cells["tax_cat"].Value.ToString());

                //        dtDescription.Rows[dtDescription.Rows.Count - 1]["Commission%"] = 0;
                //        dtDescription.Rows[dtDescription.Rows.Count - 1]["Commission@"] = 0;
                //        dtDescription.Rows[dtDescription.Rows.Count - 1]["Mark"] = "None";
                //        if (dataGridView2.Rows[i].Cells["Shortcod"].Value != null)
                //        {
                //            dtDescription.Rows[dtDescription.Rows.Count - 1]["Shortcode"] = dataGridView2.Rows[i].Cells["Shortcod"].Value.ToString();
                //        }
                //        else
                //        {
                //            dtDescription.Rows[dtDescription.Rows.Count - 1]["Shortcode"] = "";
                //        }

                //        if (dataGridView2.Rows[i].Cells["skucode"].Value != null)
                //        {
                //            dtDescription.Rows[dtDescription.Rows.Count - 1]["Skucode"] = dataGridView2.Rows[i].Cells["skucode"].Value.ToString();
                //        }
                //        else
                //        {
                //            dtDescription.Rows[dtDescription.Rows.Count - 1]["Skucode"] = null;
                //        }
                //    }
                //    else
                //    {
                //        DataRow row = dtDescription.Select("des_id=" + descrip).FirstOrDefault();
                //        row["Company_id"] = company;
                //        row["Item_id"] = item;
                //        row["Col_id"] = col;
                //        row["Group_id"] = pg;
                //        row["Tax_Cat_id"] = funs.Select_tax_cat_id(dataGridView2.Rows[i].Cells["tax_cat"].Value.ToString());
                //        if (listBox9.Text != "<None>")
                //        {
                //            row["Shortcode"] = dataGridView2.Rows[i].Cells["Shortcode"].Value.ToString();
                //        }
                //        if (listBox10.Text != "<None>")
                //        {
                //            row["Skucode"] = dataGridView2.Rows[i].Cells["skucode"].Value.ToString();
                //        }

                //    }
                //}
                //Database.SaveData(dtDescription);
                //Cursor.Current = Cursors.Default;
                //MessageBox.Show("Description imported successfully");


                string str = "";
                DataTable dtServer = new DataTable();
                dtServer.Columns.Add("desc", typeof(string));
                dtServer.Columns.Add("packing", typeof(string));
                dtServer.Columns.Add("company", typeof(string));
                dtServer.Columns.Add("item", typeof(string));
                dtServer.Columns.Add("pricegrp", typeof(string));
                dtServer.Columns.Add("color", typeof(string));
                dtServer.Columns.Add("taxcat", typeof(string));
                dtServer.Columns.Add("Shortcod", typeof(string));
                dtServer.Columns.Add("sku", typeof(string));
                for (int i = 0; i < dataGridView2.Rows.Count; i++)
                {
                    str += dataGridView2.Rows[i].Cells["desc"].Value + "|";
                    str += dataGridView2.Rows[i].Cells["pack"].Value + "|";
                    str += dataGridView2.Rows[i].Cells["company"].Value + "|";
                    str += dataGridView2.Rows[i].Cells["item"].Value + "|";
                    str += dataGridView2.Rows[i].Cells["pg"].Value + "|";
                    str += dataGridView2.Rows[i].Cells["col"].Value + "|";
                    str += dataGridView2.Rows[i].Cells["tax_cat"].Value + "|";
                    str += dataGridView2.Rows[i].Cells["Shortcod"].Value + "|";
                    str += dataGridView2.Rows[i].Cells["skucode"].Value + "~";
                }


                string[] ar = str.Split('~');
                for (int i = 0; i < ar.Length - 1; i++)
                {
                    string[] Dcell = ar[i].Split('|');
                    dtServer.Rows.Add();
                    dtServer.Rows[i][0] = Dcell[0];
                    dtServer.Rows[i][1] = Dcell[1];
                    dtServer.Rows[i][2] = Dcell[2];
                    dtServer.Rows[i][3] = Dcell[3];
                    dtServer.Rows[i][4] = Dcell[4];
                    dtServer.Rows[i][5] = Dcell[5];
                    dtServer.Rows[i][6] = Dcell[6];
                    dtServer.Rows[i][7] = Dcell[7];
                    dtServer.Rows[i][8] = Dcell[8];
                }

                DataTable LOther = new DataTable("Other");
                Database.GetSqlData("select * from Other", LOther);

                DataTable SCompany = dtServer.DefaultView.ToTable(true, "company");
                for (int y = 0; y < SCompany.Rows.Count; y++)
                {
                    if (LOther.Select("Name='" + SCompany.Rows[y][0] + "' and Type=14").Length == 0)
                    {
                        LOther.Rows.Add();
                        LOther.Rows[LOther.Rows.Count - 1]["Name"] = SCompany.Rows[y][0];
                        LOther.Rows[LOther.Rows.Count - 1]["Type"] = 14;
                    }
                }

                DataTable SItem = dtServer.DefaultView.ToTable(true, "item");
                for (int y = 0; y < SItem.Rows.Count; y++)
                {
                    if (LOther.Select("Name='" + SItem.Rows[y][0] + "' and Type=15").Length == 0)
                    {
                        LOther.Rows.Add();
                        LOther.Rows[LOther.Rows.Count - 1]["Name"] = SItem.Rows[y][0];
                        LOther.Rows[LOther.Rows.Count - 1]["Type"] = 15;
                    }
                }

                DataTable SPriceGrp = dtServer.DefaultView.ToTable(true, "pricegrp");
                for (int y = 0; y < SPriceGrp.Rows.Count; y++)
                {
                    if (LOther.Select("Name='" + SPriceGrp.Rows[y][0] + "' and Type=16").Length == 0)
                    {
                        LOther.Rows.Add();
                        LOther.Rows[LOther.Rows.Count - 1]["Name"] = SPriceGrp.Rows[y][0];
                        LOther.Rows[LOther.Rows.Count - 1]["Type"] = 16;
                    }
                }

                DataTable SColor = dtServer.DefaultView.ToTable(true, "color");
                for (int y = 0; y < SColor.Rows.Count; y++)
                {
                    if (LOther.Select("Name='" + SColor.Rows[y][0] + "' and Type=18").Length == 0)
                    {
                        LOther.Rows.Add();
                        LOther.Rows[LOther.Rows.Count - 1]["Name"] = SColor.Rows[y][0];
                        LOther.Rows[LOther.Rows.Count - 1]["Type"] = 18;
                    }
                }

                Database.SaveData(LOther);
                Database.GetSqlData("select * from Other", LOther);

                DataTable dtPacking = new DataTable();
                Database.GetSqlData("Select * from Packing", dtPacking);


                DataTable dtDesc = new DataTable("Description");
                Database.GetSqlData("Select * from Description where Des_id=0", dtDesc);

                DataTable dtTaxCat = new DataTable("TAXCATEGORY");
                Database.GetSqlData("Select * from TAXCATEGORY", dtTaxCat);


                for (int s = 0; s < dtServer.Rows.Count; s++)
                {
                    dtDesc.Rows.Add();
                    if (dtPacking.Select("Name='" + dtServer.Rows[s]["packing"] + "' ").Length == 0)
                    {
                        MessageBox.Show(dtServer.Rows[s]["desc"] + ", " + dtServer.Rows[s]["packing"].ToString() + " Not Found");
                        return;
                    }
                    else
                    {
                        dtDesc.Rows[s]["Pack_id"] = dtPacking.Select("Name='" + dtServer.Rows[s]["packing"] + "' ")[0][0];
                    }
                    dtDesc.Rows[s]["Description"] = dtServer.Rows[s]["desc"];
                    dtDesc.Rows[s]["Retail"] = 0;
                    dtDesc.Rows[s]["Wholesale"] = 0;
                    dtDesc.Rows[s]["Company_id"] = LOther.Select("Name='" + dtServer.Rows[s]["company"] + "' and Type=14")[0][0];
                    dtDesc.Rows[s]["Item_id"] = LOther.Select("Name='" + dtServer.Rows[s]["item"] + "' and Type=15")[0][0];
                    dtDesc.Rows[s]["Col_id"] = LOther.Select("Name='" + dtServer.Rows[s]["color"] + "' and Type=18")[0][0];
                    dtDesc.Rows[s]["Group_id"] = LOther.Select("Name='" + dtServer.Rows[s]["pricegrp"] + "' and Type=16")[0][0];
                    dtDesc.Rows[s]["Tax_Cat_id"] = dtTaxCat.Select("Category_Name='" + dtServer.Rows[s]["taxcat"] + "'")[0][0];
                    dtDesc.Rows[s]["Purchase_rate"] = 0;
                    dtDesc.Rows[s]["Open_stock"] = 0;
                    dtDesc.Rows[s]["Mark"] = "No";
                    dtDesc.Rows[s]["Wlavel"] = 0;
                    dtDesc.Rows[s]["Commission%"] = 0;
                    dtDesc.Rows[s]["Commission@"] = 0;
                    dtDesc.Rows[s]["ShortCode"] = dtServer.Rows[s]["Shortcod"];
                    dtDesc.Rows[s]["box_quantity"] = 1;
                    dtDesc.Rows[s]["weight"] = 1;
                    dtDesc.Rows[s]["discount_qty"] = 1;
                    dtDesc.Rows[s]["Rate_X"] = 0;
                    dtDesc.Rows[s]["Rate_Y"] = 0;
                    dtDesc.Rows[s]["Rate_Z"] = 0;
                    dtDesc.Rows[s]["Skucode"] = dtServer.Rows[s]["sku"];

                }
                Database.SaveData(dtDesc);
                Cursor.Current = Cursors.Default;
                MessageBox.Show("Description imported successfully");

                Master.UpdateDecription();
                Master.UpdateDecriptionInfo();

                apl.Quit();
                this.Close();
                this.Dispose();
            }


        }

        private int select_other_id(String nm, int type)
        {
            DataTable dtOtherId = new DataTable("Other");
            int other_id = 0;
            dtOtherId.Clear();
            Database.GetSqlData("SELECT Oth_id FROM OTHER WHERE Name='" + nm + "' and type=" + type, dtOtherId);
            if (dtOtherId.Rows.Count > 0)
            {
                other_id = int.Parse(dtOtherId.Rows[0][0].ToString());
            }

            return other_id;
        }

        private void FindError()
        {
            bool Report = true;
            for (int i = 0; i < dataGridView2.Rows.Count; i++)
            {

                if (dataGridView2.Rows[i].Cells["pack"].Value == null || dataGridView2.Rows[i].Cells["pack"].Value.ToString() == "")
                {
                    dataGridView2.Rows[i].Cells["pack"].Style.BackColor = Color.Red;
                    Report = false;
                }
                if (dataGridView2.Rows[i].Cells["tax_cat"].Value == null || funs.Select_tax_cat_id(dataGridView2.Rows[i].Cells["tax_cat"].Value.ToString()) == "")
                {
                    dataGridView2.Rows[i].Cells["tax_cat"].Style.BackColor = Color.Red;
                    Report = false;
                }


            }
            if (Report == false)
            {
                MessageBox.Show("Invalid Entries Are Marked as Red, Please See", "Pack Or Tax Catagory Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private bool validate()
        {
            bool Report = true;
            for (int i = 0; i < dataGridView2.Rows.Count; i++)
            {

                if (dataGridView2.Rows[i].Cells["pack"].Value == null || dataGridView2.Rows[i].Cells["pack"].Value.ToString() == "")
                {
                    dataGridView2.Rows[i].Cells["pack"].Style.BackColor = Color.Red;
                    Report = false;
                }
                else
                {
                    dataGridView2.Rows[i].Cells["pack"].Style.BackColor = Color.White;
                }

                if (dataGridView2.Rows[i].Cells["unt"].Value == null || dataGridView2.Rows[i].Cells["unt"].Value.ToString() == "")
                {
                    dataGridView2.Rows[i].Cells["unt"].Style.BackColor = Color.Red;
                    Report = false;
                }
                else
                {
                    dataGridView2.Rows[i].Cells["unt"].Style.BackColor = Color.White;
                }


                if (dataGridView2.Rows[i].Cells["pvalue1"].Value == null || dataGridView2.Rows[i].Cells["pvalue1"].Value.ToString() == "")
                {
                    dataGridView2.Rows[i].Cells["pvalue1"].Style.BackColor = Color.Red;
                    Report = false;
                }
                else
                {
                    dataGridView2.Rows[i].Cells["pvalue1"].Style.BackColor = Color.White;
                }


                if (dataGridView2.Rows[i].Cells["tax_cat"].Value == null || funs.Select_tax_cat_id(dataGridView2.Rows[i].Cells["tax_cat"].Value.ToString()) == "")
                {
                    dataGridView2.Rows[i].Cells["tax_cat"].Style.BackColor = Color.Red;
                    Report = false;
                }
                else
                {
                    dataGridView2.Rows[i].Cells["tax_cat"].Style.BackColor = Color.White;
                }

            }
            if (Report == false)
            {
                MessageBox.Show("Invalid Entries Are Marked as Red, Please See", "Pack Or Tax Catagory Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else
            {
                return true;
            }
        }
        private void tabControl1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {

                DialogResult chk = MessageBox.Show("Are u sure?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (chk == DialogResult.No)
                {
                    e.Handled = false;
                }
                else
                {
                    this.Dispose();
                }
            }
        }

        private void dataGridView2_CellEnter(object sender, DataGridViewCellEventArgs e)
        {

            if (dataGridView2.CurrentCell.OwningColumn.Name == "sno")
            {
                SendKeys.Send("{right}");
                this.Activate();
            }
        }

        private void listBox8_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

    }
}
