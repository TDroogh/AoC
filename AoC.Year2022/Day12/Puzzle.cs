using Xunit.Abstractions;

namespace AoC.Year2022.Day12
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
            public const int Setup1 = 31;
            public const int Puzzle1 = 456;
            public const int Setup2 = 30;
            public const int Puzzle2 = 454;
        }
        private static void InitMaps(char[,] input, out int xe, out int ye, out int[,] heightMap, out int[,] distMap)
        {
            xe = 0;
            ye = 0;

            heightMap = new int[input.GetLength(0), input.GetLength(1)];
            distMap = new int[input.GetLength(0), input.GetLength(1)];

            foreach (var (x, y) in input.GetAllPoints())
            {
                var value = input[x, y];
                switch (value)
                {
                    case 'S':
                        distMap[x, y] = 1;
                        heightMap[x, y] = -1;
                        break;
                    case 'E':
                        xe = x;
                        ye = y;
                        distMap[x, y] = -1;
                        heightMap[x, y] = 25;
                        break;
                    default:
                        distMap[x, y] = -1;
                        heightMap[x, y] = value - 'a';
                        break;
                }
            }
        }

        private int FindShortestPath(char[,] input, int[,] distMap, int[,] heightMap, int xe, int ye)
        {
            var i = 1;
            var prevCount = -1;
            var count = -1;
            do
            {
                prevCount = count;
                foreach (var (x, y) in input.GetAllPoints())
                {
                    var dist = distMap[x, y];
                    if (dist > -1)
                    {
                        continue;
                    }

                    var height = heightMap[x, y];
                    var maxDist = int.MaxValue;
                    var allTooHigh = true;

                    foreach (var (x2, y2) in input.GetAdjacentPoints(x, y, false))
                    {
                        var otherHeight = heightMap[x2, y2];
                        if (height > otherHeight + 1)
                        {
                            continue;
                        }

                        allTooHigh = false;

                        var otherDist = distMap[x2, y2];
                        if (otherDist == -1)
                        {
                            continue;
                        }

                        if (otherDist + 1 < maxDist)
                        {
                            maxDist = otherDist + 1;
                        }
                    }

                    if (maxDist < int.MaxValue)
                    {
                        distMap[x, y] = maxDist;
                    }
                    else if (allTooHigh)
                    {
                        distMap[x, y] = int.MaxValue;
                    }
                }

                count = distMap.GetAllValues().Count(x => x > -1);
                if (i++ % 100 == 0)
                {
                    _helper.WriteLine($"Iteration {i - 1}: {distMap.GetAllValues().Count(x => x > -1)}");
                }
            } while (distMap.GetAllValues().Any(x => x == -1) && i < 250 && prevCount != count);

            _helper.WriteLine($"Iteration {i}: {distMap.GetAllValues().Count(x => x > -1)}");
            return distMap[xe, ye];
        }

        #region Puzzle 1

        private object SolvePuzzle1(char[,] input)
        {
            InitMaps(input, out var xe, out var ye, out var heightMap, out var distMap);

            var (x, y) = input.GetIndex('S');
            distMap[x, y] = 0;

            return FindShortestPath(input, distMap, heightMap, xe, ye);
        }

        [Fact]
        public void Setup1()
        {
            var input = InputReader.ReadArrayInput();
            var result = SolvePuzzle1(input);
            Assert.Equal(Results.Setup1, result);
        }

        [Fact]
        public void Puzzle1()
        {
            var input = InputReader.ReadArrayInput();
            var result = SolvePuzzle1(input);
            Assert.Equal(Results.Puzzle1, result);
        }

        #endregion

        #region Puzzle 2

        private object SolvePuzzle2(char[,] input)
        {
            InitMaps(input, out var xe, out var ye, out var heightMap, out var distMap);
            var shortest = int.MaxValue;

            foreach (var (x, y) in heightMap.FindIndices(0))
            {
                var distClone = (int[,])distMap.Clone();
                distClone[x, y] = 0;
                var path = FindShortestPath(input, distClone, heightMap, xe, ye);

                if (path < shortest)
                {
                    shortest = path;
                }
            }

            return shortest;
        }

        [Fact]
        public void Setup2()
        {
            var input = InputReader.ReadArrayInput();
            var result = SolvePuzzle2(input);
            Assert.Equal(Results.Setup2, result);
        }

        [Fact]
        public void Puzzle2()
        {
            var input = InputReader.ReadArrayInput();
            var result = SolvePuzzle2(input);
            Assert.Equal(Results.Puzzle2, result);
        }

        #endregion
    }
}
