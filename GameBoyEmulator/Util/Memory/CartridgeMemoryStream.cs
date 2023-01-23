using GameBoyEmulator.HardwareComponents.Cartridge.Constants;
using GameBoyEmulator.Util.Extensions;
using System;
using System.IO;
using System.Threading.Tasks;

namespace GameBoyEmulator.Util.Memory
{
    internal sealed class CartridgeMemoryStream:MemoryStream
    {
        public async Task<RomHeader> GetRomHeader()
        {
            Position = 0x100;

            byte[] romHeaderBuffer = new byte[80];

            await ReadAsync(romHeaderBuffer, 0, 80);

            return romHeaderBuffer.CastToStruct<RomHeader>();
        }

        public async Task<string> ChecksumResult()
        {
            UInt16 x = 0;
            for (UInt16 i = 0x0134; i <= 0x014C; i++)
            {
                byte[] buff = new byte[1];
                Position = i;
                await ReadAsync(buff, 0, 1);
                x -= buff[0];
            }

            return Convert.ToBoolean(x & 0xFF) ? "PASSED" : "FAILED";
        }

        public uint GetRomSize()
        {
            return Convert.ToUInt32(Length);
        }

        public async Task<byte> ReadAdress(ushort address)
        {
            Position = address;
            byte[] buffer = new byte[1];
            await ReadAsync(buffer, 0, 1);
            return buffer[0];
        }

    }
}
