using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TreeController : MonoBehaviour
{
    public GrammarTree Tree = new GrammarTree();

    public const int VISUALISATION_WIDTH = 40;
    public const int VISUALISATION_HEIGHT = 25; 

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

        while (currentDepth >= 0)
        {
            // Get all nodes at the current depth
            var currentNodes = Tree.AllNodes.FindAll(x => x.Depth == currentDepth);

            // Create an object for each of the nodes at the current depth and assign them a position
            for (int i = 0; i < currentNodes.Count; i++)
            {
                // Create an object for the node
                GrammarTreeNodeObject newNodeObject = Instantiate(Resources.Load("Grammar Node"), Vector3.zero, Quaternion.identity) as GrammarTreeNodeObject;
                newNodeObject.SetNode(currentNodes[i]);

                // Assign the new node object a position
                Vector3 newNodePosition = Vector3.zero;
                newNodePosition.x = ((VISUALISATION_WIDTH / currentNodes.Count) * i) + (VISUALISATION_WIDTH / (currentNodes.Count * 2));
                newNodePosition.y = -(VISUALISATION_HEIGHT / maxDepth) * currentDepth + VISUALISATION_HEIGHT;
                newNodeObject.transform.position = newNodePosition;
            }

            currentDepth--;
        }
    }
}
