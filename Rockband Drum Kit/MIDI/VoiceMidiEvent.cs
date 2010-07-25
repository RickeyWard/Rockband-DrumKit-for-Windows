namespace Toub.Sound.Midi
{
    using System;
    using System.IO;
    using System.Text;

    [Serializable]
    public abstract class VoiceMidiEvent : MidiEvent
    {
        private byte _category;
        private byte _channel;

        protected VoiceMidiEvent(long deltaTime, byte category, byte channel) : base(deltaTime)
        {
            if ((category < 0) || (category > 15))
            {
                throw new ArgumentOutOfRangeException("category", category, "Category values must be in the range from 0x0 to 0xF.");
            }
            this._category = category;
            this.Channel = channel;
        }

        private byte GetStatusByte()
        {
            return (byte) ((this._category << 4) | this._channel);
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(base.ToString());
            builder.Append("\t");
            builder.Append("0x");
            builder.Append(this.Channel.ToString("X1"));
            return builder.ToString();
        }

        public override void Write(Stream outputStream)
        {
            base.Write(outputStream);
            outputStream.WriteByte(this.GetStatusByte());
        }

        public byte Category
        {
            get
            {
                return this._category;
            }
        }

        public virtual byte Channel
        {
            get
            {
                return this._channel;
            }
            set
            {
                if ((value < 0) || (value > 15))
                {
                    throw new ArgumentOutOfRangeException("Channel", value, "The channel must be in the range from 0x0 to 0xF.");
                }
                this._channel = value;
            }
        }

        internal int Message
        {
            get
            {
                return ((this.Status | (this.Parameter1 << 8)) | (this.Parameter2 << 0x10));
            }
        }

        protected abstract byte Parameter1 { get; }

        protected abstract byte Parameter2 { get; }

        public byte Status
        {
            get
            {
                return this.GetStatusByte();
            }
        }
    }
}

