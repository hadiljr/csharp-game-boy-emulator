using GameBoyEmulator.HardwareComponents.CPU.Instructions;
using GameBoyEmulator.HardwareComponents.DataBus;
using System;
using System.Collections.Generic;

namespace GameBoyEmulator.HardwareComponents.CPU.Processor
{
    public class ProcessorsList
    {
        private readonly Processors _proc;
        private Dictionary<InstructionType, Action> processors;

        public ProcessorsList(ICpu cpu, IBus bus)
        {
            _proc = new Processors(cpu, bus);
            Init();
        }

        private void Init()
        {

            processors = new Dictionary<InstructionType, Action>
            {
                {InstructionType.IN_NONE,_proc.ProcessIN_NONE},
                {InstructionType.IN_NOP,_proc.ProcessIN_NOP},
                {InstructionType.IN_LD,_proc.ProcessIN_LD},
                {InstructionType.IN_LDH,_proc.ProcessIN_LDH},
                {InstructionType.IN_JP,_proc.ProcessIN_JP},
                {InstructionType.IN_DI,_proc.ProcessIN_DI},
                {InstructionType.IN_POP,_proc.ProcessIN_POP},
                {InstructionType.IN_PUSH,_proc.ProcessIN_PUSH},
                {InstructionType.IN_JR,_proc.ProcessIN_JR},
                {InstructionType.IN_CALL,_proc.ProcessIN_CALL},
                {InstructionType.IN_RET,_proc.ProcessIN_RET},
                {InstructionType.IN_RST,_proc.ProcessIN_RST},
                {InstructionType.IN_DEC,_proc.ProcessIN_DEC},
                {InstructionType.IN_INC,_proc.ProcessIN_INC},
                {InstructionType.IN_ADD,_proc.ProcessIN_ADD},
                {InstructionType.IN_ADC,_proc.ProcessIN_ADC},
                {InstructionType.IN_SUB,_proc.ProcessIN_SUB},
                {InstructionType.IN_SBC,_proc.ProcessIN_SBC},
                {InstructionType.IN_AND,_proc.ProcessIN_AND},
                {InstructionType.IN_XOR,_proc.ProcessIN_XOR},
                {InstructionType.IN_OR,_proc.ProcessIN_OR},
                {InstructionType.IN_CP,_proc.ProcessIN_CP},
                {InstructionType.IN_CB,_proc.ProcessIN_CB},
                {InstructionType.IN_RRCA,_proc.ProcessIN_RRCA},
                {InstructionType.IN_RLCA,_proc.ProcessIN_RLCA},
                {InstructionType.IN_RRA,_proc.ProcessIN_RRA},
                {InstructionType.IN_RLA,_proc.ProcessIN_RLA},
                {InstructionType.IN_STOP,_proc.ProcessIN_STOP},
                {InstructionType.IN_HALT,_proc.ProcessIN_HALT},
                {InstructionType.IN_DAA,_proc.ProcessIN_DAA},
                {InstructionType.IN_CPL,_proc.ProcessIN_CPL},
                {InstructionType.IN_SCF,_proc.ProcessIN_SCF},
                {InstructionType.IN_CCF,_proc.ProcessIN_CCF},
                {InstructionType.IN_EI,_proc.ProcessIN_EI},
                {InstructionType.IN_RETI,_proc.ProcessIN_RETI},

            };
        }


        public Action GetInstructionProcessor(Instruction instruction)
        {
            if (instruction != null)
                return processors[instruction.Type];

            return null;

        }

    }
}
