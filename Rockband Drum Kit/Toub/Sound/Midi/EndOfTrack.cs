namespace Toub.Sound.Midi
{
    using System;
    using System.IO;

    [Serializable]
    public sealed class EndOfTrack : MetaMidiEvent
    {
        private const byte _METAID = 0x2f;

        public EndOfTrack(long deltaTime) : base(deltaTime, 0x2f)
        {
        }

        public override void Write(Stream outputStream)
        {
            base.Write(outputStream);
            outputStream.WriteByte(0);
        }
    }
}

