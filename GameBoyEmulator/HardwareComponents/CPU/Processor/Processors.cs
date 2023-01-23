using GameBoyEmulator.HardwareComponents.CPU.Instructions;
using GameBoyEmulator.Util.Bit;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameBoyEmulator.HardwareComponents.CPU.Processor
{
    public class Processors
    {
        public static void ProcessIN_NONE(CpuContext ctx)
        {
            throw new Exception("INVALID INSTRUCTION!");
        }


        public static void ProcessIN_NOP(CpuContext ctx)
        {
            //do nothing
        }


        public static void ProcessIN_DI(CpuContext ctx)
        {
            ctx.IntMasterEnabled = false;
        }

        public static void ProcessIN_LD(CpuContext ctx)
        {
            //TODO
        }

        public static void ProcessIN_XOR(CpuContext ctx)
        {
            ctx.Registers.A ^= Convert.ToByte(ctx.FetchedData & 0xFF);
            BIT_POSITION z = ctx.Registers.A == 0 ? BIT_POSITION.ON : BIT_POSITION.OFF;
            SetCpuFlags(ctx, z, BIT_POSITION.OFF, BIT_POSITION.OFF, BIT_POSITION.OFF);
        }

        public static void ProcessIN_JP(CpuContext ctx)
        {
            if (CheckCondition(ctx))
            {
                ctx.Registers.PC = ctx.FetchedData;
                ctx.GbEmulator.cicles(1);
            }
        }

        private static void SetCpuFlags(CpuContext ctx, BIT_POSITION z, BIT_POSITION n, BIT_POSITION h, BIT_POSITION c)
        {
            ctx.Registers.F = BitHelper.SetBitValue(ctx.Registers.F, 7, z);
            ctx.Registers.F = BitHelper.SetBitValue(ctx.Registers.F, 6, n);
            ctx.Registers.F = BitHelper.SetBitValue(ctx.Registers.F, 5, h);
            ctx.Registers.F = BitHelper.SetBitValue(ctx.Registers.F, 5, c);
        }

        private static bool CheckCondition(CpuContext ctx)
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
    }
}
