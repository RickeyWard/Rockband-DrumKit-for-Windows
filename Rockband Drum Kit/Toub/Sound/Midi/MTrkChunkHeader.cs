namespace Toub.Sound.Midi
{
    using System;
    using System.IO;
    using System.Runtime.InteropServices;

    [Serializable, StructLayout(LayoutKind.Sequential)]
    internal struct MTrkChunkHeader
    {
        private ChunkHeader _header;
        private byte[] _data;
        public MTrkChunkHeader(byte[] data)
        {
            this._header = new ChunkHeader(MTrkID, (data != null) ? ((long) data.Length) : ((long) 0));
            this._data = data;
        }

        public ChunkHeader Header
        {
            get
            {
                return this._header;
            }
        }
        public byte[] Data
        {
            get
            {
                return this._data;
            }
        }
        private static byte[] MTrkID
        {
            get
            {
                return new byte[] { 0x4d, 0x54, 0x72, 0x6b };
            }
        }
        private static void ValidateHeader(ChunkHeader header)
        {
            byte[] mTrkID = MTrkID;
            for (int i = 0; i < 4; i++)
            {
                if (header.ID[i] != mTrkID[i])
                {
                    throw new InvalidOperationException("Invalid MThd header.");
                }
            }
            if (header.Length <= 0L)
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
            if (this._data != null)
            {
                outputStream.Write(this._data, 0, this._data.Length);
            }
        }

        public static MTrkChunkHeader Read(Stream inputStream)
        {
            if (inputStream == null)
            {
                throw new ArgumentNullException("inputStream");
            }
            if (!inputStream.CanRead)
            {
                throw new ArgumentException("Stream must be readable.", "inputStream");
            }
            ChunkHeader header = ChunkHeader.Read(inputStream);
            ValidateHeader(header);
            byte[] buffer = new byte[header.Length];
            if (inputStream.Read(buffer, 0, buffer.Length) != buffer.Length)
            {
                throw new InvalidOperationException("Not enough data in stream to read MTrk chunk.");
            }
            return new MTrkChunkHeader(buffer);
        }
    }
}

