namespace AoC.Year2020.Day05
{
    [TestClass]
    public class Puzzle
    {
        private int GetSeatId(string input)
        {
            var result = 0;
            var multi = 512;

            for (var i = 0; i < 7; i++)
            {
                if (input[i] == 'B')
                    result += multi;
                multi /= 2;
            }
            for (var i = 7; i < 10; i++)
            {
                if (input[i] == 'R')
                    result += multi;
                multi /= 2;
            }

            return result;
        }

        [TestMethod]
        public void Setup1()
        {
            var input = InputReader.ReadInput();
            var max = input.Max(GetSeatId);

            Assert.AreEqual(820, max);
        }

        [TestMethod]
        public void Puzzle1()
        {
            var input = InputReader.ReadInput();
            var max = input.Max(GetSeatId);

            Assert.AreEqual(933, max);
        }

        [TestMethod]
        public void Puzzle2()
        {
            var input = InputReader.ReadInput();
            var ids = input.Select(GetSeatId).OrderBy(x => x).ToArray();

            var result = 0;

            foreach (var id in GetNumbers(1025))
                if (ids.Contains(id - 1) && ids.Contains(id + 1) && !ids.Contains(id))
                    result = id;

            Assert.AreEqual(711, result);
        }

        private IEnumerable<int> GetNumbers(int max)
        {
            for (var i = 0; i < max; i++)
                yield return i + 1;
        }
    }
}