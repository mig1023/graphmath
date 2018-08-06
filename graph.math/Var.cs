using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

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

        public static bool IncrementDecrement(string varName, string value)
        {
            Match condition = Regexp.Check(@"(\+|\-)\=\s*([0-9]+)\s*\n?\r?$", value, param: true);

            int change = (condition.Groups[1].Value == "-" ? -1 : 1);

            if (allVars.ContainsKey(varName))
            {
                allVars[varName].Value += Double.Parse(condition.Groups[2].Value) * change;
                return true;
            }

            return false;
        }

        public static bool isVariable(string algorithmLine)
        {
            if (Regexp.Check(@"=\s*\-?\s*[0-9]+\s*\n?\r?$", algorithmLine))
                return true;
            else
                return false;
        }

        public static bool isIncDecrement(string algorithmLine)
        {
            if (Regexp.Check(@"(\+|\-)\=\s*[0-9]+\s*\n?\r?$", algorithmLine))
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

            for (int v = 0; v < allVars.Count; v++)
            {
                string key = allVars.ElementAt(v).Key;
                string arrayKey = '[' + key + ']';
                string value = allVars.ElementAt(v).Value.Value.ToString();

                int startPos = 0;

                if (isIncDecrement(algorithmLine) || isVariable(algorithmLine))
                    startPos = algorithmLine.IndexOf("=");

                foreach (int index in allIndexOf(algorithmLine, arrayKey))
                    newAlgorithmLine = newAlgorithmLine.Replace(arrayKey, value);

                foreach (int index in allIndexOf(algorithmLine, key, startPos))
                    newAlgorithmLine = newAlgorithmLine.Replace(key, value);
            }

            return newAlgorithmLine.ToString();
        }

        public static string parseVariable(string algorithmLine)
        {
            int equalitySign = algorithmLine.IndexOf('=');

            if (equalitySign == -1) return "";

            if (algorithmLine.IndexOf("==") >= 0) return "";

            if (Regexp.Check(@"(\+\=|\-\=|\*\=)", algorithmLine)) equalitySign -= 1;

            return algorithmLine.Substring(0, equalitySign).Trim();
        }
    }
}
