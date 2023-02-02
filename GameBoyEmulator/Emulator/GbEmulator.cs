using GameBoyEmulator.Emulator.Core;
using GameBoyEmulator.Emulator.Core.Debug;
using GameBoyEmulator.HardwareComponents.DataBus;
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
            try
            {
                _context.Running = true;
                if (_context.RunMode.Equals(RunType.DEBUG))
                {
                    RunDebugMode();
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void RunDebugMode()
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

            Bus.SetCartridge(_cartridge);
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

        public static void cicles(int number)
        {
            //throw new NotImplementedException();
        }

        public Context Context => _context;
    }
}
