using System.Collections.Generic;
using System.IO;
using System.Linq;

public class GrammarTree : Tree<GrammarData>
{
	private readonly Stack<TreeNode<GrammarData>> creationStack = new Stack<TreeNode<GrammarData>>();

	/// <summary>
	/// Populates this grammar tree using data at the specified file path
	/// </summary>
	/// <param name="filePath">The file location of the grammar tree data</param>
	/// <param name="hSpacing">The horizontal spacing to use between nodes</param>
	/// <param name="vSpacing">The vertical spacing to use between nodes</param>
	public GrammarTree(string filePath, float hSpacing, float vSpacing) : base(hSpacing, vSpacing)
	{
		if (!File.Exists(filePath))
		{
			throw new FileNotFoundException();
		}

		using (var reader = new StreamReader(filePath))
		{
			// Process the root node
			var currentLine = reader.ReadLine();
			var currentNodeData = new GrammarData(GetTypeOfCurrentNode(currentLine), currentLine);
			var currentNode = new TreeNode<GrammarData>(currentNodeData);
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
					currentNodeData = new GrammarData(GetTypeOfCurrentNode(nodeTextWithoutWhitespace), nodeTextWithoutWhitespace);
					currentNode = new TreeNode<GrammarData>(creationStack.Peek(), currentNodeData);
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

					currentNodeData = new GrammarData(GetTypeOfCurrentNode(nodeTextWithoutWhitespace), nodeTextWithoutWhitespace);
					currentNode = new TreeNode<GrammarData>(creationStack.Peek(), currentNodeData);
					creationStack.Push(currentNode);
				}

				// Add the new node to the list of all available nodes
				AllNodes.Add(currentNode);
			}
		}

		// Position all nodes within this grammar tree
		PositionNodes();
	}

	/// <summary>
	/// Gets the type of the given grammar node, given its text
	/// </summary>
	/// <param name="nodeText">The text of the given grammar node</param>
	/// <returns>The type of the given grammar node</returns>
	private static GrammarNodeType GetTypeOfCurrentNode(string nodeText)
	{
		if (nodeText.First() == '[')
		{
			return GrammarNodeType.NONTERMINAL;
		}

		return GrammarNodeType.TERMINAL;
	}

}