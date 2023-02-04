using GameBoyEmulator.HardwareComponents.Cartridge;
using GameBoyEmulator.HardwareComponents.CPU;
using GameBoyEmulator.HardwareComponents.DMA;
using GameBoyEmulator.HardwareComponents.IO;
using GameBoyEmulator.HardwareComponents.PPU;
using GameBoyEmulator.HardwareComponents.RamMemory;

namespace GameBoyEmulator.HardwareComponents.DataBus
{
    public class Bus : IBus
    {
        private ICartridge _cartridge;
        private IRam _ram;
        private ICpu _cpu;
        private IIO _io;
        private IDma _dma;
        private IPpu _ppu;

        public Bus(IRam ram)
        {
            _ram = ram;
        }

        public void AttachCpu(ICpu cpu)
        {
            _cpu = cpu;
        }

        public void InsertCartridge(ICartridge cartridge)
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

        public byte Read(ushort address)
        {
            if (address < 0x8000)
            {
                return _cartridge.Read(address);
            }
            else if (address < 0xA000)
            {
                //Char/Map Data
                return _ppu.VRamRead(address);
            }
            else if (address < 0xC000)
            {
                //Cartridge RAM
                return _cartridge.Read(address);
            }
            else if (address < 0xE000)
            {
                //WRAM (Working RAM)
                return _ram.WRamRead(address);
            }
            else if (address < 0xFE00)
            {
                //reserved echo ram...
                return 0;
            }
            else if (address < 0xFEA0)
            {
                //OAM
                if (_dma.IsTrasfering())
                {
                    return 0xFF;
                }

                _ppu.OamRead(address);
            }
            else if (address < 0xFF00)
            {
                //reserved unusable
                return 0;
            }
            else if (address < 0xFF80)
            {
                //IO Registers
                return _io.Read(address);
            }
            else if (address == 0xFFFF)
            {
                //CPU ENABLE REGISTER
                return _cpu.GetIeRegister();
            }

            return _ram.HRamRead(address);
        }

        public ushort Read16(ushort address)
        {
            var lo = Read(address);
            var hi = Read((ushort)(address + 1));
            return (ushort)(lo | (hi << 8));
        }

        public void Write(ushort address, byte value)
        {
            if (address < 0x8000)
            {
                //ROM Data
                _cartridge.Write(address, value);
                return;
            }
            else if (address < 0xA000)
            {
                //Char/Map Data
                _ppu.VRamWrite(address, value);
            }
            else if (address < 0xC000)
            {
                //EXT-RAM
                _cartridge.Write(address, value);
            }
            else if (address < 0xE000)
            {
                //WRAM
                _ram.WRamWrite(address, value);
            }
            else if (address < 0xFE00)
            {
                //reserved echo ram
            }
            else if (address < 0xFEA0)
            {
                //OAM
                if (_dma.IsTrasfering())
                {
                    return;
                }

                _ppu.OamWrite(address, value);
                
            }
            else if (address < 0xFF00)
            {
                //unusable reserved
            }
            else if (address < 0xFF80)
            {
                //IO Registers
                _io.Write(address, value);
            }
            else if (address == 0xFFFF)
            {
                //CPU SET ENABLE REGISTER
                _cpu.SetIeRegister(value);
            }
            else
            {
                _ram.HRamWrite(address, value);
            }
        }

        public void Write16(ushort address, ushort value)
        {
            Write((ushort)(address + 1), (byte)((value >> 8) & 0xFF));
            Write(address, (byte)(value & 0xFF));
        }
    }
}
