namespace Toub.Sound.Midi
{
    using System;
    using System.IO;
    using System.Text;

    [Serializable]
    public sealed class NoteOn : NoteVoiceMidiEvent
    {
        private const byte _CATEGORY = 9;
        private byte _velocity;

        public NoteOn(long deltaTime, GeneralMidiPercussion percussion, byte velocity) : this(deltaTime, 9, MidiEvent.GetNoteValue(percussion), velocity)
        {
        }

        public NoteOn(long deltaTime, byte channel, byte note, byte velocity) : base(deltaTime, 9, channel, note)
        {
            this.Velocity = velocity;
        }

        public NoteOn(long deltaTime, byte channel, string note, byte velocity) : this(deltaTime, channel, MidiEvent.GetNoteValue(note), velocity)
        {
        }

        public static MidiEventCollection Complete(long deltaTime, GeneralMidiPercussion percussion, byte velocity, long duration)
        {
            return Complete(deltaTime, 9, MidiEvent.GetNoteValue(percussion), velocity, duration);
        }

        public static MidiEventCollection Complete(long deltaTime, byte channel, byte note, byte velocity, long duration)
        {
            MidiEventCollection events = new MidiEventCollection();
            events.Add(new NoteOn(deltaTime, channel, note, velocity));
            events.Add(new NoteOff(duration, channel, note, velocity));
            return events;
        }

        public static MidiEventCollection Complete(long deltaTime, byte channel, string note, byte velocity, long duration)
        {
            return Complete(deltaTime, channel, MidiEvent.GetNoteValue(note), velocity, duration);
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(base.ToString());
            builder.Append("\t");
            builder.Append("0x");
            builder.Append(this._velocity.ToString("X2"));
            return builder.ToString();
        }

        public override void Write(Stream outputStream)
        {
            base.Write(outputStream);
            outputStream.WriteByte(this._velocity);
        }

        protected override byte Parameter2
        {
            get
            {
                return this._velocity;
            }
        }

        public byte Velocity
        {
            get
            {
                return this._velocity;
            }
            set
            {
                if ((value < 0) || (value > 0x7f))
                {
                    throw new ArgumentOutOfRangeException("Velocity", value, "The velocity must be in the range from 0 to 127.");
                }
                this._velocity = value;
            }
        }
    }
}

