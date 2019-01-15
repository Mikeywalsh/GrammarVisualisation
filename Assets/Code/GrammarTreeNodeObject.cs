using UnityEngine;
using TMPro;

public class GrammarTreeNodeObject : MonoBehaviour
{
    GrammarTreeNode Node;

    public string Contents => Node.Text;

    GrammarNodeType NodeType => Node.NodeType;

    public TextMeshPro TypeText;

    public TextMeshPro ContentsText;

    public void SetNode(GrammarTreeNode node)
    {
        Node = node;
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        TypeText.text = NodeType.ToString();
        ContentsText.text = Contents;
    }
}
