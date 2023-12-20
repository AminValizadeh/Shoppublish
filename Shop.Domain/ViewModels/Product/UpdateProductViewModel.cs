using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Shop.Domain.ViewModels.Product
{
    public class UpdateProductViewModel
    {
        public int ProductId { get; set; }

        [Display(Name = "نام محصول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(300, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string Title { get; set; } = string.Empty;

        [Display(Name = "قیمت محصول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int Price { get; set; }

        [Display(Name = "توضیحات محصول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Description { get; set; } = string.Empty;

        [Display(Name = "ویژگی های محصول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Feature { get; set; } = string.Empty;

        [Display(Name = "نام تصویر محصول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string ImageName { get; set; } = string.Empty;

        [Display(Name = "تصویر محصول")]
        public IFormFile? ImageProduct { get; set; }

        [Display(Name = "دسته بندی محصول")]
        [Required(ErrorMessage = "لطفا {0} را بارگذاری کنید")]
        public int CategoryId { get; set; }
    }
    public enum UpdateProductResult
    {
        Success,
        NotValidImage,
        NotFoundProduct
    }
}
