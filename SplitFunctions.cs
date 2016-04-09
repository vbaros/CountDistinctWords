using System;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;

namespace CountDistinctWords
{
    /// <summary>
    /// Splits the words (space as a separator).
    /// Trims the Punctuation
    /// </summary>
    public static class SplitFunctions
    {
        public static string[] SplitToWords(string text)
        {
            string[] distinctWords = text.Split(' ', '.', ',', '!', '?', '\"'); // the list might not be full. This is just a test
            return distinctWords;
        }

        public static string[] SplitToWordsDistinct(string text) // slower
        {
            string[] distinctWords = text.Split(' ', '.', ',', '!', '?', '\"').Distinct().ToArray();
            return distinctWords;
        }

        public static string[] SplitToWordsRegex(string text)
        {
            //
            // Split on all non-word characters.
            // ... Returns an array of all the words.
            //
            return Regex.Split(text, @"\W+");
            // @      special verbatim string syntax
            // \W+    one or more non-word characters together
        }

        public static string[] SplitToWordsReplace(string text)
        {
            //return text.RemoveSpecialCharacters().Split(' '); // doesn't work. It will remove spaces as well -> Split will not work
            return text.Split().Select(word => word.RemoveSpecialCharacters()).ToArray();
        }

        public static string[] SplitWithLinq(string text)
        {
            var punctuation = text.Where(Char.IsPunctuation).Distinct().ToArray();
            return text.Split().Select(x => x.Trim(punctuation)).ToArray();
        }

        public static string[] SplitToWordsTest(string text) // slow
        {
            var blah = from word in text.Split()
                select (string.Concat(from c in word
                    where char.IsLetter(c)
                    select c));

            return blah.ToArray();
        }
    }

    public static class StringExtensions
    {
        public static string RemoveSpecialCharacters(this string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '\'') //(char.IsLetter(c))
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }
    }
}