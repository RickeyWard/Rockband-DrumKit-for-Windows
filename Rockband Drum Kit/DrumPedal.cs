using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace Rockband_Drum_Kit
{
    public partial class DrumPedal : UserControl
    {
        public DrumPedal()
        {
            InitializeComponent();

            this.Width = 36;
            this.Height = 70;

            this.Paint += new PaintEventHandler(DrumPedal_Paint);
            this.MouseDown += new MouseEventHandler(DrumPedal_MouseDown);
        }

        void DrumPedal_MouseDown(object sender, MouseEventArgs e)
        {
            HitAni();
        }

        void DrumPedal_Paint(object sender, PaintEventArgs e)
        {

            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;

            GraphicsPath path = new GraphicsPath();

            Rectangle tl = new Rectangle(this.ClientRectangle.Width/4, 5, this.ClientRectangle.Width /2, 6);
            Rectangle m = new Rectangle(this.ClientRectangle.Width/6, 10, this.ClientRectangle.Width - (this.ClientRectangle.Width /6)*2, 30);
                        
            //topleftarch
            path.AddArc(tl, 180F, 180F);
            path.AddArc(m, 340F, 30F);
            path.AddLine(((ClientRectangle.Width / 4) * 3) - 5, this.Height - 5, this.ClientRectangle.Width / 4 + 5, this.Height - 5);
            path.AddArc(m, 170F, 30F);
            path.CloseAllFigures();

            e.Graphics.FillPath(new SolidBrush(Color.FromArgb(40, 40, 40)), path);
            e.Graphics.DrawPath(new Pen(Brushes.Black, 3F), path);

            if (hit)
            {
                e.Graphics.FillPath(pathGradient, path);
                e.Graphics.FillPath(Brushes.DarkOrange, RoundedRectangle.Create(new Rectangle(this.ClientRectangle.Width / 4, (m.Y + m.Height) / 2, this.ClientRectangle.Width / 2, 4), 2));
                TimerHit.Start();
            }
            else
            {
                e.Graphics.FillPath(Brushes.Orange, RoundedRectangle.Create(new Rectangle(this.ClientRectangle.Width / 4, (m.Y + m.Height) / 2, this.ClientRectangle.Width / 2, 4), 2));
            }

        }

        GraphicsPath path;
        PathGradientBrush pathGradient;


        void CreateGradient()
        {
            path = new GraphicsPath();

            path.AddEllipse(new Rectangle(this.ClientRectangle.Width/6, 8, this.ClientRectangle.Width - (this.ClientRectangle.Width /6)*2, 30));

            pathGradient = new PathGradientBrush(path);

            pathGradient.CenterColor = Color.Orange;

            Color[] surroundingColors = { System.Drawing.Color.Transparent };

            pathGradient.SurroundColors = surroundingColors;
        }

        bool hit = false;

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

        private void DrumPedal_Load(object sender, EventArgs e)
        {
            CreateGradient();
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
