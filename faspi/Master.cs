using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace faspi
{
    class Master
    {
        public static DataTable Account;
        public static DataTable Salesman;
        public static DataTable AccountType;
        public static DataTable VoucherType;
        public static DataTable Agent;
        public static DataTable Other;
        public static DataTable State;
        public static DataTable TransportDetails;
        public static DataTable Description;
        public static DataTable DescriptionInfo;
        public static DataTable Charges;
        public static DataTable TaxCategory;
        public static DataTable Accountinfo;
        public static DataTable Feature;
        public static DataTable FeatureLogin;
        public static DataTable UserPower;
        public static DataTable Controlroom;
        public static DataTable SideMenu;
        public static DataTable DtRates;

        public static void UpdateRates()
        {
            DtRates = new DataTable();
            DtRates.Columns.Add("RateValue", typeof(string));
            DtRates.Columns.Add("RateId", typeof(string));

            DtRates.Rows.Add();
            DtRates.Rows[0][0] = faspi.Feature.Available("Name of PriceList1");
            DtRates.Rows[0][1] = "Purchase_rate";

            DtRates.Rows.Add();
            DtRates.Rows[1][0] = faspi.Feature.Available("Name of PriceList2");
            DtRates.Rows[1][1] = "Retail";

            DtRates.Rows.Add();
            DtRates.Rows[2][0] = faspi.Feature.Available("Name of PriceList3");
            DtRates.Rows[2][1] = "Wholesale";

            DtRates.Rows.Add();
            DtRates.Rows[3][0] = faspi.Feature.Available("Name of PriceList4");
            DtRates.Rows[3][1] = "Rate_X";

            DtRates.Rows.Add();
            DtRates.Rows[4][0] = faspi.Feature.Available("Name of PriceList5");
            DtRates.Rows[4][1] = "Rate_Y";

            DtRates.Rows.Add();
            DtRates.Rows[5][0] = faspi.Feature.Available("Name of PriceList6");
            DtRates.Rows[5][1] = "Rate_Z";
        }

        public static void UpdateAccount()
        {
            Account = new DataTable();
            Database.GetSqlData("select * from account",Account);
        }
        public static void UpdateSalesman()
        {
            Salesman = new DataTable();
            Database.GetSqlData("select * from Salesman", Salesman);
        }
        public static void UpdateControlRoom()
        {
            Controlroom = new DataTable();
            Database.GetSqlData("select * from FirmSetup", Controlroom);
        }

        public static void UpdateAccountType()
        {
            AccountType = new DataTable();
            Database.GetSqlData("select * from ACCOUNTYPE", AccountType);
        }

        public static void UpdateState()
        {
            State = new DataTable();
            Database.GetSqlData("select * from State", State);
        }

        public static void UpdateVoucherType()
        {
            VoucherType = new DataTable();
            Database.GetSqlData("select * from Vouchertype", VoucherType);
        }

        public static void UpdateOther()
        {
            Other = new DataTable();
            Database.GetSqlData("select * from Other", Other);
        }

        public static void UpdateTransportDetails()
        {
            TransportDetails = new DataTable();
            Database.GetSqlData("select * from TransportDetails", TransportDetails);
        }

        public static void UpdateDecription()
        {
            Description = new DataTable();
            Database.GetSqlData("select * from Description", Description);
        }

        public static void UpdateMenuOptions()
        {
            SideMenu = new DataTable();
            Database.GetSqlData("select * from SideMenu where Display='True'  order by DisplayIndex", SideMenu);
        }

        public static void UpdateDecriptionInfo()
        {
            DescriptionInfo = new DataTable();
            Database.GetSqlData("SELECT DISTINCT DESCRIPTION.Description, DESCRIPTION.Pack AS Packing, DESCRIPTION.Des_id, DESCRIPTION.Skucode, DESCRIPTION.ShortCode, DESCRIPTION.Pvalue, DESCRIPTION.Rate_Unit, DESCRIPTION.Purchase_rate, DESCRIPTION.[Commission%], DESCRIPTION.[Commission@], DESCRIPTION.Rate_X, DESCRIPTION.Rate_Y, DESCRIPTION.Rate_Z, DESCRIPTION.Retail, DESCRIPTION.MRP, DESCRIPTION.Tax_Cat_id, DESCRIPTION.Wholesale, TAXCATEGORY.PA, TAXCATEGORY.PTA1, TAXCATEGORY.PTA2, TAXCATEGORY.PTA3, TAXCATEGORY.SA, TAXCATEGORY.STA1, TAXCATEGORY.STA2, TAXCATEGORY.STA3, TAXCATEGORY.PTR1, TAXCATEGORY.PTR2, TAXCATEGORY.PTR3, TAXCATEGORY.STR1, TAXCATEGORY.STR3, TAXCATEGORY.STR2, DESCRIPTION.remarkreq, Sum(TAXCATEGORY.PTR1+TAXCATEGORY.PTR2) AS PurTaxRate, Sum(TAXCATEGORY.STR1+TAXCATEGORY.STR2) AS SaleTaxRate, DESCRIPTION.Group_id, DESCRIPTION.Department_id, Description.Status, Description.Square_FT, Description.Square_MT, Description.Rebate, Description.Change_des, Description.Srebate FROM DESCRIPTION INNER JOIN TAXCATEGORY ON DESCRIPTION.Tax_Cat_id = TAXCATEGORY.Category_Id GROUP BY DESCRIPTION.Description, DESCRIPTION.Pack, DESCRIPTION.Des_id, DESCRIPTION.Skucode, DESCRIPTION.ShortCode, DESCRIPTION.Pvalue, DESCRIPTION.Rate_Unit, DESCRIPTION.Purchase_rate, DESCRIPTION.[Commission%], DESCRIPTION.[Commission@], DESCRIPTION.Rate_X, DESCRIPTION.Rate_Y, DESCRIPTION.Rate_Z, DESCRIPTION.Retail, DESCRIPTION.MRP, DESCRIPTION.Tax_Cat_id, DESCRIPTION.Wholesale, TAXCATEGORY.PA, TAXCATEGORY.PTA1, TAXCATEGORY.PTA2, TAXCATEGORY.PTA3, TAXCATEGORY.SA, TAXCATEGORY.STA1, TAXCATEGORY.STA2, TAXCATEGORY.STA3, TAXCATEGORY.PTR1, TAXCATEGORY.PTR2, TAXCATEGORY.PTR3, TAXCATEGORY.STR1, TAXCATEGORY.STR3, TAXCATEGORY.STR2, DESCRIPTION.remarkreq, DESCRIPTION.Group_id, DESCRIPTION.Department_id, Description.Status, Description.Square_FT, Description.Square_MT, Description.Rebate, Description.Change_des, Description.Srebate ORDER BY DESCRIPTION.Description, DESCRIPTION.Pack", DescriptionInfo);
        }
        
       
        public static void UpdateCharge()
        {
            Charges = new DataTable();
            Database.GetSqlData("select * from Charges", Charges);
        }

        public static void UpdateTaxCategory()
        {
            TaxCategory = new DataTable();
            Database.GetSqlData("Select * from TaxCategory", TaxCategory);
        }

        public static void UpdateAccountinfo()
        {

            Accountinfo = new DataTable();
            if (Database.BMode == "B")
            {
              //  Database.GetSqlData("select Name,Address1,Address2, CASE WHEN Balance > 0 THEN CAST(Balance AS nvarchar(20)) + ' Dr.' ELSE CAST(- 1 * Balance AS nvarchar(20)) + ' Cr.' END AS Balance,Phone,Tin_number,Accounttype [Account Type], accountgroup [Account Group],Agent,SalesMan from  ( select Name,balance2 + isnull((select sum(Amount) from journal where  Journal.Ac_id=Account.ac_id and Journal.B='true'),0) as Balance, Address1,Address2,Phone,Tin_Number,(select Name from Accountype where act_id=account.act_id) as [Accounttype], (select Name from Other where oth_id=account.loc_id) as [AccountGroup] ,(select Name from Account where Ac_id=account.Con_id) as Agent, (select Name from SalesMan where S_id=account.Salesman_id) as SalesMan  from account ) as MyQry", Accountinfo);
                Database.GetSqlData("select Name,Address1,Address2, CASE WHEN Balance > 0 THEN CAST(Balance AS nvarchar(20)) + ' Dr.' ELSE CAST(- 1 * Balance AS nvarchar(20)) + ' Cr.' END AS Balance,Phone,Tin_number,Accounttype [Account Type], accountgroup [Account Group],SalesMan from  ( select Name,balance2 + isnull((select sum(Amount) from journal where  Journal.Ac_id=Account.ac_id and Journal.B='true'),0) as Balance, Address1,Address2,Phone,Tin_Number,(select Name from Accountype where act_id=account.act_id) as [Accounttype], (select Name from Other where oth_id=account.loc_id) as [AccountGroup] , (select Name from SalesMan where S_id=account.Salesman_id) as SalesMan  from account ) as MyQry", Accountinfo);
            }
            else if (Database.BMode == "AB")
            {
               
               // Database.GetSqlData("select Name,Address1,Address2, CASE WHEN Balance > 0 THEN CAST(Balance AS nvarchar(20)) + ' Dr.' ELSE CAST(- 1 * Balance AS nvarchar(20)) + ' Cr.' END AS Balance,Phone,Tin_number,Accounttype [Account Type], accountgroup [Account Group],Agent,SalesMan from  ( select Name,balance +balance2 + isnull((select sum(Amount) from journal where  Journal.Ac_id=Account.ac_id and Journal.AB='true'),0) as Balance, Address1,Address2,Phone,Tin_Number,(select Name from Accountype where act_id=account.act_id) as [Accounttype], (select Name from Other where oth_id=account.loc_id) as [AccountGroup] ,(select Name from Account where Ac_id=account.Con_id) as Agent, (select Name from SalesMan where S_id=account.Salesman_id) as SalesMan  from account ) as MyQry", Accountinfo);
                Database.GetSqlData("select Name,Address1,Address2, CASE WHEN Balance > 0 THEN CAST(Balance AS nvarchar(20)) + ' Dr.' ELSE CAST(- 1 * Balance AS nvarchar(20)) + ' Cr.' END AS Balance,Phone,Tin_number,Accounttype [Account Type], accountgroup [Account Group],SalesMan from  ( select Name,balance +balance2 + isnull((select sum(Amount) from journal where  Journal.Ac_id=Account.ac_id and Journal.AB='true'),0) as Balance, Address1,Address2,Phone,Tin_Number,(select Name from Accountype where act_id=account.act_id) as [Accounttype], (select Name from Other where oth_id=account.loc_id) as [AccountGroup] , (select Name from SalesMan where S_id=account.Salesman_id) as SalesMan  from account ) as MyQry", Accountinfo);
            }
            else
            {
                
                //Database.GetSqlData("select Name,Address1,Address2,  CASE WHEN Balance > 0 THEN CAST(Balance AS nvarchar(20)) + ' Dr.' ELSE CAST(- 1 * Balance AS nvarchar(20)) + ' Cr.' END AS Balance,Phone,Tin_number,Accounttype [Account Type], accountgroup [Account Group],Agent,SalesMan from  ( select Name,balance + isnull((select sum(Amount) from journal where  Journal.Ac_id=Account.ac_id and Journal.A='true'),0) as Balance, Address1,Address2,Phone,Tin_Number,(select Name from Accountype where act_id=account.act_id) as [Accounttype], (select Name from Other where oth_id=account.loc_id) as [AccountGroup] ,(select Name from Account where Ac_id=account.Con_id) as Agent, (select Name from SalesMan where S_id=account.Salesman_id) as SalesMan  from account ) as MyQry", Accountinfo);
                Database.GetSqlData("select Name,Address1,Address2,  CASE WHEN Balance > 0 THEN CAST(Balance AS nvarchar(20)) + ' Dr.' ELSE CAST(- 1 * Balance AS nvarchar(20)) + ' Cr.' END AS Balance,Phone,Tin_number,Accounttype [Account Type], accountgroup [Account Group],SalesMan from  ( select Name,balance + isnull((select sum(Amount) from journal where  Journal.Ac_id=Account.ac_id and Journal.A='true'),0) as Balance, Address1,Address2,Phone,Tin_Number,(select Name from Accountype where act_id=account.act_id) as [Accounttype], (select Name from Other where oth_id=account.loc_id) as [AccountGroup] , (select Name from SalesMan where S_id=account.Salesman_id) as SalesMan  from account ) as MyQry", Accountinfo);
            }
        }

        public static void UpdateFeature()
        {
            Feature = new DataTable();
            Database.GetSqlData("select * from FirmSetup", Feature);
        }

        public static void UpdateFeatureLogin()
        {
            FeatureLogin = new DataTable();
            Database.GetOtherSqlData("select * from Feature ", FeatureLogin);
        }

        public static void UpdateUserPower()
        {
            UserPower = new DataTable();
            Database.GetOtherSqlData("select * from POWER", UserPower);
        }

        public static void UpdateAll()
        {
           
            UpdateAccount();
           
            UpdateAccountType();
            UpdateVoucherType();
            //UpdateAgent();
            UpdateOther();
            UpdateTransportDetails();
            UpdateCharge();
            
            UpdateAccountinfo();
           
            UpdateFeature();
            UpdateFeatureLogin();
            UpdateUserPower();
            UpdateTaxCategory();
            UpdateState();
            UpdateDecription();
            UpdateDecriptionInfo();
            UpdateControlRoom();
            UpdateState();
            UpdateMenuOptions();
            UpdateRates();
          
        }
    }
}
