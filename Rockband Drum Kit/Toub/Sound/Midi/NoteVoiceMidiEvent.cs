namespace Toub.Sound.Midi
{
    using System;
    using System.IO;
    using System.Text;

    [Serializable]
    public abstract class NoteVoiceMidiEvent : VoiceMidiEvent
    {
        private byte _note;

        protected NoteVoiceMidiEvent(long deltaTime, byte category, byte channel, byte note) : base(deltaTime, category, channel)
        {
            this.Note = note;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(base.ToString());
            builder.Append("\t");
            if ((this.Channel == 9) && Enum.IsDefined(typeof(GeneralMidiPercussion), this._note))
            {
                builder.Append(((GeneralMidiPercussion) this._note).ToString());
            }
            else
            {
                builder.Append(MidiEvent.GetNoteName(this._note));
            }
            return builder.ToString();
        }

        public override void Write(Stream outputStream)
        {
            base.Write(outputStream);
            outputStream.WriteByte(this._note);
        }

        public byte Note
        {
            get
            {
                return this._note;
            }
            set
            {
                if ((value < 0) || (value > 0x7f))
                {
                    throw new ArgumentOutOfRangeException("Note", value, "The note must be in the range from 0 to 127.");
                }
                this._note = value;
            }
        }

        protected sealed override byte Parameter1
        {
            get
            {
                return this._note;
            }
        }
    }
}

