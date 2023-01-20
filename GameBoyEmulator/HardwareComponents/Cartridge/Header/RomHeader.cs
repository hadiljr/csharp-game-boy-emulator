using System;
using System.Runtime.InteropServices;

namespace GameBoyEmulator.HardwareComponents.Cartridge.Header
{
    public struct RomHeader
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] Entry;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x30)]
        public byte[] Logo;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string Title;

        public UInt16 NewLicenseeCode;

        public byte SgbFlag;
        public byte Type;
        public byte RomSize;
        public byte RamSize;
        public byte DestinationCode;
        public byte OldLicenseeCode;
        public byte Version;
        public byte Checksum;
        public byte GlobalChecksum;
    }
}
