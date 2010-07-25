using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace Rockband_Drum_Kit
{
    public class transTrackBar : System.Windows.Forms.TrackBar
    {
        public transTrackBar()
        {

        }


        protected override CreateParams CreateParams
        {

            get
            {

                CreateParams cp = base.CreateParams;

                cp.ExStyle |= 0x20;

                return cp;

            }

        }

        override protected void OnPaintBackground(PaintEventArgs e)
        {
            // do nothing

        }

    }
}
