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
    public partial class frm_openingstock : Form
    {
        DataTable dtOpnStk;
        bool gIskachcha = false;
        String strCombo;

        public frm_openingstock()
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
                try
                {
                    Database.BeginTran();
                    if (validate() == true)
                    {
                        save();
                        this.Close();
                        this.Dispose();
                    }
                    Database.CommitTran();
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

        public void LoadData()
        {
            dtOpnStk = new DataTable("Stock");
            Database.GetSqlData("Select * from Stock where Vid='0' and  marked=" + access_sql.Singlequote + gIskachcha + access_sql.Singlequote + "  and godown_id='" + funs.Select_ac_id(textBox1.Text) + "' and Branch_id='" + Database.BranchId + "' order by Itemsr", dtOpnStk);
            ansGridView1.Rows.Clear();

            dtOpnStk.Columns.Add("Description", typeof(string));
            dtOpnStk.Columns.Add("Pack", typeof(string));

            for (int i = 0; i < dtOpnStk.Rows.Count; i++)
            {
                dtOpnStk.Rows[i]["Description"] = funs.Select_des_nm(dtOpnStk.Rows[i]["Did"].ToString());
                string pack = Database.GetScalarText("Select pack from Description where Des_id='" + dtOpnStk.Rows[i]["Did"].ToString() + "' ");
                dtOpnStk.Rows[i]["Pack"] = pack;
            }

            dtOpnStk.DefaultView.Sort = "Itemsr,Description,Pack";
            dtOpnStk = dtOpnStk.DefaultView.ToTable();
            double opn = 0;
            for (int i = 0; i < dtOpnStk.Rows.Count; i++)
            {
                ansGridView1.Rows.Add();
                ansGridView1.Rows[i].Cells["SNo"].Value = (i + 1);
                ansGridView1.Rows[i].Cells["Ac_id"].Value = funs.Select_ac_id(textBox1.Text);
                ansGridView1.Rows[i].Cells["Des_id"].Value = dtOpnStk.Rows[i]["Did"].ToString();
                ansGridView1.Rows[i].Cells["Description"].Value = funs.Select_des_nm(dtOpnStk.Rows[i]["Did"].ToString());
                string pack = Database.GetScalarText("Select pack from Description where Des_id='" + dtOpnStk.Rows[i]["Did"].ToString() + "' ");
                ansGridView1.Rows[i].Cells["Pack"].Value = pack;
                ansGridView1.Rows[i].Cells["Qty"].Value = funs.DecimalPoint(double.Parse(dtOpnStk.Rows[i]["Receive"].ToString()), 3);                
                ansGridView1.Rows[i].Cells["Amt"].Value = funs.DecimalPoint(double.Parse(dtOpnStk.Rows[i]["ReceiveAmt"].ToString()), 2);
                double rate = 0;
                rate=double.Parse(dtOpnStk.Rows[i]["ReceiveAmt"].ToString())/double.Parse(dtOpnStk.Rows[i]["Receive"].ToString());
                opn = opn + double.Parse(dtOpnStk.Rows[i]["ReceiveAmt"].ToString());
                ansGridView1.Rows[i].Cells["Rate"].Value = funs.DecimalPoint(rate, 2);
                ansGridView1.Rows[i].Cells["batchcode"].Value = dtOpnStk.Rows[i]["Batch_no"].ToString();
            }
            label2.Text = funs.DecimalPoint(opn, 2);
            if (Feature.Available("Item Name before Packing") == "Yes")
            {
                ansGridView1.CurrentCell = ansGridView1.Rows[0].Cells["Description"];  
            }
            else
            {
                ansGridView1.CurrentCell = ansGridView1.Rows[0].Cells["Pack"];
            }            
        }

        public void LoadDatao()
        {
            dtOpnStk = new DataTable("Opening");
            Database.GetSqlData("Select * from opening where Iskachcha=" + access_sql.Singlequote + gIskachcha + access_sql.Singlequote + "  and godown_id='" + funs.Select_ac_id(textBox1.Text) + "' ", dtOpnStk);
            ansGridView1.Rows.Clear();
            for (int i = 0; i < dtOpnStk.Rows.Count; i++)
            {
                ansGridView1.Rows.Add();
                ansGridView1.Rows[i].Cells["SNo"].Value = (i + 1);
                ansGridView1.Rows[i].Cells["Ac_id"].Value = funs.Select_ac_id(textBox1.Text);
                ansGridView1.Rows[i].Cells["Des_id"].Value = dtOpnStk.Rows[i]["Did"].ToString();
                ansGridView1.Rows[i].Cells["Description"].Value = funs.Select_des_nm(dtOpnStk.Rows[i]["Did"].ToString());
                string pack = Database.GetScalarText("Select pack from Description where Des_id='" + dtOpnStk.Rows[i]["Did"].ToString() + "' ");
                ansGridView1.Rows[i].Cells["Pack"].Value = pack;
                ansGridView1.Rows[i].Cells["Qty"].Value = funs.DecimalPoint(double.Parse(dtOpnStk.Rows[i]["Quantity"].ToString()), 2);
                ansGridView1.Rows[i].Cells["Amt"].Value = funs.DecimalPoint(double.Parse(dtOpnStk.Rows[i]["Amount"].ToString()), 2);
            }
            ansGridView1.CurrentCell = ansGridView1.Rows[0].Cells["Pack"];
        }

        private void frm_openingstock_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
                this.Dispose();

            }
            else if (e.Control && e.KeyCode == Keys.S)
            {
                try
                {
                    Database.BeginTran();
                    if (validate() == true)
                    {
                        save();
                        this.Close();
                        this.Dispose();
                    }
                    Database.CommitTran();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Not Saved", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Database.RollbackTran();
                }
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.C)
            {
                textBox1.Text = funs.AddAccount();
            }
            else if (e.Control && e.KeyCode == Keys.A)
            {
                textBox1.Text = funs.EditAccount(textBox1.Text);
            }

            string act_id = Database.GetScalarText("Select act_id from Account where Name='" + textBox1.Text + "'");
            string act_name = Database.GetScalarText("Select Name from Accountype where act_id='" + act_id + "' ");

            if (act_name != "Godown")
            {
                textBox1.Text = "<MAIN>";
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            strCombo = "select distinct '<MAIN>' as name from account union all Select Name from Account where act_id='" + funs.Select_act_id("Godown") + "' and Branch_id='" + Database.BranchId + "' order by Name";
            DataTable dtgodown = new DataTable();
            Database.GetSqlData(strCombo, dtgodown);
            if (dtgodown.Rows.Count == 1)
            {
                textBox1.Text = dtgodown.Rows[0]["name"].ToString();
            }
            else
            {
                textBox1.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 1);
            }           
            LoadData();
        }

        private void ansGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            ansGridView1.Rows[e.RowIndex].Cells["SNo"].Value = e.RowIndex + 1;
            if (ansGridView1.CurrentCell.OwningColumn.Name == "SNo")
            {
                SendKeys.Send("{right}");
            }
        }

        private void ansGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            ansGridView1.CurrentCell.Value = 0;
        }
        private void DisplaySetting()
        {

            if (Feature.Available("Item Name before Packing") == "Yes")
            {
                ansGridView1.Columns["Description"].DisplayIndex = 1;
                ansGridView1.Columns["Pack"].DisplayIndex = 2;
            }
            else
            {
                ansGridView1.Columns["Pack"].DisplayIndex = 1;
                ansGridView1.Columns["Description"].DisplayIndex = 2;
            }
            if (Feature.Available("Batch Number") == "Yes")
            {
                ansGridView1.Columns["batchcode"].Visible = true;
            }
            else
            {
                ansGridView1.Columns["batchcode"].Visible = false;
            }
            ansGridView1.Columns["batchcode"].DisplayIndex = 3;
            ansGridView1.Columns["Qty"].DisplayIndex = 4;
            ansGridView1.Columns["Rate"].DisplayIndex = 5;
            ansGridView1.Columns["Amt"].DisplayIndex = 6;
        }

        private void ansGridView1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsLetter(e.KeyChar) || char.IsNumber(e.KeyChar) || e.KeyChar == ' ')
            {
                String ActiveCell = "";
                if (ansGridView1.CurrentCell.OwningColumn.Name == "Pack")
                {
                    ActiveCell = "Packing";
                }
                else if (ansGridView1.CurrentCell.OwningColumn.Name == "Description")
                {
                    ActiveCell = "Desc";
                }
                if (ActiveCell == "Desc" && (ansGridView1.CurrentCell.OwningRow.Cells["pack"].Value == null || ansGridView1.CurrentCell.OwningRow.Cells["pack"].Value == ""))
                {
                    strCombo = "select distinct(description) from description order by description";
                    ansGridView1.CurrentCell.Value = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
                    ansGridView1.CurrentCell = ansGridView1["Pack", ansGridView1.CurrentCell.RowIndex];
                    if (ansGridView1.CurrentCell.Value != "")
                    {
                        DataTable dt = new DataTable("Description");
                        Database.GetSqlData("select pack from Description where description='" + ansGridView1.CurrentCell.OwningRow.Cells["Description"].Value + "' order by Description", dt);
                        if (dt.Rows.Count == 1)
                        {
                            ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["pack"].Value=dt.Rows[0][0].ToString();
                            ansGridView1.CurrentCell = ansGridView1["Qty", ansGridView1.CurrentCell.RowIndex];
                        }
                    }
                }
                else if (ActiveCell == "Packing" && ansGridView1.CurrentCell.OwningRow.Cells["Description"].Value != null && ansGridView1.CurrentCell.OwningRow.Cells["Description"].Value.ToString() != "")
                {
                        strCombo = "select pack from Description where description='" + ansGridView1.CurrentCell.OwningRow.Cells["Description"].Value + "' order by Description";
                        ansGridView1.CurrentCell.Value = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
                        ansGridView1.CurrentCell = ansGridView1["Qty", ansGridView1.CurrentCell.RowIndex];
                }
                else if (ActiveCell == "Desc" && ansGridView1.CurrentCell.OwningRow.Cells["pack"].Value != null && ansGridView1.CurrentCell.OwningRow.Cells["pack"].Value.ToString() != "")
                {
                    strCombo = "select description from description where pack='" + ansGridView1.CurrentCell.OwningRow.Cells["Pack"].Value.ToString() + "' order by description";
                    ansGridView1.CurrentCell.Value = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
                    ansGridView1.CurrentCell = ansGridView1["Qty", ansGridView1.CurrentCell.RowIndex];
                }                
                else if (ActiveCell == "Packing" && (ansGridView1.CurrentCell.OwningRow.Cells["Description"].Value == null || ansGridView1.CurrentCell.OwningRow.Cells["Description"].Value == ""))
                {
                    strCombo = "select distinct(pack) from description order by Pack";
                    ansGridView1.CurrentCell.Value = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
                    ansGridView1.CurrentCell = ansGridView1["Description", ansGridView1.CurrentCell.RowIndex];
                }
                ansGridView1.CurrentCell.OwningRow.Cells["Qty"].Value = 0;
                ansGridView1.CurrentCell.OwningRow.Cells["Rate"].Value = 0;
                ansGridView1.CurrentCell.OwningRow.Cells["Amt"].Value = 0;
            }
        }

        private void ansGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (ansGridView1.CurrentCell == null)
            {
                return;
            }
            if (e.KeyCode == Keys.Delete)
            {
                if (ansGridView1.CurrentRow.Index == ansGridView1.Rows.Count - 1)
                {
                    for (int i = 1; i < ansGridView1.Columns.Count; i++)
                    {
                        ansGridView1.Rows[ansGridView1.CurrentRow.Index].Cells[i].Value = null;
                    }
                    Calc();
                }
                else
                {
                    int rindex = ansGridView1.CurrentRow.Index;
                    ansGridView1.Rows.RemoveAt(rindex);
                    for (int i = 0; i < ansGridView1.Rows.Count; i++)
                    {
                        ansGridView1.Rows[i].Cells["SNo"].Value = (i + 1);
                    }
                    Calc();
                    return;
                }
            }
            if (ansGridView1.CurrentCell.OwningColumn.Name == "Amt")
            {
                if (ansGridView1.CurrentCell.Value == null || ansGridView1.Rows[ansGridView1.CurrentRow.Index].Cells["Amt"].Value.ToString() == "")
                {
                    return;
                }

                if (ansGridView1.CurrentRow.Index == ansGridView1.Rows.Count - 1 && double.Parse(ansGridView1.Rows[ansGridView1.CurrentRow.Index].Cells["Amt"].Value.ToString()) == 0)
                {
                    SendKeys.Send("{tab}");
                }
            }
        }

        private void save()
        {

            DataTable dtTemp = new DataTable("Stock");
            Database.GetSqlData("select * from Stock where Vid='0' and marked=" + access_sql.Singlequote + gIskachcha + access_sql.Singlequote + " and godown_id='" + funs.Select_ac_id(textBox1.Text) + "' and Branch_id='" + Database.BranchId + "' ", dtTemp);
            for (int i = 0; i < dtTemp.Rows.Count; i++)
            {
                dtTemp.Rows[i].Delete();
            }
            Database.SaveData(dtTemp);

            dtOpnStk = new DataTable("Stock");
            Database.GetSqlData("select * from Stock where Vid='0' and marked=" + access_sql.Singlequote + gIskachcha + access_sql.Singlequote + "  and godown_id='" + funs.Select_ac_id(textBox1.Text) + "' and Branch_id='" + Database.BranchId + "' ", dtOpnStk);
            for (int i = 0; i < ansGridView1.Rows.Count - 1; i++)
            {
                string des_id = Database.GetScalarText("SELECT DESCRIPTION.Des_id FROM DESCRIPTION WHERE DESCRIPTION.Description='" + ansGridView1.Rows[i].Cells["Description"].Value + "' AND DESCRIPTION.Pack= '" + ansGridView1.Rows[i].Cells["Pack"].Value.ToString() + "'  order by description,Pack");
                dtOpnStk.Rows.Add();
                if (textBox1.Text == "<MAIN>")
                {
                    dtOpnStk.Rows[i]["godown_id"] = "";
                }
                else
                {
                    dtOpnStk.Rows[i]["godown_id"] = funs.Select_ac_id(textBox1.Text);
                }
                dtOpnStk.Rows[i]["Vid"] = "0";
                dtOpnStk.Rows[i]["Did"] = des_id;
                dtOpnStk.Rows[i]["Itemsr"] = (i+1);
                dtOpnStk.Rows[i]["Branch_id"] = Database.BranchId;
                dtOpnStk.Rows[i]["marked"] = gIskachcha;
                if (ansGridView1.Rows[i].Cells["Qty"].Value == null)
                {
                    ansGridView1.Rows[i].Cells["Qty"].Value = 0;
                }
                dtOpnStk.Rows[i]["Receive"] = ansGridView1.Rows[i].Cells["Qty"].Value;
                dtOpnStk.Rows[i]["Issue"] = 0;
                if (ansGridView1.Rows[i].Cells["Amt"].Value == null)
                {
                    ansGridView1.Rows[i].Cells["Amt"].Value = 0;
                }
                dtOpnStk.Rows[i]["ReceiveAmt"] = ansGridView1.Rows[i].Cells["Amt"].Value;
                dtOpnStk.Rows[i]["IssueAmt"] = 0;
                dtOpnStk.Rows[i]["Locationid"] = Database.LocationId;
                dtOpnStk.Rows[i]["Batch_no"] = ansGridView1.Rows[i].Cells["batchcode"].Value;
            }

            Database.SaveData(dtOpnStk);
            funs.ShowBalloonTip("Saved", "Saved Successfully");
            textBox1.Text = "";
            ansGridView1.Rows.Clear();
            LoadData();
        }

        private bool validate()
        {
            if (textBox1.Text == "")
            {
                textBox1.BackColor = Color.Aqua;
                textBox1.Focus();
                return false;
            }
            for (int i = 0; i < ansGridView1.RowCount - 1; i++)
            {
                if (ansGridView1.Rows[i].Cells["Description"].Value.ToString() == "")
                {
                    ansGridView1.CurrentCell = ansGridView1["Description", ansGridView1.CurrentCell.RowIndex];
                    MessageBox.Show("Enter Description");
                    return false;
                }
                if (ansGridView1.Rows[i].Cells["Pack"].Value.ToString() == "")
                {
                    ansGridView1.CurrentCell = ansGridView1["Pack", ansGridView1.CurrentCell.RowIndex];
                    MessageBox.Show("Enter Packing");
                    return false;
                }
            }
            return true;
        }

        private void frm_openingstock_Load(object sender, EventArgs e)
        {
            foreach (DataGridViewColumn column in ansGridView1.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            ansGridView1.Columns["Qty"].CellTemplate.ValueType = typeof(double);
            ansGridView1.Columns["Amt"].CellTemplate.ValueType = typeof(double);
            gIskachcha = Database.IsKacha;
            textBox1.Focus();
            this.Size = this.MdiParent.Size;
            if (Feature.Available("Multi-Godown") == "No")
            {
                textBox1.Enabled = false;
                textBox1.ReadOnly = true;
                textBox1.Text = "<MAIN>";

            }
            else
            {
                textBox1.Enabled = true;
                textBox1.Text = "<MAIN>";
            }
            SideFill();
            DisplaySetting();
            LoadData();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {            
            LoadData();
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox1);
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox1);
        }

        private void ansGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (ansGridView1.CurrentCell.OwningColumn.Name == "Amt")
            {  
                if (double.Parse(ansGridView1.Rows[e.RowIndex].Cells["Qty"].Value.ToString()) == 0 && double.Parse(ansGridView1.Rows[e.RowIndex].Cells["rate"].Value.ToString()) == 0)
                {
                    ansGridView1.Rows[e.RowIndex].Cells["Qty"].Value = 1;
                    ansGridView1.Rows[e.RowIndex].Cells["rate"].Value = funs.DecimalPoint(ansGridView1.Rows[e.RowIndex].Cells["Amt"].Value.ToString(), 2);
                }
                else if (double.Parse(ansGridView1.Rows[e.RowIndex].Cells["Qty"].Value.ToString()) != 0 && double.Parse(ansGridView1.Rows[e.RowIndex].Cells["rate"].Value.ToString()) == 0)
                {
                    ansGridView1.Rows[e.RowIndex].Cells["rate"].Value = funs.DecimalPoint(double.Parse(ansGridView1.Rows[e.RowIndex].Cells["Amt"].Value.ToString()) / double.Parse(ansGridView1.Rows[e.RowIndex].Cells["Qty"].Value.ToString()));
                }
                else if (double.Parse(ansGridView1.Rows[e.RowIndex].Cells["Qty"].Value.ToString()) == 0 && double.Parse(ansGridView1.Rows[e.RowIndex].Cells["rate"].Value.ToString()) != 0)
                {
                    ansGridView1.Rows[e.RowIndex].Cells["Qty"].Value = funs.DecimalPoint(double.Parse(ansGridView1.Rows[e.RowIndex].Cells["Amt"].Value.ToString()) / double.Parse(ansGridView1.Rows[e.RowIndex].Cells["rate"].Value.ToString()));
                }
                else if (double.Parse(ansGridView1.Rows[e.RowIndex].Cells["Qty"].Value.ToString()) != 0 && double.Parse(ansGridView1.Rows[e.RowIndex].Cells["rate"].Value.ToString()) != 0)
                {
                    ansGridView1.Rows[e.RowIndex].Cells["rate"].Value = funs.DecimalPoint(double.Parse(ansGridView1.Rows[e.RowIndex].Cells["Amt"].Value.ToString()) / double.Parse(ansGridView1.Rows[e.RowIndex].Cells["Qty"].Value.ToString()));
                }
            }
            if (ansGridView1.CurrentCell.OwningColumn.Name == "Qty" && ansGridView1.Rows[e.RowIndex].Cells["Qty"].Value.ToString() != "")
            {
                ansGridView1.Rows[e.RowIndex].Cells["Qty"].Value = funs.DecimalPoint(ansGridView1.Rows[e.RowIndex].Cells["Qty"].Value, 3);
                ansGridView1.Rows[e.RowIndex].Cells["Amt"].Value = funs.DecimalPoint(double.Parse(ansGridView1.Rows[e.RowIndex].Cells["Qty"].Value.ToString()) * double.Parse(ansGridView1.Rows[e.RowIndex].Cells["rate"].Value.ToString()));
            }
            if (ansGridView1.CurrentCell.OwningColumn.Name == "Rate" && ansGridView1.Rows[e.RowIndex].Cells["Rate"].Value.ToString() != "")
            {
                ansGridView1.Rows[e.RowIndex].Cells["rate"].Value = funs.DecimalPoint(ansGridView1.Rows[e.RowIndex].Cells["rate"].Value);
                ansGridView1.Rows[e.RowIndex].Cells["Amt"].Value = funs.DecimalPoint(double.Parse(ansGridView1.Rows[e.RowIndex].Cells["Qty"].Value.ToString()) * double.Parse(ansGridView1.Rows[e.RowIndex].Cells["rate"].Value.ToString()));
            }
            Calc();
        }

        private void Calc()
        {
            double amt = 0;
            for (int i = 0; i < ansGridView1.Rows.Count - 1; i++)
            {
                amt += double.Parse(ansGridView1.Rows[i].Cells["Amt"].Value.ToString());
            }
            label2.Text = funs.DecimalPoint(amt, 2);
        }
    }
}
