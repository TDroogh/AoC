namespace AoC.Year2020.Day16
{
    [TestClass]
    public class Puzzle
    {
        #region Puzzle 1

        private record RuleSet
        {
            public required string Name { get; init; }
            public required List<int> Values { get; init; }
            public required int Index { get; set; }

            public static RuleSet Parse(string line)
            {
                var split = line.Split(':');
                var name = split[0];
                var ranges = split[1].Split("or");

                var rangeResult = new List<int>();
                foreach (var range in ranges)
                {
                    var values = range.Trim().Split("-");
                    var from = int.Parse(values[0]);
                    var until = int.Parse(values[1]);
                    for (var j = from; j <= until; j++)
                        rangeResult.Add(j);
                }

                return new RuleSet
                {
                    Name = name,
                    Index = -1,
                    Values = rangeResult
                };
            }
        }

        private static void FindIndices(List<RuleSet> ruleSets, List<int[]> validTickets)
        {
            var k = 0;
            var valueCount = ruleSets.Count;
            while (ruleSets.Any(x => x.Index == -1))
            {
                for (var j = 0; j < valueCount; j++)
                {
                    var values = validTickets.Select(x => x[j]).ToArray();
                    var sets = ruleSets.Where(x => x.Index == -1 && values.All(x.Values.Contains)).ToList();

                    if (sets.Count != 1)
                        continue;

                    var set = sets.Single();
                    set.Index = j;
                }

                //Infinite loop protection
                if (k++ == valueCount)
                    Assert.IsFalse(true);
            }
        }

        private int SolvePuzzle1(string[] input)
        {
            var ruleSets = new List<RuleSet>();
            var i = 0;
            var line = input[i++];

            while (string.IsNullOrWhiteSpace(line) == false)
            {
                ruleSets.Add(RuleSet.Parse(line));
                line = input[i++];
            }

            while (line != "nearby tickets:")
                line = input[i++];

            var invalidValues = new List<int>();
            var validTickets = new List<int[]>();
            //Validate invalid values
            for (; i < input.Length; i++)
            {
                line = input[i];
                var values = line.Split(",").Select(int.Parse).ToArray();
                var allValid = true;
                foreach (var value in values)
                {
                    if (ruleSets.Select(x => x.Values).Any(x => x.Contains(value)) == false)
                    {
                        invalidValues.Add(value);
                        allValid = false;
                    }
                }

                if (allValid)
                    validTickets.Add(values);
            }

            //Find indices
            FindIndices(ruleSets, validTickets);

            var rulesDict = ruleSets.ToDictionary(x => x.Index);
            //Validate against rule set
            foreach (var ticket in validTickets)
            {
                for (var j = 0; j < ticket.Length; j++)
                {
                    var ticketValue = ticket[j];
                    var ruleSet = rulesDict[j];
                    if (ruleSet.Values.Contains(ticketValue) == false)
                        invalidValues.Add(ticketValue);
                }
            }

            return invalidValues.Sum();
        }

        [TestMethod]
        public void Setup1()
        {
            var input = InputReader.ReadInput();
            var result = SolvePuzzle1(input);
            Assert.AreEqual(71, result);
        }

        [TestMethod]
        public void Puzzle1()
        {
            var input = InputReader.ReadInput();
            var result = SolvePuzzle1(input);
            Assert.AreEqual(28873, result);
        }

        #endregion

        #region Puzzle 2

        private long SolvePuzzle2(string[] input)
        {
            var ruleSets = new List<RuleSet>();
            var i = 0;
            var line = input[i++];

            while (string.IsNullOrWhiteSpace(line) == false)
            {
                var split = line.Split(':');
                var name = split[0];
                var ranges = split[1].Split("or");

                var rangeResult = new List<int>();
                foreach (var range in ranges)
                {
                    var values = range.Trim().Split("-");
                    var from = int.Parse(values[0]);
                    var until = int.Parse(values[1]);
                    for (var j = from; j <= until; j++)
                        rangeResult.Add(j);
                }

                ruleSets.Add(new RuleSet
                {
                    Name = name,
                    Values = rangeResult,
                    Index = -1
                });

                line = input[i++];
            }

            while (line != "your ticket:")
                line = input[i++];
            var myTicket = input[i++].Split(",").Select(int.Parse).ToArray();

            while (line != "nearby tickets:")
                line = input[i++];

            var validTickets = new List<int[]>();
            //Validate invalid values
            for (; i < input.Length; i++)
            {
                line = input[i];
                var values = line.Split(",").Select(int.Parse).ToArray();
                var allValid = true;
                foreach (var value in values)
                {
                    if (ruleSets.Select(x => x.Values).Any(x => x.Contains(value)) == false)
                    {
                        allValid = false;
                    }
                }

                if (allValid)
                    validTickets.Add(values);
            }

            //Find indices
            FindIndices(ruleSets, validTickets);

            var rulesDict = ruleSets.ToDictionary(x => x.Index);

            long multiple = 1;
            for (var j = 0; j < myTicket.Length; j++)
            {
                var value = myTicket[j];
                var ruleSet = rulesDict[j];
                if (ruleSet.Name.StartsWith("departure", StringComparison.Ordinal))
                {
                    Assert.IsTrue(ruleSet.Values.Contains(value));
                    multiple *= value;
                }
            }

            return multiple;
        }

        [TestMethod]
        public void Setup2()
        {
            var input = InputReader.ReadInput();
            var result = SolvePuzzle2(input);
            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void Puzzle2()
        {
            var input = InputReader.ReadInput();
            var result = SolvePuzzle2(input);
            Assert.AreEqual(2587271823407, result);
        }

        #endregion
    }
}