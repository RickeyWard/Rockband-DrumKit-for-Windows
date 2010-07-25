namespace Toub.Sound.Midi
{
    using System;
    using System.IO;
    using System.Text;

    [Serializable]
    public sealed class SMPTEOffset : MetaMidiEvent
    {
        private byte _fractionalFrames;
        private byte _frames;
        private byte _hours;
        private const byte _METAID = 0x54;
        private byte _minutes;
        private byte _seconds;

        public SMPTEOffset(long deltaTime, byte hours, byte minutes, byte seconds, byte frames, byte fractionalFrames) : base(deltaTime, 0x54)
        {
            this.Hours = hours;
            this.Minutes = minutes;
            this.Seconds = seconds;
            this.Frames = frames;
            this.FractionalFrames = fractionalFrames;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(base.ToString());
            builder.Append("\t");
            builder.Append("0x");
            builder.Append(this._hours.ToString("X2"));
            builder.Append("\t");
            builder.Append("0x");
            builder.Append(this._minutes.ToString("X2"));
            builder.Append("\t");
            builder.Append("0x");
            builder.Append(this._seconds.ToString("X2"));
            builder.Append("\t");
            builder.Append("0x");
            builder.Append(this._frames.ToString("X2"));
            builder.Append("\t");
            builder.Append("0x");
            builder.Append(this._fractionalFrames.ToString("X2"));
            return builder.ToString();
        }

        public override void Write(Stream outputStream)
        {
            base.Write(outputStream);
            outputStream.WriteByte(5);
            outputStream.WriteByte(this._hours);
            outputStream.WriteByte(this._minutes);
            outputStream.WriteByte(this._seconds);
            outputStream.WriteByte(this._frames);
            outputStream.WriteByte(this._fractionalFrames);
        }

        public byte FractionalFrames
        {
            get
            {
                return this._fractionalFrames;
            }
            set
            {
                this._fractionalFrames = value;
            }
        }

        public byte Frames
        {
            get
            {
                return this._frames;
            }
            set
            {
                this._frames = value;
            }
        }

        public byte Hours
        {
            get
            {
                return this._hours;
            }
            set
            {
                this._hours = value;
            }
        }

        public byte Minutes
        {
            get
            {
                return this._minutes;
            }
            set
            {
                this._minutes = value;
            }
        }

        public byte Seconds
        {
            get
            {
                return this._seconds;
            }
            set
            {
                this._seconds = value;
            }
        }
    }
}

