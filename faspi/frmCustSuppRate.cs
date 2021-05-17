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
    public partial class frmCustSuppRate : Form
    {
        DataTable dtCustSuppRate;
        String dtName;
        string gstr = "";
        string gtyp = "";
        String strCombo;

        public frmCustSuppRate()
        {
            InitializeComponent();
        }

        public void LoadData(string str, string typ)
        {
            gstr = str;
            gtyp = typ;
            if (typ != "New")
            {
                textBox1.Enabled = false;
            }
            textBox1.Text = funs.Select_ac_nm(str);
            dtName = "PARTYRATE";
            dtCustSuppRate = new DataTable(dtName);

            Database.GetSqlData("SELECT PARTYRATE.Ac_id, DESCRIPTION.Des_id, DESCRIPTION.Description, DESCRIPTION.Pack AS Packing,partyrate.Rate FROM (PARTYRATE INNER JOIN DESCRIPTION ON PARTYRATE.Des_id = DESCRIPTION.Des_id)  WHERE (((PARTYRATE.Ac_id)='" + str + "'))", dtCustSuppRate);

            if (textBox1.Text == "")
            {
                return;
            }
            ansGridView1.Rows.Clear();
            for (int i = 0; i < dtCustSuppRate.Rows.Count; i++)
            {
                ansGridView1.CurrentCell = ansGridView1.Rows[0].Cells[0];
                ansGridView1.Rows.Add();
                ansGridView1.Rows[i].Cells["SNo"].Value = (i + 1);
                ansGridView1.Rows[i].Cells["Ac_id"].Value = funs.Select_ac_id(textBox1.Text);
                ansGridView1.Rows[i].Cells["Des_id"].Value = dtCustSuppRate.Rows[i]["Des_id"].ToString();
                ansGridView1.Rows[i].Cells["Description"].Value = dtCustSuppRate.Rows[i]["Description"].ToString();
                ansGridView1.Rows[i].Cells["Packing"].Value = dtCustSuppRate.Rows[i]["Packing"].ToString();
                ansGridView1.Rows[i].Cells["Rate"].Value =  double.Parse(dtCustSuppRate.Rows[i]["Rate"].ToString());

            }

            if (Feature.Available("Item Name before Packing") == "Yes")
            {
                ansGridView1.CurrentCell = ansGridView1.Rows[0].Cells["Description"];
            }
            else
            {
                ansGridView1.CurrentCell = ansGridView1.Rows[0].Cells["Packing"];
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
                    try
                    {
                        Database.BeginTran();
                        save();
                        Database.CommitTran();
                        this.Close();
                        this.Dispose();
                        //textBox1.Text = "";
                        //ansGridView1.Rows.Clear();
                        //LoadData(gstr, gtyp);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Not Saved", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Database.RollbackTran();
                    }
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
            DataTable dtTemp = new DataTable("PARTYRATE");
            Database.GetSqlData("select * from PARTYRATE where Ac_id='" + funs.Select_ac_id(textBox1.Text) + "' ", dtTemp);
            for (int i = 0; i < dtTemp.Rows.Count; i++)
            {
                dtTemp.Rows[i].Delete();
            }
            Database.SaveData(dtTemp);

            dtCustSuppRate = new DataTable("PARTYRATE");
            Database.GetSqlData("select * from PARTYRATE where Ac_id='" + funs.Select_ac_id(textBox1.Text) + "' ", dtCustSuppRate);

            for (int i = 0; i < ansGridView1.Rows.Count - 1; i++)
            {
                string des_id = Database.GetScalarText("SELECT DESCRIPTION.Des_id FROM DESCRIPTION WHERE DESCRIPTION.Description='" + ansGridView1.Rows[i].Cells["Description"].Value + "' AND DESCRIPTION.Pack= '" + ansGridView1.Rows[i].Cells["Packing"].Value + "' order by description");
                dtCustSuppRate.Rows.Add();
                dtCustSuppRate.Rows[i]["Ac_id"] = funs.Select_ac_id(textBox1.Text);
                dtCustSuppRate.Rows[i]["Des_id"] = des_id;
                if (ansGridView1.Rows[i].Cells["Rate"].Value == null)
                {
                    ansGridView1.Rows[i].Cells["Rate"].Value = 0;
                }
                dtCustSuppRate.Rows[i]["Rate"] = ansGridView1.Rows[i].Cells["Rate"].Value;
                dtCustSuppRate.Rows[i]["LocationId"] = Database.LocationId;
            }
            Database.SaveData(dtCustSuppRate);
            funs.ShowBalloonTip("Saved", "Saved Successfully");
        }

        private bool validate()
        {
            if (textBox1.Text == "")
            {
                textBox1.BackColor = Color.Aqua;
                textBox1.Focus();
                return false;
            }
            if (ansGridView1.Rows.Count - 1 == 0)
            {
                MessageBox.Show("Enter some Values");
                return false;
            }
            return true;
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
           // strCombo = funs.GetStrCombo("*");
            strCombo = funs.GetStrComboled("*");
            textBox1.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 1);
            LoadData(funs.Select_ac_id(textBox1.Text).ToString(), gtyp);
        }

        private void ansGridView1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsLetter(e.KeyChar) || char.IsNumber(e.KeyChar) || e.KeyChar == ' ')
            {
                String ActiveCell = "";
                if (ansGridView1.CurrentCell.OwningColumn.Name == "Packing")
                {
                    ActiveCell = "Packing";
                }
                else if (ansGridView1.CurrentCell.OwningColumn.Name == "Description")
                {
                    ActiveCell = "Desc";
                }
                if (ActiveCell == "Desc" && (ansGridView1.CurrentCell.OwningRow.Cells["Packing"].Value == null || ansGridView1.CurrentCell.OwningRow.Cells["Packing"].Value == ""))
                {
                    strCombo = "select distinct description from description order by description";
                    ansGridView1.CurrentCell.Value = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
                    if (ansGridView1.CurrentCell.Value != "")
                    {
                        DataTable dt = new DataTable();
                        Database.GetSqlData("select pack from Description where description='" + ansGridView1.CurrentCell.OwningRow.Cells["Description"].Value + "' order by pack", dt);
                        if (dt.Rows.Count == 1)
                        {
                            ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["Packing"].Value = dt.Rows[0]["pack"].ToString();
                            ansGridView1.CurrentCell = ansGridView1["rate", ansGridView1.CurrentCell.RowIndex];
                        }
                        else
                        {
                            ansGridView1.CurrentCell = ansGridView1["Packing", ansGridView1.CurrentCell.RowIndex];
                        }
                    }
                }
                else if (ActiveCell == "Packing" && ansGridView1.CurrentCell.OwningRow.Cells["Description"].Value != null && ansGridView1.CurrentCell.OwningRow.Cells["Description"].Value != "")
                {
                    strCombo = "select pack from Description where description='" + ansGridView1.CurrentCell.OwningRow.Cells["Description"].Value + "' order by pack";
                    ansGridView1.CurrentCell.Value = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
                    ansGridView1.CurrentCell = ansGridView1["rate", ansGridView1.CurrentCell.RowIndex];
                }
                else if (ActiveCell == "Desc" && ansGridView1.CurrentCell.OwningRow.Cells["Packing"].Value != null && ansGridView1.CurrentCell.OwningRow.Cells["Packing"].Value != "")
                {
                    strCombo = "select description from description where pack='" + ansGridView1.CurrentCell.OwningRow.Cells["Packing"].Value.ToString() + "' order by description";
                    ansGridView1.CurrentCell.Value = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
                    ansGridView1.CurrentCell = ansGridView1["rate", ansGridView1.CurrentCell.RowIndex];
                }
                else if (ActiveCell == "Packing" && (ansGridView1.CurrentCell.OwningRow.Cells["Description"].Value == null || ansGridView1.CurrentCell.OwningRow.Cells["Description"].Value == ""))
                {
                    strCombo = "select distinct(pack) from description order by Pack";
                    ansGridView1.CurrentCell.Value = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
                    ansGridView1.CurrentCell = ansGridView1["Description", ansGridView1.CurrentCell.RowIndex];
                }
            }
        }

        private void DisplaySetting()
        {
            if (Feature.Available("Item Name before Packing") == "Yes")
            {
                ansGridView1.Columns["Description"].DisplayIndex = 1;
                ansGridView1.Columns["Packing"].DisplayIndex = 2;
            }
            else
            {
                ansGridView1.Columns["Packing"].DisplayIndex = 1;
                ansGridView1.Columns["Description"].DisplayIndex = 2;
            }
            ansGridView1.Columns["rate"].DisplayIndex = 3;
            ansGridView1.Columns["SNo"].DisplayIndex = 0;
            ansGridView1.Columns["SNo"].ReadOnly = true;
            ansGridView1.Columns["Ac_id"].Visible = false;
            ansGridView1.Columns["Des_id"].Visible = false;
            ansGridView1.Columns["Packing"].ReadOnly = true;
            ansGridView1.Columns["Description"].ReadOnly = true;
        }

        private void frmCustSuppRate_Load(object sender, EventArgs e)
        {
            this.Size = this.MdiParent.Size;
            SideFill();
            DisplaySetting();
        }

        private void ansGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            ansGridView1.CurrentCell.Value = 0;
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
                }
                else
                {
                    int rindex = ansGridView1.CurrentRow.Index;
                    ansGridView1.Rows.RemoveAt(rindex);
                    for (int i = 0; i < ansGridView1.Rows.Count; i++)
                    {
                        ansGridView1.Rows[i].Cells["SNo"].Value = (i + 1);
                    }
                    return;
                }
            }
        }

        private void frmCustSuppRate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{tab}");
            }
            else if (e.Control && e.KeyCode == Keys.S)
            {
                if (validate() == true)
                {
                    try
                    {
                        Database.BeginTran();
                        save();
                        Database.CommitTran();
                        this.Close();
                        this.Dispose();
                        //textBox1.Text = "";
                        //ansGridView1.Rows.Clear();
                        //LoadData(gstr, gtyp);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Not Saved", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Database.RollbackTran();
                    }
                }
            }
            else if (e.KeyCode == Keys.Escape)
            {
                this.Close();
                this.Dispose();
            }
        }

        private void ansGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            ansGridView1.Rows[e.RowIndex].Cells["SNo"].Value = e.RowIndex + 1;
            if (ansGridView1.CurrentCell.OwningColumn.Name == "SNo")
            {
                SendKeys.Send("{right}");
            }
        }

        private void ansGridView1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (ansGridView1.Columns[e.ColumnIndex].Name == "Rate")
            {
                double dbl;
                if (double.TryParse(e.FormattedValue.ToString(), out dbl) == false)
                {
                    e.Cancel = true;
                }
                else if (double.TryParse(e.FormattedValue.ToString(), out dbl) == true)
                {
                    if (dbl < 0)
                    {
                        e.Cancel = true;
                    }
                }
            }
        }
    }
}
