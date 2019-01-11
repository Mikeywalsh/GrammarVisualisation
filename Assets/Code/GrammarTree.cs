using System.Collections.Generic;
using System.Linq;
using System.IO;

/// <summary>
/// This class represents a grammar tree
/// </summary>
public class GrammarTree
{
    /// <summary>
    /// The root node of the tree
    /// </summary>
    public GrammarTreeNode Root { get; private set; }

    /// <summary>
    /// A list of all nodes contained within the grammar tree
    /// </summary>
    public List<GrammarTreeNode> AllNodes { get; private set; }

    private Stack<GrammarTreeNode> CreationStack = new Stack<GrammarTreeNode>();

    /// <summary>
    /// The maximum depth of this grammar tree
    /// </summary>
    public int MaxDepth => AllNodes.Max(n => n.Depth);

    public GrammarTree()
    {
        AllNodes = new List<GrammarTreeNode>();
    }

    /// <summary>
    /// Populates this grammar tree using data at the specified file path
    /// </summary>
    /// <param name="path">The file location of the grammar tree data</param>
    public void ReadFromFile(string path)
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

                // Add the new node to the list of all available nodes
                AllNodes.Add(currentNode);
            }
        }
    }

    private static GrammarNodeType GetTypeOfCurrentNode(string nodeText)
    {
        if (nodeText.First() == '[')
        {
            return GrammarNodeType.NONTERMINAL;
        }

        return GrammarNodeType.TERMINAL;
    }
}
