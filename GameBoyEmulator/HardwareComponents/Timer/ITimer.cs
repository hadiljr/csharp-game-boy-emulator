namespace GameBoyEmulator.HardwareComponents.Timer
{
    public interface ITimer
    {
        void TimerInit();
        void TimerTick();
    }
}
