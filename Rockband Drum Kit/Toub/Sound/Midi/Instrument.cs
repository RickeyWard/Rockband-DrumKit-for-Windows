namespace Toub.Sound.Midi
{
    using System;

    [Serializable]
    public sealed class Instrument : TextMetaMidiEvent
    {
        private const byte _METAID = 4;

        public Instrument(long deltaTime, string text) : base(deltaTime, 4, text)
        {
        }
    }
}

