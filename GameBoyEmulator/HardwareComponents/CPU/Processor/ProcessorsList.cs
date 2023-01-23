using GameBoyEmulator.HardwareComponents.CPU.Instructions;
using System;
using System.Collections.Generic;

namespace GameBoyEmulator.HardwareComponents.CPU.Processor
{
    public static class ProcessorsList
    {
        private static readonly Dictionary<InstructionType, Action<CpuContext>> processors = new Dictionary<InstructionType, Action<CpuContext>>
        {
            {InstructionType.IN_NONE,Processors.ProcessIN_NONE},
            {InstructionType.IN_NOP,Processors.ProcessIN_NOP},
            {InstructionType.IN_LD,Processors.ProcessIN_LD},
            {InstructionType.IN_JP,Processors.ProcessIN_JP},
            {InstructionType.IN_DI,Processors.ProcessIN_DI},
            {InstructionType.IN_XOR,Processors.ProcessIN_XOR},
        };


        public static Action<CpuContext> GetInstructionProcessor(InstructionType instruction)
        {
            return processors[instruction];
        }

    }
}
