namespace AoC.Util
{
    public static class StringHelper
    {
        public static string ReplaceFirst(this string from, string search, string replace)
        {
            var pos = from.IndexOf(search, StringComparison.Ordinal);
            if (pos < 0)
                return from;

            return from[..pos] + replace + from[(pos + search.Length)..];
        }
    }
}
