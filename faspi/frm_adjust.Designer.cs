namespace faspi
{
    partial class frm_adjust
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.ansGridView1 = new faspiGrid.ansGridView(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.sno = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.reffno = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.amount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.vi_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.reff_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Ac_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.ansGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // ansGridView1
            // 
            this.ansGridView1.AllowUserToDeleteRows = false;
            this.ansGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ansGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.sno,
            this.reffno,
            this.amount,
            this.vi_id,
            this.reff_id,
            this.Ac_id});
            this.ansGridView1.Location = new System.Drawing.Point(1, 1);
            this.ansGridView1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ansGridView1.MultiSelect = false;
            this.ansGridView1.Name = "ansGridView1";
            this.ansGridView1.RowHeadersVisible = false;
            this.ansGridView1.RowHeadersWidth = 20;
            this.ansGridView1.Size = new System.Drawing.Size(341, 259);
            this.ansGridView1.TabIndex = 6;
            this.ansGridView1.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.ansGridView1_CellEndEdit);
            this.ansGridView1.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.ansGridView1_CellEnter);
            this.ansGridView1.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.ansGridView1_DataError);
            this.ansGridView1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ansGridView1_KeyDown);
            this.ansGridView1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ansGridView1_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(11, 277);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 18);
            this.label1.TabIndex = 7;
            this.label1.Text = "Total : ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.OrangeRed;
            this.label2.Location = new System.Drawing.Point(74, 277);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(36, 18);
            this.label2.TabIndex = 8;
            this.label2.Text = "0.00";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.OrangeRed;
            this.label3.Location = new System.Drawing.Point(291, 277);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 18);
            this.label3.TabIndex = 10;
            this.label3.Text = "0.00";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(203, 277);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(73, 18);
            this.label4.TabIndex = 9;
            this.label4.Text = "Balance : ";
            // 
            // sno
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            this.sno.DefaultCellStyle = dataGridViewCellStyle1;
            this.sno.HeaderText = "S.No.";
            this.sno.Name = "sno";
            this.sno.ReadOnly = true;
            this.sno.Width = 50;
            // 
            // reffno
            // 
            this.reffno.HeaderText = "Refference No";
            this.reffno.Name = "reffno";
            this.reffno.ReadOnly = true;
            this.reffno.Width = 150;
            // 
            // amount
            // 
            this.amount.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.amount.DefaultCellStyle = dataGridViewCellStyle2;
            this.amount.HeaderText = "Amount";
            this.amount.Name = "amount";
            this.amount.Width = 110;
            // 
            // vi_id
            // 
            this.vi_id.HeaderText = "vi_id";
            this.vi_id.Name = "vi_id";
            this.vi_id.Visible = false;
            // 
            // reff_id
            // 
            this.reff_id.HeaderText = "reff_id";
            this.reff_id.Name = "reff_id";
            this.reff_id.Visible = false;
            // 
            // Ac_id
            // 
            this.Ac_id.HeaderText = "Ac_id";
            this.Ac_id.Name = "Ac_id";
            this.Ac_id.Visible = false;
            // 
            // frm_adjust
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(342, 314);
            this.ControlBox = false;
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ansGridView1);
            this.KeyPreview = true;
            this.Name = "frm_adjust";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.frm_adjust_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frm_adjust_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.ansGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private faspiGrid.ansGridView ansGridView1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DataGridViewTextBoxColumn sno;
        private System.Windows.Forms.DataGridViewTextBoxColumn reffno;
        private System.Windows.Forms.DataGridViewTextBoxColumn amount;
        private System.Windows.Forms.DataGridViewTextBoxColumn vi_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn reff_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn Ac_id;
    }
}