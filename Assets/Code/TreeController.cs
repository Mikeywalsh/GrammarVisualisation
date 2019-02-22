using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

public class TreeController : MonoBehaviour
{
    public GrammarTree Tree = new GrammarTree();

    public const float H_SPACING = 10;
    public const float V_SPACING = -20;

    public Dictionary<GrammarTreeNode, GameObject> nodeToObjectMap = new Dictionary<GrammarTreeNode, GameObject>();

    private Dictionary<int, List<GrammarTreeNode>> depthMap = new Dictionary<int, List<GrammarTreeNode>>();

    void Start()
    {
        Tree.ReadFromFile("Assets/sampleGrammar.txt");

        // Create the root node as a GameObject and add it to the node To Object map
        CreateNodeObjectForNode(Tree.Root);

        // Create every other node in the tree as GameObjects
        CreateChildrenObjects(Tree.Root);

        // Position every node in the tree, starting from the leaf nodes
        var leafNodes = Tree.GetLeafNodes();
        PositionLeafNodes(leafNodes);
        PositionTreeFromLeafNodes(leafNodes);

        CreateLineConnections();
    }

    private void CreateNodeObjectForNode(GrammarTreeNode node)
    {
        GameObject newNodeObject = Instantiate(Resources.Load("Grammar Node"), Vector3.zero, Quaternion.identity) as GameObject;
        newNodeObject.GetComponent<GrammarTreeNodeObject>().SetNode(node);
        nodeToObjectMap.Add(node, newNodeObject);
    }

    private void CreateChildrenObjects(GrammarTreeNode node)
    {
        // Create GameObjects for each child
        foreach (var child in node.Children)
        {
            CreateNodeObjectForNode(child);
        }

        // Generate the positions of each child
        GrammarTreeNode lastNodePlaced = null;
        for (int i = 0; i < node.Children.Count; i++)
        {
            GrammarTreeNode currentChild = node.Children[i];

            currentChild.XPos = lastNodePlaced?.XPos + H_SPACING ?? node.XPos;

            if (i > 0)
            {
                for (int j = i; j >= 0; j--)
                {
                    node.Children[j].XPos -= H_SPACING / 2;
                }
            }

            currentChild.YPos = node.YPos - V_SPACING;
            lastNodePlaced = currentChild;


        }

        foreach (var child in node.Children)
        {
            CreateChildrenObjects(child);
        }
    }

    private void PositionLeafNodes(IEnumerable<GrammarTreeNode> leafNodes)
    {
        GrammarTreeNode previousNode = null;

        foreach (var leaf in leafNodes)
        {
            // Assign vertical position of this leaf node
            leaf.YPos = leaf.Depth * V_SPACING;

            if (previousNode != null)
            {
                leaf.XPos = previousNode.XPos + H_SPACING;

                // Add additional spacing if this leaf node is not a sibling of the previous node
                if (!leaf.IsSiblingOf(previousNode))
                {
                    leaf.XPos += H_SPACING;
                }
            }

            // Finally, assign the new leaf node position to its respective GrammarTreeNodeObject
            var nodeObjectPosition = new Vector3(leaf.XPos, leaf.YPos, 0);
            nodeToObjectMap[leaf].transform.position = nodeObjectPosition;

            // Set previousNode for the next cycles
            previousNode = leaf;
        }
    }

    private void PositionTreeFromLeafNodes(List<GrammarTreeNode> nodes)
    {
        while (true)
        {
            if (nodes == null || !nodes.Any())
            {
                return;
            }

            // Get a list of parent nodes, removing duplicates
            var parentNodes = nodes.Select(node => node.Parent).Distinct().Where(parent => parent != null).ToList();
            
            foreach (var node in parentNodes)
            {
                // Assign the Y position of the parent node
                node.YPos = node.Depth * V_SPACING;

                // Calculate the X position of the parent node based on the average of the positions of its children
                var childrenXPosSum = node.Children.Sum(child => child.XPos);
                var averageXPosOfChildren = childrenXPosSum / node.Children.Count;
                node.XPos = averageXPosOfChildren;

                // Finally, assign the new parent node position to its respective GrammarTreeNodeObject
                var nodeObjectPosition = new Vector3(node.XPos, node.YPos, 0);
                nodeToObjectMap[node].transform.position = nodeObjectPosition;
            }

            // Position nodes in the next depth up
            nodes = parentNodes;
        }
    }

    private void AssignNodePositions()
    {
        // Obtain the maximum depth of the tree
        int maxDepth = Tree.MaxDepth;
        int currentDepth = maxDepth;

        // Initialise position tracking variables
        float currentXPos = 0;
        float currentYPos = 0;

        while (currentDepth >= 0)
        {
            // Get all nodes at the current depth
            var currentNodes = depthMap[currentDepth];
            GrammarTreeNode previousNode = null;

            // Create an object for each of the nodes at the current depth and assign them a position
            for (int i = 0; i < currentNodes.Count; i++)
            {
                var currentNode = currentNodes[i];

                // Space siblings out
                if (previousNode != null && !currentNode.IsSiblingOf(previousNode))
                {
                    currentXPos += H_SPACING;
                }

                // Assign a position for this node
                if (currentNode.Children.Count == 0)
                {
                    currentXPos += H_SPACING;
                }
                else
                {
                    float childXPosSum = currentNodes[i].Children.Sum(c => c.XPos);
                    var childXPosMean = childXPosSum / (currentNode.Children.Count == 0 ? 1 : currentNode.Children.Count);

                    currentXPos = Math.Max(childXPosMean, currentXPos + H_SPACING);
                }

                currentNode.XPos = currentXPos;
                currentNode.YPos = currentYPos;

                nodeToObjectMap[currentNode].transform.position = new Vector3(currentXPos, currentYPos, 0);

                // Assign the previous node
                previousNode = currentNodes[i];
            }

            currentDepth--;
            currentYPos += V_SPACING;
            currentXPos = 0;
        }
    }

    public void CreateLineConnections()
    {
        foreach (var node in Tree.AllNodes)
        {
            if (node.Parent != null)
            {
                nodeToObjectMap[node].GetComponent<LineConnection>().SetConnections(nodeToObjectMap[node.Parent]);
            }
        }
    }
}
