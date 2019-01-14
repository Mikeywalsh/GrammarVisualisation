using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TreeController : MonoBehaviour
{
    public GrammarTree Tree = new GrammarTree();

    public const int H_SPACING = 10;
    public const int V_SPACING = 20;

    void Start()
    {
        Tree.ReadFromFile("Assets/sampleGrammar.txt");

        CreateNodeObjects();
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

            // Create an object for each of the nodes at the current depth and assign them a position
            for (int i = 0; i < currentNodes.Count; i++)
            {
                var currentNode = currentNodes[i];

                // Assign a position for this node
                if (currentDepth == maxDepth)
                {
                    if (previousNode != null && !currentNode.IsSiblingOf(previousNode))
                    {
                        currentXPos += H_SPACING;
                    }

                    currentXPos += H_SPACING;
                }
                else
                {
                    float childXPosSum = currentNodes[i].Children.Sum(c => c.XPos);
                    currentXPos = childXPosSum / currentNode.Children.Count;
                }

                currentNode.XPos = currentXPos;
                currentNode.YPos = currentYPos;

                // Assign the previous node
                previousNode = currentNodes[i];

                // Create an object for the node
                GameObject newNodeObject = Instantiate(Resources.Load("Grammar Node"),new Vector3(currentNode.XPos, currentNode.YPos, 0), Quaternion.identity) as GameObject;
                newNodeObject.GetComponent<GrammarTreeNodeObject>().SetNode(currentNode);
            }

            currentDepth--;
            currentYPos += V_SPACING;
        }
    }
}
