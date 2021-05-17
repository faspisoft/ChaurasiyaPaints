using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using Microsoft.Office.Core;
using Excel = Microsoft.Office.Interop.Excel;
using System.Web.Script.Serialization;

namespace faspi
{
    public partial class frmMaster : Form
    {
        string gstr = "";
        DataTable dt;
        BindingSource bs = new BindingSource();
        DataTable dtitem = new DataTable();
        public ToolStripProgressBar ProgrBar;
        List<UsersFeature> permission;
        public frmMaster()
        {
            InitializeComponent();
        }

        public void LoadData(string str , string frmCaption)
        {
            gstr = str;
            string sql = "";
            dtitem.Clear();
            this.Text = frmCaption;
            permission = funs.GetPermissionKey(str);
            if (permission != null)
            {
                UsersFeature ob = permission.Where(w => w.FeatureName == "Delete").FirstOrDefault();
                if (ob != null && ob.SelectedValue == "Allowed")
                {
                    ansGridView5.Columns["Delete"].Visible = true;
                }
                else
                {
                    ansGridView5.Columns["Delete"].Visible = false;
                }
            }
            if (str == "StockItem")
            {

                //if (Database.utype.ToUpper() == "SUPERADMIN" || Database.utype.ToUpper() == "ADMIN")
                //{
                    sql = "SELECT DISTINCT OTHER.Name AS Company, OTHER_1.Name AS Brand, OTHER_2.Name AS Color, TAXCATEGORY.Category_Name AS TaxCategory, Description.Description AS DisplayName, Userinfo.Uname AS Modified_By FROM Description LEFT OUTER JOIN Userinfo ON Description.Modifiedby = Userinfo.U_id LEFT OUTER JOIN OTHER ON Description.Company_id = OTHER.Oth_id LEFT OUTER JOIN OTHER AS OTHER_1 ON Description.Item_id = OTHER_1.Oth_id LEFT OUTER JOIN OTHER AS OTHER_2 ON Description.Col_id = OTHER_2.Oth_id LEFT OUTER JOIN TAXCATEGORY ON Description.Tax_Cat_id = TAXCATEGORY.Category_Id ORDER BY DisplayName, TaxCategory";
                //}
                //else if (Database.utype.ToUpper() == "SUPERUSER" || Database.utype.ToUpper() == "USER")
                //{
                //    sql = "SELECT DISTINCT OTHER.Name AS Company, OTHER_1.Name AS Brand, OTHER_2.Name AS Color, TAXCATEGORY.Category_Name AS TaxCategory, Description.Description AS DisplayName, Userinfo.Uname AS Modified_By FROM Description LEFT OUTER JOIN Userinfo ON Description.Modifiedby = Userinfo.U_id LEFT OUTER JOIN OTHER ON Description.Company_id = OTHER.Oth_id LEFT OUTER JOIN OTHER AS OTHER_1 ON Description.Item_id = OTHER_1.Oth_id LEFT OUTER JOIN OTHER AS OTHER_2 ON Description.Col_id = OTHER_2.Oth_id LEFT OUTER JOIN TAXCATEGORY ON Description.Tax_Cat_id = TAXCATEGORY.Category_Id  ORDER BY DisplayName, TaxCategory";
                //}
                
                Database.GetSqlData(sql, dtitem);
                ansGridView5.DataSource = dtitem;
                ansGridView5.ReadOnly = true;
                if (Feature.Available("Company Colour") == "No")
                {
                    ansGridView5.Columns["Company"].Visible = false;
                    ansGridView5.Columns["Brand"].Visible = false;
                    ansGridView5.Columns["Color"].Visible = false;
                }
                else
                {
                    ansGridView5.Columns["Company"].Visible = true;
                    ansGridView5.Columns["Brand"].Visible = true;
                    ansGridView5.Columns["Color"].Visible = true;
                }
                if (Feature.Available("Taxation Applicable") == "VAT")
                {
                    ansGridView5.Columns["TaxCategory"].HeaderText = "TaxCategory";
                }
                else
                {
                    ansGridView5.Columns["TaxCategory"].HeaderText = "HSN";
                }


                label2.Text = "List of StockItems";
            }

            else if (str == "Account")
            {
                // sql = "SELECT ACCOUNT.Name AS AccName, ACCOUNTYPE.Name AS Type, Userinfo_1.Uname AS Created_By, Userinfo.Uname AS Modified_By FROM ACCOUNTYPE RIGHT OUTER JOIN Userinfo AS Userinfo_1 RIGHT OUTER JOIN Userinfo RIGHT OUTER JOIN ACCOUNT ON Userinfo.U_id = ACCOUNT.Modifiedby ON Userinfo_1.U_id = ACCOUNT.user_id ON ACCOUNTYPE.Act_id = ACCOUNT.Act_id WHERE (ACCOUNT.Ac_id <> 'MAN1') AND (ACCOUNT.Branch_id = '" + Database.BranchId + "') ORDER BY AccName";
                sql = "SELECT ACCOUNT.Name AS AccName,  ACCOUNTYPE.Name AS Type,  OTHER.Name AS PaymentColl,  City.CName AS City FROM  City RIGHT OUTER JOIN    ACCOUNT ON  City.City_id =  ACCOUNT.city_id LEFT OUTER JOIN     OTHER ON  ACCOUNT.Loc_id =  OTHER.Oth_id LEFT OUTER JOIN     ACCOUNTYPE ON  ACCOUNT.Act_id = ACCOUNTYPE.Act_id WHERE(ACCOUNT.Ac_id <> 'MAN1') AND (ACCOUNT.Branch_id = '" + Database.BranchId + "') ORDER BY AccName";
                Database.GetSqlData(sql, dtitem);
                ansGridView5.DataSource = dtitem;
                ansGridView5.ReadOnly = true;
                label2.Text = "List of Accounts";

              

            }
            else if (str == "City")
            {

                sql = "SELECT CName as Name from City ORDER BY CName";
                Database.GetSqlData(sql, dtitem);
                ansGridView5.DataSource = dtitem;
                label2.Text = "List of Cities";
            }
            else if (str == "Copy Rate")
            {
                sql = "SELECT CopyRates.Cr_id, OTHER_1.Name AS Company, OTHER_2.Name AS Item, OTHER.Name AS PriceGrp, CopyRates.Pack,   PackCategory.Name AS PackingCat, dbo.CopyRates.rateto as RatetoUpd, dbo.CopyRates.Rebate2 as [Rebate] FROM CopyRates LEFT OUTER JOIN  PackCategory ON CopyRates.Pack_category_id = PackCategory.PackCat_id LEFT OUTER JOIN  Description ON CopyRates.Description = Description.Description LEFT OUTER JOIN  OTHER ON CopyRates.Group_id = OTHER.Oth_id LEFT OUTER JOIN  OTHER AS OTHER_2 ON CopyRates.Item_id = OTHER_2.Oth_id LEFT OUTER JOIN  OTHER AS OTHER_1 ON CopyRates.Company_id = OTHER_1.Oth_id GROUP BY CopyRates.Cr_id, OTHER_1.Name, OTHER_2.Name, OTHER.Name, CopyRates.Pack, PackCategory.Name, dbo.CopyRates.rateto, dbo.CopyRates.Rebate2 order by OTHER_1.Name, OTHER_2.Name, OTHER.Name, CopyRates.Pack,   PackCategory.Name, dbo.CopyRates.rateto";
                Database.GetSqlData(sql, dtitem);
                for (int i = 0; i < dtitem.Rows.Count; i++)
                {
                    if (dtitem.Columns["Rebate"].DataType.Name == "Decimal")
                    {
                        dtitem.Rows[i]["Rebate"] = funs.IndianCurr(double.Parse(dtitem.Rows[i]["Rebate"].ToString()));
                    }
                }
                ansGridView5.DataSource = dtitem;
                //ansGridView5.ReadOnly = true;
                checkBox1.Visible = true;
                ansGridView5.Columns["select"].Visible = true;
                ansGridView5.Columns["select"].ReadOnly = false;
                ansGridView5.Columns["Company"].ReadOnly = true;
                ansGridView5.Columns["Item"].ReadOnly = true;
                ansGridView5.Columns["PriceGrp"].ReadOnly = true;
                ansGridView5.Columns["RatetoUpd"].ReadOnly = true;
                ansGridView5.Columns["Rebate"].ReadOnly = true;
                ansGridView5.Columns["Pack"].ReadOnly = true;
                ansGridView5.Columns["PackingCat"].ReadOnly = true;
                ansGridView5.Columns["CR_id"].Visible = false;
                label2.Text = "List of Copy Rates";
            }


            else if (str == "Container")
            {
                sql = "SELECT Container.Cname AS Name, Userinfo_1.Uname AS Created_By, Userinfo.Uname AS Modified_By FROM Container LEFT OUTER JOIN Userinfo AS Userinfo_1 ON Container.user_id = Userinfo_1.U_id LEFT OUTER JOIN Userinfo ON Container.Modifiedby = Userinfo.U_id ORDER BY Name";
                
                Database.GetSqlData(sql, dtitem);
                ansGridView5.DataSource = dtitem;
                ansGridView5.ReadOnly = true;
               // ansGridView5.Columns["select"].Visible = true;
                label2.Text = "List of Containers";
            }

            else if (str == "Customer/Supplier Rate")
            {
                sql = "SELECT DISTINCT ACCOUNT.Name FROM ACCOUNT RIGHT OUTER JOIN PARTYRATE ON ACCOUNT.Ac_id = PARTYRATE.Ac_id ORDER BY ACCOUNT.Name";
                Database.GetSqlData(sql, dtitem);
                ansGridView5.DataSource = dtitem;
                ansGridView5.ReadOnly = true;
                label2.Text = "List of Customer/Supplier Rate";
            }

            else if (str == "User")
            {
              //  sql = "SELECT Userinfo.Uname AS Name, Userinfo.Utype, Userinfo_1.Uname AS Created_By, Userinfo_2.Uname AS Modified_By FROM Userinfo LEFT OUTER JOIN Userinfo AS Userinfo_2 ON Userinfo.Modifiedby = Userinfo_2.U_id LEFT OUTER JOIN Userinfo AS Userinfo_1 ON Userinfo.user_id = Userinfo_1.U_id WHERE Userinfo.Uname <> '" + Database.uname + "' ORDER BY Name";
                sql = "SELECT    Userinfo.Uname AS Name, dbo.SYS_Role.RoleName, Userinfo_1.Uname AS Created_By, Userinfo_2.Uname AS Modified_By FROM         dbo.Userinfo LEFT OUTER JOIN SYS_Role ON dbo.Userinfo.roleid = dbo.SYS_Role.Role_ID LEFT OUTER JOIN Userinfo AS Userinfo_2 ON dbo.Userinfo.Modifiedby = Userinfo_2.U_id LEFT OUTER JOIN Userinfo AS Userinfo_1 ON dbo.Userinfo.user_id = Userinfo_1.U_id WHERE     (dbo.Userinfo.Uname <> '"+ Database.utype+"') ORDER BY Name";
                Database.GetSqlData(sql, dtitem);
                ansGridView5.DataSource = dtitem;
                ansGridView5.ReadOnly = true;
                label2.Text = "List of Users";

            }
            else if (str == "Role")
            {
                sql = "SELECT RoleName as Name FROM SYS_Role ORDER BY RoleName";
                Database.GetSqlData(sql, dtitem);
                ansGridView5.DataSource = dtitem;
                ansGridView5.ReadOnly = true;
                ansGridView5.Columns["Delete"].Visible = false;
                label2.Text = "List of Roles";

            }
            else if (str == "ProductFormula")
            {
                sql = "SELECT     Description.Description, Description.Pack, ProductFormula.productionItem_id FROM         ProductFormula LEFT OUTER JOIN    Description ON ProductFormula.productionItem_id = Description.Des_id GROUP BY dbo.Description.Description, dbo.Description.Pack, dbo.ProductFormula.productionItem_id ";

                Database.GetSqlData(sql, dtitem);
                ansGridView5.DataSource = dtitem;
                ansGridView5.ReadOnly = true;
                label2.Text = "List of Production Formula";

                ansGridView5.Columns["productionItem_id"].Visible = false;
            }
            else if (str == "PackCategory")
            {
                sql = "SELECT name FROM PackCategory ORDER BY name";
                Database.GetSqlData(sql, dtitem);
                ansGridView5.DataSource = dtitem;
                ansGridView5.ReadOnly = true;
                label2.Text = "List of PackCategory";
            }
            else if (str == "DAT")
            {
                sql = "SELECT DisAfterTax.taxname, DisAfterTax.type FROM DisAfterTax ORDER BY DisAfterTax.taxname";
                Database.GetSqlData(sql, dtitem);
                ansGridView5.DataSource = dtitem;
                ansGridView5.ReadOnly = true;
                label2.Text = "List of Discounts";
            }

            else if (str == "Broker")
            {
                //sql = "SELECT CONTRACTOR.Name, Userinfo_1.Uname AS Created_By, Userinfo.Uname AS Modified_By FROM CONTRACTOR LEFT OUTER JOIN Userinfo ON CONTRACTOR.Modifiedby = Userinfo.U_id LEFT OUTER JOIN Userinfo AS Userinfo_1 ON CONTRACTOR.user_id = Userinfo_1.U_id ORDER BY CONTRACTOR.Name";
                sql = "SELECT CONTRACTOR.Name, ACCOUNT.Name AS Refference, Userinfo_1.Uname AS Created_By, Userinfo.Uname AS Modified_By FROM ACCOUNT RIGHT OUTER JOIN Userinfo AS Userinfo_1 RIGHT OUTER JOIN CONTRACTOR LEFT OUTER JOIN Userinfo ON CONTRACTOR.Modifiedby = Userinfo.U_id ON Userinfo_1.U_id = CONTRACTOR.user_id ON ACCOUNT.Ac_id = CONTRACTOR.Reff_id where  (CONTRACTOR.Branch_id = '" + Database.BranchId + "')  ORDER BY CONTRACTOR.Name, Refference";
                Database.GetSqlData(sql, dtitem);
                ansGridView5.DataSource = dtitem;
                ansGridView5.ReadOnly = true;
                label2.Text = "List of Brokers";
            }

            else if (str == "Account Group")
            {
                sql = "select Name from Accountype where Fixed="+ access_sql.Singlequote+"False"+ access_sql.Singlequote+" order by [name]";
                Database.GetSqlData(sql, dtitem);
                ansGridView5.DataSource = dtitem;
                ansGridView5.ReadOnly = true;
                label2.Text = "List of Account Groups";

            }

            else if (str == "Payment Collector")
            {
                sql = "SELECT OTHER.Name, Userinfo_1.Uname AS Created_By, Userinfo.Uname AS Modified_By FROM OTHER LEFT OUTER JOIN Userinfo ON OTHER.Modifiedby = Userinfo.U_id LEFT OUTER JOIN Userinfo AS Userinfo_1 ON OTHER.user_id = Userinfo_1.U_id WHERE OTHER.Type = 'SER17' ORDER BY OTHER.Name";
                //sql = "select Name from other where type='SER17' order by [name]";
                Database.GetSqlData(sql, dtitem);
                ansGridView5.DataSource = dtitem;
                ansGridView5.ReadOnly = true;
                label2.Text = "List of Payment Collectors"; 
            }

            else if (str == "Company")
            {
                sql = "SELECT OTHER.Name, Userinfo_1.Uname AS Created_By, Userinfo.Uname AS Modified_By FROM OTHER LEFT OUTER JOIN Userinfo ON OTHER.Modifiedby = Userinfo.U_id LEFT OUTER JOIN Userinfo AS Userinfo_1 ON OTHER.user_id = Userinfo_1.U_id WHERE OTHER.Type = '" + funs.Get_Company_id() + "' ORDER BY OTHER.Name";
                //sql = "SELECT OTHER.Name as Name FROM OTHER WHERE OTHER.Type='" + funs.Get_Company_id() + "' ORDER BY OTHER.Name";
                Database.GetSqlData(sql, dtitem);
                ansGridView5.DataSource = dtitem;
                ansGridView5.ReadOnly = true;
                label2.Text = "List of Company/Manufacturer";
            }
                
            else if (str == "PriceGroup")
            {
                sql = "SELECT OTHER.Name, Userinfo_1.Uname AS Created_By, Userinfo.Uname AS Modified_By FROM OTHER LEFT OUTER JOIN Userinfo ON OTHER.Modifiedby = Userinfo.U_id LEFT OUTER JOIN Userinfo AS Userinfo_1 ON OTHER.user_id = Userinfo_1.U_id WHERE OTHER.Type = '" + funs.Get_Group_id() + "' ORDER BY OTHER.Name";
                //sql = "SELECT OTHER.Name as Name FROM OTHER WHERE OTHER.Type='" + funs.Get_Group_id() + "' ORDER BY OTHER.Name";
                Database.GetSqlData(sql, dtitem);
                ansGridView5.ReadOnly = true;
                ansGridView5.DataSource = dtitem;
                label2.Text = "List of Price Group";
            }

            else if (str == "Item")
            {
                sql = "SELECT OTHER.Name, Userinfo_1.Uname AS Created_By, Userinfo.Uname AS Modified_By FROM OTHER LEFT OUTER JOIN Userinfo ON OTHER.Modifiedby = Userinfo.U_id LEFT OUTER JOIN Userinfo AS Userinfo_1 ON OTHER.user_id = Userinfo_1.U_id WHERE OTHER.Type = '" + funs.Get_Item_id() + "' ORDER BY OTHER.Name";
                //sql = "SELECT OTHER.Name as Name FROM OTHER WHERE OTHER.Type='" + funs.Get_Item_id() + "' ORDER BY OTHER.Name";
                Database.GetSqlData(sql, dtitem);
                ansGridView5.DataSource = dtitem;
                ansGridView5.ReadOnly = true;
                label2.Text = "List of Item Groups/Brand";

            }

            else if (str == "Colour")
            {
                sql = "SELECT OTHER.Name, Userinfo_1.Uname AS Created_By, Userinfo.Uname AS Modified_By FROM OTHER LEFT OUTER JOIN Userinfo ON OTHER.Modifiedby = Userinfo.U_id LEFT OUTER JOIN Userinfo AS Userinfo_1 ON OTHER.user_id = Userinfo_1.U_id WHERE OTHER.Type = '" + funs.Get_Colour_id() + "' ORDER BY OTHER.Name";
                //sql = "SELECT OTHER.Name as Name FROM OTHER WHERE OTHER.Type='" + funs.Get_Colour_id() + "' ORDER BY OTHER.Name";
                Database.GetSqlData(sql, dtitem);
                ansGridView5.DataSource = dtitem;
                ansGridView5.ReadOnly = true;
                label2.Text = "List of Colour/Variant";

            }

            else if (str == "TaxCategory")
            {
                sql = "SELECT TAXCATEGORY.Category_Name + '' as Category_Name, TAXCATEGORY.Commodity_Code , Userinfo_1.Uname AS Created_By, Userinfo.Uname AS Modified_By FROM TAXCATEGORY LEFT OUTER JOIN Userinfo ON TAXCATEGORY.Modifiedby = Userinfo.U_id LEFT OUTER JOIN Userinfo AS Userinfo_1 ON TAXCATEGORY.user_id = Userinfo_1.U_id ORDER BY TAXCATEGORY.Category_Name";
                //sql = "SELECT TAXCATEGORY.Category_Name as Name, TAXCATEGORY.Commodity_Code FROM TAXCATEGORY ORDER BY TAXCATEGORY.Category_Name";
                Database.GetSqlData(sql, dtitem);
                ansGridView5.DataSource = dtitem;
                ansGridView5.ReadOnly = true;

                if (Feature.Available("Taxation Applicable") == "VAT")
                {
                    label2.Text = "List of TaxCategories";
                }
                else
                {
                    ansGridView5.Columns["Commodity_Code"].HeaderText = "HSN No.";
                    label2.Text = "List of HSN";
                }
            }

            else if (str == "Tax")
            {
                sql = "SELECT TAXCATEGORY.Category_Name + '' AS Name, TAXCATEGORY.Commodity_Code + '' as Commodity_Code, TAXCATEGORY.STR1 + TAXCATEGORY.STR2 + '' AS [Tax%], Userinfo_1.Uname AS Created_By, Userinfo.Uname AS Modified_By FROM TAXCATEGORY LEFT OUTER JOIN Userinfo ON TAXCATEGORY.Modifiedby = Userinfo.U_id LEFT OUTER JOIN Userinfo AS Userinfo_1 ON TAXCATEGORY.user_id = Userinfo_1.U_id ORDER BY Name";
                //sql = "SELECT TAXCATEGORY.Category_Name as Name, TAXCATEGORY.Commodity_Code ,TAXCATEGORY.STR1+ TAXCATEGORY.STR2  " + access_sql.Concat + " '' as [Tax%]  FROM TAXCATEGORY ORDER BY TAXCATEGORY.Category_Name";
                Database.GetSqlData(sql, dtitem);
                ansGridView5.DataSource = dtitem;
                ansGridView5.ReadOnly = true;

                if (Feature.Available("Taxation Applicable") == "VAT")
                {
                    label2.Text = "List of Tax";
                }
                else
                {
                    ansGridView5.Columns["Commodity_Code"].HeaderText = "HSN No.";
                    label2.Text = "List of HSN";
                }
            }

            else if (str == "Charges")
            {
                sql = "SELECT CHARGES.Name, Userinfo_1.Uname AS Created_By, Userinfo.Uname AS Modified_By FROM CHARGES LEFT OUTER JOIN Userinfo ON CHARGES.Modifiedby = Userinfo.U_id LEFT OUTER JOIN Userinfo AS Userinfo_1 ON CHARGES.user_id = Userinfo_1.U_id ORDER BY CHARGES.Name";
                //sql = "SELECT CHARGES.Name as Name FROM CHARGES ORDER BY CHARGES.Name";
                Database.GetSqlData(sql, dtitem);
                ansGridView5.DataSource = dtitem;
                ansGridView5.ReadOnly = true;
                label2.Text = "List of Charges";

            }

            else if (str == "Salesman")
            {
                sql = "SELECT Salesman.Name, Userinfo_1.Uname AS Created_By, Userinfo.Uname AS Modified_By FROM Salesman LEFT OUTER JOIN Userinfo ON Salesman.Modifiedby = Userinfo.U_id LEFT OUTER JOIN Userinfo AS Userinfo_1 ON Salesman.user_id = Userinfo_1.U_id ORDER BY Salesman.Name";
                //sql = "SELECT CHARGES.Name as Name FROM CHARGES ORDER BY CHARGES.Name";
                Database.GetSqlData(sql, dtitem);
                ansGridView5.DataSource = dtitem;
                ansGridView5.ReadOnly = true;
                label2.Text = "List of Salesman";


            }

            else if (str == "ReminderDates")
            {
                sql = "select title as Title," + access_sql.fnDatFormatting("importantdate.Idate", Database.dformat) + " as ImportantDates from importantdate order by [title]";
                Database.GetSqlData(sql, dtitem);
                ansGridView5.DataSource = dtitem;
                label2.Text = "List of Reminders";
                button2.Visible = true;
            }

            else if (str == "Control Room")
            {
                sql = "SELECT [Group] AS GroupName, Features, Description, ToSuperAdmin AS SelectedValue FROM FirmSetup ORDER BY GroupName, Features";
                Database.GetSqlData(sql, dtitem);
                ansGridView5.DataSource = dtitem;
                ansGridView5.ReadOnly = true;
                label2.Text = "List of Control Room";
                ansGridView5.Columns["Delete"].Visible = false;
            }

            else if (str == "TransactionSetup")
            {
               
                    sql = "SELECT VOUCHERTYPE.Name FROM VOUCHERTYPE where Type<>'Report' and "+Database.BMode+"=" + access_sql.Singlequote + "true" + access_sql.Singlequote + " order by Name";
               

                Database.GetSqlData(sql, dtitem);
                ansGridView5.DataSource = dtitem;
                ansGridView5.ReadOnly = true;
                label2.Text = "Transaction Setup";
                ansGridView5.Columns["Delete"].Visible = false;
            }

            else if (str == "State")
            {
                sql = "SELECT State.Sname AS StateName, State.SPrintName AS PrintName, Userinfo_1.Uname AS Created_By, Userinfo.Uname AS Modified_By FROM State LEFT OUTER JOIN Userinfo ON State.Modifiedby = Userinfo.U_id LEFT OUTER JOIN Userinfo AS Userinfo_1 ON State.user_id = Userinfo_1.U_id ORDER BY StateName";
                //sql = "SELECT Sname as StateName, SPrintName as PrintName FROM State order by Sname";
                Database.GetSqlData(sql, dtitem);
                ansGridView5.DataSource = dtitem;
                ansGridView5.ReadOnly = true;
                label2.Text = "List of State";
            }

            textBox1.Focus();
            ansGridView5.Columns["Edit"].DataGridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            ansGridView5.Columns["Edit"].DisplayIndex = ansGridView5.Columns.Count - 2 + 1;
            ansGridView5.Columns["Delete"].DisplayIndex = ansGridView5.Columns.Count - 2 + 1;
            ansGridView5.Columns["select"].DisplayIndex = ansGridView5.Columns.Count - 2 + 1;
            //if (Database.utype.ToUpper() == "USER")
            //{
            //    ansGridView5.Columns["Delete"].Visible = false;
            //}
        }

        private void ansGridView5_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (gstr == "StockItem")
            {
                if(ansGridView5.CurrentCell.OwningColumn.Name=="Edit")
                {
                    frmDescription frm = new frmDescription();
                    frm.LoadData(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["DisplayName"].Value.ToString(), "Edit Description");
                    frm.MdiParent = this.MdiParent;
                    frm.Show();
                
                }
                else if (ansGridView5.CurrentCell.OwningColumn.Name == "Delete")
                {
                   if(validate(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["DisplayName"].Value.ToString()) ==true)
                    {
                        DialogResult res = MessageBox.Show("Are you sure?", "Confirm", MessageBoxButtons.OKCancel);
                        if (res == DialogResult.OK)
                        {
                            DataTable dtDelete = new DataTable();
                            dtDelete.TableName = "Description";
                            Database.GetSqlData("select * from Description where description='" + ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["DisplayName"].Value.ToString() + "' ", dtDelete);
                            for (int i = 0; i < dtDelete.Rows.Count; i++)
                            {
                               //DataTable dtdel = new DataTable("Deleted");
                               // Database.GetSqlData("Select * from Deleted", dtdel);
                               // dtdel.Rows.Add();
                               // dtdel.Rows[dtdel.Rows.Count - 1]["D_type"] = "Description";
                               // dtdel.Rows[dtdel.Rows.Count - 1]["Vi_id"] = int.Parse(dtDelete.Rows[i]["des_id"].ToString());
                               // dtdel.Rows[dtdel.Rows.Count - 1]["LocationId"] = Database.LocationId;
                               // Database.SaveData(dtdel);

                                dtDelete.Rows[i].Delete();
                            }
                            Database.SaveData(dtDelete);
                            Master.UpdateDecription();
                            Master.UpdateDecriptionInfo();
                          
                        }
                    }

                   LoadData(gstr,"StockItem");
                }
            }

            else if (gstr == "Copy Rate")
            {
                if (ansGridView5.CurrentCell.OwningColumn.Name == "Edit")
                {
                    if (ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["CR_id"].Value.ToString() == "0")
                    {
                        return;
                    }

                    frm_updaterate frm = new frm_updaterate();
                    frm.MdiParent = this.MdiParent;
                    frm.LoadData(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["CR_id"].Value.ToString(), "Copy Rate");
                    frm.Show();
                }
                else if (ansGridView5.CurrentCell.OwningColumn.Name == "Delete")
                {
                    if (ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["CR_id"].Value.ToString() != "0")
                    {
                        DialogResult res = MessageBox.Show("Are you sure?", "Confirm", MessageBoxButtons.OKCancel);
                        if (res == DialogResult.OK)
                        {
                            DataTable dtDelete = new DataTable();
                            dtDelete.TableName = "CopyRates";
                            Database.GetSqlData("select * from CopyRates where CR_id='" + ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["CR_id"].Value.ToString() + "' ", dtDelete);
                            for (int i = 0; i < dtDelete.Rows.Count; i++)
                            {
                                dtDelete.Rows[i].Delete();
                            }
                            Database.SaveData(dtDelete);
                        }
                    }
                    LoadData(gstr, "CopyRate");
                }
            }



            else if (gstr == "Container")
            {
                if (ansGridView5.CurrentCell.OwningColumn.Name == "Edit")
                {
                    if (funs.Select_Container_id(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Name"].Value.ToString()).ToString() == "0")
                    {
                        return;
                    }

                    frm_container frm = new frm_container();
                    frm.MdiParent = this.MdiParent;
                    frm.LoadData(funs.Select_Container_id(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Name"].Value.ToString()).ToString(), "Container");
                    frm.Show();
                }
                else if (ansGridView5.CurrentCell.OwningColumn.Name == "Delete")
                {
                    if (validate(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Name"].Value.ToString()) == true)
                    {
                        DialogResult res = MessageBox.Show("Are you sure?", "Confirm", MessageBoxButtons.OKCancel);
                        if (res == DialogResult.OK)
                        {
                            DataTable dtDelete = new DataTable();
                            dtDelete.TableName = "Container";
                            Database.GetSqlData("select * from Container where Cname='" + ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Name"].Value.ToString() + "' ", dtDelete);
                            for (int i = 0; i < dtDelete.Rows.Count; i++)
                            {
                                dtDelete.Rows[i].Delete();
                            }
                            Database.SaveData(dtDelete);
                        }
                    }
                    LoadData(gstr, "Container");
                }
            }

            else if (gstr == "City")
            {

                SideFill();

                if (ansGridView5.CurrentCell.OwningColumn.Name == "Edit")
                {
                    if (funs.Select_city_id(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Name"].Value.ToString()).ToString() == "0")
                    {
                        return;
                    }
                    frm_City frm = new frm_City();
                    frm.LoadData(funs.Select_city_id(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Name"].Value.ToString()).ToString(), "Edit City");
                    frm.MdiParent = this.MdiParent;
                    frm.Show();




                }
                else if (ansGridView5.CurrentCell.OwningColumn.Name == "Delete")
                {
                    if (validate(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Name"].Value.ToString()) == true)
                    {
                        DialogResult res = MessageBox.Show("Are you sure?", "Confirm", MessageBoxButtons.OKCancel);
                        if (res == DialogResult.OK)
                        {
                            DataTable dtDelete = new DataTable();
                            dtDelete.TableName = "City";
                            Database.GetSqlData("select * from City where CName='" + ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Name"].Value.ToString() + "' ", dtDelete);
                            for (int i = 0; i < dtDelete.Rows.Count; i++)
                            {
                                dtDelete.Rows[i].Delete();
                            }


                            Database.SaveData(dtDelete);


                        }
                    }
                    LoadData(gstr, "City");
                }
            }


            else if (gstr == "User")
            {
                if (ansGridView5.CurrentCell.OwningColumn.Name == "Edit")
                {
                    if (funs.Select_user_id(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Name"].Value.ToString()).ToString() == "0")
                    {
                        return;
                    }

                    frm_usermgmt frm = new frm_usermgmt();
                    frm.MdiParent = this.MdiParent;
                    frm.LoadData(funs.Select_user_id(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Name"].Value.ToString()).ToString(), "User");
                    frm.Show();
                }
                else if (ansGridView5.CurrentCell.OwningColumn.Name == "Delete")
                {
                    if (validate(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Name"].Value.ToString()) == true)
                    {
                        DialogResult res = MessageBox.Show("Are you sure?", "Confirm", MessageBoxButtons.OKCancel);
                        if (res == DialogResult.OK)
                        {
                            DataTable dtDelete = new DataTable();
                            dtDelete.TableName = "Userinfo";
                            Database.GetSqlData("select * from Userinfo where UName='" + ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Name"].Value.ToString() + "' ", dtDelete);
                            for (int i = 0; i < dtDelete.Rows.Count; i++)
                            {
                                dtDelete.Rows[i].Delete();
                            }
                            Database.SaveData(dtDelete);
                            //Master.UpdateColor();
                        }
                    }
                    LoadData(gstr, "User");
                }
            }



            else if (gstr == "Role")
            {
                if (ansGridView5.CurrentCell.OwningColumn.Name == "Edit")
                {
                    if (funs.Select_Role_id(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Name"].Value.ToString()).ToString() == "0")
                    {
                        return;
                    }

                    frm_role frm = new frm_role();
                    frm.MdiParent = this.MdiParent;
                    frm.LoadData(funs.Select_Role_id(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Name"].Value.ToString()).ToString(), "Role");
                    frm.Show();
                }
                else if (ansGridView5.CurrentCell.OwningColumn.Name == "Delete")
                {
                    if (validate(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Name"].Value.ToString()) == true)
                    {
                        DialogResult res = MessageBox.Show("Are you sure?", "Confirm", MessageBoxButtons.OKCancel);
                        if (res == DialogResult.OK)
                        {
                            DataTable dtDelete = new DataTable();
                            dtDelete.TableName = "SYS_Role";
                            Database.GetSqlData("select * from SYS_Role where RoleName='" + ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Name"].Value.ToString() + "' ", dtDelete);
                            for (int i = 0; i < dtDelete.Rows.Count; i++)
                            {
                                dtDelete.Rows[i].Delete();
                            }
                            Database.SaveData(dtDelete);
                            //Master.UpdateColor();
                        }
                    }
                    LoadData(gstr, "Role");
                }
            }


            else if (gstr == "Account")
            {

                SideFill();

                if (ansGridView5.CurrentCell.OwningColumn.Name == "Edit")
                {
                    if (funs.Select_ac_id(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["AccName"].Value.ToString()).ToString() == "0")
                    {
                        return;
                    }
                    frm_NewAcc frm = new frm_NewAcc();
                    frm.LoadData(funs.Select_ac_id(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["AccName"].Value.ToString()).ToString(), "Edit Account");
                    frm.MdiParent = this.MdiParent;
                    frm.Show();

                }
                else if (ansGridView5.CurrentCell.OwningColumn.Name == "Delete")
                {
                    if (validate(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["AccName"].Value.ToString()) == true)
                    {
                        DialogResult res = MessageBox.Show("Are you sure?", "Confirm", MessageBoxButtons.OKCancel);
                        if (res == DialogResult.OK)
                        {
                            string acid = funs.Select_ac_id(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["AccName"].Value.ToString());
                            DataTable dtDelete = new DataTable();
                            dtDelete.TableName = "BillAdjest";
                            Database.GetSqlData("select * from BillAdjest where Ac_id='" + acid+"'", dtDelete);
                            for (int i = 0; i < dtDelete.Rows.Count; i++)
                            {
                                dtDelete.Rows[i].Delete();
                            }
                            Database.SaveData(dtDelete);



                            dtDelete = new DataTable();
                            dtDelete.TableName = "Account";
                            Database.GetSqlData("select * from Account where Name='" + ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["AccName"].Value.ToString() + "' ", dtDelete);
                            for (int i = 0; i < dtDelete.Rows.Count; i++)
                            {
                                dtDelete.Rows[i].Delete();
                            }
                            Database.SaveData(dtDelete);



                            Master.UpdateAccount();
                            Master.UpdateAccountinfo();

                        }
                    }
                    LoadData(gstr,"Account");
                }
            }
            else if (gstr == "DAT")
            {
                if (ansGridView5.CurrentCell.OwningColumn.Name == "Edit")
                {
                    if (funs.Select_disaftertax_id(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["taxname"].Value.ToString()).ToString() == "0")
                    {
                        return;
                    }


                    frm_DAT frm = new frm_DAT();
                    frm.LoadData(funs.Select_disaftertax_id(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["taxname"].Value.ToString()).ToString(), "Edit Discount After Tax");
                    frm.MdiParent = this.MdiParent;
                    frm.Show();
                }
                else if (ansGridView5.CurrentCell.OwningColumn.Name == "Delete")
                {
                    if (validate(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["taxname"].Value.ToString()) == true)
                    {
                        DialogResult res = MessageBox.Show("Are you sure?", "Confirm", MessageBoxButtons.OKCancel);
                        if (res == DialogResult.OK)
                        {
                            DataTable dtDelete = new DataTable();
                            dtDelete.TableName = "DisAfterTax";
                            Database.GetSqlData("select * from DisAfterTax where taxname='" + ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["taxname"].Value.ToString() + "' ", dtDelete);
                            for (int i = 0; i < dtDelete.Rows.Count; i++)
                            {
                                dtDelete.Rows[i].Delete();
                            }
                            Database.SaveData(dtDelete);

                        }
                    }
                    LoadData(gstr, "Discount After Tax");
                }



            }

            else if (gstr == "ProductFormula")
            {
                if (ansGridView5.CurrentCell.OwningColumn.Name == "Edit")
                {
                    string productitem="";
                    productitem = ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["productionItem_id"].Value.ToString();
                    if (productitem == "0" || productitem == "")
                    {
                        return;
                    }


                    Frm_ProductFormula frm = new Frm_ProductFormula();
                    frm.LoadData(productitem, "Edit Product Formula");
                    frm.MdiParent = this.MdiParent;
                    frm.Show();
                }
                else if (ansGridView5.CurrentCell.OwningColumn.Name == "Delete")
                {
                    if (validate(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["productionItem_id"].Value.ToString()) == true)
                    {
                        DialogResult res = MessageBox.Show("Are you sure?", "Confirm", MessageBoxButtons.OKCancel);
                        if (res == DialogResult.OK)
                        {
                            DataTable dtDelete = new DataTable();
                            dtDelete.TableName = "ProductFormula";
                            Database.GetSqlData("select * from ProductFormula where productionItem_id='" + ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["productionItem_id"].Value.ToString() + "' ", dtDelete);
                            for (int i = 0; i < dtDelete.Rows.Count; i++)
                            {
                                dtDelete.Rows[i].Delete();
                            }
                            Database.SaveData(dtDelete);

                        }
                    }
                    LoadData(gstr, "Product Formula");
                }



            }

            else if (gstr == "PackCategory")
            {
                if (ansGridView5.CurrentCell.OwningColumn.Name == "Edit")
                {
                    if (funs.Select_packcat_id(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["name"].Value.ToString()).ToString() == "0")
                    {
                        return;
                    }


                    frm_packcat frm = new frm_packcat();
                    frm.LoadData(funs.Select_packcat_id(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["name"].Value.ToString()).ToString(), "Edit PackCategory");
                    frm.MdiParent = this.MdiParent;
                    frm.Show();
                }
                else if (ansGridView5.CurrentCell.OwningColumn.Name == "Delete")
                {
                    if (validate(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["name"].Value.ToString()) == true)
                    {
                        DialogResult res = MessageBox.Show("Are you sure?", "Confirm", MessageBoxButtons.OKCancel);
                        if (res == DialogResult.OK)
                        {
                            DataTable dtDelete = new DataTable();
                            dtDelete.TableName = "packcategory";
                            Database.GetSqlData("select * from packcategory where name='" + ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["name"].Value.ToString() + "' ", dtDelete);
                            for (int i = 0; i < dtDelete.Rows.Count; i++)
                            {
                                dtDelete.Rows[i].Delete();
                            }
                            Database.SaveData(dtDelete);

                        }
                    }
                    LoadData(gstr, "PackCategory");
                }



            }









            else if (gstr == "Customer/Supplier Rate")
            {
                if (ansGridView5.CurrentCell.OwningColumn.Name == "Edit")
                {
                    if (funs.Select_ac_id(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Name"].Value.ToString()).ToString() == "0")
                    {
                        return;
                    }
                    frmCustSuppRate frm = new frmCustSuppRate();
                    frm.MdiParent = this.MdiParent;
                    frm.LoadData(funs.Select_ac_id(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Name"].Value.ToString()).ToString(),"Edit");
                    frm.Show();
                    //frmBroker frm = new frmBroker();
                    //frm.LoadData(funs.Select_ac_id(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Name"].Value.ToString()).ToString(), "Edit Customer/Supplier Rate");
                    //frm.MdiParent = this.MdiParent;
                    //frm.Show();

                }
                else if (ansGridView5.CurrentCell.OwningColumn.Name == "Delete")
                {
                //    if (validate(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Name"].Value.ToString()) == true)
                //    {
                        DialogResult res = MessageBox.Show("Are you sure?", "Confirm", MessageBoxButtons.OKCancel);
                        if (res == DialogResult.OK)
                        {
                            DataTable dtDelete = new DataTable();
                            dtDelete.TableName = "PARTYRATE";
                            Database.GetSqlData("select * from PARTYRATE where Ac_id='" + funs.Select_ac_id(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Name"].Value.ToString())+"' ", dtDelete);
                            for (int i = 0; i < dtDelete.Rows.Count; i++)
                            {
                                dtDelete.Rows[i].Delete();
                            }
                            Database.SaveData(dtDelete);
                            //Master.UpdateAgent();
                        }
                    //}
                        LoadData(gstr, "Customer/Supplier Rate");
                }
            }

            else if (gstr == "Broker")
            {
                if (ansGridView5.CurrentCell.OwningColumn.Name == "Edit")
                {
                    if (funs.Select_con_id(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Name"].Value.ToString()).ToString() == "0")
                    {
                        return;
                    }
                    frmBroker frm = new frmBroker();
                    frm.LoadData(funs.Select_con_id(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Name"].Value.ToString()).ToString(), "Edit Broker");
                    frm.MdiParent = this.MdiParent;
                    frm.Show();

                }
                else if (ansGridView5.CurrentCell.OwningColumn.Name == "Delete")
                {
                    if (validate(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Name"].Value.ToString()) == true)
                    {
                        DialogResult res = MessageBox.Show("Are you sure?", "Confirm", MessageBoxButtons.OKCancel);
                        if (res == DialogResult.OK)
                        {
                            DataTable dtDelete = new DataTable();
                            dtDelete.TableName = "CONTRACTOR";
                            Database.GetSqlData("select * from CONTRACTOR where Name='" + ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Name"].Value.ToString() + "' ", dtDelete);
                            for (int i = 0; i < dtDelete.Rows.Count; i++)
                            {
                                dtDelete.Rows[i].Delete();
                            }
                            Database.SaveData(dtDelete);
                            //Master.UpdateAgent();

                        }
                    }
                    LoadData(gstr, "Broker");
                }
            }


            else if (gstr == "Payment Collector")
            {
                if (ansGridView5.CurrentCell.OwningColumn.Name == "Edit")
                {
                    if (funs.Select_oth_id(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Name"].Value.ToString()).ToString() == "0")
                    {
                        return;
                    }
                    frm_NewGroup frm = new frm_NewGroup();
                    frm.LoadData(funs.Select_oth_id(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Name"].Value.ToString()).ToString(), "Edit Payment Collector");
                    frm.MdiParent = this.MdiParent;
                    frm.Show();


                }
                else if (ansGridView5.CurrentCell.OwningColumn.Name == "Delete")
                {
                    if (validate(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Name"].Value.ToString()) == true)
                    {
                        DialogResult res = MessageBox.Show("Are you sure?", "Confirm", MessageBoxButtons.OKCancel);
                        if (res == DialogResult.OK)
                        {
                            DataTable dtDelete = new DataTable();
                            dtDelete.TableName = "Other";
                            Database.GetSqlData("select * from Other where Name='" + ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Name"].Value.ToString() + "' ", dtDelete);
                            for (int i = 0; i < dtDelete.Rows.Count; i++)
                            {
                                dtDelete.Rows[i].Delete();
                            }
                            Database.SaveData(dtDelete);

                        }
                    }
                    LoadData(gstr, "Payment Collector");
                }



            }

            else if (gstr == "Account Group")
            {
                if (ansGridView5.CurrentCell.OwningColumn.Name == "Edit")
                {
                    if (funs.Select_AccType_id(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Name"].Value.ToString()).ToString() == "0")
                    {
                        return;
                    }
                    frmnewgroup frm = new frmnewgroup();
                    frm.LoadData(funs.Select_AccType_id(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Name"].Value.ToString()).ToString(), "Edit Account Group");
                    frm.MdiParent = this.MdiParent;
                    frm.Show();
                   

                }
                else if (ansGridView5.CurrentCell.OwningColumn.Name == "Delete")
                {
                    if (validate(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Name"].Value.ToString()) == true)
                    {
                        DialogResult res = MessageBox.Show("Are you sure?", "Confirm", MessageBoxButtons.OKCancel);
                        if (res == DialogResult.OK)
                        {
                            DataTable dtDelete = new DataTable();
                            dtDelete.TableName = "Accountype";
                            Database.GetSqlData("select * from Accountype where Name='" + ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Name"].Value.ToString() + "' ", dtDelete);
                            for (int i = 0; i < dtDelete.Rows.Count; i++)
                            {
                                dtDelete.Rows[i].Delete();
                            }
                            Database.SaveData(dtDelete);
                            
                        }
                    }
                    LoadData(gstr, "Account Group");
                }



            }

            else if (gstr == "Company")
            {
                if (ansGridView5.CurrentCell.OwningColumn.Name == "Edit")
                {
                    if (funs.Select_oth_id(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Name"].Value.ToString()).ToString() == "0")
                    {
                        return;
                    }


                    frmItem frm = new frmItem();
                    frm.LoadData(funs.Select_oth_id(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Name"].Value.ToString()).ToString(), "Edit Company");
                    frm.MdiParent = this.MdiParent;
                    frm.Show();
                   


                }
                else if (ansGridView5.CurrentCell.OwningColumn.Name == "Delete")
                {
                    if (validate(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Name"].Value.ToString()) == true)
                    {
                        DialogResult res = MessageBox.Show("Are you sure?", "Confirm", MessageBoxButtons.OKCancel);
                        if (res == DialogResult.OK)
                        {
                            DataTable dtDelete = new DataTable();
                            dtDelete.TableName = "Other";
                            Database.GetSqlData("select * from Other where Name='" + ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Name"].Value.ToString() + "' ", dtDelete);
                            for (int i = 0; i < dtDelete.Rows.Count; i++)
                            {
                                dtDelete.Rows[i].Delete();
                            }
                            Database.SaveData(dtDelete);
                            Master.UpdateOther();

                        }
                    }
                    LoadData(gstr, "Company");
                }
            }

            else if (gstr == "Department")
            {
                if (ansGridView5.CurrentCell.OwningColumn.Name == "Edit")
                {
                    if (funs.Select_oth_id(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Name"].Value.ToString()).ToString() == "0")
                    {
                        return;
                    }


                    frmItem frm = new frmItem();
                    frm.LoadData(funs.Select_oth_id(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Name"].Value.ToString()).ToString(), "Edit Department");
                    frm.MdiParent = this.MdiParent;
                    frm.Show();



                }
                else if (ansGridView5.CurrentCell.OwningColumn.Name == "Delete")
                {
                    if (validate(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Name"].Value.ToString()) == true)
                    {
                        DialogResult res = MessageBox.Show("Are you sure?", "Confirm", MessageBoxButtons.OKCancel);
                        if (res == DialogResult.OK)
                        {
                            DataTable dtDelete = new DataTable();
                            dtDelete.TableName = "Other";
                            Database.GetSqlData("select * from Other where Name='" + ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Name"].Value.ToString() + "' ", dtDelete);
                            for (int i = 0; i < dtDelete.Rows.Count; i++)
                            {
                                dtDelete.Rows[i].Delete();
                            }
                            Database.SaveData(dtDelete);
                            Master.UpdateOther();

                        }
                    }
                    LoadData(gstr, "Department");
                }
            }


            else if (gstr == "PriceGroup")
            {
                if (ansGridView5.CurrentCell.OwningColumn.Name == "Edit")
                {
                    if (funs.Select_oth_id(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Name"].Value.ToString()).ToString() == "0")
                    {
                        return;
                    }


                    frmItem frm = new frmItem();
                    frm.LoadData(funs.Select_oth_id(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Name"].Value.ToString()).ToString(), "Edit PriceGroup");
                    frm.MdiParent = this.MdiParent;
                    frm.Show();



                }
                else if (ansGridView5.CurrentCell.OwningColumn.Name == "Delete")
                {
                    if (validate(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Name"].Value.ToString()) == true)
                    {
                        DialogResult res = MessageBox.Show("Are you sure?", "Confirm", MessageBoxButtons.OKCancel);
                        if (res == DialogResult.OK)
                        {
                            DataTable dtDelete = new DataTable();
                            dtDelete.TableName = "Other";
                            Database.GetSqlData("select * from Other where Name='" + ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Name"].Value.ToString() + "' ", dtDelete);
                            for (int i = 0; i < dtDelete.Rows.Count; i++)
                            {
                                dtDelete.Rows[i].Delete();
                            }
                            Database.SaveData(dtDelete);
                            Master.UpdateOther();

                        }
                    }
                    LoadData(gstr, "Price Group");
                }
            }

            else if (gstr == "State")
            {
                if (ansGridView5.CurrentCell.OwningColumn.Name == "Edit")
                {
                    if (funs.Select_state_id(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["StateName"].Value.ToString()).ToString() == "0")
                    {
                        return;
                    }


                    frm_state frm = new frm_state();
                    frm.LoadData(funs.Select_state_id(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["StateName"].Value.ToString()).ToString(), "Edit State");
                    frm.MdiParent = this.MdiParent;
                    frm.Show();
                 


                }
                else if (ansGridView5.CurrentCell.OwningColumn.Name == "Delete")
                {
                    if (validate(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["StateName"].Value.ToString()) == true)
                    {
                        DialogResult res = MessageBox.Show("Are you sure?", "Confirm", MessageBoxButtons.OKCancel);
                        if (res == DialogResult.OK)
                        {
                            DataTable dtDelete = new DataTable();
                            dtDelete.TableName = "State";
                            Database.GetSqlData("select * from State where Sname='" + ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["StateName"].Value.ToString() + "' ", dtDelete);
                            for (int i = 0; i < dtDelete.Rows.Count; i++)
                            {
                                dtDelete.Rows[i].Delete();
                            }
                            Database.SaveData(dtDelete);
                            Master.UpdateState();
                        }
                    }
                    LoadData(gstr, "State");
                }
            }



            else if (gstr == "Item")
            {
                if (ansGridView5.CurrentCell.OwningColumn.Name == "Edit")
                {
                    if (funs.Select_oth_id(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Name"].Value.ToString()).ToString() == "0")
                    {
                        return;
                    }


                    frmItem frm = new frmItem();
                    frm.LoadData(funs.Select_oth_id(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Name"].Value.ToString()).ToString(), "Edit Item");
                    frm.MdiParent = this.MdiParent;
                    frm.Show();
                  

                }
                else if (ansGridView5.CurrentCell.OwningColumn.Name == "Delete")
                {
                    if (validate(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Name"].Value.ToString()) == true)
                    {
                        DialogResult res = MessageBox.Show("Are you sure?", "Confirm", MessageBoxButtons.OKCancel);
                        if (res == DialogResult.OK)
                        {
                            DataTable dtDelete = new DataTable();
                            dtDelete.TableName = "Other";
                            Database.GetSqlData("select * from Other where Name='" + ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Name"].Value.ToString() + "' ", dtDelete);
                            for (int i = 0; i < dtDelete.Rows.Count; i++)
                            {
                                dtDelete.Rows[i].Delete();
                            }
                            Database.SaveData(dtDelete);
                            Master.UpdateOther();

                        }
                    }
                    LoadData(gstr, "Item");
                }
            }
            else if (gstr == "Colour")
            {
                if (ansGridView5.CurrentCell.OwningColumn.Name == "Edit")
                {
                    if (funs.Select_oth_id(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Name"].Value.ToString()).ToString() == "0")
                    {
                        return;
                    }


                    frmItem frm = new frmItem();
                    frm.LoadData(funs.Select_oth_id(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Name"].Value.ToString()).ToString(), "Edit Colour");
                    frm.MdiParent = this.MdiParent;
                    frm.Show();
                }
                else if (ansGridView5.CurrentCell.OwningColumn.Name == "Delete")
                {
                    if (validate(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Name"].Value.ToString()) == true)
                    {
                        DialogResult res = MessageBox.Show("Are you sure?", "Confirm", MessageBoxButtons.OKCancel);
                        if (res == DialogResult.OK)
                        {
                            DataTable dtDelete = new DataTable();
                            dtDelete.TableName = "Other";
                            Database.GetSqlData("select * from Other where Name='" + ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Name"].Value.ToString() + "' ", dtDelete);
                            for (int i = 0; i < dtDelete.Rows.Count; i++)
                            {
                                dtDelete.Rows[i].Delete();
                            }
                            Database.SaveData(dtDelete);
                            Master.UpdateOther();

                        }
                    }
                    LoadData(gstr, "Colour");
                }
            }

            else if (gstr == "Salesman")
            {
                if (ansGridView5.CurrentCell.OwningColumn.Name == "Edit")
                {
                    if (funs.Select_salesman_id(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Name"].Value.ToString()).ToString() == "0")
                    {
                        return;
                    }


                    frm_salesman frm = new frm_salesman();
                    frm.LoadData(funs.Select_salesman_id(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Name"].Value.ToString()).ToString(), "Edit Charges");
                    frm.MdiParent = this.MdiParent;
                    frm.Show();

                }
                else if (ansGridView5.CurrentCell.OwningColumn.Name == "Delete")
                {
                    if (validate(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Name"].Value.ToString()) == true)
                    {
                        DialogResult res = MessageBox.Show("Are you sure?", "Confirm", MessageBoxButtons.OKCancel);
                        if (res == DialogResult.OK)
                        {
                            DataTable dtDelete = new DataTable();
                            dtDelete.TableName = "Salesman";
                            Database.GetSqlData("select * from Salesman where Name='" + ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Name"].Value.ToString() + "' ", dtDelete);
                            for (int i = 0; i < dtDelete.Rows.Count; i++)
                            {
                                dtDelete.Rows[i].Delete();
                            }
                            Database.SaveData(dtDelete);
                            
                        }
                    }
                    LoadData(gstr, "Salesman");
                }
            }



            else if (gstr == "Charges")
            {
                if (ansGridView5.CurrentCell.OwningColumn.Name == "Edit")
                {
                    if (funs.Select_ch_id(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Name"].Value.ToString()).ToString() == "0")
                    {
                        return;
                    }


                    frmCharges frm = new frmCharges();
                    frm.LoadData(funs.Select_ch_id(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Name"].Value.ToString()).ToString(), "Edit Charges");
                    frm.MdiParent = this.MdiParent;
                    frm.Show();
                  
                }
                else if (ansGridView5.CurrentCell.OwningColumn.Name == "Delete")
                {
                    if (validate(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Name"].Value.ToString()) == true)
                    {
                        DialogResult res = MessageBox.Show("Are you sure?", "Confirm", MessageBoxButtons.OKCancel);
                        if (res == DialogResult.OK)
                        {
                            DataTable dtDelete = new DataTable();
                            dtDelete.TableName = "Charges";
                            Database.GetSqlData("select * from Charges where Name='" + ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Name"].Value.ToString() + "' ", dtDelete);
                            for (int i = 0; i < dtDelete.Rows.Count; i++)
                            {
                                dtDelete.Rows[i].Delete();
                            }
                            Database.SaveData(dtDelete);
                            Master.UpdateCharge();
                        }
                    }
                    LoadData(gstr, "Charges");
                }
            }
            //else if (gstr == "Packing")
            //{
            //    if (ansGridView5.CurrentCell.OwningColumn.Name == "Edit")
            //    {
            //        if (ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Name"].Value.ToString() == "0")
            //        {
            //            return;
            //        }
            //        frmPacking frm = new frmPacking();
            //        frm.LoadData(funs.Select_pack_id(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Name"].Value.ToString()).ToString(), "Edit Packing");
            //        frm.MdiParent = this.MdiParent;
            //        frm.Show();
                  
            //    }
            //    else if (ansGridView5.CurrentCell.OwningColumn.Name == "Delete")
            //    {
            //        if (validate(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Name"].Value.ToString()) == true)
            //        {
            //            DialogResult res = MessageBox.Show("Are you sure?", "Confirm", MessageBoxButtons.OKCancel);
            //            if (res == DialogResult.OK)
            //            {
            //                DataTable dtDelete = new DataTable();
            //                dtDelete.TableName = "Packing";
            //                Database.GetSqlData("select * from Packing where Name='" + ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Name"].Value.ToString() + "' ", dtDelete);
            //                for (int i = 0; i < dtDelete.Rows.Count; i++)
            //                {
            //                    dtDelete.Rows[i].Delete();
            //                }
            //                Database.SaveData(dtDelete);

            //            }
            //        }
            //        LoadData(gstr, "Packing");
            //    }
            //}


            else if (gstr == "Control Room")
            {
                if (ansGridView5.CurrentCell.OwningColumn.Name == "Edit")
                {
                    if (funs.Select_controlroom_id(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Features"].Value.ToString()).ToString() == "0")
                    {
                        return;
                    }
                    ControlRoom frm = new ControlRoom();
                    frm.Loaddata(funs.Select_controlroom_id(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Features"].Value.ToString()).ToString(), "Edit Control Room");
                    frm.MdiParent = this.MdiParent;
                    frm.Show();
                }
            }

            else if (gstr == "TransactionSetup")
            {
                if (ansGridView5.CurrentCell.OwningColumn.Name == "Edit")
                {
                    if (funs.Select_vt_id_vnm(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Name"].Value.ToString()).ToString() == "0")
                    {
                        return;
                    }
                    Frmvouchertype frm = new Frmvouchertype();
                    frm.LoadData(funs.Select_vt_id_vnm(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Name"].Value.ToString()).ToString(), "Edit Transaction Setup");
                    frm.MdiParent = this.MdiParent;
                    frm.Show();
                }
            }

            else if (gstr == "Tax")
            {
                if (ansGridView5.CurrentCell.OwningColumn.Name == "Edit")
                {
                    if (funs.Select_tax_cat_id(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Name"].Value.ToString()).ToString() == "0")
                    {
                        return;
                    }
                    frm_tax frm = new frm_tax();
                    frm.cmdmode = "edit";
                    frm.LoadData(funs.Select_tax_cat_id(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Name"].Value.ToString()).ToString(), "Edit Tax");
                    frm.MdiParent = this.MdiParent;
                    frm.Show();
                }
                else if (ansGridView5.CurrentCell.OwningColumn.Name == "Delete")
                {
                    if (validate(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Name"].Value.ToString()) == true)
                    {
                        DialogResult res = MessageBox.Show("Are you sure?", "Confirm", MessageBoxButtons.OKCancel);
                        if (res == DialogResult.OK)
                        {
                            DataTable dtDelete = new DataTable();
                            dtDelete.TableName = "TaxCategory";
                            Database.GetSqlData("select * from TaxCategory where Category_id='" + funs.Select_category_id(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Name"].Value.ToString()) + "' ", dtDelete);
                            dtDelete.Rows[0].Delete();
                            Database.SaveData(dtDelete);
                            Master.UpdateTaxCategory();                           
                        }
                    }
                    LoadData(gstr, "Tax");
                }
            }

            else if (gstr == "ReminderDates")
            {
                if (ansGridView5.CurrentCell.OwningColumn.Name == "Edit")
                {
                    if (funs.Select_impdates_id(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["title"].Value.ToString()).ToString() == "0")
                    {
                        return;
                    }
                    frm_impdates frm = new frm_impdates();
                    frm.Loaddata(funs.Select_impdates_id(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["title"].Value.ToString()).ToString(), "Edit ReminderDates");
                    frm.MdiParent = this.MdiParent;
                    frm.Show();

                }
                else if (ansGridView5.CurrentCell.OwningColumn.Name == "Delete")
                {
                    if (validate(ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["title"].Value.ToString()) == true)
                    {
                        DialogResult res = MessageBox.Show("Are you sure?", "Confirm", MessageBoxButtons.OKCancel);
                        if (res == DialogResult.OK)
                        {
                            DataTable dtDelete = new DataTable();
                            dtDelete.TableName = "importantdate";
                            Database.GetSqlData("select * from importantdate where title='" + ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["title"].Value.ToString() + "' ", dtDelete);
                            for (int i = 0; i < dtDelete.Rows.Count; i++)
                            {
                                dtDelete.Rows[i].Delete();
                            }
                            Database.SaveData(dtDelete);
                        }
                    }
                    LoadData(gstr, "ReminderDates");
                }
            }    
        }

        private bool validate(string name)
        {
            if (gstr == "StockItem")
            {
                dt = new DataTable("Description");
                Database.GetSqlData("Select Des_id,Pack from Description where Description='" + name + "'", dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (Database.GetScalarInt("SELECT count(*) FROM Voucherdet WHERE Des_ac_id='" + dt.Rows[i]["Des_id"].ToString() + "' ") != 0)
                    {
                        MessageBox.Show("Description is in Use");
                        return false;
                    }
                    if (Database.GetScalarInt("SELECT count(*) FROM Stock WHERE Did='" + dt.Rows[i]["Des_id"].ToString() + "' ") != 0)
                    {
                        MessageBox.Show("Description is in Use in Stock");
                        return false;
                    }
                    if (Database.GetScalarInt("SELECT count(*) FROM PARTYRATE WHERE Des_id='" + dt.Rows[i]["Des_id"].ToString() + "' ") != 0)
                    {
                        MessageBox.Show("Description is in Use in Customer Supplier Rate");
                        return false;
                    }
                    if (Database.GetScalarInt("SELECT count(*) FROM Productformula  WHERE productionItem_id='" + dt.Rows[i]["Des_id"].ToString() + "' ") != 0)
                    {
                        MessageBox.Show("Description is in Use in Product Formula");
                        return false;
                    }
                    if (Database.GetScalarInt("SELECT count(*) FROM Productformula  WHERE ConsumItem_id='" + dt.Rows[i]["Des_id"].ToString() + "' ") != 0)
                    {
                        MessageBox.Show("Description is in Use in Product Formula");
                        return false;
                    }
                }
            }

            else if (gstr == "Role")
            {
                if (Database.GetScalarInt("SELECT count(*) FROM Userinfo WHERE Role_id=" + funs.Select_Role_id(name)) != 0)
                {
                    MessageBox.Show("Selected Role is in Use with users");
                    return false;
                }
            }

            else if (gstr == "User")
            {
                if (Database.GetScalarInt("SELECT count(*) FROM Voucherinfo WHERE user_id='" + funs.Select_user_id(name) + "' ") != 0)
                {
                    MessageBox.Show("Selected User is in Use in Transaction");
                    return false;
                }
                if (Database.GetScalarInt("SELECT count(*) FROM Voucherinfo WHERE Modifiedby='" + funs.Select_user_id(name) + "' ") != 0)
                {
                    MessageBox.Show("Selected User is in Use in Transaction");
                    return false;
                }
                if (Database.GetScalarInt("SELECT count(*) FROM Voucherinfo WHERE ApprovedBy='" + funs.Select_user_id(name) + "' ") != 0)
                {
                    MessageBox.Show("Selected User is in Use in Transaction");
                    return false;
                }
                if (Database.GetScalarInt("SELECT count(*) FROM Voucherinfo WHERE Cashier_id='" + funs.Select_user_id(name) + "' ") != 0)
                {
                     MessageBox.Show("Selected User is in Use in Transaction");
                    return false;
                }
                if (Database.GetScalarInt("SELECT count(*) FROM Description WHERE user_id='" + funs.Select_user_id(name) + "' ") != 0)
                {
                    MessageBox.Show("Selected User is in Use in Description");
                    return false;
                }
                if (Database.GetScalarInt("SELECT count(*) FROM Description WHERE Modifiedby='" + funs.Select_user_id(name) + "' ") != 0)
                {
                    MessageBox.Show("Selected User is in Use in Description");
                    return false;
                }
                if (Database.GetScalarInt("SELECT count(*) FROM Account WHERE user_id='" + funs.Select_user_id(name) + "' ") != 0)
                {
                    MessageBox.Show("Selected User is in Use in Account");
                    return false;
                }
                if (Database.GetScalarInt("SELECT count(*) FROM Account WHERE Modifiedby='" + funs.Select_user_id(name) + "' ") != 0)
                {
                    MessageBox.Show("Selected User is in Use in Account");
                    return false;
                }
                if (Database.GetScalarInt("SELECT count(*) FROM Branch WHERE user_id='" + funs.Select_user_id(name) + "' ") != 0)
                {
                    MessageBox.Show("Selected User is in Use in Branch");
                    return false;
                }
                if (Database.GetScalarInt("SELECT count(*) FROM Branch WHERE Modifiedby='" + funs.Select_user_id(name) + "' ") != 0)
                {
                    MessageBox.Show("Selected User is in Use in Branch");
                    return false;
                }
                if (Database.GetScalarInt("SELECT count(*) FROM Charges WHERE user_id='" + funs.Select_user_id(name) + "' ") != 0)
                {
                    MessageBox.Show("Selected User is in Use in Charges");
                    return false;
                }
                if (Database.GetScalarInt("SELECT count(*) FROM Charges WHERE Modifiedby='" + funs.Select_user_id(name) + "' ") != 0)
                {
                    MessageBox.Show("Selected User is in Use in Charges");
                    return false;
                }
                if (Database.GetScalarInt("SELECT count(*) FROM Container WHERE user_id='" + funs.Select_user_id(name) + "' ") != 0)
                {
                    MessageBox.Show("Selected User is in Use in Container");
                    return false;
                }
                if (Database.GetScalarInt("SELECT count(*) FROM Container WHERE Modifiedby='" + funs.Select_user_id(name) + "' ") != 0)
                {
                    MessageBox.Show("Selected User is in Use in Container");
                    return false;
                }

                if (Database.GetScalarInt("SELECT count(*) FROM Other WHERE user_id='" + funs.Select_user_id(name) + "' ") != 0)
                {
                    MessageBox.Show("Selected User is in Use");
                    return false;
                }
                if (Database.GetScalarInt("SELECT count(*) FROM Other WHERE Modifiedby='" + funs.Select_user_id(name) + "' ") != 0)
                {
                    MessageBox.Show("Selected User is in Use");
                    return false;
                }
                if (Database.GetScalarInt("SELECT count(*) FROM State WHERE user_id='" + funs.Select_user_id(name) + "' ") != 0)
                {
                    MessageBox.Show("Selected User is in Use in State");
                    return false;
                }
                if (Database.GetScalarInt("SELECT count(*) FROM State WHERE Modifiedby='" + funs.Select_user_id(name) + "' ") != 0)
                {
                    MessageBox.Show("Selected User is in Use in State");
                    return false;
                }
                if (Database.GetScalarInt("SELECT count(*) FROM Taxcategory WHERE user_id='" + funs.Select_user_id(name) + "' ") != 0)
                {
                    MessageBox.Show("Selected User is in Use in Tax Category");
                    return false;
                }
                if (Database.GetScalarInt("SELECT count(*) FROM Taxcategory WHERE Modifiedby='" + funs.Select_user_id(name) + "' ") != 0)
                {
                    MessageBox.Show("Selected User is in Use in Tax Category");
                    return false;
                }
            }
            else if (gstr == "Container")
            {
                if (Database.GetScalarInt("SELECT count(*) FROM Description WHERE Container='" + funs.Select_Container_id(name) + "' ") != 0)
                {
                    MessageBox.Show("Selected Container is in Use ");
                    return false;
                }
            }

            else if (gstr == "City")
            {
                if (Database.GetScalarInt("SELECT count(*) FROM Account WHERE City_id='" + funs.Select_city_id(name)+"'") != 0)
                {
                    MessageBox.Show("Selected City  is in Use in Account");
                    return false;
                }
                if (Database.GetScalarInt("SELECT count(*) FROM Voucherinfo WHERE ShiptoCity_id='" + funs.Select_city_id(name) + "'") != 0)
                {
                    MessageBox.Show("Selected City  is in Use in Transaction");
                    return false;
                }
                if (Database.GetScalarInt("SELECT count(*) FROM Company WHERE City_id='" + funs.Select_city_id(name) + "'") != 0)
                {
                    MessageBox.Show("Selected City  is in Use in Company");
                    return false;
                }
            }
            else if (gstr == "PackCat")
            {
                if (Database.GetScalarInt("SELECT count(*) FROM Description WHERE packcat_id='" + funs.Select_Container_id(name) + "' ") != 0)
                {
                    MessageBox.Show("Selected PackCat is in Use ");
                    return false;
                }
            }
            else if (gstr == "Salesman")
            {
                if (Database.GetScalarInt("SELECT count(*) FROM Account WHERE Salesman_id='" + funs.Select_salesman_id(name) + "' ") != 0)
                {
                    MessageBox.Show("Selected Salesman is in Use ");
                    return false;
                }
            }
            else if (gstr == "Account")
            {
                if (Database.GetScalarInt("SELECT count(*) FROM journal WHERE Ac_id='" + funs.Select_ac_id(name) + "' ") != 0)
                {
                    MessageBox.Show("Selected Account is in Use in Transaction");
                    return false;
                }
                else if (Database.GetScalarInt("SELECT count(*) FROM Voucherpaydet WHERE Acc_id='" + funs.Select_ac_id(name) + "' ") != 0)
                {
                    MessageBox.Show("Selected Account is in Use in Transaction");
                    return false;
                }

                else if (Database.GetScalarInt("SELECT count(*) FROM Account WHERE Con_id='" + funs.Select_ac_id(name) + "' ") != 0)
                {
                    MessageBox.Show("Selected Agent is in Use ");
                    return false;
                }

                else if (Database.GetScalarInt("SELECT count(*) FROM Voucherinfo WHERE Conn_id='" + funs.Select_ac_id(name) + "' ") != 0)
                {
                    MessageBox.Show("Selected Agent is in Use in Transaction");
                    return false;
                }

                else if (Database.GetScalarInt("SELECT count(*) FROM Stock WHERE godown_id='" + funs.Select_ac_id(name) + "' ") != 0)
                {
                    MessageBox.Show("Selected Godown is in Use in Transaction");
                    return false;
                }
                else if (Database.GetScalarInt("SELECT count(*) FROM Rebate WHERE Acid='" + funs.Select_ac_id(name) + "' ") != 0)
                {
                    MessageBox.Show("Selected Account is in Use in Rebate");
                    return false;
                }
                else if (Database.GetScalarInt("SELECT count(*) FROM charges WHERE Ac_id='" + funs.Select_ac_id(name) + "' ") != 0)
                {
                    MessageBox.Show("Selected Account is in Use in Charges");
                    return false;
                }

                else if (Database.GetScalarInt("SELECT count(*) FROM PARTYRATE WHERE Ac_id='" + funs.Select_ac_id(name) + "' ") != 0)
                {
                    MessageBox.Show("Selected Account is in Use in Customer Supplier Rate");
                    return false;
                }
                else if (Database.GetScalarInt("SELECT count(*) FROM DisAfterTax WHERE Ac_id='" + funs.Select_ac_id(name) + "' ") != 0)
                {
                    MessageBox.Show("Selected Account is in Use in Discount After Tax");
                    return false;
                }
                if (Feature.Available("Taxation Applicable") == "VAT")
                {
                    if (Database.GetScalarInt("SELECT count(*) FROM TAXCATEGORYDETAIL WHERE Sale_Pur_Acc_id='" + funs.Select_ac_id(name) + "' ") != 0)
                    {
                        MessageBox.Show("Selected Account is in Use in TaxCategory");
                        return false;
                    }
                    else if (Database.GetScalarInt("SELECT count(*) FROM TAXCATEGORYDETAIL WHERE Tax_Acc_id='" + funs.Select_ac_id(name) + "' ") != 0)
                    {
                        MessageBox.Show("Selected Account is in Use in TaxCategory");
                        return false;
                    }
                }
                else
                {
                     if (Database.GetScalarInt("SELECT count(*) FROM Account WHERE  dispatch_id='" + funs.Select_ac_id(name)  + "' ") != 0)
                    {
                        MessageBox.Show("Selected Account is in Use in Transaction");
                        return false;
                    }
                   else if (Database.GetScalarInt("SELECT count(*) FROM TAXCATEGORY WHERE PAEX='" + funs.Select_ac_id(name) + "' ") != 0)
                    {
                        MessageBox.Show("Selected Account is in Use in TaxCategory");
                        return false;
                    }
                    else if (Database.GetScalarInt("SELECT count(*) FROM TAXCATEGORY WHERE SAEX='" + funs.Select_ac_id(name) + "' ") != 0)
                    {
                        MessageBox.Show("Selected Account is in Use in TaxCategory");
                        return false;
                    }
                    else if (Database.GetScalarInt("SELECT count(*) FROM TAXCATEGORY WHERE PA='" + funs.Select_ac_id(name) + "' ") != 0)
                    {
                        MessageBox.Show("Selected Account is in Use in TaxCategory");
                        return false;
                    }
                    else if (Database.GetScalarInt("SELECT count(*) FROM TAXCATEGORY WHERE SA='" + funs.Select_ac_id(name) + "' ") != 0)
                    {
                        MessageBox.Show("Selected Account is in Use in TaxCategory");
                        return false;
                    }
                    else if (Database.GetScalarInt("SELECT count(*) FROM TAXCATEGORY WHERE PCAEX='" + funs.Select_ac_id(name) + "' ") != 0)
                    {
                        MessageBox.Show("Selected Account is in Use in TaxCategory");
                        return false;
                    }
                    else if (Database.GetScalarInt("SELECT count(*) FROM TAXCATEGORY WHERE SCAEX='" + funs.Select_ac_id(name) + "' ") != 0)
                    {
                        MessageBox.Show("Selected Account is in Use in TaxCategory");
                        return false;
                    }
                    else if (Database.GetScalarInt("SELECT count(*) FROM TAXCATEGORY WHERE PCA='" + funs.Select_ac_id(name) + "' ") != 0)
                    {
                        MessageBox.Show("Selected Account is in Use in TaxCategory");
                        return false;
                    }
                    else if (Database.GetScalarInt("SELECT count(*) FROM TAXCATEGORY WHERE SCA='" + funs.Select_ac_id(name) + "' ") != 0)
                    {
                        MessageBox.Show("Selected Account is in Use in TaxCategory");
                        return false;
                    }
                    else if (Database.GetScalarInt("SELECT count(*) FROM TAXCATEGORY WHERE PTA1='" + funs.Select_ac_id(name) + "' ") != 0)
                    {
                        MessageBox.Show("Selected Account is in Use in TaxCategory");
                        return false;
                    }
                    else if (Database.GetScalarInt("SELECT count(*) FROM TAXCATEGORY WHERE PTA2='" + funs.Select_ac_id(name) + "' ") != 0)
                    {
                        MessageBox.Show("Selected Account is in Use in TaxCategory");
                        return false;
                    }
                    else if (Database.GetScalarInt("SELECT count(*) FROM TAXCATEGORY WHERE PTA3='" + funs.Select_ac_id(name) + "' ") != 0)
                    {
                        MessageBox.Show("Selected Account is in Use in TaxCategory");
                        return false;
                    }
                    else if (Database.GetScalarInt("SELECT count(*) FROM TAXCATEGORY WHERE STA1='" + funs.Select_ac_id(name) + "' ") != 0)
                    {
                        MessageBox.Show("Selected Account is in Use in TaxCategory");
                        return false;
                    }
                    else if (Database.GetScalarInt("SELECT count(*) FROM TAXCATEGORY WHERE STA2='" + funs.Select_ac_id(name) + "' ") != 0)
                    {
                        MessageBox.Show("Selected Account is in Use in TaxCategory");
                        return false;
                    }
                    else if (Database.GetScalarInt("SELECT count(*) FROM TAXCATEGORY WHERE STA3='" + funs.Select_ac_id(name) + "' ") != 0)
                    {
                        MessageBox.Show("Selected Account is in Use in TaxCategory");
                        return false;
                    }
                    else if (Database.GetScalarInt("SELECT count(*) FROM TAXCATEGORY WHERE RCMPay='" + funs.Select_ac_id(name) + "' ") != 0)
                    {
                        MessageBox.Show("Selected Account is in Use in TaxCategory");
                        return false;
                    }
                    else if (Database.GetScalarInt("SELECT count(*) FROM TAXCATEGORY WHERE RCMITC='" + funs.Select_ac_id(name) + "' ") != 0)
                    {
                        MessageBox.Show("Selected Account is in Use in TaxCategory");
                        return false;
                    }
                    else if (Database.GetScalarInt("SELECT count(*) FROM TAXCATEGORY WHERE RCMEli='" + funs.Select_ac_id(name) + "' ") != 0)
                    {
                        MessageBox.Show("Selected Account is in Use in TaxCategory");
                        return false;
                    }
                    else if (Database.GetScalarInt("SELECT count(*) FROM Rebate WHERE Acid='" + funs.Select_ac_id(name) + "' ") != 0)
                    {
                        MessageBox.Show("Selected Account is in Use in Rebate");
                        return false;
                    }
                    else if (Database.GetScalarInt("SELECT count(*) FROM Voucherdet WHERE RCMac_id='" + funs.Select_ac_id(name) + "' ") != 0)
                    {
                        MessageBox.Show("Selected Account is in Use in RCM");
                        return false;
                    }
                }
            }
            else if (gstr == "Broker")
            {
                if (Database.GetScalarInt("SELECT count(*) FROM ACCOUNT WHERE Con_id='" + funs.Select_con_id(name) + "' ") != 0)
                {
                    MessageBox.Show("Selected Broker Name is in Use in Account");
                    return false;
                }
            }
            else if (gstr == "State")
            {
                if (Database.GetScalarInt("SELECT count(*) FROM ACCOUNT WHERE State_id='" + funs.Select_state_id(name) + "' ") != 0)
                {
                    MessageBox.Show("Selected State Name is in Use in Account");
                    return false;
                }
                else if (Database.GetScalarInt("SELECT count(*) FROM COMPANY WHERE CState_id='" + funs.Select_state_id(name) + "' ") != 0)
                {
                    MessageBox.Show("Selected State Name Use In Company");
                    return false;
                }
                else if (Database.GetScalarInt("SELECT count(*) FROM Voucherinfo WHERE ShiptoStateid='" + funs.Select_state_id(name) + "' ") != 0)
                {
                    MessageBox.Show("Selected State Name Use In Transaction");
                    return false;
                }
            }
            else if (gstr == "Payment Collector")
            {
                if (Database.GetScalarInt("SELECT count(*) FROM ACCOUNT WHERE loc_id='" + funs.Select_oth_id(name) + "' ") != 0)
                {
                    MessageBox.Show("Selected Account Group is in Use in Account");
                    return false;
                }
            }
            else if (gstr == "Account Group")
            {
                if (Database.GetScalarInt("SELECT count(*) FROM ACCOUNT WHERE Act_id='" + funs.Select_act_id(name) + "' ") != 0)
                {
                    MessageBox.Show("Selected Account Group is in Use in Account");
                    return false;
                }
            }
            else if (gstr == "Company")
            {
                if (Database.GetScalarInt("SELECT count(*) FROM DESCRIPTION WHERE Company_id='" + funs.Select_oth_id(name) + "' ") != 0)
                {
                    MessageBox.Show("Selected Company Name is in Use ");
                    return false;
                }
                else if (Database.GetScalarInt("SELECT count(*) FROM Rebate WHERE Companyid='" + funs.Select_oth_id(name) + "' ") != 0)
                {
                    MessageBox.Show("Selected Company is in Use in Rebate");
                    return false;
                }
            }
            else if (gstr == "PriceGroup")
            {
                if (Database.GetScalarInt("SELECT count(*) FROM DESCRIPTION WHERE Group_id='" + funs.Select_oth_id(name) + "' ") != 0)
                {
                    MessageBox.Show("Selected PriceGroup  is in Use ");
                    return false;
                }
            }
            else if (gstr == "Department")
            {
                if (Database.GetScalarInt("SELECT count(*) FROM DESCRIPTION WHERE Department_id='" + funs.Select_oth_id(name) + "' ") != 0)
                {
                    MessageBox.Show("Selected Department is in Use ");
                    return false;
                }
            }
            else if (gstr == "Item")
            {
                if (Database.GetScalarInt("SELECT count(*) FROM DESCRIPTION WHERE Item_id='" + funs.Select_oth_id(name) + "' ") != 0)
                {
                    MessageBox.Show("Selected Item Name is in Use ");
                    return false;
                }
                else if (Database.GetScalarInt("SELECT count(*) FROM Rebate WHERE Itemid='" + funs.Select_oth_id(name) + "' ") != 0)
                {
                    MessageBox.Show("Selected Itenm is in Use in Rebate");
                    return false;
                }
            }
            else if (gstr == "Colour")
            {
                if (Database.GetScalarInt("SELECT count(*) FROM DESCRIPTION WHERE Col_id='" + funs.Select_oth_id(name) + "' ") != 0)
                {
                    MessageBox.Show("Selected Colour is in Use ");
                    return false;
                }
            }
            else if (gstr == "TaxCategory")
            {
                if (Database.GetScalarInt("SELECT count(*) FROM DESCRIPTION WHERE Tax_Cat_id='" + funs.Select_tax_cat_id(name) + "' ") != 0)
                {
                    MessageBox.Show("Selected TaxCategory is in Use");
                    return false;
                }
                if (Database.GetScalarInt("SELECT count(*) FROM Voucherdet WHERE Category_id='" + funs.Select_tax_cat_id(name) + "'") != 0)
                {
                    MessageBox.Show("Selected TaxCategory is in Use");
                    return false;
                }
            }
            else if (gstr == "Tax")
            {
                if (Database.GetScalarInt("SELECT count(*) FROM DESCRIPTION WHERE Tax_Cat_id='" + funs.Select_tax_cat_id(name) + "' ") != 0)
                {
                    MessageBox.Show("Selected Tax is in Use");
                    return false;
                }
                if (Database.GetScalarInt("SELECT count(*) FROM Voucherdet WHERE Category_id='" + funs.Select_tax_cat_id(name) + "'") != 0)
                {
                    MessageBox.Show("Selected TaxCategory is in Use");
                    return false;
                }
            }
            else if (gstr == "Charges")
            {
                if (Database.GetScalarInt("SELECT count(*) FROM VOUCHARGES WHERE Charg_id='" + funs.Select_ch_id(name) + "'") != 0)
                {
                    MessageBox.Show("Selected Charges Name is in Use ");
                    return false;
                }
                else if (Database.GetScalarInt("SELECT count(*) FROM ITEMCHARGES WHERE Charg_id='" + funs.Select_ch_id(name) + "'") != 0)
                {
                    MessageBox.Show("Selected Charges Name is in Use");
                    return false;
                }
            }
            return true;
        }





        private bool extravalidate(string id)
        {
            if (gstr == "StockItem")
            {
              
                    if (Database.GetScalarInt("SELECT count(*) FROM Voucherdet WHERE Des_ac_id='" + id + "' ") != 0)
                    {
                       // MessageBox.Show("Description is in Use");
                        return false;
                    }
                    if (Database.GetScalarInt("SELECT count(*) FROM Stock WHERE Did='" + id + "' ") != 0)
                    {
                       // MessageBox.Show("Description is in Use in Stock");
                        return false;
                    }
                    if (Database.GetScalarInt("SELECT count(*) FROM PARTYRATE WHERE Des_id='" + id + "' ") != 0)
                    {
                       // MessageBox.Show("Description is in Use in Customer Supplier Rate");
                        return false;
                    }
                    if (Database.GetScalarInt("SELECT count(*) FROM Productformula  WHERE productionItem_id='" + id + "' ") != 0)
                    {
                       // MessageBox.Show("Description is in Use in Product Formula");
                        return false;
                    }
                    if (Database.GetScalarInt("SELECT count(*) FROM Productformula  WHERE ConsumItem_id='" + id + "' ") != 0)
                    {
                      //  MessageBox.Show("Description is in Use in Product Formula");
                        return false;
                    }


                  
            }

            //else if (gstr == "Role")
            //{
            //    if (Database.GetScalarInt("SELECT count(*) FROM Userinfo WHERE Role_id=" + funs.Select_Role_id(name)) != 0)
            //    {
            //        MessageBox.Show("Selected Role is in Use with users");
            //        return false;
            //    }
            //}

            else if (gstr == "User")
            {



                if (Database.GetScalarInt("SELECT count(*) FROM Voucherinfo WHERE user_id='" +id + "' ") != 0)
                {
                  //  MessageBox.Show("Selected User is in Use in Transaction");
                    return false;
                }
                if (Database.GetScalarInt("SELECT count(*) FROM Voucherinfo WHERE Modifiedby='" + id + "' ") != 0)
                {
                  //  MessageBox.Show("Selected User is in Use in Transaction");
                    return false;
                }
                if (Database.GetScalarInt("SELECT count(*) FROM Voucherinfo WHERE ApprovedBy='" + id + "' ") != 0)
                {
                    // MessageBox.Show("Selected User is in Use in Transaction");
                    return false;
                }
                if (Database.GetScalarInt("SELECT count(*) FROM Voucherinfo WHERE Cashier_id='" + id + "' ") != 0)
                {
                   // MessageBox.Show("Selected User is in Use in Transaction");
                    return false;
                }
                if (Database.GetScalarInt("SELECT count(*) FROM Description WHERE user_id='" + id + "' ") != 0)
                {
                  //  MessageBox.Show("Selected User is in Use in Description");
                    return false;
                }
                if (Database.GetScalarInt("SELECT count(*) FROM Description WHERE Modifiedby='" + id + "' ") != 0)
                {
                    //MessageBox.Show("Selected User is in Use in Description");
                    return false;
                }
                if (Database.GetScalarInt("SELECT count(*) FROM Account WHERE user_id='" + id + "' ") != 0)
                {
                   // MessageBox.Show("Selected User is in Use in Account");
                    return false;
                }
                if (Database.GetScalarInt("SELECT count(*) FROM Account WHERE Modifiedby='" + id + "' ") != 0)
                {
                   // MessageBox.Show("Selected User is in Use in Account");
                    return false;
                }
                if (Database.GetScalarInt("SELECT count(*) FROM Branch WHERE user_id='" + id + "' ") != 0)
                {
                   // MessageBox.Show("Selected User is in Use in Branch");
                    return false;
                }
                if (Database.GetScalarInt("SELECT count(*) FROM Branch WHERE Modifiedby='" + id + "' ") != 0)
                {
                   // MessageBox.Show("Selected User is in Use in Branch");
                    return false;
                }
                if (Database.GetScalarInt("SELECT count(*) FROM Charges WHERE user_id='" + id + "' ") != 0)
                {
                   // MessageBox.Show("Selected User is in Use in Charges");
                    return false;
                }
                if (Database.GetScalarInt("SELECT count(*) FROM Charges WHERE Modifiedby='" + id + "' ") != 0)
                {
                   // MessageBox.Show("Selected User is in Use in Charges");
                    return false;
                }
                if (Database.GetScalarInt("SELECT count(*) FROM Container WHERE user_id='" + id + "' ") != 0)
                {
                   // MessageBox.Show("Selected User is in Use in Container");
                    return false;
                }
                if (Database.GetScalarInt("SELECT count(*) FROM Container WHERE Modifiedby='" + id + "' ") != 0)
                {
                  //  MessageBox.Show("Selected User is in Use in Container");
                    return false;
                }

                if (Database.GetScalarInt("SELECT count(*) FROM Other WHERE user_id='" + id + "' ") != 0)
                {
                   // MessageBox.Show("Selected User is in Use");
                    return false;
                }
                if (Database.GetScalarInt("SELECT count(*) FROM Other WHERE Modifiedby='" + id + "' ") != 0)
                {
                   // MessageBox.Show("Selected User is in Use");
                    return false;
                }
                if (Database.GetScalarInt("SELECT count(*) FROM State WHERE user_id='" + id + "' ") != 0)
                {
                  //  MessageBox.Show("Selected User is in Use in State");
                    return false;
                }
                if (Database.GetScalarInt("SELECT count(*) FROM State WHERE Modifiedby='" + id + "' ") != 0)
                {
                   // MessageBox.Show("Selected User is in Use in State");
                    return false;
                }
                if (Database.GetScalarInt("SELECT count(*) FROM Taxcategory WHERE user_id='" + id + "' ") != 0)
                {
                  //  MessageBox.Show("Selected User is in Use in Tax Category");
                    return false;
                }
                if (Database.GetScalarInt("SELECT count(*) FROM Taxcategory WHERE Modifiedby='" + id + "' ") != 0)
                {
                   // MessageBox.Show("Selected User is in Use in Tax Category");
                    return false;
                }
            }
            else if (gstr == "Container")
            {
                if (Database.GetScalarInt("SELECT count(*) FROM Description WHERE Container='" +id + "' ") != 0)
                {
                    MessageBox.Show("Selected Container is in Use ");
                    return false;
                }
            }

            else if (gstr == "City")
            {
                if (Database.GetScalarInt("SELECT count(*) FROM Account WHERE City_id='" + id + "'") != 0)
                {
                    //MessageBox.Show("Selected City  is in Use in Account");
                    return false;
                }
                if (Database.GetScalarInt("SELECT count(*) FROM Voucherinfo WHERE ShiptoCity_id='" + id + "'") != 0)
                {
                   // MessageBox.Show("Selected City  is in Use in Transaction");
                    return false;
                }
                if (Database.GetScalarInt("SELECT count(*) FROM Company WHERE City_id='" + id + "'") != 0)
                {
                    //MessageBox.Show("Selected City  is in Use in Company");
                    return false;
                }
                return true;
            }
            else if (gstr == "PackCat")
            {
                if (Database.GetScalarInt("SELECT count(*) FROM Description WHERE packcat_id='" + id + "' ") != 0)
                {
                  //  MessageBox.Show("Selected PackCat is in Use ");
                    return false;
                }
            }
            else if (gstr == "Salesman")
            {
                if (Database.GetScalarInt("SELECT count(*) FROM Account WHERE Salesman_id='" + id + "' ") != 0)
                {
                   // MessageBox.Show("Selected Salesman is in Use ");
                    return false;
                }
                if (Database.GetScalarInt("SELECT count(*) FROM Voucherinfo WHERE S_id='" + id + "' ") != 0)
                {
                    // MessageBox.Show("Selected Salesman is in Use ");
                    return false;
                }
            }
            else if (gstr == "Account")
            {
                if (Database.GetScalarInt("SELECT count(*) FROM journal WHERE Ac_id='" + id + "' ") != 0)
                {
                  //  MessageBox.Show("Selected Account is in Use in Transaction");
                    return false;
                }
               else if (Database.GetScalarDecimal("SELECT balance FROM Account WHERE Ac_id='" + id + "' ") != 0)
                {
                    //  MessageBox.Show("Selected Account is in Use in Transaction");
                    return false;
                }
                else if (Database.GetScalarDecimal("SELECT balance2 FROM Account WHERE Ac_id='" + id + "' ") != 0)
                {
                    //  MessageBox.Show("Selected Account is in Use in Transaction");
                    return false;
                }
               
                else if (Database.GetScalarInt("SELECT count(*) FROM Voucherpaydet WHERE Acc_id='" + id + "' ") != 0)
                {
                 //   MessageBox.Show("Selected Account is in Use in Transaction");
                    return false;
                }
                else if (Database.GetScalarInt("SELECT count(*) FROM Voucherinfo WHERE  dispatch_id='" + id + "' ") != 0)
                {
                    // MessageBox.Show("Selected Agent is in Use ");
                    return false;
                }
               
                else if (Database.GetScalarInt("SELECT count(*) FROM Account WHERE Con_id='" + id + "' ") != 0)
                {
                   // MessageBox.Show("Selected Agent is in Use ");
                    return false;
                }
                else if (Database.GetScalarInt("SELECT count(*) FROM Account WHERE Transporter_id='" + id + "' ") != 0)
                {
                    // MessageBox.Show("Selected Agent is in Use ");
                    return false;
                }

                else if (Database.GetScalarInt("SELECT count(*) FROM Voucherinfo WHERE Conn_id='" + id + "' ") != 0)
                {
                    //MessageBox.Show("Selected Agent is in Use in Transaction");
                    return false;
                }

                else if (Database.GetScalarInt("SELECT count(*) FROM Stock WHERE godown_id='" +id + "' ") != 0)
                {
                   // MessageBox.Show("Selected Godown is in Use in Transaction");
                    return false;
                }
                else if (Database.GetScalarInt("SELECT count(*) FROM Rebate WHERE Acid='" +id + "' ") != 0)
                {
                   // MessageBox.Show("Selected Account is in Use in Rebate");
                    return false;
                }
                else if (Database.GetScalarInt("SELECT count(*) FROM charges WHERE Ac_id='" +id + "' ") != 0)
                {
                    //MessageBox.Show("Selected Account is in Use in Charges");
                    return false;
                }

                else if (Database.GetScalarInt("SELECT count(*) FROM PARTYRATE WHERE Ac_id='" + id + "' ") != 0)
                {
                   // MessageBox.Show("Selected Account is in Use in Customer Supplier Rate");
                    return false;
                }
                else if (Database.GetScalarInt("SELECT count(*) FROM DisAfterTax WHERE Ac_id='" + id + "' ") != 0)
                {
                   // MessageBox.Show("Selected Account is in Use in Discount After Tax");
                    return false;
                }
                if (Feature.Available("Taxation Applicable") == "VAT")
                {
                    if (Database.GetScalarInt("SELECT count(*) FROM TAXCATEGORYDETAIL WHERE Sale_Pur_Acc_id='" + id + "' ") != 0)
                    {
                        //MessageBox.Show("Selected Account is in Use in TaxCategory");
                        return false;
                    }
                    else if (Database.GetScalarInt("SELECT count(*) FROM TAXCATEGORYDETAIL WHERE Tax_Acc_id='" + id + "' ") != 0)
                    {
                       // MessageBox.Show("Selected Account is in Use in TaxCategory");
                        return false;
                    }
                }
                else
                {
                    if (Database.GetScalarInt("SELECT count(*) FROM TAXCATEGORY WHERE PAEX='" + id + "' ") != 0)
                    {
                      //  MessageBox.Show("Selected Account is in Use in TaxCategory");
                        return false;
                    }
                    else if (Database.GetScalarInt("SELECT count(*) FROM TAXCATEGORY WHERE SAEX='" + id + "' ") != 0)
                    {
                       // MessageBox.Show("Selected Account is in Use in TaxCategory");
                        return false;
                    }
                    else if (Database.GetScalarInt("SELECT count(*) FROM TAXCATEGORY WHERE PA='" + id + "' ") != 0)
                    {
                       // MessageBox.Show("Selected Account is in Use in TaxCategory");
                        return false;
                    }
                    else if (Database.GetScalarInt("SELECT count(*) FROM TAXCATEGORY WHERE SA='" + id + "' ") != 0)
                    {
                     //   MessageBox.Show("Selected Account is in Use in TaxCategory");
                        return false;
                    }
                    else if (Database.GetScalarInt("SELECT count(*) FROM TAXCATEGORY WHERE PCAEX='" +id + "' ") != 0)
                    {
                       // MessageBox.Show("Selected Account is in Use in TaxCategory");
                        return false;
                    }
                    else if (Database.GetScalarInt("SELECT count(*) FROM TAXCATEGORY WHERE SCAEX='" + id + "' ") != 0)
                    {
                        //MessageBox.Show("Selected Account is in Use in TaxCategory");
                        return false;
                    }
                    else if (Database.GetScalarInt("SELECT count(*) FROM TAXCATEGORY WHERE PCA='" + id + "' ") != 0)
                    {
                       // MessageBox.Show("Selected Account is in Use in TaxCategory");
                        return false;
                    }
                    else if (Database.GetScalarInt("SELECT count(*) FROM TAXCATEGORY WHERE SCA='" + id + "' ") != 0)
                    {
                      //  MessageBox.Show("Selected Account is in Use in TaxCategory");
                        return false;
                    }
                    else if (Database.GetScalarInt("SELECT count(*) FROM TAXCATEGORY WHERE PTA1='" + id + "' ") != 0)
                    {
                     //   MessageBox.Show("Selected Account is in Use in TaxCategory");
                        return false;
                    }
                    else if (Database.GetScalarInt("SELECT count(*) FROM TAXCATEGORY WHERE PTA2='" + id + "' ") != 0)
                    {
                       // MessageBox.Show("Selected Account is in Use in TaxCategory");
                        return false;
                    }
                    else if (Database.GetScalarInt("SELECT count(*) FROM TAXCATEGORY WHERE PTA3='" + id + "' ") != 0)
                    {
                        //MessageBox.Show("Selected Account is in Use in TaxCategory");
                        return false;
                    }
                    else if (Database.GetScalarInt("SELECT count(*) FROM TAXCATEGORY WHERE STA1='" + id + "' ") != 0)
                    {
                       // MessageBox.Show("Selected Account is in Use in TaxCategory");
                        return false;
                    }
                    else if (Database.GetScalarInt("SELECT count(*) FROM TAXCATEGORY WHERE STA2='" + id + "' ") != 0)
                    {
                        //MessageBox.Show("Selected Account is in Use in TaxCategory");
                        return false;
                    }
                    else if (Database.GetScalarInt("SELECT count(*) FROM TAXCATEGORY WHERE STA3='" + id + "' ") != 0)
                    {
                      //  MessageBox.Show("Selected Account is in Use in TaxCategory");
                        return false;
                    }
                    else if (Database.GetScalarInt("SELECT count(*) FROM TAXCATEGORY WHERE RCMPay='" + id + "' ") != 0)
                    {
                      //  MessageBox.Show("Selected Account is in Use in TaxCategory");
                        return false;
                    }
                    else if (Database.GetScalarInt("SELECT count(*) FROM TAXCATEGORY WHERE RCMITC='" + id + "' ") != 0)
                    {
                        //MessageBox.Show("Selected Account is in Use in TaxCategory");
                        return false;
                    }
                    else if (Database.GetScalarInt("SELECT count(*) FROM TAXCATEGORY WHERE RCMEli='" + id + "' ") != 0)
                    {
                       // MessageBox.Show("Selected Account is in Use in TaxCategory");
                        return false;
                    }
                    else if (Database.GetScalarInt("SELECT count(*) FROM Rebate WHERE Acid='" + id + "' ") != 0)
                    {
                       // MessageBox.Show("Selected Account is in Use in Rebate");
                        return false;
                    }
                    else if (Database.GetScalarInt("SELECT count(*) FROM Voucherdet WHERE RCMac_id='" + id + "' ") != 0)
                    {
                       // MessageBox.Show("Selected Account is in Use in RCM");
                        return false;
                    }
                }
            }
            //else if (gstr == "Broker")
            //{
            //    if (Database.GetScalarInt("SELECT count(*) FROM ACCOUNT WHERE Con_id='" + funs.Select_con_id(name) + "' ") != 0)
            //    {
            //        MessageBox.Show("Selected Broker Name is in Use in Account");
            //        return false;
            //    }
            //}
            else if (gstr == "State")
            {
                if (Database.GetScalarInt("SELECT count(*) FROM ACCOUNT WHERE State_id='" + id + "' ") != 0)
                {
                   // MessageBox.Show("Selected State Name is in Use in Account");
                    return false;
                }
                else if (Database.GetScalarInt("SELECT count(*) FROM COMPANY WHERE CState_id='" + id + "' ") != 0)
                {
                   // MessageBox.Show("Selected State Name Use In Company");
                    return false;
                }
                if (Database.GetScalarInt("SELECT count(*) FROM Voucherinfo WHERE ShiptoStateid='" + id + "'") != 0)
                {
                    // MessageBox.Show("Selected City  is in Use in Transaction");
                    return false;
                }
            }
            else if (gstr == "Payment Collector")
            {
                if (Database.GetScalarInt("SELECT count(*) FROM ACCOUNT WHERE loc_id='" + id + "' ") != 0)
                {
                   // MessageBox.Show("Selected Account Group is in Use in Account");
                    return false;
                }
            }
            else if (gstr == "Account Group")
            {
                if (Database.GetScalarInt("SELECT count(*) FROM ACCOUNT WHERE Act_id='" + id + "' ") != 0)
                {
                   // MessageBox.Show("Selected Account Group is in Use in Account");
                    return false;
                }
            }
            else if (gstr == "Company")
            {
                if (Database.GetScalarInt("SELECT count(*) FROM DESCRIPTION WHERE Company_id='" + id + "' ") != 0)
                {
                   // MessageBox.Show("Selected Company Name is in Use ");
                    return false;
                }
                else if (Database.GetScalarInt("SELECT count(*) FROM Rebate WHERE Companyid='" + id + "' ") != 0)
                {
                   // MessageBox.Show("Selected Company is in Use in Rebate");
                    return false;
                }
            }
            else if (gstr == "PriceGroup")
            {
                if (Database.GetScalarInt("SELECT count(*) FROM DESCRIPTION WHERE Group_id='" + id + "' ") != 0)
                {
                   // MessageBox.Show("Selected PriceGroup  is in Use ");
                    return false;
                }
            }
            //else if (gstr == "Department")
            //{
            //    if (Database.GetScalarInt("SELECT count(*) FROM DESCRIPTION WHERE Department_id='" + funs.Select_oth_id(name) + "' ") != 0)
            //    {
            //        MessageBox.Show("Selected Department is in Use ");
            //        return false;
            //    }
            //}
            else if (gstr == "Item")
            {
                if (Database.GetScalarInt("SELECT count(*) FROM DESCRIPTION WHERE Item_id='" + id + "' ") != 0)
                {
                   // MessageBox.Show("Selected Item Name is in Use ");
                    return false;
                }
                else if (Database.GetScalarInt("SELECT count(*) FROM Rebate WHERE Itemid='" + id + "' ") != 0)
                {
                   // MessageBox.Show("Selected Itenm is in Use in Rebate");
                    return false;
                }
            }
            else if (gstr == "Colour")
            {
                if (Database.GetScalarInt("SELECT count(*) FROM DESCRIPTION WHERE Col_id='" + id + "' ") != 0)
                {
                   // MessageBox.Show("Selected Colour is in Use ");
                    return false;
                }
            }
            //else if (gstr == "TaxCategory")
            //{
            //    if (Database.GetScalarInt("SELECT count(*) FROM DESCRIPTION WHERE Tax_Cat_id='" + funs.Select_tax_cat_id(name) + "' ") != 0)
            //    {
            //        MessageBox.Show("Selected TaxCategory is in Use");
            //        return false;
            //    }
            //    if (Database.GetScalarInt("SELECT count(*) FROM Voucherdet WHERE Category_id='" + funs.Select_tax_cat_id(name) + "'") != 0)
            //    {
            //        MessageBox.Show("Selected TaxCategory is in Use");
            //        return false;
            //    }
            //}
            else if (gstr == "Tax")
            {
                if (Database.GetScalarInt("SELECT count(*) FROM DESCRIPTION WHERE Tax_Cat_id='" + id + "' ") != 0)
                {
                   // MessageBox.Show("Selected Tax is in Use");
                    return false;
                }
                if (Database.GetScalarInt("SELECT count(*) FROM Voucherdet WHERE Category_id='" +id + "'") != 0)
                {
                  //  MessageBox.Show("Selected TaxCategory is in Use");
                    return false;
                }
            }
            else if (gstr == "Charges")
            {
                if (Database.GetScalarInt("SELECT count(*) FROM VOUCHARGES WHERE Charg_id='" + id + "'") != 0)
                {
                    //MessageBox.Show("Selected Charges Name is in Use ");
                    return false;
                }
                else if (Database.GetScalarInt("SELECT count(*) FROM ITEMCHARGES WHERE Charg_id='" + id + "'") != 0)
                {
                    //MessageBox.Show("Selected Charges Name is in Use");
                    return false;
                }
            }
            return true;
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            filter();
        }

        private void filter()
        {
            String strTemp = textBox1.Text;
            strTemp = strTemp.Replace("%", "?");
            strTemp = strTemp.Replace("[", string.Empty);
            strTemp = strTemp.Replace("]", string.Empty);
            string strfilter = "";
            int a = 0;
            a = dtitem.Columns.Count;
            if (gstr == "Tax")
            {
                for (int i = 0; i < dtitem.Columns.Count - 1; i++)
                {
                    if (strfilter != "")
                    {
                        strfilter += " or ";
                    }
                    strfilter += "(" + dtitem.Columns[i].ColumnName + " like '*" + strTemp + "*' " + ")";
                }
            }
            else if (gstr == "Copy Rate")
            {
                for (int i = 0; i < dtitem.Columns.Count - 1; i++)
                {
                    if (strfilter != "")
                    {
                        strfilter += " or ";
                    }
                    strfilter += "(" + dtitem.Columns[i].ColumnName + " like '*" + strTemp + "*' " + ")";
                }
            }

            else
            {
                for (int i = 0; i < dtitem.Columns.Count; i++)
                {
                    if (strfilter != "")
                    {
                        strfilter += " or ";
                    }
                    strfilter += "(" + dtitem.Columns[i].ColumnName + " like '*" + strTemp + "*' " + ")";
                }
            }
            bs.Filter = null;
            bs.DataSource = dtitem;
            bs.Filter = strfilter;
        }

        private void frmMaster_Load(object sender, EventArgs e)
        {
            textBox1.Focus();
            this.Size = this.MdiParent.Size;
            SideFill();
        }

        private void SideFill()
        {
            flowLayoutPanel1.Controls.Clear();
            DataTable dtsidefill = new DataTable();
            dtsidefill.Columns.Add("Name", typeof(string));
            dtsidefill.Columns.Add("DisplayName", typeof(string));
            dtsidefill.Columns.Add("ShortcutKey", typeof(string));
            dtsidefill.Columns.Add("Visible", typeof(bool));
            
            //createnew
            dtsidefill.Rows.Add();
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Name"] = "add";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["DisplayName"] = "Create New";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["ShortcutKey"] = "^C";
            if (gstr == "Control Room")
            {
                dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = false;
            }
            else if (gstr == "DAT")
            {
                if (dtitem.Rows.Count == 1)
                {
                    dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = false;
                }
                else
                {
                    dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = true;
                }
            }
            else
            {
                dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = true;
            }

            //refresh
            dtsidefill.Rows.Add();
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Name"] = "refresh";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["DisplayName"] = "Refresh";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["ShortcutKey"] = "^R";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = true;
            if (gstr == "StockItem" || gstr == "Tax" || gstr == "Account")
            {

                //refresh
                dtsidefill.Rows.Add();
                dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Name"] = "merge";
                dtsidefill.Rows[dtsidefill.Rows.Count - 1]["DisplayName"] = "Merge";
                dtsidefill.Rows[dtsidefill.Rows.Count - 1]["ShortcutKey"] = "^M";
                if (Database.utype.ToUpper() == "SUPERADMIN" || Database.utype.ToUpper() == "ADMIN")
                {
                    dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = true;
                }
                else
                {
                    dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = false;
                }
            }
            if (gstr == "Tax")
            {
                //refresh
                dtsidefill.Rows.Add();
                dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Name"] = "update";
                dtsidefill.Rows[dtsidefill.Rows.Count - 1]["DisplayName"] = "Shift";
                dtsidefill.Rows[dtsidefill.Rows.Count - 1]["ShortcutKey"] = "^U";
                if (Database.utype.ToUpper() == "SUPERADMIN" || Database.utype.ToUpper() == "ADMIN")
                {
                    dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = true;
                }
                else
                {
                    dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = false;
                }
            }

            //Export List
            dtsidefill.Rows.Add();
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Name"] = "export";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["DisplayName"] = "Export List";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["ShortcutKey"] = "^E";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = true;


            dtsidefill.Rows.Add();
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Name"] = "execute";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["DisplayName"] = "Execute";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["ShortcutKey"] = "";
            if (gstr == "Copy Rate")
            {
                dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = true;
            }
            else
            {
                dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = false;
            }



            //close
            dtsidefill.Rows.Add();
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Name"] = "quit";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["DisplayName"] = "Quit";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["ShortcutKey"] = "Esc";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = true;
            

            //close
            dtsidefill.Rows.Add();
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Name"] = "extradel";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["DisplayName"] = "ExtraDelete";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["ShortcutKey"] = "";
            if (Database.utype.ToUpper() == "SUPERADMIN" )
            {

                dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = true;
            }
            else
            {

                dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = false;
            }

            if (gstr == "Account")
            {
                //Turnover
                dtsidefill.Rows.Add();
                dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Name"] = "turnover";
                dtsidefill.Rows[dtsidefill.Rows.Count - 1]["DisplayName"] = "TurnOver";
                dtsidefill.Rows[dtsidefill.Rows.Count - 1]["ShortcutKey"] = "";
                dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = false;
                if (ansGridView5.Rows.Count != 0)
                {
                    if (ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Type"].Value.ToString() == "SUNDRY DEBTORS" || ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Type"].Value.ToString() == "SUNDRY CREDITORS")
                    {

                        dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = true;
                    }
                    else
                    {
                        dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = false;
                    }
                }
            }

            for (int i = 0; i < dtsidefill.Rows.Count; i++)
            {
                if (bool.Parse(dtsidefill.Rows[i]["Visible"].ToString()) == true)
                {
                    Button btn = new Button();
                    btn.Size = new Size(150, 30);
                    btn.Name = dtsidefill.Rows[i]["Name"].ToString();
                    btn.Text = "";
                    Bitmap bmp = new Bitmap(btn.ClientRectangle.Width, btn.ClientRectangle.Height);
                    Graphics G = Graphics.FromImage(bmp);
                    G.Clear(btn.BackColor);
                    string line1 = dtsidefill.Rows[i]["ShortcutKey"].ToString();
                    string line2 = dtsidefill.Rows[i]["DisplayName"].ToString();
                    StringFormat SF = new StringFormat();
                    SF.Alignment = StringAlignment.Near;
                    SF.LineAlignment = StringAlignment.Center;
                    System.Drawing.Rectangle RC = btn.ClientRectangle;
                    System.Drawing.Font font = new System.Drawing.Font("Arial", 12);
                    G.DrawString(line1, font, Brushes.Red, RC, SF);
                    G.DrawString("".PadLeft(line1.Length * 2 + 1) + line2, font, Brushes.Black, RC, SF);
                    btn.Image = bmp;
                    btn.Click += new EventHandler(btn_Click);
                    flowLayoutPanel1.Controls.Add(btn);
                }
            }
        }

        private void Add()
        {
            if (gstr == "Container")
            {
                frm_container frm = new frm_container();
                frm.LoadData("0", "Container");
                frm.MdiParent = this.MdiParent;
                frm.Show();
            }
            else if (gstr == "City")
            {
                frm_City frm = new frm_City();
                frm.LoadData("0", "City");
                frm.MdiParent = this.MdiParent;
                frm.Show();
            }
            else if (gstr == "Copy Rate")
            {
                frm_updaterate frm = new frm_updaterate();
                frm.LoadData("0", "Copy Rate");
                frm.MdiParent = this.MdiParent;
                frm.Show();
            }
            else if (gstr == "ProductFormula")
            {
                Frm_ProductFormula frm = new Frm_ProductFormula();
                frm.LoadData("0", "ProductFormula");
                frm.MdiParent = this.MdiParent;
                frm.Show();
            }
                
            else if (gstr == "Salesman")
            {
                frm_salesman frm = new frm_salesman();
                frm.LoadData("0", "Salesman");
                frm.MdiParent = this.MdiParent;
                frm.Show();
            }
            else if (gstr == "StockItem")
            {
                frmDescription frm = new frmDescription();
                frm.LoadData("0", "Description");
                frm.MdiParent = this.MdiParent;
                frm.Show();
            }
            else if (gstr == "User")
            {
                frm_usermgmt frm = new frm_usermgmt();
                frm.LoadData("0", "User");
                frm.MdiParent = this.MdiParent;
                frm.Show();
            }
            else if (gstr == "Role")
            {
                frm_role frm = new frm_role();
                frm.LoadData("0", "Role");
                frm.MdiParent = this.MdiParent;
                frm.Show();
            }
            else if (gstr == "Account")
            {
                frm_NewAcc frm = new frm_NewAcc();
                frm.LoadData("0", "Account");
                
                frm.MdiParent = this.MdiParent;
                frm.Show();
            }
            else if (gstr == "PackCategory")
            {
                frm_packcat frm = new frm_packcat();
                frm.LoadData("0", "PackCategory");
                frm.MdiParent = this.MdiParent;
                frm.Show();
            }
            else if (gstr == "DAT")
            {
                frm_DAT frm = new frm_DAT();
                frm.LoadData("0", "Discount After Tax");
                frm.MdiParent = this.MdiParent;
                frm.Show();
            }
            else if (gstr == "Customer/Supplier Rate")
            {
                frmCustSuppRate frm = new frmCustSuppRate();
                frm.MdiParent = this.MdiParent;
                frm.LoadData("0", "New");
                frm.Show();
            }
            else if (gstr == "State")
            {
                frm_state frm = new frm_state();
                frm.LoadData("0", "State");
                frm.MdiParent = this.MdiParent;
                frm.Show();
            }
            else if (gstr == "Broker")
            {
                frmBroker frm = new frmBroker();
                frm.LoadData("0", "Broker");
                frm.MdiParent = this.MdiParent;
                frm.Show();
            }
            else if (gstr == "Account Group")
            {
                frmnewgroup frm = new frmnewgroup();
                frm.LoadData("0", "Account Group");
                frm.MdiParent = this.MdiParent;
                frm.Show();
            }
            else if (gstr == "Payment Collector")
            {
                frm_NewGroup frm = new frm_NewGroup();
                frm.LoadData("0", "Payment Collector");
                frm.MdiParent = this.MdiParent;
                frm.Show();
            }
            else if (gstr == "Company")
            {
                frmItem frm = new frmItem();
                frm.Type = "Company";
                frm.LoadData("0", "Company");
                frm.MdiParent = this.MdiParent;
                frm.Show();
            }
            else if (gstr == "PriceGroup")
            {
                frmItem frm = new frmItem();
                frm.Type = "Group";
                frm.LoadData("0", "PriceGroup");
                frm.MdiParent = this.MdiParent;
                frm.Show();
            }
            else if (gstr == "Item")
            {
                frmItem frm = new frmItem();
                frm.Type = "Item";
                frm.LoadData("0", "Item");
                frm.MdiParent = this.MdiParent;
                frm.Show();
            }
            else if (gstr == "Colour")
            {
                frmItem frm = new frmItem();
                frm.Type = "Colour";
                frm.LoadData("0", "Colour");
                frm.MdiParent = this.MdiParent;
                frm.Show();
            }
            else if (gstr == "Charges")
            {
                frmCharges frm = new frmCharges();
                frm.LoadData("0", "Charges");
                frm.MdiParent = this.MdiParent;
                frm.Show();
            }
            else if (gstr == "Tax")
            {
                frm_tax frm = new frm_tax();
                frm.LoadData("0", "Tax");
                frm.MdiParent = this.MdiParent;
                frm.Show();
            }
            else if (gstr == "ReminderDates")
            {
                frm_impdates frm = new frm_impdates();
                frm.Loaddata("0", "ReminderDates");
                frm.MdiParent = this.MdiParent;
                frm.Show();
            }
        }

        void btn_Click(object sender, EventArgs e)
        {
            Button tbtn = (Button)sender;
            string name = tbtn.Name.ToString();

            if (name == "add")
            {
                Add();
            }
            else if (name == "refresh")
            {
                LoadData(gstr, gstr);
            }
            else if (name == "quit")
            {
                this.Close();
                this.Dispose();
            }
            else if (name=="export")
            {
                LoadData(gstr, gstr);
                if (gstr == "Account" || gstr=="StockItem")
                {
                    AccountExcelexport();
                }
                else
                {
                    Excelexport();
                }
            }

            else if (name == "execute")
            {
                for (int i = 0; i < ansGridView5.Rows.Count; i++)
                {
                    if (ansGridView5.Rows[i].Cells["select"].Value == null)
                    {
                        ansGridView5.Rows[i].Cells["select"].Value = false;
                    }
                    if (bool.Parse(ansGridView5.Rows[i].Cells["select"].Value.ToString()) == true)
                    {
                        string cr_id = ansGridView5.Rows[i].Cells["cr_id"].Value.ToString();

                        DataTable dtcopyrate = new DataTable();
                        Database.GetSqlData("Select * from Copyrates where CR_id='" + cr_id + "'", dtcopyrate);
                        
                        string str = "";
                        if (dtcopyrate.Rows[0]["Company_id"].ToString() != "")
                        {
                            str = str + " DESCRIPTION.Company_id='" + dtcopyrate.Rows[0]["Company_id"].ToString() + "' ";
                        }



                        if (dtcopyrate.Rows[0]["HSN_id"].ToString() != "")
                        {
                            if (str != "") str += " and ";
                            str = str + "DESCRIPTION.Tax_Cat_id= '" + dtcopyrate.Rows[0]["HSN_id"].ToString() + "' ";
                        }


                        if (dtcopyrate.Rows[0]["Item_id"].ToString() != "")
                        {
                            if (str != "") str += " and ";
                            str = str + "DESCRIPTION.Item_id= '" + dtcopyrate.Rows[0]["Item_id"].ToString() + "' ";
                        }

                        if (dtcopyrate.Rows[0]["Color_id"].ToString() != "")
                        {
                            if (str != "") str += " and ";
                            str = str + "DESCRIPTION.Col_id= '" +dtcopyrate.Rows[0]["Color_id"].ToString() + "' ";
                        }
                        if (dtcopyrate.Rows[0]["Group_id"].ToString() != "")
                        {
                            if (str != "") str += " and ";
                            str = str + "DESCRIPTION.Group_id= '" + dtcopyrate.Rows[0]["Group_id"].ToString() + "' ";
                        }
                        if (dtcopyrate.Rows[0]["Description"].ToString() != "")
                        {
                            if (str != "") str += " and ";
                            str = str + "DESCRIPTION.Description= '" + dtcopyrate.Rows[0]["Description"].ToString() + "' ";
                        }
                        if (dtcopyrate.Rows[0]["Pack"].ToString() != "")
                        {
                            if (str != "") str += " and ";
                            str = str + "DESCRIPTION.Pack= '" + dtcopyrate.Rows[0]["Pack"].ToString() + "' ";
                        }
                        if (dtcopyrate.Rows[0]["Pack_category_id"].ToString() != "")
                        {
                            if (str != "") str += " and ";
                            str = str + "DESCRIPTION.PackCat_id= '" +dtcopyrate.Rows[0]["Pack_category_id"].ToString()+ "' ";
                        }
                        String sql = "Select Des_id,Description,Company_id,Item_id,Col_id,Group_id,Tax_Cat_id,Skucode,Shortcode,Open_stock2,Rate_Unit,PAck,Retail,Wholesale,Purchase_rate,MRP,Rate_X,Rate_Y,Rate_Z,Srebate,Weight,Pvalue from description";
                        str = " where (" + str + ")";
                        DataTable dtdes = new DataTable("Description");



                        Database.GetSqlData(sql + str, dtdes);
                        
                        
                        string ratetoupdate = funs.Select_Rates_Id(dtcopyrate.Rows[0]["Rateto"].ToString());
                        if (dtcopyrate.Rows[0]["Rateto"].ToString() == "MRP") ratetoupdate = "MRP";

                        string rateupdatefrom = funs.Select_Rates_Id(dtcopyrate.Rows[0]["Ratefrom"].ToString());
                        if (dtcopyrate.Rows[0]["Ratefrom"].ToString() == "MRP") rateupdatefrom = "MRP";


                        
                        for (int k = 0; k < dtdes.Rows.Count; k++)
                        {
                            double baseRate = double.Parse(dtdes.Rows[k][rateupdatefrom].ToString());


                            double pv = double.Parse(dtdes.Rows[k]["Pvalue"].ToString());
                            double wt = double.Parse(dtdes.Rows[k]["Weight"].ToString());


                            baseRate = baseRate + double.Parse(dtcopyrate.Rows[0]["Insurance"].ToString());
                            baseRate -= double.Parse(dtcopyrate.Rows[0]["rebate"].ToString()) * pv;
                            baseRate -= baseRate * double.Parse(dtcopyrate.Rows[0]["dis1"].ToString()) / 100;
                            baseRate += baseRate * double.Parse(dtcopyrate.Rows[0]["Tax"].ToString()) / 100;
                            baseRate -= baseRate * double.Parse(dtcopyrate.Rows[0]["dis2"].ToString()) / 100;
                            baseRate -= double.Parse(dtcopyrate.Rows[0]["Rebate2"].ToString()) * pv;

                            if (dtcopyrate.Rows[0]["On"].ToString() == "Weight")
                            {
                                baseRate += double.Parse(dtcopyrate.Rows[0]["freight"].ToString()) * wt;
                            }
                            else
                            {
                                baseRate += double.Parse(dtcopyrate.Rows[0]["freight"].ToString()) * pv;
                            }
                            if (dtcopyrate.Rows[0]["Rateunit"].ToString() == "%")
                            {
                                baseRate += baseRate * double.Parse(dtcopyrate.Rows[0]["Profit"].ToString()) / 100;
                            }
                            else if (dtcopyrate.Rows[0]["Rateunit"].ToString() == "/Lt")
                            {
                                baseRate += double.Parse(dtcopyrate.Rows[0]["Profit"].ToString()) * pv;
                            }
                            else
                            {
                                baseRate += double.Parse(dtcopyrate.Rows[0]["Profit"].ToString());
                            }

                            if (dtcopyrate.Rows[0]["Rounding"].ToString() == "As Actual")
                            {
                                dtdes.Rows[k][ratetoupdate] = baseRate;
                            }
                            else if (dtcopyrate.Rows[0]["Rounding"].ToString() == "Round Down")
                            {
                                dtdes.Rows[k][ratetoupdate] = Math.Floor(baseRate);
                            }
                            else if (dtcopyrate.Rows[0]["Rounding"].ToString() == "Round Up")
                            {
                                dtdes.Rows[k][ratetoupdate] = Math.Ceiling(baseRate);
                            }
                            else if (dtcopyrate.Rows[0]["Rounding"].ToString() == "Roundoff")
                            {
                                dtdes.Rows[k][ratetoupdate] = funs.Roundoff(baseRate.ToString());
                            }


                            else if (dtcopyrate.Rows[0]["Rounding"].ToString() == "Round Up /10 p")
                            {
                                baseRate = baseRate * 10;
                                baseRate = Math.Ceiling(baseRate);
                                baseRate = baseRate / 10;
                                dtdes.Rows[k][ratetoupdate] = baseRate;
                            }
                            else if (dtcopyrate.Rows[0]["Rounding"].ToString() == "Round Up /5p")
                            {
                                dtdes.Rows[k][ratetoupdate] = (Math.Ceiling(baseRate / 0.05d) * 0.05);
                            }

                            else if (dtcopyrate.Rows[0]["Rounding"].ToString() == "Round Up /10 Rs.")
                            {

                                dtdes.Rows[k][ratetoupdate] = (int)(Math.Ceiling(baseRate / 10.0d) * 10);
                            }
                            else if (dtcopyrate.Rows[0]["Rounding"].ToString() == "Round Up /5 Rs.")
                            {

                                dtdes.Rows[k][ratetoupdate] = (int)(Math.Ceiling(baseRate / 5.0d) * 5);

                            }

                            double baseRate1 = double.Parse(dtdes.Rows[k][rateupdatefrom].ToString());
                            baseRate1 =  double.Parse(funs.DecimalPoint(baseRate1,2));
                            if (baseRate1 == 0.0)
                            {
                                dtdes.Rows[k][ratetoupdate] = 0;
                            }

                        }
                        Database.SaveData(dtdes, sql);

                    }
                }
                MessageBox.Show("Done Successfully");
            }

            else if (name == "extradel")
            {


                if (gstr == "StockItem")
                {
                    dt = new DataTable("Description");
                    Database.GetSqlData("Select Des_id As Id from Description", dt);


                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                        if (extravalidate(dt.Rows[i]["Id"].ToString()) == true)
                        {
                            Database.CommandExecutor("Delete from Description where Des_id='"+ dt.Rows[i]["Id"].ToString()+"'");
                        }
                            
                           
                    }

                    MessageBox.Show("Fresh List..");

                }

                if (gstr == "User")
                {

                    dt = new DataTable("Userinfo");
                    Database.GetSqlData("Select U_id As Id from Userinfo", dt);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                        if (extravalidate(dt.Rows[i]["Id"].ToString()) == true)
                        {
                            Database.CommandExecutor("Delete from Userinfo where U_id='" + dt.Rows[i]["Id"].ToString() + "'");
                        }


                    }
                    MessageBox.Show("Fresh List..");
                }

                if (gstr == "Account")
                {
                    dt = new DataTable("Account");
                    Database.GetSqlData("Select Ac_id As Id from Account", dt);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                        if (extravalidate(dt.Rows[i]["Id"].ToString()) == true)
                        {
                            Database.CommandExecutor("Delete from Account where Ac_id='" + dt.Rows[i]["Id"].ToString() + "'");
                        }

                    }
                    MessageBox.Show("Fresh List..");

                }
                if (gstr == "City")
                {
                    dt = new DataTable("City");
                    Database.GetSqlData("Select city_id As Id from City", dt);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                        if (extravalidate(dt.Rows[i]["Id"].ToString()) == true)
                        {
                            Database.CommandExecutor("Delete from City where City_id='" + dt.Rows[i]["Id"].ToString() + "'");
                        }

                        
                    }

                    MessageBox.Show("Fresh List..");
                }
                if (gstr == "State")
                {
                    dt = new DataTable("State");
                    Database.GetSqlData("Select State_id As Id from State", dt);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                        if (extravalidate(dt.Rows[i]["Id"].ToString()) == true)
                        {
                            Database.CommandExecutor("Delete from State where State_id='" + dt.Rows[i]["Id"].ToString() + "'");
                        }


                    }

                    MessageBox.Show("Fresh List..");
                }
                if (gstr == "Charges")
                {
                    dt = new DataTable("Charges");
                    Database.GetSqlData("Select Ch_id As Id from Charges", dt);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                        if (extravalidate(dt.Rows[i]["Id"].ToString()) == true)
                        {
                            Database.CommandExecutor("Delete from Charges where Ch_id='" + dt.Rows[i]["Id"].ToString() + "'");
                        }


                    }

                    MessageBox.Show("Fresh List..");
                }

                if (gstr == "Company" )
                {
                    dt = new DataTable("Other");
                    Database.GetSqlData("Select Oth_id As Id from Other", dt);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                        if (extravalidate(dt.Rows[i]["Id"].ToString()) == true)
                        {
                            Database.CommandExecutor("Delete from Other where OTHER.Type = 'SER14' and Oth_id='" + dt.Rows[i]["Id"].ToString() + "'");
                        }


                    }

                    MessageBox.Show("Fresh List..");
                }
                if (gstr == "Item")
                {
                    dt = new DataTable("Other");
                    Database.GetSqlData("Select Oth_id As Id from Other", dt);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                        if (extravalidate(dt.Rows[i]["Id"].ToString()) == true)
                        {
                            Database.CommandExecutor("Delete from Other where OTHER.Type = 'SER15' and Oth_id='" + dt.Rows[i]["Id"].ToString() + "'");
                        }


                    }

                    MessageBox.Show("Fresh List..");
                }
                if (gstr == "PriceGroup")
                {
                    dt = new DataTable("Other");
                    Database.GetSqlData("Select Oth_id As Id from Other", dt);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                        if (extravalidate(dt.Rows[i]["Id"].ToString()) == true)
                        {
                            Database.CommandExecutor("Delete from Other where OTHER.Type = 'SER16' and Oth_id='" + dt.Rows[i]["Id"].ToString() + "'");
                        }


                    }

                    MessageBox.Show("Fresh List..");
                }
                if (gstr == "Colour")
                {
                    dt = new DataTable("Other");
                    Database.GetSqlData("Select Oth_id As Id from Other", dt);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                        if (extravalidate(dt.Rows[i]["Id"].ToString()) == true)
                        {
                            Database.CommandExecutor("Delete from Other where OTHER.Type = 'SER18' and Oth_id='" + dt.Rows[i]["Id"].ToString() + "'");
                        }


                    }

                    MessageBox.Show("Fresh List..");
                }
                if (gstr == "Salesman")
                {
                    dt = new DataTable("SalesMan");
                    Database.GetSqlData("Select S_id As Id from SalesMan", dt);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                        if (extravalidate(dt.Rows[i]["Id"].ToString()) == true)
                        {
                            Database.CommandExecutor("Delete from SalesMan where S_id='" + dt.Rows[i]["Id"].ToString() + "'");
                        }


                    }

                    MessageBox.Show("Fresh List..");
                }
                
                if (gstr == "Container")
                {
                    dt = new DataTable("Container");
                    Database.GetSqlData("Select id  from Container", dt);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                        if (extravalidate(dt.Rows[i]["Id"].ToString()) == true)
                        {
                            Database.CommandExecutor("Delete from Container where id='" + dt.Rows[i]["Id"].ToString() + "'");
                        }


                    }

                    MessageBox.Show("Fresh List..");
                }
                if (gstr == "PackCat")
                {
                    dt = new DataTable("PackCategory");
                    Database.GetSqlData("Select packCat_id as Id  from PackCategory", dt);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                        if (extravalidate(dt.Rows[i]["Id"].ToString()) == true)
                        {
                            Database.CommandExecutor("Delete from PackCategory where packCat_id='" + dt.Rows[i]["Id"].ToString() + "'");
                        }


                    }

                    MessageBox.Show("Fresh List..");
                }
                if (gstr == "Payment Collector")
                {
                    dt = new DataTable("Other");
                    Database.GetSqlData("Select Oth_id as Id  from Other", dt);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                        if (extravalidate(dt.Rows[i]["Id"].ToString()) == true)
                        {
                            Database.CommandExecutor("Delete from Other where OTHER.Type = 'SER17' and Oth_id='" + dt.Rows[i]["Id"].ToString() + "'");
                        }


                    }

                    MessageBox.Show("Fresh List..");
                }
                if (gstr == "Account Group")
                {
                    dt = new DataTable("Accountype");
                    Database.GetSqlData("Select Act_id as Id  from Accountype", dt);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                        if (extravalidate(dt.Rows[i]["Id"].ToString()) == true)
                        {
                            Database.CommandExecutor("Delete from Accountype where Act_id='" + dt.Rows[i]["Id"].ToString() + "'");
                        }


                    }

                    MessageBox.Show("Fresh List..");
                }

                if (gstr == "Tax")
                {
                    dt = new DataTable("TaxCategory");
                    Database.GetSqlData("Select Category_id as Id  from TaxCategory", dt);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                        if (extravalidate(dt.Rows[i]["Id"].ToString()) == true)
                        {
                            Database.CommandExecutor("Delete from TaxCategory where Category_id='" + dt.Rows[i]["Id"].ToString() + "'");
                        }


                    }

                    MessageBox.Show("Fresh List..");
                }
                LoadData(gstr, gstr);

            }
            else if (name == "merge")
            {

                if (gstr == "StockItem")
                {
                    frm_Merge frm = new frm_Merge();
                    frm.ShowDialog();
                }
                if (gstr == "Tax")
                {
                    frm_updateHSN frm = new frm_updateHSN();
                    frm.mode = "Merge";
                    frm.ShowDialog();
                }
                if (gstr == "Account")
                {
                    frm_MergeAcc frm = new frm_MergeAcc();
                    frm.ShowDialog();
                }
                LoadData(gstr, gstr);

            }
            else if (name == "update")
            {
                frm_updateHSN frm = new frm_updateHSN();
                frm.mode = "UpdateItems";
                frm.ShowDialog();
            }
            else if (name == "turnover")
            {
                Report gg = new Report();
                gg.PartyTurnover(Database.stDate, Database.ldate, ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["AccName"].Value.ToString(), ansGridView5.Rows[ansGridView5.CurrentRow.Index].Cells["Type"].Value.ToString());
                gg.MdiParent = this.MdiParent;
                gg.Show();
            }
        }

        public void ExportToPdf(string tPath)
        {
            string str = "";
            FileStream fs = new FileStream(tPath, FileMode.Create, FileAccess.Write, FileShare.None);
            iTextSharp.text.Rectangle rec;
            Document document;
            int Twidth = 0;
            int basevalue = 2;
            if (gstr == "StockItem")
            {
                if (Feature.Available("Company Colour") == "No")
                {
                    basevalue = 5;
                }
            }
            for (int i = basevalue; i < ansGridView5.Columns.Count; i++)
            {
                Twidth += ansGridView5.Columns[i].Width;
            }
            if (Twidth == 2000)
            {
                document = new Document(PageSize.A4.Rotate(), 20f, 10f, 20f, 10f);
            }
            
            document = new Document(PageSize.A4, 20f, 10f, 20f, 10f);
            
            //  Pagesize = GetPapersize();
            PdfWriter writer = PdfWriter.GetInstance(document, fs);
            writer.PageEvent = new MainTextEventsHandler();
            document.Open();
            HTMLWorker hw = new HTMLWorker(document);
            str = "";
            str += @"<body> <font size='1'><table border=1> <tr>";
            for (int i = basevalue; i < ansGridView5.Columns.Count; i++)
            {
                string align = "";
                string bold = "";
                int width = 0;

                if (Twidth == 2000)
                {
                    width = ansGridView5.Columns[i].Width / 20;
                }
                else
                {
                    width = ansGridView5.Columns[i].Width / 10;
                }

                if (ansGridView5.Columns[i].HeaderCell.Style.Alignment == DataGridViewContentAlignment.MiddleRight)
                {
                    align = "text-align:right;";
                }

                bold = "font-weight: bold;";

                if (width != 0)
                {
                    str += "<th width=" + width + "%  style='" + align + bold + "'>" + ansGridView5.Columns[i].HeaderText.ToString() + "</th> ";
                }
            }

            str += "</tr>";

            for (int i = 0; i < ansGridView5.Rows.Count; i++)
            {
                str += "<tr> ";
                for (int j = basevalue; j < ansGridView5.Columns.Count; j++)
                {
                    int width = 0;
                    if (Twidth == 2000)
                    {
                        width = ansGridView5.Rows[i].Cells[j].Size.Width / 20;
                    }
                    else
                    {
                        width = ansGridView5.Rows[i].Cells[j].Size.Width / 10;
                    }

                    if (width != 0)
                    {
                        if (ansGridView5.Rows[i].Cells[j].Value != null)
                        {
                            string align = "";
                            string bold = "";
                            string colspan = "";

                            if (ansGridView5.Columns[j].DefaultCellStyle.Alignment == DataGridViewContentAlignment.MiddleRight)
                            {
                                align = "text-align:right;";
                            }
                            if (ansGridView5.Rows[i].Cells[j].Style.Font != null && ansGridView5.Rows[i].Cells[j].Style.Font.Bold == true)
                            {
                                bold = "font-weight: bold;";
                            }
                            if (j == 0 && ansGridView5.Rows[i].Cells[0].Value.ToString() != "" && ansGridView5.Rows[i].Cells[1].Value == null && ansGridView5.Rows[i].Cells[2].Value == null)
                            {
                                colspan = "colspan= '2'";
                            }
                            if (ansGridView5.Rows[i].Cells[j].Value.ToString().Trim() == "")
                            {
                                str += "<td> &nbsp; </td>";
                            }
                            else
                            {
                                str += "<td " + colspan + "  style='" + align + bold + "'>" + ansGridView5.Rows[i].Cells[j].Value.ToString() + "</td> ";
                            }
                            if (j == 0 && ansGridView5.Rows[i].Cells[0].Value.ToString() != "" && ansGridView5.Rows[i].Cells[1].Value == null && ansGridView5.Rows[i].Cells[2].Value == null)
                            {
                                j++;
                            }
                        }
                        else
                        {
                            str += "<td> &nbsp; </td>";
                        }
                    }
                }
                str += "</tr> ";
            }
            str += "</table></font></body>";

            StringReader sr = new StringReader(str);
            hw.Parse(sr);
            document.Close();
        }
        internal class MainTextEventsHandler : PdfPageEventHelper
        {
            public override void OnStartPage(PdfWriter writer, Document document)
            {
                base.OnStartPage(writer, document);

                DataTable dtRheader = new DataTable();
                Database.GetSqlData("select * from company", dtRheader);
                PdfPTable table = new PdfPTable(1);
                PdfPCell cell = new PdfPCell();

                cell.Phrase = new Phrase(dtRheader.Rows[0]["name"].ToString());
                cell.BorderWidth = 0f;
                cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table.AddCell(cell);
                cell.Phrase = new Phrase(dtRheader.Rows[0]["Address1"].ToString());
                table.AddCell(cell);
                cell.Phrase = new Phrase(dtRheader.Rows[0]["Address2"].ToString());
                table.AddCell(cell);
                cell.Phrase = new Phrase(Report.DecsOfReport2);
                table.AddCell(cell);
                cell.Phrase = new Phrase("\n");
                table.AddCell(cell);
                document.Add(table);
            }

            public override void OnEndPage(PdfWriter writer, Document document)
            {
                base.OnEndPage(writer, document);
                string text = "";
                text += "Page No-" + document.PageNumber;
                PdfContentByte cb = writer.DirectContent;
                cb.BeginText();
                BaseFont bf = BaseFont.CreateFont();
                cb.SetFontAndSize(bf, 8);
                cb.SetTextMatrix(530, 8);
                cb.ShowText(text);
                cb.EndText();
            }
        }

        public void ExcelExportold()
        {
            if (ansGridView5.Rows.Count == 0)
            {
                return;
            }
            Object misValue = System.Reflection.Missing.Value;
            Excel.Application apl = new Microsoft.Office.Interop.Excel.Application();
            Excel.Workbook wb = (Excel.Workbook)apl.Workbooks.Add(misValue);
            Excel.Worksheet ws;
            ws = (Excel.Worksheet)wb.Worksheets[1];

            int lno = 1;
            DataTable dtExcel = new DataTable();

            DataTable dtRheader = new DataTable();
            Database.GetSqlData("select * from company", dtRheader);

            ws.Cells[lno, 1] = dtRheader.Rows[0]["name"].ToString();
            ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, ansGridView5.Columns.Count]).Merge(Type.Missing);
            ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, ansGridView5.Columns.Count]).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, ansGridView5.Columns.Count]).Font.Bold = true;
            lno++;

            ws.Cells[lno, 1] = dtRheader.Rows[0]["Address1"].ToString();
            ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, ansGridView5.Columns.Count]).Merge(Type.Missing);
            ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, ansGridView5.Columns.Count]).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, ansGridView5.Columns.Count]).Font.Bold = true;
            lno++;

            ws.Cells[lno, 1] = dtRheader.Rows[0]["Address2"].ToString();
            ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, ansGridView5.Columns.Count]).Merge(Type.Missing);
            ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, ansGridView5.Columns.Count]).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, ansGridView5.Columns.Count]).Font.Bold = true;
            lno++;

            for (int i = 0; i < ansGridView5.Columns.Count; i++)
            {
                if (ansGridView5.Columns[i].HeaderCell.Style.Alignment == DataGridViewContentAlignment.MiddleRight)
                {
                    ws.get_Range(ws.Cells[5, i + 1], ws.Cells[5, i + 1]).HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                }
                ws.get_Range(ws.Cells[i + 1, i + 1], ws.Cells[i + 1, i + 1]).ColumnWidth = ansGridView5.Columns[i].Width / 11.5;
                ws.Cells[5, i + 1] = ansGridView5.Columns[i].HeaderText.ToString();
            }

            for (int i = 0; i < ansGridView5.Rows.Count; i++)
            {
                for (int j = 0; j < ansGridView5.Columns.Count; j++)
                {
                    if (ansGridView5.Columns[j].HeaderCell.Style.Alignment == DataGridViewContentAlignment.MiddleRight)
                    {
                        ws.get_Range(ws.Cells[i + 6, j + 1], ws.Cells[i + 6, j + 1]).HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                        ws.get_Range(ws.Cells[i + 6, j + 1], ws.Cells[i + 6, j + 1]).NumberFormat = "0,0.00";
                    }
                    else
                    {
                        ws.get_Range(ws.Cells[i + 6, j + 1], ws.Cells[i + 6, j + 1]).HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                    }

                    if (ansGridView5.Columns[j].DefaultCellStyle.Font != null)
                    {
                        ws.get_Range(ws.Cells[i + 6, j + 1], ws.Cells[i + 6, j + 1]).Font.Bold = true;
                    }

                    if (ansGridView5.Rows[i].Cells[j].Value != null)
                    {
                        ws.Cells[i + 6, j + 1] = ansGridView5.Rows[i].Cells[j].Value.ToString().Replace(",", "");
                    }
                }
            }

            Excel.Range last = ws.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell, Type.Missing);
            ws.get_Range("A1", last).WrapText = true;
            apl.Visible = true;
        }

        private void frmMaster_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
                this.Dispose();
            }
            else if (e.Control && e.KeyCode == Keys.P)
            {
                LoadData(gstr, gstr);
                if (ansGridView5.Rows.Count == 0)
                {
                    return;
                }

                string tPath = Path.GetTempPath() + DateTime.Now.ToString("yyMMddhmmssfff") + ".pdf";
                ExportToPdf(tPath);
                GC.Collect();
                PdfReader frm = new PdfReader();
                frm.LoadFile(tPath);
                frm.Show();
            }
            else if (e.Control && e.KeyCode == Keys.E)
            {
                LoadData(gstr, gstr);
                if (gstr == "Account" || gstr == "StockItem")
                {
                    AccountExcelexport();
                }
                else
                {
                    Excelexport();
                }
            }
            else if (e.Control && e.KeyCode == Keys.C)
            {
                Add();
            }
            else if (gstr == "Account")
            {
                if (e.Control && e.Alt==true && e.KeyCode == Keys.S)
                {
                    InputBox box = new InputBox("Enter Password", "", true);
                    box.outStr = "";

                    box.ShowInTaskbar = false;
                    box.ShowDialog(this);


                    if (box.outStr == "admin")
                    {
                        for (int i = 0; i < ansGridView5.Rows.Count; i++)
                        {
                            
                            frm_NewAcc frm = new frm_NewAcc();
                            frm.MdiParent = this.MdiParent;
                            frm.gresave = true;
                            string acid = funs.Select_ac_id(ansGridView5.Rows[i].Cells["AccName"].Value.ToString());
                            frm.LoadData(acid.ToString(), "Account");
                            if (frm.gresave == true)
                            {




                            }
                        }
                        MessageBox.Show("Done Successfully");
                    }
                    else
                    {
                        MessageBox.Show("Enter Correct Password");
                    }

                   
                    //Database.CommandExecutor("Update account set code=Ac_id");
                    //MessageBox.Show("Updated Acccode");
                }

               
            }
            //if (e.Control && e.Alt == false && e.KeyCode == Keys.S)
            //{
            //    if (gstr == "Account")
            //    {
            //        for (int i = 0; i < ansGridView5.Rows.Count; i++)
            //        {

            //            frm_NewAcc frm = new frm_NewAcc();
            //            frm.MdiParent = this.MdiParent;
            //            frm.gresave = true;
            //            string acid = funs.Select_ac_id(ansGridView5.Rows[i].Cells["AccName"].Value.ToString());
            //            frm.LoadData(acid, "Account");
            //            if (frm.gresave == true)
            //            {

            //            }
            //        }



            //    }
               // LoadData(gstr, "Account");
            //}
            else if (e.Control && e.KeyCode == Keys.R)
            {
                LoadData(gstr, gstr);
            }
            if (Database.utype.ToUpper() == "SUPERADMIN" || Database.utype.ToUpper() == "ADMIN")
            {

           
                if (e.Control && e.KeyCode == Keys.M)
                {
                    if (gstr == "StockItem")
                    {
                        frm_Merge frm = new frm_Merge();
                        frm.ShowDialog();
                    }
                    if (gstr == "Tax")
                    {
                        frm_updateHSN frm = new frm_updateHSN();
                        frm.mode = "Merge";
                        frm.ShowDialog();
                    }
                    if (gstr == "Account")
                    {
                        frm_MergeAcc frm = new frm_MergeAcc();
                        frm.ShowDialog();
                    }
                    LoadData(gstr, gstr);
                }
                else if (e.Control && e.KeyCode == Keys.U)
                {
                    if (gstr == "Tax")
                    {
                        frm_updateHSN frm = new frm_updateHSN();
                        frm.mode = "UpdateItems";
                        frm.ShowDialog();
                    }
                }
            }

        }

        private void Excelexport()
        {
            if (ansGridView5.Rows.Count == 0)
            {
                return;
            }
            Object misValue = System.Reflection.Missing.Value;
            Excel.Application apl = new Microsoft.Office.Interop.Excel.Application();
            Excel.Workbook wb = (Excel.Workbook)apl.Workbooks.Add(misValue);
            Excel.Worksheet ws;
            ws = (Excel.Worksheet)wb.Worksheets[1];

            int lno = 1;
            DataTable dtExcel = new DataTable();

            DataTable dtRheader = new DataTable();
            Database.GetSqlData("select * from company", dtRheader);

            ws.Cells[lno, 1] = dtRheader.Rows[0]["name"].ToString();
            ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, ansGridView5.Columns.Count]).Merge(Type.Missing);
            ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, ansGridView5.Columns.Count]).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, ansGridView5.Columns.Count]).Font.Bold = true;
            lno++;

            ws.Cells[lno, 1] = dtRheader.Rows[0]["Address1"].ToString();
            ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, ansGridView5.Columns.Count]).Merge(Type.Missing);
            ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, ansGridView5.Columns.Count]).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, ansGridView5.Columns.Count]).Font.Bold = true;
            lno++;

            ws.Cells[lno, 1] = dtRheader.Rows[0]["Address2"].ToString();
            ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, ansGridView5.Columns.Count]).Merge(Type.Missing);
            ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, ansGridView5.Columns.Count]).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, ansGridView5.Columns.Count]).Font.Bold = true;
            lno++;

            int basevalue = 2;
            if (gstr == "StockItem")
            {
                if (Feature.Available("Company Colour") == "No")
                {
                    basevalue = 5;
                }
            }

            for (int i = basevalue; i < ansGridView5.Columns.Count; i++)
            {
                if (ansGridView5.Columns[i].HeaderCell.Style.Alignment == DataGridViewContentAlignment.MiddleRight)
                {
                    ws.get_Range(ws.Cells[5, i + 1], ws.Cells[5, i + 1]).HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                }
                ws.get_Range(ws.Cells[i + 1, i + 1], ws.Cells[i + 1, i + 1]).ColumnWidth = ansGridView5.Columns[i].Width / 11.5;
                ws.Cells[5, i + 1] = ansGridView5.Columns[i].HeaderText.ToString();
            }

            for (int i = 0; i < ansGridView5.Rows.Count; i++)
            {
                for (int j = basevalue; j < ansGridView5.Columns.Count; j++)
                {
                    if (ansGridView5.Columns[j].HeaderCell.Style.Alignment == DataGridViewContentAlignment.MiddleRight)
                    {
                        ws.get_Range(ws.Cells[i + 6, j + 1], ws.Cells[i + 6, j + 1]).HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                        ws.get_Range(ws.Cells[i + 6, j + 1], ws.Cells[i + 6, j + 1]).NumberFormat = "0,0.00";
                    }
                    else
                    {
                        ws.get_Range(ws.Cells[i + 6, j + 1], ws.Cells[i + 6, j + 1]).HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                    }

                    if (ansGridView5.Columns[j].DefaultCellStyle.Font != null)
                    {
                        ws.get_Range(ws.Cells[i + 6, j + 1], ws.Cells[i + 6, j + 1]).Font.Bold = true;
                    }

                    if (ansGridView5.Rows[i].Cells[j].Value != null)
                    {
                        ws.Cells[i + 6, j + 1] = ansGridView5.Rows[i].Cells[j].Value.ToString().Replace(",", "");
                    }
                }
            }

            Excel.Range last = ws.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell, Type.Missing);
            ws.get_Range("A1", last).WrapText = true;
            ws.Columns.AutoFit();
            apl.Visible = true;
        }


        private void AccountExcelexport()
        {
          

            DataTable dtaccount = new DataTable();
            if (gstr == "StockItem")
            {
                Database.GetSqlData("SELECT CASE WHEN OTHER_1.Name IS NULL THEN '' ELSE OTHER_1.Name END AS Company, CASE WHEN OTHER.Name IS NULL THEN '' ELSE OTHER.Name END AS Item, CASE WHEN OTHER_2.Name IS NULL THEN '' ELSE OTHER_2.Name END AS Colour, CASE WHEN OTHER_3.Name IS NULL  THEN '' ELSE OTHER_3.Name END AS Grp, Description.Description, Description.Pack, TAXCATEGORY.Category_Name AS HSN,  Description.Max_level, Description.Wlavel, Description.[Commission%], Description.Commission@, Description.ShortCode,  Description.Skucode, Description.Rate_Unit, Description.Pvalue, CASE WHEN ACCOUNT.Name IS NULL  THEN '<Main>' ELSE ACCount.name END AS Godown, Description.Retail, Description.Wholesale, Description.Purchase_rate, Description.Rate_X,  Description.Rate_Y, Description.Rate_Z, Description.MRP, Description.Rebate, Description.Srebate, Description.Container,  Description.Square_FT, Description.Square_MT, Description.box_quantity, Description.weight, PackCategory.Name AS PackCategory FROM Description LEFT OUTER JOIN PackCategory ON Description.PackCat_id = PackCategory.PackCat_id LEFT OUTER JOIN ACCOUNT ON Description.Godown_id = ACCOUNT.Ac_id LEFT OUTER JOIN TAXCATEGORY ON Description.Tax_Cat_id = TAXCATEGORY.Category_Id LEFT OUTER JOIN OTHER AS OTHER_3 ON Description.Group_id = OTHER_3.Oth_id LEFT OUTER JOIN OTHER AS OTHER_2 ON Description.Col_id = OTHER_2.Oth_id LEFT OUTER JOIN OTHER ON Description.Item_id = OTHER.Oth_id LEFT OUTER JOIN OTHER AS OTHER_1 ON Description.Company_id = OTHER_1.Oth_id GROUP BY dbo.Description.Description, dbo.Description.Pack, dbo.TAXCATEGORY.Category_Name, dbo.Description.Max_level, dbo.Description.Wlavel, Description.[Commission%], dbo.Description.Commission@, dbo.Description.ShortCode, dbo.Description.Skucode, dbo.Description.Rate_Unit,  dbo.Description.Pvalue, dbo.Description.Retail, dbo.Description.Wholesale, dbo.Description.Purchase_rate, dbo.Description.Rate_X, dbo.Description.Rate_Y,      dbo.Description.Rate_Z, dbo.Description.MRP, dbo.Description.Rebate, dbo.Description.Srebate, dbo.Description.Container, dbo.Description.Square_FT,    dbo.Description.Square_MT, dbo.Description.box_quantity, dbo.Description.weight, dbo.PackCategory.Name, CASE WHEN OTHER_1.Name IS NULL  THEN '' ELSE OTHER_1.Name END, CASE WHEN OTHER.Name IS NULL THEN '' ELSE OTHER.Name END, CASE WHEN OTHER_2.Name IS NULL    THEN '' ELSE OTHER_2.Name END, CASE WHEN OTHER_3.Name IS NULL THEN '' ELSE OTHER_3.Name END, CASE WHEN ACCOUNT.Name IS NULL THEN '<Main>' ELSE ACCount.name END", dtaccount);
            }
            else
            {


                Database.GetSqlData("SELECT  ACCOUNT.Name, ACCOUNTYPE.Name AS AccGroup, ACCOUNT.Address1, ACCOUNT.Address2, ACCOUNT.Phone, ACCOUNT.Grade ,ACCOUNT.Email, ACCOUNT.PAN, State.Sname AS StateName, ACCOUNT.Aadhaarno, ACCOUNT.RegStatus, ACCOUNT.MobileNo,                       CASE WHEN ACCOUNT.Balance > 0 THEN ACCOUNT.Balance ELSE 0 END AS Dr, CASE WHEN ACCOUNT.Balance < 0 THEN ACCOUNT.Balance ELSE 0 END AS Cr,   ACCOUNT.Closing_Bal as ClosingBal, OTHER.Name AS PaymentColl, ACCOUNT.RateApp FROM         ACCOUNT LEFT OUTER JOIN                  OTHER ON ACCOUNT.Loc_id = OTHER.Oth_id LEFT OUTER JOIN         ACCOUNTYPE ON ACCOUNT.Act_id = ACCOUNTYPE.Act_id LEFT OUTER JOIN     State ON ACCOUNT.State_id = State.State_id ORDER BY ACCOUNT.Name", dtaccount);
            }
            if (dtaccount.Rows.Count == 0)
            {
                return;
            }
           
            ProgrBar.Minimum = 0;
            ProgrBar.Maximum = dtaccount.Rows.Count;
            ProgrBar.Visible = true;
            Object misValue = System.Reflection.Missing.Value;
            Excel.Application apl = new Microsoft.Office.Interop.Excel.Application();
            Excel.Workbook wb = (Excel.Workbook)apl.Workbooks.Add(misValue);
            Excel.Worksheet ws;
            ws = (Excel.Worksheet)wb.Worksheets[1];
            
            int lno = 1;
            DataTable dtExcel = new DataTable();

            DataTable dtRheader = new DataTable();
            Database.GetSqlData("select * from company", dtRheader);

            ws.Cells[lno, 1] = dtRheader.Rows[0]["name"].ToString();
            ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, dtaccount.Columns.Count]).Merge(Type.Missing);
            ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, dtaccount.Columns.Count]).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, dtaccount.Columns.Count]).Font.Bold = true;
            lno++;

            ws.Cells[lno, 1] = dtRheader.Rows[0]["Address1"].ToString();
            ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, dtaccount.Columns.Count]).Merge(Type.Missing);
            ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, dtaccount.Columns.Count]).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, dtaccount.Columns.Count]).Font.Bold = true;
            lno++;

            ws.Cells[lno, 1] = dtRheader.Rows[0]["Address2"].ToString();
            ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, dtaccount.Columns.Count]).Merge(Type.Missing);
            ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, dtaccount.Columns.Count]).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, dtaccount.Columns.Count]).Font.Bold = true;
            lno++;



            for (int i = 0; i < dtaccount.Columns.Count; i++)
            {
                ws.Cells[5, i + 1] = dtaccount.Columns[i].ColumnName.ToString();
                ws.get_Range(ws.Cells[5, 1], ws.Cells[5, dtaccount.Columns.Count]).Font.Bold = true;
            }

         


            var data = new object[dtaccount.Rows.Count, dtaccount.Columns.Count];






            if (gstr == "StockItem")
            {

                for (int i = 0; i < dtaccount.Rows.Count; i++)
                {
                    ProgrBar.Value = i;
                    data[i, 0] = dtaccount.Rows[i]["Company"].ToString();
                    data[i, 1] = dtaccount.Rows[i]["Item"].ToString();

                    data[i, 2] = dtaccount.Rows[i]["Colour"].ToString();
                    data[i, 3] = dtaccount.Rows[i]["Grp"].ToString();
                    data[i, 4] = dtaccount.Rows[i]["Description"].ToString();

                    data[i, 5] = dtaccount.Rows[i]["Pack"].ToString();

                    data[i, 6] = dtaccount.Rows[i]["HSN"].ToString();
                    data[i, 7] = dtaccount.Rows[i]["Max_level"].ToString();
                    data[i, 8] = dtaccount.Rows[i]["Wlavel"].ToString();
                    data[i, 9] = dtaccount.Rows[i]["Commission%"].ToString();
                    data[i, 10] = dtaccount.Rows[i]["Commission@"].ToString();


                    data[i, 11] = dtaccount.Rows[i]["ShortCode"].ToString();
                    data[i, 12] = dtaccount.Rows[i]["SkuCode"].ToString();

                    data[i, 13] = dtaccount.Rows[i]["Rate_unit"].ToString();
                    data[i, 14] = dtaccount.Rows[i]["Pvalue"].ToString();
                    data[i, 15] = dtaccount.Rows[i]["Godown"].ToString();

                    data[i, 16] = funs.DecimalPoint(double.Parse(dtaccount.Rows[i]["Retail"].ToString()),2);
                    data[i, 17] = funs.DecimalPoint(double.Parse(dtaccount.Rows[i]["WholeSale"].ToString()),2);
                    data[i, 18] = funs.DecimalPoint(double.Parse(dtaccount.Rows[i]["Purchase_Rate"].ToString()), 2);

                    data[i, 19] = funs.DecimalPoint(double.Parse(dtaccount.Rows[i]["Rate_X"].ToString()), 2);
                    data[i, 20] = funs.DecimalPoint(double.Parse(dtaccount.Rows[i]["Rate_Y"].ToString()), 2);
                    data[i, 21] = funs.DecimalPoint(double.Parse(dtaccount.Rows[i]["Rate_Z"].ToString()), 2);
                    data[i, 22] = funs.DecimalPoint(double.Parse(dtaccount.Rows[i]["MRP"].ToString()), 2);
                    data[i, 23] = funs.DecimalPoint(double.Parse(dtaccount.Rows[i]["Rebate"].ToString()), 2);
                    data[i, 24] = funs.DecimalPoint(double.Parse(dtaccount.Rows[i]["Srebate"].ToString()), 2);
                    data[i, 25] = dtaccount.Rows[i]["Container"].ToString();

                    data[i, 26] = funs.DecimalPoint(double.Parse(dtaccount.Rows[i]["Square_FT"].ToString()), 2);
                    data[i, 27] = funs.DecimalPoint(double.Parse(dtaccount.Rows[i]["Square_MT"].ToString()), 2);
                    data[i, 28] = dtaccount.Rows[i]["box_quantity"].ToString();
                    data[i, 29] = funs.DecimalPoint(double.Parse(dtaccount.Rows[i]["weight"].ToString()),3);
                    data[i, 30] = dtaccount.Rows[i]["PackCategory"].ToString();


                   
                }
            }
            else
            {
                for (int i = 0; i < dtaccount.Rows.Count; i++)
                {
                    ProgrBar.Value = i;
                    data[i, 0] = dtaccount.Rows[i]["Name"].ToString();
                    data[i, 1] = dtaccount.Rows[i]["Accgroup"].ToString();

                    data[i, 2] = dtaccount.Rows[i]["Address1"].ToString();
                    data[i, 3] = dtaccount.Rows[i]["Address2"].ToString();
                    data[i, 4] = dtaccount.Rows[i]["Phone"].ToString();
                    data[i, 5] = dtaccount.Rows[i]["Grade"].ToString();

                    data[i, 6] = dtaccount.Rows[i]["Email"].ToString();

                    data[i, 7] = dtaccount.Rows[i]["PAN"].ToString();
                    data[i, 8] = dtaccount.Rows[i]["StateName"].ToString();
                    data[i, 9] = dtaccount.Rows[i]["AadhaarNo"].ToString();
                    data[i, 10] = dtaccount.Rows[i]["RegStatus"].ToString();
                    data[i, 11] = dtaccount.Rows[i]["Mobileno"].ToString();


                    data[i, 12] = funs.DecimalPoint(double.Parse(dtaccount.Rows[i]["Dr"].ToString()), 2);
                    data[i, 13] = funs.DecimalPoint(double.Parse(dtaccount.Rows[i]["Cr"].ToString()), 2);
                
                    data[i, 14] = dtaccount.Rows[i]["ClosingBal"].ToString();
                    data[i, 15] = dtaccount.Rows[i]["PaymentColl"].ToString();
                    data[i, 16] = dtaccount.Rows[i]["Rateapp"].ToString();

                }
            }


            var startcell = (Excel.Range)ws.Cells[6, 1];
            var endcell = (Excel.Range)ws.Cells[dtaccount.Rows.Count+5 ,dtaccount.Columns.Count];
            var writerange = ws.Range[startcell, endcell];
            writerange.Value = data;



            ProgrBar.Value = 0;
            ProgrBar.Visible = false;
            Excel.Range last = ws.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell, Type.Missing);
            ws.get_Range("A1", last).WrapText = true;
            ws.Columns.AutoFit();
            apl.Visible = true;
        }



        private void button2_Click(object sender, EventArgs e)
        {
            Report gg = new Report();
            gg.ReminderDates(Database.stDate, Database.ldate);
            gg.ShowDialog(this);
        }

        private void frmMaster_Enter(object sender, EventArgs e)
        {
            this.Size = this.MdiParent.Size;
            this.WindowState = FormWindowState.Maximized;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
           
                for (int i = 0; i < ansGridView5.Rows.Count; i++)
                {
                     if (checkBox1.Checked == true)
                     {
                         ansGridView5.Rows[i].Cells["select"].Value = true;

                     }
                     else
                     {
                         ansGridView5.Rows[i].Cells["select"].Value = false;

                     }
                }
            
           
        }
    }
}
