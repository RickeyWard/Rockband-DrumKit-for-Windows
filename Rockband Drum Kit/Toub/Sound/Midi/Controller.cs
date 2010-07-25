namespace Toub.Sound.Midi
{
    using System;
    using System.IO;
    using System.Text;

    [Serializable]
    public sealed class Controller : VoiceMidiEvent
    {
        private const byte _CATEGORY = 11;
        private byte _number;
        private byte _value;

        public Controller(long deltaTime, byte channel, byte number, byte value) : base(deltaTime, 11, channel)
        {
            this.Number = number;
            this.Value = value;
        }

        public Controller(long deltaTime, byte channel, Controllers number, byte value) : this(deltaTime, channel, (byte) number, value)
        {
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(base.ToString());
            builder.Append("\t");
            if (Enum.IsDefined(typeof(Controllers), this._number))
            {
                builder.Append(((Controllers) this._number).ToString());
            }
            else
            {
                builder.Append("0x");
                builder.Append(this._number.ToString("X2"));
            }
            builder.Append("\t");
            builder.Append("0x");
            builder.Append(this._value.ToString("X2"));
            return builder.ToString();
        }

        public override void Write(Stream outputStream)
        {
            base.Write(outputStream);
            outputStream.WriteByte(this._number);
            outputStream.WriteByte(this._value);
        }

        public byte Number
        {
            get
            {
                return this._number;
            }
            set
            {
                if ((value < 0) || (value > 0xff))
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
                return this._value;
            }
        }

        public byte Value
        {
            get
            {
                return this._value;
            }
            set
            {
                if ((value < 0) || (value > 0x7f))
                {
                    throw new ArgumentOutOfRangeException("Value", value, "The value must be in the range from 0 to 127.");
                }
                this._value = value;
            }
        }
    }
}

