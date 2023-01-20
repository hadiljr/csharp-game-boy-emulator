using System;

namespace GameBoyEmulator.HardwareComponents.Bus
{
    public interface IBus
    {
        byte BusRead(UInt16 address);

        void BusWrite(UInt16 address, byte value);
    }
}
