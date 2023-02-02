using System;

namespace GameBoyEmulator.HardwareComponents.CPU.Core
{
    public struct CpuRegisters
    {
        //registers
        public byte A;
        public byte F;
        public byte B;
        public byte C;
        public byte D;
        public byte E;
        public byte H;
        public byte L;

        // Program Count
        public UInt16 PC;

        // Stack Pointer
        public UInt16 SP;

    }
}
