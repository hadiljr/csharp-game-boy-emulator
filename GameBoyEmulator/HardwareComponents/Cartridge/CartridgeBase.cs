using GameBoyEmulator.Util.Memory;
using System;
using System.IO;
using System.Threading.Tasks;

namespace GameBoyEmulator.HardwareComponents.Cartridge
{
    abstract class CartridgeBase : ICartridge
    {
        protected readonly CartridgeModel cartridgeModel = new CartridgeModel();

        public virtual void LoadCartridge(string file)
        {
            cartridgeModel.Data = new CartridgeMemoryStream();
            File.OpenRead(Path.GetFullPath(file)).CopyTo(cartridgeModel.Data);

            cartridgeModel.RomSize = cartridgeModel.Data.GetRomSize();

            var header = cartridgeModel.Data.GetRomHeader();

            cartridgeModel.RomHeader = header;
        }

        public byte Read(ushort address)
        {    
            return cartridgeModel.Data.ReadAdress(address); 
        }

        public void Write(ushort adress, byte value)
        {
            throw new NotImplementedException();
        }
    }
}
