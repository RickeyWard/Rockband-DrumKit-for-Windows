using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Rockband_Drum_Kit
{
    public class DrumPadEvent
    {
        JoystickInterface.Joystick Stick;
        bool[] ButtonReady;
        Multimedia.Timer mmTimer = new Multimedia.Timer();
        
        public delegate void DrumPadHitEvent(object sender, DrumPadButtonEventArgs e);
        public event DrumPadHitEvent onDrumHit;

        public void DoDrumHit(int ButtonHit)
        {
            if (onDrumHit != null)
            {
                onDrumHit(this, new DrumPadButtonEventArgs(ButtonHit));
            }
        }


        public DrumPadEvent(Form parent)
        {
            Stick = new JoystickInterface.Joystick(parent.Handle);
            string[] st = Stick.FindJoysticks();
            Stick.AcquireJoystick(st[0]);

            ButtonReady = new bool[Stick.Buttons.Length];

            mmTimer.Period = 10;
            mmTimer.Mode = Multimedia.TimerMode.Periodic;
            mmTimer.Tick += new EventHandler(CheckTimer_Tick);
            mmTimer.Start();



            for (int i = 0; i < ButtonReady.Length; i++)
            {
                ButtonReady[i] = false;
            }
        }

  

        void CheckTimer_Tick(object sender, EventArgs e)
        {
            Stick.UpdateStatus();

            for (int i = 0; i < Stick.Buttons.Length; i++)
            {
                if (Stick.Buttons[i])
                {
                    if (ButtonReady[i])
                    {
                        DoDrumHit(i);
                        ButtonReady[i] = false;
                    }

                }
                else
                {
                    ButtonReady[i] = true;
                }

            }

         }
    }

    public class DrumPadButtonEventArgs : EventArgs
    {
        
        private int _Button;

        public int Button
        {
            get
            {
                return _Button;
            }
            set
            {
                _Button = value;
            }
        }

        public DrumPadButtonEventArgs(int button)
        {
            _Button = button;
        }


    }
}
