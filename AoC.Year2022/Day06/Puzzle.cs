namespace AoC.Year2022.Day06
{
    public class Puzzle
    {
        private static class Results
        {
            public const int Puzzle1 = 1757;
            public const int Puzzle2 = 2950;
        }

        private static int SolvePuzzle(string input, int startOfPacketLength)
        {
            for (var i = 0; i < input.Length - startOfPacketLength + 1; i++)
            {
                var line = input.Skip(i).Take(startOfPacketLength).ToList();
                if (line.Distinct().Count() == line.Count)
                {
                    return i + startOfPacketLength;
                }
            }

            return -1;
        }

        #region Puzzle 1

        [Theory]
        [InlineData("mjqjpqmgbljsphdztnvjfqwrcgsmlb", 7)]
        [InlineData("bvwbjplbgvbhsrlpgdmjqwftvncz", 5)]
        [InlineData("nppdvjthqldpwncqszvftbrmjlhg", 6)]
        [InlineData("nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg", 10)]
        [InlineData("zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw", 11)]
        public void Setup1(string input, int output)
        {
            var result = SolvePuzzle(input, 4);
            Assert.Equal(output, result);
        }

        [Fact]
        public void Puzzle1()
        {
            var input = InputReader.ReadInput();
            var result = SolvePuzzle(input[0], 4);
            Assert.Equal(Results.Puzzle1, result);
        }

        #endregion

        #region Puzzle 2

        [Theory]
        [InlineData("mjqjpqmgbljsphdztnvjfqwrcgsmlb", 19)]
        [InlineData("bvwbjplbgvbhsrlpgdmjqwftvncz", 23)]
        [InlineData("nppdvjthqldpwncqszvftbrmjlhg", 23)]
        [InlineData("nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg", 29)]
        [InlineData("zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw", 26)]
        public void Setup2(string input, int output)
        {
            var result = SolvePuzzle(input, 14);
            Assert.Equal(output, result);
        }

        [Fact]
        public void Puzzle2()
        {
            var input = InputReader.ReadInput();
            var result = SolvePuzzle(input[0], 14);
            Assert.Equal(Results.Puzzle2, result);
        }

        #endregion
    }
}
