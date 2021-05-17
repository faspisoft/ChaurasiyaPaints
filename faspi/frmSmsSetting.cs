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
    public partial class frmSmsSetting : Form
    {
        DataTable dtSms = new DataTable("smssetup");
        
        

        public frmSmsSetting()
        {
            InitializeComponent();
        }

        private void frmSmsSetting_Load(object sender, EventArgs e)
        {
            Database.GetSqlData("select * from smssetup", dtSms);
            if (dtSms.Rows.Count > 0)
            {
                textBox1.Text = dtSms.Rows[0]["uid"].ToString();
                textBox2.Text = dtSms.Rows[0]["pin"].ToString();
                textBox3.Text = dtSms.Rows[0]["sender"].ToString();
            }
            else
            {
                dtSms.Rows.Add(0);
            }

            this.Size = this.MdiParent.Size;
            SideFill();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            dtSms.Rows[0]["uid"] = textBox1.Text;
            dtSms.Rows[0]["pin"] = textBox2.Text;
            dtSms.Rows[0]["sender"] = textBox3.Text;
            Database.SaveData(dtSms);
            MessageBox.Show("Setting saved");
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
            dtsidefill.Rows[0]["Name"] = "save";
            dtsidefill.Rows[0]["DisplayName"] = "Save";
            dtsidefill.Rows[0]["ShortcutKey"] = "^S";
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

            if (name == "save")
            {
                dtSms.Rows[0]["uid"] = textBox1.Text;
                dtSms.Rows[0]["pin"] = textBox2.Text;
                dtSms.Rows[0]["sender"] = textBox3.Text;
                Database.SaveData(dtSms);
                MessageBox.Show("Setting saved");
                this.Close();
                this.Dispose();
            }

            if (name == "quit")
            {
                this.Close();
                this.Dispose();
            }


        }

        private void frmSmsSetting_KeyDown(object sender, KeyEventArgs e)
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
                    this.Close();
                    this.Dispose();
                }
            }

            else if (e.Control && e.KeyCode == Keys.S)
            {
                dtSms.Rows[0]["uid"] = textBox1.Text;
                dtSms.Rows[0]["pin"] = textBox2.Text;
                dtSms.Rows[0]["sender"] = textBox3.Text;
                Database.SaveData(dtSms);
                MessageBox.Show("Setting saved");
                this.Close();
                this.Dispose();
            }

        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }
    }
}
