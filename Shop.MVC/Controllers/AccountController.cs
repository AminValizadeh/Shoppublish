using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Shop.Application.Interfaces;
using Shop.Domain.ViewModels.Account;
using System.Security.Claims;

namespace Shop.MVC.Controllers
{
    public class AccountController : SiteBaseController
    {
        #region constructor

        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        #endregion

        #region register

        [HttpGet("register")]
        public IActionResult Register()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
                return NotFound();
            return View();
        }

        [HttpPost("register"), ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterUserViewModel register)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountService.AddUserAsync(register);

                switch (result)
                {
                    case RegisterUserResult.Success:
                        return View("SuccessRegister", new SuccessRegisterViewModel { FullName = register.FullName, Email = register.Email });
                    case RegisterUserResult.ExistUser:
                        TempData[InfoMessage] = "ایمیل وارد شده قبلا ثبت نام کرده است";
                        return View(register);
                }
            }

            return View(register);
        }


        #endregion

        #region Login

        [HttpGet("login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginViewModel login)
        {
            if (!ModelState.IsValid)
            {
                return View(login);
            }

            var user = await _accountService.StatusUserForLoginAsync(login);


            switch (user)
            {
                case LoginResult.Success:
                    var getUser = await _accountService.GetUserByEmailAsync(login.Email);

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier,getUser.Id.ToString()),
                        new Claim(ClaimTypes.Email,getUser.Email),
                        new Claim(ClaimTypes.Name,getUser.FullName),
                    };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);
                    var properties = new AuthenticationProperties
                    {
                        IsPersistent = login.IsRememberMe
                    };

                    await HttpContext.SignInAsync(principal, properties);

                    return RedirectToAction("Index", "Home");
                case LoginResult.NotExistUser:
                    ModelState.AddModelError("Email", "کاربر مورد نظر یافت نشد");
                    break;
                case LoginResult.IsNotActive:
                    ModelState.AddModelError("Email", "حساب کاربری شما فعال نمی باشد");
                    break;
                case LoginResult.IsBan:
                    ModelState.AddModelError("Email", "حساب کاربری شما مسدود می باشد");
                    break;
                default:
                    break;
            }

            return View(login);
        }

        #endregion

        #region active account

        [HttpGet("active-account/{activeCode}")]
        public async Task<IActionResult> ActiveAccount(string activeCode)
        {
            bool ResultActiveAccount = await _accountService.ActiveAccountByEmailActiveCodeAsync(activeCode);
            TempData["resultActiveAccount"] = ResultActiveAccount;
            return View();
        }

        #endregion

        #region log out

        [HttpGet("Log-out")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login");
        }

        #endregion

        #region forgot password

        [HttpGet("forgot-password")]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel forgot)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountService.GetForgotPasswordByEmailAsync(forgot);
                switch (result)
                {
                    case ForgotPasswordResult.Success:
                        TempData[SuccessMessage] = "ایمیل با موفقیت ارسال گردید";
                        break;
                    case ForgotPasswordResult.NotFound:
                        TempData[ErrorMessage] = "کاربر یافت نشد";
                        break;
                    case ForgotPasswordResult.Deleted:
                        TempData[ErrorMessage] = "کاربر یافت نشد ";
                        break;
                    case ForgotPasswordResult.Banded:
                        TempData[InfoMessage] = "حساب کاربری شما مسدود می باشد";
                        break;
                    case ForgotPasswordResult.NotEmailActive:
                        TempData[InfoMessage] = "حساب کاربری شما فعال نمی باشد";
                        break;

                }
            }
            return View(forgot);
        }

        #endregion

        #region reset password

        [HttpGet("reset-password/{activeCode}")]
        public IActionResult ResetPassword(string activeCode)
        {
            return View();
        }

        [HttpPost("reset-password/{activeCode}")]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel reset, string activeCode)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountService.ResetPassword(reset, activeCode);
                if (result)
                {
                    TempData[SuccessMessage] = "کلمه عبور با موفقیت تغییر یافت";
                    return RedirectToAction("Login");
                }
                else
                {
                    TempData[ErrorMessage] = "عملیات با خطا مواجه گردید";
                }
            }

            return View(reset);
        }
        #endregion
    }
}
