namespace Toub.Sound.Midi
{
    using System;
    using System.IO;
    using System.Text;

    [Serializable]
    public sealed class ChannelPressure : VoiceMidiEvent
    {
        private const byte _CATEGORY = 13;
        private byte _pressure;

        public ChannelPressure(long deltaTime, byte channel, byte pressure) : base(deltaTime, 13, channel)
        {
            this.Pressure = pressure;
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

        protected override byte Parameter1
        {
            get
            {
                return this._pressure;
            }
        }

        protected override byte Parameter2
        {
            get
            {
                return 0;
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

