using GameBoyEmulator.Emulator;
using GameBoyEmulator.HardwareComponents.Bus;
using GameBoyEmulator.HardwareComponents.CPU.Components;
using GameBoyEmulator.HardwareComponents.CPU.Instructions;
using GameBoyEmulator.HardwareComponents.CPU.Processor;
using System;

namespace GameBoyEmulator.HardwareComponents.CPU
{
    public class Cpu : ICpu
    {
        private readonly CpuContext _context;
        private readonly CpuFetcher fetcher;       

        public Cpu(IGbEmulator gbEmulator)
        {
            
            _context = new CpuContext();
            fetcher = new CpuFetcher(_context, gbEmulator);
        }

        public void CpuInit()
        {
            _context.Registers.PC = 0x100;
            _context.Registers.A = 0x01;
        }

        public bool CpuStep()
        {
            if (!_context.Halted)
            {
                UInt16 pc = _context.Registers.PC;
                
                fetcher.FetchInstruction();
                fetcher.FetchData();
                
                var msg = string.Format("0x{0:X4}: {1} \t(0x{2:X4} 0x{3:X4} 0x{4:X4}) \tA: 0x{5:X4} B: 0x{6:X4} C: 0x{7:X4}",
                    pc,
                    InstructionList.GetInstructionName(_context.CurrentInstruction.Type),
                    _context.CurrentOpcode,
                    BusInstance.Read((ushort)(pc + 1)),
                    BusInstance.Read((ushort)(pc + 2)),
                    _context.Registers.A,
                    _context.Registers.B,
                    _context.Registers.C);

                Console.WriteLine(msg);
               
                Execute();
            }

            return true;
        }

        private void Execute()
        {
            var processor = ProcessorsList.GetInstructionProcessor(_context.CurrentInstruction.Type);

            if (processor == null)
            {
                Console.WriteLine($"Instrução {Enum.GetName(typeof(InstructionType), _context.CurrentInstruction.Type)} não implementada");
            }

            processor(_context);
        }
    }
}
