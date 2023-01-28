using GameBoyEmulator.Emulator.Core;
using GameBoyEmulator.Emulator.Core.Debug;
using GameBoyEmulator.HardwareComponents.Bus;
using GameBoyEmulator.HardwareComponents.Cartridge;
using GameBoyEmulator.HardwareComponents.CPU;
using GameBoyEmulator.HardwareComponents.PPU;
using GameBoyEmulator.HardwareComponents.Timer;
using System;
using System.Threading.Tasks;

namespace GameBoyEmulator.Emulator
{
    public class GbEmulator : IGbEmulator
    {
        private Context _context = new Context();

        
        private ICpu _cpu;
        private IPpu _ppu;
        private ITimer _timer;
        
        private DebugCartridge _cartridge;
        private string _cartridgeFile;

        public GbEmulator(string cartridgeFile, RunType runMode)
        {
            _context.RunMode = runMode;
            _cartridgeFile = cartridgeFile;
        }

        public void Run()
        {
            _context.Running = true;
            if (_context.RunMode.Equals(RunType.DEBUG))
            {
                RunDebugMode().Wait();
            }
        }

        private async Task RunDebugMode()
        {
            try
            {
                _cartridge = new DebugCartridge();
                _cartridge.LoadCartridge(_cartridgeFile);
                Console.WriteLine(_cartridge.DebugMessage());
                
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            BusInstance.SetCartridge(_cartridge);
            _cpu = new Cpu( this);
            _cpu.CpuInit();

            Console.WriteLine("== Instructions ==");
            while (_context.Running)
            {
                if (_context.Paused)
                {
                    continue;
                }

                if (!_cpu.CpuStep())
                {
                    throw new Exception("CPU parou");
                }

                _context.ticks++;

            }

            Console.Read();
        }

        public static void cicles(int number)
        {
            //throw new NotImplementedException();
        }

        public Context Context => _context;
    }
}
