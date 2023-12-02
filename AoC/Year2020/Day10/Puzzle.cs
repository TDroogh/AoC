using System.Diagnostics;

namespace AoC.Year2020.Day10
{
    [TestClass]
    public class Puzzle
    {
        private int GetMultiple(int[] input)
        {
            var list = input.ToList();
            list.Add(0);
            list.Add(list.Max() + 3);
            var ordered = list.OrderBy(x => x).ToList();

            var diff1 = 0;
            var diff3 = 0;
            for (var i = 0; i < ordered.Count - 1; i++)
            {
                var current = ordered[i];
                var next = ordered[i + 1];

                switch (next - current)
                {
                    case 1:
                        diff1++;
                        break;
                    case 3:
                        diff3++;
                        break;
                }
            }

            return diff1 * diff3;
        }

        [TestMethod]
        public void Setup1()
        {
            var input = InputReader.ReadIntInput("setup1");
            Assert.AreEqual(220, GetMultiple(input));
        }

        [TestMethod]
        public void Puzzle1()
        {
            var input = InputReader.ReadIntInput();
            Assert.AreEqual(2475, GetMultiple(input));
        }

        private long CountArrangements(int[] input)
        {
            var list = input.ToList();
            list.Add(0);
            list.Add(list.Max() + 3);
            var ordered = list.OrderBy(x => x).ToList();
            long count = 1;
            var consecutive = 1;
            for (var i = 0; i < ordered.Count - 1; i++)
            {
                var current = ordered[i];
                var next = ordered[i + 1];

                switch (next - current)
                {
                    case 1:
                        consecutive++;
                        break;
                    case 3:
                        if (consecutive >= 2)
                            count *= GetPossibilities(consecutive);
                        consecutive = 1;
                        break;
                }
            }

            return count;
        }

        private int GetPossibilities(int seq)
        {
            var result = seq switch
            {
                1 => 1,
                2 => 1,
                3 => 2,
                4 => 4,
                5 => 7,
                _ => 1,
            };

            if (seq > 5)
                Trace.WriteLine(seq);
            return result;
        }

        [TestMethod]
        public void Setup2A()
        {
            var input = InputReader.ReadIntInput("setup2");
            Assert.AreEqual(8, CountArrangements(input));
        }

        [TestMethod]
        public void Setup2B()
        {
            var input = InputReader.ReadIntInput("setup1");
            Assert.AreEqual(19208, CountArrangements(input));
        }

        [TestMethod]
        public void Puzzle2()
        {
            var input = InputReader.ReadIntInput();
            Assert.AreEqual(442136281481216, CountArrangements(input));
        }
    }
}
