public interface ITreeNodeObject<T>
{
	TreeNode<T> Node { get; }

    void SetNode(TreeNode<T> node);

    void UpdateDisplay();

    void PositionNode();
}
