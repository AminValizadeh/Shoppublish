using Shop.Application.Interfaces;
using Shop.Domain.Entities.Ticket;
using Shop.Domain.Interfaces;
using Shop.Domain.Models.Account;
using Shop.Domain.ViewModels.Ticket;

namespace Shop.Application.Services
{
    public class TicketService : ITicketService
    {
        #region constructor

        private readonly ITicketRepository _ticketRepository;
        private readonly ITicketMessageRepository _ticketMessageRepository;
        private readonly IAccountRepository _accountRepository;

        public TicketService(ITicketRepository ticketRepository, ITicketMessageRepository ticketMessageRepository, IAccountRepository accountRepository)
        {
            _ticketRepository = ticketRepository;
            _ticketMessageRepository = ticketMessageRepository;
            _accountRepository = accountRepository;
        }

        public async Task<AddTicketyAdminResult> AddTicketFromAdminForUser(AddTicketByAdminViewModel ticket, int userIdTicketAdmin)
        {
            if (string.IsNullOrEmpty(ticket.Text)) return AddTicketyAdminResult.Error;

            // add ticket
            var newTicket = new Ticket
            {
                OwnerId = ticket.UserId,
                IsReadByAdmin = true,
                TicketPriority = ticket.TicketPriority,
                Title = ticket.Title,
                TicketSection = ticket.TicketSection,
                TicketState = TicketState.Answered
            };

            await _ticketRepository.AddTicketAsync(newTicket);

            // add ticket message
            var newMessage = new TicketMessage
            {
                TicketId = newTicket.Id,
                Text = ticket.Text,
                SenderId = userIdTicketAdmin,
            };

            await _ticketMessageRepository.AddTicketMessageAsync(newMessage);

            return AddTicketyAdminResult.Success;
        }

        #endregion

        public async Task<AddTicketResult> AddUserTicket(AddTicketViewModel ticket, int userId)
        {
            if (string.IsNullOrEmpty(ticket.Text)) return AddTicketResult.Error;

            // add ticket
            var newTicket = new Ticket
            {
                OwnerId = userId,
                IsReadByOwner = true,
                TicketPriority = ticket.TicketPriority,
                Title = ticket.Title,
                TicketSection = ticket.TicketSection,
                TicketState = TicketState.UnderProgress
            };

            await _ticketRepository.AddTicketAsync(newTicket);

            // add ticket message
            var newMessage = new TicketMessage
            {
                TicketId = newTicket.Id,
                Text = ticket.Text,
                SenderId = userId,
            };

            await _ticketMessageRepository.AddTicketMessageAsync(newMessage);

            return AddTicketResult.Success;
        }

        public async Task<AnswerTicketResult> AnswerTicket(AnswerTicketViewModel answer, int userId)
        {
            var ticket = await _ticketRepository.GetTicketByTicketIdAsync(answer.Id);
            if (ticket == null) return AnswerTicketResult.NotFound;
            if (ticket.OwnerId != userId) return AnswerTicketResult.NotForUser;
            var ticketMessage = new TicketMessage
            {
                TicketId = ticket.Id,
                SenderId = userId,
                Text = answer.Text,
            };

            await _ticketMessageRepository.AddTicketMessageAsync(ticketMessage);

            ticket.IsReadByAdmin = false;
            ticket.IsReadByOwner = true;
            ticket.TicketState = TicketState.UnderProgress;
            await _ticketRepository.SaveChangeAsync();
            return AnswerTicketResult.Success;
        }

        public async Task<AnswerTicketResult> AnswerTicketFromAdmin(AnswerTicketViewModel answer, int userId)
        {
            var ticket = await _ticketRepository.GetTicketByTicketIdAsync(answer.Id);
            if (ticket == null) return AnswerTicketResult.NotFound;
            var ticketMessage = new TicketMessage
            {
                TicketId = ticket.Id,
                SenderId = userId,
                Text = answer.Text,
            };

            await _ticketMessageRepository.AddTicketMessageAsync(ticketMessage);

            ticket.IsReadByAdmin = true;
            ticket.IsReadByOwner = false;
            ticket.TicketState = TicketState.Answered;
            await _ticketRepository.SaveChangeAsync();
            return AnswerTicketResult.Success;
        }

        public async Task<bool> ClosedTicketByTicketId(int ticketId)
        {
            var ticket = await _ticketRepository.GetTicketByTicketIdAsync(ticketId);
            if (ticket != null && ticket.TicketState != TicketState.Closed)
            {
                ticket.TicketState = TicketState.Closed;
                await _ticketRepository.UpdateTicket(ticket);
                return true;
            }
            return false;
        }

        public async Task<FilterTicketViewModel> FilterTickets(FilterTicketViewModel filter)
        {
            return await _ticketRepository.FilterTickets(filter);
        }

        public async Task<List<User>> FilterUserByEmailForCreateTicketFromAdmin(string email)
        {
            return await _accountRepository.FilterUserByEmail(email);
        }

        public async Task<TicketDetailViewModel?> GetTicketForShow(int ticketId, int userId)
        {
            var ticket = await _ticketRepository.GetTicketByTicketIdAsync(ticketId);
            if (ticket == null || ticket.OwnerId != userId) return null;
            return new TicketDetailViewModel
            {
                Ticket = ticket,
                TicketMessages = await _ticketMessageRepository.GetListTicketMessagesByTicketId(ticketId),
            };
        }

        public async Task<TicketDetailViewModel?> GetTicketForShowAdmin(int ticketId)
        {
            var ticket = await _ticketRepository.GetTicketByTicketIdAsync(ticketId);
            if (ticket == null) return null;
            return new TicketDetailViewModel
            {
                Ticket = ticket,
                TicketMessages = await _ticketMessageRepository.GetListTicketMessagesByTicketId(ticketId),
            };
        }
    }
}
