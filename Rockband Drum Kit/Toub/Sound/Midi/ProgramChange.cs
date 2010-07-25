namespace Toub.Sound.Midi
{
    using System;
    using System.IO;
    using System.Text;

    [Serializable]
    public sealed class ProgramChange : VoiceMidiEvent
    {
        private const byte _CATEGORY = 12;
        private byte _number;

        public ProgramChange(long deltaTime, byte channel, byte number) : base(deltaTime, 12, channel)
        {
            this.Number = number;
        }

        public ProgramChange(long deltaTime, byte channel, GeneralMidiInstruments number) : this(deltaTime, channel, (byte) number)
        {
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(base.ToString());
            builder.Append("\t");
            if (Enum.IsDefined(typeof(GeneralMidiInstruments), this._number))
            {
                builder.Append(((GeneralMidiInstruments) this._number).ToString());
            }
            else
            {
                builder.Append("0x");
                builder.Append(this._number.ToString("X2"));
            }
            return builder.ToString();
        }

        public override void Write(Stream outputStream)
        {
            base.Write(outputStream);
            outputStream.WriteByte(this._number);
        }

        public byte Number
        {
            get
            {
                return this._number;
            }
            set
            {
                if ((value < 0) || (value > 0x7f))
                {
                    throw new ArgumentOutOfRangeException("Number", value, "The number must be in the range from 0 to 127.");
                }
                this._number = value;
            }
        }

        protected override byte Parameter1
        {
            get
            {
                return this._number;
            }
        }

        protected override byte Parameter2
        {
            get
            {
                return 0;
            }
        }
    }
}

