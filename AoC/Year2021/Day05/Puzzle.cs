namespace AoC.Year2021.Day05
{
    [TestClass]
    public class Puzzle
    {
        private static class Results
        {
            public const int Setup1 = 5;
            public const int Puzzle1 = 7085;
            public const int Setup2 = 12;
            public const int Puzzle2 = 20271;
        }

        public enum Order
        {
            Horizontal,
            Vertical,
            Diagonal
        }

        public class Line
        {
            public int FromX { get; set; }
            public int FromY { get; set; }
            public int ToX { get; set; }
            public int ToY { get; set; }
            public Order Order { get; set; }

            public static Line Parse(string input)
            {
                var split = input.Split(" -> ");
                var from = split[0].Split(",");
                var to = split[1].Split(",");

                var fromX = int.Parse(from[0]);
                var fromY = int.Parse(from[1]);
                var toX = int.Parse(to[0]);
                var toY = int.Parse(to[1]);

                return new Line
                {
                    FromX = fromX,
                    FromY = fromY,
                    ToX = toX,
                    ToY = toY,
                    Order = from[0] == to[0]
                        ? Order.Vertical
                        : from[1] == to[1]
                            ? Order.Horizontal
                            : Order.Diagonal
                };
            }

            /// <inheritdoc />
            public override string ToString()
            {
                return $"{Order}: from {FromX},{FromY} to {ToX},{ToY}";
            }
        }

        #region Puzzle 1

        private object SolvePuzzle1(string[] input)
        {
            var lines = input.Select(Line.Parse).ToArray();
            var size = lines.Max(x => Math.Max(Math.Max(x.FromX, x.FromY), Math.Max(x.ToX, x.ToY))) + 1;

            var grid = new int[size, size];
            foreach (var line in lines)
            {
                if (line.Order == Order.Horizontal)
                {
                    var y = line.FromY;
                    for (var x = Math.Min(line.FromX, line.ToX); x <= Math.Max(line.FromX, line.ToX); x++)
                        grid[x, y]++;
                }

                if (line.Order == Order.Vertical)
                {
                    var x = line.FromX;
                    for (var y = Math.Min(line.FromY, line.ToY); y <= Math.Max(line.FromY, line.ToY); y++)
                        grid[x, y]++;
                }
            }

            var count = 0;
            for (var i = 0; i < size; i++)
            {
                for (var j = 0; j < size; j++)
                {
                    if (grid[i, j] > 1)
                        count++;
                }
            }

            return count;
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
            var lines = input.Select(Line.Parse).ToArray();
            var size = lines.Max(x => Math.Max(Math.Max(x.FromX, x.FromY), Math.Max(x.ToX, x.ToY))) + 1;

            var grid = new int[size, size];
            foreach (var line in lines)
            {
                if (line.Order == Order.Horizontal)
                {
                    var y = line.FromY;
                    for (var x = Math.Min(line.FromX, line.ToX); x <= Math.Max(line.FromX, line.ToX); x++)
                        grid[x, y]++;
                }
                if (line.Order == Order.Vertical)
                {
                    var x = line.FromX;
                    for (var y = Math.Min(line.FromY, line.ToY); y <= Math.Max(line.FromY, line.ToY); y++)
                        grid[x, y]++;
                }

                if (line.Order == Order.Diagonal)
                {
                    var diff = Math.Abs(line.FromY - line.ToY);
                    var leftToRight = line.FromX < line.ToX;
                    var topToBottom = line.FromY < line.ToY;

                    for (var i = 0; i <= diff; i++)
                    {
                        var x = line.FromX + (leftToRight ? i : -i);
                        var y = line.FromY + (topToBottom ? i : -i);

                        grid[x, y]++;
                    }
                }
            }

            var count = 0;
            for (var i = 0; i < size; i++)
            {
                for (var j = 0; j < size; j++)
                {
                    if (grid[i, j] > 1)
                        count++;
                }
            }

            return count;
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