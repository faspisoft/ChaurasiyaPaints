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
    public partial class frmEditGroup : Form
    {
        DataTable dtEditGroup;
        String dtName;
         

        public frmEditGroup()
        {
            InitializeComponent();
        }

        public void LoadData(String str, String frmCaption,String wh)
        { 
            dtName = "Description";
            dtEditGroup = new DataTable(dtName);
            Database.GetSqlData("SELECT DESCRIPTION.Description, Description.Pack AS Pack, DESCRIPTION.Des_id,  DESCRIPTION.Retail, DESCRIPTION.Wholesale, DESCRIPTION.Company_id, DESCRIPTION.Item_id, DESCRIPTION.Col_id, DESCRIPTION.Group_id, DESCRIPTION.Tax_Cat_id, DESCRIPTION.Purchase_rate, DESCRIPTION.Open_stock, DESCRIPTION.Wlavel, DESCRIPTION.[Commission%], DESCRIPTION.[Commission@], DESCRIPTION.ShortCode,DESCRIPTION.Mark, DESCRIPTION.box_quantity, DESCRIPTION.weight, DESCRIPTION.discount_qty, DESCRIPTION.Rate_X, DESCRIPTION.Rate_Y, DESCRIPTION.Rate_Z, DESCRIPTION.Skucode, DESCRIPTION.Open_stock2, DESCRIPTION.MRP, DESCRIPTION.state,DESCRIPTION.Rate_Unit,DESCRIPTION.Pvalue,DESCRIPTION.remarkreq,DESCRIPTION.stkMaintain FROM DESCRIPTION  " + wh + " ORDER BY DESCRIPTION.Description, Description.Pvalue DESC ", dtEditGroup);
            //Database.GetSqlData("SELECT  DESCRIPTION.*, PACKING.Name as Pack FROM DESCRIPTION INNER JOIN PACKING ON DESCRIPTION.Pack_id = PACKING.Pack_id" + wh, dtEditGroup);
            ansGridView1.DataSource = dtEditGroup;
            ansGridView1.CurrentCell = ansGridView1.Rows[0].Cells[0];
            ansGridView1.Columns["Pack"].Visible = false;
//ansGridView1.Columns["Pack_id"].Visible = false;
         
            ansGridView1.Columns["Des_id"].Visible = false;
            ansGridView1.Columns["Company_id"].Visible = false;
            ansGridView1.Columns["remarkreq"].Visible = false;
            ansGridView1.Columns["stkMaintain"].Visible = false;
            ansGridView1.Columns["Item_id"].Visible = false;
            ansGridView1.Columns["Rate_Unit"].Visible = false;
            ansGridView1.Columns["Pvalue"].Visible = false;
            ansGridView1.Columns["Group_id"].Visible = false;
            ansGridView1.Columns["Col_id"].Visible = false;
            ansGridView1.Columns["tax_cat_id"].Visible = false;
            ansGridView1.Columns["Open_Stock"].Visible = false;
            ansGridView1.Columns["Open_Stock2"].Visible = false;
            ansGridView1.Columns["Wlavel"].Visible = false;
            ansGridView1.Columns["Commission@"].Visible = false;
            ansGridView1.Columns["Commission%"].Visible = false;
            ansGridView1.Columns["ShortCode"].Visible = false;
            ansGridView1.Columns["Mark"].Visible = false;
            ansGridView1.Columns["locationid"].Visible = false;
            ansGridView1.Columns["box_quantity"].Visible = false;
            ansGridView1.Columns["weight"].Visible = false;
            ansGridView1.Columns["discount_qty"].Visible = false;
            ansGridView1.Columns["Skucode"].Visible = false;
            ansGridView1.Columns["state"].Visible = false;
            ansGridView1.Columns["Description"].ReadOnly = true;
            ansGridView1.Columns["packnm"].ReadOnly = true;
            ansGridView1.Columns["sno"].DisplayIndex = 0;
            ansGridView1.Columns["Description"].DisplayIndex = 1;
            ansGridView1.Columns["packnm"].DisplayIndex = 2;
            ansGridView1.Columns["packnm"].DataPropertyName = "Pack";
            ansGridView1.Columns["Purchase_rate"].DisplayIndex = 3;
            ansGridView1.Columns["Wholesale"].DisplayIndex = 4;
            ansGridView1.Columns["Description"].Width = 250;
            ansGridView1.Columns["Purchase_rate"].Width = 71;
            ansGridView1.Columns["Retail"].Width = 71;
            ansGridView1.Columns["Wholesale"].Width = 71;
            ansGridView1.Columns["Rate_Z"].Width = 71;
            ansGridView1.Columns["Rate_X"].Width = 71;
            ansGridView1.Columns["Rate_Y"].Width = 71;
            ansGridView1.Columns["sno"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            ansGridView1.Columns["Purchase_rate"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            ansGridView1.Columns["Retail"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            ansGridView1.Columns["Wholesale"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            ansGridView1.Columns["Rate_X"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            ansGridView1.Columns["Rate_Y"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            ansGridView1.Columns["Rate_Z"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            ansGridView1.Columns["Rate_X"].HeaderText = "Rate-X";
            ansGridView1.Columns["Rate_Y"].HeaderText = "Rate-Y";
            ansGridView1.Columns["Rate_Z"].HeaderText = "Rate-Z";

            for (int i = 0; i < dtEditGroup.Rows.Count; i++)
            {
                ansGridView1.Rows[i].Cells["sno"].Value = (i + 1);
            }
        }


        private void button5_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }


        private void frmEditGroup_Load(object sender, EventArgs e)
        {
           //Dongle.lockOk();
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
                    int Deleted = dtEditGroup.Select("", "", DataViewRowState.Deleted).Length;
                    int Modefied = dtEditGroup.Select("", "", DataViewRowState.ModifiedCurrent).Length;
                    Database.SaveData(dtEditGroup);
                    Master.UpdateDecription();
                    Master.UpdateDecriptionInfo();

                    // MessageBox.Show("saved successfully, " + (Deleted + Modefied) + " item(s) effected");

                    funs.ShowBalloonTip("Saved", "saved successfully, " + (Deleted + Modefied) + " item(s) effected");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                this.Dispose();
            }


            if (name == "quit")
            {
                this.Close();
                this.Dispose();
            }


        }




        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                int Deleted = dtEditGroup.Select("", "", DataViewRowState.Deleted).Length;
                int Modefied = dtEditGroup.Select("", "", DataViewRowState.ModifiedCurrent).Length;
                Database.SaveData(dtEditGroup);
                Master.UpdateDecription();
                Master.UpdateDecriptionInfo();

               // MessageBox.Show("saved successfully, " + (Deleted + Modefied) + " item(s) effected");
                
                funs.ShowBalloonTip("Saved", "saved successfully, " + (Deleted + Modefied) + " item(s) effected");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            this.Dispose();
        }

        private void ansGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            ansGridView1.CurrentCell.Value = 0;
        }

        private void frmEditGroup_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
            {
                try
                {
                    int Deleted = dtEditGroup.Select("", "", DataViewRowState.Deleted).Length;
                    int Modefied = dtEditGroup.Select("", "", DataViewRowState.ModifiedCurrent).Length;
                    Database.SaveData(dtEditGroup);
                    Master.UpdateDecription();
                    Master.UpdateDecriptionInfo();

                    // MessageBox.Show("saved successfully, " + (Deleted + Modefied) + " item(s) effected");

                    funs.ShowBalloonTip("Saved", "saved successfully, " + (Deleted + Modefied) + " item(s) effected");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                this.Dispose();
            }

            
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

        private void ansGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (ansGridView1.CurrentCell.OwningColumn.Name == "sno")
            {
                SendKeys.Send("{right}");
                this.Activate();
            }
        }

        private void ansGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["state"].Value = "Modified";
        }

        private void ansGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
