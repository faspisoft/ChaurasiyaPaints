using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace faspi
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);


            //if (Database.DatabaseType == "sql")
            //{

                 access_sql.setconnection();
               
           // }
                 Database.OpenConnection();
         //   Dongle.cllogin(false);
            //System.Threading.Thread MyThread = new System.Threading.Thread(Loaddll);
            //MyThread.Start();
          

                Application.Run(new frmLogin());
            
            //Application.Run(new Form1());
        }

        static void Loaddll()
        {
            CrystalDecisions.CrystalReports.Engine.ReportDocument rptOther = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
            try
            {
                rptOther.Load(AppDomain.CurrentDomain.BaseDirectory + "\\Report.net\\LadgerA5.rpt");
            }
            catch
            {
                MessageBox.Show("Error: Loading Crystal Report Dll");
            }
        
        }

    }
}
