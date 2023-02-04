using GameBoyEmulator.Emulator;
using GameBoyEmulator.HardwareComponents.CPU.Processor;
using GameBoyEmulator.Util.Debuger;
using GameBoyEmulator.Util.Extensions;
using Serilog;
using System;

namespace GameBoyEmulator.HardwareComponents.CPU
{
    public partial class Cpu
    {
        public bool Step()
        {
            if (!ctx.Halted)
            {
                UInt16 pc = ctx.Registers.PC;

                FetchInstruction();
                if (ctx.CurrentInstruction == null) throw new Exception("Instrução nula");

                GbEmulator.Cicles(1);
                FetchData();
#if DEBUG
                var flags = $"{(Convert.ToBoolean(ctx.Registers.F & (1 << 7)) ? "Z" : "-")}" +
                            $"{(Convert.ToBoolean(ctx.Registers.F & (1 << 6)) ? "N" : "-")}" +
                            $"{(Convert.ToBoolean(ctx.Registers.F & (1 << 5)) ? "H" : "-")}" +
                            $"{(Convert.ToBoolean(ctx.Registers.F & (1 << 4)) ? "C" : "-")}";

                var msg = $"{GbEmulator.Context.ticks:X8} - 0x{pc:X4}: {ctx.CurrentInstruction} \t" +
                    $"(0x{ctx.CurrentOpcode:X2} 0x{_bus.Read((ushort)(pc + 1)):X2} 0x{_bus.Read((ushort)(pc + 2)):X2}) \t" +
                    $"A: 0x{ctx.Registers.A:X2} F: {flags} " +
                    $"BC: 0x{ctx.Registers.B:X2}{ctx.Registers.C:X2} DE: 0x{ctx.Registers.D:X2}{ctx.Registers.E:X2} HL: 0x{ctx.Registers.H:X2}{ctx.Registers.L:X2}";

                Log.Debug(msg);
                CpuDebugger.Update();
                CpuDebugger.Print();
#endif
                Execute();
            }
            else
            {
                GbEmulator.Cicles(1);
                if (ctx.InterruptionFlags.ToBool())
                {
                    ctx.Halted = false;
                }
            }

            if (ctx.InterruptionMasterEnabled)
            {
                HandleInterrupts();
                ctx.EnableIME = false;
            }

            if (ctx.EnableIME)
            {
                ctx.InterruptionMasterEnabled = true;
            }

            return true;
        }

        private void Execute()
        {
            var processor = ProcessorsList.GetInstructionProcessor(ctx.CurrentInstruction);

            if (processor == null)
            {
                Console.WriteLine($"Instrução não implementada");
                return;
            }

            processor();
        }
    }
}
