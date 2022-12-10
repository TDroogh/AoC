using Xunit.Abstractions;

namespace AoC.Year2022.Day10
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
            public const int Setup1 = 13140;
            public const int Puzzle1 = 16020;
            public const string Setup2 = @"
##..##..##..##..##..##..##..##..##..##..
###...###...###...###...###...###...###.
####....####....####....####....####....
#####.....#####.....#####.....#####.....
######......######......######......####
#######.......#######.......#######.....";

            public const string Puzzle2 = @"
####..##..####.#..#.####..##..#....###..
#....#..#....#.#..#....#.#..#.#....#..#.
###..#......#..#..#...#..#..#.#....#..#.
#....#.....#...#..#..#...####.#....###..
#....#..#.#....#..#.#....#..#.#....#.#..
####..##..####..##..####.#..#.####.#..#.";
        }

        #region Puzzle 1

        private static int SolvePuzzle1(string[] input)
        {
            var strength = 0;
            var x = 1;
            var cycle = 1;

            void IncrementCycle()
            {
                if ((cycle - 20) % 40 == 0)
                {
                    strength += x * cycle;
                }

                cycle++;
            }

            foreach (var command in input)
            {
                if (command == "noop")
                {
                    IncrementCycle();
                }
                else
                {
                    IncrementCycle();
                    IncrementCycle();

                    x += int.Parse(command[4..]);
                }
            }

            return strength;
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

        private string SolvePuzzle2(string[] input)
        {
            var x = 1;
            var cycle = 0;
            var lines = new List<bool[]>
            {
                new bool[40],
                new bool[40],
                new bool[40],
                new bool[40],
                new bool[40],
                new bool[40],
            };

            bool[] GetSprite()
            {
                var sprite = new bool[40];

                void SetVal(int i)
                {
                    if (i is >= 0 and <= 40)
                    {
                        sprite[i] = true;
                    }
                }

                SetVal(x - 1);
                SetVal(x);
                SetVal(x + 1);

                return sprite;
            }

            void IncrementCycle()
            {
                var sprite = GetSprite();

                var pos = cycle % 40;
                var line = lines[cycle / 40];
                if (sprite[pos])
                {
                    line[pos] = true;
                }

                cycle++;
            }

            foreach (var command in input)
            {
                if (command == "noop")
                {
                    IncrementCycle();
                }
                else
                {
                    IncrementCycle();
                    IncrementCycle();

                    x += int.Parse(command[4..]);
                }
            }

            _helper.WriteLine("RESULT");
            var result = lines.Select(line => new string(line.Select(val => val ? '#' : '.').ToArray())).Aggregate("", (p, n) => $"{p}\r\n{n}");
            _helper.WriteLine(result);
            return result;
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
