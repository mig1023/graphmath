using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace graph.math
{
    class Conditional
    {
        public static bool skipThisBlock = false;

        public static int error(string exp)
        {
            Regex regexCheck = new Regex(@"\(\s*(\d+)\s*(==|>=|<=)\s*(\d+)\s*\)");

            Match condition = regexCheck.Match(exp);

            if (!condition.Success)
                return 0;

            else if (condition.Groups[2].Value == "==" && (condition.Groups[1].Value == condition.Groups[3].Value))
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
