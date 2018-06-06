using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace graph.math
{
    class Block
    {
        public static Dictionary<int, Block> allBlocks = new Dictionary<int, Block>();

        public static Stack<int> lastBlock = new Stack<int>();

        public int startLine;
        public int endLine = -1;

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
    }
}
