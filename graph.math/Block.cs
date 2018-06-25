using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace graph.math
{
    class Block
    {
        public static Dictionary<int, Block> allBlocks = new Dictionary<int, Block>();

        public static Stack<int> lastBlock = new Stack<int>();

        public int startLine;
        public int endLine = -1;
        public bool skipThisBlock = false;

        public static void openBlock(int startLine)
        {
            Block block = new Block();

            block.startLine = startLine;

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

        public static void lineSeparator(string[] algorithmLines)
        {
            int currentIndentation;
            int prevIndentation = 0;
            int lastLine = 0;

            Regex regexSpaces = new Regex(@"^(\s+)");
            Regex regexTabs = new Regex(@"^(\t+)");

            for (int line = 0; line < algorithmLines.Length; line++)
            {
                currentIndentation = 0;

                Match matchSpace = regexSpaces.Match(algorithmLines[line]);

                if (matchSpace.Success)
                    currentIndentation =
                        matchSpace.Groups[1].Value.ToCharArray().Where(c => c == ' ').Count() / 4;

                Match matchTabs = regexTabs.Match(algorithmLines[line]);

                if (matchTabs.Success)
                    currentIndentation =
                        matchTabs.Groups[1].Value.ToCharArray().Where(c => c == Convert.ToChar(9)).Count();

                if (currentIndentation > prevIndentation)
                    openBlock(line);

                else if (currentIndentation < prevIndentation)
                    for(int a = currentIndentation; a > prevIndentation; a--)
                    	closeBlock(line);

                prevIndentation = currentIndentation;
                lastLine = line;
            }

            closeAllUnclosedBlock(lastLine);
        }
    }
}
