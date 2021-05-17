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
    public partial class frmSmartDocumentFinder : Form
    {        
        public frmSmartDocumentFinder()
        {
            InitializeComponent();
        }

        private void frmSmartDocumentFinder_Load(object sender, EventArgs e)
        {
            dateTimePicker1.CustomFormat = Database.dformat;
            dateTimePicker2.CustomFormat = Database.dformat;
            if (Database.IsKacha == false)
            {
                Database.FillList(listBox1, "select [name] from vouchertype where not (Type='Pending' or Type='Report' or Type='Temp') and active=" + access_sql.Singlequote + "true" + access_sql.Singlequote + " and A=" + access_sql.Singlequote + "true" + access_sql.Singlequote + " order by [name]");
            }
            else
            {
                Database.FillList(listBox1, "select [name] from vouchertype where not (Type='Pending' or Type='Report' or Type='Temp') and active=" + access_sql.Singlequote + "true" + access_sql.Singlequote + " and B=" + access_sql.Singlequote + "True" + access_sql.Singlequote + " order by [name]");
            }

            Database.FillList(listBox2, "select [name] from account WHERE not (ACCOUNT.Act_id = 'SER2') order by [name]");
            listBox3.Text = "<None>";
            listBox4.Text = "<None>";
            this.Size = this.MdiParent.Size;
            SideFill();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        private void listBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox4.Text == "Between")
            {
                label2.Visible = true;
                textBox1.Visible = true;
                textBox2.Visible = true;
            }
            else if (listBox4.Text == "Equal To (=)" || listBox4.Text == "Greater Than (>)" || listBox4.Text == "Greater than Equal To (>=)" || listBox4.Text == "Less Than (<)" || listBox4.Text == "Less Than Equal To (<=)")
            {
                label2.Visible = false;
                textBox1.Visible = true;
                textBox2.Visible = false;
            }
            else
            {
                label2.Visible = false;
                textBox1.Visible = false;
                textBox2.Visible = false;
            }
        }

        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox3.Text == "Between")
            {
                label1.Visible = true;
                dateTimePicker1.Visible = true;
                dateTimePicker2.Visible = true;
            }
            else if (listBox3.Text == "Equal To (=)" || listBox3.Text == "Greater Than (>)" || listBox3.Text == "Greater than Equal To (>=)" || listBox3.Text == "Less Than (<)" || listBox3.Text == "Less Than Equal To (<=)")
            {
                label1.Visible = false;
                dateTimePicker1.Visible = true;
                dateTimePicker2.Visible = false;
            }
            else
            {
                label1.Visible = false;
                dateTimePicker1.Visible = false;
                dateTimePicker2.Visible = false;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                listBox1.Enabled = true;
            }
            else
            {
                listBox1.Enabled = false;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true)
            {
                listBox2.Enabled = true;
            }
            else
            {
                listBox2.Enabled = false;
            }
        }


        private String VoucherAmtQuery()
        {
            String str = "";
            if (Database.IsKacha == false)
            {
                str = "SELECT qu1.Short, qu1.Vnumber, qu1.Vdate, qu1.Cr, qu1.Vt_id, qu1.Vi_id FROM (SELECT VOUCHERTYPE.Short, VOUCHERTYPE.Vt_id, VOUCHERINFO.Vnumber, VOUCHERINFO.Vdate, VOUCHERINFO.Vi_id, VOUCHERINFO.Totalamount as Cr FROM VOUCHERINFO INNER JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id  WHERE (((VOUCHERTYPE.A)="+ access_sql.Singlequote+"True"+ access_sql.Singlequote+")))  AS qu1";
            }
            else 
            {
                str = "SELECT qu1.Short, qu1.Vnumber, qu1.Vdate, qu1.Cr, qu1.Vt_id, qu1.Vi_id FROM (SELECT VOUCHERTYPE.Short, VOUCHERTYPE.Vt_id, VOUCHERINFO.Vnumber, VOUCHERINFO.Vdate, VOUCHERINFO.Vi_id, VOUCHERINFO.Totalamount as Cr FROM VOUCHERINFO INNER JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id  WHERE (((VOUCHERTYPE.B)=" + access_sql.Singlequote + "True" + access_sql.Singlequote + ")))  AS qu1";
            }
            return str;
        }

        private String AccQuery()
        {
            String str = "";
            if (Database.IsKacha == false)
            {
                str = "SELECT qu1.Short, qu1.Vnumber, qu1.Vdate, qu1.Name, qu1.Cr, qu1.Vt_id, qu1.Vi_id FROM (SELECT JOURNAL.Vdate, VOUCHERTYPE.Short, VOUCHERTYPE.Vt_id, JOURNAL.Ac_id, ACCOUNT.Name, VOUCHERINFO.Vnumber, VOUCHERINFO.Vi_id, JOURNAL.Cr FROM ((JOURNAL INNER JOIN ACCOUNT ON JOURNAL.Ac_id = ACCOUNT.Ac_id) INNER JOIN VOUCHERINFO ON JOURNAL.Vi_id = VOUCHERINFO.Vi_id) INNER JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id WHERE (((JOURNAL.Cr)<>0) AND ((VOUCHERTYPE.A)="+ access_sql.Singlequote+"True"+ access_sql.Singlequote +")) union ALL SELECT JOURNAL.Vdate, VOUCHERTYPE.Short, VOUCHERTYPE.Vt_id, JOURNAL.Ac_id, ACCOUNT.Name, VOUCHERINFO.Vnumber, VOUCHERINFO.Vi_id, JOURNAL.Dr FROM ((JOURNAL INNER JOIN ACCOUNT ON JOURNAL.Ac_id = ACCOUNT.Ac_id) INNER JOIN VOUCHERINFO ON JOURNAL.Vi_id = VOUCHERINFO.Vi_id) INNER JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id WHERE (((JOURNAL.Dr)<>0) AND ((VOUCHERTYPE.A)="+ access_sql.Singlequote+"True"+ access_sql.Singlequote+")) ) AS qu1";
            }
            else
            {
                str = "SELECT qu1.Short, qu1.Vnumber, qu1.Vdate, qu1.Name, qu1.Cr, qu1.Vt_id, qu1.Vi_id FROM (SELECT JOURNAL.Vdate, VOUCHERTYPE.Short, VOUCHERTYPE.Vt_id, JOURNAL.Ac_id, ACCOUNT.Name, VOUCHERINFO.Vnumber, VOUCHERINFO.Vi_id, JOURNAL.Cr FROM ((JOURNAL INNER JOIN ACCOUNT ON JOURNAL.Ac_id = ACCOUNT.Ac_id) INNER JOIN VOUCHERINFO ON JOURNAL.Vi_id = VOUCHERINFO.Vi_id) INNER JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id WHERE (((JOURNAL.Cr)<>0) AND ((VOUCHERTYPE.B)="+ access_sql.Singlequote+"True"+ access_sql.Singlequote +")) union ALL SELECT JOURNAL.Vdate, VOUCHERTYPE.Short, VOUCHERTYPE.Vt_id, JOURNAL.Ac_id, ACCOUNT.Name, VOUCHERINFO.Vnumber, VOUCHERINFO.Vi_id, JOURNAL.Dr FROM ((JOURNAL INNER JOIN ACCOUNT ON JOURNAL.Ac_id = ACCOUNT.Ac_id) INNER JOIN VOUCHERINFO ON JOURNAL.Vi_id = VOUCHERINFO.Vi_id) INNER JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id WHERE (((JOURNAL.Dr)<>0) AND ((VOUCHERTYPE.B)="+access_sql.Singlequote +"True"+ access_sql.Singlequote+")) ) AS qu1";
            }
            return str;
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
            dtsidefill.Rows[0]["Name"] = "ok";
            dtsidefill.Rows[0]["DisplayName"] = "Ok";
            dtsidefill.Rows[0]["ShortcutKey"] = "";
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

            if (name == "ok")
            {
                ok_save();
            }


            if (name == "quit")
            {
                this.Close();
                this.Dispose();
            }


        }


        private String DateQuery()
        {
            String str = "";
            if (listBox3.Text == "Equal To (=)")
            {
                str = "qu1.Vdate="+access_sql.Hash + dateTimePicker1.Value.Date + access_sql.Hash+" ";
            }
            else if (listBox3.Text == "Greater Than (>)")
            {
                str = "qu1.Vdate>"+access_sql.Hash + dateTimePicker1.Value.Date + access_sql.Hash+" ";
            }
            else if (listBox3.Text == "Greater than Equal To (>=)")
            {
                str = "qu1.Vdate>="+access_sql.Hash + dateTimePicker1.Value.Date + access_sql.Hash+"";
            }
            else if (listBox3.Text == "Less Than (<)")
            {
                str = "qu1.Vdate<"+access_sql.Hash + dateTimePicker1.Value.Date + access_sql.Hash+" ";
            }
            else if (listBox3.Text == "Less Than Equal To (<=)")
            {
                str = "qu1.Vdate<="+access_sql.Hash + dateTimePicker1.Value.Date + access_sql.Hash+" ";
            }
            else if (listBox3.Text == "Not Equal To (!=)")
            {
                str = "Not qu1.Vdate="+access_sql.Hash + dateTimePicker1.Value.Date + access_sql.Hash+" ";
            }
            else if (listBox3.Text == "Between")
            {
                str = "qu1.Vdate>="+access_sql.Hash + dateTimePicker1.Value.Date + access_sql.Hash+" and qu1.Vdate<="+access_sql.Hash + dateTimePicker1.Value.Date + access_sql.Hash+" ";
            }
            return str;
        }



        private String AmountQuery()
        {
            
            String str = "";
            if (listBox4.Text == "Equal To (=)")
            {
                str = "qu1.Cr=" +   textBox1.Text + " ";
            }
            else if (listBox4.Text == "Greater Than (>)")
            {
                str = "qu1.Cr>" + textBox1.Text + " ";
            }
            else if (listBox4.Text == "Greater than Equal To (>=)")
            {
                str = "qu1.Cr>=" + textBox1.Text + "";
            }
            else if (listBox4.Text == "Less Than (<)")
            {
                str = "qu1.Cr<" + textBox1.Text + " ";
            }
            else if (listBox4.Text == "Less Than Equal To (<=)")
            {
                str = "qu1.Cr<=" + textBox1.Text + " ";
            }
            else if (listBox4.Text == "Not Equal To (!=)")
            {
                str = "Not qu1.Cr="+ textBox1.Text + " ";
            }
            else if (listBox4.Text == "Between")
            {
                str = "qu1.Cr>=" + textBox1.Text + " and qu1.Cr<=" + textBox2.Text + " ";
            }


            return str;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            String sql = "";
            Boolean flg = false;


            if (checkBox1.Checked == false && checkBox2.Checked == false)
            {
                sql = VoucherAmtQuery();
            }

            else if (checkBox1.Checked == true && checkBox2.Checked == false)
            {
                sql = VoucherAmtQuery() + " Where Qu1.Vt_id='" + funs.Select_vt_id_vnm(listBox1.Text) + "' ";
                flg = true;
            }
            else if (checkBox1.Checked == false && checkBox2.Checked == true)
            {
                sql = AccQuery() + " WHERE Qu1.Ac_id='" + funs.Select_ac_id(listBox2.Text) + "' ";
                flg = true;
            }
            else if (checkBox1.Checked == true && checkBox2.Checked == true)
            {
                sql = AccQuery() + " WHERE Qu1.Ac_id='" + funs.Select_ac_id(listBox2.Text) + "' AND Qu1.Vt_id='" + funs.Select_vt_id_vnm(listBox1.Text) + "' ";
                flg = true;
            }

            if (listBox3.Text != "<None>" && listBox4.Text != "<None>")
            {
                if (flg == false)
                {
                    sql = sql + " where ";
                }
                else
                {
                    sql = sql + " and ";
                }
                sql = sql + AccQuery() + " and " + DateQuery();
            }
            else if (listBox4.Text != "<None>")
            {
                if (flg == false)
                {
                    sql = sql + " where " + AmountQuery();
                }
                else
                {
                    sql = sql + " and " + AmountQuery();
                }
            }
            else if (listBox3.Text != "<None>")
            {
                if (flg == false)
                {
                    sql = sql + " where " + DateQuery();
                }
                else
                {
                    sql = sql + " and " + DateQuery();
                }
            }

            sql += " ORDER BY qu1.Vdate desc,  qu1.Short desc, qu1.Vnumber desc";

            DataTable dt = new DataTable();
            String str;
            dt.Clear();
            Database.GetSqlData(sql, dt);
            dataGridView1.Rows.Clear();
            if ((checkBox1.Checked == false && checkBox2.Checked == false) || (checkBox1.Checked == true && checkBox2.Checked == false))
            {
                dataGridView1.Columns["acc"].Visible = false;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dataGridView1.Rows.Add();
                    str = dt.Rows[i]["Short"] + " " + DateTime.Parse(dt.Rows[i]["Vdate"].ToString()).ToString("yyyyMMdd") + " " + dt.Rows[i]["Vnumber"];
                    dataGridView1.Rows[i].Cells["sno"].Value = (i + 1);
                    dataGridView1.Rows[i].Cells["dt"].Value = DateTime.Parse(dt.Rows[i]["Vdate"].ToString()).ToString("dd-MMM-yyyy");
                    dataGridView1.Rows[i].Cells["vou"].Value = str;
                    dataGridView1.Rows[i].Cells["amt"].Value = funs.DecimalPoint(double.Parse(dt.Rows[i]["Cr"].ToString()), 2);
                    dataGridView1.Rows[i].Cells["vid"].Value = dt.Rows[i]["vi_id"];
                    dataGridView1.Rows[i].Cells["vtyp"].Value = dt.Rows[i]["vt_id"];
                    dataGridView1.Rows[i].Cells["vnm"].Value = dt.Rows[i]["Vnumber"];
                }
            }
            else if ((checkBox1.Checked == false && checkBox2.Checked == true) || (checkBox1.Checked == true && checkBox2.Checked == true))
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dataGridView1.Rows.Add();
                    str = dt.Rows[i]["Short"] + " " + DateTime.Parse(dt.Rows[i]["Vdate"].ToString()).ToString("yyyyMMdd") + " " + dt.Rows[i]["Vnumber"];
                    dataGridView1.Rows[i].Cells["sno"].Value = (i + 1);
                    dataGridView1.Rows[i].Cells["dt"].Value = DateTime.Parse(dt.Rows[i]["Vdate"].ToString()).ToString("dd-MMM-yyyy");
                    dataGridView1.Rows[i].Cells["vou"].Value = str;
                    dataGridView1.Rows[i].Cells["acc"].Value = dt.Rows[i]["Name"];
                    dataGridView1.Rows[i].Cells["amt"].Value = funs.DecimalPoint(double.Parse(dt.Rows[i]["Cr"].ToString()), 2);
                    dataGridView1.Rows[i].Cells["vid"].Value = dt.Rows[i]["vi_id"];
                    dataGridView1.Rows[i].Cells["vtyp"].Value = dt.Rows[i]["vt_id"];
                    dataGridView1.Rows[i].Cells["vnm"].Value = dt.Rows[i]["Vnumber"];
                }
            }
            tabControl1.SelectedIndex = 4;

            button4.Visible = true;
            button5.Visible = false;
        }



        private void ok_save()
        {
            String sql = "";
            Boolean flg = false;


            if (checkBox1.Checked == false && checkBox2.Checked == false)
            {
                sql = VoucherAmtQuery();
            }

            else if (checkBox1.Checked == true && checkBox2.Checked == false)
            {
                sql = VoucherAmtQuery() + " Where Qu1.Vt_id='" + funs.Select_vt_id_vnm(listBox1.Text)+"' ";
                flg = true;
            }
            else if (checkBox1.Checked == false && checkBox2.Checked == true)
            {
                sql = AccQuery() + " WHERE Qu1.Ac_id='" + funs.Select_ac_id(listBox2.Text) + "' ";
                flg = true;
            }
            else if (checkBox1.Checked == true && checkBox2.Checked == true)
            {
                sql = AccQuery() + " WHERE Qu1.Ac_id='" + funs.Select_ac_id(listBox2.Text) + "' AND Qu1.Vt_id='" + funs.Select_vt_id_vnm(listBox1.Text) + "' ";
                flg = true;
            }

            if (listBox3.Text != "<None>" && listBox4.Text != "<None>")
            {
                if (flg == false)
                {
                    sql = sql + " where ";
                }
                else
                {
                    sql = sql + " and ";
                }
                sql = sql + AccQuery() + " and " + DateQuery();
            }
            else if (listBox4.Text != "<None>")
            {
                if (flg == false)
                {
                    sql = sql + " where " + AmountQuery();
                }
                else
                {
                    sql = sql + " and " + AmountQuery();
                }
            }
            else if (listBox3.Text != "<None>")
            {
                if (flg == false)
                {
                    sql = sql + " where " + DateQuery();
                }
                else
                {
                    sql = sql + " and " + DateQuery();
                }
            }

            sql += " ORDER BY qu1.Vdate desc,  qu1.Short desc, qu1.Vnumber desc";

            DataTable dt = new DataTable();
            String str;
            dt.Clear();
            Database.GetSqlData(sql, dt);
            dataGridView1.Rows.Clear();
            if ((checkBox1.Checked == false && checkBox2.Checked == false) || (checkBox1.Checked == true && checkBox2.Checked == false))
            {
                dataGridView1.Columns["acc"].Visible = false;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dataGridView1.Rows.Add();
                    str = dt.Rows[i]["Short"] + " " + DateTime.Parse(dt.Rows[i]["Vdate"].ToString()).ToString("yyyyMMdd") + " " + dt.Rows[i]["Vnumber"];
                    dataGridView1.Rows[i].Cells["sno"].Value = (i + 1);
                    dataGridView1.Rows[i].Cells["dt"].Value = DateTime.Parse(dt.Rows[i]["Vdate"].ToString()).ToString("dd-MMM-yyyy");
                    dataGridView1.Rows[i].Cells["vou"].Value = str;
                    dataGridView1.Rows[i].Cells["amt"].Value = funs.DecimalPoint(double.Parse(dt.Rows[i]["Cr"].ToString()), 2);
                    dataGridView1.Rows[i].Cells["vid"].Value = dt.Rows[i]["vi_id"];
                    dataGridView1.Rows[i].Cells["vtyp"].Value = dt.Rows[i]["vt_id"];
                    dataGridView1.Rows[i].Cells["vnm"].Value = dt.Rows[i]["Vnumber"];
                }
            }
            else if ((checkBox1.Checked == false && checkBox2.Checked == true) || (checkBox1.Checked == true && checkBox2.Checked == true))
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dataGridView1.Rows.Add();
                    str = dt.Rows[i]["Short"] + " " + DateTime.Parse(dt.Rows[i]["Vdate"].ToString()).ToString("yyyyMMdd") + " " + dt.Rows[i]["Vnumber"];
                    dataGridView1.Rows[i].Cells["sno"].Value = (i + 1);
                    dataGridView1.Rows[i].Cells["dt"].Value = DateTime.Parse(dt.Rows[i]["Vdate"].ToString()).ToString("dd-MMM-yyyy");
                    dataGridView1.Rows[i].Cells["vou"].Value = str;
                    dataGridView1.Rows[i].Cells["acc"].Value = dt.Rows[i]["Name"];
                    dataGridView1.Rows[i].Cells["amt"].Value = funs.DecimalPoint(double.Parse(dt.Rows[i]["Cr"].ToString()), 2);
                    dataGridView1.Rows[i].Cells["vid"].Value = dt.Rows[i]["vi_id"];
                    dataGridView1.Rows[i].Cells["vtyp"].Value = dt.Rows[i]["vt_id"];
                    dataGridView1.Rows[i].Cells["vnm"].Value = dt.Rows[i]["Vnumber"];
                }
            }
            tabControl1.SelectedIndex = 4;

            button4.Visible = true;
            button5.Visible = false;
        }



        private void button1_Click(object sender, EventArgs e)
        {
          
            button4.Visible = true;
            button5.Visible = true;
            tabControl1.SelectedIndex = 0;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string tempid = dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells["vid"].Value.ToString();
            if (tempid != "")
            {
                funs.OpenFrm(this, tempid, false); ;
            }           
        }

        private void frmSmartDocumentFinder_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
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

        private void tabControl1_KeyDown(object sender, KeyEventArgs e)
        {
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

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && !(e.KeyChar.ToString() == ".");
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && !(e.KeyChar.ToString() == ".");
        }
    }
}
