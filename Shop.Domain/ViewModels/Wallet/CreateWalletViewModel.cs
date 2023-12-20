using System.ComponentModel.DataAnnotations;

namespace Shop.Domain.ViewModels.Wallet
{
    public class CreateWalletViewModel
    {
        [Display(Name = "ایدی کاربر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int UserId { get; set; }


        [Display(Name = "قیمت")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int Price { get; set; }

        
        [Display(Name = "توضیحات")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string Description { get; set; } = string.Empty;

        [Display(Name = "پرداخت شده / پرداخت نشده")]
        public bool IsPaid { get; set; }
    }
}
