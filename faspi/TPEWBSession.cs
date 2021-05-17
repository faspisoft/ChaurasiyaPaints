using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TaxProEWB.API;

namespace faspi
{
    class TPEWBSession : EWBSession
    {
        public string TaxPayerID { get; set; }
        public string TaxPayerName { get; set; }

        public TPEWBSession() : base(false, false)
        {

            LoadEwbLoginDetail();
            RefreshAuthTokenCompleted += SaveNewAuthToken;
            LoadEwbApiSetting();
        }

        private void LoadEwbApiSetting()
        {

            EwbApiSetting = new EWBAPISetting();
            EwbApiSetting.ID = 0;

            // EwbApiSetting.GSPName = "TaxPro_Sandbox";
            //EwbApiSetting.BaseUrl = "http://testapi.taxprogsp.co.in/ewaybillapi/v1.03";



            EwbApiSetting.GSPName = "TaxPro_Production";
            EwbApiSetting.BaseUrl = "https://api.taxprogsp.co.in/v1.03";

            EwbApiSetting.AspUserId = "1616963119";
            EwbApiSetting.AspPassword = "marwari123@";
            EwbApiSetting.EWBClientId = "";
            EwbApiSetting.EWBClientSecret = "";
            EwbApiSetting.EWBGSPUserID = "";
            EwbApiSetting.AuthUrl = null;



            EwbApiSetting.AspUrl = null;

        }

        private void LoadEwbLoginDetail()
        {

            string strJSON = Database.GetScalarText("select EwbLoginDetail from company");
            EwbApiLoginDetails = JsonConvert.DeserializeObject<EWBAPILoginDetails>(strJSON);
        }


        private void SaveNewAuthToken(object sender, EventArgs e)
        {

            //string strJSON = JsonConvert.SerializeObject(this.EwbApiLoginDetails);
            //Database.CommandExecutor("Update Company set EwbLoginDetail='" + strJSON + "'");

        }

        public override void LogAPITxn(APITxnLogArgs e)
        {
            //Write your code to Log API Txn
        }

    }
}
