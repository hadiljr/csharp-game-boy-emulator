using GameBoyEmulator.HardwareComponents.DataBus;
using System;

namespace GameBoyEmulator.Util.Debuger
{
    public class CpuDebugger
    {
        private string msg;
        private readonly IBus _bus;

        public CpuDebugger(IBus bus)
        {
            _bus = bus;
        }

        public void Update()
        {
            if (_bus.Read(0xFF02) == 0x81)
            {
                char c = (char)_bus.Read(0xFF01);
                msg += c;
                _bus.Write(0xFF02, 0);
            }
        }

        public void Print()
        {
            if (!string.IsNullOrWhiteSpace(msg))
            {
                Console.Write(msg);

            }
        }
    }
}
