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
    public partial class frm_odetails : Form
    {
        public string field1, field2, field3, field4, field5, field6, field7, field8 = "";

        public frm_odetails(string gfield1, string gfield2, string gfield3, string gfield4, string gfield5, string gfield6, string gfield7, string gfield8)
        {
            InitializeComponent();
             field1 = gfield1;
             field2 = gfield2;
             field3 = gfield3;
             field4 = gfield4;
             field5 = gfield5;
             field6 = gfield6;
             field7 = gfield7;
             field8 = gfield8;
        }

        public void LoadData()
        {
            for (int i = 0; i < Master.TransportDetails.Rows.Count; i++)
            {
                if (Master.TransportDetails.Rows[i]["status"].ToString() != "Not Visible")
                {
                    ansGridView5.Rows.Add();
                    ansGridView5.Rows[ansGridView5.Rows.Count - 1].Cells["displayname"].Value = Master.TransportDetails.Rows[i]["ShowingName"].ToString();
                    ansGridView5.Rows[ansGridView5.Rows.Count - 1].Cells["FName"].Value = Master.TransportDetails.Rows[i]["FName"].ToString();
                    if (Master.TransportDetails.Rows[i]["FName"].ToString() == "Field1")
                    {
                        ansGridView5.Rows[ansGridView5.Rows.Count - 1].Cells["value"].Value = field1;

                    }
                    else if (Master.TransportDetails.Rows[i]["FName"].ToString() == "Field2")
                    {
                        ansGridView5.Rows[ansGridView5.Rows.Count - 1].Cells["value"].Value = field2;
                    }
                    else if (Master.TransportDetails.Rows[i]["FName"].ToString() == "Field3")
                    {
                        ansGridView5.Rows[ansGridView5.Rows.Count - 1].Cells["value"].Value = field3;
                    }
                    else if (Master.TransportDetails.Rows[i]["FName"].ToString() == "Field4")
                    {
                        ansGridView5.Rows[ansGridView5.Rows.Count - 1].Cells["value"].Value = field4;
                    }
                    else if (Master.TransportDetails.Rows[i]["FName"].ToString() == "Field5")
                    {
                        ansGridView5.Rows[ansGridView5.Rows.Count - 1].Cells["value"].Value = field5;
                    }
                    else if (Master.TransportDetails.Rows[i]["FName"].ToString() == "Field6")
                    {
                        ansGridView5.Rows[ansGridView5.Rows.Count - 1].Cells["value"].Value = field6;
                    }
                    else if (Master.TransportDetails.Rows[i]["FName"].ToString() == "Field7")
                    {
                        ansGridView5.Rows[ansGridView5.Rows.Count - 1].Cells["value"].Value = field7;
                    }
                    else if (Master.TransportDetails.Rows[i]["FName"].ToString() == "Field8")
                    {
                        ansGridView5.Rows[ansGridView5.Rows.Count - 1].Cells["value"].Value = field8;
                    }
                }
            }
        }

        private void frm_odetails_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                for (int i = 0; i < ansGridView5.Rows.Count; i++)
                {
                    if (ansGridView5.Rows[i].Cells["FName"].Value.ToString() == "Field1")
                    {
                      field1  = ansGridView5.Rows[i].Cells["value"].Value.ToString();
                    }
                    else if (ansGridView5.Rows[i].Cells["FName"].Value.ToString() == "Field2")
                    {
                       field2 = ansGridView5.Rows[i].Cells["value"].Value.ToString();
                    }
                    else if (ansGridView5.Rows[i].Cells["FName"].Value.ToString() == "Field3")
                    {
                        field3 = ansGridView5.Rows[i].Cells["value"].Value.ToString();
                    }
                    else if (ansGridView5.Rows[i].Cells["FName"].Value.ToString() == "Field4")
                    {
                        field4 = ansGridView5.Rows[i].Cells["value"].Value.ToString();
                    }
                    else if (ansGridView5.Rows[i].Cells["FName"].Value.ToString() == "Field5")
                    {
                        field5 = ansGridView5.Rows[i].Cells["value"].Value.ToString();
                    }
                    else if (ansGridView5.Rows[i].Cells["FName"].Value.ToString() == "Field6")
                    {
                        field6 = ansGridView5.Rows[i].Cells["value"].Value.ToString();
                    }
                    else if (ansGridView5.Rows[i].Cells["FName"].Value.ToString() == "Field7")
                    {
                        field7 = ansGridView5.Rows[i].Cells["value"].Value.ToString();
                    }
                    else if (ansGridView5.Rows[i].Cells["FName"].Value.ToString() == "Field8")
                    {
                        field8 = ansGridView5.Rows[i].Cells["value"].Value.ToString();
                    }
                }

                this.Close();
                this.Dispose();
            }
        }

    }
}
