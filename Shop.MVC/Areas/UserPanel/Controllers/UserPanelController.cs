using Microsoft.AspNetCore.Mvc;
using Shop.Application.Interfaces;
using Shop.Domain.ViewModels.UserPanel;
using Shop.MVC.PresentationExtensions;

namespace Shop.MVC.Areas.UserPanel.Controllers
{
    public class UserPanelController : UserBaseController
    {
        #region constructor

        private readonly IAccountService _accountService;

        public UserPanelController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        #endregion

        #region index

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            return View();
        }

        #endregion

        #region edit user

        [HttpGet("edit-user")]
        public async Task<IActionResult> EditUser()
        {
            int id = User.GetUserId();
            EditUserPanelViewModel model = await _accountService.GetUserByIdForEditUserPanelAsync(id);
            return View(model);
        }

        [HttpPost("edit-user"), ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(EditUserPanelViewModel editUserPanel)
        {
            if (ModelState.IsValid)
            {
                bool editResult = await _accountService.EditUserInUserPanelAsync(editUserPanel, User.GetUserId());
                if (editResult)
                {
                    TempData[SuccessMessage] = "حساب کاربری با موفقیت ویرایش شد";
                    return RedirectToAction("Index");
                }
                else
                    TempData[ErrorMessage] = "مشکلی در ویرایش حساب پی آمده است";

            }
            return View(editUserPanel);
        }

        #endregion
    }
}
