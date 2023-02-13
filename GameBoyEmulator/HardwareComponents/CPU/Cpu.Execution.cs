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
#if DEBUG
        private long counter = 0;
#endif

        public bool Step()
        {
            if (!ctx.Halted)
            {
                
                UInt16 pc = ctx.Registers.PC;

                FetchInstruction();
                if (ctx.CurrentInstruction == null) throw new Exception("Instrução nula");

                _board.Cicles(1);
                FetchData();
#if DEBUG
                counter++;
                var flags = $"{(Convert.ToBoolean(ctx.Registers.F & (1 << 7)) ? "Z" : "-")}" +
                        $"{(Convert.ToBoolean(ctx.Registers.F & (1 << 6)) ? "N" : "-")}" +
                        $"{(Convert.ToBoolean(ctx.Registers.F & (1 << 5)) ? "H" : "-")}" +
                        $"{(Convert.ToBoolean(ctx.Registers.F & (1 << 4)) ? "C" : "-")}";

                var msg = $"{counter:X4}-{pc:X4}: {ctx.CurrentInstruction.ToInfo(this, _bus),-12} " +
                    $"({ctx.CurrentOpcode:X2} {_bus.Read((ushort)(pc + 1)):X2} {_bus.Read((ushort)(pc + 2)):X2}) " +
                    $"A: {ctx.Registers.A:X2} F: {flags} " +
                    $"BC: {ctx.Registers.B:X2}{ctx.Registers.C:X2} DE: {ctx.Registers.D:X2}{ctx.Registers.E:X2} HL: {ctx.Registers.H:X2}{ctx.Registers.L:X2} Data:{ctx.FetchedData:X4}";





                _cpuDebugger.Update();


                Log.Debug(msg);
                _cpuDebugger.Print();



#endif

                Execute();
            }
            else
            {
                _board.Cicles(1);
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
            var processor = _processorsList.GetInstructionProcessor(ctx.CurrentInstruction);

            if (processor == null)
            {

                Console.WriteLine($"Instrução não implementada");
                return;
            }

            processor();
        }

        public void CallCicles(int number)
        {
            CiclingEvent?.Invoke(number);
        }
    }
}
