using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace faspi
{
    public partial class frm_updaterates : Form
    {
        string strCombo = "";
        DataTable dtRate;
        public string type = "";
        string product_id = "";

        public frm_updaterates()
        {
            InitializeComponent();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            strCombo = "select [name] as Company from other where Type='SER14' order by [name]";
            textBox1.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
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
            dtsidefill.Rows[0]["Name"] = "save";
            dtsidefill.Rows[0]["DisplayName"] = "Save";
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

            if (name == "save")
            {
                if (validate() == true)
                {
                    save();
                    textBox1.Text = "";
                    textBox2.Text = "";
                    textBox3.Text = "";
                    ansGridView1.Rows.Clear();
                    ansGridView1.Columns.Clear();
                }
            }
            else if (name == "quit")
            {
                this.Close();
                this.Dispose();
            }
        }

        private bool validate()
        {
            if (textBox1.Text == "" && textBox2.Text == "")
            {
                MessageBox.Show("Please Enter Atleast One..");
                return false;
            }
            if (ansGridView1.Rows.Count == 0)//-1
            {
                MessageBox.Show("Enter some Values");
                return false;
            }
            return true;
        }

        private void save()
        {
            ansGridView1.EndEdit();
            for (int i = 0; i < ansGridView1.Rows.Count; i++)
            {
                for (int k = 1; k < ansGridView1.Columns.Count; k++)
                {
                    if (ansGridView1.Rows[i].Cells[k].Value.ToString() == "" || ansGridView1.Rows[i].Cells[k].Value == null)
                    {
                        ansGridView1.Rows[i].Cells[k].Value = 0;
                    }
                    string str1 = "";
                    string str2 = "";
                    if (textBox1.Text != "")
                    {
                        str1 = "and company_id='" + funs.Select_oth_id(textBox1.Text) + "'";
                    }
                    if (textBox2.Text != "")
                    {
                        str2 = " and Item_id='" + funs.Select_oth_id(textBox2.Text) + "'";
                    }
                    if (textBox3.Text != "MRP" && textBox3.Text != "Rebate")
                    {
                        if (funs.Select_color_group_id(ansGridView1.Rows[i].Cells[0].Value.ToString()) == "0")
                        {
                            Database.CommandExecutor("Update Description set " + funs.Select_Rates_Id(textBox3.Text) + "= " + double.Parse(ansGridView1.Rows[i].Cells[k].Value.ToString()) + " where pack='" + ansGridView1.Columns[k].HeaderText + "'  "+str1+str2+"  and Group_id='' ");
                        }
                        else
                        {
                            Database.CommandExecutor("Update Description set " + funs.Select_Rates_Id(textBox3.Text) + "= " + double.Parse(ansGridView1.Rows[i].Cells[k].Value.ToString()) + " where pack='" + ansGridView1.Columns[k].HeaderText + "' " + str1 + str2 + " and Group_id='" + funs.Select_color_group_id(ansGridView1.Rows[i].Cells[0].Value.ToString()) + "' ");
                        }
                        //Database.CommandExecutor("Update Description set " + funs.Select_Rates_Id(textBox3.Text) + "= " + double.Parse(ansGridView1.Rows[i].Cells[k].Value.ToString()) + " where pack='" + ansGridView1.Columns[k].HeaderText + "' and company_id='" + funs.Select_oth_id(textBox1.Text) + "' and Item_id='" + funs.Select_oth_id(textBox2.Text) + "' and Group_id='" + funs.Select_color_group_id(ansGridView1.Rows[i].Cells[0].Value.ToString()) + "' ");
                    }
                    else
                    {
                        Database.CommandExecutor("Update Description set " + textBox3.Text + "= " + double.Parse(ansGridView1.Rows[i].Cells[k].Value.ToString()) + " where pack='" + ansGridView1.Columns[k].HeaderText + "'  " + str1 + str2 + " and Group_id='" + funs.Select_color_group_id(ansGridView1.Rows[i].Cells[0].Value.ToString()) + "' ");
                    }
                }
            }
            Master.UpdateRates();
            Master.UpdateDecription();
            Master.UpdateDecriptionInfo();
            funs.ShowBalloonTip("Saved", "Saved Successfully");
        }

        private void Loaddata()
        {
            foreach (DataGridViewColumn column in ansGridView1.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            //if (textBox1.Text == "")
            //{
            //    ansGridView1.Rows.Clear();
            //    ansGridView1.Columns.Clear();
            //}
            //else
            //{
                DataTable dtother = new DataTable();
                ansGridView1.Rows.Clear();
                ansGridView1.Columns.Clear();

                string str1 = "";
                string str2 = "";

                if (textBox1.Text != "")
                {
                    str1 = " (OTHER.Name = '" + textBox1.Text + "') ";
                }
                if (textBox2.Text != "")
                {
                    if (str1 == "")
                    {
                        str2 = " (OTHER_1.Name = '" + textBox2.Text + "') ";
                    }
                    else
                    {
                        str2 = " And (OTHER_1.Name = '" + textBox2.Text + "') ";
                    }
                }
                if (type == "Rates")
                {
                    string str3 = "";
                    if (textBox2.Text != "")
                    {
                        if (str1 == "")
                        {
                            str3 = " (OTHER_2.Name = '" + textBox2.Text + "') ";
                        }
                        else
                        {
                            str3 = " And (OTHER_2.Name = '" + textBox2.Text + "') ";
                        }
                    }
                    Database.GetSqlData("SELECT Description.Pack AS Packing FROM Description INNER JOIN OTHER ON Description.Company_id = OTHER.Oth_id LEFT OUTER JOIN OTHER AS OTHER_1 ON Description.Item_id = OTHER_1.Oth_id WHERE "+str1+str2+" GROUP BY Description.Pack ORDER BY MAX(Description.Pvalue) DESC", dtother);

                    if (dtother.Rows.Count == 0)
                    {
                        return;
                    }

                    string strColumns = "";
                    for (int i = 0; i < dtother.Rows.Count; i++)
                    {
                        strColumns += "[" + dtother.Rows[i]["Packing"].ToString() + "],";
                    }
                    strColumns = strColumns.TrimEnd(',');
                    product_id = funs.Select_oth_id(textBox1.Text);


                    dtRate = new DataTable();
                    if (textBox3.Text != "MRP" && textBox3.Text != "Rebate")
                    {
                        Database.GetSqlData("Select  grp,  " + strColumns + "  from (SELECT OTHER_1.Name as grp, Description.Pack, MAX(Description." + funs.Select_Rates_Id(textBox3.Text) + ") AS Rate FROM Description LEFT OUTER JOIN OTHER AS OTHER_2 ON Description.Item_id = OTHER_2.Oth_id LEFT OUTER JOIN OTHER ON Description.Company_id = OTHER.Oth_id LEFT OUTER JOIN OTHER AS OTHER_1 ON Description.Group_id = OTHER_1.Oth_id WHERE "+str1 + str3+" GROUP BY OTHER_1.Name, Description.Pack) as res PIVOT (max(Rate) FOR Pack  IN (" + strColumns + ")) AS Pivoting ORDER BY grp," + strColumns + " DESC", dtRate);
                    }
                    else
                    {
                        Database.GetSqlData("Select  grp,  " + strColumns + "  from (SELECT OTHER_1.Name as grp, Description.Pack, MAX(Description." + textBox3.Text + ") AS Rate FROM Description LEFT OUTER JOIN OTHER AS OTHER_2 ON Description.Item_id = OTHER_2.Oth_id LEFT OUTER JOIN OTHER ON Description.Company_id = OTHER.Oth_id LEFT OUTER JOIN OTHER AS OTHER_1 ON Description.Group_id = OTHER_1.Oth_id WHERE " + str1 + str3 + " GROUP BY OTHER_1.Name, Description.Pack) as res PIVOT (max(Rate) FOR Pack  IN (" + strColumns + ")) AS Pivoting ORDER BY grp," + strColumns + " DESC", dtRate);
                    }
                    for (int i = 0; i < dtRate.Columns.Count; i++)
                    {
                        dtRate.Columns[i].ColumnName = dtRate.Columns[i].ColumnName.Replace('_', '.');
                    }
                    if (dtRate.Rows.Count == 0)
                    {


                    }
                    else
                    {
                        for (int i = 0; i < dtRate.Columns.Count; i++)
                        {
                            ansGridView1.Columns.Add(dtRate.Columns[i].ColumnName, dtRate.Columns[i].ColumnName);
                        }
                        for (int l = 0; l < dtRate.Rows.Count; l++)
                        {
                            ansGridView1.Rows.Add();
                            ansGridView1.Columns[0].ReadOnly = true;
                            for (int k = 0; k < dtRate.Columns.Count; k++)
                            {
                                ansGridView1.Rows[l].Cells[dtRate.Columns[k].ColumnName].Value = dtRate.Rows[l][dtRate.Columns[k].ColumnName].ToString();
                            }
                        }
                    }
                }
           // }
        }

        private void frm_updaterates_Load(object sender, EventArgs e)
        {
            SideFill();
            this.Size = this.MdiParent.Size;
            if (type == "Rates")
            {
                label1.Text = "Modify Items Rates";
            }
            else if (type == "Purchase Rebates")
            {
                label1.Text = "Modify Items Purchase Rebates";
            }
            else if (type == "Sale Rebates")
            {
                label1.Text = "Modify Items Sale Rebates";
            }
        }

        private void frm_updaterates_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
            {
                save();

                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                ansGridView1.Rows.Clear();
                ansGridView1.Columns.Clear();
            }
            else if (e.KeyCode == Keys.Escape)
            {
                this.Close();
                this.Dispose();
            }
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
           // DataTable dt1 = Master.DtRates.Select().CopyToDataTable();
            DataTable DtRates = new DataTable();
            DtRates.Columns.Add("RateValue", typeof(string));
            DtRates.Columns.Add("RateId", typeof(string));

            DtRates.Rows.Add();
            DtRates.Rows[0][0] = faspi.Feature.Available("Name of PriceList1");
            DtRates.Rows[0][1] = "Purchase_rate";

            DtRates.Rows.Add();
            DtRates.Rows[1][0] = faspi.Feature.Available("Name of PriceList2");
            DtRates.Rows[1][1] = "Retail";

            DtRates.Rows.Add();
            DtRates.Rows[2][0] = faspi.Feature.Available("Name of PriceList3");
            DtRates.Rows[2][1] = "Wholesale";

            DtRates.Rows.Add();
            DtRates.Rows[3][0] = faspi.Feature.Available("Name of PriceList4");
            DtRates.Rows[3][1] = "Rate_X";

            DtRates.Rows.Add();
            DtRates.Rows[4][0] = faspi.Feature.Available("Name of PriceList5");
            DtRates.Rows[4][1] = "Rate_Y";

            DtRates.Rows.Add();
            DtRates.Rows[5][0] = faspi.Feature.Available("Name of PriceList6");
            DtRates.Rows[5][1] = "Rate_Z";



            DtRates.Rows.Add();
            DtRates.Rows[6][0] ="MRP";
            DtRates.Rows[6][1] = "MRP";

            DtRates.Rows.Add();
            DtRates.Rows[7][0] = "Rebate";
            DtRates.Rows[7][1] = "Rebate";

            textBox3.Text = SelectCombo.ComboDt(this, DtRates, 0);
            SendKeys.Send("{tab}");
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            strCombo = "select [name] from other where Type='SER15' order by [name]";
            textBox2.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" && textBox2.Text == "")
            {
                MessageBox.Show("Enter Atleast One within Company/Manufacturer");
                textBox1.Focus();
                return;
            }
           
            if (textBox3.Text == "")
            {
                MessageBox.Show("Enter Rates to be Updated");
                textBox3.Focus();
                return;
            }
            Loaddata();
        }

        private void ansGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
