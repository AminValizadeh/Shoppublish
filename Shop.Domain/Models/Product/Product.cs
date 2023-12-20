using Shop.Domain.Models.Common;
using Shop.Domain.Models.Order;
using System.ComponentModel.DataAnnotations;

namespace Shop.Domain.Models.Product
{
    public class Product : BaseEntity
    {
        #region properties

        [Display(Name = "نام محصول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(300, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string Title { get; set; } = string.Empty;

        [Display(Name = "تصویر محصول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(300, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string ImageName { get; set; } = string.Empty;

        [Display(Name = "ویژگی ها")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Feature { get; set; } = string.Empty;

        [Display(Name = "قیمت محصول")]
        public int Price { get; set; }

        [Display(Name = "توضیحات اصلی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Description { get; set; } = string.Empty;

        [Display(Name ="دسته بندی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int ProductCategoryId { get; set; }

        #endregion

        #region relations

        public ProductCategory ProductCategory { get; set; }

        public IEnumerable<ProductGallery> ProductGalleries { get; set; }
        public IEnumerable<ProductDiscount> ProductDiscounts { get; set; }
        public IEnumerable<OrderDetail> OrderDetails { get; set; }
        public IEnumerable<ProductColor> ProductColors { get; set; }

        #endregion
    }
}
