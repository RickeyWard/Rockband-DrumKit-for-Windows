namespace Toub.Sound.Midi
{
    using System;

    [Serializable]
    public sealed class Marker : TextMetaMidiEvent
    {
        private const byte _METAID = 6;

        public Marker(long deltaTime, string text) : base(deltaTime, 6, text)
        {
        }
    }
}

