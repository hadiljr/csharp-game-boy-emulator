using GameBoyEmulator.Emulator;
using GameBoyEmulator.HardwareComponents.CPU.Instructions;
using GameBoyEmulator.HardwareComponents.DataBus;
using System;

namespace GameBoyEmulator.HardwareComponents.CPU
{

    public partial class Cpu
    {
        public void FetchInstruction()
        {
            ctx.CurrentOpcode = _bus.Read(ctx.Registers.PC);
            ctx.Registers.PC++;
            ctx.CurrentInstruction = InstructionList.InstructionByOpcode(ctx.CurrentOpcode);
        }

        public void FetchData()
        {
            ctx.MemoryDestination = 0;
            ctx.DestinationIsMemory = false;

            if (ctx.CurrentInstruction == null) return;

            switch (ctx.CurrentInstruction.Mode)
            {
                case AdressModeType.AM_IMP:
                    return;

                case AdressModeType.AM_R:
                    ctx.FetchedData = ReadRegister(ctx.CurrentInstruction.Register1.Value);
                    return;

                case AdressModeType.AM_R_R:
                    ctx.FetchedData = ReadRegister(ctx.CurrentInstruction.Register2.Value);
                    return;


                case AdressModeType.AM_R_D8:
                    ctx.FetchedData = _bus.Read(ctx.Registers.PC);
                    CiclingEvent?.Invoke(1);
                    ctx.Registers.PC++;
                    return;

                case AdressModeType.AM_R_D16:
                case AdressModeType.AM_D16:

                    UInt16 lo = _bus.Read(ctx.Registers.PC);
                    CiclingEvent?.Invoke(1);

                    UInt16 hi = _bus.Read((UInt16)(ctx.Registers.PC + 1));
                    CiclingEvent?.Invoke(1);

                    ctx.FetchedData = (UInt16)(lo | (hi << 8));
                    ctx.Registers.PC += 2;

                    return;

                case AdressModeType.AM_MR_R:
                    var addr = ReadRegister(ctx.CurrentInstruction.Register2.Value);
                    if (ctx.CurrentInstruction.Register2 == RegisterType.RT_C)
                    {
                        addr |= 0xFF00;
                    }

                    ctx.FetchedData = _bus.Read(addr);
                    GbEmulator.Cicles(1);
                    return;

                case AdressModeType.AM_R_MR:
                    addr = ReadRegister(ctx.CurrentInstruction.Register2.Value);

                    if (ctx.CurrentInstruction.Register2.Value == RegisterType.RT_C)
                    {
                        addr |= 0xFF00;
                    }

                    ctx.FetchedData = _bus.Read(addr);
                    CiclingEvent?.Invoke(1);
                    return;

                case AdressModeType.AM_R_HLI:
                    ctx.FetchedData = _bus.Read(ReadRegister(ctx.CurrentInstruction.Register2.Value));
                    CiclingEvent?.Invoke(1);
                    SetRegister(RegisterType.RT_HL, (UInt16)(ReadRegister(RegisterType.RT_HL) + 1));
                    return;

                case AdressModeType.AM_R_HLD:
                    ctx.FetchedData = _bus.Read(ReadRegister(ctx.CurrentInstruction.Register2.Value));
                    CiclingEvent?.Invoke(1);
                    SetRegister(RegisterType.RT_HL, (UInt16)(ReadRegister(RegisterType.RT_HL) - 1));
                    return;

                case AdressModeType.AM_HLI_R:
                    ctx.FetchedData = ReadRegister(ctx.CurrentInstruction.Register2.Value);
                    ctx.MemoryDestination = ReadRegister(ctx.CurrentInstruction.Register1.Value);
                    ctx.DestinationIsMemory = true;
                    var regHL = ReadRegister(RegisterType.RT_HL);
                    SetRegister(RegisterType.RT_HL, (UInt16)(regHL + 1));
                    return;

                case AdressModeType.AM_HLD_R:
                    ctx.FetchedData = ReadRegister(ctx.CurrentInstruction.Register2.Value);
                    ctx.MemoryDestination = ReadRegister(ctx.CurrentInstruction.Register1.Value);
                    ctx.DestinationIsMemory = true;
                    regHL = ReadRegister(RegisterType.RT_HL);
                    SetRegister(RegisterType.RT_HL, (UInt16)(regHL - 1));
                    return;

                case AdressModeType.AM_R_A8:
                    ctx.FetchedData = _bus.Read(ctx.Registers.PC);
                    CiclingEvent?.Invoke(1);
                    ctx.Registers.PC++;
                    return;

                case AdressModeType.AM_A8_R:
                    ctx.MemoryDestination = (UInt16)(_bus.Read(ctx.Registers.PC) | 0xFF00);
                    ctx.DestinationIsMemory = true;
                    CiclingEvent?.Invoke(1);
                    ctx.Registers.PC++;
                    return;

                case AdressModeType.AM_D8:
                case AdressModeType.AM_HL_SPR:
                    ctx.FetchedData = _bus.Read(ctx.Registers.PC);
                    CiclingEvent?.Invoke(1);
                    ctx.Registers.PC++;
                    return;

                case AdressModeType.AM_A16_R:
                case AdressModeType.AM_D16_R:
                    lo = _bus.Read(ctx.Registers.PC);
                    CiclingEvent?.Invoke(1);

                    hi = _bus.Read((UInt16)(ctx.Registers.PC + 1));
                    CiclingEvent?.Invoke(1);

                    ctx.MemoryDestination = (UInt16)(lo | (hi << 8));
                    ctx.DestinationIsMemory = true;

                    ctx.Registers.PC += 2;
                    ctx.FetchedData = ReadRegister(ctx.CurrentInstruction.Register2.Value);
                    return;

                case AdressModeType.AM_MR_D8:
                    ctx.FetchedData = _bus.Read(ctx.Registers.PC);
                    CiclingEvent?.Invoke(1);

                    ctx.Registers.PC++;
                    ctx.MemoryDestination = ReadRegister(ctx.CurrentInstruction.Register1.Value);
                    ctx.DestinationIsMemory = true;
                    return;

                case AdressModeType.AM_MR:
                    ctx.MemoryDestination = ReadRegister(ctx.CurrentInstruction.Register1.Value);
                    ctx.DestinationIsMemory = true;
                    ctx.FetchedData = ReadRegister(ctx.CurrentInstruction.Register1.Value);
                    CiclingEvent?.Invoke(1);
                    return;

                case AdressModeType.AM_R_A16:
                    lo = _bus.Read(ctx.Registers.PC);
                    CiclingEvent?.Invoke(1);

                    hi = _bus.Read((UInt16)(ctx.Registers.PC + 1));
                    CiclingEvent?.Invoke(1);

                    addr = (UInt16)(lo | (hi << 8));

                    ctx.Registers.PC += 2;
                    ctx.FetchedData = _bus.Read(addr);
                    CiclingEvent?.Invoke(1);
                    return;

                default:
                    throw new Exception($"Unknown Addressing Mode! Mode: {ctx.CurrentInstruction.Mode} Opcode: {ctx.CurrentOpcode}");
            }
        }
    }
}
