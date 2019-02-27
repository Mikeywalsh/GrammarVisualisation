using TreeVisualisation.Core;
using TreeVisualisation.Implementations.Grammar;
using UnityEngine;
using TMPro;

namespace Assets.Code
{
    public class UnityTreeNodeObject : MonoBehaviour, ITreeNodeObject<GrammarData>
	{
        public TextMeshPro TypeText;

        public TextMeshPro ContentsText;

        public TreeNode<GrammarData> Node { get; private set; }

        public string Contents()
        {
	        return Node.Data.NodeText;
        }

        public GrammarNodeType NodeType()
        {
	        return Node.Data.NodeType;
        }

        public void SetNode(TreeNode<GrammarData> node)
        {
            Node = node;
            UpdateDisplay();
        }

        public void PositionNode()
        {
            transform.position = new Vector3(Node.Position.X,
											 Node.Position.Y,
											 0);
        }

        public void UpdateDisplay()
        {
            TypeText.text = NodeType().ToString();
            ContentsText.text = Contents();
        }
    }
}
