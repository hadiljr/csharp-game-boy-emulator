using GameBoyEmulator.HardwareComponents.Cartridge.Constants;
using GameBoyEmulator.Util.Memory;
using System;
using System.Runtime.InteropServices;

namespace GameBoyEmulator.HardwareComponents.Cartridge
{
    public class CartridgeState
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
        public string Filename;

        public UInt32 RomSize;

        public RomHeader RomHeader;

        public CartridgeMemoryStream Data;
    }
}
