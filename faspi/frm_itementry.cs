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
    public partial class frm_itementry : Form
    {
        public bool TaxChanged;
        public string Taxabelamount, pur_sale_acc, rate1,  taxamt1,  tax1,  rate2,  taxamt2,  tax2,  rate3,  taxamt3,  tax3,  rate4,  taxamt4,  tax4;

        public frm_itementry(bool TaxChanged, string Taxabelamount, string pur_sale_acc, string rate1, string taxamt1, string tax1, string rate2, string taxamt2, string tax2, string rate3, string taxamt3, string tax3, string rate4, string taxamt4, string tax4)
        {
            InitializeComponent();
            this.TaxChanged = TaxChanged;

            Amt0.Text = Taxabelamount;
            Acc0.Text = funs.Select_ac_nm(pur_sale_acc);

            Per1.Text = rate1;
            this.taxamt1 = taxamt1;
            Amt1.Text = taxamt1;
            Acc1.Text = funs.Select_ac_nm(tax1);

            Per2.Text = rate2;
            this.taxamt2 = taxamt2;
            Amt2.Text = taxamt2;
            Acc2.Text = funs.Select_ac_nm(tax2);

            Per3.Text = rate3;
            this.taxamt3 = taxamt3;
            Amt3.Text = taxamt3;
            Acc3.Text = funs.Select_ac_nm(tax3);

            Per4.Text = rate4;
            this.taxamt4 = taxamt4;
            Amt4.Text = taxamt4;
            Acc4.Text = funs.Select_ac_nm(tax4);
        }

        private void frm_itementry_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                if(double.Parse(this.taxamt1)!=double.Parse(Amt1.Text))
                {
                    this.taxamt1 = Amt1.Text;
                    this.TaxChanged = true;
                }
                if (double.Parse(this.taxamt2) != double.Parse(Amt2.Text))
                {
                    this.taxamt2 = Amt2.Text;
                    this.TaxChanged = true;
                }
                if (double.Parse(this.taxamt3) != double.Parse(Amt3.Text))
                {
                    this.taxamt3 = Amt3.Text;
                    this.TaxChanged = true;
                }
                if (double.Parse(this.taxamt4) != double.Parse(Amt4.Text))
                {
                    this.taxamt4 = Amt4.Text;
                    this.TaxChanged = true;
                }

                this.Close();
            }


        }

       
  
    }
}