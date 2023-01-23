using GameBoyEmulator.Emulator;
using GameBoyEmulator.HardwareComponents.Bus;
using GameBoyEmulator.HardwareComponents.CPU.Core;
using GameBoyEmulator.HardwareComponents.CPU.Instructions;
using GameBoyEmulator.Util.Bit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GameBoyEmulator.HardwareComponents.CPU
{
    public class CpuContext
    {
        public CpuRegisters Registers;

        public UInt16 FetchedData { get; set; }
        public UInt16 MemoryDestination { get; set; }

        public byte CurrentOpcode { get; set; }
        public Instruction CurrentInstruction { get; set; }

        public bool Halted { get; set; }

        public bool Stepping { get; set; }

        public bool DestinationIsMemory { get; set; }

        public bool IntMasterEnabled { get; set; }

        public CpuContext()
        {
         
            Registers = new CpuRegisters();
        }

        public bool ZFlag()
        {
            return BitHelper.GetBitValue(Registers.F, 7);
        }

        public bool CFlag()
        {
            return BitHelper.GetBitValue(Registers.F, 4);
        }
    }
}

