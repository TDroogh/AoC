namespace AoC.Year2023.Day01
{
    public class Puzzle
    {
        private static class Results
        {
            public const int Setup1 = 142;
            public const int Puzzle1 = 54927;
            public const int Setup2 = 281;
            public const int Puzzle2 = 54581;
        }

        #region Puzzle 1

        private object SolvePuzzle1(string[] input)
        {
            var sum = 0;
            foreach (var line in input)
            {
                var firstChar = line.First(char.IsNumber);
                var lastChar = line.Last(char.IsNumber);

                sum += int.Parse($"{firstChar}{lastChar}");
            }
            return sum;
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

        private static readonly string[] Digits = { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };

        private object SolvePuzzle2(string[] input)
        {
            var sum = 0;
            foreach (var line in input)
            {
                var value = GetValue(line);

                sum += value;
            }

            return sum;
        }

        private static int GetValue(string line)
        {
            var firstIndices = Digits.Select(num => line.IndexOf(num, StringComparison.OrdinalIgnoreCase)).ToList();
            var lastIndices = Digits.Select(num => line.LastIndexOf(num, StringComparison.OrdinalIgnoreCase)).ToList();

            var firstText = firstIndices.Where(val => val >= 0).DefaultIfEmpty(int.MaxValue).Min();
            var lastText = lastIndices.Where(val => val >= 0).DefaultIfEmpty(int.MinValue).Max();
            var firstChar = line.Any(char.IsNumber) ? line.IndexOf(line.FirstOrDefault(char.IsNumber)) : int.MaxValue;
            var lastChar = line.Any(char.IsNumber) ? line.LastIndexOf(line.LastOrDefault(char.IsNumber)) : int.MinValue;

            var first = firstText < firstChar ? firstIndices.IndexOf(firstText) : int.Parse($"{line[firstChar]}");
            var last = lastText > lastChar ? lastIndices.IndexOf(lastText) : int.Parse($"{line[lastChar]}");
            return first * 10 + last;
        }

        [Fact]
        public void Setup2()
        {
            var input = InputReader.ReadInput("setup2");
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
