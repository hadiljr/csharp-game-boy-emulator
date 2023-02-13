using GameBoyEmulator.HardwareComponents.CPU.Instructions;
using GameBoyEmulator.Util.Bit;
using System;

namespace GameBoyEmulator.HardwareComponents.CPU
{
    public partial class Cpu
    {
        public byte GetIeRegister()
        {
            return ctx.IeRegister;
        }

        public void SetIeRegister(byte value)
        {
            ctx.IeRegister = value;
        }

        public UInt16 ReadRegister(RegisterType registerType)
        {
            switch (registerType)
            {
                case RegisterType.RT_A: return ctx.Registers.A;
                case RegisterType.RT_F: return ctx.Registers.F;
                case RegisterType.RT_B: return ctx.Registers.B;
                case RegisterType.RT_C: return ctx.Registers.C;
                case RegisterType.RT_D: return ctx.Registers.D;
                case RegisterType.RT_E: return ctx.Registers.E;
                case RegisterType.RT_H: return ctx.Registers.H;
                case RegisterType.RT_L: return ctx.Registers.L;

                case RegisterType.RT_AF: return BitHelper.CombineTwoValuesOf8bits(ctx.Registers.A, ctx.Registers.F);
                case RegisterType.RT_BC: return BitHelper.CombineTwoValuesOf8bits(ctx.Registers.B, ctx.Registers.C);
                case RegisterType.RT_DE: return BitHelper.CombineTwoValuesOf8bits(ctx.Registers.D, ctx.Registers.E);
                case RegisterType.RT_HL: return BitHelper.CombineTwoValuesOf8bits(ctx.Registers.H, ctx.Registers.L);

                case RegisterType.RT_PC: return ctx.Registers.PC;
                case RegisterType.RT_SP: return ctx.Registers.SP;
                default: return 0;
            }
        }

        public byte ReadRegister8bits(RegisterType registerType)
        {
            switch (registerType)
            {
                case RegisterType.RT_A: return ctx.Registers.A;
                case RegisterType.RT_F: return ctx.Registers.F;
                case RegisterType.RT_B: return ctx.Registers.B;
                case RegisterType.RT_C: return ctx.Registers.C;
                case RegisterType.RT_D: return ctx.Registers.D;
                case RegisterType.RT_E: return ctx.Registers.E;
                case RegisterType.RT_H: return ctx.Registers.H;
                case RegisterType.RT_L: return ctx.Registers.L;

                case RegisterType.RT_HL:
                    return _bus.Read(ReadRegister(RegisterType.RT_HL));

                default:
                    throw new Exception("Invalid register 8 bit");

            }
        }

        public void SetRegister(RegisterType registerType, UInt16 value)
        {
            switch (registerType)
            {
                case RegisterType.RT_A:
                    ctx.Registers.A = (byte)(value & 0xFF);
                    break;
                case RegisterType.RT_F:
                    ctx.Registers.F = (byte)(value & 0xFF);
                    break;
                case RegisterType.RT_B:
                    ctx.Registers.B = (byte)(value & 0xFF);
                    break;
                case RegisterType.RT_C:
                    ctx.Registers.C = (byte)(value & 0xFF);
                    break;
                case RegisterType.RT_D:
                    ctx.Registers.D = (byte)(value & 0xFF);
                    break;
                case RegisterType.RT_E:
                    ctx.Registers.E = (byte)(value & 0xFF);
                    break; 
                case RegisterType.RT_H:
                    ctx.Registers.H = (byte)(value & 0xFF);
                    break;
                case RegisterType.RT_L:
                    ctx.Registers.L = (byte)(value & 0xFF);
                    break;

                case RegisterType.RT_AF:
                    var af = BitHelper.ExtractTwoValuesOf8Bits(value);
                    ctx.Registers.A = af.Item1;
                    ctx.Registers.F = af.Item2;
                    break;
                case RegisterType.RT_BC:
                    var bc = BitHelper.ExtractTwoValuesOf8Bits(value);
                    ctx.Registers.B = bc.Item1;
                    ctx.Registers.C = bc.Item2;
                    break;
                case RegisterType.RT_DE:
                    var de = BitHelper.ExtractTwoValuesOf8Bits(value);
                    ctx.Registers.D = de.Item1;
                    ctx.Registers.E = de.Item2;
                    break;
                case RegisterType.RT_HL:
                    var hl = BitHelper.ExtractTwoValuesOf8Bits(value);
                    ctx.Registers.H = hl.Item1;
                    ctx.Registers.L = hl.Item2;
                    break;


                case RegisterType.RT_PC:
                    ctx.Registers.PC = value;
                    break;
                case RegisterType.RT_SP:
                    ctx.Registers.SP = value;
                    break;

                case RegisterType.RT_NONE:
                    break;
            }
        }

        public void SetRegister8bits(RegisterType registerType, byte value)
        {
            switch (registerType)
            {
                case RegisterType.RT_A:
                    ctx.Registers.A = (byte)(value & 0xFF);
                    break;
                case RegisterType.RT_F:
                    ctx.Registers.F = (byte)(value & 0xFF);
                    break;
                case RegisterType.RT_B:
                    ctx.Registers.B = (byte)(value & 0xFF);
                    break;
                case RegisterType.RT_C:
                    ctx.Registers.C = (byte)(value & 0xFF);
                    break;
                case RegisterType.RT_D:
                    ctx.Registers.D = (byte)(value & 0xFF);
                    break;
                case RegisterType.RT_E:
                    ctx.Registers.E = (byte)(value & 0xFF);
                    break; 
                case RegisterType.RT_H:
                    ctx.Registers.H = (byte)(value & 0xFF);
                    break;
                case RegisterType.RT_L:
                    ctx.Registers.L = (byte)(value & 0xFF);
                    break;
                case RegisterType.RT_HL:
                    _bus.Write(ReadRegister(RegisterType.RT_HL), value);
                    break;

                default:
                    throw new Exception("Invalid register 8 bit");
            }
        }
    }
}
