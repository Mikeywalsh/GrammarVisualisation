using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class TreeNode : MonoBehaviour
{
    public TreeNode Parent;
    public List<TreeNode> Children = new List<TreeNode>();

    public ITicket Ticket;

    public int Depth;

    public TextMeshPro TypeLabel;
    public TextMeshPro NameLabel;
    public TextMeshPro DescriptionLabel;

    public void AddChild(TreeNode node)
    {
        // Correctly set up the hierarchical relationship between parent and child
        Children.Add(node);
        node.Parent = this;

        // Create a line between the parent and child in the scene
        node.gameObject.AddComponent<LineConnection>().SetConnections(gameObject);
    }

    public void SetTicket(ITicket ticket)
    {
        // Set text on the ticket object itself
        Ticket = ticket;
        TypeLabel.text = ticket.TicketType.ToString();
        NameLabel.text = ticket.Name;
        DescriptionLabel.text = ticket.Description;

        // Make tickets easier to track in the editor
        transform.name = ticket.TicketType.ToString() + ": " + ticket.Name;
    }

    public int SiblingCount => Parent != null ? Parent.Children.Count : 0;
}
