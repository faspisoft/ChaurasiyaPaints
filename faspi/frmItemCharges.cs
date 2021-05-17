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
    public partial class frmItemCharges : Form
    {
        public DataTable gdt = new DataTable();
        String gstrcombo, gvid, gdesid;
        double bamt,gqty;
        int  gitemsr;
        public double gtot = 0;

        public frmItemCharges(DataTable dt, string vid, int itemsr, String strcombo, double amt, string desid, double qty)
        {
            InitializeComponent();
            bamt = amt;
            gqty = qty;
            gstrcombo = strcombo;
            gvid = vid;
            gitemsr = itemsr;
            gdesid = desid;
            gdt.Clear();
            gdt = dt;

            ansGridView1.Columns.Clear();
            ansGridView1.Columns.Add("Vi_id", "Vi_id");
            ansGridView1.Columns.Add("Itemsr", "Itemsr");
            ansGridView1.Columns.Add("Chargesr", "Chargesr");
            ansGridView1.Columns.Add("Charg_id", "Charg_id");
            ansGridView1.Columns.Add("Accid", "Accid");
            ansGridView1.Columns.Add("Addsub", "Addsub");
            ansGridView1.Columns.Add("Ctype", "Ctype");
            ansGridView1.Columns.Add("ChargeName", "Charge Name");
            ansGridView1.Columns.Add("Amount", "Amount");
            ansGridView1.Columns.Add("Camount", "Camount");

            ansGridView1.Columns["Vi_id"].Visible = false;
            ansGridView1.Columns["Itemsr"].Visible = false;
            ansGridView1.Columns["Chargesr"].Visible = false;
            ansGridView1.Columns["Charg_id"].Visible = false;
            ansGridView1.Columns["Accid"].Visible = false;
            ansGridView1.Columns["Addsub"].Visible = false;
            ansGridView1.Columns["Ctype"].Visible = false;

            ansGridView1.Columns["Amount"].Width = 60;
            ansGridView1.Columns["Camount"].Width = 60;

            ansGridView1.Columns["ChargeName"].DisplayIndex = 0;
            ansGridView1.Columns["ChargeName"].ReadOnly = true;

            DataRow[] drow = gdt.Select("Vi_id='" + vid + "; and Itemsr=" + itemsr);
            for (int i = 0; i < drow.Length; i++)
            {
                ansGridView1.Rows.Add();
                ansGridView1.Rows[i].Cells["Vi_id"].Value = drow[i]["Vi_id"];
                ansGridView1.Rows[i].Cells["Itemsr"].Value = drow[i]["Itemsr"];
                ansGridView1.Rows[i].Cells["Chargesr"].Value = drow[i]["Chargesr"];
                ansGridView1.Rows[i].Cells["Charg_id"].Value = drow[i]["Charg_id"];
                ansGridView1.Rows[i].Cells["Accid"].Value = drow[i]["Accid"];
                ansGridView1.Rows[i].Cells["Addsub"].Value = drow[i]["Addsub"];
                ansGridView1.Rows[i].Cells["Amount"].Value = drow[i]["Amount"];
                ansGridView1.Rows[i].Cells["Camount"].Value = drow[i]["Camount"];
                ansGridView1.Rows[i].Cells["ChargeName"].Value = drow[i]["ChargeName"];
                ansGridView1.Rows[i].Cells["Ctype"].Value = drow[i]["Ctype"];
            }
        }

        private void frmItemCharges_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                SendKeys.Send("{Down}");
                bool res = ansGridView1.EndEdit();
                for (int i = 0; i < gdt.Rows.Count; i++)
                {
                    if (gdt.Rows.Count > 0)
                    {
                        if (gdt.Rows[i]["Itemsr"].ToString() != "")
                        {
                            if (int.Parse(gdt.Rows[i]["Itemsr"].ToString()) == gitemsr)
                            {
                                gdt.Rows[i].Delete();
                            }
                        }
                    }
                }
                gdt.AcceptChanges();

                for (int i = 0; i < ansGridView1.Rows.Count - 1; i++)
                {
                    if (ansGridView1.Rows[i].Cells["Amount"].Value != null && ansGridView1.Rows[i].Cells["Camount"].Value != null)
                    {
                        gdt.Rows.Add();
                    }
                    else
                    {
                        continue;
                    }

                    gdt.Rows[gdt.Rows.Count - 1]["Vi_id"] = ansGridView1.Rows[i].Cells["Vi_id"].Value;

                    gdt.Rows[gdt.Rows.Count - 1]["Itemsr"] = ansGridView1.Rows[i].Cells["Itemsr"].Value;
                    gdt.Rows[gdt.Rows.Count - 1]["Chargesr"] = ansGridView1.Rows[i].Cells["Chargesr"].Value;
                    gdt.Rows[gdt.Rows.Count - 1]["Charg_id"] = ansGridView1.Rows[i].Cells["Charg_id"].Value;
                    gdt.Rows[gdt.Rows.Count - 1]["Accid"] = ansGridView1.Rows[i].Cells["Accid"].Value;
                    gdt.Rows[gdt.Rows.Count - 1]["Amount"] = ansGridView1.Rows[i].Cells["Amount"].Value;
                    gdt.Rows[gdt.Rows.Count - 1]["Camount"] = ansGridView1.Rows[i].Cells["Camount"].Value;
                    gdt.Rows[gdt.Rows.Count - 1]["Addsub"] = ansGridView1.Rows[i].Cells["Addsub"].Value;
                    gdt.Rows[gdt.Rows.Count - 1]["Ctype"] = ansGridView1.Rows[i].Cells["Ctype"].Value;
                    gdt.Rows[gdt.Rows.Count - 1]["ChargeName"] = ansGridView1.Rows[i].Cells["ChargeName"].Value;
                }
                gdt.AcceptChanges();
                this.Close();
            }
        }

        private void ansGridView1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = false;
            if (char.IsLetter(e.KeyChar) || char.IsNumber(e.KeyChar) || e.KeyChar == ' ')
            {
                if (ansGridView1.CurrentCell.OwningColumn.Name == "ChargeName")
                {
                    ansGridView1.CurrentCell.Value = SelectCombo.ComboKeypress(this, e.KeyChar, gstrcombo, e.KeyChar.ToString(), 0);
                }
              
                SendKeys.Send("{Tab}");
            }
        }

        private void ansGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (ansGridView1.CurrentCell.OwningColumn.Name == "Amount")
            {
                calc();
                SendKeys.Send("{down}");
                SendKeys.Send("{up}");
            }
            DataTable dtPackVal = new DataTable();
            Database.GetSqlData("select pvalue from Description where Des_id='" + gdesid + "' ", dtPackVal);
            double pval = 0;
            if (dtPackVal.Rows.Count > 0)
            {
                pval = double.Parse(dtPackVal.Rows[0][0].ToString());
            }
            if (ansGridView1.CurrentCell.OwningColumn.Name == "Camount" && ansGridView1.Rows[e.RowIndex].Cells["Amount"].Value == null)
            {
                ansGridView1.Rows[e.RowIndex].Cells["Amount"].Value = double.Parse(ansGridView1.Rows[e.RowIndex].Cells["Camount"].Value.ToString()) / (pval*gqty);
            }
            ansGridView1.Rows[e.RowIndex].Cells["Vi_id"].Value = gvid;
            ansGridView1.Rows[e.RowIndex].Cells["Itemsr"].Value = gitemsr;
            ansGridView1.Rows[e.RowIndex].Cells["Chargesr"].Value = ansGridView1.Rows[e.RowIndex].Index + 1;
            ansGridView1.Rows[e.RowIndex].Cells["Charg_id"].Value = funs.Select_ch_id(ansGridView1.Rows[e.RowIndex].Cells["ChargeName"].Value.ToString());

            DataTable dtCharges = new DataTable();
            Database.GetSqlData("select Ch_id,Ac_id,Charge_type,Add_sub from charges where [name]='" + ansGridView1.Rows[e.RowIndex].Cells["ChargeName"].Value + "'", dtCharges);
            ansGridView1.Rows[e.RowIndex].Cells["Charg_id"].Value = dtCharges.Rows[0]["Ch_id"];
            ansGridView1.Rows[e.RowIndex].Cells["Accid"].Value = dtCharges.Rows[0]["Ac_id"];
            ansGridView1.Rows[e.RowIndex].Cells["Ctype"].Value = dtCharges.Rows[0]["Charge_type"];
            ansGridView1.Rows[e.RowIndex].Cells["Addsub"].Value = dtCharges.Rows[0]["Add_sub"];
        }

        private void ansGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            ansGridView1.CurrentCell.Value = 0;
        }

        private void ansGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                ansGridView1.Rows.RemoveAt(ansGridView1.CurrentRow.Index);
                calc();
                return;
            }
           
        }

        private void calc()
        {
            double tempamt = bamt;
            gtot = 0;
            for (int i = 0; i < ansGridView1.Rows.Count - 1; i++)
            {
                DataTable dtAddSub = new DataTable();
                if (ansGridView1.Rows[i].Cells["ChargeName"].Value.ToString() != "")
                {
                    Database.GetSqlData("select Charge_type,add_sub from charges where [name]='" + ansGridView1.Rows[i].Cells["ChargeName"].Value + "'", dtAddSub);
                }
                if (dtAddSub.Rows.Count > 0)
                {
                    if (int.Parse(dtAddSub.Rows[0]["Charge_type"].ToString()) == 1 && int.Parse(dtAddSub.Rows[0]["add_sub"].ToString()) == 4)
                    {
                        ansGridView1.Rows[i].Cells["Camount"].Value = funs.DecimalPoint((tempamt * double.Parse(ansGridView1.Rows[i].Cells["Amount"].Value.ToString())) / 100);
                    }
                    else if (int.Parse(dtAddSub.Rows[0]["Charge_type"].ToString()) == 1 && int.Parse(dtAddSub.Rows[0]["add_sub"].ToString()) == 5)
                    {
                        ansGridView1.Rows[i].Cells["Camount"].Value = funs.DecimalPoint(-(tempamt * double.Parse(ansGridView1.Rows[i].Cells["Amount"].Value.ToString()) / 100));
                    }
                    else if (int.Parse(dtAddSub.Rows[0]["Charge_type"].ToString()) == 3 && int.Parse(dtAddSub.Rows[0]["add_sub"].ToString()) == 4)
                    {
                        ansGridView1.Rows[i].Cells["Camount"].Value = funs.DecimalPoint(double.Parse(ansGridView1.Rows[i].Cells["Amount"].Value.ToString()));
                    }
                    else if (int.Parse(dtAddSub.Rows[0]["Charge_type"].ToString()) == 3 && int.Parse(dtAddSub.Rows[0]["add_sub"].ToString()) == 5)
                    {
                        ansGridView1.Rows[i].Cells["Camount"].Value = funs.DecimalPoint(-(double.Parse(ansGridView1.Rows[i].Cells["Amount"].Value.ToString())));
                    }
                    else if (int.Parse(dtAddSub.Rows[0]["Charge_type"].ToString()) == 2 && int.Parse(dtAddSub.Rows[0]["add_sub"].ToString()) == 4)
                    {
                        ansGridView1.Rows[i].Cells["Camount"].Value = funs.DecimalPoint(double.Parse(ansGridView1.Rows[i].Cells["Amount"].Value.ToString()) * funs.Select_pack_value(gdesid) * gqty);
                    }
                    else if (int.Parse(dtAddSub.Rows[0]["Charge_type"].ToString()) == 2 && int.Parse(dtAddSub.Rows[0]["add_sub"].ToString()) == 5)
                    {
                        ansGridView1.Rows[i].Cells["Camount"].Value = funs.DecimalPoint(-(double.Parse(ansGridView1.Rows[i].Cells["Amount"].Value.ToString()) * funs.Select_pack_value(gdesid) * gqty));
                    }
                    else if (int.Parse(dtAddSub.Rows[0]["Charge_type"].ToString()) == 4 && int.Parse(dtAddSub.Rows[0]["add_sub"].ToString()) == 4)
                    {
                        ansGridView1.Rows[i].Cells["Camount"].Value = funs.DecimalPoint(double.Parse(ansGridView1.Rows[i].Cells["Amount"].Value.ToString()) * gqty);
                    }
                    else if (int.Parse(dtAddSub.Rows[0]["Charge_type"].ToString()) == 4 && int.Parse(dtAddSub.Rows[0]["add_sub"].ToString()) == 5)
                    {
                        ansGridView1.Rows[i].Cells["Camount"].Value = funs.DecimalPoint(-(double.Parse(ansGridView1.Rows[i].Cells["Amount"].Value.ToString()) * gqty));
                    }

                    tempamt += double.Parse(ansGridView1.Rows[i].Cells["Camount"].Value.ToString());
                    gtot += double.Parse(ansGridView1.Rows[i].Cells["Camount"].Value.ToString());
                }
            }
        }
    }
}
