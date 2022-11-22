using AoC.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace AoC.Year2015.Day06
{
    [TestClass]
    public class Puzzle
    {
        #region Puzzle 1

        private enum InstructionType
        {
            Off,
            On,
            Toggle
        }

        private class Instruction
        {
            public InstructionType Type { get; set; }
            public int FromX { get; set; }
            public int FromY { get; set; }
            public int ToX { get; set; }
            public int ToY { get; set; }

            public static InstructionType GetType(string input)
            {
                if (input.StartsWith("toggle", StringComparison.Ordinal))
                    return InstructionType.Toggle;
                if (input.StartsWith("turn on", StringComparison.Ordinal))
                    return InstructionType.On;
                return InstructionType.Off;
            }

            public static Instruction Parse(string input)
            {
                //turn on 0,0 through 999,999
                Console.WriteLine(input);
                var type = GetType(input);
                input = input.Replace("turn ", "");
                var values = input.Split(" ");
                var from = values[1].Split(",").Select(int.Parse).ToArray();
                var to = values[3].Split(",").Select(int.Parse).ToArray();

                return new Instruction
                {
                    Type = type,
                    FromX = from[0],
                    FromY = from[1],
                    ToX = to[0],
                    ToY = to[1],
                };
            }
        }

        private int SolvePuzzle1(params string[] input)
        {
            var grid = new bool[1000, 1000];

            foreach (var line in input)
            {
                var ins = Instruction.Parse(line);

                for (var x = ins.FromX; x <= ins.ToX; x++)
                {
                    for (var y = ins.FromY; y <= ins.ToY; y++)
                    {
                        grid[x, y] = ins.Type switch
                        {
                            InstructionType.Off => false,
                            InstructionType.On => true,
                            InstructionType.Toggle => !grid[x, y],
                            _ => throw new ArgumentOutOfRangeException()
                        };
                    }
                }
            }

            var sum = 0;
            for (var x = 0; x < 1000; x++)
            {
                for (var y = 0; y < 1000; y++)
                {
                    if (grid[x, y])
                        sum++;
                }
            }

            return sum;
        }

        [TestMethod]
        public void Setup1()
        {
            var result = SolvePuzzle1("turn on 0,0 through 999,999");
            Assert.AreEqual(1_000_000, result);

            result = SolvePuzzle1("turn on 250,250 through 749,749");
            Assert.AreEqual(250_000, result);

            result = SolvePuzzle1("toggle 250,250 through 749,749");
            Assert.AreEqual(250_000, result);
        }

        [TestMethod]
        public void Puzzle1()
        {
            var input = InputReader.ReadInput();
            var result = SolvePuzzle1(input);
            Assert.AreEqual(377891, result);
        }

        #endregion

        #region Puzzle 2

        private int SolvePuzzle2(params string[] input)
        {
            var grid = new int[1000, 1000];

            foreach (var line in input)
            {
                var ins = Instruction.Parse(line);

                for (var x = ins.FromX; x <= ins.ToX; x++)
                {
                    for (var y = ins.FromY; y <= ins.ToY; y++)
                    {
                        grid[x, y] = ins.Type switch
                        {
                            InstructionType.Off when grid[x, y] > 0 => grid[x, y] - 1,
                            InstructionType.Off when grid[x, y] == 0 => 0,
                            InstructionType.On => grid[x, y] + 1,
                            InstructionType.Toggle => grid[x, y] + 2,
                            _ => throw new ArgumentOutOfRangeException()
                        };
                    }
                }
            }

            var sum = 0;
            for (var x = 0; x < 1000; x++)
            {
                for (var y = 0; y < 1000; y++)
                {
                    sum += grid[x, y];
                }
            }

            return sum;
        }

        [TestMethod]
        public void Setup2()
        {
            var result = SolvePuzzle2("turn on 0,0 through 999,999");
            Assert.AreEqual(1_000_000, result);

            result = SolvePuzzle2("turn on 250,250 through 749,749");
            Assert.AreEqual(250_000, result);

            result = SolvePuzzle2("toggle 250,250 through 749,749");
            Assert.AreEqual(500_000, result);
        }

        [TestMethod]
        public void Puzzle2()
        {
            var input = InputReader.ReadInput();
            var result = SolvePuzzle2(input);
            Assert.AreEqual(14110788, result);
        }

        #endregion
    }
}