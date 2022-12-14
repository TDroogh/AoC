using Xunit.Abstractions;

namespace AoC.Year2022.Day14
{
    public class Puzzle
    {
        private readonly ITestOutputHelper _helper;

        public Puzzle(ITestOutputHelper helper)
        {
            _helper = helper;
        }

        private static class Results
        {
            public const int Setup1 = 24;
            public const int Puzzle1 = 696;
            public const int Setup2 = 93;
            public const int Puzzle2 = 23610;
        }

        public class Line
        {
            public List<(int x, int y)> Points { get; } = new List<(int x, int y)>();

            public static Line Parse(string input)
            {
                var line = new Line();

                foreach (var point in input.Split("->"))
                {
                    var split = point.Split(",");
                    line.Points.Add((int.Parse(split[0]), int.Parse(split[1])));
                }

                return line;
            }
        }

        #region Puzzle 1

        private object SolvePuzzle1(string[] input)
        {
            var lines = input.Select(Line.Parse).ToList();
            var minX = lines.SelectMany(line => line.Points).Min(point => point.x) - 1;
            var maxX = lines.SelectMany(line => line.Points).Max(point => point.x);
            var maxY = lines.SelectMany(line => line.Points).Max(point => point.y);

            var cave = new char[maxX + 2 - minX, maxY + 2];
            foreach (var (x, y) in cave.GetAllPoints())
            {
                cave[x, y] = '.';
            }

            foreach (var line in lines)
            {
                for (var i = 0; i < line.Points.Count - 1; i++)
                {
                    foreach (var (x, y) in ArrayHelper.GetPointsBetween(line.Points[i], line.Points[i + 1]))
                    {
                        cave[x - minX, y] = '#';
                    }
                }
            }

            return GetCount(cave, minX);
        }

        private object GetCount(char[,] cave, int minX)
        {
            bool isPreviousLanded;
            do
            {
                var y = 0;
                var x = 500 - minX;
                do
                {
                    if (y + 1 == cave.GetLength(1))
                    {
                        isPreviousLanded = false;
                        break;
                    }

                    var down = cave[x, y + 1];
                    if (down == '.')
                    {
                        y++;
                        continue;
                    }

                    var downLeft = cave[x - 1, y + 1];
                    if (downLeft == '.')
                    {
                        x--;
                        y++;
                        continue;
                    }

                    var downRight = cave[x + 1, y + 1];
                    if (downRight == '.')
                    {
                        x++;
                        y++;
                        continue;
                    }

                    cave[x, y] = 'o';
                    isPreviousLanded = y != 0;
                    break;
                } while (true);
            } while (isPreviousLanded);

            return cave.GetAllValues().Count(val => val == 'o');
        }

        [Fact]
        public void Setup1()
        {
            var input = InputReader.ReadInput();
            var result = SolvePuzzle1(input);
            Assert.Equal(Results.Setup1, result);
        }

        [Fact]
        public void Puzzle1()
        {
            var input = InputReader.ReadInput();
            var result = SolvePuzzle1(input);
            Assert.Equal(Results.Puzzle1, result);
        }

        #endregion

        #region Puzzle 2

        private object SolvePuzzle2(string[] input)
        {
            var lines = input.Select(Line.Parse).ToList();
            var maxY = lines.SelectMany(line => line.Points).Max(point => point.y) + 3;
            var minX = 500 - maxY + 1;
            var maxX = 500 + maxY;

            var cave = new char[maxX - minX, maxY];
            foreach (var (x, y) in cave.GetAllPoints())
            {
                cave[x, y] = y == maxY - 1 ? '#' : '.';
            }

            foreach (var line in lines)
            {
                for (var i = 0; i < line.Points.Count - 1; i++)
                {
                    foreach (var (x, y) in ArrayHelper.GetPointsBetween(line.Points[i], line.Points[i + 1]))
                    {
                        cave[x - minX, y] = '#';
                    }
                }
            }

            try
            {
                return GetCount(cave, minX);
            }
            finally
            {
                cave.Print(false, _helper.WriteLine);
            }
        }

        [Fact]
        public void Setup2()
        {
            var input = InputReader.ReadInput();
            var result = SolvePuzzle2(input);
            Assert.Equal(Results.Setup2, result);
        }

        [Fact]
        public void Puzzle2()
        {
            var input = InputReader.ReadInput();
            var result = SolvePuzzle2(input);
            Assert.Equal(Results.Puzzle2, result);
        }

        #endregion
    }
}
