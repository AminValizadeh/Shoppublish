using Microsoft.EntityFrameworkCore;
using Shop.Data.Context;
using Shop.Domain.Entities.Ticket;
using Shop.Domain.Interfaces;

namespace Shop.Data.Repository
{
    public class TicketMessageRepository : ITicketMessageRepository
    {

        #region constructor

        private readonly ShopContext _context;

        public TicketMessageRepository(ShopContext context)
        {
            _context = context;
        }

        #endregion


        public async Task AddTicketMessageAsync(TicketMessage ticketMessage)
        {
            ticketMessage.CreateDate = DateTime.Now;
            await _context.TicketMessages.AddAsync(ticketMessage);
            await SaveChangeAsync();
        }

        public async Task<List<TicketMessage>> GetListTicketMessagesByTicketId(int ticketId)
        {
            return await _context.TicketMessages
                .Include(s=> s.Sender)
                .OrderByDescending(s => s.CreateDate)
                .Where(s => s.TicketId == ticketId && !s.IsDelete).ToListAsync();
        }

        public async Task SaveChangeAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
