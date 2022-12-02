using Xunit.Abstractions;

namespace AoC.Year2022.Day02
{
    public class Puzzle
    {
        private readonly ITestOutputHelper _testHelper;

        public Puzzle(ITestOutputHelper testHelper)
        {
            _testHelper = testHelper;
        }

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

            public int GetScore()
            {
                if (Opponent == You)
                {
                    return 3 + (int)You;
                }

                return (int)You + You switch
                {
                    Shape.Rock when Opponent == Shape.Paper => 0,
                    Shape.Paper when Opponent == Shape.Scissors => 0,
                    Shape.Scissors when Opponent == Shape.Rock => 0,
                    _ => 6
                };
            }

            public static Round Parse1(string input)
            {
                var split = input.Split(" ");

                return new Round
                {
                    Opponent = GetOpponentShape(split[0]),
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
                var opponent = GetOpponentShape(split[0]);

                return new Round
                {
                    Opponent = opponent,
                    You = split[1] switch
                    {
                        "X" => opponent == Shape.Rock ? Shape.Scissors : opponent - 1,
                        "Y" => opponent,
                        _ => opponent == Shape.Scissors ? Shape.Rock : opponent + 1,
                    }
                };
            }

            private static Shape GetOpponentShape(string value)
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
            var i = 0;
            return input.Sum(x =>
            {
                var round = Round.Parse1(x);
                var score = round.GetScore();
                _testHelper.WriteLine($"Round {++i}: {score} ({round.You} vs {round.Opponent})");

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
            var i = 0;
            return input.Sum(x =>
            {
                var round = Round.Parse2(x);
                var score = round.GetScore();
                _testHelper.WriteLine($"Round {++i}: {score} ({round.You} vs {round.Opponent})");

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
