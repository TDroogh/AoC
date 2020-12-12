using System.Collections.Generic;
using System.Linq;
using AoC.Inputs;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Year2020
{
    [TestClass]
    public class Day7
    {
        private class Bag
        {
            public string Name { get; set; }
            public Dictionary<string, int> Carries { get; set; }

            public static Bag Parse(string line)
            {
                var bag = new Bag {Name = line.Split("bags")[0].Trim()};
                var contains = line.Split("contain")[1].Trim().TrimEnd('.');

                bag.Carries = contains.Split(',')
                    .Select(ParseContains)
                    .Where(x => x.Value > 0)
                    .ToDictionary(x => x.Key, y => y.Value);

                return bag;
            }

            private static KeyValuePair<string, int> ParseContains(string contains)
            {
                contains = contains.Trim();

                if (contains.StartsWith("no")) return new KeyValuePair<string, int>("other", 0);

                var containPart = contains.Split("bag")[0].Trim();
                var countPart = containPart.Split(" ")[0];
                var count = int.Parse(countPart);
                var namePart = containPart.Replace(countPart, "").Trim();

                return new KeyValuePair<string, int>(namePart, count);
            }

            public bool CarriesBag(string other, Dictionary<string, Bag> allBags)
            {
                if (Carries.Keys.Any())
                {
                    if (Carries.Keys.Any(x => x == other))
                        return true;

                    foreach (var bagName in Carries.Keys)
                    {
                        var bag = allBags[bagName];
                        if (bag.CarriesBag(other, allBags))
                            return true;
                    }
                }

                return false;
            }

            public int BagsCarried(Bag[] allBags)
            {
                var count = 0;
                
                foreach (var carry in Carries)
                {
                    count += carry.Value;

                    var bag = allBags.Single(x => x.Name == carry.Key);
                    count += bag.BagsCarried(allBags) * carry.Value;
                }

                return count;
            }
        }

        [TestMethod]
        public void Setup1()
        {
            var input = InputReader.ReadInput(2020, 7, 0);
            var bags = input.Select(Bag.Parse).ToArray();

            var bagToTest = "shiny gold";
            var result = bags.Count(x => x.CarriesBag(bagToTest, bags.ToDictionary(y => y.Name)));

            Assert.AreEqual(4, result);
        }

        [TestMethod]
        public void Puzzle1()
        {
            var input = InputReader.ReadInput(2020, 7, 1);
            var bags = input.Select(Bag.Parse).ToArray();

            var bagToTest = "shiny gold";
            var result = bags.Count(x => x.CarriesBag(bagToTest, bags.ToDictionary(y => y.Name)));

            Assert.AreEqual(185, result);
        }

        [TestMethod]
        public void Setup2()
        {
            var input = InputReader.ReadInput(2020, 7, 0);
            var bags = input.Select(Bag.Parse).ToArray();

            var bag = bags.Single(x => x.Name == "shiny gold");
            var result = bag.BagsCarried(bags);

            Assert.AreEqual(32, result);
        }

        [TestMethod]
        public void Puzzle2()
        {
            var input = InputReader.ReadInput(2020, 7, 1);
            var bags = input.Select(Bag.Parse).ToArray();

            var bag = bags.Single(x => x.Name == "shiny gold");
            var result = bag.BagsCarried(bags);

            Assert.AreEqual(89084, result);
        }
    }
}
