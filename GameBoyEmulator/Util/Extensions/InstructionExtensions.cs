using GameBoyEmulator.HardwareComponents.CPU;
using GameBoyEmulator.HardwareComponents.CPU.Instructions;
using GameBoyEmulator.HardwareComponents.DataBus;
using System;

namespace GameBoyEmulator.Util.Extensions
{
    public static class InstructionExtensions
    {
        public static string ToInfo(this Instruction instruction, ICpu cpu, IBus bus)
        {
            var name = InstructionList.GetInstructionName(instruction);

           
                switch (instruction.Mode)
                {
                    case AdressModeType.AM_IMP:
                        return name ;
                    case AdressModeType.AM_R_D16:
                    case AdressModeType.AM_R_A16:
                        return $"{name} {instruction.Register1.Value.ToName()},{cpu.State.FetchedData:X4}";

                    case AdressModeType.AM_R:
                        return $"{name} {instruction.Register1.Value.ToName()}";

                    case AdressModeType.AM_R_R:
                        return $"{name} {instruction.Register1.Value.ToName()},{instruction.Register2.Value.ToName()}";

                    case AdressModeType.AM_MR_R:
                        return $"{name} ({instruction.Register1.Value.ToName()}),{instruction.Register2.Value.ToName()}";

                    case AdressModeType.AM_MR:
                        return $"{name} ({instruction.Register1.Value.ToName()})";

                    case AdressModeType.AM_R_MR:
                        return $"{name} {instruction.Register1.Value.ToName()},({instruction.Register2.Value.ToName()})";

                    case AdressModeType.AM_R_D8:
                    case AdressModeType.AM_R_A8:
                        return $"{name} {instruction.Register1.Value.ToName()},{cpu.State.FetchedData & 0xFF:X2}";

                    case AdressModeType.AM_R_HLI:
                        return $"{name} {instruction.Register1.Value.ToName()},({instruction.Register2.Value.ToName()}+)";

                    case AdressModeType.AM_R_HLD:
                        return $"{name} {instruction.Register1.Value.ToName()},({instruction.Register2.Value.ToName()}-)";

                    case AdressModeType.AM_HLI_R:
                        return $"{name} ({instruction.Register1.Value.ToName()}+),{instruction.Register2.Value.ToName()}";

                    case AdressModeType.AM_HLD_R:
                        return $"{name} ({instruction.Register1.Value.ToName()}-),{instruction.Register2.Value.ToName()}";

                    case AdressModeType.AM_A8_R:
                        return $"{name} {bus.Read((UInt16)(cpu.State.Registers.PC - 1)):X2},{instruction.Register2.Value.ToName()}";

                    case AdressModeType.AM_HL_SPR:
                        return $"{name} ({instruction.Register1.Value.ToName()}),SP+{cpu.State.FetchedData & 0xFF:D}";

                    case AdressModeType.AM_D8:
                        return $"{name} {cpu.State.FetchedData:X2}";

                    case AdressModeType.AM_D16:
                        return $"{name} {cpu.State.FetchedData:X4}";

                    case AdressModeType.AM_MR_D8:
                        return $"{name} ({instruction.Register1.Value.ToName()}),{cpu.State.FetchedData & 0xFF:X2}";

                    case AdressModeType.AM_A16_R:
                        return $"{name} ({cpu.State.FetchedData:X4}),{instruction.Register2.Value.ToName()}";

                    default:
                        throw new Exception($"INVALID AM: {instruction.Mode}");
                }
            
        }
    }
}
