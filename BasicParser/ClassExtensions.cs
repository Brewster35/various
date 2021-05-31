using System.Text.RegularExpressions;

namespace Parser
{
    static class ClassExtensions
    {
        public static string RegExMatch(this string str, string pattern)
        {
            Regex regEx;
            MatchCollection matches;

            regEx = new Regex(pattern);
            matches = regEx.Matches(str);

            if (matches.Count > 0)
            {
                return matches[0].Value;
            }

            return null;
        }
    }
}
