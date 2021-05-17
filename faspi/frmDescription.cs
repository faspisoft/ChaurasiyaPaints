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
    public partial class frmDescription : Form
    {
        DataTable dtDesc;
        String dtName;
        public bool calledIndirect = false;
        public String DescriptionName;
        public int DescriptionId;
        List<UsersFeature> permission;

        String strCombo;
        String gStr;

        public frmDescription()
        {
            InitializeComponent();
        }

        private void frmDescription_Load(object sender, EventArgs e)
        {

            if (Feature.Available("High Striction On Account").ToUpper() == "YES")
            {
                label7.Visible = true;
                label8.Visible = true;
                label9.Visible = true;
            }
            else
            {
                label7.Visible = false;
                label8.Visible = false;
                label9.Visible = false;
            }
            if (Feature.Available("Company Colour") == "No")
            {
                groupBox1.Visible = false;
                groupBox2.Visible = false;
                groupBox4.Visible = false;
            }
            if (Feature.Available("Required PriceGroup") == "No")
            {
                groupBox8.Visible = false;
            }
            if (Feature.Available("Required Department").ToUpper() == "NO")
            {
                groupBox7.Visible = false;
            }
            if (Feature.Available("Multi-Godown").ToUpper() == "NO")
            {
                groupBox10.Visible = false;
            }
            else
            {
                groupBox10.Visible = false;
            }

            if (Feature.Available("Taxation Applicable") == "VAT")
            {
                groupBox5.Text = "Tax Category";
            }
            else
            {
                groupBox5.Text = "HSN Name";
            }
            SideFill();
        }

        public void LoadData(String str, String frmCaption)
        {
            gStr = str;
            dtDesc = new DataTable("Description");

            Database.GetSqlData("SELECT Pack, Rate_Unit, Pvalue, Purchase_rate,  Wholesale, Retail,Rate_X, Rate_Y, Rate_Z, MRP, Skucode, ShortCode, [Commission%], Commission@, Rebate, Srebate, Container, Wlavel, Max_level, box_quantity, Status, Square_FT, Square_MT, Des_id, Description, Company_id, Item_id, Col_id, Group_id, Tax_Cat_id, Open_stock, Mark, weight, discount_qty, Open_stock2, remarkreq, StkMaintain, Department_id, Godown_id, LocationId, Nid, Change_des, User_id, Modifiedby,PackCat_id FROM Description WHERE Description = '" + str + "' order by pvalue  desc", dtDesc);

            //Database.GetSqlData("select * from Description where Description='" + str + "' ", dtDesc);
            this.Text = frmCaption;

            if (dtDesc.Rows.Count == 0)
            {
                textBox3.Text = "";
                checkBox2.Checked = true;
                label4.Visible = false;
            }

            else
            {
                textBox3.Text = dtDesc.Rows[0]["Description"].ToString();

                if (bool.Parse(dtDesc.Rows[0]["Change_des"].ToString()) == true)
                {
                    checkBox3.Checked = true;
                }
                else
                {
                    checkBox3.Checked = false;
                }


                if (bool.Parse(dtDesc.Rows[0]["remarkreq"].ToString()) == true)
                {
                    checkBox1.Checked = true;
                }
                else
                {
                    checkBox1.Checked = false;
                }
                if (bool.Parse(dtDesc.Rows[0]["stkMaintain"].ToString()) == true)
                {
                    checkBox2.Checked = true;
                }
                else
                {
                    checkBox2.Checked = false;
                }
                if (dtDesc.Rows[0]["Department_id"].ToString() == "")
                {
                    dtDesc.Rows[0]["Department_id"] = 0;
                }
                if (dtDesc.Rows[0]["Group_id"].ToString() == "")
                {
                    dtDesc.Rows[0]["Group_id"] = 0;
                }
                if (dtDesc.Rows[0]["Company_id"].ToString() == "")
                {
                    dtDesc.Rows[0]["Company_id"] = 0;
                }
                if (dtDesc.Rows[0]["Godown_id"].ToString() == "")
                {
                    dtDesc.Rows[0]["Godown_id"] = 0;
                }
                textBox1.Text = funs.Select_oth_nm(dtDesc.Rows[0]["Department_id"].ToString());
                if (dtDesc.Rows[0]["Godown_id"].ToString() == "0")
                {
                    textBox4.Text = "<MAIN>";
                }
                else
                {
                    textBox4.Text = funs.Select_ac_nm(dtDesc.Rows[0]["Godown_id"].ToString());
                }
                textBox10.Text = funs.Select_oth_nm(dtDesc.Rows[0]["Company_id"].ToString());
                if (dtDesc.Rows[0]["Item_id"].ToString() == "")
                {
                    dtDesc.Rows[0]["Item_id"] = 0;
                }
                textBox11.Text = funs.Select_oth_nm(dtDesc.Rows[0]["Item_id"].ToString());
                if (dtDesc.Rows[0]["col_id"].ToString() == "")
                {
                    dtDesc.Rows[0]["col_id"] = 0;
                }
                textBox13.Text = funs.Select_oth_nm(dtDesc.Rows[0]["col_id"].ToString());
                textBox2.Text = funs.Select_oth_nm(dtDesc.Rows[0]["Group_id"].ToString());

                textBox14.Text = funs.Select_category_nm(dtDesc.Rows[0]["tax_cat_id"].ToString());
                if (textBox14.Text != "")
                {
                    label4.Visible = true;
                    string var1 = "HSN Code: ";
                    var1 += funs.Select_tax_cat_code(textBox14.Text);
                    var1 += "   Tax: ";

                    double highercgst = 0;
                    double higherigst = 0;
                    if (funs.Select_tax_cat_salecgst(textBox14.Text) >= funs.Select_tax_cat_purcgst(textBox14.Text))
                    {
                        highercgst = funs.Select_tax_cat_salecgst(textBox14.Text);
                    }
                    else if (funs.Select_tax_cat_salecgst(textBox14.Text) <= funs.Select_tax_cat_purcgst(textBox14.Text))
                    {
                        highercgst = funs.Select_tax_cat_purcgst(textBox14.Text);
                    }

                    if (funs.Select_tax_cat_saleigst(textBox14.Text) >= funs.Select_tax_cat_purigst(textBox14.Text))
                    {
                        higherigst = funs.Select_tax_cat_salecgst(textBox14.Text);
                    }
                    else if (funs.Select_tax_cat_saleigst(textBox14.Text) <= funs.Select_tax_cat_purigst(textBox14.Text))
                    {
                        higherigst = funs.Select_tax_cat_purcgst(textBox14.Text);
                    }
                    if (highercgst >= higherigst)
                    {
                        var1 += highercgst.ToString() + "%";
                    }
                    else
                    {
                        var1 += higherigst.ToString() + "%";
                    }

                    label4.Text = var1;
                }

                for (int i = 0; i < dtDesc.Rows.Count; i++)
                {
                    dtDesc.Rows[i]["Pack"] = dtDesc.Rows[i]["Pack"].ToString();
                }
            }

            dtDesc.Columns.Add("godown", typeof(string));
            dtDesc.Columns.Add("PackCat", typeof(string));
            dtDesc.Columns.Add("container_name", typeof(string));
            if (dtDesc.Rows.Count > 0)
            {
                for (int i = 0; i < dtDesc.Rows.Count; i++)
                {
                    dtDesc.Rows[i]["PackCat"] = funs.Select_packcat_name(dtDesc.Rows[i]["packcat_id"].ToString());
                    dtDesc.Rows[i]["godown"] = funs.Select_ac_nm(dtDesc.Rows[i]["Godown_id"].ToString());
                    if (dtDesc.Rows[i]["godown"].ToString() == "")
                    {
                        dtDesc.Rows[i]["godown"] = "<MAIN>";
                    }
                    dtDesc.Rows[i]["container_name"] = funs.Select_Container_name(dtDesc.Rows[i]["Container"].ToString());
                }
            }

            ansGridView5.DataSource = dtDesc;
            
            Display();

            foreach (DataGridViewColumn column in ansGridView5.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
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
            permission = funs.GetPermissionKey("StockItem");
            //create
            UsersFeature ob = permission.Where(w => w.FeatureName == "Create").FirstOrDefault();
            if (ob != null && gStr == "0" && ob.SelectedValue == "Allowed")
            {
                dtsidefill.Rows[0]["Visible"] = true;
            }
            else if (gStr == "0")
            {
                dtsidefill.Rows[0]["Visible"] = false;
            }

            //alter
            ob = permission.Where(w => w.FeatureName == "Alter").FirstOrDefault();
            if (ob != null && gStr != "0" && ob.SelectedValue == "Allowed")
            {
                dtsidefill.Rows[0]["Visible"] = true;
            }
            else if (gStr != "0")
            {
                dtsidefill.Rows[0]["Visible"] = false;
            }
            //if (gStr != "0")
            //{
            //    if (Database.utype.ToUpper() == "SUPERADMIN" || Database.utype.ToUpper() == "ADMIN")
            //    {
            //        dtsidefill.Rows[0]["Visible"] = true;
            //    }
            //    else
            //    {
            //        dtsidefill.Rows[0]["Visible"] = false;
            //    }
            //}
            //else
            //{
            //    dtsidefill.Rows[0]["Visible"] = true;
            //}

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
                    //if (Database.utype.ToUpper() == "SUPERADMIN" || Database.utype.ToUpper() == "ADMIN")
                    //{
                    //    save();
                    //}
                    //else if (gStr == "0")
                    //{
                    //    save();
                    //}
                    //create
                    UsersFeature ob = permission.Where(w => w.FeatureName == "Create").FirstOrDefault();
                    if (ob != null && gStr == "0" && ob.SelectedValue == "Allowed")
                    {
                        save();
                    }

                    //alter
                    ob = permission.Where(w => w.FeatureName == "Alter").FirstOrDefault();
                    if (ob != null && gStr != "0" && ob.SelectedValue == "Allowed")
                    {
                        save();
                    }
                }
            }
           
            else if (name == "quit")
            {
                this.Close();
                this.Dispose();
            }
        }

        private void Display()
        {
            if (Feature.Available("Required Remark1") == "No" && Feature.Available("Required Remark2") == "No" && Feature.Available("Required Remark3") == "No" && Feature.Available("Required Remark4") == "No")
            {
                groupBox3.Visible = false;
            }

            ansGridView5.Columns["Modifiedby"].Visible = false;
            ansGridView5.Columns["User_id"].Visible = false;
            ansGridView5.Columns["packcat_id"].Visible = false;
            ansGridView5.Columns["remarkreq"].Visible = false;
            ansGridView5.Columns["Godown_id"].Visible = false;
            ansGridView5.Columns["LocationId"].Visible = false;
            ansGridView5.Columns["Department_id"].Visible = false;
            ansGridView5.Columns["StkMaintain"].Visible = false;
            ansGridView5.Columns["Pack"].ReadOnly = true;
            ansGridView5.Columns["Rate_Unit"].ReadOnly = true;
            ansGridView5.Columns["des_id"].Visible = false;
            ansGridView5.Columns["Pack"].Visible = true;
            ansGridView5.Columns["Company_id"].Visible = false;
            ansGridView5.Columns["Item_id"].Visible = false;
            ansGridView5.Columns["Col_id"].Visible = false;
            ansGridView5.Columns["Group_id"].Visible = false;
            ansGridView5.Columns["Tax_Cat_id"].Visible = false;
            ansGridView5.Columns["Mark"].Visible = false;
            ansGridView5.Columns["Description"].Visible = false;
            ansGridView5.Columns["discount_qty"].Visible = false;
            ansGridView5.Columns["Open_stock"].Visible = false;
            ansGridView5.Columns["Nid"].Visible = false;
            ansGridView5.Columns["Container"].Visible = false;
            ansGridView5.Columns["Change_des"].Visible = false;

            if (Feature.Available("Square Feet/Square Meter").ToUpper() == "YES")
            {
                ansGridView5.Columns["Square_FT"].Visible = true;
                ansGridView5.Columns["Square_MT"].Visible = true;
            }
            else
            {
                ansGridView5.Columns["Square_FT"].Visible = false;
                ansGridView5.Columns["Square_MT"].Visible = false;
            }

            ansGridView5.Columns["Status"].ReadOnly = true;

            if (Database.SoftwareName == "Faspi Iron Pro.")
            {
                ansGridView5.Columns["wholesale"].Visible = false;
                ansGridView5.Columns["Rate_X"].Visible = false;
                ansGridView5.Columns["Rate_Y"].Visible = false;
                ansGridView5.Columns["Rate_Z"].Visible = false;
                ansGridView5.Columns["Retail"].HeaderText = "Rates";
                ansGridView5.Columns["Retail"].Width = 100;
            }

        
            ansGridView5.Columns["packcat"].HeaderText = "Pack Cat";
            ansGridView5.Columns["Wlavel"].HeaderText = "Min. Level";
            ansGridView5.Columns["Max_level"].HeaderText = "Max Level";
            ansGridView5.Columns["Purchase_rate"].HeaderText = Feature.Available("Name of PriceList1");
            ansGridView5.Columns["Retail"].HeaderText = Feature.Available("Name of PriceList2");
            ansGridView5.Columns["Wholesale"].HeaderText = Feature.Available("Name of PriceList3");
            ansGridView5.Columns["Rate_X"].HeaderText = Feature.Available("Name of PriceList4");
            ansGridView5.Columns["Rate_Y"].HeaderText = Feature.Available("Name of PriceList5");
            ansGridView5.Columns["Rate_Z"].HeaderText = Feature.Available("Name of PriceList6");
            ansGridView5.Columns["Rate_Unit"].HeaderText = "Unit";
            ansGridView5.Columns["Pvalue"].HeaderText = "Pack Value";           
            ansGridView5.Columns["Wlavel"].HeaderText = "Warning Level";
            ansGridView5.Columns["box_quantity"].HeaderText = "BoxQuantity";
            ansGridView5.Columns["Pack"].DefaultCellStyle.BackColor = Color.LightGray;
            ansGridView5.Columns["Square_FT"].HeaderText = "Sq. Feet";
            ansGridView5.Columns["Square_MT"].HeaderText = "Sq. Meter";
            ansGridView5.Columns["Rebate"].HeaderText = "Pur. Rebate";
            ansGridView5.Columns["Srebate"].HeaderText = "Sale Rebate";
            ansGridView5.Columns["Pack"].DisplayIndex = 0;
            ansGridView5.Columns["PackCat"].DisplayIndex = 1;
            ansGridView5.Columns["Rate_Unit"].DisplayIndex = 2;
            ansGridView5.Columns["Pvalue"].DisplayIndex = 3;
            ansGridView5.Columns["Purchase_rate"].DisplayIndex = 4;
            ansGridView5.Columns["Wholesale"].DisplayIndex = 5;
            ansGridView5.Columns["Retail"].DisplayIndex = 6;

            ansGridView5.Columns["Rate_X"].DisplayIndex = 7;
            ansGridView5.Columns["Rate_Y"].DisplayIndex = 8;
            ansGridView5.Columns["Rate_Z"].DisplayIndex = 9;
            ansGridView5.Columns["MRP"].DisplayIndex = 10;
            ansGridView5.Columns["ShortCode"].DisplayIndex = 11;
            ansGridView5.Columns["Skucode"].DisplayIndex = 12;
            ansGridView5.Columns["Commission%"].DisplayIndex = 13;
            ansGridView5.Columns["Commission@"].DisplayIndex = 14;
            ansGridView5.Columns["Wlavel"].DisplayIndex = 15;
            ansGridView5.Columns["Max_level"].DisplayIndex = 16;
            ansGridView5.Columns["box_quantity"].DisplayIndex = 17;
            ansGridView5.Columns["Rebate"].DisplayIndex = 18;
            ansGridView5.Columns["Srebate"].DisplayIndex = 19;
            ansGridView5.Columns["Square_FT"].DisplayIndex = 20;
            ansGridView5.Columns["Square_MT"].DisplayIndex = 21;
            ansGridView5.Columns["Status"].DisplayIndex = 22;
            if (Database.IsKacha == false)
            {
                ansGridView5.Columns["Open_stock2"].Visible = false;
            }
            else
            {
                ansGridView5.Columns["Open_stock2"].HeaderText = "Opening Stock2";
                ansGridView5.Columns["Open_stock2"].DisplayIndex = 14;
                ansGridView5.Columns["Open_stock2"].Visible = false;
            }

            ansGridView5.Columns["godown"].ReadOnly = true;
          //  ansGridView5.Columns["godown"].Visible = false;
            ansGridView5.Columns["godown"].Visible = true;
            ansGridView5.Columns["packcat"].ReadOnly = true;
            ansGridView5.Columns["container_name"].ReadOnly = true;
        }

        public void clear()
        {
            textBox3.Text = "";
            ansGridView5.Rows.Clear();
        }

        private void save()
        {
            dtDesc.Columns.Remove("godown");
            dtDesc.Columns.Remove("container_name");
            dtDesc.Columns.Remove("packcat");
            DescriptionName = textBox3.Text;
            if (gStr == "0" && ansGridView5.Rows.Count == 0)
            {
                MessageBox.Show("Please Add Atleast One Packing", "Packing", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            DataTable dtid = new DataTable();
            Database.GetSqlData("select max(Nid) as Nid from DESCRIPTION where locationid='" + Database.LocationId + "'", dtid);
            int count = 0;
            int nid = 1;
            int Nid;

            if (dtid.Rows.Count == 0)
            {
                Nid = 1;
            }
            else
            {
                if (dtid.Rows[0][0].ToString() == "")
                {
                    Nid = 1;
                }
                else
                {
                    Nid = int.Parse(dtid.Rows[0][0].ToString());
                }
            }
            string s = "";
            for (int i = 0; i < dtDesc.Rows.Count; i++)
            {  
                if (dtDesc.Rows[i].RowState.ToString() != "Deleted")
                {
                    if (dtDesc.Rows[i]["Des_id"].ToString() == "0")
                    {
                        DataTable dtCount = new DataTable();
                        Database.GetSqlData("select count(*) from DESCRIPTION where locationid='" + Database.LocationId + "'", dtCount);
                        count = Database.GetScalarInt("select count(*) from DESCRIPTION where locationid='" + Database.LocationId + "'");


                        if (count == 0)
                        {
                            dtDesc.Rows[i]["Des_id"] = Database.LocationId + nid.ToString();
                            dtDesc.Rows[i]["Nid"] = nid;
                            dtDesc.Rows[i]["LocationId"] = Database.LocationId;
                            nid++;
                            dtDesc.Rows[i]["user_id"] = Database.user_id;


                            if (dtDesc.Select("Modifiedby<>''").Length == 0)
                            {
                                dtDesc.Rows[i]["Modifiedby"] = "";
                            }
                            else
                            {
                                dtDesc.Rows[i]["Modifiedby"] = Database.user_id;
                            }
                        }
                        else
                        {
                            dtDesc.Rows[i]["Des_id"] = Database.LocationId + (Nid + 1);
                            dtDesc.Rows[i]["Nid"] = (Nid + 1);
                            dtDesc.Rows[i]["LocationId"] = Database.LocationId;
                            Nid = Nid + 1;
                            dtDesc.Rows[i]["user_id"] = Database.user_id;
                            if (dtDesc.Select("Modifiedby<>''").Length == 0)
                            {
                                dtDesc.Rows[i]["Modifiedby"] = "";
                            }
                            else
                            {
                                dtDesc.Rows[i]["Modifiedby"] = Database.user_id;
                            }
                        }
                    }
                    else
                    {


                       

                        dtDesc.Rows[i]["Modifiedby"] = Database.user_id;
                    }
                }
            }

            for (int i = 0; i < dtDesc.Rows.Count; i++)
            {
                if (dtDesc.Rows[i].RowState.ToString() != "Deleted")
                {

                    dtDesc.Rows[i]["Department_id"] = funs.Select_oth_id(textBox1.Text);
                    //dtDesc.Rows[i]["Godown_id"] = funs.Select_ac_id(textBox4.Text);
                    dtDesc.Rows[i]["Company_id"] = funs.Select_oth_id(textBox10.Text);
                    dtDesc.Rows[i]["Item_id"] = funs.Select_oth_id(textBox11.Text);
                    dtDesc.Rows[i]["Col_id"] = funs.Select_oth_id(textBox13.Text);
                    dtDesc.Rows[i]["Tax_Cat_id"] = funs.Select_tax_cat_id(textBox14.Text);
                    dtDesc.Rows[i]["Group_id"] = funs.Select_oth_id(textBox2.Text);
                    dtDesc.Rows[i]["Description"] = textBox3.Text;
                    dtDesc.Rows[i]["Mark"] = "No";

                    if (checkBox1.Checked == true)
                    {
                        dtDesc.Rows[i]["remarkreq"] = true;
                    }
                    else
                    {
                        dtDesc.Rows[i]["remarkreq"] = false;
                    }

                    if (checkBox2.Checked == true)
                    {
                        dtDesc.Rows[i]["StkMaintain"] = true;
                    }
                    else
                    {
                        dtDesc.Rows[i]["StkMaintain"] = false;
                    }

                    if (checkBox3.Checked == true)
                    {
                        dtDesc.Rows[i]["Change_des"] = true;
                    }
                    else
                    {
                        dtDesc.Rows[i]["Change_des"] = false;
                    }
                }
            }

            Database.SaveData(dtDesc);
            Master.UpdateDecription();
            Master.UpdateOther();
            Master.UpdateDecriptionInfo();
            funs.ShowBalloonTip("Saved", "Saved Successfully");

            if (calledIndirect == true)
            {
                this.Close();
                this.Dispose();
            }
            else if (gStr == "0")
            {
                LoadData("0", this.Text);
            }
            else
            {
                this.Close();
                this.Dispose();
            }  
        }

        private bool validate()
        {
            if (textBox3.Text == "")
            {
                textBox3.BackColor = Color.Aqua;
                textBox3.Focus();
                return false;
            }
           
            else if (textBox14.Text == "")
            {
                textBox14.BackColor = Color.Aqua;
                textBox14.Focus();
                return false;
            }
          
            for (int i = 0; i < ansGridView5.Rows.Count; i++)
            {
                if (ansGridView5.Rows[i].Cells["pack"].Value == null || ansGridView5.Rows[i].Cells["pack"].Value.ToString() == "")
                {
                    ansGridView5.Rows[i].Cells["pack"].Style.BackColor = Color.Red;
                    return false;
                }
                else
                {
                    ansGridView5.Rows[i].Cells["pack"].Style.BackColor = Color.LightGray;
                }

                if (ansGridView5.Rows[i].Cells["rate_unit"].Value == null || ansGridView5.Rows[i].Cells["rate_unit"].Value.ToString() == "")
                {
                    ansGridView5.Rows[i].Cells["rate_unit"].Value = "UNT-UNITS";
                }
                if (ansGridView5.Rows[i].Cells["Pvalue"].Value == null || ansGridView5.Rows[i].Cells["Pvalue"].Value.ToString() == "")
                {

                    ansGridView5.Rows[i].Cells["Pvalue"].Style.BackColor = Color.Red;
                    return false;

                }
                else
                {
                    ansGridView5.Rows[i].Cells["Pvalue"].Style.BackColor = Color.White;
                }
                if (Feature.Available("High Striction On Account").ToUpper() == "YES")
                {
                    //if (textBox4.Text == "")
                    //{
                    //    textBox4.BackColor = Color.Aqua;
                    //    textBox4.Focus();
                    //    return false;
                    //}
                    if (textBox10.Text == "")
                    {
                        textBox10.BackColor = Color.Aqua;
                        MessageBox.Show("Enter Company");
                        textBox10.Focus();
                        return false;
                    }

                    else if (textBox11.Text == "")
                    {
                        textBox11.BackColor = Color.Aqua;
                        MessageBox.Show("Enter Item/Brand");
                        textBox11.Focus();
                        return false;
                    }

                  
                    if (ansGridView5.Rows[i].Cells["packcat"].Value == null || ansGridView5.Rows[i].Cells["packcat"].Value.ToString() == "")
                    {

                        ansGridView5.Rows[i].Cells["packcat"].Style.BackColor = Color.Red;
                        return false;

                    }
                    else
                    {
                        ansGridView5.Rows[i].Cells["packcat"].Style.BackColor = Color.White;
                    }
                    if (ansGridView5.Rows[i].Cells["container_name"].Value == null || ansGridView5.Rows[i].Cells["container_name"].Value.ToString() == "")
                    {

                        ansGridView5.Rows[i].Cells["container_name"].Style.BackColor = Color.Red;
                        return false;

                    }
                    else
                    {
                        ansGridView5.Rows[i].Cells["container_name"].Style.BackColor = Color.White;
                    }
                    if (ansGridView5.Rows[i].Cells["box_quantity"].Value == null || ansGridView5.Rows[i].Cells["box_quantity"].Value.ToString() == "")
                    {

                        ansGridView5.Rows[i].Cells["box_quantity"].Style.BackColor = Color.Red;
                        return false;

                    }
                    else
                    {
                        if (ansGridView5.Rows[i].Cells["box_quantity"].Value.ToString() == "0")
                        {

                            ansGridView5.Rows[i].Cells["box_quantity"].Style.BackColor = Color.Red;
                            return false;
                        }
                        else
                        {
                            ansGridView5.Rows[i].Cells["box_quantity"].Style.BackColor = Color.White;
                        }

                    }
                }
              //   ansGridView5.Columns[""].
                if (ansGridView5.Rows[i].Cells["Square_FT"].Value == null || ansGridView5.Rows[i].Cells["Square_FT"].Value.ToString() == "")
                {
                    ansGridView5.Rows[i].Cells["Square_FT"].Value = 1;
                }

                if (ansGridView5.Rows[i].Cells["Square_MT"].Value == null || ansGridView5.Rows[i].Cells["Square_MT"].Value.ToString() == "")
                {
                    ansGridView5.Rows[i].Cells["Square_MT"].Value = 1;
                }

                if (ansGridView5.Rows[i].Cells["status"].Value == null || ansGridView5.Rows[i].Cells["status"].Value.ToString() == "")
                {
                    ansGridView5.Rows[i].Cells["status"].Value = "Enabled";
                }

                if (ansGridView5.Rows[i].Cells["weight"].Value == null || ansGridView5.Rows[i].Cells["weight"].Value.ToString() == "")
                {
                    ansGridView5.Rows[i].Cells["weight"].Value = 1;
                }

                if (ansGridView5.Rows[i].Cells["Rebate"].Value == null || ansGridView5.Rows[i].Cells["Rebate"].Value.ToString() == "")
                {
                    ansGridView5.Rows[i].Cells["Rebate"].Value = 0;
                }

                if (ansGridView5.Rows[i].Cells["Srebate"].Value == null || ansGridView5.Rows[i].Cells["Srebate"].Value.ToString() == "")
                {
                    ansGridView5.Rows[i].Cells["Srebate"].Value = 0;
                }

                if (Database.GetScalarInt("SELECT Count(DESCRIPTION.Des_id) AS Con FROM DESCRIPTION WHERE (((DESCRIPTION.Description)='" + textBox3.Text + "' ) AND ((DESCRIPTION.Pack)='" + ansGridView5.Rows[i].Cells["pack"].Value.ToString() + "') AND ((DESCRIPTION.Des_id)<> '" + ansGridView5.Rows[i].Cells["des_id"].Value + "'))") != 0)
                {
                    MessageBox.Show("Display Name Already Exists With Same Packing.");
                    return false;
                }
            }
            return true;
        }

        private void frmDescription_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
            {
                if (validate() == true)
                {
                    //create
                    UsersFeature ob = permission.Where(w => w.FeatureName == "Create").FirstOrDefault();
                    if (ob != null && gStr == "0" && ob.SelectedValue == "Allowed")
                    {
                        save();
                    }

                    //alter
                    ob = permission.Where(w => w.FeatureName == "Alter").FirstOrDefault();
                    if (ob != null && gStr != "0" && ob.SelectedValue == "Allowed")
                    {
                        save();
                    }
                    //if (Database.utype.ToUpper() == "SUPERADMIN" || Database.utype.ToUpper() == "ADMIN")
                    //{
                    //    save();
                    //}
                    //else if (gStr == "0")
                    //{
                    //    save();
                    //}
                }
            }

            else if (e.KeyCode == Keys.Escape)
            {
                if (textBox3.Text != "")
                {
                    DialogResult chk = MessageBox.Show("Are u sure?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                    if (chk == DialogResult.No)
                    {
                        e.Handled = false;
                    }
                    else
                    {
                        this.Close();
                        this.Dispose();
                    }
                }
                else
                {
                    this.Close();
                    this.Dispose();
                }
            }
        }

        private void textBox10_KeyPress(object sender, KeyPressEventArgs e)
        {
            strCombo = "select [name] from other where Type='" + funs.Get_Company_id() + "' order by [name]";
            textBox10.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
        }

        private void textBox11_KeyPress(object sender, KeyPressEventArgs e)
        {
            strCombo = "select [name] from other where Type='" + funs.Get_Item_id() + "' order by [name]";
            textBox11.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
        }

        private void textBox13_KeyPress(object sender, KeyPressEventArgs e)
        {
            strCombo = "select [name] from other where Type='" + funs.Get_Colour_id() + "' order by [name]";
            textBox13.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
        }

        private void textBox14_KeyPress(object sender, KeyPressEventArgs e)
        {
            strCombo = "select category_name from taxcategory";
            textBox14.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
            if (textBox14.Text != "")
            {
                label4.Visible = true;
                string var1 = "HSN Code: ";
                var1 += funs.Select_tax_cat_code(textBox14.Text);
                var1 += "   Tax: ";

                double highercgst = 0;
                double higherigst = 0;
                if (funs.Select_tax_cat_salecgst(textBox14.Text) >= funs.Select_tax_cat_purcgst(textBox14.Text))
                {
                    highercgst = funs.Select_tax_cat_salecgst(textBox14.Text);
                }
                else if (funs.Select_tax_cat_salecgst(textBox14.Text) <= funs.Select_tax_cat_purcgst(textBox14.Text))
                {
                    highercgst = funs.Select_tax_cat_purcgst(textBox14.Text);
                }

                if (funs.Select_tax_cat_saleigst(textBox14.Text) >= funs.Select_tax_cat_purigst(textBox14.Text))
                {
                    higherigst = funs.Select_tax_cat_salecgst(textBox14.Text);
                }
                else if (funs.Select_tax_cat_saleigst(textBox14.Text) <= funs.Select_tax_cat_purigst(textBox14.Text))
                {
                    higherigst = funs.Select_tax_cat_purcgst(textBox14.Text);
                }
                if (highercgst >= higherigst)
                {
                    var1 += highercgst.ToString() + "%";
                }
                else
                {
                    var1 += higherigst.ToString() + "%";
                }

                label4.Text = var1;
            }
        }

        private void textBox11_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.C)
            {
                textBox11.Text = funs.AddProduct("Item");
            }
            else if (e.Control && e.KeyCode == Keys.A)
            {
                if (textBox11.Text != "")
                {
                    textBox11.Text = funs.EditItem(textBox11.Text);
                }
            }
        }

        private void textBox13_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.C)
            {
                textBox13.Text = funs.AddProduct("Colour");
            }
            else if (e.Control && e.KeyCode == Keys.A)
            {
                if (textBox13.Text != "")
                {
                    textBox13.Text = funs.EditColor(textBox13.Text);
                }
            }
        }

        private void textBox10_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
            {
                if (textBox10.Text != "")
                {
                    textBox10.Text = funs.EditCompany(textBox10.Text);
                }
            }
            else if (e.Control && e.KeyCode == Keys.C)
            {
                textBox10.Text = funs.AddProduct("Company");
            }
        }

        private void textBox14_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.C)
            {
                textBox14.Text = funs.AddTax();
            }
            else if (e.Control && e.KeyCode == Keys.A)
            {
                if (textBox14.Text != "")
                {
                    textBox14.Text = funs.EditTax(textBox14.Text);
                }
            }
        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void ansGridView5_KeyDown(object sender, KeyEventArgs e)
        {
            if (ansGridView5.CurrentCell == null)
            {
                return;
            }
            //if (ansGridView5.CurrentCell.OwningColumn.Name == "godown" && e.KeyCode != Keys.Delete)
            //{
            //    if (Feature.Available("Multi-Godown") == "Yes")
            //    {
            //        DataTable dt = new DataTable();
            //        Database.GetSqlData("select distinct '<MAIN>' as name from account union all SELECT ACCOUNT.Name as name FROM ACCOUNT LEFT JOIN ACCOUNTYPE ON ACCOUNT.Act_id = ACCOUNTYPE.Act_id WHERE ACCOUNTYPE.Name='Godown' and Account.Branch_id='" + Database.BranchId + "' GROUP BY ACCOUNT.Name", dt);
            //        ansGridView5.CurrentCell.Value = SelectCombo.ComboDt(this, dt, 0);
            //        if (ansGridView5.CurrentCell.Value != null)
            //        {                        
            //            ansGridView5.Rows[ansGridView5.CurrentCell.RowIndex].Cells["Godown_id"].Value = funs.Select_ac_id(ansGridView5.CurrentCell.Value.ToString());
            //        }
            //    }
            //    else
            //    {
            //        ansGridView5.Rows[ansGridView5.CurrentCell.RowIndex].Cells["Godown_id"].Value = "0";
            //    }
            //}
            //else if (ansGridView5.CurrentCell.OwningColumn.Name == "container_name" && e.KeyCode != Keys.Delete)
            //{
            //    DataTable dt = new DataTable();
            //    Database.GetSqlData("select cname as Container_Name from Container order by id", dt);
            //    ansGridView5.CurrentCell.Value = SelectCombo.ComboDt(this, dt, 0);
            //    if (ansGridView5.CurrentCell.Value != null)
            //    {
            //        ansGridView5.Rows[ansGridView5.CurrentCell.RowIndex].Cells["Container"].Value = funs.Select_Container_id(ansGridView5.CurrentCell.Value.ToString());
            //    }
            //    else
            //    {
            //        ansGridView5.Rows[ansGridView5.CurrentCell.RowIndex].Cells["Container"].Value = "0";
            //    }
            //}
            //else if (ansGridView5.CurrentCell.OwningColumn.Name == "Status" && e.KeyCode != Keys.Delete)
            //{
            //    DataTable dtcombo = new DataTable();
            //    dtcombo.Columns.Add("Status", typeof(string));

            //    dtcombo.Rows.Add();
            //    dtcombo.Rows[0][0] = "Enable";

            //    dtcombo.Rows.Add();
            //    dtcombo.Rows[1][0] = "Disable";

            //    ansGridView5.CurrentCell.Value = SelectCombo.ComboDt(this, dtcombo, 0);
                
            //}
            //else if (ansGridView5.CurrentCell.OwningColumn.Name == "Rate_Unit" && e.KeyCode != Keys.Delete)
            //{
            //    DataTable dtcombo = new DataTable();
            //    dtcombo.Columns.Add("Unit", typeof(string));

            //    dtcombo.Columns["Unit"].ColumnName = "UOC";

            //    dtcombo.Rows.Add();
            //    dtcombo.Rows[0][0] = "BAG-BAGS";

            //    dtcombo.Rows.Add();
            //    dtcombo.Rows[1][0] = "BAL-BALE";

            //    dtcombo.Rows.Add();
            //    dtcombo.Rows[2][0] = "BDL-BUNDLES";

            //    dtcombo.Rows.Add();
            //    dtcombo.Rows[3][0] = "BKL-BUCKLES";

            //    dtcombo.Rows.Add();
            //    dtcombo.Rows[4][0] = "BOU-BILLION OF UNITS";

            //    dtcombo.Rows.Add();
            //    dtcombo.Rows[5][0] = "BOX-BOX";

            //    dtcombo.Rows.Add();
            //    dtcombo.Rows[6][0] = "BTL-BOTTLES";

            //    dtcombo.Rows.Add();
            //    dtcombo.Rows[7][0] = "BUN-BUNCHES";

            //    dtcombo.Rows.Add();
            //    dtcombo.Rows[8][0] = "CAN-CANS";

            //    dtcombo.Rows.Add();
            //    dtcombo.Rows[9][0] = "CBM-CUBIC METERS";

            //    dtcombo.Rows.Add();
            //    dtcombo.Rows[10][0] = "CCM-CUBIC CENTIMETERS";

            //    dtcombo.Rows.Add();
            //    dtcombo.Rows[11][0] = "CMS-CENTIMETERS";

            //    dtcombo.Rows.Add();
            //    dtcombo.Rows[12][0] = "CTN-CARTONS";

            //    dtcombo.Rows.Add();
            //    dtcombo.Rows[13][0] = "DOZ-DOZENS";

            //    dtcombo.Rows.Add();
            //    dtcombo.Rows[14][0] = "DRM-DRUMS";

            //    dtcombo.Rows.Add();
            //    dtcombo.Rows[15][0] = "GGK-GREAT GROSS";

            //    dtcombo.Rows.Add();
            //    dtcombo.Rows[16][0] = "GMS-GRAMMES";

            //    dtcombo.Rows.Add();
            //    dtcombo.Rows[17][0] = "GRS-GROSS";

            //    dtcombo.Rows.Add();
            //    dtcombo.Rows[18][0] = "GYD-GROSS YARDS";

            //    dtcombo.Rows.Add();
            //    dtcombo.Rows[19][0] = "KGS-KILOGRAMS";

            //    dtcombo.Rows.Add();
            //    dtcombo.Rows[20][0] = "KLR-KILOLITRE";

            //    dtcombo.Rows.Add();
            //    dtcombo.Rows[21][0] = "KME-KILOMETRE";

            //    dtcombo.Rows.Add();
            //    dtcombo.Rows[22][0] = "MLT-MILILITRE";

            //    dtcombo.Rows.Add();
            //    dtcombo.Rows[23][0] = "MTR-METERS";

            //    dtcombo.Rows.Add();
            //    dtcombo.Rows[24][0] = "MTS-METERIC TON";

            //    dtcombo.Rows.Add();
            //    dtcombo.Rows[25][0] = "NOS-NUMBERS";

            //    dtcombo.Rows.Add();
            //    dtcombo.Rows[26][0] = "PAC-PACKS";

            //    dtcombo.Rows.Add();
            //    dtcombo.Rows[27][0] = "PCS-PIECES";

            //    dtcombo.Rows.Add();
            //    dtcombo.Rows[28][0] = "PRS-PAIRS";

            //    dtcombo.Rows.Add();
            //    dtcombo.Rows[29][0] = "QTL-QUINTAL";

            //    dtcombo.Rows.Add();
            //    dtcombo.Rows[30][0] = "ROL-ROLLS";

            //    dtcombo.Rows.Add();
            //    dtcombo.Rows[31][0] = "SET-SETS";

            //    dtcombo.Rows.Add();
            //    dtcombo.Rows[32][0] = "SQF-SQUARE FEET";

            //    dtcombo.Rows.Add();
            //    dtcombo.Rows[33][0] = "SQM-SQUARE METERS";

            //    dtcombo.Rows.Add();
            //    dtcombo.Rows[34][0] = "SQY-SQUARE YARDS";

            //    dtcombo.Rows.Add();
            //    dtcombo.Rows[35][0] = "TBS-TABLETS";

            //    dtcombo.Rows.Add();
            //    dtcombo.Rows[36][0] = "TGM-TEN GROSS";

            //    dtcombo.Rows.Add();
            //    dtcombo.Rows[37][0] = "THD-THOUSANDS";

            //    dtcombo.Rows.Add();
            //    dtcombo.Rows[38][0] = "TON-TONNES";

            //    dtcombo.Rows.Add();
            //    dtcombo.Rows[39][0] = "TUB-TUBES";

            //    dtcombo.Rows.Add();
            //    dtcombo.Rows[40][0] = "UGS-US GALLONS";

            //    dtcombo.Rows.Add();
            //    dtcombo.Rows[41][0] = "UNT-UNITS";

            //    dtcombo.Rows.Add();
            //    dtcombo.Rows[42][0] = "YDS-YARDS";

            //    dtcombo.Rows.Add();
            //    dtcombo.Rows[43][0] = "LTR-LITRES";

            //    dtcombo.Rows.Add();
            //    dtcombo.Rows[44][0] = "OTH-OTHERS";

            //    ansGridView5.CurrentCell.Value = SelectCombo.ComboDt(this, dtcombo, 0);

            //    if (ansGridView5.CurrentCell.Value != "")
            //    {
            //        ansGridView5.CurrentCell = ansGridView5["Pvalue", ansGridView5.CurrentCell.RowIndex];
            //    }
            //}

            else if (e.KeyCode == Keys.Delete)
            {
                if (ansGridView5.Rows[ansGridView5.SelectedCells[0].RowIndex].Cells["Des_id"].Value != null && ansGridView5.Rows[ansGridView5.SelectedCells[0].RowIndex].Cells["Des_id"].Value.ToString() != "0")
                {
                    if (Database.GetScalarInt("SELECT count(*) FROM Voucherdet WHERE Des_ac_id='" + ansGridView5.Rows[ansGridView5.SelectedCells[0].RowIndex].Cells["Des_id"].Value.ToString()+"'") != 0)
                    {
                        MessageBox.Show("Selected Packing is in Use");
                        return;
                    }
                    else
                    {
                        int rindex = ansGridView5.CurrentRow.Index;
                        dtDesc.Rows[rindex].Delete();

                    }
                }
            }
        }

        private void textBox3_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox3);
        }

        private void textBox3_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox3);
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\'')
            {
                e.Handled = true;
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            string selected = "";
            DialogResult chk = MessageBox.Show("Press Yes for List of Available Packing." + Environment.NewLine + "Press No to type Manually.", "Existing", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            if (chk == DialogResult.Yes)
            {
                string strCombo = "SELECT Distinct Pack FROM Description order by Pack";
                char cg = 'a';
                selected = SelectCombo.ComboKeypress(this, cg, strCombo, "", 0);
            }
            else if(chk == DialogResult.No)
            {
                InputBox box = new InputBox("Enter Packing", "", false);
                box.ShowInTaskbar = false;
                box.ShowDialog(this);
                if (box.outStr != null)
                {
                    selected = box.outStr;
                }
            }

            if (selected != "")
            {
                for (int i = 0; i < dtDesc.Rows.Count; i++)
                {
                    if (dtDesc.Rows[i]["Pack"].ToString() == "")
                    {
                        dtDesc.Rows[i]["Pack"] = 0;
                    }
                }

                dtDesc.Rows.Add();
                dtDesc.Rows[dtDesc.Rows.Count - 1]["Des_id"] = 0;
                dtDesc.Rows[dtDesc.Rows.Count - 1]["Pack"] = selected;
                dtDesc.Rows[dtDesc.Rows.Count - 1]["Rate_Unit"] = "UNT-UNITS";
                dtDesc.Rows[dtDesc.Rows.Count - 1]["Pvalue"] = 1;
                dtDesc.Rows[dtDesc.Rows.Count - 1]["Purchase_rate"] = 0;
                dtDesc.Rows[dtDesc.Rows.Count - 1]["Wholesale"] = 0;
                dtDesc.Rows[dtDesc.Rows.Count - 1]["Retail"] = 0;
                dtDesc.Rows[dtDesc.Rows.Count - 1]["Rate_X"] = 0;
                dtDesc.Rows[dtDesc.Rows.Count - 1]["Rate_Y"] = 0;
                dtDesc.Rows[dtDesc.Rows.Count - 1]["Rate_Z"] = 0;
                dtDesc.Rows[dtDesc.Rows.Count - 1]["MRP"] = 0;
                dtDesc.Rows[dtDesc.Rows.Count - 1]["Wlavel"] = 0;
                dtDesc.Rows[dtDesc.Rows.Count - 1]["box_quantity"] = 1;
                dtDesc.Rows[dtDesc.Rows.Count - 1]["Open_stock"] = 0;
                dtDesc.Rows[dtDesc.Rows.Count - 1]["Open_stock2"] = 0;
                dtDesc.Rows[dtDesc.Rows.Count - 1]["Commission%"] = 0;
                dtDesc.Rows[dtDesc.Rows.Count - 1]["Commission@"] = 0;
                dtDesc.Rows[dtDesc.Rows.Count - 1]["Status"] = "Enable";
                dtDesc.Rows[dtDesc.Rows.Count - 1]["Square_FT"] = 1;
                dtDesc.Rows[dtDesc.Rows.Count - 1]["Square_MT"] = 1;
                dtDesc.Rows[dtDesc.Rows.Count - 1]["Max_level"] = 0;
                dtDesc.Rows[dtDesc.Rows.Count - 1]["Rebate"] = 0;
                dtDesc.Rows[dtDesc.Rows.Count - 1]["Srebate"] = 0;
                dtDesc.Rows[dtDesc.Rows.Count - 1]["Weight"] = 1;

                ansGridView5.CurrentCell = ansGridView5["Pack", ansGridView5.CurrentCell.RowIndex];
            }
        }

        private void ansGridView5_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            ansGridView5.CurrentCell.Value = 0;
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            strCombo = "select [name] from other where Type='" + funs.Get_Department_id() + "' order by [name]";
            textBox1.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox1);
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox1);
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
            {
                if (textBox1.Text != "")
                {
                    textBox1.Text = funs.EditDepartment(textBox1.Text);
                }
            }
            else if (e.Control && e.KeyCode == Keys.C)
            {
                textBox1.Text = funs.AddProduct("Department");
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            strCombo = "select [name] from other where Type='" + funs.Get_Group_id() + "' order by [name]";
            textBox2.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox2);
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox2);
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
            {
                if (textBox2.Text != "")
                {
                    textBox2.Text = funs.EditDepartment(textBox1.Text);
                }
            }
            else if (e.Control && e.KeyCode == Keys.C)
            {
                textBox2.Text = funs.AddProduct("Group");
            }
        }

        private void textBox4_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox4);
        }

        private void textBox4_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox4);
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            strCombo = "select distinct  '<MAIN>' as name from account union all Select Name from Account where act_id='" + funs.Select_act_id("Godown") + "' order by Name";
            textBox4.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 1);
        }

        private void ansGridView5_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (ansGridView5.CurrentCell.OwningColumn.Name == "godown")
            {
                if (Feature.Available("Multi-Godown").ToUpper() == "YES")
                {
                    DataTable dt = new DataTable();
                    Database.GetSqlData("select distinct '<MAIN>' as name from account union all SELECT ACCOUNT.Name as name FROM ACCOUNT LEFT JOIN ACCOUNTYPE ON ACCOUNT.Act_id = ACCOUNTYPE.Act_id WHERE ACCOUNTYPE.Name='Godown' and Account.Branch_id='" + Database.BranchId + "' GROUP BY ACCOUNT.Name", dt);
                    ansGridView5.CurrentCell.Value = SelectCombo.ComboDt(this, dt, 0);
                    if (ansGridView5.CurrentCell.Value != null)
                    {
                        ansGridView5.Rows[ansGridView5.CurrentCell.RowIndex].Cells["Godown_id"].Value = funs.Select_ac_id(ansGridView5.CurrentCell.Value.ToString());
                    }
                }
                else
                {
                    ansGridView5.Rows[ansGridView5.CurrentCell.RowIndex].Cells["Godown_id"].Value = "0";
                }
            }
            else if (ansGridView5.CurrentCell.OwningColumn.Name == "container_name")
            {
                DataTable dt = new DataTable();
                Database.GetSqlData("select cname as Container_Name from Container order by id", dt);
                ansGridView5.CurrentCell.Value = SelectCombo.ComboDt(this, dt, 0);
                if (ansGridView5.CurrentCell.Value != null)
                {
                    ansGridView5.Rows[ansGridView5.CurrentCell.RowIndex].Cells["Container"].Value = funs.Select_Container_id(ansGridView5.CurrentCell.Value.ToString());
                }
                else
                {
                    ansGridView5.Rows[ansGridView5.CurrentCell.RowIndex].Cells["Container"].Value = "0";
                }
            }

            else if (ansGridView5.CurrentCell.OwningColumn.Name == "PackCat")
            {
                DataTable dt = new DataTable();
                Database.GetSqlData("select name as PackCategory from PackCategory order by packcat_id", dt);
                ansGridView5.CurrentCell.Value = SelectCombo.ComboDt(this, dt, 0);
                if (ansGridView5.CurrentCell.Value != null)
                {
                    ansGridView5.Rows[ansGridView5.CurrentCell.RowIndex].Cells["packcat_id"].Value = funs.Select_packcat_id(ansGridView5.CurrentCell.Value.ToString());
                }
               
            }
            else if (ansGridView5.CurrentCell.OwningColumn.Name == "Status")
            {
                DataTable dtcombo = new DataTable();
                dtcombo.Columns.Add("Status", typeof(string));

                dtcombo.Rows.Add();
                dtcombo.Rows[0][0] = "Enable";

                dtcombo.Rows.Add();
                dtcombo.Rows[1][0] = "Disable";

                ansGridView5.CurrentCell.Value = SelectCombo.ComboDt(this, dtcombo, 0);                
            }
            else if (ansGridView5.CurrentCell.OwningColumn.Name == "Rate_Unit")
            {
                DataTable dtcombo = new DataTable();
                dtcombo.Columns.Add("Unit", typeof(string));

                dtcombo.Columns["Unit"].ColumnName = "UOC";

                dtcombo.Rows.Add();
                dtcombo.Rows[0][0] = "BAG-BAGS";

                dtcombo.Rows.Add();
                dtcombo.Rows[1][0] = "BAL-BALE";

                dtcombo.Rows.Add();
                dtcombo.Rows[2][0] = "BDL-BUNDLES";

                dtcombo.Rows.Add();
                dtcombo.Rows[3][0] = "BKL-BUCKLES";

                dtcombo.Rows.Add();
                dtcombo.Rows[4][0] = "BOU-BILLION OF UNITS";

                dtcombo.Rows.Add();
                dtcombo.Rows[5][0] = "BOX-BOX";

                dtcombo.Rows.Add();
                dtcombo.Rows[6][0] = "BTL-BOTTLES";

                dtcombo.Rows.Add();
                dtcombo.Rows[7][0] = "BUN-BUNCHES";

                dtcombo.Rows.Add();
                dtcombo.Rows[8][0] = "CAN-CANS";

                dtcombo.Rows.Add();
                dtcombo.Rows[9][0] = "CBM-CUBIC METERS";

                dtcombo.Rows.Add();
                dtcombo.Rows[10][0] = "CCM-CUBIC CENTIMETERS";

                dtcombo.Rows.Add();
                dtcombo.Rows[11][0] = "CMS-CENTIMETERS";

                dtcombo.Rows.Add();
                dtcombo.Rows[12][0] = "CTN-CARTONS";

                dtcombo.Rows.Add();
                dtcombo.Rows[13][0] = "DOZ-DOZENS";

                dtcombo.Rows.Add();
                dtcombo.Rows[14][0] = "DRM-DRUMS";

                dtcombo.Rows.Add();
                dtcombo.Rows[15][0] = "GGK-GREAT GROSS";

                dtcombo.Rows.Add();
                dtcombo.Rows[16][0] = "GMS-GRAMMES";

                dtcombo.Rows.Add();
                dtcombo.Rows[17][0] = "GRS-GROSS";

                dtcombo.Rows.Add();
                dtcombo.Rows[18][0] = "GYD-GROSS YARDS";

                dtcombo.Rows.Add();
                dtcombo.Rows[19][0] = "KGS-KILOGRAMS";

                dtcombo.Rows.Add();
                dtcombo.Rows[20][0] = "KLR-KILOLITRE";

                dtcombo.Rows.Add();
                dtcombo.Rows[21][0] = "KME-KILOMETRE";

                dtcombo.Rows.Add();
                dtcombo.Rows[22][0] = "MLT-MILILITRE";

                dtcombo.Rows.Add();
                dtcombo.Rows[23][0] = "MTR-METERS";

                dtcombo.Rows.Add();
                dtcombo.Rows[24][0] = "MTS-METERIC TON";

                dtcombo.Rows.Add();
                dtcombo.Rows[25][0] = "NOS-NUMBERS";

                dtcombo.Rows.Add();
                dtcombo.Rows[26][0] = "PAC-PACKS";

                dtcombo.Rows.Add();
                dtcombo.Rows[27][0] = "PCS-PIECES";

                dtcombo.Rows.Add();
                dtcombo.Rows[28][0] = "PRS-PAIRS";

                dtcombo.Rows.Add();
                dtcombo.Rows[29][0] = "QTL-QUINTAL";

                dtcombo.Rows.Add();
                dtcombo.Rows[30][0] = "ROL-ROLLS";

                dtcombo.Rows.Add();
                dtcombo.Rows[31][0] = "SET-SETS";

                dtcombo.Rows.Add();
                dtcombo.Rows[32][0] = "SQF-SQUARE FEET";

                dtcombo.Rows.Add();
                dtcombo.Rows[33][0] = "SQM-SQUARE METERS";

                dtcombo.Rows.Add();
                dtcombo.Rows[34][0] = "SQY-SQUARE YARDS";

                dtcombo.Rows.Add();
                dtcombo.Rows[35][0] = "TBS-TABLETS";

                dtcombo.Rows.Add();
                dtcombo.Rows[36][0] = "TGM-TEN GROSS";

                dtcombo.Rows.Add();
                dtcombo.Rows[37][0] = "THD-THOUSANDS";

                dtcombo.Rows.Add();
                dtcombo.Rows[38][0] = "TON-TONNES";

                dtcombo.Rows.Add();
                dtcombo.Rows[39][0] = "TUB-TUBES";

                dtcombo.Rows.Add();
                dtcombo.Rows[40][0] = "UGS-US GALLONS";

                dtcombo.Rows.Add();
                dtcombo.Rows[41][0] = "UNT-UNITS";

                dtcombo.Rows.Add();
                dtcombo.Rows[42][0] = "YDS-YARDS";

                dtcombo.Rows.Add();
                dtcombo.Rows[43][0] = "LTR-LITRES";

                dtcombo.Rows.Add();
                dtcombo.Rows[44][0] = "OTH-OTHERS";

                ansGridView5.CurrentCell.Value = SelectCombo.ComboDt(this, dtcombo, 0);

                if (ansGridView5.CurrentCell.Value != "")
                {
                    ansGridView5.CurrentCell = ansGridView5["Pvalue", ansGridView5.CurrentCell.RowIndex];
                }
            }
        }

        private void groupBox11_Enter(object sender, EventArgs e)
        {

        }
    }
}

