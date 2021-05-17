using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net;

namespace faspi
{
    public partial class frm_uploaddoc : Form
    {
        string gvid = "0";
        string filename = "";
        String sourcefilepath = "";
        OpenFileDialog opFile = new OpenFileDialog();
        string fileName = "";
        string extension = "";
        public frm_uploaddoc(string Vid)
        {
            InitializeComponent();
            gvid = Vid;
        }

        private string GetfileName(string filepath)
        {
            fileName = Path.GetFileNameWithoutExtension(sourcefilepath);
            fileName = "";
            extension = Path.GetExtension(sourcefilepath);
            fileName = DateTime.Now.ToString("yyyyMMddhmmff");
            return fileName + extension;
        }

        private void frm_uploaddoc_Load(object sender, EventArgs e)
        {
            if (gvid == "0")
            {
                return;
            }
            else
            {
                filename = Database.GetScalarText("Select uploaddoc from Voucherinfo where Vi_id='" + gvid+"'");
                if (filename == "")
                {
                    button1.Visible = false;
                }
                else
                {
                    button1.Visible = true;
                }

            }
        }

        private void frm_uploaddoc_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
                this.Dispose();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            filename = Database.GetScalarText("Select uploaddoc from Voucherinfo where Vi_id='" + gvid+"'");
            if (filename != "")
            {
                extension = Path.GetExtension(filename);
                if (extension == ".pdf")
                {
                    this.Visible = false;
                   
                    System.Diagnostics.Process.Start(Application.StartupPath + "\\Document\\" + filename);
                }
                else if (extension == ".jpg" || extension==".png")
                {
                    this.Visible = false;
                    System.Diagnostics.Process.Start(Application.StartupPath + "\\Document\\" + filename);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            opFile.Title = "Select a Document";

            if (opFile.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    textBox1.Text = opFile.FileName;

                    sourcefilepath = textBox1.Text;
                    if (textBox1.Text == null || textBox1.Text=="")
                    {


                        textBox1.Text = "";
                    }
                    string FiName = GetfileName(textBox1.Text);
                    DirectoryInfo dInfo = new System.IO.DirectoryInfo(Application.StartupPath + "\\Document");

                    if (dInfo.Exists == false)
                    {
                        Directory.CreateDirectory(Application.StartupPath + "\\Document");

                        File.Move(opFile.FileName, Application.StartupPath + "\\Document\\" + FiName);
                        Database.CommandExecutor("Update voucherinfo set uploaddoc='" + FiName + "' where Vi_id='" + gvid+"'");
                      
                    }
                    else
                    {

                        File.Move(opFile.FileName, Application.StartupPath + "\\Document\\" + FiName);
                        Database.CommandExecutor("Update voucherinfo set uploaddoc='" + FiName + "' where Vi_id='" + gvid+"'");
                    }
                   
                }
                catch (Exception exp)
                {
                    MessageBox.Show("Unable to open file " + exp.Message);
                }
            }
            else
            {
                opFile.Dispose();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
