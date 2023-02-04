namespace GameBoyEmulator.HardwareComponents.DMA
{
    internal struct DmaState
    {
        public bool active;
        public byte @byte;
        public byte value;
        public byte startDelay;
    }
}
