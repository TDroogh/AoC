namespace AoC.Year2022.Day01
{
    public class Puzzle
    {
        private static class Results
        {
            public const int Setup1 = 24000;
            public const int Puzzle1 = 70369;
            public const int Setup2 = 45000;
            public const int Puzzle2 = 203002;
        }

        private static IEnumerable<int> CalculateCalories(string[] input)
        {
            var currentElf = 0;
            foreach (var cal in input)
            {
                if (string.IsNullOrWhiteSpace(cal))
                {
                    yield return currentElf;
                    currentElf = 0;
                }
                else
                {
                    currentElf += int.Parse(cal);
                }
            }

            yield return currentElf;
        }

        #region Puzzle 1

        private int SolvePuzzle1(string[] input)
        {
            return CalculateCalories(input).Max();
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

        private int SolvePuzzle2(string[] input)
        {
            return CalculateCalories(input).OrderDescending().Take(3).Sum();
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
