using GameBoyEmulator.Emulator;
using GameBoyEmulator.HardwareComponents.CPU.Instructions;
using GameBoyEmulator.HardwareComponents.DataBus;
using GameBoyEmulator.HardwareComponents.ProgramStack;
using GameBoyEmulator.Util.Bit;
using GameBoyEmulator.Util.Extensions;
using System;

namespace GameBoyEmulator.HardwareComponents.CPU.Processor
{
    public class Processors
    {
        public static void ProcessIN_NONE(CpuState ctx)
        {
            throw new Exception("INVALID INSTRUCTION!");
        }

        public static void ProcessIN_NOP(CpuState ctx)
        {
            //do nothing
        }

        public static void ProcessIN_DI(CpuState ctx)
        {
            ctx.InterruptionMasterEnabled = false;
        }

        public static void ProcessIN_LD(CpuState ctx)
        {
            if (ctx.DestinationIsMemory)
            {
                //16 bit register
                if (ctx.CurrentInstruction.Register2.Value >= RegisterType.RT_AF)
                {
                    GbEmulator.cicles(1);
                    Bus.Write16(ctx.MemoryDestination, ctx.FetchedData);

                }
                else
                {
                    Bus.Write(ctx.MemoryDestination, (byte)ctx.FetchedData);
                }

                return;
            }

            if (ctx.CurrentInstruction.Mode == AdressModeType.AM_HL_SPR)
            {
                bool hflag = (Cpu.ReadRegister(ctx.CurrentInstruction.Register2.Value) & 0xF) + (ctx.FetchedData & 0xF) >= 0x10;
                bool cflag = (Cpu.ReadRegister(ctx.CurrentInstruction.Register2.Value) & 0xFF) + (ctx.FetchedData & 0xFF) >= 0x100;

                SetCpuFlags(ctx, BIT_POSITION.OFF, BIT_POSITION.OFF, hflag.ToBitPosition(), cflag.ToBitPosition());

                var reg2Data = Cpu.ReadRegister(ctx.CurrentInstruction.Register2.Value);
                var data = ctx.FetchedData;
                Cpu.SetRegister(ctx.CurrentInstruction.Register1.Value, (UInt16)(reg2Data + data));

                return;
            }

            Cpu.SetRegister(ctx.CurrentInstruction.Register1.Value, ctx.FetchedData);
        }

        public static void ProcessIN_LDH(CpuState ctx)
        {
            if (ctx.CurrentInstruction.Register1.Value == RegisterType.RT_A)
            {
                var data = Bus.Read((UInt16)(0xFF00 | ctx.FetchedData));
                Cpu.SetRegister(ctx.CurrentInstruction.Register1.Value, data);
            }
            else
            {
                Bus.Write(ctx.MemoryDestination, ctx.Registers.A);
            }

            GbEmulator.cicles(1);
        }

        public static void ProcessIN_XOR(CpuState ctx)
        {
            ctx.Registers.A ^= Convert.ToByte(ctx.FetchedData & 0xFF);
            BIT_POSITION z = ctx.Registers.A == 0 ? BIT_POSITION.ON : BIT_POSITION.OFF;
            SetCpuFlags(ctx, z, BIT_POSITION.OFF, BIT_POSITION.OFF, BIT_POSITION.OFF);
        }

        public static void ProcessIN_JP(CpuState ctx)
        {
            GotoAddress(ctx, ctx.FetchedData, false);
        }

        public static void ProcessIN_JR(CpuState ctx)
        {
            var data = ctx.FetchedData & 0xFF;
            UInt16 adress = (UInt16)(ctx.Registers.PC + data);
            GotoAddress(ctx, adress, false);
        }

        public static void ProcessIN_CALL(CpuState ctx)
        {
            GotoAddress(ctx, ctx.FetchedData, true);
        }

        public static void ProcessIN_RST(CpuState ctx)
        {
            GotoAddress(ctx, ctx.CurrentInstruction.Parameter.Value, true);
        }

        public static void ProcessIN_RET(CpuState ctx)
        {
            if (ctx.CurrentInstruction.Condition != ConditionType.CT_NONE)
            {
                GbEmulator.cicles(1);
            }

            if (CheckCondition(ctx))
            {
                UInt16 lo = Stack.Pop();
                GbEmulator.cicles(1);
                UInt16 hi = Stack.Pop();
                GbEmulator.cicles(1);

                UInt16 res = (UInt16)((hi << 8) | lo);
                ctx.Registers.PC = res;
                GbEmulator.cicles(1);
            }
        }

        public static void ProcessIN_RETI(CpuState ctx)
        {
            ctx.InterruptionMasterEnabled = true;
            ProcessIN_RET(ctx);
        }

        public static void ProcessIN_POP(CpuState ctx)
        {
            UInt16 lo = Stack.Pop();
            GbEmulator.cicles(1);
            UInt16 hi = Stack.Pop();
            GbEmulator.cicles(1);

            UInt16 res = (UInt16)((hi << 8) | lo);

            Cpu.SetRegister(ctx.CurrentInstruction.Register1.Value, res);

            if (ctx.CurrentInstruction.Register1.Value == RegisterType.RT_AF)
            {
                Cpu.SetRegister(ctx.CurrentInstruction.Register1.Value, (UInt16)(res & 0xFFF0));
            }

        }

        public static void ProcessIN_PUSH(CpuState ctx)
        {

            byte hi = (byte)((Cpu.ReadRegister(ctx.CurrentInstruction.Register1.Value) >> 8) & 0xFF);
            GbEmulator.cicles(1);
            Stack.Push(hi);

            byte lo = (byte)(Cpu.ReadRegister(ctx.CurrentInstruction.Register1.Value) & 0xFF);
            GbEmulator.cicles(1);
            Stack.Push(hi);

            GbEmulator.cicles(1);
        }

        public static void ProcessIN_INC(CpuState ctx)
        {
            UInt16 value = (UInt16)(Cpu.ReadRegister(ctx.CurrentInstruction.Register1.Value) + 1);

            if (Is16BitRegisterType(ctx.CurrentInstruction.Register1.Value))
            {
                GbEmulator.cicles(1);
            }

            if (ctx.CurrentInstruction.Register1.Value == RegisterType.RT_HL &&
                ctx.CurrentInstruction.Mode == AdressModeType.AM_MR)
            {
                value = (UInt16)(Bus.Read(Cpu.ReadRegister(RegisterType.RT_HL)) + 1);
                value &= 0xFF;
                Bus.Write(Cpu.ReadRegister(RegisterType.RT_HL), (byte)value);
            }
            else
            {
                Cpu.SetRegister(ctx.CurrentInstruction.Register1.Value, value);
                value = Cpu.ReadRegister(ctx.CurrentInstruction.Register1.Value);
            }

            if ((ctx.CurrentOpcode & 0x03) == 0x03) return;

            SetCpuFlags(ctx, (value == 0).ToBitPosition(), BIT_POSITION.OFF, ((value & 0x0F) == 0).ToBitPosition(), BIT_POSITION.SAME);
        }

        public static void ProcessIN_DEC(CpuState ctx)
        {
            UInt16 value = (UInt16)(Cpu.ReadRegister(ctx.CurrentInstruction.Register1.Value) - 1);

            if (Is16BitRegisterType(ctx.CurrentInstruction.Register1.Value))
            {
                GbEmulator.cicles(1);
            }

            if (ctx.CurrentInstruction.Register1.Value == RegisterType.RT_HL &&
               ctx.CurrentInstruction.Mode == AdressModeType.AM_MR)
            {
                value = (UInt16)(Bus.Read(Cpu.ReadRegister(RegisterType.RT_HL)) - 1);
                Bus.Write(Cpu.ReadRegister(RegisterType.RT_HL), (byte)value);
            }
            else
            {
                Cpu.SetRegister(ctx.CurrentInstruction.Register1.Value, value);
                value = Cpu.ReadRegister(ctx.CurrentInstruction.Register1.Value);
            }

            if ((ctx.CurrentOpcode & 0x0B) == 0x0B) return;

            SetCpuFlags(ctx, (value == 0).ToBitPosition(), BIT_POSITION.ON, ((value & 0x0F) == 0x0F).ToBitPosition(), BIT_POSITION.SAME);
        }

        public static void ProcessIN_SUB(CpuState ctx)
        {
            UInt16 value = (UInt16)(Cpu.ReadRegister(ctx.CurrentInstruction.Register1.Value) - ctx.FetchedData);
            BIT_POSITION z = (value == 0).ToBitPosition();
            BIT_POSITION h = (((Cpu.ReadRegister(ctx.CurrentInstruction.Register1.Value) & 0xF) - (ctx.FetchedData & 0xF)) < 0).ToBitPosition();
            BIT_POSITION c = ((Cpu.ReadRegister(ctx.CurrentInstruction.Register1.Value) - ctx.FetchedData) < 0).ToBitPosition();

            Cpu.SetRegister(ctx.CurrentInstruction.Register1.Value, value);
            SetCpuFlags(ctx, z, BIT_POSITION.ON, h, c);
        }

        public static void ProcessIN_SBC(CpuState ctx)
        {
            byte value = (byte)(ctx.FetchedData + Convert.ToUInt16(ctx.CFlag()));
            BIT_POSITION z = ((Cpu.ReadRegister(ctx.CurrentInstruction.Register1.Value) - value) == 0).ToBitPosition();
            BIT_POSITION h = (((Cpu.ReadRegister(ctx.CurrentInstruction.Register1.Value) & 0xF) - (ctx.FetchedData & 0xF) - Convert.ToUInt16(ctx.CFlag())) < 0).ToBitPosition();
            BIT_POSITION c = ((Cpu.ReadRegister(ctx.CurrentInstruction.Register1.Value) - ctx.FetchedData - Convert.ToUInt16(ctx.CFlag())) < 0).ToBitPosition();


            Cpu.SetRegister(ctx.CurrentInstruction.Register1.Value, (UInt16)(Cpu.ReadRegister(ctx.CurrentInstruction.Register1.Value) - value));
            SetCpuFlags(ctx, z, BIT_POSITION.ON, h, c);
        }

        public static void ProcessIN_ADC(CpuState ctx)
        {
            UInt16 data = ctx.FetchedData;
            UInt16 regA = ctx.Registers.A;
            UInt16 flagC = Convert.ToUInt16(ctx.ZFlag());

            ctx.Registers.A = (byte)((regA + data + flagC) & 0xFF);

            BIT_POSITION z = (ctx.Registers.A == 0).ToBitPosition();
            BIT_POSITION h = ((regA & 0xF) + (data & 0xF) + flagC > 0xF).ToBitPosition();
            BIT_POSITION c = (regA + data + flagC > 0xFF).ToBitPosition();

            SetCpuFlags(ctx, z, BIT_POSITION.OFF, h, c);
        }

        public static void ProcessIN_ADD(CpuState ctx)
        {
            UInt16 value = (UInt16)(Cpu.ReadRegister(ctx.CurrentInstruction.Register1.Value) + ctx.FetchedData);
            bool is16Bit = Is16BitRegisterType(ctx.CurrentInstruction.Register1.Value);

            if (is16Bit) GbEmulator.cicles(1);

            if (ctx.CurrentInstruction.Register1.Value == RegisterType.RT_SP)
            {
                value = (UInt16)(Cpu.ReadRegister(ctx.CurrentInstruction.Register1.Value) + (char)ctx.FetchedData);
            }


            BIT_POSITION z = (value == 0).ToBitPosition();
            BIT_POSITION h = ((Cpu.ReadRegister(ctx.CurrentInstruction.Register1.Value) & 0xF) + (ctx.FetchedData & 0xF) >= 0x10).ToBitPosition();
            BIT_POSITION c = ((Cpu.ReadRegister(ctx.CurrentInstruction.Register1.Value) & 0xFF) + (ctx.FetchedData & 0xFF) >= 0x100).ToBitPosition();

            if (is16Bit)
            {
                z = BIT_POSITION.OFF;
                h = ((Cpu.ReadRegister(ctx.CurrentInstruction.Register1.Value) & 0xFFF) + (ctx.FetchedData & 0xFFF) >= 0x1000).ToBitPosition();
                c = ((Cpu.ReadRegister(ctx.CurrentInstruction.Register1.Value) + ctx.FetchedData) >= 0x10000).ToBitPosition();
            }

            if (ctx.CurrentInstruction.Register1.Value == RegisterType.RT_SP)
            {
                z = BIT_POSITION.ON;
                h = ((Cpu.ReadRegister(ctx.CurrentInstruction.Register1.Value) & 0xF) + (ctx.FetchedData & 0xF) >= 0x10).ToBitPosition();
                c = ((Cpu.ReadRegister(ctx.CurrentInstruction.Register1.Value) & 0xFF) + (ctx.FetchedData & 0xFF) >= 0x100).ToBitPosition();
            }

            Cpu.SetRegister(ctx.CurrentInstruction.Register1.Value, (UInt16)(value & 0xFFFF));
            SetCpuFlags(ctx, z, BIT_POSITION.OFF, h, c);
        }

        public static void ProcessIN_CB(CpuState ctx)
        {
            byte op = (byte)ctx.FetchedData;
            RegisterType register = DecodeRegister((byte)(op & 0b111));
            byte bit = (byte)((op >> 3) & 0b111);
            byte bit_op = (byte)((op >> 6) & 0b11);
            byte registerValue = Cpu.ReadRegister8bits(register);

            GbEmulator.cicles(1);

            if (register == RegisterType.RT_HL) GbEmulator.cicles(2);

            switch (bit_op)
            {
                case 1:
                    //BIT
                    var zFlag = (!Convert.ToBoolean(registerValue & (1 << bit))).ToBitPosition();
                    SetCpuFlags(ctx, zFlag, BIT_POSITION.OFF, BIT_POSITION.ON, BIT_POSITION.SAME);
                    return;
                case 2:
                    //RST
                    registerValue = (byte)(registerValue & ~(1 << bit));
                    Cpu.SetRegister8bits(register, registerValue);
                    return;
                case 3:
                    //SET
                    registerValue = (byte)(registerValue | (1 << bit));
                    Cpu.SetRegister8bits(register, registerValue);
                    return;
            }

            bool cFlag = ctx.ZFlag();

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

                    Cpu.SetRegister8bits(register, result);
                    SetCpuFlags(ctx, (result == 0).ToBitPosition(), BIT_POSITION.OFF, BIT_POSITION.OFF, setC.ToBitPosition());
                    return;

                case 1:
                    //RRC
                    byte old = registerValue;
                    registerValue >>= 1;
                    registerValue = (byte)(registerValue | (old << 7));
                    Cpu.SetRegister8bits(register, registerValue);

                    var z = (!(Convert.ToBoolean(registerValue))).ToBitPosition();
                    var c = Convert.ToBoolean(old & 1).ToBitPosition();
                    SetCpuFlags(ctx, z, BIT_POSITION.OFF, BIT_POSITION.OFF, c);
                    return;

                case 2:
                    //RL
                    old = registerValue;
                    registerValue <<= 1;
                    registerValue = (byte)(registerValue | (old << 7));
                    Cpu.SetRegister8bits(register, registerValue);

                    z = (!(Convert.ToBoolean(registerValue))).ToBitPosition();
                    c = (!Convert.ToBoolean(old & 0x80)).ToBitPosition();
                    SetCpuFlags(ctx, z, BIT_POSITION.OFF, BIT_POSITION.OFF, c);
                    return;

                case 3:
                    //RR
                    old = registerValue;
                    registerValue >>= 1;
                    registerValue = (byte)(registerValue | (Convert.ToInt32(cFlag) << 7));
                    Cpu.SetRegister8bits(register, registerValue);

                    z = (!(Convert.ToBoolean(registerValue))).ToBitPosition();
                    c = Convert.ToBoolean(old & 1).ToBitPosition();
                    SetCpuFlags(ctx, z, BIT_POSITION.OFF, BIT_POSITION.OFF, c);
                    return;

                case 4:
                    //SLA
                    old = registerValue;
                    registerValue <<= 1;

                    Cpu.SetRegister8bits(register, registerValue);

                    z = (!(Convert.ToBoolean(registerValue))).ToBitPosition();
                    c = (!Convert.ToBoolean(old & 0x80)).ToBitPosition();
                    SetCpuFlags(ctx, z, BIT_POSITION.OFF, BIT_POSITION.OFF, c);
                    return;

                case 5:
                    //SRA
                    old = (byte)(registerValue >> 1);
                    Cpu.SetRegister8bits(register, old);

                    z = (!(Convert.ToBoolean(old))).ToBitPosition();
                    c = Convert.ToBoolean(registerValue & 1).ToBitPosition();
                    SetCpuFlags(ctx, z, BIT_POSITION.OFF, BIT_POSITION.OFF, c);
                    return;
                case 6:
                    //SRA
                    registerValue = (byte)(((registerValue & 0xF0) >> 4) | ((registerValue & 0xF) << 4));
                    Cpu.SetRegister8bits(register, registerValue);

                    z = (registerValue == 0).ToBitPosition();

                    SetCpuFlags(ctx, z, BIT_POSITION.OFF, BIT_POSITION.OFF, BIT_POSITION.OFF);
                    return;

                case 7:
                    //SRL
                    old = (byte)(registerValue >> 1);

                    Cpu.SetRegister8bits(register, old);

                    z = (!(Convert.ToBoolean(old))).ToBitPosition();
                    c = Convert.ToBoolean(registerValue & 1).ToBitPosition();
                    SetCpuFlags(ctx, z, BIT_POSITION.OFF, BIT_POSITION.OFF, c);
                    return;
            }
        }

        public static void ProcessIN_AND(CpuState ctx)
        {
            ctx.Registers.A &= (byte)ctx.FetchedData;
            var z = (ctx.Registers.A == 0).ToBitPosition();
            SetCpuFlags(ctx, z, BIT_POSITION.OFF, BIT_POSITION.ON, BIT_POSITION.OFF);
        }

        public static void ProcessIN_OR(CpuState ctx)
        {
            ctx.Registers.A |= (byte)(ctx.FetchedData & 0xFF);
            var z = (ctx.Registers.A == 0).ToBitPosition();
            SetCpuFlags(ctx, z, BIT_POSITION.OFF, BIT_POSITION.ON, BIT_POSITION.OFF);
        }

        public static void ProcessIN_CP(CpuState ctx)
        {
            int result = ctx.Registers.A - ctx.FetchedData;

            var z = (result == 0).ToBitPosition();
            var h = (((ctx.Registers.A & 0x0F) - (ctx.FetchedData & 0x0F)) < 0).ToBitPosition();
            var c = (result < 0).ToBitPosition();
            SetCpuFlags(ctx, z, BIT_POSITION.ON, h, c);
        }

        public static void ProcessIN_RRA(CpuState ctx)
        {
            byte carry = Convert.ToByte(ctx.CFlag());

            ctx.Registers.A >>= 1;
            ctx.Registers.A = (byte)(ctx.Registers.A | (carry << 7));

            var cFlag = Convert.ToBoolean(ctx.Registers.A & 1).ToBitPosition();
            SetCpuFlags(ctx, BIT_POSITION.OFF, BIT_POSITION.OFF, BIT_POSITION.OFF, cFlag);
        }

        public static void ProcessIN_RLCA(CpuState ctx)
        {
            byte result = ctx.Registers.A;
            var c = (result >> 7) & 1;
            var cFlag = Convert.ToBoolean(c).ToBitPosition();
            result = (byte)((result << 1) | c);
            ctx.Registers.A = result;
            SetCpuFlags(ctx, BIT_POSITION.OFF, BIT_POSITION.OFF, BIT_POSITION.OFF, cFlag);
        }

        public static void ProcessIN_RRCA(CpuState ctx)
        {
            byte b = (byte)(ctx.Registers.A & 1);
            ctx.Registers.A >>= 1;
            ctx.Registers.A = (byte)(ctx.Registers.A | (b << 7));

            var cFlag = Convert.ToBoolean(b).ToBitPosition();
            SetCpuFlags(ctx, BIT_POSITION.OFF, BIT_POSITION.OFF, BIT_POSITION.OFF, cFlag);
        }

        public static void ProcessIN_RLA(CpuState ctx)
        {
            byte reg = ctx.Registers.A;
            byte cFlagByteValue = Convert.ToByte(ctx.CFlag());
            byte result = (byte)((reg >> 7) & 1);

            ctx.Registers.A = (byte)((reg << 1) | cFlagByteValue);

            var cFlag = Convert.ToBoolean(result).ToBitPosition();
            SetCpuFlags(ctx, BIT_POSITION.OFF, BIT_POSITION.OFF, BIT_POSITION.OFF, cFlag);
        }

        public static void ProcessIN_STOP(CpuState ctx)
        {
            throw new Exception("Stopping...");
        }

        public static void ProcessIN_DAA(CpuState ctx)
        {
            byte u = 0;
            int fc = 0;

            if (ctx.HFlag() || (!ctx.NFlag() && (ctx.Registers.A & 0xF) > 9))
            {
                u = 6;
            }

            if (ctx.CFlag() || (!ctx.NFlag() && ctx.Registers.A > 0x99))
            {
                u |= 0x60;
                fc = 1;
            }

            ctx.Registers.A = (byte)(ctx.Registers.A + (ctx.NFlag() ? -u : u));

            var z = (ctx.Registers.A == 0).ToBitPosition();
            var c = Convert.ToBoolean(fc).ToBitPosition();
            SetCpuFlags(ctx, z, BIT_POSITION.SAME, BIT_POSITION.OFF, c);
        }

        public static void ProcessIN_CPL(CpuState ctx)
        {
            ctx.Registers.A = (byte)~ctx.Registers.A;
            SetCpuFlags(ctx, BIT_POSITION.SAME, BIT_POSITION.ON, BIT_POSITION.ON, BIT_POSITION.SAME);
        }

        public static void ProcessIN_SCF(CpuState ctx)
        {
            SetCpuFlags(ctx, BIT_POSITION.SAME, BIT_POSITION.OFF, BIT_POSITION.OFF, BIT_POSITION.ON);
        }

        public static void ProcessIN_CCF(CpuState ctx)
        {
            var c = Convert.ToBoolean(Convert.ToInt32(ctx.CFlag()) ^ 1).ToBitPosition();
            SetCpuFlags(ctx, BIT_POSITION.SAME, BIT_POSITION.OFF, BIT_POSITION.OFF, c);
        }

        public static void ProcessIN_HALT(CpuState ctx)
        {
            ctx.Halted = true;
        }

        public static void ProcessIN_EI(CpuState ctx)
        {
            ctx.EnableIME = true;
        }

        private static void GotoAddress(CpuState ctx, UInt16 address, bool pushPC)
        {
            if (CheckCondition(ctx))
            {
                if (pushPC)
                {
                    GbEmulator.cicles(2);
                    Stack.Push16(ctx.Registers.PC);
                }

                ctx.Registers.PC = address;
                GbEmulator.cicles(1);
            }
        }

        private static void SetCpuFlags(CpuState ctx, BIT_POSITION z, BIT_POSITION n, BIT_POSITION h, BIT_POSITION c)
        {
            ctx.Registers.F = BitHelper.SetBitValue(ctx.Registers.F, 7, z);
            ctx.Registers.F = BitHelper.SetBitValue(ctx.Registers.F, 6, n);
            ctx.Registers.F = BitHelper.SetBitValue(ctx.Registers.F, 5, h);
            ctx.Registers.F = BitHelper.SetBitValue(ctx.Registers.F, 4, c);
        }

        private static bool CheckCondition(CpuState ctx)
        {
            bool z = ctx.ZFlag();
            bool c = ctx.CFlag();

            switch (ctx.CurrentInstruction.Condition)
            {
                case ConditionType.CT_NONE: return true;
                case ConditionType.CT_C: return c;
                case ConditionType.CT_NC: return !c;
                case ConditionType.CT_Z: return z;
                case ConditionType.CT_NZ: return !z;
            }

            return false;
        }

        private static bool Is16BitRegisterType(RegisterType registerType)
        {
            return registerType >= RegisterType.RT_AF;
        }

        private static RegisterType DecodeRegister(byte register)
        {
            if (register > 0b111) return RegisterType.RT_NONE;

            return RegisterLookup[register];
        }

        private static RegisterType[] RegisterLookup = {
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
