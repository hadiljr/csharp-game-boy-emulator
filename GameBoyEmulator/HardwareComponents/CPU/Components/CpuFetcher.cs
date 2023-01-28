using GameBoyEmulator.Emulator;
using GameBoyEmulator.HardwareComponents.Bus;
using GameBoyEmulator.HardwareComponents.CPU.Instructions;
using GameBoyEmulator.Util.Bit;
using System;
using System.Threading.Tasks;

namespace GameBoyEmulator.HardwareComponents.CPU.Components
{
    class CpuFetcher
    {
        private readonly CpuContext ctx;
        private readonly IGbEmulator _gbEmulator;

        public CpuFetcher(CpuContext cpuContext, IGbEmulator gbEmulator)
        {
            ctx = cpuContext;
            _gbEmulator = gbEmulator;
        }

        public void FetchInstruction()
        {
            ctx.CurrentOpcode = BusInstance.Read(ctx.Registers.PC);
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
                    ctx.FetchedData = ctx.Registers.ReadRegister(ctx.CurrentInstruction.Register1.Value);
                    return;

                case AdressModeType.AM_R_R:
                    ctx.FetchedData = ctx.Registers.ReadRegister(ctx.CurrentInstruction.Register2.Value);
                    return;


                case AdressModeType.AM_R_D8:
                    ctx.FetchedData = BusInstance.Read(ctx.Registers.PC);
                    GbEmulator.cicles(1);
                    ctx.Registers.PC++;
                    return;

                case AdressModeType.AM_R_D16:
                case AdressModeType.AM_D16:

                    UInt16 lo = BusInstance.Read(ctx.Registers.PC);
                    GbEmulator.cicles(1);

                    UInt16 hi = BusInstance.Read((UInt16)(ctx.Registers.PC + 1));
                    GbEmulator.cicles(1);

                    ctx.FetchedData = (UInt16)(lo | (hi << 8));
                    ctx.Registers.PC += 2;

                    return;

                case AdressModeType.AM_MR_R:
                    var addr = ctx.Registers.ReadRegister(ctx.CurrentInstruction.Register2.Value);
                    if(ctx.CurrentInstruction.Register2 == RegisterType.RT_C)
                    {
                        addr |= 0xFF00;
                    }

                    ctx.FetchedData = BusInstance.Read(addr);
                    GbEmulator.cicles(1);
                    return;

                case AdressModeType.AM_HLI_R:
                    ctx.FetchedData = ctx.Registers.ReadRegister(ctx.CurrentInstruction.Register2.Value);
                    ctx.MemoryDestination = ctx.Registers.ReadRegister(ctx.CurrentInstruction.Register1.Value);
                    ctx.DestinationIsMemory = true;
                    var regHL = ctx.Registers.ReadRegister(RegisterType.RT_HL);
                    ctx.Registers.SetRegister(RegisterType.RT_HL, (UInt16)(regHL + 1));
                    return;

                case AdressModeType.AM_HLD_R:
                    ctx.FetchedData = ctx.Registers.ReadRegister(ctx.CurrentInstruction.Register2.Value);
                    ctx.MemoryDestination = ctx.Registers.ReadRegister(ctx.CurrentInstruction.Register1.Value);
                    ctx.DestinationIsMemory = true;
                    regHL = ctx.Registers.ReadRegister(RegisterType.RT_HL);
                    ctx.Registers.SetRegister(RegisterType.RT_HL, (UInt16)(regHL - 1));
                    return;

                case AdressModeType.AM_R_A8:
                    ctx.FetchedData = BusInstance.Read(ctx.Registers.PC);
                    GbEmulator.cicles(1);
                    ctx.Registers.PC++;
                    return;

                case AdressModeType.AM_A8_R:
                    ctx.MemoryDestination = (UInt16)(BusInstance.Read(ctx.Registers.PC) | 0xFF00);
                    ctx.DestinationIsMemory = true;
                    GbEmulator.cicles(1);
                    ctx.Registers.PC++;
                    return;

                case AdressModeType.AM_D8:
                case AdressModeType.AM_HL_SPR:
                    ctx.FetchedData = BusInstance.Read(ctx.Registers.PC);
                    GbEmulator.cicles(1);
                    ctx.Registers.PC++;
                    return;

                case AdressModeType.AM_A16_R:
                case AdressModeType.AM_D16_R:
                    lo = BusInstance.Read(ctx.Registers.PC);
                    GbEmulator.cicles(1);

                    hi = BusInstance.Read((UInt16)(ctx.Registers.PC+1));
                    GbEmulator.cicles(1);

                    ctx.MemoryDestination = (UInt16)(lo | (hi << 8));
                    ctx.DestinationIsMemory = true;

                    ctx.Registers.PC += 2;
                    ctx.FetchedData = ctx.Registers.ReadRegister(ctx.CurrentInstruction.Register2.Value);
                    return;

                case AdressModeType.AM_MR_D8:
                    ctx.FetchedData = BusInstance.Read(ctx.Registers.PC);
                    GbEmulator.cicles(1);

                    ctx.Registers.PC++;
                    ctx.MemoryDestination = ctx.Registers.ReadRegister(ctx.CurrentInstruction.Register1.Value);
                    ctx.DestinationIsMemory = true;
                    return;

                case AdressModeType.AM_MR:
                    ctx.MemoryDestination = ctx.Registers.ReadRegister(ctx.CurrentInstruction.Register1.Value);
                    ctx.DestinationIsMemory = true;
                    ctx.FetchedData = ctx.Registers.ReadRegister(ctx.CurrentInstruction.Register1.Value);
                    GbEmulator.cicles(1);
                    return;

                case AdressModeType.AM_R_A16:
                    lo = BusInstance.Read(ctx.Registers.PC);
                    GbEmulator.cicles(1);

                    hi = BusInstance.Read((UInt16)(ctx.Registers.PC + 1));
                    GbEmulator.cicles(1);

                    addr = (UInt16)(lo | (hi << 8));

                    ctx.Registers.PC += 2;
                    ctx.FetchedData = BusInstance.Read(addr);
                    GbEmulator.cicles(1);
                    return;



                default:
                    throw new Exception($"Unknown Addressing Mode! Mode: {ctx.CurrentInstruction.Mode} Opcode: {ctx.CurrentOpcode}");
            }
        }

      

    }
}
