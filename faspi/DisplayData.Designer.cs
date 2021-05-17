namespace faspi
{
    partial class DisplayData
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
            ((System.ComponentModel.ISupportInitialize)(this.ansGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // ansGridView1
            // 
            this.ansGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ansGridView1.Location = new System.Drawing.Point(2, 2);
            this.ansGridView1.MultiSelect = false;
            this.ansGridView1.Name = "ansGridView1";
            this.ansGridView1.Size = new System.Drawing.Size(525, 261);
            this.ansGridView1.TabIndex = 1;
            this.ansGridView1.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.ansGridView1_CellEndEdit);
            // 
            // DisplayData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(527, 264);
            this.ControlBox = false;
            this.Controls.Add(this.ansGridView1);
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DisplayData";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.DisplayData_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.ansGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private faspiGrid.ansGridView ansGridView1;
    }
}