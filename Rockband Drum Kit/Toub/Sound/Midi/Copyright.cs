namespace Toub.Sound.Midi
{
    using System;

    [Serializable]
    public sealed class Copyright : TextMetaMidiEvent
    {
        private const byte _METAID = 2;

        public Copyright(long deltaTime, string text) : base(deltaTime, 2, text)
        {
        }
    }
}

