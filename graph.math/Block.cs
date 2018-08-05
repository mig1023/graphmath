using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace graph.math
{
    class Block
    {
        public static Dictionary<int, Block> allBlocks = new Dictionary<int, Block>();

        public static Stack<int> lastBlock = new Stack<int>();

        public static string[] algorithmLines;

        public int startLine;
        public int endLine = -1;
        int currentIndentation = 0;
        public bool skipThisBlock = false;

        public static void openBlock(int startLine, int currentIndentation)
        {
            Block block = new Block();

            block.startLine = startLine;

            block.currentIndentation = currentIndentation;

            allBlocks.Add(startLine, block);

            lastBlock.Push(startLine);
        }

        public static void closeBlock(int endLine)
        {
            allBlocks[lastBlock.Pop()].endLine = endLine - 1;
        }

        public static void closeAllUnclosedBlock(int endLine)
        {
            foreach (KeyValuePair<int, Block> b in allBlocks)
                if (b.Value.endLine == -1)
                    b.Value.endLine = endLine;
        }

        public static int startOfThisBlock(int endLine)
        {
            int startLine = -1;
            int lineIndentation = currentLineIndentation(algorithmLines[endLine]) + 1;

            for (int line = 0; line < endLine; line++)
            {
                if (algorithmLines[line].IndexOf("if") == -1) continue;

                if (!allBlocks.ContainsKey(line + 1)) return -1;

                if (allBlocks[line + 1].currentIndentation != lineIndentation) continue;

                startLine = line + 1;
            }

            return startLine;
        }

        static int currentLineIndentation(string line)
        {
            int currentIndentation = 0;

            Match matchSpace = Regexp.Check(@"^(\s+)", line, param: true);

            if (matchSpace.Success)
                currentIndentation =
                    matchSpace.Groups[1].Value.ToCharArray().Where(c => c == ' ').Count() / 4;

            Match matchTabs = Regexp.Check(@"^(\t+)", line, param: true);

            if (matchTabs.Success)
                currentIndentation =
                    matchTabs.Groups[1].Value.ToCharArray().Where(c => c == Convert.ToChar(9)).Count();

            return currentIndentation;
        }

        public static void lineSeparator(string[] algorithmLines)
        {
            int currentIndentation;
            int prevIndentation = 0;
            int lastLine = 0;

            for (int line = 0; line < algorithmLines.Length; line++)
            {
                if (Regexp.Check(@"^[\t\r\n\s]*$", algorithmLines[line]))
                    continue;

                currentIndentation = currentLineIndentation(algorithmLines[line]);

                if (currentIndentation > prevIndentation)
                    openBlock(line, currentIndentation);

                else if (currentIndentation < prevIndentation)
                    for (int a = currentIndentation; a < prevIndentation; a++)
                        closeBlock(line);

                prevIndentation = currentIndentation;
                lastLine = line;
            }

            closeAllUnclosedBlock(lastLine);
        }
    }
}
