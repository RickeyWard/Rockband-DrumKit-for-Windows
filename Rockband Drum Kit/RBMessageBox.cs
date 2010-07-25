using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Rockband_Drum_Kit
{
    public partial class RBMessageBox : MessageDialog
    {
        public RBMessageBox()
        {
            InitializeComponent();
            this.Paint += new PaintEventHandler(RBMessageBox_Paint);
        }

        void RBMessageBox_Paint(object sender, PaintEventArgs e)
        {
            //create a rectange for the string to live in, allow 5px padding around text
            System.Drawing.Rectangle rect = new System.Drawing.Rectangle(14, 30, this.Width - 18, this.Height - 60);
            //draw string to form's background using the graphics.drawstring method.
            System.Drawing.StringFormat format = new System.Drawing.StringFormat();
            format.Alignment = System.Drawing.StringAlignment.Near;
            format.LineAlignment = System.Drawing.StringAlignment.Center;
            e.Graphics.DrawString(privateVar, new Font("Arial", 14F), System.Drawing.Brushes.Gray, rect, format);
        }


        private string privateVar = "This is a custom message";

        public string Message
        {
            get
            {
                return privateVar;
            }
            set
            {
                privateVar = value;
            }
        }

        private void OKBTn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
