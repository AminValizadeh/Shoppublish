using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Shop.Domain.ViewModels.Account
{
    public class CreateUserByAdminViewModel
    {
        [Display(Name = "ایمیل")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        [EmailAddress(ErrorMessage = "ایمیل وارد شده معتبر نمیباشد")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;


        [Display(Name = "نام و نام خانوادگی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string FullName { get; set; } = string.Empty;

        [Display(Name = "تلفن همراه")]
        [MaxLength(11, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string? Mobile { get; set; }

        [Display(Name = "کلمه عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string Password { get; set; } = string.Empty;

        public IFormFile? Avatar { get; set; }

        public bool IsAdmin { get; set; }
        public List<int>? SelectedRoles { get; set; }


    }

    public enum CreateUserByAdminResult
    {
        Success,
        ExistUser
    }
}
