using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Web.Script.Serialization;
namespace faspi
{
    public partial class SmsTakada : Form
    {
        string field = "";
        List<UsersFeature> permission;

        public SmsTakada()
        {
            InitializeComponent();
        }
        public void LoadData()
        {   
            DataTable DtSms=new DataTable();
            string sql = "";
           
            //if (Feature.Available("Sms Sent to MobileNo/PhoneNo").ToUpper() == "PHONE")
            //{
                field = "Phone";
            //}
            //else
            //{
            //    field = "MobileNo";
            //}

            //if (Database.IsKacha == false)
            //{
            //    sql = "SELECT X.Name, ACCOUNTYPE.Name as GroupName, ACCOUNT.Address1, ACCOUNT.Address2,Sum(X.Dr) AS Dr, Sum(X.Cr) As Cr, ACCOUNT.Phone,Account.Note FROM ((SELECT QryJournal.Name, Sum(QryJournal.Dr) AS Dr, Sum(QryJournal.Cr) AS Cr FROM QryJournal WHERE (((QryJournal.Vdate)<=" + access_sql.Hash + Database.ldate + access_sql.Hash + ") AND ((QryJournal.A)=" + access_sql.Singlequote + "True" + access_sql.Singlequote + ")) GROUP BY QryJournal.Name UNION ALL SELECT QryAccountinfo.Name, QryAccountinfo.Dr as Dr, QryAccountinfo.Cr as Cr FROM QryAccountinfo)  AS X LEFT JOIN ACCOUNT ON X.Name = ACCOUNT.Name) LEFT JOIN ACCOUNTYPE ON ACCOUNT.Act_id = ACCOUNTYPE.Act_id GROUP BY X.Name, ACCOUNTYPE.Name,ACCOUNT.Address1, ACCOUNT.Address2, ACCOUNT.Phone,Account.Note ORDER BY X.Name";
            //}
            //else
            //{

            //    sql = "SELECT X.Name, ACCOUNTYPE.Name as GroupName,ACCOUNT.Address1, ACCOUNT.Address2, Sum(X.Dr) AS Dr, Sum(X.Cr) As Cr, ACCOUNT.Phone,Account.Note FROM ((SELECT QryJournal.Name, Sum(QryJournal.Dr) AS Dr, Sum(QryJournal.Cr) AS Cr FROM QryJournal WHERE (((QryJournal.Vdate)<=" + access_sql.Hash + Database.ldate + access_sql.Hash + ") AND ((QryJournal.B)=" + access_sql.Singlequote + "True" + access_sql.Singlequote + ")) GROUP BY QryJournal.Name UNION ALL SELECT QryAccountinfo.Name, QryAccountinfo.Dr2 as Dr, QryAccountinfo.Cr2 as Cr FROM QryAccountinfo)  AS X LEFT JOIN ACCOUNT ON X.Name = ACCOUNT.Name) LEFT JOIN ACCOUNTYPE ON ACCOUNT.Act_id = ACCOUNTYPE.Act_id GROUP BY X.Name, ACCOUNTYPE.Name, ACCOUNT.Address1, ACCOUNT.Address2,ACCOUNT.Phone,Account.Note ORDER BY X.Name";
            //}

            if (Database.BMode == "A")
            {
                sql = "SELECT X.Name, ACCOUNTYPE.Name as GroupName, ACCOUNT.Address1, ACCOUNT.Address2,Sum(X.Dr) AS Dr, Sum(X.Cr) As Cr, ACCOUNT." + field + ",Account.Note FROM ( (SELECT     Name, SUM(Dr) AS Dr, SUM(Cr) AS Cr FROM    dbo.QryJournal WHERE     (Vdate <= " + access_sql.Hash + Database.ldate + access_sql.Hash + ") AND (A = " + access_sql.Singlequote + "True" + access_sql.Singlequote + ") AND (Branch_id = '" + Database.BranchId + "') GROUP BY Name UNION ALL  SELECT     Name, Dr AS Dr, Cr AS Cr FROM         dbo.QryAccountinfo WHERE     (Branch_id = '" + Database.BranchId + "'))  AS X LEFT JOIN ACCOUNT ON X.Name = ACCOUNT.Name) LEFT JOIN ACCOUNTYPE ON ACCOUNT.Act_id = ACCOUNTYPE.Act_id GROUP BY X.Name, ACCOUNTYPE.Name,ACCOUNT.Address1, ACCOUNT.Address2, ACCOUNT." + field + ",Account.Note ORDER BY X.Name";
            }
            else if (Database.BMode == "B")
            {
                sql = "SELECT X.Name, ACCOUNTYPE.Name as GroupName, ACCOUNT.Address1, ACCOUNT.Address2,Sum(X.Dr) AS Dr, Sum(X.Cr) As Cr, ACCOUNT." + field + ",Account.Note FROM ( (SELECT     Name, SUM(Dr) AS Dr, SUM(Cr) AS Cr FROM    dbo.QryJournal WHERE     (Vdate <= " + access_sql.Hash + Database.ldate + access_sql.Hash + ") AND (B = " + access_sql.Singlequote + "True" + access_sql.Singlequote + ") AND (Branch_id = '" + Database.BranchId + "') GROUP BY Name UNION ALL  SELECT     Name, Dr1 AS Dr, Cr1 AS Cr FROM         dbo.QryAccountinfo WHERE     (Branch_id = '" + Database.BranchId + "'))  AS X LEFT JOIN ACCOUNT ON X.Name = ACCOUNT.Name) LEFT JOIN ACCOUNTYPE ON ACCOUNT.Act_id = ACCOUNTYPE.Act_id GROUP BY X.Name, ACCOUNTYPE.Name,ACCOUNT.Address1, ACCOUNT.Address2, ACCOUNT." + field + ",Account.Note ORDER BY X.Name";
            }
            else
            {
                sql = "SELECT X.Name, ACCOUNTYPE.Name as GroupName, ACCOUNT.Address1, ACCOUNT.Address2,Sum(X.Dr) AS Dr, Sum(X.Cr) As Cr, ACCOUNT." + field + ",Account.Note FROM ( (SELECT     Name, SUM(Dr) AS Dr, SUM(Cr) AS Cr FROM    dbo.QryJournal WHERE     (Vdate <= " + access_sql.Hash + Database.ldate + access_sql.Hash + ") AND (AB = " + access_sql.Singlequote + "True" + access_sql.Singlequote + ") AND (Branch_id = '" + Database.BranchId + "') GROUP BY Name UNION ALL  SELECT     Name, Dr12 AS Dr, Cr12 AS Cr FROM         dbo.QryAccountinfo WHERE     (Branch_id = '" + Database.BranchId + "'))  AS X LEFT JOIN ACCOUNT ON X.Name = ACCOUNT.Name) LEFT JOIN ACCOUNTYPE ON ACCOUNT.Act_id = ACCOUNTYPE.Act_id GROUP BY X.Name, ACCOUNTYPE.Name,ACCOUNT.Address1, ACCOUNT.Address2, ACCOUNT." + field + ",Account.Note ORDER BY X.Name";
            }
            Database.GetSqlData(sql, DtSms);
            DataTable dtselect = new DataTable();

            dtselect = DtSms.Select("GroupName='SUNDRY CREDITORS' or GroupName='SUNDRY DEBTORS' ").CopyToDataTable();

          
            if (dtselect.Select("(Dr>Cr)").Length==0)
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
                dataGridView1.Rows[j].Cells["balance"].Value = funs.IndianCurr(double.Parse(dtselect.Rows[j]["Dr"].ToString()) -double.Parse(dtselect.Rows[j]["Cr"].ToString()));
                dataGridView1.Rows[j].Cells["phone"].Value = dtselect.Rows[j][field];
                dataGridView1.Rows[j].Cells["Note"].Value = dtselect.Rows[j]["note"];
                dataGridView1.Rows[j].Cells["check"].Value = false;
                dataGridView1.Columns["balance"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }

            textBox1.Text = "Dear Customer, Your Outstanding Balance is : Rs.{Amount}";
       
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
            dtsidefill.Rows[0]["DisplayName"] = "Send SMS";
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
                permission = funs.GetPermissionKey("SMS Setup");

                UsersFeature ob = permission.Where(w => w.FeatureName == "Send SMS").FirstOrDefault();

                if (ob != null && ob.SelectedValue == "No")
                {
                    return;
                }
                else if (ob != null && ob.SelectedValue == "Ask")
                {
                    if (MessageBox.Show("Are you want to send SMS?", "SMS", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
                    {
                        return;
                    }
                }
                       

                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        if (bool.Parse(dataGridView1.Rows[i].Cells["check"].Value.ToString()) == true && (dataGridView1.Rows[i].Cells["phone"].Value.ToString() != "0" && dataGridView1.Rows[i].Cells["phone"].Value.ToString() != ""))
                        {
                            DataTable dtSmsInfo = new DataTable();
                            Database.GetSqlData("select * from smssetup", dtSmsInfo);
                            string AuthKey = "";
                            string SenderID = "";
                            string Footer = "";
                            if (dtSmsInfo.Rows.Count > 0)
                            {
                                AuthKey = dtSmsInfo.Rows[0]["uid"].ToString();
                                SenderID = dtSmsInfo.Rows[0]["sender"].ToString();
                                Footer = dtSmsInfo.Rows[0]["pin"].ToString();
                                Footer = Footer.Replace(" ", "%20");
                                Footer = Footer.Replace("(", "%28");
                                Footer = Footer.Replace("(", "%29");
                                Footer = Footer.Replace(",", "%2C");
                                Footer = Footer.Replace(":", "%3a");
                            }
                            else
                            {
                                return;
                            }

                            string gmatter = textBox1.Text;
                            gmatter = gmatter.Replace("{Amount}", funs.IndianCurr(double.Parse(dataGridView1.Rows[i].Cells["balance"].Value.ToString())));
                            gmatter = gmatter.Replace("\r", "");
                            gmatter = gmatter.Replace(" ", "%20");
                            gmatter = gmatter.Replace("(", "%28");
                            gmatter = gmatter.Replace("(", "%29");
                            gmatter = gmatter.Replace(",", "%2C");
                            gmatter = gmatter.Replace("\n", "%0A");
                            gmatter = gmatter.Replace(":", "%3a");


                            if (funs.isDouble(dataGridView1.Rows[i].Cells["phone"].Value.ToString()) == true)
                            {
                                if (dataGridView1.Rows[i].Cells["phone"].Value.ToString() != "0")
                                {
                                    sms objsms = new sms();
                                    objsms.send(gmatter, dataGridView1.Rows[i].Cells["phone"].Value.ToString(), dataGridView1.Rows[i].Cells["Cname"].Value.ToString());
                                }
                            }
                        }

                    }

                
            }


            else if (name == "quit")
            {
                this.Close();
                this.Dispose();
            }
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

     
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        private void SmsTakada_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                DialogResult chk = MessageBox.Show("Are u sure?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (chk == DialogResult.No)
                {
                    e.Handled = false;
                }
                else
                {
                    this.Dispose();
                }
            }
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.CurrentCell.OwningColumn.Name == "phone")
            {
                Database.CommandExecutor("Update Account set " + field + "='" + dataGridView1.Rows[e.RowIndex].Cells["phone"].Value + "' where name='" + dataGridView1.Rows[e.RowIndex].Cells["cname"].Value + "' ");
            }

            if (dataGridView1.CurrentCell.OwningColumn.Name == "note")
            {
                Database.CommandExecutor("Update Account set [note]='" + dataGridView1.Rows[e.RowIndex].Cells["note"].Value + "' where name='" + dataGridView1.Rows[e.RowIndex].Cells["cname"].Value + "' ");
            }
            
            

        }

        private void SmsTakada_Load(object sender, EventArgs e)
        {
            SideFill();
            this.Size = this.MdiParent.Size;
        }

    }
}
