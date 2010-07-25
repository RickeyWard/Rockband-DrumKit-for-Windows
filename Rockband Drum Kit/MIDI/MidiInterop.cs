namespace Toub.Sound.Midi
{
    using System;
    using System.Runtime.InteropServices;
    using System.Text;

    internal sealed class MidiInterop
    {
        private const int _MIDI_MAPPER = -1;

        private static string GetMciError(int errorCode)
        {
            StringBuilder lpszErrorText = new StringBuilder(0xff);
            if (NativeMethods.MciGetErrorString(errorCode, lpszErrorText, lpszErrorText.Capacity) == 0)
            {
                return null;
            }
            return lpszErrorText.ToString();
        }

        public static void MciSendString(string command)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }
            int rv = NativeMethods.MciSendString(command, null, 0, IntPtr.Zero);
            if (rv != 0)
            {
                ThrowMCIError(rv, "Could not execute command '" + command + "'.");
            }
        }

        public static MidiDeviceHandle OpenMidiOut()
        {
            return OpenMidiOut(-1);
        }

        public static MidiDeviceHandle OpenMidiOut(int deviceID)
        {
            int lphMidiOut = 0;
            int rv = NativeMethods.MidiOutOpen(ref lphMidiOut, deviceID, IntPtr.Zero, 0, 0);
            if (rv != 0)
            {
                ThrowMCIError(rv, "Could not open MIDI out.");
            }
            return new MidiDeviceHandle(lphMidiOut);
        }

        public static void SendMidiMessage(MidiDeviceHandle handle, int message)
        {
            if (handle == null)
            {
                throw new ArgumentNullException("handle", "The handle does not exist.  Make sure the MIDI device has been opened.");
            }
            int rv = NativeMethods.MidiOutShortMessage(handle.Handle, message);
            if (rv != 0)
            {
                ThrowMCIError(rv, "Could not execute message '" + message + "'.");
            }
        }

        private static void ThrowMCIError(int rv, string optionalMessage)
        {
            string mciError = GetMciError(rv);
            if (mciError == null)
            {
                mciError = "Could not close MIDI out.";
            }
            throw new InvalidOperationException(mciError);
        }

        public sealed class MidiDeviceHandle : IDisposable
        {
            private int _handle = 0;
            private bool _isDisposed = false;

            public MidiDeviceHandle(int handle)
            {
                this._handle = handle;
            }

            public void Close()
            {
                if (this.IsOpen)
                {
                    int rv = MidiInterop.NativeMethods.MidiOutClose(this._handle);
                    this._handle = 0;
                    if (rv != 0)
                    {
                        MidiInterop.ThrowMCIError(rv, "Could not close MIDI out.");
                    }
                }
            }

            private void Dispose(bool disposing)
            {
                if (!this._isDisposed)
                {
                    this.Close();
                    this._isDisposed = true;
                    if (disposing)
                    {
                        GC.SuppressFinalize(this);
                    }
                }
            }

            ~MidiDeviceHandle()
            {
                this.Dispose(false);
            }

            void IDisposable.Dispose()
            {
                this.Dispose(true);
            }

            public int Handle
            {
                get
                {
                    return this._handle;
                }
            }

            public bool IsOpen
            {
                get
                {
                    return (this._handle != 0);
                }
            }
        }

        private sealed class NativeMethods
        {
            [DllImport("winmm.dll", EntryPoint="mciGetErrorStringA", CharSet=CharSet.Ansi)]
            public static extern int MciGetErrorString(int fdwError, StringBuilder lpszErrorText, int cchErrorText);
            [DllImport("winmm.dll", EntryPoint="mciSendStringA", CharSet=CharSet.Ansi)]
            public static extern int MciSendString(string lpszCommand, StringBuilder lpszReturnString, int cchReturn, IntPtr hwndCallback);
            [DllImport("winmm.dll", EntryPoint="midiOutClose", CharSet=CharSet.Ansi)]
            public static extern int MidiOutClose(int hMidiOut);
            [DllImport("winmm.dll", EntryPoint="midiOutOpen", CharSet=CharSet.Ansi)]
            public static extern int MidiOutOpen(ref int lphMidiOut, int uDeviceID, IntPtr dwCallback, int dwInstance, int dwFlags);
            [DllImport("winmm.dll", EntryPoint="midiOutShortMsg", CharSet=CharSet.Ansi)]
            public static extern int MidiOutShortMessage(int hMidiOut, int dwMsg);
        }
    }
}

