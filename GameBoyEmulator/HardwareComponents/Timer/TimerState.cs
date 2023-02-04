using System;

namespace GameBoyEmulator.HardwareComponents.Timer
{
    public struct TimerState
    {
        public UInt16 Div;
        public byte Tima;
        public byte Tma;
        public byte Tac;
    }
}
