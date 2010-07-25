using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;

namespace Rockband_Drum_Kit
{
    class SimplePlaySound
    {
        byte[] Sbuffer;
        //IntPtr SoundPoint;
        string path;

        bool _buffer;
       // GCHandle h;

        public SimplePlaySound(string fileName, bool buffer)
        {
            if (!System.IO.File.Exists(fileName))
            {
                throw new NotImplementedException("Error: File Name Incorrect");
            }

            _buffer = buffer;

            if (buffer)
            {
                using (FileStream s = new FileStream(fileName, FileMode.Open))
                {
                    Sbuffer = new byte[s.Length];
                    s.Read(Sbuffer, 0, (int)s.Length);
                  //  h = GCHandle.Alloc(Sbuffer);
                   // SoundPoint = Marshal.UnsafeAddrOfPinnedArrayElement(Sbuffer, 0);
                }
            }
            else
            {
                path = fileName;
            }
        }
        // PlaySound()
        [DllImport("winmm.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        static extern bool PlaySound(string pszSound,
            IntPtr hMod, SoundFlags sf);

        [DllImport("winmm.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        static extern bool sndPlaySound(IntPtr ptr, int fuSound);


        [Flags]
        public enum SoundFlags : int
        {
            SND_SYNC = 0x0000,  /* play synchronously (default) */
            SND_ASYNC = 0x0001,  /* play asynchronously */
            SND_NODEFAULT = 0x0002,  /* silence (!default) if sound not found */
            SND_MEMORY = 0x0004,  /* pszSound points to a memory file */
            SND_LOOP = 0x0008,  /* loop the sound until next sndPlaySound */
            SND_NOSTOP = 0x0010,  /* don't stop any currently playing sound */
            SND_NOWAIT = 0x00002000, /* don't wait if the driver is busy */
            SND_ALIAS = 0x00010000, /* name is a registry alias */
            SND_ALIAS_ID = 0x00110000, /* alias is a predefined ID */
            SND_FILENAME = 0x00020000, /* name is file name */
            SND_RESOURCE = 0x00040004  /* name is resource name or atom */
        }

        public void Play()
        {
            if (_buffer)
            {
                GCHandle h = GCHandle.Alloc(Sbuffer);
                IntPtr SoundPoint = Marshal.UnsafeAddrOfPinnedArrayElement(Sbuffer, 0);
                sndPlaySound(SoundPoint, (int)SoundFlags.SND_MEMORY | (int)SoundFlags.SND_ASYNC);
                h.Free();
            }
            else
            {
                PlaySound(path, new System.IntPtr(), SoundFlags.SND_SYNC);
            }
        }


        //private bool _Disposed = false;

        //private void Dispose(bool disposing)
        //{
        //    if (!this._Disposed)
        //    {
        //        if (disposing)
        //        {
        //            h.Free();
        //        }
        //    }
        //    this._Disposed = true;
        //}


        //public void Dispose()
        //{
        //    this.Dispose(true);
        //    GC.SuppressFinalize(this);
        //}


    }
}
