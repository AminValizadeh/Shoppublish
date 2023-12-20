using System.ComponentModel.DataAnnotations;

namespace Shop.Domain.ViewModels.ProductCategory
{
    public class CreateOrEditProductCategoryViewModel
    {
        public int? ParentId { get; set; }
        public string? ParentTitle { get; set; }

        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string Title { get; set; } = string.Empty;
    }
}
