using Shop.Domain.Entities.Ticket;

namespace Shop.Domain.ViewModels.Ticket
{
    public class TicketDetailViewModel
    {
        public Shop.Domain.Entities.Ticket.Ticket Ticket { get; set; }
        public List<TicketMessage> TicketMessages { get; set; }
    }
}
