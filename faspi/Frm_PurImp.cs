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
    public partial class Frm_PurImp : Form
    {
        String strCombo;
        String wh1 = "", wh2 = "";
        public String SubCategory_Name = "Local Purchase";
        bool gtaxinvoice = false;
        String desc = "", unit = "";

        public Frm_PurImp()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            dataGridView2.Rows.Clear();
            label2.Text = "*********";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView2.Rows.Clear();
            string s = Clipboard.GetText();
            if (s.IndexOf("Invoice Number:") == -1)
            {
                return;
            }

            label2.Text = s.Substring(s.IndexOf("Invoice Number:") + 15, 12).Replace("\r", "").Trim();

            s = s.Substring(s.IndexOf("Item Amt.(Rs.)") + 15, s.Length - (s.IndexOf("Item Amt.(Rs.)") + 15));
            string[] lines = s.Split('\n');
            int row = 0;

            foreach (string line in lines)
            {
                if (line == "\r")
                {
                    break;
                }
                else if (line.Length > 10)
                {
                    dataGridView2.Rows.Add();
                }
                else
                {
                    continue;
                }
                if (row < dataGridView2.RowCount && line.Length > 0)
                {
                    string[] sCells = line.Split('\t');
                    for (int i = 0; i < sCells.GetLength(0); i++)
                    {
                        if (i < this.dataGridView2.Columns.Count)
                        {
                            dataGridView2[i, (row)].Value = sCells[i].ToString();
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                else
                {
                    break;
                }
                row++;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

            if (validate() == true)
            {
                DataTable dtSave = new DataTable();
                dtSave.Columns.Add("SKU");
                dtSave.Columns.Add("SKUCode");
                dtSave.Columns.Add("Description");
                dtSave.Columns.Add("Packing");
                dtSave.Columns.Add("Quantity");
                dtSave.Columns.Add("Rate");
                dtSave.Columns.Add("Inbillscheme");
                dtSave.Columns.Add("CD");
                dtSave.Columns.Add("Category_id");
                dtSave.Columns.Add("Description_id");
                dtSave.Columns.Add("cost");
                dtSave.Columns.Add("Ino");
                dtSave.Columns.Add("Idt");
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    dtSave.Rows.Add();
                    dtSave.Rows[i]["SKU"] = dataGridView1.Rows[i].Cells["dataGridViewTextBoxColumn3"].Value.ToString();
                    dtSave.Rows[i]["SKUCode"] = dataGridView1.Rows[i].Cells["dataGridViewTextBoxColumn2"].Value.ToString();
                    dtSave.Rows[i]["Description"] = dataGridView1.Rows[i].Cells["Description"].Value.ToString();
                    dtSave.Rows[i]["Packing"] = dataGridView1.Rows[i].Cells["Packing"].Value.ToString();
                    dtSave.Rows[i]["Quantity"] = dataGridView1.Rows[i].Cells["dataGridViewTextBoxColumn4"].Value.ToString();
                    if (dataGridView1.Rows[i].Cells["Rate"].Value == null)
                    {
                        dataGridView1.Rows[i].Cells["Rate"].Value = "0";
                    }
                    dtSave.Rows[i]["Rate"] = dataGridView1.Rows[i].Cells["Rate"].Value.ToString();
                    dtSave.Rows[i]["Inbillscheme"] = dataGridView1.Rows[i].Cells["dataGridViewTextBoxColumn5"].Value.ToString();
                    dtSave.Rows[i]["CD"] = dataGridView1.Rows[i].Cells["CD"].Value.ToString();
                    dtSave.Rows[i]["Category_id"] = dataGridView1.Rows[i].Cells["Category_id"].Value.ToString();
                    dtSave.Rows[i]["Description_id"] = dataGridView1.Rows[i].Cells["description_id"].Value.ToString();
                    dtSave.Rows[i]["cost"] = dataGridView1.Rows[i].Cells["Rate"].Value.ToString();
                    dtSave.Rows[i]["Ino"] = label2.Text;
                    dtSave.Rows[i]["Idt"] = dataGridView2.Rows[i].Cells["Date"].Value.ToString();
                    Database.CommandExecutor("UPDATE DESCRIPTION SET Skucode = '" + dataGridView1.Rows[i].Cells["dataGridViewTextBoxColumn2"].Value.ToString() + "' WHERE Des_id='" + dataGridView1.Rows[i].Cells["description_id"].Value.ToString() + "' ");
                }
                frmTransaction frm = new frmTransaction();
                frm.MdiParent = this.MdiParent;
                frm.Show();
                frm.LoadData("", "Purchase", true, false, false);
                frm.ImportData(dtSave);
                this.Close();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            tabControl1.SelectedIndex = 1;

            for (int i = 0; i < dataGridView2.Rows.Count; i++)
            {
                dataGridView1.Rows.Add();
                dataGridView1.Rows[i].Cells["dataGridViewTextBoxColumn2"].Value = dataGridView2.Rows[i].Cells["SKUCode"].Value;
                dataGridView1.Rows[i].Cells["dataGridViewTextBoxColumn3"].Value = dataGridView2.Rows[i].Cells["SKU"].Value;
                DataTable dtDescription = new DataTable("Description");
                Database.GetSqlData("Select * from DESCRIPTION", dtDescription);
                DataRow row = dtDescription.Select("Skucode='" + dataGridView2.Rows[i].Cells["SKUCode"].Value.ToString() + "'").FirstOrDefault();
                //Double TaxPer=0;
                if (row != null)
                {
                    dataGridView1.Rows[i].Cells["Description"].Value = row["Description"];
                    dataGridView1.Rows[i].Cells["Packing"].Value = row["Pack"];
                    dataGridView1.Rows[i].Cells["Category_id"].Value = row["Tax_Cat_id"];
                    dataGridView1.Rows[i].Cells["description_id"].Value = row["Des_id"];
                }
                dataGridView1.Rows[i].Cells["cd"].Value = textBox1.Text;
                dataGridView1.Rows[i].Cells["Rate"].Value = "";
                dataGridView1.Rows[i].Cells["dataGridViewTextBoxColumn4"].Value = dataGridView2.Rows[i].Cells["Qty"].Value;
                if (dataGridView2.Rows[i].Cells["Scheme"].Value.ToString() == "")
                {
                    dataGridView1.Rows[i].Cells["dataGridViewTextBoxColumn5"].Value = 0;
                }
                else
                {
                    dataGridView1.Rows[i].Cells["dataGridViewTextBoxColumn5"].Value = dataGridView2.Rows[i].Cells["Scheme"].Value.ToString().Replace("-", "");
                }
                if (dataGridView2.Rows[i].Cells["NetValue"].Value.ToString() == "")
                {
                    dataGridView1.Rows[i].Cells["Amount"].Value = 0;
                }
                else
                {
                    dataGridView1.Rows[i].Cells["Amount"].Value = dataGridView2.Rows[i].Cells["NetValue"].Value;
                }
                double BCD = (double.Parse(dataGridView1.Rows[i].Cells["Amount"].Value.ToString()) * 100) / (100 - double.Parse(dataGridView1.Rows[i].Cells["CD"].Value.ToString()));
                double DPL = BCD;
                if (dataGridView2.Rows[i].Cells["Discount"].Value.ToString() != "")
                {
                    DPL = (BCD + double.Parse(dataGridView2.Rows[i].Cells["Discount"].Value.ToString().Replace("-", ""))) / double.Parse(dataGridView2.Rows[i].Cells["Qty"].Value.ToString());
                }
                else
                {
                    DPL = BCD / double.Parse(dataGridView2.Rows[i].Cells["Qty"].Value.ToString());
                }
                dataGridView1.Rows[i].Cells["Rate"].Value = funs.DecimalPoint(DPL);
            }
        }

        private bool validate()
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (dataGridView1.Rows[i].Cells["Description"].Value == null)
                {
                    dataGridView1.Rows[i].Cells["Description"].Style.BackColor = Color.Red;
                    return false;
                }
                if (dataGridView1.Rows[i].Cells["Packing"].Value == null || dataGridView1.Rows[i].Cells["Packing"].Value == "")
                {
                    dataGridView1.Rows[i].Cells["Packing"].Style.BackColor = Color.Red;
                    return false;
                }
            }
            return true;
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (dataGridView1.CurrentCell == null)
            {
                return;
            }
            if (dataGridView1.CurrentCell.OwningColumn.Name == "Description")
            {

                if (e.KeyCode == Keys.F7)
                {
                    if (dataGridView1.CurrentCell.Value.ToString() != "" && dataGridView1.CurrentCell.OwningRow.Cells["description_id"].Value != null)
                    {
                        string tdid = funs.EditDescription(dataGridView1.CurrentCell.OwningRow.Cells["description_id"].Value.ToString());
                        DataTable dttemp = new DataTable();
                        Database.GetSqlData("select Pack,Tax_Cat_id from Description where Des_id='" + tdid + "' ",dttemp);
                        dataGridView1.CurrentCell.OwningRow.Cells["description"].Value = dataGridView1.CurrentCell.Value;
                        dataGridView1.CurrentCell.OwningRow.Cells["description_id"].Value = tdid;
                        dataGridView1.CurrentCell.OwningRow.Cells["Packing"].Value = dttemp.Rows[0]["Pack"];
                        dataGridView1.CurrentCell.OwningRow.Cells["Category_id"].Value = dttemp.Rows[0]["Tax_Cat_id"];
                    }
                }
                else if (e.KeyCode == Keys.F8)
                {
                    string tdid = funs.AddDescription();
                    DataTable dttemp = new DataTable();
                    Database.GetSqlData("select Description,Pack,Tax_Cat_id from Description where Des_id='" + tdid + "' ", dttemp);
                    dataGridView1.CurrentCell.Value = dttemp.Rows[0]["Description"];
                    dataGridView1.CurrentCell.OwningRow.Cells["description"].Value = dataGridView1.CurrentCell.Value;
                    dataGridView1.CurrentCell.OwningRow.Cells["description_id"].Value = tdid;
                    dataGridView1.CurrentCell.OwningRow.Cells["Packing"].Value = dttemp.Rows[0]["Pack"];
                    dataGridView1.CurrentCell.OwningRow.Cells["Category_id"].Value = dttemp.Rows[0]["Tax_Cat_id"];
                }
                else if (e.KeyCode == Keys.Delete)
                {
                    dataGridView1.CurrentCell.OwningRow.Cells["description"].Value = "";
                    dataGridView1.CurrentCell.OwningRow.Cells["Packing"].Value = "";
                    dataGridView1.CurrentCell.OwningRow.Cells["description_id"].Value = "";
                    desc = "";
                    unit = "";

                }
            }

        }

        private void dataGridView1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (dataGridView1.CurrentCell.OwningColumn.Name == "Description")
            {

                string StrSql = "";

                if (dataGridView1.CurrentCell.OwningRow.Cells["packing"].Value != null && dataGridView1.CurrentCell.OwningRow.Cells["packing"].Value != "")
                {
                    wh1 = " DESCRIPTION.Pack ='" + dataGridView1.CurrentCell.OwningRow.Cells["packing"].Value.ToString() + "' ";
                }
                if (wh1 != "")
                {
                    wh2 = " AND TAXCATEGORYDETAIL.SubCategory_Name='" + SubCategory_Name + "'";
                }
                else
                {
                    wh2 = " TAXCATEGORYDETAIL.SubCategory_Name='" + SubCategory_Name + "'";
                }
                StrSql = "SELECT DISTINCT DESCRIPTION.Description FROM (DESCRIPTION INNER JOIN TAXCATEGORY ON DESCRIPTION.Tax_Cat_id = TAXCATEGORY.Category_Id) INNER JOIN TAXCATEGORYDETAIL ON TAXCATEGORY.Category_Id = TAXCATEGORYDETAIL.Category_Id WHERE " + wh1 + wh2 + " GROUP BY DESCRIPTION.Description, DESCRIPTION.Pack_id, TAXCATEGORYDETAIL.SubCategory_Name";

                if (gtaxinvoice == true)
                {
                    StrSql = "SELECT DISTINCT DESCRIPTION.Description FROM (DESCRIPTION INNER JOIN TAXCATEGORY ON DESCRIPTION.Tax_Cat_id = TAXCATEGORY.Category_Id) INNER JOIN TAXCATEGORYDETAIL ON TAXCATEGORY.Category_Id = TAXCATEGORYDETAIL.Category_Id WHERE " + wh1 + wh2 + " GROUP BY DESCRIPTION.Description, DESCRIPTION.Pack_id, TAXCATEGORYDETAIL.SubCategory_Name HAVING (((Sum(TAXCATEGORYDETAIL.Tax_Rate))<>0))";
                }

                String retStr = SelectCombo.ComboKeypress(this, e.KeyChar, StrSql, e.KeyChar.ToString(), 0);
                if (retStr != "")
                {
                    dataGridView1.CurrentCell.Value = retStr;


                }
                else
                {
                    dataGridView1.CurrentCell.Value = "";
                    dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells["description"].Value = "";
                    return;
                }

                String strCnt = "";
                if (dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells["description"].Value == null || dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells["description"].Value.ToString() == "")
                {
                    strCombo = "select [name] from packing";
                    strCnt = "select count(*) from packing";
                }
                else
                {
                    strCombo = "select [name] from packing where Pack_id in (select Pack_id from description where description='" + dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells["Description"].Value.ToString() + "')";
                    strCnt = "select count(*) from packing where Pack_id in (select Pack_id from description where description='" + dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells["Description"].Value.ToString() + "')";

                }


                int cnt;
                cnt = Database.GetScalar(strCnt);
                if (cnt == 1)
                {
                    dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells["Packing"].Value = Database.GetScalarText(strCombo);
                    unit = dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells["Packing"].Value.ToString();

                }
                else if (cnt == 0)
                {
                    dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells["Packing"].Value = "";
                    unit = "";

                }
                desc = dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells["Description"].Value.ToString();

                dataGridView1.Select();
                SendKeys.Send("{right}");
            }
            if (dataGridView1.CurrentCell.OwningColumn.Name == "Packing")
            {
                if (dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells["Description"].Value == null || dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells["Description"].Value.ToString() == "")
                {
                    strCombo = "select distinct [pack] from description order by description";
                }
                else
                {
                    strCombo = "select Pack from description where description='" + dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells["Description"].Value.ToString() + "' ";
                }
                String packing = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
                DataTable dtDesc = new DataTable();
                Database.GetSqlData("select description,Pack from description where shortcode='" + packing + "'", dtDesc);
                if (dtDesc.Rows.Count == 1)
                {

                    dataGridView1.CurrentCell.OwningRow.Cells["Packing"].Value = dtDesc.Rows[0]["Pack"].ToString();
                    dataGridView1.CurrentCell.OwningRow.Cells["description"].Value = dtDesc.Rows[0]["description"];
                }
                dataGridView1.CurrentCell.Value = packing;
                if (dataGridView1.CurrentCell.OwningRow.Cells["description"].Value == null)
                {
                    dataGridView1.CurrentCell.OwningRow.Cells["description"].Value = "";
                }
                unit = dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells["Packing"].Value.ToString();
                dataGridView1.Select();
                SendKeys.Send("{down}");
                SendKeys.Send("{left}");
            }

            if (unit != null && desc != null)
            {
                getDescdata(desc, unit);
            }
        }

        private void getDescdata(String dsc, String un)
        {
            DataTable dtImportRate = new DataTable();
            dtImportRate.Clear();
            Database.GetSqlData("select Des_id,Tax_Cat_id,Purchase_rate from description where description='" + dsc + "' and Pack='" + un + "' ", dtImportRate);
            if (dtImportRate.Rows.Count > 0)
            {
                dataGridView1.CurrentCell.OwningRow.Cells["Category_id"].Value = dtImportRate.Rows[0]["Tax_Cat_id"].ToString();
                dataGridView1.CurrentCell.OwningRow.Cells["Description_id"].Value = dtImportRate.Rows[0]["Des_id"].ToString();
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
        }

        private void Frm_PurImp_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                DialogResult chk = MessageBox.Show("Are u sure?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (chk == DialogResult.No)
                {
                    e.Handled = false;
                }
                else
                {
                    this.Close();
                    this.Dispose();
                }
            }
        }
    }
}

