namespace AoC.Year2023.Day04
{
    public class Puzzle
    {
        private static class Results
        {
            public const int Setup1 = 13;
            public const int Puzzle1 = 25571;
            public const int Setup2 = 30;
            public const int Puzzle2 = 8805731;
        }

        public class ScratchCard
        {
            public required int Number { get; init; }
            public required int Count { get; set; }
            public required int[] CardNumbers { get; init; }
            public required int[] WinningNumbers { get; init; }

            public static ScratchCard Parse(string line)
            {
                var split = line.Split(':', '|');
                return new ScratchCard
                {
                    Count = 1,
                    Number = int.Parse(split[0].Replace("Card ", "")),
                    WinningNumbers = split[1].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray(),
                    CardNumbers = split[2].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray(),
                };
            }

            public int GetScore()
            {
                var overlap = GetWinCount();

                if (overlap == 0)
                {
                    return 0;
                }

                return (int)Math.Pow(2, overlap - 1);
            }

            public int GetWinCount()
            {
                return CardNumbers.Intersect(WinningNumbers).Count();
            }
        }

        #region Puzzle 1

        private object SolvePuzzle1(string[] input)
        {
            var cards = input.Select(ScratchCard.Parse).ToList();
            var sum = 0;
            foreach (var card in cards)
            {
                sum += card.GetScore();
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
            var cards = input.Select(ScratchCard.Parse).ToList();
            var j = 0;
            foreach (var card in cards)
            {
                j++;
                var winCount = card.GetWinCount();

                for (var i = 0; i < winCount; i++)
                {
                    var nextCard = cards[j + i];
                    nextCard.Count += card.Count;
                }
            }

            return cards.Sum(c => c.Count);
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
