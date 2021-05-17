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
    public partial class frm_bulkupdreb : Form
    {
        DataTable dt = new DataTable();
        BindingSource bs = new BindingSource();
        public frm_bulkupdreb()
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
                    save();
                    Database.CommitTran();
                    MessageBox.Show("Saved Successfully");
                }
                catch (Exception ex)
                {
                    Database.RollbackTran();
                    MessageBox.Show("Not Saved");
                }
            }

            else if (name == "quit")
            {
                this.Close();
                this.Dispose();
            }
        }

        private void frm_bulkupdreb_Load(object sender, EventArgs e)
        {
            SideFill();
            dt = new DataTable("CopyRates");
            string sql = "SELECT CopyRates.Cr_id, OTHER_1.Name AS Company, OTHER_2.Name AS Item, OTHER.Name AS PriceGrp, CopyRates.Pack,   PackCategory.Name AS PackingCat, dbo.CopyRates.rateto as RatetoUpd, dbo.CopyRates.Rebate2 as [Rebate] FROM CopyRates LEFT OUTER JOIN  PackCategory ON CopyRates.Pack_category_id = PackCategory.PackCat_id LEFT OUTER JOIN  Description ON CopyRates.Description = Description.Description LEFT OUTER JOIN  OTHER ON CopyRates.Group_id = OTHER.Oth_id LEFT OUTER JOIN  OTHER AS OTHER_2 ON CopyRates.Item_id = OTHER_2.Oth_id LEFT OUTER JOIN  OTHER AS OTHER_1 ON CopyRates.Company_id = OTHER_1.Oth_id GROUP BY CopyRates.Cr_id, OTHER_1.Name, OTHER_2.Name, OTHER.Name, CopyRates.Pack, PackCategory.Name, dbo.CopyRates.rateto, dbo.CopyRates.Rebate2 order by OTHER_1.Name, OTHER_2.Name, OTHER.Name, CopyRates.Pack, PackCategory.Name, dbo.CopyRates.rateto";
            Database.GetSqlData(sql, dt);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Columns["Rebate"].DataType.Name == "Decimal")
                {
                    dt.Rows[i]["Rebate"] = funs.IndianCurr(double.Parse(dt.Rows[i]["Rebate"].ToString()));
                }
            }
            ansGridView5.DataSource = dt;
            ansGridView5.Columns["Company"].ReadOnly = true;
            ansGridView5.Columns["Item"].ReadOnly = true;
            ansGridView5.Columns["PriceGrp"].ReadOnly = true;
            ansGridView5.Columns["RatetoUpd"].ReadOnly = true;
            ansGridView5.Columns["Rebate"].ReadOnly = false;
            ansGridView5.Columns["Pack"].ReadOnly = true;
            ansGridView5.Columns["PackingCat"].ReadOnly = true;
            ansGridView5.Columns["CR_id"].Visible = false;
        }
        private void save()
        {
            Database.SaveData(dt, "SELECT CopyRates.Cr_id,CopyRates.Pack,   dbo.CopyRates.rateto as RatetoUpd, dbo.CopyRates.Rebate2 as [Rebate] FROM CopyRates GROUP BY CopyRates.Cr_id,  CopyRates.Pack,  dbo.CopyRates.rateto, dbo.CopyRates.Rebate2");
        }
        private void filter()
        {
            String strTemp = textBox1.Text;
            strTemp = strTemp.Replace("%", "?");
            strTemp = strTemp.Replace("[", string.Empty);
            strTemp = strTemp.Replace("]", string.Empty);
            string strfilter = "";

          
                for (int i = 0; i < dt.Columns.Count - 1; i++)
                {
                    if (strfilter != "")
                    {
                        strfilter += " or ";
                    }
                    strfilter += "(" + dt.Columns[i].ColumnName + " like '*" + strTemp + "*' " + ")";
                }
          
            bs.Filter = null;
            bs.DataSource = dt;
            bs.Filter = strfilter;
        }

        private void frm_bulkupdreb_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
            {
                try
                {
                    Database.BeginTran();
                    save();
                    Database.CommitTran();
                    MessageBox.Show("Saved Successfully");
                }
                catch (Exception ex)
                {
                    Database.RollbackTran();
                    MessageBox.Show("Not Saved");
                } 
            }

            else if (e.KeyCode == Keys.Escape)
            {
                
                    this.Close();
                    this.Dispose();
                
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            filter();
        }
    }
}
