using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace GrammarTreeGenerator
{
    /// <summary>
    /// This class reads grammar tree data from a file and constructs a series
    /// of <see cref="TreeNode"/>'s representing the tree contained within the data
    /// </summary>
    public class GrammarTreeReader
    {
        /// <summary>
        /// The root node of the tree generated from reading
        /// </summary>
        public GrammarTreeNode Root { get; private set; }

        private Stack<GrammarTreeNode> CreationStack = new Stack<GrammarTreeNode>();

        /// <summary>
        /// Creates a new tree using data at the specified file path
        /// </summary>
        /// <param name="path">The file location of the grammar tree data</param>
        public GrammarTreeReader(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException();
            }

            using (StreamReader reader = new StreamReader(path))
            {
                // Process the root node
                string currentLine = reader.ReadLine();
                GrammarTreeNode currentNode = new GrammarTreeNode(GetTypeOfCurrentNode(currentLine), currentLine);
                CreationStack.Push(currentNode);
                Root = currentNode;

                // Process each line individually
                while (!reader.EndOfStream)
                {
                    currentLine = reader.ReadLine();

                    // Determine the depth of the current node
                    int currentDepth = currentLine.TakeWhile(x => x == ' ').Count();

                    string nodeTextWithoutWhitespace = currentLine.Substring(currentDepth);

                    if (currentDepth > CreationStack.Peek().Depth)
                    {
                        // Child of previous node
                        currentNode = new GrammarTreeNode(CreationStack.Peek(), GetTypeOfCurrentNode(nodeTextWithoutWhitespace), nodeTextWithoutWhitespace);
                        CreationStack.Push(currentNode);
                    }
                    else
                    {
                        // Pop the creation stack until we arrive at one with the same depth
                        // This is a sibling node
                        while (currentDepth <= CreationStack.Peek().Depth)
                        {
                            CreationStack.Pop();
                        }

                        currentNode = new GrammarTreeNode(CreationStack.Peek(), GetTypeOfCurrentNode(nodeTextWithoutWhitespace), nodeTextWithoutWhitespace);
                        CreationStack.Push(currentNode);
                    }
                }
            }
        }

        private static GrammarNodeType GetTypeOfCurrentNode(string nodeText)
        {
            if(nodeText.First() == '[')
            {
                return GrammarNodeType.NONTERMINAL;
            }

            return GrammarNodeType.TERMINAL;
        }
    }
}