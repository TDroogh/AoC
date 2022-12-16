using System.Text.RegularExpressions;
using Xunit.Abstractions;

namespace AoC.Year2022.Day15
{
    public partial class Puzzle
    {
        private readonly ITestOutputHelper _helper;

        public Puzzle(ITestOutputHelper helper)
        {
            _helper = helper;
        }

        private static class Results
        {
            public const int Setup1 = 26;
            public const int Puzzle1 = 5256611;
            public const long Setup2 = 56000011;
            public const long Puzzle2 = 13337919186981;
        }

        public partial class Sensor
        {
            [GeneratedRegex("x=([-]?[\\d]+), y=([-]?[\\d+]+)")]
            private static partial Regex SensorRegex();

            public Sensor(string line)
            {
                var regex = SensorRegex();
                var matches = regex.Matches(line);
                Assert.Equal(2, matches.Count);

                var location = matches[0];
                Assert.Equal(3, location.Groups.Count);
                Location = (int.Parse(location.Groups[1].Value), int.Parse(location.Groups[2].Value));

                var beacon = matches[1];
                Assert.Equal(3, beacon.Groups.Count);
                ClosestBeacon = (int.Parse(beacon.Groups[1].Value), int.Parse(beacon.Groups[2].Value));

                ManhattanDistance = CalculateManhattanDistance(ClosestBeacon);
            }

            public (int x, int y) Location { get; }
            public (int x, int y) ClosestBeacon { get; }
            public int ManhattanDistance { get; }

            public bool IsCloserThanBeacon((int x, int y) other)
            {
                var distX = Math.Abs(Location.x - other.x);
                if (distX > ManhattanDistance)
                {
                    return false;
                }

                var distY = Math.Abs(Location.y - other.y);
                if (distY + distX > ManhattanDistance)
                {
                    return false;
                }

                return true;
            }

            public int CalculateManhattanDistance((int x, int y) other)
            {
                return Math.Abs(Location.x - other.x) + Math.Abs(Location.y - other.y);
            }
        }

        #region Puzzle 1

        private object SolvePuzzle1(string[] input, int row)
        {
            var sensors = input.Select(l => new Sensor(l)).ToList();

            var points = new HashSet<int>();

            foreach (var sensor in sensors)
            {
                var dist = sensor.ManhattanDistance;

                _helper.WriteLine($"Location {sensor.Location.x}, {sensor.Location.y}, Closest {sensor.ClosestBeacon.x}, {sensor.ClosestBeacon.y}, Manhattan: {dist}");

                if (Math.Abs(row - sensor.Location.y) <= dist)
                {
                    var pointsAtDistance = ArrayHelper.GetPointsAtDistance(sensor.Location, dist).Where(p => p.y == row).ToList();

                    foreach (var point in ArrayHelper.GetPointsBetween(pointsAtDistance.First(), pointsAtDistance.Last()))
                    {
                        points.Add(point.x);
                    }
                }
            }

            return points.Except(sensors.Select(s => s.ClosestBeacon).Where(p => p.Item2 == row).Select(p => p.Item1)).Count();
        }

        [Fact]
        public void Setup1()
        {
            var input = InputReader.ReadInput();
            var result = SolvePuzzle1(input, 10);
            Assert.Equal(Results.Setup1, result);
        }

        [Fact]
        public void Puzzle1()
        {
            var input = InputReader.ReadInput();
            var result = SolvePuzzle1(input, 2_000_000);
            Assert.Equal(Results.Puzzle1, result);
        }

        #endregion

        #region Puzzle 2

        private object SolvePuzzle2(string[] input, int maxVal)
        {
            var sensors = input.Select(l => new Sensor(l)).OrderByDescending(s => s.ManhattanDistance).ToList();

            for (var x = 0; x <= maxVal; x++)
            {
                if (x % 100000 == 0)
                {
                    _helper.WriteLine($"Iteration {x}");
                }

                var y = 0;
                do
                {
                    var distances = sensors.Select(s => s.ManhattanDistance - s.CalculateManhattanDistance((x, y))).ToList();
                    if (distances.All(d => d < 0))
                    {
                        _helper.WriteLine($"{x}, {y} ?!");
                        return (long)x * 4000000 + y;
                    }

                    y += Math.Max(1, distances.Where(d => d >= 0).Min());
                } while (y < maxVal);
            }

            return -1;
        }

        [Fact]
        public void Setup2()
        {
            var input = InputReader.ReadInput();
            var result = SolvePuzzle2(input, 20);
            Assert.Equal(Results.Setup2, result);
        }

        [Fact]
        public void Puzzle2()
        {
            var input = InputReader.ReadInput();
            var result = SolvePuzzle2(input, 4000000);
            Assert.Equal(Results.Puzzle2, result);
        }

        #endregion
    }
}
