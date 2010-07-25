namespace Toub.Sound.Midi
{
    using System;
    using System.IO;
    using System.Text;

    [Serializable]
    public class SystemExclusiveMidiEvent : MidiEvent
    {
        private byte[] _data;

        public SystemExclusiveMidiEvent(long deltaTime, byte[] data) : base(deltaTime)
        {
            this._data = data;
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
            outputStream.WriteByte(240);
            base.WriteVariableLength(outputStream, (long) (1 + ((this._data != null) ? this._data.Length : 0)));
            if (this._data != null)
            {
                outputStream.Write(this._data, 0, this._data.Length);
            }
            outputStream.WriteByte(0xf7);
        }

        public virtual byte[] Data
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

