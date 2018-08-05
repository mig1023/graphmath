using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace graph.math
{
    class Parse
    {
        public static string[] parseStrParam(string algorithmLine)
        {
            int start = algorithmLine.IndexOf('(') + 1;
            int end = algorithmLine.IndexOf(')') - algorithmLine.IndexOf('(') - 1;

            if (start == -1 || end == -1) return new string[] { };

            string[] paramLines = Regex.Split(algorithmLine.Substring(start, end), ",|:|->");

            string[] paramLinesT = Array.ConvertAll(paramLines, n => n.Trim());

            return paramLinesT;
        }

        public static int[] parseParam(string algorithmLine)
        {
            string[] paramLines = parseStrParam(algorithmLine);

            int param;

            for (int a = 0; a < paramLines.Length; a++)
                if (!int.TryParse(paramLines[a], out param))
                    return new int[] { };

            return Array.ConvertAll(paramLines, n => int.Parse(n));
        }

    }
}
