using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace faspi
{
    public partial class frm_treemode : Form
    {
        public frm_treemode()
        {
            InitializeComponent();
        }

        private void frm_treemode_Load(object sender, EventArgs e)
        {
            TreeNode tNode;
            tNode = treeView1.Nodes.Add("Menus");
           
            int node1 = 0;
           
            DataTable dtpagerole= new DataTable();
            Database.GetSqlData("Select PageId,ParentPageid,PageTitle from WinPage where ParentPageid=0", dtpagerole);
            for (int i = 0; i < dtpagerole.Rows.Count; i++)
            {


                int node2 = 0;
                int node3 = 0;
                treeView1.Nodes[0].Nodes.Add(dtpagerole.Rows[i]["PageTitle"].ToString());
                DataTable dtpage = new DataTable();
                Database.GetSqlData("Select Pageid,PageTitle from WinPage where ParentPageid=" + dtpagerole.Rows[i]["Pageid"].ToString(), dtpage);
                for (int j = 0; j < dtpage.Rows.Count; j++)
                {
                    

                        treeView1.Nodes[0].Nodes[node1].Nodes.Add(dtpage.Rows[j]["PageTitle"].ToString());
                      
                        DataTable dtpagesub = new DataTable();
                        Database.GetSqlData("Select Pageid,PageTitle from WinPage where ParentPageid=" + dtpage.Rows[j]["Pageid"].ToString(), dtpagesub);
                        for (int k = 0; k < dtpagesub.Rows.Count; k++)
                        {
                            treeView1.Nodes[0].Nodes[node1].Nodes[node2].Nodes.Add(dtpagesub.Rows[k]["PageTitle"].ToString());


                            DataTable dtpagesubsub = new DataTable();
                            Database.GetSqlData("Select PageTitle from WinPage where ParentPageid=" + dtpagesub.Rows[k]["Pageid"].ToString(), dtpagesubsub);
                            for (int l = 0; l < dtpagesubsub.Rows.Count; l++)
                            {
                                treeView1.Nodes[0].Nodes[node1].Nodes[node2].Nodes[node3].Nodes.Add(dtpagesubsub.Rows[l]["PageTitle"].ToString());

                            }

                            node3++;
                        }
                        node2++;
                }

                node1++;


                //treeView1.Nodes[0].Nodes.Add("Vb.net-informations.com");
                //treeView1.Nodes[0].Nodes[1].Nodes.Add("String Tutorial");
                //treeView1.Nodes[0].Nodes[1].Nodes.Add("Excel Tutorial");

                //treeView1.Nodes[0].Nodes.Add("Csharp.net-informations.com");
                //treeView1.Nodes[0].Nodes[2].Nodes.Add("ADO.NET");
                //treeView1.Nodes[0].Nodes[2].Nodes[0].Nodes.Add("Dataset");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show(treeView1.SelectedNode.FullPath.ToString());
        }
    }
}
