using Shop.Domain.Entities.Ticket;
using System.ComponentModel.DataAnnotations;

namespace Shop.Domain.ViewModels.Ticket
{
    public class AddTicketViewModel
    {

        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string Title { get; set; } = string.Empty;

        [Display(Name = "بخش مورد نظر")]
        public TicketSection TicketSection { get; set; }

        [Display(Name = "اولویت")]
        public TicketPriority TicketPriority { get; set; }

        [Display(Name = "متن پیام")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Text { get; set; } = string.Empty;

    }

    public enum AddTicketResult
    {
        Error,
        Success,
    }
}
