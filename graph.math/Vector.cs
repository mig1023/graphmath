using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace graph.math
{
    class Vector
    {
        public static Dictionary<string, Vector> allVectors = new Dictionary<string, Vector>();

        public string variableName;

        public double x1;
        public double y1;

        public double x2;
        public double y2;

        public static void createNewVector(string varName, double x1, double y1, double x2, double y2)
        {
            Vector vec = new Vector();

            vec.variableName = varName;
            vec.x1 = x1;
            vec.y1 = y1;
            vec.x2 = x2;
            vec.y2 = y2;

            Vector.allVectors.Add(varName, vec);
        }

        public static void createNewVector(string varName, int[] coordinate)
        {
            createNewVector(
                varName: varName,
                x1: coordinate[0],
                y1: coordinate[1],
                x2: coordinate[2],
                y2: coordinate[3]
            );
        }
    }
}
