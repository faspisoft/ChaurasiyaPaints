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
    public partial class frm_needApp : Form
    {
        public frm_needApp()
        {
            InitializeComponent();
        }

        private void frm_needApp_Load(object sender, EventArgs e)
        {
            this.Size = this.MdiParent.Size;

        }
        private int IsDocumentNumber(String str)
        {
           
            return Database.GetScalarInt("SELECT DISTINCT VOUCHERINFO.Vi_id, " + access_sql.Docnumber + " AS DocNumber FROM (VOUCHERINFO LEFT JOIN ACCOUNT ON VOUCHERINFO.Ac_id = ACCOUNT.Ac_id) LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id WHERE (((VOUCHERINFO.Vt_id)=[VOUCHERTYPE].[Vt_id]) AND (" + access_sql.Docnumber + "='" + str + "'))");
        }

        public void Loaddata()
        {
            DataTable dt = new DataTable();
            Database.GetSqlData("SELECT " + access_sql.Docnumber + " AS DocNumber, VOUCHERINFO.Vi_id,VOUCHERTYPE.Type FROM VOUCHERINFO LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id WHERE VOUCHERTYPE.A=" + access_sql.Singlequote + "True" + access_sql.Singlequote + " AND VOUCHERINFO.NApproval=" + access_sql.Singlequote + "True" + access_sql.Singlequote + " ORDER BY VOUCHERINFO.Vi_id", dt);
            ansGridView5.Rows.Clear();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ansGridView5.Rows.Add();
                ansGridView5.Rows[i].Cells["sno"].Value= (i+1);
                ansGridView5.Rows[i].Cells["Vid"].Value = dt.Rows[i]["Vi_id"].ToString();
                ansGridView5.Rows[i].Cells["Type"].Value = dt.Rows[i]["Type"].ToString();
                ansGridView5.Rows[i].Cells["DocNumber"].Value = dt.Rows[i]["DocNumber"].ToString();
            }
        }

        private void frm_needApp_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
                this.Dispose();
            }
        }

        private void ansGridView5_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (ansGridView5.CurrentCell.OwningColumn.Name == "DocNumber")
            {

                string gstr = ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Type"].Value.ToString();

                if (gstr == "Receipt")
                {
                    if (ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vid"].Value.ToString() == "0")
                    {
                        return;
                    }
                    frmCashRec frm = new frmCashRec();
                    frm.recpay = "Receipt";
                    frm.cmdnm = "edit";
                    frm.Text = "Edit Receipt";
                    frm.LoadData(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vid"].Value.ToString(), frm.Text);
                    frm.MdiParent = this.MdiParent;
                    frm.Show();
                }
                else if (gstr == "Payment")
                {
                    if (ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vid"].Value.ToString() == "0")
                    {
                        return;
                    }
                    frmCashRec frm = new frmCashRec();
                    frm.recpay = "Payment";
                    frm.cmdnm = "edit";
                    frm.Text = "Edit Payment";
                    frm.LoadData(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vid"].Value.ToString(), frm.Text);
                    frm.MdiParent = this.MdiParent;
                    frm.Show();
                }


                else if (gstr == "Contra")
                {
                    if (ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vid"].Value.ToString() == "0")
                    {
                        return;
                    }
                    frmCashRec frm = new frmCashRec();
                    frm.recpay = "Contra";
                    frm.cmdnm = "edit";
                    frm.Text = "Edit Contra";
                    frm.LoadData(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vid"].Value.ToString(), frm.Text);
                    frm.MdiParent = this.MdiParent;
                    frm.Show();

                }

                else if (gstr == "Journal")
                {
                    if (ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vid"].Value.ToString() == "0")
                    {
                        return;
                    }

                    frmJournal frm = new frmJournal();
                    frm.LoadData(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vid"].Value.ToString(), "Journal Voucher");
                    frm.MdiParent = this.MdiParent;
                    frm.Show();

                }
                else if (gstr == "Dnote")
                {
                    if (ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vid"].Value.ToString() == "0")
                    {
                        return;
                    }
                    frmDebitCredit frm = new frmDebitCredit();
                    frm.dr_cr_note = "Debit Note";
                    frm.cmdnm = "edit";
                    frm.LoadData(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vid"].Value.ToString(), "Debit Note");
                    frm.MdiParent = this.MdiParent;
                    frm.Show();

                }
                else if (gstr == "Cnote")
                {
                    if (ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vid"].Value.ToString() == "0")
                    {
                        return;
                    }
                    frmDebitCredit frm = new frmDebitCredit();
                    frm.dr_cr_note = "Credit Note";
                    frm.cmdnm = "edit";
                    frm.LoadData(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vid"].Value.ToString(), "Credit Note");
                    frm.MdiParent = this.MdiParent;
                    frm.Show();

                }


                else if (gstr == "Purchase")
                {
                    if (ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vid"].Value.ToString() == "0")
                    {
                        return;
                    }
                    DataTable dtalter = new DataTable();
                    Database.GetSqlData("SELECT VOUCHERTYPE.Type, VOUCHERINFO.Tdtype, VOUCHERTYPE.ExState, VOUCHERTYPE.Unregistered FROM VOUCHERINFO,VOUCHERTYPE WHERE VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id and VOUCHERINFO.Vi_id='" + ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vid"].Value.ToString()+"' ", dtalter);
                    frmTransaction frm = new frmTransaction();
                    frm.LoadData(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vid"].Value.ToString(), dtalter.Rows[0]["Type"].ToString(), bool.Parse(dtalter.Rows[0]["Tdtype"].ToString()), bool.Parse(dtalter.Rows[0]["ExState"].ToString()), bool.Parse(dtalter.Rows[0]["Unregistered"].ToString()));
                    frm.MdiParent = this.MdiParent;
                    frm.Show();

                }

                else if (gstr == "RCM")
                {
                    if (ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vid"].Value.ToString() == "0")
                    {
                        return;
                    }
                    DataTable dtalter = new DataTable();
                    Database.GetSqlData("SELECT VOUCHERTYPE.Type, VOUCHERINFO.Tdtype, VOUCHERTYPE.ExState, VOUCHERTYPE.Unregistered FROM VOUCHERINFO,VOUCHERTYPE WHERE VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id and VOUCHERINFO.Vi_id='" + ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vid"].Value.ToString()+"' ", dtalter);
                    frmTransaction frm = new frmTransaction();
                    frm.LoadData(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vid"].Value.ToString(), dtalter.Rows[0]["Type"].ToString(), bool.Parse(dtalter.Rows[0]["Tdtype"].ToString()), bool.Parse(dtalter.Rows[0]["ExState"].ToString()), bool.Parse(dtalter.Rows[0]["Unregistered"].ToString()));
                    frm.MdiParent = this.MdiParent;
                    frm.Show();

                }
                else if (gstr == "Sale")
                {
                    if (ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["vid"].Value.ToString() == "0")
                    {
                        return;
                    }
                    DataTable dtalter = new DataTable();
                    Database.GetSqlData("SELECT VOUCHERTYPE.Type, VOUCHERINFO.Tdtype, VOUCHERTYPE.ExState, VOUCHERTYPE.Unregistered FROM VOUCHERINFO,VOUCHERTYPE WHERE VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id and VOUCHERINFO.Vi_id='" + ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vid"].Value.ToString()+"' ", dtalter);
                    frmTransaction frm = new frmTransaction();
                    frm.LoadData(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["vid"].Value.ToString(), dtalter.Rows[0]["Type"].ToString(), bool.Parse(dtalter.Rows[0]["Tdtype"].ToString()), bool.Parse(dtalter.Rows[0]["ExState"].ToString()), bool.Parse(dtalter.Rows[0]["Unregistered"].ToString()));
                    frm.MdiParent = this.MdiParent;
                    frm.Show();

                }

                else if (gstr == "P Return")
                {
                    if (ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vid"].Value.ToString() == "0")
                    {
                        return;
                    }
                    DataTable dtalter = new DataTable();
                    Database.GetSqlData("SELECT VOUCHERTYPE.Type, VOUCHERINFO.Tdtype, VOUCHERTYPE.ExState, VOUCHERTYPE.Unregistered FROM VOUCHERINFO,VOUCHERTYPE WHERE VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id and VOUCHERINFO.Vi_id='" + ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vid"].Value.ToString()+"' ", dtalter);
                    frmTransaction frm = new frmTransaction();
                    frm.LoadData(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vid"].Value.ToString(), dtalter.Rows[0]["Type"].ToString(), bool.Parse(dtalter.Rows[0]["Tdtype"].ToString()), bool.Parse(dtalter.Rows[0]["ExState"].ToString()), bool.Parse(dtalter.Rows[0]["Unregistered"].ToString()));
                    frm.MdiParent = this.MdiParent;
                    frm.Show();

                }
                else if (gstr == "Return")
                {
                    if (ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vid"].Value.ToString() == "0")
                    {
                        return;
                    }
                    DataTable dtalter = new DataTable();
                    Database.GetSqlData("SELECT VOUCHERTYPE.Type, VOUCHERINFO.Tdtype, VOUCHERTYPE.ExState, VOUCHERTYPE.Unregistered FROM VOUCHERINFO,VOUCHERTYPE WHERE VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id and VOUCHERINFO.Vi_id=" + ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vid"].Value.ToString()+"' ", dtalter);
                    frmTransaction frm = new frmTransaction();
                    frm.LoadData(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vid"].Value.ToString(), dtalter.Rows[0]["Type"].ToString(), bool.Parse(dtalter.Rows[0]["Tdtype"].ToString()), bool.Parse(dtalter.Rows[0]["ExState"].ToString()), bool.Parse(dtalter.Rows[0]["Unregistered"].ToString()));
                    frm.MdiParent = this.MdiParent;
                    frm.Show();
                }

                
                else if (gstr == "Transfer")
                {
                    if (ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString() == "0")
                    {
                        return;
                    }
                    DataTable dtalter = new DataTable();
                    Database.GetSqlData("SELECT VOUCHERTYPE.Type, VOUCHERINFO.Tdtype, VOUCHERTYPE.ExState, VOUCHERTYPE.Unregistered FROM VOUCHERINFO,VOUCHERTYPE WHERE VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id and VOUCHERINFO.Vi_id=" + int.Parse(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString()), dtalter);
                    frm_stkjournal frm = new frm_stkjournal();
                    frm.LoadData(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vi_id"].Value.ToString(), "Edit Stock Journal");
                    frm.MdiParent = this.MdiParent;
                    frm.Show();
                }
                else if (gstr == "Pending")
                {
                    if (ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vid"].Value.ToString() == "0")
                    {
                        return;
                    }
                    DataTable dtalter = new DataTable();
                    Database.GetSqlData("SELECT VOUCHERTYPE.Type, VOUCHERINFO.Tdtype, VOUCHERTYPE.ExState, VOUCHERTYPE.Unregistered FROM VOUCHERINFO,VOUCHERTYPE WHERE VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id and VOUCHERINFO.Vi_id='" + ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vid"].Value.ToString()+"' ", dtalter);
                    frmTransaction frm = new frmTransaction();
                    frm.LoadData(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Vid"].Value.ToString(), dtalter.Rows[0]["Type"].ToString(), bool.Parse(dtalter.Rows[0]["Tdtype"].ToString()), bool.Parse(dtalter.Rows[0]["ExState"].ToString()), bool.Parse(dtalter.Rows[0]["Unregistered"].ToString()));
                    frm.MdiParent = this.MdiParent;
                    frm.Show();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Loaddata();
        }
    }
}
