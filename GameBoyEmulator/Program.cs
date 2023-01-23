using GameBoyEmulator.Emulator;
using System;

namespace GameBoyEmulator
{
    class Program
    {

        static void Main(string[] args)
        {
            var gbEmulator = new GbEmulator(args[0], Emulator.Core.RunType.DEBUG);
            gbEmulator.Run();
        }
    }
}
