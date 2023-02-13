using GameBoyEmulator.Emulator;
using Serilog;
using System;
using System.IO;

namespace GameBoyEmulator
{
    class Program
    {

        static void Main(string[] args)
        {
            //try
            //{
            File.Delete("./debug.txt");
            CreateLogger();
            var gbEmulator = new GbEmulator(args[0], Emulator.Core.RunType.DEBUG);
            gbEmulator.Run();
            Log.CloseAndFlush();
            //}
            //catch(Exception ex)
            //{
            //    Log.Fatal(ex, ex.Message);
            //    Log.CloseAndFlush();
            //}


        }


        private static void CreateLogger()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console(outputTemplate: "{Message:lj}{NewLine}", restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information)
                .WriteTo.File("./debug.txt", outputTemplate: "{Message:lj}{NewLine}")
                .CreateLogger();
        }
    }
}
