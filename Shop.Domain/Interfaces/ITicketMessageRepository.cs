using Shop.Domain.Entities.Ticket;

namespace Shop.Domain.Interfaces
{
    public interface ITicketMessageRepository
    {
        Task AddTicketMessageAsync(TicketMessage ticket);
        Task<List<TicketMessage>> GetListTicketMessagesByTicketId(int ticketId);
        Task SaveChangeAsync();
    }
}
