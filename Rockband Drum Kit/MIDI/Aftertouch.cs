namespace Toub.Sound.Midi
{
    using System;
    using System.IO;
    using System.Text;

    [Serializable]
    public sealed class Aftertouch : NoteVoiceMidiEvent
    {
        private const byte _CATEGORY = 10;
        private byte _pressure;

        public Aftertouch(long deltaTime, GeneralMidiPercussion percussion, byte pressure) : this(deltaTime, 9, MidiEvent.GetNoteValue(percussion), pressure)
        {
        }

        public Aftertouch(long deltaTime, byte channel, byte note, byte pressure) : base(deltaTime, 10, channel, note)
        {
            this.Pressure = pressure;
        }

        public Aftertouch(long deltaTime, byte channel, string note, byte pressure) : this(deltaTime, channel, MidiEvent.GetNoteValue(note), pressure)
        {
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(base.ToString());
            builder.Append("\t");
            builder.Append("0x");
            builder.Append(this._pressure.ToString("X2"));
            return builder.ToString();
        }

        public override void Write(Stream outputStream)
        {
            base.Write(outputStream);
            outputStream.WriteByte(this._pressure);
        }

        protected override byte Parameter2
        {
            get
            {
                return this._pressure;
            }
        }

        public byte Pressure
        {
            get
            {
                return this._pressure;
            }
            set
            {
                if ((value < 0) || (value > 0x7f))
                {
                    throw new ArgumentOutOfRangeException("Pressure", value, "The pressure must be in the range from 0 to 127.");
                }
                this._pressure = value;
            }
        }
    }
}

