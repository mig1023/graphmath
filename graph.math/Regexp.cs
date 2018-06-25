using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace graph.math
{
    class Regexp
    {
        public static bool Check(string exp, string line)
        {
            Regex regexCheck = new Regex(exp);

            Match condition = regexCheck.Match(line);

            return condition.Success;
        }

        public static Match Check(string exp, string line, bool param = true)
        {
            Regex regexCheck = new Regex(exp);

            return regexCheck.Match(line);
        }
    }
}
