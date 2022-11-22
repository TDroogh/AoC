using AoC.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AoC.Year2021.Day16
{
    [TestClass]
    public class Puzzle
    {
        private static class Results
        {
            public const long Setup10 = 6;
            public const long Setup11 = 9;
            public const long Setup12 = 14;
            public const long Setup13 = 16;
            public const long Setup14 = 12;
            public const long Setup15 = 23;
            public const long Setup16 = 31;
            public const long Puzzle1 = 875;
            public const long Setup20 = 2021;
            public const long Setup27 = 3;
            public const long Setup28 = 54;
            public const long Setup29 = 7;
            public const long Setup210 = 1;
            public const long Setup211 = 0;
            public const long Setup212 = 0;
            public const long Setup213 = 1;
            public const long Puzzle2 = 1264857437203;
        }

        public abstract class Packet
        {
            public long Version { get; set; }
            public long Type { get; set; }
            public abstract IEnumerable<Packet> GetAllPackets();
            public abstract long GetResult();

            public static Packet Create(long version, long type)
            {
                return type == 4
                    ? new LiteralPacket { Version = version, Type = type }
                    : new OperatorPacket { Version = version, Type = type };
            }
        }

        public class LiteralPacket : Packet
        {
            public long LiteralValue { get; set; }

            public override IEnumerable<Packet> GetAllPackets()
            {
                yield return this;
            }

            /// <inheritdoc />
            public override long GetResult()
            {
                return LiteralValue;
            }

            /// <inheritdoc />
            public override string ToString()
            {
                return LiteralValue.ToString();
            }
        }

        public class OperatorPacket : Packet
        {
            public OperatorPacket()
            {
                SubPackets = new List<Packet>();
            }

            public List<Packet> SubPackets { get; }

            public override IEnumerable<Packet> GetAllPackets()
            {
                yield return this;
                foreach (var packet in SubPackets.SelectMany(x => x.GetAllPackets()))
                    yield return packet;
            }

            /// <inheritdoc />
            public override long GetResult()
            {
                return Type switch
                {
                    0 => SubPackets.Select(x => x.GetResult()).Sum(),
                    1 => GetProduct(SubPackets),
                    2 => SubPackets.Select(x => x.GetResult()).Min(),
                    3 => SubPackets.Select(x => x.GetResult()).Max(),
                    5 => SubPackets[0].GetResult() > SubPackets[1].GetResult() ? 1 : 0,
                    6 => SubPackets[0].GetResult() < SubPackets[1].GetResult() ? 1 : 0,
                    7 => SubPackets[0].GetResult() == SubPackets[1].GetResult() ? 1 : 0,
                    _ => throw new ArgumentOutOfRangeException(nameof(Type), Type, "Invalid type")
                };
            }

            private long GetProduct(IEnumerable<Packet> packets)
            {
                long product = 1;
                foreach (var value in packets.Select(x => x.GetResult()))
                    product *= value;
                return product;
            }

            /// <inheritdoc />
            public override string ToString()
            {
                var expression = Type switch
                {
                    0 => string.Join(" + ", SubPackets.Select(x => x.ToString())),
                    1 => string.Join(" * ", SubPackets.Select(x => x.ToString())),
                    2 => $"Min({string.Join(", ", SubPackets.Select(x => x.ToString()))})",
                    3 => $"Max({string.Join(", ", SubPackets.Select(x => x.ToString()))})",
                    5 => SubPackets[0] + " < " + SubPackets[1],
                    6 => SubPackets[0] + " > " + SubPackets[1],
                    7 => SubPackets[0] + " == " + SubPackets[1],
                    _ => throw new ArgumentOutOfRangeException(nameof(Type), Type, "Invalid type")
                };

                return $"({expression})";
            }
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
            while (endIndex - 1 < input.Length)
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
            return ReadPacket(GetBinary(input), out _)
                .GetAllPackets()
                .Sum(x => x.Version);
        }

        private static Packet ReadPacket(bool[] input, out int endIndex)
        {
            endIndex = 0;
            var version = GetDecimalValue(input[..3]);
            var packetType = GetDecimalValue(input[3..6]);
            var packet = Packet.Create(version, packetType);

            //Packet type 4 == literal value
            if (packet is LiteralPacket literal)
            {
                var result = ReadLiteralValue(input[6..], out endIndex);
                endIndex += 6;
                literal.LiteralValue = result;
            }
            else if (packet is OperatorPacket op)
            {
                //Length type 0 = next 15 bits is the total number of bytes
                //Length type 1 = next 11 bits is the number of sub-packets
                var lengthTypeId = input[6];
                if (lengthTypeId)
                {
                    var leftover = input;
                    endIndex = 18;
                    var subEndIndex = 18;
                    var numberOfPackages = GetDecimalValue(input[7..18]);
                    for (var p = 0; p < numberOfPackages; p++)
                    {
                        leftover = leftover[subEndIndex..];
                        op.SubPackets.Add(ReadPacket(leftover, out subEndIndex));
                        endIndex += subEndIndex;
                    }
                }
                else
                {
                    var leftover = input;
                    endIndex = 22;
                    var subEndIndex = 22;
                    var numberOfBytes = GetDecimalValue(input[7..22]);
                    var bytesRead = 0;
                    do
                    {
                        leftover = leftover[subEndIndex..];
                        op.SubPackets.Add(ReadPacket(leftover, out subEndIndex));
                        bytesRead += subEndIndex;
                        endIndex += subEndIndex;
                    } while (bytesRead < numberOfBytes);
                }
            }

            return packet;
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
        [DataRow(4, Results.Setup14)]
        [DataRow(5, Results.Setup15)]
        [DataRow(6, Results.Setup16)]
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

        private object SolvePuzzle2(string input)
        {
            var packet = ReadPacket(GetBinary(input), out _);

            Trace.WriteLine(packet);

            return packet.GetResult();
        }

        [DataTestMethod]
        [DataRow(0, Results.Setup20)]
        [DataRow(7, Results.Setup27)]
        [DataRow(8, Results.Setup28)]
        [DataRow(9, Results.Setup29)]
        [DataRow(10, Results.Setup210)]
        [DataRow(11, Results.Setup211)]
        [DataRow(12, Results.Setup212)]
        [DataRow(13, Results.Setup213)]
        public void Setup2(int line, long expected)
        {
            var input = InputReader.ReadInput();
            var result = SolvePuzzle2(input[0]);
            Assert.AreEqual(Results.Setup20, result);
        }

        [TestMethod]
        public void Puzzle2()
        {
            var input = InputReader.ReadInput();
            var result = SolvePuzzle2(input[0]);
            Assert.AreEqual(Results.Puzzle2, result);
        }

        #endregion
    }
}