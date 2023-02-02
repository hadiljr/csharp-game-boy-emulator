using GameBoyEmulator.HardwareComponents.CPU;
using GameBoyEmulator.HardwareComponents.DataBus;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameBoyEmulator.HardwareComponents.ProgramStack
{
    public static class Stack
    {
        public static void Push(byte data)
        {
            Cpu.State.Registers.SP--;
            Bus.Write(Cpu.State.Registers.SP, data);
        }

        public static void Push16(UInt16 data)
        {
            var first = (byte)((data >> 8) & 0xFF);
            var second = (byte)(data& 0xFF);
            Push(first);
            Push(second);
        }

        public static byte Pop()
        {
            return Bus.Read(Cpu.State.Registers.SP++);
        }

        public static UInt16 Pop16()
        {
            ushort lo = Pop();
            ushort hi = Pop();

            return (UInt16)((hi << 8) | lo);
        }
    }
}
