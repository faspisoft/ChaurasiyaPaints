using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports;
using CrystalDecisions.CrystalReports.Engine;
using System.Net.Mail;
using System.Net;
using System.IO;

namespace faspi
{
    public partial class frm_sendmail : Form
    {
        ReportDocument rptOther = new ReportDocument();
        bool mailSent = false;
        string status = "";
        
        public frm_sendmail()
        {
            InitializeComponent();
            dateTimePicker1.CustomFormat = Database.dformat;
            dateTimePicker2.CustomFormat = Database.dformat;

            dateTimePicker1.MinDate = Database.stDate;
            dateTimePicker1.MaxDate = Database.ldate;
            dateTimePicker2.MinDate = Database.stDate;


            dateTimePicker2.MaxDate = Database.ldate;
            dateTimePicker1.Value = Database.stDate;
            dateTimePicker2.Value = Database.ldate;
        }

        private void SideFill()
        {
            flowLayoutPanel1.Controls.Clear();
            DataTable dtsidefill = new DataTable();
            dtsidefill.Columns.Add("Name", typeof(string));
            dtsidefill.Columns.Add("DisplayName", typeof(string));
            dtsidefill.Columns.Add("ShortcutKey", typeof(string));
            dtsidefill.Columns.Add("Visible", typeof(bool));
            //save
            dtsidefill.Rows.Add();
            dtsidefill.Rows[0]["Name"] = "send";
            dtsidefill.Rows[0]["DisplayName"] = "Send Mail";
            dtsidefill.Rows[0]["ShortcutKey"] = "";
            dtsidefill.Rows[0]["Visible"] = true;




            //close
            dtsidefill.Rows.Add();
            dtsidefill.Rows[1]["Name"] = "quit";
            dtsidefill.Rows[1]["DisplayName"] = "Quit";
            dtsidefill.Rows[1]["ShortcutKey"] = "Esc";
            dtsidefill.Rows[1]["Visible"] = true;






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
        void btn_Click(object sender, EventArgs e)
        {
            Button tbtn = (Button)sender;
            string name = tbtn.Name.ToString();

            if (name == "send")
            {
                if (Feature.Available("Send Mail") == "Yes")
                {


                    string smtpAddress = "";
                    string emailFrom = "";
                    string password = "";
                    int port = 0;
                    bool Credentials = false;
                    bool EnableSsl = true;

                    DataTable dtFirmMailInfo = new DataTable();
                    Database.GetSqlData("select * from mailer", dtFirmMailInfo);
                    if (dtFirmMailInfo.Rows.Count > 0)
                    {
                        smtpAddress = dtFirmMailInfo.Rows[0]["smtp"].ToString();
                        emailFrom = dtFirmMailInfo.Rows[0]["emailid"].ToString();
                        password = dtFirmMailInfo.Rows[0]["password"].ToString();
                        port = int.Parse(dtFirmMailInfo.Rows[0]["port"].ToString());
                        Credentials = bool.Parse(dtFirmMailInfo.Rows[0]["Credentials"].ToString());
                        EnableSsl = bool.Parse(dtFirmMailInfo.Rows[0]["EnableSsl"].ToString());
                    }



                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        if (bool.Parse(dataGridView1.Rows[i].Cells["check"].Value.ToString()) == true && (dataGridView1.Rows[i].Cells["Emailid"].Value.ToString() != "None" && dataGridView1.Rows[i].Cells["Emailid"].Value.ToString() != ""))
                        {
                            SmtpClient client = new SmtpClient(smtpAddress, port);
                            client.UseDefaultCredentials = Credentials;
                            client.EnableSsl = EnableSsl;
                            client.Credentials = new NetworkCredential(emailFrom, password);

                            string accnam = dataGridView1.Rows[i].Cells["cname"].Value.ToString();
                            string emailTo = dataGridView1.Rows[i].Cells["Emailid"].Value.ToString();
                            string subject = "Customer Report";
                            string body = "Dear <b>" + accnam + "</b>,<br/>";
                            body += "Please find enclosed Bills Details.<br/>";

                            body += "Your Balance is: <b>" + funs.accbal(funs.Select_ac_id(accnam)) + "</b>";
                            body += "</b><br/><br/>Regards";
                            body += "<br/><b>" + Database.fname;

                            MailMessage mail = new MailMessage();
                            mail.From = new MailAddress(emailFrom);
                            mail.To.Add(emailTo);
                            mail.Subject = subject;
                            mail.Body = body;
                            mail.IsBodyHtml = true;


                            try
                            {
                                Report gg = new Report();
                                gg.CustomerDetailBillWise(dateTimePicker1.Value, dateTimePicker2.Value, accnam);
                                gg.ExportToPdf("System\\Bill-" + accnam + ".pdf");
                                gg.Close();
                                gg.Dispose();


                                System.Net.Mime.ContentType contype = new System.Net.Mime.ContentType();
                                mail.Attachments.Add(new Attachment("System\\Bill-" + accnam + ".pdf", contype));
                                object userState = mail;

                                status = "Mail Sent Successfully";
                                client.SendCompleted += new SendCompletedEventHandler(SmtpClient_OnCompleted);
                                client.SendAsync(mail, userState);
                            }
                            catch (Exception ex)
                            {
                                status = "Mail Not Sent";

                            }


                            DataTable dtemail = new DataTable("EmailLOG");
                            Database.GetSqlData("select * from EmailLOG where id=0", dtemail);
                            dtemail.Rows.Add();
                            dtemail.Rows[0]["AccName"] = accnam;
                            dtemail.Rows[0]["Email"] = emailTo;
                            dtemail.Rows[0]["Status"] = status;
                            dtemail.Rows[0]["SDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
                            dtemail.Rows[0]["STime"] = DateTime.Now.ToString("HH:mm");
                            Database.SaveData(dtemail);



                        }

                    }

                    this.Close();
                    this.Dispose();


                }
            }


            else if (name == "quit")
            {
                this.Close();
                this.Dispose();
            }
        }



        public void LoadData()
        {
            DataTable DtSms = new DataTable();
          //  string sql = "SELECT X.Name, ACCOUNTYPE.RefineName AS GroupName, ACCOUNT.Address1, ACCOUNT.Address2, Sum(X.Dr) AS Dr, Sum(X.Cr) AS Cr, ACCOUNT.Email FROM ((SELECT QryJournal.ACCOUNT.Name, sum(QryJournal.Dr) as Dr, sum(QryJournal.Cr) as Cr From QryJournal Where (((QryJournal.Vdate)  <= "+access_sql.Hash + Database.ldate.ToString(Database.dformat) +  access_sql.Hash+")) GROUP BY QryJournal.ACCOUNT.Name UNION ALL SELECT QryAccountinfo.Name, QryAccountinfo.Dr as Dr, QryAccountinfo.Cr as Cr FROM QryAccountinfo)  AS X LEFT JOIN ACCOUNT ON X.Name = ACCOUNT.Name) LEFT JOIN ACCOUNTYPE ON ACCOUNT.Act_id = ACCOUNTYPE.Act_id GROUP BY X.Name, ACCOUNTYPE.RefineName, ACCOUNT.Address1, ACCOUNT.Address2, ACCOUNT.Email order by X.Name";
            string sql = "SELECT X.Name, RefineName AS GroupName, Address1, Address2, Sum(X.Dr) AS Dr, Sum(X.Cr) AS Cr, Email FROM ((SELECT QryJournal.Name, sum(QryJournal.Dr) as Dr, sum(QryJournal.Cr) as Cr From QryJournal Where (((QryJournal.Vdate)  <= " + access_sql.Hash + Database.ldate.ToString(Database.dformat) + access_sql.Hash + ")) GROUP BY QryJournal.Name UNION ALL SELECT QryAccountinfo.Name, QryAccountinfo.Dr as Dr, QryAccountinfo.Cr as Cr FROM QryAccountinfo)  AS X LEFT JOIN ACCOUNT ON X.Name = ACCOUNT.Name) LEFT JOIN ACCOUNTYPE ON ACCOUNT.Act_id = ACCOUNTYPE.Act_id GROUP BY X.Name, RefineName, Address1, Address2, Email order by X.Name";
            Database.GetSqlData(sql, DtSms);
            DataTable dtselect = new DataTable();
            dtselect = DtSms.Select("GroupName='SUNDRY CREDITORS'  or GroupName='SUNDRY DEBTORS'").CopyToDataTable();

            if (dtselect.Select("(Dr>Cr)").Length == 0)
            {
                return;
            }

            dtselect = dtselect.Select("(Dr>Cr)").CopyToDataTable();
            dtselect.DefaultView.Sort = "Name";
            dtselect = dtselect.DefaultView.ToTable();
            for (int j = 0; j < dtselect.Rows.Count; j++)
            {
                dataGridView1.Rows.Add();
                dataGridView1.Rows[j].Cells["cname"].Value = dtselect.Rows[j]["Name"];
                dataGridView1.Rows[j].Cells["gname"].Value = dtselect.Rows[j]["GroupName"];
                dataGridView1.Rows[j].Cells["address1"].Value = dtselect.Rows[j]["Address1"];
                dataGridView1.Rows[j].Cells["address2"].Value = dtselect.Rows[j]["Address2"];
                dataGridView1.Rows[j].Cells["balance"].Value = funs.IndianCurr(double.Parse(dtselect.Rows[j]["Dr"].ToString()) - double.Parse(dtselect.Rows[j]["Cr"].ToString()));
                dataGridView1.Rows[j].Cells["Emailid"].Value = dtselect.Rows[j]["Email"];


                dataGridView1.Rows[j].Cells["check"].Value = false;
                dataGridView1.Columns["balance"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }

        }

        public void SmtpClient_OnCompleted(object sender, AsyncCompletedEventArgs e)
        {
            //Get the Original MailMessage object
            MailMessage mail = (MailMessage)e.UserState;
            //write out the subject
            string subject = mail.Subject;




            if (e.Cancelled)
            {
                status = "Send canceled for mail with subject [{0}]." + subject;
            }
            if (e.Error != null)
            {
                status = "Error {1} occurred when sending mail [{0}] " + subject + e.Error.ToString();
            }

            else
            {
                status = "Mail sent";
            }


            mailSent = true;


        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

            if (checkBox1.Checked == true)
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    dataGridView1.Rows[i].Cells["check"].Value = true;
                }
            }
            else
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    dataGridView1.Rows[i].Cells["check"].Value = false;
                }

            }
        }

        private void frm_sendmail_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
                this.Dispose();
            }
        }

        private void frm_sendmail_Load(object sender, EventArgs e)
        {
            this.Size = this.MdiParent.Size;
            SideFill();
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.CurrentCell.OwningColumn.Name == "emailid")
            {
                Database.CommandExecutor("Update Account set Email='" + dataGridView1.Rows[e.RowIndex].Cells["emailid"].Value + "' where name='" + dataGridView1.Rows[e.RowIndex].Cells["cname"].Value + "' ");
            }
        }
    }
}
