using System;
using System.Collections.Generic;
using System.Text;

namespace GameBoyEmulator.HardwareComponents.CPU.Instructions
{
    public static class InstructionList
    {
        private static Dictionary<byte, Instruction> _instructions = new Dictionary<byte, Instruction> {
            {0x00, new Instruction (InstructionType.IN_NOP, AdressModeType.AM_IMP ) },
            {0x05, new Instruction (InstructionType.IN_DEC, AdressModeType.AM_R, RegisterType.RT_B ) },
            {0x0E, new Instruction (InstructionType.IN_LD, AdressModeType.AM_R_D8, RegisterType.RT_C) },
            {0xAF, new Instruction (InstructionType.IN_XOR, AdressModeType.AM_R, RegisterType.RT_A ) },
            {0xC3, new Instruction (InstructionType.IN_JP,AdressModeType.AM_D16) },
            {0xF3, new Instruction (InstructionType.IN_DI)},
            };

        private static string[] _instructionsNames = new string[]{
            "<NONE>",
            "NOP",
            "LD",
            "INC",
            "DEC",
            "RLCA",
            "ADD",
            "RRCA",
            "STOP",
            "RLA",
            "JR",
            "RRA",
            "DAA",
            "CPL",
            "SCF",
            "CCF",
            "HALT",
            "ADC",
            "SUB",
            "SBC",
            "AND",
            "XOR",
            "OR",
            "CP",
            "POP",
            "JP",
            "PUSH",
            "RET",
            "CB",
            "CALL",
            "RETI",
            "LDH",
            "JPHL",
            "DI",
            "EI",
            "RST",
            "IN_ERR",
            "IN_RLC",
            "IN_RRC",
            "IN_RL",
            "IN_RR",
            "IN_SLA",
            "IN_SRA",
            "IN_SWAP",
            "IN_SRL",
            "IN_BIT",
            "IN_RES",
            "IN_SET"
            };

        public static Instruction InstructionByOpcode(byte opcode)
        {
            _instructions.TryGetValue(opcode, out Instruction instruction);
            return instruction;
        }

        public static string GetInstructionName(InstructionType type)
        {
            return _instructionsNames[(byte)type];
        }
    }
}
