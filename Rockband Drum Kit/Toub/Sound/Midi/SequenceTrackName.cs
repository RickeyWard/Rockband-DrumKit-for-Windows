namespace Toub.Sound.Midi
{
    using System;

    [Serializable]
    public sealed class SequenceTrackName : TextMetaMidiEvent
    {
        private const byte _METAID = 3;

        public SequenceTrackName(long deltaTime, string text) : base(deltaTime, 3, text)
        {
        }
    }
}

