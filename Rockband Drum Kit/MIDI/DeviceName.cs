namespace Toub.Sound.Midi
{
    using System;

    [Serializable]
    public sealed class DeviceName : TextMetaMidiEvent
    {
        private const byte _METAID = 9;

        public DeviceName(long deltaTime, string text) : base(deltaTime, 9, text)
        {
        }
    }
}

