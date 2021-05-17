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
    public partial class frm_orderdetails : Form
    {
        int vid, itemsrno;
        double Aqty;
        int gvid;
        public DataTable gdt;
        string gPartyname="";
        public frm_orderdetails(string PartyName, DataTable dtitems)
        {
            InitializeComponent();
           // gdt = dtitems;
            gPartyname = PartyName;
            gvid = vid;
        }

        private void frm_orderdetails_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
                this.Dispose();
            }
        }

        private void frm_orderdetails_Load(object sender, EventArgs e)
        {
           // string sqltest = "SELECT VOUCHERINFO_2.Invoiceno, VOUCHERINFO_2.Vdate, res.Itemsr, res.Description, res.Pack, SUM(res.Quantity) AS Qty, res.Rate_am,  res.Vi_id FROM (SELECT Voucherdet.Itemsr, Description.Description, Description.Pack, Voucherdet.Quantity, Voucherdet.Rate_am, Voucherdet.Vi_id  FROM Description RIGHT OUTER JOIN  Voucherdet ON Description.Des_id = Voucherdet.Des_ac_id RIGHT OUTER JOIN  VOUCHERINFO INNER JOIN  VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id ON Voucherdet.Vi_id = VOUCHERINFO.Vi_id  WHERE ( VOUCHERTYPE.Type = 'Order') AND ( VOUCHERINFO.Ac_id =" + funs.Select_ac_id(gPartyname) + ") AND ( VOUCHERINFO.Iscancel = 'false')  UNION ALL  SELECT Voucherdet_1.ritemsr, Description_1.Description, Description_1.Pack, - (1 * Voucherdet_1.Quantity) AS quantity, Voucherdet_1.Rate_am,  Voucherdet_1.rvi_id  FROM Description AS Description_1 RIGHT OUTER JOIN  Voucherdet AS Voucherdet_1 ON Description_1.Des_id = Voucherdet_1.Des_ac_id RIGHT OUTER JOIN  VOUCHERINFO AS VOUCHERINFO_1 INNER JOIN  VOUCHERTYPE AS VOUCHERTYPE_1 ON VOUCHERINFO_1.Vt_id = VOUCHERTYPE_1.Vt_id ON Voucherdet_1.Vi_id = VOUCHERINFO_1.Vi_id  WHERE (VOUCHERTYPE_1.Type = 'Sale') AND (VOUCHERINFO_1.Ac_id = " + funs.Select_ac_id(gPartyname) + ") AND (VOUCHERINFO_1.Iscancel = 'false')) AS res LEFT OUTER JOIN  VOUCHERINFO AS VOUCHERINFO_2 ON res.Vi_id = VOUCHERINFO_2.Vi_id GROUP BY res.Itemsr, res.Description, res.Pack, res.Rate_am, res.Vi_id, VOUCHERINFO_2.Vdate, VOUCHERINFO_2.Invoiceno HAVING (SUM(res.Quantity) > 0) ORDER BY res.Itemsr";
            string sqltest = "SELECT VOUCHERINFO.Invoiceno, VOUCHERINFO.Vdate, res.Itemsr, res.Description, res.Pack, Sum(res.Quantity) AS Quantity, res.Rate_am, res.Vi_id FROM (SELECT Voucherdet.Itemsr, Description.Description, Description.Pack, Voucherdet.Quantity, Voucherdet.Rate_am, Voucherdet.Vi_id FROM ((VOUCHERINFO LEFT JOIN Voucherdet ON VOUCHERINFO.Vi_id = Voucherdet.Vi_id) LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id) LEFT JOIN Description ON Voucherdet.Des_ac_id = Description.Des_id WHERE (((VOUCHERTYPE.Type)='Sale Order') AND ((VOUCHERINFO.Iscancel)=" + access_sql.Singlequote + "False" + access_sql.Singlequote + ") AND ((VOUCHERINFO.Ac_id)='" + funs.Select_ac_id(gPartyname) + "')) Union all SELECT Voucherdet.ritemsr, Description.Description, Description.Pack, -1*[Quantity] AS Qty, Voucherdet.Rate_am, Voucherdet.rvi_id FROM ((VOUCHERINFO LEFT JOIN Voucherdet ON VOUCHERINFO.Vi_id = Voucherdet.Vi_id) LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id) LEFT JOIN Description ON Voucherdet.Des_ac_id = Description.Des_id WHERE (((VOUCHERTYPE.Type)='Sale') AND ((VOUCHERINFO.Iscancel)=" + access_sql.Singlequote + "False" + access_sql.Singlequote + ") AND ((VOUCHERINFO.Ac_id)='" + funs.Select_ac_id(gPartyname) + "')))  AS res LEFT JOIN VOUCHERINFO ON res.Vi_id = VOUCHERINFO.Vi_id GROUP BY VOUCHERINFO.Invoiceno, VOUCHERINFO.Vdate, res.Itemsr, res.Description, res.Pack, res.Rate_am, res.Vi_id HAVING (((Sum(res.Quantity))>0))";

           
            DataTable dt1 = new DataTable();
            Database.GetSqlData(sqltest, dt1);
            ansGridView5.Rows.Clear();
            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                ansGridView5.Rows.Add();
                ansGridView5.Rows[i].Cells["Orderno"].Value = dt1.Rows[i]["Invoiceno"].ToString();
                ansGridView5.Rows[i].Cells["Vi_id"].Value = dt1.Rows[i]["Vi_id"].ToString();
                ansGridView5.Rows[i].Cells["vdate"].Value = DateTime.Parse(dt1.Rows[i]["Vdate"].ToString()).ToString(Database.dformat);
                ansGridView5.Rows[i].Cells["itemsr"].Value = dt1.Rows[i]["Itemsr"].ToString();
               
                ansGridView5.Rows[i].Cells["description"].Value = dt1.Rows[i]["Description"].ToString();
                ansGridView5.Rows[i].Cells["pack"].Value = dt1.Rows[i]["Pack"].ToString();
                ansGridView5.Rows[i].Cells["qty"].Value = dt1.Rows[i]["Quantity"].ToString();
                ansGridView5.Rows[i].Cells["rate"].Value = dt1.Rows[i]["Rate_am"].ToString();
                ansGridView5.Rows[i].Cells["Select"].Value = true;

            }
            gdt = new DataTable();
            gdt.Columns.Add("Vi_id", typeof(int));
            gdt.Columns.Add("Itemsr", typeof(int));
           // gdt.Columns.Add("Qty", typeof(double));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < ansGridView5.Rows.Count; i++)
            {
                if (ansGridView5.Rows[i].Cells["select"].Value != null)
                {
                    if (bool.Parse(ansGridView5.Rows[i].Cells["select"].Value.ToString()) == true)
                    {
                        vid = int.Parse(ansGridView5.Rows[i].Cells["Vi_id"].Value.ToString());
                        itemsrno = int.Parse(ansGridView5.Rows[i].Cells["itemsr"].Value.ToString());
                        Aqty = double.Parse(ansGridView5.Rows[i].Cells["Qty"].Value.ToString());
                        if (vid != 0 && itemsrno != 0)
                        {
                            gdt.Rows.Add();
                            gdt.Rows[gdt.Rows.Count - 1]["Vi_id"] = vid;
                            gdt.Rows[gdt.Rows.Count - 1]["Itemsr"] = itemsrno;
                        }
                       // gdt.Rows[gdt.Rows.Count - 1]["Qty"] = Aqty;
                    }
                }
            }
            this.Close();
        }


    }
}
