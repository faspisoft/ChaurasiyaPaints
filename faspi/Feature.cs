using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;

namespace faspi
{
   class Feature
    {
        public static string Available(String feature)
        {
            string found = "No";

            if (Master.Feature.Select("[Features]='" + feature + "' ").Length == 0)
            {
            }
            else
            {
                found = Master.Feature.Select("[Features]='" + feature + "' ").FirstOrDefault()["ToSuperAdmin"].ToString();
            }
            //else if(Database.utype.ToUpper()=="SUPERADMIN")
            //{
            //    found = Master.Feature.Select("[Features]='" + feature + "' ").FirstOrDefault()["ToSuperAdmin"].ToString();
            //}
            //else if (Database.utype.ToUpper() == "ADMIN")
            //{
            //    found = Master.Feature.Select("[Features]='" + feature + "' ").FirstOrDefault()["ToAdmin"].ToString();
            //}
            //else if (Database.utype.ToUpper() == "SUPERUSER")
            //{
            //    found = Master.Feature.Select("[Features]='" + feature + "' ").FirstOrDefault()["ToSuperUser"].ToString();
            //}
            //else if (Database.utype.ToUpper() == "USER")
            //{
            //    found = Master.Feature.Select("[Features]='" + feature + "' ").FirstOrDefault()["ToUser"].ToString();
            //}
            //else if (Database.utype.ToUpper() == "CASHIER")
            //{
            //    found = Master.Feature.Select("[Features]='" + feature + "' ").FirstOrDefault()["ToCashier"].ToString();
            //}
            return found;
        }

        public static bool AvailableLogin(String feature)
        {

            if (Master.FeatureLogin.Select("[Features]='" + feature + "' ").Length == 0)
            {
                return false;
            }
            else
            {
                return bool.Parse(Master.FeatureLogin.Select("[Features]='" + feature + "' ").FirstOrDefault()["Active"].ToString());
            }

        }
        public static bool UserPower(String feature)
        {

            if (Master.UserPower.Select("[power]='" + feature + "' ").Length == 0)
            {
                return false;
            }
            else
            {
                return bool.Parse(Master.UserPower.Select("[Power]='" + feature + "' ").FirstOrDefault()["Active"].ToString());
            }




            //bool found = false; ;
            //DataTable dtFeature = new DataTable();

            //try
            //{
            //    Database.GetOtherSqlData("select * from POWER where power='" + feature + "'", dtFeature);
            //}
            //catch
            //{
            //    return true;
            //}
            //if (dtFeature.Rows.Count == 1)
            //{
            //    bool active = bool.Parse(dtFeature.Rows[0]["Active"].ToString());
            //    if (active == true)
            //    {
            //        found = true;
            //    }
            //}
            //return found;

        }
    }
}
