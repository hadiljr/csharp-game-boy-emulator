namespace GameBoyEmulator.HardwareComponents.CPU
{
    public static partial class Cpu
    {
        private static readonly CpuState ctx = new CpuState();

        public static void CpuInit()
        {
            ctx.Registers.PC = 0x100;
            ctx.Registers.A = 0x01;
        }

        public static CpuState State
        {
            get
            {
                return ctx;
            }
        }
        
    }
}
