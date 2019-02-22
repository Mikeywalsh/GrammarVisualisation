using System.Collections.Generic;
using System.Linq;

public abstract class TreeController
{
    public GrammarTree Tree = new GrammarTree();

    public const float H_SPACING = 1;
    public const float V_SPACING = -2;

    public Dictionary<GrammarTreeNode, IGrammarTreeNodeObject> nodeToObjectMap = new Dictionary<GrammarTreeNode, IGrammarTreeNodeObject>();

    private void Start()
    {
        Tree.ReadFromFile("Assets/sampleGrammar.txt");

        // Create the root node as a GameObject and add it to the node To Object map
        CreateNodeObjectForNode(Tree.Root);

        // Create every other node in the tree as GameObjects
        CreateChildrenObjects(Tree.Root);

        // Position every node in the tree, starting from the leaf nodes
        var positionedLeafNodes = PositionLeafNodes(Tree);
        PositionParentNodes(positionedLeafNodes);

        // Create a line connection between each node in the tree
        CreateLineConnections();
    }

    protected abstract void CreateNodeObjectForNode(GrammarTreeNode node);

    private void CreateChildrenObjects(GrammarTreeNode node)
    {
        // Create GameObjects for each child
        foreach (var child in node.Children)
        {
            CreateNodeObjectForNode(child);
            CreateChildrenObjects(child);
        }
    }

    /// <summary>
    /// Given a <see cref="GrammarTree"/>, position each of its leaf nodes
    /// </summary>
    /// <param name="tree">The tree to position the leaf nodes of</param>
    /// <returns>A list of leaf nodes that have been positioned</returns>
    private List<GrammarTreeNode> PositionLeafNodes(GrammarTree tree)
    {
        var leafNodes = tree.GetLeafNodes();

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

            // Finally, assign the new leaf node position to its respective IGrammarTreeNodeObject
            nodeToObjectMap[leaf].PositionNode(leaf.XPos, leaf.YPos);

            // Set previousNode for the next cycles
            previousNode = leaf;
        }

        return leafNodes;
    }

    /// <summary>
    /// Given a list of nodes that have already been positioned, position the next layer of the tree iteratively
    /// </summary>
    /// <param name="nodes">A list of nodes that have already been positioned</param>
    private void PositionParentNodes(List<GrammarTreeNode> nodes)
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

                // Finally, assign the new parent node position to its respective IGrammarTreeNodeObject
                nodeToObjectMap[node].PositionNode(node.XPos, node.YPos);
            }

            // Position nodes in the next depth up
            nodes = parentNodes;
        }
    }

    /// <summary>
    /// For each node in the tree, create a line connection between it and its parent, if it has one
    /// </summary>
    protected abstract void CreateLineConnections();
}
