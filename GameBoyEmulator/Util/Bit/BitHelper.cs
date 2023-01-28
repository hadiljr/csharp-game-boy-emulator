using System;

namespace GameBoyEmulator.Util.Bit
{

    public enum BIT_POSITION
    {
        SAME = -1,
        OFF = 0,
        ON = 1
    }

    public class BitHelper
    {
        public static bool GetBitValue(byte value, byte position)
        {
            return Convert.ToBoolean(value & (1 << position));
        }

        public static byte SetBitValue(byte value, byte position, BIT_POSITION pos)
        {
            int result;
            switch (pos){
                case BIT_POSITION.ON:
                    result = value | (1 << position);
                    return Convert.ToByte(result);
                case BIT_POSITION.OFF:
                    result = value & (~(1 << position));
                    return Convert.ToByte(result);
                case BIT_POSITION.SAME:
                default:
                    return value;
            }
        }

        public static Tuple<byte,byte> ExtractTwoValuesOf8Bits(UInt16 value)
        {
            byte first = (byte)((value >> 8) & 0xFF);
            byte second = (byte)(value & 0xFF);

            return new Tuple<byte, byte>(first, second);
        }

        public static UInt16 CombineTwoValuesOf8bits(byte first, byte second)
        {
            var result = (UInt16)((first << 8) | second);
            return result;
        }
    }
}
