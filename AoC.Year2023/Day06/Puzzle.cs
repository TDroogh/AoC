namespace AoC.Year2023.Day06
{
    public class Puzzle()
    {
        private static class Results
        {
            public const long Setup1 = 288;
            public const long Puzzle1 = 227850;
            public const long Setup2 = 71503;
            public const long Puzzle2 = 42948149;
        }

        public class Race
        {
            public required long Time { get; set; }
            public required long Record { get; set; }

            public static IEnumerable<Race> ParseAll(string[] input)
            {
                var times = input[0].Split(' ', StringSplitOptions.RemoveEmptyEntries);
                var records = input[1].Split(' ', StringSplitOptions.RemoveEmptyEntries);

                for (var i = 1; i < times.Length; i++)
                {
                    yield return new Race
                    {
                        Time = long.Parse(times[i]),
                        Record = long.Parse(records[i])
                    };
                }
            }

            public static Race Parse2(string[] input)
            {
                var times = input[0].Split(':', StringSplitOptions.RemoveEmptyEntries);
                var records = input[1].Split(':', StringSplitOptions.RemoveEmptyEntries);

                return new Race
                {
                    Time = long.Parse(times[1].Replace(" ", "")),
                    Record = long.Parse(records[1].Replace(" ", ""))
                };
            }
        }

        #region Puzzle 1

        private object SolvePuzzle1(string[] input)
        {
            long result = 1;
            foreach (var race in Race.ParseAll(input))
            {
                result *= GetResult(race);
            }

            return result;
        }

        private long GetResult(Race race)
        {
            var losses = 0;
            for (var i = 0; i < race.Time; i++)
            {
                var distance = i * (race.Time - i);
                //output.WriteLine($"Distance {race.Time}, it {i}, distance {distance} record {race.Record}");

                if (distance > race.Record)
                {
                    break;
                }

                losses++;
            }

            //output.WriteLine($"Distance {race.Time}, result {race.Time + 1 - losses * 2}");
            return (race.Time + 1 - losses * 2);
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
            var race = Race.Parse2(input);

            return GetResult(race);
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
