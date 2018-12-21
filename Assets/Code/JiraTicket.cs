using System.Collections.Generic;

public class JiraTicket : ITicket
{
    public TicketType TicketType { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public ITicket DependsOn { get; private set; }

    public JiraTicket(TicketType ticketType, string name, string description, ITicket dependsOn)
    {
        TicketType = ticketType;
        Name = name;
        Description = description;
        DependsOn = dependsOn;
    }
}
