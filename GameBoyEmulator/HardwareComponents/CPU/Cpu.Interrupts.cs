using GameBoyEmulator.HardwareComponents.Interruptions;
using System;

namespace GameBoyEmulator.HardwareComponents.CPU
{
    public partial class Cpu
    {
        internal void HandleInterrupts()
        {
            var IntAndAddress = GetInterruptTypeAndAddress(ctx);
            HandleInterrupst(IntAndAddress.Item2, IntAndAddress.Item1);
            //_context.EnableIME = false;
        }

        public void RequestInterrupts(InterruptType interruptType)
        {
            ctx.InterruptionFlags = (byte)(ctx.InterruptionFlags | (int)interruptType);
        }

        private void HandleInterrupst(UInt16 address, InterruptType interruptType)
        {
            _stack.Push16(ctx.Registers.PC);
            ctx.Registers.PC = address;

            ctx.InterruptionFlags &= (byte)interruptType;
            ctx.Halted = false;
            ctx.InterruptionMasterEnabled = false;
        }

        private Tuple<InterruptType, byte> GetInterruptTypeAndAddress(CpuState ctx)
        {
            if (InterruptionCheck(ctx, InterruptType.IT_VBLANK))
            {
                return new Tuple<InterruptType, byte>(InterruptType.IT_VBLANK, 0x40);
            }
            else if (InterruptionCheck(ctx, InterruptType.IT_LCD_STAT))
            {
                return new Tuple<InterruptType, byte>(InterruptType.IT_LCD_STAT, 0x48);
            }
            else if (InterruptionCheck(ctx, InterruptType.IT_TIMER))
            {
                return new Tuple<InterruptType, byte>(InterruptType.IT_TIMER, 0x50);
            }
            else if (InterruptionCheck(ctx, InterruptType.IT_SERIAL))
            {
                return new Tuple<InterruptType, byte>(InterruptType.IT_SERIAL, 0x58);
            }
            else if (InterruptionCheck(ctx, InterruptType.IT_JOYPAD))
            {
                return new Tuple<InterruptType, byte>(InterruptType.IT_JOYPAD, 0x60);
            }

            throw new NotImplementedException("Interruption Not implemented");
        }

        private bool InterruptionCheck(CpuState ctx, InterruptType interruptType)
        {
            return (Convert.ToBoolean(ctx.InterruptionFlags & (int)interruptType) &&
                Convert.ToBoolean(ctx.InterruptionFlags & (int)interruptType));


        }
    }
}
