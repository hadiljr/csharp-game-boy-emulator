using GameBoyEmulator.Emulator.Core;

namespace GameBoyEmulator.Emulator
{
    public interface IGbEmulator
    {
        Context Context { get; }

        void Run();

        void cicles(int number);
    }
}
