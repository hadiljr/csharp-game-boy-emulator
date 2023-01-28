using GameBoyEmulator.HardwareComponents.CPU.Instructions;
using GameBoyEmulator.Util.Bit;
using System;
using System.Collections.Generic;
using System.Text;

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


        public UInt16 ReadRegister(RegisterType registerType)
        {
            switch (registerType)
            {
                case RegisterType.RT_A: return A;
                case RegisterType.RT_F: return F;
                case RegisterType.RT_B: return B;
                case RegisterType.RT_C: return C;
                case RegisterType.RT_D: return D;
                case RegisterType.RT_E: return E;
                case RegisterType.RT_H: return H;
                case RegisterType.RT_L: return L;

                case RegisterType.RT_AF: return BitHelper.CombineTwoValuesOf8bits(A, F);
                case RegisterType.RT_BC: return BitHelper.CombineTwoValuesOf8bits(B, C);
                case RegisterType.RT_DE: return BitHelper.CombineTwoValuesOf8bits(D, E);
                case RegisterType.RT_HL: return BitHelper.CombineTwoValuesOf8bits(H, L);

                case RegisterType.RT_PC: return PC;
                case RegisterType.RT_SP: return SP;
                default: return 0;
            }
        }



        public void SetRegister(RegisterType registerType, UInt16 value)
        {
            switch (registerType)
            {
                case RegisterType.RT_A:
                    A = (byte)(value & 0xFF);
                    break;
                case RegisterType.RT_F:
                    F = (byte)(value & 0xFF);
                    break;
                case RegisterType.RT_B:
                    B = (byte)(value & 0xFF);
                    break;
                case RegisterType.RT_C:
                    C = (byte)(value & 0xFF);
                    break;
                case RegisterType.RT_D:
                    D = (byte)(value & 0xFF);
                    break;
                case RegisterType.RT_E:
                    E = (byte)(value & 0xFF);
                    break;
                case RegisterType.RT_L:
                    L = (byte)(value & 0xFF);
                    break;

                case RegisterType.RT_AF:
                    var af = BitHelper.ExtractTwoValuesOf8Bits(value);
                    A = af.Item1;
                    F = af.Item2;
                    break;
                case RegisterType.RT_BC:
                    var bc = BitHelper.ExtractTwoValuesOf8Bits(value);
                    B = bc.Item1;
                    C = bc.Item2;
                    break;
                case RegisterType.RT_DE:
                    var de = BitHelper.ExtractTwoValuesOf8Bits(value);
                    D = de.Item1;
                    E = de.Item2;
                    break;
                case RegisterType.RT_HL:
                    var hl = BitHelper.ExtractTwoValuesOf8Bits(value);
                    H = hl.Item1;
                    L = hl.Item2;
                    break;


                case RegisterType.RT_PC:
                    PC = value;
                    break;
                case RegisterType.RT_SP:
                    SP = value;
                    break;

                case RegisterType.RT_NONE:
                    break;
            }
        }
    }
}
