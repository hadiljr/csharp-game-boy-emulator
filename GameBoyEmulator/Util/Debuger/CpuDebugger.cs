using GameBoyEmulator.HardwareComponents.DataBus;
using System;

namespace GameBoyEmulator.Util.Debuger
{
    static class CpuDebugger
    {
        private static string msg;

        public static void Update()
        {
            if (Bus.Read(0xFF02) == 0x81)
            {
                char c = (char)Bus.Read(0xFF01);
                msg += c;
                Bus.Write(0xFF02, 0);
            }
        }

        public static void Print()
        {
            if (!string.IsNullOrWhiteSpace(msg))
            {
                Console.Write(msg);
               
            }
        }
    }
}
