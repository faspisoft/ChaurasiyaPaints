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
    public partial class DisplayData : Form
    {
        public DataTable gdt = new DataTable();

        public DisplayData(DataTable dt, DataTable dt2)
        {
            InitializeComponent();
            gdt.Clear();
            gdt = dt2;
            ansGridView1.Columns.Clear();
            ansGridView1.Columns.Add("desc", "Description");
            ansGridView1.Columns.Add("qty", "lt/kg");
            ansGridView1.Columns.Add("qd", "Discount (Q.D.)");
            ansGridView1.Columns.Add("amt", "Amount");
            ansGridView1.Columns["desc"].Width = 150;
            ansGridView1.Columns["desc"].ReadOnly = true;
            ansGridView1.Columns["qty"].ReadOnly = true;
            DataTable qty = new DataTable();
            qty.Columns.Add("qty", typeof(double));
            qty.Columns.Add("desc");

            if (gdt.Rows.Count > 0)
            {
                for (int i = 0; i < gdt.Rows.Count; i++)
                {
                    ansGridView1.Rows.Add();
                    if (Feature.Available("Company Colour") == "No")
                    {
                        ansGridView1.Rows[i].Cells["desc"].Value = gdt.Rows[i]["desc"];
                    }
                    else
                    {
                        ansGridView1.Rows[i].Cells["desc"].Value = gdt.Rows[i]["name"];
                    }
                    ansGridView1.Rows[i].Cells["qty"].Value = gdt.Rows[i]["qty"];
                    ansGridView1.Rows[i].Cells["qd"].Value = gdt.Rows[i]["dis"];
                    ansGridView1.Rows[i].Cells["amt"].Value = gdt.Rows[i]["amt"];
                }
            }
            else
            {
                if (Feature.Available("Company Colour") == "No")
                {
                    DataTable dtItem = dt.DefaultView.ToTable(true, "desc");

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        qty.Rows.Add();
                        qty.Rows[i]["qty"] = double.Parse(dt.Rows[i]["qty"].ToString()) * double.Parse(dt.Rows[i]["pval"].ToString());
                        qty.Rows[i]["desc"] = dt.Rows[i]["desc"];
                    }
                    qty.AcceptChanges();
                    double[] gpQty = new double[10];
                    for (int i = 0; i < dtItem.Rows.Count; i++)
                    {
                        gpQty[i] = double.Parse(qty.Compute("Sum(qty)", "desc='" + dtItem.Rows[i]["desc"] + "'").ToString());
                    }
                    for (int i = 0; i < dtItem.Rows.Count; i++)
                    {
                        ansGridView1.Rows.Add();
                        ansGridView1.Rows[i].Cells["desc"].Value = dtItem.Rows[i][0].ToString();
                        ansGridView1.Rows[i].Cells["qty"].Value = gpQty[i];
                    }
                }
                else
                {
                    DataTable dtItem = dt.DefaultView.ToTable(true, "name");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        qty.Rows.Add();
                        qty.Rows[i]["qty"] = double.Parse(dt.Rows[i]["qty"].ToString()) * double.Parse(dt.Rows[i]["pval"].ToString());
                        qty.Rows[i]["desc"] = dt.Rows[i]["name"];
                    }
                    qty.AcceptChanges();
                    double[] gpQty = new double[10];
                    for (int j = 0; j < dtItem.Rows.Count; j++)
                    {
                        gpQty[j] = double.Parse(qty.Compute("Sum(qty)", "desc='" + dtItem.Rows[j]["name"].ToString() + "'").ToString());
                    }
                    for (int j = 0; j < dtItem.Rows.Count; j++)
                    {
                        ansGridView1.Rows.Add();
                        ansGridView1.Rows[j].Cells["desc"].Value = dtItem.Rows[j][0].ToString();
                        ansGridView1.Rows[j].Cells["qty"].Value = gpQty[j];
                    }
                }
            }
        }

        private void DisplayData_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
                this.Dispose();
            }
        }

        private void ansGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (ansGridView1.CurrentCell.OwningColumn.Name == "qd")
            {
                ansGridView1.Rows[e.RowIndex].Cells["amt"].Value = (double.Parse(ansGridView1.Rows[e.RowIndex].Cells["qty"].Value.ToString()) * double.Parse(ansGridView1.Rows[e.RowIndex].Cells["qd"].Value.ToString()));
                gdt.Rows.Add();
               
                    gdt.Rows[e.RowIndex]["name"] = ansGridView1.Rows[e.RowIndex].Cells["desc"].Value;
              
                gdt.Rows[e.RowIndex]["qty"] = ansGridView1.Rows[e.RowIndex].Cells["qty"].Value;
                gdt.Rows[e.RowIndex]["dis"] = ansGridView1.Rows[e.RowIndex].Cells["qd"].Value;
                gdt.Rows[e.RowIndex]["amt"] = ansGridView1.Rows[e.RowIndex].Cells["amt"].Value;
                gdt.AcceptChanges();
            }
            else if (ansGridView1.CurrentCell.OwningColumn.Name == "amt")
            {
                ansGridView1.Rows[e.RowIndex].Cells["qd"].Value = (double.Parse(ansGridView1.Rows[e.RowIndex].Cells["amt"].Value.ToString()) / double.Parse(ansGridView1.Rows[e.RowIndex].Cells["qty"].Value.ToString()));
                gdt.Rows.Add();

                gdt.Rows[e.RowIndex]["name"] = ansGridView1.Rows[e.RowIndex].Cells["desc"].Value;

                gdt.Rows[e.RowIndex]["qty"] = ansGridView1.Rows[e.RowIndex].Cells["qty"].Value;
                gdt.Rows[e.RowIndex]["dis"] = ansGridView1.Rows[e.RowIndex].Cells["qd"].Value;
                gdt.Rows[e.RowIndex]["amt"] = ansGridView1.Rows[e.RowIndex].Cells["amt"].Value;
                gdt.AcceptChanges();
            }
        }

    }
}
