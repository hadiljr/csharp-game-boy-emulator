using GameBoyEmulator.Util.Memory;
using System;
using System.IO;
using System.Threading.Tasks;

namespace GameBoyEmulator.HardwareComponents.Cartridge
{
    abstract class CartridgeBase : ICartridge
    {
        protected readonly CartridgeModel cartridgeModel = new CartridgeModel();

        public virtual async Task LoadCartridge(string file)
        {
            cartridgeModel.Data = new CartridgeMemoryStream();
            File.OpenRead(Path.GetFullPath(file)).CopyTo(cartridgeModel.Data);

            cartridgeModel.RomSize = cartridgeModel.Data.GetRomSize();
            cartridgeModel.RomHeader = await cartridgeModel.Data.GetRomHeader();
        }

        public async Task<byte> Read(ushort address)
        {
            return await cartridgeModel.Data.ReadAdress(address);
            
        }

        public void Write(ushort adress, byte value)
        {
            throw new NotImplementedException();
        }
    }
}
