namespace CommonLibrary.Extensions
{
    public static class StringExtensions
    {
        public static string FormatString(this string s, params object[] args)
        {
            return string.Format(s, args);
        }
    }
}