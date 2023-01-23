namespace GameBoyEmulator.Emulator.Core
{
    public struct Context
    {
        public RunType RunMode;
        public bool Paused;
        public bool Running;
        public ulong ticks;
    }
}
