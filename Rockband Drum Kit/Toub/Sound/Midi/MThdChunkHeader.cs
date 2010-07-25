namespace Toub.Sound.Midi
{
    using System;
    using System.IO;
    using System.Runtime.InteropServices;

    [Serializable, StructLayout(LayoutKind.Sequential)]
    internal struct MThdChunkHeader
    {
        private ChunkHeader _header;
        private int _format;
        private int _numTracks;
        private int _division;
        public MThdChunkHeader(int format, int numTracks, int division)
        {
            if ((format < 0) || (format > 2))
            {
                throw new ArgumentOutOfRangeException("format", format, "Format must be 0, 1, or 2.");
            }
            if (numTracks < 1)
            {
                throw new ArgumentOutOfRangeException("numTracks", numTracks, "There must be at least 1 track.");
            }
            if (division < 1)
            {
                throw new ArgumentOutOfRangeException("division", division, "Division must be at least 1.");
            }
            this._header = new ChunkHeader(MThdID, 6L);
            this._format = format;
            this._numTracks = numTracks;
            this._division = division;
        }

        public ChunkHeader Header
        {
            get
            {
                return this._header;
            }
        }
        public int Format
        {
            get
            {
                return this._format;
            }
        }
        public int NumberOfTracks
        {
            get
            {
                return this._numTracks;
            }
        }
        public int Division
        {
            get
            {
                return this._division;
            }
        }
        private static byte[] MThdID
        {
            get
            {
                return new byte[] { 0x4d, 0x54, 0x68, 100 };
            }
        }
        private static void ValidateHeader(ChunkHeader header)
        {
            byte[] mThdID = MThdID;
            for (int i = 0; i < 4; i++)
            {
                if (header.ID[i] != mThdID[i])
                {
                    throw new InvalidOperationException("Invalid MThd header.");
                }
            }
            if (header.Length != 6L)
            {
                throw new InvalidOperationException("The length of the MThd header is incorrect.");
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
            this._header.Write(outputStream);
            outputStream.WriteByte((byte) ((this._format & 0xff00) >> 8));
            outputStream.WriteByte((byte) (this._format & 0xff));
            outputStream.WriteByte((byte) ((this._numTracks & 0xff00) >> 8));
            outputStream.WriteByte((byte) (this._numTracks & 0xff));
            outputStream.WriteByte((byte) ((this._division & 0xff00) >> 8));
            outputStream.WriteByte((byte) (this._division & 0xff));
        }

        public static MThdChunkHeader Read(Stream inputStream)
        {
            if (inputStream == null)
            {
                throw new ArgumentNullException("inputStream");
            }
            if (!inputStream.CanRead)
            {
                throw new ArgumentException("Stream must be readable.", "inputStream");
            }
            ValidateHeader(ChunkHeader.Read(inputStream));
            int format = 0;
            for (int i = 0; i < 2; i++)
            {
                int num3 = inputStream.ReadByte();
                if (num3 < 0)
                {
                    throw new InvalidOperationException("The stream is invalid.");
                }
                format = format << 8;
                format |= num3;
            }
            int numTracks = 0;
            for (int j = 0; j < 2; j++)
            {
                int num6 = inputStream.ReadByte();
                if (num6 < 0)
                {
                    throw new InvalidOperationException("The stream is invalid.");
                }
                numTracks = numTracks << 8;
                numTracks |= num6;
            }
            int division = 0;
            for (int k = 0; k < 2; k++)
            {
                int num9 = inputStream.ReadByte();
                if (num9 < 0)
                {
                    throw new InvalidOperationException("The stream is invalid.");
                }
                division = division << 8;
                division |= num9;
            }
            return new MThdChunkHeader(format, numTracks, division);
        }
    }
}

