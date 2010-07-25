namespace Toub.Sound.Midi
{
    using System;
    using System.IO;
    using System.Text;

    [Serializable]
    public sealed class KeySignature : MetaMidiEvent
    {
        private Toub.Sound.Midi.Key _key;
        private const byte _METAID = 0x59;
        private Toub.Sound.Midi.Tonality _tonality;

        public KeySignature(long deltaTime, byte key, byte tonality) : this(deltaTime, (Toub.Sound.Midi.Key) key, (Toub.Sound.Midi.Tonality) tonality)
        {
        }

        public KeySignature(long deltaTime, Toub.Sound.Midi.Key key, Toub.Sound.Midi.Tonality tonality) : base(deltaTime, 0x59)
        {
            this.Key = key;
            this.Tonality = tonality;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(base.ToString());
            builder.Append("\t");
            builder.Append(this._key.ToString());
            builder.Append("\t");
            builder.Append(this._tonality.ToString());
            return builder.ToString();
        }

        public override void Write(Stream outputStream)
        {
            base.Write(outputStream);
            outputStream.WriteByte(2);
            outputStream.WriteByte((byte) this._key);
            outputStream.WriteByte((byte) this._tonality);
        }

        public Toub.Sound.Midi.Key Key
        {
            get
            {
                return this._key;
            }
            set
            {
                Toub.Sound.Midi.Key key = (Toub.Sound.Midi.Key) ((sbyte) value);
                if (!Enum.IsDefined(typeof(Toub.Sound.Midi.Key), key))
                {
                    throw new ArgumentOutOfRangeException("Key", value, "Not a valid key.");
                }
                this._key = key;
            }
        }

        public Toub.Sound.Midi.Tonality Tonality
        {
            get
            {
                return this._tonality;
            }
            set
            {
                if (!Enum.IsDefined(typeof(Toub.Sound.Midi.Tonality), value))
                {
                    throw new ArgumentOutOfRangeException("Tonality", value, "Not a valid tonality.");
                }
                this._tonality = value;
            }
        }
    }
}

