namespace AoC.Year2020.Day15
{
    [TestClass]
    public class Puzzle
    {
        #region Puzzle 1

        private object SolvePuzzle1(string input, int nr)
        {
            //zero based vs one based
            nr--;

            var split = input.Split(",").Select(int.Parse).ToList();
            var dict = new Dictionary<int, int>();
            for (var i = 0; i < split.Count; i++)
            {
                dict[split[i]] = i;
            }

            for (var i = split.Count - 1; i < nr; i++)
            {
                var last = split[i];
                if (dict.TryGetValue(last, out var previousTime) == false)
                    previousTime = i;

                split.Add(i - previousTime);
                dict[last] = i;
            }

            return split.Last();
        }

        [TestMethod]
        public void Setup1()
        {
            var input = "0,3,6";
            Assert.AreEqual(0, SolvePuzzle1(input, 8));
            Assert.AreEqual(4, SolvePuzzle1(input, 9));
            Assert.AreEqual(0, SolvePuzzle1(input, 10));
        }

        [TestMethod]
        public void Puzzle1()
        {
            var input = "20,9,11,0,1,2";
            Assert.AreEqual(1111, SolvePuzzle1(input, 2020));
        }

        #endregion

        #region Puzzle 2

        //private object SolvePuzzle2(string[] input)
        //{
        //    return 2;
        //}

        [TestMethod]
        public void Setup2()
        {
            var input = "0,3,6";
            Assert.AreEqual(175594, SolvePuzzle1(input, 30000000));
        }

        [TestMethod]
        public void Puzzle2()
        {
            var input = "20,9,11,0,1,2";
            Assert.AreEqual(48568, SolvePuzzle1(input, 30000000));
        }

        #endregion
    }
}
