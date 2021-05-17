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
    public partial class frm_Merge : Form
    {
        public frm_Merge()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                Merge frm = new Merge();
                frm.mode = "2Packings";
                frm.ShowDialog();
            }
            else
            {
                Merge frm = new Merge();
                frm.mode = "2DisplayName";
                frm.ShowDialog();
            }


        }
    }
}
