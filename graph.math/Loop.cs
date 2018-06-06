using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace graph.math
{
    class Loop
    {
        public static List<Loop> allLoops = new List<Loop>();

        public string varName;

        public int endStatment;
        public int currentVar;

        public int startLine;
        public int endLine;

        public static bool createNewLoop(string varName, int startLine, int endLine,
            int currentVar, int endStatment)
        {
            Loop loop = new Loop();

            loop.varName = varName;
            loop.startLine = startLine;
            loop.endLine = endLine;
            loop.currentVar = currentVar;
            loop.endStatment = endStatment;

            Var.createNewVar(varName, currentVar.ToString());

            allLoops.Add(loop);

            return true;
        }

        public static int returnLoop(int line)
        {
            foreach (Loop l in allLoops)
                if (line == l.endLine)
                {
                    l.currentVar++;

                    Var.allVars[l.varName].Value = l.currentVar;

                    if (l.currentVar <= l.endStatment)
                        line = (l.startLine - 1);
                }

            return line;
        }
    }
}
