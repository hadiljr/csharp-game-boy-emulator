using GameBoyEmulator.Emulator.Core;
using GameBoyEmulator.Emulator.Core.Debug;
using GameBoyEmulator.HardwareComponents;

namespace GameBoyEmulator.Emulator
{
    public class GbEmulator : IGbEmulator
    {
        private static Context _context = new Context();

        private DebugCartridge _cartridge;
        private Board _board;
        private string _cartridgeFile;

        public GbEmulator(string cartridgeFile, RunType runMode)
        {
            _context.RunMode = runMode;
            _context.Running = true;
            _context.Paused = false;
            _context.ticks = 0;

            _cartridgeFile = cartridgeFile;


        }

        public void Run()
        {

            _context.Running = true;
            _board = new Board();
            _cartridge = new DebugCartridge(_cartridgeFile);
            _board.Run(_cartridge);

        }

        //public static void Cicles(int cicle)
        //{
        //    //throw new NotImplementedException();
        //    var cicling = cicle * 4;
        //    for (int i = 0; i < cicling; i++)
        //    {
        //        _context.ticks++;
        //        Timer.Tick();
        //    }
        //}

        //public static Context Context => _context;
    }
}
