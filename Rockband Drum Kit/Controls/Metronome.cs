using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.IO;
using Toub.Sound.Midi;
using System.Threading;


namespace Rockband_Drum_Kit.Controls
{
    public class Metronome : Control, IDisposable
    {
        IrrKlang.ISoundEngine engine;
        Multimedia.Timer timer = new Multimedia.Timer();
        BasicHorizontalSlider slider = new BasicHorizontalSlider();

        NumericUpDown textbpm;
        //1000 / (bpm / 60)

      //  Thread loopthread;

        public int BPM
        {
            get
            {
                return _BPM;
            }

            set
            {
                _BPM = value;

                timer.Period = (int)1000 / (value / 60);
                

            }
        }

        int _BPM = 120;

      //  string path;

        public Metronome()
        {
            this.SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer |
            ControlStyles.AllPaintingInWmPaint | ControlStyles.SupportsTransparentBackColor,
            true);

          //  path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
          //  path = Application.StartupPath;

            this.BackColor = Color.Transparent;

            this.Width = 200;
            this.Height = 50;

            this.DoubleBuffered = true;

            slider.Location = new Point(40, 20);
            slider.Width = 110;
            slider.Height = 25;
            slider.Minimum = 60;
            slider.Maximum = 220;
            slider.Value = 120;
            slider.ValueChanged += new EventHandler(slider_ValueChanged);
            this.Controls.Add(slider);

            textbpm = new NumericUpDown();
            textbpm.BorderStyle = BorderStyle.FixedSingle;
            textbpm.BackColor = Color.FromArgb(137, 138, 139);
            textbpm.Maximum = 220;
            textbpm.Minimum = 60;

            textbpm.Width = 40;
            textbpm.Location = new Point(153, 20);
            textbpm.Value = BPM;
            textbpm.ValueChanged += new EventHandler(textbpm_ValueChanged);
            this.Controls.Add(textbpm);

            engine = new IrrKlang.ISoundEngine();

            if(File.Exists(Application.StartupPath + @"\Sounds\metronome.wav"))
                addsource(engine, Application.StartupPath + @"\Sounds\metronome.wav", "Tick");
            
            //addsource(engine, @"C:\Users\Rickey\Documents\Visual Studio 2008\Projects\Rockband Drum Kit\Rockband Drum Kit\bin\Release\Sounds\metronome.wav", "Tick");
            
          
            engine.SoundVolume = 2F;         


            this.Click += new EventHandler(Metronome_Click);
            this.MouseEnter += new EventHandler(Metronome_MouseEnter);
            this.MouseLeave += new EventHandler(Metronome_MouseLeave);

          //  loopthread = new Thread(new ThreadStart(SetUpThread));
            //loopthread.Start();
            SetUpThread();
        }


        void SetUpThread()
        {
            //timer = new Multimedia.Timer();
            timer.Period = 500;
            timer.Mode = Multimedia.TimerMode.Periodic;
            timer.Tick += new EventHandler(timer_Tick);

            //Application.Run();

        }


        void Metronome_MouseLeave(object sender, EventArgs e)
        {
            if (!this.ClientRectangle.Contains(PointToClient(Cursor.Position)))
            {
                mousedOver = false;
                this.Invalidate();
            }
        }

        void Metronome_MouseEnter(object sender, EventArgs e)
        {
            mousedOver = true;
            this.Invalidate();
        }

        void textbpm_ValueChanged(object sender, EventArgs e)
        {
            slider.Value = (int)textbpm.Value;
        }


        void slider_ValueChanged(object sender, EventArgs e)
        {
            BPM = slider.Value;
            textbpm.Value = slider.Value;
            textbpm.Refresh();
            this.Invalidate();
        }

        void Metronome_Click(object sender, EventArgs e)
        {

                if (timer.IsRunning)
                {
                    timer.Stop();
                }
                else
                {
                    timer.Start();
                }
            
            this.Invalidate();
        }

        void timer_Tick(object sender, EventArgs e)
        {
           engine.Play2D("Tick");
           // MidiPlayer.Play(new NoteOn(0, GeneralMidiPercussion.SideStick, 127));
            //if (this.InvokeRequired)
            //{
            //    this.Invoke(new MethodInvoker(delegate()
            //    {
            //        tickmetronome();
            //    }));
            //}
            //else
            //{
            //    tickmetronome();
          //  }
        }

        void tickmetronome()
        {
            //engine.Play2D("Tick");
          //  MidiPlayer.Play(new NoteOn(0, GeneralMidiPercussion.Claves, 127));
            MidiPlayer.Play(new NoteOn(0, GeneralMidiPercussion.SideStick, 127));

        }

        public void Start()
        {

                timer.Start();
            
        }

        public void Stop()
        {

                timer.Stop();
            
        }


        private void addsource(IrrKlang.ISoundEngine e, string path, string name)
        {

            using (FileStream s = new FileStream(path, FileMode.Open))
            {
                byte[] SBuffer = new byte[s.Length];
                s.Read(SBuffer, 0, (int)s.Length);
                e.AddSoundSourceFromMemory(SBuffer, name);
            }

        }

        bool mousedOver = false;
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (mousedOver)
            {
                e.Graphics.DrawImage(Rockband_Drum_Kit.Properties.Resources.MetronomeBGMoused, new Point(0, 0));
            }
            else
            {
                e.Graphics.DrawImage(Rockband_Drum_Kit.Properties.Resources.MetronomeBG, new Point(0, 0));
            }
            //e.Graphics.DrawRectangle(Pens.Black, new Rectangle(1,1,ClientRectangle.Width -2, ClientRectangle.Height -2));
           // e.Graphics.DrawString(BPM.ToString(), new Font("Arial", 12), Brushes.Black, new PointF(162, 3));

            if (timer.IsRunning)
            {
                e.Graphics.DrawString("ON", new Font("Arial", 12), Brushes.Black, new PointF(85, 3));
            }
            else
            {
                e.Graphics.DrawString("OFF", new Font("Arial", 12), Brushes.Black, new PointF(85, 3));
            }

        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            //Kill Dispose of the timer, ending the interlop it uses freeing the thread.
            timer.Dispose();

            base.Dispose(disposing);
        }

    }
}
