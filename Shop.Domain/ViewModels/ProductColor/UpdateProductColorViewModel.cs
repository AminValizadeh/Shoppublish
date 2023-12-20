using System.ComponentModel.DataAnnotations;

namespace Shop.Domain.ViewModels.ProductColor
{
    public class UpdateProductColorViewModel
    {
        public string? ProductTitle { get; set; }

        [Display(Name = "ایدی محصول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int ProductId { get; set; }

        [Display(Name = "نام رنگ")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string ColorName { get; set; }

        [Display(Name = "کد رنگ")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string ColorCode { get; set; }

        [Display(Name = "تعداد رنگ")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int Count { get; set; }
    }

    public enum UpdateProductColorResult
    {
        Success,
        NotFoundProductColor,
        ExistProductColorForProduct,
    }
}
