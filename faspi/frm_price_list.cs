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
    public partial class frm_price_list : Form
    {
        string strCombo = "";
        public string typ = "";

        public frm_price_list()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string sql = "", sql2 = "" ;
            if (textBox4.Text != "")
            {
                sql += " And (CASE WHEN ACCOUNT.Name IS NULL THEN '<MAIN>' ELSE ACCOUNT.Name END = '" + textBox4.Text + "')";
            }
            if (textBox1.Text != "")
            {
                sql2 += " And Voucherinfo.Branch_id='" + funs.Select_branch_id(textBox1.Text)+"'";
            }
            if (textBox10.Text != "")
            {
                sql += " and OTHER_1.Name = '" + textBox10.Text + "'";
            }
            if (textBox11.Text != "")
            {
                sql += " and OTHER_4.Name = '" + textBox11.Text + "'";
            }
            if (textBox13.Text != "")
            {
                sql += " and OTHER_2.Name = '" + textBox13.Text + "'";
            }
            if (textBox5.Text != "")
            {
                sql += " and OTHER_3.Name = '" + textBox5.Text + "'";
            }
            if (textBox2.Text != "")
            {
                sql += " and Description.Description = '" + textBox2.Text + "'";
            }
            if (textBox7.Text != "")
            {
                sql += " and Description.Pack = '" + textBox7.Text + "'";
            }

            if (checkBox1.Checked == true)
            {
                if (sql != "")
                {
                    sql = sql.Remove(0, 4);
                    sql = "HAVING" + sql;
                }
                Reorder(sql,sql2);
            }
            else
            {
                if (sql != "")
                {
                    sql = sql.Remove(0, 4);
                    sql = "HAVING" + sql;
                }
                Report gg = new Report();
                gg.MdiParent = this.MdiParent;
                gg.Reorder(dateTimePicker1.Value, dateTimePicker2.Value, sql,sql2);
                gg.Show();
                this.Close();
                this.Dispose();
            }                    
        }

        private void Reorder(string str,string str2)
        {
            DataTable dt = new DataTable();
            string qry = "SELECT CASE WHEN ACCOUNT.Name IS NULL THEN '<MAIN>' ELSE ACCOUNT.Name END AS godown,    OTHER_1.Name AS company, OTHER_4.Name AS item, OTHER_2.Name AS color, OTHER_3.Name AS price,  Description.Description,  Description.Pack,  Description.Max_level,  Description.Wlavel,  Description.box_quantity, SUM( Stock.Receive -  Stock.Issue) AS stock, MAX( Description.Pvalue)   AS Pvalue FROM  VOUCHERINFO RIGHT OUTER JOIN   Stock ON  VOUCHERINFO.Vi_id =  Stock.Vid LEFT OUTER JOIN  Description ON  Stock.Did =  Description.Des_id LEFT OUTER JOIN   OTHER AS OTHER_3 ON  Description.Group_id = OTHER_3.Oth_id LEFT OUTER JOIN   OTHER AS OTHER_2 ON  Description.Col_id = OTHER_2.Oth_id LEFT OUTER JOIN   OTHER AS OTHER_4 ON  Description.Item_id = OTHER_4.Oth_id LEFT OUTER JOIN   OTHER AS OTHER_1 ON  Description.Company_id = OTHER_1.Oth_id LEFT OUTER JOIN   ACCOUNT ON  Description.Godown_id =  ACCOUNT.Ac_id WHERE  ( VOUCHERINFO.Vdate <='"+ dateTimePicker2.Value.Date.ToString(Database.dformat)+"') "+str2+" OR   ( VOUCHERINFO.Vdate IS NULL) GROUP BY  OTHER_1.Name, OTHER_4.Name, OTHER_2.Name, OTHER_3.Name,  Description.Description,  Description.Pack,    Description.Max_level,  Description.Wlavel,  Description.box_quantity, CASE WHEN ACCOUNT.Name IS NULL    THEN '<MAIN>' ELSE ACCOUNT.Name END " + str + " ORDER BY godown, company, item, color, price,  Description.Description,  Description.Pack";
          //  string qry = "SELECT CASE WHEN ACCOUNT.Name IS NULL THEN '<MAIN>' ELSE ACCOUNT.Name END AS godown, OTHER.Name AS department, OTHER_1.Name AS company, OTHER_4.Name AS item, OTHER_2.Name AS color, OTHER_3.Name AS price, Description.Description, Description.Pack, Description.Max_level, Description.Wlavel, Description.box_quantity, SUM(Stock.Receive - Stock.Issue) AS stock, MAX(Description.Pvalue) AS Pvalue FROM Description RIGHT OUTER JOIN Stock ON Description.Des_id = Stock.Did LEFT OUTER JOIN OTHER ON Description.Department_id = OTHER.Oth_id LEFT OUTER JOIN OTHER AS OTHER_3 ON Description.Group_id = OTHER_3.Oth_id LEFT OUTER JOIN OTHER AS OTHER_2 ON Description.Col_id = OTHER_2.Oth_id LEFT OUTER JOIN OTHER AS OTHER_4 ON Description.Item_id = OTHER_4.Oth_id LEFT OUTER JOIN OTHER AS OTHER_1 ON Description.Company_id = OTHER_1.Oth_id LEFT OUTER JOIN ACCOUNT ON Description.Godown_id = ACCOUNT.Ac_id GROUP BY OTHER.Name, OTHER_1.Name, OTHER_4.Name, OTHER_2.Name, OTHER_3.Name, Description.Description, Description.Pack, Description.Max_level, Description.Wlavel, Description.box_quantity, CASE WHEN ACCOUNT.Name IS NULL THEN '<MAIN>' ELSE ACCOUNT.Name END " + str + " ORDER BY godown, department, company, item, color, price, Description.Description, Description.Pack";
           
            Database.GetSqlData(qry, dt);
            dt.Columns.Add("reorder", typeof(double));

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["item"].ToString() == "")
                {
                    dt.Rows[i]["item"] = "<UNCLASSIFIED>";
                }

                dt.Rows[i]["reorder"] = "0";
                if (double.Parse(dt.Rows[i]["Max_level"].ToString()) > 0 && double.Parse(dt.Rows[i]["Wlavel"].ToString()) > 0)
                {
                    if (double.Parse(dt.Rows[i]["Wlavel"].ToString()) > double.Parse(dt.Rows[i]["stock"].ToString()))
                    {
                        double max = 0, reorder1 = 0, reorder2 = 0;
                        max = double.Parse(dt.Rows[i]["Max_level"].ToString());
                        reorder1 = max - double.Parse(dt.Rows[i]["stock"].ToString());
                        double qty = reorder1 / double.Parse(dt.Rows[i]["box_quantity"].ToString());
                        string[] st = qty.ToString().Split('.');
                        reorder2 = double.Parse(st[0].ToString()) * double.Parse(dt.Rows[i]["box_quantity"].ToString());
                        dt.Rows[i]["reorder"] = reorder2.ToString();
                    }
                }
            }

            DataRow[] drow;
            drow = dt.Select("reorder<>0");
            DataTable tdt = new DataTable();
            if (drow.GetLength(0) > 0)
            {
                tdt = drow.CopyToDataTable();
            }
            if (tdt.Rows.Count == 0)
            {
                return;
            }


            tdt.DefaultView.Sort = "company, item, color, price, Description";
            tdt = tdt.DefaultView.ToTable();
            DataTable dtfn = tdt.DefaultView.ToTable(true, "item", "Description", "Pack", "reorder","Pvalue");

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

            ws.Cells[lno, 1] = "Reorder Management Date : " + Database.ldate.ToString(Database.dformat);
            ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, SheetWeidht]).Merge(Type.Missing);
            ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, SheetWeidht]).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, SheetWeidht]).Font.Bold = true;
            lno++;

            ws.get_Range("a:a").ColumnWidth = 30;
            ws.get_Range("b:i").ColumnWidth = 6;
            ws.get_Range("b:i").HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
            lno++;

            DataTable distinctGroup = tdt.DefaultView.ToTable(true, "item");
            distinctGroup.DefaultView.Sort = "item";
            distinctGroup = distinctGroup.DefaultView.ToTable();
            for (int i = 0; i < distinctGroup.Rows.Count; i++)
            {
                //print Group
                ws.Cells[lno, 1] = distinctGroup.Rows[i][0].ToString();
                ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, SheetWeidht]).Font.Bold = true;
                ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, SheetWeidht]).Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);
                ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, SheetWeidht]).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.MediumSlateBlue);

                DataTable dt1 = tdt.Select("item='" + distinctGroup.Rows[i][0].ToString() + "'").CopyToDataTable();
                DataTable distinctDesc = dt1.DefaultView.ToTable(true, "Description");
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
                    DataTable dt2 = dt1.Select("Description='" + distinctDesc.Rows[j][0].ToString() + "'").CopyToDataTable();

                    //Print Description
                    ws.Cells[lno, 1] = distinctDesc.Rows[j]["Description"].ToString();

                    for (int k = 0; k < distinctPack.Rows.Count; k++)
                    {
                        //print Stock
                        if (dt2.Select("Pack='" + distinctPack.Rows[k][0].ToString() + "'").Length > 0)
                        {
                            ws.Cells[lno, k + 2] = dt2.Select("Pack='" + distinctPack.Rows[k][0].ToString() + "'")[0]["reorder"].ToString();
                        }
                    }
                    lno++;
                }
            }

            Excel.Range last = ws.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell, Type.Missing);
            ws.get_Range("A1", last).WrapText = true;
            apl.Visible = true;
        }        

        private void frm_price_list_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
                this.Dispose();
            }
        }

        private void frm_price_list_Load(object sender, EventArgs e)
        {
            dateTimePicker1.Value = Database.ldate;
            dateTimePicker1.MaxDate = Database.ldate;
            dateTimePicker1.MinDate = Database.stDate;
            dateTimePicker1.CustomFormat = Database.dformat;
            dateTimePicker2.Value = Database.ldate;
            dateTimePicker2.MaxDate = Database.ldate;
            dateTimePicker2.MinDate = Database.stDate;
            dateTimePicker2.CustomFormat = Database.dformat;
            this.Text = "Price List";
        }

        private void dateTimePicker1_Enter(object sender, EventArgs e)
        {
            Database.setFocus(dateTimePicker1);
        }

        private void dateTimePicker1_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void dateTimePicker1_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(dateTimePicker1);
        }

        private void dateTimePicker2_Enter(object sender, EventArgs e)
        {
            Database.setFocus(dateTimePicker2);
        }

        private void dateTimePicker2_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void dateTimePicker2_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(dateTimePicker2);
        }

       

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }

      

        private void textBox1_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox1);
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            strCombo = "SELECT Bname as BranchName from Branch order by Bname";
            textBox1.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox1);
        }

        private void textBox4_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox4);
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            strCombo = "select distinct '<MAIN>' as name from account union all SELECT ACCOUNT.Name as name FROM ACCOUNT LEFT JOIN ACCOUNTYPE ON ACCOUNT.Act_id = ACCOUNTYPE.Act_id WHERE ACCOUNTYPE.Name='Godown' and Account.Branch_id='" + Database.BranchId + "' GROUP BY ACCOUNT.Name";
            textBox4.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 1);
        }

        private void textBox4_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox4);
        }

        private void textBox10_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox10);
        }

        private void textBox10_KeyPress(object sender, KeyPressEventArgs e)
        {
            strCombo = "select [name] from other where Type='" + funs.Get_Company_id() + "' order by [name]";
            textBox10.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
        }

        private void textBox10_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox10);
        }

        private void textBox11_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox11);
        }

        private void textBox11_KeyPress(object sender, KeyPressEventArgs e)
        {
            strCombo = "select [name] from other where Type='" + funs.Get_Item_id() + "' order by [name]";
            textBox11.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
        }

        private void textBox11_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox11);
        }

        private void textBox13_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox13);
        }

        private void textBox13_KeyPress(object sender, KeyPressEventArgs e)
        {
            strCombo = "select [name] from other where Type='" + funs.Get_Colour_id() + "' order by [name]";
            textBox13.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
        }

        private void textBox13_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox13);
        }

        private void textBox5_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox5);
        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            strCombo = "select [name] from other where Type='" + funs.Get_Group_id() + "' order by [name]";
            textBox5.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
        }

        private void textBox5_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox5);
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox2);
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            strCombo = "SELECT DISTINCT Description FROM Description ORDER BY Description";
            textBox2.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox2);
        }

        private void textBox7_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox7);
        }

        private void textBox7_KeyPress(object sender, KeyPressEventArgs e)
        {
            strCombo = "SELECT DISTINCT Pack as Packing FROM Description ORDER BY Packing";
            textBox7.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
        }

        private void textBox7_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox7);
        }

        private void textBox8_KeyPress(object sender, KeyPressEventArgs e)
        {
            DataTable dtcombo = new DataTable();
            dtcombo.Columns.Add("PriceList", typeof(string));
            dtcombo.Rows.Add();
            dtcombo.Rows[0][0] = Feature.Available("Name of PriceList1");
            dtcombo.Rows.Add();
            dtcombo.Rows[1][0] = Feature.Available("Name of PriceList2");
            dtcombo.Rows.Add();
            dtcombo.Rows[2][0] = Feature.Available("Name of PriceList3");
            dtcombo.Rows.Add();
            dtcombo.Rows[3][0] = Feature.Available("Name of PriceList4");
            dtcombo.Rows.Add();
            dtcombo.Rows[4][0] = Feature.Available("Name of PriceList5");
            dtcombo.Rows.Add();
            dtcombo.Rows[5][0] = Feature.Available("Name of PriceList6");
            dtcombo.Rows.Add();
            dtcombo.Rows[6][0] = "MRP";
            textBox8.Text = SelectCombo.ComboDt(this, dtcombo, 0);
        }

        private void textBox8_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox8);
        }

        private void textBox8_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox8);
        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }
    }
}
