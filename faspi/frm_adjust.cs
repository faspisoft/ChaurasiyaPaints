using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace faspi
{
    public partial class frm_adjust : Form
    {
        string strCombo = "",gtype="";
        string acid = "", gvid = "";

        public DataTable gdt = new DataTable();
        double gamount;
        int gitemsr;


        double curbal=0;
        public frm_adjust(DataTable dt,int itemsr,String type,string vid,double amountt,string ac_id)
        {
            InitializeComponent();
            acid = ac_id;
            gvid = vid;
            gtype=type;
            gdt.Clear();
            gamount = amountt;
            gdt = dt;
            gitemsr = itemsr;
            DataRow[] drow = gdt.Select("Itemsr=" + itemsr + " and Ac_id='" + acid+"'");
            for (int i = 0; i < drow.Length; i++)
            {
                ansGridView1.Rows.Add();

                ansGridView1.Rows[i].Cells["sno"].Value = drow[i]["adjustsr"];
                ansGridView1.Rows[i].Cells["Vi_id"].Value =gvid;
                ansGridView1.Rows[i].Cells["ac_id"].Value = acid;
               // ansGridView1.Rows[i].Cells["reffno"].Value = drow[i]["reffno"];
                string reffid =drow[i]["reff_id"].ToString();
                if (gvid == reffid)
                {

                    ansGridView1.Rows[i].Cells["reffno"].Value = "<New Refference>";
                }
                else if (funs.DocumentNumber(reffid) == "")
                {

                    ansGridView1.Rows[i].Cells["reffno"].Value = "Opening";
                }


                else
                {

                    ansGridView1.Rows[i].Cells["reffno"].Value = funs.DocumentNumber(reffid);
                }
                ansGridView1.Rows[i].Cells["reff_id"].Value = reffid;
                if (gtype == "Receipt")
                {
                    ansGridView1.Rows[i].Cells["amount"].Value = -1 * double.Parse(drow[i]["amount"].ToString());


                }
                else
                {
                    ansGridView1.Rows[i].Cells["amount"].Value= double.Parse(drow[i]["amount"].ToString());


                }

            }
            calcTot();
        }
        private string IsDocumentNumber(String str)
        {

            return Database.GetScalarText("SELECT DISTINCT VOUCHERINFO.Vi_id FROM VOUCHERINFO LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id WHERE (((VOUCHERTYPE.Short + ' ' + CONVERT(nvarchar, VOUCHERINFO.Vdate, 112) + ' ' + CAST(VOUCHERINFO.Vnumber AS nvarchar(10)))='" + str + "'))");
        }
        private void ansGridView1_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (char.IsLetter(e.KeyChar) || char.IsNumber(e.KeyChar) || e.KeyChar == ' ' || Convert.ToInt32(e.KeyChar) == 13)
            {
                if (ansGridView1.CurrentCell.OwningColumn.Name == "reffno")
                {

                    DataTable dtcombo = new DataTable();
                    if (gvid != "")
                    {
                        strCombo = "select distinct '<New Refference>' as ReffNo, CAST(0.00 AS nvarchar(10))  as Amt  from Account union all  select  case when DocNumber is null then 'Opening' Else DocNumber End as ReffNo,CAST(Amt AS nvarchar(10))   as Amt from(SELECT  VOUCHERTYPE.Short + ' ' + CONVERT(nvarchar, VOUCHERINFO.Vdate, 112) + ' ' + CAST(VOUCHERINFO.Vnumber AS nvarchar(10)) AS DocNumber, Sum(BILLADJEST.Amount) AS amt FROM (BILLADJEST LEFT JOIN VOUCHERINFO ON BILLADJEST.Reff_id = VOUCHERINFO.Vi_id) LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id WHERE BILLADJEST.Ac_id ='" + acid + "' AND BILLADJEST.Vi_id <>'" + gvid + "' GROUP BY VOUCHERTYPE.Short +' ' + CONVERT(nvarchar, VOUCHERINFO.Vdate, 112) + ' ' + CAST(VOUCHERINFO.Vnumber AS nvarchar(10)) HAVING (((Sum(BILLADJEST.Amount))<>0))) as res";
                    }
                    else
                    {
                        strCombo = "select distinct '<New Refference>' as ReffNo,CAST(0.00 AS nvarchar(10))   as Amt  from Account union all  select Case when DocNumber is null then 'Opening' Else DocNumber End as ReffNo,CAST(Amt AS nvarchar(10))  as Amt from(SELECT VOUCHERTYPE.Short + ' ' + CONVERT(nvarchar, VOUCHERINFO.Vdate, 112) + ' ' + CAST(VOUCHERINFO.Vnumber AS nvarchar(10)) AS DocNumber, Sum(BILLADJEST.Amount) AS amt FROM (BILLADJEST LEFT JOIN VOUCHERINFO ON BILLADJEST.Reff_id = VOUCHERINFO.Vi_id) LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id WHERE BILLADJEST.Ac_id ='" + acid + "'  GROUP BY VOUCHERTYPE.Short +' ' + CONVERT(nvarchar, VOUCHERINFO.Vdate, 112) + ' ' + CAST(VOUCHERINFO.Vnumber AS nvarchar(10)) HAVING (((Sum(BILLADJEST.Amount))<>0))) as res";
                    }
                    Database.GetSqlData(strCombo, dtcombo);

                    ansGridView1.CurrentCell.Value = SelectCombo.ComboDt(this, dtcombo, 1);
                    //if (ansGridView1.CurrentCell.Value != "")
                    //{
                    //    SendKeys.Send("{Enter}");
                    //}

                    if (ansGridView1.CurrentRow.Cells["reffno"].Value == null || ansGridView1.CurrentRow.Cells["reffno"].Value.ToString() == "<New Refference>")
                    {
                        ansGridView1.CurrentRow.Cells["Reff_id"].Value = gvid;
                    }
                    else
                    {
                        ansGridView1.CurrentRow.Cells["Reff_id"].Value = IsDocumentNumber(ansGridView1.CurrentRow.Cells["reffno"].Value.ToString());
                    }

                    dtcombo = new DataTable();
                    strCombo = "select distinct '<New Refference>' as ReffNo, 0 as Amt from Account union all  select case when DocNumber='' then 'Opening' Else DocNumber End  as ReffNo,amt from(SELECT VOUCHERTYPE.Short + ' ' + CONVERT(nvarchar, VOUCHERINFO.Vdate, 112) + ' ' + CAST(VOUCHERINFO.Vnumber AS nvarchar(10)) AS DocNumber, Sum(BILLADJEST.Amount) AS amt FROM (BILLADJEST LEFT JOIN VOUCHERINFO ON BILLADJEST.Reff_id = VOUCHERINFO.Vi_id) LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id WHERE (((BILLADJEST.Ac_id)='" + acid + "' )) GROUP BY VOUCHERTYPE.Short + ' ' + CONVERT(nvarchar, VOUCHERINFO.Vdate, 112) + ' ' + CAST(VOUCHERINFO.Vnumber AS nvarchar(10)) HAVING (((Sum(BILLADJEST.Amount))<>0))) as res";
                    Database.GetSqlData(strCombo, dtcombo);

                    double amount = 0;
                    if (dtcombo.Select("ReffNo='" + ansGridView1.CurrentCell.Value + "'").Length > 0)
                    {
                        amount = double.Parse(dtcombo.Compute("sum(amt)", " ReffNo='" + ansGridView1.CurrentCell.Value + "'").ToString());
                    }
                    //if (gtype == "Receipt")
                    //{
                    //    if (curbal <= amount)
                    //    {
                    //        ansGridView1.CurrentRow.Cells["amount"].Value = curbal;
                    //        calcTot();
                    //    }
                    //}
                    //else
                    //{
                    //    if (curbal <= amount)
                    //    {
                    //        ansGridView1.CurrentRow.Cells["amount"].Value = curbal;
                    //        calcTot();
                    //    }
                    //}
                    ansGridView1.CurrentRow.Cells["amount"].Value = curbal;
                    SendKeys.Send("{Enter}");
                    ansGridView1.Columns["amount"].ReadOnly = false;
                    //if (gtype == "Payment")
                    //{

                    //    curbal = -1 * curbal;
                     
                    //}
                    

                    SendKeys.Send(curbal.ToString());
                  //  SendKeys.Send("{Enter}");
                }
            }

        }


        private bool Validate()
        {

            if (double.Parse(label3.Text)!=0)
            {
                return false;
            }

            return true;
        }
        private void frm_adjust_KeyDown(object sender, KeyEventArgs e)
        {
           
            if (e.KeyCode == Keys.Escape)
            {
                if (Validate() == true)
                {
                    if (e.KeyCode == Keys.Escape)
                    {
                        ansGridView1.EndEdit();
                        DataTable dttemp = gdt.Copy();
                        for (int i = gdt.Rows.Count - 1; i > -1; i--)
                        {
                            if (gdt.Rows[i]["Itemsr"].ToString() != "")
                            {
                                if (int.Parse(gdt.Rows[i]["Itemsr"].ToString()) == gitemsr)
                                {
                                    gdt.Rows[i].Delete();
                                }
                            }

                        }
                        gdt.AcceptChanges();

                        for (int i = 0; i < ansGridView1.Rows.Count; i++)
                        {
                            if (ansGridView1.Rows[i].Cells["Amount"].Value != null )
                            {
                                gdt.Rows.Add();
                            }
                            else
                            {
                                continue;
                            }
                            gdt.Rows[gdt.Rows.Count - 1]["Vi_id"] = gvid;
                            gdt.Rows[gdt.Rows.Count - 1]["Itemsr"] = gitemsr;
                            gdt.Rows[gdt.Rows.Count - 1]["ac_id"] = acid;
                            gdt.Rows[gdt.Rows.Count - 1]["AdjustSr"] = ansGridView1.Rows[i].Cells["sno"].Value;
                            gdt.Rows[gdt.Rows.Count - 1]["reff_id"] = ansGridView1.Rows[i].Cells["reff_id"].Value;
                            if (gtype == "Payment")
                            {
                                gdt.Rows[gdt.Rows.Count - 1]["amount"] = double.Parse(ansGridView1.Rows[i].Cells["amount"].Value.ToString());
                            }
                            else
                            {
                                gdt.Rows[gdt.Rows.Count - 1]["amount"] = -1* double.Parse(ansGridView1.Rows[i].Cells["amount"].Value.ToString());
                            }
                           // gdt.Rows[gdt.Rows.Count - 1]["Tax_Acc_id"] = ansGridView1.Rows[i].Cells["Tax_Acc_id"].Value;
                            //gdt.Rows[gdt.Rows.Count - 1]["Tax_Name"] = ansGridView1.Rows[i].Cells["Tax_Name"].Value;
                            //gdt.Rows[gdt.Rows.Count - 1]["Tax_Rate"] = ansGridView1.Rows[i].Cells["Tax_Rate"].Value;
                            //gdt.Rows[gdt.Rows.Count - 1]["Taxable"] = ansGridView1.Rows[i].Cells["Taxable"].Value;
                            //gdt.Rows[gdt.Rows.Count - 1]["Tax_Amount"] = ansGridView1.Rows[i].Cells["Tax_Amount"].Value;
                            //gdt.Rows[gdt.Rows.Count - 1]["Type"] = ansGridView1.Rows[i].Cells["Type"].Value;
                            //gdt.Rows[gdt.Rows.Count - 1]["+/-"] = ansGridView1.Rows[i].Cells["+/-"].Value;
                            //gdt.Rows[gdt.Rows.Count - 1]["Changed"] = ansGridView1.Rows[i].Cells["Changed"].Value;

                          
                        }
                        gdt.AcceptChanges();
                        this.Close();
                    }
                }
            }
        }

        private void ansGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (ansGridView1.CurrentCell.OwningColumn.Name == "sno")
            {
                ansGridView1.Rows[e.RowIndex].Cells["sno"].Value = e.RowIndex + 1;
                SendKeys.Send("{tab}");
            }
        }

        private void ansGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            ansGridView1.CurrentCell.Value = 0;
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
                    ansGridView1.Rows[ansGridView1.CurrentRow.Index].Cells[1].Value = "";
                    ansGridView1.Rows[ansGridView1.CurrentRow.Index].Cells["amount"].Value = "0.00";
                    calcTot();
                    return;
                }
                else
                {
                    ansGridView1.Rows.RemoveAt(ansGridView1.CurrentRow.Index);
                    for (int i = 0; i < ansGridView1.Rows.Count; i++)
                    {
                        ansGridView1.Rows[i].Cells["sno"].Value = (i + 1);
                    }
                    calcTot();
                    return;
                }
            }
            ansGridView1.CurrentCell.OwningRow.Cells["sno"].Value = ansGridView1.CurrentCell.OwningRow.Index + 1;
        }
        private void calcTot()
        {
            double rtot = 0.0, ptot = 0.0;
            for (int i = 0; i < ansGridView1.RowCount - 1; i++)
            {
                if (ansGridView1.Rows[i].Cells["amount"].Value != null)
                {
                    rtot += double.Parse(ansGridView1.Rows[i].Cells["amount"].Value.ToString());
                }
                
            }
            label2.Text = rtot.ToString();
            //if (gtype == "Payment")
            //{
                //rtot = -1 * rtot;
           // }
         
            curbal= gamount-rtot;
            label3.Text= funs.DecimalPoint((gamount-rtot).ToString());


        }
        private void frm_adjust_Load(object sender, EventArgs e)
        {

        }

        private void ansGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (ansGridView1.CurrentCell.OwningColumn.Name == "amount")
            {

                
                calcTot();

            }
        }
    }
}
