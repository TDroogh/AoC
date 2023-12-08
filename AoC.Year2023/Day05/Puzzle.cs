namespace AoC.Year2023.Day05
{
    public class Puzzle(ITestOutputHelper helper)
    {
        private static class Results
        {
            public const long Setup1 = 35;
            public const long Puzzle1 = 309796150;
            public const long Setup2 = 46;
            public const long Puzzle2 = 50716416;
        }

        #region Puzzle 1

        public record Input
        {
            public required long[] Numbers { get; set; }
            public required List<MappingList> Mappings { get; set; }

            public static Input Parse(string[] lines)
            {
                var input = lines[0].Replace("seeds: ", "").Split(" ").Select(long.Parse).ToArray();
                var mappings = ParseMappings(lines);

                return new Input
                {
                    Numbers = input,
                    Mappings = mappings
                };
            }

            public static Input Parse2(string[] lines)
            {
                var input2 = GetInput2(lines);

                var mappings = ParseMappings(lines);

                return new Input
                {
                    Numbers = input2.Distinct().ToArray(),
                    Mappings = mappings
                };
            }

            public static IEnumerable<long> GetInput2(string[] lines)
            {
                var input = lines[0].Replace("seeds: ", "").Split(" ").Select(long.Parse).ToArray();

                for (var i = 0; i < input.Length / 2; i++)
                {
                    var i1 = input[i * 2];
                    var i2 = input[i * 2 + 1];

                    for (var j = 0; j < i2; j++)
                    {
                        yield return (i1 + j);
                    }
                    yield return -1;
                }
            }

            public static IEnumerable<(long start, long end)> GetReverseInput(string[] lines)
            {
                var input = lines[0].Replace("seeds: ", "").Split(" ").Select(long.Parse).ToArray();

                for (var i = 0; i < input.Length / 2; i++)
                {
                    var i1 = input[i * 2];
                    var i2 = input[i * 2 + 1];

                    yield return (i1, i2 + i1);
                }
            }

            public static List<MappingList> ParseMappings(string[] lines)
            {
                var mappings = new List<MappingList>();
                var list = new List<string>();

                for (var i = 2; i < lines.Length; i++)
                {
                    var line = lines[i];
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        mappings.Add(new MappingList
                        {
                            Mappings = list.Skip(1).Select(Mapping.Parse).OrderBy(x => x.DestinationEnd).ToList(),
                            LastMapping = null,
                            Name = list[0]
                        });

                        list = new List<string>();
                    }
                    else
                    {
                        list.Add(line);
                    }
                }

                var m = new MappingList
                {
                    Mappings = list.Skip(1).Select(Mapping.Parse).OrderBy(x => x.DestinationEnd).ToList(),
                    LastMapping = null,
                    Name = list[0]
                };

                mappings.Add(m);

                return mappings;
            }
        }

        public record MappingList
        {
            public required string Name { get; set; }
            public required Mapping? LastMapping { get; set; }
            public required List<Mapping> Mappings { get; init; }
        }

        public class Mapping
        {
            public long DestinationStart { get; init; }
            public long DestinationEnd { get; init; }
            public long SourceStart { get; init; }
            public long SourceEnd { get; init; }

            public bool ContainsReverseValue(long value)
            {
                return DestinationStart <= value && DestinationEnd > value;
            }

            public static Mapping Parse(string val)
            {
                var split = val.Split(' ');
                var sourceStart = long.Parse(split[1]);
                var destinationStart = long.Parse(split[0]);
                var length = long.Parse(split[2]);

                return new Mapping
                {
                    DestinationStart = destinationStart,
                    SourceStart = sourceStart,
                    SourceEnd = sourceStart + length,
                    DestinationEnd = destinationStart + length
                };
            }
        }

        private object SolvePuzzle1(string[] lines)
        {
            var input = Input.Parse(lines);

            return SolvePuzzle(input);
        }

        private object SolvePuzzle(Input input)
        {
            var minValue = long.MaxValue;
            var mappings = input.Mappings.Select(m => m.Mappings).ToList();

            foreach (var value in input.Numbers)
            {
                var currentValue = value;

                foreach (var mapping in mappings)
                {
                    var map = mapping.FirstOrDefault(val => val.SourceStart <= currentValue && val.SourceEnd > currentValue);
                    if (map != null)
                    {
                        currentValue = map.DestinationStart + currentValue - map.SourceStart;
                    }
                }

                helper.WriteLine($"Start {value}; end {currentValue}");

                if (currentValue < minValue)
                {
                    minValue = currentValue;
                }
            }

            return minValue;
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

        private object ReverseSolvePuzzle2(string[] lines)
        {
            var inputs = Input.GetReverseInput(lines).ToList();
            var mappings = Input.ParseMappings(lines);
            mappings.Reverse();

            var minValue = long.MaxValue;

            long last = 0;
            long found = 0;
            long notFound = 0;

            var start = DateTime.UtcNow;
            for (long i = 0; i < long.MaxValue; i++)
            {
                var currentValue = i;
                foreach (var mapping in mappings)
                {
                    Mapping? map = null;

                    if (mapping.LastMapping != null)
                    {
                        if (mapping.LastMapping.ContainsReverseValue(currentValue))
                        {
                            map = mapping.LastMapping;
                            last++;
                        }
                    }

                    if (map == null)
                    {
                        map = mapping.Mappings.Find(val => val.ContainsReverseValue(currentValue));
                        mapping.LastMapping = map;
                        found++;
                    }

                    if (map != null)
                    {
                        currentValue = map.SourceStart + currentValue - map.DestinationStart;
                    }
                    else
                    {
                        notFound++;
                    }
                }

                if (i % 10000000 == 0)
                {
                    helper.WriteLine($"{DateTime.UtcNow - start:g} {i}: Start {i}; end {currentValue} (last: {last}, found: {found}, not: {notFound})");
                }

                if (inputs.Exists(val => currentValue >= val.start && currentValue < val.end))
                {
                    return i;
                }
            }

            return minValue;
        }

        [Fact]
        public void Setup2()
        {
            var input = InputReader.ReadInput();
            var result = ReverseSolvePuzzle2(input);
            Assert.Equal(Results.Setup2, result);
        }

        [Fact]
        public void Puzzle2()
        {
            var input = InputReader.ReadInput();
            var result = ReverseSolvePuzzle2(input);
            Assert.Equal(Results.Puzzle2, result);
        }

        #endregion
    }
}
