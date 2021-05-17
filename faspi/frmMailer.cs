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
    public partial class frmMailer : Form
    {
        DataTable dtMailer = new DataTable("mailer");
        
        

        public frmMailer()
        {
            InitializeComponent();
        }

        private void frmMailer_Load(object sender, EventArgs e)
        {
            Database.GetSqlData("select * from mailer", dtMailer);
            if (dtMailer.Rows.Count > 0)
            {
                textBox1.Text = dtMailer.Rows[0]["emailid"].ToString();
                textBox2.Text = dtMailer.Rows[0]["password"].ToString();
                textBox3.Text = dtMailer.Rows[0]["smtp"].ToString();
                textBox4.Text = dtMailer.Rows[0]["port"].ToString();
                if (bool.Parse(dtMailer.Rows[0]["Credentials"].ToString()) == true)
                {
                    radioButton1.Checked = true;
                }
                else
                {
                    radioButton2.Checked = true;
                }

                if (bool.Parse(dtMailer.Rows[0]["EnableSsl"].ToString()) == true)
                {
                    radioButton4.Checked = true;
                }
                else
                {
                    radioButton3.Checked = true;
                }
            }
            else
            {
                dtMailer.Rows.Add(0);
            }

            this.Size = this.MdiParent.Size;
            SideFill();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            dtMailer.Rows[0]["emailid"] = textBox1.Text;
            dtMailer.Rows[0]["password"] = textBox2.Text;
            dtMailer.Rows[0]["smtp"] = textBox3.Text;

            Database.SaveData(dtMailer);
            MessageBox.Show("Information Saved");

            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
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
                dtMailer.Rows[0]["emailid"] = textBox1.Text;
                dtMailer.Rows[0]["password"] = textBox2.Text;
                dtMailer.Rows[0]["smtp"] = textBox3.Text;
                dtMailer.Rows[0]["port"] = textBox4.Text;
                if (radioButton1.Checked == true)
                {
                    dtMailer.Rows[0]["Credentials"] = true;
                }
                else
                {
                    dtMailer.Rows[0]["Credentials"] = false;
                }
                if (radioButton4.Checked == true)
                {
                    dtMailer.Rows[0]["EnableSsl"] = true;
                }
                else
                {
                    dtMailer.Rows[0]["EnableSsl"] = false;
                }

                Database.SaveData(dtMailer);
                MessageBox.Show("Information Saved");

                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                this.Close();
                this.Dispose();
            }

            if (name == "quit")
            {
                this.Close();
                this.Dispose();
            }


        }

        private void frmMailer_KeyDown(object sender, KeyEventArgs e)
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
                dtMailer.Rows[0]["emailid"] = textBox1.Text;
                dtMailer.Rows[0]["password"] = textBox2.Text;
                dtMailer.Rows[0]["smtp"] = textBox3.Text;
                dtMailer.Rows[0]["port"] = textBox4.Text;
                if (radioButton1.Checked == true)
                {
                    dtMailer.Rows[0]["Credentials"] = true;
                }
                else
                {
                    dtMailer.Rows[0]["Credentials"] = false;
                }
                if (radioButton4.Checked == true)
                {
                    dtMailer.Rows[0]["EnableSsl"] = true;
                }
                else
                {
                    dtMailer.Rows[0]["EnableSsl"] = false;
                }

                Database.SaveData(dtMailer);
                MessageBox.Show("Information Saved");

                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                this.Close();
                this.Dispose();
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            this.Dispose();
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

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }
    }
}
