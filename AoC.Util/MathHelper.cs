namespace AoC.Util
{
    public static class MathHelper
    {
        public static long LeastCommonMultiple(long a, long b)
        {
            // https://stackoverflow.com/a/20824923/5367685
            return a / GreatestCommonFactor(a, b) * b;
        }

        private static long GreatestCommonFactor(long a, long b)
        {
            while (b != 0)
            {
                var temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }
    }
}
