using System;
using System.Threading.Tasks;

namespace GameBoyEmulator.HardwareComponents.Bus
{
    public interface IBus
    {
        Task<byte> ReadAsync(UInt16 address);
        Task<UInt16> Read16Async(UInt16 address);

        byte Read(UInt16 address);

        Task WriteAsync(UInt16 address, byte value);
        Task Write16Async(UInt16 address, UInt16 value);
    }
}
