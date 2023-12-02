namespace AoC.Year2020.Day06
{
    [TestClass]
    public class Puzzle
    {
        private int CalculateSum1(string[] input)
        {
            var groupLetters = new List<char>();
            var totalCount = 0;
            foreach (var line in input)
            {
                if (string.IsNullOrEmpty(line))
                {
                    totalCount += groupLetters.Distinct().Count();
                    groupLetters = new List<char>();
                }
                else
                {
                    groupLetters.AddRange(line);
                }
            }

            totalCount += groupLetters.Distinct().Count();

            return totalCount;
        }

        private int CalculateSum2(string[] input)
        {
            var groupLetters = new List<char>();
            var totalCount = 0;
            var first = true;
            foreach (var line in input)
            {
                if (string.IsNullOrEmpty(line))
                {
                    totalCount += groupLetters.Distinct().Count();
                    groupLetters = new List<char>();
                    first = true;
                }
                else if (first)
                {
                    groupLetters.AddRange(line);
                    first = false;
                }
                else
                {
                    groupLetters = groupLetters.Intersect(line).ToList();
                }
            }

            totalCount += groupLetters.Distinct().Count();

            return totalCount;
        }

        [TestMethod]
        public void Setup1()
        {
            var input = InputReader.ReadInput();
            Assert.AreEqual(11, CalculateSum1(input));
        }

        [TestMethod]
        public void Puzzle1()
        {
            var input = InputReader.ReadInput();
            Assert.AreEqual(6532, CalculateSum1(input));
        }

        [TestMethod]
        public void Setup2()
        {
            var input = InputReader.ReadInput();
            Assert.AreEqual(6, CalculateSum2(input));
        }

        [TestMethod]
        public void Puzzle2()
        {
            var input = InputReader.ReadInput();
            Assert.AreEqual(3427, CalculateSum2(input));
        }
    }
}