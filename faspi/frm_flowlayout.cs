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
    public partial class frm_flowlayout : Form
    {
        public ToolStripProgressBar ProgrBar;
        ToolTip tooltip = new ToolTip();
        public frm_flowlayout()
        {
            InitializeComponent();
        }

        private void frm_flowlayout_Load(object sender, EventArgs e)
        {
            this.Dock = DockStyle.Right;
            SideFill();
            flowLayoutPanel1.AutoScroll = true;
            flowLayoutPanel1.Visible = true;
        }

        void btn_Click(object sender, EventArgs e)
        {
            Button tbtn = (Button)sender;
            string name = tbtn.Name.ToString();

            if (name == "Receipt")
            {
                frmMasterVou frm = new frmMasterVou();
                frm.LoadData("Receipt", "Receipt Vouchers");
                frm.MdiParent = this.MdiParent;
                frm.Show();
            }
            else if (name == "Contra")
            {
                frmMasterVou frm = new frmMasterVou();
                frm.MdiParent = this.MdiParent;
                frm.LoadData("Contra", "Contra Vouchers");
                frm.Show();
            }
            else if (name == "Stock Jou")
            {
                frmMasterVou frm = new frmMasterVou();
                frm.MdiParent = this.MdiParent;
                frm.LoadData("Transfer", "Stock Journal Vouchers");
                frm.Show();
            }            
            else if (name == "Payment")
            {
                frmMasterVou frm = new frmMasterVou();
                frm.MdiParent = this.MdiParent;
                frm.LoadData("Payment", "Payment Vouchers");
                frm.Show();
            }
            else if (name == "Purchase")
            {
                frmMasterVou frm = new frmMasterVou();
                frm.MdiParent = this.MdiParent;
                frm.LoadData("Purchase", "Purchase Vouchers");
                frm.Show();
            }
            else if (name == "Sale")
            {
                frmMasterVou frm = new frmMasterVou();
                frm.MdiParent = this.MdiParent;
                frm.LoadData("Sale", "Sale Vouchers");
               // frm.ProgrBar = toolStripProgressBar1;
                frm.Show();
            }
            else if (name == "Purchase Return")
            {
                frmMasterVou frm = new frmMasterVou();
                frm.MdiParent = this.MdiParent;
                frm.LoadData("P Return", "Purchase Return Vouchers");
                frm.Show();
            }
            else if (name == "Sale Return")
            {
                frmMasterVou frm = new frmMasterVou();
                frm.MdiParent = this.MdiParent;
                frm.LoadData("Return", "Sale Return Vouchers");
                frm.Show();
            }
            else if (name == "Journal")
            {
                frmMasterVou frm = new frmMasterVou();
                frm.MdiParent = this.MdiParent;
                frm.LoadData("Journal", "Journal Vouchers");
                frm.Show();
            }
            else if (name == "Control Room")
            {
                frmMaster frm = new frmMaster();
                frm.LoadData("Control Room", "Control Room");
                frm.MdiParent = this.MdiParent;
                frm.Show();
            }
            else if (name == "Vouchers Confg")
            {
                frmMaster frm = new frmMaster();
                frm.LoadData("TransactionSetup", "TransactionSetup");
                frm.MdiParent = this.MdiParent;
                frm.Show();
            }
            else if (name == "Ledger")
            {
                Report gg = new Report();
                string strCombo = funs.GetStrComboled("*");
                char cg = 'a';
                string selected = SelectCombo.ComboKeypress(this, cg, strCombo, "", 1);
                if (Feature.Available("Ledger with Remarks") == "Yes")
                {
                    gg.LedgerRemark(Database.stDate, Database.ldate, selected);
                }
                else
                {

                    gg.LedgerNew(Database.stDate, Database.ldate, selected);
                }
                gg.MdiParent = this.MdiParent;
                gg.Show();
            }
            else if (name == "Godown Stock")
            {
            
                Form1 frm = new Form1();
                frm.ReportName = "StockSummary";
                frm.Show();
            }
        }

        private void SideFill()
        {
            flowLayoutPanel1.Controls.Clear();
            for (int i = 0; i < Master.SideMenu.Rows.Count; i++)
            {


                if (bool.Parse(Master.SideMenu.Rows[i]["Display"].ToString()) == true)
                {

                    if (Master.SideMenu.Rows[i]["MenuOption"].ToString() == "Control Room" || Master.SideMenu.Rows[i]["MenuOption"].ToString() == "Vouchers Confg")
                    {
                        if (Database.utype.ToUpper() == "SUPERADMIN")
                        {
                            Button btn = new Button();
                            btn.Size = new Size(150, 30);
                            btn.Name = Master.SideMenu.Rows[i]["MenuOption"].ToString();
                            btn.Text = "";
                            Bitmap bmp = new Bitmap(btn.ClientRectangle.Width, btn.ClientRectangle.Height);
                            Graphics G = Graphics.FromImage(bmp);
                            G.Clear(btn.BackColor);
                            string line1 = Master.SideMenu.Rows[i]["ShortcutKey"].ToString();
                            string line2 = Master.SideMenu.Rows[i]["DisplayName"].ToString();
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
                    else
                    {
                        Button btn = new Button();
                        btn.Size = new Size(150, 30);
                        btn.Name = Master.SideMenu.Rows[i]["MenuOption"].ToString();
                        btn.Text = "";
                        Bitmap bmp = new Bitmap(btn.ClientRectangle.Width, btn.ClientRectangle.Height);
                        Graphics G = Graphics.FromImage(bmp);
                        G.Clear(btn.BackColor);
                        string line1 = Master.SideMenu.Rows[i]["ShortcutKey"].ToString();
                        string line2 = Master.SideMenu.Rows[i]["DisplayName"].ToString();
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
        }

        private void flowLayoutPanel1_MouseHover(object sender, EventArgs e)
        {
            
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            try
            {
                Master.UpdateAll();
                funs.ShowBalloonTip("Updated", "Updated Successfully");
            }
            catch (Exception ex)
            {

                MessageBox.Show("Timer "+ex.ToString());
            }
           
        }

        private void pictureBox1_MouseHover(object sender, EventArgs e)
        {
            if (this.Cursor == Cursors.Arrow)
            {
                tooltip.Show("Refresh", pictureBox1);
            }
            else
            {
                tooltip.Hide(pictureBox1);
                tooltip.Dispose();
            }
        }
    }
}
