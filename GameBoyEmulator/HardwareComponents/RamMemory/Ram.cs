using System;

namespace GameBoyEmulator.HardwareComponents.RamMemory
{
    public class Ram : IRam
    {
        private RamDefinition _ram = new RamDefinition();

        public Ram()
        {
            _ram.Init();
        }
      
        public  byte WRamRead(UInt16 address)
        {
            address -= 0xC000;

            if (address >= 0x2000)
            {
                throw new Exception($"INVALID WRAM ADDR 0x{address + 0xC000:X8}");
            }

            return _ram.wram[address];
        }

        public void WRamWrite(UInt16 address, byte value)
        {
            address -= 0xC000;
            _ram.wram[address] = value;
        }

        public byte HRamRead(UInt16 address)
        {
            address -= 0xFF80;
            return _ram.hram[address];
        }

        public void HRamWrite(UInt16 address, byte value)
        {
            address -= 0xFF80;
            _ram.hram[address] = value;
        }
    }
}
