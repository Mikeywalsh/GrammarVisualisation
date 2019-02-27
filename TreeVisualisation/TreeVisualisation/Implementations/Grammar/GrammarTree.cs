using System.Collections.Generic;
using System.IO;
using System.Linq;
using TreeVisualisation.Core;

namespace TreeVisualisation.Implementations.Grammar
{
	public sealed class GrammarTree : Tree<GrammarData>
	{
		/// <summary>
		/// Creates a grammar tree using data at the specified file path
		/// </summary>
		/// <param name="filePath">The file location of the grammar tree data</param>
		/// <param name="hSpacing">The horizontal spacing to use between nodes</param>
		/// <param name="vSpacing">The vertical spacing to use between nodes</param>
		public GrammarTree(string filePath, float hSpacing, float vSpacing) : base(hSpacing, vSpacing)
		{
			// Initialise the creation stack used to assist parsing
			var creationStack = new Stack<TreeNode<GrammarData>>();

			if (!File.Exists(filePath))
			{
				throw new FileNotFoundException();
			}

			using (var reader = new StreamReader(filePath))
			{
				// Process the root node
				var currentLine = reader.ReadLine();
				var currentNodeData = new GrammarData(GetTypeOfCurrentNode(currentLine), currentLine);

				// Set the root node of this tree
				var currentNode = SetRoot(currentNodeData);
				creationStack.Push(currentNode);

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
						currentNode = Add(creationStack.Peek(), currentNodeData);
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
						currentNode = Add(creationStack.Peek(), currentNodeData);
						creationStack.Push(currentNode);
					}
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
}