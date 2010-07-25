namespace Toub.Sound.Midi
{
    using System;
    using System.IO;
    using System.Text;

    [Serializable]
    public sealed class MidiPort : MetaMidiEvent
    {
        private const byte _METAID = 0x21;
        private byte _port;

        public MidiPort(long deltaTime, byte port) : base(deltaTime, 0x21)
        {
            this.Port = port;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(base.ToString());
            builder.Append("\t");
            builder.Append("0x");
            builder.Append(this._port.ToString("X2"));
            return builder.ToString();
        }

        public override void Write(Stream outputStream)
        {
            base.Write(outputStream);
            outputStream.WriteByte(1);
            outputStream.WriteByte(this._port);
        }

        public byte Port
        {
            get
            {
                return this._port;
            }
            set
            {
                if ((value < 0) || (value > 0x7f))
                {
                    throw new ArgumentOutOfRangeException("Port", value, "The port must be in the range from 0x0 to 0x7F.");
                }
                this._port = value;
            }
        }
    }
}

