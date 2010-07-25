namespace Toub.Sound.Midi
{
    using System;
    using System.IO;
    using System.Text;

    [Serializable]
    public abstract class MetaMidiEvent : MidiEvent
    {
        private byte _metaEventID;

        protected MetaMidiEvent(long deltaTime, byte metaEventID) : base(deltaTime)
        {
            this._metaEventID = metaEventID;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(base.ToString());
            builder.Append("\t");
            builder.Append("0x");
            builder.Append(this.MetaEventID.ToString("X2"));
            return builder.ToString();
        }

        public override void Write(Stream outputStream)
        {
            base.Write(outputStream);
            outputStream.WriteByte(0xff);
            outputStream.WriteByte(this._metaEventID);
        }

        public byte MetaEventID
        {
            get
            {
                return this._metaEventID;
            }
        }
    }
}

