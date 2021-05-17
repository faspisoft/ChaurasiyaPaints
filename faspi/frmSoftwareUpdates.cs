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
    public partial class frmSoftwareUpdates : Form
    {
       
        public frmSoftwareUpdates()
        {
            InitializeComponent();
        }

        private void frmSoftwareUpdates_Load(object sender, EventArgs e)
        {
            DataTable dtSoftwareUpdates = new DataTable();
            Database.GetSqlData("select * from SoftUpdates order by Date,SNo", dtSoftwareUpdates);
            ansGridView1.DataSource = dtSoftwareUpdates;
            ansGridView1.Columns["Date"].Width = 75;
            ansGridView1.Columns["SNo"].Width = 40;
            ansGridView1.Columns["Update"].Width = 200;
            ansGridView1.Columns["Details"].Width = 750;
        }

        public void Update()
        {
            String sql;
            string LastUpdate;
            Database.OpenConnection();

            LastUpdate = Database.GetScalarDate("SELECT SoftUpdates.Date as Udate FROM SoftUpdates WHERE [Update]='Update Upto' ");
            if (DateTime.Parse(LastUpdate).ToString(Database.dformat) == Database.ExeDate.ToString(Database.dformat))
            {
                Database.CloseConnection();
                return;
            }
            else if (DateTime.Parse(LastUpdate) > DateTime.Parse(Database.ExeDate.ToString(Database.dformat)))
            {

                MessageBox.Show("Please Contact your Administrator");
                Environment.Exit(0);
            }
            else
            {

                Master.UpdateAll();
                Modeinjou();
                CashierId();
                updBranchAgent();
                RemarkText();
                rateapp2();
                AccountwiseBroker();
                Backup();
                gradeinacc();
                OpeningRename();
                OpeningA();
                Companydbdet();
                AccCode();
                AddRebate();
                Addpageroleid();
                Extradel();
                Batchno();

                Billadjest();
                Reffno();
                winpage();
                winpageroleid();
                winpageaccsale();
                winpageroleidaccsale();
                BilladSr();
                //Ewaybill
                Pincode();
                City();
                FillCity();

                EwayBill();
                IsVehicle();
                Ewblogindetail();
                EwayBillNo();
                dispatch();
                transportdet();
                GSTFeature();
                winpagecity();
                winpageroleidcity();
                Updateexe();
                Master.UpdateAll();
               // u();
            }
            if (Database.DatabaseType == "access")
            {
                Database.CommandExecutor("Drop table QryJournal");
                sql = "Create view QryJournal as SELECT JOURNAL.Vdate, VOUCHERTYPE.Short, VOUCHERINFO.Vnumber, ACCOUNT.Name, IIf(JOURNAL.Amount>0,JOURNAL.Amount,0) AS Dr, IIf(JOURNAL.Amount<0,-1*(JOURNAL.Amount),0) AS Cr, JOURNAL.Narr AS Expr1, VOUCHERTYPE.Short & ' ' & Format(JOURNAL.Vdate,'yyyymmdd' & ' ' & VOUCHERINFO.Vnumber) AS DocNumber, VOUCHERTYPE.A, VOUCHERTYPE.B, JOURNAL.Sno FROM JOURNAL, ACCOUNT, VOUCHERINFO, VOUCHERTYPE WHERE (((JOURNAL.Ac_id)=[ACCOUNT].[Ac_id]) AND ((JOURNAL.Vi_id)=[VOUCHERINFO].[Vi_id]) AND ((VOUCHERINFO.Vt_id)=[VOUCHERTYPE].[Vt_id]))";
                Database.CommandExecutor(sql);

                Database.CommandExecutor("Drop table QryAccountinfo");
                sql = "Create view QryAccountinfo as SELECT ACCOUNT.Name, ACCOUNT.Address1, ACCOUNT.Address2, ACCOUNT.Phone, ACCOUNT.Email, ACCOUNT.Tin_number, ACCOUNT.Loc_id, iif(ACCOUNT.Balance>0,ACCOUNT.Balance,0) AS Dr, iif(ACCOUNT.Balance<0,-1*(ACCOUNT.Balance),0) AS Cr, ACCOUNT.Blimit, ACCOUNT.Dlimit, CONTRACTOR.Name AS ContName, CONTRACTOR.Address1 AS ContAddress1, CONTRACTOR.Address2 AS ContAddress2, CONTRACTOR.Phone AS ContPhone, CONTRACTOR.Email AS ContEmail, CONTRACTOR.Loc_id AS ContLoc_id,  iif(ACCOUNT.Balance2>0,ACCOUNT.Balance2,0) AS Dr2, iif(ACCOUNT.Balance2<0,-1*(ACCOUNT.Balance2),0) AS Cr2 FROM (ACCOUNT LEFT JOIN ACCOUNTYPE ON ACCOUNT.Act_id = ACCOUNTYPE.Act_id) LEFT JOIN CONTRACTOR ON ACCOUNT.Con_id = CONTRACTOR.Con_id";
                Database.CommandExecutor(sql);

                Database.CommandExecutor("Drop table QryVoucher");
                sql = "Create view QryVoucher as SELECT VOUCHERINFO.Vi_id, VOUCHERINFO.Vnumber, VOUCHERINFO.Vdate, VOUCHERINFO.Duedate, ACC.Name, ACC.Address1, ACC.Address2, Sum(VOUCHERDET.Taxabelamount) AS ItemTaxable, ACC.Phone, ACC.Email, ACC.Tin_number, ACC.PAN, VOUCHERDET.Itemsr, VOUCHERDET.Rate_am, VOUCHERDET.weight, VOUCHERDET.Quantity, VOUCHERDET.Description, VOUCHERDET.remark1, VOUCHERDET.remark2, VOUCHERDET.remark3, VOUCHERDET.remark4, VOUCHERDET.orgpacking AS Packing, ITEMCHARGES.Chargesr, CHARGES.Name, ITEMCHARGES.Amount, ITEMCHARGES.Camount, VOUCHERTYPE.Short, VOUCHERINFO.Svnum, VOUCHERINFO.Svdate, VOUCHERINFO.Totalamount, VOUCHERINFO.Formno, VOUCHERINFO.Transport1, VOUCHERINFO.Transport2, VOUCHERINFO.DeliveryAt, VOUCHERDET.qd, VOUCHERDET.cd, VOUCHERDET.Amount AS GridAmount, VOUCHERDET.TotTaxPer AS TaxSlab, VOUCHERDET.MRP, DESCRIPTION.Skucode, VOUCHERINFO.Transport3, VOUCHERINFO.Transport4, VOUCHERINFO.Transport5, VOUCHERINFO.Transport6, VOUCHERINFO.Grno, VOUCHERDET.comqty, VOUCHERDET.rate1 AS TaxRate1, VOUCHERDET.rate2 AS TaxRate2, VOUCHERDET.rate3 AS TaxRate3, VOUCHERDET.rate4 AS TaxRate4, VOUCHERDET.taxamt1, VOUCHERDET.taxamt2, VOUCHERDET.taxamt3, VOUCHERDET.taxamt4, TAXCATEGORY.Category_Name, TAXCATEGORY.Commodity_Code, VOUCHERDET.flatdis, State.Sname, State.GSTCode, VOUCHERDET.QDAmount, VOUCHERDET.CDAmount, VOUCHERDET.FDAmount, VOUCHERDET.TotalDis, VOUCHERDET.Amount0, VOUCHERDET.Amount5, VOUCHERDET.bottomdis, VOUCHERINFO.CreTime, VOUCHERINFO.ModTime, Userinfo.Uname, VOUCHERINFO.Shipto AS ShiptoN, VOUCHERINFO.ShiptoAddress1, VOUCHERINFO.ShiptoAddress2, State_1.Sname AS ShiptoState, State_1.GSTCode AS ShiptoStateCode, VOUCHERINFO.ShiptoPhone, VOUCHERINFO.ShiptoEmail, VOUCHERINFO.ShiptoTIN, DESCRIPTION.box_quantity, VOUCHERINFO.Invoiceno, ACC.Printname, VOUCHERINFO.Shipto, VOUCHERDET.Pvalue, ACC.Aadhaarno, VOUCHERINFO.ShiptoPAN, VOUCHERINFO.ShiptoAadhar, VOUCHERDET.Batch_Code, VOUCHERDET.DAT, VOUCHERDET.DATAmount, VOUCHERINFO.RCM, VOUCHERDET.TotTaxPer, TAXCATEGORY.Item_Type, VOUCHERINFO.Reffno, VOUCHERDET.Type, DESCRIPTION.Description AS Orgdescription,iif(ACCOUNT_1.Name is null,'<MAIN>',ACCOUNT_1.Name) AS Godown FROM (((((((((((VOUCHERINFO LEFT JOIN VOUCHERDET ON VOUCHERINFO.Vi_id = VOUCHERDET.Vi_id) LEFT JOIN ITEMCHARGES ON (VOUCHERDET.Vi_id = ITEMCHARGES.Vi_id) AND (VOUCHERDET.Itemsr = ITEMCHARGES.Itemsr)) LEFT JOIN (SELECT * FROM ACCOUNT)  AS ACC ON VOUCHERINFO.Ac_id = ACC.Ac_id) LEFT JOIN DESCRIPTION ON VOUCHERDET.Des_ac_id = DESCRIPTION.Des_id) LEFT JOIN CHARGES ON ITEMCHARGES.Charg_id = CHARGES.Ch_id) LEFT JOIN ACCOUNT ON ITEMCHARGES.Accid = ACCOUNT.Ac_id) LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id) LEFT JOIN TAXCATEGORY ON VOUCHERDET.Category_Id = TAXCATEGORY.Category_Id) LEFT JOIN State ON ACC.State_id = State.State_id) LEFT JOIN Userinfo ON VOUCHERINFO.user_id = Userinfo.U_id) LEFT JOIN State AS State_1 ON VOUCHERINFO.ShiptoStateid = State_1.State_id) LEFT JOIN ACCOUNT AS ACCOUNT_1 ON VOUCHERDET.godown_id = ACCOUNT_1.Ac_id GROUP BY VOUCHERINFO.Vi_id, VOUCHERINFO.Vnumber, VOUCHERINFO.Vdate, VOUCHERINFO.Duedate, ACC.Name, ACC.Address1, ACC.Address2, ACC.Phone, ACC.Email, ACC.Tin_number, ACC.PAN, VOUCHERDET.Itemsr, VOUCHERDET.Rate_am, VOUCHERDET.weight, VOUCHERDET.Quantity, VOUCHERDET.Description, VOUCHERDET.remark1, VOUCHERDET.remark2, VOUCHERDET.remark3, VOUCHERDET.remark4, VOUCHERDET.orgpacking, ITEMCHARGES.Chargesr, CHARGES.Name, ITEMCHARGES.Amount, ITEMCHARGES.Camount, VOUCHERTYPE.Short, VOUCHERINFO.Svnum, VOUCHERINFO.Svdate, VOUCHERINFO.Totalamount, VOUCHERINFO.Formno, VOUCHERINFO.Transport1, VOUCHERINFO.Transport2, VOUCHERINFO.DeliveryAt, VOUCHERDET.qd, VOUCHERDET.cd, VOUCHERDET.Amount, VOUCHERDET.TotTaxPer, VOUCHERDET.MRP, DESCRIPTION.Skucode, VOUCHERINFO.Transport3, VOUCHERINFO.Transport4, VOUCHERINFO.Transport5, VOUCHERINFO.Transport6, VOUCHERINFO.Grno, VOUCHERDET.comqty, VOUCHERDET.rate1, VOUCHERDET.rate2, VOUCHERDET.rate3, VOUCHERDET.rate4, VOUCHERDET.taxamt1, VOUCHERDET.taxamt2, VOUCHERDET.taxamt3, VOUCHERDET.taxamt4, TAXCATEGORY.Category_Name, TAXCATEGORY.Commodity_Code, VOUCHERDET.flatdis, State.Sname, State.GSTCode, VOUCHERDET.QDAmount, VOUCHERDET.CDAmount, VOUCHERDET.FDAmount, VOUCHERDET.TotalDis, VOUCHERDET.Amount0, VOUCHERDET.Amount5, VOUCHERDET.bottomdis, VOUCHERINFO.CreTime, VOUCHERINFO.ModTime, Userinfo.Uname, VOUCHERINFO.Shipto, VOUCHERINFO.ShiptoAddress1, VOUCHERINFO.ShiptoAddress2, State_1.Sname, State_1.GSTCode, VOUCHERINFO.ShiptoPhone, VOUCHERINFO.ShiptoEmail, VOUCHERINFO.ShiptoTIN, DESCRIPTION.box_quantity, VOUCHERINFO.Invoiceno, ACC.Printname, VOUCHERINFO.Shipto, VOUCHERDET.Pvalue, ACC.Aadhaarno, VOUCHERINFO.ShiptoPAN, VOUCHERINFO.ShiptoAadhar, VOUCHERDET.Batch_Code, VOUCHERDET.DAT, VOUCHERDET.DATAmount, VOUCHERINFO.RCM, VOUCHERDET.TotTaxPer, TAXCATEGORY.Item_Type, VOUCHERINFO.Reffno, VOUCHERDET.Type, DESCRIPTION.Description, ACCOUNT_1.Name";
                Database.CommandExecutor(sql);

                Database.CommandExecutor("Drop table QryVoucherTax");
                sql = "Create view QryVoucherTax as SELECT VOUCHERINFO.Vi_id, TAXCATEGORY.Commodity_Code, TAXCATEGORY.Category_Name, Sum(VOUCHERDET.Taxabelamount) AS ItemTaxable, Sum(VOUCHERDET.taxamt1) AS Tax1, Sum(VOUCHERDET.taxamt2) AS Tax2, Sum(VOUCHERDET.taxamt3) AS Tax3, Sum(VOUCHERDET.taxamt4) AS Tax4, Max(VOUCHERDET.rate1) AS rate1, Max(VOUCHERDET.rate2) AS rate2, Max(VOUCHERDET.rate3) AS rate3, Max(VOUCHERDET.rate4) AS rate4 FROM (((VOUCHERINFO LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id) LEFT JOIN ACCOUNT ON VOUCHERINFO.Ac_id = ACCOUNT.Ac_id) LEFT JOIN (VOUCHERDET LEFT JOIN DESCRIPTION ON VOUCHERDET.Des_ac_id = DESCRIPTION.Des_id) ON VOUCHERINFO.Vi_id = VOUCHERDET.Vi_id) LEFT JOIN TAXCATEGORY ON VOUCHERDET.Category_Id = TAXCATEGORY.Category_Id GROUP BY VOUCHERINFO.Vi_id, TAXCATEGORY.Commodity_Code, TAXCATEGORY.Category_Name";
                Database.CommandExecutor(sql);


                Database.CommandExecutor("Drop table QryVoucherdes");
                sql = "Create view QryVoucherdes as SELECT test.vi_id, test.Sequence, test.SubSequence, test.Name, test.value, test.Amount, test.[+/-], test.[@/Flat/%] FROM (SELECT VOUCHARGES.Vi_id, VOUCHARGES.Entry_typ AS Sequence, VOUCHARGES.Srno AS SubSequence, VOUCHARGES.Charg_Name AS Name, VOUCHARGES.Amount AS [Value], VOUCHARGES.Camount AS Amount, VOUCHARGES.Addsub AS [+/-], VOUCHARGES.Ctype AS [@/Flat/%] FROM VOUCHARGES LEFT JOIN VOUCHERINFO ON VOUCHARGES.Vi_id = VOUCHERINFO.Vi_id UNION ALL SELECT ITEMCHARGES.Vi_id, 0 AS Sequence, 4 AS SubSequence, CHARGES.Name & ACCOUNT.Name AS Name, 0 AS [Value], Sum(ITEMCHARGES.Camount) AS Amount, 0 AS [+/-], 0 AS [@/Flat/%] FROM (ITEMCHARGES LEFT JOIN ACCOUNT ON ITEMCHARGES.Accid = ACCOUNT.Ac_id) LEFT JOIN CHARGES ON ITEMCHARGES.Charg_id = CHARGES.Ch_id GROUP BY ITEMCHARGES.Vi_id, CHARGES.Name & ACCOUNT.Name )  AS test";
                Database.CommandExecutor(sql);

                Database.CommandExecutor("Drop table QryItemTranjection");
                sql = "Create view QryItemTranjection as SELECT VOUCHERTYPE.Type, VOUCHERTYPE.Short, VOUCHERINFO.Vnumber, VOUCHERINFO.Vdate, VOUCHERINFO.Duedate, VOUCHERINFO.TaxableAmount AS VoucherTaxable, VOUCHERINFO.Totalamount AS VoucherNetAmt, VOUCHERDET.Quantity, VOUCHERDET.Rate_am, VOUCHERDET.weight, VOUCHERDET.Description AS Description, VOUCHERDET.Taxabelamount AS ItemTaxable, VOUCHERDET.Amount AS ItemAmount, DESCRIPTION.Description AS OrgDescription, TAXCATEGORY.Category_Name, TAXCATEGORY.Commodity_Code, ACCOUNT.Name, ACCOUNT.Address1, ACCOUNT.Address2, ACCOUNT.Phone, ACCOUNT.Email, ACCOUNT.Tin_number, CONTRACTOR.Name, DESCRIPTION.Mark, VOUCHERINFO.Svnum, VOUCHERINFO.Svdate, OTHER.Name AS Company, OTHER_1.Name AS Item, OTHER_2.Name AS Color, OTHER_3.Name AS [Group], VOUCHERDET.[Commission%], VOUCHERDET.[Commission@], DESCRIPTION.Des_id, [VOUCHERTYPE].[Short] & ' ' & Format(VOUCHERINFO.Vdate,'yyyymmdd' & ' ' & [VOUCHERINFO].[Vnumber]) AS DocNumber, VOUCHERTYPE.Effect_On_Stock, VOUCHERTYPE.Effect_On_Acc, VOUCHERTYPE.IncludingTax, VOUCHERTYPE.ExcludingTax, VOUCHERTYPE.ExState, VOUCHERTYPE.TaxInvoice, VOUCHERTYPE.Unregistered, VOUCHERDET.qd, VOUCHERDET.cd, VOUCHERDET.Cost, VOUCHERTYPE.A, VOUCHERTYPE.B, VOUCHERINFO.Formno, VOUCHERDET.packing AS Packing,VOUCHERDET.pvalue AS Pvalue, VOUCHERDET.Rate_unit AS Utype FROM ((((VOUCHERINFO LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id) LEFT JOIN ACCOUNT ON VOUCHERINFO.Ac_id = ACCOUNT.Ac_id) LEFT JOIN (((((VOUCHERDET LEFT JOIN DESCRIPTION ON VOUCHERDET.Des_ac_id = DESCRIPTION.Des_id) LEFT JOIN OTHER ON DESCRIPTION.Company_id = OTHER.Oth_id) LEFT JOIN OTHER AS OTHER_1 ON DESCRIPTION.Item_id = OTHER_1.Oth_id) LEFT JOIN OTHER AS OTHER_2 ON DESCRIPTION.Col_id = OTHER_2.Oth_id) LEFT JOIN OTHER AS OTHER_3 ON DESCRIPTION.Group_id = OTHER_3.Oth_id) ON VOUCHERINFO.Vi_id = VOUCHERDET.Vi_id) LEFT JOIN CONTRACTOR ON VOUCHERINFO.Conn_id = CONTRACTOR.Con_id) LEFT JOIN TAXCATEGORY ON VOUCHERDET.Category_Id = TAXCATEGORY.Category_Id";
                Database.CommandExecutor(sql);

                Database.CommandExecutor("Drop table QryPriceList");
                sql = "Create view QryPriceList as SELECT Description.Pack AS Packing, DESCRIPTION.Description, OTHER.Name AS Company, OTHER_1.Name AS Item, OTHER_3.Name AS Color, OTHER_2.Name AS [Group], DESCRIPTION.Purchase_rate, DESCRIPTION.Wholesale, DESCRIPTION.Retail, DESCRIPTION.Pvalue, DESCRIPTION.Rate_X, DESCRIPTION.Rate_Y, DESCRIPTION.Rate_Z, DESCRIPTION.MRP FROM (((DESCRIPTION LEFT JOIN OTHER ON DESCRIPTION.Company_id = OTHER.Oth_id) LEFT JOIN OTHER AS OTHER_1 ON DESCRIPTION.Item_id = OTHER_1.Oth_id) LEFT JOIN OTHER AS OTHER_2 ON DESCRIPTION.Col_id = OTHER_2.Oth_id) LEFT JOIN OTHER AS OTHER_3 ON DESCRIPTION.Group_id = OTHER_3.Oth_id";
                Database.CommandExecutor(sql);
            }
            Database.CloseConnection();
        }
        private void GSTFeature()
        {
            try
            {
                Database.BeginTran();
                //AddFeatureNew("Transaction", "Sms Sent to MobileNo/PhoneNo", "Sms Sent to MobileNo/PhoneNo", true, false, "Phone;Mobile;", "Mobile", "Mobile", "Mobile", "Mobile", "Mobile", "ComboBox");

              //  AddFeatureNew("Transaction", "IP Backup", "IP Backup", true, false, "Yes;No", "Yes", "Yes", "Yes", "Yes", "Yes", "ComboBox");


                AddFeatureNew("Transactions", "Eway Bill On Quantity", "Eway Bill On Quantity", false, false, "Quantity;Weight;", "Quantity", "Quantity", "Quantity", "Quantity", "Quantity", "ComboBox");
                AddFeatureNew("Transactions", "Eway Bill Unit if Quantity in Weight", "Eway Bill Unit if Quantity in Weight", false, false, "QTL;KGS;", "QTL", "QTL","QTL","QTL","QTL", "ComboBox");


             
                Database.CommitTran();
            }
            catch (Exception ex)
            {
                Database.RollbackTran();
            }
        }

        private void Pincode()
        {
            try
            {
                Database.BeginTran();
                    if (Database.CommandExecutor("ALTER TABLE Company ADD Pincode int") == true)
                    {
                        Database.CommandExecutor("ALTER TABLE Account ADD Pincode int");
                        Database.CommandExecutor("ALTER TABLE Voucherinfo ADD ShiptoPincode nvarchar(50)");
                        Database.CommandExecutor("Update Company set Pincode=0");
                        Database.CommandExecutor("Update Account set Pincode=0");
                        Database.CommandExecutor("Update Voucherinfo set ShiptoPincode=''");
                    }
               
                Database.CommitTran();
            }
            catch (Exception ex)
            {
                Database.RollbackTran();
            }
        }

        private void City()
        {
            try
            {
                Database.BeginTran();

              
                    DataTable dtbranch = new DataTable();
                    Database.GetSqlData("SELECT 1 AS Expr1 FROM  sys.tables WHERE (name = 'City')", dtbranch);
                    if (dtbranch.Rows.Count == 0)
                    {
                        if (Database.CommandExecutor("create table City ([City_id]  nvarchar(100), [CName] nvarchar (255),Nid int, LocationId nvarchar (255)  CONSTRAINT cityn PRIMARY KEY(CName))") == true)
                        {
                            //DataTable dtaddress2 = new DataTable();
                            //Database.GetSqlData("SELECT distinct Address2 as city from Account where (Address2 <> '')", dtaddress2);
                            //int Nid = 1;
                            //for (int i = 0; i < dtaddress2.Rows.Count; i++)
                            //{
                            //    string cityid = Database.LocationId + (Nid );
                            //    Database.CommandExecutor("insert into City ([City_id],[CName],Nid,LocationId) values('" + cityid + "','" + dtaddress2.Rows[i]["city"].ToString() + "'," + Nid + ",'" + Database.LocationId + "')");
                            //    Nid++;  
                            //}
                        }
                    }
                Database.CommitTran();
            }
            catch (Exception ex)
            {
                Database.RollbackTran();
            }
        }


        private void FillCity()
        {
            try
            {
                Database.BeginTran();



                DataTable dtaddress2 = new DataTable();
                Database.GetSqlData("SELECT distinct Address2 as city from Account where (Address2 <> '')", dtaddress2);
                int Nid = 1;
                DataTable dtcity = new DataTable("City");
                Database.GetSqlData("select * from City where locationid='" + Database.LocationId + "'", dtcity);
                if (dtcity.Rows.Count == 0)
                {

                    for (int i = 0; i < dtaddress2.Rows.Count; i++)
                    {
                        //string cityid = Database.LocationId + (Nid);
                        //Database.CommandExecutor("insert into City ([City_id],[CName],Nid,LocationId) values('" + cityid + "','" + dtaddress2.Rows[i]["city"].ToString() + "'," + Nid + ",'" + Database.LocationId + "')");
                        //Nid++;


                        dtcity.Rows.Add();
                        dtcity.Rows[dtcity.Rows.Count - 1]["city_id"] = Database.LocationId + (Nid);
                        dtcity.Rows[dtcity.Rows.Count - 1]["Nid"] = (Nid);
                        dtcity.Rows[dtcity.Rows.Count - 1]["LocationId"] = Database.LocationId;
                        dtcity.Rows[dtcity.Rows.Count - 1]["CNAME"] = dtaddress2.Rows[i]["city"].ToString();




                        Nid++;

                    }
                }
                Database.SaveData(dtcity);
                Database.CommitTran();
            }
            catch (Exception ex)
            {
                Database.RollbackTran();
            }
        }
        private void EwayBill()
        {
            try
            {
                Database.BeginTran();
              
                    if (Database.CommandExecutor("ALTER TABLE Account ADD Distance money") == true)
                    {
                        Database.CommandExecutor("ALTER TABLE Account ADD  city_id nvarchar(50)");
                        Database.CommandExecutor("ALTER TABLE Account ADD  Transporter_id nvarchar(50)");
                        Database.CommandExecutor("ALTER TABLE Company ADD  City_id nvarchar(50)");
                        Database.CommandExecutor("ALTER TABLE Voucherinfo ADD  Transporter_id nvarchar(50)");
                        Database.CommandExecutor("ALTER TABLE Voucherinfo ADD  ShiptoCity_id nvarchar(50)");
                      
                        Database.CommandExecutor("ALTER TABLE Voucherinfo ADD  ShiptoDistance money");


                        Database.CommandExecutor("Update Account set Distance=0");
                        Database.CommandExecutor("update Account set Account.City_id=  dbo.City.City_id    FROM    Account,city where    dbo.Account.Address2 = dbo.City.Cname and Account.Address2<>''");
                       // Database.CommandExecutor("Update Account set City_id=0");
                        Database.CommandExecutor("Update Account set Transporter_id=''");

                        Database.CommandExecutor("Update Voucherinfo set ShiptoCity_id=''");
                        Database.CommandExecutor("Update Voucherinfo set Transporter_id=''");
                        Database.CommandExecutor("Update Voucherinfo set ShiptoDistance=0");
                    }
          
                Database.CommitTran();
            }
            catch (Exception ex)
            {
                Database.RollbackTran();
            }

        }
        private void IsVehicle()
        {
            try
            {
                Database.BeginTran();
             
                    if (Database.CommandExecutor("ALTER TABLE transportdetails ADD IsVehicleNo bit") == true)
                    {
                        Database.CommandExecutor("Update transportdetails set IsVehicleNo='true' where FName='Field1'");
                        Database.CommandExecutor("Update transportdetails set IsVehicleNo='false' where FName<>'Field1'");
                    }
              
                Database.CommitTran();
            }
            catch (Exception ex)
            {
                Database.RollbackTran();
            }

        }

        private void Ewblogindetail()
        {
            try
            {
                Database.BeginTran();
               
                    if (Database.CommandExecutor("ALTER TABLE Company ADD EwbLoginDetail nvarchar(MAX)") == true)
                    {
                    }
               
                Database.CommitTran();
            }
            catch (Exception ex)
            {
                Database.RollbackTran();
            }

        }



        private void EwayBillNo()
        {
            try
            {
                Database.BeginTran();
              
                    if (Database.CommandExecutor("ALTER TABLE Voucherinfo ADD EwayBillno nvarchar(255)") == true)
                    {

                    }
               
                Database.CommitTran();
            }
            catch (Exception ex)
            {
                Database.RollbackTran();



            }

        }

        private void dispatch()
        {
            try
            {
                Database.BeginTran();
               
                    if (Database.CommandExecutor("ALTER TABLE Voucherinfo ADD dispatch_id nvarchar(255)") == true)
                    {
                        Database.CommandExecutor("Update Voucherinfo set dispatch_id=''");
                    }
               
                Database.CommitTran();
            }
            catch (Exception ex)
            {
                Database.RollbackTran();
            }

        }


        private void transportdet()
        {
            try
            {
                Database.BeginTran();
                if (Database.CommandExecutor("ALTER TABLE Voucherinfo ADD TransDocno  nvarchar(50)") == true)
                {
                    Database.CommandExecutor("ALTER TABLE Voucherinfo ADD TransDocDate Datetime");
                    Database.CommandExecutor("ALTER TABLE Voucherinfo ADD TransVehNo  nvarchar(50)");
                    Database.CommandExecutor("Update Voucherinfo set TransDocno=''");
                    Database.CommandExecutor("Update Voucherinfo set TransDocDate=" + DBNull.Value);
                    string selectedcol = "";
                    string vehcolname = Database.GetScalarText("Select Fname from TransportDetails where IsVehicleNo='true'");
                    if (vehcolname == "Field1")
                    {
                        selectedcol = "Transport1";
                    }
                    else if (vehcolname == "Field2")
                    {
                        selectedcol = "Transport2";
                    }
                    else if (vehcolname == "Field3")
                    {
                        selectedcol = "DeliveryAt";
                    }
                    else if (vehcolname == "Field4")
                    {
                        selectedcol = "Grno";
                    }
                    else if (vehcolname == "Field5")
                    {
                        selectedcol = "Transport5";
                    }
                    else if (vehcolname == "Field6")
                    {
                        selectedcol = "Transport6";
                    }
                    else if (vehcolname == "Field7")
                    {
                        selectedcol = "Transport7";
                    }
                    else if (vehcolname == "Field8")
                    {
                        selectedcol = "Transport8";
                    }
                    Database.CommandExecutor("Update Voucherinfo set TransVehNo=" + selectedcol);
                    Database.CommandExecutor("Update TransportDetails set status='Not Visible' where IsVehicleNo='true'");
                }
                Database.CommitTran();
            }
            catch (Exception ex)
            {
                Database.RollbackTran();
            }

        }









        private void Addprimarykey()
        {
            try
            {
                Database.BeginTran();

                if (Database.DatabaseType == "sql")
                {
                    Database.CommandExecutor("ALTER TABLE Account ADD CONSTRAINT [AC_Name] PRIMARY KEY CLUSTERED ([Name] ASC)");
                    Database.CommandExecutor("ALTER TABLE Description ADD CONSTRAINT PK_Person PRIMARY KEY (Description,Pack)");
                }

                Database.CommitTran();
            }
            catch (Exception ex)
            {
                Database.RollbackTran();
            }
        }
        private void Extradel()
        {
            try
            {
                Database.BeginTran();

                if (Database.DatabaseType == "sql")
                {
                    Database.CommandExecutor("delete from voucherdet where vi_id='ser10958' and des_ac_id='0'");
                    Database.CommandExecutor("delete FROM OTHER WHERE     (Oth_id NOT IN (SELECT     Loc_id FROM   ACCOUNT)) AND (Type = 'SER17')");
                    Database.CommandExecutor("delete FROM OTHER WHERE     (Oth_id NOT IN (SELECT     company_id FROM   description)) AND (Type = 'SER14')");
                    Database.CommandExecutor("delete FROM OTHER WHERE     (Oth_id NOT IN (SELECT     item_id FROM   description)) AND (Type = 'SER15')");
                    Database.CommandExecutor("delete FROM OTHER WHERE     (Oth_id NOT IN (SELECT     col_id FROM   description)) AND (Type = 'SER18')");
                    Database.CommandExecutor("delete FROM OTHER WHERE     (Oth_id NOT IN (SELECT     group_id FROM   description)) AND (Type = 'SER16')");
                }

                Database.CommitTran();
            }
            catch (Exception ex)
            {
                Database.RollbackTran();
            }
        }



        private void BilladSr()
        {
            try
            {
                Database.BeginTran();
                if (Database.CommandExecutor("ALTER TABLE BillAdjest ADD  ItemSr int"))
                {
                    Database.CommandExecutor("ALTER TABLE BillAdjest ADD  AdjustSr int");
                    Database.CommandExecutor("Update BillAdjest set ItemSr=1");
                    Database.CommandExecutor("Update BillAdjest set AdjustSr=1");
                }

                Database.CommitTran();

            }
            catch (Exception ex)
            {
                Database.RollbackTran();
            }

        }



        private void AccountwiseBroker()
        {
            try
            {
                Database.BeginTran();
                //if (Database.GetScalarInt("Select Count(*) from Accountype where Name='Agent'") == 1)
                //{
                //    string act_id = Database.GetScalarText("Select Act_id from Accountype where Name='Agent'");
                //    DataTable dt = new DataTable();
                //    Database.GetSqlData("Select * from Contractor ", dt);
                //    for (int i = 0; i < dt.Rows.Count; i++)
                //    {
                //        DataTable dtAcid = new DataTable("Account");

                //        Database.GetSqlData("select max(Nid) as Nid from account where locationid='" + Database.LocationId + "'", dtAcid);
                //        int Nid = int.Parse(dtAcid.Rows[0][0].ToString());
                //        dtAcc.Rows[0]["ac_id"] = Database.LocationId + (Nid + 1);
                //        dtAcc.Rows[0]["Nid"] = (Nid + 1);
                //        dtAcc.Rows[0]["LocationId"] = Database.LocationId;
                //        dtAcc.Rows[0]["user_id"] = Database.user_id;
                //        dtAcc.Rows[0]["Modifiedby"] = "";

                //        Database.CommandExecutor("insert into Account ()");

                //        string newcon_id = dt.Rows[i]["Reff_id"].ToString();
                //        Database.CommandExecutor("Update Account set act_id='" + act_id + "' where ac_id='" + newcon_id + "'");
                //        Database.CommandExecutor("Update Account set Con_id='" + reffac_id + "' where Con_id='" + dt.Rows[i]["Con_id"].ToString() + "'");
                //        Database.CommandExecutor("Update Voucherinfo set Conn_id='" + newcon_id + "' where Conn_id='" + dt.Rows[i]["Con_id"].ToString() + "'");
                //    }


                //}
                Database.CommitTran();
            }
            catch (Exception ex)
            {
                Database.RollbackTran();
            }
        }


        private void CashierId()
        {

            try
            {
                Database.BeginTran();
                if (Database.CommandExecutor("ALTER TABLE Voucherinfo ADD  Cashier_id nvarchar(20)") == true)
                {
                    Database.CommandExecutor("Update Voucherinfo set Cashier_id='SER1'");
                }
                Database.CommitTran();
            }
            catch (Exception ex)
            {
                Database.RollbackTran();
            }
        }

        private void Modeinjou()
        {

            try
            {
                Database.BeginTran();
               
                    if (Database.CommandExecutor("ALTER TABLE Journal ADD  A bit")==true)
                    {
                        Database.CommandExecutor("ALTER TABLE Journal ADD  B bit");
                        Database.CommandExecutor("ALTER TABLE Journal ADD  AB bit");

                        Database.CommandExecutor("update journal set journal.A=  dbo.VOUCHERTYPE.A    FROM    Voucherinfo,Vouchertype,Journal where    dbo.VOUCHERTYPE.Vt_id = dbo.VOUCHERINFO.Vt_id and  dbo.VOUCHERINFO.Vi_id = dbo.Journal.Vi_id");
                        Database.CommandExecutor("update journal set journal.B=  dbo.VOUCHERTYPE.B    FROM    Voucherinfo,Vouchertype,Journal where    dbo.VOUCHERTYPE.Vt_id = dbo.VOUCHERINFO.Vt_id and  dbo.VOUCHERINFO.Vi_id = dbo.Journal.Vi_id");
                        Database.CommandExecutor("update journal set journal.AB=  dbo.VOUCHERTYPE.AB    FROM    Voucherinfo,Vouchertype,Journal where    dbo.VOUCHERTYPE.Vt_id = dbo.VOUCHERINFO.Vt_id and  dbo.VOUCHERINFO.Vi_id = dbo.Journal.Vi_id");                       
                    }
               
                Database.CommitTran();
            }
            catch (Exception ex)
            {
                Database.RollbackTran();
            }
        }
        private void gradeinacc()
        {

            try
            {
                Database.BeginTran();

                if (Database.CommandExecutor("ALTER TABLE Account ADD  Grade nvarchar(10)") == true)
                {
                   
                   
                }

                Database.CommitTran();
            }
            catch (Exception ex)
            {
                Database.RollbackTran();
            }
        }
      

        private void ProduConsum()
        {

            try
            {
                Database.BeginTran();
                DataTable dtProduConsum = new DataTable();
                Database.GetSqlData("SELECT 1 AS Expr1 FROM  sys.tables WHERE (name = 'ProductFormula')", dtProduConsum);
                if (dtProduConsum.Rows.Count == 0)
                {
                    if (Database.CommandExecutor("create table ProductFormula([productionItem_id] nvarchar(255),[ConsumItem_id] nvarchar(255),Sno int,qty money CONSTRAINT PR PRIMARY KEY (productionItem_id,sno))") == true)
                    {
                        
                    }
                }
                Database.CommitTran();
            }
            catch (Exception ex)
            {
                Database.RollbackTran();
            }
        }



        private void Billadjest()
        {
            try
            {
                Database.BeginTran();
                DataTable dtProduConsum = new DataTable();
                Database.GetSqlData("SELECT 1 AS Expr1 FROM  sys.tables WHERE (name = 'Billadjest')", dtProduConsum);
                if (dtProduConsum.Rows.Count == 0)
                {
                    if (Database.CommandExecutor("create table Billadjest([id] nvarchar(255),[Ac_id] nvarchar(255),[Vi_id] nvarchar(255),[Reff_id] nvarchar(255),Amount money,Nid int,LocationId nvarchar(255), A bit, B bit, AB bit CONSTRAINT BA PRIMARY KEY (id))") == true)
                    {

                    }
                }
                Database.CommitTran();
            }
            catch (Exception ex)
            {
                Database.RollbackTran();
            }
        }



        private void SalesMan()
        {

            try
            {
                Database.BeginTran();
                DataTable dtsalesman = new DataTable();
                Database.GetSqlData("SELECT 1 AS Expr1 FROM  sys.tables WHERE (name = 'Salesman')", dtsalesman);
                if (dtsalesman.Rows.Count == 0)
                {
                    if (Database.CommandExecutor("create table Salesman([S_id] nvarchar(255),Nid int,LocationId nvarchar(255),[Name] nvarchar (255), user_id nvarchar(255),Modifiedby nvarchar(255) CONSTRAINT S1 PRIMARY KEY (S_id))") == true)
                    {
                        Database.CommandExecutor("ALTER TABLE Account ADD  Salesman_id nvarchar(255)");
                    }
                }
                Database.CommitTran();
            }
            catch (Exception ex)
            {
                Database.RollbackTran();
            }
        }


        private void Reffno()
        {
            try
            {
                Database.BeginTran();
                if (Database.CommandExecutor("ALTER TABLE Voucheractotal Add reffno nvarchar(20)") == true)
                {
                    Database.CommandExecutor("update voucheractotal set reffno=vi_id");
                }
               
                Database.CommitTran();
            }
            catch (Exception ex)
            {
                Database.RollbackTran();
            }
        }

        private void updateNarr()
        {
            try
            {
                Database.BeginTran();
                if (Database.CommandExecutor("ALTER TABLE journal Add Opp_acid nvarchar(20) Default ''") == true)
                {
                    Database.CommandExecutor("update journal set Opp_acid=''");
                }
                if (Database.CommandExecutor("ALTER TABLE Vouchertype ADD NarrTemplate nvarchar(255) Default ''") == true)
                {
                    Database.CommandExecutor("UPDATE VOUCHERTYPE SET NarrTemplate = ''");
                    Database.CommandExecutor("UPDATE VOUCHERTYPE SET NarrTemplate = 'Being Goods Sold by {Vouchertype}' where type='Sale' ");
                    Database.CommandExecutor("UPDATE VOUCHERTYPE SET NarrTemplate = 'Being Goods Pendings' where type='Pending' ");
                    Database.CommandExecutor("UPDATE VOUCHERTYPE SET NarrTemplate = 'Being Goods Purchase Bill No.{Svnum} Dt. {Svdate}' where type='Purchase' ");
                    Database.CommandExecutor("UPDATE VOUCHERTYPE SET NarrTemplate = 'Being Goods Purchase Return  Bill No.{Svnum} Dt. {Svdate}' where type='P Return' ");
                    Database.CommandExecutor("UPDATE VOUCHERTYPE SET NarrTemplate = 'Temporary Voucher' where type='Temp' ");
                    Database.CommandExecutor("UPDATE VOUCHERTYPE SET NarrTemplate = 'Being Goods Return' where type='Return' ");
                    Database.CommandExecutor("UPDATE VOUCHERTYPE SET NarrTemplate = 'Being Credit Note issued' where type='Cnote' ");
                    Database.CommandExecutor("UPDATE VOUCHERTYPE SET NarrTemplate = 'Being Debit Note issued' where type='Dnote' ");
                    Database.CommandExecutor("UPDATE VOUCHERTYPE SET NarrTemplate = 'Contra Voucher' where type='Contra' ");
                }
                if (Database.CommandExecutor("ALTER TABLE Journal ADD Narr2 nvarchar(255) Default ''") == true)
                {
                    Database.CommandExecutor("UPDATE Journal SET Narr2 = Narr ");
                }

                Database.CommitTran();
            }
            catch (Exception ex)
            {
                Database.RollbackTran();
            }
        }

        private void updBranch()
        {
            try
            {
                if (Database.CommandExecutor("Alter table Branch Add Godown_id nvarchar(20)") == true)
                {
                    Database.CommandExecutor("update Branch set Godown_id='' where Bname='Main'");
                    Database.CommandExecutor("update Branch set Godown_id='" + funs.Select_ac_id("Indira Market") + "' where Bname='Indra Market'");
                }
            }
            catch (Exception ex)
            {
                Database.RollbackTran();
            }
        }


        private void updBranchAgent()
        {
            try
            {
                if (Database.CommandExecutor("Alter table contractor Add Branch_id nvarchar(20)") == true)
                {
                    Database.CommandExecutor("update contractor set Branch_id='SER'");
                   
                }
            }
            catch (Exception ex)
            {
                Database.RollbackTran();
            }
        }

        private void rateapp2()
        {
            try
            {
                if (Database.CommandExecutor("Alter table Account Add RateApp2 nvarchar(20)") == true)
                {
                    Database.CommandExecutor("update Account set RateApp2=RateApp");
                    Database.CommandExecutor("update Journal set A='False',B='True',AB='True' where sno=10002");
                    Database.CommandExecutor("update Journal set ac_id='MAN1' where ac_id='SER1' and sno=10002");
                    Database.CommandExecutor("update Journal set opp_acid='MAN1' where opp_acid='SER1' and sno=10002");
                }
            }
            catch (Exception ex)
            {
                Database.RollbackTran();
            }
        }
        private void updAgent()
        {
            try
            {
                if (Database.CommandExecutor("Alter table CONTRACTOR Add Reff_id nvarchar(255)") == true)
                {
                }
                if (Database.CommandExecutor("Alter table VOUCHERINFO Add CmsnAmt money") == true)
                {
                    Database.CommandExecutor("update VOUCHERINFO set CmsnAmt=0");
                }
            }
            catch (Exception ex)
            {
                Database.RollbackTran();
            }
        }

        private void updgodownid()
        {
            try
            {
                Database.BeginTran();
                Database.CommandExecutor("update voucherdet set godown_id='' where godown_id='0'");
                Database.CommandExecutor("update voucherdet set godown_id='' where godown_id is null");
                Database.CommandExecutor("update stock set godown_id='' where godown_id='0'");
                Database.CommandExecutor("update stock set godown_id='' where godown_id is null");
                Database.CommitTran();
            }
            catch (Exception ex)
            {
                Database.RollbackTran();
            }
        }

        private void Opening()
        {
            try
            {
                Database.BeginTran();
                int vtnid = Database.GetScalarInt("Select Max(Nid) from Vouchertype")+1;

                if (Database.GetScalarInt("Select count(*) from Vouchertype where Name='Opening Stock'") == 0)
                {
                    Database.CommandExecutor("insert into VOUCHERTYPE (Vt_id,[Name],[Type],[Stationary],[Numtype],[Short],[AliasName],[Default1],[Default2],[Default3],[ReportName],[Code],[Effect_On_Stock],[Effect_On_Acc],[IncludingTax],[ExcludingTax],[ExState],[TaxInvoice],[Unregistered],[Active],[PaperSize],[SmsTemplate],[A],[B],[printcopy],[CashTransaction],[Calculation],[Postfix],[Prefix],[Padding],[Exempted],[VoucCategory],[Ratetype],[Nid]) values('SER" + vtnid.ToString() + "','Opening Stock','Opening'," + access_sql.Singlequote + "true" + access_sql.Singlequote + ",1,'OPN','Opening','Original Copy','Duplicate Copy','Office Copy','GSTTIA4.rpt','OPN','Y','Y'," + access_sql.Singlequote + "true" + access_sql.Singlequote + "," + access_sql.Singlequote + "true" + access_sql.Singlequote + "," + access_sql.Singlequote + "false" + access_sql.Singlequote + "," + access_sql.Singlequote + "true" + access_sql.Singlequote + "," + access_sql.Singlequote + "true" + access_sql.Singlequote + "," + access_sql.Singlequote + "true" + access_sql.Singlequote + ",'A4','Dear Customer, Thanks For Purchasing With Us, Your Last Bill No is: {Vno}'," + access_sql.Singlequote + "true" + access_sql.Singlequote + "," + access_sql.Singlequote + "true" + access_sql.Singlequote + ",'Original for Recipient,True;Duplicate for Transporter,True;Triplicate for Supplier,True;','Not Allowed','Default Excluding Tax','','O-',6,'Allowed','Opening','Purchase_rate'," + vtnid + ")");
                    //Database.CommandExecutor("insert into VOUCHERTYPE (Vt_id,[Name],[Type],[Stationary],[Numtype],[Short],[AliasName],[Default1],[Default2],[Default3],[ReportName],[Code],[Effect_On_Stock],[Effect_On_Acc],[IncludingTax],[ExcludingTax],[ExState],[TaxInvoice],[Unregistered],[Active],[PaperSize],[SmsTemplate],[A],[B],[printcopy],[CashTransaction],[Calculation],[Postfix],[Prefix],[Padding],[Exempted],[VoucCategory],[Ratetype],[Nid],[LocationId]) values('SER" + vtnid.ToString() + "','Opening Stock','Opening'," + access_sql.Singlequote + "true" + access_sql.Singlequote + ",1,'OPN','Opening','Original Copy','Duplicate Copy','Office Copy','GSTTIA4.rpt','OPN','Y','Y'," + access_sql.Singlequote + "true" + access_sql.Singlequote + "," + access_sql.Singlequote + "true" + access_sql.Singlequote + "," + access_sql.Singlequote + "false" + access_sql.Singlequote + "," + access_sql.Singlequote + "true" + access_sql.Singlequote + "," + access_sql.Singlequote + "true" + access_sql.Singlequote + "," + access_sql.Singlequote + "true" + access_sql.Singlequote + ",'A4','Dear Customer, Thanks For Purchasing With Us, Your Last Bill No is: {Vno}'," + access_sql.Singlequote + "true" + access_sql.Singlequote + "," + access_sql.Singlequote + "true" + access_sql.Singlequote + ",'Original for Recipient,True;Duplicate for Transporter,True;Triplicate for Supplier,True;','Not Allowed','Default Excluding Tax','','O-',6,'Allowed','Opening','Purchase_rate',"+vtnid+",'"+Database.LocationId+"')");
                }
                Database.CommitTran();
            }
            catch (Exception ex)
            {
                Database.RollbackTran();
            }
        }

        private void OpeningRename()
        {
            try
            {
                Database.BeginTran();
               
                if (Database.GetScalarInt("Select count(*) from Vouchertype where Name='Opening Stock' and A='false'") == 1)
                {
                    Database.CommandExecutor("update Vouchertype set Name='Opening Stock K' where Name='Opening Stock' ");
                   // Database.CommandExecutor("insert into VOUCHERTYPE (Vt_id,[Name],[Type],[Stationary],[Numtype],[Short],[AliasName],[Default1],[Default2],[Default3],[ReportName],[Code],[Effect_On_Stock],[Effect_On_Acc],[IncludingTax],[ExcludingTax],[ExState],[TaxInvoice],[Unregistered],[Active],[PaperSize],[SmsTemplate],[A],[B],[printcopy],[CashTransaction],[Calculation],[Postfix],[Prefix],[Padding],[Exempted],[VoucCategory],[Ratetype],[Nid]) values('SER" + vtnid.ToString() + "','Opening Stock','Opening'," + access_sql.Singlequote + "true" + access_sql.Singlequote + ",1,'OPN','Opening','Original Copy','Duplicate Copy','Office Copy','GSTTIA4.rpt','OPN','Y','Y'," + access_sql.Singlequote + "true" + access_sql.Singlequote + "," + access_sql.Singlequote + "true" + access_sql.Singlequote + "," + access_sql.Singlequote + "false" + access_sql.Singlequote + "," + access_sql.Singlequote + "true" + access_sql.Singlequote + "," + access_sql.Singlequote + "true" + access_sql.Singlequote + "," + access_sql.Singlequote + "true" + access_sql.Singlequote + ",'A4','Dear Customer, Thanks For Purchasing With Us, Your Last Bill No is: {Vno}'," + access_sql.Singlequote + "true" + access_sql.Singlequote + "," + access_sql.Singlequote + "true" + access_sql.Singlequote + ",'Original for Recipient,True;Duplicate for Transporter,True;Triplicate for Supplier,True;','Not Allowed','Default Excluding Tax','','O-',6,'Allowed','Opening','Purchase_rate'," + vtnid + ")");
                   
                }
                Database.CommitTran();
            }
            catch (Exception ex)
            {
                Database.RollbackTran();
            }
        }
        private void OpeningA()
        {
            try
            {
                Database.BeginTran();
                int vtnid = Database.GetScalarInt("Select Max(Nid) from Vouchertype") + 1;

                if (Database.GetScalarInt("Select count(*) from Vouchertype where Name='Opening Stock' and A='True'") == 0)
                {
                    
                    Database.CommandExecutor("insert into VOUCHERTYPE (Vt_id,[Name],[Type],[Stationary],[Numtype],[Short],[AliasName],[Default1],[Default2],[Default3],[ReportName],[Code],[Effect_On_Stock],[Effect_On_Acc],[IncludingTax],[ExcludingTax],[ExState],[TaxInvoice],[Unregistered],[Active],[PaperSize],[SmsTemplate],[A],[B],[printcopy],[CashTransaction],[Calculation],[Postfix],[Prefix],[Padding],[Exempted],[VoucCategory],[Ratetype],[Nid],[locationid],[Allowed0Val],[NarrTemplate],[AB]) values('SER" + vtnid.ToString() + "','Opening Stock','Opening','true',1,'OPN','Opening','Original Copy','Duplicate Copy','Office Copy','GSTTIA4.rpt','OPN','Y','Y'," + access_sql.Singlequote + "true" + access_sql.Singlequote + "," + access_sql.Singlequote + "true" + access_sql.Singlequote + "," + access_sql.Singlequote + "false" + access_sql.Singlequote + "," + access_sql.Singlequote + "true" + access_sql.Singlequote + "," + access_sql.Singlequote + "true" + access_sql.Singlequote + "," + access_sql.Singlequote + "true" + access_sql.Singlequote + ",'A4','Dear Customer, Thanks For Purchasing With Us, Your Last Bill No is: {Vno}'," + access_sql.Singlequote + "true" + access_sql.Singlequote + "," + access_sql.Singlequote + "false" + access_sql.Singlequote + ",'Original for Recipient,True;Duplicate for Transporter,True;Triplicate for Supplier,True;','Not Allowed','Default Excluding Tax','','O-',6,'Allowed','Opening','Purchase_rate'," + vtnid + ",'SER','Not Allowed','','True')");

                   
                }
                Database.CommitTran();
            }
            catch (Exception ex)
            {
                Database.RollbackTran();
            }
        }

        private void RemarkText()
        {
            try
            {
                Database.BeginTran();



               // AddFeatureGST("Transaction", "Sms Sent to MobileNo/PhoneNo", "Sms Sent to MobileNo/PhoneNo", false, false, "Phone;Mobile;", "Mobile", "ComboBox");
                AddFeatureNew("Transaction", "Sms Sent to MobileNo/PhoneNo", "Sms Sent to MobileNo/PhoneNo", true, false, "Phone;Mobile;", "Mobile", "Mobile", "Mobile", "Mobile", "Mobile", "ComboBox");
                AddFeatureNew("Transaction", "Export Vouchers in Tally", "Export Vouchers in Tally", true, false, "All;CurrentBranch;", "All", "All", "All", "All", "All", "ComboBox");
             
              //  AddFeatureGST("Transaction", "Show Text on Remark1", "Show Text on Remark1", false, false, "Remark1", "Remark1", "Textbox");
               
                Database.CommitTran();
            }
            catch (Exception ex)
            {
                Database.RollbackTran();
            }
        }

        private void Updatevoucherinfonstock()
        {
            //try
            //{               
            //    Database.BeginTran();
            //    if (Database.DatabaseType == "sql")
            //    {
            //        //dirct run in database
            //        Database.CommandExecutor("ALTER TABLE Voucherinfo ALTER COLUMN Branch_id INT NOT NULL");
            //        Database.CommandExecutor("ALTER TABLE Stock ALTER COLUMN Branch_id INT NOT NULL");
            //    }
            //    Database.CommitTran();
            //}
            //catch (Exception ex)
            //{
            //    Database.RollbackTran();
            //}
        }

        private void CashCredit()
        {
            try
            {
                Database.BeginTran();
                if (Database.DatabaseType == "sql")
                {
                    if (Database.CommandExecutor("ALTER TABLE Voucherinfo ADD CashCredit nvarchar(255)") == true)
                    {
                        Database.CommandExecutor("Update Voucherinfo set CashCredit='Credit'");
                    }
                    if (Database.CommandExecutor("ALTER TABLE Description ADD Status nvarchar(255)") == true)
                    {
                        Database.CommandExecutor("Update Description set Status='Enable'");
                    }
                }
                Database.CommitTran();
            }
            catch (Exception ex)
            {
                Database.RollbackTran();
            }
        }



        private void by()
        {
            try
            {
                Database.BeginTran();
                if (Database.DatabaseType == "sql")
                {
                    if (Database.CommandExecutor("ALTER TABLE Voucherinfo ADD Modifiedby nvarchar(255)") == true)
                    {
                        Database.CommandExecutor("Update Voucherinfo set Modifiedby=''");
                    }
                    if (Database.CommandExecutor("ALTER TABLE Voucherinfo ADD ApprovedBy  nvarchar(255)") == true)
                    {
                        Database.CommandExecutor("Update Voucherinfo set ApprovedBy=''");
                    }

                    if (Database.CommandExecutor("ALTER TABLE Vouchertype ADD Allowed0Val  nvarchar(255)") == true)
                    {
                        Database.CommandExecutor("Update Vouchertype set Allowed0Val='Not Allowed'");
                    }
                }
                Database.CommitTran();
            }
            catch (Exception ex)
            {
                Database.RollbackTran();
            }
        }


        private void UpdateCom()
        {
            try
            {
                Database.BeginTran();



                Database.CommandExecutor("update Other set LocationId='SER' where Locationid is null");
                Database.CommandExecutor("UPDATE OTHER SET  Oth_id = 'SER' + CAST(Nid AS nvarchar) WHERE  (Oth_id IS NULL)");
                Database.CommitTran();
               
            }
            catch (Exception ex)
            {
                Database.RollbackTran();
            }
        }



        private void ModeAB()
        {
            try
            {
                Database.BeginTran();
                if (Database.DatabaseType == "sql")
                {
                    Database.CommandExecutor("Update SideMenu set Display='true' where MenuOption='Godown Stock'");
                    if (Database.CommandExecutor("ALTER TABLE Vouchertype ADD AB bit") == true)
                    {
                        Database.CommandExecutor("Update SideMenu set Display='False' where MenuOption='Godown Stock'");
                        Database.CommandExecutor("update Account set Balance2=Balance2-Balance");
                        Database.CommandExecutor("update Account set Closing_Bal2= CAST(Closing_Bal2 AS float)  - CAST(Closing_Bal AS float)");
                        Database.CommandExecutor("Update Vouchertype set AB='True'");
                        Database.CommandExecutor("Update Vouchertype set B='True' where A='False'");
                        Database.CommandExecutor("Update Vouchertype set B='False' where A='True'");
                    }
                    
                }
                Database.CommitTran();
            }
            catch (Exception ex)
            {
                Database.RollbackTran();
            }
        }

        private void Conversioninttostring()
        {
            //try
            //{

            //    Database.BeginTran();
            //    if (Database.DatabaseType == "sql")
            //    {
                 
            //        Database.CommandExecutor("ALTER TABLE Account ALTER COLUMN Ac_id nvarchar(20)");
            //        Database.CommandExecutor("ALTER TABLE Account ALTER COLUMN Act_id nvarchar(20)");
            //        Database.CommandExecutor("ALTER TABLE Account ALTER COLUMN Loc_id nvarchar(20)");
            //        Database.CommandExecutor("ALTER TABLE Account ALTER COLUMN Con_id nvarchar(20)");
            //        Database.CommandExecutor("ALTER TABLE Account ALTER COLUMN State_id nvarchar(20)");
            //        Database.CommandExecutor("ALTER TABLE Account ALTER COLUMN Branch_id nvarchar(20)");
            //    }
            //    Database.CommitTran();
            //}
            //catch (Exception ex)
            //{
            //    Database.RollbackTran();
            //}
        }

        private void Updateexe()
        {
            try
            {
                Database.BeginTran();
                if (Database.GetScalarInt("select count(*) from SoftUpdates where [Update]='Update Upto'") == 0)
                {
                    Database.CommandExecutor("insert into SoftUpdates values( " + access_sql.Hash + Database.ExeDate.ToString("dd-MMM-yyyy") + access_sql.Hash + ",'1','Update Upto','UpDated Exe')");
                }
                else
                {
                    Database.CommandExecutor("UPDATE SoftUpdates SET [Date] = " + access_sql.Hash + Database.ExeDate.ToString("dd-MMM-yyyy") + access_sql.Hash + " WHERE [Update]='Update Upto'");
                }

                Database.CommitTran();
            }
            catch (Exception ex)
            {
                Database.RollbackTran();
            }
        }
      
        //multilocation
        private void AddBranch()
        {
            try
            {
                Database.BeginTran();
                if (Database.DatabaseType == "access")
                {
                    if (Database.CommandExecutor("create table Branch ([id] AUTOINCREMENT, [Bname] text (255))") == true)
                    {
                        Database.CommandExecutor("Alter table Branch Add Primary Key (Bname)");
                        Database.CommandExecutor("insert into Branch (Bname) values('Main')");
                    }
                    if (Database.CommandExecutor("ALTER TABLE Userinfo ADD Column Branch_id number") == true)
                    {
                        Database.CommandExecutor("ALTER TABLE Voucherinfo ADD Column Branch_id number");
                        Database.CommandExecutor("ALTER TABLE Stock ADD Column Branch_id number");
                        Database.CommandExecutor("ALTER TABLE Account ADD Column Branch_id number");
                        Database.CommandExecutor("update Userinfo set Branch_id=1");
                        Database.CommandExecutor("update Voucherinfo set Branch_id=1");
                        Database.CommandExecutor("update Account set Branch_id=1");
                        Database.CommandExecutor("update Stock set Branch_id=1");
                    }
                }
                else
                {
                    DataTable dtbranch = new DataTable();
                    Database.GetSqlData("SELECT 1 AS Expr1 FROM  sys.tables WHERE (name = 'Branch')", dtbranch);
                    if (dtbranch.Rows.Count == 0)
                    {
                        if (Database.CommandExecutor("create table Branch ([id] int Identity, [Bname] nvarchar (255)  CONSTRAINT bk_branch PRIMARY KEY(Bname))") == true)
                        {
                            Database.CommandExecutor("insert into Branch (Bname) values('Main')");
                        }
                    }
                    if (Database.CommandExecutor("ALTER TABLE Userinfo ADD  Branch_id int") == true)
                    {
                        Database.CommandExecutor("ALTER TABLE Voucherinfo ADD  Branch_id int");
                        Database.CommandExecutor("ALTER TABLE Account ADD  Branch_id int");
                        Database.CommandExecutor("ALTER TABLE Stock ADD  Branch_id int");
                        Database.CommandExecutor("update Userinfo set Branch_id=1");
                        Database.CommandExecutor("update Voucherinfo set Branch_id=1");
                        Database.CommandExecutor("update Account set Branch_id=1");
                        Database.CommandExecutor("update Stock set Branch_id=1");
                    }
                }
                Database.CommitTran();
            }
            catch (Exception ex)
            {
                Database.RollbackTran();
            }
        }


        private void AddRebate()
        {
            try
            {
                Database.BeginTran();
              
                    DataTable dtbranch = new DataTable();
                    Database.GetSqlData("SELECT 1 AS Expr1 FROM  sys.tables WHERE (name = 'Rebate')", dtbranch);
                    if (dtbranch.Rows.Count == 0)
                    {
                        if (Database.CommandExecutor("create table Rebate ([id] int Identity, [Companyid] nvarchar (255), [Itemid] nvarchar (255), [Acid] nvarchar (255),dis1 money,dis2 money,dis3 money  CONSTRAINT bk_rebate PRIMARY KEY(Companyid,ItemId,Acid))") == true)
                        {
                            Database.CommandExecutor("insert into Winpage (PageName,PageTitle,PArentPageid,Feature,keyvalue) values ('rebateToolStripMenuItem','RebateSet',21,'','')");
                        }
                    }
                   
                   
                Database.CommitTran();
            }
            catch (Exception ex)
            {
                Database.RollbackTran();
            }
        }


        private void winpage()
        {
            try
            {
                Database.BeginTran();

                DataTable dtbranch = new DataTable();
                Database.GetSqlData("SELECT * from  Winpage  WHERE (PageName = 'grToolStripMenuItem')", dtbranch);
                if (dtbranch.Rows.Count == 0)
                {

                    Database.CommandExecutor("insert into Winpage (PageName,PageTitle,PArentPageid,Feature,keyvalue) values ('grToolStripMenuItem','Grade Wise',120,'','')");
                    
                }


                Database.CommitTran();
            }
            catch (Exception ex)
            {
                Database.RollbackTran();
            }
        }



        private void winpageroleid()
        {
            try
            {
                Database.BeginTran();

                DataTable dtbranch = new DataTable();
                Database.GetSqlData("SELECT *  FROM  Winpage WHERE (PageName = 'grToolStripMenuItem')", dtbranch);
                if (dtbranch.Rows.Count == 1)
                {
                    int pageid = int.Parse(dtbranch.Rows[0]["pageid"].ToString());

                    Database.CommandExecutor("insert into winpagerole (Roleid,Pageid,Visible,Feature) values(1," + pageid + ",'true','')");

                }


                Database.CommitTran();
            }
            catch (Exception ex)
            {
                Database.RollbackTran();
            }
        }



        private void winpageaccsale()
        {
            try
            {
                Database.BeginTran();

                DataTable dtbranch = new DataTable();
                Database.GetSqlData("SELECT * from  Winpage  WHERE (PageName = 'accountantSaleToolStripMenuItem')", dtbranch);
                if (dtbranch.Rows.Count == 0)
                {

                    Database.CommandExecutor("insert into Winpage (PageName,PageTitle,PArentPageid,Feature,keyvalue) values ('accountantSaleToolStripMenuItem','Accountant Sale',120,'','')");

                }


                Database.CommitTran();
            }
            catch (Exception ex)
            {
                Database.RollbackTran();
            }
        }



        private void winpageroleidaccsale()
        {
            try
            {
                Database.BeginTran();

                DataTable dtbranch = new DataTable();
                Database.GetSqlData("SELECT *  FROM  Winpage WHERE (PageName = 'accountantSaleToolStripMenuItem')", dtbranch);
                if (dtbranch.Rows.Count == 1)
                {
                    int pageid = int.Parse(dtbranch.Rows[0]["pageid"].ToString());

                    Database.CommandExecutor("insert into winpagerole (Roleid,Pageid,Visible,Feature) values(1," + pageid + ",'true','')");

                }


                Database.CommitTran();
            }
            catch (Exception ex)
            {
                Database.RollbackTran();
            }
        }



        private void winpagecity()
        {
            try
            {
                Database.BeginTran();

                DataTable dtbranch = new DataTable();
                Database.GetSqlData("SELECT * from  Winpage  WHERE (PageName = 'cityToolStripMenuItem')", dtbranch);
                if (dtbranch.Rows.Count == 0)
                {

                    Database.CommandExecutor("insert into Winpage (PageName,PageTitle,PArentPageid,Feature,keyvalue) values ('cityToolStripMenuItem','City',154,'','City')");

                }


                Database.CommitTran();
            }
            catch (Exception ex)
            {
                Database.RollbackTran();
            }
        }



        private void winpageroleidcity()
        {
            try
            {
                Database.BeginTran();

                DataTable dtbranch = new DataTable();
                Database.GetSqlData("SELECT *  FROM  Winpage WHERE (PageName = 'cityToolStripMenuItem')", dtbranch);
                if (dtbranch.Rows.Count == 1)
                {
                    int pageid = int.Parse(dtbranch.Rows[0]["pageid"].ToString());

                    Database.CommandExecutor("insert into winpagerole (Roleid,Pageid,Visible,Feature) values(1," + pageid + ",'true','')");

                }


                Database.CommitTran();
            }
            catch (Exception ex)
            {
                Database.RollbackTran();
            }
        }

        private void Addpageroleid()
        {
            try
            {
                Database.BeginTran();

                DataTable dtbranch = new DataTable();
                Database.GetSqlData("SELECT *  FROM  Winpage WHERE (PageName = 'rebateToolStripMenuItem')", dtbranch);
                if (dtbranch.Rows.Count == 1)
                {
                    int pageid = int.Parse(dtbranch.Rows[0]["pageid"].ToString());

                    Database.CommandExecutor("insert into winpagerole (Roleid,Pageid,Visible,Feature) values(1," + pageid + ",'true','')");

                }


                Database.CommitTran();
            }
            catch (Exception ex)
            {
                Database.RollbackTran();
            }
        }




        private void InsertAcctype()
        {
            try
            {
                Database.BeginTran();
                if (Database.DatabaseType == "access")
                {

                    if (Database.GetScalarInt("Select count(*) from Accountype where Name='LOAN (LIABILITIES)'") == 0)
                    {
                        Database.CommandExecutor("Insert into Accountype (Name,Type,RefineName,Nature,Under,Fixed,[level],Sequence) values('LOAN (LIABILITIES)','Account','LOAN (LIABILITIES)','L',0,True,1,2)");
                    }
                }
                else
                {
                    if (Database.GetScalarInt("Select count(*) from Accountype where Name='LOAN (LIABILITIES)'") == 0)
                    {
                        Database.CommandExecutor("Insert into Accountype (Name,Type,RefineName,Nature,Under,Fixed,[level],Sequence) values('LOAN (LIABILITIES)','Account','LOAN (LIABILITIES)','L',0,'True',1,2)");
                    }
                }
                Database.CommitTran();
            }
            catch (Exception ex)
            {
                Database.RollbackTran();
            }
        }       

        private void RCMAccount()
        {
            try
            {
                Database.BeginTran();
                
                if (Database.CommandExecutor("insert into Account (Act_id, Name, Address1, Address2, Phone, Email, Tin_number, Loc_id, Dr, Cr, Blimit, Dlimit, Con_id,[note], Dr2, Cr2, Closing_Bal, Closing_Bal2, PAN, state,Printname, State_id, Status, AllowPS, Aadhaarno, Balance, Balance2, RegStatus) values(12,'RCM Payable','None','None','0','None','0',0,0,0,0,0,0,'',0,0,'0','0','','Added','RCM Payable',0," + access_sql.Singlequote + "true" + access_sql.Singlequote + "," + access_sql.Singlequote + "false" + access_sql.Singlequote + ",'',0,0,'Unregistered') ") == true)
                {
                    Database.CommandExecutor("insert into Account (Act_id, Name, Address1, Address2, Phone, Email, Tin_number, Loc_id, Dr, Cr, Blimit, Dlimit, Con_id, [note], Dr2, Cr2, Closing_Bal, Closing_Bal2, PAN, state,Printname, State_id, Status, AllowPS, Aadhaarno, Balance, Balance2, RegStatus) values(12,'RCM Eligible ITC','None','None','0','None','0',0,0,0,0,0,0,'',0,0,'0','0','','Added','RCM Eligible ITC',0," + access_sql.Singlequote + "true" + access_sql.Singlequote + "," + access_sql.Singlequote + "false" + access_sql.Singlequote + ",'',0,0,'Unregistered') ");
                    Database.CommandExecutor("insert into Account (Act_id, Name, Address1, Address2, Phone, Email, Tin_number, Loc_id, Dr, Cr, Blimit, Dlimit, Con_id, [note], Dr2, Cr2, Closing_Bal, Closing_Bal2, PAN, state,Printname, State_id, Status, AllowPS, Aadhaarno, Balance, Balance2, RegStatus) values(6,'RCM Ineligible ITC','None','None','0','None','0',0,0,0,0,0,0,'',0,0,'0','0','','Added','RCM Ineligible ITC',0," + access_sql.Singlequote + "true" + access_sql.Singlequote + "," + access_sql.Singlequote + "false" + access_sql.Singlequote + ",'',0,0,'Unregistered') ");

                    int rcmpayac_id = Database.GetScalarInt("Select Ac_id from Account where Name='RCM Payable'");
                    int rcmitcac_id = Database.GetScalarInt("Select Ac_id from Account where Name='RCM Eligible ITC'");
                    int rcmeliac_id = Database.GetScalarInt("Select Ac_id from Account where Name='RCM Ineligible ITC'");
                    Database.CommandExecutor("Update taxcategory set RCMPay=" + rcmpayac_id + ",RCMITC=" + rcmitcac_id + ",RCMEli=" + rcmeliac_id);
                }

                if (Database.DatabaseType == "access")
                {
                    if (Database.CommandExecutor("ALTER TABLE Voucherdet ADD Column RCMac_id number") == true)
                    {
                        Database.CommandExecutor("update Voucherdet set RCMac_id=0");
                    }
                }
                else
                {
                    if (Database.CommandExecutor("ALTER TABLE Voucherdet ADD RCMac_id int") == true)
                    {
                        Database.CommandExecutor("update Voucherdet set RCMac_id=0");
                    }
                }

                Database.CommitTran();
            }
            catch (Exception ex)
            {
                Database.RollbackTran();
            }
        }


        private bool AddFeatureNew(String Grp, String Features, String Description, Boolean Default, Boolean disabled, string OptionValue, string TSA, string TA, string TSU, string TU, string TC, string gType)
        {
            try
            {
                String Str = "";

                if (Database.DatabaseType == "sql")
                {
                    Str = "select count(Features) as cnt from [dbo].[FirmSetup] WHERE CONVERT(VARCHAR(255), Features) = '" + Features + "' ";
                }

                if (Database.GetScalarInt(Str) == 0)
                {
                    if (Database.DatabaseType == "sql")
                    {
                        Str = "INSERT INTO FirmSetup ([Group],[Features],[Description],[Active],[Disabled],[OptionValues],[ToSuperAdmin],[ToAdmin],[ToSuperUser],[ToUser],[ToCashier],[Type]) values('" + Grp + "', '" + Features + "','" + Description + "','" + Default + "','" + disabled + "','" + OptionValue + "','" + TSA + "','" + TA + "','" + TSU + "','" + TU + "','" + TC + "','" + gType + "')";
                    }
                    Database.CommandExecutor(Str);
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }
       
    
        private bool AddFeatureGST(String Grp, String Features, String Description, Boolean Default, Boolean disabled, string OptionValue, string selected, string gType)
        {
            try
            {
                String Str = "";

                if (Database.DatabaseType == "access")
                {
                    Str = "select count(Features) as cnt from firmsetup where [Features]='" + Features + "'";
                }
                else if (Database.DatabaseType == "sql")
                {
                    Str = "select count(Features) as cnt from [dbo].[FirmSetup] WHERE CONVERT(VARCHAR(255), Features) = '" + Features + "' ";
                }

                if (Database.GetScalarInt(Str) == 0)
                {
                    if (Database.DatabaseType == "access")
                    {
                        Str = "INSERT INTO FirmSetup ([Group],[Features],[Description],[Active],[Disabled],[OptionValues],[selected_value],[Type]) values('" + Grp + "', '" + Features + "','" + Description + "'," + Default + "," + disabled + ",'" + OptionValue + "','" + selected + "','" + gType + "')";
                    }
                    else if (Database.DatabaseType == "sql")
                    {
                        Str = "INSERT INTO FirmSetup ([Group],[Features],[Description],[Active],[Disabled],[OptionValues],[selected_value],[Type]) values('" + Grp + "', '" + Features + "','" + Description + "','" + Default + "','" + disabled + "','" + OptionValue + "','" + selected + "','" + gType + "')";
                    }

                    Database.CommandExecutor(Str);
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        private void upd19Mar()
        {
            DataTable dtCount = new DataTable();
            Database.GetSqlData("select count(*) from Accountype where locationid='" + Database.LocationId + "'", dtCount);

            int nid = 0;

            if (int.Parse(dtCount.Rows[0][0].ToString()) == 0)
            {
                nid = 1;
            }
            else
            {
                DataTable dtid = new DataTable();
                Database.GetSqlData("select max(Nid) as Nid from Accountype where locationid='" + Database.LocationId + "'", dtid);
                nid = int.Parse(dtid.Rows[0][0].ToString()) + 1;
            }

            string actid = Database.LocationId + nid.ToString();
            if (Database.GetScalarInt("Select count(*) from Accountype where Name='AGENT'") == 0)
            {
                Database.CommandExecutor("Insert into Accountype (Act_id,Name,Type,RefineName,Nature,Under,Fixed,[level],Sequence,LocationId,Nid,Regsqn,[Path]) values('" + actid + "','AGENT','Account','AGENT','A','SER39','False',3,0,'" + Database.LocationId + "'," + nid + ",2,'1;39;" + nid + ";')");
            }
        }
     
        private void ansGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void openingStock()
        {
            try
            {
                DataTable dtstock = new DataTable();
                
                Database.GetSqlData("select * from stock where vid='0'", dtstock);
                
                DataTable dt1 = dtstock.DefaultView.ToTable(true, "godown_id");

                
                
                for (int i = 0; i < dt1.Rows.Count; i++)
                {

                    DataTable dtVoucherInfo = new DataTable("Voucherinfo");
                    Database.GetSqlData("select * from Voucherinfo where Vi_id='0'", dtVoucherInfo);

                    DataTable dtVoucherDet = new DataTable("VOUCHERDET");
                    Database.GetSqlData("select * from VOUCHERDET where vi_id='0'", dtVoucherDet);

                    int nid = 0;
                    string vid = "";
                    string vtid = funs.Select_vt_id_vnm("Opening Stock");
                    DateTime dt = Database.stDate.AddDays(-1);

                    DataTable dtCount = new DataTable();
                    Database.GetSqlData("select count(*) from VOUCHERINFO where locationid='" + Database.LocationId + "'", dtCount);
                    if (int.Parse(dtCount.Rows[0][0].ToString()) == 0)
                    {
                        nid = 1;
                    }
                    else
                    {
                        DataTable dtid = new DataTable();
                        Database.GetSqlData("select max(Nid) as Nid from VOUCHERINFO where locationid='" + Database.LocationId + "'", dtid);
                        nid = int.Parse(dtid.Rows[0][0].ToString())+1;
                    }
                    
                    
                    DataTable dtitems = dtstock.Select("godown_id='" + dt1.Rows[i][0].ToString() + "'").CopyToDataTable();
                    double Vamt = double.Parse(dtitems.Compute("sum(ReceiveAmt)", "").ToString());
                    int vno = i+1;
                    
                    vid = Database.LocationId + nid.ToString();
                    //voucherinfo
                    
                        dtVoucherInfo.Rows.Add();
                    

                    dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count-1]["Vi_id"] = vid;
                    dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count-1]["Nid"] = nid;
                    dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count - 1]["LocationId"] = dtitems.Rows[0]["LocationId"].ToString();

                    string prefix = "";
                    string postfix = "";
                    int padding = 0;
                    prefix = Database.GetScalarText("Select prefix from Vouchertype where vt_id='" + vtid + "' ");
                    postfix = Database.GetScalarText("Select postfix from Vouchertype where vt_id='" + vtid + "' ");
                    padding = Database.GetScalarInt("Select padding from Vouchertype where vt_id='" + vtid + "' ");

                    string invoiceno = vno.ToString();
                    dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count-1]["Invoiceno"] = prefix + invoiceno.PadLeft(padding, '0') + postfix;
                    dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count-1]["Vt_id"] = vtid;
                    dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count-1]["Vnumber"] = vno;
                    dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count-1]["user_id"] = Database.user_id;
                    dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count-1]["ac_id"] = dt1.Rows[i][0].ToString();
                    dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count-1]["Ac_id2"] = "0";
                    dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count-1]["Vdate"] = dt.ToString();
                    dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count-1]["Svdate"] = Database.ldate.Date.ToString("dd-MMM-yyyy");
                    dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count-1]["Duedate"] = Database.ldate.Date.ToString("dd-MMM-yyyy");
                    dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count-1]["Narr"] = "Opening Stock";
                    dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count-1]["Reffno"] = "";
                    dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count-1]["TaxableAmount"] = Vamt.ToString();
                    dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count-1]["Totalamount"] = Vamt.ToString(); 
                    dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count-1]["rate"] = 0;
                    dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count-1]["Roff"] = 0;
                    dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count-1]["Tdtype"] = "true";
                    dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count-1]["DirectChanged"] = "false";
                    dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count - 1]["Branch_id"] = dtitems.Rows[0]["Branch_id"].ToString();
                    dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count-1]["RCM"] = false;
                    dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count-1]["5000Allowed"] = false;
                    dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count-1]["ITC"] = false;
                    dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count-1]["RoffChanged"] = "false";
                    dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count-1]["TaxChanged"] = "false";
                    dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count-1]["Svnum"] = "0";
                    dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count-1]["Transport1"] = "";
                    dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count-1]["Transport2"] = "";
                    dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count-1]["Grno"] = "";
                    dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count-1]["DeliveryAt"] = "";
                    dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count-1]["Transport3"] = "";
                    dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count-1]["Transport4"] = "";
                    dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count-1]["Transport5"] = "";
                    dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count-1]["Transport6"] = "";
                    dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count-1]["ShiptoAddress1"] = "";
                    dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count-1]["ShiptoAddress2"] = "";
                    dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count-1]["ShiptoEmail"] = "";
                    dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count-1]["ShiptoTIN"] = "";
                    dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count-1]["ShiptoPhone"] = "";
                    dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count-1]["ShiptoStateid"] = "0";
                    dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count-1]["Shipto"] = "";
                    dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count-1]["ShiptoPAN"] = "";
                    dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count-1]["ShiptoAadhar"] = "";
                    dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count-1]["FormC"] = "false";
                    dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count-1]["Conn_id"] = "0";
                    dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count-1]["Iscancel"] = false;
                    dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count-1]["CreTime"] = System.DateTime.Now.ToString("HH:mm:ss");
                    dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count-1]["ModTime"] = System.DateTime.Now.ToString("HH:mm:ss");
                    if (Database.utype.ToUpper() == "USER")
                    {
                        dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count-1]["NApproval"] = true;
                    }
                    else
                    {
                        dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count-1]["NApproval"] = false;
                    }
                    dtVoucherInfo.Rows[dtVoucherInfo.Rows.Count - 1]["Cash_Pending"] = false;

                   
                    //voucherDetails
                    Feature.Available("Type of Discount1");
                    string qdtype = Feature.Available("Type of Discount1");
                    string cdtype = Feature.Available("Type of Discount2");
                    string fdtype = Feature.Available("Type of Discount3");
                    

                    for (int j = 0; j < dtitems.Rows.Count; j++)
                    {
                        DataTable dtdes = new DataTable();
                        Database.GetSqlData("select * from Description where Des_id='" + dtitems.Rows[j]["Did"].ToString() + "'", dtdes);

                        DataTable dttax = new DataTable();
                        Database.GetSqlData("select * from taxcategory where Category_id='" + dtdes.Rows[0]["Tax_Cat_id"].ToString() + "'", dttax);

                        dtVoucherDet.Rows.Add();
                        dtVoucherDet.Rows[dtVoucherDet.Rows.Count-1]["vi_id"] = vid;
                        dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["LocationId"] = dtitems.Rows[j]["LocationId"].ToString();
                        dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["Itemsr"] = j + 1;
                        dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["Description"] = funs.Select_des_nm(dtitems.Rows[j]["Did"].ToString());
                        dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["Quantity"] = dtitems.Rows[j]["Receive"].ToString();
                        dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["comqty"] = dtitems.Rows[j]["Receive"].ToString();
                        decimal rate = 0;
                        if (decimal.Parse((dtitems.Rows[j]["ReceiveAmt"].ToString())) != 0)
                        {
                            rate = decimal.Parse((dtitems.Rows[j]["ReceiveAmt"].ToString())) / decimal.Parse((dtitems.Rows[j]["Receive"].ToString()));
                        }
                        dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["Rate_am"] = rate;
                        dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["Amount"] = dtitems.Rows[j]["ReceiveAmt"].ToString();
                        dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["Des_ac_id"] = dtitems.Rows[j]["Did"].ToString();
                        dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["Category_Id"] = dtdes.Rows[0]["Tax_Cat_id"].ToString();
                        dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["Taxabelamount"] = dtitems.Rows[j]["ReceiveAmt"].ToString();
                        dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["Rvi_id"] = "0";
                        dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["RItemsr"] = "0";
                        dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["Batch_Code"] = "";
                        dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["Commission%"] = "0";
                        dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["packing"] = dtdes.Rows[0]["Pack"].ToString();
                        dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["orgpacking"] = dtdes.Rows[0]["Pack"].ToString();
                        dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["pvalue"] = dtdes.Rows[0]["Pvalue"].ToString();
                        dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["Rate_unit"] = dtdes.Rows[0]["Rate_Unit"].ToString();
                        dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["remark1"] = "";                       
                        dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["remark2"] = "";
                        dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["remark3"] = "";
                        dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["remark4"] = "";
                        dtVoucherDet.Rows[dtVoucherDet.Rows.Count-1]["remarkreq"] = "false";
                        dtVoucherDet.Rows[dtVoucherDet.Rows.Count-1]["Type"] = 0;
                        dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["flatdis"] = 0;
                        dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["godown_id"] = dt1.Rows[i][0].ToString();                       
                        dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["qd"] = 0;
                        dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["cd"] = 0;
                        dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["dattype"] = "";
                        dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["datamount"] = 0;
                        dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["dat"] = 0;                        
                        dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["datac_id"] = "0";                        
                        dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["RCMac_id"] = "0";
                        dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["weight"] = 0;
                        dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["Cost"] = rate;
                        dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["MRP"] = dtdes.Rows[0]["MRP"];                        
                        dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["Commission@"] = 0;

                        //new fields
                        dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["pur_sale_acc"] = dttax.Rows[0]["PA"];
                        dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["tax1"] = 0;
                        dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["tax2"] = 0;
                        dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["tax3"] = 0;
                        dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["tax4"] = 0;
                        dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["rate1"] = 0;
                        dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["rate2"] = 0;
                        dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["rate3"] = 0;
                        dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["rate4"] = 0;
                        dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["taxamt1"] = 0;
                        dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["taxamt2"] = 0;
                        dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["taxamt3"] = 0;
                        dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["taxamt4"] = 0;
                        dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["bottomdis"] = 0;
                        dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["Amount0"] = dtitems.Rows[j]["ReceiveAmt"].ToString();
                        dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["QDType"] = qdtype;
                        dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["QDAmount"] = 0;
                        dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["Amount1"] = dtitems.Rows[j]["ReceiveAmt"].ToString();
                        dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["CDType"] = cdtype;
                        dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["CDAmount"] = 0;
                        dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["Amount2"] = dtitems.Rows[j]["ReceiveAmt"].ToString();
                        dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["FDType"] = fdtype;
                        dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["FDAmount"] = 0;
                        dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["Amount3"] = dtitems.Rows[j]["ReceiveAmt"].ToString();
                        dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["GridDis"] = 0;
                        dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["TotalDis"] = 0;
                        dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["Amount4"] = dtitems.Rows[j]["ReceiveAmt"].ToString();
                        dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["TotTaxPer"] = 0;
                        dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["TotTaxAmount"] = 0;
                        dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["Amount5"] = dtitems.Rows[j]["ReceiveAmt"].ToString();
                        dtVoucherDet.Rows[dtVoucherDet.Rows.Count - 1]["ExpAmount"] = 0;

                    }

                    Database.SaveData(dtVoucherInfo);
                    Database.SaveData(dtVoucherDet);


                    //DataTable dtfinal = new DataTable("Stock");
                    //Database.GetSqlData("select * from Stock where vid='0' and godown_id='" + dt1.Rows[i]["godown_id"].ToString() + "'", dtfinal);

                    //for (int k = 0; k < dtfinal.Rows.Count; k++)
                    //{
                    //    dtfinal.Rows[k]["Vid"] = vid;
                    //}

                    //Database.SaveData(dtfinal);
                }




            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void u()
        {
            Database.CommandExecutor("Update account set Status='true'");
            Database.CommandExecutor("Update account set Allowps='true'");
        }
        private void AddLocationVouchertype()
        {
            try
            {
                if (Database.DatabaseType == "access")
                {
                    if (Database.CommandExecutor("ALTER TABLE Vouchertype ADD Column LocationId text(20)") == true)
                    {
                        Database.CommandExecutor("update Vouchertype set LocationId='SER'");
                    }
                  
                }
                if (Database.DatabaseType == "sql")
                {

                    if (Database.CommandExecutor("ALTER TABLE Vouchertype ADD LocationId nvarchar(20)") == true)
                    {
                        Database.CommandExecutor("update Vouchertype set LocationId='SER'");
                    }
                   
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void AddLocation()
        {
            try
            {
                if (Database.DatabaseType == "access")
                {
                    if (Database.CommandExecutor("ALTER TABLE EmailLOG ADD Column LocationId text(20)") == true)
                    {
                        Database.CommandExecutor("update EmailLOG set LocationId='SER'");
                    }
                    if (Database.CommandExecutor("ALTER TABLE BillByBill ADD Column LocationId text(20)") == true)
                    {
                        Database.CommandExecutor("update BillByBill set LocationId='SER'");
                    }
                    if (Database.CommandExecutor("ALTER TABLE smssetup ADD Column LocationId text(20)") == true)
                    {
                        Database.CommandExecutor("update smssetup set LocationId='SER'");
                    }
                }
                if (Database.DatabaseType == "sql")
                {
                    if (Database.CommandExecutor("ALTER TABLE EmailLOG ADD LocationId nvarchar(20)") == true)
                    {
                        Database.CommandExecutor("update EmailLOG set LocationId='SER'");
                    }
                    if (Database.CommandExecutor("ALTER TABLE BillByBill ADD LocationId nvarchar(20)") == true)
                    {
                        Database.CommandExecutor("update BillByBill set LocationId='SER'");
                    }
                    if (Database.CommandExecutor("ALTER TABLE smssetup ADD LocationId nvarchar(20)") == true)
                    {
                        Database.CommandExecutor("update smssetup set LocationId='SER'");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void AddNid()
        {
            try
            {
                if (Database.DatabaseType == "access")
                {
                    if (Database.CommandExecutor("ALTER TABLE Account ADD Column Nid number") == true)
                    {
                        Database.CommandExecutor("update Account set Nid=Ac_id");
                    }
                    if (Database.CommandExecutor("ALTER TABLE ACCOUNTYPE ADD Column Nid number") == true)
                    {
                        Database.CommandExecutor("update ACCOUNTYPE set Nid=Act_id");
                    }
                    if (Database.CommandExecutor("ALTER TABLE BILLBYBILL ADD Column Nid number") == true)
                    {
                        Database.CommandExecutor("update BILLBYBILL set Nid=ID");
                    }
                    if (Database.CommandExecutor("ALTER TABLE CHARGES ADD Column Nid number") == true)
                    {
                        Database.CommandExecutor("update CHARGES set Nid=Ch_id");
                    }
                    if (Database.CommandExecutor("ALTER TABLE Contractor ADD Column Nid number") == true)
                    {
                        Database.CommandExecutor("update Contractor set Nid=Con_id");
                    }
                    if (Database.CommandExecutor("ALTER TABLE [DESCRIPTION] ADD Column Nid number") == true)
                    {
                        Database.CommandExecutor("update [DESCRIPTION] set Nid=Des_id");
                    }
                    if (Database.CommandExecutor("ALTER TABLE EmailLOG ADD Column Nid number") == true)
                    {
                        Database.CommandExecutor("update EmailLOG set Nid=id");
                    }
                    if (Database.CommandExecutor("ALTER TABLE importantdate ADD Column Nid number") == true)
                    {
                        Database.CommandExecutor("update importantdate set Nid=id");
                    }
                    if (Database.CommandExecutor("ALTER TABLE OTHER ADD Column Nid number") == true)
                    {
                        Database.CommandExecutor("update OTHER set Nid=Oth_id");
                    }
                    if (Database.CommandExecutor("ALTER TABLE State ADD Column Nid number") == true)
                    {
                        Database.CommandExecutor("update State set Nid=State_id");
                    }
                    if (Database.CommandExecutor("ALTER TABLE smssetup ADD Column Nid number") == true)
                    {
                        Database.CommandExecutor("update smssetup set Nid=id");
                    }
                    if (Database.CommandExecutor("ALTER TABLE TAXCATEGORY ADD Column Nid number") == true)
                    {
                        Database.CommandExecutor("update TAXCATEGORY set Nid=Category_Id");
                    }
                    if (Database.CommandExecutor("ALTER TABLE Userinfo ADD Column Nid number") == true)
                    {
                        Database.CommandExecutor("update Userinfo set Nid=U_Id");
                    }
                    if (Database.CommandExecutor("ALTER TABLE VOUCHERTYPE ADD Column Nid number") == true)
                    {
                        Database.CommandExecutor("update VOUCHERTYPE set Nid=Vt_id");
                    }
                    if (Database.CommandExecutor("ALTER TABLE VOUCHERINFO ADD Column Nid number") == true)
                    {
                        Database.CommandExecutor("update VOUCHERINFO set Nid=Vi_id");
                    }
                }
                else if (Database.DatabaseType == "sql")
                {
                    if (Database.CommandExecutor("ALTER TABLE Account ADD Nid int") == true)
                    {
                        Database.CommandExecutor("update Account set Nid=Ac_id");
                    }
                    if (Database.CommandExecutor("ALTER TABLE ACCOUNTYPE ADD  Nid int") == true)
                    {
                        Database.CommandExecutor("update ACCOUNTYPE set Nid=Act_id");
                    }
                    if (Database.CommandExecutor("ALTER TABLE BILLBYBILL ADD  Nid int") == true)
                    {
                        Database.CommandExecutor("update BILLBYBILL set Nid=ID");
                    }
                    if (Database.CommandExecutor("ALTER TABLE CHARGES ADD  Nid int") == true)
                    {
                        Database.CommandExecutor("update CHARGES set Nid=Ch_id");
                    }
                    if (Database.CommandExecutor("ALTER TABLE Contractor ADD  Nid int") == true)
                    {
                        Database.CommandExecutor("update Contractor set Nid=Con_id");
                    }
                    if (Database.CommandExecutor("ALTER TABLE [DESCRIPTION] ADD  Nid int") == true)
                    {
                        Database.CommandExecutor("update [DESCRIPTION] set Nid=Des_id");
                    }
                    if (Database.CommandExecutor("ALTER TABLE EmailLOG ADD  Nid int") == true)
                    {
                        Database.CommandExecutor("update EmailLOG set Nid=id");
                    }
                    if (Database.CommandExecutor("ALTER TABLE importantdate ADD  Nid int") == true)
                    {
                        Database.CommandExecutor("update importantdate set Nid=id");
                    }
                    if (Database.CommandExecutor("ALTER TABLE OTHER ADD  Nid int") == true)
                    {
                        Database.CommandExecutor("update OTHER set Nid=Oth_id");
                    }
                    if (Database.CommandExecutor("ALTER TABLE State ADD  Nid int") == true)
                    {
                        Database.CommandExecutor("update State set Nid=State_id");
                    }
                    if (Database.CommandExecutor("ALTER TABLE smssetup ADD  Nid int") == true)
                    {
                        Database.CommandExecutor("update smssetup set Nid=id");
                    }
                    if (Database.CommandExecutor("ALTER TABLE TAXCATEGORY ADD  Nid int") == true)
                    {
                        Database.CommandExecutor("update TAXCATEGORY set Nid=Category_Id");
                    }
                    if (Database.CommandExecutor("ALTER TABLE Userinfo ADD  Nid int") == true)
                    {
                        Database.CommandExecutor("update Userinfo set Nid=U_Id");
                    }
                    if (Database.CommandExecutor("ALTER TABLE VOUCHERTYPE ADD  Nid int") == true)
                    {
                        Database.CommandExecutor("update VOUCHERTYPE set Nid=Vt_id");
                    }
                    if (Database.CommandExecutor("ALTER TABLE VOUCHERINFO ADD  Nid int") == true)
                    {
                        Database.CommandExecutor("update VOUCHERINFO set Nid=Vi_id");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }




        private void DeleteCols()
        {
            try
            {
                if (Database.DatabaseType == "access")
                {
                    if (Database.CommandExecutor("DROP TABLE Deleted") == true)
                    {

                    }
                    if (Database.CommandExecutor("DROP TABLE PACKING1") == true)
                    {
                    }
                    if (Database.CommandExecutor("DROP TABLE StockCategory") == true)
                    {
                    }
                    if (Database.CommandExecutor("ALTER TABLE ACCOUNT DROP COLUMN [state]") == true)
                    {
                    }
                    if (Database.CommandExecutor("ALTER TABLE ACCOUNT DROP COLUMN Dr") == true)
                    {
                    }
                    if (Database.CommandExecutor("ALTER TABLE ACCOUNT DROP COLUMN Cr") == true)
                    {
                    }
                    if (Database.CommandExecutor("ALTER TABLE ACCOUNT DROP COLUMN Cr2") == true)
                    {
                    }
                    if (Database.CommandExecutor("ALTER TABLE ACCOUNT DROP COLUMN Dr2") == true)
                    {
                    }
                    if (Database.CommandExecutor("ALTER TABLE [DESCRIPTION] DROP COLUMN [state]") == true)
                    {
                    }
                    if (Database.CommandExecutor("ALTER TABLE VOUCHERINFO DROP COLUMN [state]") == true)
                    {
                    }
                }
                if (Database.DatabaseType == "sql")
                {
                    if (Database.CommandExecutor("DROP TABLE Deleted") == true)
                    {
                    }
                    if (Database.CommandExecutor("DROP TABLE PACKING1") == true)
                    {
                    }
                    if (Database.CommandExecutor("DROP TABLE StockCategory") == true)
                    {
                    }
                    if (Database.CommandExecutor("ALTER TABLE ACCOUNT DROP COLUMN [state]") == true)
                    {
                    }
                    if (Database.CommandExecutor("ALTER TABLE ACCOUNT DROP COLUMN Dr") == true)
                    {
                    }
                    if (Database.CommandExecutor("ALTER TABLE ACCOUNT DROP COLUMN Cr") == true)
                    {
                    }
                    if (Database.CommandExecutor("ALTER TABLE ACCOUNT DROP COLUMN Cr2") == true)
                    {
                    }
                    if (Database.CommandExecutor("ALTER TABLE ACCOUNT DROP COLUMN Dr2") == true)
                    {
                    }
                    if (Database.CommandExecutor("ALTER TABLE [DESCRIPTION] DROP COLUMN [state]") == true)
                    {
                    }
                    if (Database.CommandExecutor("ALTER TABLE VOUCHERINFO DROP COLUMN [state]") == true)
                    {
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }



        private void update07feb()
        {
            if (Database.DatabaseType == "sql")
            {
                if (Database.CommandExecutor("ALTER TABLE FirmSetup ADD ToSuperAdmin nvarchar(225)") == true)
                {
                    Database.CommandExecutor("update FirmSetup set ToSuperAdmin=selected_value");
                }
                if (Database.CommandExecutor("ALTER TABLE FirmSetup ADD ToAdmin nvarchar(225)") == true)
                {
                    Database.CommandExecutor("update FirmSetup set ToAdmin=selected_value");
                }
                if (Database.CommandExecutor("ALTER TABLE FirmSetup ADD ToSuperUser nvarchar(225)") == true)
                {
                    Database.CommandExecutor("update FirmSetup set ToSuperUser=selected_value");
                }
                if (Database.CommandExecutor("ALTER TABLE FirmSetup ADD ToUser nvarchar(225)") == true)
                {
                    Database.CommandExecutor("update FirmSetup set ToUser=selected_value");
                }
                if (Database.CommandExecutor("ALTER TABLE FirmSetup ADD ToCashier nvarchar(225)") == true)
                {
                    Database.CommandExecutor("update FirmSetup set ToCashier=selected_value");
                }
                if (Database.CommandExecutor("ALTER TABLE FirmSetup DROP COLUMN selected_value") == true)
                {
                }
            }
        }

        private void Update14()
        {
            if (Database.DatabaseType == "sql")
            {
                Database.CommandExecutor("update [Description] set Status='Enabled' where status is null");
                Database.CommandExecutor("update [Description] set MRP=Retail where MRP is null");
                if (AddFeatureNew("Description", "Square Feet/Square Meter", "Square Feet/Square Meter on Description", true, false, "Yes;No", "No", "No", "No", "No", "No", "ComboBox") == true)
                {
                    if (Database.CommandExecutor("ALTER TABLE [Description] ADD Square_FT money") == true)
                    {
                        Database.CommandExecutor("update [Description] set Square_FT=1");
                    }

                    if (Database.CommandExecutor("ALTER TABLE [Description] ADD Square_MT money") == true)
                    {
                        Database.CommandExecutor("update [Description] set Square_MT=1");
                    }

                   
                    if (Database.CommandExecutor("ALTER TABLE [Voucherinfo] ADD Sq_FT_MT nvarchar(50)") == true)
                    {
                        Database.CommandExecutor("update [Voucherinfo] set Sq_FT_MT='Sq. Feet'");
                    }
                }
            }
        }

        private void update19()
        {
            if (Database.DatabaseType == "sql")
            {
                if (Database.CommandExecutor("ALTER TABLE [Description] ADD Max_level money") == true)
                {
                    Database.CommandExecutor("update [Description] set Max_level=Wlavel");
                }
                if (Database.CommandExecutor("create table Container ([id] nvarchar(20), [Cname] nvarchar (255), Nid int, LocationId nvarchar(255)  CONSTRAINT Ck_branch PRIMARY KEY(Cname))") == true)
                {
                }

                if (Database.CommandExecutor("ALTER TABLE [Voucherdet] ADD Square_FT money") == true)
                {
                    Database.CommandExecutor("update [Voucherdet] set Square_FT=1");
                }

                if (Database.CommandExecutor("ALTER TABLE [Voucherdet] ADD Square_MT money") == true)
                {
                    Database.CommandExecutor("update [Voucherdet] set Square_MT=1");
                }
            }
        }

        private void update28feb()
        {
            if (Database.DatabaseType == "sql")
            {
                if (Database.CommandExecutor("ALTER TABLE [Description] ADD Change_des bit") == true)
                {
                    Database.CommandExecutor("update [Description] set Change_des='true'");
                }
                Database.CommandExecutor("update [Description] set Rebate=0 where Rebate is null");
                Database.CommandExecutor("update [Description] set Weight=1 where Weight is null");
                //if (AddFeatureNew("Transaction", "Voucher Editing Power", "Voucher Editing Power Upto Voucher Count", true, false, "Unlimited", "Unlimited", "Unlimited", "0", "0", "0", "Textbox") == true)
                //{
                //}
            }
        }

        private void update3march()
        {
            if (Database.DatabaseType == "sql")
            {
                if (Database.CommandExecutor("ALTER TABLE [Description] ADD Srebate money") == true)
                {
                    Database.CommandExecutor("update [Description] set Srebate=0");
                }
            }
        }

        private void Backup()
        {

            AddFeatureNew("Transaction", "IP Backup", "IP Backup", true, false, "Yes;No", "Yes", "Yes", "Yes", "Yes", "Yes", "ComboBox");
                
           
        }
        private void Update5march()
        {
            if (Database.DatabaseType == "sql")
            {
                //if (AddFeatureNew("Transaction", "Voucher Delete Permission", "Voucher Delete Permission", true, false, "Yes;No", "Yes", "Yes", "No", "No", "No", "ComboBox") == true)
                //{
                //}
            }
        }

        private void SalesVou()
        {
            try
            {
                Database.BeginTran();
                if (Database.CommandExecutor("Alter table Voucherinfo add S_id nvarchar(255)") == true)
                {
                    Database.CommandExecutor("update Voucherinfo set s_id='0'");
                }
                Database.CommitTran();
            }
            catch (Exception ex)
            {
                Database.RollbackTran();
            }
        }

        private void PackingCat()
        {
            try
            {
                Database.BeginTran();

                if (Database.DatabaseType == "sql")
                {
                    DataTable dtbranch = new DataTable();
                    Database.GetSqlData("SELECT 1 AS Expr1 FROM  sys.tables WHERE (name = 'PackCategory')", dtbranch);
                    if (dtbranch.Rows.Count == 0)
                    {
                        if (Database.CommandExecutor("create table PackCategory ([Nid] int,[PackCat_id] nvarchar(50), [Name] nvarchar (255),user_id nvarchar(250),Modifiedby nvarchar(250)  CONSTRAINT packcat PRIMARY KEY(Name))") == true)
                        {

                        }
                    }

                    if (Database.CommandExecutor("Alter table Description add PackCat_id nvarchar(255)") == true)
                    {

                        Database.CommandExecutor("update description set packcat_id='0'");

                        DataTable dtpaydet = new DataTable();
                        Database.GetSqlData("SELECT     Vi_id,totalamount FROM VOUCHERINFO WHERE     (CashCredit = 'Cash') AND (Branch_id = '" + Database.BranchId + "')", dtpaydet);
                       // Database.CommandExecutor("Delete from Voucherpaydet");
                        string cashac_id = Database.GetScalarText("select ac_id from account where act_id='ser3' and Branch_id='" + Database.BranchId + "'");
                        for (int i = 0; i < dtpaydet.Rows.Count; i++)
                        {
                            Database.CommandExecutor("insert into Voucherpaydet (Vi_id,Itemsr,Acc_id,Instrumentno,Amount) values('" + dtpaydet.Rows[i]["Vi_id"].ToString() + "',1,'" + cashac_id + "',''," + double.Parse(dtpaydet.Rows[i]["totalamount"].ToString()) + ")");

                        }
                    }

                    Database.CommandExecutor("insert into PackCategory (Nid,PackCat_id,Name,User_id,modifiedBy) values(1,'SER1','Bulk Pack','SER42','')");
                    Database.CommandExecutor("insert into PackCategory (Nid,PackCat_id,Name,User_id,modifiedBy) values(2,'SER2','Retail Pack','SER42','')");
                    Database.CommandExecutor("insert into PackCategory (Nid,PackCat_id,Name,User_id,modifiedBy) values(3,'SER3','Small Pack','SER42','')");
                    Database.CommandExecutor("update description set PackCat_id='SER1' where Pvalue>8");
                    Database.CommandExecutor("update description set PackCat_id='SER3' where Pvalue<0.4");
                    Database.CommandExecutor("update description set PackCat_id='SER2' where Pvalue>=0.4 and Pvalue<=8");
                }
                Database.CommitTran();
            }
            catch (Exception ex)
            {
                Database.RollbackTran();
            }
        }

        private void Companydbdet()
        {
           
            try
            {
                Database.BeginTran();
                if (Database.CommandExecutor("Alter table Company ADD [hostname] nvarchar(100)") == true)
                {
                    Database.CommandExecutor("Alter table Company ADD [username] nvarchar(100)");
                    Database.CommandExecutor("Alter table Company ADD [dbname] nvarchar(100)");
                    Database.CommandExecutor("Alter table Company ADD [pwd] nvarchar(100)");
                    Database.CommandExecutor("Update company set hostname=''");
                    Database.CommandExecutor("Update company set username=''");
                    Database.CommandExecutor("Update company set dbname=''");
                    Database.CommandExecutor("Update company set pwd=''");
                }


                Database.CommitTran();
            }
            catch (Exception ex)
            {
                Database.RollbackTran();
            }
        }


        private void CopyRate()
        {
            try
            {
                Database.BeginTran();
                if (Database.DatabaseType == "sql")
                {
                    DataTable dtbranch = new DataTable();
                    Database.GetSqlData("SELECT 1 AS Expr1 FROM  sys.tables WHERE (name = 'CopyRates')", dtbranch);
                    if (dtbranch.Rows.Count == 0)
                    {
                        if (Database.CommandExecutor("create table CopyRates ([Nid] int,[Cr_id] nvarchar(50), [Company_id] nvarchar (255),Item_id nvarchar(250),Color_id nvarchar(250),Group_id nvarchar(250),HSN_id nvarchar(250),Description nvarchar(250),Pack nvarchar(250),Pack_category_id nvarchar(250),ratefrom nvarchar(250),rateto nvarchar(250),insurance money,Rebate money,Dis1 money,Tax money, Dis2 money,Rebate2 money,Freight money,[on] nvarchar(250),Profit money,Rateunit nvarchar(250),Rounding nvarchar(250) ,LocationId nvarchar(250) CONSTRAINT packcat1 PRIMARY KEY(Cr_id))") == true)
                        {


                        }
                    }
                }
                Database.CommitTran();
            }
            catch (Exception ex)
            {
                Database.RollbackTran();
            }
        }


        private void MultiplecashAcc()
        {
            if (Database.DatabaseType == "sql")
            {
                //AddFeatureNew("Report", "Personal Mode Allowed", "Personal Mode Allowed", true, false, "Yes;No;", "Yes", "Yes", "Yes", "No", "Yes", "ComboBox");
                //AddFeatureNew("Report", "Both Mode Allowed", "Both Mode Allowed", true, false, "Yes;No;", "Yes", "Yes", "Yes", "No", "Yes", "ComboBox");
                AddFeatureNew("Report", "Narration Required on Ledger", "Narration Required on Ledger", true, false, "Yes;No;", "Yes", "Yes", "Yes", "Yes", "Yes", "ComboBox");
                AddFeatureNew("Transaction", "Particular Required on Ledger", "Particular Required on Ledger", true, false, "Yes;No", "Yes", "Yes", "Yes", "Yes", "Yes", "ComboBox");
                AddFeatureNew("Transaction", "Required UpdateRate Option", "Required UpdateRate Option", true, false, "Yes;No", "Yes", "No", "No", "No", "No", "ComboBox");

                AddFeatureNew("Transaction", "Required PaymentMode Form", "Required PaymentMode Form", true, false, "Yes;No", "No", "No", "No", "No", "No", "ComboBox");
                DataTable dtbranch = new DataTable();
                Database.GetSqlData("SELECT 1 AS Expr1 FROM  sys.tables WHERE (name = 'VoucherpayDet')", dtbranch);
                if (dtbranch.Rows.Count == 0)
                {
                    if (Database.CommandExecutor("create table VoucherpayDet ([Vi_id] nvarchar(50) , [Itemsr] int, [Acc_id] nvarchar (255),Instrumentno nvarchar(255),amount money  CONSTRAINT vou_pay PRIMARY KEY(Vi_id,Acc_id))") == true)
                    {

                       // Database.CommandExecutor("insert into Branch (Bname) values('Main')");
                    }
                }
            }
        }
        private void Dr_Cr_ac_id()
        {

            try
            {
                Database.BeginTran();
                if (Database.DatabaseType == "access")
                {
                    if (Database.CommandExecutor("ALTER TABLE Voucherinfo ADD Column  Dr_Ac_id number"))
                    {
                        Database.CommandExecutor("ALTER TABLE Voucherinfo ADD Column  Cr_Ac_id number");
                    }

                }
                else
                {
                    if (Database.CommandExecutor("ALTER TABLE Voucherinfo ADD   Dr_Ac_id nvarchar(255)"))
                    {
                        Database.CommandExecutor("ALTER TABLE Voucherinfo ADD   Cr_Ac_id nvarchar(255)");
                    }
                }
                Database.CommitTran();
            }
            catch (Exception ex)
            {
                Database.RollbackTran();
            }
        }


        private void Update8mar()
        {
            if (Database.DatabaseType == "sql")
            {
                if (AddFeatureNew("Display", "Text Case", "Text Case", true, false, "As Actual;To UpperCase;To LowerCase;To CamelCase", "As Actual", "As Actual", "As Actual", "As Actual", "As Actual", "ComboBox") == true)
                {

                }

                if (AddFeatureNew("Transaction", "Show Taxes in Including Tax", "Show Taxes in Including Tax", false, false, "No;Yes;", "No", "No", "No", "No", "No", "ComboBox") == true)
                {

                }
              
                if (AddFeatureNew("Report", "Default Ledger Type", "Ledger Type", false, false, "Detailed;Summarized;", "Summarized", "Summarized", "Summarized", "Summarized", "Summarized", "ComboBox") == true)
                {

                }
            }
        }

        private void Update9mar()
        {
            if (Database.DatabaseType == "sql")
            {
                if (Database.CommandExecutor("ALTER TABLE VOUCHERINFO ADD Cashier_approved bit") == true)
                {
                    Database.CommandExecutor("Update VOUCHERINFO set Cashier_approved=1 where cash_pending=0");
                    Database.CommandExecutor("Update VOUCHERINFO set Cashier_approved=0 where cash_pending=1");
                }
            }
        }

        private void Update9march()
        {
            if (Database.DatabaseType == "sql")
            {
                if (Database.CommandExecutor("ALTER TABLE VOUCHERINFO ADD Approved bit") == true)
                {
                    Database.CommandExecutor("Update VOUCHERINFO set Approved=1 where NApproval=0");
                    Database.CommandExecutor("Update VOUCHERINFO set Approved=0 where NApproval=1");
                }
            }
        }

        private void update8March()
        {
            string user = Database.GetScalarText("SELECT U_id FROM Userinfo WHERE Uname = 'Aman'");
            try
            {
                Database.BeginTran();
                if (Database.DatabaseType == "sql")
                {
                    if (Database.CommandExecutor("ALTER TABLE ACCOUNT ADD user_id nvarchar(20)") == true)
                    {
                        Database.CommandExecutor("Update ACCOUNT set user_id='" + user + "'");
                    }
                    if (Database.CommandExecutor("ALTER TABLE ACCOUNT ADD Modifiedby nvarchar(20)") == true)
                    {
                        Database.CommandExecutor("Update ACCOUNT set Modifiedby=''");
                    }

                    if (Database.CommandExecutor("ALTER TABLE Description ADD user_id nvarchar(20)") == true)
                    {
                        Database.CommandExecutor("Update Description set user_id='" + user + "'");
                    }
                    if (Database.CommandExecutor("ALTER TABLE Description ADD Modifiedby nvarchar(20)") == true)
                    {
                        Database.CommandExecutor("Update Description set Modifiedby=''");
                    }

                    if (Database.CommandExecutor("ALTER TABLE CONTRACTOR ADD user_id nvarchar(20)") == true)
                    {
                        Database.CommandExecutor("Update CONTRACTOR set user_id='" + user + "'");
                    }
                    if (Database.CommandExecutor("ALTER TABLE CONTRACTOR ADD Modifiedby nvarchar(20)") == true)
                    {
                        Database.CommandExecutor("Update CONTRACTOR set Modifiedby=''");
                    }

                    if (Database.CommandExecutor("ALTER TABLE CHARGES ADD user_id nvarchar(20)") == true)
                    {
                        Database.CommandExecutor("Update CHARGES set user_id='" + user + "'");
                    }
                    if (Database.CommandExecutor("ALTER TABLE CHARGES ADD Modifiedby nvarchar(20)") == true)
                    {
                        Database.CommandExecutor("Update CHARGES set Modifiedby=''");
                    }

                    if (Database.CommandExecutor("ALTER TABLE CONTAINER ADD user_id nvarchar(20)") == true)
                    {
                        Database.CommandExecutor("Update CONTAINER set user_id='" + user + "'");
                    }
                    if (Database.CommandExecutor("ALTER TABLE CONTAINER ADD Modifiedby nvarchar(20)") == true)
                    {
                        Database.CommandExecutor("Update CONTAINER set Modifiedby=''");
                    }

                    if (Database.CommandExecutor("ALTER TABLE STATE ADD user_id nvarchar(20)") == true)
                    {
                        Database.CommandExecutor("Update STATE set user_id='" + user + "'");
                    }
                    if (Database.CommandExecutor("ALTER TABLE STATE ADD Modifiedby nvarchar(20)") == true)
                    {
                        Database.CommandExecutor("Update STATE set Modifiedby=''");
                    }

                    if (Database.CommandExecutor("ALTER TABLE USERINFO ADD user_id nvarchar(20)") == true)
                    {
                        Database.CommandExecutor("Update USERINFO set user_id='" + user + "'");
                    }
                    if (Database.CommandExecutor("ALTER TABLE USERINFO ADD Modifiedby nvarchar(20)") == true)
                    {
                        Database.CommandExecutor("Update USERINFO set Modifiedby=''");
                    }

                    if (Database.CommandExecutor("ALTER TABLE BRANCH ADD user_id nvarchar(20)") == true)
                    {
                        Database.CommandExecutor("Update BRANCH set user_id='" + user + "'");
                    }
                    if (Database.CommandExecutor("ALTER TABLE BRANCH ADD Modifiedby nvarchar(20)") == true)
                    {
                        Database.CommandExecutor("Update BRANCH set Modifiedby=''");
                    }

                    if (Database.CommandExecutor("ALTER TABLE OTHER ADD user_id nvarchar(20)") == true)
                    {
                        Database.CommandExecutor("Update OTHER set user_id='" + user + "'");
                    }
                    if (Database.CommandExecutor("ALTER TABLE OTHER ADD Modifiedby nvarchar(20)") == true)
                    {
                        Database.CommandExecutor("Update OTHER set Modifiedby=''");
                    }

                    if (Database.CommandExecutor("ALTER TABLE TAXCATEGORY ADD user_id nvarchar(20)") == true)
                    {
                        Database.CommandExecutor("Update TAXCATEGORY set user_id='" + user + "'");
                    }
                    if (Database.CommandExecutor("ALTER TABLE TAXCATEGORY ADD Modifiedby nvarchar(20)") == true)
                    {
                        Database.CommandExecutor("Update TAXCATEGORY set Modifiedby=''");
                    }

                    //if (Database.CommandExecutor("ALTER TABLE PARTYRATE ADD user_id nvarchar(20)") == true)
                    //{
                    //    Database.CommandExecutor("Update PARTYRATE set user_id='" + user + "'");
                    //}
                    //if (Database.CommandExecutor("ALTER TABLE PARTYRATE ADD Modifiedby nvarchar(20)") == true)
                    //{
                    //    Database.CommandExecutor("Update PARTYRATE set Modifiedby=''");
                    //}

                    //if (Database.CommandExecutor("ALTER TABLE DisAfterTax ADD user_id nvarchar(20)") == true)
                    //{
                    //    Database.CommandExecutor("Update DisAfterTax set user_id='" + user + "'");
                    //}
                    //if (Database.CommandExecutor("ALTER TABLE DisAfterTax ADD Modifiedby nvarchar(20)") == true)
                    //{
                    //    Database.CommandExecutor("Update DisAfterTax set Modifiedby=''");
                    //}
                }
                Database.CommitTran();
            }
            catch (Exception ex)
            {
                Database.RollbackTran();
            }
        }
        private void Batchno()
        {
            try
            {
                Database.BeginTran();
                if (Database.CommandExecutor("ALTER TABLE Syncronizer ADD  Batchno bigint "))
                {
                   
                }

                Database.CommitTran();

            }
            catch (Exception ex)
            {
                Database.RollbackTran();
            }

        }
        private void AccCode()
        {
            try
            {
                Database.BeginTran();
                if (Database.CommandExecutor("ALTER TABLE Account ADD  Code nvarchar (255)"))
                {
                    Database.CommandExecutor("Update Account set Code=''");
                }

                Database.CommitTran();

            }
            catch (Exception ex)
            {
                Database.RollbackTran();
            }

        }
        private void Update7march()
        {
            try
            {
                Database.BeginTran();
                int vtnid = Database.GetScalarInt("Select Max(Nid) from Vouchertype") + 1;

                if (Database.GetScalarInt("Select count(*) from Vouchertype where Name='Credit Note K'") == 0)
                {
                    Database.CommandExecutor("insert into VOUCHERTYPE (Vt_id,[Name],[Type],[Stationary],[Numtype],[Short],[AliasName],[Default1],[Default2],[Default3],[ReportName],[Code],[Effect_On_Stock],[Effect_On_Acc],[IncludingTax],[ExcludingTax],[ExState],[TaxInvoice],[Unregistered],[Active],[PaperSize],[SmsTemplate],[A],[B],[printcopy],[CashTransaction],[Calculation],[Postfix],[Prefix],[Padding],[Exempted],[VoucCategory],[Ratetype],[Nid],[LocationId]) values('SER" + vtnid.ToString() + "','Credit Note K','Cnote'," + access_sql.Singlequote + "true" + access_sql.Singlequote + ",1,'CRNK*','Kachcha Credit Note','Original Copy','None','Office Copy','ReceptPayment.rpt','CRNK*','N','Y'," + access_sql.Singlequote + "false" + access_sql.Singlequote + "," + access_sql.Singlequote + "false" + access_sql.Singlequote + "," + access_sql.Singlequote + "false" + access_sql.Singlequote + "," + access_sql.Singlequote + "true" + access_sql.Singlequote + "," + access_sql.Singlequote + "true" + access_sql.Singlequote + "," + access_sql.Singlequote + "true" + access_sql.Singlequote + ",'A4','Dear Customer, Thanks For Purchasing With Us, Your Last Bill No is: {Vno}'," + access_sql.Singlequote + "false" + access_sql.Singlequote + "," + access_sql.Singlequote + "true" + access_sql.Singlequote + ",'Original for Recipient,True;Duplicate for Transporter,True;Triplicate for Supplier,True;','Not Allowed','Default Excluding Tax','','O-',6,'Not Allowed','Others','Retail'," + vtnid + ",'SER')");
                    vtnid = Database.GetScalarInt("Select Max(Nid) from Vouchertype") + 1;
                }

                if (Database.GetScalarInt("Select count(*) from Vouchertype where Name='Debit Note K'") == 0)
                {
                    Database.CommandExecutor("insert into VOUCHERTYPE (Vt_id,[Name],[Type],[Stationary],[Numtype],[Short],[AliasName],[Default1],[Default2],[Default3],[ReportName],[Code],[Effect_On_Stock],[Effect_On_Acc],[IncludingTax],[ExcludingTax],[ExState],[TaxInvoice],[Unregistered],[Active],[PaperSize],[SmsTemplate],[A],[B],[printcopy],[CashTransaction],[Calculation],[Postfix],[Prefix],[Padding],[Exempted],[VoucCategory],[Ratetype],[Nid],[LocationId]) values('SER" + vtnid.ToString() + "','Debit Note K','Dnote'," + access_sql.Singlequote + "true" + access_sql.Singlequote + ",1,'DRNK*','Kachcha Debit Note','Original Copy','None','Office Copy','ReceptPayment.rpt','DRNK*','N','Y'," + access_sql.Singlequote + "false" + access_sql.Singlequote + "," + access_sql.Singlequote + "false" + access_sql.Singlequote + "," + access_sql.Singlequote + "false" + access_sql.Singlequote + "," + access_sql.Singlequote + "true" + access_sql.Singlequote + "," + access_sql.Singlequote + "true" + access_sql.Singlequote + "," + access_sql.Singlequote + "true" + access_sql.Singlequote + ",'A4','Dear Customer, Thanks For Purchasing With Us, Your Last Bill No is: {Vno}'," + access_sql.Singlequote + "false" + access_sql.Singlequote + "," + access_sql.Singlequote + "true" + access_sql.Singlequote + ",'Original for Recipient,True;Duplicate for Transporter,True;Triplicate for Supplier,True;','Not Allowed','Default Excluding Tax','','O-',6,'Not Allowed','Others','Retail'," + vtnid + ",'SER')");
                    vtnid = Database.GetScalarInt("Select Max(Nid) from Vouchertype") + 1;
                }

                if (Database.GetScalarInt("Select count(*) from Vouchertype where Name='Journal Voucher K'") == 0)
                {
                    Database.CommandExecutor("insert into VOUCHERTYPE (Vt_id,[Name],[Type],[Stationary],[Numtype],[Short],[AliasName],[Default1],[Default2],[Default3],[ReportName],[Code],[Effect_On_Stock],[Effect_On_Acc],[IncludingTax],[ExcludingTax],[ExState],[TaxInvoice],[Unregistered],[Active],[PaperSize],[SmsTemplate],[A],[B],[printcopy],[CashTransaction],[Calculation],[Postfix],[Prefix],[Padding],[Exempted],[VoucCategory],[Ratetype],[Nid],[LocationId]) values('SER" + vtnid.ToString() + "','Journal Voucher K','Journal'," + access_sql.Singlequote + "true" + access_sql.Singlequote + ",1,'JOUK*','Kachcha Journal Voucher','Original Copy','None','Office Copy','ReceptPayment.rpt','JOUK*','N','Y'," + access_sql.Singlequote + "false" + access_sql.Singlequote + "," + access_sql.Singlequote + "false" + access_sql.Singlequote + "," + access_sql.Singlequote + "false" + access_sql.Singlequote + "," + access_sql.Singlequote + "true" + access_sql.Singlequote + "," + access_sql.Singlequote + "true" + access_sql.Singlequote + "," + access_sql.Singlequote + "true" + access_sql.Singlequote + ",'A4','Dear Customer, Thanks For Purchasing With Us, Your Last Bill No is: {Vno}'," + access_sql.Singlequote + "false" + access_sql.Singlequote + "," + access_sql.Singlequote + "true" + access_sql.Singlequote + ",'Original for Recipient,True;Duplicate for Transporter,True;Triplicate for Supplier,True;','Not Allowed','Default Excluding Tax','','O-',6,'Not Allowed','Others','Retail'," + vtnid + ",'SER')");
                }

                Database.CommitTran();
            }
            catch (Exception ex)
            {
                Database.RollbackTran();
            }
        }

        private void update06march()
        {
            if (Database.CommandExecutor("create table MenuOption ([id] int Identity, [Menu] nvarchar (250), [Under] int, SuperAdmin bit, Admin bit, SuperUser bit, [User] bit, Cashier bit  CONSTRAINT pk_menuunder PRIMARY KEY(Menu,Under))") == true)
            {
                //all Menu
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('List',0,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Transaction',0,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Tool',0,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Reports',0,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Activate',0,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Settings',0,'true','true','true','true','true')");

                //sub menu of list
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Account',1,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Product and Service',1,'true','true','true','true','true')");

                //sub menu of transaction
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Receipt',2,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Payment',2,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Journal Voucher',2,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Sale',2,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Sale Return',2,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Purchase',2,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Purchase Return',2,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Debite Note',2,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Credit Note',2,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Contra',2,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Pendings',2,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Stock Journal',2,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Sale Order',2,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('RCM Invoice',2,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Debit Note With GST',2,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Credit Note With GST',2,'true','true','true','true','true')");

                //sub menu of tool
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Calculator',3,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Modify Item',3,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Modify Rate',3,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Account Opening Balance',3,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Opening Stock',3,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('List of Description',3,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Import Rate',3,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Import Description',3,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Smart Document Finder',3,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Customer Supplier Rate',3,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Bill By Bill Adjustment',3,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Miss Tinting',3,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Import Color',3,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Tinting System',3,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Tally Connection',3,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Check for Update',3,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Bulk Updates',3,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Purchase Import',3,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('SMS Takada',3,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('SMS Log',3,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Remove SMS Log',3,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Import From Faspi',3,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Email Log',3,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Customer Mail List',3,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Approval',3,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Cashier',3,'true','true','true','true','true')");

                //sub menu of reports
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Account Report',4,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Cost Report',4,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('VAT Report',4,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('GST Report',4,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Customer Report',4,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Broker Report',4,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Supplier Report',4,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Sale Report',4,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Purchase Report',4,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Stock Report',4,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Other Report',4,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Customer Outstanding Report',4,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Pending Order Report',4,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Audit Report',4,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Cashier Report',4,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Due Report',4,'true','true','true','true','true')");

                //submenu of settings
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Firm Settings',6,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('All List',6,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Data Backup',6,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Change Password',6,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Marwari Setup',6,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Switch Firm',6,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Log Off',6,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Exit',6,'true','true','true','true','true')");

                //marwari setup
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Control Room',71,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Voucher Configuration',71,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Email Setup',71,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('SMS Setup',71,'true','true','true','true','true')");

                //all List
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Account',68,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Account Group',68,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Payment Collector',68,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Broker',68,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Product and Service',68,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Company/Manufacturer',68,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Brand/Item Group',68,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Color/Variant',68,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Price Group',68,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Department',68,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Container',68,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Tax Category',68,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Discount/Charges',68,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Important Dates',68,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('State',68,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Other Details',68,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Discount After Tax',68,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('User Management',68,'true','true','true','true','true')");

                //firm settings
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Change Firm Information',67,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Data Restore',67,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Delete Firm',67,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Change Background Image',67,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Create New Financial Year',67,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Import Balance',67,'true','true','true','true','true')");

                //import Balance
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Account Balance Import',102,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Stock Item Balance Import',102,'true','true','true','true','true')");

                //account report
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Journal(DOS)',51,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Journal',51,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Ledger',51,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Detail Ledger',51,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Group Ledger',51,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Cash Book',51,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Trial Balance',51,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Moved Account Summary',51,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Payment Collector Balance',51,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Trading Account and Profit & Loss',51,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Balance Sheet',51,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Bank Book',51,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('New Cash Book',51,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Statement of Affair',51,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Cash Book Summary',51,'true','true','true','true','true')");

                //cost report
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Customer Profit',52,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Customerwise Profit',52,'true','true','true','true','true')");

                //vat report
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('VAT Annexure A',53,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('VAT Annexure B',53,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('VAT Annexure C',53,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Unregistered Purchase List',53,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Commodity Summary',53,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Commodity Detail',53,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('E-Filing UP',53,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('E-Filing UP CST',53,'true','true','true','true','true')");

                //customer reprot
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Customer Detail Bill Wise',55,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Customer Detail Item Wise',55,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Cash Sales',55,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Outstanding Report',55,'true','true','true','true','true')");

                //broker report
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Customer Brokrage',56,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Broker Detail Customer Wise',56,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Broker Detail Item Wise',56,'true','true','true','true','true')");

                //supplier report
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Supplier Detail Bill Wise',57,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Supplier Detail Item Wise',57,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Purchase Before Tax/ After Tax',57,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('In Bill Discount',57,'true','true','true','true','true')");

                //sale report
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Sale Register',58,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Sale Register-Tax Slab Wise',58,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Sale Register-HSN Wise',58,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Summarized Sale Register ',58,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Item Lifting Report',58,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Item Lifting Report(Detailed)',58,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Company Sale Register',58,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Commodity Sale Regiter',58,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('CrossTab Sale Register',58,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Cash/Credit Sale',58,'true','true','true','true','true')");

                //Purchase Report
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Purchase Register',59,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Purchase Register- Tax Slab Wise',59,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Purchase Register- HSN Wise',59,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Summarized Purchase Register',59,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Item Lifting Report',59,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Commodity Purchase Regiter',59,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('CrossTab Purchase Register',59,'true','true','true','true','true')");

                //Stock Report
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Godown Stock',60,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Item Ledger',60,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Below Stock Warning',60,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Stock Valuation',60,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Stock Liquidation',60,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('TaxSlab Wise Stock Valuation',60,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Godown In/Out',60,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Stock Summary',60,'true','true','true','true','true')");

                //Other Report
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Price List',61,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Party Price List',61,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Address Printing',61,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Continuous Bill Printinhg',61,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Debtor Address Book',61,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Creditor Address Book',61,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Graphical Report',61,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Receipt Register',61,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Payment Register',61,'true','true','true','true','true')");

                //customer outstanding
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Outstanding Bills',62,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Adjustment Detail',62,'true','true','true','true','true')");

                //audit report
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Day Summary',64,'true','true','true','true','true')");

                //due report
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Customer Bill Due',66,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Supplier Bill Due',66,'true','true','true','true','true')");

                //GST Report
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('B2B Inter State',54,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('B2B Intra State(With in UP)',54,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('B2C Inter State',54,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('B2C Intra State(With in UP)',54,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Purchase Unregistered Intra State(With in UP)',54,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Purchase Registered Intra State(With in UP)',54,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Purchase Composition Dealer Intra State(With in UP)',54,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Purchase Unregistered Inter State',54,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Purchase Registered Inter State',54,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Purchase Composition Dealer Inter State',54,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Commodity Summary',54,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Commodity Detail',54,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('HSN Summary Purchase',54,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('HSN Summary Sale',54,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('GSTR 3B(pdf)',54,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('GSTR 3B(excel)',54,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('GSTR 1',54,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('GSTR 2',54,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('GSTR 2A Matching',54,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('RCM',54,'true','true','true','true','true')");

                //trial Balance
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Standard Trial Balance',111,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Opening Trial Balance',111,'true','true','true','true','true')");
                Database.CommandExecutor("insert into MenuOption(Menu,Under,SuperAdmin,Admin,SuperUser,[User],Cashier) values('Grouped Trial Balance',111,'true','true','true','true','true')");
            }
        }

        
    }
}
