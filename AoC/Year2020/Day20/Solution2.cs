using System.Diagnostics;

namespace AoC.Year2020.Day20
{
    [TestClass]
    public class Solution2
    {
        public enum TileOrientation
        {
            Top = 1,
            Right,
            Bottom,
            Left
        }

        public enum TilePosition
        {
            Undefined = 1,
            Corner,
            Edge,
            Center
        }

        public record TileMatch
        {
            public required TileOrientation Orientation { get; init; }
            public required TileVariant Other { get; init; }
        }

        public record TileVariant
        {
            public required Tile Tile { get; init; }
            public required Tile Original { get; init; }
            public required int Rotation { get; init; }
            public required bool Flipped { get; init; }
        }

        public class Tile
        {
            public Tile(long id, int ribSize, bool[,] values, TilePosition position, List<TileMatch> matches)
            {
                Id = id;
                RibSize = ribSize;
                Values = values;
                Position = position;
                Matches = matches;
            }

            public Tile(Tile other, bool[,] values) : this(other.Id, other.RibSize, values, other.Position, other.Matches)
            {
                Assert.AreNotEqual(other.Position, (TilePosition)0);
            }

            public long Id { get; }
            public int RibSize { get; }
            public bool[,] Values { get; }
            public TilePosition Position { get; set; }
            public List<TileMatch> Matches { get; }

            public static Tile Parse(List<string> tileLines)
            {
                var title = tileLines[0];
                var id = title.Split(new[] { "Tile ", ":" }, StringSplitOptions.RemoveEmptyEntries).Single(x => string.IsNullOrWhiteSpace(x) == false);
                var values = ParseValues(tileLines.Skip(1));

                Assert.AreEqual(values.GetLength(0), values.GetLength(1));

                return new Tile(long.Parse(id), values.GetLength(0), values, TilePosition.Undefined, new List<TileMatch>());
            }

            public static bool[,] ParseValues(IEnumerable<string> tileLines)
            {
                var lines = tileLines.Where(x => string.IsNullOrWhiteSpace(x) == false).ToList();
                var size = lines[0].Length;
                var values = new bool[lines.Count, size];
                for (var i = 0; i < lines.Count; i++)
                {
                    var line = lines[i];
                    for (var j = 0; j < line.Length; j++)
                    {
                        values[i, j] = line[j] == '#';
                    }
                }

                return values;
            }

            public IEnumerable<TileVariant> GetVariants()
            {
                foreach (var rotation in new[] { 0, 90, 180, 270 })
                {
                    var rotatedTile = RotateTile(this, rotation);

                    yield return new TileVariant
                    {
                        Tile = rotatedTile,
                        Original = this,
                        Flipped = false,
                        Rotation = rotation,
                    };
                    yield return new TileVariant
                    {
                        Tile = FlipTile(rotatedTile, true),
                        Original = this,
                        Flipped = true,
                        Rotation = rotation,
                    };
                }
            }

            public static IEnumerable<bool[,]> GetVariants(bool[,] original)
            {
                foreach (var rotation in new[] { 0, 90, 180, 270 })
                {
                    var rotatedTile = RotateValues(original, rotation);

                    yield return rotatedTile;
                    yield return FlipValues(rotatedTile, true);
                }
            }

            public bool[] GetEdge(TileOrientation orientation)
            {
                return GetEdge(Values, orientation);
            }

            public static Tile RotateTile(Tile tile, int angle)
            {
                return new Tile(tile, RotateValues(tile.Values, angle));
            }

            public static bool[,] RotateValues(bool[,] values, int angle)
            {
                var size = values.GetLength(0);
                var newValues = new bool[size, size];

                if (angle % 90 != 0)
                    throw new ArgumentException();

                for (var i = 0; i < size; i++)
                {
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
                }

                return newValues;
            }

            public static Tile FlipTile(Tile tile, bool upDown)
            {
                return new Tile(tile, FlipValues(tile.Values, upDown));
            }

            public static bool[,] FlipValues(bool[,] values, bool upDown)
            {
                var size = values.GetLength(0);
                var newValues = new bool[size, size];

                for (var i = 0; i < size; i++)
                {
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
                }

                return newValues;
            }

            private static bool[] GetEdge(bool[,] values, TileOrientation orientation)
            {
                var length = values.GetLength(0);
                var edge = new bool[length];
                for (var i = 0; i < edge.Length; i++)
                    edge[i] = orientation switch
                    {
                        TileOrientation.Top => values[0, i],
                        TileOrientation.Bottom => values[length - 1, i],
                        TileOrientation.Left => values[i, 0],
                        TileOrientation.Right => values[i, length - 1],
                        _ => edge[i]
                    };

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
            SortTiles(tiles);
            return tiles.Where(x => x.Position == TilePosition.Corner).Aggregate(1L, (current, tile) => current * tile.Id);
        }

        private long SolvePuzzle1B(string[] input)
        {
            var tiles = ReadTiles(input);
            SortTiles(tiles);
            var grid = BuildGrid(tiles);

            var l = grid.GetLength(0) - 1;
            return grid[0, 0].Id * grid[0, l].Id * grid[l, 0].Id * grid[l, l].Id;
        }

        private static void SortTiles(List<Tile> tiles)
        {
            foreach (var tile in tiles)
            {
                var matched = 0;
                foreach (var orientation in Enum.GetValues<TileOrientation>())
                {
                    var hasMatch = false;
                    foreach (var other in tiles.Where(x => x != tile))
                    {
                        foreach (var match in other
                                     .GetVariants()
                                     .Where(x => tile.IsMatch(x.Tile, orientation))
                                     .Select(x => new TileMatch { Orientation = orientation, Other = x }))
                        {
                            hasMatch = true;
                            tile.Matches.Add(match);
                        }
                    }

                    if (hasMatch)
                        matched++;
                }

                Assert.IsTrue(matched >= 2 && matched <= 4, tile.Id.ToString());

                tile.Position = matched switch
                {
                    2 => TilePosition.Corner,
                    3 => TilePosition.Edge,
                    4 => TilePosition.Center,
                    _ => default
                };
            }

            foreach (var match in tiles.SelectMany(x => x.Matches))
                match.Other.Tile.Position = match.Other.Original.Position;

            var centerRibSize = (int)Math.Sqrt(tiles.Count) - 2;
            Assert.AreEqual(4, tiles.Count(x => x.Position == TilePosition.Corner));
            Assert.AreEqual(4 * centerRibSize, tiles.Count(x => x.Position == TilePosition.Edge));
            Assert.AreEqual(centerRibSize * centerRibSize, tiles.Count(x => x.Position == TilePosition.Center));

            foreach (var cornerTile in tiles.Where(x => x.Position == TilePosition.Corner))
            {
                Trace.WriteLine($"Tile {cornerTile.Id}----------");
                foreach (var match in cornerTile.Matches)
                {
                    Trace.WriteLine($"- {match.Orientation}: {match.Other.Original.Id} {match.Other.Rotation} degrees {(match.Other.Flipped ? "flipped" : "not flipped")}");
                }
            }
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
        public void Setup1()
        {
            var input = InputReader.ReadInput();
            var result = SolvePuzzle1(input);
            Assert.AreEqual(20899048083289, result);
        }

        [TestMethod]
        public void Setup1B()
        {
            var input = InputReader.ReadInput();
            var result = SolvePuzzle1B(input);
            Assert.AreEqual(20899048083289, result);
        }

        [TestMethod]
        public void Puzzle1()
        {
            var input = InputReader.ReadInput();
            var result = SolvePuzzle1(input);
            Assert.AreEqual(8425574315321, result);
        }

        [TestMethod]
        public void Puzzle1B()
        {
            var input = InputReader.ReadInput();
            var result = SolvePuzzle1B(input);
            Assert.AreEqual(8425574315321, result);
        }

        #endregion

        #region Puzzle 2

        private static IEnumerable<Tile> GetTilesInGrid(Tile?[,] grid)
        {
            for (var i = 0; i < grid.GetLength(0); i++)
            {
                for (var j = 0; j < grid.GetLength(1); j++)
                {
                    if (grid[i, j] != null)
                        yield return grid[i, j]!;
                }
            }
        }

        private static Tile[,] BuildGrid(List<Tile> tiles)
        {
            var gridSize = (int)Math.Sqrt(tiles.Count);
            var grid = new Tile?[gridSize, gridSize];

            for (var i = 0; i < gridSize; i++)
            {
                for (var j = 0; j < gridSize; j++)
                {
                    if (i == 0 && j == 0)
                    {
                        var first = tiles.FirstOrDefault(x => x.Position == TilePosition.Corner);
                        Assert.IsNotNull(first);
                        var bottom = first.Matches.Any(x => x.Orientation == TileOrientation.Bottom);
                        var right = first.Matches.Any(x => x.Orientation == TileOrientation.Right);

                        var rotation = 0;
                        if (!bottom && right)
                            rotation = 90;
                        if (!bottom && !right)
                            rotation = 180;
                        if (bottom && !right)
                            rotation = 270;

                        grid[i, j] = Tile.RotateTile(first, rotation);
                        Trace.WriteLine($"{i},{j}: [{grid[i, j]!.Id}]");
                    }
                    else
                    {
                        var iPrev = i - 1;
                        var jPrev = j;

                        if (i == 0)
                        {
                            iPrev = i;
                            jPrev = j - 1;
                        }

                        var isEdge = i == 0 || i == gridSize - 1 || j == 0 || j == gridSize - 1;
                        var isCorner = (i == 0 || i == gridSize - 1) && (j == 0 || j == gridSize - 1);
                        var type = isCorner
                            ? TilePosition.Corner
                            : isEdge
                                ? TilePosition.Edge
                                : TilePosition.Center;

                        var previousTile = grid[iPrev, jPrev]!;
                        var included = GetTilesInGrid(grid).Select(x => x.Id);

                        var valid = false;

                        foreach (var match in previousTile.Matches.Where(x => x.Other.Tile.Position == type && included.Contains(x.Other.Original.Id) == false))
                        {
                            foreach (var option in match.Other.Tile.GetVariants())
                            {
                                grid[i, j] = option.Tile;
                                ValidateTileInGrid(grid, iPrev, jPrev, out var checks, out var matches);
                                if (checks == matches)
                                {
                                    valid = true;
                                    break;
                                }
                            }
                        }

                        Trace.WriteLine($"{i},{j}: [{grid[i, j]!.Id} ({grid[i, j]!.Position})] (prev was {grid[iPrev, jPrev]!.Id})");

                        Assert.IsTrue(valid);
                    }
                }
            }

            Assert.IsTrue(ValidateGrid(grid!));
            return grid!;
        }

        private static bool ValidateGrid(Tile[,] grid)
        {
            var valid = true;
            for (var i = 0; i < grid.GetLength(0); i++)
            {
                for (var j = 0; j < grid.GetLength(1); j++)
                {
                    valid &= ValidateTileInGrid(grid, i, j, out _, out _);
                }
            }

            return valid;
        }

        private static bool ValidateTileInGrid(Tile?[,] grid, int i, int j, out int checks, out int matches)
        {
            var tile = grid[i, j];
            if (tile == null)
            {
                checks = 1;
                matches = 0;
                return false;
            }

            var numChecks = 0;
            var numMatches = 0;
            var foundNull = false;

            void ValidateOrientation(Tile? other, TileOrientation orientation)
            {
                if (other == null)
                {
                    foundNull = true;
                    return;
                }

                numChecks++;
                if (tile.IsMatch(other, orientation))
                    numMatches++;
            }

            if (i != 0)
                ValidateOrientation(grid[i - 1, j], TileOrientation.Left);

            if (i != grid.GetLength(0) - 1)
                ValidateOrientation(grid[i + 1, j], TileOrientation.Right);

            if (j != 0)
                ValidateOrientation(grid[i, j - 1], TileOrientation.Top);

            if (j != grid.GetLength(1) - 1)
                ValidateOrientation(grid[i, j + 1], TileOrientation.Bottom);

            checks = numChecks;
            matches = numMatches;

            return !foundNull && matches == checks;
        }

        private static bool[,] DrawGrid(Tile[,] grid)
        {
            var gridSize = grid.GetLength(0);
            var tileSize = grid[0, 0].RibSize - 2;
            var size = gridSize * tileSize;
            var drawing = new bool[size, size];
            Trace.WriteLine("-------------------");
            for (var j1 = 0; j1 < gridSize; j1++)
            {
                for (var j2 = 0; j2 < tileSize; j2++)
                {
                    Trace.WriteLine("");
                    for (var i1 = 0; i1 < gridSize; i1++)
                    {
                        var tile = grid[j1, i1];
                        Trace.Write(" ");
                        for (var i2 = 0; i2 < tileSize; i2++)
                        {
                            var val = tile.Values[i2 + 1, j2 + 1];
                            drawing[i1 * tileSize + i2, j1 * tileSize + j2] = val;
                            Trace.Write(val ? "#" : ".");
                        }
                    }
                }

                Trace.WriteLine("");
            }

            Draw(drawing);

            return drawing;
        }

        private static void Draw(bool[,] drawing)
        {
            Trace.WriteLine("");
            Trace.WriteLine("---------------------------------");
            for (var j1 = 0; j1 < drawing.GetLength(1); j1++)
            {
                Trace.WriteLine("");
                for (var i1 = 0; i1 < drawing.GetLength(0); i1++)
                {
                    var val = drawing[i1, j1];
                    Trace.Write(val ? "#" : ".");
                }
            }
        }

        private int FindMonsters(bool[,] drawing)
        {
            var input = InputReader.ReadInput("monster");
            var monster = Tile.ParseValues(input);
            var tries = 0;
            foreach (var drawVariant in Tile.GetVariants(drawing))
            {
                Trace.WriteLine("");
                Trace.WriteLine("---------------------------------------------------");
                Draw(drawVariant);
                Trace.WriteLine("");
                Trace.WriteLine("---------------------------------------------------");
                Draw(monster);
                Trace.WriteLine("");
                Trace.WriteLine("---------------------------------------------------");
                var count = 0;

                for (var i = 0; i < drawVariant.GetLength(0) - monster.GetLength(0); i++)
                {
                    for (var j = 0; j < drawVariant.GetLength(1) - monster.GetLength(1); j++)
                    {
                        var matches = 0;
                        for (var m1 = 0; m1 < monster.GetLength(0); m1++)
                        {
                            for (var m2 = 0; m2 < monster.GetLength(1); m2++)
                            {
                                var m = monster[m1, m2];
                                if (m && drawVariant[i + m1, j + m2])
                                {
                                    matches++;
                                }
                            }
                        }

                        if (matches == 15)
                            count++;
                    }
                }

                Trace.WriteLine($"Found {count} monsters after {++tries} tries");

                if (count != 0)
                    return count;
            }

            return -1;
        }

        private object SolvePuzzle2(string[] input)
        {
            var tiles = ReadTiles(input);
            SortTiles(tiles);
            var grid = BuildGrid(tiles);
            var drawing = DrawGrid(grid);
            var monsters = FindMonsters(drawing);
            Assert.IsTrue(monsters > 0);

            var count = 0;
            for (var i = 0; i < drawing.GetLength(0); i++)
            {
                for (var j = 0; j < drawing.GetLength(1); j++)
                    if (drawing[i, j])
                        count++;
            }

            return count - 15 * monsters;
        }

        [TestMethod]
        public void Setup2()
        {
            var input = InputReader.ReadInput();
            var result = SolvePuzzle2(input);
            Assert.AreEqual(273, result);
        }

        [TestMethod]
        public void Puzzle2()
        {
            var input = InputReader.ReadInput();
            var result = SolvePuzzle2(input);
            Assert.AreEqual(1841, result);
        }

        #endregion
    }
}