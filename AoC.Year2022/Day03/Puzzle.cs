namespace AoC.Year2022.Day03
{
    public class Puzzle
    {
        private static class Results
        {
            public const int Setup1 = 157;
            public const int Puzzle1 = 7848;
            public const int Setup2 = 70;
            public const int Puzzle2 = 2616;
        }

        private class Rucksack
        {
            private string All { get; init; } = string.Empty;
            private string FirstHalf { get; init; } = string.Empty;
            private string LastHalf { get; init; } = string.Empty;

            public char GetOverlap()
            {
                return FirstHalf.Intersect(LastHalf).Single();
            }

            public static char GetOverlap(params Rucksack[] rucksacks)
            {
                IEnumerable<char> intersect = rucksacks[0].All;

                foreach (var rucksack in rucksacks.Skip(1))
                {
                    intersect = intersect.Intersect(rucksack.All);
                }

                return intersect.Single();
            }

            public static Rucksack Parse(string input)
            {
                var length = input.Length / 2;

                return new Rucksack
                {
                    All = input,
                    FirstHalf = input[..length],
                    LastHalf = input[length..]
                };
            }
        }

        private static int GetPriority(char input)
        {
            return char.IsLower(input) ? input - 'a' + 1 : input - 'A' + 27;
        }

        #region Puzzle 1

        private static object SolvePuzzle1(string[] input)
        {
            return input.Select(i =>
            {
                var rucksack = Rucksack.Parse(i);
                var overlap = rucksack.GetOverlap();

                return GetPriority(overlap);
            }).Sum();
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

        private static object SolvePuzzle2(string[] input)
        {
            var rucksacks = input.Select(Rucksack.Parse).ToArray();
            var total = 0;

            for (var i = 0; i < rucksacks.Length; i += 3)
            {
                var elf1 = rucksacks[i];
                var elf2 = rucksacks[i + 1];
                var elf3 = rucksacks[i + 2];

                var overlap = Rucksack.GetOverlap(elf1, elf3, elf2);

                total += GetPriority(overlap);
            }

            return total;
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
