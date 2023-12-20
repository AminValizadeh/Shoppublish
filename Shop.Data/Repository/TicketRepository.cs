using Microsoft.EntityFrameworkCore;
using Shop.Data.Context;
using Shop.Domain.Entities.Ticket;
using Shop.Domain.Interfaces;
using Shop.Domain.ViewModels.Paging;
using Shop.Domain.ViewModels.Ticket;

namespace Shop.Data.Repository
{
    public class TicketRepository : ITicketRepository
    {
        #region constructor

        private readonly ShopContext _context;

        public TicketRepository(ShopContext context)
        {
            _context = context;
        }

        #endregion

        public async Task AddTicketAsync(Ticket ticket)
        {
            ticket.CreateDate = DateTime.Now;
            await _context.Tickets.AddAsync(ticket);
            await SaveChangeAsync();
        }

        public async Task<FilterTicketViewModel> FilterTickets(FilterTicketViewModel filter)
        {
            var query = _context.Tickets.AsQueryable();

            #region state

            switch (filter.FilterTicketState)
            {
                case FilterTicketState.All:
                    break;
                case FilterTicketState.NotDeleted:
                    query = query.Where(s => !s.IsDelete);
                    break;
                case FilterTicketState.Deleted:
                    query = query.Where(s => s.IsDelete);
                    break;
                default: break;
            }

            switch (filter.OrderBy)
            {
                case FilterTicketOrder.CreateDat_ASC:
                    query = query.OrderBy(s => s.CreateDate);
                    break;
                case FilterTicketOrder.CreateDate_DES:
                    query = query.OrderByDescending(s => s.CreateDate);
                    break;
                default:
                    break;
            }

            #endregion

            #region filter

            if (!string.IsNullOrEmpty(filter.Title))
                query = query.Where(s => EF.Functions.Like(s.Title, $"%{filter.Title}%"));

            if (filter.TicketPriority != null)
                query = query.Where(s => s.TicketPriority == filter.TicketPriority.Value);

            if (filter.TicketState != null)
                query = query.Where(s => s.TicketState == filter.TicketState.Value);

            if (filter.TicketSection != null)
                query = query.Where(s => s.TicketSection == filter.TicketSection.Value);

            if (filter.UserId != null && filter.UserId != 0)
                query = query.Where(s => s.OwnerId == filter.UserId.Value);

            #endregion

            #region paging

            var ticketsCount = await query.CountAsync();
            var pager = Pager.Build(filter.PageId, ticketsCount, filter.TakeEntity, filter.HowManyShowPageAfterAndBefore);
            var allEntities = await query.Paging(pager).ToListAsync();

            #endregion

            return filter.SetPaging(pager).SetTickets(allEntities);
        }

        public async Task<Ticket> GetTicketByTicketIdAsync(int ticketId)
        {
            return await _context.Tickets.Include(s => s.Owner).SingleOrDefaultAsync(s => s.Id == ticketId);
        }

        public async Task SaveChangeAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTicket(Ticket ticket)
        {
            _context.Tickets.Update(ticket);
            await SaveChangeAsync();
        }
    }
}
