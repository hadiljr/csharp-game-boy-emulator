using GameBoyEmulator.HardwareComponents.CPU;
using System;

namespace GameBoyEmulator.HardwareComponents.Timer
{
    public interface ITimer
    {
        void Init(ICpu cpu);

        void Tick();

        void SetDiv(UInt16 value);

        void Write(UInt16 address, byte value);

        byte Read(UInt16 address);
    }
}
