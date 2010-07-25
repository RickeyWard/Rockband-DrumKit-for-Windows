namespace Toub.Sound.Midi
{
    using System;

    [Serializable]
    public sealed class CuePoint : TextMetaMidiEvent
    {
        private const byte _METAID = 7;

        public CuePoint(long deltaTime, string text) : base(deltaTime, 7, text)
        {
        }
    }
}

