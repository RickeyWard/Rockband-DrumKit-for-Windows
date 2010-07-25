using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Rockband_Drum_Kit
{
    public class RockBandDrumControler : Control
    {
        public RockBandDrumControler()
        {
            this.Width = 254;
            this.Height = 140;


            //Set up double buffering and a little extra.
            this.SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer |
            ControlStyles.AllPaintingInWmPaint | ControlStyles.SupportsTransparentBackColor,
            true);

            this.BackColor = Color.Transparent;

            //red Rects
            RectRed = new Rectangle(2, 35, 70, 70);

            RectRedB = new Rectangle(RectRed.X + 5, RectRed.Y + 5, RectRed.Width - 11, RectRed.Height - 11);
            RectRedP = new Rectangle(RectRed.X + 7, RectRed.Y + 7, RectRed.Width - 15, RectRed.Height - 15);

            //yellow Rects
            RectYellow = new Rectangle(60, 4, 70, 70);

            RectYellowB = new Rectangle(RectYellow.X + 5, RectYellow.Y + 5, RectYellow.Width - 11, RectYellow.Height - 11);
            RectYellowP = new Rectangle(RectYellow.X + 7, RectYellow.Y + 7, RectYellow.Width - 15, RectYellow.Height - 15);

            //Blue Rects
            RectBlue = new Rectangle(126, 4, 70, 70);

            RectBlueB = new Rectangle(RectBlue.X + 5, RectBlue.Y + 5, RectBlue.Width - 11, RectBlue.Height - 11);
            RectBlueP = new Rectangle(RectBlue.X + 7, RectBlue.Y + 7, RectBlue.Width - 15, RectBlue.Height - 15);

            //Green Rects
            RectGreen = new Rectangle(184, 35, 70, 70);

            RectGreenB = new Rectangle(RectGreen.X + 5, RectGreen.Y + 5, RectGreen.Width - 11, RectGreen.Height - 11);
            RectGreenP = new Rectangle(RectGreen.X + 7, RectGreen.Y + 7, RectGreen.Width - 15, RectGreen.Height - 15);

            //orange Rects
            RectOrange = new Rectangle(123, 64, 36, 70);

            //events
            this.Paint += new PaintEventHandler(RockBandDrumControler_Paint);

            this.MouseDown += new MouseEventHandler(RockBandDrumControler_MouseDown);

            //Create Gradient Brushes
            CreateGradient();

            RedTimer.Interval = 100;
            RedTimer.Tick += new EventHandler(RedTimer_Tick);

            BlueTimer.Interval = 100;
            BlueTimer.Tick += new EventHandler(BlueTimer_Tick);

            YellowTimer.Interval = 100;
            YellowTimer.Tick += new EventHandler(YellowTimer_Tick);

            GreenTimer.Interval = 100;
            GreenTimer.Tick += new EventHandler(GreenTimer_Tick);

            OrangeTimer.Interval = 100;
            OrangeTimer.Tick += new EventHandler(OrangeTimer_Tick);

        }

        void OrangeTimer_Tick(object sender, EventArgs e)
        {
            OrangeHit = false;
            OrangeTimer.Stop();
            Invalidate(RectOrange);
        }

        void GreenTimer_Tick(object sender, EventArgs e)
        {
            GreenHit = false;
            GreenTimer.Stop();
            Invalidate(RectGreen);
        }

        void YellowTimer_Tick(object sender, EventArgs e)
        {
            YellowHit = false;
            YellowTimer.Stop();
            Invalidate(RectYellow);
        }

        void BlueTimer_Tick(object sender, EventArgs e)
        {
            BlueHit = false;
            BlueTimer.Stop();
            Invalidate(RectBlue);
        }

        void RedTimer_Tick(object sender, EventArgs e)
        {
            RedHit = false;
            RedTimer.Stop();
            Invalidate(RectRed);
        }

        public delegate void DrumPadClickedEventHandler(object sender, DrumEventArgs e);
        public event DrumPadClickedEventHandler DrumPadClicked;

        void RockBandDrumControler_MouseDown(object sender, MouseEventArgs e)
        {
            if (RectRedP.Contains(e.Location))
            {
                RedHit = true;
                this.Invalidate(RectRed);
                RedTimer.Start();

                if (DrumPadClicked != null)
                {
                    DrumPadClicked(this, new DrumEventArgs(PS3RockbandControlerEvents.DrumPadTypes.RedPad));
                }
            }

            if (RectYellowP.Contains(e.Location))
            {
                YellowHit = true;
                this.Invalidate(RectYellow);
                YellowTimer.Start();

                if (DrumPadClicked != null)
                {
                    DrumPadClicked(this, new DrumEventArgs(PS3RockbandControlerEvents.DrumPadTypes.YellowPad));
                }
            }

            if (RectBlueP.Contains(e.Location))
            {
                BlueHit = true;
                this.Invalidate(RectBlue);
                BlueTimer.Start();

                if (DrumPadClicked != null)
                {
                    DrumPadClicked(this, new DrumEventArgs(PS3RockbandControlerEvents.DrumPadTypes.BluePad));
                }
            }

            if (RectGreenP.Contains(e.Location))
            {
                GreenHit = true;
                this.Invalidate(RectGreen);
                GreenTimer.Start();
                
                if (DrumPadClicked != null)
                {
                    DrumPadClicked(this, new DrumEventArgs(PS3RockbandControlerEvents.DrumPadTypes.GreenPad));
                }
            }

            if (RectOrange.Contains(e.Location))
            {
                OrangeHit = true;
                this.Invalidate(RectOrange);
                OrangeTimer.Start();

                if(DrumPadClicked != null)
                {
                    DrumPadClicked(this, new DrumEventArgs(PS3RockbandControlerEvents.DrumPadTypes.OrangePedal));
                }
            }
        }

        Rectangle RectRed;
        Rectangle RectYellow;
        Rectangle RectBlue;
        Rectangle RectGreen;
        Rectangle RectOrange;

        Rectangle RectRedB;
        Rectangle RectRedP;

        Rectangle RectYellowB;
        Rectangle RectYellowP;

        Rectangle RectBlueB;
        Rectangle RectBlueP;

        Rectangle RectGreenB;
        Rectangle RectGreenP;

        bool RedHit = false;
        bool YellowHit = false;
        bool BlueHit = false;
        bool GreenHit = false;
        bool OrangeHit = false;

        Timer RedTimer = new Timer();
        Timer YellowTimer = new Timer();
        Timer BlueTimer = new Timer();
        Timer GreenTimer = new Timer();
        Timer OrangeTimer = new Timer();

        void RockBandDrumControler_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(Rockband_Drum_Kit.Properties.Resources.Drumset, new Point(0, 0));
            //e.Graphics.DrawRectangle(Pens.Red, RectRed);

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;


            //Draw Red Pad
            e.Graphics.FillEllipse(Brushes.DarkGray, RectRedP);

            if (RedHit)
            {
                e.Graphics.FillPath(RedpathGradient, Redpath);
               // TimerHit.Start();
            }

            using (Pen P = new Pen(Brushes.Red, 5F))
            {
                e.Graphics.DrawEllipse(P, RectRedP);
            }

            using (Pen BorderPen = new Pen(Brushes.Black, 4F))
            {
                e.Graphics.DrawEllipse(BorderPen, RectRedB);
            }

            //Draw Yellow Pad
            e.Graphics.FillEllipse(Brushes.DarkGray, RectYellowP);

            if (YellowHit)
            {
                e.Graphics.FillPath(YellowpathGradient, Yellowpath);
                // TimerHit.Start();
            }

            using (Pen P = new Pen(Brushes.Yellow, 5F))
            {
                e.Graphics.DrawEllipse(P, RectYellowP);
            }

            using (Pen BorderPen = new Pen(Brushes.Black, 4F))
            {
                e.Graphics.DrawEllipse(BorderPen, RectYellowB);
            }

            //Draw Blue Pad
            e.Graphics.FillEllipse(Brushes.DarkGray, RectBlueP);

            if (BlueHit)
            {
                e.Graphics.FillPath(BluepathGradient, Bluepath);
                // TimerHit.Start();
            }

            using (Pen P = new Pen(Brushes.RoyalBlue, 5F))
            {
                e.Graphics.DrawEllipse(P, RectBlueP);
            }

            using (Pen BorderPen = new Pen(Brushes.Black, 4F))
            {
                e.Graphics.DrawEllipse(BorderPen, RectBlueB);
            }

            //Draw Green Pad
            e.Graphics.FillEllipse(Brushes.DarkGray, RectGreenP);

            if (GreenHit)
            {
                e.Graphics.FillPath(GreenpathGradient, Greenpath);
                // TimerHit.Start();
            }

            using (Pen P = new Pen(Brushes.Green, 5F))
            {
                e.Graphics.DrawEllipse(P, RectGreenP);
            }

            using (Pen BorderPen = new Pen(Brushes.Black, 4F))
            {
                e.Graphics.DrawEllipse(BorderPen, RectGreenB);
            }

            //Draw Pedal

            GraphicsPath path = new GraphicsPath();

            Rectangle tl = new Rectangle(RectOrange.X + (RectOrange.Width / 4), RectOrange.Y + 5, RectOrange.Width / 2, 6);
            Rectangle m = new Rectangle(RectOrange.X + (RectOrange.Width / 6), RectOrange.Y + 10, RectOrange.Width - (RectOrange.Width / 6) * 2, 30);

            //topleftarch
            path.AddArc(tl, 180F, 180F);
            path.AddArc(m, 340F, 30F);
            path.AddLine(RectOrange.X + ((RectOrange.Width / 4) * 3) - 5, RectOrange.Y + (RectOrange.Height) - 5, RectOrange.X + (RectOrange.Width / 4 + 5), RectOrange.Y + RectOrange.Height - 5);
            path.AddArc(m, 170F, 30F);
            path.CloseAllFigures();

            e.Graphics.FillPath(new SolidBrush(Color.FromArgb(40, 40, 40)), path);
            e.Graphics.DrawPath(new Pen(Brushes.Black, 3F), path);

            if (OrangeHit)
            {
                e.Graphics.FillPath(OrangepathGradient, Orangepath);
                e.Graphics.FillPath(Brushes.DarkOrange, RoundedRectangle.Create(new Rectangle(RectOrange.X + (RectOrange.Width / 4), (m.Y + m.Height/3), RectOrange.Width / 2, 4), 2));
                //TimerHit.Start();
            }
            else
            {
                e.Graphics.FillPath(Brushes.Orange, RoundedRectangle.Create(new Rectangle(RectOrange.X + (RectOrange.Width / 4), (m.Y + m.Height/3) , RectOrange.Width / 2, 4), 2));
            }
        }


        GraphicsPath Redpath;
        PathGradientBrush RedpathGradient;

        GraphicsPath Yellowpath;
        PathGradientBrush YellowpathGradient;

        GraphicsPath Bluepath;
        PathGradientBrush BluepathGradient;

        GraphicsPath Greenpath;
        PathGradientBrush GreenpathGradient;

        GraphicsPath Orangepath;
        PathGradientBrush OrangepathGradient;


        void CreateGradient()
        {
            //red
            Redpath = new GraphicsPath();

            Redpath.AddEllipse(RectRedP);

            RedpathGradient = new PathGradientBrush(Redpath);

            RedpathGradient.CenterColor = Color.Red;

            Color[] surroundingColors = { System.Drawing.Color.Transparent };

            RedpathGradient.SurroundColors = surroundingColors;

            //yellow
            Yellowpath = new GraphicsPath();

            Yellowpath.AddEllipse(RectYellowP);

            YellowpathGradient = new PathGradientBrush(Yellowpath);

            YellowpathGradient.CenterColor = Color.Yellow;

            Color[] surroundingColorsY = { System.Drawing.Color.Transparent };

            YellowpathGradient.SurroundColors = surroundingColorsY;

            //Blue
            Bluepath = new GraphicsPath();

            Bluepath.AddEllipse(RectBlueP);

            BluepathGradient = new PathGradientBrush(Bluepath);

            BluepathGradient.CenterColor = Color.RoyalBlue;

            Color[] surroundingColorsB = { System.Drawing.Color.Transparent };

            BluepathGradient.SurroundColors = surroundingColorsB;

            //Green
            Greenpath = new GraphicsPath();

            Greenpath.AddEllipse(RectGreenP);

            GreenpathGradient = new PathGradientBrush(Greenpath);

            GreenpathGradient.CenterColor = Color.Green;

            Color[] surroundingColorsG = { System.Drawing.Color.Transparent };

            GreenpathGradient.SurroundColors = surroundingColorsG;

            //Orange
            Orangepath = new GraphicsPath();

            Orangepath.AddEllipse(new Rectangle(RectOrange.X + (RectOrange.Width / 6), RectOrange.Y + 8, RectOrange.Width - (RectOrange.Width / 6) * 2, 30));

            OrangepathGradient = new PathGradientBrush(Orangepath);

            OrangepathGradient.CenterColor = Color.Orange;

            Color[] surroundingColorsO = { System.Drawing.Color.Transparent };

            OrangepathGradient.SurroundColors = surroundingColorsO;

        }

        public void RedAni()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(delegate()
                {
                    RedAni();
                }));
            }
            else
            {
                RedHit = true;
                this.Invalidate(RectRed);
                RedTimer.Start();
            }
            
        }
        public void YellowAni()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(delegate()
                {
                    YellowAni();
                }));
            }
            else
            {
                YellowHit = true;
                this.Invalidate(RectYellow);
                YellowTimer.Start();
            }
        }
        public void BlueAni()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(delegate()
                {
                    BlueAni();
                }));
            }
            else
            {
                BlueHit = true;
                this.Invalidate(RectBlue);
                BlueTimer.Start();
            }
        }
        public void GreenAni()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(delegate()
                {
                    GreenAni();
                }));
            }
            else
            {
                GreenHit = true;
                this.Invalidate(RectGreen);
                GreenTimer.Start();
            }
        }
        public void OrangeAni()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(delegate()
                {
                    OrangeAni();
                }));
            }
            else
            {
                OrangeHit = true;
                this.Invalidate(RectOrange);
                OrangeTimer.Start();
            }
        }

    }
}
