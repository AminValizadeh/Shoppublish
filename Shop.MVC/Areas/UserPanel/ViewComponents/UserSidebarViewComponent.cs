using Microsoft.AspNetCore.Mvc;
using Shop.Application.Interfaces;
using Shop.Domain.ViewModels.UserPanel;
using Shop.MVC.PresentationExtensions;

namespace Shop.MVC.Areas.UserPanel.ViewComponents
{
    public class UserSidebarViewComponent : ViewComponent
    {
        #region constructor

        private readonly IAccountService _accountService;
        private readonly IWalletService _walletService;

        public UserSidebarViewComponent(IAccountService accountService, IWalletService walletService)
        {
            _accountService = accountService;
            _walletService = walletService;
        }

        #endregion

        public async Task<IViewComponentResult> InvokeAsync()
        {
            int id = User.GetUserId();
            UserInformationViewModel userInformationModel = await _accountService.GetUserByIdForUserPanelAsync(id);

            var sumDeposits = await _walletService.SumWalletDepositsByUserId(User.GetUserId());
            var sumWithdrawals = await _walletService.SumWalletWithdrawalsByUserId(User.GetUserId());
            userInformationModel.WalletBalance = sumDeposits - sumWithdrawals;

            return View("UserSidebar", userInformationModel);
        }
    }
}
