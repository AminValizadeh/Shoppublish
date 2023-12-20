using Microsoft.AspNetCore.Mvc;
using Shop.Application.Interfaces;
using Shop.Domain.ViewModels.Account;

namespace Shop.MVC.Areas.Admin.Controllers
{
    [Route("admin/user/")]
    public class AccountController : AdminBaseController
    {
        #region constructor

        private readonly IAccountService _accountService;
        private readonly IRolePermissionService _rolePermissionService;

        public AccountController(IAccountService accountService, IRolePermissionService rolePermissionService)
        {
            _accountService = accountService;
            _rolePermissionService = rolePermissionService;
        }

        #endregion


        #region list users

        [HttpGet("list-users")]
        public async Task<IActionResult> Index(FilterUsersInAdminViewModel filter)
        {
            filter = await _accountService.GetUsersForAdminAsync(filter);
            return View(filter);
        }

        #endregion

        #region create user 

        [HttpGet("create-user")]
        public async Task<IActionResult> CreateUser()
        {
            ViewData["Roles"] = await _rolePermissionService.GetRoles();
            return View();
        }

        [HttpPost("create-user"), ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUser(CreateUserByAdminViewModel createUser)
        {
            if (ModelState.IsValid)
            {
                var resultCreateUser = await _accountService.AddUserByAdminAsync(createUser);
                switch (resultCreateUser)
                {
                    case CreateUserByAdminResult.Success:
                        TempData[SuccessMessage] = "کاربر مورد نظر با موفقیت ثبت شد";
                        return RedirectToAction("Index");
                    case CreateUserByAdminResult.ExistUser:
                        ModelState.AddModelError("Email", "ایمیل وارد شده قبلا ثبت نام کرده است");
                        TempData[WarningMessage] = "ایمیل وارد شده قبلا ثبت نام کرده است";
                        break;
                }
            }

            ViewData["Roles"] = await _rolePermissionService.GetRoles();
            return View(createUser);
        }

        #endregion

        #region edit user 

        [HttpGet("update-user/{id}")]
        public async Task<IActionResult> UpdateUser(int id)
        {
            UpdateUserByAdminViewModel? updateUser = await _accountService.GetUserForEditByAdminAsync(id);
            if (updateUser == null) return NotFound();
            ViewData["Roles"] = await _rolePermissionService.GetRoles();
            return View(updateUser);
        }

        [HttpPost("update-user/{id}"), ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateUser(UpdateUserByAdminViewModel updateUser)
        {
            if (ModelState.IsValid)
            {
                var resultUpdate = await _accountService.UpdateUserByAdminAsync(updateUser);
                if (resultUpdate)
                {
                    TempData[SuccessMessage] = "کاربر مورد نظر با موفقیت ویرایش شد";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData[ErrorMessage] = "خطایی در ویرایش کاربر وجود دارد";
                }
            }
            ViewData["Roles"] = await _rolePermissionService.GetRoles();
            return View(updateUser);
        }

        #endregion

        #region delete user
        [HttpGet("delete-user/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _accountService.DeleteUserById(id);
            if (result)
            {
                TempData[SuccessMessage] = "کاربر مورد نظر با موفقیت حذف گردید";
            }
            else
            {
                TempData[ErrorMessage] = "کاربر مورد نظر یافت نشد";
            }
            return RedirectToAction("Index");
        }
        #endregion
    }
}
