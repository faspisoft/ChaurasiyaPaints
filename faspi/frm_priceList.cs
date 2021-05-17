using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using Microsoft.Office.Core;
using Excel = Microsoft.Office.Interop.Excel;

namespace faspi
{
    public partial class frm_priceList : Form
    {
        public frm_priceList()
        {
            InitializeComponent();
        }

        private void frm_priceList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
                this.Dispose();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string sql = "";
            if (textBox8.Text == "")
            {
                MessageBox.Show("Enter Price");
                return;
            }
            if (textBox10.Text != "")
            {
                sql += " and OTHER_1.Name = '" + textBox10.Text + "'";
            }

            string rate = funs.Select_Rates_Id(textBox8.Text);
            if (textBox8.Text == "MRP")
            {
                rate = "MRP";
            }

            if (checkBox1.Checked == true)
            {
                sql = "";
                if (textBox10.Text != "")
                {
                    sql = sql + " AND (CASE WHEN OTHER_3.Name IS NULL THEN '<UNCLASSIFIED>' ELSE OTHER_3.Name END = '" + textBox10.Text + "')";
                }
                DataTable dt = new DataTable();
                string str = "SELECT CASE WHEN OTHER_3.Name IS NULL THEN '<UNCLASSIFIED>' ELSE OTHER_3.Name END AS company, CASE WHEN OTHER_1.Name IS NULL  THEN '<UNCLASSIFIED>' ELSE OTHER_1.Name END AS Item, CASE WHEN OTHER_2.Name IS NULL THEN '-' ELSE OTHER_2.Name END AS Price,  CASE WHEN OTHER.Name IS NULL THEN '<UNCLASSIFIED>' ELSE OTHER.Name END AS color, Description.Pack, MAX(" + rate + ") AS rate,  MAX(Description.Pvalue) AS Pvalue FROM         Description LEFT OUTER JOIN OTHER ON Description.Col_id = OTHER.Oth_id LEFT OUTER JOIN OTHER AS OTHER_3 ON Description.Company_id = OTHER_3.Oth_id LEFT OUTER JOIN OTHER AS OTHER_2 ON Description.Group_id = OTHER_2.Oth_id LEFT OUTER JOIN OTHER AS OTHER_1 ON Description.Item_id = OTHER_1.Oth_id GROUP BY CASE WHEN OTHER_1.Name IS NULL THEN '<UNCLASSIFIED>' ELSE OTHER_1.Name END, Description.Pack, CASE WHEN OTHER_2.Name IS NULL  THEN '-' ELSE OTHER_2.Name END, CASE WHEN OTHER.Name IS NULL THEN '<UNCLASSIFIED>' ELSE OTHER.Name END, CASE WHEN OTHER_3.Name IS NULL  THEN '<UNCLASSIFIED>' ELSE OTHER_3.Name END, CASE WHEN OTHER.Name IS NULL THEN '<UNCLASSIFIED>' ELSE OTHER.Name END HAVING      (MAX(" + rate + ") <> 0) " + sql + " ORDER BY Item, CASE WHEN OTHER_2.Name IS NULL THEN '-' ELSE OTHER_2.Name END";
                //string str = "SELECT CASE WHEN OTHER_3.Name IS NULL THEN '<UNCLASSIFIED>' ELSE OTHER_3.Name END AS company, CASE WHEN OTHER_1.Name IS NULL  THEN '<UNCLASSIFIED>' ELSE OTHER_1.Name END AS Item, CASE WHEN OTHER_2.Name IS NULL THEN '<UNCLASSIFIED>' ELSE OTHER_2.Name END AS Price,  CASE WHEN OTHER.Name IS NULL THEN '<UNCLASSIFIED>' ELSE OTHER.Name END AS color, Description.Pack, MAX(" + funs.Select_Rates_Id(textBox8.Text) + ") AS rate FROM         Description LEFT OUTER JOIN OTHER ON Description.Col_id = OTHER.Oth_id LEFT OUTER JOIN OTHER AS OTHER_3 ON Description.Company_id = OTHER_3.Oth_id LEFT OUTER JOIN OTHER AS OTHER_2 ON Description.Group_id = OTHER_2.Oth_id LEFT OUTER JOIN OTHER AS OTHER_1 ON Description.Item_id = OTHER_1.Oth_id GROUP BY CASE WHEN OTHER_1.Name IS NULL THEN '<UNCLASSIFIED>' ELSE OTHER_1.Name END, Description.Pack, CASE WHEN OTHER_2.Name IS NULL  THEN '<UNCLASSIFIED>' ELSE OTHER_2.Name END, CASE WHEN OTHER.Name IS NULL THEN '<UNCLASSIFIED>' ELSE OTHER.Name END,  CASE WHEN OTHER_3.Name IS NULL THEN '<UNCLASSIFIED>' ELSE OTHER_3.Name END, CASE WHEN OTHER.Name IS NULL  THEN '<UNCLASSIFIED>' ELSE OTHER.Name END HAVING (MAX(" + funs.Select_Rates_Id(textBox8.Text) + ") <> 0) " + sql + " ORDER BY Item, Price";
                Database.GetSqlData(str, dt);
                DataTable dtf = dt.DefaultView.ToTable(true, "Item", "Price", "Pack", "rate","Pvalue");

                
                PriceList(dtf, rate,textBox10.Text);
            }
            else
            {
                if (sql != "")
                {
                    sql = sql.Remove(0, 4);
                    sql = "WHERE" + sql;
                }
                Report gg = new Report();
                gg.MdiParent = this.MdiParent;
                gg.PriceListNew(Database.ldate, Database.ldate, sql, rate);
                gg.Show();
                this.Close();
                this.Dispose();
            }
        }

        private void PriceList(DataTable dt, string rate,string cmpny)
        {
            Object misValue = System.Reflection.Missing.Value;
            Excel.Application apl = new Microsoft.Office.Interop.Excel.Application();
            Excel.Workbook wb = (Excel.Workbook)apl.Workbooks.Add(misValue);
            Excel.Worksheet ws;
            ws = (Excel.Worksheet)wb.Worksheets[1];
            int lno = 1;
            DataTable dtExcel = new DataTable();
            DataTable dtRheader = new DataTable();

            Database.GetSqlData("select * from company", dtRheader);
            int SheetWeidht = 9;
            ws.Cells[lno, 1] = dtRheader.Rows[0]["name"].ToString();
            ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, SheetWeidht]).Merge(Type.Missing);
            ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, SheetWeidht]).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, SheetWeidht]).Font.Bold = true;
            lno++;

            ws.Cells[lno, 1] = dtRheader.Rows[0]["Address1"].ToString();
            ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, SheetWeidht]).Merge(Type.Missing);
            ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, SheetWeidht]).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, SheetWeidht]).Font.Bold = true;
            lno++;

            ws.Cells[lno, 1] = dtRheader.Rows[0]["Address2"].ToString();
            ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, SheetWeidht]).Merge(Type.Missing);
            ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, SheetWeidht]).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, SheetWeidht]).Font.Bold = true;
            lno++;

            ws.Cells[lno, 1] = "Price List Date : " + Database.ldate.ToString(Database.dformat);
            ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, SheetWeidht]).Merge(Type.Missing);
            ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, SheetWeidht]).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, SheetWeidht]).Font.Bold = true;
            lno++;

            ws.Cells[lno, 1] = cmpny + " - " + rate;
            ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, SheetWeidht]).Merge(Type.Missing);
            ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, SheetWeidht]).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, SheetWeidht]).Font.Bold = true;
            lno++;

            ws.get_Range("a:a").ColumnWidth = 30;
            ws.get_Range("b:i").ColumnWidth = 6;
            ws.get_Range("b:i").HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
            lno++;

            DataTable distinctGroup = dt.DefaultView.ToTable(true, "Item");

            for (int i = 0; i < distinctGroup.Rows.Count; i++)
            {
                //print Group
                ws.Cells[lno, 1] = distinctGroup.Rows[i][0].ToString();
                ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, SheetWeidht]).Font.Bold = true;
                ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, SheetWeidht]).Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);
                ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, SheetWeidht]).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.MediumSlateBlue);

                DataTable dt1 = dt.Select("Item='" + distinctGroup.Rows[i][0].ToString() + "'").CopyToDataTable();
                DataTable distinctDesc = dt1.DefaultView.ToTable(true, "Price");
                DataTable distinctPack = dt1.DefaultView.ToTable(true, "Pack");

                distinctPack.Columns.Add("Pvalue", typeof(double));

                for (int j = 0; j < distinctPack.Rows.Count; j++)
                {
                    distinctPack.Rows[j]["Pvalue"] = dt1.Compute("max(Pvalue)", "Pack='" + distinctPack.Rows[j]["Pack"].ToString() + "'");
                }

                DataView view = distinctPack.DefaultView;
                view.Sort = "Pvalue DESC";
                distinctPack = view.ToTable();

                for (int j = 0; j < distinctPack.Rows.Count; j++)
                {
                    //Print Avalable Pack Size
                    ws.Cells[lno, j + 2] = distinctPack.Rows[j]["Pack"].ToString();
                }
                lno++;

                for (int j = 0; j < distinctDesc.Rows.Count; j++)
                {
                    DataTable dt2 = dt1.Select("Price='" + distinctDesc.Rows[j][0].ToString() + "'").CopyToDataTable();

                    //Print Description
                    ws.Cells[lno, 1] = distinctDesc.Rows[j]["Price"].ToString();

                    for (int k = 0; k < distinctPack.Rows.Count; k++)
                    {
                        //print Stock
                        if (dt2.Select("Pack='" + distinctPack.Rows[k][0].ToString() + "'").Length > 0)
                        {
                            ws.Cells[lno, k + 2] = dt2.Select("Pack='" + distinctPack.Rows[k][0].ToString() + "'")[0]["rate"].ToString();
                        }
                    }

                    lno++;
                }
            }

            Excel.Range last = ws.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell, Type.Missing);
            ws.get_Range("A1", last).WrapText = true;
            apl.Visible = true;
        }

        private void textBox10_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox10);
        }

        private void textBox10_KeyPress(object sender, KeyPressEventArgs e)
        {
           string strCombo = "select [name] from other where Type='" + funs.Get_Company_id() + "' order by [name]";
            textBox10.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
        }

        private void textBox10_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox10);
        }

        private void textBox8_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox8);
        }

        private void textBox8_KeyPress(object sender, KeyPressEventArgs e)
        {
            DataTable dtcombo = new DataTable();
            dtcombo.Columns.Add("PriceList", typeof(string));
            if (Feature.Available("Name of PriceList1")!="Purchase Rate")
            {


                dtcombo.Rows.Add();
                dtcombo.Rows[dtcombo.Rows.Count-1][0] = Feature.Available("Name of PriceList1");
            }
            dtcombo.Rows.Add();
            dtcombo.Rows[dtcombo.Rows.Count - 1][0] = Feature.Available("Name of PriceList2");
            dtcombo.Rows.Add();
            dtcombo.Rows[dtcombo.Rows.Count - 1][0] = Feature.Available("Name of PriceList3");
            dtcombo.Rows.Add();
            dtcombo.Rows[dtcombo.Rows.Count - 1][0] = Feature.Available("Name of PriceList4");
            dtcombo.Rows.Add();
            dtcombo.Rows[dtcombo.Rows.Count - 1][0] = Feature.Available("Name of PriceList5");
            dtcombo.Rows.Add();
            dtcombo.Rows[dtcombo.Rows.Count - 1][0] = Feature.Available("Name of PriceList6");
            dtcombo.Rows.Add();
            dtcombo.Rows[dtcombo.Rows.Count - 1][0] = "MRP";
            textBox8.Text = SelectCombo.ComboDt(this, dtcombo, 0);
        }

        private void textBox8_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox8);
        }

    }
}
