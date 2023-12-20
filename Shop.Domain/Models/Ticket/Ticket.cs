using Shop.Domain.Models.Account;
using Shop.Domain.Models.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shop.Domain.Entities.Ticket
{
    public class Ticket : BaseEntity
    {
        #region properties

        public int OwnerId { get; set; }

        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string Title { get; set; } = string.Empty;

        [Display(Name = "وضعیت تیکت")]
        public TicketState TicketState { get; set; }

        [Display(Name = "بخش مورد نظر")]
        public TicketSection TicketSection { get; set; }

        [Display(Name = "اولویت")]
        public TicketPriority TicketPriority { get; set; }

        public bool IsReadByOwner { get; set; }
        public bool IsReadByAdmin { get; set; }


        #endregion

        #region relations

        [ForeignKey("OwnerId")]
        public User? Owner { get; set; }

        public IEnumerable<TicketMessage>? TicketMessages { get; set; }

        #endregion
    }

    #region enums

    public enum TicketSection
    {
        [Display(Name = "پشتیبانی")]
        Support,
        [Display(Name = "فنی")]
        Technical,
        [Display(Name = "آموزشی")]
        Academic
    }

    public enum TicketPriority
    {
        [Display(Name = "کم")]
        Low,
        [Display(Name = "متوسط")]
        Medium,
        [Display(Name = "زیاد")]
        Hight
    }

    public enum TicketState
    {
        [Display(Name = "در حال بررسی")]
        UnderProgress,
        [Display(Name = "پاسخ داده شده")]
        Answered,
        [Display(Name = "بسته شده")]
        Closed
    }

    #endregion

}
