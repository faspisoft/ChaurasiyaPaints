namespace faspi
{
    partial class frmEditGroup
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
            this.ansGridView1 = new faspiGrid.ansGridView(this.components);
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.sno = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.packnm = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.ansGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // ansGridView1
            // 
            this.ansGridView1.AllowUserToAddRows = false;
            this.ansGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ansGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.ansGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.ansGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ansGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.sno,
            this.packnm});
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.ansGridView1.DefaultCellStyle = dataGridViewCellStyle4;
            this.ansGridView1.Location = new System.Drawing.Point(12, 82);
            this.ansGridView1.MultiSelect = false;
            this.ansGridView1.Name = "ansGridView1";
            this.ansGridView1.RowHeadersVisible = false;
            this.ansGridView1.Size = new System.Drawing.Size(820, 577);
            this.ansGridView1.TabIndex = 35;
            this.ansGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.ansGridView1_CellContentClick);
            this.ansGridView1.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.ansGridView1_CellEndEdit);
            this.ansGridView1.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.ansGridView1_CellEnter);
            this.ansGridView1.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.ansGridView1_DataError);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.Location = new System.Drawing.Point(861, 11);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(157, 728);
            this.flowLayoutPanel1.TabIndex = 37;
            // 
            // sno
            // 
            this.sno.FillWeight = 40F;
            this.sno.HeaderText = "S.No.";
            this.sno.Name = "sno";
            this.sno.ReadOnly = true;
            this.sno.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.sno.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // packnm
            // 
            this.packnm.HeaderText = "Packing";
            this.packnm.Name = "packnm";
            // 
            // frmEditGroup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1018, 740);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.ansGridView1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmEditGroup";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Edit Group";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmEditGroup_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmEditGroup_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.ansGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private faspiGrid.ansGridView ansGridView1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.DataGridViewTextBoxColumn sno;
        private System.Windows.Forms.DataGridViewTextBoxColumn packnm;
       

    }
}