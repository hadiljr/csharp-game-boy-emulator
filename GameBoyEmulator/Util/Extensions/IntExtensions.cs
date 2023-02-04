namespace GameBoyEmulator.Util.Extensions
{
    static class IntExtensions
    {
        public static bool ToBool(this int value)
        {
            return value > 0;
        }
    }
}
