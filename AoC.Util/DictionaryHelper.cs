namespace AoC.Util
{
    public static class DictionaryHelper
    {
        public static void AddOrIncrement<T>(this Dictionary<T, long> templateCounts, T key, long incrementBy = 1) where T : notnull
        {
            if (templateCounts.ContainsKey(key))
                templateCounts[key] += incrementBy;
            else
                templateCounts[key] = incrementBy;
        }

        public static void AddOrIncrement<T>(this Dictionary<T, int> templateCounts, T key, int incrementBy = 1) where T : notnull
        {
            if (templateCounts.ContainsKey(key))
                templateCounts[key] += incrementBy;
            else
                templateCounts[key] = incrementBy;
        }
    }
}
