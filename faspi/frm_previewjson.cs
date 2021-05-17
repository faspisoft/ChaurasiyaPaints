using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using System.IO;
using TaxProEWB.API;
using Newtonsoft.Json;
using System.Net;

namespace faspi
{
    public partial class frm_previewjson : Form
    {

        ReqGenEwbPl ewbGen = new ReqGenEwbPl();
        TPEWBSession EwbSession = new TPEWBSession();
        long EwbNo = 0;
        clsPLEwayBillList objBills = new clsPLEwayBillList();
        string gvid = "";
       
        DataTable dtcompany = new DataTable();
        DataTable dtvou = new DataTable();

        string LicenceKey = Database.Dongleno ;
        string grno = "";
        string grdate = "";
        public frm_previewjson()
        {
            InitializeComponent();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }

        private void DisplayData()
        {


         //   lbluserGstin.Text = "UserGSTIN : " +  ewbGen.userGstin;
            lblsupplyType.Text = "SupplyType : " + ewbGen.supplyType;
            lblsubSupplyType.Text = "SubSupplyType : " + ewbGen.subSupplyType;

            lbldocType.Text = "DocType : " + ewbGen.docType;
            lbldocNo.Text = "DocumentNo : " + ewbGen.docNo;
            lbldocDate.Text = "Document Date : " + ewbGen.docDate;
            lbltransType.Text = "TransType : " + ewbGen.transactionType;
            lblfromGstin.Text = "GSTIN : " + ewbGen.fromGstin;
            lblfromTrdName.Text = "TraderName : " + ewbGen.fromTrdName;
            lblfromAddr1.Text = "Address1 : " + ewbGen.fromAddr1;
            lblfromAddr2.Text = "Address2 : " + ewbGen.fromAddr2;
            lblfromPlace.Text = "Place : " + ewbGen.fromPlace;
            lblfromPincode.Text = "PinCode : " + ewbGen.fromPincode;
            lblfromStateCode.Text = "StateCode : " + ewbGen.fromStateCode;
            lblactualFromStateCode.Text = "Actual StateCode : " + ewbGen.actFromStateCode;
            lbldispatchfromtrader.Text = "Dispatch From TraderName : " + ewbGen.dispatchFromTradeName;
            lbldispatchfromgstin.Text = "Dispatch From GSTIN : " + ewbGen.dispatchFromGSTIN;

            lbltoGstin.Text = "GSTIN : " + ewbGen.toGstin;
            lbltoTrdName.Text = "TraderName : " + ewbGen.toTrdName;
            lblShiptoTIN.Text = "Ship To GSTIN : " + ewbGen.shipToGSTIN;
            lblshipto.Text = "Ship To TraderName : " + ewbGen.shipToTradeName;
            lbltoAddr1.Text = "Address1 : " + ewbGen.toAddr1;
            lbltoAddr2.Text = "Address2 : " + ewbGen.toAddr2;
            lbltoPlace.Text = "Place : " + ewbGen.toPlace;
            lbltoPincode.Text = "PinCode : " + ewbGen.toPincode;
            lbltoStateCode.Text = "StateCode : " + ewbGen.toStateCode;
            lblactualToStatecode.Text = "Actual StateCode : " + ewbGen.actToStateCode;

            lbltotalValue.Text = "Total Value : " + ewbGen.totalValue;
            lblcgstValue.Text = "CGST Value : " + ewbGen.cgstValue;
            lblsgstValue.Text = "SGST Value : " + ewbGen.sgstValue;
            lbligstValue.Text = "IGST Value : " + ewbGen.igstValue;
            lblcessValue.Text = "Cess Value : " + ewbGen.cessValue;

            lblTotNonAdvolVal.Text = "Total NonAdvolVal : " + ewbGen.cessNonAdvolValue;
            lblOthValue.Text = "Other Value : " + ewbGen.otherValue;
            lbltotalValue.Text = "Total Value :" + ewbGen.totalValue;
            lbltotInvValue.Text = "Total Invoice Value : " + ewbGen.totInvValue;
            lbltransMode.Text = "Trans Mode : " + ewbGen.transMode;
            lbltransDistance.Text = "Distance : " + ewbGen.transDistance;

            lbltransporterName.Text = "Transporter Name : " + ewbGen.transporterName;
            lbltransporterId.Text = "Transporter ID : " + ewbGen.transporterId;
            lbltransDocNo.Text = "Transaction Doc No : " + ewbGen.transDocNo;
            lbltransDocDate.Text = "Transaction Doc Date : " + ewbGen.transDocDate;

            lblvehicleNo.Text = "Vehicle No : " + ewbGen.vehicleNo;
            lblvehicleType.Text = "Vehicle Type : " + ewbGen.vehicleType;



            ansGridView5.DataSource = ewbGen.itemList;
        }

        public bool LoadData(string vid)
        {
            gvid = vid;
           
           
          //  objBills.billLists.Add(obj);
            
            Database.GetSqlData("Select * from Company", dtcompany);
            if (dtcompany.Rows.Count == 0)
            {
                MessageBox.Show("Please Fill Company Details");
                return false;
            }
            else if (dtcompany.Rows[0]["Tin_no"].ToString() == "" || dtcompany.Rows[0]["Tin_no"].ToString() == "0")
            {
                MessageBox.Show("Please Fill Company  GSTIN");
                return false;
            }
            else if (dtcompany.Rows[0]["City_id"].ToString() == "" || dtcompany.Rows[0]["City_id"].ToString() == "0")
            {
                MessageBox.Show("Please Fill Company  City");
                return false;
            }
            else if (dtcompany.Rows[0]["Pincode"].ToString() == "" || dtcompany.Rows[0]["Pincode"].ToString() == "0")
            {
                MessageBox.Show("Please Fill Company  PINCODE");
                return false;
            }

            Database.GetSqlData("SELECT   TransDocno,TransDocDate,TransVehNo, dispatch_id,ShiptoTIN,Shipto,EwayBillno, VOUCHERINFO.Invoiceno, VOUCHERINFO.Vdate, ACCOUNT.Name AS Party, City.CName, State.Sname AS BilltoState, State_1.Sname AS ShiptoState, Sum(Voucherdet.Taxabelamount) AS Taxable, Sum(Voucherdet.taxamt1) AS Cgst, Sum(Voucherdet.taxamt2) AS SGST, Sum(Voucherdet.taxamt3) AS IGST, VOUCHERINFO.Totalamount AS Invamount, VOUCHERINFO.ShiptoDistance AS Distance, ACCOUNT_1.Name AS Transporter, ACCOUNT_1.Tin_number AS TransporterGSTIN FROM ((((((VOUCHERINFO LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id) LEFT JOIN Voucherdet ON VOUCHERINFO.Vi_id = Voucherdet.Vi_id) LEFT JOIN ACCOUNT ON VOUCHERINFO.Ac_id = ACCOUNT.Ac_id) LEFT JOIN City ON VOUCHERINFO.ShiptoCity_id = City.City_id) LEFT JOIN State ON ACCOUNT.State_id = State.State_id) LEFT JOIN ACCOUNT AS ACCOUNT_1 ON VOUCHERINFO.Transporter_id = ACCOUNT_1.Ac_id) LEFT JOIN State AS State_1 ON VOUCHERINFO.ShiptoStateid = State_1.State_id GROUP BY  dispatch_id,ShiptoTIN,Shipto,   TransDocno,TransDocDate,TransVehNo, VOUCHERINFO.Invoiceno, VOUCHERINFO.Vdate, ACCOUNT.Name, City.CName, State.Sname, State_1.Sname, VOUCHERINFO.Totalamount, VOUCHERINFO.ShiptoDistance, ACCOUNT_1.Name, ACCOUNT_1.Tin_number, VOUCHERINFO.Vi_id, VOUCHERTYPE.Type,EwayBillno, VOUCHERTYPE." + Database.BMode + " HAVING (((VOUCHERINFO.Vi_id)='" + vid + "') AND ((VOUCHERTYPE.Type)='Sale') AND ((VOUCHERTYPE." + Database.BMode + ")=" + access_sql.Singlequote + "True" + access_sql.Singlequote + ")); ", dtvou);

            if (validateewaybill(dtvou) == true)
            {
             


               

               

                //DataTable dtveh = new DataTable();
                //Database.GetSqlData("Select * from Transportdetails where IsVehicleNo=" + access_sql.Singlequote + "true" + access_sql.Singlequote, dtveh);
                //string fieldname = "";
                //if (dtveh.Rows.Count == 1)
                //{
                //    if (dtveh.Rows[0]["FName"].ToString() == "Field1")
                //    {
                //        fieldname = "Transport1";
                //    }
                //    else if (dtveh.Rows[0]["FName"].ToString() == "Field2")
                //    {
                //        fieldname = "Transport2";
                //    }
                //    else if (dtveh.Rows[0]["FName"].ToString() == "Field4")
                //    {
                //        fieldname = "Grno";
                //    }
                //    else if (dtveh.Rows[0]["FName"].ToString() == "Field3")
                //    {
                //        fieldname = "DeliveryAt";
                //    }

                //    else if (dtveh.Rows[0]["FName"].ToString() == "Field5")
                //    {
                //        fieldname = "Transport3";
                //    }
                //    else if (dtveh.Rows[0]["FName"].ToString() == "Field6")
                //    {
                //        fieldname = "Transport4";
                //    }
                //    else if (dtveh.Rows[0]["FName"].ToString() == "Field7")
                //    {
                //        fieldname = "Transport5";
                //    }
                //    else if (dtveh.Rows[0]["FName"].ToString() == "Field8")
                //    {
                //        fieldname = "Transport6";
                //    }
                //}


              

                
               


                FillJSONObj();
                DisplayData();

                if (dtvou.Rows[0]["EwayBillno"].ToString() != "")
                {
                    EwbNo = long.Parse(dtvou.Rows[0]["EwayBillno"].ToString());
                }

                SideFill();

                return true;

            }
            else
            {
                return false;
            }

        }

        private void ExportJson()
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            string retVal = String.Empty;
            using (MemoryStream ms = new MemoryStream())
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(objBills.GetType());
                serializer.WriteObject(ms, objBills);
                var byteArray = ms.ToArray();
                retVal = Encoding.UTF8.GetString(byteArray, 0, byteArray.Length);
            }

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string jsonpath = saveFileDialog1.FileName;

                File.Create(jsonpath + ".json").Dispose();
                TextWriter tw = new StreamWriter(jsonpath + ".json");


                tw.WriteLine(retVal);
                tw.Close();
               
                MessageBox.Show("JSON Created Successfully");
               
            }
        }

        private bool validateewaybill(DataTable dt)
        {





            if (dt.Rows[0]["Cname"].ToString() == "")
            {
                MessageBox.Show("Enter City in this voucher");
                return false;
            }

            if (double.Parse(dt.Rows[0]["distance"].ToString()) == 0 || double.Parse(dt.Rows[0]["distance"].ToString()) < 0)
            {
                MessageBox.Show("Enter Valid Distance in this voucher");
                return false;
            }
           
            if (funs.Select_Pincode(dt.Rows[0]["Party"].ToString()) == "" || funs.Select_Pincode(dt.Rows[0]["Party"].ToString()) == "0")
            {
                MessageBox.Show("Enter Party's PINCODE of this voucher");
                return false;
            }


            //DataTable dtveh = new DataTable();
            //Database.GetSqlData("Select * from Transportdetails where IsVehicleNo=" + access_sql.Singlequote + "true" + access_sql.Singlequote, dtveh);
            //string fieldname = "";
            //if (dtveh.Rows.Count == 1)
            //{
            //    if (dtveh.Rows[0]["FName"].ToString() == "Field1")
            //    {
            //        fieldname = "Transport1";
            //    }
            //    else if (dtveh.Rows[0]["FName"].ToString() == "Field2")
            //    {
            //        fieldname = "Transport2";
            //    }
            //    else if (dtveh.Rows[0]["FName"].ToString() == "Field4")
            //    {
            //        fieldname = "Grno";
            //    }
            //    else if (dtveh.Rows[0]["FName"].ToString() == "Field3")
            //    {
            //        fieldname = "DeliveryAt";
            //    }

            //    else if (dtveh.Rows[0]["FName"].ToString() == "Field5")
            //    {
            //        fieldname = "Transport3";
            //    }
            //    else if (dtveh.Rows[0]["FName"].ToString() == "Field6")
            //    {
            //        fieldname = "Transport4";
            //    }
            //    else if (dtveh.Rows[0]["FName"].ToString() == "Field7")
            //    {
            //        fieldname = "Transport5";
            //    }

            //    else if (dtveh.Rows[0]["FName"].ToString() == "Field8")
            //    {
            //        fieldname = "Transport6";
            //    }
            //}

            string vehicleno = "",transportergstin="";
            vehicleno = dt.Rows[0]["TransVehNo"].ToString();
            transportergstin = dt.Rows[0]["TransporterGSTIN"].ToString();
            if (vehicleno == "" && transportergstin == "")
            {
                MessageBox.Show("Enter TransporterGSTIN or VehicleNo in this voucher");
                return false;
            }

              

            return true;
        }

        private void frm_previewjson_Load(object sender, EventArgs e)
        {
            CheckEwayBalance(false);
            
        }

        private void SideFill()
        {
            flowLayoutPanel1.Controls.Clear();
            DataTable dtsidefill = new DataTable();
            dtsidefill.Columns.Add("Name", typeof(string));
            dtsidefill.Columns.Add("DisplayName", typeof(string));
            dtsidefill.Columns.Add("ShortcutKey", typeof(string));
            dtsidefill.Columns.Add("Visible", typeof(bool));
            //JSON
            dtsidefill.Rows.Add();
            dtsidefill.Rows[0]["Name"] = "JSON";
            dtsidefill.Rows[0]["DisplayName"] = "Create JSON";
            dtsidefill.Rows[0]["ShortcutKey"] = "^J";
            dtsidefill.Rows[0]["Visible"] = false;
            
            //OnlineEWB
            dtsidefill.Rows.Add();
            dtsidefill.Rows[1]["Name"] = "OnlineEWB";
            dtsidefill.Rows[1]["DisplayName"] = "Online EWB";
            dtsidefill.Rows[1]["ShortcutKey"] = "^O";

            if (EwbNo.ToString() != "0")
            {
                dtsidefill.Rows[1]["Visible"] = false;
            }
            else
            {

                dtsidefill.Rows[1]["Visible"] = true; 
            }


            dtsidefill.Rows.Add();
            dtsidefill.Rows[2]["Name"] = "printewb";
            dtsidefill.Rows[2]["DisplayName"] = "Print EWB";
            dtsidefill.Rows[2]["ShortcutKey"] = "^P";


            if (EwbNo.ToString() != "0")
            {
                dtsidefill.Rows[2]["Visible"] = true;
            }
            else
            {
                dtsidefill.Rows[2]["Visible"] = false;
            }

            //close
            dtsidefill.Rows.Add();
            dtsidefill.Rows[3]["Name"] = "quit";
            dtsidefill.Rows[3]["DisplayName"] = "Quit";
            dtsidefill.Rows[3]["ShortcutKey"] = "Esc";
            dtsidefill.Rows[3]["Visible"] = true;






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
                    Rectangle RC = btn.ClientRectangle;
                    Font font = new Font("Arial", 12);
                    G.DrawString(line1, font, Brushes.Red, RC, SF);
                    G.DrawString("".PadLeft(line1.Length * 2 + 1) + line2, font, Brushes.Black, RC, SF);

                    btn.Image = bmp;

                    btn.Click += new EventHandler(btn_Click);
                    flowLayoutPanel1.Controls.Add(btn);
                }

            }

        }

        bool CheckEwayBalance(bool isShoMsg=true) {

            FaspiApiHandler.BLEwaybill objEway = new FaspiApiHandler.BLEwaybill();
            long iRes = objEway.GetRemainEwayBill(LicenceKey);

            groupBox1.Text = "Basic Detail";
            lblAPIBalannce.Text = "API Balance : " + iRes.ToString();

            if (objEway.StatusCode.ToLower() == "ok" && iRes > 0)
            {               
                return true ;
            }
            else if (objEway.StatusCode.ToLower() == "ok" && iRes <= 0 && isShoMsg ==true )
            {
                MessageBox.Show(this, "Insufficient Api Balance.", "Eway Bill", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if(isShoMsg ==true)
            {
                MessageBox.Show(this, "Something Went Wrong. Check Your Network Connection, Try Again.","Eway Bill", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
                        
            return false;
        
        }

        bool UpdateEwayBalance(long Qty,string Remarks)
        {
            FaspiApiHandler.BLEwaybill objEway = new FaspiApiHandler.BLEwaybill();
            string strRes = objEway.AddUseQuantity(LicenceKey, Qty, Remarks);
            if (objEway.StatusCode.ToLower() == "ok")
            {
                return true;
            }

            return false;

        }
         void btn_Click(object sender, EventArgs e)
         {
                Button tbtn = (Button)sender;
                string name = tbtn.Name.ToString();

            if (name == "JSON")
            {
                ExportJson();
            }

            if (name == "quit")
            {
                this.Close();
                this.Dispose();
            }
            if (name == "OnlineEWB")
            {
                if (CheckEwayBalance()==false ) { return; }

                OnlineewayBill();

                CheckEwayBalance(false);

            }
            if (name == "printewb")
            {
                if (CheckEwayBalance()==false) { return; }

                PrintewayBill();


            }
        }
         private async void PrintewayBill()
         {

             string strJSON = "";

             if (DateTime.Now < EwbSession.EwbApiLoginDetails.EwbTokenExp)
             {
                 lblResponce.Text = EwbSession.EwbApiLoginDetails.EwbAuthToken + "valid Upto:" + EwbSession.EwbApiLoginDetails.EwbTokenExp.ToString();
             }
             else
             {
                 TxnRespWithObj<EWBSession> TxnSesResp = await EWBAPI.GetAuthTokenAsync(EwbSession);
                 if (TxnSesResp.IsSuccess == true)
                 {
                     lblResponce.Text = TxnSesResp.RespObj.EwbApiLoginDetails.EwbAuthToken;
                     strJSON = JsonConvert.SerializeObject(TxnSesResp.RespObj.EwbApiLoginDetails);
                     Database.CommandExecutor("Update Company set EwbLoginDetail='" + strJSON + "'");
                     UpdateEwayBalance(-1, "On Print Auth");
                     CheckEwayBalance(false);
                 }
                 else
                 {
                     lblResponce.Text = TxnSesResp.TxnOutcome;
                     CheckEwayBalance(false);
                     return;
                 }

             }

            
             string rtbResponce = "";
             TxnRespWithObj<RespGetEWBDetail> TxnResp = await EWBAPI.GetEWBDetailAsync(EwbSession, EwbNo);
             if (TxnResp.IsSuccess == true)
             {
                 UpdateEwayBalance(-1, "On Print Detail");
                 EWBAPI.PrintEWB(EwbSession, TxnResp.RespObj, "", true, true);
                 UpdateEwayBalance(-1, "On Print");

             }
             else
             {
                 rtbResponce =  TxnResp.TxnOutcome;
                 UpdateEwayBalance(-1, TxnResp.TxnOutcome);
             }

             CheckEwayBalance(false);
          
         }

         private async void OnlineewayBill()
         {
             //GetAuthenticationToken();

             string strJSON = "";

             if (DateTime.Now < EwbSession.EwbApiLoginDetails.EwbTokenExp)
             {
                 lblResponce.Text = EwbSession.EwbApiLoginDetails.EwbAuthToken + "valid Upto:" + EwbSession.EwbApiLoginDetails.EwbTokenExp.ToString();
             }
             else
             {
                 TxnRespWithObj<EWBSession> TxnSesResp = await EWBAPI.GetAuthTokenAsync(EwbSession);
                 if (TxnSesResp.IsSuccess == true)
                 {
                     lblResponce.Text = TxnSesResp.RespObj.EwbApiLoginDetails.EwbAuthToken;
                     strJSON = JsonConvert.SerializeObject(TxnSesResp.RespObj.EwbApiLoginDetails);
                      Database.CommandExecutor("Update Company set EwbLoginDetail='" + strJSON + "'");
                      UpdateEwayBalance(-1, "On Generate Eway Auth");
                      CheckEwayBalance(false);
                 }
                 else
                 {
                     lblResponce.Text = TxnSesResp.TxnOutcome;
                     UpdateEwayBalance(-1, TxnSesResp.TxnOutcome);
                     CheckEwayBalance(false);
                     return;
                 }
             }



             strJSON = JsonConvert.SerializeObject(ewbGen);
           //  System.IO.File.WriteAllText(Applidispcation.StartupPath + "\\EWAYJSON.txt", strJSON);

             //strJSON = System.IO.File.ReadAllText(Application.StartupPath + "\\EWAYJSON.txt");

             if (DateTime.Now > EwbSession.EwbApiLoginDetails.EwbTokenExp) return;


             TxnRespWithObj<RespGenEwbPl> TxnResp = await EWBAPI.GenEWBAsync(EwbSession, strJSON);

             if (TxnResp.IsSuccess)
             {
                 lblResponce.Text = JsonConvert.SerializeObject(TxnResp.RespObj);
                 Database.CommandExecutor("Update voucherinfo set EwayBillno='" + TxnResp.RespObj.ewayBillNo + "' where vi_id='" + gvid+"'");
                 EwbNo = long.Parse(TxnResp.RespObj.ewayBillNo);

                 UpdateEwayBalance(-1, TxnResp.RespObj.ewayBillNo);

             }
             else
             {
                 lblResponce.Text =lblResponce.Text + Environment.NewLine + TxnResp.TxnOutcome;
                 UpdateEwayBalance(-1, TxnResp.TxnOutcome);
             }

             CheckEwayBalance(false);
             SideFill();
         }
         private void FillJSONObj()
         {

        
             ewbGen.supplyType = "O";

             ewbGen.transactionType = 1;
             ewbGen.subSupplyType = "1";

             ewbGen.subSupplyDesc = "";
             ewbGen.docType = "INV";

             ewbGen.docNo = dtvou.Rows[0]["Invoiceno"].ToString();
             ewbGen.docDate = DateTime.Parse(dtvou.Rows[0]["vdate"].ToString()).ToString("dd/MM/yyyy").Replace('-', '/');

             ewbGen.fromGstin = dtcompany.Rows[0]["Tin_no"].ToString();
             ewbGen.fromTrdName = dtcompany.Rows[0]["Name"].ToString();
             ewbGen.fromAddr1 = dtcompany.Rows[0]["Address1"].ToString();
             ewbGen.fromAddr2 = dtcompany.Rows[0]["Address2"].ToString();
             ewbGen.fromPlace = funs.Select_city_name(dtcompany.Rows[0]["City_id"].ToString());

             ewbGen.fromPincode = int.Parse(dtcompany.Rows[0]["Pincode"].ToString());

             ewbGen.fromStateCode = int.Parse(funs.Select_state_GST(funs.Select_state_nm(dtcompany.Rows[0]["Cstate_id"].ToString())));
             ewbGen.actFromStateCode = int.Parse(funs.Select_state_GST(funs.Select_state_nm(dtcompany.Rows[0]["Cstate_id"].ToString())));
             if (dtvou.Rows[0]["transDocNo"].ToString() == "")
             {
                 ewbGen.transDocNo = "";
                 ewbGen.transDocDate = "";
             }
             else
             {
                 ewbGen.transDocNo = dtvou.Rows[0]["transDocNo"].ToString();
                 ewbGen.transDocDate = DateTime.Parse(dtvou.Rows[0]["transDocDate"].ToString()).ToString("dd/MM/yyyy").Replace('-', '/');


             }
            


             //if (dtvou.Rows[0]["transDocDate"].ToString() == "")
             //{
             //    ewbGen.transDocDate = "";
             //}
             //else
             //{
                

             //}
             //to
             if (funs.Select_TIN(dtvou.Rows[0]["Party"].ToString()) == "0" || funs.Select_TIN(dtvou.Rows[0]["Party"].ToString()) == "")
             {
                 ewbGen.toGstin = "URP";
             }
             else
             {
                 ewbGen.toGstin = funs.Select_TIN(dtvou.Rows[0]["Party"].ToString());
             }

             ewbGen.toTrdName = dtvou.Rows[0]["Party"].ToString();
             ewbGen.toAddr1 = funs.Select_Address1(dtvou.Rows[0]["Party"].ToString());
             ewbGen.toAddr2 = funs.Select_Address2(dtvou.Rows[0]["Party"].ToString());
             ewbGen.toPlace = dtvou.Rows[0]["CNAMe"].ToString();


             ewbGen.toPincode = int.Parse(funs.Select_Pincode(dtvou.Rows[0]["Party"].ToString()));
             //doubt


             ewbGen.toStateCode = int.Parse(funs.Select_state_GST(dtvou.Rows[0]["billtostate"].ToString()));


             if (dtvou.Rows[0]["dispatch_id"].ToString() == "0")
             {
                 ewbGen.dispatchFromTradeName = dtcompany.Rows[0]["Name"].ToString(); ;
                 ewbGen.dispatchFromGSTIN = dtcompany.Rows[0]["Tin_no"].ToString(); ;
             }
             else
             {
                 ewbGen.dispatchFromTradeName = funs.Select_ac_nm(dtvou.Rows[0]["dispatch_id"].ToString());
                 ewbGen.dispatchFromGSTIN = funs.Select_TIN(funs.Select_ac_nm(dtvou.Rows[0]["dispatch_id"].ToString()));
             }

            
          
             if (dtvou.Rows[0]["ShiptoTIN"].ToString() == "0" || dtvou.Rows[0]["ShiptoTIN"].ToString() == "")
             {
                 ewbGen.shipToGSTIN = "URP";
             }
             else
             {
                 ewbGen.shipToGSTIN = dtvou.Rows[0]["ShiptoTIN"].ToString();
             }


             //ewbGen.shipToGSTIN = ewbGen.toGstin;
             ewbGen.shipToTradeName = dtvou.Rows[0]["Shipto"].ToString(); ;
             //shipto state
             ewbGen.actToStateCode = int.Parse(funs.Select_state_GST(dtvou.Rows[0]["shiptostate"].ToString()));



             long lDistance = 0;
             double dDistance = 0;

             double.TryParse( dtvou.Rows[0]["distance"].ToString(),out dDistance );
             long.TryParse(dDistance.ToString("0"), out lDistance);

             
             ewbGen.totalValue = double.Parse(dtvou.Rows[0]["Taxable"].ToString());
             ewbGen.cgstValue = double.Parse(dtvou.Rows[0]["cgst"].ToString());
             ewbGen.sgstValue = double.Parse(dtvou.Rows[0]["sgst"].ToString());
             ewbGen.igstValue = double.Parse(dtvou.Rows[0]["igst"].ToString());
             ewbGen.cessValue = 0;
             ewbGen.cessNonAdvolValue = 0;
             ewbGen.otherValue = 0;
             ewbGen.totInvValue = double.Parse(dtvou.Rows[0]["Invamount"].ToString());




          
             ewbGen.transDistance = lDistance.ToString();  //ASK FROM CLIENT
             ewbGen.transporterName = dtvou.Rows[0]["transporter"].ToString();


             ewbGen.transporterId = dtvou.Rows[0]["TransporterGSTIN"].ToString();  //ASK
             //ewbGen.transDocNo = "";
             //ewbGen.transDocDate = "";

             //DataTable dtveh = new DataTable();
             //Database.GetSqlData("Select * from Transportdetails where IsVehicleNo=" + access_sql.Singlequote + "true" + access_sql.Singlequote, dtveh);
             //string fieldname = "";
             //if (dtveh.Rows.Count == 1)
             //{
             //    if (dtveh.Rows[0]["FName"].ToString() == "Field1")
             //    {
             //        fieldname = "Transport1";
             //    }
             //    else if (dtveh.Rows[0]["FName"].ToString() == "Field2")
             //    {
             //        fieldname = "Transport2";
             //    }
             //    else if (dtveh.Rows[0]["FName"].ToString() == "Field4")
             //    {
             //        fieldname = "Grno";
             //    }
             //    else if (dtveh.Rows[0]["FName"].ToString() == "Field3")
             //    {
             //        fieldname = "DeliveryAt";
             //    }

             //    else if (dtveh.Rows[0]["FName"].ToString() == "Field5")
             //    {
             //        fieldname = "Transport3";
             //    }
             //    else if (dtveh.Rows[0]["FName"].ToString() == "Field6")
             //    {
             //        fieldname = "Transport4";
             //    }
             //    else if (dtveh.Rows[0]["FName"].ToString() == "Field7")
             //    {
             //        fieldname = "Transport5";
             //    }
             //    else if (dtveh.Rows[0]["FName"].ToString() == "Field8")
             //    {
             //        fieldname = "Transport6";
             //    }
             //}


             //if (dtvou.Rows[0][fieldname].ToString() != "")
             //{
                 //if (MessageBox.Show("Vehicle No","Is, It is not your Vehicle No:" +  dtvou.Rows[0]["Transport2"].ToString(), MessageBoxButtons.YesNo) == DialogResult.Yes)
                 //{
             ewbGen.vehicleNo = dtvou.Rows[0]["TransVehNo"].ToString();
                 //}
                 //else
                 //{
                 //    return;
                 //}
             //}
             //else
             //{
             //    ewbGen.vehicleNo = "";
             //}

             if (ewbGen.vehicleNo == "")
             {
                 ewbGen.transMode = "";
             }
             else
             {
                 ewbGen.transMode = "1";
             }

         
             ewbGen.vehicleType = "R";
             //doubtfirst item hsn code
             DataTable dtvou1 = new DataTable();

            
                 if (Feature.Available("Eway Bill On Quantity") != "Quantity")
                 {
                     Database.GetSqlData("SELECT  TAXCATEGORY.Commodity_Code,  TAXCATEGORY.Category_Name, Sum(VOUCHERDET.weight) as QtyinLtr, SUM( Voucherdet.Taxabelamount)    AS Taxableamt,  Voucherdet.rate1,  Voucherdet.rate2,  Voucherdet.rate3,  Voucherdet.rate4, Voucherdet.Rate_Unit FROM  Voucherdet LEFT OUTER JOIN  TAXCATEGORY ON  Voucherdet.Category_Id =  TAXCATEGORY.Category_Id WHERE ( Voucherdet.Vi_id = '" + gvid + "') GROUP BY  TAXCATEGORY.Commodity_Code,  TAXCATEGORY.Category_Name,   Voucherdet.rate1,  Voucherdet.rate2,   Voucherdet.rate3,  Voucherdet.rate4,  Voucherdet.Rate_Unit ORDER BY Sum(Voucherdet.Taxabelamount) DESC", dtvou1);
                 }
                 else
                 {
                     Database.GetSqlData("SELECT  TAXCATEGORY.Commodity_Code,  TAXCATEGORY.Category_Name, Sum(VOUCHERDET.quantity*VOUCHERDET.Pvalue) as QtyinLtr, SUM( Voucherdet.Taxabelamount)    AS Taxableamt,  Voucherdet.rate1,  Voucherdet.rate2,  Voucherdet.rate3,  Voucherdet.rate4, Voucherdet.Rate_Unit FROM  Voucherdet LEFT OUTER JOIN  TAXCATEGORY ON  Voucherdet.Category_Id =  TAXCATEGORY.Category_Id WHERE ( Voucherdet.Vi_id = '" + gvid + "') GROUP BY  TAXCATEGORY.Commodity_Code,  TAXCATEGORY.Category_Name,   Voucherdet.rate1,  Voucherdet.rate2,   Voucherdet.rate3,  Voucherdet.rate4,  Voucherdet.Rate_Unit ORDER BY Sum(Voucherdet.Taxabelamount) DESC", dtvou1);
                 }
            


             //ewbGen.mainHsnCode = int.Parse(dtvou1.Rows[0]["Commodity_Code"].ToString());

             ewbGen.itemList = new List<ReqGenEwbPl.ItemListInReqEWBpl>();

             for (int i = 0; i < dtvou1.Rows.Count; i++)
             {

                 if (Feature.Available("Eway Bill On Quantity") == "Quantity")
                 {



                     if (dtvou1.Rows[i]["Rate_Unit"].ToString().Length < 3)
                     {

                         MessageBox.Show(dtvou1.Rows[i]["Category_Name"].ToString() + " GST Unit Should be Minimum in 3 Char : " + dtvou1.Rows[i]["Rate_Unit"].ToString());
                         //  return false;
                     }

                     ewbGen.itemList.Add(new ReqGenEwbPl.ItemListInReqEWBpl
                     {
                         productName = dtvou1.Rows[i]["Category_Name"].ToString(),
                         productDesc = dtvou1.Rows[i]["Category_Name"].ToString(),
                         hsnCode = int.Parse(dtvou1.Rows[i]["Commodity_Code"].ToString()),
                         quantity = double.Parse(dtvou1.Rows[i]["QtyinLtr"].ToString()),

                         qtyUnit = dtvou1.Rows[i]["Rate_Unit"].ToString().Substring(0, 3),
                         taxableAmount = double.Parse(dtvou1.Rows[i]["taxableamt"].ToString()),
                         sgstRate = double.Parse(dtvou1.Rows[i]["rate2"].ToString()),
                         cgstRate = double.Parse(dtvou1.Rows[i]["rate1"].ToString()),
                         igstRate = double.Parse(dtvou1.Rows[i]["rate3"].ToString()),
                         cessRate = 0,
                         cessNonAdvol = 0
                     });
                 }
                 else
                 {
                     ewbGen.itemList.Add(new ReqGenEwbPl.ItemListInReqEWBpl
                     {
                         productName = dtvou1.Rows[i]["Category_Name"].ToString(),
                         productDesc = dtvou1.Rows[i]["Category_Name"].ToString(),
                         hsnCode = int.Parse(dtvou1.Rows[i]["Commodity_Code"].ToString()),
                         quantity = double.Parse(dtvou1.Rows[i]["QtyinLtr"].ToString()),

                         qtyUnit = Feature.Available("Eway Bill Unit if Quantity in Weight"),
                         taxableAmount = double.Parse(dtvou1.Rows[i]["taxableamt"].ToString()),
                         sgstRate = double.Parse(dtvou1.Rows[i]["rate2"].ToString()),
                         cgstRate = double.Parse(dtvou1.Rows[i]["rate1"].ToString()),
                         igstRate = double.Parse(dtvou1.Rows[i]["rate3"].ToString()),
                         cessRate = 0,
                         cessNonAdvol = 0
                     });
                 }
             }
           
           // return JsonConvert.SerializeObject(ewbGen);
            
         }

         private void frm_previewjson_KeyDown(object sender, KeyEventArgs e)
         {
             if (e.KeyCode == Keys.Escape)
            {
               
                    this.Close();
                    this.Dispose();
             

            }
             //if (e.Control &&  e.KeyCode == Keys.J)
             //{



             //    ExportJson();


             //}


         }

         private void groupBox5_Enter(object sender, EventArgs e)
         {

         }



        }




    }

