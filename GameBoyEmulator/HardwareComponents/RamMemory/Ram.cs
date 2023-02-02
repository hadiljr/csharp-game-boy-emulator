using System;
using System.Collections.Generic;
using System.Text;

namespace GameBoyEmulator.HardwareComponents.RamMemory
{
    public static class Ram
    {
        private static RamDefinition _ram = new RamDefinition();

        public static byte WRamRead(UInt16 address)
        {
            address -= 0xC000;

            if (address >= 0x2000)
            {
                throw new Exception(string.Format("INVALID WRAM ADDR 0x{0:X8}", address + 0xC000));
            }

            return _ram.wram[address];
        }

        public static void WRamWrite(UInt16 address, byte value)
        {
            address -= 0xC000;
            _ram.wram[address] = value;
        }

        public static byte HRamRead(UInt16 address)
        {
            address -= 0xFF80;
            return _ram.hram[address];
        }

        public static void HRamWrite(UInt16 address, byte value)
        {
            address -= 0xFF80;
            _ram.hram[address] = value;
        }
    }
}
