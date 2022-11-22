namespace AoC.Year2019.Day01
{
    public class Puzzle
    {
        #region Puzzle 1

        private static int SolvePuzzle1(int[] input)
        {
            var total = 0;

            foreach (var mass in input)
            {
                var fuel = CalculateFuel(mass);
                total += fuel;
            }

            return total;
        }

        [Fact]
        public void Setup1()
        {
            var input = InputReader.ReadIntInput();
            var result = SolvePuzzle1(input);
            Assert.Equal(2 + 2 + 654 + 33583, result);
        }

        [Fact]
        public void Puzzle1()
        {
            var input = InputReader.ReadIntInput();
            var result = SolvePuzzle1(input);
            Assert.Equal(3361299, result);
        }

        #endregion

        #region Puzzle 2

        private static int SolvePuzzle2(int[] input)
        {
            var total = 0;
            foreach (var mass in input)
            {
                var fuel = CalculateFuel(mass);
                while (fuel > 0)
                {
                    total += fuel;
                    fuel = CalculateFuel(fuel);
                }
            }

            return total;
        }

        private static int CalculateFuel(int mass)
        {
            return (int)Math.Floor(mass / 3m) - 2;
        }

        [Fact]
        public void Setup2()
        {
            var input = InputReader.ReadIntInput();
            var result = SolvePuzzle2(input);
            Assert.Equal(2 + 2 + 966 + 50346, result);
        }

        [Fact]
        public void Puzzle2()
        {
            var input = InputReader.ReadIntInput();
            var result = SolvePuzzle2(input);
            Assert.Equal(5039071, result);
        }

        #endregion
    }
}