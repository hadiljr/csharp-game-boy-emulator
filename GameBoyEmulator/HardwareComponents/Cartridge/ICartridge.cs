using System;
namespace GameBoyEmulator.HardwareComponents.Cartridge
{
    public interface ICartridge
    {
        byte Read(UInt16 address);

        void Write(UInt16 adress, byte value);
    }
}
