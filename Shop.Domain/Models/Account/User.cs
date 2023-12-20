using Shop.Domain.Entities.Ticket;
using Shop.Domain.Models.Common;
using Shop.Domain.Models.Permissions;
using Shop.Domain.Models.Wallet;
using System.ComponentModel.DataAnnotations;

namespace Shop.Domain.Models.Account
{
    public class User : BaseEntity
    {
        #region propertis

        [Display(Name = "ایمیل")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        [EmailAddress(ErrorMessage = "ایمیل وارد شده معتبر نمیباشد")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string EmailActiveCode { get; set; } = string.Empty;

        [Display(Name = "ایمیل فعال / غیر فعال")]
        public bool IsEmailActive { get; set; }


        [Display(Name = "تلفن همراه")]
        [MaxLength(11, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string? Mobile { get; set; }

        [Display(Name = "نام و نام خانوادگی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string FullName { get; set; } = string.Empty;

        [Display(Name = "کلمه عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "آواتار")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string? ImageName { get; set; }

        [Display(Name = "مدیر / کاربر عادی ")]
        public bool IsAdmin { get; set; }

        [Display(Name = " فعال / غیر فعال")]
        public bool IsBan { get; set; }

        #endregion

        #region relations

        
        public IEnumerable<TicketMessage>? TicketMessages { get; set; }
        public IEnumerable<Ticket>? Tickets { get; set; }
        public IEnumerable<UserRole> UserRoles { get; set; }
        public IEnumerable<Wallet.Wallet> Wallet { get; set; }

        #endregion
    }
}
