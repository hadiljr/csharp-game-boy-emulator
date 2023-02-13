using GameBoyEmulator.HardwareComponents.CPU;
using GameBoyEmulator.HardwareComponents.DMA;
using GameBoyEmulator.HardwareComponents.Timer;
using System;

namespace GameBoyEmulator.HardwareComponents.IO
{
    public class IO : IIO
    {
        private readonly char[] serialCableData = new char[2];

        private readonly ICpu _cpu;
        private readonly ITimer _timer;
        private readonly IDma _dma;

        public IO(ICpu cpu, ITimer timer, IDma dma)
        {
            _cpu = cpu;
            _timer = timer;
            _dma = dma;
        }

        public byte Read(UInt16 address)
        {
            if (address == 0xFF01)
            {
                return (byte)serialCableData[0];
            }

            if (address == 0xFF02)
            {
                return (byte)serialCableData[1];
            }

            if (address >= 0xFF04 && address <= 0xFF07)
            {
                return _timer.Read(address);
            }

            if (address == 0xFF0F)
            {
                return _cpu.State.InterruptionFlags;

            }

            return 0;
        }

        public void Write(UInt16 address, byte value)
        {
            if (address == 0xFF01)
            {
                serialCableData[0] = (char)value;
                return;
            }

            if (address == 0xFF02)
            {
                serialCableData[1] = (char)value;
                return;
            }

            if (address >= 0xFF04 && address <= 0xFF07)
            {
                _timer.Write(address, value);
                return;
            }

            if (address == 0xFF0F)
            {
                _cpu.State.InterruptionFlags = value;
                return;

            }

            if (address == 0xFF46)
            {
                _dma.Start( value);
                return;

            }
        }
    }
}
