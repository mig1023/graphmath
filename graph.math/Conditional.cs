using System.Text.RegularExpressions;

namespace graph.math
{
    class Conditional
    {
        public static int check(string exp)
        {
            Match condition = Regexp.Check(@"\(\s*(\d+)\s*(==|>=|<=|>|<)\s*(\d+)\s*\)", exp, param: true);

            if (!condition.Success)
                return 0;

            else if (condition.Groups[2].Value == "==" && (condition.Groups[1].Value == condition.Groups[3].Value))
                return 1;

            else if (condition.Groups[2].Value == ">" && (int.Parse(condition.Groups[1].Value) > int.Parse(condition.Groups[3].Value)))
                return 1;

            else if (condition.Groups[2].Value == "<" && (int.Parse(condition.Groups[1].Value) < int.Parse(condition.Groups[3].Value)))
                return 1;

            else if (condition.Groups[2].Value == ">=" && (int.Parse(condition.Groups[1].Value) >= int.Parse(condition.Groups[3].Value)))
                return 1;

            else if (condition.Groups[2].Value == "<=" && (int.Parse(condition.Groups[1].Value) <= int.Parse(condition.Groups[3].Value)))
                return 1;

            else
                return 2;
        }
    }
}
