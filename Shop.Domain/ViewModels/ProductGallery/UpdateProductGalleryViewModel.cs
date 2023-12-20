using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Shop.Domain.ViewModels.ProductGallery
{
    public class UpdateProductGalleryViewModel
    {
        public string? ProductTitle { get; set; }

        public int ProductId { get; set; }

        [Display(Name = "نام تصویر قبلی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string ProductImageName { get; set; }

        [Display(Name ="تصویر محصول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public IFormFile ProductImage { get; set; }
    }

    public enum UpdateProductGalleryResult
    {
        Success,
        NotFoundProductGallery,
        NotValidImage
    }
}
