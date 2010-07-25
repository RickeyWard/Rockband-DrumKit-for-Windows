using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Rockband_Drum_Kit
{
    public partial class DrumPad : UserControl
    {
        public DrumPad()
        {

            _PadColor = Color.Red;

            InitializeComponent();




            this.Paint += new PaintEventHandler(DrumPad_Paint);

            this.MouseDown += new MouseEventHandler(DrumPad_MouseDown);

        }

        void DrumPad_MouseDown(object sender, MouseEventArgs e)
        {
            HitAni();
        }

        Rectangle BRect;
        Rectangle PRect;

        bool hit = false;

        void DrumPad_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            e.Graphics.FillEllipse(Brushes.DarkGray, PRect);

            if (hit)
            {
                e.Graphics.FillPath(pathGradient, path);
                TimerHit.Start();
            }
         
            using (Pen P = new Pen(new SolidBrush(PadColor), 5F))
            {
                e.Graphics.DrawEllipse(P, PRect);
            }

            using (Pen BorderPen = new Pen(Brushes.Black, 4F))
            {
                e.Graphics.DrawEllipse(BorderPen, BRect);
            }

        }

        GraphicsPath path;
        PathGradientBrush pathGradient;
        

        void CreateGradient()
        {
            path = new GraphicsPath();

            path.AddEllipse(PRect);

            pathGradient = new PathGradientBrush(path);

            pathGradient.CenterColor = PadColor;

            Color[] surroundingColors = { System.Drawing.Color.Transparent };

            pathGradient.SurroundColors = surroundingColors;
        }

        private Color _PadColor;

        public Color PadColor
        {
            get
            {
                return _PadColor;
            }
            set
            {
                _PadColor = value;
                if (loaded)
                {
                    CreateGradient();
                }
            }
                
        }

        bool loaded = false;

        private void DrumPad_Load(object sender, EventArgs e)
        {
            BRect = new Rectangle(5, 5, this.ClientRectangle.Width - 11, this.ClientRectangle.Height - 11);
            PRect = new Rectangle(7, 7, this.ClientRectangle.Width - 15, this.ClientRectangle.Height - 15);

            loaded = true;

            CreateGradient();
        }

        private void TimerHit_Tick(object sender, EventArgs e)
        {
            hit = false;
            TimerHit.Stop();
            this.Invalidate();

        }

        public void HitAni()
        {
            hit = true;
            this.Invalidate();
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
