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

        static int[] allIndexOf(string line, string subLine, int startIndex = 0)
        {
            var indices = new List<int>();
            int index = line.IndexOf(subLine, startIndex);

            while (index > -1)
            {
                indices.Add(index);
                index = line.IndexOf(subLine, index + subLine.Length);
            }

            return indices.ToArray();
        }

        public static string replaceVariable(string algorithmLine)
        {
            StringBuilder newAlgorithmLine = new StringBuilder(algorithmLine);

            int equalitySign = algorithmLine.IndexOf('=');

            if (equalitySign < 0) equalitySign = 0;

            for (int v = 0; v < Var.allVars.Count; v++)
            {
                string key = Var.allVars.ElementAt(v).Key;
                string arrayKey = '[' + key + ']';
                string value = Var.allVars.ElementAt(v).Value.Value.ToString();

                foreach(int index in allIndexOf(algorithmLine, arrayKey))
                    newAlgorithmLine = newAlgorithmLine.Replace(arrayKey, value);

                foreach (int index in allIndexOf(algorithmLine, key, equalitySign))
                    newAlgorithmLine = newAlgorithmLine.Replace(key, value);
            }

            return newAlgorithmLine.ToString();
        }
    }
}
