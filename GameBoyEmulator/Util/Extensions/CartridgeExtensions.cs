using GameBoyEmulator.HardwareComponents.Cartridge;
using System.Text;

namespace GameBoyEmulator.Util.Extensions
{
    public static class CartridgeExtensions
    {
        public static string CartridgeInfo(this CartridgeBase cartridge)
        {
            StringBuilder msg = new StringBuilder();
            msg.Clear();
            msg.AppendLine($"Cartridge File name: {cartridge.FilePath}");
            msg.AppendLine("\n== Cartridge Loaded ==\n");
            msg.AppendLine(cartridge.State.RomHeader.ToString());
            msg.AppendLine($"Checksum: \t{cartridge.State.RomHeader.Checksum} - {cartridge.State.Data.ChecksumResult()}");
            return msg.ToString();
        }
    }
}
