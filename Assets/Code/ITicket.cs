using System.Collections.Generic;

public interface ITicket
{
    TicketType TicketType { get; }
    string Name { get; }
    string Description { get; }

    ITicket DependsOn { get; }
}
