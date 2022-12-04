namespace AoC.Year2022.Day04
{
    public class Puzzle
    {
        private static class Results
        {
            public const int Setup1 = 2;
            public const int Puzzle1 = 466;
            public const int Setup2 = 4;
            public const int Puzzle2 = 865;
        }

        public class Elf
        {
            private int FromSector { get; init; }
            private int ToSector { get; init; }

            public bool FullyOverlaps(Elf other)
            {
                return FromSector <= other.FromSector && ToSector >= other.ToSector;
            }

            public bool PartiallyOverlaps(Elf other)
            {
                return GetSectors().Intersect(other.GetSectors()).Any();
            }

            private IEnumerable<int> GetSectors()
            {
                for (var i = FromSector; i <= ToSector; i++)
                {
                    yield return i;
                }
            }

            public static Elf Parse(string input)
            {
                var split = input.Split('-');

                return new Elf
                {
                    FromSector = int.Parse(split[0]),
                    ToSector = int.Parse(split[1])
                };
            }
        }

        public class Pair
        {
            private Elf First { get; }
            private Elf Last { get; }

            private Pair(Elf first, Elf last)
            {
                First = first;
                Last = last;
            }

            public bool FullyOverlaps()
            {
                return First.FullyOverlaps(Last) || Last.FullyOverlaps(First);
            }

            public bool PartiallyOverlaps()
            {
                return First.PartiallyOverlaps(Last);
            }

            public static Pair Parse(string input)
            {
                var split = input.Split(',');
                return new Pair(Elf.Parse(split[0]), Elf.Parse(split[1]));
            }
        }

        #region Puzzle 1

        private static object SolvePuzzle1(string[] input)
        {
            return input.Select(Pair.Parse).Count(pair => pair.FullyOverlaps());
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
            return input.Select(Pair.Parse).Count(pair => pair.PartiallyOverlaps());
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
