using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace faspi
{
    public partial class frm_rebate : Form
    {
        public frm_rebate()
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
            dtsidefill.Rows[0]["Name"] = "save";
            dtsidefill.Rows[0]["DisplayName"] = "Save";
            dtsidefill.Rows[0]["ShortcutKey"] = "^S";
            dtsidefill.Rows[0]["Visible"] = true;

           



            //close
            dtsidefill.Rows.Add();
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Name"] = "quit";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["DisplayName"] = "Quit";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["ShortcutKey"] = "Esc";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = true;



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
                try
                {
                    Database.BeginTran();
                    save();
                    Database.CommitTran();
                    this.Close();
                    this.Dispose();

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Not Saved", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Database.RollbackTran();
                }
                
            }

           

            else if (name == "quit")
            {
                this.Close();
                this.Dispose();
            }
        }

        private void save()
        {

            DataTable dtTemp = new DataTable("Rebate");
            Database.GetSqlData("select * from Rebate", dtTemp);
            for (int i = 0; i < dtTemp.Rows.Count; i++)
            {
                dtTemp.Rows[i].Delete();
            }
            Database.SaveData(dtTemp);



            DataTable dtrebate = new DataTable("Rebate");
            Database.GetSqlData("Select * from rebate order by id",dtrebate);

            for (int i = 0; i < ansGridView5.Rows.Count ; i++)
            {
               
                dtrebate.Rows.Add();
                if (ansGridView5.Rows[i].Cells["Account"].Value == null)
                {
                    ansGridView5.Rows[i].Cells["Account"].Value = "";
                }
                if (ansGridView5.Rows[i].Cells["Company"].Value == null)
                {
                    ansGridView5.Rows[i].Cells["Company"].Value = "";
                }
                if (ansGridView5.Rows[i].Cells["Item"].Value == null)
                {
                    ansGridView5.Rows[i].Cells["Item"].Value = "";
                }

                dtrebate.Rows[i]["Acid"] = funs.Select_ac_id(ansGridView5.Rows[i].Cells["Account"].Value.ToString());
                dtrebate.Rows[i]["Companyid"] = funs.Select_oth_id(ansGridView5.Rows[i].Cells["Company"].Value.ToString());
                dtrebate.Rows[i]["Itemid"] = funs.Select_oth_id(ansGridView5.Rows[i].Cells["Item"].Value.ToString());



               
                if (ansGridView5.Rows[i].Cells["dis1"].Value == null)
                {
                    ansGridView5.Rows[i].Cells["dis1"].Value = 0;
                }
                if (ansGridView5.Rows[i].Cells["dis2"].Value == null)
                {
                    ansGridView5.Rows[i].Cells["dis2"].Value = 0;
                }
                if (ansGridView5.Rows[i].Cells["dis3"].Value == null)
                {
                    ansGridView5.Rows[i].Cells["dis3"].Value = 0;
                }
                dtrebate.Rows[i]["dis1"] = double.Parse(ansGridView5.Rows[i].Cells["dis1"].Value.ToString());
                dtrebate.Rows[i]["dis2"] = double.Parse(ansGridView5.Rows[i].Cells["dis2"].Value.ToString());
                dtrebate.Rows[i]["dis3"] = double.Parse(ansGridView5.Rows[i].Cells["dis3"].Value.ToString());
            }
            Database.SaveData(dtrebate);
            funs.ShowBalloonTip("Saved", "Saved Successfully");


        }



        private void button1_Click(object sender, EventArgs e)
        {

            if ((textBox1.Text != "" && textBox2.Text != "" && textBox3.Text != "") && (textBox4.Text != "0" || textBox5.Text != "0" || textBox6.Text != "0"))
            {
                ansGridView5.Rows.Add();

                ansGridView5.Rows[ansGridView5.Rows.Count - 1].Cells["sno"].Value = ansGridView5.Rows.Count;
                ansGridView5.Rows[ansGridView5.Rows.Count - 1].Cells["company"].Value = textBox1.Text;
                ansGridView5.Rows[ansGridView5.Rows.Count - 1].Cells["item"].Value = textBox2.Text;
                ansGridView5.Rows[ansGridView5.Rows.Count - 1].Cells["account"].Value = textBox3.Text;
                ansGridView5.Rows[ansGridView5.Rows.Count - 1].Cells["dis1"].Value = textBox4.Text;
                ansGridView5.Rows[ansGridView5.Rows.Count - 1].Cells["dis2"].Value = textBox5.Text;
                ansGridView5.Rows[ansGridView5.Rows.Count - 1].Cells["dis3"].Value = textBox6.Text;

                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                textBox4.Text = "0";
                textBox5.Text = "0";
                textBox6.Text = "0";
            }
            else
            {
                MessageBox.Show("Enter minimum values");
            }
        }

        private void frm_rebate_Load(object sender, EventArgs e)
        {
            SideFill();
            ansGridView5.Columns["dis1"].HeaderText = Feature.Available("Show Text on Discount1");
            ansGridView5.Columns["dis2"].HeaderText = Feature.Available("Show Text on Discount2");
            ansGridView5.Columns["dis3"].HeaderText = Feature.Available("Show Text on Discount3");
            label1.Text = Feature.Available("Show Text on Discount1");
            label2.Text = Feature.Available("Show Text on Discount2");
            label3.Text = Feature.Available("Show Text on Discount3");
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            string strCombo = "Select  distinct '<ALL>' as name from  other union all select [name] from other where Type='" + funs.Get_Company_id() + "' order by [name]";
            textBox1.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            string strCombo = "Select  distinct '<ALL>' as name from  other union all select [name] from other where Type='" + funs.Get_Item_id() + "' order by [name]";
            textBox2.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            string strCombo = "Select  distinct '<ALL>' as name from  Account union all select [name] from Account where branch_id='"+ Database.BranchId+"' order by name";
            textBox3.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 1);
        }

        private void ansGridView5_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (ansGridView5.CurrentCell.OwningColumn.Name == "del")
            {
                    DialogResult res = MessageBox.Show("Are you sure?", "Confirm", MessageBoxButtons.OKCancel);
                    if (res == DialogResult.OK)
                    {

                     //  ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["DisplayName"].Value.ToString()
                        ansGridView5.Rows.RemoveAt(ansGridView5.CurrentRow.Index);
                        for (int i = 0; i < ansGridView5.Rows.Count; i++)
                        {
                            ansGridView5.Rows[i].Cells["sno"].Value = (i + 1);
                        }
                    }
            }
            else if (ansGridView5.CurrentCell.OwningColumn.Name == "edit")
            {
                textBox1.Text = ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Company"].Value.ToString();
                textBox2.Text = ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["item"].Value.ToString();
                textBox3.Text = ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Account"].Value.ToString();
                textBox4.Text = ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["dis1"].Value.ToString();
                textBox5.Text = ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["dis2"].Value.ToString();
                textBox6.Text = ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["dis3"].Value.ToString();
                ansGridView5.Rows.RemoveAt(ansGridView5.CurrentRow.Index);
                for (int i = 0; i < ansGridView5.Rows.Count; i++)
                {
                    ansGridView5.Rows[i].Cells["sno"].Value = (i + 1);
                }
            }
        }
        public void LoadData()
        {

            DataTable dtrebate = new DataTable();

            Database.GetSqlData("SELECT     ACCOUNT.Name AS account, OTHER.Name AS Company, OTHER_1.Name AS Item, Rebate.dis1, Rebate.dis2,    Rebate.dis3  FROM         Rebate LEFT OUTER JOIN     OTHER AS OTHER_1 ON Rebate.Itemid = OTHER_1.Oth_id LEFT OUTER JOIN  OTHER ON Rebate.Companyid = OTHER.Oth_id LEFT OUTER JOIN  ACCOUNT ON Rebate.Acid = ACCOUNT.Ac_id ORDER BY Rebate.id", dtrebate);

            
            ansGridView5.Rows.Clear();
            for (int i = 0; i < dtrebate.Rows.Count; i++)
            {
              //  ansGridView5.CurrentCell = ansGridView5.Rows[0].Cells[0];
                ansGridView5.Rows.Add();
                ansGridView5.Rows[i].Cells["SNo"].Value = (i + 1);

                if (dtrebate.Rows[i]["Account"].ToString() == "")
                {
                    dtrebate.Rows[i]["Account"] = "<ALL>";
                }

                ansGridView5.Rows[i].Cells["Account"].Value = dtrebate.Rows[i]["Account"].ToString();

                if (dtrebate.Rows[i]["company"].ToString() == "")
                {
                    dtrebate.Rows[i]["company"] = "<ALL>";
                }

                ansGridView5.Rows[i].Cells["Company"].Value = dtrebate.Rows[i]["company"].ToString();
                if (dtrebate.Rows[i]["Item"].ToString() == "")
                {
                    dtrebate.Rows[i]["Item"] = "<ALL>";
                }
                ansGridView5.Rows[i].Cells["Item"].Value = dtrebate.Rows[i]["Item"].ToString();
           
                ansGridView5.Rows[i].Cells["dis1"].Value = double.Parse(dtrebate.Rows[i]["dis1"].ToString());
                ansGridView5.Rows[i].Cells["dis2"].Value = double.Parse(dtrebate.Rows[i]["dis2"].ToString());
                ansGridView5.Rows[i].Cells["dis3"].Value = double.Parse(dtrebate.Rows[i]["dis3"].ToString());
            }

          
        }
        private void frm_rebate_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.Control && e.KeyCode == Keys.S)
            {
                //if (validate() == true)
                //{
                    try
                    {
                        Database.BeginTran();
                        save();
                        Database.CommitTran();
                        this.Close();
                        this.Dispose();
                       
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Not Saved", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Database.RollbackTran();
                    }
                //}
            }
            else if (e.KeyCode == Keys.Escape)
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
        }



        private void textBox4_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox5_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox6_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }
    }
}
