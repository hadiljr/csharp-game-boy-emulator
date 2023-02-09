using GameBoyEmulator.HardwareComponents.DataBus;
using GameBoyEmulator.Util.Extensions;
using System;

namespace GameBoyEmulator.HardwareComponents.CPU.Instructions
{
    public class Instruction
    {
        public InstructionType Type { get; set; }
        public AdressModeType? Mode { get; set; }
        public RegisterType? Register1 { get; set; }
        public RegisterType? Register2 { get; set; }
        public ConditionType Condition { get; set; }
        public byte? Parameter { get; set; }

        public Instruction(InstructionType type, AdressModeType? mode = null, RegisterType? register1 = null, RegisterType? register2 = null, ConditionType condition = ConditionType.CT_NONE,byte? parameter=null)
        {
            Type = type;
            Mode = mode;
            Register1 = register1;
            Register2 = register2;
            Condition = condition;
            Parameter = parameter;
        }

    }
}
