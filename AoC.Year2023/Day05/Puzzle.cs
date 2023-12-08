using Xunit.Abstractions;

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

            public static List<MappingList> ParseMappings(string[] lines)
            {
                var mappings = new List<List<Mapping>>();
                var list = new List<string>();

                for (var i = 2; i < lines.Length; i++)
                {
                    var line = lines[i];
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        var mapping = list.Skip(1).Select(Mapping.Parse).OrderBy(x => x.SourceStart).ToList();
                        mapping.ForEach(val =>
                        {
                            val.NextMapping = mapping.Find(m => m.SourceEnd == val.SourceStart);
                        });
                        mappings.Add(mapping);

                        list = new List<string>();
                    }
                    else
                    {
                        list.Add(line);
                    }
                }

                var m = list.Skip(1).Select(Mapping.Parse).ToList();
                mappings.Add(m);

                return mappings.Select(x => new MappingList { Mappings = x, LastMapping = null }).ToList();
            }
        }

        public record MappingList
        {
            public required Mapping? LastMapping { get; set; }
            public required List<Mapping> Mappings { get; init; }
        }

        public record Mapping
        {
            public long DestinationStart { get; init; }
            public long SourceStart { get; init; }
            public long SourceEnd { get; init; }
            public long Length { get; init; }

            public Mapping? NextMapping { get; set; }

            public bool ContainsValue(long value)
            {
                return SourceStart <= value && SourceEnd > value;
            }

            public static Mapping Parse(string val)
            {
                var split = val.Split(' ');
                var sourceStart = long.Parse(split[1]);
                var length = long.Parse(split[2]);

                return new Mapping
                {
                    DestinationStart = long.Parse(split[0]),
                    SourceStart = sourceStart,
                    Length = length,
                    SourceEnd = sourceStart + length
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

        private object SolvePuzzle2(string[] lines)
        {
            var mappings = Input.ParseMappings(lines);
            var minValue = long.MaxValue;

            long i = 0;

            //helper.WriteLine(Input.GetInput2(lines).LongCount().ToString());
            var start = DateTime.UtcNow;
            foreach (var value in Input.GetInput2(lines))
            {
                var currentValue = value;

                if (value == -1)
                {
                    mappings.ForEach(m => m.LastMapping = null);
                    continue;
                }

                foreach (var mapping in mappings)
                {
                    Mapping? map = null;

                    if (mapping.LastMapping != null)
                    {
                        if (mapping.LastMapping.ContainsValue(currentValue))
                        {
                            map = mapping.LastMapping;
                        }
                        else if (mapping.LastMapping.NextMapping?.ContainsValue(currentValue) == true)
                        {
                            map = mapping.LastMapping.NextMapping;
                        }
                    }

                    if (map == null)
                    {
                        map = mapping.Mappings.Find(val => val.ContainsValue(currentValue));
                        mapping.LastMapping = map;
                    }

                    if (map != null)
                    {
                        currentValue = map.DestinationStart + currentValue - map.SourceStart;
                    }
                }

                if (i % 100000000 == 0)
                {
                    helper.WriteLine($"{DateTime.UtcNow - start:g} {i}: Start {value}; end {currentValue}");
                }

                if (currentValue < minValue)
                {
                    minValue = currentValue;
                }
                i++;
            }

            return minValue;
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
