using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class GrammarTreeNodeObject : MonoBehaviour
{
    GrammarTreeNode Node;

    public string Text => Node.Text;

    GrammarNodeType NodeType => Node.NodeType;

    public void SetNode(GrammarTreeNode node)
    {
        Node = node;
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {

    }
}
