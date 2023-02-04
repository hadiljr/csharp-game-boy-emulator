using System.Runtime.InteropServices;

namespace GameBoyEmulator.HardwareComponents.RamMemory
{
    struct RamDefinition
    {
        //Work ram
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x2000)]
        public byte[] wram;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x80)]
        public byte[] hram;


        public void Init()
        {
            wram = new byte[0x2000];
            hram = new byte[0x2000];
        }
    }
}
