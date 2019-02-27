namespace GrammarTree.Implementations.Grammar
{
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

		public override bool Equals(object obj)
		{
			return obj is GrammarData gData &&
			       gData.NodeType == NodeType &&
			       gData.NodeText == NodeText;
		}

		public override int GetHashCode()
		{
			var charSum = 0;

			foreach (var character in NodeText)
			{
				charSum += character;
			}

			return (int)NodeType ^ charSum;
		}

		public static bool operator ==(GrammarData a, GrammarData b)
		{
			return a.Equals(b);
		}

		public static bool operator !=(GrammarData a, GrammarData b)
		{
			return !(a == b);
		}
	}
}
