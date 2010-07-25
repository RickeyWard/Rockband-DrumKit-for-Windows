namespace Toub.Sound.Midi
{
    using System;
    using System.IO;
    using System.Text;

    [Serializable]
    public sealed class SequenceNumber : MetaMidiEvent
    {
        private const byte _METAID = 0;
        private int _number;

        public SequenceNumber(long deltaTime, int number) : base(deltaTime, 0)
        {
            this.Number = number;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(base.ToString());
            builder.Append("\t");
            builder.Append(this._number.ToString());
            return builder.ToString();
        }

        public override void Write(Stream outputStream)
        {
            base.Write(outputStream);
            outputStream.WriteByte(2);
            outputStream.WriteByte((byte) ((this._number & 0xff00) >> 8));
            outputStream.WriteByte((byte) (this._number & 0xff));
        }

        public int Number
        {
            get
            {
                return this._number;
            }
            set
            {
                if ((value < 0) || (value > 0xffff))
                {
                    throw new ArgumentOutOfRangeException("Number", value, "The number must be in the range from 0x0 to 0xFFFF.");
                }
                this._number = value;
            }
        }
    }
}

