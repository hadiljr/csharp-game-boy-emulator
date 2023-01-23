using System;
using System.Runtime.InteropServices;

namespace GameBoyEmulator.HardwareComponents.Cartridge.Constants
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
        public byte LicenseeCode;
        public byte Version;
        public byte Checksum;
        public byte GlobalChecksum;

        public override string ToString()
        {
            return  $"Title: \t\t{Title}\n"+
                    $"Type: \t\t{Type} ({RomTypes.NameCartridgeType(Type)})\n" +
                    $"ROM Size: \t{32 << RomSize} KB\n" +
                    $"RAM Size: \t{RamSize}\n" +
                    $"LIC Code: \t{LicenseeCode} ({LicenseeCodes.LicenseeName(LicenseeCode)})\n" +
                    $"ROM Version: \t{Version}";
        }
    }
}
