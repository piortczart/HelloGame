namespace HelloGame.Common.Extensions
{
    public static class ExtensionsString
    {
        public static string SubstringSafe(this string text, int start, int length)
        {
            return text.Length <= start ? ""
                : text.Length - start <= length ? text.Substring(start)
                : text.Substring(start, length);
        }
    }
}