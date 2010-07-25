using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Rockband_Drum_Kit
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            SplashScreen.SplashScreen splash = new SplashScreen.SplashScreen();
            splash.Show();
            splash.SetProgress("Initializing...", 0.0);
            Application.Run(new Form1(splash));
        }
    }
}
