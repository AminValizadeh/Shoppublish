using System.ComponentModel.DataAnnotations;

namespace Shop.Domain.ViewModels.RolePermission
{
    public class AddRoleViewModel
    {
        [Display(Name = "عنوان نقش")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(20, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string RoleTitle { get; set; }

        public List<int>? SelectedPermission { get; set; }
    }
}
