namespace Toub.Sound.Midi
{
    using System;
    using System.IO;

    [Serializable]
    public class MidiTrack
    {
        private MidiEventCollection _events = new MidiEventCollection();
        private bool _requireEndOfTrack = true;

        public override string ToString()
        {
            StringWriter writer = new StringWriter();
            this.ToString(writer);
            return writer.ToString();
        }

        public void ToString(TextWriter writer)
        {
            if (writer == null)
            {
                throw new ArgumentNullException("writer");
            }
            foreach (MidiEvent event2 in this.Events)
            {
                writer.WriteLine(event2.ToString());
            }
        }

        public void Write(Stream outputStream)
        {
            if (outputStream == null)
            {
                throw new ArgumentNullException("outputStream");
            }
            if (!outputStream.CanWrite)
            {
                throw new ArgumentException("Cannot write to stream.", "outputStream");
            }
            if (!this.HasEndOfTrack && this._requireEndOfTrack)
            {
                throw new InvalidOperationException("The track cannot be written until it has an end of track marker.");
            }
            MemoryStream stream = new MemoryStream();
            for (int i = 0; i < this._events.Count; i++)
            {
                this._events[i].Write(stream);
            }
            new MTrkChunkHeader(stream.ToArray()).Write(outputStream);
        }

        public MidiEventCollection Events
        {
            get
            {
                return this._events;
            }
        }

        public bool HasEndOfTrack
        {
            get
            {
                if (this._events.Count == 0)
                {
                    return false;
                }
                return (this._events[this._events.Count - 1] is EndOfTrack);
            }
        }

        public bool RequireEndOfTrack
        {
            get
            {
                return this._requireEndOfTrack;
            }
            set
            {
                this._requireEndOfTrack = value;
            }
        }
    }
}

