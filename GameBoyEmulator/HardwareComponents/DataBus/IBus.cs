using GameBoyEmulator.HardwareComponents.Cartridge;
using GameBoyEmulator.HardwareComponents.CPU;
using GameBoyEmulator.HardwareComponents.Timer;

namespace GameBoyEmulator.HardwareComponents.DataBus
{
    public interface IBus
    {
        void AttachCpu(ICpu cpu, ITimer timer);

        byte Read(ushort address);
        ushort Read16(ushort address);
        void Write(ushort address, byte value);
        void Write16(ushort address, ushort value);
        void InsertCartridge(ICartridge cartridge);
    }
}
