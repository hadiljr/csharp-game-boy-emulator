using GameBoyEmulator.Emulator;
using GameBoyEmulator.HardwareComponents.CPU.Instructions;
using GameBoyEmulator.HardwareComponents.DataBus;
using GameBoyEmulator.HardwareComponents.ProgramStack;
using GameBoyEmulator.Util.Bit;
using GameBoyEmulator.Util.Extensions;
using Serilog;
using System;

namespace GameBoyEmulator.HardwareComponents.CPU.Processor
{
    public class Processors
    {
        private readonly ICpu _cpu;
        private readonly IBus _bus;



        public Processors(ICpu cpu, IBus bus)
        {
            _cpu = cpu;
            _bus = bus;

        }

        public void ProcessIN_NONE()
        {
            throw new Exception("INVALID INSTRUCTION!");
        }

        public void ProcessIN_NOP()
        {
            //do nothing
        }

        public void ProcessIN_DI()
        {
            _cpu.State.InterruptionMasterEnabled = false;
        }

        public void ProcessIN_LD()
        {
            if (_cpu.State.DestinationIsMemory)
            {
                //16 bit register
                if (_cpu.State.CurrentInstruction.Register2!=null && Is16BitRegisterType(_cpu.State.CurrentInstruction.Register2.Value))
                {

                    _cpu.CallCicles(1);
                    _bus.Write16(_cpu.State.MemoryDestination, _cpu.State.FetchedData);

                }
                else
                {
                    _bus.Write(_cpu.State.MemoryDestination, (byte)_cpu.State.FetchedData);
                }
                _cpu.CallCicles(1);
                return;
            }

            if (_cpu.State.CurrentInstruction.Mode == AdressModeType.AM_HL_SPR)
            {
                bool hflag = (_cpu.ReadRegister(_cpu.State.CurrentInstruction.Register2.Value) & 0xF) + (_cpu.State.FetchedData & 0xF) >= 0x10;
                bool cflag = (_cpu.ReadRegister(_cpu.State.CurrentInstruction.Register2.Value) & 0xFF) + (_cpu.State.FetchedData & 0xFF) >= 0x100;

                SetCpuFlags(BIT_POSITION.OFF, BIT_POSITION.OFF, hflag.ToBitPosition(), cflag.ToBitPosition());

                var reg2Data = _cpu.ReadRegister(_cpu.State.CurrentInstruction.Register2.Value);
                var data = (sbyte)_cpu.State.FetchedData;
                _cpu.SetRegister(_cpu.State.CurrentInstruction.Register1.Value, (UInt16)(reg2Data + data));

                return;
            }

            _cpu.SetRegister(_cpu.State.CurrentInstruction.Register1.Value, _cpu.State.FetchedData);
        }

        public void ProcessIN_LDH()
        {
            if (_cpu.State.CurrentInstruction.Register1.Value == RegisterType.RT_A)
            {

                var data = _bus.Read((UInt16)(0xFF00 | _cpu.State.FetchedData));
                _cpu.SetRegister(_cpu.State.CurrentInstruction.Register1.Value, data);
            }
            else
            {
                _bus.Write(_cpu.State.MemoryDestination, _cpu.State.Registers.A);
            }

            _cpu.CallCicles(1);
        }

        public void ProcessIN_XOR()
        {
            _cpu.State.Registers.A ^= Convert.ToByte(_cpu.State.FetchedData & 0xFF);
            BIT_POSITION z = _cpu.State.Registers.A == 0 ? BIT_POSITION.ON : BIT_POSITION.OFF;
            SetCpuFlags(z, BIT_POSITION.OFF, BIT_POSITION.OFF, BIT_POSITION.OFF);
        }

        public void ProcessIN_JP()
        {
            GotoAddress(_cpu.State.FetchedData, false);
        }

        public void ProcessIN_JR()
        {
            var data = (sbyte)(_cpu.State.FetchedData & 0xFF);
            UInt16 adress = (UInt16)(_cpu.State.Registers.PC + data);
            GotoAddress(adress, false);
        }

        public void ProcessIN_CALL()
        {
            GotoAddress(_cpu.State.FetchedData, true);
        }

        public void ProcessIN_RST()
        {
            GotoAddress(_cpu.State.CurrentInstruction.Parameter.Value, true);
        }

        public void ProcessIN_RET()
        {
            if (_cpu.State.CurrentInstruction.Condition != ConditionType.CT_NONE)
            {
                _cpu.CallCicles(1);
            }

            if (CheckCondition())
            {
                UInt16 lo = _cpu.GetStack().Pop();
                _cpu.CallCicles(1);
                UInt16 hi = _cpu.GetStack().Pop();
                _cpu.CallCicles(1);

                UInt16 res = (UInt16)((hi << 8) | lo);
                _cpu.State.Registers.PC = res;
                _cpu.CallCicles(1);
            }
        }

        public void ProcessIN_RETI()
        {
            _cpu.State.InterruptionMasterEnabled = true;
            ProcessIN_RET();
        }

        public void ProcessIN_POP()
        {
            UInt16 lo = _cpu.GetStack().Pop();
            _cpu.CallCicles(1);
            UInt16 hi = _cpu.GetStack().Pop();
            _cpu.CallCicles(1);

            UInt16 res = (UInt16)((hi << 8) | lo);

            _cpu.SetRegister(_cpu.State.CurrentInstruction.Register1.Value, res);

            if (_cpu.State.CurrentInstruction.Register1.Value == RegisterType.RT_AF)
            {
                _cpu.SetRegister(_cpu.State.CurrentInstruction.Register1.Value, (UInt16)(res & 0xFFF0));
            }

        }

        public void ProcessIN_PUSH()
        {

            byte hi = (byte)((_cpu.ReadRegister(_cpu.State.CurrentInstruction.Register1.Value) >> 8) & 0xFF);
            _cpu.CallCicles(1);
            _cpu.GetStack().Push(hi);

            byte lo = (byte)(_cpu.ReadRegister(_cpu.State.CurrentInstruction.Register1.Value) & 0xFF);
            _cpu.CallCicles(1);
            _cpu.GetStack().Push(lo);

            _cpu.CallCicles(1);
        }

        public void ProcessIN_INC()
        {
            UInt16 value = (UInt16)(_cpu.ReadRegister(_cpu.State.CurrentInstruction.Register1.Value) + 1);

            if (Is16BitRegisterType(_cpu.State.CurrentInstruction.Register1.Value))
            {
                _cpu.CallCicles(1);
            }

            if (_cpu.State.CurrentInstruction.Register1.Value == RegisterType.RT_HL &&
                _cpu.State.CurrentInstruction.Mode == AdressModeType.AM_MR)
            {
                value = (UInt16)(_bus.Read(_cpu.ReadRegister(RegisterType.RT_HL)) + 1);
                value &= 0xFF;
                _bus.Write(_cpu.ReadRegister(RegisterType.RT_HL), (byte)value);
            }
            else
            {
                _cpu.SetRegister(_cpu.State.CurrentInstruction.Register1.Value, value);
                value = _cpu.ReadRegister(_cpu.State.CurrentInstruction.Register1.Value);
            }

            if ((_cpu.State.CurrentOpcode & 0x03) == 0x03) return;

            SetCpuFlags((value == 0).ToBitPosition(), BIT_POSITION.OFF, ((value & 0x0F) == 0).ToBitPosition(), BIT_POSITION.SAME);
        }

        public void ProcessIN_DEC()
        {
            UInt16 value = (UInt16)(_cpu.ReadRegister(_cpu.State.CurrentInstruction.Register1.Value) - 1);

            if (Is16BitRegisterType(_cpu.State.CurrentInstruction.Register1.Value))
            {
                _cpu.CallCicles(1);
            }

            if (_cpu.State.CurrentInstruction.Register1.Value == RegisterType.RT_HL &&
               _cpu.State.CurrentInstruction.Mode == AdressModeType.AM_MR)
            {
                value = (UInt16)(_bus.Read(_cpu.ReadRegister(RegisterType.RT_HL)) - 1);
                _bus.Write(_cpu.ReadRegister(RegisterType.RT_HL), (byte)value);
            }
            else
            {
                _cpu.SetRegister(_cpu.State.CurrentInstruction.Register1.Value, value);
                value = _cpu.ReadRegister(_cpu.State.CurrentInstruction.Register1.Value);
            }

            if ((_cpu.State.CurrentOpcode & 0x0B) == 0x0B) return;

            SetCpuFlags((value == 0).ToBitPosition(), BIT_POSITION.ON, ((value & 0x0F) == 0x0F).ToBitPosition(), BIT_POSITION.SAME);
        }

        public void ProcessIN_SUB()
        {
            UInt16 value = (UInt16)(_cpu.ReadRegister(_cpu.State.CurrentInstruction.Register1.Value) - _cpu.State.FetchedData);
            BIT_POSITION z = (value == 0).ToBitPosition();
            BIT_POSITION h = (((_cpu.ReadRegister(_cpu.State.CurrentInstruction.Register1.Value) & 0xF) - (_cpu.State.FetchedData & 0xF)) < 0).ToBitPosition();
            BIT_POSITION c = ((_cpu.ReadRegister(_cpu.State.CurrentInstruction.Register1.Value) - _cpu.State.FetchedData) < 0).ToBitPosition();

            _cpu.SetRegister(_cpu.State.CurrentInstruction.Register1.Value, value);
            SetCpuFlags(z, BIT_POSITION.ON, h, c);
        }

        public void ProcessIN_SBC()
        {
            byte value = (byte)(_cpu.State.FetchedData + Convert.ToUInt16(_cpu.State.CFlag()));
            BIT_POSITION z = ((_cpu.ReadRegister(_cpu.State.CurrentInstruction.Register1.Value) - value) == 0).ToBitPosition();
            BIT_POSITION h = (((_cpu.ReadRegister(_cpu.State.CurrentInstruction.Register1.Value) & 0xF) - (_cpu.State.FetchedData & 0xF) - Convert.ToUInt16(_cpu.State.CFlag())) < 0).ToBitPosition();
            BIT_POSITION c = ((_cpu.ReadRegister(_cpu.State.CurrentInstruction.Register1.Value) - _cpu.State.FetchedData - Convert.ToUInt16(_cpu.State.CFlag())) < 0).ToBitPosition();


            _cpu.SetRegister(_cpu.State.CurrentInstruction.Register1.Value, (UInt16)(_cpu.ReadRegister(_cpu.State.CurrentInstruction.Register1.Value) - value));
            SetCpuFlags(z, BIT_POSITION.ON, h, c);
        }

        public void ProcessIN_ADC()
        {
            UInt16 data = _cpu.State.FetchedData;
            UInt16 regA = _cpu.State.Registers.A;
            UInt16 flagC = Convert.ToUInt16(_cpu.State.CFlag());

            _cpu.State.Registers.A = (byte)((regA + data + flagC) & 0xFF);

            BIT_POSITION z = (_cpu.State.Registers.A == 0).ToBitPosition();
            BIT_POSITION h = ((regA & 0xF) + (data & 0xF) + flagC > 0xF).ToBitPosition();
            BIT_POSITION c = (regA + data + flagC > 0xFF).ToBitPosition();

            SetCpuFlags(z, BIT_POSITION.OFF, h, c);
        }

        public void ProcessIN_ADD()
        {
            UInt32 value = (UInt32)(_cpu.ReadRegister(_cpu.State.CurrentInstruction.Register1.Value) + _cpu.State.FetchedData);
            bool is16Bit = Is16BitRegisterType(_cpu.State.CurrentInstruction.Register1.Value);

            if (is16Bit) _cpu.CallCicles(1);

            if (_cpu.State.CurrentInstruction.Register1.Value == RegisterType.RT_SP)
            {
                value = (UInt16)(_cpu.ReadRegister(_cpu.State.CurrentInstruction.Register1.Value) + (sbyte)_cpu.State.FetchedData);
            }


            BIT_POSITION z = ((value & 0xFF) == 0).ToBitPosition();
            BIT_POSITION h = ((_cpu.ReadRegister(_cpu.State.CurrentInstruction.Register1.Value) & 0xF) + (_cpu.State.FetchedData & 0xF) >= 0x10).ToBitPosition();
            BIT_POSITION c = ((_cpu.ReadRegister(_cpu.State.CurrentInstruction.Register1.Value) & 0xFF) + (_cpu.State.FetchedData & 0xFF) >= 0x100).ToBitPosition();

            if (is16Bit)
            {
                z = BIT_POSITION.SAME;
                h = ((_cpu.ReadRegister(_cpu.State.CurrentInstruction.Register1.Value) & 0xFFF) + (_cpu.State.FetchedData & 0xFFF) >= 0x1000).ToBitPosition();
                c = ((_cpu.ReadRegister(_cpu.State.CurrentInstruction.Register1.Value) + _cpu.State.FetchedData) >= 0x10000).ToBitPosition();
            }

            if (_cpu.State.CurrentInstruction.Register1.Value == RegisterType.RT_SP)
            {
                z = BIT_POSITION.OFF;
                h = ((_cpu.ReadRegister(_cpu.State.CurrentInstruction.Register1.Value) & 0xF) + (_cpu.State.FetchedData & 0xF) >= 0x10).ToBitPosition();
                c = ((_cpu.ReadRegister(_cpu.State.CurrentInstruction.Register1.Value) & 0xFF) + (_cpu.State.FetchedData & 0xFF) >= 0x100).ToBitPosition();
            }

            _cpu.SetRegister(_cpu.State.CurrentInstruction.Register1.Value, (UInt16)(value & 0xFFFF));
            SetCpuFlags(z, BIT_POSITION.OFF, h, c);
        }

        public void ProcessIN_CB()
        {
            byte op = (byte)_cpu.State.FetchedData;
            RegisterType register = DecodeRegister((byte)(op & 0b111));
            byte bit = (byte)((op >> 3) & 0b111);
            byte bit_op = (byte)((op >> 6) & 0b11);
            byte registerValue = _cpu.ReadRegister8bits(register);

            _cpu.CallCicles(1);

            if (register == RegisterType.RT_HL) _cpu.CallCicles(2);

            switch (bit_op)
            {
                case 1:
                    //BIT
                    var zFlag = (!Convert.ToBoolean(registerValue & (1 << bit))).ToBitPosition();
                    SetCpuFlags(zFlag, BIT_POSITION.OFF, BIT_POSITION.ON, BIT_POSITION.SAME);
                    return;
                case 2:
                    //RST
                    registerValue = (byte)(registerValue & ~(1 << bit));
                    _cpu.SetRegister8bits(register, registerValue);
                    return;
                case 3:
                    //SET
                    registerValue = (byte)(registerValue | (1 << bit));
                    _cpu.SetRegister8bits(register, registerValue);
                    return;
            }

            bool cFlag = _cpu.State.CFlag();

            switch (bit)
            {
                case 0:
                    //RLC
                    bool setC = false;
                    byte result = (byte)((registerValue << 1) & 0xFF);
                    if ((registerValue & (1 << 7)) != 0)
                    {
                        result |= 1;
                        setC = true;
                    }

                    _cpu.SetRegister8bits(register, result);
                    SetCpuFlags((result == 0).ToBitPosition(), BIT_POSITION.OFF, BIT_POSITION.OFF, setC.ToBitPosition());
                    return;

                case 1:
                    //RRC
                    byte old = registerValue;
                    registerValue >>= 1;
                    registerValue = (byte)(registerValue | (old << 7));
                    _cpu.SetRegister8bits(register, registerValue);

                    var z = (!(Convert.ToBoolean(registerValue))).ToBitPosition();
                    var c = Convert.ToBoolean(old & 1).ToBitPosition();
                    SetCpuFlags(z, BIT_POSITION.OFF, BIT_POSITION.OFF, c);
                    return;

                case 2:
                    //RL
                    old = registerValue;
                    registerValue <<= 1;
                    registerValue |= Convert.ToByte(cFlag);
                    _cpu.SetRegister8bits(register, registerValue);

                    z = (!(Convert.ToBoolean(registerValue))).ToBitPosition();
                    c = (Convert.ToBoolean(old & 0x80)).ToBitPosition();
                    SetCpuFlags(z, BIT_POSITION.OFF, BIT_POSITION.OFF, c);
                    return;

                case 3:
                    //RR
                    old = registerValue;
                    registerValue >>= 1;
                    registerValue = (byte)(registerValue | (Convert.ToInt32(cFlag) << 7));
                    _cpu.SetRegister8bits(register, registerValue);

                    z = (!(Convert.ToBoolean(registerValue))).ToBitPosition();
                    c = Convert.ToBoolean(old & 1).ToBitPosition();
                    SetCpuFlags(z, BIT_POSITION.OFF, BIT_POSITION.OFF, c);
                    return;

                case 4:
                    //SLA
                    old = registerValue;
                    registerValue <<= 1;

                    _cpu.SetRegister8bits(register, registerValue);

                    z = (!(Convert.ToBoolean(registerValue))).ToBitPosition();
                    c = (Convert.ToBoolean(old & 0x80)).ToBitPosition();
                    SetCpuFlags(z, BIT_POSITION.OFF, BIT_POSITION.OFF, c);
                    return;

                case 5:
                    //SRA
                    old = (byte)((sbyte)registerValue >> 1);
                    _cpu.SetRegister8bits(register, old);

                    z = (!(Convert.ToBoolean(old))).ToBitPosition();
                    c = Convert.ToBoolean(registerValue & 1).ToBitPosition();
                    SetCpuFlags(z, BIT_POSITION.OFF, BIT_POSITION.OFF, c);
                    return;
                case 6:
                    //SRA
                    registerValue = (byte)(((registerValue & 0xF0) >> 4) | ((registerValue & 0xF) << 4));
                    _cpu.SetRegister8bits(register, registerValue);

                    z = (registerValue == 0).ToBitPosition();

                    SetCpuFlags(z, BIT_POSITION.OFF, BIT_POSITION.OFF, BIT_POSITION.OFF);
                    return;

                case 7:
                    //SRL
                    old = (byte)(registerValue >> 1);

                    _cpu.SetRegister8bits(register, old);

                    z = (!(Convert.ToBoolean(old))).ToBitPosition();
                    c = Convert.ToBoolean(registerValue & 1).ToBitPosition();
                    SetCpuFlags(z, BIT_POSITION.OFF, BIT_POSITION.OFF, c);
                    return;
            }
        }

        public void ProcessIN_AND()
        {
            _cpu.State.Registers.A &= (byte)_cpu.State.FetchedData;
            var z = (_cpu.State.Registers.A == 0).ToBitPosition();
            SetCpuFlags(z, BIT_POSITION.OFF, BIT_POSITION.ON, BIT_POSITION.OFF);
        }

        public void ProcessIN_OR()
        {
            _cpu.State.Registers.A |= (byte)(_cpu.State.FetchedData & 0xFF);
            var z = (_cpu.State.Registers.A == 0).ToBitPosition();
            SetCpuFlags(z, BIT_POSITION.OFF, BIT_POSITION.OFF, BIT_POSITION.OFF);
        }

        public void ProcessIN_CP()
        {
            int result = _cpu.State.Registers.A - _cpu.State.FetchedData;

            var z = (result == 0).ToBitPosition();
            var h = (((_cpu.State.Registers.A & 0x0F) - (_cpu.State.FetchedData & 0x0F)) < 0).ToBitPosition();
            var c = (result < 0).ToBitPosition();
            SetCpuFlags(z, BIT_POSITION.ON, h, c);
        }

        public void ProcessIN_RRA()
        {
            byte carry = Convert.ToByte(_cpu.State.CFlag());
            var cFlag = Convert.ToBoolean(_cpu.State.Registers.A & 1).ToBitPosition();

            _cpu.State.Registers.A >>= 1;
            _cpu.State.Registers.A = (byte)(_cpu.State.Registers.A | (carry << 7));


            SetCpuFlags(BIT_POSITION.OFF, BIT_POSITION.OFF, BIT_POSITION.OFF, cFlag);
        }

        public void ProcessIN_RLCA()
        {
            byte result = _cpu.State.Registers.A;
            var c = (result >> 7) & 1;
            var cFlag = Convert.ToBoolean(c).ToBitPosition();
            result = (byte)((result << 1) | c);
            _cpu.State.Registers.A = result;
            SetCpuFlags(BIT_POSITION.OFF, BIT_POSITION.OFF, BIT_POSITION.OFF, cFlag);
        }

        public void ProcessIN_RRCA()
        {
            byte b = (byte)(_cpu.State.Registers.A & 1);
            _cpu.State.Registers.A >>= 1;
            _cpu.State.Registers.A = (byte)(_cpu.State.Registers.A | (b << 7));

            var cFlag = Convert.ToBoolean(b).ToBitPosition();
            SetCpuFlags(BIT_POSITION.OFF, BIT_POSITION.OFF, BIT_POSITION.OFF, cFlag);
        }

        public void ProcessIN_RLA()
        {
            byte reg = _cpu.State.Registers.A;
            byte cFlagByteValue = Convert.ToByte(_cpu.State.CFlag());
            byte result = (byte)((reg >> 7) & 1);

            _cpu.State.Registers.A = (byte)((reg << 1) | cFlagByteValue);

            var cFlag = Convert.ToBoolean(result).ToBitPosition();
            SetCpuFlags(BIT_POSITION.OFF, BIT_POSITION.OFF, BIT_POSITION.OFF, cFlag);
        }

        public void ProcessIN_STOP()
        {
            throw new Exception("Stopping...");
        }

        public void ProcessIN_DAA()
        {
            byte u = 0;
            int fc = 0;

            if (_cpu.State.HFlag() || (!_cpu.State.NFlag() && (_cpu.State.Registers.A & 0xF) > 9))
            {
                u = 6;
            }

            if (_cpu.State.CFlag() || (!_cpu.State.NFlag() && _cpu.State.Registers.A > 0x99))
            {
                u |= 0x60;
                fc = 1;
            }

            _cpu.State.Registers.A = (byte)(_cpu.State.Registers.A + (_cpu.State.NFlag() ? -u : u));

            var z = (_cpu.State.Registers.A == 0).ToBitPosition();
            var c = Convert.ToBoolean(fc).ToBitPosition();
            SetCpuFlags(z, BIT_POSITION.SAME, BIT_POSITION.OFF, c);
        }

        public void ProcessIN_CPL()
        {
            _cpu.State.Registers.A = (byte)~_cpu.State.Registers.A;
            SetCpuFlags(BIT_POSITION.SAME, BIT_POSITION.ON, BIT_POSITION.ON, BIT_POSITION.SAME);
        }

        public void ProcessIN_SCF()
        {
            SetCpuFlags(BIT_POSITION.SAME, BIT_POSITION.OFF, BIT_POSITION.OFF, BIT_POSITION.ON);
        }

        public void ProcessIN_CCF()
        {
            var c = Convert.ToBoolean(Convert.ToInt32(_cpu.State.CFlag()) ^ 1).ToBitPosition();
            SetCpuFlags(BIT_POSITION.SAME, BIT_POSITION.OFF, BIT_POSITION.OFF, c);
        }

        public void ProcessIN_HALT()
        {
            _cpu.State.Halted = true;
        }

        public void ProcessIN_EI()
        {
            _cpu.State.EnableIME = true;
        }

        private void GotoAddress(UInt16 address, bool pushPC)
        {
            if (CheckCondition())
            {
                if (pushPC)
                {
                    _cpu.CallCicles(2);
                    _cpu.GetStack().Push16(_cpu.State.Registers.PC);
                }

                _cpu.State.Registers.PC = address;
                _cpu.CallCicles(1);
            }
        }

        private void SetCpuFlags(BIT_POSITION z, BIT_POSITION n, BIT_POSITION h, BIT_POSITION c)
        {
            _cpu.State.Registers.F = BitHelper.SetBitValue(_cpu.State.Registers.F, 7, z);
            _cpu.State.Registers.F = BitHelper.SetBitValue(_cpu.State.Registers.F, 6, n);
            _cpu.State.Registers.F = BitHelper.SetBitValue(_cpu.State.Registers.F, 5, h);
            _cpu.State.Registers.F = BitHelper.SetBitValue(_cpu.State.Registers.F, 4, c);
        }

        private bool CheckCondition()
        {
            bool z = _cpu.State.ZFlag();
            bool c = _cpu.State.CFlag();

            switch (_cpu.State.CurrentInstruction.Condition)
            {
                case ConditionType.CT_NONE: return true;
                case ConditionType.CT_C: return c;
                case ConditionType.CT_NC: return !c;
                case ConditionType.CT_Z: return z;
                case ConditionType.CT_NZ: return !z;
            }

            return false;
        }

        private bool Is16BitRegisterType(RegisterType registerType)
        {
            return registerType >= RegisterType.RT_AF;
        }

        private RegisterType DecodeRegister(byte register)
        {
            if (register > 0b111) return RegisterType.RT_NONE;

            return RegisterLookup[register];
        }

        private RegisterType[] RegisterLookup = {
            RegisterType.RT_B,
            RegisterType.RT_C,
            RegisterType.RT_D,
            RegisterType.RT_E,
            RegisterType.RT_H,
            RegisterType.RT_L,
            RegisterType.RT_HL,
            RegisterType.RT_A
        };
    }
}
