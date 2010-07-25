namespace Toub.Sound.Midi
{
    using System;
    using System.IO;
    using System.Text;

    [Serializable]
    public sealed class TimeSignature : MetaMidiEvent
    {
        private byte _denominator;
        private const byte _METAID = 0x58;
        private byte _midiClocksPerClick;
        private byte _numberOfNotated32nds;
        private byte _numerator;

        public TimeSignature(long deltaTime, byte numerator, byte denominator, byte midiClocksPerClick, byte numberOfNotated32nds) : base(deltaTime, 0x58)
        {
            this.Numerator = numerator;
            this.Denominator = denominator;
            this.MidiClocksPerClick = midiClocksPerClick;
            this.NumberOfNotated32nds = numberOfNotated32nds;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(base.ToString());
            builder.Append("\t");
            builder.Append("0x");
            builder.Append(this._numerator.ToString("X2"));
            builder.Append("\t");
            builder.Append("0x");
            builder.Append(this._denominator.ToString("X2"));
            builder.Append("\t");
            builder.Append("0x");
            builder.Append(this._midiClocksPerClick.ToString("X2"));
            builder.Append("\t");
            builder.Append("0x");
            builder.Append(this._numberOfNotated32nds.ToString("X2"));
            return builder.ToString();
        }

        public override void Write(Stream outputStream)
        {
            base.Write(outputStream);
            outputStream.WriteByte(4);
            outputStream.WriteByte(this._numerator);
            outputStream.WriteByte(this._denominator);
            outputStream.WriteByte(this._midiClocksPerClick);
            outputStream.WriteByte(this._numberOfNotated32nds);
        }

        public byte Denominator
        {
            get
            {
                return this._denominator;
            }
            set
            {
                this._denominator = value;
            }
        }

        public byte MidiClocksPerClick
        {
            get
            {
                return this._midiClocksPerClick;
            }
            set
            {
                this._midiClocksPerClick = value;
            }
        }

        public byte NumberOfNotated32nds
        {
            get
            {
                return this._numberOfNotated32nds;
            }
            set
            {
                this._numberOfNotated32nds = value;
            }
        }

        public byte Numerator
        {
            get
            {
                return this._numerator;
            }
            set
            {
                this._numerator = value;
            }
        }
    }
}

