using GameBoyEmulator.Util.Bit;

namespace GameBoyEmulator.Util.Extensions
{
    static class BoolExtensions
    {
        public static BIT_POSITION ToBitPosition(this bool boolean)
        {
            if (boolean) return BIT_POSITION.ON;
            else return BIT_POSITION.OFF;
        }
    }
}
