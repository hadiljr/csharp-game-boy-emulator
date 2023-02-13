namespace GameBoyEmulator.HardwareComponents.DMA
{
    public interface IDma
    {

        void Start(byte value);
        void Tick();
        bool IsTrasfering();
    
    }
}
