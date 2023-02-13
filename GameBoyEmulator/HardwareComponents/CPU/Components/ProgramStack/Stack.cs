using GameBoyEmulator.HardwareComponents.CPU;
using GameBoyEmulator.HardwareComponents.DataBus;
using System;

namespace GameBoyEmulator.HardwareComponents.ProgramStack
{
    public  class Stack :IStack
    {

        private readonly IBus _bus;
        private readonly ICpu _cpu;

        public Stack(IBus bus, ICpu cpu)
        {
            _bus = bus;
            _cpu = cpu;
        }

        public  void Push(byte data)
        {
            _cpu.State.Registers.SP--;
            _bus.Write(_cpu.State.Registers.SP, data);
        }

        public  void Push16(UInt16 data)
        {
            var first = (byte)((data >> 8) & 0xFF);
            var second = (byte)(data & 0xFF);
            Push(first);
            Push(second);
        }

        public  byte Pop()
        {
            return _bus.Read(_cpu.State.Registers.SP++);
        }

        public  UInt16 Pop16()
        {
            ushort lo = Pop();
            ushort hi = Pop();

            return (UInt16)((hi << 8) | lo);
        }
    }
}
