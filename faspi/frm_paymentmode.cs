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
    public partial class frm_paymentmode : Form
    {

        public DataTable gdt = new DataTable();
        string gvid = "";
        double gamt = 0;

        public frm_paymentmode(DataTable dt, string vid, double billamt)
        {
            InitializeComponent();


            gvid = vid;
            gamt = billamt;
            gdt = dt;

           
            for (int i = 0; i < gdt.Rows.Count; i++)
            {


                ansGridView1.Rows.Add();
                ansGridView1.Rows[i].Cells["Sno"].Value = gdt.Rows[i]["Itemsr"].ToString(); 
                ansGridView1.Rows[i].Cells["Accname"].Value = funs.Select_ac_nm(gdt.Rows[i]["Acc_id"].ToString());
                ansGridView1.Rows[i].Cells["instrumentno"].Value = gdt.Rows[i]["instrumentno"].ToString(); 
                ansGridView1.Rows[i].Cells["amount"].Value = double.Parse(gdt.Rows[i]["Amount"].ToString());

            }


            label2.Text = billamt.ToString();
            calc();

        }

        private void ansGridView1_Enter(object sender, EventArgs e)
        {

        }

        private void ansGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            ansGridView1.Rows[e.RowIndex].Cells["SNo"].Value = e.RowIndex + 1;
            if (ansGridView1.CurrentCell.OwningColumn.Name == "SNo")
            {
                SendKeys.Send("{right}");
            }
            this.Activate();
        }
        private void calc()
        {
            double total = 0.0;
            for (int i = 0; i < ansGridView1.RowCount - 1; i++)
            {
                total += double.Parse(ansGridView1.Rows[i].Cells["Amount"].Value.ToString());
            }
            
            label3.Text = total.ToString();
            label5.Text = (gamt - total).ToString();
        }

        private void ansGridView1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsLetter(e.KeyChar) || char.IsNumber(e.KeyChar) || e.KeyChar == ' ' || Convert.ToInt32(e.KeyChar) == 13)
            {
            }
            else
            {
                return;
            }
            string wheresrt = "";
            string strCombo = "";

            if (ansGridView1.CurrentCell.OwningColumn.Name == "Accname")
            {
                wheresrt = " Path  LIKE '1;3;%'  or Path  like '1;2;%'  ";
                strCombo = funs.GetStrCombonew(wheresrt, "   Status=" + access_sql.Singlequote + "true" + access_sql.Singlequote + " and Branch_id='" + Database.BranchId + "' ");
            }
            ansGridView1.CurrentCell.Value = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 1);
            calc();
        }

        private void ansGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (ansGridView1.CurrentCell == null)
            {
                return;
            }

            if (e.KeyCode == Keys.Delete)
            {
                if (ansGridView1.CurrentRow.Index == ansGridView1.Rows.Count - 1)
                {
                    for (int i = 1; i < ansGridView1.Columns.Count; i++)
                    {
                        ansGridView1.Rows[ansGridView1.CurrentRow.Index].Cells[i].Value = "";
                    }
                    return;
                }
                else
                {
                    ansGridView1.Rows.RemoveAt(ansGridView1.CurrentRow.Index);
                    for (int i = 0; i < ansGridView1.Rows.Count; i++)
                    {
                        ansGridView1.Rows[i].Cells["sno"].Value = (i + 1);
                    }
                    calc();
                    return;
                }
            }

            ansGridView1.CurrentCell.OwningRow.Cells["sno"].Value = ansGridView1.CurrentCell.OwningRow.Index + 1;
            if (ansGridView1.CurrentCell.OwningColumn.Name == "instrumentno")
            {
                if (ansGridView1.Rows[ansGridView1.CurrentRow.Index].Cells["Amount"].Value == null)
                {
                    if (ansGridView1.Rows[ansGridView1.CurrentRow.Index].Cells["Amount"].Value == null || ansGridView1.Rows[ansGridView1.CurrentRow.Index].Cells["Amount"].Value.ToString() == "")
                    {
                        SendKeys.Send("{tab}");
                    }
                }
            }
        }

        private void ansGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            ansGridView1.CurrentCell.Value = 0;
        }

        private void frm_paymentmode_KeyDown(object sender, KeyEventArgs e)
        {
 
                if (e.KeyCode == Keys.Escape)
                {
                    SendKeys.Send("{Down}");
                    bool res = ansGridView1.EndEdit();
                    for (int i = 0; i < gdt.Rows.Count; i++)
                    {
                        if (gdt.Rows.Count > 0)
                        {
                            
                               
                             gdt.Rows[i].Delete();
              
                        }
                    }
                    gdt.AcceptChanges();

                    for (int i = 0; i < ansGridView1.Rows.Count - 1; i++)
                    {
                        if (ansGridView1.Rows[i].Cells["Amount"].Value != null)
                        {
                            gdt.Rows.Add();
                        }
                        else
                        {
                            continue;
                        }

                      
                        gdt.Rows[gdt.Rows.Count - 1]["Amount"] =double.Parse(ansGridView1.Rows[i].Cells["Amount"].Value.ToString());
                        gdt.Rows[gdt.Rows.Count - 1]["Acc_id"] = funs.Select_ac_id(ansGridView1.Rows[i].Cells["Accname"].Value.ToString());
                        gdt.Rows[gdt.Rows.Count - 1]["instrumentno"] = ansGridView1.Rows[i].Cells["instrumentno"].Value;
                        gdt.Rows[gdt.Rows.Count - 1]["itemsr"] = ansGridView1.Rows[i].Cells["sno"].Value.ToString();
                    }
                    gdt.AcceptChanges();
                    this.Close();
                }
            
        }

        private void ansGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (ansGridView1.CurrentCell.OwningColumn.Name == "amount")
            {
                try
                {
                    double amt1 = double.Parse(ansGridView1.Rows[e.RowIndex].Cells["amount"].Value.ToString());

                    calc();
                }
                catch (Exception ex)
                {
                    ansGridView1.Rows[e.RowIndex].Cells["amount"].Value = "0.00";
                    return;
                }
            }
        }

      
    }
}
