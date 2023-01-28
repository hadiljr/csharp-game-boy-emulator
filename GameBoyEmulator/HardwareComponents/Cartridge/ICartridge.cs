using System;
namespace GameBoyEmulator.HardwareComponents.Cartridge
{
    public interface ICartridge
    {
        void LoadCartridge(string file);

        byte Read(UInt16 address);

        void Write(UInt16 adress, byte value);
    }
}
