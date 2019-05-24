using System.Collections.Generic;
using System.Numerics;

namespace TreeVisualisation.Core
{
	/// <summary>
	/// A node that is part of a tree <para/>
	/// Each node has a parent a list of children and a position
	/// </summary>
	public class TreeNode<T> : ITreeNode
	{
		/// <summary>
		/// The parent node of this node
		/// </summary>
		public TreeNode<T> Parent { get; }

		/// <summary>
		/// A list of child nodes this node has, if any
		/// </summary>
		public List<TreeNode<T>> Children { get; } = new List<TreeNode<T>>();

		/// <summary>
		/// The position of this node when the tree has been positioned
		/// </summary>
		public Vector2 Position { get; set; }

		/// <summary>
		/// The data contained within this node
		/// </summary>
		public T Data;

		/// <summary>
		/// The depth in the tree of this node
		/// </summary>
		public int Depth { get; }

		public TreeNode(T data) : this(null, data) { }

		public TreeNode(TreeNode<T> parent, T data)
		{
			if (parent != null)
			{
				Parent = parent;
				Parent.AddChild(this);
			}

			Depth = parent?.Depth + 1 ?? 0;
			Data = data;
		}

		/// <summary>
		/// Adds a node as a child of this node
		/// </summary>
		/// <param name="node">The node to add as a child of this node</param>
		private void AddChild(TreeNode<T> node)
		{
			// Correctly set up the hierarchical relationship between parent and child
			Children.Add(node);
		}

		/// <summary>
		/// The number of sibling tree nodes that this <see cref="TreeNode{T}"/> has
		/// </summary>
		public int SiblingCount() => Parent?.Children.Count - 1 ?? 0;

		/// <summary>
		/// Checks if this node is a sibling of a provided node and returns a flag signalling this
		/// </summary>
		/// <param name="node">The node to check the sibling relationship of</param>
		/// <returns>A flag indicating if this node is a sibling of the provided node</returns>
		public bool IsSiblingOf(TreeNode<T> node)
		{
			if (Parent == null || node.Parent == null)
			{
				return false;
			}

			return Parent == node.Parent;
		}
	}
}
