using GameBoyEmulator.HardwareComponents.DataBus;
using GameBoyEmulator.HardwareComponents.PPU;
using GameBoyEmulator.Util.Extensions;

namespace GameBoyEmulator.HardwareComponents.DMA
{
    public class Dma : IDma
    {
        private DmaState ctx = new DmaState();
        private readonly IBus _bus;
        private readonly IPpu _ppu;

        public Dma( IBus bus, IPpu ppu)
        {
            _bus = bus;
            _ppu = ppu;
        }

        public void Start(byte value)
        {
            ctx.active = true;
            ctx.@byte = 0;
            ctx.startDelay = 2;
            ctx.value = value;
        }

        public void Tick()
        {
            if (!ctx.active)
            {
                return;
            }

            if (ctx.startDelay.ToBool())
            {
                ctx.startDelay--;
                return;
            }

            _ppu.OamWrite(ctx.@byte, _bus.Read((byte)((ctx.value * 0x100) + ctx.@byte)));

            ctx.@byte++;
            ctx.active = ctx.@byte < 0xA0;

            if (!ctx.active)
            {
                // dma done
                // wait 2 seconds?
            }
        }

        public bool IsTrasfering()
        {
            return ctx.active;
        }
    }
}
