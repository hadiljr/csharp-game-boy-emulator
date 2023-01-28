using FluentAssertions;
using GameBoyEmulator.Util.Bit;
using System;
using Xunit;

namespace GameBoyEmulatorTests.Unit.Util.Bit
{
    public class BitHelperTest
    {

        [Theory]
        [InlineData(0b0000_0000,0,BIT_POSITION.ON,0b0000_0001)]
        [InlineData(0b0000_0001,0,BIT_POSITION.OFF,0b0000_0000)]
        [InlineData(0b0000_0001,0,BIT_POSITION.SAME,0b0000_0001)]
        public void Should_set_bit_value_into_position(byte value, byte position,BIT_POSITION bitPosition,byte expected)
        {

            //Arrange Act
           
            //act
            var result = BitHelper.SetBitValue(value, position, bitPosition);
            //assert
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(0b00010001,0,true)]
        [InlineData(0b00010001,2,false)]
        public void Should_get_bit_value(byte bytes,byte position,bool expectedValue)
        {
            //Arrange
            //Act
            var result = BitHelper.GetBitValue(bytes, position);
            //Assert
            result.Should().Be(expectedValue);
        }

        [Theory]
        [InlineData(0b00010001, 0b11110000, 0b0001000111110000)]
        [InlineData(0b10010001, 0b11110001, 0b1001000111110001)]
        [InlineData(0b00000001, 0b11110000, 0b0000000111110000)]
        public void Should_combine_two_values_of_8_bits(byte first,byte second,UInt16 expectedResult)
        {
            //Arrange
            //Act
            var result = BitHelper.CombineTwoValuesOf8bits(first, second);

            //Assert
            result.Should().Be(expectedResult);
        }

        [Theory]
        [InlineData(0b0001000111110000,0b00010001, 0b11110000)]
        [InlineData(0b1001000111110001,0b10010001, 0b11110001)]
        [InlineData(0b0000000111110000,0b00000001, 0b11110000)]
        public void Should_extract_two_values_of_8_bits(UInt16 value, byte expectedValue1,byte expectedValue2)
        {
            //Arrange Act
            var result = BitHelper.ExtractTwoValuesOf8Bits(value);

            //Assert
            result.Item1.Should().Be(expectedValue1);
            result.Item2.Should().Be(expectedValue2);
        }
    }
}
