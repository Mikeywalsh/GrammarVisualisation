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

	private Dictionary<int, List<GrammarTreeNode>> depthMap = new Dictionary<int, List<GrammarTreeNode>>();

	void Start()
	{
		Tree.ReadFromFile("Assets/sampleGrammar.txt");

		CreateNodeObjects();
		AssignNodePositions();
		CreateLineConnections();
	}

	private void CreateNodeObjects()
	{
		// Obtain the maximum depth of the tree
		int maxDepth = Tree.MaxDepth;
		int currentDepth = 0;

		var visitQueue = new Queue<GrammarTreeNode>();
		visitQueue.Enqueue(Tree.Root);

		while (visitQueue.Any())
		{
			var currentNode = visitQueue.Dequeue();

			if (!nodeToObjectMap.ContainsKey(currentNode))
			{
				// Create an object for the node
				GameObject newNodeObject = Instantiate(Resources.Load("Grammar Node"), new Vector3(currentNode.XPos, currentNode.YPos, 0), Quaternion.identity) as GameObject;
				newNodeObject.GetComponent<GrammarTreeNodeObject>().SetNode(currentNode);

				// Create a mapping between the current node and the new GameObject created
				nodeToObjectMap.Add(currentNode, newNodeObject);

				if (!depthMap.ContainsKey(currentNode.Depth))
				{
					depthMap.Add(currentNode.Depth, new List<GrammarTreeNode>());
				}

				depthMap[currentNode.Depth].Add(currentNode);

				foreach (var child in currentNode.Children)
				{
					if (!nodeToObjectMap.ContainsKey(child))
					{
						visitQueue.Enqueue(child);
					}
				}
			}

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
