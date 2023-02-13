using System;
using System.Collections.Generic;
using System.Text;

namespace GameBoyEmulator.HardwareComponents.PPU
{
    public class Ppu : IPpu
    {

        

        public void Tick()
        {

        }

        public void OamWrite(UInt16 address, byte value)
        {
            //TODO
        }

        public byte OamRead(UInt16 address)
        {
            //TODO
            return 0;
        }

        public void VRamWrite(UInt16 address, byte value)
        {
            //TODO
        }

        public byte VRamRead(UInt16 address)
        {
            //TODO
            return 0;
        }
    }
}
