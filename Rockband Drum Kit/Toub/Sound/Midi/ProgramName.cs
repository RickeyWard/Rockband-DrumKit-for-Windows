namespace Toub.Sound.Midi
{
    using System;

    [Serializable]
    public sealed class ProgramName : TextMetaMidiEvent
    {
        private const byte _METAID = 8;

        public ProgramName(long deltaTime, string text) : base(deltaTime, 8, text)
        {
        }
    }
}

