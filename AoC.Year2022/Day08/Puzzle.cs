namespace AoC.Year2022.Day08
{
    public class Puzzle
    {
        private static class Results
        {
            public const int Setup1 = 21;
            public const int Puzzle1 = 1695;
            public const int Setup2 = 8;
            public const int Puzzle2 = 287040;
        }

        private enum Direction
        {
            Left,
            Right,
            Up,
            Down
        }

        private static IEnumerable<int> GetTreesInDirection(int[,] input, int i, int j, Direction direction)
        {
            switch (direction)
            {
                case Direction.Left:
                    for (var k = i - 1; k >= 0; k--)
                    {
                        yield return input[k, j];
                    }

                    break;
                case Direction.Right:
                    for (var k = i + 1; k < input.GetLength(0); k++)
                    {
                        yield return input[k, j];
                    }

                    break;
                case Direction.Up:
                    for (var k = j - 1; k >= 0; k--)
                    {
                        yield return input[i, k];
                    }

                    break;
                case Direction.Down:
                    for (var k = j + 1; k < input.GetLength(1); k++)
                    {
                        yield return input[i, k];
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
        }

        #region Puzzle 1

        private static int SolvePuzzle1(int[,] input)
        {
            var visibleCount = 0;
            for (var i = 0; i < input.GetLength(0); i++)
            {
                for (var j = 0; j < input.GetLength(1); j++)
                {
                    if (IsVisible(input, i, j))
                    {
                        visibleCount++;
                    }
                }
            }

            return visibleCount;
        }

        private static bool IsVisible(int[,] input, int i, int j)
        {
            //On the edge is always visible
            if (i == 0 || j == 0 || i == input.GetLength(0) - 1 || j == input.GetLength(1) - 1)
            {
                return true;
            }

            var tree = input[i, j];

            return Enum.GetValues<Direction>().Any(dir => GetTreesInDirection(input, i, j, dir).All(size => size < tree));
        }

        [Fact]
        public void Setup1()
        {
            var input = InputReader.ReadIntArrayInput();
            var result = SolvePuzzle1(input);
            Assert.Equal(Results.Setup1, result);
        }

        [Fact]
        public void Puzzle1()
        {
            var input = InputReader.ReadIntArrayInput();
            var result = SolvePuzzle1(input);
            Assert.Equal(Results.Puzzle1, result);
        }

        #endregion

        #region Puzzle 2

        private static int SolvePuzzle2(int[,] input)
        {
            var maxScenicScore = 0;

            for (var i = 0; i < input.GetLength(0); i++)
            {
                for (var j = 0; j < input.GetLength(1); j++)
                {
                    //Edges can be excluded (one side is always 0)
                    if (i == 0 || j == 0 || i == input.GetLength(0) - 1 || j == input.GetLength(1) - 1)
                    {
                        continue;
                    }

                    var scenicScore = GetScenicScore(input, i, j);

                    if (scenicScore > maxScenicScore)
                    {
                        maxScenicScore = scenicScore;
                    }
                }
            }

            return maxScenicScore;
        }

        private static int GetScenicScore(int[,] input, int i, int j)
        {
            var tree = input[i, j];
            var scenicScore = 1;

            foreach (var direction in Enum.GetValues<Direction>())
            {
                var directionScore = 0;
                foreach (var directionTree in GetTreesInDirection(input, i, j, direction))
                {
                    if (directionTree >= tree)
                    {
                        directionScore++;
                        break;
                    }

                    directionScore++;
                }

                scenicScore *= directionScore;
            }

            return scenicScore;
        }

        [Fact]
        public void Setup2()
        {
            var input = InputReader.ReadIntArrayInput();
            var result = SolvePuzzle2(input);
            Assert.Equal(Results.Setup2, result);
        }

        [Fact]
        public void Puzzle2()
        {
            var input = InputReader.ReadIntArrayInput();
            var result = SolvePuzzle2(input);
            Assert.Equal(Results.Puzzle2, result);
        }

        #endregion
    }
}
