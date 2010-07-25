namespace Toub.Sound.Midi
{
    using System;
    using System.Collections;
    using System.IO;
    using System.Reflection;

    [Serializable]
    public class MidiSequence : IEnumerable
    {
        private int _division;
        private int _format;
        private ArrayList _tracks;

        public MidiSequence(int format, int division)
        {
            this.SetFormat(format);
            this.Division = division;
            this._tracks = new ArrayList();
        }

        public MidiTrack AddTrack()
        {
            MidiTrack track = new MidiTrack();
            this.AddTrack(track);
            return track;
        }

        public void AddTrack(MidiTrack track)
        {
            if (track == null)
            {
                throw new ArgumentNullException("track");
            }
            if (this._tracks.Contains(track))
            {
                throw new ArgumentException("This track is already part of the sequence.");
            }
            if ((this._format == 0) && (this._tracks.Count >= 1))
            {
                throw new InvalidOperationException("Format 0 MIDI files can only have 1 track.");
            }
            this._tracks.Add(track);
        }

        public static MidiSequence Convert(MidiSequence sequence, int format)
        {
            return Convert(sequence, format, FormatConversionOptions.None);
        }

        public static MidiSequence Convert(MidiSequence sequence, int format, FormatConversionOptions options)
        {
            if (sequence == null)
            {
                throw new ArgumentNullException("sequence");
            }
            if ((format < 0) || (format > 2))
            {
                throw new ArgumentOutOfRangeException("format", format, "The format must be 0, 1, or 2.");
            }
            if (sequence.Format == format)
            {
                return sequence;
            }
            if ((format != 0) || (sequence.NumberOfTracks == 1))
            {
                sequence.SetFormat(format);
                return sequence;
            }
            MidiSequence sequence2 = new MidiSequence(format, sequence.Division);
            MidiTrack track = sequence2.AddTrack();
            foreach (MidiTrack track2 in sequence)
            {
                track2.Events.ConvertDeltasToTotals();
            }
            int num = 0;
            foreach (MidiTrack track3 in sequence)
            {
                foreach (MidiEvent event2 in track3.Events)
                {
                    if ((((options & FormatConversionOptions.CopyTrackToChannel) > FormatConversionOptions.None) && (event2 is VoiceMidiEvent)) && ((num >= 0) && (num <= 15)))
                    {
                        ((VoiceMidiEvent) event2).Channel = (byte) num;
                    }
                    if (!(event2 is EndOfTrack))
                    {
                        track.Events.Add(event2);
                    }
                }
                num++;
            }
            track.Events.SortByTime();
            track.Events.ConvertTotalsToDeltas();
            track.Events.Add(new EndOfTrack(0L));
            return sequence2;
        }

        public IEnumerator GetEnumerator()
        {
            return this._tracks.GetEnumerator();
        }

        public MidiTrack[] GetTracks()
        {
            return (MidiTrack[]) this._tracks.ToArray(typeof(MidiTrack));
        }

        public static MidiSequence Import(Stream inputStream)
        {
            if (inputStream == null)
            {
                throw new ArgumentNullException("inputStream");
            }
            if (!inputStream.CanRead)
            {
                throw new ArgumentException("Stream must be readable.", "inputStream");
            }
            MThdChunkHeader header = MThdChunkHeader.Read(inputStream);
            MTrkChunkHeader[] headerArray = new MTrkChunkHeader[header.NumberOfTracks];
            for (int i = 0; i < header.NumberOfTracks; i++)
            {
                headerArray[i] = MTrkChunkHeader.Read(inputStream);
            }
            MidiSequence sequence = new MidiSequence(header.Format, header.Division);
            for (int j = 0; j < header.NumberOfTracks; j++)
            {
                sequence.AddTrack(MidiParser.ParseToTrack(headerArray[j].Data));
            }
            return sequence;
        }

        public static MidiSequence Import(string path)
        {
            using (FileStream stream = new FileStream(path, FileMode.Open))
            {
                return Import(stream);
            }
        }

        public static bool IsValid(string path)
        {
            try
            {
                Import(path);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void RemoveTrack(MidiTrack track)
        {
            this._tracks.Remove(track);
        }

        public void Save(Stream outputStream)
        {
            if (outputStream == null)
            {
                throw new ArgumentNullException("outputStream");
            }
            if (!outputStream.CanWrite)
            {
                throw new ArgumentException("Can't write to stream.", "outputStream");
            }
            if (this._tracks.Count < 1)
            {
                throw new InvalidOperationException("No tracks have been added.");
            }
            this.WriteHeader(outputStream, this._tracks.Count);
            for (int i = 0; i < this._tracks.Count; i++)
            {
                ((MidiTrack) this._tracks[i]).Write(outputStream);
            }
        }

        public void Save(string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path", "No path provided.");
            }
            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                this.Save(stream);
            }
        }

        private void SetFormat(int format)
        {
            if ((format < 0) || (format > 2))
            {
                throw new ArgumentOutOfRangeException("format", format, "Format must be 0, 1, or 2.");
            }
            this._format = format;
        }

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
            writer.WriteLine("MIDI Sequence");
            writer.WriteLine("Format: " + this._format);
            writer.WriteLine("Tracks: " + this._tracks.Count);
            writer.WriteLine("Division: " + this._division);
            writer.WriteLine("");
            foreach (MidiTrack track in this)
            {
                writer.WriteLine(track.ToString());
            }
        }

        public static void Transpose(MidiSequence sequence, int steps)
        {
            Transpose(sequence, steps, false);
        }

        public static void Transpose(MidiSequence sequence, int steps, bool includeDrums)
        {
            if (sequence == null)
            {
                throw new ArgumentNullException("sequence");
            }
            foreach (MidiTrack track in sequence)
            {
                foreach (MidiEvent event2 in track.Events)
                {
                    NoteVoiceMidiEvent event3 = event2 as NoteVoiceMidiEvent;
                    if ((event3 != null) && (includeDrums || (event3.Channel != 9)))
                    {
                        event3.Note = (byte) ((event3.Note + steps) % 0x80);
                    }
                }
            }
        }

        public static MidiSequence Trim(MidiSequence sequence, long totalTime)
        {
            MidiSequence sequence2 = new MidiSequence(sequence.Format, sequence.Division);
            foreach (MidiTrack track in sequence)
            {
                MidiTrack track2 = sequence2.AddTrack();
                track.Events.ConvertDeltasToTotals();
                for (int i = 0; (i < track.Events.Count) && (track.Events[i].DeltaTime < totalTime); i++)
                {
                    track2.Events.Add(track.Events[i].Clone());
                }
                track.Events.ConvertTotalsToDeltas();
                track2.Events.ConvertTotalsToDeltas();
                if (!track2.HasEndOfTrack)
                {
                    track2.Events.Add(new EndOfTrack(0L));
                }
            }
            return sequence2;
        }

        public void WriteHeader(Stream outputStream, int numTracks)
        {
            if (outputStream == null)
            {
                throw new ArgumentNullException("outputStream");
            }
            if (!outputStream.CanWrite)
            {
                throw new ArgumentException("Can't write to stream.", "outputStream");
            }
            if (numTracks < 1)
            {
                throw new ArgumentOutOfRangeException("numTracks", numTracks, "Sequences require at least 1 track.");
            }
            new MThdChunkHeader(this._format, numTracks, this._division).Write(outputStream);
        }

        public int Division
        {
            get
            {
                return this._division;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("Division", value, "The division must not be negative.");
                }
                this._division = value;
            }
        }

        public int Format
        {
            get
            {
                return this._format;
            }
        }

        public MidiTrack this[int index]
        {
            get
            {
                return (MidiTrack) this._tracks[index];
            }
            set
            {
                this._tracks[index] = value;
            }
        }

        public int NumberOfTracks
        {
            get
            {
                return this._tracks.Count;
            }
        }

        public enum FormatConversionOptions
        {
            None,
            CopyTrackToChannel
        }
    }
}

