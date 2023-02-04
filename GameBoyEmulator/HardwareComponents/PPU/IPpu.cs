using System;

namespace GameBoyEmulator.HardwareComponents.PPU
{
    public interface IPpu
    {
        void Tick();
      
        void OamWrite(UInt16 address, byte value);

        byte OamRead(UInt16 address);

        void VRamWrite(UInt16 address, byte value);

        byte VRamRead(UInt16 address);   
    }
}
