using GameBoyEmulator.HardwareComponents.CPU.Instructions;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameBoyEmulator.Util.Extensions
{
    static class RegisterTypeEnumExtensions
    {
        private static readonly string[] registersTypeNames =
        {
            "<>",
             "A",
            "F",
            "B",
            "C",
            "D",
            "E",
            "H",
            "L",
            "AF",
            "BC",
            "DE",
            "HL",
            "SP",
            "PC"
        };
        public static string ToName(this RegisterType rt)
        {
            return registersTypeNames[(int)rt];
        }
    }
}
