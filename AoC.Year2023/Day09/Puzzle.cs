namespace AoC.Year2023.Day09
{
    public class Puzzle(ITestOutputHelper output)
    {
        private static class Results
        {
            public const int Setup1 = 114;
            public const int Puzzle1 = 1938731307;
            public const int Setup2 = 2;
            public const int Puzzle2 = 948;
        }

        #region Puzzle 1

        private object SolvePuzzle1(string[] input)
        {
            var sum = 0;

            foreach (var line in input)
            {
                var numbers = line.Split(' ').Select(int.Parse).ToArray();
                var list = Expand(numbers);

                list.Reverse();

                sum += list.Sum(item => item[^1]);
            }

            return sum;
        }

        private List<int[]> Expand(int[] numbers)
        {
            var list = new List<int[]> { numbers };
            output.WriteLine(string.Join(",", numbers));

            while (list[^1].Any(val => val != 0))
            {
                var last = list[^1];
                var newList = new int[last.Length - 1];
                for (var i = 0; i < newList.Length; i++)
                {
                    newList[i] = last[i + 1] - last[i];
                }
                list.Add(newList);
                output.WriteLine(string.Join(",", newList));
            }
            return list;
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
            var sum = 0;

            foreach (var line in input)
            {
                var numbers = line.Split(' ').Select(int.Parse).ToArray();
                var list = Expand(numbers);

                list.Reverse();

                sum += list.Aggregate(0, (current, item) => item[0] - current);
            }

            return sum;
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
