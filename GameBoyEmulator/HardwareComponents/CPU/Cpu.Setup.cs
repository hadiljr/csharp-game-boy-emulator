using GameBoyEmulator.HardwareComponents.CPU.Processor;
using GameBoyEmulator.HardwareComponents.DataBus;
using GameBoyEmulator.HardwareComponents.ProgramStack;
using GameBoyEmulator.HardwareComponents.Timer;
using GameBoyEmulator.Util.Debuger;
using System;

namespace GameBoyEmulator.HardwareComponents.CPU
{
    public partial class Cpu : ICpu
    {

        public event Action<int> CiclingEvent;

        private readonly CpuState ctx = new CpuState();
        private readonly IBus _bus;
        private readonly ITimer _timer;
        private readonly IStack _stack;
        private readonly Board _board;
        private readonly CpuDebugger _cpuDebugger;

        private readonly ProcessorsList _processorsList;

        public Cpu(Board board, IBus bus, ITimer timer)
        {
            _board = board;
            _bus = bus;
            _bus.AttachCpu(this, timer);

            _timer = timer;
            _timer.Init(this);

            _stack = new Stack(_bus, this);

            _processorsList = new ProcessorsList(this, _bus);
            _cpuDebugger = new CpuDebugger(_bus);


            CpuInit();
        }

        private void CpuInit()
        {
            ctx.Registers.PC = 0x100;
            ctx.Registers.SP = 0xFFFE;
            ctx.Registers.A = 0x01;
            ctx.Registers.F = 0xB0;
            ctx.Registers.B = 0x00;
            ctx.Registers.C = 0x13;
            ctx.Registers.D = 0x00;
            ctx.Registers.E = 0xD8;
            ctx.Registers.H = 0x01;
            ctx.Registers.L = 0x4D;


            ctx.IeRegister = 0;
            ctx.InterruptionFlags = 0;
            ctx.InterruptionMasterEnabled = false;
            ctx.EnableIME = false;
            _timer.SetDiv(0xABCC);
        }

        public CpuState State
        {
            get
            {
                return ctx;
            }
        }

        public IStack GetStack()
        {
            return _stack;
        }

    }
}
