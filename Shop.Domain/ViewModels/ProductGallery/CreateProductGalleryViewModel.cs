using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Shop.Domain.ViewModels.ProductGallery
{
    public class CreateProductGalleryViewModel
    {
        public string? ProductTitle { get; set; }

        [Display(Name = "ایدی محصول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int ProductId { get; set; }
        
        [Display(Name ="تصویر محصول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public IFormFile ProductImage { get; set; }
    }

    public enum CreateProductGalleryResult
    {
        Success,
        NotFoundProduct,
        NotValidImage
    }
}
