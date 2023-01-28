using GameBoyEmulator.HardwareComponents.Cartridge.Constants;
using GameBoyEmulator.Util.Extensions;
using System;
using System.IO;
using System.Threading.Tasks;

namespace GameBoyEmulator.Util.Memory
{
    internal sealed class CartridgeMemoryStream:MemoryStream
    {
        public RomHeader GetRomHeader()
        {
            Position = 0x100;

            byte[] romHeaderBuffer = new byte[80];

            ReadAsync(romHeaderBuffer, 0, 80).Wait();
            
            return romHeaderBuffer.CastToStruct<RomHeader>();
        }

        public string ChecksumResult()
        {
            UInt16 x = 0;
            for (UInt16 i = 0x0134; i <= 0x014C; i++)
            {
                byte[] buff = new byte[1];
                Position = i;
                ReadAsync(buff, 0, 1).Wait();
                x -= buff[0];
            }

            return Convert.ToBoolean(x & 0xFF) ? "PASSED" : "FAILED";
        }

        public uint GetRomSize()
        {
            return Convert.ToUInt32(Length);
        }

        public byte ReadAdress(ushort address)
        {
            Position = address;
            byte[] buffer = new byte[1];
            ReadAsync(buffer, 0, 1).Wait();
            return buffer[0];
        }

    }
}
