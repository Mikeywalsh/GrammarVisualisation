using System.Collections.Generic;
using System.Linq;
using Assets.Code;
using TreeVisualisation.Core;
using TreeVisualisation.Implementations.Grammar;
using UnityEngine;

public class UnityTreeController : MonoBehaviour
{
	public GrammarTree CurrentTree;

    public const float H_SPACING = 7;
    public const float V_SPACING = -10;

    public Dictionary<TreeNode<GrammarData>, GameObject> nodeToObjectMap = new Dictionary<TreeNode<GrammarData>, GameObject>();


    private void Start()
    {
		CurrentTree = new GrammarTree("Assets/sampleGrammar.txt", H_SPACING, V_SPACING);

        // Create the root node as a GameObject and add it to the node To Object map
        CreateNodeObjectForNode(CurrentTree.Root);

        // Create every other node in the tree as GameObjects
        CreateChildrenObjects(CurrentTree.Root);

        // Create a line connection between each node in the tree
        CreateLineConnections();

        var treeBounds = CalculateTreeBounds();
        Camera.main.GetComponent<CameraController>().PositionCamera(treeBounds);
    }

    private Rect CalculateTreeBounds()
    {
        var minX = CurrentTree.AllNodes.Min(n => n.Position.X);
        var maxX = CurrentTree.AllNodes.Max(n => n.Position.X);
        var minY = CurrentTree.AllNodes.Min(n => n.Position.Y);
        var maxY = CurrentTree.AllNodes.Max(n => n.Position.Y);

        //minX -= 50;
        //maxX += 50;
        //minY -= 50;
        //maxY += 50;

        return new Rect(minX, minY, maxX - minX, maxY - minY);
    }

    private void CreateNodeObjectForNode(TreeNode<GrammarData> node)
    {
        GameObject newNodeObject = Instantiate(Resources.Load("Grammar Node"), Vector3.zero, Quaternion.identity) as GameObject;
        newNodeObject.GetComponent<UnityTreeNodeObject>().SetNode(node);
        nodeToObjectMap.Add(node, newNodeObject);
		newNodeObject.GetComponent<UnityTreeNodeObject>().PositionNode();
    }

    private void CreateChildrenObjects(TreeNode<GrammarData> node)
    {
        // Create GameObjects for each child
        foreach (var treeNode in node.Children)
        {
	        var child = treeNode;
	        CreateNodeObjectForNode(child);
            CreateChildrenObjects(child);
        }
    }

	/// <summary>
	/// For each node in the tree, create a line connection between it and its parent, if it has one
	/// </summary>
	public void CreateLineConnections()
    {
        foreach (var node in CurrentTree.AllNodes)
        {
            if (node.Parent != null)
            {
                nodeToObjectMap[node].GetComponent<LineConnection>().SetConnections(nodeToObjectMap[node.Parent]);
            }
        }
    }
}
