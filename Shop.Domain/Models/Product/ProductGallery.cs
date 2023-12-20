using Shop.Domain.Models.Common;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Shop.Domain.Models.Product
{
    public class ProductGallery : BaseEntity
    {
        #region properties

        public int ProductId { get; set; }

        [Display(Name = "نام تصویر")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        [MaxLength(300, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string ImageName { get; set; } = string.Empty;

        #endregion

        #region relations

        public Product Product { get; set; }

        #endregion
    }
}
