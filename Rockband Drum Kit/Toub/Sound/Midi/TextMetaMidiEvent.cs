namespace Toub.Sound.Midi
{
    using System;
    using System.IO;
    using System.Text;

    [Serializable]
    public abstract class TextMetaMidiEvent : MetaMidiEvent
    {
        private string _text;

        protected TextMetaMidiEvent(long deltaTime, byte metaEventID, string text) : base(deltaTime, metaEventID)
        {
            this._text = text;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(base.ToString());
            builder.Append("\t");
            builder.Append(this.Text.ToString());
            return builder.ToString();
        }

        public override void Write(Stream outputStream)
        {
            base.Write(outputStream);
            byte[] bytes = Encoding.ASCII.GetBytes(this._text);
            base.WriteVariableLength(outputStream, (long) bytes.Length);
            outputStream.Write(bytes, 0, bytes.Length);
        }

        public string Text
        {
            get
            {
                return this._text;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("Text");
                }
                this._text = value;
            }
        }
    }
}

