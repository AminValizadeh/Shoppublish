using Shop.Domain.Entities.Ticket;
using Shop.Domain.ViewModels.Ticket;

namespace Shop.Domain.Interfaces
{
    public interface ITicketRepository
    {
        Task AddTicketAsync(Ticket ticket);
        Task<Ticket> GetTicketByTicketIdAsync(int ticketId);
        Task<FilterTicketViewModel> FilterTickets(FilterTicketViewModel filter);
        Task UpdateTicket(Ticket ticket);
        Task SaveChangeAsync();
    }
}
