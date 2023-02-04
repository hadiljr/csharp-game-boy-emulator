using GameBoyEmulator.Util.Memory;
using System.IO;

namespace GameBoyEmulator.HardwareComponents.Cartridge
{
    public abstract class CartridgeBase : ICartridge
    {
        protected readonly CartridgeState state = new CartridgeState();
        protected string _filePath;

        public string FilePath { get { return _filePath; } }
        public CartridgeState State { get { return state; } }

        public CartridgeBase(string filePath)
        {
            _filePath = filePath;
            LoadCartridge();
        }

        private void LoadCartridge()
        {

            state.Data = new CartridgeMemoryStream();
            File.OpenRead(Path.GetFullPath(_filePath)).CopyTo(state.Data);

            state.RomSize = state.Data.GetRomSize();

            var header = state.Data.GetRomHeader();

            state.RomHeader = header;
        }

        public byte Read(ushort address)
        {    
            return state.Data.ReadAdress(address); 
        }

        public void Write(ushort adress, byte value)
        {
           // throw new NotImplementedException();
        }
    }
}
