using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace MicrowaveOvenClasses
{
    //This class was created using an answer by Dmitri Bychenko on Stack Overflow on May 18 '15
    //Link: https://stackoverflow.com/a/30300521

    public static class Helper
    {
        public static bool RegexMatchWithWildCard(string value, string pattern)
        {
            return Regex.IsMatch(value, WildCardToRegular(pattern));
        }

        public static void ClearStringWriter(StringWriter stringWriter)
        {
            StringBuilder stringBuilder = stringWriter.GetStringBuilder();
            stringBuilder.Remove(0, stringBuilder.Length);
        }

        private static string WildCardToRegular(string pattern)
        {
            return "^" + Regex.Escape(pattern).Replace("\\*", ".*") + "$";
        }
    }
}