﻿using System.Collections.Generic;

namespace GameBoyEmulator.HardwareComponents.CPU.Instructions
{
    public static class InstructionList
    {
        private static Dictionary<byte, Instruction> _instructions = new Dictionary<byte, Instruction> {
            //0x0X - OK
            {0x00, new Instruction (InstructionType.IN_NOP, AdressModeType.AM_IMP ) },
            {0x01, new Instruction (InstructionType.IN_LD, AdressModeType.AM_R_D16,RegisterType.RT_BC ) },
            {0x02, new Instruction (InstructionType.IN_LD, AdressModeType.AM_MR_R,RegisterType.RT_BC,RegisterType.RT_A ) },
            {0x03, new Instruction (InstructionType.IN_INC, AdressModeType.AM_R,RegisterType.RT_BC ) },
            {0x04, new Instruction (InstructionType.IN_INC, AdressModeType.AM_R,RegisterType.RT_B ) },
            {0x05, new Instruction (InstructionType.IN_DEC, AdressModeType.AM_R, RegisterType.RT_B ) },
            {0x06, new Instruction (InstructionType.IN_LD, AdressModeType.AM_R_D8, RegisterType.RT_B ) },
            {0x07, new Instruction (InstructionType.IN_RLCA, AdressModeType.AM_R_D8, RegisterType.RT_B ) },
            {0x08, new Instruction (InstructionType.IN_LD, AdressModeType.AM_A16_R, RegisterType.RT_NONE,RegisterType.RT_SP ) },
            {0x09, new Instruction (InstructionType.IN_ADD, AdressModeType.AM_R_R, RegisterType.RT_HL,RegisterType.RT_BC ) },
            {0x0A, new Instruction (InstructionType.IN_LD, AdressModeType.AM_R_MR, RegisterType.RT_A,RegisterType.RT_BC ) },
            {0x0B, new Instruction (InstructionType.IN_DEC, AdressModeType.AM_R, RegisterType.RT_BC ) },
            {0x0C, new Instruction (InstructionType.IN_INC, AdressModeType.AM_R, RegisterType.RT_C ) },
            {0x0D, new Instruction (InstructionType.IN_DEC, AdressModeType.AM_R, RegisterType.RT_C ) },
            {0x0E, new Instruction (InstructionType.IN_LD, AdressModeType.AM_R_D8, RegisterType.RT_C) },
            {0x0F, new Instruction (InstructionType.IN_RRCA) },

            //0x1X - OK
            {0x10, new Instruction (InstructionType.IN_STOP ) },
            {0x11, new Instruction (InstructionType.IN_LD, AdressModeType.AM_R_D16, RegisterType.RT_DE ) },
            {0x12, new Instruction (InstructionType.IN_LD, AdressModeType.AM_MR_R, RegisterType.RT_DE,RegisterType.RT_A ) },
            {0x13, new Instruction (InstructionType.IN_INC, AdressModeType.AM_R, RegisterType.RT_DE ) },
            {0x14, new Instruction (InstructionType.IN_INC, AdressModeType.AM_R, RegisterType.RT_D ) },
            {0x15, new Instruction (InstructionType.IN_DEC, AdressModeType.AM_R, RegisterType.RT_D ) },
            {0x16, new Instruction (InstructionType.IN_LD, AdressModeType.AM_R_D8, RegisterType.RT_D ) },
            {0x17, new Instruction (InstructionType.IN_RLA ) },
            {0x18, new Instruction (InstructionType.IN_JR, AdressModeType.AM_D8) },
            {0x19, new Instruction (InstructionType.IN_ADD, AdressModeType.AM_R_R, RegisterType.RT_HL,RegisterType.RT_DE ) },
            {0x1A, new Instruction (InstructionType.IN_LD, AdressModeType.AM_R_MR, RegisterType.RT_A,RegisterType.RT_DE ) },
            {0x1B, new Instruction (InstructionType.IN_DEC, AdressModeType.AM_R, RegisterType.RT_DE ) },
            {0x1C, new Instruction (InstructionType.IN_INC, AdressModeType.AM_R, RegisterType.RT_E ) },
            {0x1D, new Instruction (InstructionType.IN_DEC, AdressModeType.AM_R, RegisterType.RT_E ) },
            {0x1E, new Instruction (InstructionType.IN_LD, AdressModeType.AM_R_D8, RegisterType.RT_E ) },
            {0x1F, new Instruction (InstructionType.IN_RRA ) },

            //0x2X - OK
            {0x20, new Instruction (InstructionType.IN_JR, AdressModeType.AM_D8, RegisterType.RT_NONE,RegisterType.RT_NONE,ConditionType.CT_NZ) },
            {0x21, new Instruction (InstructionType.IN_LD, AdressModeType.AM_R_D16, RegisterType.RT_HL ) },
            {0x22, new Instruction (InstructionType.IN_LD, AdressModeType.AM_HLI_R, RegisterType.RT_HL,RegisterType.RT_A ) },
            {0x23, new Instruction (InstructionType.IN_INC, AdressModeType.AM_R, RegisterType.RT_HL ) },
            {0x24, new Instruction (InstructionType.IN_INC, AdressModeType.AM_R, RegisterType.RT_H) },
            {0x25, new Instruction (InstructionType.IN_DEC, AdressModeType.AM_R, RegisterType.RT_H) },
            {0x26, new Instruction (InstructionType.IN_LD, AdressModeType.AM_R_D8, RegisterType.RT_H ) },
            {0x27, new Instruction (InstructionType.IN_DAA) },
            {0x28, new Instruction (InstructionType.IN_JR, AdressModeType.AM_D8, RegisterType.RT_NONE,RegisterType.RT_NONE,ConditionType.CT_Z) },
            {0x29, new Instruction (InstructionType.IN_ADD, AdressModeType.AM_R, RegisterType.RT_HL,RegisterType.RT_HL) },
            {0x2A, new Instruction (InstructionType.IN_LD, AdressModeType.AM_R_HLI, RegisterType.RT_A,RegisterType.RT_HL ) },
            {0x2B, new Instruction (InstructionType.IN_DEC, AdressModeType.AM_R,RegisterType.RT_HL ) },
            {0x2C, new Instruction (InstructionType.IN_INC, AdressModeType.AM_R, RegisterType.RT_L ) },
            {0x2D, new Instruction (InstructionType.IN_DEC, AdressModeType.AM_R, RegisterType.RT_L ) },
            {0x2E, new Instruction (InstructionType.IN_LD, AdressModeType.AM_R_D8, RegisterType.RT_L ) },
            {0x2F, new Instruction (InstructionType.IN_CPL ) },

            //0x3X - OK
            {0x30, new Instruction (InstructionType.IN_JR, AdressModeType.AM_D8, RegisterType.RT_NONE,RegisterType.RT_NONE,ConditionType.CT_NC) },
            {0x31, new Instruction (InstructionType.IN_LD, AdressModeType.AM_R_D16, RegisterType.RT_SP ) },
            {0x32, new Instruction (InstructionType.IN_LD, AdressModeType.AM_HLD_R, RegisterType.RT_HL, RegisterType.RT_A ) },
            {0x33, new Instruction (InstructionType.IN_INC, AdressModeType.AM_R, RegisterType.RT_SP) },
            {0x34, new Instruction (InstructionType.IN_INC, AdressModeType.AM_MR, RegisterType.RT_HL) },
            {0x35, new Instruction (InstructionType.IN_DEC, AdressModeType.AM_R, RegisterType.RT_HL ) },
            {0x36, new Instruction (InstructionType.IN_LD, AdressModeType.AM_MR_D8, RegisterType.RT_HL ) },
            {0x37, new Instruction (InstructionType.IN_SCF ) },
            {0x38, new Instruction (InstructionType.IN_JR, AdressModeType.AM_D8, RegisterType.RT_NONE,RegisterType.RT_NONE,ConditionType.CT_C) },
            {0x39, new Instruction (InstructionType.IN_ADD, AdressModeType.AM_R_R, RegisterType.RT_HL,RegisterType.RT_SP) },
            {0x3A, new Instruction (InstructionType.IN_LD, AdressModeType.AM_R_HLD, RegisterType.RT_A,RegisterType.RT_HL ) },
            {0x3B, new Instruction (InstructionType.IN_DEC, AdressModeType.AM_R, RegisterType.RT_SP ) },
            {0x3C, new Instruction (InstructionType.IN_INC, AdressModeType.AM_R, RegisterType.RT_A ) },
            {0x3D, new Instruction (InstructionType.IN_DEC, AdressModeType.AM_R, RegisterType.RT_A) },
            {0x3E, new Instruction (InstructionType.IN_LD, AdressModeType.AM_R_D8, RegisterType.RT_A ) },
            {0x3F, new Instruction (InstructionType.IN_SCF ) },

            //0x4X - OK
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

            //0x5X - OK
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

            //0x6X - OK
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

            //0x7X - OK
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

            //0x8X - OK
            {0x80, new Instruction (InstructionType.IN_ADD, AdressModeType.AM_R_R, RegisterType.RT_A,RegisterType.RT_B ) },
            {0x81, new Instruction (InstructionType.IN_ADD, AdressModeType.AM_R_R, RegisterType.RT_A,RegisterType.RT_C ) },
            {0x82, new Instruction (InstructionType.IN_ADD, AdressModeType.AM_R_R, RegisterType.RT_A,RegisterType.RT_D ) },
            {0x83, new Instruction (InstructionType.IN_ADD, AdressModeType.AM_R_R, RegisterType.RT_A,RegisterType.RT_E ) },
            {0x84, new Instruction (InstructionType.IN_ADD, AdressModeType.AM_R_R, RegisterType.RT_A,RegisterType.RT_H ) },
            {0x85, new Instruction (InstructionType.IN_ADD, AdressModeType.AM_R_R, RegisterType.RT_A,RegisterType.RT_L ) },
            {0x86, new Instruction (InstructionType.IN_ADD, AdressModeType.AM_R_MR, RegisterType.RT_A,RegisterType.RT_HL ) },
            {0x87, new Instruction (InstructionType.IN_ADD, AdressModeType.AM_R_R, RegisterType.RT_A,RegisterType.RT_A ) },
            {0x88, new Instruction (InstructionType.IN_ADC, AdressModeType.AM_R_R, RegisterType.RT_A,RegisterType.RT_B ) },
            {0x89, new Instruction (InstructionType.IN_ADC, AdressModeType.AM_R_R, RegisterType.RT_A,RegisterType.RT_C ) },
            {0x8A, new Instruction (InstructionType.IN_ADC, AdressModeType.AM_R_R, RegisterType.RT_A,RegisterType.RT_D ) },
            {0x8B, new Instruction (InstructionType.IN_ADC, AdressModeType.AM_R_R, RegisterType.RT_A,RegisterType.RT_E ) },
            {0x8C, new Instruction (InstructionType.IN_ADC, AdressModeType.AM_R_R, RegisterType.RT_A,RegisterType.RT_H ) },
            {0x8D, new Instruction (InstructionType.IN_ADC, AdressModeType.AM_R_R, RegisterType.RT_A,RegisterType.RT_L ) },
            {0x8E, new Instruction (InstructionType.IN_ADC, AdressModeType.AM_R_MR, RegisterType.RT_A,RegisterType.RT_HL ) },
            {0x8F, new Instruction (InstructionType.IN_ADC, AdressModeType.AM_R_R, RegisterType.RT_A,RegisterType.RT_A ) },

            //0x9X - OK
            {0x90, new Instruction (InstructionType.IN_SUB, AdressModeType.AM_R_R, RegisterType.RT_A,RegisterType.RT_B ) },
            {0x91, new Instruction (InstructionType.IN_SUB, AdressModeType.AM_R_R, RegisterType.RT_A,RegisterType.RT_C ) },
            {0x92, new Instruction (InstructionType.IN_SUB, AdressModeType.AM_R_R, RegisterType.RT_A,RegisterType.RT_D ) },
            {0x93, new Instruction (InstructionType.IN_SUB, AdressModeType.AM_R_R, RegisterType.RT_A,RegisterType.RT_E ) },
            {0x94, new Instruction (InstructionType.IN_SUB, AdressModeType.AM_R_R, RegisterType.RT_A,RegisterType.RT_H ) },
            {0x95, new Instruction (InstructionType.IN_SUB, AdressModeType.AM_R_R, RegisterType.RT_A,RegisterType.RT_L ) },
            {0x96, new Instruction (InstructionType.IN_SUB, AdressModeType.AM_R_MR, RegisterType.RT_A,RegisterType.RT_HL ) },
            {0x97, new Instruction (InstructionType.IN_SUB, AdressModeType.AM_R_R, RegisterType.RT_A,RegisterType.RT_A ) },
            {0x98, new Instruction (InstructionType.IN_SBC, AdressModeType.AM_R_R, RegisterType.RT_A,RegisterType.RT_B ) },
            {0x99, new Instruction (InstructionType.IN_SBC, AdressModeType.AM_R_R, RegisterType.RT_A,RegisterType.RT_C ) },
            {0x9A, new Instruction (InstructionType.IN_SBC, AdressModeType.AM_R_R, RegisterType.RT_A,RegisterType.RT_D ) },
            {0x9B, new Instruction (InstructionType.IN_SBC, AdressModeType.AM_R_R, RegisterType.RT_A,RegisterType.RT_E ) },
            {0x9C, new Instruction (InstructionType.IN_SBC, AdressModeType.AM_R_R, RegisterType.RT_A,RegisterType.RT_H ) },
            {0x9D, new Instruction (InstructionType.IN_SBC, AdressModeType.AM_R_R, RegisterType.RT_A,RegisterType.RT_L ) },
            {0x9E, new Instruction (InstructionType.IN_SBC, AdressModeType.AM_R_MR, RegisterType.RT_A,RegisterType.RT_HL ) },
            {0x9F, new Instruction (InstructionType.IN_SBC, AdressModeType.AM_R_R, RegisterType.RT_A,RegisterType.RT_A) },

            //0xAX - OK
            {0xA0, new Instruction (InstructionType.IN_AND, AdressModeType.AM_R_R, RegisterType.RT_A, RegisterType.RT_B ) },
            {0xA1, new Instruction (InstructionType.IN_AND, AdressModeType.AM_R_R, RegisterType.RT_A, RegisterType.RT_C  ) },
            {0xA2, new Instruction (InstructionType.IN_AND, AdressModeType.AM_R_R, RegisterType.RT_A, RegisterType.RT_D ) },
            {0xA4, new Instruction (InstructionType.IN_AND, AdressModeType.AM_R_R, RegisterType.RT_A, RegisterType.RT_E  ) },
            {0xA3, new Instruction (InstructionType.IN_AND, AdressModeType.AM_R_R, RegisterType.RT_A, RegisterType.RT_H  ) },
            {0xA5, new Instruction (InstructionType.IN_AND, AdressModeType.AM_R_R, RegisterType.RT_A, RegisterType.RT_L ) },
            {0xA6, new Instruction (InstructionType.IN_AND, AdressModeType.AM_R_MR, RegisterType.RT_A, RegisterType.RT_HL  ) },
            {0xA7, new Instruction (InstructionType.IN_AND, AdressModeType.AM_R_R, RegisterType.RT_A, RegisterType.RT_A ) },
            {0xA8, new Instruction (InstructionType.IN_XOR, AdressModeType.AM_R_R, RegisterType.RT_A, RegisterType.RT_B ) },
            {0xA9, new Instruction (InstructionType.IN_XOR, AdressModeType.AM_R_R, RegisterType.RT_A, RegisterType.RT_C ) },
            {0xAA, new Instruction (InstructionType.IN_XOR, AdressModeType.AM_R_R, RegisterType.RT_A, RegisterType.RT_D ) },
            {0xAB, new Instruction (InstructionType.IN_XOR, AdressModeType.AM_R_R, RegisterType.RT_A, RegisterType.RT_E ) },
            {0xAC, new Instruction (InstructionType.IN_XOR, AdressModeType.AM_R_R, RegisterType.RT_A, RegisterType.RT_H ) },
            {0xAD, new Instruction (InstructionType.IN_XOR, AdressModeType.AM_R_R, RegisterType.RT_A, RegisterType.RT_L ) },
            {0xAE, new Instruction (InstructionType.IN_XOR, AdressModeType.AM_R_MR, RegisterType.RT_A, RegisterType.RT_HL ) },
            {0xAF, new Instruction (InstructionType.IN_XOR, AdressModeType.AM_R_R, RegisterType.RT_A, RegisterType.RT_A) },

            //0xBX - OK
            {0xB0, new Instruction (InstructionType.IN_OR, AdressModeType.AM_R_R, RegisterType.RT_A, RegisterType.RT_B ) },
            {0xB1, new Instruction (InstructionType.IN_OR, AdressModeType.AM_R_R, RegisterType.RT_A, RegisterType.RT_C ) },
            {0xB2, new Instruction (InstructionType.IN_OR, AdressModeType.AM_R_R, RegisterType.RT_A, RegisterType.RT_D ) },
            {0xB3, new Instruction (InstructionType.IN_OR, AdressModeType.AM_R_R, RegisterType.RT_A, RegisterType.RT_E ) },
            {0xB4, new Instruction (InstructionType.IN_OR, AdressModeType.AM_R_R, RegisterType.RT_A, RegisterType.RT_H ) },
            {0xB5, new Instruction (InstructionType.IN_OR, AdressModeType.AM_R_R, RegisterType.RT_A, RegisterType.RT_L ) },
            {0xB6, new Instruction (InstructionType.IN_OR, AdressModeType.AM_R_MR, RegisterType.RT_A, RegisterType.RT_HL ) },
            {0xB7, new Instruction (InstructionType.IN_OR, AdressModeType.AM_R_R, RegisterType.RT_A, RegisterType.RT_A ) },
            {0xB8, new Instruction (InstructionType.IN_CP, AdressModeType.AM_R_R, RegisterType.RT_A, RegisterType.RT_B ) },
            {0xB9, new Instruction (InstructionType.IN_CP, AdressModeType.AM_R_R, RegisterType.RT_A, RegisterType.RT_C ) },
            {0xBA, new Instruction (InstructionType.IN_CP, AdressModeType.AM_R_R, RegisterType.RT_A, RegisterType.RT_D ) },
            {0xBB, new Instruction (InstructionType.IN_CP, AdressModeType.AM_R_R, RegisterType.RT_A, RegisterType.RT_E ) },
            {0xBC, new Instruction (InstructionType.IN_CP, AdressModeType.AM_R_R, RegisterType.RT_A, RegisterType.RT_H ) },
            {0xBD, new Instruction (InstructionType.IN_CP, AdressModeType.AM_R_R, RegisterType.RT_A, RegisterType.RT_L ) },
            {0xBE, new Instruction (InstructionType.IN_CP, AdressModeType.AM_R_MR, RegisterType.RT_A, RegisterType.RT_HL ) },
            {0xBF, new Instruction (InstructionType.IN_CP, AdressModeType.AM_R_R, RegisterType.RT_A, RegisterType.RT_A) },

            //0xCX - OK
            {0xC0, new Instruction (InstructionType.IN_RET,AdressModeType.AM_IMP,RegisterType.RT_NONE,RegisterType.RT_NONE,ConditionType.CT_NZ)},
            {0xC1, new Instruction (InstructionType.IN_POP,AdressModeType.AM_R,RegisterType.RT_BC) },
            {0xC2, new Instruction (InstructionType.IN_JP,AdressModeType.AM_D16,RegisterType.RT_NONE,RegisterType.RT_NONE,ConditionType.CT_NZ) },
            {0xC3, new Instruction (InstructionType.IN_JP,AdressModeType.AM_D16) },
            {0xC4, new Instruction (InstructionType.IN_CALL,AdressModeType.AM_D16,RegisterType.RT_NONE,RegisterType.RT_NONE,ConditionType.CT_NZ) },
            {0xC5, new Instruction (InstructionType.IN_PUSH,AdressModeType.AM_R,RegisterType.RT_BC) },
            {0xC6, new Instruction (InstructionType.IN_ADD,AdressModeType.AM_R_D8,RegisterType.RT_A) },
            {0xC7, new Instruction (InstructionType.IN_RST,AdressModeType.AM_IMP,RegisterType.RT_NONE,RegisterType.RT_NONE,ConditionType.CT_NONE,0x00) },
            {0xC8, new Instruction (InstructionType.IN_RET,AdressModeType.AM_IMP,RegisterType.RT_NONE,RegisterType.RT_NONE,ConditionType.CT_Z) },
            {0xC9, new Instruction (InstructionType.IN_RET) },
            {0xCA, new Instruction (InstructionType.IN_JP,AdressModeType.AM_D16,RegisterType.RT_NONE,RegisterType.RT_NONE,ConditionType.CT_Z) },
            {0xCB, new Instruction (InstructionType.IN_CB,AdressModeType.AM_D8) },
            {0xCC, new Instruction (InstructionType.IN_CALL,AdressModeType.AM_D16,RegisterType.RT_NONE,RegisterType.RT_NONE,ConditionType.CT_Z) },
            {0xCD, new Instruction (InstructionType.IN_CALL,AdressModeType.AM_D16) },
            {0xCE, new Instruction (InstructionType.IN_ADC,AdressModeType.AM_R_D8,RegisterType.RT_A) },
            {0xCF, new Instruction (InstructionType.IN_RST,AdressModeType.AM_IMP,RegisterType.RT_NONE,RegisterType.RT_NONE,ConditionType.CT_NONE,0x08) },

            //0xDx
            {0xD0, new Instruction (InstructionType.IN_RET,AdressModeType.AM_IMP,RegisterType.RT_NONE,RegisterType.RT_NONE,ConditionType.CT_NC)},
            {0xD1, new Instruction (InstructionType.IN_POP,AdressModeType.AM_R,RegisterType.RT_DE)},
            {0xD2, new Instruction (InstructionType.IN_JP,AdressModeType.AM_D16,RegisterType.RT_NONE,RegisterType.RT_NONE,ConditionType.CT_NC)},
            {0xD4, new Instruction (InstructionType.IN_CALL,AdressModeType.AM_D16,RegisterType.RT_NONE,RegisterType.RT_NONE,ConditionType.CT_NC)},
            {0xD5, new Instruction (InstructionType.IN_PUSH,AdressModeType.AM_R,RegisterType.RT_DE)},
            {0xD6, new Instruction (InstructionType.IN_SUB,AdressModeType.AM_R_D8,RegisterType.RT_A)},
            {0xD7, new Instruction (InstructionType.IN_RST,AdressModeType.AM_IMP,RegisterType.RT_NONE,RegisterType.RT_NONE,ConditionType.CT_NONE,0x10)},
            {0xD8, new Instruction (InstructionType.IN_RET,AdressModeType.AM_IMP,RegisterType.RT_NONE,RegisterType.RT_NONE,ConditionType.CT_C)},
            {0xD9, new Instruction (InstructionType.IN_RETI)},
            {0xDA, new Instruction (InstructionType.IN_JP,AdressModeType.AM_D16,RegisterType.RT_NONE,RegisterType.RT_NONE,ConditionType.CT_C)},
            {0xDC, new Instruction (InstructionType.IN_CALL,AdressModeType.AM_D16,RegisterType.RT_NONE,RegisterType.RT_NONE,ConditionType.CT_C)},
            {0xDE, new Instruction (InstructionType.IN_SBC,AdressModeType.AM_R_D8,RegisterType.RT_A)},
            {0xDF, new Instruction (InstructionType.IN_RST,AdressModeType.AM_IMP,RegisterType.RT_NONE,RegisterType.RT_NONE,ConditionType.CT_NONE,0x18)},

            //0xEX
            {0xE0, new Instruction (InstructionType.IN_LDH, AdressModeType.AM_A8_R, RegisterType.RT_NONE,RegisterType.RT_A ) },
            {0xE1, new Instruction (InstructionType.IN_POP, AdressModeType.AM_R, RegisterType.RT_HL ) },
            {0xE2, new Instruction (InstructionType.IN_LD, AdressModeType.AM_MR_R, RegisterType.RT_C,RegisterType.RT_A ) },
            {0xE5, new Instruction (InstructionType.IN_PUSH, AdressModeType.AM_R, RegisterType.RT_HL ) },
            {0xE6, new Instruction (InstructionType.IN_AND, AdressModeType.AM_R_D8, RegisterType.RT_A) },
            {0xE7, new Instruction (InstructionType.IN_RST, AdressModeType.AM_IMP, RegisterType.RT_NONE,RegisterType.RT_NONE,ConditionType.CT_NONE,0x20 ) },
            {0xE8, new Instruction (InstructionType.IN_ADD, AdressModeType.AM_R_D8, RegisterType.RT_SP ) },
            {0xE9, new Instruction (InstructionType.IN_JP, AdressModeType.AM_R, RegisterType.RT_HL ) },
            {0xEA, new Instruction (InstructionType.IN_LD, AdressModeType.AM_A16_R, RegisterType.RT_NONE,RegisterType.RT_A ) },
            {0xEE, new Instruction (InstructionType.IN_XOR, AdressModeType.AM_R_D8, RegisterType.RT_A ) },
            {0xEF, new Instruction (InstructionType.IN_RST, AdressModeType.AM_IMP, RegisterType.RT_NONE,RegisterType.RT_NONE,ConditionType.CT_NONE,0x28 ) },

            //0xFX
            {0xF0, new Instruction (InstructionType.IN_LDH,AdressModeType.AM_R_A8,RegisterType.RT_A)},
            {0xF1, new Instruction (InstructionType.IN_POP,AdressModeType.AM_R,RegisterType.RT_AF)},
            {0xF2, new Instruction (InstructionType.IN_LD,AdressModeType.AM_R_MR,RegisterType.RT_A,RegisterType.RT_C)},
            {0xF3, new Instruction (InstructionType.IN_DI)},
            {0xF5, new Instruction (InstructionType.IN_PUSH,AdressModeType.AM_R,RegisterType.RT_AF)},
            {0xF6, new Instruction (InstructionType.IN_OR, AdressModeType.AM_D8,RegisterType.RT_A)},
            {0xF7, new Instruction (InstructionType.IN_RST,AdressModeType.AM_IMP,RegisterType.RT_NONE,RegisterType.RT_NONE,ConditionType.CT_NONE,0x30)},
            {0xF8, new Instruction (InstructionType.IN_LD,AdressModeType.AM_HL_SPR,RegisterType.RT_HL,RegisterType.RT_SP)},
            {0xF9, new Instruction (InstructionType.IN_LD,AdressModeType.AM_R_R,RegisterType.RT_SP,RegisterType.RT_HL)},
            {0xFA, new Instruction (InstructionType.IN_LD,AdressModeType.AM_R_A16,RegisterType.RT_A)},
            {0xFB, new Instruction (InstructionType.IN_EI)},
            {0xFE, new Instruction (InstructionType.IN_CP,AdressModeType.AM_R_D8,RegisterType.RT_A)},
            {0xFF, new Instruction (InstructionType.IN_RST,AdressModeType.AM_IMP,RegisterType.RT_NONE,RegisterType.RT_NONE,ConditionType.CT_NONE,0x38)},
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

        public static string GetInstructionName(Instruction instruction)
        {
            if(instruction!=null)
            return _instructionsNames[(byte)instruction.Type];

            return "Uknown";
        }
    }
}
