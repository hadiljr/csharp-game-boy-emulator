using System;

namespace GameBoyEmulator.Util.Extensions
{
    static class ByteExtensions
    {
        public static bool ToBool(this byte byteValue)
        {
            return byteValue > 0;
        }

        public static string ToBinaryString(this byte value)
        {
            return Convert.ToString(value, 2).PadLeft(8, '0');
        }
    }
}
