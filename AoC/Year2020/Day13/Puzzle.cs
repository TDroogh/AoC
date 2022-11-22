using AoC.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace AoC.Year2020.Day13
{
    [TestClass]
    public class Puzzle
    {
        #region Puzzle 1

        private object SolvePuzzle1(string[] input)
        {
            var minutes = int.Parse(input[0]);
            var busIds = input[1].Split(',')
                .Where(x => x != "x")
                .Select(int.Parse);

            var minLength = int.MaxValue;
            var minBus = 0;

            foreach (var busId in busIds)
            {
                var duration = busId - (minutes % busId);
                if (duration < minLength)
                {
                    minBus = busId;
                    minLength = duration;
                }
            }

            return minLength * minBus;
        }

        [TestMethod]
        public void Setup1()
        {
            var input = InputReader.ReadInput();
            var result = SolvePuzzle1(input);
            Assert.AreEqual(295, result);
        }

        [TestMethod]
        public void Puzzle1()
        {
            var input = InputReader.ReadInput();
            var result = SolvePuzzle1(input);
            Assert.AreEqual(222, result);
        }

        #endregion

        #region Puzzle 2

        private object SolvePuzzle2(string[] input)
        {
            long maxMinutes = 1;
            var i = 0;
            var busIds = input[1].Split(',')
                .ToDictionary(_ => i++, x =>
                {
                    if (x == "x")
                        return -1;
                    var id = int.Parse(x);
                    maxMinutes *= id;
                    return id;
                })
                .Where(y => y.Value > 0)
                .ToDictionary(x => x.Key, x => x.Value);

            var startBus = busIds[0];
            var busInc = startBus;
            var firstBus = busIds.Skip(1).First();

            long startMinute = 0;
            while (true)
            {
                if ((startMinute + firstBus.Key) % firstBus.Value == 0)
                    break;
                startMinute += busInc;
            }
            busInc *= firstBus.Value;

            var secondBus = busIds.Skip(2).First();
            while (true)
            {
                if ((startMinute + secondBus.Key) % secondBus.Value == 0)
                    break;
                startMinute += busInc;
            }
            busInc *= secondBus.Value;

            var thirdBus = busIds.Skip(3).First();
            while (true)
            {
                if ((startMinute + thirdBus.Key) % thirdBus.Value == 0)
                    break;
                startMinute += busInc;
            }
            busInc *= thirdBus.Value;

            var fourthBus = busIds.Skip(4).First();
            while (true)
            {
                if ((startMinute + fourthBus.Key) % fourthBus.Value == 0)
                    break;
                startMinute += busInc;
            }
            busInc *= fourthBus.Value;

            while (startMinute < maxMinutes)
            {
                if (busIds.Count == 5 || busIds.Skip(5).All(x => (startMinute + x.Key) % x.Value == 0))
                    return startMinute;

                startMinute += busInc;
            }

            return -1;
        }

        [TestMethod]
        public void Setup2()
        {
            var input = InputReader.ReadInput();
            var result = SolvePuzzle2(input);
            Assert.AreEqual((long)1068781, result);
        }

        [TestMethod]
        public void Puzzle2()
        {
            var input = InputReader.ReadInput();
            var result = SolvePuzzle2(input);
            Assert.AreEqual(408270049879073, result);
        }

        #endregion
    }
}