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
        private readonly IBus _bus;
        private readonly IGbEmulator _gbEmulator;

        public CpuFetcher(CpuContext cpuContext, IBus bus, IGbEmulator gbEmulator)
        {
            ctx = cpuContext;
            _bus = bus;
            _gbEmulator = gbEmulator;
        }

        public async Task FetchInstruction()
        {
            ctx.CurrentOpcode = await _bus.ReadAsync(ctx.Registers.PC);
            ctx.Registers.PC++;
            ctx.CurrentInstruction = InstructionList.InstructionByOpcode(ctx.CurrentOpcode);
        }

        public async Task FetchData()
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
                    ctx.FetchedData = await _bus.ReadAsync(ctx.Registers.PC);
                    _gbEmulator.cicles(1);
                    ctx.Registers.PC++;
                    return;

                case AdressModeType.AM_R_D16:
                case AdressModeType.AM_D16:

                    UInt16 lo = await _bus.ReadAsync(ctx.Registers.PC);
                    _gbEmulator.cicles(1);

                    UInt16 hi = await _bus.ReadAsync((UInt16)(ctx.Registers.PC + 1));
                    _gbEmulator.cicles(1);

                    ctx.FetchedData = (UInt16)(lo | (hi << 8));
                    ctx.Registers.PC += 2;

                    return;

                case AdressModeType.AM_MR_R:
                    //ctx.FetchedData =

                default:
                    throw new Exception($"Unknown Addressing Mode! Mode: {ctx.CurrentInstruction.Mode} Opcode: {ctx.CurrentOpcode}");
            }
        }

      

    }
}
