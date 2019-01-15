using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

public class TreeController : MonoBehaviour
{
    public GrammarTree Tree = new GrammarTree();

    public const int H_SPACING = 10;
    public const int V_SPACING = 20;

    public Dictionary<GrammarTreeNode, GameObject> nodeToObjectMap = new Dictionary<GrammarTreeNode, GameObject>();

    void Start()
    {
        Tree.ReadFromFile("Assets/sampleGrammar.txt");

        CreateNodeObjects();
        CreateLineConnections();
    }

    private void CreateNodeObjects()
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
            var currentNodes = Tree.AllNodes.FindAll(x => x.Depth == currentDepth);
            GrammarTreeNode previousNode = null;

            var nodesWithChildren = currentNodes.Where(n => n.Children.Count > 0);
            var nodesWithoutChildren = currentNodes.Where(n => n.Children.Count == 0);

            nodesWithChildren = nodesWithChildren.OrderBy(n => n.Parent);

            var orderedNodes = nodesWithChildren.Concat(nodesWithoutChildren).ToList();


            Debug.Log("DEPTH: " + currentDepth + "      " + orderedNodes.Count);

            // Create an object for each of the nodes at the current depth and assign them a position
            for (int i = 0; i < orderedNodes.Count; i++)
            {
                var currentNode = orderedNodes[i];

                // Assign a position for this node
                if (currentNode.Children.Count == 0)
                {
                    if (previousNode != null && !currentNode.IsSiblingOf(previousNode))
                    {
                        currentXPos += H_SPACING;
                    }

                    currentXPos += H_SPACING;

                    Debug.Log(currentNodes.Count + " " + currentXPos);
                }
                else
                {
                    float childXPosSum = currentNodes[i].Children.Sum(c => c.XPos);
                    currentXPos = childXPosSum / currentNode.Children.Count == 0 ? 1 : currentNode.Children.Count;
                }

                currentNode.XPos = currentXPos;
                currentNode.YPos = currentYPos;

                // Assign the previous node
                previousNode = currentNodes[i];

                // Create an object for the node
                GameObject newNodeObject = Instantiate(Resources.Load("Grammar Node"), new Vector3(currentNode.XPos, currentNode.YPos, 0), Quaternion.identity) as GameObject;
                newNodeObject.GetComponent<GrammarTreeNodeObject>().SetNode(currentNode);

                // Create a mapping between the current node and the new GameObject created
                nodeToObjectMap.Add(currentNode, newNodeObject);
            }

            currentDepth--;
            currentYPos += V_SPACING;
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
