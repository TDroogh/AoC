namespace AoC.Year2023.Day03
{
    public class Puzzle
    {
        private static class Results
        {
            public const int Setup1 = 4361;
            public const int Puzzle1 = 531561;
            public const int Setup2 = 467835;
            public const int Puzzle2 = 83279367;
        }

        #region Puzzle 1

        private object SolvePuzzle1(char[,] input)
        {
            var sum = 0;
            for (var y = 0; y < input.GetLength(1); y++)
            {
                var currentNumber = string.Empty;

                for (var x = 0; x < input.GetLength(0); x++)
                {
                    var chr = input[x, y];
                    if (char.IsNumber(chr))
                    {
                        currentNumber += chr;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(currentNumber) && IsPartNumber(input, x, y, currentNumber.Length))
                        {
                            var value = int.Parse(currentNumber);
                            sum += value;
                        }

                        currentNumber = string.Empty;
                    }
                }

                if (!string.IsNullOrEmpty(currentNumber) && IsPartNumber(input, input.GetLength(0), y, currentNumber.Length))
                {
                    var value = int.Parse(currentNumber);
                    sum += value;
                }
            }
            return sum;
        }

        private static bool IsPartNumber(char[,] input, int x, int y, int length)
        {
            for (var i = 1; i <= length; i++)
            {
                foreach (var point in input.GetAdjacentPoints(x - i, y, true))
                {
                    var value = input[point.x, point.y];
                    if (!char.IsNumber(value) && value != '.')
                    {
                        return true;
                    }
                }
            }

            return false;
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
            var pairs = new List<(int number, HashSet<(int x, int y)> gears)>();

            for (var y = 0; y < input.GetLength(1); y++)
            {
                var currentNumber = string.Empty;

                for (var x = 0; x < input.GetLength(0); x++)
                {
                    var chr = input[x, y];
                    if (char.IsNumber(chr))
                    {
                        currentNumber += chr;
                    }
                    else if (!string.IsNullOrEmpty(currentNumber))
                    {
                        pairs.Add((int.Parse(currentNumber), GetGearCoordinates(input, x, y, currentNumber.Length).Distinct().ToHashSet()));

                        currentNumber = string.Empty;
                    }
                }

                if (!string.IsNullOrEmpty(currentNumber))
                {
                    pairs.Add((int.Parse(currentNumber), GetGearCoordinates(input, input.GetLength(0), y, currentNumber.Length).Distinct().ToHashSet()));
                }
            }

            var sum = 0;
            foreach (var gear in pairs.SelectMany(pair => pair.gears).Distinct())
            {
                var parts = pairs.Where(pair => pair.gears.Contains(gear)).ToList();

                if (parts.Count == 2)
                {
                    sum += parts[0].number * parts[1].number;
                }
            }
            return sum;
        }

        private static IEnumerable<(int x, int y)> GetGearCoordinates(char[,] input, int x, int y, int length)
        {
            for (var i = 1; i <= length; i++)
            {
                foreach (var point in input.GetAdjacentPoints(x - i, y, true))
                {
                    var value = input[point.x, point.y];
                    if (value == '*')
                    {
                        yield return (point.x, point.y);
                    }
                }
            }
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
