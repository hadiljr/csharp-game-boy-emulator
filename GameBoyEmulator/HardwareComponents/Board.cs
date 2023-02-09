using GameBoyEmulator.HardwareComponents.Cartridge;
using GameBoyEmulator.HardwareComponents.CPU;
using GameBoyEmulator.HardwareComponents.DataBus;
using GameBoyEmulator.HardwareComponents.RamMemory;
using GameBoyEmulator.HardwareComponents.Timer;
using GameBoyEmulator.Util.Extensions;
using Serilog;
using System;

namespace GameBoyEmulator.HardwareComponents
{
    public class Board:IBoard
    {

        private readonly IRam ram = new Ram();
        private readonly ITimer timer = new Timer.Timer();

        private readonly IBus bus;
        private readonly ICpu cpu;

        public bool Running { get; set; }
        public bool Paused { get; set; }

        public ulong ticks;

        public Board()
        {
            bus = new Bus(ram);
            cpu = new Cpu(this,bus, timer);

            cpu.CiclingEvent += Cicles;
        }

        public void Run(CartridgeBase cartridge)
        {
            Running = true;
            
#if DEBUG
            Log.Debug(cartridge.CartridgeInfo());
#endif
            bus.InsertCartridge(cartridge);

            while (Running)
            {
                if (Paused)
                {
                    continue;
                }
                if (!cpu.Step())
                {
                    throw new Exception("CPU parou");
                }

                ticks++;
            }

#if DEBUG
            Console.Read();
#endif

        }

        public void Cicles(int cicles)
        {
            cicles *= 4;

            for (int i = 0; i < cicles; i++)
            {
               ticks++;
                timer.Tick();
            }
        }

        //public static void Cicles(int cicle)
        //{
        //    //throw new NotImplementedException();
        //    var cicling = cicle * 4;
        //    for (int i = 0; i < cicling; i++)
        //    {
        //        _context.ticks++;
        //        timer.Tick();
        //    }
        //}

    }
}
