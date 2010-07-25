namespace Toub.Sound.Midi
{
    using System;
    using System.IO;
    using System.Text;

    [Serializable]
    public sealed class UnknownMetaMidiEvent : MetaMidiEvent
    {
        private byte[] _data;

        public UnknownMetaMidiEvent(long deltaTime, byte metaEventID, byte[] data) : base(deltaTime, metaEventID)
        {
            this.Data = data;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(base.ToString());
            if (this._data != null)
            {
                builder.Append("\t");
            }
            builder.Append(MidiEvent.DataToString(this._data));
            return builder.ToString();
        }

        public override void Write(Stream outputStream)
        {
            base.Write(outputStream);
            base.WriteVariableLength(outputStream, (this._data != null) ? ((long) this._data.Length) : ((long) 0));
            if (this._data != null)
            {
                outputStream.Write(this._data, 0, this._data.Length);
            }
        }

        public byte[] Data
        {
            get
            {
                return this._data;
            }
            set
            {
                this._data = value;
            }
        }
    }
}

