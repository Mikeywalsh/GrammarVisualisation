using System;
using System.Collections.Generic;
using System.Linq;

namespace TreeVisualisation.Core
{
	/// <summary>
	/// This class represents a tree, used to store data in a hierarchical manner
	/// </summary>
	public class Tree<T>
	{
		/// <summary>
		/// The root node of the tree
		/// </summary>
		public TreeNode<T> Root { get; protected set; }

		/// <summary>
		/// A list of all nodes contained within the tree
		/// </summary>
		public List<TreeNode<T>> AllNodes { get; } = new List<TreeNode<T>>();

		/// <summary>
		/// The horizontal spacing to use between nodes in the tree
		/// </summary>
		public float HorizontalSpacing;

		/// <summary>
		/// The vertical spacing to use between nodes in the tree
		/// </summary>
		public float VerticalSpacing;

		/// <summary>
		/// The maximum depth of this tree
		/// </summary>
		public int MaxDepth => AllNodes.Max(n => n.Depth);

		/// <summary>
		/// Creates a tree with the giving spacing between its nodes
		/// </summary>
		/// <param name="hSpacing">The horizontal spacing to use between nodes</param>
		/// <param name="vSpacing">The vertical spacing to use between nodes</param>
		public Tree(float hSpacing, float vSpacing)
		{
			HorizontalSpacing = hSpacing;
			VerticalSpacing = vSpacing;
		}

		/// <summary>
		/// Sets the root of this tree <para/>
		/// Should only be called if the root has not already been set
		/// </summary>
		/// <param name="data">The value the root of the tree will have</param>
		/// <returns>A reference to the created root node</returns>
		public TreeNode<T> SetRoot(T data)
		{
			if (Root != null)
			{
				throw new Exception("Root is already set!");
			}

			Root = new TreeNode<T>(null, data);
			AllNodes.Add(Root);
			return Root;
		}

		/// <summary>
		/// Adds a node with the given data to the tree as a child of a given parent node
		/// </summary>
		/// <param name="parent">The parent of this new node</param>
		/// <param name="data">The value the new node will contain</param>
		/// <returns>A reference to the newly created node</returns>
		public TreeNode<T> Add(TreeNode<T> parent, T data)
		{
			if (!AllNodes.Contains(parent))
			{
				throw new Exception("Parent node not found in list of all nodes!");
			}

			var newNode = new TreeNode<T>(parent, data);
			AllNodes.Add(newNode);
			return newNode;
		}

		/// <summary>
		/// Gets a list of all leaf nodes within this tree
		/// </summary>
		/// <returns>A list of all nodes within this tree</returns>
		public List<TreeNode<T>> GetLeafNodes()
		{
			return AllNodes.Where(node => node.Children.Count == 0).ToList();
		}

		/// <summary>
		/// Assigns a position to each node in the tree <para/>
		/// Should be used after the tree is generated
		/// </summary>
		public void PositionNodes()
		{
			var positionedLeafNodes = PositionLeafNodes();
			PositionParentNodes(positionedLeafNodes);
		}

		/// <summary>
		/// Position each of the leaf nodes within this tree
		/// </summary>
		/// <returns>A list of leaf nodes that have been positioned</returns>
		private List<TreeNode<T>> PositionLeafNodes()
		{
			var leafNodes = GetLeafNodes();

			TreeNode<T> previousNode = null;

			foreach (var leaf in leafNodes)
			{
				// Initialise position of this leaf node
				var yPos = leaf.Depth * VerticalSpacing;
				var xPos = 0f;

				if (previousNode != null)
				{
					xPos = previousNode.Position.X + HorizontalSpacing;

					// Add additional spacing if this leaf node is not a sibling of the previous node
					if (!leaf.IsSiblingOf(previousNode))
					{
						xPos += HorizontalSpacing * 0.5f;
					}
				}

				// Finally, assign the new leaf node position to its respective IGrammarTreeNodeObject
				var leafPosition = new Vector2D(xPos, yPos);
				leaf.Position = leafPosition;

				// Set previousNode for the next cycles
				previousNode = leaf;
			}

			return leafNodes;
		}

		/// <summary>
		/// Given a list of nodes that have already been positioned, position the next layer of the tree iteratively
		/// </summary>
		/// <param name="nodes">A list of nodes that have already been positioned</param>
		private void PositionParentNodes(List<TreeNode<T>> nodes)
		{
			while (true)
			{
				if (nodes == null || !nodes.Any())
				{
					return;
				}

				// Get a list of parent nodes, removing duplicates
				var parentNodes = nodes.Select(node => node.Parent).Distinct().Where(parent => parent != null).ToList();

				foreach (var node in parentNodes)
				{
					// Initialise Y position of the parent node
					var yPos = node.Depth * VerticalSpacing;

					// Calculate the X position of the parent node based on the average of the positions of its children
					var childrenXPosSum = node.Children.Sum(child => child.Position.X);
					var averageXPosOfChildren = childrenXPosSum / node.Children.Count;
					var xPos = averageXPosOfChildren;

					// Finally, assign the new parent node position to its respective IGrammarTreeNodeObject
					var nodePosition = new Vector2D(xPos, yPos);
					node.Position = nodePosition;
				}

				// Position nodes in the next depth up
				nodes = parentNodes;
			}
		}
	}
}
