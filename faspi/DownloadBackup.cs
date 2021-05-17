using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Forms;
using Microsoft.Win32;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Text;
using System.ComponentModel;

namespace faspi
{
    public partial class DownloadBackup : Form
    {
        public string strFoldePath { get; set; }
        public string dbName { get; set; }
        public string BackType { get; set; }

        string strFileName { get; set; }
        private bool isClose { get; set; }

        string strFilePath { get; set; }

        string strBaseUri = "http://192.168.1.114:8080/";

        DateTime dtFrom;


        public DownloadBackup()
        {
            InitializeComponent();
        }

        private void DownloadBackup_Load(object sender, EventArgs e)
        {
            isClose = false;
            label2.Text = "";

            if (BackType != "MANUAL" && BackType != "SMART" && BackType != "EVEN-ODD")
            {
                CloseForm("Invalid Backup Mode.", true);
                return;
            }

            if (!System.IO.Directory.Exists(strFoldePath))
            {
                CloseForm("Invalid Backup Path.", true);

                return;
            }

            //  dbName = Database.databaseName;
            dtFrom = DateTime.Now;
            timer1.Start();
            strFileName = Database.databaseName + DateTime.Now.ToString("yyyyMMddHHmmss");

            label1.Text = "Generating Backup . . .";
            label1.Refresh();

            backgroundWorker1.RunWorkerAsync();

            //CreateBackup();
        }
        public void download()
        {
            label1.Text = "Downloading Backup . . .";
            label1.Refresh();

            using (WebClient webclient = new WebClient())
            {
                webclient.DownloadProgressChanged += OnDownloadProgressChanged;
                webclient.DownloadFileCompleted += OnDownloadFileCompleted;

                strFilePath = strFoldePath + "\\" + strFileName + ".zip";

                if (BackType == "SMART") //SMART
                {
                    strFilePath = strFoldePath + "\\" + "S" + dbName + "D" + DateTime.Now.ToString("dd") + ".zip";
                }
                else if (BackType == "EVEN-ODD") //EVEN-ODD
                {
                    int ir = int.Parse(DateTime.Now.ToString("dd")) % 2;
                    strFilePath = strFoldePath + "\\" + "S" + dbName + ir.ToString() + ".zip";
                }

                if (File.Exists(strFilePath) == true)
                {
                    File.Delete(strFilePath);
                }

                progressBar1.Value = 0;
                webclient.DownloadFileAsync(new Uri(strBaseUri + "dbbackups/" + strFileName + ".zip"), strFilePath);

            }
        }

        private void OnDownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        private void OnDownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            progressBar1.Value = 0;
            if (e.Error != null)
            {
                CloseForm(e.Error.Message, true);
                return;
            }

            using (WebClient webclient = new WebClient())
            {
                string strResult = webclient.DownloadString(new Uri(strBaseUri + "createbackup.aspx?action=delete&bkfile=" + strFileName));
            }

            if (BackType == "SMART")
            {
                try
                {
                    string strCopyFIle = strFoldePath + "\\" + "S" + dbName + "M" + DateTime.Now.ToString("MM") + ".zip";
                    System.IO.File.Copy(strFilePath, strCopyFIle, true);
                }
                catch (Exception ex) { }
            }

            CloseForm("Success");

        }

        private void DownloadBackup_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isClose == false)
            {
                e.Cancel = true;
            }
        }


        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            using (WebClient webclient = new WebClient())
            {
                string strResult = webclient.DownloadString(new Uri(strBaseUri + "createbackup.aspx?action=backup&dbn=" + dbName + "&bkfile=" + strFileName));
                e.Result = strResult.ToLower();
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Result.ToString().ToLower() == "success")
            {
                download();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DateTime dtTill = DateTime.Now;
            label2.Text = "Time Taken : " + (dtTill - dtFrom).TotalSeconds.ToString("00") + " Second";
        }

        void CloseForm(string strMsg, bool isErr = false)
        {
            timer1.Stop();
            if (!isErr)            
            {
                if (BackType == "MANUAL")
                {
                    MessageBox.Show(this, strMsg, "Success");
                }
            }
            else
            {
                MessageBox.Show(this, strMsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            isClose = true;
            this.Close();
        }

    }
}
