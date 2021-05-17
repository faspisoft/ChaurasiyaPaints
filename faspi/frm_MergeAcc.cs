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
    public partial class frm_MergeAcc : Form
    {
        String strCombo;
        
        public frm_MergeAcc()
        {
            InitializeComponent();
        }

        private void save()
        {
            string ac_idfrom = funs.Select_ac_id(textBox1.Text);
            string ac_idTo = funs.Select_ac_id(textBox2.Text);
            double balancefrom = 0;
            double balance2from = 0;
            double balanceto = 0;
            double balance2to = 0;
            double totbal = 0;
            double totbal2 = 0;

            balancefrom = Database.GetScalarDecimal("Select Balance from Account where Ac_id='" + ac_idfrom + "' ");
            balance2from = Database.GetScalarDecimal("Select Balance2 from Account where Ac_id='" + ac_idfrom + "' ");
            balanceto = Database.GetScalarDecimal("Select Balance from Account where Ac_id='" + ac_idTo + "' ");
            balance2to = Database.GetScalarDecimal("Select Balance2 from Account where Ac_id='" + ac_idTo + "' ");

            totbal = balancefrom + balanceto;
            totbal2 = balance2to + balance2from;

            Database.CommandExecutor("Update Account set Balance=" + totbal + "  where ac_id='" + ac_idTo + "'");
            Database.CommandExecutor("Update Account set Balance2=" + totbal2 + "  where ac_id='" + ac_idTo + "'");

            Database.CommandExecutor("Update BILLBYBILL set Ac_id='"+ac_idTo+"' where Ac_id='"+ac_idfrom+"'");
            Database.CommandExecutor("Update CHARGES set Ac_id='" + ac_idTo + "' where Ac_id='" + ac_idfrom+"'");
            Database.CommandExecutor("Update DisAfterTax set Ac_id='" + ac_idTo + "' where Ac_id='" + ac_idfrom + "'");
            Database.CommandExecutor("Update importantdate set Ac_id='" + ac_idTo + "' where Ac_id='" + ac_idfrom + "'");
            Database.CommandExecutor("Update ITEMCHARGES set Accid='" + ac_idTo + "' where Accid='" + ac_idfrom + "'");
            Database.CommandExecutor("Update Journal set Ac_id='" + ac_idTo + "' where Ac_id='" + ac_idfrom + "'");
            Database.CommandExecutor("Update PARTYRATE set Ac_id='" + ac_idTo + "' where Ac_id='" + ac_idfrom + "'");
            Database.CommandExecutor("Update Stock set godown_id='" + ac_idTo + "' where godown_id='" + ac_idfrom + "'");
            Database.CommandExecutor("Update TAXCATEGORY set PA='" + ac_idTo + "' where PA='" + ac_idfrom + "'");
            Database.CommandExecutor("Update TAXCATEGORY set SA='" + ac_idTo + "' where SA='" + ac_idfrom + "'");
            Database.CommandExecutor("Update TAXCATEGORY set PAEX='" + ac_idTo + "' where PAEX='" + ac_idfrom + "'");
            Database.CommandExecutor("Update TAXCATEGORY set SAEX='" + ac_idTo + "' where SAEX='" + ac_idfrom + "'");
            Database.CommandExecutor("Update TAXCATEGORY set PCA='" + ac_idTo + "' where PCA='" + ac_idfrom + "'");
            Database.CommandExecutor("Update TAXCATEGORY set SCA='" + ac_idTo + "' where SCA='" + ac_idfrom + "'");
            Database.CommandExecutor("Update TAXCATEGORY set PCAEX='" + ac_idTo + "' where PCAEX='" + ac_idfrom + "'");
            Database.CommandExecutor("Update TAXCATEGORY set SCAEX='" + ac_idTo + "' where SCAEX='" + ac_idfrom + "'");
            Database.CommandExecutor("Update TAXCATEGORY set PTA1='" + ac_idTo + "' where PTA1='" + ac_idfrom + "'");
            Database.CommandExecutor("Update TAXCATEGORY set PTA2='" + ac_idTo + "' where PTA2='" + ac_idfrom + "'");
            Database.CommandExecutor("Update TAXCATEGORY set PTA3='" + ac_idTo + "' where PTA3='" + ac_idfrom + "'");
            Database.CommandExecutor("Update TAXCATEGORY set STA1='" + ac_idTo + "' where STA1='" + ac_idfrom + "'");
            Database.CommandExecutor("Update TAXCATEGORY set STA2='" + ac_idTo + "' where STA2='" + ac_idfrom + "'");
            Database.CommandExecutor("Update TAXCATEGORY set STA3='" + ac_idTo + "' where STA3='" + ac_idfrom + "'");
            Database.CommandExecutor("Update TAXCATEGORY set RCMPay=" + ac_idTo + " where RCMPay='" + ac_idfrom + "'");
            Database.CommandExecutor("Update TAXCATEGORY set RCMITC='" + ac_idTo + "' where RCMITC='" + ac_idfrom + "'");
            Database.CommandExecutor("Update TAXCATEGORY set RCMEli='" + ac_idTo + "' where RCMEli='" + ac_idfrom + "'");
            Database.CommandExecutor("Update VOUCHERDET set godown_id='" + ac_idTo + "' where godown_id='" + ac_idfrom + "'");
            Database.CommandExecutor("Update VOUCHERDET set pur_sale_acc='" + ac_idTo + "' where pur_sale_acc='" + ac_idfrom + "'");
            Database.CommandExecutor("Update VOUCHERDET set tax1='" + ac_idTo + "' where tax1='" + ac_idfrom + "'");
            Database.CommandExecutor("Update VOUCHERDET set tax2='" + ac_idTo + "' where tax2='" + ac_idfrom + "'");
            Database.CommandExecutor("Update VOUCHERDET set tax3='" + ac_idTo + "' where tax3='" + ac_idfrom + "'");
            Database.CommandExecutor("Update VOUCHERDET set tax4='" + ac_idTo + "' where tax4='" + ac_idfrom + "'");
            Database.CommandExecutor("Update VOUCHERDET set DATac_id='" + ac_idTo + "' where DATac_id='" + ac_idfrom + "'");
            Database.CommandExecutor("Update VOUCHERDET set RCMac_id='" + ac_idTo + "' where RCMac_id='" + ac_idfrom + "'");
            Database.CommandExecutor("Update VOUCHARGES set Accid='" + ac_idTo + "' where Accid='" + ac_idfrom + "'");
            Database.CommandExecutor("Update VOUCHERACTOTAL set Accid='" + ac_idTo + "' where Accid='" + ac_idfrom + "'");
            Database.CommandExecutor("Update Voucherinfo set ac_id='" + ac_idTo + "' where ac_id='" + ac_idfrom + "'");
            Database.CommandExecutor("Update Voucherpaydet set acc_id='" + ac_idTo + "' where acc_id='" + ac_idfrom + "'");
            Database.CommandExecutor("Update Voucherinfo set ac_id2='" + ac_idTo + "' where ac_id2='" + ac_idfrom + "'");
            Database.CommandExecutor("Delete from Account where Ac_id='" + ac_idfrom+"'");
            Master.UpdateAll();
            funs.ShowBalloonTip("Merge","Save Successfully");
        }


        private void Button1_Click(object sender, EventArgs e)
        {
            if (validate() == true)
            {
                try
                {
                    Database.BeginTran();
                    save();
                    textBox1.Text = "";
                    textBox2.Text = "";
                    textBox1.Focus();
                    Database.CommitTran();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Account Not Merged", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Database.RollbackTran();
                }
            }
        }

        private bool validate()
        {
            if (textBox1.Text == "")
            {
                textBox1.Focus();
                return false;
            }
            if (textBox2.Text == "")
            {
                textBox2.Focus();
                return false;
            }
            if (textBox1.Text == textBox2.Text)
            {
                textBox2.Focus();
                MessageBox.Show("Both Values Must not be Same.");
                return false;
            }
            return true;
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
           // strCombo = funs.GetStrCombo("*");
            strCombo = funs.GetStrComboled("*");
            textBox1.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, textBox1.Text, 1);
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            //strCombo = funs.GetStrCombo("*");
            strCombo = funs.GetStrComboled("*");
            textBox2.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, textBox2.Text, 1);
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox1);
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox1);
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox2);
        }

        private void frm_MergeAcc_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.S)
            {
                if (validate() == true)
                {
                    try
                    {
                        Database.BeginTran();
                        save();
                        textBox1.Text = "";
                        textBox2.Text = "";
                        textBox1.Focus();
                        Database.CommitTran();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Account Not Merged", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Database.RollbackTran();
                    }
                }
            }

            else if (e.KeyCode == Keys.Escape)
            {
                this.Close();
                this.Dispose();
            }
                 
        }
    }
}
