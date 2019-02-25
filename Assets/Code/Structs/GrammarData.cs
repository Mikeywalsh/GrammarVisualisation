/// <summary>
/// Data which can be used to describe a grammar node
/// </summary>
public struct GrammarData
{
	/// <summary>
	/// The type of grammar node this describes
	/// </summary>
	public readonly GrammarNodeType NodeType;

	/// <summary>
	/// The text contained within the described node
	/// </summary>
	public readonly string NodeText;

	/// <summary>
	/// Creates a data structure which holds information used to describe a grammar node
	/// </summary>
	/// <param name="nodeType">The type of grammar node this describes</param>
	/// <param name="nodeText">The text contained within the described node</param>
	public GrammarData(GrammarNodeType nodeType, string nodeText)
	{
		NodeType = nodeType;
		NodeText = nodeText;
	}
}
