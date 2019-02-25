/// <summary>
/// This interface represents an object used for displaying information
/// about a node to the screen, and can be implemented in any graphical
/// engine/framework
/// </summary>
/// <typeparam name="T">The type of data used by the node being represented</typeparam>
public interface ITreeNodeObject<T>
{
	/// <summary>
	/// The tree node that this node object represents
	/// </summary>
	TreeNode<T> Node { get; }

	/// <summary>
	/// Sets the tree node that this node object represents
	/// </summary>
	/// <param name="node">The node to set</param>
    void SetNode(TreeNode<T> node);

	/// <summary>
	/// Updates the display with correct information about this current node
	/// </summary>
    void UpdateDisplay();

	/// <summary>
	/// Positions this node object in world space according to the position
	/// of the node that it represents
	/// </summary>
    void PositionNode();
}
