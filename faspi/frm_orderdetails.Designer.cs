namespace faspi
{
    partial class frm_orderdetails
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.ansGridView5 = new faspiGrid.ansGridView(this.components);
            this.Orderno = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Vi_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.vdate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.itemsr = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Pack = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.qty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Select = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.ansGridView5)).BeginInit();
            this.SuspendLayout();
            // 
            // ansGridView5
            // 
            this.ansGridView5.AllowUserToAddRows = false;
            this.ansGridView5.AllowUserToDeleteRows = false;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.ansGridView5.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.ansGridView5.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ansGridView5.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Orderno,
            this.Vi_id,
            this.vdate,
            this.itemsr,
            this.description,
            this.Pack,
            this.qty,
            this.rate,
            this.Select});
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.ansGridView5.DefaultCellStyle = dataGridViewCellStyle4;
            this.ansGridView5.Location = new System.Drawing.Point(0, 0);
            this.ansGridView5.MultiSelect = false;
            this.ansGridView5.Name = "ansGridView5";
            this.ansGridView5.RowHeadersVisible = false;
            this.ansGridView5.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.ansGridView5.Size = new System.Drawing.Size(802, 463);
            this.ansGridView5.TabIndex = 24;
            // 
            // Orderno
            // 
            this.Orderno.HeaderText = "Orderno";
            this.Orderno.Name = "Orderno";
            this.Orderno.ReadOnly = true;
            // 
            // Vi_id
            // 
            this.Vi_id.HeaderText = "Vi_id";
            this.Vi_id.Name = "Vi_id";
            this.Vi_id.ReadOnly = true;
            this.Vi_id.Visible = false;
            // 
            // vdate
            // 
            this.vdate.HeaderText = "Date";
            this.vdate.Name = "vdate";
            this.vdate.ReadOnly = true;
            // 
            // itemsr
            // 
            this.itemsr.HeaderText = "Itemsr";
            this.itemsr.Name = "itemsr";
            this.itemsr.ReadOnly = true;
            this.itemsr.Visible = false;
            // 
            // description
            // 
            this.description.HeaderText = "Item Name";
            this.description.Name = "description";
            this.description.ReadOnly = true;
            this.description.Width = 140;
            // 
            // Pack
            // 
            this.Pack.HeaderText = "Pack";
            this.Pack.Name = "Pack";
            this.Pack.ReadOnly = true;
            // 
            // qty
            // 
            this.qty.HeaderText = "Qty";
            this.qty.Name = "qty";
            this.qty.ReadOnly = true;
            // 
            // rate
            // 
            this.rate.HeaderText = "Rate";
            this.rate.Name = "rate";
            this.rate.ReadOnly = true;
            this.rate.Width = 110;
            // 
            // Select
            // 
            this.Select.HeaderText = "Select";
            this.Select.Name = "Select";
            this.Select.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Select.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(699, 478);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(96, 33);
            this.button1.TabIndex = 25;
            this.button1.Text = "Submit";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // frm_orderdetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(809, 521);
            this.ControlBox = false;
            this.Controls.Add(this.button1);
            this.Controls.Add(this.ansGridView5);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.KeyPreview = true;
            this.Name = "frm_orderdetails";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.frm_orderdetails_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frm_orderdetails_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.ansGridView5)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private faspiGrid.ansGridView ansGridView5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Orderno;
        private System.Windows.Forms.DataGridViewTextBoxColumn Vi_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn vdate;
        private System.Windows.Forms.DataGridViewTextBoxColumn itemsr;
        private System.Windows.Forms.DataGridViewTextBoxColumn description;
        private System.Windows.Forms.DataGridViewTextBoxColumn Pack;
        private System.Windows.Forms.DataGridViewTextBoxColumn qty;
        private System.Windows.Forms.DataGridViewTextBoxColumn rate;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Select;
        private System.Windows.Forms.Button button1;
    }
}