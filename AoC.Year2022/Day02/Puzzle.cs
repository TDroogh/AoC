namespace AoC.Year2022.Day02
{
    public class Puzzle
    {
        private static class Results
        {
            public const int Setup1 = 15;
            public const int Puzzle1 = 12586;
            public const int Setup2 = 12;
            public const int Puzzle2 = 13193;
        }

        private enum Shape
        {
            Rock = 1,
            Paper = 2,
            Scissors = 3
        }

        private class Round
        {
            public Shape Opponent { get; set; }
            public Shape You { get; set; }

            private static Shape GetShapeWinningFrom(Shape input)
            {
                return input switch
                {
                    Shape.Rock => Shape.Paper,
                    Shape.Paper => Shape.Scissors,
                    Shape.Scissors => Shape.Rock,
                    _ => throw new ArgumentOutOfRangeException(nameof(input))
                };
            }

            private static Shape GetShapeLosingTo(Shape input)
            {
                return input switch
                {
                    Shape.Rock => Shape.Scissors,
                    Shape.Paper => Shape.Rock,
                    Shape.Scissors => Shape.Paper,
                    _ => throw new ArgumentOutOfRangeException(nameof(input))
                };
            }

            public int GetScore()
            {
                if (You == Opponent)
                {
                    return 3 + (int)You;
                }

                if (You == GetShapeWinningFrom(Opponent))
                {
                    return 6 + (int)You;
                }

                return 0 + (int)You;
            }

            public static Round Parse1(string input)
            {
                var split = input.Split(" ");

                return new Round
                {
                    Opponent = ParseOpponentShape(split[0]),
                    You = split[1] switch
                    {
                        "X" => Shape.Rock,
                        "Y" => Shape.Paper,
                        _ => Shape.Scissors,
                    }
                };
            }

            public static Round Parse2(string input)
            {
                var split = input.Split(" ");
                var opponent = ParseOpponentShape(split[0]);

                return new Round
                {
                    Opponent = opponent,
                    You = split[1] switch
                    {
                        "X" => GetShapeLosingTo(opponent),
                        "Y" => opponent,
                        _ => GetShapeWinningFrom(opponent),
                    }
                };
            }

            private static Shape ParseOpponentShape(string value)
            {
                return value switch
                {
                    "A" => Shape.Rock,
                    "B" => Shape.Paper,
                    _ => Shape.Scissors,
                };
            }
        }

        #region Puzzle 1

        private object SolvePuzzle1(string[] input)
        {
            return input.Sum(x =>
            {
                var round = Round.Parse1(x);
                var score = round.GetScore();

                return score;
            });
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
            return input.Sum(x =>
            {
                var round = Round.Parse2(x);
                var score = round.GetScore();

                return score;
            });
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
