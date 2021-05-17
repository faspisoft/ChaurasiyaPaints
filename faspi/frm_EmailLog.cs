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
    public partial class frm_EmailLog : Form
    {
        DataTable dtemaillog = new DataTable();

        public frm_EmailLog()
        {
            InitializeComponent();
            dateTimePicker1.MinDate = Database.stDate;
            dateTimePicker1.MaxDate = Database.ldate;
            dateTimePicker1.Value = Database.stDate;
            dateTimePicker1.CustomFormat = Database.dformat;
            dateTimePicker2.CustomFormat = Database.dformat;
            dateTimePicker2.MinDate = Database.stDate;
            dateTimePicker2.MaxDate = Database.ldate;
            dateTimePicker2.Value = Database.ldate;
        }

        private void frm_EmailLog_Load(object sender, EventArgs e)
        {
            this.Size = this.MdiParent.Size;
            SideFill();
        }

        public void LoadData()
        {
            dtemaillog.Clear();
            ansGridView5.Rows.Clear();

            Database.GetSqlData("Select * from EmailLOG where Sdate>="+ access_sql.Hash+ dateTimePicker1.Value.Date.ToString(Database.dformat) + access_sql.Hash +" and Sdate<="+access_sql.Hash + dateTimePicker2.Value.Date.ToString(Database.dformat) + access_sql.Hash+ " order by id", dtemaillog);
            ansGridView5.Columns["Status"].Visible = true;
            for (int i = 0; i < dtemaillog.Rows.Count; i++)
            {
                ansGridView5.Rows.Add();
                ansGridView5.Rows[i].Cells["id"].Value = dtemaillog.Rows[i]["id"];
                ansGridView5.Rows[i].Cells["Sno"].Value = (i + 1);
                ansGridView5.Rows[i].Cells["AccName"].Value = dtemaillog.Rows[i]["AccName"];
                ansGridView5.Rows[i].Cells["EmailId"].Value = dtemaillog.Rows[i]["Email"];
                ansGridView5.Rows[i].Cells["Date"].Value = DateTime.Parse(dtemaillog.Rows[i]["SDate"].ToString()).ToString(Database.dformat);
                ansGridView5.Rows[i].Cells["Time"].Value = dtemaillog.Rows[i]["STime"];
                ansGridView5.Rows[i].Cells["Status"].Value = dtemaillog.Rows[i]["Status"];
            }
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
                LoadData();
            }
            else if (name == "quit")
            {
                this.Close();
                this.Dispose();
            }
        }
    }
}
