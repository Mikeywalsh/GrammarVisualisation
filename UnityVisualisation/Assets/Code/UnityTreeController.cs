using System.Collections.Generic;
using System.Linq;
using Assets.Code;
using TreeVisualisation.Core;
using TreeVisualisation.Implementations.Grammar;
using UnityEngine;

public class UnityTreeController : MonoBehaviour
{
    public GrammarTree CurrentTree;

    public const float H_SPACING = 1;
    public const float V_SPACING = 6;

    public const float NODE_WIDTH = 7;
    public const float NODE_HEIGHT = 3;

    public Dictionary<TreeNode<GrammarData>, GameObject> nodeToObjectMap = new Dictionary<TreeNode<GrammarData>, GameObject>();

    private void Start()
    {
        CurrentTree = new GrammarTree("Assets/sampleGrammar.txt", new Vector2D(H_SPACING, V_SPACING), new Vector2D(NODE_WIDTH, NODE_HEIGHT));

        // Create the root node as a GameObject and add it to the node To Object map
        CreateNodeObjectForNode(CurrentTree.Root);

        // Create every other node in the tree as GameObjects
        CreateChildrenObjects(CurrentTree.Root);

        // Create a line connection between each node in the tree
        CreateLineConnections();

        // Set the starting camera position
        var treeBounds = CalculateTreeBounds();
        Camera.main.GetComponent<CameraController>().PositionCamera(treeBounds);

        // Initialise the UI
        UIController.Controller.InitialiseUI();

        // Hide all Node Objects
        foreach (var nodeObject in nodeToObjectMap.Values)
        {
            nodeObject.SetActive(false);
        }

        SpawnNextLayer();
    }

    private int currentSpawningDepth = 0;
    private int nodesThisDepth;
    private int spawnsThisDepth;

    public void SpawnNextLayer()
    {
        // Get all gameobjects that represent treenodes at the next layer
        var treeNodesToSpawn = nodeToObjectMap.Keys.Where(n => n.Depth == currentSpawningDepth).ToList();

        nodesThisDepth = treeNodesToSpawn.Count;
        spawnsThisDepth = 0;

        foreach (var node in treeNodesToSpawn)
        {
            var nodeObject = nodeToObjectMap[node];
            nodeObject.SetActive(true);
            if (node.Parent != null)
            {
                nodeObject.transform.position = new Vector3(node.Parent.Position.X, node.Parent.Position.Y, 0);
            }

            iTween.MoveTo(nodeObject, iTween.Hash("x", node.Position.X, "y", node.Position.Y, "time", 0.25f, "onCompleteTarget", gameObject, "onComplete", "SpawnComplete"));
        }
    }

    public void SpawnComplete()
    {
        spawnsThisDepth++;

        if (nodesThisDepth == spawnsThisDepth)
        {
            SpawnNextLayer();
            currentSpawningDepth++;
        }
    }

    private Rect CalculateTreeBounds()
    {
        var minX = CurrentTree.AllNodes.Min(n => n.Position.X);
        var maxX = CurrentTree.AllNodes.Max(n => n.Position.X);
        var minY = CurrentTree.AllNodes.Min(n => n.Position.Y);
        var maxY = CurrentTree.AllNodes.Max(n => n.Position.Y);

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
