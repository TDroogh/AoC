using Xunit.Abstractions;

namespace AoC.Year2022.Day09
{
    public class Puzzle
    {
        private readonly ITestOutputHelper _helper;

        public Puzzle(ITestOutputHelper helper)
        {
            _helper = helper;
        }

        private static class Results
        {
            public const int Setup1 = 13;
            public const int Puzzle1 = 6745;
            public const int Setup2 = 36;
            public const int Puzzle2 = 2793;
        }

        public class Knot
        {
            public int XPos { get; set; }
            public int YPos { get; set; }

            public void Move(char direction)
            {
                switch (direction)
                {
                    case 'U':
                    {
                        YPos++;
                        break;
                    }
                    case 'D':
                    {
                        YPos--;
                        break;
                    }
                    case 'L':
                    {
                        XPos--;
                        break;
                    }
                    case 'R':
                    {
                        XPos++;
                        break;
                    }
                }
            }

            public bool Follow(Knot other)
            {
                var xDiff = other.XPos - XPos;
                var yDiff = other.YPos - YPos;

                if (Math.Abs(xDiff) <= 1 && Math.Abs(yDiff) <= 1)
                {
                    //No movement required
                    return false;
                }

                XPos += GetMovement(xDiff);
                YPos += GetMovement(yDiff);
                return true;
            }

            private static int GetMovement(int diff)
            {
                return diff switch
                {
                    0 => 0,
                    1 => diff,
                    -1 => diff,
                    2 => 1,
                    -2 => -1,
                    _ => throw new ArgumentOutOfRangeException(nameof(diff), diff, "Diff is larger than 2")
                };
            }

            public override string ToString()
            {
                return $"({XPos}, {YPos})";
            }
        }

        #region Puzzle 1

        private object SolvePuzzle1(string[] input)
        {
            var locations = new HashSet<(int x, int y)> { (0, 0) };
            var head = new Knot();
            var tail = new Knot();

            foreach (var instruction in input)
            {
                var direction = instruction[0];
                var count = int.Parse(instruction[1..]);

                for (var i = 0; i < count; i++)
                {
                    head.Move(direction);

                    if (tail.Follow(head))
                    {
                        locations.Add((tail.XPos, tail.YPos));
                    }
                }

                _helper.WriteLine($"Head: {head}, Tail: {tail} ({locations.Distinct().Count()} distinct locations)");
            }

            return locations.Distinct().Count();
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
            var locations = new HashSet<(int x, int y)> { (0, 0) };
            var knots = Enumerable.Range(0, 10).Select(_ => new Knot()).ToList();
            var head = knots[0];
            var tail = knots[9];

            foreach (var instruction in input)
            {
                var direction = instruction[0];
                var count = int.Parse(instruction[1..]);

                for (var i = 0; i < count; i++)
                {
                    head.Move(direction);

                    var previousKnot = head;
                    foreach (var knot in knots.Skip(1))
                    {
                        knot.Follow(previousKnot);
                        previousKnot = knot;
                    }

                    locations.Add((tail.XPos, tail.YPos));
                }

                _helper.WriteLine($"Head: {head}, Tail: {tail} ({locations.Distinct().Count()} distinct locations)");
            }

            return locations.Distinct().Count();
        }

        [Fact]
        public void Setup2()
        {
            var input = InputReader.ReadInput("setup-large");
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
