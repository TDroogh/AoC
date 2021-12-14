using System;
using System.Collections.Generic;
using System.Linq;
using AoC.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Year2021.Day13
{
    [TestClass]
    public class Puzzle
    {
        private static class Results
        {
            public const int Setup1 = 17;
            public const int Puzzle1 = 1;
            public const int Setup2 = 16;
            public const int Puzzle2 = 2;
        }

        public class Instructions
        {
            public List<(int, int)> Points { get; set; }
            public List<FoldingInstruction> FoldingInstructions { get; set; }

            public static Instructions Parse(string[] input)
            {
                var instructions = new Instructions
                {
                    FoldingInstructions = new List<FoldingInstruction>(),
                    Points = new List<(int, int)>()
                };

                foreach (var line in input)
                {
                    if (line.StartsWith("fold along "))
                    {
                        instructions.FoldingInstructions.Add(FoldingInstruction.Parse(line));
                        continue;
                    }
                    if (string.IsNullOrEmpty(line))
                        continue;

                    var coord = line.Split(",");
                    instructions.Points.Add((int.Parse(coord[0]), int.Parse(coord[1])));
                }

                return instructions;
            }
        }

        public class FoldingInstruction
        {
            public bool Horizontal { get; set; }
            public int Number { get; set; }

            public static FoldingInstruction Parse(string input)
            {
                input = input.Replace("fold along ", "");
                var split = input.Split("=");

                return new FoldingInstruction
                {
                    Number = int.Parse(split[1]),
                    Horizontal = split[0] == "x"
                };
            }

            public bool[,] Apply(bool[,] input)
            {
                if (Horizontal)
                {
                    var cols = (input.GetLength(0) - 1) / 2;
                    var rows = input.GetLength(1);
                    var output = new bool[cols, rows];

                    for (var x = 0; x < cols; x++)
                    {
                        var x2 = input.GetLength(0) - 1 - x;
                        for (var y = 0; y < rows; y++)
                        {
                            output[x, y] = input[x, y] | input[x2, y];
                        }
                    }

                    return output;
                }
                else
                {
                    var cols = input.GetLength(0);
                    var rows = (input.GetLength(1) - 1) / 2;
                    var output = new bool[cols, rows];

                    for (var y = 0; y < rows; y++)
                    {
                        var y2 = input.GetLength(1) - 1 - y;
                        for (var x = 0; x < cols; x++)
                        {
                            output[x, y] = input[x, y] | input[x, y2];
                        }
                    }

                    return output;
                }
            }
        }

        #region Puzzle 1

        private object SolvePuzzle1(string[] input)
        {
            var instructions = Instructions.Parse(input);
            var lenX1 = instructions.Points.Max(x => x.Item1) + 1;
            var lenX2 = instructions.FoldingInstructions.Where(x => x.Horizontal).Max(x => x.Number) * 2 + 1;
            var lenX = Math.Max(lenX1, lenX2);
            //if (lenX % 2 == 0)
            //    lenX++;
            var lenY1 = instructions.Points.Max(x => x.Item2) + 1;
            var lenY2 = instructions.FoldingInstructions.Where(x => !x.Horizontal).Max(x => x.Number) * 2 + 1;
            var lenY = Math.Max(lenY1, lenY2);
            //if (lenY % 2 == 0)
            //    lenY++;
            Console.WriteLine($"Grid [{lenX1} {lenX2} {lenX}, {lenY1} {lenY2} {lenY}]");
            var grid = new bool[lenX, lenY];

            foreach (var (x, y) in instructions.Points)
                grid[x, y] = true;

            var newGrid = instructions.FoldingInstructions[0].Apply(grid);
            return newGrid.GetAllValues().Count(x => x);
        }

        [TestMethod]
        public void Setup1()
        {
            var input = InputReader.ReadInput();
            var result = SolvePuzzle1(input);
            Assert.AreEqual(Results.Setup1, result);
        }

        [TestMethod]
        public void Puzzle1()
        {
            var input = InputReader.ReadInput();
            var result = SolvePuzzle1(input);
            Assert.AreEqual(Results.Puzzle1, result);
        }

        #endregion

        #region Puzzle 2

        private object SolvePuzzle2(string[] input)
        {
            var instructions = Instructions.Parse(input);
            var lenX1 = instructions.Points.Max(x => x.Item1) + 1;
            var lenX2 = instructions.FoldingInstructions.Where(x => x.Horizontal).Max(x => x.Number) * 2 + 1;
            var lenX = Math.Max(lenX1, lenX2);
            //if (lenX % 2 == 0)
            //    lenX++;
            var lenY1 = instructions.Points.Max(x => x.Item2) + 1;
            var lenY2 = instructions.FoldingInstructions.Where(x => !x.Horizontal).Max(x => x.Number) * 2 + 1;
            var lenY = Math.Max(lenY1, lenY2);
            //if (lenY % 2 == 0)
            //    lenY++;
            Console.WriteLine($"Grid [{lenX1} {lenX2} {lenX}, {lenY1} {lenY2} {lenY}]");
            var grid = new bool[lenX, lenY];

            foreach (var (x, y) in instructions.Points)
                grid[x, y] = true;

            var newGrid = instructions.FoldingInstructions.Aggregate(grid, (p, n) => n.Apply(p));
            for (var y = 0; y < newGrid.GetLength(1); y++)
            {
                var line = "";

                for (var x = 0; x < newGrid.GetLength(0); x++)
                {
                    line += newGrid[x, y] ? "X" : ".";
                }

                Console.WriteLine(line);
            }

            return newGrid.GetAllValues().Count(x => x);
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