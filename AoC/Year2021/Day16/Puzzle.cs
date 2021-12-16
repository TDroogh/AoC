using System;
using System.Collections.Generic;
using System.Linq;
using AoC.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Year2021.Day16
{
    [TestClass]
    public class Puzzle
    {
        private static class Results
        {
            public const long Setup10 = 2021;
            public const long Setup11 = 30;
            public const long Setup12 = 6;
            public const long Setup13 = 16;
            public const long Setup14 = 12;
            public const long Setup15 = 23;
            public const long Setup16 = 31;
            public const long Puzzle1 = 1;
            public const long Setup2 = 2;
            public const long Puzzle2 = 2;
        }

        #region Puzzle 1

        private bool[] GetBinary(string line)
        {
            var array = new bool[line.Length * 4];
            for (var i = 0; i < line.Length; i++)
            {
                var chr = GetBinary(line[i]);
                for (var j = 0; j < 4; j++)
                    array[i * 4 + j] = chr[j] == '1';
            }

            return array;
        }

        private static string GetBinary(char chr)
        {
            return chr switch
            {
                '0' => "0000",
                '1' => "0001",
                '2' => "0010",
                '3' => "0011",
                '4' => "0100",
                '5' => "0101",
                '6' => "0110",
                '7' => "0111",
                '8' => "1000",
                '9' => "1001",
                'A' => "1010",
                'B' => "1011",
                'C' => "1100",
                'D' => "1101",
                'E' => "1110",
                'F' => "1111",
                _ => throw new Exception()
            };
        }

        private static long GetDecimalValue(bool[] input)
        {
            long result = 0;
            var len = input.Length;
            for (var i = 0; i < len; i++)
            {
                if (input[input.Length - i - 1])
                    result += (long)1 << i;
            }

            return result;
        }

        private static long ReadLiteralValue(bool[] input, out int endIndex)
        {
            endIndex = 5;

            var fullBitString = new List<bool[]>();
            while (endIndex -1 < input.Length)
            {
                var startIndex = endIndex - 5;
                fullBitString.Add(input[(startIndex + 1)..endIndex]);
                if (input[startIndex] == false)
                    break;
                endIndex += 5;
            }

            return GetDecimalValue(fullBitString.SelectMany(x => x).ToArray());
        }

        private object SolvePuzzle1(string input)
        {
            return GetValue(GetBinary(input), out _);
        }

        private static long GetValue(bool[] input, out int endIndex)
        {
            endIndex = 0;
            //var version = GetDecimalValue(input[..3]);
            var packetType = GetDecimalValue(input[3..6]);

            //Packet type 4 == literal value
            if (packetType == 4)
            {
                var result = ReadLiteralValue(input[6..], out endIndex);
                endIndex += 6;
                return result;
            }

            //Length type 0 = next 15 bits is the total number of bytes
            //Length type 1 = next 11 bits is the number of sub-packets
            var lengthTypeId = input[6];
            long sum = 0;
            if (lengthTypeId)
            {
                var leftover = input[18..];
                var numberOfPackages = GetDecimalValue(input[7..18]);
                for (var p = 0; p < numberOfPackages; p++)
                {
                    sum += GetValue(leftover, out endIndex);
                    leftover = leftover[endIndex..];
                }
            }
            else
            {
                var leftover = input[22..];
                var numberOfBytes = GetDecimalValue(input[7..22]);
                while (endIndex < numberOfBytes)
                {
                    sum += GetValue(leftover, out endIndex);
                    numberOfBytes -= endIndex;
                    leftover = leftover[endIndex..];
                }
            }

            return sum;
        }

        [DataTestMethod]
        [DataRow("111", 7)]
        [DataRow("100", 4)]
        [DataRow("1001", 9)]
        public void GetDecimalValue_SeveralTests(string bits, long expectedValue)
        {
            var arr = bits.Select(x => x == '1').ToArray();
            Assert.AreEqual(expectedValue, GetDecimalValue(arr));
        }

        [DataTestMethod]
        [DataRow("101111111000101000", 2021)]
        [DataRow("101111111000101002341324rqweafrsadfewwqrsfzvc0", 2021)]
        [DataRow("00001", 1)]
        [DataRow("01010", 10)]
        public void ReadLiteralValue_SeveralTests(string bits, long expectedValue)
        {
            var arr = bits.Select(x => x == '1').ToArray();
            Assert.AreEqual(expectedValue, ReadLiteralValue(arr, out _));
        }
        
        [DataTestMethod]
        [DataRow(0, Results.Setup10)]
        [DataRow(1, Results.Setup11)]
        [DataRow(2, Results.Setup12)]
        [DataRow(3, Results.Setup13)]
        public void Setup1(int line, long expected)
        {
            var input = InputReader.ReadInput();
            Assert.AreEqual(expected, SolvePuzzle1(input[line]));
        }

        [TestMethod]
        public void Puzzle1()
        {
            var input = InputReader.ReadInput();
            var result = SolvePuzzle1(input[0]);
            Assert.AreEqual(Results.Puzzle1, result);
        }

        #endregion

        #region Puzzle 2

        private object SolvePuzzle2(string[] input)
        {
            return 2;
        }

        [TestMethod]
        public void Setup2()
        {
            var input = InputReader.ReadInput();
            var result = SolvePuzzle2(input);
            Assert.AreEqual(Results.Setup2, result);
        }

        [TestMethod]
        public void Puzzle2()
        {
            var input = InputReader.ReadInput();
            var result = SolvePuzzle2(input);
            Assert.AreEqual(Results.Puzzle2, result);
        }

        #endregion
    }
}