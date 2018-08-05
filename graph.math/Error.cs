using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;

namespace graph.math
{
    class Error
    {
        public static TextBox graphText;

        static List<int> linesStart = new List<int>();
        static List<int> linesEnd = new List<int>();

        public static bool algorithmError(int line)
        {
            graphText.SelectionBrush = Brushes.Red;
            graphText.Focus();
            graphText.Select(linesStart[line], linesEnd[line]);

            return true;
        }

        public static void algorithmLinesCalc(string[] algorithmLines)
        {
            int currentLineStart = 0;

            linesStart.Clear();
            linesEnd.Clear();

            for (int line = 0; line < algorithmLines.Length; line++)
            {
                linesStart.Add(currentLineStart);
                linesEnd.Add(algorithmLines[line].Length);

                currentLineStart += algorithmLines[line].Length + 1;
            }
        }


    }
}
