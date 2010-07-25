namespace Toub.Sound.Midi
{
    using System;
    using System.IO;
    using System.Text;

    [Serializable]
    public sealed class PitchWheel : VoiceMidiEvent
    {
        private const byte _CATEGORY = 14;
        private byte _lowerBits;
        private byte _upperBits;

        public PitchWheel(long deltaTime, byte channel, int position) : base(deltaTime, 14, channel)
        {
            this.Position = position;
        }

        public PitchWheel(long deltaTime, byte channel, PitchWheelSteps steps) : this(deltaTime, channel, (int) steps)
        {
        }

        public PitchWheel(long deltaTime, byte channel, byte upperBits, byte lowerBits) : base(deltaTime, 14, channel)
        {
            this.UpperBits = upperBits;
            this.LowerBits = lowerBits;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(base.ToString());
            builder.Append("\t");
            builder.Append(this.Position.ToString());
            return builder.ToString();
        }

        public override void Write(Stream outputStream)
        {
            base.Write(outputStream);
            int position = this.Position;
            outputStream.WriteByte(this.Parameter1);
            outputStream.WriteByte(this.Parameter2);
        }

        public byte LowerBits
        {
            get
            {
                return this._lowerBits;
            }
            set
            {
                if ((this._lowerBits < 0) || (this._lowerBits > 0x7f))
                {
                    throw new ArgumentOutOfRangeException("LowerBits", value, "Value must be in the range from 0x0 to 0x7F.");
                }
                this._lowerBits = value;
            }
        }

        protected override byte Parameter1
        {
            get
            {
                return (byte) ((this.Position & 0xff00) >> 8);
            }
        }

        protected override byte Parameter2
        {
            get
            {
                return (byte) (this.Position & 0xff);
            }
        }

        public int Position
        {
            get
            {
                return MidiEvent.CombineBytesTo14Bits(this._upperBits, this._lowerBits);
            }
            set
            {
                if ((value < 0) || (value > 0x3fff))
                {
                    throw new ArgumentOutOfRangeException("Position", value, "Pitch wheel position must be in the range from 0x0 to 0x3FFF.");
                }
                MidiEvent.Split14BitsToBytes(value, out this._upperBits, out this._lowerBits);
            }
        }

        public byte UpperBits
        {
            get
            {
                return this._upperBits;
            }
            set
            {
                if ((this._upperBits < 0) || (this._upperBits > 0x7f))
                {
                    throw new ArgumentOutOfRangeException("UpperBits", value, "Value must be in the range from 0x0 to 0x7F.");
                }
                this._upperBits = value;
            }
        }
    }
}

