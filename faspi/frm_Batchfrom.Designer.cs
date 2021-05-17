namespace faspi
{
    partial class frm_Batchfrom
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
            this.ansGridView1 = new faspiGrid.ansGridView(this.components);
            this.batchno = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.select = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.button3 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.ansGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // ansGridView1
            // 
            this.ansGridView1.AllowUserToAddRows = false;
            this.ansGridView1.AllowUserToDeleteRows = false;
            this.ansGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ansGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.ansGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ansGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.batchno,
            this.select});
            this.ansGridView1.Location = new System.Drawing.Point(10, 11);
            this.ansGridView1.MultiSelect = false;
            this.ansGridView1.Name = "ansGridView1";
            this.ansGridView1.RowHeadersVisible = false;
            this.ansGridView1.Size = new System.Drawing.Size(294, 423);
            this.ansGridView1.TabIndex = 12;
            // 
            // batchno
            // 
            this.batchno.HeaderText = "Batch No";
            this.batchno.Name = "batchno";
            this.batchno.ReadOnly = true;
            // 
            // select
            // 
            this.select.HeaderText = "Select";
            this.select.Name = "select";
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.Blue;
            this.button3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button3.ForeColor = System.Drawing.Color.White;
            this.button3.Location = new System.Drawing.Point(209, 440);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(96, 37);
            this.button3.TabIndex = 18;
            this.button3.Text = "Submit";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // frm_Batchfrom
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ClientSize = new System.Drawing.Size(316, 489);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.ansGridView1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "frm_Batchfrom";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frm_Batchfrom";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frm_Batchfrom_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.ansGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private faspiGrid.ansGridView ansGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn batchno;
        private System.Windows.Forms.DataGridViewCheckBoxColumn select;
        private System.Windows.Forms.Button button3;

    }
}