using System;

namespace GameBoyEmulator.HardwareComponents.IO
{
    public interface IIO
    {
        byte Read(UInt16 address);
        void Write(UInt16 address, byte value);
    }
}
