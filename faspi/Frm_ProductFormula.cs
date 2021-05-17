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
    public partial class Frm_ProductFormula : Form
    {
        string strCombo = "";
        string gStr = "";
        List<UsersFeature> permission;
        DataTable dtprocon = new DataTable();
        public Frm_ProductFormula()
        {
            InitializeComponent();

        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (textBox2.Text != "")
            {
                strCombo = "SELECT DISTINCT Description as name FROM Description WHERE Pack = '" + textBox2.Text + "' ORDER BY Description";
            }
            else
            {
                strCombo = "select distinct Description as name from Description order by Description";
            }
            textBox1.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 1);
        }


        public void LoadData(string str,String frmcaption)
        {
            gStr = str;
            ansGridView5.Rows.Clear();
            textBox1.Text = "";
            textBox2.Text = "";
            dtprocon = new DataTable("Productformula");
            Database.GetSqlData("Select * from Productformula where productionItem_id='"+str+"' order by Sno",dtprocon);
            if (dtprocon.Rows.Count <= 0)
            {

            }
            else
            {
                ansGridView5.Rows.Clear();
                for (int i = 0; i < dtprocon.Rows.Count; i++)
                {
                    ansGridView5.Rows.Add();
                   textBox1.Text = funs.Select_des_nm(dtprocon.Rows[i]["productionItem_id"].ToString());
                   textBox2.Text = funs.Select_pack_nm(dtprocon.Rows[i]["productionItem_id"].ToString());

                   ansGridView5.Rows[i].Cells["itemname"].Value = funs.Select_des_nm(dtprocon.Rows[i]["ConsumItem_id"].ToString());
                   ansGridView5.Rows[i].Cells["pack"].Value = funs.Select_pack_nm(dtprocon.Rows[i]["ConsumItem_id"].ToString());
                   ansGridView5.Rows[i].Cells["sno"].Value =i+1;
                   ansGridView5.Rows[i].Cells["Quantity"].Value =funs.DecimalPoint(double.Parse(dtprocon.Rows[i]["qty"].ToString()));
                    
                }
            }

        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (textBox1.Text != "")
            {
                strCombo = "SELECT DISTINCT Pack as Packing FROM Description WHERE Description = '" + textBox1.Text + "' ORDER BY Pack";
            }
            else
            {
                strCombo = "SELECT DISTINCT Pack as Packing FROM Description ORDER BY Packing";
            }
            textBox2.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
            
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
            //dtsidefill.Rows[0]["Visible"] = true;
            permission = funs.GetPermissionKey("ProductFormula");
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
                    //create
                    UsersFeature ob = permission.Where(w => w.FeatureName == "Create").FirstOrDefault();
                    if (ob != null && gStr == "0" && ob.SelectedValue == "Allowed")
                    {
                        Save();
                    }

                    //alter
                    ob = permission.Where(w => w.FeatureName == "Alter").FirstOrDefault();
                    if (ob != null && gStr != "0" && ob.SelectedValue == "Allowed")
                    {
                        Save();
                    }

                    //if (Database.utype.ToUpper() == "SUPERADMIN" || Database.utype.ToUpper() == "ADMIN")
                    //{
                    //    Save();
                    //}

                    //else if (gStr == "0")
                    //{
                    //    Save();
                    //}
                    
                }
            }

            else if (name == "quit")
            {
                this.Close();
                this.Dispose();
            }
        }

        private void  Save()
        {
            for (int i = 0; i < dtprocon.Rows.Count; i++)
            {
                dtprocon.Rows[i].Delete();



            }
            Database.SaveData(dtprocon);
            for (int i = 0; i < ansGridView5.Rows.Count-1; i++)
            {
                dtprocon.Rows.Add();
                dtprocon.Rows[dtprocon.Rows.Count - 1]["productionItem_id"] = funs.Select_des_id(textBox1.Text, textBox2.Text);
                dtprocon.Rows[dtprocon.Rows.Count - 1]["ConsumItem_id"] = funs.Select_des_id(ansGridView5.Rows[i].Cells["itemname"].Value.ToString(), ansGridView5.Rows[i].Cells["pack"].Value.ToString());
                dtprocon.Rows[dtprocon.Rows.Count - 1]["sno"] = i+1;
                dtprocon.Rows[dtprocon.Rows.Count - 1]["qty"] = double.Parse(ansGridView5.Rows[i].Cells["quantity"].Value.ToString());

            }

            Database.SaveData(dtprocon);
            funs.ShowBalloonTip("Saved Successfully", "Saved");
            this.Close();
            this.Dispose();
        }


        private bool validate()
        {
            if (textBox1.Text == "")
            {
                textBox1.Focus();
                return false;
            }
            if (textBox2.Text == "")
            {
                textBox2.Focus();
                return false;
            }
            if (ansGridView5.Rows.Count == 1)
            {
                return false;
            }
            return true;
        }
        private void ansGridView5_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            ansGridView5.Rows[e.RowIndex].Cells["sno"].Value = e.RowIndex + 1;
            if (ansGridView5.CurrentCell.OwningColumn.Name == "sno")
            {
                SendKeys.Send("{right}");
                this.Activate();
            }
        }

        private void ansGridView5_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsLetter(e.KeyChar) || char.IsNumber(e.KeyChar) || e.KeyChar == ' ' || Convert.ToInt32(e.KeyChar) == 13)
            {
            }
            else
            {
                return;
            }


            if (ansGridView5.CurrentCell.OwningColumn.Name == "pack" || ansGridView5.CurrentCell.OwningColumn.Name == "itemname")
            {
                String ActiveCell = "";
                if (ansGridView5.CurrentCell.OwningColumn.Name == "pack")
                {
                    ActiveCell = "Packing";
                }
                else if (ansGridView5.CurrentCell.OwningColumn.Name == "itemname")
                {
                    ActiveCell = "Desc";
                }
                DataTable dtDesc = new DataTable();
                if (Master.DescriptionInfo.Select("Description<>''", "Description, PACKING").Length == 0)
                {
                    return;
                }
                else
                {

                    dtDesc = Master.DescriptionInfo.Select("Description<>'' and Status='Enable'", "Description, PACKING").CopyToDataTable();

                }
            
                DataTable dtPack = new DataTable(); ;
                if (ActiveCell == "Packing" && ansGridView5.CurrentCell.OwningRow.Cells["itemname"].Value != null && ansGridView5.CurrentCell.OwningRow.Cells["itemname"].Value.ToString() != "")
                {
                    dtDesc = dtDesc.Select("description='" + ansGridView5.CurrentCell.OwningRow.Cells["itemname"].Value.ToString() + "'").CopyToDataTable();
                }
                else if (ActiveCell == "Desc" && ansGridView5.CurrentCell.OwningRow.Cells["pack"].Value != null && ansGridView5.CurrentCell.OwningRow.Cells["pack"].Value.ToString() != "")
                {
                    dtDesc = dtDesc.Select("Packing='" + ansGridView5.CurrentCell.OwningRow.Cells["pack"].Value.ToString() + "'").CopyToDataTable();
                }
                if (ActiveCell == "Packing")
                {
                    dtPack = dtDesc.DefaultView.ToTable(true, "Packing");
                }
                else
                {
                    dtPack = dtDesc.DefaultView.ToTable(true, "description");
                }

                DataRow[] SKU = dtDesc.DefaultView.ToTable(true, "Skucode").Select("Skucode<>''");
                DataRow[] ShortCode = dtDesc.DefaultView.ToTable(true, "ShortCode").Select("ShortCode<>''");
                DataTable dtPS;
                DataTable dtPSS;
                if (SKU.Length > 0)
                {
                    DataTable dtSKU = SKU.CopyToDataTable();
                    dtPS = dtPack.AsEnumerable()
                        .Union(dtSKU.AsEnumerable()).CopyToDataTable();
                }
                else
                {
                    dtPS = dtPack.Copy();
                }
                if (ShortCode.Length > 0)
                {
                    DataTable dtShortCode = ShortCode.CopyToDataTable();
                    dtPSS = dtPS.AsEnumerable()
                         .Union(dtShortCode.AsEnumerable()).CopyToDataTable();
                }
                else
                {
                    dtPSS = dtPS.Copy();
                }
                String packing = "", Desc = "";
                if (ActiveCell == "Packing")
                {
                    packing = SelectCombo.CallHelp(this, dtPSS, e.KeyChar.ToString(), 0);
                    if (packing == "") return;
                    dtDesc = dtDesc.Select("Packing='" + packing + "' or Skucode='" + packing + "' or ShortCode='" + packing + "'").CopyToDataTable();
                }
                else
                {
                    Desc = SelectCombo.CallHelp(this, dtPSS, e.KeyChar.ToString(), 0);
                    if (Desc == "") return;
                    dtDesc = dtDesc.Select("description='" + Desc + "' or Skucode='" + Desc + "' or ShortCode='" + Desc + "'").CopyToDataTable();
                }


               
                if (dtDesc.Rows.Count == 1)
                {

                    if (ansGridView5.CurrentCell.OwningRow.Cells["itemname"].Value == null || ansGridView5.CurrentCell.OwningRow.Cells["itemname"].Value.ToString() == "")
                    {
                        ansGridView5.CurrentCell.OwningRow.Cells["itemname"].Value = dtDesc.Rows[0]["description"];
                        
                    }
                    ansGridView5.CurrentCell.OwningRow.Cells["Pack"].Value = dtDesc.Rows[0]["Packing"];
                    ansGridView5.CurrentCell.OwningRow.Cells["Quantity"].Value = 0;
                   
                    this.Activate();
                   
                }
                else if (dtDesc.Rows.Count > 1)
                {
                    if (ActiveCell == "Packing")
                    {
                        ansGridView5.CurrentCell.OwningRow.Cells["pack"].Value = dtDesc.Rows[0]["Packing"];
                       
                        ansGridView5.CurrentCell.OwningRow.Cells["itemname"].Value = "";
                        
                    }
                    else
                    {
                        ansGridView5.CurrentCell.OwningRow.Cells["itemname"].Value = dtDesc.Rows[0]["description"];
                      
                        ansGridView5.CurrentCell.OwningRow.Cells["pack"].Value = "";
                    }

                   
                    ansGridView5.CurrentCell.OwningRow.Cells["Quantity"].Value = 0;
                  
                   

                    this.Activate();
                    if (ActiveCell == "Packing")
                    {
                        ansGridView5.CurrentCell = ansGridView5["itemname", ansGridView5.CurrentCell.RowIndex];
                    }
                    else
                    {
                        ansGridView5.CurrentCell = ansGridView5["pack", ansGridView5.CurrentCell.RowIndex];
                    }
                }
            }
        }

        private void Frm_ProductFormula_Load(object sender, EventArgs e)
        {
            SideFill();
        }

        private void Frm_ProductFormula_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
            {
                if (validate() == true)
                {

                    //create
                    UsersFeature ob = permission.Where(w => w.FeatureName == "Create").FirstOrDefault();
                    if (ob != null && gStr == "0" && ob.SelectedValue == "Allowed")
                    {
                        Save();
                    }

                    //alter
                    ob = permission.Where(w => w.FeatureName == "Alter").FirstOrDefault();
                    if (ob != null && gStr != "0" && ob.SelectedValue == "Allowed")
                    {
                        Save();
                    }

                    //if (Database.utype.ToUpper() == "SUPERADMIN" || Database.utype.ToUpper() == "ADMIN")
                    //{
                    //    Save();
                    //}

                    //else if (gStr == "0")
                    //{
                    //    Save();
                    //}
                }
            }
            
            else if (e.KeyCode == Keys.Escape)
            {
                if (textBox1.Text != "")
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

        private void textBox1_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox1);
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox2);
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox1);
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox2);
        }

        private void ansGridView5_KeyDown(object sender, KeyEventArgs e)
        {
            if (ansGridView5.CurrentCell == null)
            {
                return;
            }


            if (e.KeyCode == Keys.Delete)
            {
                if (ansGridView5.CurrentRow.Index == ansGridView5.Rows.Count - 1)
                {
                    for (int i = 1; i < ansGridView5.Columns.Count; i++)
                    {
                        ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells[i].Value = null;
                    }
                }
                else
                {
                    int rindex = ansGridView5.CurrentRow.Index;
                    ansGridView5.Rows.RemoveAt(rindex);
                    for (int i = 0; i < ansGridView5.Rows.Count; i++)
                    {
                        ansGridView5.Rows[i].Cells["sno"].Value = (i + 1);
                    }
                    return;
                }
            }

        }
    }
}
