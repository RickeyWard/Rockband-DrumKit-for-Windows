namespace Toub.Sound.Midi
{
    using System;
    using System.IO;
    using System.Runtime.InteropServices;

    [Serializable, StructLayout(LayoutKind.Sequential)]
    internal struct ChunkHeader
    {
        private byte[] _id;
        private long _length;
        public ChunkHeader(byte[] id, long length)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }
            if (id.Length != 4)
            {
                throw new ArgumentException("The id must be 4 bytes in length.", "id");
            }
            if (length < 0L)
            {
                throw new ArgumentException("The length must be not be negative.", "length");
            }
            this._id = id;
            this._length = length;
        }

        public byte[] ID
        {
            get
            {
                return this._id;
            }
        }
        public long Length
        {
            get
            {
                return this._length;
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
            outputStream.WriteByte(this._id[0]);
            outputStream.WriteByte(this._id[1]);
            outputStream.WriteByte(this._id[2]);
            outputStream.WriteByte(this._id[3]);
            outputStream.WriteByte((byte) ((this._length & 0xff000000L) >> 0x18));
            outputStream.WriteByte((byte) ((this._length & 0xff0000L) >> 0x10));
            outputStream.WriteByte((byte) ((this._length & 0xff00L) >> 8));
            outputStream.WriteByte((byte) (this._length & 0xffL));
        }

        public static ChunkHeader Read(Stream inputStream)
        {
            if (inputStream == null)
            {
                throw new ArgumentNullException("inputStream");
            }
            if (!inputStream.CanRead)
            {
                throw new ArgumentException("Stream must be readable.", "inputStream");
            }
            byte[] buffer = new byte[4];
            if (inputStream.Read(buffer, 0, buffer.Length) != buffer.Length)
            {
                throw new InvalidOperationException("The stream is invalid.");
            }
            long length = 0L;
            for (int i = 0; i < 4; i++)
            {
                int num3 = inputStream.ReadByte();
                if (num3 < 0)
                {
                    throw new InvalidOperationException("The stream is invalid.");
                }
                length = length << 8;
                length |= (byte) num3;
            }
            return new ChunkHeader(buffer, length);
        }
    }
}

