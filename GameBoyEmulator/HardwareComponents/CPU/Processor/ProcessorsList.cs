using GameBoyEmulator.HardwareComponents.CPU.Instructions;
using System;
using System.Collections.Generic;

namespace GameBoyEmulator.HardwareComponents.CPU.Processor
{
    public static class ProcessorsList
    {
        private static readonly Dictionary<InstructionType, Action<CpuState>> processors = new Dictionary<InstructionType, Action<CpuState>>
        {
            {InstructionType.IN_NONE,Processors.ProcessIN_NONE},
            {InstructionType.IN_NOP,Processors.ProcessIN_NOP},
            {InstructionType.IN_LD,Processors.ProcessIN_LD},
            {InstructionType.IN_LDH,Processors.ProcessIN_LDH},
            {InstructionType.IN_JP,Processors.ProcessIN_JP},
            {InstructionType.IN_DI,Processors.ProcessIN_DI},
            {InstructionType.IN_POP,Processors.ProcessIN_POP},
            {InstructionType.IN_PUSH,Processors.ProcessIN_PUSH},
            {InstructionType.IN_JR,Processors.ProcessIN_JR},
            {InstructionType.IN_CALL,Processors.ProcessIN_CALL},
            {InstructionType.IN_RET,Processors.ProcessIN_RET},
            {InstructionType.IN_RST,Processors.ProcessIN_RST},
            {InstructionType.IN_DEC,Processors.ProcessIN_DEC},
            {InstructionType.IN_INC,Processors.ProcessIN_INC},
            {InstructionType.IN_ADD,Processors.ProcessIN_ADD},
            {InstructionType.IN_ADC,Processors.ProcessIN_ADC},
            {InstructionType.IN_SUB,Processors.ProcessIN_SUB},
            {InstructionType.IN_SBC,Processors.ProcessIN_SBC},
            {InstructionType.IN_AND,Processors.ProcessIN_AND},
            {InstructionType.IN_XOR,Processors.ProcessIN_XOR},
            {InstructionType.IN_OR,Processors.ProcessIN_OR},
            {InstructionType.IN_CP,Processors.ProcessIN_CP},
            {InstructionType.IN_CB,Processors.ProcessIN_CB},
            {InstructionType.IN_RRCA,Processors.ProcessIN_RRCA},
            {InstructionType.IN_RLCA,Processors.ProcessIN_RLCA},
            {InstructionType.IN_RRA,Processors.ProcessIN_RRA},
            {InstructionType.IN_RLA,Processors.ProcessIN_RLA},
            {InstructionType.IN_STOP,Processors.ProcessIN_STOP},
            {InstructionType.IN_HALT,Processors.ProcessIN_HALT},
            {InstructionType.IN_DAA,Processors.ProcessIN_DAA},
            {InstructionType.IN_CPL,Processors.ProcessIN_CPL},
            {InstructionType.IN_SCF,Processors.ProcessIN_SCF},
            {InstructionType.IN_CCF,Processors.ProcessIN_CCF},
            {InstructionType.IN_EI,Processors.ProcessIN_EI},
            {InstructionType.IN_RETI,Processors.ProcessIN_RETI},
            
        };


        public static Action<CpuState> GetInstructionProcessor(InstructionType instruction)
        {
            return processors[instruction];
        }

    }
}
