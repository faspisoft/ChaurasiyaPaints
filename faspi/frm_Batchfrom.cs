using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace faspi
{
    public partial class frm_Batchfrom : Form
    {
        public DataTable gdt;
        DataTable dtinfo;
        int vid, itemsrno;

        public frm_Batchfrom(string des_id)
        {
            InitializeComponent();
            dtinfo = new DataTable();
            Database.GetSqlData("SELECT Sum(Stock.Receive)-Sum(Stock.Issue) AS Quantity, Stock.Batch_no as Batch_Code FROM Stock WHERE (((Stock.Did)='" + des_id + "')) GROUP BY Stock.Batch_no HAVING (((Sum(Stock.Receive)-Sum(Stock.Issue))>0) AND ((Stock.Batch_no)<>''))", dtinfo);
            for (int i = 0; i < dtinfo.Rows.Count; i++)
            {
                ansGridView1.Rows.Add();
                ansGridView1.Rows[i].Cells["batchno"].Value = dtinfo.Rows[i]["Batch_Code"].ToString();
                ansGridView1.Rows[i].Cells["select"].Value = false;
            }
            gdt = new DataTable();
            gdt.Columns.Add("Vi_id", typeof(int));
            gdt.Columns.Add("Batchno", typeof(string));
        }

        private void button3_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < ansGridView1.Rows.Count; i++)
            {
                if (bool.Parse(ansGridView1.Rows[i].Cells["select"].Value.ToString()) == true)
                {
                    vid = Database.GetScalarInt("SELECT VOUCHERINFO.Vi_id FROM (VOUCHERINFO LEFT JOIN Voucherdet ON VOUCHERINFO.Vi_id = Voucherdet.Vi_id) LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id WHERE (((Voucherdet.Batch_Code)='" + ansGridView1.Rows[i].Cells["batchno"].Value.ToString() + "')) GROUP BY VOUCHERTYPE.Type, VOUCHERINFO.Vi_id HAVING (((VOUCHERTYPE.Type)='Purchase'))");
                    gdt.Rows.Add();
                    gdt.Rows[gdt.Rows.Count - 1]["Vi_id"] = vid;
                    gdt.Rows[gdt.Rows.Count - 1]["batchno"] = ansGridView1.Rows[i].Cells["batchno"].Value.ToString();
                }
            }
            this.Close();
            this.Dispose();
        }

        private void frm_Batchfrom_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
                this.Dispose();
            }
        }
    }
}
