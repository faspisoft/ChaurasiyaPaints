using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using TaxProEWB.API;
using System.Text.RegularExpressions;
using System.Net;
using System.IO;
using System.Net.Cache;
using System.Web.Script.Serialization;

namespace faspi
{
    public partial class frm_gstindet : Form
    {
        string LicenceKey = Database.Dongleno;
        public TxnRespWithObj<GSTINDetail> TxnResp;
        public GstinDetail obj;
        public frm_gstindet()
        {
            InitializeComponent();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;  
        }


        private bool Validate()
        {
         //   Regex obj = new Regex("^[0-9]{2}[A-Z]{5}[0-9]{4}[A-Z]{1}[0-9A-Z]{1}Z[0-9A-Z]{1}$");
            Regex obj = new Regex("^[0-9]{2}[A-Z]{5}[0-9]{4}[A-Z]{1}[0-9A-Z]{2}[0-9A-Z]{1}$");
            if (textBox9.Text.Trim() == "" || textBox9.Text == "0")
            {
                textBox9.Focus();
                return false;
            }
            else if (obj.IsMatch(textBox9.Text) == false)
            {
                MessageBox.Show("GSTIN is Not Correct");
                return false;
            }
            return true;
        }

        private void frm_gstindet_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
                this.Dispose();
            }
        }
        bool UpdateEwayBalance(long Qty, string Remarks)
        {
            FaspiApiHandler.BLEwaybill objEway = new FaspiApiHandler.BLEwaybill();
            string strRes = objEway.AddUseQuantity(LicenceKey, Qty, Remarks);
            if (objEway.StatusCode.ToLower() == "ok")
            {
                return true;
            }

            return false;

        }


        bool CheckEwayBalance(bool isShoMsg = true)
        {

            FaspiApiHandler.BLEwaybill objEway = new FaspiApiHandler.BLEwaybill();
            long iRes = objEway.GetRemainEwayBill(LicenceKey);

        
            lblAPIBalannce.Text = "API Balance : " + iRes.ToString();

            if (objEway.StatusCode.ToLower() == "ok" && iRes > 0)
            {
                return true;
            }
            else if (objEway.StatusCode.ToLower() == "ok" && iRes <= 0 && isShoMsg == true)
            {
                MessageBox.Show(this, "Insufficient Api Balance.", "Eway Bill", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (isShoMsg == true)
            {
                MessageBox.Show(this, "Something Went Wrong. Check Your Network Connection, Try Again.", "Eway Bill", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return false;

        }
        private async void button1_Click(object sender, EventArgs e)
        {
            //if (Validate() == true)
            //{

            //    TPEWBSession EwbSession = new TPEWBSession();

            //    string strJSON = "";

            //    if (DateTime.Now < EwbSession.EwbApiLoginDetails.EwbTokenExp)
            //    {
            //        lblResponce.Text = EwbSession.EwbApiLoginDetails.EwbAuthToken + "valid Upto:" + EwbSession.EwbApiLoginDetails.EwbTokenExp.ToString();
            //    }
            //    else
            //    {

            //        TxnRespWithObj<EWBSession> TxnSesResp = await EWBAPI.GetAuthTokenAsync(EwbSession);
            //        if (TxnSesResp.IsSuccess == true)
            //        {
            //            lblResponce.Text = TxnSesResp.RespObj.EwbApiLoginDetails.EwbAuthToken;
            //            strJSON = JsonConvert.SerializeObject(TxnSesResp.RespObj.EwbApiLoginDetails);
            //            Database.CommandExecutor("Update Company set EwbLoginDetail='" + strJSON + "'");
            //            UpdateEwayBalance(-1, "On Generate Eway Auth");
            //            CheckEwayBalance(false);
            //        }
            //        else
            //        {
            //            lblResponce.Text = TxnSesResp.TxnOutcome;
            //            UpdateEwayBalance(-1, TxnSesResp.TxnOutcome);
            //            CheckEwayBalance(false);
            //            return;
            //        }
            //    }


            //    if (DateTime.Now > EwbSession.EwbApiLoginDetails.EwbTokenExp) return;

            //    string GSTIN = textBox9.Text;
            //    TxnResp = await EWBAPI.GetGSTNDetailAsync(EwbSession, GSTIN);
            //    if (TxnResp.IsSuccess)
            //    {
            //        lblResponce.Text = JsonConvert.SerializeObject(TxnResp.RespObj);
            //        UpdateEwayBalance(-1, GSTIN + " Details");

            //        this.Close();
            //    }
            //    else
            //    {
            //        lblResponce.Text = TxnResp.TxnOutcome;
            //        UpdateEwayBalance(-1, GSTIN + " Details " + TxnResp.TxnOutcome);
            //    }
            //    CheckEwayBalance(false);

              
                
            //}
        }

        private void frm_gstindet_Load(object sender, EventArgs e)
        {
            if (CheckEwayBalance() == false) { return; }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (Validate() == true && CheckEwayBalance(false) == true)
            {
                String address = "https://api.taxprogsp.co.in/commonapi/v1.1/search?aspid=1616963119&password=marwari123@&action=TP&Gstin=" + textBox9.Text;
                WebRequest webRequest = WebRequest.Create(address);
                webRequest.CachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);
                WebResponse webResponse;
                try
                {
                    webResponse = webRequest.GetResponse();
                    Stream stream = webResponse.GetResponseStream();
                    StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                    String str = reader.ReadToEnd();

                   // String str = "{\"stjCd\":\"UP1793\",\"dty\":\"Regular\",\"lgnm\":\"FASPI ENTERPRISES PRIVATE LIMITED\",\"stj\":\"Muzaffar Nagar Sector-1\",\"adadr\":[{\"addr\":{\"bnm\":\"VORBIT SPACES PVT. LTD.\",\"st\":\"SECTOR-4\",\"loc\":\"NOIDA\",\"bno\":\"A-31\",\"dst\":\"Gautam Buddha Nagar\",\"stcd\":\"Uttar Pradesh\",\"city\":\"\",\"flno\":\"1ST FLOOR\",\"lt\":\"\",\"pncd\":\"201301\",\"lg\":\"\"},\"ntr\":\"Supplier of Services, Factory / Manufacturing, Retail Business, Recipient of Goods or Services, Wholesale Business\"}],\"cxdt\":\"\",\"gstin\":\"09AACCF6742M1Z7\",\"nba\":[\"Input Service Distributor (ISD)\",\"Retail Business\",\"Service Provision\",\"Office / Sale Office\",\"Recipient of Goods or Services\",\"Wholesale Business\",\"Supplier of Services\",\"Factory / Manufacturing\"],\"lstupdt\":\"13/11/2018\",\"ctb\":\"Private Limited Company\",\"rgdt\":\"01/07/2017\",\"pradr\":{\"addr\":{\"bnm\":\"\",\"st\":\"VAKIL ROAD\",\"loc\":\"NEW MANDI\",\"bno\":\"114-B\",\"dst\":\"Muzaffarnagar\",\"stcd\":\"Uttar Pradesh\",\"city\":\"\",\"flno\":\"\",\"lt\":\"\",\"pncd\":\"251001\",\"lg\":\"\"},\"ntr\":\"Input Service Distributor (ISD), Retail Business, Service Provision, Office / Sale Office, Recipient of Goods or Services, Wholesale Business\"},\"ctjCd\":\"YB0301\",\"tradeNam\":\"FASPI ENTERPRISES PRIVATE LIMITED\",\"sts\":\"Active\",\"ctj\":\"RANGE- I\"}";
                    obj = new JavaScriptSerializer().Deserialize<GstinDetail>(str);
                    UpdateEwayBalance(-1, "Detail of " + textBox9.Text + " " + obj.tradenam);
                    CheckEwayBalance(false);
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }


        }




    }

    public class GstinDetail
    {
        public string stjcd { get; set; }
        public string lgnm { get; set; }
        public string stj { get; set; }
        public string dty { get; set; }
        public List<adadr> adadr { get; set; }

        public string cxdt { get; set; }
        public string gstin { get; set; }
        public List<string> nba { get; set; }

        public string lstupdt { get; set; }
        public string rgdt { get; set; }
        public string ctb { get; set; }
        public adadr pradr { get; set; }

        public string tradenam { get; set; }
        public string sts { get; set; }
        public string ctjcd { get; set; }
        public string ctj { get; set; }
    }
    public class adadr
    {
        public addr addr { get; set; }
        public string ntr { get; set; }
    }
    public class addr
    {
        public string bnm { get; set; }
        public string st { get; set; }
        public string loc { get; set; }
        public string bno { get; set; }
        public string dst { get; set; }
        public string stcd { get; set; }
        public string city { get; set; }
        public string flno { get; set; }
        public string lt { get; set; }
        public string pncd { get; set; }
        public string lg { get; set; }
    }

}
