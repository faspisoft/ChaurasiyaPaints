namespace faspi
{
    partial class frm_gstindet
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
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.textBox9 = new System.Windows.Forms.TextBox();
            this.lblResponce = new System.Windows.Forms.Label();
            this.lblAPIBalannce = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.textBox9);
            this.groupBox5.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox5.Location = new System.Drawing.Point(23, 53);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(403, 61);
            this.groupBox5.TabIndex = 21;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "GSTIN";
            // 
            // textBox9
            // 
            this.textBox9.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox9.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.textBox9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox9.Location = new System.Drawing.Point(7, 26);
            this.textBox9.MaxLength = 50;
            this.textBox9.Name = "textBox9";
            this.textBox9.Size = new System.Drawing.Size(381, 22);
            this.textBox9.TabIndex = 21;
            // 
            // lblResponce
            // 
            this.lblResponce.AutoSize = true;
            this.lblResponce.ForeColor = System.Drawing.Color.Red;
            this.lblResponce.Location = new System.Drawing.Point(25, 129);
            this.lblResponce.Name = "lblResponce";
            this.lblResponce.Size = new System.Drawing.Size(10, 13);
            this.lblResponce.TabIndex = 23;
            this.lblResponce.Text = ".";
            // 
            // lblAPIBalannce
            // 
            this.lblAPIBalannce.AutoSize = true;
            this.lblAPIBalannce.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAPIBalannce.ForeColor = System.Drawing.Color.Red;
            this.lblAPIBalannce.Location = new System.Drawing.Point(24, 20);
            this.lblAPIBalannce.Name = "lblAPIBalannce";
            this.lblAPIBalannce.Size = new System.Drawing.Size(13, 20);
            this.lblAPIBalannce.TabIndex = 24;
            this.lblAPIBalannce.Text = ".";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(320, 165);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(142, 29);
            this.button2.TabIndex = 25;
            this.button2.Text = "Close (Esc)";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(172, 165);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(142, 29);
            this.button3.TabIndex = 26;
            this.button3.Text = "Get GSTIN Details";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // frm_gstindet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(470, 244);
            this.ControlBox = false;
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.lblAPIBalannce);
            this.Controls.Add(this.lblResponce);
            this.Controls.Add(this.groupBox5);
            this.KeyPreview = true;
            this.Name = "frm_gstindet";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.frm_gstindet_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frm_gstindet_KeyDown);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.TextBox textBox9;
        private System.Windows.Forms.Label lblResponce;
        private System.Windows.Forms.Label lblAPIBalannce;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
    }
}