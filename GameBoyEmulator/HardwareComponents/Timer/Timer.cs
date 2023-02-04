using GameBoyEmulator.HardwareComponents.CPU;
using GameBoyEmulator.HardwareComponents.Interruptions;
using GameBoyEmulator.Util.Extensions;
using System;

namespace GameBoyEmulator.HardwareComponents.Timer
{
    public class Timer : ITimer
    {
        private TimerState ctx = new TimerState();
        private ICpu _cpu;


        public void SetDiv(UInt16 value)
        {
            ctx.Div = value;
        }
        public void Init(ICpu cpu)
        {
            ctx.Div = 0xAC00;
            _cpu = cpu;
        }

        public void Tick()
        {
            var prevDiv = ctx.Div;

            ctx.Div++;

            var timerUpdate = false;

            switch (ctx.Tac & 0b11)
            {
                case 0b00:
                    timerUpdate = GetTimeUpdate(prevDiv, ctx.Div, 9);
                    break;
                case 0b01:
                    timerUpdate = GetTimeUpdate(prevDiv, ctx.Div, 3);
                    break;
                case 0b10:
                    timerUpdate = GetTimeUpdate(prevDiv, ctx.Div, 5);
                    break;
                case 0b11:
                    timerUpdate = GetTimeUpdate(prevDiv, ctx.Div, 7);
                    break;
            }

            if (timerUpdate && (ctx.Tac & (1 << 2)).ToBool())
            {
                ctx.Tima++;
                if (ctx.Tima == 0xFF)
                {
                    ctx.Tima = ctx.Tma;
                    _cpu.RequestInterrupts(InterruptType.IT_TIMER);
                }
            }
        }

        public void Write(UInt16 address, byte value)
        {
            switch (address)
            {
                case 0xFF04:
                    //DIV
                    ctx.Div = 0;
                    break;
                case 0xFF05:
                    //TIMA
                    ctx.Tima = value;
                    break;
                case 0xFF06:
                    //TMA
                    ctx.Tma = value;
                    break;
                case 0xFF07:
                    //TAC
                    ctx.Tac = value;
                    break;
            }
        }

        public byte Read(UInt16 address)
        {
            switch (address)
            {
                case 0xFF04:
                    return (byte)(ctx.Div >> 8);
                case 0xFF05:
                    return ctx.Tima;
                case 0xFF06:
                    return ctx.Tma;
                case 0xFF07:
                    return ctx.Tac;
                default:
                    break;
            }

            throw new Exception("Invalid addresss for timer");
        }

        private bool GetTimeUpdate(UInt16 preDiv, UInt16 div, int bitRotation)
        {
            bool preDivOp = (preDiv & (1 << bitRotation)).ToBool();
            bool divOp = !(ctx.Div & (1 << bitRotation)).ToBool();
            return preDivOp && divOp;
        }
    }
}
