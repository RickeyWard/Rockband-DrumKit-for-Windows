namespace Toub.Sound.Midi
{
    using System;

    [Serializable]
    public enum PitchWheelSteps
    {
        HalfStepDown = 0x1000,
        HalfStepUp = 0x3000,
        NoStep = 0x2000,
        QuarterStepDown = 0x1500,
        QuarterStepUp = 0x2500,
        ThreeQuarterStepDown = 0x500,
        ThreeQuarterStepUp = 0x3500,
        WholeStepDown = 0,
        WholeStepUp = 0x3fff
    }
}

