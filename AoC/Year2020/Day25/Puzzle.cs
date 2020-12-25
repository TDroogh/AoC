using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Year2020.Day25
{
    [TestClass]
    public class Puzzle
    {
        #region Puzzle 1

        private long SolvePuzzle1(long doorPk, long cardPk)
        {
            const long subject = 7;
            long doorResult = 1;
            long cardResult = 1;

            var doorLoop = DetermineLoopSize(ref doorResult, subject, doorPk);
            var cardLoop = DetermineLoopSize(ref cardResult, subject, cardPk);

            Console.WriteLine($"Door loop: {doorLoop} result {doorResult}");
            Console.WriteLine($"Card loop: {cardLoop} result {cardResult}");

            cardResult = GetValue(cardResult, doorLoop);
            doorResult = GetValue(doorResult, cardLoop);
            Assert.AreEqual(cardResult, doorResult);

            return cardResult;
        }

        private long DetermineLoopSize(ref long result, long subject, long pk)
        {
            long loops;

            for (loops = 0; result != pk && loops < 100_000_000; loops++)
                result = GetNextValue(subject, result);

            return loops;
        }

        private static long GetNextValue(long subject, long input)
        {
            var value = input * subject;
            return value % 20201227;
        }

        private static long GetValue(long subject, long loop)
        {
            long totalResult = 1;
            for (var i = 0; i < loop; i++)
                totalResult = GetNextValue(subject, totalResult);
            return totalResult;
        }

        [TestMethod]
        public void Setup0()
        {
            long result = 1;
            Assert.AreEqual(8, DetermineLoopSize(ref result, 7, 5764801));
            result = 1;
            Assert.AreEqual(11, DetermineLoopSize(ref result, 7, 17807724));

            Assert.AreEqual(5764801, GetValue(7, 8));
            Assert.AreEqual(17807724, GetValue(7, 11));
        }

        [TestMethod]
        public void Setup1()
        {
            var result = SolvePuzzle1(17807724, 5764801);
            Assert.AreEqual(14897079, result);
        }

        [TestMethod]
        public void Puzzle1()
        {
            var result = SolvePuzzle1(15733400, 6408062);
            Assert.AreEqual(16457981, result);
        }

        #endregion
    }
}