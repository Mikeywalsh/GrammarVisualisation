﻿using System.Collections.Generic;

/// <summary>
/// A node that is part of a grammar tree <para/>
/// Each node has a parent and a list of children
/// </summary>
public class GrammarTreeNode
{
    public GrammarTreeNode Parent;

    public List<GrammarTreeNode> Children = new List<GrammarTreeNode>();

    public string Text;

    public GrammarNodeType NodeType;

    /// <summary>
    /// The depth in the tree of this node
    /// </summary>
    public int Depth;

    public GrammarTreeNode(GrammarNodeType nodeType, string text) : this(null, nodeType, text) { }

    public GrammarTreeNode(GrammarTreeNode parent, GrammarNodeType nodeType, string text)
    {
        if (parent != null)
        {
            Parent = parent;
            Parent.AddChild(this);
        }

        Depth = parent != null ? parent.Depth + 1 : 0;
        Text = text;
        NodeType = nodeType;
    }

    /// <summary>
    /// Adds a node as a child of this node
    /// </summary>
    /// <param name="node">The node to add as a child of this node</param>
    public void AddChild(GrammarTreeNode node)
    {
        // Correctly set up the hierarchical relationship between parent and child
        Children.Add(node);
    }

    /// <summary>
    /// The number of sibling tree nodes that this <see cref="GrammarTreeNode"/> has
    /// </summary>
    public int SiblingCount => Parent != null ? Parent.Children.Count - 1 : 0;
}