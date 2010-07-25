namespace Toub.Sound.Midi
{
    using System;

    [Serializable]
    public sealed class Lyric : TextMetaMidiEvent
    {
        private const byte _METAID = 5;

        public Lyric(long deltaTime, string text) : base(deltaTime, 5, text)
        {
        }
    }
}

