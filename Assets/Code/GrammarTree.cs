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
    public List<GrammarTreeNode> AllNodes { get; private set; } = new List<GrammarTreeNode>();

    private readonly Stack<GrammarTreeNode> creationStack = new Stack<GrammarTreeNode>();

    /// <summary>
    /// The maximum depth of this grammar tree
    /// </summary>
    public int MaxDepth => AllNodes.Max(n => n.Depth);

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

        using (var reader = new StreamReader(path))
        {
            // Process the root node
            var currentLine = reader.ReadLine();
            var currentNode = new GrammarTreeNode(GetTypeOfCurrentNode(currentLine), currentLine);
            creationStack.Push(currentNode);
            Root = currentNode;

            // Add the new node to the list of all available nodes
            AllNodes.Add(currentNode);

            // Process each line individually
            while (!reader.EndOfStream)
            {
                currentLine = reader.ReadLine();

                // Determine the depth of the current node
                var currentDepth = currentLine.TakeWhile(x => x == ' ').Count();

                var nodeTextWithoutWhitespace = currentLine.Substring(currentDepth);

                if (currentDepth > creationStack.Peek().Depth)
                {
                    // Child of previous node
                    currentNode = new GrammarTreeNode(creationStack.Peek(), GetTypeOfCurrentNode(nodeTextWithoutWhitespace), nodeTextWithoutWhitespace);
                    creationStack.Push(currentNode);
                }
                else
                {
                    // Pop the creation stack until we arrive at one with the same depth
                    // This is a sibling node
                    while (currentDepth <= creationStack.Peek().Depth)
                    {
                        creationStack.Pop();
                    }

                    currentNode = new GrammarTreeNode(creationStack.Peek(), GetTypeOfCurrentNode(nodeTextWithoutWhitespace), nodeTextWithoutWhitespace);
                    creationStack.Push(currentNode);
                }

                // Add the new node to the list of all available nodes
                AllNodes.Add(currentNode);
            }
        }
    }

    public List<GrammarTreeNode> GetLeafNodes()
    {
        return AllNodes.Where(node => node.Children.Count == 0).ToList();
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
