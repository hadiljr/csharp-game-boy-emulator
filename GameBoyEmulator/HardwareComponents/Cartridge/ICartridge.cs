using System;
using System.Threading.Tasks;

namespace GameBoyEmulator.HardwareComponents.Cartridge
{
    public interface ICartridge
    {
        Task LoadCartridge(string file);

        Task<byte> Read(UInt16 address);

        Task Write(UInt16 adress, byte value);
    }
}
