using System;
using System.Collections.Generic;
using System.Text;

namespace GameBoyEmulator.HardwareComponents.DMA
{
    interface IDma
    {
        void Tick();
        bool IsTrasfering();
    
    }
}
