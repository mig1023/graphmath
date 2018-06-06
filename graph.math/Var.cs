using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace graph.math
{
    class Var
    {
        public static Dictionary<string, Var> allVars = new Dictionary<string, Var>();

        public string variableName;

        public double Value;

        public static bool createNewVar(string varName, string value)
        {
            Var var = new Var();

            var.variableName = varName;

            int equalitySign = value.IndexOf('=');

            var.Value =int.Parse(value.Substring(equalitySign+1).Trim());

            if (allVars.ContainsKey(varName))
                allVars[varName] = var;
            else      
                allVars.Add(varName, var);

            return true;
        }

        public static bool isVariable(string algorithmLine)
        {
            Regex number = new Regex(@"=\s*[0-9]+\n?\r?$");

            if (number.IsMatch(algorithmLine))
                return true;
            else
                return false;
        }
    }
}
