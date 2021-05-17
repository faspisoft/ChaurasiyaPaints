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
    public partial class frm_billprint : Form
    {
        string gstr = "";
        string gFrmCaption = "";
        DataTable dt;
        DataTable dtvou = new DataTable();

        public frm_billprint()
        {
            InitializeComponent();
            dateTimePicker1.CustomFormat = Database.dformat;
            dateTimePicker1.Value = Database.ldate;
            dateTimePicker1.MinDate = Database.stDate;
            dateTimePicker1.MaxDate = Database.ldate;
            dateTimePicker2.CustomFormat = Database.dformat;
            dateTimePicker2.MinDate = Database.stDate;
            dateTimePicker2.MaxDate = Database.ldate;            
            dateTimePicker2.Value = Database.ldate;
        }

        private void frm_billprint_Load(object sender, EventArgs e)
        {
            this.Size = this.MdiParent.Size;
            this.WindowState = FormWindowState.Maximized;
            SideFill();
            LoadData(gstr, "Print Vouchers");
            for (int i = 0; i < ansGridView5.Rows.Count; i++)
            {
                ansGridView5.Rows[i].Cells["select"].Value = true;
            }
        }

        public void LoadData(string str, string FrmCaption)
        {
            gstr = str;
            gFrmCaption = FrmCaption;

            string sql = "";
            string accstr = "";
            if (textBox14.Text != "")
            {
                accstr = " AND (ACCOUNT.Name = '" + textBox14.Text + "')";
            }
            if (Database.DatabaseType == "access")
            {
                if (Database.IsKacha == false)
                {
                    sql = "SELECT Format$([Voucherinfo].[Vdate],'dd-mmm-yyyy') AS VDate, VOUCHERTYPE.Name, ACCOUNT.Name AS AccName, VOUCHERINFO.Vnumber, VOUCHERINFO.Totalamount AS Amount, VOUCHERINFO.Vi_id FROM (VOUCHERINFO LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id) LEFT JOIN ACCOUNT ON VOUCHERINFO.Ac_id = ACCOUNT.Ac_id WHERE (((VOUCHERINFO.Vdate)>=#" + dateTimePicker1.Value.Date.ToString(Database.dformat) + "# And (VOUCHERINFO.Vdate)<=#" + dateTimePicker2.Value.Date.ToString(Database.dformat) + "#) AND ((VOUCHERTYPE.Name)='" + gstr + "') AND ((VOUCHERTYPE.A)=True)) ORDER BY VOUCHERINFO.Vdate DESC,VOUCHERTYPE.Name DESC, VOUCHERINFO.Vnumber DESC";
                }
                else
                {
                    sql = "SELECT Format$([Voucherinfo].[Vdate],'dd-mmm-yyyy') AS VDate, VOUCHERTYPE.Name, ACCOUNT.Name AS AccName, VOUCHERINFO.Vnumber,VOUCHERINFO.Totalamount AS Amount,VOUCHERINFO.Vi_id FROM (VOUCHERINFO LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id) LEFT JOIN ACCOUNT ON VOUCHERINFO.Ac_id = ACCOUNT.Ac_id WHERE (((VOUCHERINFO.Vdate)>=#" + dateTimePicker1.Value.Date.ToString(Database.dformat) + "# And (VOUCHERINFO.Vdate)<=#" + dateTimePicker2.Value.Date.ToString(Database.dformat) + "#) AND ((VOUCHERTYPE.Name)='" + gstr + "') AND ((VOUCHERTYPE.B)=True)) ORDER BY VOUCHERINFO.Vdate DESC,VOUCHERTYPE.Name DESC, VOUCHERINFO.Vnumber DESC";
                }
            }
            else
            {
                if (Database.IsKacha == false)
                {
                    sql = "SELECT  CONVERT(nvarchar,Voucherinfo.Vdate, 106) AS VDate, VOUCHERTYPE.Name,ACCOUNT.Name AS AccName, VOUCHERINFO.Vnumber,VOUCHERINFO.Totalamount AS Amount,VOUCHERINFO.Vi_id FROM VOUCHERINFO LEFT OUTER JOIN ACCOUNT ON VOUCHERINFO.Ac_id = ACCOUNT.Ac_id LEFT OUTER JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id WHERE (VOUCHERINFO.Vdate >= '" + dateTimePicker1.Value.Date.ToString(Database.dformat) + "') AND (VOUCHERINFO.Vdate <= '" + dateTimePicker2.Value.Date.ToString(Database.dformat) + "') AND (VOUCHERTYPE.Name = '" + gstr + "') AND (VOUCHERTYPE.A = 'true') AND (VOUCHERINFO.Branch_id = '" + Database.BranchId + "') " + accstr + " ORDER BY VOUCHERINFO.Vdate DESC,VOUCHERTYPE.Name DESC, VOUCHERINFO.Vnumber DESC";
                }
                else
                {
                    sql = "SELECT  CONVERT(nvarchar,Voucherinfo.Vdate, 106) AS VDate, VOUCHERTYPE.Name,ACCOUNT.Name AS AccName, VOUCHERINFO.Vnumber,VOUCHERINFO.Totalamount AS Amount,VOUCHERINFO.Vi_id FROM VOUCHERINFO LEFT OUTER JOIN ACCOUNT ON VOUCHERINFO.Ac_id = ACCOUNT.Ac_id LEFT OUTER JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id WHERE (VOUCHERINFO.Vdate >= '" + dateTimePicker1.Value.Date.ToString(Database.dformat) + "') AND (VOUCHERINFO.Vdate <= '" + dateTimePicker2.Value.Date.ToString(Database.dformat) + "') AND (VOUCHERTYPE.Name = '" + gstr + "') AND (VOUCHERTYPE.B = 'true') AND (VOUCHERINFO.Branch_id = '" + Database.BranchId + "') " + accstr + " ORDER BY VOUCHERINFO.Vdate DESC,VOUCHERTYPE.Name DESC, VOUCHERINFO.Vnumber DESC";
                    //sql = "SELECT  CONVERT(nvarchar,Voucherinfo.Vdate, 106) AS VDate, VOUCHERTYPE.Name,ACCOUNT.Name AS AccName, VOUCHERINFO.Vnumber,VOUCHERINFO.Totalamount AS Amount,VOUCHERINFO.Vi_id FROM VOUCHERINFO LEFT OUTER JOIN ACCOUNT ON VOUCHERINFO.Ac_id = ACCOUNT.Ac_id LEFT OUTER JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id WHERE (VOUCHERINFO.Vdate >= '" + dateTimePicker1.Value.Date.ToString(Database.dformat) + "') AND (VOUCHERINFO.Vdate <= '" + dateTimePicker2.Value.Date.ToString(Database.dformat) + "') AND (VOUCHERTYPE.Name = '" + gstr + "') AND (VOUCHERTYPE.B = 'true') ORDER BY VOUCHERINFO.Vdate,VOUCHERTYPE.Name, VOUCHERINFO.Vnumber DESC";
                }
            }

            Database.GetSqlData(sql, dtvou);
            SideFill();
            
            ansGridView5.Columns["select"].DataGridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            ansGridView5.Columns["select"].DisplayIndex = ansGridView5.Columns.Count - 1;          
            ansGridView5.Columns["Vi_id"].Visible = false;

            ansGridView5.Rows.Clear();
            for (int i = 0; i < dtvou.Rows.Count; i++)
            {
                ansGridView5.Rows.Add();
                ansGridView5.Rows[i].Cells["Vdate"].Value = dtvou.Rows[i]["Vdate"].ToString();
                ansGridView5.Rows[i].Cells["Vname"].Value = dtvou.Rows[i]["name"].ToString();
                ansGridView5.Rows[i].Cells["AccName"].Value = dtvou.Rows[i]["AccName"].ToString();
                ansGridView5.Rows[i].Cells["Vnumber"].Value = dtvou.Rows[i]["Vnumber"].ToString();
                ansGridView5.Rows[i].Cells["Amount"].Value =funs.DecimalPoint(double.Parse(dtvou.Rows[i]["Amount"].ToString()),2);            
                ansGridView5.Rows[i].Cells["Vi_id"].Value = dtvou.Rows[i]["Vi_id"].ToString();                
                ansGridView5.Rows[i].Cells["select"].Value = true;
            }
            this.Text = gFrmCaption;
        }

        private void SideFill()
        {
            flowLayoutPanel1.Controls.Clear();
            DataTable dtsidefill = new DataTable();
            dtsidefill.Columns.Add("Name", typeof(string));
            dtsidefill.Columns.Add("DisplayName", typeof(string));
            dtsidefill.Columns.Add("ShortcutKey", typeof(string));
            dtsidefill.Columns.Add("Visible", typeof(bool));
            
            //createnew
            dtsidefill.Rows.Add();
            dtsidefill.Rows[0]["Name"] = "print";
            dtsidefill.Rows[0]["DisplayName"] = "Print";
            dtsidefill.Rows[0]["ShortcutKey"] = "^P";
            if (ansGridView5.Rows.Count == 0)
            {
                dtsidefill.Rows[0]["Visible"] = false;
            }
            else
            {
                dtsidefill.Rows[0]["Visible"] = true;
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

            if (name == "print")
            {
                frm_printcopy frm = new frm_printcopy("Print", "", funs.Select_vt_id_vid(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString()));
                frm.directmode = "multipleprint";
                frm.ShowDialog();

                OtherReport rpt = new OtherReport();
                for (int i = 0; i < ansGridView5.Rows.Count; i++)
                {
                    if (ansGridView5.Rows[i].Cells["select"].Value != null)
                    {
                        if (bool.Parse(ansGridView5.Rows[i].Cells["select"].Value.ToString()) == true)
                        {
                            String[] print_option = frm.copyname1.Split(';');

                            for (int j = 0; j < print_option.Length; j++)
                            {
                                if (print_option[j] != "")
                                {
                                    rpt.voucherprint(this, funs.Select_vt_id_vid(ansGridView5.Rows[i].Cells["Vi_id"].Value.ToString()), ansGridView5.Rows[i].Cells["Vi_id"].Value.ToString(), print_option[j], true, "Print");
                                }
                            }
                        }
                    }
                }

                MessageBox.Show("Done..");
                this.Close();
                this.Dispose();
            }
            else if (name == "quit")
            {
                this.Close();
                this.Dispose();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            LoadData(gstr, gFrmCaption);
            SideFill();
        }

        private void frm_billprint_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
                this.Dispose();
            }
            else if (e.Control && e.KeyCode == Keys.P)
            {
                frm_printcopy frm = new frm_printcopy("Print", "", funs.Select_vt_id_vid(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString()));
                frm.directmode = "multipleprint";
                frm.ShowDialog();

                OtherReport rpt = new OtherReport();
                for (int i = 0; i < ansGridView5.Rows.Count; i++)
                {
                    if (bool.Parse(ansGridView5.Rows[i].Cells["select"].Value.ToString()) == true)
                    {
                        String[] print_option = frm.copyname1.Split(';');

                        for (int j = 0; j < print_option.Length; j++)
                        {
                            if (print_option[j] != "")
                            {
                                rpt.voucherprint(this, funs.Select_vt_id_vid(ansGridView5.Rows[i].Cells["Vi_id"].Value.ToString()), ansGridView5.Rows[i].Cells["Vi_id"].Value.ToString(), print_option[j], true, "Print");
                            }
                        }
                    }
                }

                MessageBox.Show("Done..");
                this.Close();
                this.Dispose();
            }
        }

        private void textBox14_KeyPress(object sender, KeyPressEventArgs e)
        {
            string strCombo = funs.GetStrCombonew(" (Path LIKE '1;39;%') or (Path LIKE '1;38;%')  OR  (Path LIKE '1;3;%')   or   (Path LIKE '8;40;%' and  AllowPS=" + access_sql.Singlequote + "true" + access_sql.Singlequote + ")  or   (Path LIKE '8;39;%' and  AllowPS=" + access_sql.Singlequote + "true" + access_sql.Singlequote + ") ", "   Status=" + access_sql.Singlequote + "true" + access_sql.Singlequote + " and Branch_id='" + Database.BranchId + "' ");
            textBox14.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 1);

        }
    }
}
