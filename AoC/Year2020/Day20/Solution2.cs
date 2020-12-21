using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using AoC.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Year2020.Day20
{
    [TestClass]
    public class Solution2
    {
        public enum TileOrientation
        {
            Top,
            Right,
            Bottom,
            Left
        }

        public class Tile
        {
            public long Id { get; set; }
            public int RibSize { get; set; }
            public bool[,] Values { get; set; }

            public static Tile Parse(List<string> tileLines)
            {
                var title = tileLines[0];
                var id = title.Split(new[] { "Tile ", ":" }, StringSplitOptions.RemoveEmptyEntries).Single(x => string.IsNullOrWhiteSpace(x) == false);
                var size = tileLines[1].Length;
                var values = new bool[size, size];
                for (var i = 1; i < tileLines.Count; i++)
                {
                    var line = tileLines[i];
                    for (var j = 0; j < line.Length; j++)
                    {
                        values[i - 1, j] = line[j] == '#';
                    }
                }

                return new Tile
                {
                    Id = long.Parse(id),
                    Values = values,
                    RibSize = size
                };
            }

            public bool[] GetEdge(TileOrientation orientation)
            {
                return GetEdge(Values, orientation);
            }

            public static Tile RotateTile(Tile tile, int angle)
            {
                return new Tile
                {
                    Id = tile.Id,
                    Values = RotateValues(tile.Values, angle),
                    RibSize = tile.RibSize,
                };
            }

            public static bool[,] RotateValues(bool[,] values, int angle)
            {
                var size = values.GetLength(0);
                var newValues = new bool[size, size];

                if (angle % 90 != 0)
                    throw new ArgumentException();

                for (var i = 0; i < size; i++)
                    for (var j = 0; j < size; j++)
                    {
                        var k = size - i - 1;
                        var l = size - j - 1;

                        newValues[i, j] = angle switch
                        {
                            90 => values[l, i],
                            180 => values[k, l],
                            270 => values[j, k],
                            _ => values[i, j]
                        };
                    }

                return newValues;
            }

            public static Tile FlipTile(Tile tile, bool upDown)
            {
                return new Tile
                {
                    Id = tile.Id,
                    Values = FlipValues(tile.Values, upDown),
                    RibSize = tile.RibSize,
                };
            }

            public static bool[,] FlipValues(bool[,] values, bool upDown)
            {
                var size = values.GetLength(0);
                var newValues = new bool[size, size];

                for (var i = 0; i < size; i++)
                    for (var j = 0; j < size; j++)
                    {
                        var k = size - i - 1;
                        var l = size - j - 1;

                        newValues[i, j] = upDown switch
                        {
                            true => values[i, l],
                            false => values[k, j]
                        };
                    }

                return newValues;
            }

            private static bool[] GetEdge(bool[,] values, TileOrientation orientation)
            {
                var length = values.GetLength(0);
                var edge = new bool[length];
                switch (orientation)
                {
                    case TileOrientation.Top:
                    {
                        for (var i = 0; i < edge.Length; i++)
                            edge[i] = values[0, i];
                        break;
                    }
                    case TileOrientation.Bottom:
                    {
                        for (var i = 0; i < edge.Length; i++)
                            edge[i] = values[length - 1, i];
                        break;
                    }
                    case TileOrientation.Left:
                    {
                        for (var i = 0; i < edge.Length; i++)
                            edge[i] = values[i, 0];
                        break;
                    }
                    case TileOrientation.Right:
                    {
                        for (var i = 0; i < edge.Length; i++)
                            edge[i] = values[i, length - 1];
                        break;
                    }
                }

                return edge;
            }

            public bool IsMatch(Tile other, TileOrientation side)
            {
                return IsMatch(Values, other.Values, side);
            }

            public static bool IsMatch(bool[,] tile, bool[,] other, TileOrientation side)
            {
                var opposite = side switch
                {
                    TileOrientation.Top => TileOrientation.Bottom,
                    TileOrientation.Right => TileOrientation.Left,
                    TileOrientation.Bottom => TileOrientation.Top,
                    TileOrientation.Left => TileOrientation.Right,
                    _ => throw new ArgumentOutOfRangeException(nameof(side), side, null)
                };

                var edge = GetEdge(tile, side);
                var otherEdge = GetEdge(other, opposite);

                for (var i = 0; i < edge.Length; i++)
                    if (edge[i] != otherEdge[i])
                        return false;

                return true;
            }
        }

        private List<Tile> ReadTiles(string[] input)
        {
            var tiles = new List<Tile>();
            var tileLines = new List<string>();
            foreach (var line in input)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    var tile = Tile.Parse(tileLines);
                    tiles.Add(tile);
                    tileLines = new List<string>();
                }
                else
                {
                    tileLines.Add(line);
                }
            }

            if (tileLines.Any())
            {
                var tile = Tile.Parse(tileLines);
                tiles.Add(tile);
            }
            return tiles;
        }

        #region Puzzle 1

        private long SolvePuzzle1(string[] input)
        {
            var tiles = ReadTiles(input);
            var cornerTile = new List<Tile>();
            foreach (var tile in tiles)
            {
                var matched = 0;
                foreach (var orientation in Enum.GetValues<TileOrientation>())
                {
                    var hasMatch = false;

                    foreach (var otherTile in tiles.Where(x => x != tile))
                    {
                        foreach (var rotation in new[] { 0, 90, 180, 270 })
                        {
                            var rotatedTile = Tile.RotateTile(otherTile, rotation);

                            if (tile.IsMatch(rotatedTile, orientation)
                                || tile.IsMatch(Tile.FlipTile(rotatedTile, true), orientation)
                                || tile.IsMatch(Tile.FlipTile(rotatedTile, false), orientation))
                            {
                                hasMatch = true;
                            }
                        }
                    }

                    if (hasMatch)
                        matched++;
                }

                Assert.IsTrue(matched >= 2, tile.Id.ToString());

                if (matched == 2)
                    cornerTile.Add(tile);
            }

            Assert.AreEqual(4, cornerTile.Count);

            long product = 1;
            foreach (var tile in cornerTile)
                product *= tile.Id;
            return product;
        }

        private static bool ValidateGrid(Tile[,] grid, out int score)
        {
            score = 0;
            var maxScore = 0;
            for (var i = 0; i < grid.GetLength(0); i++)
            {
                for (var j = 0; j < grid.GetLength(1); j++)
                {
                    ValidateTileInGrid(grid, i, j, out var checks, out var matches);

                    score += matches;
                    maxScore += checks;
                }
            }

            return score == maxScore;
        }

        private static bool ValidateTileInGrid(Tile[,] grid, int i, int j, out int checks, out int matches)
        {
            var tile = grid[i, j];

            var isLeft = i == 0;
            var isRight = i == grid.GetLength(0) - 1;
            var isTop = j == 0;
            var isBottom = j == grid.GetLength(1) - 1;

            checks = 0;
            matches = 0;
            if (!isLeft)
            {
                checks++;
                var otherTile = grid[i - 1, j];
                if (tile.IsMatch(otherTile, TileOrientation.Left))
                    matches++;
            }

            if (!isRight)
            {
                checks++;
                var otherTile = grid[i + 1, j];
                if (tile.IsMatch(otherTile, TileOrientation.Right))
                    matches++;
            }

            if (!isTop)
            {
                checks++;
                var otherTile = grid[i, j - 1];
                if (tile.IsMatch(otherTile, TileOrientation.Top))
                    matches++;
            }

            if (!isBottom)
            {
                checks++;
                var otherTile = grid[i, j + 1];
                if (tile.IsMatch(otherTile, TileOrientation.Bottom))
                    matches++;
            }

            return matches == checks;
        }

        private static long GetCornerProduct(Tile[,] grid)
        {
            var tileLength = grid[0, 0].RibSize;
            for (var y = 0; y < grid.GetLength(1); y++)
            {
                Trace.WriteLine("");
                for (var y2 = 0; y2 < tileLength; y2++)
                {
                    Trace.WriteLine("");
                    for (var x = 0; x < grid.GetLength(0); x++)
                    {
                        var tile = grid[y, x];
                        Trace.Write(" ");

                        for (var x2 = 0; x2 < tileLength; x2++)
                            Trace.Write(tile.Values[x2, y2] ? "#" : ".");
                    }
                }
            }

            var max = grid.GetLength(0) - 1;
            var topLeft = grid[0, 0];
            var topRight = grid[0, max];
            var bottomLeft = grid[max, 0];
            var bottomRight = grid[max, max];

            return topLeft.Id * topRight.Id * bottomLeft.Id * bottomRight.Id;
        }

        [TestMethod]
        public void SetupReadTiles()
        {
            var input = InputReader.ReadInput();
            var tiles = ReadTiles(input);
            Assert.AreEqual(tiles.Count, 9);
            var firstTile = tiles[0];
            Assert.AreEqual(false, firstTile.Values[0, 0]);
            Assert.AreEqual(false, firstTile.Values[0, 1]);
            Assert.AreEqual(true, firstTile.Values[0, 2]);
            Assert.AreEqual(true, firstTile.Values[1, 0]);
            Assert.AreEqual(true, firstTile.Values[1, 1]);
            Assert.AreEqual(false, firstTile.Values[1, 2]);
        }

        [TestMethod]
        public void CheckRotate90()
        {
            var values = new[,] { { true, false }, { true, false } };
            var rotated = Tile.RotateValues(values, 90);

            Assert.AreEqual(rotated[0, 0], true);
            Assert.AreEqual(rotated[0, 1], true);
            Assert.AreEqual(rotated[1, 0], false);
            Assert.AreEqual(rotated[1, 1], false);
        }

        [TestMethod]
        public void CheckRotate180()
        {
            var values = new[,] { { true, false }, { true, false } };
            var rotated = Tile.RotateValues(values, 180);

            Assert.AreEqual(rotated[0, 0], false);
            Assert.AreEqual(rotated[0, 1], true);
            Assert.AreEqual(rotated[1, 0], false);
            Assert.AreEqual(rotated[1, 1], true);
        }

        [TestMethod]
        public void CheckRotate270()
        {
            var values = new[,] { { true, false }, { true, false } };
            var rotated = Tile.RotateValues(values, 270);

            Assert.AreEqual(rotated[0, 0], false);
            Assert.AreEqual(rotated[0, 1], false);
            Assert.AreEqual(rotated[1, 0], true);
            Assert.AreEqual(rotated[1, 1], true);
        }

        [TestMethod]
        public void CheckMatch()
        {
            var values1 = new[,] { { true, false }, { true, false } };
            var values2 = new[,] { { true, true }, { true, false } };

            Assert.IsTrue(Tile.IsMatch(values1, values2, TileOrientation.Top));
            Assert.IsFalse(Tile.IsMatch(values1, values2, TileOrientation.Bottom));
            Assert.IsFalse(Tile.IsMatch(values1, values2, TileOrientation.Left));
            Assert.IsFalse(Tile.IsMatch(values1, values2, TileOrientation.Right));
        }

        [TestMethod]
        public void Setup0()
        {
            var input = InputReader.ReadInput("setup0");
            var result = SolvePuzzle1(input);
            Assert.AreEqual(4389, result);
        }

        [TestMethod]
        public void Setup1()
        {
            var input = InputReader.ReadInput();
            var result = SolvePuzzle1(input);
            Assert.AreEqual(20899048083289, result);
        }

        [TestMethod]
        public void Puzzle1()
        {
            var input = InputReader.ReadInput();
            var result = SolvePuzzle1(input);
            Assert.AreEqual(8425574315321, result);
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
            Assert.AreEqual(2, result);
        }

        [TestMethod]
        public void Puzzle2()
        {
            var input = InputReader.ReadInput();
            var result = SolvePuzzle2(input);
            Assert.AreEqual(2, result);
        }

        #endregion
    }
}