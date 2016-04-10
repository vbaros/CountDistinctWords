using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CountDistinctWords
{
    /// <summary>
    /// Splits a line of text to separate words (for a given separator).
    /// </summary>
    public static class SplitFunctions
    {
        /// <summary>
        /// Split to words by using a given set of characters. If characters are knows
        /// this method could be the fastest.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string[] SplitToWordsNormal(string text)
        {
            string[] distinctWords = text.Split(' ', '.', ',', '!', '?'); // the list is no full. This is just a test.
            return distinctWords;
        }

        /// <summary>
        /// Same as default SplitToWords function. The difference is it calls the distinct on resulting word list.
        /// It turns out, the method is slower then calling Distinct() on the whole word list at the end.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string[] SplitToWordsNormalDistinct(string text)
        {
            string[] distinctWords = text.Split(' ', '.', ',', '!', '?', '\"').Distinct().ToArray();
            return distinctWords;
        }

        /// <summary>
        /// Splits to words using Regex function. Regex is slow for splitting.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Splits then removes special scharacters from the words using StringBuilder
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string[] SplitToWordsReplace(string text)
        {
            //return text.RemoveSpecialCharacters().Split(' '); // doesn't work. It will remove spaces as well so splitting will not work
            return text.Split().Select(word => word.RemoveSpecialCharacters()).ToArray();
        }

        /// <summary>
        /// Takes punctuation characters and trims them from the words after splitting.
        /// In most cases the fastest
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
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