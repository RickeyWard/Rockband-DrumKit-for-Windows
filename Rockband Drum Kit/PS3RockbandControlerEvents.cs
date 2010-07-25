using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JoystickInterface;
using System.Windows.Forms;


namespace Rockband_Drum_Kit
{
    public class PS3RockbandControlerEvents
    {

        Joystick jst;

       public enum DrumPadTypes
        {
            RedPad,
            YellowPad,
            BluePad,
            GreenPad,
            OrangePedal
        }

        public delegate void DrumPadHitEventHandler(object sender, DrumEventArgs e);
        public event DrumPadHitEventHandler DrumPadHit;


        private string Padtitle;

        public string DrumPadName
        {
            get
            {
                return Padtitle;
            }
            set
            {
                Padtitle = value;
            }
        }

        //Timer TimerDRUMS = new Timer();
        Multimedia.Timer TimerDRUMS = new Multimedia.Timer();
        const int DefaultDrumInterval = 12;


        private int _timerVal;

        public int Sensitivity
        {
            get
            {
                return _timerVal;
            }
            set
            {
                _timerVal = value;
                //TimerDRUMS.Interval = value;
                TimerDRUMS.Period = value;
            }
        }

        RockBandDrumControler DrumsSetControler;

        public PS3RockbandControlerEvents(Form Parent, bool GetPad, RockBandDrumControler RBDC)
        {
            jst = new Joystick(Parent.Handle);

            //TimerDRUMS.Interval = DefaultDrumInterval;
            TimerDRUMS.Period = DefaultDrumInterval;
            TimerDRUMS.Mode = Multimedia.TimerMode.Periodic;

            TimerDRUMS.Tick += new EventHandler(TimerDRUMS_Tick);

            if (GetPad)
            {
                GetRockBandDrumKit();
            }

            if (RBDC != null)
            {
                DrumsSetControler = RBDC;
            }

        }

        public PS3RockbandControlerEvents(Form Parent)
        {
            jst = new Joystick(Parent.Handle);

           // TimerDRUMS.Interval = DefaultDrumInterval;
            TimerDRUMS.Period = DefaultDrumInterval;
            TimerDRUMS.Mode = Multimedia.TimerMode.Periodic;
            TimerDRUMS.Tick += new EventHandler(TimerDRUMS_Tick);

        }

        public void GetRockBandDrumKit()
        {
            try
            {
                string[] sticks = jst.FindJoysticks();
                bool found = false;
                foreach (string s in sticks)
                {
                    if (s == "Harmonix Drum Kit for PlayStation(R)3")
                    {
                        found = true;
                    }
                }
                if (found)
                {
                    if (jst.AcquireJoystick("Harmonix Drum Kit for PlayStation(R)3"))
                    {
                        DrumPadName = "Harmonix Drum Kit for PlayStation(R)3";
                        TimerDRUMS.Start();
                    }
                    else
                    {
                        MessageBox.Show("PS3 Drums kit not found");
                    }
                }
                else
                {
                    MessageBox.Show("PS3 Drums kit not found");
                }
            }
            catch
            {
                MessageBox.Show("Could not find a RockBand DrumKit\n Try reconnecting the DrumKit and restarting the Application");
                DrumPadName = "No DrumKit Connected";
            }
        }

        bool ready2 = false;
        bool ready3 = false;
        bool ready0 = false;
        bool ready1 = false;
        bool ready4 = false;

        void TimerDRUMS_Tick(object sender, EventArgs e)
        {
            jst.UpdateStatus();
            //check FL
            if (jst.Buttons[2])
            {
                if (ready2)
                {
                    if (DrumPadHit != null)
                    {
                        DrumPadHit(this, new DrumEventArgs(DrumPadTypes.RedPad));
                    }
                    if (DrumsSetControler != null)
                    {
                        DrumsSetControler.RedAni();
                    }

                    ready2 = false;

                }

            }
            if (!jst.Buttons[2])
            {
                ready2 = true;
            }
            //check L
            if (jst.Buttons[3])
            {
                if (ready3)
                {
                    if (DrumPadHit != null)
                    {
                        DrumPadHit(this, new DrumEventArgs(DrumPadTypes.YellowPad));
                    }
                    if (DrumsSetControler != null)
                    {
                        DrumsSetControler.YellowAni();
                    }

                    ready3 = false;
                }

            }
            if (!jst.Buttons[3])
            {
                ready3 = true;
            }
            //check R
            if (jst.Buttons[0])
            {
                if (ready0)
                {

                    if (DrumPadHit != null)
                    {
                        DrumPadHit(this, new DrumEventArgs(DrumPadTypes.BluePad));
                    }
                    if (DrumsSetControler != null)
                    {
                        DrumsSetControler.BlueAni();
                    }

                    ready0 = false;

                }

            }
            if (!jst.Buttons[0])
            {
                ready0 = true;
            }
            //check FR
            if (jst.Buttons[1])
            {
                if (ready1)
                {
                    if (DrumPadHit != null)
                    {
                        DrumPadHit(this, new DrumEventArgs(DrumPadTypes.GreenPad));
                    }
                    if (DrumsSetControler != null)
                    {
                        DrumsSetControler.GreenAni();
                    }

                    ready1 = false;

                }

            }
            if (!jst.Buttons[1])
            {
                ready1 = true;
            }
            //check Pedal
            if (jst.Buttons[4])
            {
                if (ready4)
                {
                    if (DrumPadHit != null)
                    {
                        DrumPadHit(this, new DrumEventArgs(DrumPadTypes.OrangePedal));
                    }
                    if (DrumsSetControler != null)
                    {
                        DrumsSetControler.OrangeAni();
                    }

                    ready4 = false;

                }

            }
            if (!jst.Buttons[4])
            {
                ready4 = true;
            }
        }

        public void DefaultSensitivity()
        {
            //TimerDRUMS.Interval = DefaultDrumInterval;
            TimerDRUMS.Period = DefaultDrumInterval;
        }

    }

    public class DrumEventArgs : EventArgs
    {
        public PS3RockbandControlerEvents.DrumPadTypes HitPad { get; set; }

        public DrumEventArgs(PS3RockbandControlerEvents.DrumPadTypes PadHit)
        {
            HitPad = PadHit;
        }

    }
}
