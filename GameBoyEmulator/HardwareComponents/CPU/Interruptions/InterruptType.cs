namespace GameBoyEmulator.HardwareComponents.Interruptions
{
    public enum InterruptType
    {
        IT_VBLANK = 1,
        IT_LCD_STAT = 2,
        IT_TIMER = 4,
        IT_SERIAL = 8,
        IT_JOYPAD = 16,
    }
}
