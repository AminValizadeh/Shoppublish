using Shop.Domain.Models.Common;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Shop.Domain.Models.Product
{
    public class ProductColor : BaseEntity
    {
        #region properties

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

        #endregion

        #region relations

        public Product Product { get; set; }

        #endregion
    }
}
