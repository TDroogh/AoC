using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Infi
{
    [TestClass]
    public class Year2020
    {
        private long GetPackCount(int ribSize)
        {
            long totalRib = ribSize * 3;
            var square = totalRib * totalRib;
            for (var i = 1; i <= ribSize; i++)
                square -= i * 4;
            return square;
        }

        private int GetSize(long numOfPackages)
        {
            for (var i = 1;; i++)
            {
                var count = GetPackCount(i);
                if (count > numOfPackages)
                    return i;
            }
        }

        [TestMethod]
        public void Puzzle1()
        {
            var size = GetSize(17487455);

            Assert.AreEqual(1581, size);
        }

        [TestMethod]
        public void Puzzle2()
        {
            //var asia = GetSize(4_541_364_666);
            //var africa = GetSize(1_340_974_282);
            //var europe = GetSize(747_797_282);
            //var southAm = GetSize(430_850_243);
            //var northAm = GetSize(368_958_361);
            //var oceania = GetSize(42_729_035);

            //Assert.AreEqual(67227, asia + africa + europe + southAm + northAm + oceania);
        }
    }
}
