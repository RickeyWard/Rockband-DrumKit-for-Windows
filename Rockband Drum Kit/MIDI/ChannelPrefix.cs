namespace Toub.Sound.Midi
{
    using System;
    using System.IO;
    using System.Text;

    [Serializable]
    public sealed class ChannelPrefix : MetaMidiEvent
    {
        private const byte _METAID = 0x20;
        private byte _prefix;

        public ChannelPrefix(long deltaTime, byte prefix) : base(deltaTime, 0x20)
        {
            this.Prefix = prefix;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(base.ToString());
            builder.Append("\t");
            builder.Append("0x");
            builder.Append(this._prefix.ToString("X2"));
            return builder.ToString();
        }

        public override void Write(Stream outputStream)
        {
            base.Write(outputStream);
            outputStream.WriteByte(1);
            outputStream.WriteByte(this._prefix);
        }

        public byte Prefix
        {
            get
            {
                return this._prefix;
            }
            set
            {
                if ((value < 0) || (value > 0x7f))
                {
                    throw new ArgumentOutOfRangeException("Prefix", value, "The prefix must be in the range from 0x0 to 0x7F.");
                }
                this._prefix = value;
            }
        }
    }
}

