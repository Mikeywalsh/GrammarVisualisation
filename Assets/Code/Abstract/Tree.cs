using System.Collections.Generic;
using System.Linq;

/// <summary>
/// This class represents a tree, used to store data in a hierarchical manner
/// </summary>
public abstract class Tree<T>
{
	public float HorizontalSpacing = 7;
	public float VerticalSpacing = -10;

    /// <summary>
    /// The root node of the tree
    /// </summary>
    public TreeNode<T> Root { get; protected set; }

    /// <summary>
    /// A list of all nodes contained within the tree
    /// </summary>
    public List<TreeNode<T>> AllNodes { get; protected set; } = new List<TreeNode<T>>();

    /// <summary>
    /// The maximum depth of this tree
    /// </summary>
    public int MaxDepth => AllNodes.Max(n => n.Depth);

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
    protected void PositionNodes()
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
			    // Initialise position of of the parent node
			    var yPos = node.Depth * VerticalSpacing;
			    var xPos = 0f;

			    // Calculate the X position of the parent node based on the average of the positions of its children
			    var childrenXPosSum = node.Children.Sum(child => child.Position.X);
			    var averageXPosOfChildren = childrenXPosSum / node.Children.Count;
			    xPos = averageXPosOfChildren;

			    // Finally, assign the new parent node position to its respective IGrammarTreeNodeObject
			    var nodePosition = new Vector2D(xPos, yPos);
			    node.Position = nodePosition;
		    }

		    // Position nodes in the next depth up
		    nodes = parentNodes;
	    }
    }
}
