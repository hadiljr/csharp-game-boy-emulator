using GameBoyEmulator.HardwareComponents.Cartridge;
using System.Text;
using System.Threading.Tasks;

namespace GameBoyEmulator.Emulator.Core.Debug
{
    internal class DebugCartridge : CartridgeBase
    {

        private StringBuilder msg = new StringBuilder();

        public DebugCartridge(string filePath):base(filePath)
        {

        }

        public string DebugMessage()
        {
            return msg.ToString();
        }

        protected override void LoadCartridge()
        {
            base.LoadCartridge();
            msg.Clear();
            msg.AppendLine($"Cartridge File name: {_filePath}");
            msg.AppendLine("\n== Cartridge Loaded ==\n");
            msg.AppendLine(state.RomHeader.ToString());
            msg.AppendLine($"Checksum: \t{state.RomHeader.Checksum} - {state.Data.ChecksumResult()}");
        }

       
    }
}
