namespace AoC.Year2022.Day01
{
    public class Puzzle
    {
        private static class Results
        {
            public const int Setup1 = 4;
            public const int Puzzle1 = 7;
            public const int Setup2 = 4;
            public const int Puzzle2 = 7;
        }

        #region Puzzle 1

        private static int SolvePuzzle1(string[] input)
        {
            return input.Length;
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

        private static int SolvePuzzle2(string[] input)
        {
            return input.Length;
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
