namespace Toub.Sound.Midi
{
    using System;
    using System.IO;
    using System.Text;

    [Serializable]
    public sealed class NoteOff : NoteVoiceMidiEvent
    {
        private const byte _CATEGORY = 8;
        private byte _velocity;

        public NoteOff(long deltaTime, GeneralMidiPercussion percussion, byte velocity) : this(deltaTime, 9, MidiEvent.GetNoteValue(percussion), velocity)
        {
        }

        public NoteOff(long deltaTime, byte channel, byte note, byte velocity) : base(deltaTime, 8, channel, note)
        {
            this.Velocity = velocity;
        }

        public NoteOff(long deltaTime, byte channel, string note, byte velocity) : this(deltaTime, channel, MidiEvent.GetNoteValue(note), velocity)
        {
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

