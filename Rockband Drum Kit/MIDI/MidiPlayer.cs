namespace Toub.Sound.Midi
{
    using System;
    using System.IO;

    public sealed class MidiPlayer
    {
        private static MidiInterop.MidiDeviceHandle _handle;
        private static object _midiLock = new object();
        private static int _references = 0;

        private MidiPlayer()
        {
        }

        public static void CloseMidi()
        {
            lock (_midiLock)
            {
                if (_references != 0)
                {
                    _references--;
                    if (_references == 0)
                    {
                        InternalCloseMidi();
                    }
                }
            }
        }

        private static void InternalCloseMidi()
        {
            if (_handle != null)
            {
                ((IDisposable) _handle).Dispose();
                _handle = null;
            }
        }

        private static void InternalOpenMidi()
        {
            _handle = MidiInterop.OpenMidiOut();
        }

        public static void OpenMidi()
        {
            lock (_midiLock)
            {
                if (_references == 0)
                {
                    InternalOpenMidi();
                }
                _references++;
            }
        }

        public static void Play(string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }
            if (!File.Exists(path))
            {
                throw new FileNotFoundException("The MIDI file was not found.", path);
            }
            string str = Guid.NewGuid().ToString("N");
            lock (_midiLock)
            {
                bool flag = (_handle != null) && _handle.IsOpen;
                if (flag)
                {
                    InternalCloseMidi();
                }
                MidiInterop.MciSendString("open \"" + path + "\" type mpegvideo alias " + str);
                MidiInterop.MciSendString("play " + str + " wait");
                MidiInterop.MciSendString("close " + str);
                if (flag)
                {
                    InternalOpenMidi();
                }
            }
        }

        public static void Play(MidiEvent ev)
        {
            lock (_midiLock)
            {
                if (ev is VoiceMidiEvent)
                {
                    MidiInterop.SendMidiMessage(_handle, ((VoiceMidiEvent) ev).Message);
                }
            }
        }

        public static void Play(MidiSequence sequence)
        {
            string tempFileName;
            if (sequence == null)
            {
                throw new ArgumentNullException("sequence");
            }
            try
            {
                tempFileName = Path.GetTempFileName();
                sequence.Save(tempFileName);
            }
            catch (Exception exception)
            {
                throw new ApplicationException("Unable to save the sequence to a temporary file.  " + exception.Message, exception);
            }
            Play(tempFileName);
            try
            {
                File.Delete(tempFileName);
            }
            catch
            {
            }
        }

        public static void Play(MidiEventCollection events, int division)
        {
            MidiSequence sequence = new MidiSequence(0, division);
            MidiTrack track = sequence.AddTrack();
            track.Events.Add(events);
            if (!track.HasEndOfTrack)
            {
                track.Events.Add(new EndOfTrack(0L));
            }
            Play(sequence);
        }

        public static void Play(MidiTrack track, int division)
        {
            MidiSequence sequence = new MidiSequence(0, division);
            sequence.AddTrack(track);
            Play(sequence);
        }
    }
}

