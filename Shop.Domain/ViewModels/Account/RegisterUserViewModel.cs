using System.ComponentModel.DataAnnotations;

namespace Shop.Domain.ViewModels.Account
{
    public class RegisterUserViewModel
    {
        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        [EmailAddress(ErrorMessage = "ایمیل وارد شده معتبر نمیباشد")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "نام و نام خانوادگی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string FullName { get; set; } = string.Empty;

        [Display(Name = "کلمه عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "تکرار کلمه عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        [Compare("Password", ErrorMessage = "تکرار کلمه عبور با کلمه عبور مغایرت دارد")]
        public string RePassword { get; set; } = string.Empty;
    }

    public enum RegisterUserResult
    {
        Success,
        ExistUser,
    }
}
