using GameBoyEmulator.HardwareComponents.CPU.Instructions;
using GameBoyEmulator.HardwareComponents.Interruptions;
using GameBoyEmulator.HardwareComponents.ProgramStack;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameBoyEmulator.HardwareComponents.CPU
{
    public interface ICpu
    {
        event Action<int> CiclingEvent;

        CpuState State { get; }

        bool Step();

        IStack GetStack();

        byte GetIeRegister();
        void SetIeRegister(byte value);

        UInt16 ReadRegister(RegisterType registerType);

        byte ReadRegister8bits(RegisterType registerType);

        void SetRegister(RegisterType registerType, UInt16 value);

        void SetRegister8bits(RegisterType registerType, byte value);

        void RequestInterrupts(InterruptType interruptType);

    }
}
