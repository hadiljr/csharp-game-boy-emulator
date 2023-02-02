using GameBoyEmulator.Emulator;
using GameBoyEmulator.HardwareComponents.CPU.Instructions;
using GameBoyEmulator.HardwareComponents.CPU.Processor;
using GameBoyEmulator.HardwareComponents.DataBus;
using System;

namespace GameBoyEmulator.HardwareComponents.CPU
{
    public static partial class Cpu
    {
        public static bool CpuStep()
        {
            if (!ctx.Halted)
            {
                UInt16 pc = ctx.Registers.PC;

                FetchInstruction();
                GbEmulator.cicles(1);
                FetchData();


                var msg = string.Format("0x{0:X4}: {1} \t(0x{2:X4} 0x{3:X4} 0x{4:X4}) \tA: 0x{5:X4} B: 0x{6:X4} C: 0x{7:X4}",
                    pc,
                    InstructionList.GetInstructionName(ctx.CurrentInstruction.Type),
                    ctx.CurrentOpcode,
                    Bus.Read((ushort)(pc + 1)),
                    Bus.Read((ushort)(pc + 2)),
                    ctx.Registers.A,
                    ctx.Registers.B,
                    ctx.Registers.C);

                Console.WriteLine(msg);

                Execute();
            }
            else
            {
                GbEmulator.cicles(1);
                if (Convert.ToBoolean(ctx.InterruptionFlags))
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

        private static void Execute()
        {
            var processor = ProcessorsList.GetInstructionProcessor(ctx.CurrentInstruction.Type);

            if (processor == null)
            {
                Console.WriteLine($"Instrução {Enum.GetName(typeof(InstructionType), ctx.CurrentInstruction.Type)} não implementada");
            }

            processor(ctx);
        }
    }
}
