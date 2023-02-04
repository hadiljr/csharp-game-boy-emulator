using GameBoyEmulator.Emulator;
using Serilog;
using System;

namespace GameBoyEmulator
{
    class Program
    {

        static void Main(string[] args)
        {
            try
            {
                CreateLogger();
                var gbEmulator = new GbEmulator(args[0], Emulator.Core.RunType.DEBUG);
                gbEmulator.Run();
                Log.CloseAndFlush();
            }
            catch(Exception ex)
            {
                Log.Fatal(ex, ex.Message);
                Log.CloseAndFlush();
            }


        }


        private static void CreateLogger()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .CreateLogger();
        }
    }
}
