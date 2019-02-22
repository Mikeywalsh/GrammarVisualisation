using UnityEngine;
using TMPro;

namespace Assets.Code
{
    public class UnityGrammarTreeNodeObject : MonoBehaviour, IGrammarTreeNodeObject
    {
        public TextMeshPro TypeText;

        public TextMeshPro ContentsText;

        public GrammarTreeNode Node { get; private set; }

        public string Contents()
        {
            return Node.Text;
        }

        public GrammarNodeType NodeType()
        {
            return Node.NodeType;
        }

        public void SetNode(GrammarTreeNode node)
        {
            Node = node;
            UpdateDisplay();
        }

        void IGrammarTreeNodeObject.UpdateDisplay()
        {
            UpdateDisplay();
        }

        private void UpdateDisplay()
        {
            TypeText.text = NodeType().ToString();
            ContentsText.text = Contents();
        }
    }
}
