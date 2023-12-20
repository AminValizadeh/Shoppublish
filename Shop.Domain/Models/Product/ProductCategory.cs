using Shop.Domain.Models.Common;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Shop.Domain.Models.Product
{
    public class ProductCategory : BaseEntity
    {
        #region properties

        public int? ParentId { get; set; }

        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string Title { get; set; } = string.Empty;

        #endregion

        #region relations

        public ProductCategory? Parent { get; set; }

        public IEnumerable<Product> Product { get; set; }

        #endregion
    }
}
