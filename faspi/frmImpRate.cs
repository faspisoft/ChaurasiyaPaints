using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Office.Core;
using Excel = Microsoft.Office.Interop.Excel;

namespace faspi
{
    public partial class frmImpRate : Form
    {
        String fName = "";
        static Object misValue = System.Reflection.Missing.Value;
        static Excel.Application apl = new Microsoft.Office.Interop.Excel.Application();
        Excel.Workbook wb;
        Excel.Worksheet ws;
        public ToolStripProgressBar ProgrBar;

        public frmImpRate()
        {
            InitializeComponent();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if(textBox1.Text=="")
            {
                textBox1.BackColor = Color.Aqua;
            }
            else
            {
            tabControl1.SelectedIndex = 1;



            wb = (Excel.Workbook)apl.Workbooks.Open(ofd.FileName,true,true,misValue,null,null,false,misValue,null,false,false,misValue,misValue,misValue,false);
            foreach (Excel.Worksheet ws in wb.Worksheets)
            {
                listBox6.Items.Add(ws.Name);
            }
            listBox1.Text = "Column A";
            listBox2.Text = "Column B";
           
            listBox6.SelectedIndex = 0;
          
            listBox10.Text = "<None>";
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


            if (tabControl1.SelectedIndex == 0)
            {
                //save
                dtsidefill.Rows.Add();
                dtsidefill.Rows[0]["Name"] = "next";
                dtsidefill.Rows[0]["DisplayName"] = "Next";
                dtsidefill.Rows[0]["ShortcutKey"] = "";
                dtsidefill.Rows[0]["Visible"] = true;

                //close
                dtsidefill.Rows.Add();
                dtsidefill.Rows[1]["Name"] = "quit";
                dtsidefill.Rows[1]["DisplayName"] = "Quit";
                dtsidefill.Rows[1]["ShortcutKey"] = "Esc";
                dtsidefill.Rows[1]["Visible"] = true;
            }
            else if (tabControl1.SelectedIndex == 1)
            {
                //back
                dtsidefill.Rows.Add();
                dtsidefill.Rows[0]["Name"] = "back";
                dtsidefill.Rows[0]["DisplayName"] = "Back";
                dtsidefill.Rows[0]["ShortcutKey"] = "";
                dtsidefill.Rows[0]["Visible"] = true;
                //save
                dtsidefill.Rows.Add();
                dtsidefill.Rows[1]["Name"] = "next2";
                dtsidefill.Rows[1]["DisplayName"] = "Next";
                dtsidefill.Rows[1]["ShortcutKey"] = "";
                dtsidefill.Rows[1]["Visible"] = true;

                //close
                dtsidefill.Rows.Add();
                dtsidefill.Rows[2]["Name"] = "quit";
                dtsidefill.Rows[2]["DisplayName"] = "Quit";
                dtsidefill.Rows[2]["ShortcutKey"] = "Esc";
                dtsidefill.Rows[2]["Visible"] = true;

            }

            else if (tabControl1.SelectedIndex == 2)
            {

                //back
                dtsidefill.Rows.Add();
                dtsidefill.Rows[0]["Name"] = "back2";
                dtsidefill.Rows[0]["DisplayName"] = "Back";
                dtsidefill.Rows[0]["ShortcutKey"] = "";
                dtsidefill.Rows[0]["Visible"] = true;
                //save
                dtsidefill.Rows.Add();
                dtsidefill.Rows[1]["Name"] = "finish";
                dtsidefill.Rows[1]["DisplayName"] = "Finish";
                dtsidefill.Rows[1]["ShortcutKey"] = "";
                dtsidefill.Rows[1]["Visible"] = true;

                //close
                dtsidefill.Rows.Add();
                dtsidefill.Rows[2]["Name"] = "quit";
                dtsidefill.Rows[2]["DisplayName"] = "Quit";
                dtsidefill.Rows[2]["ShortcutKey"] = "Esc";
                dtsidefill.Rows[2]["Visible"] = true;

            }



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

            if (name == "next")
            {
                if (textBox1.Text == "")
                {
                    textBox1.BackColor = Color.Aqua;
                }
                else
                {
                    tabControl1.SelectedIndex = 1;
                    SideFill();
                    wb = (Excel.Workbook)apl.Workbooks.Open(ofd.FileName, true, true, misValue, null, null, false, misValue, null, false, false, misValue, misValue, misValue, false);
                    foreach (Excel.Worksheet ws in wb.Worksheets)
                    {
                        listBox6.Items.Add(ws.Name);
                    }
                    listBox1.Text = "Column E";
                    listBox2.Text = "Column F";
                   
                    listBox6.SelectedIndex = 0;

                    listBox10.Text = "Column L";
                }
            }
            else if (name == "back")
            {

                tabControl1.SelectedIndex = 0;
                SideFill();
            }


            else if (name == "next2")
            {
                wb = (Excel.Workbook)apl.Workbooks.Open(ofd.FileName, true, true, misValue, null, null, false, misValue, null, false, false, misValue, misValue, misValue, false);
                ws = (Excel.Worksheet)wb.Worksheets[listBox6.SelectedIndex + 1];
                dataGridView3.Rows.Clear();
                ProgrBar.Minimum = 0;
                ProgrBar.Maximum = 4200;
                ProgrBar.Visible = true;
                int i = 0;
                Excel.Range range;
                range = ws.UsedRange;
                while ((range.Cells[(i + 6), 1] as Excel.Range).Value2 != null)
                {
                    ProgrBar.Value = i;
                    dataGridView3.Rows.Add();
                    dataGridView3.Rows[i].Cells["sno"].Value = (i + 1);
                    dataGridView3.Rows[i].Cells["desc"].Value = (range.Cells[(i + 6), listBox1.SelectedIndex + 1] as Excel.Range).Value2.ToString().Replace("  ", " ").Trim();
                    dataGridView3.Rows[i].Cells["pack"].Value = (range.Cells[(i + 6), listBox2.SelectedIndex + 1] as Excel.Range).Value2.ToString().Replace("  ", " ").Trim();
                    


                        dataGridView3.Columns["code"].Visible = true;
                        dataGridView3.Rows[i].Cells["code"].Value = (range.Cells[(i + 6), listBox10.SelectedIndex] as Excel.Range).Value2;
                   

                    i++;
                }
                ProgrBar.Value = 0;
                ProgrBar.Visible = false;
                wb.Close(false, ofd.FileName, misValue);
                tabControl1.SelectedIndex = 2;
                SideFill();
            }
            else if (name == "back2")
            {
                dataGridView3.Rows.Clear();
                tabControl1.SelectedIndex = 1;
                SideFill();
            }
            else if (name == "finish")
            {
                DataTable dtDesc = new DataTable("Description");
                dtDesc.Clear();
                Database.GetSqlData("select * from Description", dtDesc);
                ProgrBar.Minimum = 0;
                ProgrBar.Maximum = dtDesc.Rows.Count; ;
                ProgrBar.Visible = true;
                for (int i = 0; i < dtDesc.Rows.Count; i++)
                {
                    ProgrBar.Value = i;
                    dtDesc.Rows[i]["Description"] = dtDesc.Rows[i]["Description"].ToString().Replace("  ", " ").Trim();
                }
                ProgrBar.Value = 0;
                ProgrBar.Visible = false;
                Database.SaveData(dtDesc);


                for (int i = 0; i < dataGridView3.Rows.Count; i++)
                {
                    string descrip = funs.Select_des_id(dataGridView3.Rows[i].Cells["desc"].Value.ToString(), dataGridView3.Rows[i].Cells["pack"].Value.ToString());

                    if (descrip == "" || descrip == "0")
                    {
                       

                        // row["state"] = "Modified";
                    }
                    else
                    {
                        DataRow row = dtDesc.Select("des_id='" + descrip + "'").FirstOrDefault();

                        if (dataGridView3.Rows[i].Cells["code"].Value != null)
                        {
                            row["ShortCode"] = dataGridView3.Rows[i].Cells["code"].Value;
                        }

                    }
                }

                Database.SaveData(dtDesc);
                MessageBox.Show("Codes Imported successfully");
                Master.UpdateDecription();
                Master.UpdateDecriptionInfo();
                apl.Quit();
                this.Close();
                this.Dispose();
            }

            if (name == "quit")
            {
                this.Close();
                this.Dispose();
            }


        }

        private void button9_Click(object sender, EventArgs e)
        {
            wb = (Excel.Workbook)apl.Workbooks.Open(ofd.FileName, true, true, misValue, null, null, false, misValue, null, false, false, misValue, misValue, misValue, false);
            ws = (Excel.Worksheet) wb.Worksheets[listBox6.SelectedIndex + 1];
            dataGridView3.Rows.Clear();
            int i = 0;
            Excel.Range range;
            range = ws.UsedRange;
            while ((range.Cells[(i + 1), 1] as Excel.Range).Value2 != null)
            {
                dataGridView3.Rows.Add();
                dataGridView3.Rows[i].Cells["sno"].Value = (i + 1);
                dataGridView3.Rows[i].Cells["desc"].Value = (range.Cells[(i + 1), listBox1.SelectedIndex + 1] as Excel.Range).Value2.ToString().Replace("  ", " ").Trim();
                dataGridView3.Rows[i].Cells["pack"].Value = (range.Cells[(i + 1), listBox2.SelectedIndex + 1] as Excel.Range).Value2.ToString().Replace("  ", " ").Trim();
               
             

               

                if (listBox10.Text != "<None>")
                {
                    dataGridView3.Columns["MRP1"].Visible = true;
                    dataGridView3.Rows[i].Cells["MRP1"].Value = (range.Cells[(i + 1), listBox10.SelectedIndex] as Excel.Range).Value2;
                }
                else
                {
                    dataGridView3.Columns["MRP1"].Visible = false;
                }

                i++;
            }
            wb.Close(false, ofd.FileName, misValue);
            tabControl1.SelectedIndex = 2;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            dataGridView3.Rows.Clear();
            tabControl1.SelectedIndex = 1;
        }

        private void frmImpRate_Load(object sender, EventArgs e)
        {

          


           

          



            ofd.Filter = "Excel Files(*.xls)|*.xlsx";
           
            foreach (DataGridViewColumn column in dataGridView3.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            

            this.Size = this.MdiParent.Size;
            SideFill();
        }
        private void button6_Click(object sender, EventArgs e)
        {
            if (DialogResult.OK == ofd.ShowDialog())
            {
                fName = ofd.FileName;
                textBox1.Text = fName;
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            DataTable dtDesc = new DataTable("Description");
            dtDesc.Clear();
            Database.GetSqlData("select * from Description", dtDesc);

            for (int i = 0; i < dtDesc.Rows.Count; i++)
            {
                dtDesc.Rows[i]["Description"] = dtDesc.Rows[i]["Description"].ToString().Replace("  ", " ").Trim();   
            }
            Database.SaveData(dtDesc);


            for (int i = 0; i < dataGridView3.Rows.Count; i++)
            {
                string descrip = funs.Select_des_id(dataGridView3.Rows[i].Cells["desc"].Value.ToString(), dataGridView3.Rows[i].Cells["pack"].Value.ToString());

                if (descrip != "")
                {
                    DataRow row = dtDesc.Select("des_id='" + descrip + "' ").FirstOrDefault();

                    if (dataGridView3.Rows[i].Cells["prate"].Value != null)
                    {
                        row["Purchase_rate"] = dataGridView3.Rows[i].Cells["prate"].Value;
                    }

                    if (dataGridView3.Rows[i].Cells["wrate"].Value != null)
                    {
                        row["Wholesale"] = dataGridView3.Rows[i].Cells["wrate"].Value;
                    }

                    if (dataGridView3.Rows[i].Cells["rrate"].Value != null)
                    {
                        row["Retail"] = dataGridView3.Rows[i].Cells["rrate"].Value;
                    }


                    if (dataGridView3.Rows[i].Cells["Rate_X"].Value != null)
                    {
                        row["Rate_X"] = dataGridView3.Rows[i].Cells["Rate_X"].Value;
                    }

                    if (dataGridView3.Rows[i].Cells["Rate_Y"].Value != null)
                    {
                        row["Rate_Y"] = dataGridView3.Rows[i].Cells["Rate_Y"].Value;
                    }

                    if (dataGridView3.Rows[i].Cells["Rate_Z"].Value != null)
                    {
                        row["Rate_Z"] = dataGridView3.Rows[i].Cells["Rate_Z"].Value;
                    }

                    if (dataGridView3.Rows[i].Cells["MRP1"].Value != null)
                    {
                        row["MRP"] = dataGridView3.Rows[i].Cells["MRP1"].Value;
                    }

                    row["state"] = "Modified";

                }
            }

            Database.SaveData(dtDesc);
            MessageBox.Show("Rates Imported successfully");

            Master.UpdateDecription();
            Master.UpdateDecriptionInfo();

            apl.Quit();
            this.Close();
            this.Dispose();
          
        }

        private void frmImpRate_KeyDown(object sender, KeyEventArgs e)
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

        private void tabControl1_KeyDown(object sender, KeyEventArgs e)
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

        private void dataGridView3_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView3.CurrentCell.OwningColumn.Name == "sno")
            {
                SendKeys.Send("{right}");
                this.Activate();
            }

            foreach (DataGridViewColumn column in dataGridView3.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

          

         
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void listBox6_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listBox4_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listBox5_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listBox7_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listBox8_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
