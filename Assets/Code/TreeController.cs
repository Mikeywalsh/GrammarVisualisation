using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TreeController : MonoBehaviour
{
    public List<TreeNode> TreeNodes;

    public const int VISUALISATION_WIDTH = 40;
    public const int VISUALISATION_HEIGHT = 25; 

    void Start()
    {
        #region Create Fake Tickets
        ITicket cap1 = new JiraTicket(TicketType.Capability, "Do huge stuff", "HUGE STUFF", null);
        ITicket epic1 = new JiraTicket(TicketType.Epic, "Do big stuff", "BIG STUFF", cap1);
        ITicket story1 = new JiraTicket(TicketType.Story, "Do medium stuff", "Descriptions are hard", epic1);
        ITicket story2 = new JiraTicket(TicketType.Story, "Other medium stuff", "Blah blah blah", epic1);
        ITicket task1 = new JiraTicket(TicketType.Task, "Some task", "Lots of work", story1);
        ITicket task2 = new JiraTicket(TicketType.Task, "Another task", "Existence is pain", story1);
        ITicket task3 = new JiraTicket(TicketType.Task, "Oh boy more tasks", "I am uncreative", story2);
        ITicket task4 = new JiraTicket(TicketType.Task, "Help", "foo foo foo foo", story2);
        ITicket task5 = new JiraTicket(TicketType.Task, "Last task", "baa baa baa baa", story2);
        #endregion

        // Populate tickets list and initialise list of nodes
        List<ITicket> allTickets = new List<ITicket>() { cap1, epic1, story1, story2, task1, task2, task3, task4, task5 };
        TreeNodes = new List<TreeNode>();

        // Process all tickets, creating a TreeNode for each ticket
        // TODO It's assumed the tickets being read are in a valid tree structure with no cycles, check for this in future
        var root = allTickets.Find(x => x.DependsOn == null);
        ProcessTicket(allTickets, root);

        // Ensure there are no leftover tickets after processing
        if (allTickets.Count != 0)
        {
            throw new Exception("Ticket count should be 0 after processing, instead it is: " + allTickets.Count);
        }

        AssignPositionToAllNodes();
    }

    private void AssignPositionToAllNodes()
    {
        // Obtain the maximum depth of the tree
        int maxDepth = TreeNodes.Max(x => x.Depth);
        int currentDepth = maxDepth;

        while (currentDepth >= 0)
        {
            // Get all nodes at the current depth
            var currentNodes = TreeNodes.FindAll(x => x.Depth == currentDepth);

            // Assign a position to each of the nodes at the current depth
            for (int i = 0; i < currentNodes.Count; i++)
            {
                Vector3 newNodePosition = Vector3.zero;
                newNodePosition.x = ((VISUALISATION_WIDTH / currentNodes.Count) * i) + (VISUALISATION_WIDTH / (currentNodes.Count * 2));
                newNodePosition.y = -(VISUALISATION_HEIGHT / maxDepth) * currentDepth + VISUALISATION_HEIGHT;
                currentNodes[i].transform.position = newNodePosition;
            }

            currentDepth--;
        }
    }

    /// <summary>
    /// Given a list of all tickets and a specific ticket to process, this method will consume the
    /// ticket and generate a Tree of it and any tickets that depend on it, recursively
    /// </summary>
    /// <param name="allTickets">A list of all tickets</param>
    /// <param name="currentTicket">A specific ticket to process the hierarchy of</param>
    /// <param name="depth">The current recursion depth</param>
    /// <returns>The root node of the resultant tree created from the processed tickets</returns>
    private TreeNode ProcessTicket(List<ITicket> allTickets, ITicket currentTicket, int depth = 0)
    {
        var newNode = CreateNode();
        newNode.SetTicket(currentTicket);

        var dependencies = allTickets.FindAll(x => x.DependsOn == currentTicket);

        foreach (var dependency in dependencies)
        {
            var childNode = ProcessTicket(allTickets, dependency, depth + 1);
            childNode.Depth = depth + 1;
            newNode.AddChild(childNode);
        }

        allTickets.Remove(currentTicket);
        TreeNodes.Add(newNode);
        return newNode;
    }

    private TreeNode CreateNode()
    {
        var newNode = Instantiate(Resources.Load("Ticket Node"), Vector3.zero, Quaternion.identity) as GameObject;
        return newNode.GetComponent<TreeNode>();
    }
}
