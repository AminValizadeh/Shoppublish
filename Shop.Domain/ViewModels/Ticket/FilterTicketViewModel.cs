using Shop.Domain.Entities.Ticket;
using Shop.Domain.ViewModels.Paging;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Shop.Domain.ViewModels.Ticket
{
    public class FilterTicketViewModel : BasePaging
    {
        #region properties

        public string? Title { get; set; }
        public int? UserId { get; set; }
        public FilterTicketState FilterTicketState { get; set; }
        public FilterTicketOrder OrderBy { get; set; }
        public TicketSection? TicketSection { get; set; }
        public TicketPriority? TicketPriority { get; set; }
        public TicketState? TicketState { get; set; }
        public List<Domain.Entities.Ticket.Ticket> Tickets { get; set; } = new List<Domain.Entities.Ticket.Ticket>();

        #endregion

        #region methods

        public FilterTicketViewModel SetTickets(List<Domain.Entities.Ticket.Ticket> tickets)
        {
            this.Tickets = tickets;
            return this;
        }

        public FilterTicketViewModel SetPaging(BasePaging paging)
        {
            this.PageId = paging.PageId;
            this.AllEntitiesCount = paging.AllEntitiesCount;
            this.StarPage = paging.StarPage;
            this.EndPage = paging.EndPage;
            this.HowManyShowPageAfterAndBefore = paging.HowManyShowPageAfterAndBefore;
            this.TakeEntity = paging.TakeEntity;
            this.SkipEntity = paging.SkipEntity;
            this.PageCount = paging.PageCount;
            return this;
        }

        #endregion
    }

    #region enums

    public enum FilterTicketState
    {
        [Display(Name = "همه")]
        All,

        [Display(Name = "حذف نشده ها")]
        NotDeleted,

        [Display(Name = "حذف شده ها")]
        Deleted
    }

    public enum FilterTicketOrder
    {
        [Display(Name = "صعودی")]
        CreateDat_ASC,

        [Display(Name = "نزولی")]
        CreateDate_DES,
    }

    #endregion

}
