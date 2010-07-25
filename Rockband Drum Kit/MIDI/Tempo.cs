namespace Toub.Sound.Midi
{
    using System;
    using System.IO;
    using System.Text;

    [Serializable]
    public sealed class Tempo : MetaMidiEvent
    {
        private const byte _METAID = 0x51;
        private int _tempo;

        public Tempo(long deltaTime, int value) : base(deltaTime, 0x51)
        {
            this.Value = value;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(base.ToString());
            builder.Append("\t");
            builder.Append(this._tempo.ToString());
            return builder.ToString();
        }

        public override void Write(Stream outputStream)
        {
            base.Write(outputStream);
            outputStream.WriteByte(3);
            outputStream.WriteByte((byte) ((this._tempo & 0xff0000) >> 0x10));
            outputStream.WriteByte((byte) ((this._tempo & 0xff00) >> 8));
            outputStream.WriteByte((byte) (this._tempo & 0xff));
        }

        public int Value
        {
            get
            {
                return this._tempo;
            }
            set
            {
                if ((value < 0) || (value > 0xffffff))
                {
                    throw new ArgumentOutOfRangeException("Tempo", value, "The tempo must be in the range from 0x0 to 0xFFFFFF.");
                }
                this._tempo = value;
            }
        }
    }
}

