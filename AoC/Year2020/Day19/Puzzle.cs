namespace AoC.Year2020.Day19
{
    [TestClass]
    public class Puzzle
    {
        private record Rule
        {
            public required int Nr { get; init; }
            public char? Character { get; set; }
            public int[][]? SubRuleSets { get; set; }

            public static Rule Parse(string line)
            {
                var ruleNr = int.Parse(line.Split(":")[0]);
                var rule = line.Split(":")[1].Trim();

                var result = new Rule
                {
                    Nr = ruleNr
                };

                switch (rule)
                {
                    case "\"a\"":
                        result.Character = 'a';
                        break;
                    case "\"b\"":
                        result.Character = 'b';
                        break;
                    default:
                        result.SubRuleSets = rule
                            .Split("|")
                            .Select(x => x.Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray())
                            .ToArray();
                        break;
                }

                return result;
            }

            public bool Match(string line, Dictionary<int, Rule> otherRules)
            {
                var result = Match(line, otherRules, out var leftover);

                return result && leftover.Any(string.IsNullOrWhiteSpace);
            }

            private bool Match(string line, Dictionary<int, Rule> otherRules, out string[] leftovers)
            {
                leftovers = Array.Empty<string>();
                if (line.Length == 0)
                {
                    return false;
                }

                if (Character != default)
                {
                    if (line[0] == Character)
                    {
                        leftovers = new[] { line.Length == 1 ? string.Empty : line[1..] };
                        return true;
                    }

                    return false;
                }

                var totalSuccess = new List<string>();
                foreach (var ruleSet in SubRuleSets!)
                {
                    var successLeftovers = new[] { line };

                    foreach (var ruleNr in ruleSet)
                    {
                        var rule = otherRules[ruleNr];
                        successLeftovers = successLeftovers
                            .SelectMany(x => rule.Match(x, otherRules, out var outRules) ? outRules : Array.Empty<string>())
                            .ToArray();
                    }

                    if (successLeftovers.Any())
                        totalSuccess.AddRange(successLeftovers);
                }

                if (totalSuccess.Any())
                {
                    leftovers = totalSuccess.ToArray();
                    return true;
                }

                return false;
            }
        }

        #region Puzzle 1

        private int SolvePuzzle1(string[] input)
        {
            var rules = ParseRules(input);
            var ruleZero = rules[0];

            return input.Skip(rules.Count).Count(x => ruleZero.Match(x, rules));
        }

        private static Dictionary<int, Rule> ParseRules(string[] input)
        {
            var rules = new Dictionary<int, Rule>();

            foreach (var line in input)
            {
                if (string.IsNullOrWhiteSpace(line))
                    break;

                var rule = Rule.Parse(line);
                rules.Add(rule.Nr, rule);
            }

            return rules;
        }

        [TestMethod]
        public void Setup1()
        {
            var input = InputReader.ReadInput();
            var rules = ParseRules(input);

            var ruleZero = rules[0];

            Assert.IsTrue(ruleZero.Match("ababbb", rules));
            Assert.IsFalse(ruleZero.Match("bababa", rules));
            Assert.IsTrue(ruleZero.Match("abbbab", rules));
            Assert.IsFalse(ruleZero.Match("aaabbb", rules));
            Assert.IsFalse(ruleZero.Match("aaaabbb", rules));

            var result = SolvePuzzle1(input);
            Assert.AreEqual(2, result);
        }

        [TestMethod]
        public void Puzzle1()
        {
            var input = InputReader.ReadInput();
            var result = SolvePuzzle1(input);
            Assert.AreEqual(299, result);
        }

        #endregion

        #region Puzzle 2

        private int SolvePuzzle2(string[] input)
        {
            //Part 1 parse rules
            var rules = ParseRules(input);
            //rules[8] = "8: 42 | 42 8";
            rules[8] = Rule.Parse("8: 42 | 42 8");
            //rules[11] = "42 31 | 42 11 31";
            rules[11] = Rule.Parse("11: 42 31 | 42 11 31");

            var ruleZero = rules[0];

            return input.Skip(rules.Count).Count(x => ruleZero.Match(x, rules));
        }

        [DataTestMethod]
        [DataRow(true, true, "bbabbbbaabaabba")]
        [DataRow(true, false, "babbbbaabbbbbabbbbbbaabaaabaaa")]
        [DataRow(true, false, "aaabbbbbbaaaabaababaabababbabaaabbababababaaa")]
        [DataRow(true, false, "bbbbbbbaaaabbbbaaabbabaaa")]
        [DataRow(true, false, "bbbababbbbaaaaaaaabbababaaababaabab")]
        [DataRow(true, true, "ababaaaaaabaaab")]
        [DataRow(true, true, "ababaaaaabbbaba")]
        [DataRow(true, false, "baabbaaaabbaaaababbaababb")]
        [DataRow(true, false, "abbbbabbbbaaaababbbbbbaaaababb")]
        [DataRow(true, false, "aaaaabbaabaaaaababaa")]
        [DataRow(true, false, "aaaabbaabbaaaaaaabbbabbbaaabbaabaaa")]
        [DataRow(true, false, "aabbbbbaabbbaaaaaabbbbbababaaaaabbaaabba")]
        [DataRow(false, false, "aaaabbaaaabbaaa")]
        public void Setup2A(bool output2, bool output1, string testString)
        {
            var input = InputReader.ReadInput("setup2");
            var rules = ParseRules(input);
            var ruleZero = rules[0];
            Assert.AreEqual(ruleZero.Match(testString, rules), output1);

            //rules[8] = "8: 42 | 42 8";
            rules[8] = Rule.Parse("8: 42 | 42 8");
            //rules[11] = "42 31 | 42 11 31";
            rules[11] = Rule.Parse("11: 42 31 | 42 11 31");

            Assert.AreEqual(ruleZero.Match(testString, rules), output2);
        }

        [TestMethod]
        public void Setup2B()
        {
            var input = InputReader.ReadInput("setup2");
            Assert.AreEqual(3, SolvePuzzle1(input));
        }

        [TestMethod]
        public void Setup2C()
        {
            var input = InputReader.ReadInput("setup2");
            Assert.AreEqual(12, SolvePuzzle2(input));
        }

        [TestMethod]
        public void Puzzle2()
        {
            var input = InputReader.ReadInput();
            var result = SolvePuzzle2(input);
            Assert.AreEqual(414, result);
        }

        #endregion
    }
}