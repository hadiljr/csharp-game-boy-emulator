namespace GameBoyEmulator.Util.Extensions
{
    static class ByteExtensions
    {
        public static bool ToBool(this byte byteValue)
        {
            return byteValue > 0;
        }
    }
}
