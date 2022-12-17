using System.Text.RegularExpressions;
using Xunit.Abstractions;

namespace AoC.Year2022.Day16
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
            public const int Setup1 = 1651;
            public const int Puzzle1 = 1701;
            public const int Setup2 = 1707;
            public const int Puzzle2 = 2455;
        }

        public partial class Valve
        {
            [GeneratedRegex("Valve (?<Name>[A-Z]{2}) has flow rate=(?<FlowRate>[\\d]+); [a-z ]+ (?<Tunnels>[A-Z, ]+)")]
            private static partial Regex ValveRegex();

            public string Name { get; }
            public int FlowRate { get; }
            public bool IsOpen { get; set; }
            public bool InProgress { get; set; }
            public string[] Tunnels { get; }
            public Dictionary<string, int> Distances { get; }

            public Valve(string input)
            {
                var match = ValveRegex().Match(input);

                Name = match.Groups["Name"].Value;
                FlowRate = int.Parse(match.Groups["FlowRate"].Value);
                Tunnels = match.Groups["Tunnels"].Value.Split(", ");
                Distances = new Dictionary<string, int>();
            }
        }

        private Dictionary<string, Valve> ParseValves(string[] input)
        {
            var valves = input.Select(i => new Valve(i)).ToDictionary(v => v.Name);

            foreach (var valve in valves.Values)
            {
                _helper.WriteLine($"Valve {valve.Name}, flow rate {valve.FlowRate}, tunnels {string.Join(',', valve.Tunnels)}");
            }

            var i = 0;
            while (++i < valves.Count && valves.Values.SelectMany(v => v.Distances).Count() < (valves.Count * (valves.Count - 1)))
            {
                foreach (var valve in valves.Values)
                {
                    foreach (var otherValve in valves.Values.Where(v => v != valve && !valve.Distances.ContainsKey(v.Name)))
                    {
                        if (valve.Tunnels.Contains(otherValve.Name))
                        {
                            valve.Distances.Add(otherValve.Name, 1);
                        }
                        else
                        {
                            var shortest = valve.Tunnels.Select(v => valves[v]).Select(v =>
                            {
                                var dist = v.Distances.GetValueOrDefault(otherValve.Name, int.MaxValue);
                                return dist;
                            }).Min();
                            if (shortest < i)
                            {
                                valve.Distances.Add(otherValve.Name, shortest + 1);
                            }
                        }
                    }
                }

                _helper.WriteLine($"Iteration {i} Total distances: {valves.Values.SelectMany(v => v.Distances).Count()}, expected {valves.Count * (valves.Count - 1)}");
            }

            return valves;
        }

        #region Puzzle 1

        private object SolvePuzzle1(string[] input)
        {
            var valves = ParseValves(input);

            var minute = 0;
            var totalPressure = 0;

            var currentValve = valves["AA"];
            var movingDistance = 0;
            var isOpening = false;

            while (++minute <= 30)
            {
                var shouldOpenNew = currentValve.Name == "AA";
                if (movingDistance > 1)
                {
                    _helper.WriteLine($"Moving to valve {currentValve.Name}, distance {movingDistance}");
                    movingDistance--;
                }
                else if (isOpening || currentValve.FlowRate == 0)
                {
                    isOpening = false;
                    currentValve.IsOpen = true;
                    _helper.WriteLine($"Opened valve {currentValve.Name}");
                    shouldOpenNew = true;
                }
                else
                {
                    isOpening = true;
                }

                if (shouldOpenNew)
                {
                    var nextValve = DetermineNextValve(valves, currentValve, minute, out _);
                    if (nextValve != null)
                    {
                        movingDistance = currentValve.Distances[nextValve.Name];
                        currentValve = nextValve;
                    }
                }

                var currentPressure = valves.Where(v => v.Value.IsOpen).Sum(v => v.Value.FlowRate);
                totalPressure += currentPressure;

                _helper.WriteLine($"Minute {minute}: Valve {currentValve.Name}, pressure {currentPressure}, total {totalPressure}");
            }

            return totalPressure;
        }

        private Valve? DetermineNextValve(Dictionary<string, Valve> valves, Valve currentValve, int currentMinute, out decimal maxPotential)
        {
            var valvesToConsider = valves.Values.Where(v => v is { IsOpen: false, InProgress: false, FlowRate: > 0 }).ToList();
            _helper.WriteLine($"Currently at {currentValve.Name}, minute {currentMinute}, to consider {valvesToConsider.Count}");

            maxPotential = -1m;
            Valve? maxValve = null;

            var rnd = new Random();
            foreach (var valve in valvesToConsider)
            {
                var distanceToValve = currentValve.Distances[valve.Name];
                var potential = (decimal)valve.FlowRate * (30 - currentMinute - distanceToValve + 1) / (decimal)(distanceToValve * distanceToValve * rnd.NextDouble());

                if (potential > maxPotential)
                {
                    maxValve = valve;
                    maxPotential = potential;
                }
            }

            if (maxValve != null)
            {
                _helper.WriteLine($"Valve {maxValve.Name} has potential {maxPotential:F3} ({maxValve.FlowRate}, {currentValve.Distances[maxValve.Name]})");
            }

            return maxValve;
        }

        [Fact]
        public void Setup1()
        {
            var input = InputReader.ReadInput();
            var max = 0;

            while (max != Results.Setup1)
            {
                max = (int)SolvePuzzle1(input);
            }

            Assert.Equal(Results.Setup1, max);
        }

        [Fact]
        public void Puzzle1()
        {
            var input = InputReader.ReadInput();
            var max = 0;

            while (max != Results.Puzzle1)
            {
                max = (int)SolvePuzzle1(input);
            }

            Assert.Equal(Results.Puzzle1, max);
        }

        #endregion

        #region Puzzle 2

        private object SolvePuzzle2(string[] input)
        {
            var valves = ParseValves(input);

            var minute = 0;
            var totalPressure = 0;

            var myCurrentValve = valves["AA"];
            var myMovingDistance = 0;
            var myIsOpening = false;

            var elCurrentValve = valves["AA"];
            var elMovingDistance = 0;
            var elIsOpening = false;

            while (++minute <= 26)
            {
                var myShouldOpenNew = myCurrentValve.Name == "AA";
                var elShouldOpenNew = elCurrentValve.Name == "AA";

                if (myMovingDistance > 1)
                {
                    myMovingDistance--;
                }
                else if (myIsOpening || myCurrentValve.FlowRate == 0)
                {
                    myIsOpening = false;
                    myCurrentValve.IsOpen = true;
                    myShouldOpenNew = true;
                }
                else
                {
                    myIsOpening = true;
                }

                if (elMovingDistance > 1)
                {
                    elMovingDistance--;
                }
                else if (elIsOpening || elCurrentValve.FlowRate == 0)
                {
                    elIsOpening = false;
                    elCurrentValve.IsOpen = true;
                    elShouldOpenNew = true;
                }
                else
                {
                    elIsOpening = true;
                }

                if (myShouldOpenNew & elShouldOpenNew)
                {
                    var myNext = DetermineNextValve(valves, myCurrentValve, minute, out var myMaxPotential);
                    var elNext = DetermineNextValve(valves, elCurrentValve, minute, out var elMaxPotential);

                    if (myNext != null && elNext != null && myNext.Name == elNext.Name)
                    {
                        if (myMaxPotential >= elMaxPotential)
                        {
                            myMovingDistance = myCurrentValve.Distances[myNext.Name];
                            myCurrentValve = myNext;
                            myNext.InProgress = true;
                            myShouldOpenNew = false;
                        }
                        else
                        {
                            elMovingDistance = elCurrentValve.Distances[elNext.Name];
                            elCurrentValve = elNext;
                            elNext.InProgress = true;
                            elShouldOpenNew = false;
                        }
                    }
                }

                if (myShouldOpenNew)
                {
                    var nextValve = DetermineNextValve(valves, myCurrentValve, minute, out _);
                    if (nextValve != null)
                    {
                        myMovingDistance = myCurrentValve.Distances[nextValve.Name];
                        myCurrentValve = nextValve;
                        nextValve.InProgress = true;
                    }
                }

                if (elShouldOpenNew)
                {
                    var nextValve = DetermineNextValve(valves, elCurrentValve, minute, out _);
                    if (nextValve != null)
                    {
                        elMovingDistance = elCurrentValve.Distances[nextValve.Name];
                        elCurrentValve = nextValve;
                        nextValve.InProgress = true;
                    }
                }

                var currentPressure = valves.Where(v => v.Value.IsOpen).Sum(v => v.Value.FlowRate);
                totalPressure += currentPressure;

                _helper.WriteLine($"Minute {minute}: pressure {currentPressure}, total {totalPressure}");
            }

            return totalPressure;
        }

        [Fact]
        public void Setup2()
        {
            var input = InputReader.ReadInput();
            var max = 0;

            while (max != Results.Setup2)
            {
                max = (int)SolvePuzzle2(input);
            }

            Assert.Equal(Results.Setup2, max);
        }

        [Fact]
        public void Puzzle2()
        {
            var input = InputReader.ReadInput();
            var max = 0;

            while (max < Results.Puzzle2)
            {
                max = (int)SolvePuzzle2(input);
            }

            Assert.Equal(Results.Puzzle2, max);
        }

        #endregion
    }
}
