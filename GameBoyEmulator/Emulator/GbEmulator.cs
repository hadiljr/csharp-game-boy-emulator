using GameBoyEmulator.Emulator.Core;
using GameBoyEmulator.Emulator.Core.Debug;
using GameBoyEmulator.HardwareComponents.DataBus;
using GameBoyEmulator.HardwareComponents.Cartridge;
using GameBoyEmulator.HardwareComponents.CPU;
using GameBoyEmulator.HardwareComponents.PPU;
using GameBoyEmulator.HardwareComponents.Timer;
using System;
using System.Threading.Tasks;
using GameBoyEmulator.HardwareComponents.RamMemory;

namespace GameBoyEmulator.Emulator
{
    public class GbEmulator : IGbEmulator
    {
        private static Context _context = new Context();

        private DebugCartridge _cartridge;
        private string _cartridgeFile;

        public GbEmulator(string cartridgeFile, RunType runMode)
        {
            _context.RunMode = runMode;
            _context.Running = true;
            _context.Paused = false;
            _context.ticks = 0;

            _cartridgeFile = cartridgeFile;

            Ram.Init();
        }

        public void Run()
        {
           
                _context.Running = true;
                if (_context.RunMode.Equals(RunType.DEBUG))
                {
                    RunDebugMode();
                }
           
        }

        private void RunDebugMode()
        {
           
                _cartridge = new DebugCartridge();
                _cartridge.LoadCartridge(_cartridgeFile);
                Console.WriteLine(_cartridge.DebugMessage());



                Bus.SetCartridge(_cartridge);
                Timer.Init();
                Cpu.CpuInit();


                Console.WriteLine("== Instructions ==");
                while (_context.Running)
                {
                    if (_context.Paused)
                    {
                        continue;
                    }

                    if (!Cpu.CpuStep())
                    {
                        throw new Exception("CPU parou");
                    }

                    _context.ticks++;

                }

                Console.Read();

            
        }

        public static void Cicles(int cicle)
        {
            //throw new NotImplementedException();
            var cicling = cicle * 4;
            for (int i = 0; i < cicling; i++)
            {
                _context.ticks++;
                Timer.Tick();
            }
        }

        public static Context Context => _context;
    }
}
