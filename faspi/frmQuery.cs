using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Office.Core;
using Excel = Microsoft.Office.Interop.Excel;

namespace faspi
{
    public partial class frmQuery : Form
    {
        Database  Database = new Database();
        DataTable dtQuery = new DataTable();
        Object misValue = System.Reflection.Missing.Value;
        Excel.Application apl = new Microsoft.Office.Interop.Excel.Application();
        Excel.Workbook wb;

        public frmQuery()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            String qry = "";
            if (Database.databaseName == "")
            {
                qry = textBox1.Text.Trim();
                if (qry.Substring(0, 6).ToUpper() == "SELECT")
                {
                    wb = (Excel.Workbook)apl.Workbooks.Add(misValue);
                    Excel.Worksheet ws = (Excel.Worksheet)wb.Worksheets[1];
                    disp(qry, ref ws);
                    apl.Visible = true;
                }

                else if (qry.Substring(0, 6).ToUpper() == "INSERT" || qry.Substring(0, 6).ToUpper() == "UPDATE")
                {
                    Database.CommandExecutorOther(qry);
                    textBox1.Text = "";
                }

                else if (qry.Substring(0, 6).ToUpper() == "DELETE")
                {
                    DialogResult res = MessageBox.Show("Are you sure?", "Confirm", MessageBoxButtons.OKCancel);
                    if (res == DialogResult.OK)
                    {
                        Database.CommandExecutorOther(qry);
                        textBox1.Text = "";
                    }
                }

               



                return;
            }

         
                Database.OpenConnection();
                qry = textBox1.Text.Trim();

            
                if (qry.ToUpper() == "GST")
                {
                    frm_NewCompany frm = new frm_NewCompany();
                    frm.frmMenuTyp = "GST";
                    frm.NewFinancial("New Fianncial Year");
                    frm.ShowDialog();

                    //Form[] frms = this.MdiChildren;
                    //foreach (Form frm1 in frms)
                    //{
                    //    frm1.Dispose();
                    //}

                   
                    //this.Text = Database.fname + "[" + Database.fyear + "]";
                }
                else  if (qry.Substring(0, 6).ToUpper() == "SELECT")
                {
                    wb = (Excel.Workbook)apl.Workbooks.Add(misValue);
                    Excel.Worksheet ws = (Excel.Worksheet)wb.Worksheets[1];
                    disp(qry, ref ws);
                    apl.Visible = true;
                }

                else if (qry.Substring(0, 6).ToUpper() == "INSERT" || qry.Substring(0, 6).ToUpper() == "UPDATE")
                {
                    Database.CommandExecutor(qry);
                    textBox1.Text = "";
                }

                else if (qry.Substring(0, 6).ToUpper() == "DELETE")
                {
                    DialogResult res = MessageBox.Show("Are you sure?", "Confirm", MessageBoxButtons.OKCancel);
                    if (res == DialogResult.OK)
                    {
                        Database.CommandExecutor(qry);
                        textBox1.Text = "";
                    }
                }


            this.Close();
            Database.CloseConnection();
        }

        private void disp(String qry, ref Excel.Worksheet ws)
        {
            dtQuery.Clear();

            if (Database.databaseName != "")
            {
                Database.GetSqlData(qry, dtQuery);
            }
            else
            {
                Database.GetOtherSqlData(qry, dtQuery);
            }
                int colNum = dtQuery.Columns.Count;
                int rowNum = dtQuery.Rows.Count;
                for (int i = 0; i < dtQuery.Rows.Count; i++)
                {
                    ws.get_Range("a" + (i + 1) + ":" + (char)(65 + colNum - 1) + (i + 1), misValue).Value2 = dtQuery.Rows[i].ItemArray;

                }
           
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
