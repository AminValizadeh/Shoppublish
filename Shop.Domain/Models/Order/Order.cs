using Shop.Domain.Models.Common;
using System.ComponentModel.DataAnnotations;

namespace Shop.Domain.Models.Order
{
    public class Order : BaseEntity
    {
        #region properties

        public int UserId { get; set; }

        public int FinalPrice { get; set; }
        public int FinalDiscount{ get; set; }
        public int FinalPriceWithDiscount { get; set; }

        public bool IsPaid { get; set; }

        [Display(Name = "کد پیگیری")]
        [MaxLength(300, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string? TracingCode { get; set; }

        #endregion

        #region relations

        public IEnumerable<OrderDetail> OrderDetails { get; set; }

        #endregion
    }
}
