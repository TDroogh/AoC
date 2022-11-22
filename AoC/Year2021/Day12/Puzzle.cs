using AoC.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Year2021.Day12
{
    [TestClass]
    public class Puzzle
    {
        private static class Results
        {
            public const int Setup11 = 10;
            public const int Setup12 = 19;
            public const int Setup13 = 226;
            public const int Puzzle1 = 4241;
            public const int Setup21 = 36;
            public const int Setup22 = 103;
            public const int Setup23 = 3509;
            public const int Puzzle2 = 122134;
        }

        private class CaveSystem
        {
            public bool AllowDoubleSmallCaveVisit { get; set; }
            public List<Cave> Caves { get; set; }
            public List<Connection> Connections { get; set; }

            public Cave GetOrAdd(string name)
            {
                var cave = Caves.SingleOrDefault(x => x.Name == name);
                if (cave != null)
                    return cave;

                cave = new Cave
                {
                    Name = name,
                    Type = name switch
                    {
                        "start" => CaveType.Start,
                        "end" => CaveType.End,
                        _ => name.All(char.IsUpper)
                            ? CaveType.Big
                            : CaveType.Small
                    }
                };
                Caves.Add(cave);
                return cave;
            }

            public static CaveSystem Parse(string[] input, int nrOfVisits)
            {
                var system = new CaveSystem
                {
                    Caves = new List<Cave>(),
                    Connections = new List<Connection>(),
                    AllowDoubleSmallCaveVisit = nrOfVisits == 2
                };

                foreach (var line in input)
                {
                    var split = line.Split("-");

                    system.Connections.Add(new Connection
                    {
                        From = system.GetOrAdd(split[0]),
                        To = system.GetOrAdd(split[1])
                    });
                }

                return system;
            }

            public int CalculateNumberOfPaths()
            {
                var start = Caves.Single(x => x.Type == CaveType.Start);
                var end = Caves.Single(x => x.Type == CaveType.End);

                return CalculateNumberOfPaths(start, end, "-", !AllowDoubleSmallCaveVisit);
            }

            public int CalculateNumberOfPaths(Cave from, Cave final, string path, bool hadDoubleSmallCaveVisit)
            {
                var totalCount = 0;
                path += $"{from.Name}-";
                Console.WriteLine($"{path}");
                foreach (var to in GetDestinations(from))
                {
                    var doubleVisit = hadDoubleSmallCaveVisit;
                    if (to == final)
                    {
                        totalCount++;
                        continue;
                    }

                    if (to.Type != CaveType.Big)
                    {
                        if (to.Type == CaveType.Start)
                            continue;

                        var count = path.Split("-").Count(x => x == to.Name);

                        switch (count)
                        {
                            case > 1:
                            case 1 when doubleVisit:
                                continue;
                            case 1:
                                doubleVisit = true;
                                break;
                        }
                    }

                    totalCount += CalculateNumberOfPaths(to, final, path, doubleVisit);
                }

                Console.WriteLine($"From {path} to {final.Name}: {totalCount} possibilities");
                return totalCount;
            }

            public IEnumerable<Cave> GetDestinations(Cave from)
            {
                foreach (var option in Connections.Where(x => x.From == from))
                    yield return option.To;
                foreach (var option in Connections.Where(x => x.To == from))
                    yield return option.From;
            }

            /// <inheritdoc />
            public override string ToString()
            {
                return Caves.Aggregate("Cave system:", (p, n) => $"{p}\r\n{n.Name} ({n.Type}) (paths to {Connections.Where(x => x.From == n).Aggregate("", (x, y) => $"{x},{y.To.Name}")})");
            }
        }

        private class Cave
        {
            public string Name { get; set; }
            public CaveType Type { get; set; }
        }

        private class Connection
        {
            public Cave From { get; set; }
            public Cave To { get; set; }
        }

        private enum CaveType
        {
            Start,
            Small,
            Big,
            End
        }

        #region Puzzle 1

        private object SolvePuzzle1(string[] input)
        {
            var system = CaveSystem.Parse(input, 1);
            Console.WriteLine(system);

            return system.CalculateNumberOfPaths();
        }

        [DataTestMethod]
        [DataRow("setup1", Results.Setup11)]
        [DataRow("setup2", Results.Setup12)]
        [DataRow("setup3", Results.Setup13)]
        public void Setup1(string name, int expected)
        {
            var input = InputReader.ReadInput(name);
            var result = SolvePuzzle1(input);
            Assert.AreEqual(expected, result);
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
            var system = CaveSystem.Parse(input, 2);
            Console.WriteLine(system);

            return system.CalculateNumberOfPaths();
        }

        [DataTestMethod]
        [DataRow("setup1", Results.Setup21)]
        [DataRow("setup2", Results.Setup22)]
        [DataRow("setup3", Results.Setup23)]
        public void Setup2(string name, int expected)
        {
            var input = InputReader.ReadInput(name);
            var result = SolvePuzzle2(input);
            Assert.AreEqual(expected, result);
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