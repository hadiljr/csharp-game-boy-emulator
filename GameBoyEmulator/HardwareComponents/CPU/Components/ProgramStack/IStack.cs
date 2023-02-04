using System;

namespace GameBoyEmulator.HardwareComponents.ProgramStack
{
    public interface IStack
    {
        void Push(byte data);
        void Push16(UInt16 data);
        byte Pop();
        UInt16 Pop16();
    }
}
