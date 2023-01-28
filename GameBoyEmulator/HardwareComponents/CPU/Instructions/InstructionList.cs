using System;
using System.Collections.Generic;
using System.Text;

namespace GameBoyEmulator.HardwareComponents.CPU.Instructions
{
    public static class InstructionList
    {
        private static Dictionary<byte, Instruction> _instructions = new Dictionary<byte, Instruction> {
            {0x00, new Instruction (InstructionType.IN_NOP, AdressModeType.AM_IMP ) },
            {0x01, new Instruction (InstructionType.IN_LD, AdressModeType.AM_R_D16,RegisterType.RT_BC ) },
            {0x02, new Instruction (InstructionType.IN_LD, AdressModeType.AM_MR_R,RegisterType.RT_BC,RegisterType.RT_A ) },


            {0x05, new Instruction (InstructionType.IN_DEC, AdressModeType.AM_R, RegisterType.RT_B ) },
            {0x06, new Instruction (InstructionType.IN_LD, AdressModeType.AM_R_D8, RegisterType.RT_B ) },

            {0x08, new Instruction (InstructionType.IN_LD, AdressModeType.AM_A16_R, RegisterType.RT_NONE,RegisterType.RT_SP ) },

            {0x0A, new Instruction (InstructionType.IN_LD, AdressModeType.AM_R_MR, RegisterType.RT_A,RegisterType.RT_BC ) },

            {0x0E, new Instruction (InstructionType.IN_LD, AdressModeType.AM_R_D8, RegisterType.RT_C) },

            //0x1X
            {0x11, new Instruction (InstructionType.IN_LD, AdressModeType.AM_R_D16, RegisterType.RT_DE ) },
            {0x12, new Instruction (InstructionType.IN_LD, AdressModeType.AM_MR_R, RegisterType.RT_DE,RegisterType.RT_A ) },
            {0x15, new Instruction (InstructionType.IN_DEC, AdressModeType.AM_R, RegisterType.RT_D ) },
            {0x16, new Instruction (InstructionType.IN_LD, AdressModeType.AM_R_D8, RegisterType.RT_D ) },
            {0x1A, new Instruction (InstructionType.IN_LD, AdressModeType.AM_R_MR, RegisterType.RT_A,RegisterType.RT_DE ) },
            {0x1E, new Instruction (InstructionType.IN_LD, AdressModeType.AM_R_D8, RegisterType.RT_E ) },

            //0x2X
            {0x21, new Instruction (InstructionType.IN_LD, AdressModeType.AM_R_D16, RegisterType.RT_HL ) },
            {0x22, new Instruction (InstructionType.IN_LD, AdressModeType.AM_HLI_R, RegisterType.RT_HL,RegisterType.RT_A ) },
            {0x25, new Instruction (InstructionType.IN_DEC, AdressModeType.AM_R, RegisterType.RT_H) },
            {0x26, new Instruction (InstructionType.IN_LD, AdressModeType.AM_R_D8, RegisterType.RT_H ) },
            {0x2A, new Instruction (InstructionType.IN_LD, AdressModeType.AM_R_HLI, RegisterType.RT_A,RegisterType.RT_HL ) },
            {0x2E, new Instruction (InstructionType.IN_LD, AdressModeType.AM_R_D8, RegisterType.RT_L ) },

            //0x3X
            {0x31, new Instruction (InstructionType.IN_LD, AdressModeType.AM_R_D16, RegisterType.RT_SP ) },
            {0x32, new Instruction (InstructionType.IN_LD, AdressModeType.AM_HLD_R, RegisterType.RT_HL, RegisterType.RT_A ) },
            {0x35, new Instruction (InstructionType.IN_DEC, AdressModeType.AM_R, RegisterType.RT_HL ) },
            {0x36, new Instruction (InstructionType.IN_LD, AdressModeType.AM_MR_D8, RegisterType.RT_HL ) },
            {0x3A, new Instruction (InstructionType.IN_LD, AdressModeType.AM_R_HLD, RegisterType.RT_A,RegisterType.RT_HL ) },
            {0x3E, new Instruction (InstructionType.IN_LD, AdressModeType.AM_R_D8, RegisterType.RT_A ) },

            //0x4X
            {0x40, new Instruction (InstructionType.IN_LD, AdressModeType.AM_R_R, RegisterType.RT_B,RegisterType.RT_B ) },
            {0x41, new Instruction (InstructionType.IN_LD, AdressModeType.AM_R_R, RegisterType.RT_B,RegisterType.RT_C ) },
            {0x42, new Instruction (InstructionType.IN_LD, AdressModeType.AM_R_R, RegisterType.RT_B,RegisterType.RT_D ) },
            {0x43, new Instruction (InstructionType.IN_LD, AdressModeType.AM_R_R, RegisterType.RT_B,RegisterType.RT_E ) },
            {0x44, new Instruction (InstructionType.IN_LD, AdressModeType.AM_R_R, RegisterType.RT_B,RegisterType.RT_H ) },
            {0x45, new Instruction (InstructionType.IN_LD, AdressModeType.AM_R_R, RegisterType.RT_B,RegisterType.RT_L ) },
            {0x46, new Instruction (InstructionType.IN_LD, AdressModeType.AM_R_MR, RegisterType.RT_B,RegisterType.RT_HL ) },
            {0x47, new Instruction (InstructionType.IN_LD, AdressModeType.AM_R_R, RegisterType.RT_B,RegisterType.RT_A ) },
            {0x48, new Instruction (InstructionType.IN_LD, AdressModeType.AM_R_R, RegisterType.RT_C,RegisterType.RT_B ) },
            {0x49, new Instruction (InstructionType.IN_LD, AdressModeType.AM_R_R, RegisterType.RT_C,RegisterType.RT_C ) },
            {0x4A, new Instruction (InstructionType.IN_LD, AdressModeType.AM_R_R, RegisterType.RT_C,RegisterType.RT_D ) },
            {0x4B, new Instruction (InstructionType.IN_LD, AdressModeType.AM_R_R, RegisterType.RT_C,RegisterType.RT_E ) },
            {0x4C, new Instruction (InstructionType.IN_LD, AdressModeType.AM_R_R, RegisterType.RT_C,RegisterType.RT_H ) },
            {0x4D, new Instruction (InstructionType.IN_LD, AdressModeType.AM_R_R, RegisterType.RT_C,RegisterType.RT_L ) },
            {0x4E, new Instruction (InstructionType.IN_LD, AdressModeType.AM_R_MR, RegisterType.RT_C,RegisterType.RT_HL ) },
            {0x4F, new Instruction (InstructionType.IN_LD, AdressModeType.AM_R_R, RegisterType.RT_C,RegisterType.RT_A) },

            //0x5X
            {0x50, new Instruction (InstructionType.IN_LD, AdressModeType.AM_R_R, RegisterType.RT_D,RegisterType.RT_B ) },
            {0x51, new Instruction (InstructionType.IN_LD, AdressModeType.AM_R_R, RegisterType.RT_D,RegisterType.RT_C ) },
            {0x52, new Instruction (InstructionType.IN_LD, AdressModeType.AM_R_R, RegisterType.RT_D,RegisterType.RT_D ) },
            {0x53, new Instruction (InstructionType.IN_LD, AdressModeType.AM_R_R, RegisterType.RT_D,RegisterType.RT_E ) },
            {0x54, new Instruction (InstructionType.IN_LD, AdressModeType.AM_R_R, RegisterType.RT_D,RegisterType.RT_H ) },
            {0x55, new Instruction (InstructionType.IN_LD, AdressModeType.AM_R_R, RegisterType.RT_D,RegisterType.RT_L ) },
            {0x56, new Instruction (InstructionType.IN_LD, AdressModeType.AM_R_MR, RegisterType.RT_D,RegisterType.RT_HL ) },
            {0x57, new Instruction (InstructionType.IN_LD, AdressModeType.AM_R_R, RegisterType.RT_D,RegisterType.RT_A ) },
            {0x58, new Instruction (InstructionType.IN_LD, AdressModeType.AM_R_R, RegisterType.RT_E,RegisterType.RT_B ) },
            {0x59, new Instruction (InstructionType.IN_LD, AdressModeType.AM_R_R, RegisterType.RT_E,RegisterType.RT_C ) },
            {0x5A, new Instruction (InstructionType.IN_LD, AdressModeType.AM_R_R, RegisterType.RT_E,RegisterType.RT_D ) },
            {0x5B, new Instruction (InstructionType.IN_LD, AdressModeType.AM_R_R, RegisterType.RT_E,RegisterType.RT_E ) },
            {0x5C, new Instruction (InstructionType.IN_LD, AdressModeType.AM_R_R, RegisterType.RT_E,RegisterType.RT_H ) },
            {0x5D, new Instruction (InstructionType.IN_LD, AdressModeType.AM_R_R, RegisterType.RT_E,RegisterType.RT_L ) },
            {0x5E, new Instruction (InstructionType.IN_LD, AdressModeType.AM_R_MR, RegisterType.RT_E,RegisterType.RT_HL ) },
            {0x5F, new Instruction (InstructionType.IN_LD, AdressModeType.AM_R_R, RegisterType.RT_E,RegisterType.RT_A ) },

            //0x6X
            {0x60, new Instruction (InstructionType.IN_LD, AdressModeType.AM_R_R, RegisterType.RT_H,RegisterType.RT_B ) },
            {0x61, new Instruction (InstructionType.IN_LD, AdressModeType.AM_R_R, RegisterType.RT_H,RegisterType.RT_C ) },
            {0x62, new Instruction (InstructionType.IN_LD, AdressModeType.AM_R_R, RegisterType.RT_H,RegisterType.RT_D ) },
            {0x63, new Instruction (InstructionType.IN_LD, AdressModeType.AM_R_R, RegisterType.RT_H,RegisterType.RT_E ) },
            {0x64, new Instruction (InstructionType.IN_LD, AdressModeType.AM_R_R, RegisterType.RT_H,RegisterType.RT_H ) },
            {0x65, new Instruction (InstructionType.IN_LD, AdressModeType.AM_R_R, RegisterType.RT_H,RegisterType.RT_L ) },
            {0x66, new Instruction (InstructionType.IN_LD, AdressModeType.AM_R_MR, RegisterType.RT_H,RegisterType.RT_HL ) },
            {0x67, new Instruction (InstructionType.IN_LD, AdressModeType.AM_R_R, RegisterType.RT_H,RegisterType.RT_A ) },
            {0x68, new Instruction (InstructionType.IN_LD, AdressModeType.AM_R_R, RegisterType.RT_L,RegisterType.RT_B ) },
            {0x69, new Instruction (InstructionType.IN_LD, AdressModeType.AM_R_R, RegisterType.RT_L,RegisterType.RT_C ) },
            {0x6A, new Instruction (InstructionType.IN_LD, AdressModeType.AM_R_R, RegisterType.RT_L,RegisterType.RT_D ) },
            {0x6B, new Instruction (InstructionType.IN_LD, AdressModeType.AM_R_R, RegisterType.RT_L,RegisterType.RT_E ) },
            {0x6C, new Instruction (InstructionType.IN_LD, AdressModeType.AM_R_R, RegisterType.RT_L,RegisterType.RT_H ) },
            {0x6D, new Instruction (InstructionType.IN_LD, AdressModeType.AM_R_R, RegisterType.RT_L,RegisterType.RT_L ) },
            {0x6E, new Instruction (InstructionType.IN_LD, AdressModeType.AM_R_MR, RegisterType.RT_L,RegisterType.RT_HL ) },
            {0x6F, new Instruction (InstructionType.IN_LD, AdressModeType.AM_R_R, RegisterType.RT_L,RegisterType.RT_A ) },

            //0x7X
            {0x70, new Instruction (InstructionType.IN_LD, AdressModeType.AM_MR_R, RegisterType.RT_HL,RegisterType.RT_B ) },
            {0x71, new Instruction (InstructionType.IN_LD, AdressModeType.AM_MR_R, RegisterType.RT_HL,RegisterType.RT_C ) },
            {0x72, new Instruction (InstructionType.IN_LD, AdressModeType.AM_MR_R, RegisterType.RT_HL,RegisterType.RT_D ) },
            {0x73, new Instruction (InstructionType.IN_LD, AdressModeType.AM_MR_R, RegisterType.RT_HL,RegisterType.RT_E ) },
            {0x74, new Instruction (InstructionType.IN_LD, AdressModeType.AM_MR_R, RegisterType.RT_HL,RegisterType.RT_H ) },
            {0x75, new Instruction (InstructionType.IN_LD, AdressModeType.AM_MR_R, RegisterType.RT_HL,RegisterType.RT_L ) },
            {0x76, new Instruction (InstructionType.IN_HALT)},
            {0x77, new Instruction (InstructionType.IN_LD, AdressModeType.AM_MR_R, RegisterType.RT_HL,RegisterType.RT_A ) },
            {0x78, new Instruction (InstructionType.IN_LD, AdressModeType.AM_R_R, RegisterType.RT_A,RegisterType.RT_B ) },
            {0x79, new Instruction (InstructionType.IN_LD, AdressModeType.AM_R_R, RegisterType.RT_A,RegisterType.RT_C ) },
            {0x7A, new Instruction (InstructionType.IN_LD, AdressModeType.AM_R_R, RegisterType.RT_A,RegisterType.RT_D ) },
            {0x7B, new Instruction (InstructionType.IN_LD, AdressModeType.AM_R_R, RegisterType.RT_A,RegisterType.RT_E ) },
            {0x7C, new Instruction (InstructionType.IN_LD, AdressModeType.AM_R_R, RegisterType.RT_A,RegisterType.RT_H ) },
            {0x7D, new Instruction (InstructionType.IN_LD, AdressModeType.AM_R_R, RegisterType.RT_A,RegisterType.RT_L ) },
            {0x7E, new Instruction (InstructionType.IN_LD, AdressModeType.AM_MR_R, RegisterType.RT_A,RegisterType.RT_HL ) },
            {0x7F, new Instruction (InstructionType.IN_LD, AdressModeType.AM_R_R, RegisterType.RT_A,RegisterType.RT_A ) },

            {0xAF, new Instruction (InstructionType.IN_XOR, AdressModeType.AM_R, RegisterType.RT_A ) },

            {0xC3, new Instruction (InstructionType.IN_JP,AdressModeType.AM_D16) },

            //0xEX
            {0xE2, new Instruction (InstructionType.IN_LD, AdressModeType.AM_MR_R, RegisterType.RT_C,RegisterType.RT_A ) },
            {0xEA, new Instruction (InstructionType.IN_LD, AdressModeType.AM_A16_R, RegisterType.RT_NONE,RegisterType.RT_A ) },

            //0xFX
            {0xF2, new Instruction (InstructionType.IN_LD,AdressModeType.AM_R_MR,RegisterType.RT_A,RegisterType.RT_C)},
            {0xF3, new Instruction (InstructionType.IN_DI)},
            {0xFA, new Instruction (InstructionType.IN_LD,AdressModeType.AM_R_A16,RegisterType.RT_A)},
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
