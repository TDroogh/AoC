using System;
using System.Collections.Generic;
using System.Linq;
using AoC.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Year2020.Day19
{
    [TestClass]
    public class Puzzle
    {
        #region Puzzle 1

        private int SolvePuzzle1(string[] input)
        {
            //Part 1 parse rules
            var rules = ParseRules(input);
            var options = GetOptions(rules, 0).Distinct();

            return input.Skip(rules.Count).Intersect(options).Count();
        }

        private static Dictionary<int, string> ParseRules(string[] input)
        {
            var rules = new Dictionary<int, string>();

            foreach (var line in input)
            {
                if (string.IsNullOrWhiteSpace(line))
                    break;

                var ruleNr = int.Parse(line.Split(":")[0]);
                var rule = line.Split(":")[1];

                rules.Add(ruleNr, rule);
            }

            return rules;
        }

        private static IEnumerable<string> GetOptions(Dictionary<int, string> rules, int ruleNr, Dictionary<int, int> handledRules = null)
        {
            var rule = rules[ruleNr];
            foreach (var option in rule.Split("|").Select(x => x.Trim()))
            {
                if (option == "\"a\"")
                {
                    yield return "a";
                }
                else if (option == "\"b\"")
                {
                    yield return "b";
                }
                else
                {
                    var subRules = option.Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse);
                    var results = new List<string>();
                    foreach (var subRuleNr in subRules)
                    {
                        var newHandledRules = new Dictionary<int, int>();
                        if (handledRules != null)
                            foreach (var handledRule in handledRules)
                                newHandledRules[handledRule.Key] = handledRule.Value;
                        if (newHandledRules.ContainsKey(subRuleNr))
                        {
                            if (newHandledRules[subRuleNr]++ > 2)
                                break;
                        }
                        else
                        {
                            newHandledRules[subRuleNr] = 1;
                        }

                        var subResults = GetOptions(rules, subRuleNr, newHandledRules).ToList();
                        if (results.Count == 0)
                            foreach (var subResult in subResults)
                                results.Add(subResult);
                        else
                        {
                            var newResults = new List<string>();
                            foreach (var result in results)
                                foreach (var subResult in subResults)
                                    newResults.Add(result + subResult);
                            results = newResults;
                        }
                    }

                    foreach (var result in results)
                        yield return result;
                }
            }
        }

        [TestMethod]
        public void Setup1()
        {
            var input = InputReader.ReadInput();
            var rules = ParseRules(input);
            var options = GetOptions(rules, 0).ToList();
            Assert.IsTrue(options.Contains("ababbb"));
            Assert.IsFalse(options.Contains("bababa"));
            Assert.IsTrue(options.Contains("abbbab"));
            Assert.IsFalse(options.Contains("aaabbb"));
            Assert.IsFalse(options.Contains("aaaabbb"));

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
            rules[8] = "42 | 42 8";
            rules[11] = "42 31 | 42 11 31";
            var options = GetOptions(rules, 0).Distinct();

            var count = 0;
            var inputs = new HashSet<string>(input.Skip(rules.Count));
            foreach (var option in options)
                if (inputs.Contains(option))
                    count++;

            return count;
        }

        [TestMethod]
        public void Setup2()
        {
            var input = InputReader.ReadInput("setup2");
            Assert.AreEqual(3, SolvePuzzle1(input));
            Assert.AreEqual(12, SolvePuzzle2(input));
        }

        [TestMethod]
        public void Puzzle2()
        {
            var input = InputReader.ReadInput();
            var result = SolvePuzzle2(input);
            Assert.AreEqual(2, result);
        }

        #endregion
    }
}