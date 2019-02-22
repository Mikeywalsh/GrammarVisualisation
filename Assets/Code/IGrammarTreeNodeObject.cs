public interface IGrammarTreeNodeObject
{
    GrammarTreeNode Node { get; }

    string Contents();

    GrammarNodeType NodeType();

    void SetNode(GrammarTreeNode node);

    void UpdateDisplay();
}
