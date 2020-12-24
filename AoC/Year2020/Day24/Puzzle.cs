using System;
using System.Collections.Generic;
using System.Linq;
using AoC.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Year2020.Day24
{
    [TestClass]
    public class Puzzle
    {
        #region Puzzle 1

        private object SolvePuzzle1(string[] input)
        {
            var handledTiles = GetFloor(input);

            return handledTiles.Count(x => x.Value);
        }

        private Dictionary<(decimal, int), bool> GetFloor(string[] input)
        {
            var handledTiles = new Dictionary<(decimal, int), bool>();

            foreach (var line in input)
            {
                var currentTile = GetEnd(line);

                if (handledTiles.ContainsKey(currentTile) && handledTiles[currentTile])
                    handledTiles[currentTile] = false;
                else
                    handledTiles[currentTile] = true;
            }

            return handledTiles;
        }

        private (decimal, int) GetEnd(string line)
        {
            var currentTile = (0m, 0);

            foreach (var direction in GetDirections(line))
            {
                currentTile = GetTile(direction, currentTile);
            }

            return currentTile;
        }

        private static (decimal, int) GetTile(Orientation direction, (decimal, int) currentTile)
        {
            switch (direction)
            {
                case Orientation.East:
                    currentTile.Item1++;
                    break;
                case Orientation.West:
                    currentTile.Item1--;
                    break;
                case Orientation.NorthEast:
                    currentTile.Item1 += 0.5m;
                    currentTile.Item2++;
                    break;
                case Orientation.SouthEast:
                    currentTile.Item1 += 0.5m;
                    currentTile.Item2--;
                    break;
                case Orientation.SouthWest:
                    currentTile.Item1 -= 0.5m;
                    currentTile.Item2--;
                    break;
                case Orientation.NorthWest:
                    currentTile.Item1 -= 0.5m;
                    currentTile.Item2++;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return currentTile;
        }

        public enum Orientation
        {
            NorthEast,
            East,
            SouthEast,
            SouthWest,
            West,
            NorthWest,
        }

        private IEnumerable<Orientation> GetDirections(string line)
        {
            for (var i = 0; i < line.Length; i++)
            {
                var chr = line[i];
                switch (chr)
                {
                    case 'e':
                        yield return Orientation.East;
                        break;
                    case 'w':
                        yield return Orientation.West;
                        break;
                    case 'n':
                    {
                        if (line[++i] == 'e')
                            yield return Orientation.NorthEast;
                        if (line[i] == 'w')
                            yield return Orientation.NorthWest;
                        break;
                    }
                    case 's':
                    {
                        if (line[++i] == 'e')
                            yield return Orientation.SouthEast;
                        if (line[i] == 'w')
                            yield return Orientation.SouthWest;
                        break;
                    }
                }
            }
        }

        [TestMethod]
        public void Setup0()
        {
            Assert.AreEqual((0, 0), GetEnd("nwwswee"));
        }

        [TestMethod]
        public void Setup1()
        {
            var input = InputReader.ReadInput();
            var result = SolvePuzzle1(input);
            Assert.AreEqual(10, result);
        }

        [TestMethod]
        public void Puzzle1()
        {
            var input = InputReader.ReadInput();
            var result = SolvePuzzle1(input);
            Assert.AreEqual(465, result);
        }

        #endregion

        #region Puzzle 2

        private object SolvePuzzle2(string[] input, int days)
        {
            var floor = GetFloor(input);

            for (var day = 0; day < days; day++)
            {
                floor = ExpandFloor(floor);
                var newFloor = new Dictionary<(decimal, int), bool>();

                foreach (var (tileKey, isBlack) in floor)
                {
                    var count = 0;
                    foreach (var adjTile in GetAdjacentTiles(tileKey))
                    {
                        if (floor.ContainsKey(adjTile) && floor[adjTile])
                            count++;
                    }

                    var flip = isBlack
                        ? (count == 0 || count > 2)
                        : count == 2;

                    if (flip)
                        newFloor[tileKey] = !isBlack;
                    else
                        newFloor[tileKey] = isBlack;
                }

                floor = newFloor;
            }

            return floor.Count(x => x.Value);
        }

        private Dictionary<(decimal, int), bool> ExpandFloor(Dictionary<(decimal, int), bool> oldFloor)
        {
            var newFloor = new Dictionary<(decimal, int), bool>();

            foreach (var tile in oldFloor)
            {
                newFloor[tile.Key] = tile.Value;
                foreach (var adjTile in GetAdjacentTiles(tile.Key))
                {
                    if (newFloor.ContainsKey(adjTile) == false)
                        newFloor.Add(adjTile, false);
                }
            }

            return newFloor;
        }

        private IEnumerable<(decimal, int)> GetAdjacentTiles((decimal, int) tile)
        {
            foreach (var dir in Enum.GetValues<Orientation>())
                yield return GetTile(dir, tile);
        }

        [TestMethod]
        public void Setup2()
        {
            var input = InputReader.ReadInput();
            Assert.AreEqual(10, SolvePuzzle2(input, 0));
            Assert.AreEqual(15, SolvePuzzle2(input, 1));
            Assert.AreEqual(12, SolvePuzzle2(input, 2));
            Assert.AreEqual(25, SolvePuzzle2(input, 3));
            Assert.AreEqual(132, SolvePuzzle2(input, 20));
            Assert.AreEqual(566, SolvePuzzle2(input, 50));
            Assert.AreEqual(2208, SolvePuzzle2(input, 100));
        }

        [TestMethod]
        public void Puzzle2()
        {
            var input = InputReader.ReadInput();
            var result = SolvePuzzle2(input, 100);
            Assert.AreEqual(4078, result);
        }

        #endregion
    }
}