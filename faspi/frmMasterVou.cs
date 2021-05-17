using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using Microsoft.Office.Core;
using Excel = Microsoft.Office.Interop.Excel;
using System.Web.Script.Serialization;




namespace faspi
{
    public partial class frmMasterVou : Form
    {
        string gstr = "";
        string gFrmCaption = "";
        DataTable dt;
        DataTable dtvou = new DataTable();
        ToolTip tooltip = new ToolTip();
        BindingSource bs = new BindingSource();
        public ToolStripProgressBar ProgrBar;
        List<UsersFeature> permission;
        public frmMasterVou()
        {
            InitializeComponent();          
        }

        private void Excelexport()
        {
            if (ansGridView5.Rows.Count == 0)
            {
                return;
            }

            Object misValue = System.Reflection.Missing.Value;
            Excel.Application apl = new Microsoft.Office.Interop.Excel.Application();
            Excel.Workbook wb = (Excel.Workbook)apl.Workbooks.Add(misValue);
            Excel.Worksheet ws;
            ws = (Excel.Worksheet)wb.Worksheets[1];
            int lno = 1;
            DataTable dtExcel = new DataTable();
            DataTable dtRheader = new DataTable();
            Database.GetSqlData("select * from company", dtRheader);

            ws.Cells[lno, 1] = dtRheader.Rows[0]["name"].ToString();
            ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, ansGridView5.Columns.Count]).Merge(Type.Missing);
            ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, ansGridView5.Columns.Count]).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, ansGridView5.Columns.Count]).Font.Bold = true;
            lno++;

            ws.Cells[lno, 1] = dtRheader.Rows[0]["Address1"].ToString();
            ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, ansGridView5.Columns.Count]).Merge(Type.Missing);
            ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, ansGridView5.Columns.Count]).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, ansGridView5.Columns.Count]).Font.Bold = true;
            lno++;

            ws.Cells[lno, 1] = dtRheader.Rows[0]["Address2"].ToString();
            ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, ansGridView5.Columns.Count]).Merge(Type.Missing);
            ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, ansGridView5.Columns.Count]).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, ansGridView5.Columns.Count]).Font.Bold = true;
            lno++;

            int a = 0;

            for (int i = 5; i < 10; i++)
            {
                if (ansGridView5.Columns[a].HeaderCell.Style.Alignment == DataGridViewContentAlignment.MiddleRight)
                {
                    ws.get_Range(ws.Cells[5, a + 1], ws.Cells[5, a + 1]).HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                }
                ws.get_Range(ws.Cells[i + 1, a + 1], ws.Cells[a + 1, a + 1]).ColumnWidth = ansGridView5.Columns[i].Width / 11.5;
                ws.Cells[5, a + 1] = ansGridView5.Columns[i].HeaderText.ToString();
                a++;
            }
           
            for (int i = 0; i < ansGridView5.Rows.Count; i++)
            {
                int b = 0;
                for (int j = 5; j < 10; j++)
                {                    
                    if (ansGridView5.Columns[j].HeaderCell.Style.Alignment == DataGridViewContentAlignment.MiddleRight)
                    {
                        ws.get_Range(ws.Cells[i + 6, b + 1], ws.Cells[i + 6, b + 1]).HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                        ws.get_Range(ws.Cells[i + 6, b + 1], ws.Cells[i + 6, b + 1]).NumberFormat = "0,0.00";
                    }
                    else
                    {
                        ws.get_Range(ws.Cells[i + 6, b + 1], ws.Cells[i + 6, b + 1]).HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                    }

                    if (ansGridView5.Columns[j].DefaultCellStyle.Font != null)
                    {
                        ws.get_Range(ws.Cells[i + 6, b + 1], ws.Cells[i + 6, b + 1]).Font.Bold = true;
                    }

                    if (ansGridView5.Rows[i].Cells[j].Value != null)
                    {
                        ws.Cells[i + 6, b + 1] = ansGridView5.Rows[i].Cells[j].Value.ToString().Replace(",", "");
                    }
                    b++;
                }
            }

            Excel.Range last = ws.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell, Type.Missing);
            ws.get_Range("A1", last).WrapText = true;
            ws.Columns.AutoFit();
            apl.Visible = true;
        }

        public void LoadData(string str, string FrmCaption)
        {
            gstr = str;
            gFrmCaption = FrmCaption;
            this.Text = gFrmCaption;
            string sql = "";


            permission = funs.GetPermissionKey(str);
            if (permission != null)
            {
                UsersFeature ob = permission.Where(w => w.FeatureName == "Delete").FirstOrDefault();
                if (ob != null && ob.SelectedValue == "Not Allowed")
                {
                    ansGridView5.Columns["Delet"].Visible = false;
                }
                else 
                {

                    ansGridView5.Columns["Delet"].Visible = true;
                }
               
               
            }
            //if (permission != null)
            //{
            //    UsersFeature ob = permission.Where(w => w.FeatureName == "Alter").FirstOrDefault();
            //    if (ob != null && ob.SelectedValue == "Not Allowed")
            //    {
            //        ansGridView5.Columns["edit"].Visible = false;
            //    }
            //    else
            //    {

            //        ansGridView5.Columns["edit"].Visible = true;
            //    }


            //}
            if (gstr == "Purchase" || gstr == "P Return")
            {

                sql = "SELECT CONVERT(nvarchar, VOUCHERINFO.Vdate, 106) AS VDate, VOUCHERTYPE.Name,  case when ACCOUNT.Name is null then '<Main>' Else ACCOUNT.Name End AS AccName, CAST(VOUCHERINFO.Svnum AS nvarchar(25)) AS Vnumber, VOUCHERINFO.user_id AS Usr, Userinfo_1.Uname AS Modified_By, VOUCHERINFO.Totalamount AS Amount, VOUCHERINFO.printcount AS PrintCount, VOUCHERINFO.Vi_id, Userinfo.Uname AS Approved_By FROM VOUCHERINFO LEFT OUTER JOIN Userinfo ON VOUCHERINFO.ApprovedBy = Userinfo.U_id LEFT OUTER JOIN Userinfo AS Userinfo_1 ON VOUCHERINFO.Modifiedby = Userinfo_1.U_id LEFT OUTER JOIN ACCOUNT ON VOUCHERINFO.Ac_id = ACCOUNT.Ac_id LEFT OUTER JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id WHERE (VOUCHERINFO.Branch_id = '" + Database.BranchId + "') AND (VOUCHERINFO.Vdate >= '" + dateTimePicker1.Value.Date.ToString(Database.dformat) + "') AND (VOUCHERINFO.Vdate <= '" + dateTimePicker2.Value.Date.ToString(Database.dformat) + "') AND (VOUCHERTYPE.Type = '" + gstr + "') AND (VOUCHERTYPE." + Database.BMode + " = 'true') ORDER BY VOUCHERINFO.Vdate DESC, VOUCHERTYPE.Name DESC";
            }
            else
            {
                sql = "SELECT CONVERT(nvarchar, VOUCHERINFO.Vdate, 106) AS VDate, VOUCHERTYPE.Name,  case when ACCOUNT.Name is null then '<Main>' Else ACCOUNT.Name End AS AccName, CAST(VOUCHERINFO.Vnumber AS nvarchar(25)) AS Vnumber, VOUCHERINFO.user_id AS Usr, Userinfo_1.Uname AS Modified_By, VOUCHERINFO.Totalamount AS Amount, VOUCHERINFO.printcount AS PrintCount, VOUCHERINFO.Vi_id, Userinfo.Uname AS Approved_By FROM VOUCHERINFO LEFT OUTER JOIN Userinfo ON VOUCHERINFO.ApprovedBy = Userinfo.U_id LEFT OUTER JOIN Userinfo AS Userinfo_1 ON VOUCHERINFO.Modifiedby = Userinfo_1.U_id LEFT OUTER JOIN ACCOUNT ON VOUCHERINFO.Ac_id = ACCOUNT.Ac_id LEFT OUTER JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id WHERE (VOUCHERINFO.Branch_id = '" + Database.BranchId + "') AND (VOUCHERINFO.Vdate >= '" + dateTimePicker1.Value.Date.ToString(Database.dformat) + "') AND (VOUCHERINFO.Vdate <= '" + dateTimePicker2.Value.Date.ToString(Database.dformat) + "') AND (VOUCHERTYPE.Type = '" + gstr + "') AND (VOUCHERTYPE." + Database.BMode + " = 'true') ORDER BY VOUCHERINFO.Vdate DESC, VOUCHERTYPE.Name DESC, VOUCHERINFO.Vnumber DESC";
            }

            if (gstr == "Opening")
            {
                dateTimePicker1.Value = Database.stDate.AddDays(-1);
                dateTimePicker2.Value = Database.stDate.AddDays(-1);
                groupBox3.Visible = false;
                groupBox4.Visible = false;
                button3.Visible = false;
                //if (Database.IsKacha == false)
                //{
                    sql = "SELECT CONVERT(nvarchar, VOUCHERINFO.Vdate, 106) AS VDate, VOUCHERTYPE.Name,  case when ACCOUNT.Name is null then '<Main>' Else ACCOUNT.Name End AS AccName, CAST(VOUCHERINFO.Vnumber AS nvarchar(10)) AS Vnumber, VOUCHERINFO.user_id AS Usr, Userinfo_1.Uname AS Modified_By, VOUCHERINFO.Totalamount AS Amount, VOUCHERINFO.printcount AS PrintCount, VOUCHERINFO.Vi_id, Userinfo.Uname AS Approved_By FROM VOUCHERINFO LEFT OUTER JOIN Userinfo ON VOUCHERINFO.ApprovedBy = Userinfo.U_id LEFT OUTER JOIN Userinfo AS Userinfo_1 ON VOUCHERINFO.Modifiedby = Userinfo_1.U_id LEFT OUTER JOIN ACCOUNT ON VOUCHERINFO.Ac_id = ACCOUNT.Ac_id LEFT OUTER JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id WHERE (VOUCHERINFO.Branch_id = '" + Database.BranchId + "') AND (VOUCHERINFO.Vdate >= '" + dateTimePicker1.Value.Date.ToString(Database.dformat) + "') AND (VOUCHERINFO.Vdate <= '" + dateTimePicker2.Value.Date.ToString(Database.dformat) + "') AND (VOUCHERTYPE.Type = '" + gstr + "') AND (VOUCHERTYPE."+Database.BMode+" = 'true') ORDER BY VOUCHERINFO.Vdate DESC, VOUCHERTYPE.Name DESC, VOUCHERINFO.Vnumber DESC";

                //}
                //else
                //{
                //    sql = "SELECT CONVERT(nvarchar, VOUCHERINFO.Vdate, 106) AS VDate, VOUCHERTYPE.Name, case when ACCOUNT.Name is null then '<Main>' Else ACCOUNT.Name End AS AccName, CAST(VOUCHERINFO.Vnumber AS nvarchar(10)) AS Vnumber, VOUCHERINFO.user_id AS Usr, Userinfo_1.Uname AS Modified_By, VOUCHERINFO.Totalamount AS Amount, VOUCHERINFO.printcount AS PrintCount, VOUCHERINFO.Vi_id, Userinfo.Uname AS Approved_By FROM VOUCHERINFO LEFT OUTER JOIN Userinfo ON VOUCHERINFO.ApprovedBy = Userinfo.U_id LEFT OUTER JOIN Userinfo AS Userinfo_1 ON VOUCHERINFO.Modifiedby = Userinfo_1.U_id LEFT OUTER JOIN ACCOUNT ON VOUCHERINFO.Ac_id = ACCOUNT.Ac_id LEFT OUTER JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id WHERE (VOUCHERINFO.Branch_id = '" + Database.BranchId + "') AND (VOUCHERINFO.Vdate >= '" + dateTimePicker1.Value.Date.ToString(Database.dformat) + "') AND (VOUCHERINFO.Vdate <= '" + dateTimePicker2.Value.Date.ToString(Database.dformat) + "') AND (VOUCHERTYPE.Type = '" + gstr + "') AND (VOUCHERTYPE.B = 'true') ORDER BY VOUCHERINFO.Vdate DESC, VOUCHERTYPE.Name DESC, VOUCHERINFO.Vnumber DESC";

                //}
            }

            Database.GetSqlData(sql, dtvou);
            SideFill();
            double total = 0;

            for (int i = 0; i < dtvou.Rows.Count; i++)
            {
                total = total + double.Parse(dtvou.Rows[i]["Amount"].ToString());
                if (dtvou.Columns["Amount"].DataType.Name == "Decimal")
                {

                    dtvou.Rows[i]["Amount"] = double.Parse(dtvou.Rows[i]["Amount"].ToString());
                }
            }

            ansGridView5.DataSource = dtvou;
            label1.Text = funs.IndianCurr(total);
            ansGridView5.Columns["Vdate"].DisplayIndex =0;
            ansGridView5.Columns["Name"].DisplayIndex = 1;
            ansGridView5.Columns["AccName"].DisplayIndex = 2;
            ansGridView5.Columns["Vnumber"].DisplayIndex = 3;
            ansGridView5.Columns["Amount"].DisplayIndex = 4;
            ansGridView5.Columns["Usr"].DisplayIndex = 5;
            ansGridView5.Columns["PrintCount"].DisplayIndex = 6;           
            ansGridView5.Columns["Vi_id"].DisplayIndex = 12;
            ansGridView5.Columns["Entered"].DisplayIndex = 7;
            ansGridView5.Columns["Modified_By"].DisplayIndex = 8;
            ansGridView5.Columns["Approved_By"].DisplayIndex = 9;
            ansGridView5.Columns["view"].DisplayIndex = 10;
            ansGridView5.Columns["print"].DisplayIndex = 11;
            ansGridView5.Columns["Edit"].DisplayIndex = 12;
            ansGridView5.Columns["Delet"].DisplayIndex = 13;

            ansGridView5.Columns["Vi_id"].Visible = false;
            ansGridView5.Columns["usr"].Visible = false;
            if (Feature.Available("Show Print Count") == "Yes")
            {
                ansGridView5.Columns["PrintCount"].Visible = true;              
            }
            else
            {
                ansGridView5.Columns["PrintCount"].Visible = false;
            }
            //if (Feature.Available("Voucher Delete Permission") == "Yes")
            //{
            //    ansGridView5.Columns["Delet"].Visible = true;
            //}
            //else
            //{
            //    ansGridView5.Columns["Delet"].Visible = false;
            //}

            
            for (int i = 0; i < dtvou.Columns.Count; i++)
            {
                if (dtvou.Columns[i].DataType.Name == "Decimal")
                {
                    ansGridView5.Columns[dtvou.Columns[i].ColumnName].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    ansGridView5.Columns[dtvou.Columns[i].ColumnName].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
                if (dtvou.Columns[i].DataType.Name == "Int32")
                {
                    ansGridView5.Columns[dtvou.Columns[i].ColumnName].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    ansGridView5.Columns[dtvou.Columns[i].ColumnName].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                if (dtvou.Columns[i].DataType.Name == "Double")
                {
                    ansGridView5.Columns[dtvou.Columns[i].ColumnName].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    ansGridView5.Columns[dtvou.Columns[i].ColumnName].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
            }

            for (int i = 0; i < ansGridView5.Rows.Count; i++)
            {               
                //if (Database.utype.ToUpper() == "USER")
                //{
                //    ansGridView5.Columns["Delet"].Visible = false;
                //}

                //if (gstr == "Opening" && (Database.utype.ToUpper()=="ADMIN" || Database.utype.ToUpper() == "USER"))
                //{
                   
                //    ansGridView5.Columns["Delet"].Visible = false;
                //}
                ansGridView5.Rows[i].Cells["Entered"].Value = Database.GetScalarText("Select Uname from Userinfo where u_id='" + ansGridView5.Rows[i].Cells["Usr"].Value.ToString() + "' ");
                string uploaddoc = Database.GetScalarText("Select uploaddoc from Voucherinfo where Vi_id='" + ansGridView5.Rows[i].Cells["Vi_id"].Value.ToString() + "' ");
                if (uploaddoc.Trim() != "")
                {
                    ansGridView5.Rows[i].Cells["Vnumber"].Style.BackColor = Color.LightGray;
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
            
            //createnew
            dtsidefill.Rows.Add();
            dtsidefill.Rows[0]["Name"] = "add";
            dtsidefill.Rows[0]["DisplayName"] = "Create New";
            dtsidefill.Rows[0]["ShortcutKey"] = "^C";
            dtsidefill.Rows[0]["Visible"] = true;

            //refresh
            dtsidefill.Rows.Add();
            dtsidefill.Rows[1]["Name"] = "refresh";
            dtsidefill.Rows[1]["DisplayName"] = "Refresh";
            dtsidefill.Rows[1]["ShortcutKey"] = "^R";
            dtsidefill.Rows[1]["Visible"] = true;


            //Export List
            dtsidefill.Rows.Add();
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Name"] = "export";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["DisplayName"] = "Export List";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["ShortcutKey"] = "^E";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = true;

            //close
            dtsidefill.Rows.Add();
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Name"] = "quit";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["DisplayName"] = "Quit";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["ShortcutKey"] = "Esc";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = true;

            //upload Doc
            dtsidefill.Rows.Add();
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Name"] = "upload";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["DisplayName"] = "Upload Rec.";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["ShortcutKey"] = "";
            if (ansGridView5.Rows.Count == 0)
            {
                dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = false;
            }
            else
            {
                dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = true;
            }
            //upload ewaybill
            dtsidefill.Rows.Add();
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Name"] = "ewaybill";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["DisplayName"] = "E-Way Bill";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["ShortcutKey"] = "";
            if (ansGridView5.Rows.Count == 0)
            {
                dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = false;
            }
            else
            {
                if (gstr == "Sale")
                {
                    dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = true;
                }
                else
                {
                    dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = false;
                }
            }

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
                    System.Drawing.Rectangle RC = btn.ClientRectangle;
                    System.Drawing.Font font = new System.Drawing.Font("Arial", 12);
                    G.DrawString(line1, font, Brushes.Red, RC, SF);
                    G.DrawString("".PadLeft(line1.Length * 2 + 1) + line2, font, Brushes.Black, RC, SF);
                    btn.Image = bmp;
                    btn.Click += new EventHandler(btn_Click);
                    flowLayoutPanel1.Controls.Add(btn);
                }
            }
        }

        private void ADD()
        {
            if (gstr == "Receipt")
            {
                frmCashRec frm = new frmCashRec();
                frm.recpay = "Receipt";
                frm.LoadData("", "Receipt");
                frm.MdiParent = this.MdiParent;
                frm.Show();
            }
            else if (gstr == "Payment")
            {
                frmCashRec frm = new frmCashRec();
                frm.recpay = "Payment";
                frm.LoadData("", "Payment");
                frm.MdiParent = this.MdiParent;
                frm.Show();
            }
            else if (gstr == "Contra")
            {
                frmCashRec frm = new frmCashRec();
                frm.recpay = "Contra";
                frm.LoadData("", "Contra");
                frm.MdiParent = this.MdiParent;
                frm.Show();
            }
            else if (gstr == "Journal")
            {
                frmJournal frm = new frmJournal();
                frm.LoadData("", "Journal Voucher");
                frm.MdiParent = this.MdiParent;
                frm.Show();
            }
            else if (gstr == "Dnote")
            {
                frmDebitCredit frm = new frmDebitCredit();
                frm.dr_cr_note = "Debit Note";
                frm.LoadData("", "Debit Note");
                frm.MdiParent = this.MdiParent;
                frm.Show();
            }
            else if (gstr == "Cnote")
            {
                frmDebitCredit frm = new frmDebitCredit();
                frm.dr_cr_note = "Credit Note";
                frm.LoadData("", "Credit Note");
                frm.MdiParent = this.MdiParent;
                frm.Show();
            }
            else if (gstr == "Purchase")
            {
                frmTransaction frm = new frmTransaction();
                frm.LoadData("", "Purchase", true, false, false);
                frm.MdiParent = this.MdiParent;
                frm.Show();
            }
            else if (gstr == "PWDebitNote")
            {
                frmTransaction frm = new frmTransaction();
                frm.LoadData("", "PWDebitNote", true, false, false);
                frm.MdiParent = this.MdiParent;
                frm.Show();
            }
            else if (gstr == "Sale")
            {
                frmTransaction frm = new frmTransaction();
                frm.LoadData("", "Sale", true, false, false);
                frm.MdiParent = this.MdiParent;
                frm.Show();
            }
            else if (gstr == "P Return")
            {
                frmTransaction frm = new frmTransaction();
                frm.LoadData("", "P Return", true, false, false);
                frm.MdiParent = this.MdiParent;
                frm.Show();
            }
            else if (gstr == "Return")
            {
                frmTransaction frm = new frmTransaction();
                frm.LoadData("", "Return", true, false, false);
                frm.MdiParent = this.MdiParent;
                frm.Show();
            }
            else if (gstr == "Pending")
            {
                frmTransaction frm = new frmTransaction();
                frm.LoadData("", "Pending", false, false, false);
                frm.MdiParent = this.MdiParent;
                frm.Show();
            }

            else if (gstr == "receive")
            {
                frmTransaction frm = new frmTransaction();
                frm.MdiParent = this.MdiParent;
                frm.LoadData("", "receive", false, false, false);
                frm.Show();
            }
            else if (gstr == "issue")
            {
                frmTransaction frm = new frmTransaction();
                frm.MdiParent = this.MdiParent;
                frm.LoadData("", "issue", false, false, false);
                frm.Show();
            }
            else if (gstr == "RCM")
            {
                frmTransaction frm = new frmTransaction();
                frm.LoadData("", "RCM", true, false, false);
                frm.MdiParent = this.MdiParent;
                frm.Show();
            }
            else if (gstr == "Sale Order")
            {
                frmTransaction frm = new frmTransaction();
                frm.LoadData("", "Sale Order", true, false, false);
                frm.MdiParent = this.MdiParent;
                frm.Show();
            }
            else if (gstr == "Transfer")
            {
                frm_stkjournal frm = new frm_stkjournal();
                frm.MdiParent = this.MdiParent;
                frm.LoadData("", "Transfer");
                frm.Show();
            }

            else if (gstr == "Opening")
            {
                frmTransaction frm = new frmTransaction();
                frm.LoadData("", "Opening", true, false, false);
                frm.MdiParent = this.MdiParent;
                frm.Show();
            }
        }

        void btn_Click(object sender, EventArgs e)
        {
            Button tbtn = (Button)sender;
            string name = tbtn.Name.ToString();

            if (name == "add")
            {
                ADD();
            }
            else if (name == "refresh")
            {
                LoadData(gstr, gFrmCaption);
            }
            else if (name == "quit")
            {
                this.Close();
                this.Dispose();
            }
            else if (name == "upload")
            {
                frm_uploaddoc frm = new frm_uploaddoc(ansGridView5.CurrentRow.Cells["Vi_id"].Value.ToString());
                frm.Show();
            }
            else if (name == "export")
            {
                LoadData(gstr, gFrmCaption);
                Excelexport();
            }
            else if (name == "ewaybill")
            {
                frm_previewjson frm = new frm_previewjson();
                if (frm.LoadData(ansGridView5.CurrentRow.Cells["Vi_id"].Value.ToString()) == true)
                {
                    frm.ShowDialog();
                }
                else
                {
                    frm.Dispose();
                }

                //  CreateJson(int.Parse(ansGridView5.CurrentRow.Cells["Vi_id"].Value.ToString()));


            }
        }

        public void ExportToPdf(string tPath)
        {
            string str = "";

            FileStream fs = new FileStream(tPath, FileMode.Create, FileAccess.Write, FileShare.None);
            iTextSharp.text.Rectangle rec;
            Document document;
            int Twidth = 0;
            for (int i = 5; i < 10; i++)
            {
                Twidth += ansGridView5.Columns[i].Width;
            }
            if (Twidth == 2000)
            {
                document = new Document(PageSize.A4.Rotate(), 20f, 10f, 20f, 10f);
            }

            document = new Document(PageSize.A4, 20f, 10f, 20f, 10f);

            //  Pagesize = GetPapersize();
            PdfWriter writer = PdfWriter.GetInstance(document, fs);
            writer.PageEvent = new MainTextEventsHandler();
            document.Open();
            HTMLWorker hw = new HTMLWorker(document);
            str = "";
            str += @"<body> <font size='1'><table border=1> <tr>";
            for (int i = 5; i < 10; i++)
            {
                string align = "";
                string bold = "";
                int width = 0;

                if (Twidth == 2000)
                {
                    width = ansGridView5.Columns[i].Width / 20;
                }
                else
                {
                    width = ansGridView5.Columns[i].Width / 10;
                }

                if (ansGridView5.Columns[i].HeaderCell.Style.Alignment == DataGridViewContentAlignment.MiddleRight)
                {
                    align = "text-align:right;";
                }

                bold = "font-weight: bold;";

                if (width != 0)
                {
                    str += "<th width=" + width + "%  style='" + align + bold + "'>" + ansGridView5.Columns[i].HeaderText.ToString() + "</th> ";
                }
            }

            str += "</tr>";

            for (int i = 0; i < ansGridView5.Rows.Count; i++)
            {
                str += "<tr> ";
                for (int j = 5; j < 10; j++)
                {
                    int width = 0;
                    if (Twidth == 2000)
                    {
                        width = ansGridView5.Rows[i].Cells[j].Size.Width / 20;
                    }
                    else
                    {
                        width = ansGridView5.Rows[i].Cells[j].Size.Width / 10;
                    }

                    if (width != 0)
                    {
                        if (ansGridView5.Rows[i].Cells[j].Value != null)
                        {
                            string align = "";
                            string bold = "";
                            string colspan = "";

                            if (ansGridView5.Columns[j].DefaultCellStyle.Alignment == DataGridViewContentAlignment.MiddleRight)
                            {
                                align = "text-align:right;";
                            }
                            if (ansGridView5.Rows[i].Cells[j].Style.Font != null && ansGridView5.Rows[i].Cells[j].Style.Font.Bold == true)
                            {
                                bold = "font-weight: bold;";
                            }

                            if (j == 0 && ansGridView5.Rows[i].Cells[0].Value.ToString() != "" && ansGridView5.Rows[i].Cells[1].Value == null && ansGridView5.Rows[i].Cells[2].Value == null)
                            {
                                colspan = "colspan= '2'";
                            }
                            if (ansGridView5.Rows[i].Cells[j].Value.ToString().Trim() == "")
                            {
                                str += "<td> &nbsp; </td>";
                            }
                            else
                            {
                                str += "<td " + colspan + "  style='" + align + bold + "'>" + ansGridView5.Rows[i].Cells[j].Value.ToString() + "</td> ";
                            }

                            if (j == 0 && ansGridView5.Rows[i].Cells[0].Value.ToString() != "" && ansGridView5.Rows[i].Cells[1].Value == null && ansGridView5.Rows[i].Cells[2].Value == null)
                            {
                                j++;
                            }
                        }
                        else
                        {
                            str += "<td> &nbsp; </td>";
                        }
                    }
                }
                str += "</tr> ";
            }
            str += "</table></font></body>";

            StringReader sr = new StringReader(str);
            hw.Parse(sr);
            document.Close();
        }

        internal class MainTextEventsHandler : PdfPageEventHelper
        {
            public override void OnStartPage(PdfWriter writer, Document document)
            {
                base.OnStartPage(writer, document);

                DataTable dtRheader = new DataTable();
                Database.GetSqlData("select * from company", dtRheader);
                PdfPTable table = new PdfPTable(1);
                PdfPCell cell = new PdfPCell();

                cell.Phrase = new Phrase(dtRheader.Rows[0]["name"].ToString());
                cell.BorderWidth = 0f;
                cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table.AddCell(cell);
                cell.Phrase = new Phrase(dtRheader.Rows[0]["Address1"].ToString());
                table.AddCell(cell);
                cell.Phrase = new Phrase(dtRheader.Rows[0]["Address2"].ToString());
                table.AddCell(cell);
                cell.Phrase = new Phrase(Report.DecsOfReport2);
                table.AddCell(cell);
                cell.Phrase = new Phrase("\n");
                table.AddCell(cell);

                document.Add(table);
            }

            public override void OnEndPage(PdfWriter writer, Document document)
            {
                base.OnEndPage(writer, document);
                string text = "";
                text += "Page No-" + document.PageNumber;
                PdfContentByte cb = writer.DirectContent;
                cb.BeginText();
                BaseFont bf = BaseFont.CreateFont();
                cb.SetFontAndSize(bf, 8);
                cb.SetTextMatrix(530, 8);
                cb.ShowText(text);
                cb.EndText();
            }
        }

        private void frmMasterVou_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.P)
            {
                LoadData(gstr, gFrmCaption);
                if (ansGridView5.Rows.Count == 0)
                {
                    return;
                }

                string tPath = Path.GetTempPath() + DateTime.Now.ToString("yyMMddhmmssfff") + ".pdf";
                ExportToPdf(tPath);
                GC.Collect();
                PdfReader frm = new PdfReader();
                frm.LoadFile(tPath);
                frm.Show();
            }
            if (e.Control && e.KeyCode == Keys.E)
            {
                LoadData(gstr, gFrmCaption);
                Excelexport();
            }
            if (e.Control && e.KeyCode == Keys.C)
            {
                ADD();
            }

            else if (e.Control && e.KeyCode == Keys.S)
            {
                //if (gstr == "Transfer" || gstr == "Purchase" || gstr == "Sale" || gstr == "Return" || gstr == "P Return" || gstr == "Opening" || gstr == "Receipt" || gstr == "Payment")
                //{
                    InputBox box = new InputBox("Enter Password", "", true);
                    box.outStr = "";
                    box.ShowInTaskbar = false;
                    box.ShowDialog(this);

                    if (box.outStr == "admin")
                    {
                        if (Database.databaseName != "")
                        {
                            if (Database.DatabaseType == "access")
                            {
                                if (Database.AccessCnn.State == ConnectionState.Open)
                                {
                                    Database.CloseConnection();
                                }
                                File.Copy(Application.StartupPath + "\\Database\\" + Database.databaseName + ".mdb", Application.StartupPath + "\\System\\rs" + DateTime.Now.ToString("yyyyMMddhmmff"));
                            }
                            else
                            {
                                if (Database.SqlCnn.State == ConnectionState.Open)
                                {
                                    Database.CloseConnection();
                                }
                                string pathbackup = Application.StartupPath + "\\System\\rs" + Database.databaseName + DateTime.Now.ToString("yyyyMMddhmmff") + ".bak";
                                Database.CommandExecutor("Backup database " + Database.databaseName + " to disk='" + pathbackup + "'",false);
                            }
                        }

                        //DataTable dtvid = new DataTable();
                        //Database.GetSqlData("SELECT Vi_id FROM Voucherdet WHERE (Itemsr = 2) AND (Vi_id IN (SELECT     Vi_id FROM  Journal WHERE      (Narr = 'Commission Due')))", dtvid);
                        //ProgrBar.Minimum = 0;
                        //ProgrBar.Maximum = ansGridView5.Rows.Count;
                        //ProgrBar.Visible = true;
                        for (int i = 0; i < ansGridView5.Rows.Count; i++)
                        {
                           // ProgrBar.Value = i;
                            string oid = ansGridView5.Rows[i].Cells["Vi_id"].Value.ToString();
                            funs.OpenFrm(this, oid, true);
                        }
                       // Database.CommandExecutor("update Journal set Sno=10002 where narr='Commission Due' and Sno=2 ");

                        //ProgrBar.Value = 0;
                        //ProgrBar.Visible = false;
                        LoadData(gstr, gFrmCaption);








                        MessageBox.Show("Done Successfully");
                    }
                    else
                    {
                        MessageBox.Show("Wrong Password..");
                    }
               // }
            }

            else if (e.Control && e.Alt==false && e.KeyCode == Keys.R)
            {
                LoadData(gstr, gFrmCaption);
            }
            
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
                this.Dispose();
            }
        }

        private void ansGridView5_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (ansGridView5.CurrentCell == null)
            {
                return;
            }

            if (ansGridView5.CurrentCell.OwningColumn.Name == "delet")
            {
                if (ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString() == "")
                {
                    return;
                }
                DialogResult res = MessageBox.Show("Are you sure?", "Confirm", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (res == DialogResult.OK)
                {
                    permission = funs.GetPermissionKey(gstr);
                    if (permission != null)
                    {
                        UsersFeature ob = permission.Where(w => w.FeatureName == "Delete").FirstOrDefault();
                        
                       if (ob != null && ob.SelectedValue == "Days Restricted")
                        {
                            string vdate = Database.GetScalarDate("Select Vdate from Voucherinfo where vi_id='" + ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString() + "' ");
                            ob = permission.Where(w => w.FeatureName == "Delete  Restrictions").FirstOrDefault();
                            double days = double.Parse(ob.SelectedValue.ToString());
                            DateTime dt1 = Database.ldate.AddDays(-1*days);
                            if (dt1 >= DateTime.Parse(vdate))
                            {
                                MessageBox.Show("Dear User You Don't Have Permission to Delete.","",  MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                       else if (ob != null && ob.SelectedValue == "Count Restricted")
                       {

                           string user_id = Database.GetScalarText("Select User_id from Voucherinfo where vi_id='" + ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString() + "'");
                           string vt_id = Database.GetScalarText("Select Vt_id from Voucherinfo where vi_id='" + ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString() + "'");
                           if (Database.user_id != user_id)
                           {
                               return;
                           }

                           int nid = Database.GetScalarInt("Select Nid from Voucherinfo where vi_id='"+ ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString() +"'");

                           int countvou = Database.GetScalarInt("Select count(vi_id) from Voucherinfo where vt_id='"+vt_id+"' and  User_id='" + Database.user_id + "' and  nid>=" + nid );
                           ob = permission.Where(w => w.FeatureName == "Delete  Restrictions").FirstOrDefault();
                         
                           double countres = double.Parse(ob.SelectedValue.ToString());



                           if (countvou > countres)
                           {
                               MessageBox.Show("Dear User You Don't Have Permission to Delete.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                               return;
                           }
                          
                         
                       }

                    }
                    

                    if (Feature.Available("Freeze Transaction").ToUpper() == "NO")
                    {
                        delete(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString());
                    }
                    else
                    {
                        string vdate = Database.GetScalarText("Select Vdate from Voucherinfo where vi_id='" + ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString() + "' ");

                        if (DateTime.Parse(vdate) > DateTime.Parse(Feature.Available("Freeze Transaction")))
                        {
                            delete(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString());
                        }
                        else
                        {
                            MessageBox.Show("Your Voucher is Freezed");
                        }
                    }
                    LoadData(gstr, gFrmCaption);
                }
            }

            else if (ansGridView5.CurrentCell.OwningColumn.Name == "print")
            {
                if (ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString() == "")
                {
                    return;
                }

                frm_printcopy frm = new frm_printcopy("Print", ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString(), funs.Select_vt_id_vid(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString()));
                frm.ShowDialog();
                LoadData(gstr, gFrmCaption);
            }

            else if (ansGridView5.CurrentCell.OwningColumn.Name == "view")
            {
                if (ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString() == "")
                {
                    return;
                }

                frm_printcopy frm = new frm_printcopy("View", ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString(), funs.Select_vt_id_vid(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString()));
                frm.Show();
                LoadData(gstr, gFrmCaption);
            }

            else if (ansGridView5.CurrentCell.OwningColumn.Name == "Edit")
            {
                string vi_id = ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString();
                //if (vi_id == "")
                //{
                //    return;
                //}
                //else
                //{
                //    permission = funs.GetPermissionKey(gstr);
                //    if (permission != null)
                //    {
                //        UsersFeature ob = permission.Where(w => w.FeatureName == "Alter").FirstOrDefault();

                //        if (ob != null && ob.SelectedValue == "Days Restricted")
                //        {
                //            string vdate = Database.GetScalarDate("Select Vdate from Voucherinfo where vi_id='" + ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString() + "' ");
                //            ob = permission.Where(w => w.FeatureName == "Alter Restrictions").FirstOrDefault();
                //            double days = double.Parse(ob.SelectedValue.ToString());
                //            DateTime dt1 = Database.ldate.AddDays(-1 * days);
                //            if (dt1 >= DateTime.Parse(vdate))
                //            {
                //                MessageBox.Show("Dear User You Don't Have Permission to Edit.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //                return;
                //            }
                //        }
                //        else if (ob != null && ob.SelectedValue == "Count Restricted")
                //        {

                //            string user_id = Database.GetScalarText("Select User_id from Voucherinfo where vi_id='" + ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString() + "'");
                //            string vt_id = Database.GetScalarText("Select Vt_id from Voucherinfo where vi_id='" + ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString() + "'");
                //            if (Database.user_id != user_id)
                //            {
                //                MessageBox.Show("Dear User You Don't Have Permission to Edit.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //                return;
                //            }

                //            int nid = Database.GetScalarInt("Select Nid from Voucherinfo where vi_id='" + ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString() + "'");

                //            int countvou = Database.GetScalarInt("Select count(vi_id) from Voucherinfo where vt_id='" + vt_id + "' and  User_id='" + Database.user_id + "' and  nid>=" + nid);
                //            ob = permission.Where(w => w.FeatureName == "Alter Restrictions").FirstOrDefault();

                //            double countres = double.Parse(ob.SelectedValue.ToString());



                //            if (countvou > countres)
                //            {
                //                //MessageBox.Show("Dear User You Don't Have Permission to Edit.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //                //return;
                //            }


                //        }

                //    }
                    
                //}
                if (gstr == "Receipt")
                {
                    if (ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString() == "")
                    {
                        return;
                    }
                    frmCashRec frm = new frmCashRec();
                    frm.recpay = "Receipt";
                    frm.cmdnm = "edit";
                    frm.Text = "Edit Receipt";
                    frm.LoadData(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString(), frm.Text);
                    frm.MdiParent = this.MdiParent;
                    frm.Show();
                }
                else if (gstr == "Payment")
                {
                    if (ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString() == "")
                    {
                        return;
                    }
                    frmCashRec frm = new frmCashRec();
                    frm.recpay = "Payment";
                    frm.cmdnm = "edit";
                    frm.Text = "Edit Payment";
                    frm.LoadData(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString(), frm.Text);
                    frm.MdiParent = this.MdiParent;
                    frm.Show();
                }
                else if (gstr == "Contra")
                {
                    if (ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString() == "")
                    {
                        return;
                    }
                    frmCashRec frm = new frmCashRec();
                    frm.recpay = "Contra";
                    frm.cmdnm = "edit";
                    frm.Text = "Edit Contra";
                    frm.LoadData(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString(), frm.Text);
                    frm.MdiParent = this.MdiParent;
                    frm.Show();
                }
                else if (gstr == "Journal")
                {
                    if (ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString() == "")
                    {
                        return;
                    }
                    frmJournal frm = new frmJournal();
                    frm.LoadData(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString(), "Journal Voucher");
                    frm.MdiParent = this.MdiParent;
                    frm.Show();
                }
                else if (gstr == "Dnote")
                {
                    if (ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString() == "")
                    {
                        return;
                    }
                    frmDebitCredit frm = new frmDebitCredit();
                    frm.dr_cr_note = "Debit Note";
                    frm.cmdnm = "edit";
                    frm.LoadData(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString(), "Debit Note");
                    frm.MdiParent = this.MdiParent;
                    frm.Show();
                }
                else if (gstr == "Cnote")
                {
                    if (ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString() == "")
                    {
                        return;
                    }
                    frmDebitCredit frm = new frmDebitCredit();
                    frm.dr_cr_note = "Credit Note";
                    frm.cmdnm = "edit";
                    frm.LoadData(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString(), "Credit Note");
                    frm.MdiParent = this.MdiParent;
                    frm.Show();
                }
                else if (gstr == "Purchase")
                {
                    if (ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString() == "")
                    {
                        return;
                    }
                    DataTable dtalter = new DataTable();
                    Database.GetSqlData("SELECT VOUCHERTYPE.Type, VOUCHERINFO.Tdtype, VOUCHERTYPE.ExState, VOUCHERTYPE.Unregistered FROM VOUCHERINFO,VOUCHERTYPE WHERE VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id and VOUCHERINFO.Vi_id='" + ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString() + "' ", dtalter);
                    frmTransaction frm = new frmTransaction();
                    frm.LoadData(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString(), dtalter.Rows[0]["Type"].ToString(), bool.Parse(dtalter.Rows[0]["Tdtype"].ToString()), bool.Parse(dtalter.Rows[0]["ExState"].ToString()), bool.Parse(dtalter.Rows[0]["Unregistered"].ToString()));
                    frm.MdiParent = this.MdiParent;
                    frm.Show();
                }
                else if (gstr == "Sale Order")
                {
                    if (ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString() == "")
                    {
                        return;
                    }
                    DataTable dtalter = new DataTable();
                    Database.GetSqlData("SELECT VOUCHERTYPE.Type, VOUCHERINFO.Tdtype, VOUCHERTYPE.ExState, VOUCHERTYPE.Unregistered FROM VOUCHERINFO,VOUCHERTYPE WHERE VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id and VOUCHERINFO.Vi_id='" + ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString() + "' ", dtalter);
                    frmTransaction frm = new frmTransaction();
                    frm.LoadData(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString(), dtalter.Rows[0]["Type"].ToString(), bool.Parse(dtalter.Rows[0]["Tdtype"].ToString()), bool.Parse(dtalter.Rows[0]["ExState"].ToString()), bool.Parse(dtalter.Rows[0]["Unregistered"].ToString()));
                    frm.MdiParent = this.MdiParent;
                    frm.Show();
                }
                else if (gstr == "PWDebitNote")
                {
                    if (ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString() == "")
                    {
                        return;
                    }
                    DataTable dtalter = new DataTable();
                    Database.GetSqlData("SELECT VOUCHERTYPE.Type, VOUCHERINFO.Tdtype, VOUCHERTYPE.ExState, VOUCHERTYPE.Unregistered FROM VOUCHERINFO,VOUCHERTYPE WHERE VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id and VOUCHERINFO.Vi_id='" + ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString() + "' ", dtalter);
                    frmTransaction frm = new frmTransaction();
                    frm.LoadData(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString(), dtalter.Rows[0]["Type"].ToString(), bool.Parse(dtalter.Rows[0]["Tdtype"].ToString()), bool.Parse(dtalter.Rows[0]["ExState"].ToString()), bool.Parse(dtalter.Rows[0]["Unregistered"].ToString()));
                    frm.MdiParent = this.MdiParent;
                    frm.Show();
                }
                else if (gstr == "RCM")
                {
                    if (ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString() == "")
                    {
                        return;
                    }
                    DataTable dtalter = new DataTable();
                    Database.GetSqlData("SELECT VOUCHERTYPE.Type, VOUCHERINFO.Tdtype, VOUCHERTYPE.ExState, VOUCHERTYPE.Unregistered FROM VOUCHERINFO,VOUCHERTYPE WHERE VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id and VOUCHERINFO.Vi_id='" + ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString() + "' ", dtalter);
                    frmTransaction frm = new frmTransaction();
                    frm.LoadData(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString(), dtalter.Rows[0]["Type"].ToString(), bool.Parse(dtalter.Rows[0]["Tdtype"].ToString()), bool.Parse(dtalter.Rows[0]["ExState"].ToString()), bool.Parse(dtalter.Rows[0]["Unregistered"].ToString()));
                    frm.MdiParent = this.MdiParent;
                    frm.Show();
                }
                else if (gstr == "Sale")
                {
                    if (ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString() == "")
                    {
                        return;
                    }
                    DataTable dtalter = new DataTable();
                    Database.GetSqlData("SELECT VOUCHERTYPE.Type, VOUCHERINFO.Tdtype, VOUCHERTYPE.ExState, VOUCHERTYPE.Unregistered FROM VOUCHERINFO,VOUCHERTYPE WHERE VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id and VOUCHERINFO.Vi_id='" + ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString() + "' ", dtalter);
                    frmTransaction frm = new frmTransaction();

                    frm.LoadData(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString(), dtalter.Rows[0]["Type"].ToString(), bool.Parse(dtalter.Rows[0]["Tdtype"].ToString()), bool.Parse(dtalter.Rows[0]["ExState"].ToString()), bool.Parse(dtalter.Rows[0]["Unregistered"].ToString()));
                    frm.MdiParent = this.MdiParent;
                    frm.Show();
                }
                else if (gstr == "P Return")
                {
                    if (ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString() == "")
                    {
                        return;
                    }
                    DataTable dtalter = new DataTable();
                    Database.GetSqlData("SELECT VOUCHERTYPE.Type, VOUCHERINFO.Tdtype, VOUCHERTYPE.ExState, VOUCHERTYPE.Unregistered FROM VOUCHERINFO,VOUCHERTYPE WHERE VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id and VOUCHERINFO.Vi_id='" + ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString() + "' ", dtalter);
                    frmTransaction frm = new frmTransaction();
                    frm.LoadData(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString(), dtalter.Rows[0]["Type"].ToString(), bool.Parse(dtalter.Rows[0]["Tdtype"].ToString()), bool.Parse(dtalter.Rows[0]["ExState"].ToString()), bool.Parse(dtalter.Rows[0]["Unregistered"].ToString()));
                    frm.MdiParent = this.MdiParent;
                    frm.Show();
                }
                else if (gstr == "Return")
                {
                    if (ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString() == "")
                    {
                        return;
                    }
                    DataTable dtalter = new DataTable();
                    Database.GetSqlData("SELECT VOUCHERTYPE.Type, VOUCHERINFO.Tdtype, VOUCHERTYPE.ExState, VOUCHERTYPE.Unregistered FROM VOUCHERINFO,VOUCHERTYPE WHERE VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id and VOUCHERINFO.Vi_id='" + ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString() + "' ", dtalter);

                    frmTransaction frm = new frmTransaction();
                    frm.LoadData(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString(), dtalter.Rows[0]["Type"].ToString(), bool.Parse(dtalter.Rows[0]["Tdtype"].ToString()), bool.Parse(dtalter.Rows[0]["ExState"].ToString()), bool.Parse(dtalter.Rows[0]["Unregistered"].ToString()));
                    frm.MdiParent = this.MdiParent;
                    frm.Show();
                }
                else if (gstr == "receive")
                {
                    if (ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString() == "")
                    {
                        return;
                    }
                    DataTable dtalter = new DataTable();
                    Database.GetSqlData("SELECT VOUCHERTYPE.Type, VOUCHERINFO.Tdtype, VOUCHERTYPE.ExState, VOUCHERTYPE.Unregistered FROM VOUCHERINFO,VOUCHERTYPE WHERE VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id and VOUCHERINFO.Vi_id='" + ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString() + "' ", dtalter);
                    frmTransaction frm = new frmTransaction();
                    frm.LoadData(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString(), dtalter.Rows[0]["Type"].ToString(), bool.Parse(dtalter.Rows[0]["Tdtype"].ToString()), bool.Parse(dtalter.Rows[0]["ExState"].ToString()), bool.Parse(dtalter.Rows[0]["Unregistered"].ToString()));
                    frm.MdiParent = this.MdiParent;
                    frm.Show();
                }
                else if (gstr == "issue")
                {
                    if (ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString() == "")
                    {
                        return;
                    }
                    DataTable dtalter = new DataTable();
                    Database.GetSqlData("SELECT VOUCHERTYPE.Type, VOUCHERINFO.Tdtype, VOUCHERTYPE.ExState, VOUCHERTYPE.Unregistered FROM VOUCHERINFO,VOUCHERTYPE WHERE VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id and VOUCHERINFO.Vi_id='" + ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString() + "' ", dtalter);
                    frmTransaction frm = new frmTransaction();
                    frm.LoadData(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString(), dtalter.Rows[0]["Type"].ToString(), bool.Parse(dtalter.Rows[0]["Tdtype"].ToString()), bool.Parse(dtalter.Rows[0]["ExState"].ToString()), bool.Parse(dtalter.Rows[0]["Unregistered"].ToString()));
                    frm.MdiParent = this.MdiParent;
                    frm.Show();
                }
                else if (gstr == "Transfer")
                {
                    if (ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString() == "")
                    {
                        return;
                    }
                    DataTable dtalter = new DataTable();
                    Database.GetSqlData("SELECT VOUCHERTYPE.Type, VOUCHERINFO.Tdtype, VOUCHERTYPE.ExState, VOUCHERTYPE.Unregistered FROM VOUCHERINFO,VOUCHERTYPE WHERE VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id and VOUCHERINFO.Vi_id='" + ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString() + "' ", dtalter);
                    frm_stkjournal frm = new frm_stkjournal();
                    frm.LoadData(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString(), "Edit Stock Journal");
                    frm.MdiParent = this.MdiParent;
                    frm.Show();
                }
                else if (gstr == "Pending")
                {
                    if (ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString() == "")
                    {
                        return;
                    }
                    DataTable dtalter = new DataTable();
                    Database.GetSqlData("SELECT VOUCHERTYPE.Type, VOUCHERINFO.Tdtype, VOUCHERTYPE.ExState, VOUCHERTYPE.Unregistered FROM VOUCHERINFO,VOUCHERTYPE WHERE VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id and VOUCHERINFO.Vi_id='" + ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString() + "' ", dtalter);
                    frmTransaction frm = new frmTransaction();
                    frm.LoadData(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString(), dtalter.Rows[0]["Type"].ToString(), bool.Parse(dtalter.Rows[0]["Tdtype"].ToString()), bool.Parse(dtalter.Rows[0]["ExState"].ToString()), bool.Parse(dtalter.Rows[0]["Unregistered"].ToString()));
                    frm.MdiParent = this.MdiParent;
                    frm.Show();
                }
                else if (gstr == "Opening")
                {
                    if (ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString() == "")
                    {
                        return;
                    }
                    DataTable dtalter = new DataTable();
                    Database.GetSqlData("SELECT VOUCHERTYPE.Type, VOUCHERINFO.Tdtype, VOUCHERTYPE.ExState, VOUCHERTYPE.Unregistered FROM VOUCHERINFO,VOUCHERTYPE WHERE VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id and VOUCHERINFO.Vi_id='" + ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString() + "' ", dtalter);
                    frmTransaction frm = new frmTransaction();
                    frm.LoadData(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString(), dtalter.Rows[0]["Type"].ToString(), bool.Parse(dtalter.Rows[0]["Tdtype"].ToString()), bool.Parse(dtalter.Rows[0]["ExState"].ToString()), bool.Parse(dtalter.Rows[0]["Unregistered"].ToString()));
                    frm.MdiParent = this.MdiParent;
                    frm.Show();
                }
            }
        }

        private void delete(string vid)
        {
            DataTable dttemp;

            if (gstr == "Receipt" || gstr == "Payment" || gstr == "Journal" || gstr=="Contra")
            {
                dttemp = new DataTable("voucheractotal");
                Database.GetSqlData("Select * from voucheractotal where vi_id='" + vid+"' ", dttemp);
                for (int i = 0; i < dttemp.Rows.Count; i++)
                {
                    dttemp.Rows[i].Delete();
                }
                Database.SaveData(dttemp);
            }

            else
            {
                dttemp = new DataTable("Voucherdet");
                Database.GetSqlData("Select * from Voucherdet where vi_id='" + vid+"' ", dttemp);
                for (int i = 0; i < dttemp.Rows.Count; i++)
                {
                    dttemp.Rows[i].Delete();
                }
                Database.SaveData(dttemp);


                dttemp = new DataTable("Voucherpaydet");
                Database.GetSqlData("Select * from Voucherpaydet where vi_id='" + vid + "' ", dttemp);
                for (int i = 0; i < dttemp.Rows.Count; i++)
                {
                    dttemp.Rows[i].Delete();
                }
                Database.SaveData(dttemp);

                dttemp = new DataTable("Stock");
                Database.GetSqlData("Select * from Stock where vid='" + vid+"' ", dttemp);
                for (int i = 0; i < dttemp.Rows.Count; i++)
                {
                    dttemp.Rows[i].Delete();
                }
                Database.SaveData(dttemp);

                dttemp = new DataTable("Voucharges");
                Database.GetSqlData("Select * from Voucharges where vi_id='" + vid+"' ", dttemp);
                for (int i = 0; i < dttemp.Rows.Count; i++)
                {
                    dttemp.Rows[i].Delete();
                }
                Database.SaveData(dttemp);
            }

            dttemp = new DataTable("BILLBYBILL");
            Database.GetSqlData("Select * from BILLBYBILL where receive_id='" + vid+"' ", dttemp);
            for (int i = 0; i < dttemp.Rows.Count; i++)
            {
                dttemp.Rows[i].Delete();
            }
            Database.SaveData(dttemp);

            dttemp = new DataTable("BILLBYBILL");
            Database.GetSqlData("Select * from BILLBYBILL where Bill_id='" + vid+"' ", dttemp);
            for (int i = 0; i < dttemp.Rows.Count; i++)
            {
                dttemp.Rows[i].Delete();
            }
            Database.SaveData(dttemp);
            dttemp = new DataTable("Billadjest");
            Database.GetSqlData("Select * from Billadjest where Vi_id='" + vid + "' ", dttemp);
            for (int i = 0; i < dttemp.Rows.Count; i++)
            {
                dttemp.Rows[i].Delete();
            }
            Database.SaveData(dttemp);
            dttemp = new DataTable("Billadjest");
            Database.GetSqlData("Select * from Billadjest where Reff_id='" + vid + "' ", dttemp);
            for (int i = 0; i < dttemp.Rows.Count; i++)
            {
                dttemp.Rows[i].Delete();
            }
            Database.SaveData(dttemp);
           
            dttemp = new DataTable("itemcharges");
            Database.GetSqlData("Select * from itemcharges where vi_id='" + vid+"' ", dttemp);
            for (int i = 0; i < dttemp.Rows.Count; i++)
            {
                dttemp.Rows[i].Delete();
            }
            Database.SaveData(dttemp);

            dttemp = new DataTable("VoucherInfo");
            Database.GetSqlData("Select * from VoucherInfo where vi_id='" + vid+"' ", dttemp);
            for (int i = 0; i < dttemp.Rows.Count; i++)
            {
                dttemp.Rows[i].Delete();
            }
            Database.SaveData(dttemp);

            dttemp = new DataTable("Journal");
            Database.GetSqlData("Select * from Journal where vi_id='" + vid+"' ", dttemp);
            for (int i = 0; i < dttemp.Rows.Count; i++)
            {
                dttemp.Rows[i].Delete();
            }
            Database.SaveData(dttemp);

            MessageBox.Show("Deleted successfully");
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            LoadData(gstr, gFrmCaption);
        }

        private void frmMasterVou_Load(object sender, EventArgs e)
        {
            SideFill();
            ansGridView5.Columns["Vdate"].DisplayIndex = 0;
            ansGridView5.Columns["Name"].DisplayIndex = 1;
            ansGridView5.Columns["AccName"].DisplayIndex = 2;
            ansGridView5.Columns["Vnumber"].DisplayIndex = 3;
            ansGridView5.Columns["Amount"].DisplayIndex = 4;
            ansGridView5.Columns["Usr"].DisplayIndex = 5;
            ansGridView5.Columns["PrintCount"].DisplayIndex = 6;
            ansGridView5.Columns["Vi_id"].DisplayIndex = 12;
            ansGridView5.Columns["Entered"].DisplayIndex = 7;
            ansGridView5.Columns["Modified_By"].DisplayIndex = 8;
            ansGridView5.Columns["Approved_By"].DisplayIndex = 9;
            ansGridView5.Columns["view"].DisplayIndex = 10;
            ansGridView5.Columns["print"].DisplayIndex = 11;
            ansGridView5.Columns["Edit"].DisplayIndex = 12;
            ansGridView5.Columns["Delet"].DisplayIndex = 13;

            ansGridView5.Columns["Vi_id"].Visible = false;
            ansGridView5.Columns["usr"].Visible = false;

            if (gstr == "Opening")
            {
                dateTimePicker1.CustomFormat = Database.dformat;
                dateTimePicker2.CustomFormat = Database.dformat;
                dateTimePicker1.MinDate = Database.stDate.AddDays(-1);
                dateTimePicker1.MaxDate = Database.stDate.AddDays(-1);
                dateTimePicker2.MinDate = Database.stDate.AddDays(-1);
                dateTimePicker2.MaxDate = Database.stDate.AddDays(-1);
                dateTimePicker1.Value = Database.stDate.AddDays(-1);
                dateTimePicker2.Value = Database.stDate.AddDays(-1);
                groupBox3.Visible = false;
                groupBox4.Visible = false;
                button3.Visible = false;                
            }
            else
            {
                dateTimePicker1.CustomFormat = Database.dformat;
                dateTimePicker2.CustomFormat = Database.dformat;
                dateTimePicker1.MinDate = Database.stDate;
                dateTimePicker1.MaxDate = Database.ldate;
                dateTimePicker2.MinDate = Database.stDate;
                dateTimePicker2.MaxDate = Database.ldate;
                dateTimePicker1.Value = Database.ldate;
                dateTimePicker2.Value = Database.ldate;
            }
        }

        private void frmMasterVou_Enter(object sender, EventArgs e)
        {
            this.Size = this.MdiParent.Size;
            this.WindowState = FormWindowState.Maximized;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            LoadData(gstr, gFrmCaption);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            filter();
        }

        private void filter()
        {
            String strTemp = textBox1.Text;
            strTemp = strTemp.Replace("%", "?");
            strTemp = strTemp.Replace("[", string.Empty);
            strTemp = strTemp.Replace("]", string.Empty);
            string strfilter = "";            
            int a = 0;
            a = dtvou.Columns.Count;

            for (int i = 0; i < dtvou.Columns.Count; i++)
            {
                if (dtvou.Columns[i].ColumnName == "Vnumber")
                {
                    if (strfilter != "")
                    {
                        strfilter += " or ";
                    }
                    strfilter += "(" + dtvou.Columns[i].ColumnName + " like '*" + strTemp + "*' " + ")";
                }
                bs.Filter = null;
                bs.DataSource = dtvou;
                bs.Filter = strfilter;
            }           
        }
    }
}
