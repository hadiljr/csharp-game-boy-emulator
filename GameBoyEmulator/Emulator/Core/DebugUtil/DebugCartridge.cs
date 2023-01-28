using GameBoyEmulator.HardwareComponents.Cartridge;
using System.Text;
using System.Threading.Tasks;

namespace GameBoyEmulator.Emulator.Core.Debug
{
    internal class DebugCartridge : CartridgeBase
    {

        private StringBuilder msg = new StringBuilder();

        public string DebugMessage()
        {
            return msg.ToString();
        }

        public override void LoadCartridge(string file)
        {
            base.LoadCartridge(file);
            msg.Clear();
            msg.AppendLine($"Cartridge File name: {file}");
            msg.AppendLine("\n== Cartridge Loaded ==\n");
            msg.AppendLine(cartridgeModel.RomHeader.ToString());
            msg.AppendLine($"Checksum: \t{cartridgeModel.RomHeader.Checksum} - {cartridgeModel.Data.ChecksumResult()}");
        }
    }
}
