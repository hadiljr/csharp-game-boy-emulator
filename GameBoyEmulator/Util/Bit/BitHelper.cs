using System;

namespace GameBoyEmulator.Util.Bit
{

    enum BIT_POSITION
    {
        ON,
        OFF,
        SAME
    }

    class BitHelper
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
            byte first = (byte)(value & 0xFF);
            byte second = (byte)((value >> 8)&0xFF);

            return new Tuple<byte, byte>(first, second);
        }

        public static UInt16 CombineTwoValuesOf8bits(byte first, byte second)
        {
            return (UInt16)((first & 0x0F) | ((second & 0x0F) << 4));
        }

        public static UInt16 Reverse(UInt16 value)
        {
            return (UInt16)(((value & 0xFF00) >> 8) | ((value & 0x00FF) << 8));
        }

    }
}
