using Shop.Domain.Models.Common;
using System.ComponentModel.DataAnnotations;

namespace Shop.Domain.Models.Product
{
    public class ProductDiscount : BaseEntity
    {
        #region properties

        [Display(Name = "کد تخفیف")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(300, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string DiscountCode { get; set; } = string.Empty;

        [Range(0, 100)]
        public int Percentage { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        [Display(Name = "محصول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int ProductId { get; set; }

        #endregion

        #region relations


        public Product Product { get; set; }

        #endregion

    }
}
