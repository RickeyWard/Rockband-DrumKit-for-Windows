using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Media;
using System.ComponentModel;


namespace Rockband_Drum_Kit
{
    class SoundEngine
    {
        public SoundEngine()
        {

        }

        private Dictionary<string, SimplePlaySound> Sounds = new Dictionary<string,SimplePlaySound>();

        public void AddSound(SimplePlaySound SoundObject, string NameOfSound)
        {
            Sounds.Add(NameOfSound, SoundObject);
        }

        public void AddSound(string SoundPath, bool buffer, string NameOfSound)
        {
            Sounds.Add(NameOfSound , new SimplePlaySound(SoundPath, buffer));
        }

        public void PlaySound(string SoundName)
        {
            Sounds[SoundName].Play();
        }

        /// <summary>
        /// Plays a stream Asychronously using the System.Media.SoundPlayer Object.
        /// </summary>
        /// <param name="S">Any stream, ex: a wav sound resource.</param>
        public void QuickPlay(System.IO.Stream S)
        {
            SoundPlayer wavPlayer = new SoundPlayer();
            wavPlayer.Stream = S;
            wavPlayer.LoadCompleted += new AsyncCompletedEventHandler(wavPlayer_LoadCompleted);
            wavPlayer.LoadAsync();
        }
        private void wavPlayer_LoadCompleted(object sender, AsyncCompletedEventArgs e)
        {
            ((System.Media.SoundPlayer)sender).Play();
        }


    }
}
