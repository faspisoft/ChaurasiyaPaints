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
    public partial class frmDescList : Form
    {
        String dtName;
        DataTable dtDescList;
              
        String strCombo;
       
        public frmDescList()
        {
            InitializeComponent();
        }

        public void LoadData(String str, String frmCaption, String wh)
        {
            dtName = "Description";
            dtDescList = new DataTable(dtName);
            Database.GetSqlData("SELECT DESCRIPTION.Des_id, DESCRIPTION.Description,  Description.Pack AS Pack, DESCRIPTION.Open_stock, DESCRIPTION.Open_stock2, OTHER_2.Name AS Company, OTHER.Name AS Item, OTHER_3.Name AS Color, OTHER_1.Name AS GroupName, TAXCATEGORY.Category_Name AS [Tax Category], DESCRIPTION.Company_id, DESCRIPTION.Item_id, DESCRIPTION.Col_id, DESCRIPTION.Group_id, DESCRIPTION.Tax_Cat_id, DESCRIPTION.Skucode, DESCRIPTION.Shortcode,DESCRIPTION.Pvalue, DESCRIPTION.Rate_Unit, DESCRIPTION.state,DESCRIPTION.stkMaintain  FROM ((((Description LEFT JOIN OTHER AS OTHER_2 ON DESCRIPTION.Company_id = OTHER_2.Oth_id) LEFT JOIN OTHER ON DESCRIPTION.Item_id = OTHER.Oth_id) LEFT JOIN OTHER AS OTHER_3 ON DESCRIPTION.Col_id = OTHER_3.Oth_id) LEFT JOIN OTHER AS OTHER_1 ON DESCRIPTION.Group_id = OTHER_1.Oth_id) LEFT JOIN TAXCATEGORY ON DESCRIPTION.Tax_Cat_id = TAXCATEGORY.Category_Id " + wh + " ORDER BY DESCRIPTION.Description, Description.Pvalue DESC", dtDescList);

            ansGridView1.DataSource = dtDescList;
            ansGridView1.Columns["Des_id"].Visible = false;
            ansGridView1.Columns["open_stock"].Visible = false;
            ansGridView1.Columns["Company_id"].Visible = false;
            ansGridView1.Columns["stkMaintain"].Visible = false;
            ansGridView1.Columns["Item_id"].Visible = false;
            ansGridView1.Columns["Col_id"].Visible = false;
            ansGridView1.Columns["Group_id"].Visible = false;
            ansGridView1.Columns["Tax_Cat_id"].Visible = false;
            ansGridView1.Columns["state"].Visible = false;
            ansGridView1.Columns["locationid"].Visible = false;
            ansGridView1.Columns["GroupName"].Visible = false;
            ansGridView1.Columns["Description"].ReadOnly = true;
            ansGridView1.Columns["Description"].Width = 123;
            ansGridView1.Columns["Pack"].ReadOnly = true;
            ansGridView1.Columns["Company"].ReadOnly = true;
            ansGridView1.Columns["Item"].ReadOnly = true;
            ansGridView1.Columns["Rate_Unit"].Visible = false;
            ansGridView1.Columns["Color"].ReadOnly = true;
            ansGridView1.Columns["Pvalue"].Visible = false;
            ansGridView1.Columns["Tax Category"].ReadOnly = true;
            ansGridView1.Columns["Skucode"].HeaderText = "SKU Code";
            ansGridView1.Columns["Shortcode"].HeaderText = "Short Code";
            ansGridView1.Columns["Rate_Unit"].HeaderText = "Unit";
            ansGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            if (Database.IsKacha == false)
            {
                ansGridView1.Columns["Open_stock2"].Visible = false;
            }
            else
            {
                ansGridView1.Columns["Open_stock2"].Visible = true;
            }            
            for (int i = 0; i < dtDescList.Rows.Count; i++)
            {
                ansGridView1.Rows[i].Cells["sno"].Value = (i + 1);
            }
            ansGridView1.CurrentCell = ansGridView1.Rows[0].Cells[2];

            foreach (DataGridViewColumn column in ansGridView1.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        private void ansGridView1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == ' ')
            {
                if (ansGridView1.CurrentCell.OwningColumn.Name == "Rate_Unit")
                {                  
                    DataTable dtcombo = new DataTable();
                    dtcombo.Columns.Add("Unit", typeof(string));

                    dtcombo.Columns["Unit"].ColumnName = "Unit";
                    dtcombo.Rows.Add();
                    dtcombo.Rows[0][0] = "Pis";

                    dtcombo.Rows.Add();
                    dtcombo.Rows[1][0] = "Kg.";

                    dtcombo.Rows.Add();
                    dtcombo.Rows[2][0] = "Lt.";

                    dtcombo.Rows.Add();
                    dtcombo.Rows[3][0] = "Unit";

                    dtcombo.Rows.Add();
                    dtcombo.Rows[4][0] = "Meter";

                    dtcombo.Rows.Add();
                    dtcombo.Rows[5][0] = "Sq. Meter";

                    dtcombo.Rows.Add();
                    dtcombo.Rows[6][0] = "Quintal";

                    ansGridView1.CurrentCell.Value = SelectCombo.ComboDt(this, dtcombo, 0);                   
                }
                if (ansGridView1.CurrentCell.OwningColumn.Name == "Company")
                {
                    strCombo = "select [name] from other where Type='SER14' order by [name]";
                    ansGridView1.CurrentCell.Value = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
                    ansGridView1.Rows[ansGridView1.CurrentRow.Index].Cells["Company_id"].Value = funs.Select_oth_id(ansGridView1.CurrentCell.Value.ToString());
                }
                if (ansGridView1.CurrentCell.OwningColumn.Name == "Item")
                {

                    strCombo = "select [name] from other where Type='SER15' order by [name]";
                    ansGridView1.CurrentCell.Value = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
                    ansGridView1.Rows[ansGridView1.CurrentRow.Index].Cells["Item_id"].Value = funs.Select_oth_id(ansGridView1.CurrentCell.Value.ToString());
                }
                if (ansGridView1.CurrentCell.OwningColumn.Name == "Color")
                {
                    strCombo = "select [name] from other where Type='SER18' order by [name]";
                    ansGridView1.CurrentCell.Value = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
                    ansGridView1.Rows[ansGridView1.CurrentRow.Index].Cells["Col_id"].Value = funs.Select_oth_id(ansGridView1.CurrentCell.Value.ToString());
                }
                if (ansGridView1.CurrentCell.OwningColumn.Name == "GroupName")
                {
                    strCombo = "select [name] from other where Type='SER16' order by [name]";
                    ansGridView1.CurrentCell.Value = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
                    ansGridView1.Rows[ansGridView1.CurrentRow.Index].Cells["Group_id"].Value = funs.Select_oth_id(ansGridView1.CurrentCell.Value.ToString());
                }
                if (ansGridView1.CurrentCell.OwningColumn.Name == "Tax Category")
                {
                    strCombo = "select category_name from taxcategory";
                    ansGridView1.CurrentCell.Value = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
                    ansGridView1.Rows[ansGridView1.CurrentRow.Index].Cells["Tax_Cat_id"].Value = funs.Select_tax_cat_id(ansGridView1.CurrentCell.Value.ToString());
                }
            }
        }

        private void frmDescList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
            {
                flowLayoutPanel1.Focus();

                for (int i = 0; i < ansGridView1.Rows.Count; i++)
                {
                    if (ansGridView1.Rows[i].Cells["Shortcode"].Value == "")
                    {
                        ansGridView1.Rows[i].Cells["Shortcode"].Value = "0";
                    }
                }
                try
                {
                    int Deleted = dtDescList.Select("", "", DataViewRowState.Deleted).Length;
                    int Modified = dtDescList.Select("", "", DataViewRowState.ModifiedCurrent).Length;
                    Database.SaveData(dtDescList, "SELECT Des_id,Description,Pack,Company_id,Item_id,Col_id,Group_id,Tax_Cat_id,Skucode,Shortcode,Open_stock,Open_stock2,state,Rate_Unit,Pvalue FROM DESCRIPTION");
                    funs.ShowBalloonTip("Saved", "saved successfully," + (Deleted + Modified) + " item(s) effected");
                    Master.UpdateDecription();
                    Master.UpdateDecriptionInfo();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                this.Dispose();
                this.Close();
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
                    this.Dispose();
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
                for (int i = 0; i < ansGridView1.Rows.Count; i++)
                {
                    if (ansGridView1.Rows[i].Cells["Shortcode"].Value == "")
                    {
                        ansGridView1.Rows[i].Cells["Shortcode"].Value = "0";
                    }
                }
                try
                {
                    int Deleted = dtDescList.Select("", "", DataViewRowState.Deleted).Length;
                    int Modified = dtDescList.Select("", "", DataViewRowState.ModifiedCurrent).Length;
                    Database.SaveData(dtDescList, "SELECT Des_id,Pack,Description,Company_id,Item_id,Col_id,Group_id,Tax_Cat_id,Skucode,Shortcode,Open_stock2,state,Rate_Unit,Pvalue FROM DESCRIPTION");
                    funs.ShowBalloonTip("Saved", "saved successfully," + (Deleted + Modified) + " item(s) effected");
                    Master.UpdateDecription();
                    Master.UpdateDecriptionInfo();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                this.Dispose();
                this.Close();
            }
            else if (name == "quit")
            {
                this.Close();
                this.Dispose();
            }
        }

        private void frmDescList_Load(object sender, EventArgs e)
        {
            SideFill();
            this.Size = this.MdiParent.Size;
        }

        private void ansGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            ansGridView1.CurrentCell.Value = 0;
        }

        private void ansGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                
                string desid = ansGridView1.Rows[ansGridView1.CurrentRow.Index].Cells["Des_id"].Value.ToString();
                if (Database.GetScalarInt("select count(*) from voucherdet where Des_ac_id='" + desid + "' ") == 0)
                {
                    if (ansGridView1.CurrentRow.Index == ansGridView1.Rows.Count - 1)
                    {
                        for (int i = 1; i < ansGridView1.Columns.Count; i++)
                        {
                            ansGridView1.Rows[ansGridView1.CurrentRow.Index].Cells[i].Value = "";
                        }
                        return;
                    }
                    else
                    {
                        ansGridView1.Rows.RemoveAt(ansGridView1.CurrentRow.Index);
                        for (int i = 0; i < ansGridView1.Rows.Count; i++)
                        {
                            ansGridView1.Rows[i].Cells["sno"].Value = (i + 1);
                        }
                        return;
                    }
                }
            }
        }

        private void ansGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (ansGridView1.CurrentCell.OwningColumn.Name == "sno")
            {
                SendKeys.Send("{tab}");
                this.Activate();
            }
        }

        private void ansGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["state"].Value = "Modified";
        }

    }
}
