using GameBoyEmulator.HardwareComponents.Cartridge;
using System;
using System.Threading.Tasks;

namespace GameBoyEmulator.HardwareComponents.Bus
{
    static class BusInstance
    {
        private static ICartridge _cartridge;


        public static void SetCartridge(ICartridge cartridge)
        {
            _cartridge = cartridge;
        }       
      
        // 0x0000 - 0x3FFF : ROM Bank 0
        // 0x4000 - 0x7FFF : ROM Bank 1 - Switchable
        // 0x8000 - 0x97FF : CHR RAM
        // 0x9800 - 0x9BFF : BG Map 1
        // 0x9C00 - 0x9FFF : BG Map 2
        // 0xA000 - 0xBFFF : Cartridge RAM
        // 0xC000 - 0xCFFF : RAM Bank 0
        // 0xD000 - 0xDFFF : RAM Bank 1-7 - switchable - Color only
        // 0xE000 - 0xFDFF : Reserved - Echo RAM
        // 0xFE00 - 0xFE9F : Object Attribute Memory
        // 0xFEA0 - 0xFEFF : Reserved - Unusable
        // 0xFF00 - 0xFF7F : I/O Registers
        // 0xFF80 - 0xFFFE : Zero Page

       

        public static ushort Read16(ushort address)
        {
            var lo = Read(address);
            var hi = Read((ushort)(address+1));
            return (ushort)(lo | (hi << 8));
        }

        public static byte Read(ushort address)
        {
            if (address < 0x8000)
            {
                return _cartridge.Read(address);
            }

            throw new Exception($"Adress {address} not implemented.");
        }

        public static void Write(ushort address, byte value)
        {
            if (address < 0x8000)
            {
                //ROM Data
                _cartridge.Write(address, value);
                return;
            }
        }

        public static void Write16(ushort address, ushort value)
        {
            Write((ushort)(address + 1), (byte)((value >> 8) & 0xFF));
            Write(address, (byte)(value & 0xFF));
        }
    }
}
