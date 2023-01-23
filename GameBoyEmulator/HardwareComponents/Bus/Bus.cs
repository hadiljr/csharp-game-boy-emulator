using GameBoyEmulator.HardwareComponents.Cartridge;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GameBoyEmulator.HardwareComponents.Bus
{
    class Bus : IBus
    {
        private readonly ICartridge _cartridge;

        public Bus(ICartridge cartridge)
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

        public async Task<byte> ReadAsync(ushort address)
        {
            if (address < 0x8000)
            {
                //ROM Data
                return await _cartridge.Read(address);
            }

            throw new Exception($"Adress {address} not implemented.");
        }

        public async Task<ushort> Read16Async(ushort address)
        {
            var lo = await ReadAsync(address);
            var hi = await ReadAsync((ushort)(address+1));

            return (ushort)(lo | (hi << 8));
        }

        public byte Read(ushort address)
        {
            if (address < 0x8000)
            {
                var task = _cartridge.Read(address);
                task.Wait();
                return task.Result;
            }

            throw new Exception($"Adress {address} not implemented.");
        }

        public async Task WriteAsync(ushort address, byte value)
        {
            if (address < 0x8000)
            {
                //ROM Data
                await _cartridge.Write(address, value);

                return;
            }
        }

        public async Task Write16Async(ushort address, ushort value)
        {
            await WriteAsync((ushort)(address + 1), (byte)((value >> 8) & 0xFF));
            await WriteAsync(address, (byte)(value & 0xFF));
        }
    }
}
