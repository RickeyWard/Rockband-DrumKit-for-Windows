using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Media;
using IrrKlang;
using System.IO;
using System.Runtime.InteropServices;
using Toub.Sound.Midi;

namespace Rockband_Drum_Kit
{
    public partial class Form1 : Form
    {
        SplashScreen.SplashScreen mSplash;

        ISoundEngine engine = new ISoundEngine();

        //Allows us to notify the form that a joystick or other HID device has been plugged into the system
        DetectHIDDeviceConnect.JoystickConnectNotification JoyConnectNotify;
        
        //Trademark text copy//™

        string startpath;

        public Form1(SplashScreen.SplashScreen splash)
        {
            mSplash = splash;

            //Set up auto detect of USB device plugged in.
            JoyConnectNotify = new DetectHIDDeviceConnect.JoystickConnectNotification(this);
            JoyConnectNotify.OnJoystickConnected += new DetectHIDDeviceConnect.JoystickConnectNotification.JoyConnectDel(JoyConnectNotify_OnJoystickConnected);

            InitializeComponent();

            this.BackColor = Color.FromArgb(137, 138, 139);

            rockBandDrumControler1.DrumPadClicked += new RockBandDrumControler.DrumPadClickedEventHandler(DrumKit_DrumPadHit);

            mSplash.SetProgress("Enabling Midi Device", 0.4);
            //start midi engine
            MidiPlayer.OpenMidi();
           
        }

        void JoyConnectNotify_OnJoystickConnected(DetectHIDDeviceConnect.JoyConnectedArgs ConArgs)
        {
            if (ConArgs.ConnectType == DetectHIDDeviceConnect.JoystickConnectNotification.JoyConEventType.Connected)
            {
                getDrums_Click(this, EventArgs.Empty);
            }
        }


        private string GetAssemblyPath()
        {
            //System.Reflection.Assembly exe = System.Reflection.Assembly.GetEntryAssembly();

            //return System.IO.Path.GetDirectoryName(exe.Location);

            return Application.StartupPath;
        }


        private void Form1_Load(object sender, EventArgs e)
        {

            startpath = GetAssemblyPath();

            //load

        //PKick = startpath + @"\Basic_Drums\CB_Kick.wav";
        //PSnare = startpath + @"\Basic_Drums\CB_Snare.wav";
        //PHat = startpath + @"\Basic_Drums\CB_Hat.wav";
        //PClap = startpath + @"\Basic_Drums\CB_Clap.wav";

        //    addsource(engine, PKick, "kick");
        //    addsource(engine, PSnare, "snare");
        //    addsource(engine, PHat, "hat");
        //    addsource(engine, PClap, "clap");

            mSplash.SetProgress("Loading Drum Sounds", 0.6);

            loadSounds(startpath + @"\Basic_Drums\");

            setcmbsource(sounds);

            engine.SoundVolume = .5f;

            //set default values
            cmbRedPad.SelectedIndex = 3;
            cmbYellowPad.SelectedIndex = 3;
            cmbBluePad.SelectedIndex = 1;
            cmbGreenPad.SelectedIndex = 0;
            cmbOrangePedal.SelectedIndex = 2;

            cmb_MidiPercusions.DataSource = Enum.GetValues(typeof(GeneralMidiPercussion));

            mSplash.SetProgress("Done Loading", 1.0);

            //add a little delay so the done loading and full progress bar gets shown to the users.
            System.Threading.Thread.Sleep(500);

            if (mSplash != null)
                mSplash.Hide();
        }

        private void setcmbsource(List<string> s)
        {
            foreach (Control c in this.Controls)
            {
                if (c.GetType() == typeof(ComboBox))
                {
                    ComboBox cm = (ComboBox)c;
                    cm.Items.Clear();
                    foreach (string item in s)
                    {
                        cm.Items.Add(item);
                    }
                }
            }
        }

        List<string> sounds = new List<string>();

        private void addsource(ISoundEngine e, string path, string name)
        {

            using (FileStream s = new FileStream(path, FileMode.Open))
            {
                byte[] SBuffer = new byte[s.Length];
                s.Read(SBuffer, 0, (int)s.Length);
               // ISoundSource iss =
                e.AddSoundSourceFromMemory(SBuffer, name);
               // sourceKick = e.AddSoundSourceFromMemory(SBuffer, name);
               // so = e.AddSoundSourceFromMemory(SBuffer, name);
                sounds.Add(name);
            }
     
        }

        private void loadSounds(string sourceDir)
        {
            // Process the list of files found in all directory.

            string[] fileEntries = Directory.GetFiles(sourceDir, "*.wav", SearchOption.AllDirectories);

            ConfirmWav wavcheck = new ConfirmWav();

            foreach (string fileName in fileEntries)
            {

                if (wavcheck.isWave(fileName)) //Verify our *.wav files are really wavs to prevent errors.
                {
                    FileInfo f = new FileInfo(fileName);
                    addsource(engine, fileName, f.Name.Substring(0, f.Name.Length - 4));
                }

                

            }
        }


        PS3RockbandControlerEvents DrumKit;

        private void getDrums_Click(object sender, EventArgs e)
        {
            DrumKit = new PS3RockbandControlerEvents(this, false, rockBandDrumControler1);
            DrumKit.DrumPadHit += new PS3RockbandControlerEvents.DrumPadHitEventHandler(DrumKit_DrumPadHit);
            DrumKit.GetRockBandDrumKit();
            lblDrumKit.Text = DrumKit.DrumPadName;
        }

        private volatile string rpad = "CB_Snare";
        private volatile string ypad = "CB_Snare";
        private volatile string bpad = "CB_Hat";
        private volatile string gpad = "CB_Clap";
        private volatile string opad = "CB_Kick";

        public delegate void OnDrumControlerEventDelegate(object sender, DrumEventArgs e);

        void DrumKit_DrumPadHit(object sender, DrumEventArgs e)
        {
            //if (this.InvokeRequired)
            //{
            //    Just in case we want to play with the UI
            //    this.Invoke(new OnDrumControlerEventDelegate(DrumKit_DrumPadHit), sender, e);
            //}
            //else
            //{

                switch (e.HitPad)
                {
                    case PS3RockbandControlerEvents.DrumPadTypes.RedPad:
                       // engine.Play2D(CheckDrum(1));
                        // MidiPlayer.Play(new NoteOn(0, GeneralMidiPercussion.SideStick, 127));
                       // engine.Play2D(cmbRedPad.SelectedItem.ToString());
                        engine.Play2D(rpad);
                        //   rockBandDrumControler1.RedAni();
                        break;
                    case PS3RockbandControlerEvents.DrumPadTypes.YellowPad:
                        engine.Play2D(ypad);
                        //engine.Play2D(cmbYellowPad.SelectedItem.ToString());
                        //   rockBandDrumControler1.YellowAni();
                        break;
                    case PS3RockbandControlerEvents.DrumPadTypes.BluePad:
                        engine.Play2D(bpad);    
                    //engine.Play2D(cmbBluePad.SelectedItem.ToString());
                        //   rockBandDrumControler1.BlueAni();
                        break;
                    case PS3RockbandControlerEvents.DrumPadTypes.GreenPad:
                        engine.Play2D(gpad);
                    //engine.Play2D(cmbGreenPad.SelectedItem.ToString());
                        //  rockBandDrumControler1.GreenAni();
                        break;
                    case PS3RockbandControlerEvents.DrumPadTypes.OrangePedal:
                        engine.Play2D(opad);
                    //engine.Play2D(cmbOrangePedal.SelectedItem.ToString());
                        //  rockBandDrumControler1.OrangeAni();
                        break;
                    default:
                        break;
               // }
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(Rockband_Drum_Kit.Properties.Resources.TitleImage, new Point(0, 0));
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            MidiPlayer.CloseMidi();
            base.OnFormClosed(e);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MidiPlayer.Play(new NoteOn(0, (GeneralMidiPercussion)cmb_MidiPercusions.SelectedItem, 127));
        }

        //private void button2_Click(object sender, EventArgs e)
        //{
        //    DrumPadEvent dp = new DrumPadEvent(this);
        //    dp.onDrumHit += new DrumPadEvent.DrumPadHitEvent(ondrumhitevent);
            
            
        //}

        //public delegate void OnDrumHitDelegate(object sender, DrumPadButtonEventArgs e);

        //private void ondrumhitevent(object sender, DrumPadButtonEventArgs e)
        //{
        //    if (this.InvokeRequired)
        //    {
        //        //Just in case we want to play with the UI
        //        this.Invoke(new OnDrumHitDelegate(ondrumhitevent), sender, e);
        //    }
        //    else
        //    {

        //        labelpadhit.Text = e.Button.ToString();

        //    }

        //    //if (this.InvokeRequired)
        //    //{
        //    //    this.Invoke(new MethodInvoker(delegate()
        //    //    {
        //    //        ondrumhitevent(sender, e);
        //    //    }));
        //    //}
        //    //else
        //    //{
        //    //    button.Text = e.Button.ToString();
        //    //}


        //}

        //private void button3_Click(object sender, EventArgs e)
        //{
        //    //System.Threading.Thread.Sleep(2000);
        //    //MessageBox.Show("2 Sec Pause Over");
        //    RBMessageBox md = new RBMessageBox();
        //    md.Text = "Cool Dialog Box";
        //    md.Message = "This is an interesting message box. It contains information about some errors and stuff. OH, and i wanna kill the zombies by shooting them in the head!";
        // // md.StartPosition = FormStartPosition.CenterParent;
        //    md.Show();
        //}

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }

        private void cmbRedPad_SelectedIndexChanged(object sender, EventArgs e)
        {
            rpad = cmbRedPad.SelectedItem.ToString();
        }

        private void cmbYellowPad_SelectedIndexChanged(object sender, EventArgs e)
        {
            ypad = cmbYellowPad.SelectedItem.ToString();
        }

        private void cmbBluePad_SelectedIndexChanged(object sender, EventArgs e)
        {
            bpad = cmbBluePad.SelectedItem.ToString();
        }

        private void cmbGreenPad_SelectedIndexChanged(object sender, EventArgs e)
        {
            gpad = cmbGreenPad.SelectedItem.ToString();
        }

        private void cmbOrangePedal_SelectionChangeCommitted(object sender, EventArgs e)
        {
            opad = cmbOrangePedal.SelectedItem.ToString();
        }





  


    }
   
}
