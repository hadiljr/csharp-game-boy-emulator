namespace GameBoyEmulator.HardwareComponents.CPU
{
    public interface ICpu
    {
        void CpuInit();
        bool CpuStep();
    }
}
