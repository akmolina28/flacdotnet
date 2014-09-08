using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LibMetadata.IO
{
    public static class TapeInfoHelper
    {
        /// <summary>
        /// Attempts to parse out the track titles and return them as a list.
        /// </summary>
        /// <remarks>
        /// The function requires a seed title to assist the pattern matching.
        /// </remarks>
        /// <param name="infoText">The text read directly from the info file.</param>
        /// <param name="titleSeed">One manually parsed out title.</param>
        /// <returns></returns>
        public static string[] GetTitles(string infoText, string titleSeed, bool removeSpecialChars = true)
        {
            if (string.IsNullOrWhiteSpace(titleSeed))
            {
                throw new ArgumentException("titleSeed can't be empty");
            }

            char[] specialChars = new char[] { '*', '>', '#', '$', '%', '^', '&' };

            List<string> ret = new List<string>();
            ComplexToken ctStart = null;
            ComplexToken ctEnd = null;
            bool found = false;

            string[] lines = infoText.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Contains(titleSeed))
                {
                    string[] split = lines[i].Split(new string[] { titleSeed }, StringSplitOptions.None);
                    
                    ctStart = new ComplexToken(split[0]);
                    ctEnd = new ComplexToken(split[1]);

                    found = true;
                    break;
                }
            }

            int lineIndex = 0;
            while (found)
            {
                if (lineIndex == lines.Length)
                {
                    break;
                }

                if (ctStart.StringStartsWithToken(lines[lineIndex])) //&& ctEnd.StringEndsWithToken(lines[lineIndex]))
                {
                    string title = ctStart.ReplaceTokenWith(lines[lineIndex], "");
                    title = ctEnd.ReplaceTokenWith(title, "");
                    foreach (char special in specialChars)
                    {
                        title = title.Replace(special.ToString(), "");
                    }
                    if (!string.IsNullOrEmpty(title))
                    {
                        ret.Add(title.Trim());
                    }
                }

                lineIndex++;
            }

            return ret.ToArray();
        }

        public static DateTime GetDateFromFileName(string fileName)
        {
            DateTime ret = DateTime.Now;
            Match m = Regex.Match(fileName, "[1-2][0-9][0-9][0-9]-[0-1][0-9]-[0-3][0-9]");
            if (m.Success)
            {
                return DateTime.ParseExact(m.Value, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            }
            m = Regex.Match(fileName, @"[0-9][0-9]-[0-1][0-9]-[0-3][0-9]");
            if (m.Success)
            {
                ret = DateTime.ParseExact(m.Value, "yy-MM-dd", CultureInfo.InvariantCulture);
            }
            return ret;
        }
    }
}
