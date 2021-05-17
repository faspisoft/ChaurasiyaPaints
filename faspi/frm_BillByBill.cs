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
    public partial class frm_BillByBill : Form
    {
        public string CustName = "";
        public bool Indirect = false;
        String strCombo;

        public frm_BillByBill()
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
            dtsidefill.Rows[0]["Name"] = "adjust";
            dtsidefill.Rows[0]["DisplayName"] = "Adjust";
            dtsidefill.Rows[0]["ShortcutKey"] = "Alt+A";
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

            if (name == "adjust")
            {
                DataTable dtsave = new DataTable("BILLBYBILL");
                Database.GetSqlData("select * from [BILLBYBILL] where Ac_id='" + funs.Select_ac_id(textBox1.Text) + "' ", dtsave);
                dtsave.Rows.Add();
                dtsave.Rows[dtsave.Rows.Count - 1]["Ac_id"] = funs.Select_ac_id(textBox1.Text);
                dtsave.Rows[dtsave.Rows.Count - 1]["Bill_id"] = ansGridView2.Rows[ansGridView2.CurrentCell.RowIndex].Cells["vid1"].Value.ToString();
                dtsave.Rows[dtsave.Rows.Count - 1]["receive_id"] = ansGridView3.Rows[ansGridView3.CurrentCell.RowIndex].Cells["vid2"].Value.ToString();

                double DrA = double.Parse(ansGridView2.Rows[ansGridView2.CurrentCell.RowIndex].Cells["Unadjusted1"].Value.ToString());
                double CrA = double.Parse(ansGridView3.Rows[ansGridView3.CurrentCell.RowIndex].Cells["Unadjusted2"].Value.ToString());
                if (DrA < CrA)
                {
                    dtsave.Rows[dtsave.Rows.Count - 1]["Amount"] = DrA;
                }
                else
                {
                    dtsave.Rows[dtsave.Rows.Count - 1]["Amount"] = CrA;
                }
                Database.SaveData(dtsave);

                Loaddata();
            }

            else if (name == "quit")
            {
                this.Close();
                this.Dispose();
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
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            //strCombo = funs.GetStrCombo("*");
            strCombo = funs.GetStrComboled("*");
            textBox1.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 1);           
        }

        private void displaySetting()
        {
            textBox1.Select();
            ansGridView1.Columns["billdatecr"].DefaultCellStyle.BackColor = Color.BlanchedAlmond;
            ansGridView1.Columns["docnumbercr"].DefaultCellStyle.BackColor = Color.BlanchedAlmond;
            ansGridView1.Columns["amountcr"].DefaultCellStyle.BackColor = Color.BlanchedAlmond;

            ansGridView1.Columns["amountdr"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            ansGridView1.Columns["amountcr"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            ansGridView1.Columns["adjustamt"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            ansGridView2.Columns["amount2"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            ansGridView2.Columns["unadjusted1"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            ansGridView3.Columns["amount3"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            ansGridView3.Columns["unadjusted2"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            foreach (DataGridViewColumn column in ansGridView1.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            foreach (DataGridViewColumn column in ansGridView2.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            foreach (DataGridViewColumn column in ansGridView3.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        private void Loaddata()
        {
            displaySetting();
            double odr=0, odrad=0, ocr=0, ocrad=0,opn=0;

            if (Database.IsKacha == false)
            {
                opn = Database.GetScalarDecimal("SELECT Balance FROM ACCOUNT WHERE Name='" + textBox1.Text + "'");
                if (opn >= 0)
                {
                    odr = opn;
                }
                else
                {
                    ocr = opn;
                }

                odrad = Database.GetScalarDecimal("SELECT Sum(BILLBYBILL.Amount) AS SumOfAmount FROM BILLBYBILL LEFT JOIN ACCOUNT ON BILLBYBILL.Ac_id = ACCOUNT.Ac_id WHERE (((ACCOUNT.Name)='" + textBox1.Text + "') AND ((BILLBYBILL.Bill_id)='SER0'))");
                ocrad = Database.GetScalarDecimal("SELECT Sum(BILLBYBILL.Amount) AS SumOfAmount FROM BILLBYBILL LEFT JOIN ACCOUNT ON BILLBYBILL.Ac_id = ACCOUNT.Ac_id WHERE (((ACCOUNT.Name)='" + textBox1.Text + "') AND ((BILLBYBILL.receive_id)='SER0'))");
            }
            else
            {
                opn = Database.GetScalarDecimal("SELECT Balance2 FROM ACCOUNT WHERE Name='" + textBox1.Text + "'");
                odrad = Database.GetScalarDecimal("SELECT Sum(BILLBYBILL.Amount) AS SumOfAmount FROM BILLBYBILL LEFT JOIN ACCOUNT ON BILLBYBILL.Ac_id = ACCOUNT.Ac_id WHERE (((ACCOUNT.Name)='" + textBox1.Text + "') AND ((BILLBYBILL.Bill_id)='SER0'))");
                if (opn >= 0)
                {
                    odr = opn;
                }
                else
                {
                    ocr = opn;
                }

                ocrad = Database.GetScalarDecimal("SELECT Sum(BILLBYBILL.Amount) AS SumOfAmount FROM BILLBYBILL LEFT JOIN ACCOUNT ON BILLBYBILL.Ac_id = ACCOUNT.Ac_id WHERE (((ACCOUNT.Name)='" + textBox1.Text + "') AND ((BILLBYBILL.receive_id)='SER0'))");
            }
            DataTable dtbill = new DataTable();        
            if (Database.IsKacha == false)
            {
                Database.GetSqlData("SELECT VOUCHERINFO.Vdate AS DrDate," + access_sql.Docnumber + " AS DocNumber,   " + access_sql.fnstring("JOURNAL.Amount>0", "JOURNAL.Amount", "0") + " AS Dr, VOUCHERINFO_1.Vdate AS CrDate,  " + access_sql.Docnumber1 + " , " + access_sql.fnstring("JOURNAL_1.Amount<0", "-1*(JOURNAL_1.Amount)", "0") + " as Cr, BILLBYBILL.Amount, BILLBYBILL.Ac_id, BILLBYBILL.Bill_id, BILLBYBILL.receive_id FROM ((((((BILLBYBILL LEFT JOIN VOUCHERINFO ON BILLBYBILL.Bill_id = VOUCHERINFO.Vi_id) LEFT JOIN JOURNAL ON (BILLBYBILL.Bill_id = JOURNAL.Vi_id) AND (BILLBYBILL.Ac_id = JOURNAL.Ac_id)) LEFT JOIN VOUCHERINFO AS VOUCHERINFO_1 ON BILLBYBILL.receive_id = VOUCHERINFO_1.Vi_id) LEFT JOIN JOURNAL AS JOURNAL_1 ON (BILLBYBILL.receive_id = JOURNAL_1.Vi_id) AND (BILLBYBILL.Ac_id = JOURNAL_1.Ac_id)) LEFT JOIN VOUCHERTYPE AS VOUCHERTYPE_1 ON VOUCHERINFO_1.Vt_id = VOUCHERTYPE_1.Vt_id) LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id) LEFT JOIN ACCOUNT ON BILLBYBILL.Ac_id = ACCOUNT.Ac_id WHERE (((ACCOUNT.Name)='" + textBox1.Text + "') AND ((VOUCHERTYPE_1.A)=" + access_sql.Singlequote + "True" + access_sql.Singlequote + ")) ORDER BY VOUCHERINFO.Vdate, VOUCHERINFO_1.Vdate", dtbill);
            }
            else
            {
                Database.GetSqlData("SELECT VOUCHERINFO.Vdate AS DrDate,  " + access_sql.Docnumber + " AS DocNumber,   " + access_sql.fnstring("JOURNAL.Amount>0", "JOURNAL.Amount", "0") + " AS Dr, VOUCHERINFO_1.Vdate AS CrDate, " + access_sql.Docnumber1 + " , " + access_sql.fnstring("JOURNAL_1.Amount<0", "-1*(JOURNAL_1.Amount)", "0") + " as Cr, BILLBYBILL.Amount, BILLBYBILL.Ac_id, BILLBYBILL.Bill_id, BILLBYBILL.receive_id FROM ((((((BILLBYBILL LEFT JOIN VOUCHERINFO ON BILLBYBILL.Bill_id = VOUCHERINFO.Vi_id) LEFT JOIN JOURNAL ON (BILLBYBILL.Bill_id = JOURNAL.Vi_id) AND (BILLBYBILL.Ac_id = JOURNAL.Ac_id)) LEFT JOIN VOUCHERINFO AS VOUCHERINFO_1 ON BILLBYBILL.receive_id = VOUCHERINFO_1.Vi_id) LEFT JOIN JOURNAL AS JOURNAL_1 ON (BILLBYBILL.receive_id = JOURNAL_1.Vi_id) AND (BILLBYBILL.Ac_id = JOURNAL_1.Ac_id)) LEFT JOIN VOUCHERTYPE AS VOUCHERTYPE_1 ON VOUCHERINFO_1.Vt_id = VOUCHERTYPE_1.Vt_id) LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id) LEFT JOIN ACCOUNT ON BILLBYBILL.Ac_id = ACCOUNT.Ac_id WHERE (((ACCOUNT.Name)='" + textBox1.Text + "') AND ((VOUCHERTYPE_1.B)=" + access_sql.Singlequote + "True" + access_sql.Singlequote + ")) ORDER BY VOUCHERINFO.Vdate, VOUCHERINFO_1.Vdate", dtbill);
            }
            ansGridView1.Rows.Clear();

            for (int i = 0; i < dtbill.Rows.Count; i++)
            {
                ansGridView1.Rows.Add();

                if (int.Parse(dtbill.Rows[i]["Bill_id"].ToString()) != 0)
                {
                    ansGridView1.Rows[i].Cells["vidbill"].Value = dtbill.Rows[i]["Bill_id"].ToString();
                    ansGridView1.Rows[i].Cells["billdatedr"].Value = DateTime.Parse(dtbill.Rows[i]["DrDate"].ToString()).ToString(Database.dformat);
                    ansGridView1.Rows[i].Cells["docnumberdr"].Value = dtbill.Rows[i]["DocNumber"].ToString();
                    ansGridView1.Rows[i].Cells["amountdr"].Value = funs.IndianCurr(double.Parse(dtbill.Rows[i]["Dr"].ToString()));
                }
                else
                {
                    ansGridView1.Rows[i].Cells["vidbill"].Value = 0;
                    ansGridView1.Rows[i].Cells["docnumberdr"].Value = "Opening Balance";
                    ansGridView1.Rows[i].Cells["amountdr"].Value = funs.IndianCurr(odr);
                }

                if (int.Parse(dtbill.Rows[i]["receive_id"].ToString()) != 0)
                {
                    ansGridView1.Rows[i].Cells["vidrec"].Value = dtbill.Rows[i]["receive_id"].ToString();
                    ansGridView1.Rows[i].Cells["billdatecr"].Value = DateTime.Parse(dtbill.Rows[i]["CrDate"].ToString()).ToString(Database.dformat);
                    ansGridView1.Rows[i].Cells["docnumbercr"].Value = dtbill.Rows[i]["DocNumber2"].ToString();
                    ansGridView1.Rows[i].Cells["amountcr"].Value = funs.IndianCurr(double.Parse(dtbill.Rows[i]["Cr"].ToString()));
                }
                else
                {
                    ansGridView1.Rows[i].Cells["vidrec"].Value = 0;
                    ansGridView1.Rows[i].Cells["docnumbercr"].Value = "Opening Balance";
                    ansGridView1.Rows[i].Cells["amountcr"].Value = funs.IndianCurr(ocr);

                }
                ansGridView1.Rows[i].Cells["adjustamt"].Value = funs.IndianCurr(double.Parse(dtbill.Rows[i]["Amount"].ToString()));
            }

            DataTable dtdr = new DataTable();

            if (Database.IsKacha == false)
            {
                Database.GetSqlData("SELECT Test.Vdate, Test.DocNumber, Test.Amount, Test.Vi_id, " + access_sql.fnstring("Test.Adjusted " + access_sql.IsNull + " Null", "0", "Test.Adjusted") + " AS Adj, Test.Amount-  " + access_sql.fnstring("Test.Adjusted " + access_sql.IsNull + " Null", "0", "Test.Adjusted") + "  AS UnAdj FROM (SELECT VOUCHERINFO.Vdate, " + access_sql.Docnumber + " AS DocNumber, " + access_sql.fnstring("JOURNAL.Amount>0", "JOURNAL.Amount", "0") + " AS Amount, VoucherAmount.Vi_id, Sum(BILLBYBILL.Amount) AS Adjusted FROM (((((SELECT VOUCHERINFO.Vi_id, " + access_sql.fnstring("VOUCHERACTOTAL.Amount " + access_sql.IsNull + " null", "VOUCHERINFO.Ac_id", "VOUCHERACTOTAL.Accid") + "  AS AccountId FROM VOUCHERINFO LEFT JOIN VOUCHERACTOTAL ON VOUCHERINFO.Vi_id = VOUCHERACTOTAL.Vi_id)  AS VoucherAmount LEFT JOIN JOURNAL ON (VoucherAmount.AccountId = JOURNAL.Ac_id) AND (VoucherAmount.Vi_id = JOURNAL.Vi_id)) LEFT JOIN ACCOUNT ON VoucherAmount.AccountId = ACCOUNT.Ac_id) LEFT JOIN VOUCHERINFO ON VoucherAmount.Vi_id = VOUCHERINFO.Vi_id) LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id) LEFT JOIN BILLBYBILL ON VoucherAmount.Vi_id = BILLBYBILL.Bill_id WHERE (((ACCOUNT.Name)='" + textBox1.Text + "') AND ((JOURNAL.Amount)>0) AND ((VOUCHERTYPE.A)=" + access_sql.Singlequote + "True" + access_sql.Singlequote + ")) GROUP BY VOUCHERINFO.Vdate, " + access_sql.Docnumber + ", " + access_sql.fnstring("JOURNAL.Amount>0", "JOURNAL.Amount", "0") + ", VoucherAmount.Vi_id)  AS Test WHERE (Test.Amount- (" + access_sql.fnstring("Test.Adjusted " + access_sql.IsNull + " Null", "0", "Test.Adjusted") + ") <>0)", dtdr);
            }
            else
            {
                Database.GetSqlData("SELECT Test.Vdate, Test.DocNumber, Test.Amount, Test.Vi_id, " + access_sql.fnstring("Test.Adjusted " + access_sql.IsNull + " Null", "0", "Test.Adjusted") + " AS Adj, Test.Amount-" + access_sql.fnstring("Test.Adjusted " + access_sql.IsNull + " Null", "0", "Test.Adjusted") + "  AS UnAdj FROM (SELECT VOUCHERINFO.Vdate, " + access_sql.Docnumber + " AS DocNumber, " + access_sql.fnstring("JOURNAL.Amount>0", "JOURNAL.Amount", "0") + " AS Amount, VoucherAmount.Vi_id, Sum(BILLBYBILL.Amount) AS Adjusted FROM (((((SELECT VOUCHERINFO.Vi_id, " + access_sql.fnstring("VOUCHERACTOTAL.Amount " + access_sql.IsNull + " Null", "VOUCHERINFO.Ac_id", "VOUCHERACTOTAL.Accid") + " AS AccountId FROM VOUCHERINFO LEFT JOIN VOUCHERACTOTAL ON VOUCHERINFO.Vi_id = VOUCHERACTOTAL.Vi_id)  AS VoucherAmount LEFT JOIN JOURNAL ON (VoucherAmount.AccountId = JOURNAL.Ac_id) AND (VoucherAmount.Vi_id = JOURNAL.Vi_id)) LEFT JOIN ACCOUNT ON VoucherAmount.AccountId = ACCOUNT.Ac_id) LEFT JOIN VOUCHERINFO ON VoucherAmount.Vi_id = VOUCHERINFO.Vi_id) LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id) LEFT JOIN BILLBYBILL ON VoucherAmount.Vi_id = BILLBYBILL.Bill_id WHERE (((ACCOUNT.Name)='" + textBox1.Text + "') AND ((JOURNAL.Amount)>0) AND ((VOUCHERTYPE.B)=" + access_sql.Singlequote + "True" + access_sql.Singlequote + ")) GROUP BY VOUCHERINFO.Vdate, " + access_sql.Docnumber + ", " + access_sql.fnstring("JOURNAL.Amount>0", "JOURNAL.Amount", "0") + ", VoucherAmount.Vi_id)  AS Test WHERE (((Test.Amount - " + access_sql.fnstring("Test.Adjusted " + access_sql.IsNull + " Null", "0", "Test.Adjusted") + " )<>0))", dtdr);
            }
            if (odr > odrad)
            {
                DataRow drdr = dtdr.NewRow();
                drdr["DocNumber"] = "Opening Balance";
                drdr["Amount"] = odr;
                drdr["Vi_id"] = 0;
                drdr["Adj"] = odrad;
                drdr["UnAdj"] = odr - odrad;
                dtdr.Rows.InsertAt(drdr, 0);
            }
            ansGridView2.Rows.Clear();
            for (int i = 0; i < dtdr.Rows.Count; i++)
            {
                ansGridView2.Rows.Add();
                if (dtdr.Rows[i]["Vdate"].ToString() != "")
                {
                    ansGridView2.Rows[i].Cells["billdate2"].Value = DateTime.Parse(dtdr.Rows[i]["Vdate"].ToString()).ToString(Database.dformat);
                }
                ansGridView2.Rows[i].Cells["docnumber2"].Value = dtdr.Rows[i]["DocNumber"].ToString();
                ansGridView2.Rows[i].Cells["amount2"].Value = funs.IndianCurr(double.Parse(dtdr.Rows[i]["Amount"].ToString()));
                ansGridView2.Rows[i].Cells["unadjusted1"].Value = funs.IndianCurr(double.Parse(dtdr.Rows[i]["UnAdj"].ToString()));
                ansGridView2.Rows[i].Cells["vid1"].Value = dtdr.Rows[i]["Vi_id"].ToString();
            }

            DataTable dtcr = new DataTable();       

            if (Database.IsKacha == false)
            {
                Database.GetSqlData("SELECT Test.Vdate, Test.DocNumber, Test.Amount, Test.Vi_id, " + access_sql.fnstring("Test.Adjusted " + access_sql.IsNull + " Null", "0", "Test.Adjusted") + " AS Adj, Test.Amount-" + access_sql.fnstring("Test.Adjusted " + access_sql.IsNull + "  Null", "0", "Test.Adjusted") + " AS UnAdj FROM (SELECT VOUCHERINFO.Vdate," + access_sql.Docnumber + " AS DocNumber, " + access_sql.fnstring("JOURNAL.Amount<0", "-1*(JOURNAL.Amount)", "0") + "  AS Amount, VoucherAmount.Vi_id, Sum(BILLBYBILL.Amount) AS Adjusted FROM (((((SELECT VOUCHERINFO.Vi_id, " + access_sql.fnstring("VOUCHERACTOTAL.Amount " + access_sql.IsNull + " Null", "VOUCHERINFO.Ac_id", "VOUCHERACTOTAL.Accid") + " AS AccountId FROM VOUCHERINFO LEFT JOIN VOUCHERACTOTAL ON VOUCHERINFO.Vi_id = VOUCHERACTOTAL.Vi_id)  AS VoucherAmount LEFT JOIN JOURNAL ON (VoucherAmount.Vi_id = JOURNAL.Vi_id) AND (VoucherAmount.AccountId = JOURNAL.Ac_id)) LEFT JOIN ACCOUNT ON VoucherAmount.AccountId = ACCOUNT.Ac_id) LEFT JOIN VOUCHERINFO ON VoucherAmount.Vi_id = VOUCHERINFO.Vi_id) LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id) LEFT JOIN BILLBYBILL ON VoucherAmount.Vi_id = BILLBYBILL.receive_id WHERE (((ACCOUNT.Name)='" + textBox1.Text + "') AND ((JOURNAL.Amount)<0) AND ((VOUCHERTYPE.A)=" + access_sql.Singlequote + "True" + access_sql.Singlequote + ")) GROUP BY VOUCHERINFO.Vdate, " + access_sql.Docnumber + ", " + access_sql.fnstring("JOURNAL.Amount<0", "-1*(JOURNAL.Amount)", "0") + ", VoucherAmount.Vi_id)  AS Test WHERE (((Test.Amount- " + access_sql.fnstring("Test.Adjusted " + access_sql.IsNull + " Null", "0", "Test.Adjusted") + ")<>0))", dtcr);
            }
            else
            {
                Database.GetSqlData("SELECT Test.Vdate, Test.DocNumber, Test.Amount, Test.Vi_id," + access_sql.fnstring("Test.Adjusted " + access_sql.IsNull + " Null", "0", "Test.Adjusted") + " AS Adj, Test.Amount-" + access_sql.fnstring("Test.Adjusted " + access_sql.IsNull + " Null", "0", "Test.Adjusted") + " AS UnAdj FROM (SELECT VOUCHERINFO.Vdate, " + access_sql.Docnumber + " AS DocNumber, " + access_sql.fnstring("JOURNAL.Amount<0", "-1*(JOURNAL.Amount)", "0") + "  AS Amount, VoucherAmount.Vi_id, Sum(BILLBYBILL.Amount) AS Adjusted FROM (((((SELECT VOUCHERINFO.Vi_id," + access_sql.fnstring("VOUCHERACTOTAL.Amount " + access_sql.IsNull + " Null", "VOUCHERINFO.Ac_id", "VOUCHERACTOTAL.Accid") + " AS AccountId FROM VOUCHERINFO LEFT JOIN VOUCHERACTOTAL ON VOUCHERINFO.Vi_id = VOUCHERACTOTAL.Vi_id)  AS VoucherAmount LEFT JOIN JOURNAL ON (VoucherAmount.Vi_id = JOURNAL.Vi_id) AND (VoucherAmount.AccountId = JOURNAL.Ac_id)) LEFT JOIN ACCOUNT ON VoucherAmount.AccountId = ACCOUNT.Ac_id) LEFT JOIN VOUCHERINFO ON VoucherAmount.Vi_id = VOUCHERINFO.Vi_id) LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id) LEFT JOIN BILLBYBILL ON VoucherAmount.Vi_id = BILLBYBILL.receive_id WHERE (((ACCOUNT.Name)='" + textBox1.Text + "') AND ((JOURNAL.Amount)<0) AND ((VOUCHERTYPE.B)=" + access_sql.Singlequote + "True" + access_sql.Singlequote + ")) GROUP BY VOUCHERINFO.Vdate, " + access_sql.Docnumber + ", " + access_sql.fnstring("JOURNAL.Amount<0", "-1*(JOURNAL.Amount)", "0") + ", VoucherAmount.Vi_id)  AS Test WHERE (((Test.Amount- " + access_sql.fnstring("Test.Adjusted " + access_sql.IsNull + " Null", "0", "Test.Adjusted") + ")<>0))", dtcr);
            }
            if (ocr > ocrad)
            {
                DataRow crdr = dtcr.NewRow();
                crdr["DocNumber"] = "Opening Balance";
                crdr["Amount"] = ocr;
                crdr["Vi_id"] = 0;
                crdr["Adj"] = ocrad;
                crdr["UnAdj"] = ocr - ocrad;
                dtcr.Rows.InsertAt(crdr, 0);
            }
            ansGridView3.Rows.Clear();
            for (int i = 0; i < dtcr.Rows.Count; i++)
            {
                ansGridView3.Rows.Add();
                if (dtcr.Rows[i]["Vdate"].ToString() != "")
                {
                    ansGridView3.Rows[i].Cells["billdate3"].Value = DateTime.Parse(dtcr.Rows[i]["Vdate"].ToString()).ToString(Database.dformat);
                }
                ansGridView3.Rows[i].Cells["docnumber3"].Value = dtcr.Rows[i]["DocNumber"].ToString();
                ansGridView3.Rows[i].Cells["amount3"].Value = funs.IndianCurr(double.Parse(dtcr.Rows[i]["Amount"].ToString()));
                ansGridView3.Rows[i].Cells["unadjusted2"].Value = funs.IndianCurr(double.Parse(dtcr.Rows[i]["UnAdj"].ToString()));
                ansGridView3.Rows[i].Cells["vid2"].Value = dtcr.Rows[i]["Vi_id"].ToString();
            }
            if (dtdr.Compute("sum(UnAdj)", "").ToString() != "")
            {
                textBox2.Text = funs.IndianCurr(double.Parse(dtdr.Compute("sum(UnAdj)", "").ToString()));
            }
            else
            {
                textBox2.Text = funs.IndianCurr(0);
            }
            if (dtcr.Compute("sum(UnAdj)", "").ToString() != "")
            {
                textBox3.Text = funs.IndianCurr(double.Parse(dtcr.Compute("sum(UnAdj)", "").ToString()));
            }
            else
            {
                textBox3.Text = funs.IndianCurr(0);
            }
            textBox6.Text = funs.AccountBalance(textBox1.Text);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Loaddata();
        }

        private void frm_BillByBill_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
                this.Dispose();
            }
            else if (e.Alt && e.KeyCode == Keys.A)
            {
                
            }
        }

        private void ansGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                string billid = ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["vidbill"].Value.ToString();
                string recid = ansGridView1.Rows[ansGridView1.CurrentCell.RowIndex].Cells["vidrec"].Value.ToString();
                Database.CommandExecutor("delete from BILLBYBILL where Ac_id='" + funs.Select_ac_id(textBox1.Text) + " and Bill_id='" + billid + "' and receive_id='" + recid+"' ");
                Loaddata();
            } 
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox1);
        }

        private void textBox6_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox6);
        }

        private void frm_BillByBill_Load(object sender, EventArgs e)
        {
            textBox1.Text = CustName;
            if (Indirect == true)
            {
                textBox1.Enabled = false;
            }
            this.Size = this.MdiParent.Size;
            SideFill();
        }
    }
}
