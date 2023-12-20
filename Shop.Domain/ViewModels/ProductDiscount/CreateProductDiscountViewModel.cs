using System.ComponentModel.DataAnnotations;

namespace Shop.Domain.ViewModels.ProductDiscount
{
    public class CreateProductDiscountViewModel
    {
        [Display(Name = "کد تخفیف")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(300, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string DiscountCode { get; set; } = string.Empty;

        [Display(Name = "درصد تخفیف")]
        [Range(0, 100)]
        public int Percentage { get; set; }

        [Display(Name = "از تاریخ")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public DateTime StartTime { get; set; }

        [Display(Name = "تا تاریخ")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public DateTime EndTime { get; set; }

        [Display(Name = "محصول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int ProductId { get; set; }
    }

    public enum CreateProductDiscountResult
    {
        Success,
        ExistDiscount,
    }
}
