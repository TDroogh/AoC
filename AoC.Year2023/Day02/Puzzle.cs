namespace AoC.Year2023.Day02
{
    public class Puzzle
    {
        private static class Results
        {
            public const int Setup1 = 8;
            public const int Puzzle1 = 2476;
            public const int Setup2 = 2286;
            public const int Puzzle2 = 54911;
        }

        private class Game
        {
            public int Id { get; init; }
            public List<DiceSet> Sets { get; init; } = default!;

            public static Game Parse(string input)
            {
                var firstSplit = input.Split(":");
                var id = int.Parse(firstSplit[0].Replace("Game ", ""));
                var diceSets = firstSplit[1].Split(";");

                return new Game
                {
                    Id = id,
                    Sets = diceSets.Select(DiceSet.Parse).ToList()
                };
            }
        }

        private class DiceSet
        {
            public int BlueCount { get; set; }
            public int RedCount { get; set; }
            public int GreenCount { get; set; }

            public static DiceSet Parse(string input)
            {
                var set = new DiceSet();
                foreach (var color in input.Split(","))
                {
                    var count = int.Parse(color.Trim().Split(" ")[0].Trim());
                    if (color.Contains("red"))
                    {
                        set.RedCount = count;
                    }
                    if (color.Contains("blue"))
                    {
                        set.BlueCount = count;
                    }
                    if (color.Contains("green"))
                    {
                        set.GreenCount = count;
                    }
                }
                return set;
            }
        }

        #region Puzzle 1

        private object SolvePuzzle1(string[] input)
        {
            var sum = 0;

            foreach (var game in input.Select(Game.Parse))
            {
                var possible = game.Sets.TrueForAll(set => set is { GreenCount: <= 13, RedCount: <= 12, BlueCount: <= 14 });
                if (possible)
                {
                    sum += game.Id;
                }
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

        private object SolvePuzzle2(string[] input)
        {
            var sum = 0;

            foreach (var game in input.Select(Game.Parse))
            {
                var red = game.Sets.Max(s => s.RedCount);
                var blue = game.Sets.Max(s => s.BlueCount);
                var green = game.Sets.Max(s => s.GreenCount);

                sum += red * blue * green;
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
