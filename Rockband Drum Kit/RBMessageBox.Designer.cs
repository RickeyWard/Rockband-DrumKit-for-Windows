namespace Rockband_Drum_Kit
{
    partial class RBMessageBox
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
            this.OKBTn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // OKBTn
            // 
            this.OKBTn.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OKBTn.Location = new System.Drawing.Point(313, 251);
            this.OKBTn.Name = "OKBTn";
            this.OKBTn.Size = new System.Drawing.Size(75, 23);
            this.OKBTn.TabIndex = 1;
            this.OKBTn.Text = "OK";
            this.OKBTn.UseVisualStyleBackColor = true;
            this.OKBTn.Click += new System.EventHandler(this.OKBTn_Click);
            // 
            // RBMessageBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 300);
            this.Controls.Add(this.OKBTn);
            this.Name = "RBMessageBox";
            this.Text = "Message";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button OKBTn;
    }
}