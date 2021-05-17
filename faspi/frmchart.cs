using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace faspi
{
    public partial class frmchart : Form
    {
        ToolTip tooltip = new ToolTip();
        public string frmtype = "";
        public frmchart()
        {
            InitializeComponent();
           
        }

        public void Fillchart()
        {
            DataTable dt = new DataTable();

            if (Database.DatabaseType == "sql")
            {
                Database.GetSqlData("SELECT   LEFT(DATENAME(MONTH, VOUCHERINFO.Vdate), 3) + '-' + DATENAME(yyyy, VOUCHERINFO.Vdate) AS M, DATEPART(Month,  VOUCHERINFO.Vdate) AS Monthno,  SUM(CASE WHEN Type = 'Purchase' THEN Totalamount ELSE CASE WHEN Type = 'P Return' THEN - 1 * Totalamount ELSE 0 END END) AS AmountP,  SUM(CASE WHEN Type = 'Sale' THEN Totalamount ELSE CASE WHEN Type = 'Return' THEN - 1 * Totalamount ELSE 0 END END) AS AmountS FROM  VOUCHERINFO LEFT OUTER JOIN  VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id WHERE     (VOUCHERTYPE.Type = 'Sale') OR (VOUCHERTYPE.Type = 'Return') OR                      (VOUCHERTYPE.Type = 'Purchase') OR (VOUCHERTYPE.Type = 'P Return') GROUP BY LEFT(DATENAME(MONTH, VOUCHERINFO.Vdate), 3) + '-' + DATENAME(yyyy, VOUCHERINFO.Vdate), DATEPART(Month, VOUCHERINFO.Vdate) ORDER BY Monthno", dt);  
            }
            else
            {
                Database.GetSqlData("SELECT Format([vdate],'mmm-yyyy') AS M, Sum(IIf([Vouchertype.Type]='Purchase',[VOUCHERINFO.Totalamount],iif([Vouchertype.Type]='P Return', -1*[VOUCHERINFO.Totalamount] ,0  ))) AS AmountP, Sum(IIf([Vouchertype.Type]='Sale',[VOUCHERINFO.Totalamount], iif([Vouchertype.Type]='Return', -1*[VOUCHERINFO.Totalamount] ,0  ))) AS AmountS FROM VOUCHERINFO LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id WHERE (((VOUCHERTYPE.Type)='Sale' Or (VOUCHERTYPE.Type)='Return')) OR (((VOUCHERTYPE.Type)='Purchase' Or (VOUCHERTYPE.Type)='P Return')) GROUP BY Format([vdate],'mmm-yyyy'), Format([vdate],'yyyymm') ORDER BY Format([vdate],'yyyymm')", dt);
            }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["M"] = dt.Rows[i]["M"].ToString() + "\n  P:" + funs.IndianCurr(double.Parse(dt.Rows[i]["AmountP"].ToString())) + "\n  S:" + funs.IndianCurr(double.Parse(dt.Rows[i]["AmountS"].ToString()));
            }
            
            chart1.DataSource = dt;
            chart1.Series["Sale"].XValueMember = "M";
            chart1.Series["Sale"].YValueMembers = "AmountS";
            chart1.Series["Purchase"].YValueMembers = "AmountP";
            chart1.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            chart1.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
            chart1.ChartAreas[0].AxisX.Interval = 1;
            chart1.ChartAreas[0].AxisY.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.False;
        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }
        private void frmchart_Load(object sender, EventArgs e)
        {
            this.Width = this.MdiParent.Width;
            this.Height = this.MdiParent.Height;
            SideFill();
            Fillchart();

        }
        private void SideFill()
        {
            flowLayoutPanel1.Controls.Clear();
            DataTable dtsidefill = new DataTable();
            dtsidefill.Columns.Add("Name", typeof(string));
            dtsidefill.Columns.Add("DisplayName", typeof(string));
            dtsidefill.Columns.Add("ShortcutKey", typeof(string));
            dtsidefill.Columns.Add("Visible", typeof(bool));
            //purchase-sale
            dtsidefill.Rows.Add();
            dtsidefill.Rows[0]["Name"] = "purchase-sale";
            dtsidefill.Rows[0]["DisplayName"] = "Purchase-Sale";
            dtsidefill.Rows[0]["ShortcutKey"] = "";
            dtsidefill.Rows[0]["Visible"] = true;

            //Purchase
            dtsidefill.Rows.Add();
            dtsidefill.Rows[1]["Name"] = "purchase";
            dtsidefill.Rows[1]["DisplayName"] = "Purchase";
            dtsidefill.Rows[1]["ShortcutKey"] = "";
            dtsidefill.Rows[1]["Visible"] = true;

            //sale
            dtsidefill.Rows.Add();
            dtsidefill.Rows[2]["Name"] = "sale";
            dtsidefill.Rows[2]["DisplayName"] = "Sale";
            dtsidefill.Rows[2]["ShortcutKey"] = "";
            dtsidefill.Rows[2]["Visible"] = true;
            

           

            //close
            dtsidefill.Rows.Add();
            dtsidefill.Rows[3]["Name"] = "quit";
            dtsidefill.Rows[3]["DisplayName"] = "Quit";
            dtsidefill.Rows[3]["ShortcutKey"] = "Esc";
            dtsidefill.Rows[3]["Visible"] = true;






            for (int i = 0; i < dtsidefill.Rows.Count; i++)
            {


                if (bool.Parse(dtsidefill.Rows[i]["Visible"].ToString()) == true)
                {

                    Button btn = new Button();
                    btn.Size = new Size(150, 45);
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
                    System.Drawing.Rectangle RC = btn.ClientRectangle;
                    System.Drawing.Font font = new System.Drawing.Font("Arial", 14);


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
           
            if (name == "quit")
            {
                this.Close();
                this.Dispose();
            }







        }



        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Fillchart();
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

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
           // tooltip.Hide(pictureBox1);
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click_1(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click_2(object sender, EventArgs e)
        {

            for (int i = 226; i >= 0; i--)
            {
                
                this.Size = new System.Drawing.Size(1030, i);
                
            }
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void frmchart_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
                this.Dispose();
            }
        }

        private void chart1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
                this.Dispose();
            }
        }


    }
}
