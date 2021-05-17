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
    public partial class frm_Missprint : Form
    {
        DataTable dtMissTint;
        String dtName;
        String strCombo;
        
        public frm_Missprint()
        {
            InitializeComponent();
        }

        public void LoadData()
        {
            dtName = "Misstint";
            dtMissTint = new DataTable(dtName);
           
            Database.GetSqlData("SELECT DESCRIPTION.Des_id, Description.Pack AS Packing,DESCRIPTION.Description, Misstint.Note,Misstint.Shade FROM (Misstint INNER JOIN DESCRIPTION ON Misstint.Des_id = DESCRIPTION.Des_id)  order by description", dtMissTint);
          ansGridView1.DataSource = dtMissTint;
           
            ansGridView1.Columns["SNo"].ReadOnly = true;
            ansGridView1.Columns["Des_id"].Visible = false;
          
            ansGridView1.Columns["SNo"].DisplayIndex = 0;
            ansGridView1.Columns["Packing"].DisplayIndex = 1;
            ansGridView1.Columns["Description"].DisplayIndex = 2;
            ansGridView1.Columns["Shade"].DisplayIndex = 3;
            ansGridView1.Columns["Note"].DisplayIndex = 4;

            ansGridView1.Columns["Packing"].ReadOnly = true;
            ansGridView1.Columns["Description"].ReadOnly = true;
            
            ansGridView1.Columns["Sno"].Width = 40;
            ansGridView1.Columns["Packing"].Width = 50;
            ansGridView1.Columns["Description"].Width = 250;
            ansGridView1.Columns["Shade"].Width = 80;
            ansGridView1.Columns["Note"].Width = 200;

            ansGridView1.CurrentCell = ansGridView1.Rows[0].Cells[0];
            ansGridView1.CurrentCell = ansGridView1.Rows[0].Cells[0];
            ansGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
          
            for (int i = 0; i < dtMissTint.Rows.Count;  i++)
            {
                ansGridView1.Rows[i].Cells["SNo"].Value = (i + 1);
            }
        }

        private void save()
        {
            for (int i = 0; i < ansGridView1.Rows.Count - 1; i++)
            {
                DataTable dtDescId = new DataTable();
                Database.GetSqlData("SELECT DESCRIPTION.Des_id FROM DESCRIPTION  WHERE (((DESCRIPTION.Description)='" + ansGridView1.Rows[i].Cells["Description"].Value + "') AND ((Pack)='" + ansGridView1.Rows[i].Cells["Packing"].Value.ToString() + "')) order by description", dtDescId);
                if (dtDescId.Rows.Count > 0)
                {
                    dtMissTint.Rows[i]["Des_id"] = dtDescId.Rows[0]["Des_id"];
                    dtMissTint.Rows[i]["LocationId"] = Database.LocationId;
                }
            }
            Database.SaveData(dtMissTint);            
            funs.ShowBalloonTip("Saved", "Saved Successfully");
            this.Close();
            this.Dispose();
        }

        private bool validate()
        {
            for (int i = 0; i < ansGridView1.Rows.Count - 1; i++)
            {              
                if (ansGridView1.Rows[i].Cells["Note"].Value == null)
                {
                    MessageBox.Show("Enter some value");
                    return false;
                }                
            }
            return true;
        }

        private void ansGridView1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsLetter(e.KeyChar) || char.IsNumber(e.KeyChar) || e.KeyChar == ' ')
            {
                if (ansGridView1.CurrentCell.OwningColumn.Name == "Packing")
                {
                    strCombo = "select Distinct [pack] from Description order by Pack";
                    ansGridView1.CurrentCell.Value = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
                }
                if (ansGridView1.CurrentCell.OwningColumn.Name == "Description")
                {
                    strCombo = "select description from description where pack='" + ansGridView1.CurrentCell.OwningRow.Cells["Packing"].Value.ToString() + "' order by description";
                    ansGridView1.CurrentCell.Value = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
                }
            }
        }

        private void ansGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            ansGridView1.CurrentCell.Value = 0;
        }

        private void ansGridView1_KeyDown(object sender, KeyEventArgs e)
        {
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

        private void ansGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            ansGridView1.Rows[e.RowIndex].Cells["SNo"].Value = e.RowIndex + 1;
            if (ansGridView1.CurrentCell.OwningColumn.Name == "SNo")
            {
                SendKeys.Send("{right}");
                this.Activate();
            }
        }

        private void ansGridView1_DataError_1(object sender, DataGridViewDataErrorEventArgs e)
        {
            ansGridView1.CurrentCell.Value = 0;
        }

        private void ansGridView1_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            if (char.IsLetter(e.KeyChar) || char.IsNumber(e.KeyChar) || e.KeyChar == ' ')
            {
                if (ansGridView1.CurrentCell.OwningColumn.Name == "Packing")
                {
                    strCombo = "select Distinct [pack] from Description order by Pack";
                    ansGridView1.CurrentCell.Value = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
                }
                if (ansGridView1.CurrentCell.OwningColumn.Name == "Description")
                {
                    strCombo = "select description from description where pack='" +ansGridView1.CurrentCell.OwningRow.Cells["Packing"].Value.ToString()+"' ";
                    ansGridView1.CurrentCell.Value = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
                }
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
                    save();
                }
            }

            else if (name == "quit")
            {
                this.Close();
                this.Dispose();
            }
        }

        private void frm_Missprint_Load(object sender, EventArgs e)
        {
            this.Size = this.MdiParent.Size;
            SideFill();
        }

        private void Button2_Click_1(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
        }

        private void frm_Missprint_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
            {
                if (validate() == true)
                {
                    save();
                }
            }
            else if (e.KeyCode == Keys.Escape)
            {
                this.Close();
                this.Dispose();
            }
        }
    }
}

