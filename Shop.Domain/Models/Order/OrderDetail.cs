using Shop.Domain.Models.Common;
using System.ComponentModel.DataAnnotations;

namespace Shop.Domain.Models.Order
{
    public class OrderDetail : BaseEntity
    {
        #region properties

        public int OrderId { get; set; }

        public int ProductId { get; set; }

        public int Count { get; set; }

        public int ProductPrice { get; set; }
        public int ProductDiscount { get; set; }
        public int ProductPriceWithDiscount { get; set; }

        [Display(Name = "کد تخفیف")]
        [MaxLength(300, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string? DiscountCode { get; set; }

        #endregion

        #region relations

        public Order Order { get; set; }

        public Product.Product Product { get; set; }


        #endregion
    }
}
