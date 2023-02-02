using GameBoyEmulator.HardwareComponents.CPU.Core;
using GameBoyEmulator.HardwareComponents.CPU.Instructions;
using GameBoyEmulator.Util.Bit;
using System;

namespace GameBoyEmulator.HardwareComponents.CPU
{
    public class CpuState
    {
        public CpuRegisters Registers;

        public UInt16 FetchedData { get; set; }
        public UInt16 MemoryDestination { get; set; }

        public byte CurrentOpcode { get; set; }
        public Instruction CurrentInstruction { get; set; }

        public bool Halted { get; set; }

        public bool Stepping { get; set; }

        public bool DestinationIsMemory { get; set; }

        public bool InterruptionMasterEnabled { get; set; }

        public bool EnableIME { get; set; }

        public byte IeRegister { get; set; }

        public byte InterruptionFlags { get; set; }

        public CpuState()
        {
         
            Registers = new CpuRegisters();
        }

        public bool ZFlag()
        {
            return BitHelper.GetBitValue(Registers.F, 7);
        }

        public bool NFlag()
        {
            return BitHelper.GetBitValue(Registers.F, 6);
        }

        public bool HFlag()
        {
            return BitHelper.GetBitValue(Registers.F, 5);
        }

        public bool CFlag()
        {
            return BitHelper.GetBitValue(Registers.F, 4);
        }
    }
}

