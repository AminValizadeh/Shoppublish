using Shop.Domain.Models.Account;
using Shop.Domain.Models.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shop.Domain.Entities.Ticket
{
    public class TicketMessage : BaseEntity
    {

        #region properties

        public int TicketId { get; set; }
        public int SenderId { get; set; }

        [Display(Name = "متن پیام")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Text { get; set; } = string.Empty;

        #endregion

        #region relations

        [ForeignKey("TicketId")]
        public Ticket? Ticket { get; set; }

        [ForeignKey("SenderId")]
        public User? Sender { get; set; }

        #endregion
    }
}
