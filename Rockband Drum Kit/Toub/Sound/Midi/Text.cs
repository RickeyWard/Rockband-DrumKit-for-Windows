namespace Toub.Sound.Midi
{
    using System;

    [Serializable]
    public sealed class Text : TextMetaMidiEvent
    {
        private const byte _METAID = 1;

        public Text(long deltaTime, string text) : base(deltaTime, 1, text)
        {
        }
    }
}

