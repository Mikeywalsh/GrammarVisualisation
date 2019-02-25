public struct GrammarData
{
	public readonly GrammarNodeType NodeType;
	public readonly string NodeText;

	public GrammarData(GrammarNodeType nodeType, string nodeText)
	{
		NodeType = nodeType;
		NodeText = nodeText;
	}
}
