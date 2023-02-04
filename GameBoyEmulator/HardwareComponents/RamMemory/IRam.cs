using System;

namespace GameBoyEmulator.HardwareComponents.RamMemory
{
    public interface IRam
    {
        byte WRamRead(UInt16 address);
        void WRamWrite(UInt16 address, byte value);
        byte HRamRead(UInt16 address);
        void HRamWrite(UInt16 address, byte value);
    }
}
